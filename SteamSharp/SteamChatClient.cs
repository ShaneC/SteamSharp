using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
//using Windows.Web.Http;

namespace SteamSharp {
	
	public sealed partial class SteamChatClient : SteamClient {

		public event EventHandler<SteamChatMessagesReceivedEventArgs> SteamChatMessagesReceived;
		public event EventHandler<SteamChatUserStateChangeEventArgs> SteamChatUserStateChange;
		public event EventHandler<SteamChatConnectionChangeEventArgs> SteamChatConnectionChanged;

		/// <summary>
		/// Maintains the list of users currently known to the Steam Chat Client. Not available until after the client has connected.
		/// </summary>
		public SteamFriendsList FriendsList {
			get {
				return _friendsList;
			}
			private set { _friendsList = value; }
		}
		private SteamFriendsList _friendsList = null;

		public ClientConnectionStatus ConnectionStatus {
			get { return _connectionStatus; }
			set { _connectionStatus = value; }
		}
		private ClientConnectionStatus _connectionStatus = ClientConnectionStatus.Disconnected;

		public bool AutoReconnect {
			get { return _autoReconnect; }
			set {
				_autoReconnect = value;
			}
		}
		private bool _autoReconnect = true;

		private SteamChatSession ChatSession;

		private long LastMessageSentID = 0;

		private CancellationTokenSource Cancellation = new CancellationTokenSource();

		private bool IsManualDisconnection = false;

		private bool HasInitialized = false;

		public SteamClient SteamClient { get; set; }

		public SteamChatClient( SteamClient client ) {
			this.SteamChatConnectionChanged += SteamChatConnectionChangeHandler;
			SteamClient = client;
		}

		public SteamChatClient() {
			this.SteamChatConnectionChanged += SteamChatConnectionChangeHandler;
		}

		/// <summary>
		/// (Async) (Requires <see cref="SteamSharp.Authenticators.UserAuthenticator"/>)
		/// Logs the SteamChatClient on to the Steam Chat Service under the context of the authenticated user (UserAuthenticator attached to the targeted SteamClient).
		/// </summary>
		/// <returns>
		///	Logs the SteamClient authenticated user on to the Steam Chat Service.
		/// </returns>
		/// <exception cref="SteamRequestException">SteamRequestException</exception>
		/// <exception cref="SteamAuthenticationException">SteamAuthenticationException</exception>
		public async Task LogOn() {
			await LogOn( SteamClient );
		}

		/// <summary>
		/// (Async) (Requires <see cref="SteamSharp.Authenticators.UserAuthenticator"/>)
		/// Logs the SteamChatClient on to the Steam Chat Service under the context of the authenticated user (UserAuthenticator attached to the targeted SteamClient).
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <returns>
		///	Logs the SteamClient authenticated user on to the Steam Chat Service.
		/// </returns>
		/// <exception cref="SteamRequestException">SteamRequestException</exception>
		/// <exception cref="SteamAuthenticationException">SteamAuthenticationException</exception>
		public async Task LogOn( SteamClient client ) {

			IsManualDisconnection = false;

			try { 

				client.IsAuthorizedCall( new Type[] {
					typeof( Authenticators.UserAuthenticator )
				} );

				SteamRequest request = new SteamRequest( "ISteamWebUserPresenceOAuth", "Logon", "v0001", HttpMethod.Post );
				ChatSession = SteamInterface.VerifyAndDeserialize<SteamChatSession>( ( await client.ExecuteAsync( request ) ) );
				LastMessageSentID = ChatSession.MessageBaseID;

				Authenticator = client.Authenticator;

				// Initialize Friends List
				FriendsList = await SteamCommunity.GetFriendsListAsync( this, ChatSession.SteamID );

				HasInitialized = true;

				BeginPoll();

			} catch( Exception e ) {
				if( e is AggregateException && e.InnerException != null )
					throw e.InnerException;
				throw e;
			}

		}

