using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteamSharp.Test.TestHelpers;
using System.Net;
using System.Net.Http;

namespace SteamSharp.Test {

	[TestClass]
	public class RequestExecution {

		[TestMethod]
		[TestCategory( "Network Behavior" )]
		public void NonExistant_Base_Returns_Error() {

			SteamClient client = new SteamClient( "http://hopefullyth-is-domain-nev-rexists.com" );

			SteamRequest request = new SteamRequest( "/resource" );
			var response = client.Execute( request );

			Assert.AreEqual( ResponseStatus.Error, response.ResponseStatus );

		}

		[TestMethod]
		[TestCategory( "Network Behavior" )]
		public void ClientContext_Request_Correctly_Times_Out() {

			using( SimulatedServer.Create( AccessConstants.SimulatedServerUrl, Timeout_Simulator ) ) {

				SteamClient client = new SteamClient( AccessConstants.SimulatedServerUrl );
				client.Timeout = 1000;

				SteamRequest request = new SteamRequest( "/404" );
				var response = client.Execute( request );

				Assert.AreEqual( ResponseStatus.TimedOut, response.ResponseStatus );

			}

		}

		[TestMethod]
		[TestCategory( "Network Behavior" )]
		public void RequestContext_Request_Correctly_Times_Out() {

			using( SimulatedServer.Create( AccessConstants.SimulatedServerUrl, Timeout_Simulator ) ) {

				SteamClient client = new SteamClient( AccessConstants.SimulatedServerUrl );

				SteamRequest request = new SteamRequest( "/404" );
				request.Timeout = 1000;
				var response = client.Execute( request );

				Assert.AreEqual( ResponseStatus.TimedOut, response.ResponseStatus );

			}

		}

		[TestMethod]
		[TestCategory( "POST Submission" )]
		public void POST_Can_Add_Body_NoParams_Raw() {

			using( SimulatedServer.Create( AccessConstants.SimulatedServerUrl, Post_Body_Echo ) ) {

				SteamClient client = new SteamClient( AccessConstants.SimulatedServerUrl );
				var request = new SteamRequest( "/resource", HttpMethod.Post );
				request.DataFormat = PostDataFormat.Raw;

				string payload = "myContent 123 abc's fun!";
				
				request.AddBody( payload );

				var response = client.Execute( request );

				Assert.IsNotNull( response );
				Assert.IsNotNull( response.Content );
				Assert.AreEqual( payload, response.Content );

			}

		}

		[TestMethod]
		[TestCategory( "POST Submission" )]
		public void POST_Can_Add_Body_WithParams_Raw() {

			using( SimulatedServer.Create( AccessConstants.SimulatedServerUrl, Post_Body_Echo ) ) {

				SteamClient client = new SteamClient( AccessConstants.SimulatedServerUrl );
				var request = new SteamRequest( "/resource", HttpMethod.Post );
				request.DataFormat = PostDataFormat.Raw;

				string payload = "myContent 123 abc's fun!";
				request.AddBody( payload );

				request.AddParameter( "howdy", "doody" );
				request.AddParameter( "foo", "bar" );

				var response = client.Execute( request );

				Assert.IsNotNull( response );
				Assert.IsNotNull( response.Content );
				Assert.AreEqual( payload, response.Content );

			}

		}

		[TestMethod]
		[TestCategory( "POST Submission" )]
		public void POST_Can_Add_Body_NoParams_Json() {

			using( SimulatedServer.Create( AccessConstants.SimulatedServerUrl, Post_Body_Echo ) ) {

				SteamClient client = new SteamClient( AccessConstants.SimulatedServerUrl );
				var request = new SteamRequest( "/resource", HttpMethod.Post );

				// Verify request defaults to JSON
				Assert.AreEqual( PostDataFormat.Json, request.DataFormat );

				string payload = "myContent 123 abc's fun!";

				request.AddBody( payload );

				var response = client.Execute( request );

				Assert.IsNotNull( response );
				Assert.IsNotNull( response.Content );
				Assert.AreEqual( JsonConvert.SerializeObject( payload ), response.Content );

			}

		}

		[TestMethod]
		[TestCategory( "POST Submission" )]
		public void POST_Can_Add_Body_WithParams_Json() {

			using( SimulatedServer.Create( AccessConstants.SimulatedServerUrl, Post_Body_Echo ) ) {

				SteamClient client = new SteamClient( AccessConstants.SimulatedServerUrl );
				var request = new SteamRequest( "/resource", HttpMethod.Post );
				request.DataFormat = PostDataFormat.Json;

				request.AddBody( "myContent 123 abc's fun!" );

				request.AddParameter( "howdy", "doody" );
				request.AddParameter( "foo", "bar" );

				var response = client.Execute( request );

				Assert.IsNotNull( response );
				Assert.IsNotNull( response.Content );

				JObject obj = JObject.Parse( response.Content );

				Assert.AreEqual( "doody", obj["howdy"] );
				Assert.AreEqual( "bar", obj["foo"] );
				Assert.AreEqual( "myContent 123 abc's fun!", obj["application/json"] );

			}

		}

		[TestMethod]
		[TestCategory( "POST Submission" )]
		public void POST_Can_Add_DataStructure_Json() {

			using( SimulatedServer.Create( AccessConstants.SimulatedServerUrl, Post_Body_Echo ) ) {

				SteamClient client = new SteamClient( AccessConstants.SimulatedServerUrl );
				var request = new SteamRequest( "/resource", HttpMethod.Post );
				request.DataFormat = PostDataFormat.Json;

				object payload = new {
					appids_filter = new int[] { 10, 20, 30, 40, 50 }
				};
				
				request.AddBody( payload );

				var response = client.Execute( request );

				Assert.IsNotNull( response );
				Assert.IsNotNull( response.Content );

				DataStructureResponseTest obj = JsonConvert.DeserializeObject<DataStructureResponseTest>( response.Content );

				Assert.AreEqual( 5, obj.appids_filter.Length );

			}

		}

		private class DataStructureResponseTest {
			public int[] appids_filter { get; set; }
		}

		private void Timeout_Simulator( HttpListenerContext context ) {
			System.Threading.Thread.Sleep( 5000 );
		}

		private void Post_Body_Echo( HttpListenerContext context ) {
			var request = context.Request;
			string fromRequest = request.InputStream.StreamToString();
			context.Response.OutputStream.WriteStringUTF8( fromRequest );
		}

	}

}
