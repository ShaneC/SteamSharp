using System.Collections.Generic;
using System.Net;
using Xunit;

namespace SteamSharp.TestFramework {

	public class ISteamUser {

		#region GetPlayerSummaries
		[Fact]
		public void GET_GetPlayerSummaries_ByClass() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

			var response = SteamUser.GetPlayerSummaries( client, new string[] { "76561197960435530", "76561198067189899" } );

			Assert.NotNull( response );
			Assert.IsType<List<SteamUser.Player>>( response );
			Assert.Equal( 2, response.Count );

		}

		[Fact]
		public void GET_GetPlayerSummaries_ByClass_Unauthenticated() {

			SteamClient client = new SteamClient();

			Assert.Throws<SteamRequestException>( () => {
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
		#endregion

		#region GetPlayerSummary
		[Fact]
		public void GET_GetPlayerSummary_ByClass() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

			var response = SteamUser.GetPlayerSummary( client, "76561197960435530" );

			Assert.NotNull( response );
			Assert.IsType<SteamUser.Player>( response );

		}

		[Fact]
		public void GET_GetPlayerSummary_ByClass_Unauthenticated() {

			SteamClient client = new SteamClient();

			Assert.Throws<SteamRequestException>( () => {
				SteamUser.GetPlayerSummary( client, "76561197960435530" );
			} );

		}
		#endregion

		#region GetFriendList
		[Fact]
		public void GET_GetFriendList_ByClass_AllRelationships() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

			var response = SteamUser.GetFriendList( client, "76561197960435530", SteamUser.PlayerRelationshipType.All );

			Assert.NotNull( response );
			Assert.IsType<List<SteamUser.Friend>>( response );

		}

		[Fact]
		public void GET_GetFriendList_ByClass_FriendRelationships() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

			var response = SteamUser.GetFriendList( client, "76561197960435530", SteamUser.PlayerRelationshipType.Friend );

			Assert.NotNull( response );
			Assert.IsType<List<SteamUser.Friend>>( response );

		}

		[Fact]
		public void GET_GetFriendList_ByClass_Unauthenticated() {

			SteamClient client = new SteamClient();

			Assert.Throws<SteamRequestException>( () => {
				SteamUser.GetFriendList( client, "76561197960435530", SteamUser.PlayerRelationshipType.All );
			} );

		}
		#endregion
		
	}

}
