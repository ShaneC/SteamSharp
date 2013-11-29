using System;
using Xunit;
using SteamSharp;
using System.Net;

namespace SteamSharp.TestFramework {

	public class ISteamNews {

		[Fact]
		public void GET_GetNewsForApp_ByUri() {

			SteamClient client = new SteamClient();
			SteamRequest request = new SteamRequest( "ISteamNews/GetNewsForApp/v0002/" );

			request.AddParameter( "appid", 440 );
			request.AddParameter( "count", 2 );
			request.AddParameter( "maxlength", 100 );

			var response = client.Execute( request );

			Assert.Equal( HttpStatusCode.OK, response.StatusCode );
			Assert.NotNull( response.Content );

		}

		[Fact]
		public void GET_GetNewsForApp_ByEnums() {

			SteamClient client = new SteamClient();
			SteamRequest request = new SteamRequest( SteamInterface.ISteamNews, "GetNewsForApp", SteamMethodVersion.v0002 );

			request.AddParameter( "appid", 440 );
			request.AddParameter( "count", 2 );
			request.AddParameter( "maxlength", 100 );

			var response = client.Execute( request );

			Assert.Equal( HttpStatusCode.OK, response.StatusCode );
			Assert.NotNull( response.Content );

		}

		[Fact]
		public void GET_GetNewsForApp_ByClass() {

			var response = SteamNews.GetNewsForApp( 440, 2, 100 );

			Assert.Equal( HttpStatusCode.OK, response.StatusCode );
			Assert.NotNull( response.Content );

		}

		[Fact]
		public void GET_GetNewsForApp_NoValues() {

			SteamClient client = new SteamClient();
			SteamRequest request = new SteamRequest( "ISteamNews/GetNewsForApp/v0002/" );

			var response = client.Execute( request );

			Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );

		}

	}

}
