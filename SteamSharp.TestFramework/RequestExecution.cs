using SteamSharp.TestFramework.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SteamSharp.TestFramework {

	public class RequestExecution {

		[Fact]
		public void NonExistant_Base_Returns_Error() {

			SteamClient client = new SteamClient( "http://hopefullyth-is-domain-nev-rexists.com" );

			SteamRequest request = new SteamRequest( "/resource" );
			var response = client.Execute( request );

			Assert.Equal( ResponseStatus.Error, response.ResponseStatus );

		}

		[Fact]
		public void ClientContext_Request_Correctly_Times_Out() {

			using( SimulatedServer.Create( ResourceConstants.SimulatedServerUrl, Timeout_Simulator ) ) {

				SteamClient client = new SteamClient( ResourceConstants.SimulatedServerUrl );
				client.Timeout = 1000;

				SteamRequest request = new SteamRequest( "/404" );
				var response = client.Execute( request );

				Assert.Equal( ResponseStatus.TimedOut, response.ResponseStatus );

			}

		}

		[Fact]
		public void RequestContext_Request_Correctly_Times_Out() {

			using( SimulatedServer.Create( ResourceConstants.SimulatedServerUrl, Timeout_Simulator ) ) {

				SteamClient client = new SteamClient( ResourceConstants.SimulatedServerUrl );

				SteamRequest request = new SteamRequest( "/404" );
				request.Timeout = 1000;
				var response = client.Execute( request );

				Assert.Equal( ResponseStatus.TimedOut, response.ResponseStatus );

			}

		}

		private static void Timeout_Simulator( HttpListenerContext context ) {
			System.Threading.Thread.Sleep( 5000 );
		}

	}

}
