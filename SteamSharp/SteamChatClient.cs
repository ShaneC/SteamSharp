using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SteamSharp {
	
	public sealed partial class SteamChatClient : SteamClient {

		public event EventHandler<SteamChatMessagesReceivedEventArgs> SteamChatMessagesReceived;
		public event EventHandler<SteamChatConnectionChangeEventArgs> SteamChatConnected;
		public event EventHandler<SteamChatConnectionChangeEventArgs> SteamChatDisconnected;

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

								LastMessageSentID = result.PollLastMessageSentID;

								System.Diagnostics.Debug.WriteLine( "-- Timeout = " + result.SecondsUntilTimeout );
								System.Diagnostics.Debug.WriteLine( "-- Error = " + result.Error );

								if( result.Messages != null )
									System.Diagnostics.Debug.WriteLine( "-- Message Count = " + result.Messages.Count );

								await Task.Delay( 1000, Cancellation.Token );

							}

						}catch( OperationCanceledException ) {
							IndicateConnectionState( ClientConnectionStatus.Disconnected );
							return;
						}catch( Exception e ) {
							IndicateConnectionState( ClientConnectionStatus.Disconnected );
							if( e is SteamRequestException || e is SteamAuthenticationException )
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

		public async Task IndicateTyping( SteamID destinationUser ) {
			await SendMessage( destinationUser, null );
		}

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

		private void OnSteamChatClientConnectionChange( SteamChatConnectionChangeEventArgs e ) {
			EventHandler<SteamChatConnectionChangeEventArgs> handler;
			if( e.NewConnectionState == ClientConnectionStatus.Connected )
				handler = SteamChatConnected;
			else
				handler = SteamChatDisconnected;
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
			/// UTC DateTime when the change took place.
			/// </summary>
			public DateTime ChangeDateTime { get; set; }

			/// <summary>
			/// List of the <see cref="SteamChat.SteamChatMessage"/> objects which have been received since the last event.
			/// </summary>
			public List<SteamSharp.SteamChat.SteamChatMessage> NewMessages { get; set; }

		}

	}

}