		/// <summary>
		/// Opens a long-poll modeled connection to retrieve updated information from the Steam server.
		/// </summary>
		private async void BeginPoll() {

			IndicateConnectionState( ClientConnectionStatus.Connecting );

			SteamRequest requestBase = new SteamRequest( "ISteamWebUserPresenceOAuth", "Poll", "v0001", HttpMethod.Post );
			requestBase.AddParameter( "umqid", ChatSession.ChatSessionID, ParameterType.GetOrPost );
			requestBase.AddParameter( "sectimeout", 20, ParameterType.GetOrPost );

			if( Authenticator != null ) {
				Authenticator.Authenticate( this, requestBase );
			}

			try {

				using( var client = new HttpClient() ) {

					Cancellation.Token.Register( () => client.CancelPendingRequests() );
					client.Timeout = TimeSpan.FromSeconds( 25 );

					while( true ) {

						requestBase.AddParameter( "message", LastMessageSentID, ParameterType.GetOrPost );

						using( var httpRequest = BuildHttpRequest( requestBase ) ) {

							try {

								using( var response = await client.SendAsync( httpRequest, HttpCompletionOption.ResponseContentRead ).ConfigureAwait( false ) ) {

									SteamChatPollResult result = SteamInterface.VerifyAndDeserialize<SteamChatPollResult>( ConvertToResponse( requestBase, response, null ) );
									System.Diagnostics.Debug.WriteLine( await response.Content.ReadAsStringAsync() );

									IndicateConnectionState( ClientConnectionStatus.Connected );

									if( result.PollStatus == ChatPollStatus.OK ) {
										LastMessageSentID = result.PollLastMessageSentID;
										if( result.Messages != null )
											ProcessMessagesReceived( result );
									}

									await Task.Delay( 1000, Cancellation.Token );

								}

							} catch( Exception e ) {
								System.Diagnostics.Debug.WriteLine( e.ToString() );
								throw e;
							}

						}

					}

				}

			} catch( Exception e ) {
				System.Diagnostics.Debug.WriteLine( e.ToString() );
				IndicateConnectionState( ClientConnectionStatus.Disconnected );
				if( e is OperationCanceledException || e is TaskCanceledException ) {
					Cancellation.Dispose();
					Cancellation = new CancellationTokenSource();
					return;
				} else if( e is SteamRequestException ) {
					if( ( e as SteamRequestException ).StatusCode == HttpStatusCode.NotFound ) {
						// Network Problem
						return;
					}else if( ( e as SteamRequestException ).StatusCode == HttpStatusCode.Unauthorized ){
						// User is Unauthorized
						throw new SteamAuthenticationException( "SteamChat Client: User is Unauthorized", e );
					}
					// Likely a transient Steam API problem
					System.Diagnostics.Debug.WriteLine( "SteamRequestException Encountered: " + ( e as SteamRequestException ).StatusCode );
				}
				System.Diagnostics.Debug.WriteLine( "Encountered Unexpected Exception: " + e.StackTrace );
				throw e;
			}

		}

		/// <summary>
		/// Stops polling and disconnects the client from the Steam server.
		/// </summary>
		public async Task Disconnect() {

			IsManualDisconnection = true;

			if( this.ConnectionStatus == ClientConnectionStatus.Disconnected )
				return;

			Cancellation.Cancel();
			SteamRequest request = new SteamRequest( "ISteamWebUserPresenceOAuth", "Logoff", "v0001", HttpMethod.Post );
			request.AddParameter( "umqid", ChatSession.ChatSessionID, ParameterType.GetOrPost );

			await ExecuteAsync( request );
			
		}

		/// <summary>
		/// Informs the target user, via the Steam Service, that the current user is currently typing.
		/// </summary>
		/// <param name="destinationUser"></param>
		/// <returns>Asynchronous task to be used for tracking request completion.</returns>
		public async Task IndicateTyping( SteamID destinationUser ) {
			await SendMessage( destinationUser, null );
		}

		/// <summary>
		/// Sends a message, with text, to the target user.
		/// </summary>
		/// <param name="destinationUser">Targeted receipient for the message.</param>
		/// <param name="text">Message to send.</param>
		/// <returns>Asynchronous task to be used for tracking request completion.</returns>
		public async Task SendMessage( SteamID destinationUser, string text ) {

			SteamRequest request = new SteamRequest( "ISteamWebUserPresenceOAuth", "Message", "v0001", HttpMethod.Post );

			request.AddParameter( "umqid", ChatSession.ChatSessionID, ParameterType.GetOrPost );
			request.AddParameter( "type", ( ( text == null ) ? "typing" : "saytext" ), ParameterType.GetOrPost );
			request.AddParameter( "steamid_dst", destinationUser.ToString(), ParameterType.GetOrPost );

			if( text != null )
				request.AddParameter( "text", text, ParameterType.GetOrPost );

			SteamInterface.VerifyAndDeserialize<SteamChatSendMessageResponse>( ( await ExecuteAsync( request ) ) );

		}

		private void IndicateConnectionState( ClientConnectionStatus newState ) {

			if( ConnectionStatus == newState )
				return;

			var previousState = ConnectionStatus;
			ConnectionStatus = newState;

			if( newState == ClientConnectionStatus.Disconnected && AutoReconnect ) {
				AttemptReconnectIfAppropriate();
			}

			OnSteamChatClientConnectionChange( new SteamChatConnectionChangeEventArgs {
				ChangeDateTime = DateTime.UtcNow,
				PreviousConnectionState = previousState,
				NewConnectionState = ConnectionStatus
			} );

		}

