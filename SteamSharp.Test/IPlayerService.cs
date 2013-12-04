using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace SteamSharp.Test {

	[TestClass]
	public class IPlayerService {

		#region GetOwnedGames
		[TestMethod]
		[TestCategory( "IPlayerService" )]
		public void GET_GetOwnedGames_ByClass() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

			var response = PlayerService.GetOwnedGames( client, "76561197960434622", true, true );
			
			Assert.IsNotNull( response );
			Assert.IsInstanceOfType( response, typeof(PlayerService.OwnedGames) );

			Assert.IsNotNull( response.GameCount );
			Assert.IsTrue( ( response.GameCount > 5 ) );

			Assert.IsNotNull( response.Games );
			Assert.IsTrue( ( response.Games.Count > 0 ) );

			Assert.IsNotNull( response.Games[1] );

			Assert.IsNotNull( response.Games[1].GameID );
			Assert.IsNotNull( response.Games[1].HasCommunityVisibileStats );
			Assert.IsNotNull( response.Games[1].ImgIconURL );
			Assert.IsNotNull( response.Games[1].ImgLogoURL );
			Assert.IsNotNull( response.Games[1].Name );
			Assert.IsNotNull( response.Games[1].PlaytimeForever );
			Assert.IsNotNull( response.Games[1].PlaytimeTwoWeeks );

		}
		#endregion

		#region GetRecentlyPlayedGames
		[TestMethod]
		[TestCategory( "IPlayerService" )]
		public void GET_GetRecentlyPlayedGames_ByClass() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

			var response = PlayerService.GetRecentlyPlayedGames( client, "76561197960434622" );

			Assert.IsNotNull( response );
			Assert.IsInstanceOfType( response, typeof( PlayerService.PlayedGames ) );

			Assert.IsNotNull( response.TotalCount );

			Assert.IsNotNull( response.Games );

			// Note: This test case will absolutely blow up if Robin goes two weeks without playing a game :)
			Assert.IsTrue( ( response.Games.Count > 0 ) );

			Assert.IsNotNull( response.Games[0] );

			Assert.IsNotNull( response.Games[0].GameID );
			Assert.IsNotNull( response.Games[0].ImgIconURL );
			Assert.IsNotNull( response.Games[0].ImgLogoURL );
			Assert.IsNotNull( response.Games[0].Name );
			Assert.IsNotNull( response.Games[0].PlaytimeForever );
			Assert.IsNotNull( response.Games[0].PlaytimeTwoWeeks );

		}
		#endregion

		#region IsPlayingSharedGame
		[TestMethod]
		[TestCategory( "IPlayerService" )]
		public void GET_IsPlayingSharedGame_ByClass() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

			var response = PlayerService.IsPlayingSharedGame( client, "76561197960434622", 440 );

			Assert.IsNotNull( response );
			Assert.IsInstanceOfType( response, typeof( PlayerService.SharedGameData ) );

			if( response.IsUserPlayingSharedGame ) {
				Assert.IsNotNull( response.GameBorrowerSteamID );
				Assert.IsNotNull( response.GameOwnerSteamID );
				Assert.AreEqual( "76561197960434622", response.GameBorrowerSteamID );
				Assert.AreNotEqual( "0", response.GameOwnerSteamID );
			} else {
				Assert.IsNull( response.GameBorrowerSteamID );
				Assert.IsNull( response.GameOwnerSteamID );
			}

		}
		#endregion

	}

}
