using System.Collections.Generic;
using System.Net;
using Xunit;

namespace SteamSharp.TestFramework {

	public class ISteamUserStats {

		#region GetGlobalAchievementPercentagesForApp
		[Fact]
		public void GET_GetGlobalAchievementPercentagesForApp_ByClass() {

			SteamClient client = new SteamClient();

			var response = SteamUserStats.GetGlobalAchievementPercentagesForApp( client, 440 );

			Assert.NotNull( response );
			Assert.IsType<List<SteamUserStats.GlobalAchievement>>( response );

		}

		[Fact]
		public void GET_GetGlobalAchievementPercentagesForApp_NoValues() {

			SteamClient client = new SteamClient();
			SteamRequest request = new SteamRequest( "ISteamUserStats/GetGlobalAchievementPercentagesForApp/v0002/" );

			var response = client.Execute( request );

			Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );

		}
		#endregion

		#region GetPlayerAchievements 
		[Fact]
		public void GET_GetPlayerAchievements_ByClass_NoLanguage() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

			var response = SteamUserStats.GetPlayerAchievements( client, "76561197972495328", 440 );

			Assert.NotNull( response );
			Assert.IsType<SteamUserStats.PlayerAchievements>( response );

		}

		[Fact]
		public void GET_GetPlayerAchievements_ByClass_EnglishLanguage() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

			var response = SteamUserStats.GetPlayerAchievements( client, "76561197972495328", 440, RequestedLangage.English );

			Assert.NotNull( response );
			Assert.IsType<SteamUserStats.PlayerAchievements>( response );

		}

		[Fact]
		public void GET_GetPlayerAchievements_ByClass_JapaneseLanguage() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

			var response = SteamUserStats.GetPlayerAchievements( client, "76561197972495328", 440, RequestedLangage.Japanese );

			Assert.NotNull( response );
			Assert.IsType<SteamUserStats.PlayerAchievements>( response );

		}

		[Fact]
		public void GET_GetPlayerAchievements_ByClass_RussianLanguage() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

			var response = SteamUserStats.GetPlayerAchievements( client, "76561197972495328", 440, RequestedLangage.Russian );

			Assert.NotNull( response );
			Assert.IsType<SteamUserStats.PlayerAchievements>( response );

		}
		#endregion

		#region GetUserStatsForGame
		[Fact]
		public void GET_GetUserStatsForGame_ByClass() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

			var response = SteamUserStats.GetUserStatsForGame( client, "76561197972495328", 440 );

			Assert.NotNull( response );
			Assert.IsType<SteamUserStats.PlayerStats>( response );

			Assert.NotNull( response.GameName );
			Assert.NotNull( response.Stats );
			Assert.NotNull( response.SteamID );

			Assert.True( ( response.Stats.Count > 0 ) );

			Assert.NotNull( response.Stats[0] );

			Assert.NotNull( response.Stats[0].APIName );
			Assert.NotNull( response.Stats[0].Value );

		}
		#endregion

	}

}
