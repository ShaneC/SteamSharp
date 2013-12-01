using System;
using Xunit;
using SteamSharp;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SteamSharp.TestFramework {

	public class ISteamUser {

		[Fact]
		public void GET_GetPlayerSummaries_ByClass() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

			var response = SteamUser.GetPlayerSummaries( client, new string[] { "76561197960435530", "76561198067189899" } );

			Assert.NotNull( response );
			Assert.IsType<List<SteamUser.Player>>( response );

		}

		[Fact]
		public void GET_GetPlayerSummaries_ByClass_Unauthenticated() {

			SteamClient client = new SteamClient();

			Assert.Throws<AggregateException>( () => {
				SteamUser.GetPlayerSummaries( client, new string[] { "76561197960435530", "76561198067189899" } );
			} );

		}

		[Fact]
		public void GET_GetPlayerSummaries_NoValues() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

			SteamRequest request = new SteamRequest( "ISteamUser/GetPlayerSummaries/v0002/" );

			var response = client.Execute( request );

			Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );

		}

		[Fact]
		public void GET_GetPlayerSummaries_Unauthenticated() {

			SteamClient client = new SteamClient();

			SteamRequest request = new SteamRequest( "ISteamUser/GetPlayerSummaries/v0002/" );
			request.AddParameter( "steamids", "76561197960435530", ParameterType.QueryString );
			
			var response = client.Execute( request );

			Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );

		}

		[Fact]
		public void GET_GetPlayerSummaries_BadAuth() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( "8675309" );

			SteamRequest request = new SteamRequest( "ISteamUser/GetPlayerSummaries/v0002/" );
			request.AddParameter( "steamids", "76561197960435530", ParameterType.QueryString );

			var response = client.Execute( request );

			Assert.Equal( HttpStatusCode.Unauthorized, response.StatusCode );

		}

	}

}
