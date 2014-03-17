using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteamSharp.Test.TestHelpers;
using System.Collections.Generic;
using System.Net;

namespace SteamSharp.Test {

	[TestClass]
	public class ISteamUser {

		#region GetPlayerSummaries
		[TestMethod]
		[TestCategory( "ISteamUser" )]
		public void GET_GetPlayerSummaries_ByClass() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( AccessConstants.APIKey );

			var response = SteamCommunity.GetUsers( client, SteamID.CreateFromList( new string[] { "76561197960435530", "76561198067189899" } ) );

			Assert.IsNotNull( response );
			Assert.IsInstanceOfType( response, typeof( List<SteamUser> ) );
			
			Assert.AreEqual( 2, response.Count );

		}

		[TestMethod]
		[TestCategory( "ISteamUser" )]
		public void GET_GetPlayerSummaries_ByClass_Unauthenticated() {

			SteamClient client = new SteamClient();

			AssertException.Throws<SteamAuthenticationException>( () => {
				SteamCommunity.GetUsers( client, SteamID.CreateFromList( new string[] { "76561197960435530", "76561198067189899" } ) );
			} );

		}

		[TestMethod]
		[TestCategory( "ISteamUser" )]
		public void GET_GetPlayerSummaries_NoValues() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( AccessConstants.APIKey );

			SteamRequest request = new SteamRequest( "ISteamUser/GetPlayerSummaries/v0002/" );

			var response = client.Execute( request );

			Assert.AreEqual( HttpStatusCode.BadRequest, response.StatusCode );

		}

		[TestMethod]
		[TestCategory( "ISteamUser" )]
		public void GET_GetPlayerSummaries_Unauthenticated() {

			SteamClient client = new SteamClient();

			SteamRequest request = new SteamRequest( "ISteamUser/GetPlayerSummaries/v0002/" );
			request.AddParameter( "steamids", "76561197960435530", ParameterType.QueryString );
			
			var response = client.Execute( request );

			Assert.AreEqual( HttpStatusCode.BadRequest, response.StatusCode );

		}

		[TestMethod]
		[TestCategory( "ISteamUser" )]
		public void GET_GetPlayerSummaries_BadAuth() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( "8675309" );

			SteamRequest request = new SteamRequest( "ISteamUser/GetPlayerSummaries/v0002/" );
			request.AddParameter( "steamids", "76561197960435530", ParameterType.QueryString );

			var response = client.Execute( request );

			Assert.AreEqual( HttpStatusCode.Unauthorized, response.StatusCode );

		}
		#endregion

		#region GetPlayerSummary
		[TestMethod]
		[TestCategory( "ISteamUser" )]
		public void GET_GetPlayerSummary_ByClass() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( AccessConstants.APIKey );

			var response = SteamCommunity.GetUser( client, new SteamID( "76561197960435530" ) );

			Assert.IsNotNull( response );
			Assert.IsInstanceOfType( response, typeof( SteamUser ) );

		}

		[TestMethod]
		[TestCategory( "ISteamUser" )]
		public void GET_GetPlayerSummary_ByClass_Unauthenticated() {

			SteamClient client = new SteamClient();

			AssertException.Throws<SteamAuthenticationException>( () => {
				SteamCommunity.GetUser( client, new SteamID( "76561197960435530" ) );
			} );

		}
		#endregion

		#region GetFriendList
		[TestMethod]
		[TestCategory( "ISteamUser" )]
		public void GET_GetFriendList_ByClass_AllRelationships() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( AccessConstants.APIKey );

			var response = SteamCommunity.GetFriendsList( client, new SteamID( "76561197960435530" ) );

			Assert.IsNotNull( response );
			Assert.IsInstanceOfType( response, typeof( SteamCommunity.SteamFriendsList ) );

		}

		[TestMethod]
		[TestCategory( "ISteamUser" )]
		public void GET_GetFriendList_ByClass_FriendRelationships() {

			SteamClient client = new SteamClient();
			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( AccessConstants.APIKey );

			var response = SteamCommunity.GetFriendsList( client, new SteamID( "76561197960435530" ) );

			Assert.IsNotNull( response );
			Assert.IsInstanceOfType( response, typeof( SteamCommunity.SteamFriendsList ) );

		}

		[TestMethod]
		[TestCategory( "ISteamUser" )]
		public void GET_GetFriendList_ByClass_Unauthenticated() {

			SteamClient client = new SteamClient();

			AssertException.Throws<SteamAuthenticationException>( () => {
				SteamCommunity.GetFriendsList( client, new SteamID( "76561197960435530" ) );
			} );

		}
		#endregion
		
	}

}
