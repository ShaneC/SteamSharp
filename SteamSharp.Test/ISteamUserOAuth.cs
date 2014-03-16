using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteamSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp.Test {

	[TestClass]
	public class ISteamUserOAuth {

		[TestMethod]
		[TestCategory( "ISteamUserOAuth" )]
		public void GET_GetFriendsList() {

			Assert.IsFalse( ResourceConstants.OAuthAccessToken.Length < 1, "OPERATOR FAILURE: Must specify an OAuthAccessToken in ResourceConstants." );

			SteamClient client = new SteamClient();
			client.Authenticator = UserAuthenticator.ForProtectedResource( ResourceConstants.OAuthAccessToken );

			// Validate basic protected API call
			var response = SteamUserOAuth.GetFriendsList( client, ResourceConstants.OAuthUserSteamID );
			Assert.IsNotNull( response.Friends );

		}

	}

}

