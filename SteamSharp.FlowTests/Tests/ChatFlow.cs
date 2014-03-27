using SteamSharp.Authenticators;
using System;

namespace SteamSharp.FlowTests.Tests {

	public class ChatFlow : ITestClass {

		SteamChatClient chatClient = new SteamChatClient();

		public bool Invoke() {

			try {

				SteamID targetUser = new SteamID( "76561198129947779" );

				SteamClient client = new SteamClient();
				client.Authenticator = UserAuthenticator.ForProtectedResource( AccessConstants.OAuthAccessToken );

				chatClient.SteamChatConnected += chatClient_SteamChatConnected;
				chatClient.SteamChatDisconnected += chatClient_SteamChatDisconnected;

				chatClient.SteamChatMessagesReceived += chatClient_SteamChatMessagesReceived;
				chatClient.SteamChatUserStateChange += chatClient_SteamChatUserStateChange;

				chatClient.LogOn( client ).Wait();

				while( true ) {
					chatClient.SendMessage( targetUser, WriteConsole.Prompt( "Type New Message: " ) );
				}
				
			} catch( Exception e ) {
				WriteConsole.Error( e.Message + "\n" + e.ToString() );
				return false;
			}

		}

		void chatClient_SteamChatUserStateChange( object sender, SteamChatClient.SteamChatUserStateChangeEventArgs e ) {
			foreach( var notification in e.StateChanges ) {
				if( notification.Type == ChatMessageType.PersonaStateChange )
					WriteConsole.Information( notification.PersonaName + " is now " + Enum.GetName( typeof( PersonaState ), notification.PersonaState ) );
			}
		}

		private void chatClient_SteamChatMessagesReceived( object sender, SteamChatClient.SteamChatMessagesReceivedEventArgs e ) {
			foreach( var message in e.NewMessages ) {
				switch( message.Type ) {
					case ChatMessageType.Typing: WriteConsole.Information( chatClient.FriendsList.Friends[message.FromUser].PlayerInfo.PersonaName + " is typing..." ); break;
					case ChatMessageType.MessageText: WriteConsole.Information( chatClient.FriendsList.Friends[message.FromUser].PlayerInfo.PersonaName + ": " + message.Text ); break;
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
