using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteamSharp.TestFramework.Helpers;
using System.Net;
using System.Net.Http;
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

		[Fact]
		public void POST_Can_Add_Body_NoParams_Raw() {

			using( SimulatedServer.Create( ResourceConstants.SimulatedServerUrl, Post_Body_Echo ) ) {

				SteamClient client = new SteamClient( ResourceConstants.SimulatedServerUrl );
				var request = new SteamRequest( "/resource", HttpMethod.Post );
				request.DataFormat = PostDataFormat.Raw;

				string payload = "myContent 123 abc's fun!";
				
				request.AddBody( payload );

				var response = client.Execute( request );

				Assert.NotNull( response );
				Assert.NotNull( response.Content );
				Assert.Equal( payload, response.Content );

			}

		}

		[Fact]
		public void POST_Can_Add_Body_WithParams_Raw() {

			using( SimulatedServer.Create( ResourceConstants.SimulatedServerUrl, Post_Body_Echo ) ) {

				SteamClient client = new SteamClient( ResourceConstants.SimulatedServerUrl );
				var request = new SteamRequest( "/resource", HttpMethod.Post );
				request.DataFormat = PostDataFormat.Raw;

				string payload = "myContent 123 abc's fun!";
				request.AddBody( payload );

				request.AddParameter( "howdy", "doody" );
				request.AddParameter( "foo", "bar" );

				var response = client.Execute( request );

				Assert.NotNull( response );
				Assert.NotNull( response.Content );
				Assert.Equal( payload, response.Content );

			}

		}

		[Fact]
		public void POST_Can_Add_Body_NoParams_Json() {

			using( SimulatedServer.Create( ResourceConstants.SimulatedServerUrl, Post_Body_Echo ) ) {

				SteamClient client = new SteamClient( ResourceConstants.SimulatedServerUrl );
				var request = new SteamRequest( "/resource", HttpMethod.Post );

				// Verify request defaults to JSON
				Assert.Equal( PostDataFormat.Json, request.DataFormat );

				string payload = "myContent 123 abc's fun!";

				request.AddBody( payload );

				var response = client.Execute( request );

				Assert.NotNull( response );
				Assert.NotNull( response.Content );
				Assert.Equal( JsonConvert.SerializeObject( payload ), response.Content );

			}

		}

		[Fact]
		public void POST_Can_Add_Body_WithParams_Json() {

			using( SimulatedServer.Create( ResourceConstants.SimulatedServerUrl, Post_Body_Echo ) ) {

				SteamClient client = new SteamClient( ResourceConstants.SimulatedServerUrl );
				var request = new SteamRequest( "/resource", HttpMethod.Post );
				request.DataFormat = PostDataFormat.Json;

				request.AddBody( "myContent 123 abc's fun!" );

				request.AddParameter( "howdy", "doody" );
				request.AddParameter( "foo", "bar" );

				var response = client.Execute( request );

				Assert.NotNull( response );
				Assert.NotNull( response.Content );

				JObject obj = JObject.Parse( response.Content );

				Assert.Equal( "doody", obj["howdy"] );
				Assert.Equal( "bar", obj["foo"] );
				Assert.Equal( "myContent 123 abc's fun!", obj["application/json"] );

			}

		}

		private void Timeout_Simulator( HttpListenerContext context ) {
			System.Threading.Thread.Sleep( 5000 );
		}

		private void Post_Body_Echo( HttpListenerContext context ) {
			var request = context.Request;
			context.Response.OutputStream.WriteStringUTF8( request.InputStream.StreamToString() );
		}

	}

}
