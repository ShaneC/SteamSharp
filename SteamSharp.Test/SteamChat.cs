using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteamSharp.Authenticators;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SteamSharp.Test {

	[TestClass]
	public class SteamChat {

		[TestMethod]
		public async Task LogOnTest() {

			SteamClient client = new SteamClient();
			client.Authenticator = UserAuthenticator.ForProtectedResource( AccessConstants.OAuthAccessToken );

			SteamChatClient chatClient = new SteamChatClient();

			chatClient.SteamChatConnectionChanged += chatClient_SteamChatConnected;

			chatClient.SteamChatMessagesReceived += chatClient_SteamChatMessagesReceived;

			await chatClient.LogOn( client );

		}

		private void chatClient_SteamChatMessagesReceived( object sender, SteamChatClient.SteamChatMessagesReceivedEventArgs e ) {
			Debug.WriteLine( e.NewMessages.Count + " new message(s) received!" );
		}

		private void chatClient_SteamChatConnected( object sender, SteamSharp.SteamChatClient.SteamChatConnectionChangeEventArgs e ) {
			switch( e.NewConnectionState ) {
				case ClientConnectionStatus.Connected: Debug.WriteLine( "-- STEAM CLIENT CONNECTED --" ); break;
				case ClientConnectionStatus.Disconnected: Debug.WriteLine( "-- STEAM CLIENT DISCONNECTED --" ); break;
				case ClientConnectionStatus.Connecting: Debug.WriteLine( "-- STEAM CLIENT CONNECTING --" ); break;
			}
		}

	}

}
