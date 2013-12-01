using System;
using Xunit;
using SteamSharp;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SteamSharp.TestFramework {

	public class ISteamUserStats {

		[Fact]
		public void GET_GetGlobalAchievementPercentagesForApp_ByClass() {

			SteamClient client = new SteamClient();

			var response = SteamUserStats.GetGlobalAchievementPercentagesForApp( client, 440 );

			Assert.NotNull( response );
			Assert.IsType<List<SteamUserStats.Achievement>>( response );

		}

		[Fact]
		public void GET_GetGlobalAchievementPercentagesForApp_NoValues() {

			SteamClient client = new SteamClient();
			SteamRequest request = new SteamRequest( "ISteamUserStats/GetGlobalAchievementPercentagesForApp/v0002/" );

			var response = client.Execute( request );

			Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );

		}

	}

}
