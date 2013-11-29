using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SteamSharp.TestFramework {

	public class RequestExecution {

		[Fact]
		public void NonExistant_Base_Returns_Error() {

			SteamClient client = new SteamClient {
				BaseAPIEndpoint = "http://hopefullyth-is-domain-nev-rexists.com"
			};

			SteamRequest request = new SteamRequest( "/resource" );
			var response = client.Execute( request );

			Assert.Equal( ResponseStatus.Error, response.ResponseStatus );

		}

		[Fact]
		public void ClientContext_Request_Correctly_Times_Out() {

			SteamClient client = new SteamClient {
				BaseAPIEndpoint = "http://bing.com"
			};

			client.Timeout = 1;
			SteamRequest request = new SteamRequest( "/resource" );
			var response = client.Execute( request );

			Assert.Equal( ResponseStatus.TimedOut, response.ResponseStatus );

		}

		[Fact]
		public void RequestContext_Request_Correctly_Times_Out() {

			SteamClient client = new SteamClient {
				BaseAPIEndpoint = "http://bing.com"
			};

			SteamRequest request = new SteamRequest( "/resource" );
			request.Timeout = 1;
			var response = client.Execute( request );

			Assert.Equal( ResponseStatus.TimedOut, response.ResponseStatus );

		}

	}

}
