using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net;

namespace SteamSharp.Test {

	[TestClass]
	public class ISteamUserStats {

		#region GetGlobalAchievementPercentagesForApp
		[TestMethod]
		[TestCategory( "ISteamUserStats" )]
		public void GET_GetGlobalAchievementPercentagesForApp_ByClass() {

			SteamClient client = new SteamClient();

			var response = SteamUserStats.GetGlobalAchievementPercentagesForApp( client, 440 );

			Assert.IsNotNull( response );
			Assert.IsInstanceOfType( response, typeof( List<SteamUserStats.GlobalAchievement> ) );

		}

		[TestMethod]
		[TestCategory( "ISteamUserStats" )]
		public void GET_GetGlobalAchievementPercentagesForApp_NoValues() {

			SteamClient client = new SteamClient();
			SteamRequest request = new SteamRequest( "ISteamUserStats/GetGlobalAchievementPercentagesForApp/v0002/" );

			var response = client.Execute( request );

			Assert.AreEqual( HttpStatusCode.BadRequest, response.StatusCode );

		}
		#endregion

		#region GetPlayerAchievements 
		[TestMethod]
		[TestCategory( "ISteamUserStats" )]
		public void GET_GetPlayerAchievements_ByClass_NoLanguage() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.APIKey );

			var response = SteamUserStats.GetPlayerAchievements( client, "76561197972495328", 440 );

			Assert.IsNotNull( response );
			Assert.IsInstanceOfType( response, typeof( SteamUserStats.PlayerAchievements ) );

		}

		[TestMethod]
		[TestCategory( "ISteamUserStats" )]
		public void GET_GetPlayerAchievements_ByClass_EnglishLanguage() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.APIKey );

			var response = SteamUserStats.GetPlayerAchievements( client, "76561197972495328", 440, RequestedLangage.English );

			Assert.IsNotNull( response );
			Assert.IsInstanceOfType( response, typeof( SteamUserStats.PlayerAchievements ) );

		}

		[TestMethod]
		[TestCategory( "ISteamUserStats" )]
		public void GET_GetPlayerAchievements_ByClass_JapaneseLanguage() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.APIKey );

			var response = SteamUserStats.GetPlayerAchievements( client, "76561197972495328", 440, RequestedLangage.Japanese );

			Assert.IsNotNull( response );
			Assert.IsInstanceOfType( response, typeof( SteamUserStats.PlayerAchievements ) );

		}

		[TestMethod]
		[TestCategory( "ISteamUserStats" )]
		public void GET_GetPlayerAchievements_ByClass_RussianLanguage() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.APIKey );

			var response = SteamUserStats.GetPlayerAchievements( client, "76561197972495328", 440, RequestedLangage.Russian );

			Assert.IsNotNull( response );
			Assert.IsInstanceOfType( response, typeof( SteamUserStats.PlayerAchievements ) );

		}
		#endregion

		#region GetUserStatsForGame
		[TestMethod]
		[TestCategory( "ISteamUserStats" )]
		public void GET_GetUserStatsForGame_ByClass() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.APIKey );

			var response = SteamUserStats.GetUserStatsForGame( client, "76561197972495328", 440 );

			Assert.IsNotNull( response );
			Assert.IsInstanceOfType( response, typeof( SteamUserStats.PlayerStats ) );

			Assert.IsNotNull( response.GameName );
			Assert.IsNotNull( response.Stats );
			Assert.IsNotNull( response.SteamID );

			Assert.IsTrue( ( response.Stats.Count > 0 ) );

			Assert.IsNotNull( response.Stats[0] );

			Assert.IsNotNull( response.Stats[0].APIName );
			Assert.IsNotNull( response.Stats[0].Value );

		}
		#endregion

	}

}
