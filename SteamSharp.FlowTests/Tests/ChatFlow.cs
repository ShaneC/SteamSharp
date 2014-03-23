using SteamSharp.Authenticators;
using System;

namespace SteamSharp.FlowTests.Tests {

	public class ChatFlow : ITestClass {

		public bool Invoke() {

			try {

				SteamID targetUser = new SteamID( "76561198129947779" );

				SteamClient client = new SteamClient();
				client.Authenticator = UserAuthenticator.ForProtectedResource( AccessConstants.OAuthAccessToken );

				SteamChatClient chatClient = new SteamChatClient();

				chatClient.SteamChatConnected += chatClient_SteamChatConnected;
				chatClient.SteamChatDisconnected += chatClient_SteamChatDisconnected;

				chatClient.SteamChatMessagesReceived += chatClient_SteamChatMessagesReceived;

				chatClient.LogOn( client ).Wait();

				while( true ) {
					chatClient.SendMessage( targetUser, WriteConsole.Prompt( "Type New Message: " ) );
				}
				
			} catch( Exception e ) {
				WriteConsole.Error( e.Message + "\n" + e.ToString() );
				return false;
			}

		}

		private void chatClient_SteamChatMessagesReceived( object sender, SteamChatClient.SteamChatMessagesReceivedEventArgs e ) {

			foreach( var message in e.NewMessages ) {

				switch( message.Type ) {
					case ChatMessageType.Typing: WriteConsole.Information( message.PersonaName + " is typing..." ); break;
					case ChatMessageType.MessageText: WriteConsole.Information( message.PersonaName + ": " + message.Text ); break;
					case ChatMessageType.PersonaStateChange: WriteConsole.Information( message.PersonaName + " is now " + Enum.GetName( typeof( PersonaState ), message.PersonaState ) ); break;
					default: WriteConsole.Error( "Unknown message type detected!" ); break;
				}

			}

		}

		private void chatClient_SteamChatConnected( object sender, SteamSharp.SteamChatClient.SteamChatConnectionChangeEventArgs e ) {
			WriteConsole.Success( "-- STEAM CLIENT CONNECTED --" );
		}

		private void chatClient_SteamChatDisconnected( object sender, SteamChatClient.SteamChatConnectionChangeEventArgs e ) {
			WriteConsole.Success( "-- STEAM CLIENT DISCONNECTED --" );
		}

	}

}
