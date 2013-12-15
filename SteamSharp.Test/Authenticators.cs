using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteamSharp.Test.TestHelpers;
using System;
using System.Diagnostics;
using System.Net;
using System.Web;

namespace SteamSharp.Test {
	
	[TestClass]
	public class Authenticators {

		[TestMethod]
		[TestCategory( "Authenticators" )]
		public void Verify_APIKey_Added() {

			// Did you remember to add your API Token?
			Assert.IsNotNull( ResourceConstants.AccessToken );
			Assert.AreNotEqual( ResourceConstants.AccessToken, "" );

			using( SimulatedServer.Create( ResourceConstants.SimulatedServerUrl, ApiKeyEchoHandler ) ) {

				var client = new SteamClient( ResourceConstants.SimulatedServerUrl );

				client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

				SteamRequest request = new SteamRequest( "/resource" );
				var response = client.Execute( request );

				Assert.AreEqual( "key|" + ResourceConstants.AccessToken, response.Content );

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

		[TestMethod]
		[TestCategory( "Authenticators" )]
		public void UserAuth_AccessToken() {

			SteamClient client = new SteamClient();
			string username = "username";
			string password = "password";

			// Input actual username and password into debugger
			Debugger.Break();

			var response = SteamSharp.Authenticators.UserAuthenticator.GetAccessTokenForUserAsync( username, password );

			Assert.IsNotNull( response );
			Assert.IsTrue( ( response.IsSuccessful || response.IsCaptchaNeeded || response.IsSteamGuardNeeded ) );

		}

		//[TestMethod]
		//[TestCategory( "Authenticators" )]
		//public void RSA_Encrypt_Decrypt() {

		//	string modulus = "a5261939975948bb7a58dffe5ff54e65f0498f9175f5a09288810b8975871e99af3b5dd94057b0fc07535f5f97444504fa35169d461d0d30cf0192e307727c065168c788771c561a9400fb49175e9e6aa4e23fe11af69e9412dd23b0cb6684c4c2429bce139e848ab26d0829073351f4acd36074eafd036a5eb83359d2a698d3";
		//	string exponent = "010001";
		//	string privateKey = "8e9912f6d3645894e8d38cb58c0db81ff516cf4c7e5a14c7f1eddb1459d2cded4d8d293fc97aee6aefb861859c8b6a3d1dfe710463e1f9ddc72048c09751971c4a580aa51eb523357a3cc48d31cfad1d4a165066ed92d4748fb6571211da5cb14bc11b6e2df7c1a559e6d5ac1cd5c94703a22891464fba23d0d965086277a161";

		//	RSAHelper rsa = new RSAHelper( modulus.HexToByteArray(), exponent.HexToByteArray() );

		//	string message = "Howdy do! foo ba$r/b@rs.";

		//	byte[] cipherText = rsa.Encrypt( Encoding.UTF8.GetBytes( message ) );

		//	byte[] roundTrip = rsa.Decrypt( cipherText, privateKey.HexToByteArray() );
		//	string decodedRoundTrip = Encoding.UTF8.GetString( roundTrip );

		//	Assert.AreEqual( message, decodedRoundTrip );

		//}

		private static void RSAStringEchoHandler( HttpListenerContext context ) {

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