		/// <summary>
		/// Internal helper function to update local copy of the friends list.
		/// </summary>
		/// <param name="messages">Messages received from the Steam service.</param>
		private async void ProcessMessagesReceived( SteamChatPollResult result ) {

			result.Messages.Sort();

			List<SteamChatMessage> messages = new List<SteamChatMessage>();
			List<SteamChatRelationshipNotification> notifications = new List<SteamChatRelationshipNotification>();

			foreach( var message in result.Messages ) {

				if( message.Type == ChatMessageType.MessageText || message.Type == ChatMessageType.Typing || message.Type == ChatMessageType.LeftConversation ) {
					messages.Add( SteamChatMessage.CreateFromPollMessage( message ) );
				} else if( message.Type == ChatMessageType.PersonaStateChange || message.Type == ChatMessageType.PersonaRelationship ) {

					// Filter out notifications about the currently authenticated user
					if( message.FromUser.Equals( ChatSession.SteamID ) )
						continue;

					notifications.Add( SteamChatRelationshipNotification.CreateFromPollMessage( message ) );

					if( FriendsList.Friends.ContainsKey( message.FromUser ) ) {
						FriendsList.Friends[message.FromUser].PlayerInfo.PersonaState = message.PersonaState;
						FriendsList.Friends[message.FromUser].PlayerInfo.PersonaName = message.PersonaName;
						if( message.PersonaState == PersonaState.Offline )
							FriendsList.Friends[message.FromUser].PlayerInfo.LastLogOff = message.UTCMessageDateTime;
					} else {
						var newUser = new SteamUser {
							SteamID = message.FromUser,
							PlayerInfo = new SteamCommunity.PlayerInfo {
								PersonaName = message.PersonaName,
								PersonaState = message.PersonaState
							}
						};
						try {
							await newUser.GetProfileDataAsync( this );
						} catch( Exception ) {
							// Likely Network Connectivity Failure
						}
						FriendsList.Friends.Add( message.FromUser, newUser );
					}

				}

			}

			if( messages.Count > 0 ) {
				OnSteamChatMessagesReceived( new SteamChatMessagesReceivedEventArgs {
					UTCServerChangeDateTime = result.UTCTimestamp,
					NewMessages = messages
				} );
			}

			if( notifications.Count > 0 ) {
				OnSteamChatUserStateChange( new SteamChatUserStateChangeEventArgs {
					UTCServerChangeDateTime = result.UTCTimestamp,
					StateChanges = notifications
				} );
			}

		}

		private void AttemptReconnectIfAppropriate(){

			Task retry = Task.Run( async () => {

				while( this.ConnectionStatus == ClientConnectionStatus.Disconnected && AutoReconnect ) {

					if( HasInitialized && !IsManualDisconnection && this.ConnectionStatus == ClientConnectionStatus.Disconnected )
						this.BeginPoll();
					await Task.Delay( 10000 );

				}

			});

		}

		private void OnSteamChatClientConnectionChange( SteamChatConnectionChangeEventArgs e ) {
			EventHandler<SteamChatConnectionChangeEventArgs> handler;
			handler = SteamChatConnectionChanged;
			if( handler != null )
				handler( this, e );
		}

		private void OnSteamChatUserStateChange( SteamChatUserStateChangeEventArgs e ) {
			EventHandler<SteamChatUserStateChangeEventArgs> handler = SteamChatUserStateChange;
			if( handler != null )
				handler( this, e );
		}

		private void OnSteamChatMessagesReceived( SteamChatMessagesReceivedEventArgs e ) {
			EventHandler<SteamChatMessagesReceivedEventArgs> handler = SteamChatMessagesReceived;
			if( handler != null )
				handler( this, e );
		}

		private void SteamChatConnectionChangeHandler( object sender, SteamChatConnectionChangeEventArgs e ) {
			
		}

		public class SteamChatConnectionChangeEventArgs : EventArgs {

			/// <summary>
			/// UTC DateTime when the change took place.
			/// </summary>
			public DateTime ChangeDateTime { get; set; }

			/// <summary>
			/// Provides the connection state held by the client prior to the state change.
			/// </summary>
			public ClientConnectionStatus PreviousConnectionState { get; set; }
			
			/// <summary>
			/// Provides the connection state held by the client after the state change.
			/// </summary>
			public ClientConnectionStatus NewConnectionState { get; set; }

		}

		public class SteamChatMessagesReceivedEventArgs : EventArgs {

			/// <summary>
			/// UTC DateTime from the Steam API indicating when the poll was updated.
			/// </summary>
			public DateTime UTCServerChangeDateTime { get; set; }

			/// <summary>
			/// Sorted list of the <see cref="SteamChat.SteamPollMessage"/> objects which have been received since the last event. Sort order is Old to New (MessageDateTime ASC).
			/// </summary>
			public List<SteamChatMessage> NewMessages { get; set; }

		}

		public class SteamChatUserStateChangeEventArgs : EventArgs {

			/// <summary>
			/// UTC DateTime from the Steam API indicating when the poll was updated.
			/// </summary>
			public DateTime UTCServerChangeDateTime { get; set; }

			/// <summary>
			/// Sorted list of the <see cref="SteamChat.SteamPollMessage"/> objects which have been received since the last event. Sort order is Old to New (MessageDateTime ASC).
			/// </summary>
			public List<SteamChatRelationshipNotification> StateChanges { get; set; }

		}

	}

}
