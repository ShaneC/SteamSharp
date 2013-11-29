using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SteamSharp.TestFramework {

	public class UriBuilderTests {

		[Fact]
		public void GET_As_Default() {

			var request = new SteamRequest( "/resource" );
			//Assert.Equal( request.Method, HttpMethod.Get );

		}

		[Fact]
		public void GET_With_Leading_Slash() {

			var request = new SteamRequest( "/resource" );
			var client = new SteamClient {
				BaseAPIEndpoint = "http://steamapiurl.com/"
			};

			var expected = new Uri( "http://steamapiurl.com/resource" );
			var output = client.BuildUri( request );

			Assert.Equal( expected, output );

		}

		[Fact]
		public void GET_Without_Leading_Slash() {

			var request = new SteamRequest( "resource" );
			var client = new SteamClient {
				BaseAPIEndpoint = "http://steamapiurl.com/"
			};

			var expected = new Uri( "http://steamapiurl.com/resource" );
			var output = client.BuildUri( request );

			Assert.Equal( expected, output );

		}

		[Fact]
		public void GET_With_NoSlash_Base_And_Resource_Leading_Slash() {

			var request = new SteamRequest( "/resource" );
			var client = new SteamClient {
				BaseAPIEndpoint = "http://iforgottoslash.com"
			};

			var expected = new Uri( "http://iforgottoslash.com/resource" );
			var output = client.BuildUri( request );

			Assert.Equal( expected, output );

		}

		[Fact]
		public void GET_With_NoSlash_Base_And_No_Leading_Slash() {

			var request = new SteamRequest( "resource" );
			var client = new SteamClient {
				BaseAPIEndpoint = "http://iforgottoslash.com"
			};

			var expected = new Uri( "http://iforgottoslash.com/resource" );
			var output = client.BuildUri( request );

			Assert.Equal( expected, output );

		}

		[Fact(Skip="This will throw an exception and thus a breaking point in debugger.")]
		public void Detect_Malformed_BaseApi() {

			var request = new SteamRequest( "resource" );

			var client = new SteamClient {
				BaseAPIEndpoint = "Definitely isn't a URI... How sad :("
			};

			Assert.Throws<FormatException>( () => { client.BuildUri( request ); } );

		}

		/// <summary>
		/// Scenario: User adds two parameters with the same name and type
		/// Expected: The most recently added parameter is honored
		/// </summary>
		[Fact]
		public void Add_Two_Of_Same_Parameter() {

			var request = new SteamRequest( "/resource" );
			request.AddParameter( "MyFancyParam", 1234, ParameterType.GetOrPost );
			request.AddParameter( "MyFancyParam", 5678, ParameterType.GetOrPost );

			Assert.Equal( request.Parameters.Count( p => p.Name == "MyFancyParam" ), 1 );
			Assert.Equal( request.Parameters.FirstOrDefault( p => p.Name == "MyFancyParam" ).Value, 5678 );

		}


		[Fact]
		public void GET_With_Resource_Containing_Tokens() {

			var request = new SteamRequest( "resource/{foo}" );
			request.AddUrlSegment( "foo", "bar" );

			var client = new SteamClient {
				BaseAPIEndpoint = "http://steamapiurl.com/"
			};

			var expected = new Uri( "http://steamapiurl.com/resource/bar" );
			var output = client.BuildUri( request );

			Assert.Equal( expected, output );

		}

		[Fact]
		public void POST_With_Resource_Containing_Tokens() {
			
			var request = new SteamRequest( "resource/{foo}", HttpMethod.Post );
			request.AddUrlSegment( "foo", "bar" );

			var client = new SteamClient {
				BaseAPIEndpoint = "http://steamapiurl.com/"
			};

			var expected = new Uri( "http://steamapiurl.com/resource/bar" );
			var output = client.BuildUri( request );

			Assert.Equal( expected, output );

		}

	}

}
