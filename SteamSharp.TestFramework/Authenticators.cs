using SteamSharp.TestFramework.Helpers;
using System;
using System.Net;
using System.Web;
using Xunit;

namespace SteamSharp.TestFramework {
	
	public class Authenticators {
	
		[Fact]
		public void Verify_APIKey_Added() {

			// Did you remember to add your API Token?
			Assert.NotNull( ResourceConstants.AccessToken );
			Assert.NotEqual( ResourceConstants.AccessToken, "" );

			using( SimulatedServer.Create( ResourceConstants.SimulatedServerUrl, ApiKeyEchoHandler ) ) {

				var client = new SteamClient( ResourceConstants.SimulatedServerUrl );

				client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

				SteamRequest request = new SteamRequest( "/resource" );
				var response = client.Execute( request );

				Assert.Equal( "key|" + ResourceConstants.AccessToken, response.Content );

			}

		}

		private static void ApiKeyEchoHandler( HttpListenerContext context ) {

			var data = context.Request;

			Uri requestUri = context.Request.Url;
			var queryParams = HttpUtility.ParseQueryString( requestUri.Query );

			string output = "";
			if( queryParams["key"] != null )
				output = "key|" + queryParams["key"];

			context.Response.OutputStream.WriteStringUTF8( output );

		}

	}

}
