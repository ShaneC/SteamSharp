using System;
using System.Net;
using Xunit;

namespace SteamSharp.TestFramework {

	public class IPlayerService {

		#region GetOwnedGames
		[Fact]
		public void GET_GetOwnedGames_ByClass() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

			var response = PlayerService.GetOwnedGames( client, "76561197960434622", true, true );

			Assert.NotNull( response );
			Assert.IsType<PlayerService.OwnedGames>( response );

			Assert.NotNull( response.GameCount );
			Assert.True( ( response.GameCount > 5 ) );

			Assert.NotNull( response.Games );
			Assert.True( ( response.Games.Count > 0 ) );

			Assert.NotNull( response.Games[1] );

			Assert.NotNull( response.Games[1].GameID );
			Assert.NotNull( response.Games[1].HasCommunityVisibileStats );
			Assert.NotNull( response.Games[1].ImgIconURL );
			Assert.NotNull( response.Games[1].ImgLogoURL );
			Assert.NotNull( response.Games[1].Name );
			Assert.NotNull( response.Games[1].PlaytimeForever );
			Assert.NotNull( response.Games[1].PlaytimeTwoWeeks );

		}
		#endregion

		#region GetRecentlyPlayedGames
		[Fact]
		public void GET_GetRecentlyPlayedGames_ByClass() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

			var response = PlayerService.GetRecentlyPlayedGames( client, "76561197960434622" );

			Assert.NotNull( response );
			Assert.IsType<PlayerService.PlayedGames>( response );

			Assert.NotNull( response.TotalCount );

			Assert.NotNull( response.Games );

			// Note: This test case will absolutely blow up if Robin goes two weeks without playing a game :)
			Assert.True( ( response.Games.Count > 0 ) );

			Assert.NotNull( response.Games[0] );

			Assert.NotNull( response.Games[0].GameID );
			Assert.NotNull( response.Games[0].ImgIconURL );
			Assert.NotNull( response.Games[0].ImgLogoURL );
			Assert.NotNull( response.Games[0].Name );
			Assert.NotNull( response.Games[0].PlaytimeForever );
			Assert.NotNull( response.Games[0].PlaytimeTwoWeeks );

		}
		#endregion

	}

}
