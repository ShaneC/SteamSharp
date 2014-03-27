using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SteamSharp {
	
	public sealed partial class SteamChatClient : SteamClient {

		public event EventHandler<SteamChatMessagesReceivedEventArgs> SteamChatMessagesReceived;
		public event EventHandler<SteamChatUserStateChangeEventArgs> SteamChatUserStateChange;
		public event EventHandler<SteamChatConnectionChangeEventArgs> SteamChatConnected;
		public event EventHandler<SteamChatConnectionChangeEventArgs> SteamChatDisconnected;

		/// <summary>
		/// Maintains the list of users currently known to the Steam Chat Client. Not available until after the client has connected.
		/// </summary>
		public SteamFriendsList FriendsList {
			get {
				if( _friendsList == null )
					throw new NullReferenceException( "Attempting to reference SteamFriendsList before it has loaded. Do not attempt a reference until after SteamChatConnected has been fired." );
				return _friendsList;
			}
			private set { _friendsList = value; }
		}
		private SteamFriendsList _friendsList = null;

		public ClientConnectionStatus ConnectionStatus {
			get { return _connectionStatus; }
			set { _connectionStatus = value; }
		}
		private ClientConnectionStatus _connectionStatus = ClientConnectionStatus.Connecting;

		private SteamChatSession ChatSession;

		private long LastMessageSentID = 0;

		private CancellationTokenSource Cancellation = new CancellationTokenSource();

		public SteamChatClient() {
			this.SteamChatConnected += SteamChatConnectionChangeHandler;
			this.SteamChatDisconnected += SteamChatConnectionChangeHandler;
		}

		/// <summary>
		/// (Async) (Requires <see cref="SteamSharp.Authenticators.UserAuthenticator"/>)
		/// Logs the SteamChatClient on to the Steam Chat Service under the context of the authenticated user (UserAuthenticator attached to the targeted SteamClient).
		/// Throws <see cref="SteamRequestException"/> or <see cref="SteamAuthenticationException"/> on failure.
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <returns>
		///	Logs the SteamClient authenticated user on to the Steam Chat Service.
		/// </returns>
		public async Task LogOn( SteamClient client ) {

			client.IsAuthorizedCall( new Type[] {
				typeof( Authenticators.UserAuthenticator )
			} );

			SteamRequest request = new SteamRequest( "ISteamWebUserPresenceOAuth", "Logon", "v0001", HttpMethod.Post );
			ChatSession = SteamInterface.VerifyAndDeserialize<SteamChatSession>( ( await client.ExecuteAsync( request ) ) );
			LastMessageSentID = ChatSession.MessageBaseID;

			Authenticator = client.Authenticator;

			// Initialize Friends List
			FriendsList = await SteamCommunity.GetFriendsListAsync( this, ChatSession.SteamID );
			
			BeginPoll();

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

			using( var client = new HttpClient() ) {

				Cancellation.Token.Register( () => client.CancelPendingRequests() );

				while( true ) {

					requestBase.AddParameter( "message", LastMessageSentID, ParameterType.GetOrPost );

					using( var httpRequest = BuildHttpRequest( requestBase ) ) {

						httpRequest.Headers.Add( "Connection", "Keep-Alive" );

						try {

							using( var response = await client.SendAsync( httpRequest, HttpCompletionOption.ResponseContentRead ).ConfigureAwait( false ) ) {

								SteamChatPollResult result = SteamInterface.VerifyAndDeserialize<SteamChatPollResult>( ConvertToResponse( requestBase, response, null ) );

								IndicateConnectionState( ClientConnectionStatus.Connected );

								if( result.PollStatus == ChatPollStatus.OK ) {
									LastMessageSentID = result.PollLastMessageSentID;
									if( result.Messages != null )
										ProcessMessagesReceived( result );
								}

								await Task.Delay( 1000, Cancellation.Token );

							}

						}catch( OperationCanceledException ) {
							IndicateConnectionState( ClientConnectionStatus.Disconnected );
							return;
						}catch( Exception e ) {
							IndicateConnectionState( ClientConnectionStatus.Disconnected );
							throw e;
						}

					}

				}

			}

		}

		/// <summary>
		/// Stops polling and disconnects the client from the Steam server.
		/// </summary>
		public void Disconnect() {
			Cancellation.Cancel();
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

				if( message.Type == ChatMessageType.MessageText || message.Type == ChatMessageType.Typing ) {
					messages.Add( SteamChatMessage.CreateFromPollMessage( message ) );
				} else if( message.Type == ChatMessageType.PersonaStateChange || message.Type == ChatMessageType.PersonaRelationship ) {

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
						await newUser.GetProfileDataAsync( this );
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

		private void OnSteamChatClientConnectionChange( SteamChatConnectionChangeEventArgs e ) {
			EventHandler<SteamChatConnectionChangeEventArgs> handler;
			if( e.NewConnectionState == ClientConnectionStatus.Connected )
				handler = SteamChatConnected;
			else
				handler = SteamChatDisconnected;
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
