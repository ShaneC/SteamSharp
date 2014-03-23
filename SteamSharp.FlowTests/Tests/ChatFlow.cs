using SteamSharp.Authenticators;
using System;

namespace SteamSharp.FlowTests.Tests {

	public class ChatFlow : ITestClass {

		public bool Invoke() {

			try {

				string accessToken = WriteConsole.Prompt( "Please enter valid OAuth access token:" );
				SteamID targetUser = new SteamID( WriteConsole.Prompt( "Please enter valid SteamID of target user:" ) );

				SteamClient client = new SteamClient();
				client.Authenticator = UserAuthenticator.ForProtectedResource( accessToken );

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
			WriteConsole.Success( e.NewMessages.Count + " new message(s) received!" );
		}

		private void chatClient_SteamChatConnected( object sender, SteamSharp.SteamChatClient.SteamChatConnectionChangeEventArgs e ) {
			WriteConsole.Success( "-- STEAM CLIENT CONNECTED --" );
		}

		private void chatClient_SteamChatDisconnected( object sender, SteamChatClient.SteamChatConnectionChangeEventArgs e ) {
			WriteConsole.Success( "-- STEAM CLIENT DISCONNECTED --" );
		}

	}

}
