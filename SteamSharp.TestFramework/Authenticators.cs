using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SteamSharp.TestFramework {
	
	public class Authenticators {
	
		[Fact]
		public void Verify_APIKey_Added() {

			// Did you remember to add your API Token?
			Assert.NotNull( ResourceConstants.AccessToken );
			Assert.NotEqual( ResourceConstants.AccessToken, "" );

			SteamClient client = new SteamClient {
				BaseAPIEndpoint = "http://hopefullyth-is-domain-nev-rexists.com"
			};

			client.Authenticator = SteamSharp.Authenticators.APIKeyAuthenticator.ForProtectedResource( ResourceConstants.AccessToken );

			SteamRequest request = new SteamRequest( "/resource" );
			var response = client.Execute( request );

			Assert.Equal( ResponseStatus.Error, response.ResponseStatus );

			//Assert.True( response.Request.Parameters.Any( p => p.Name == "key" && ((long)p.Value) == ResourceConstants.AccessToken && p.Type == ParameterType.QueryString ) );

		}

	}

}
