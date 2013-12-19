using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteamSharp.Test.TestHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace SteamSharp.Test {

	[TestClass]
	public class UriBuilderTests {

		public byte[] testByteArr = new byte[] {
			0x30, 0x81, 0x9D, 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 
			0x05, 0x00, 0x03, 0x81, 0x8B, 0x00, 0x30, 0x81, 0x87, 0x02, 0x81, 0x81, 0x00, 0xDF, 0xEC, 0x1A, 
			0xD6, 0x2C, 0x10, 0x66, 0x2C, 0x17, 0x35, 0x3A, 0x14, 0xB0, 0x7C, 0x59, 0x11, 0x7F, 0x9D, 0xD3, 
			0xD8, 0x2B, 0x7A, 0xE3, 0xE0, 0x15, 0xCD, 0x19, 0x1E, 0x46, 0xE8, 0x7B, 0x87, 0x74, 0xA2, 0x18, 
			0x46, 0x31, 0xA9, 0x03, 0x14, 0x79, 0x82, 0x8E, 0xE9, 0x45, 0xA2, 0x49, 0x12, 0xA9, 0x23, 0x68, 
			0x73, 0x89, 0xCF, 0x69, 0xA1, 0xB1, 0x61, 0x46, 0xBD, 0xC1, 0xBE, 0xBF, 0xD6, 0x01, 0x1B, 0xD8, 
			0x81, 0xD4, 0xDC, 0x90, 0xFB, 0xFE, 0x4F, 0x52, 0x73, 0x66, 0xCB, 0x95, 0x70, 0xD7, 0xC5, 0x8E, 
			0xBA, 0x1C, 0x7A, 0x33, 0x75, 0xA1, 0x62, 0x34, 0x46, 0xBB, 0x60, 0xB7, 0x80, 0x68, 0xFA, 0x13, 
			0xA7, 0x7A, 0x8A, 0x37, 0x4B, 0x9E, 0xC6, 0xF4, 0x5D, 0x5F, 0x3A, 0x99, 0xF9, 0x9E, 0xC4, 0x3A, 
			0xE9, 0x63, 0xA2, 0xBB, 0x88, 0x19, 0x28, 0xE0, 0xE7, 0x14, 0xC0, 0x42, 0x89, 0x02, 0x01, 0x11, 
		};

		[TestMethod]
		[TestCategory( "URI - Resource" )]
		public void Testtesttest() {

			var test = HttpUtility.UrlEncode( testByteArr );
			var test2 = SteamSharp.Helpers.StringFormat.UrlEncode( testByteArr );

			Assert.AreEqual( test, test2 );

			return;

		}

		[TestMethod]
		[TestCategory( "URI - Resource" )]
		public void GET_As_Default() {

			var request = new SteamRequest( "/resource" );
			Assert.AreEqual( request.Method, HttpMethod.Get );

		}

		[TestMethod]
		[TestCategory( "URI - Resource" )]
		public void GET_With_Leading_Slash() {

			var request = new SteamRequest( "/resource" );
			var client = new SteamClient( "http://steamapiurl.com/" );

			var expected = new Uri( "http://steamapiurl.com/resource" );
			var output = client.BuildUri( request );

			Assert.AreEqual( expected, output );

		}

		[TestMethod]
		[TestCategory( "URI - Resource" )]
		public void GET_Without_Leading_Slash() {

			var request = new SteamRequest( "resource" );
			var client = new SteamClient( "http://steamapiurl.com/" );

			var expected = new Uri( "http://steamapiurl.com/resource" );
			var output = client.BuildUri( request );

			Assert.AreEqual( expected, output );

		}

		[TestMethod]
		[TestCategory( "URI - Resource" )]
		public void GET_With_NoSlash_Base_And_Resource_Leading_Slash() {

			var request = new SteamRequest( "/resource" );
			var client = new SteamClient( "http://iforgottoslash.com" );

			var expected = new Uri( "http://iforgottoslash.com/resource" );
			var output = client.BuildUri( request );

			Assert.AreEqual( expected, output );

		}

		[TestMethod]
		[TestCategory( "URI - Resource" )]
		public void GET_With_NoSlash_Base_And_No_Leading_Slash() {

			var request = new SteamRequest( "resource" );
			var client = new SteamClient( "http://iforgottoslash.com" );

			var expected = new Uri( "http://iforgottoslash.com/resource" );
			var output = client.BuildUri( request );

			Assert.AreEqual( expected, output );

		}

		[TestMethod]
		[TestCategory( "URI - Resource" )]
		public void Detect_Malformed_BaseApi() {

			var request = new SteamRequest( "resource" );

			var client = new SteamClient(  "Definitely isn't a URI... How sad :(" );

			AssertException.Throws<FormatException>( () => { client.BuildUri( request ); } );

		}

		/// <summary>
		/// Scenario: User adds two parameters with the same name and type
		/// Expected: The most recently added parameter is honored
		/// </summary>
		[TestMethod]
		[TestCategory( "URI - Parameters" )]
		public void Add_Two_Of_Same_Parameter() {

			var request = new SteamRequest( "/resource" );
			request.AddParameter( "MyFancyParam", 1234, ParameterType.GetOrPost );
			request.AddParameter( "MyFancyParam", 5678, ParameterType.GetOrPost );

			Assert.AreEqual( request.Parameters.Count( p => p.Name == "MyFancyParam" ), 1 );
			Assert.AreEqual( request.Parameters.FirstOrDefault( p => p.Name == "MyFancyParam" ).Value, 5678 );

		}

		[TestMethod]
		[TestCategory( "URI - Parameters" )]
		public void GET_With_Resource_Containing_Tokens() {

			var request = new SteamRequest( "resource/{foo}" );
			request.AddUrlSegment( "foo", "bar" );

			var client = new SteamClient( "http://steamapiurl.com/" );

			var expected = new Uri( "http://steamapiurl.com/resource/bar" );
			var output = client.BuildUri( request );

			Assert.AreEqual( expected, output );

		}

		[TestMethod]
		[TestCategory( "URI - Parameters" )]
		public void POST_With_Resource_Containing_Tokens() {
			
			var request = new SteamRequest( "resource/{foo}", HttpMethod.Post );
			request.AddUrlSegment( "foo", "bar" );

			var client = new SteamClient( "http://steamapiurl.com/" );

			var expected = new Uri( "http://steamapiurl.com/resource/bar" );
			var output = client.BuildUri( request );

			Assert.AreEqual( expected, output );

		}

		[TestMethod]
		[TestCategory( "URI - Parameters" )]
		public void GET_Add_QueryString_Params() {

			using( SimulatedServer.Create( ResourceConstants.SimulatedServerUrl, QueryString_Echo ) ) {

				// All Params added, GetOrPost & QueryString, should be present in the result set
				SteamClient client = new SteamClient( ResourceConstants.SimulatedServerUrl );
				var request = new SteamRequest( "/resource" );

				request.AddParameter( "param1", "1234", ParameterType.GetOrPost );
				request.AddParameter( "param2", "5678", ParameterType.QueryString );

				var response = client.Execute( request );

				Assert.IsNotNull( response.Content );

				string[] temp = response.Content.Split( '|' );

				NameValueCollection coll = new NameValueCollection();
				foreach( string s in temp ) {
					var split = s.Split( '=' );
					coll.Add( split[0], split[1] );
				}

				Assert.IsNotNull( coll["param1"] );
				Assert.AreEqual( "1234", coll["param1"] );

				Assert.IsNotNull( coll["param2"] );
				Assert.AreEqual( "5678", coll["param2"] );

			}

		}

		[TestMethod]
		[TestCategory( "URI - Parameters" )]
		public void POST_Add_QueryString_Params_Raw() {

			using( SimulatedServer.Create( ResourceConstants.SimulatedServerUrl, QueryString_Echo ) ) {

				// All Params added, GetOrPost & QueryString, should be present in the result set
				SteamClient client = new SteamClient( ResourceConstants.SimulatedServerUrl );
				var request = new SteamRequest( "/resource", HttpMethod.Post );
				request.DataFormat = PostDataFormat.Raw;

				request.AddParameter( "param1", "1234", ParameterType.GetOrPost );
				request.AddParameter( "param2", "5678", ParameterType.QueryString );

				var response = client.Execute( request );

				Assert.IsNotNull( response.Content );

				string[] temp = response.Content.Split( '|' );

				NameValueCollection coll = new NameValueCollection();
				foreach( string s in temp ) {
					var split = s.Split( '=' );
					coll.Add( split[0], split[1] );
				}

				Assert.IsNotNull( coll["param1"] );
				Assert.AreEqual( "1234", coll["param1"] );

				Assert.IsNotNull( coll["param2"] );
				Assert.AreEqual( "5678", coll["param2"] );

			}

		}

		[TestMethod]
		[TestCategory( "URI - Parameters" )]
		public void POST_Add_QueryString_Params_Json() {

			using( SimulatedServer.Create( ResourceConstants.SimulatedServerUrl, QueryString_Echo ) ) {

				// Query String params should be in the URI, GetOrPost should not
				SteamClient client = new SteamClient( ResourceConstants.SimulatedServerUrl );
				var request = new SteamRequest( "/resource", HttpMethod.Post );
				request.DataFormat = PostDataFormat.Json;

				request.AddParameter( "param1", "1234", ParameterType.GetOrPost );
				request.AddParameter( "param2", "5678", ParameterType.QueryString );

				var response = client.Execute( request );

				Assert.IsNotNull( response.Content );

				string[] temp = response.Content.Split( '|' );

				NameValueCollection coll = new NameValueCollection();
				foreach( string s in temp ) {
					var split = s.Split( '=' );
					coll.Add( split[0], split[1] );
				}

				Assert.IsNull( coll["param1"] );

				Assert.IsNotNull( coll["param2"] );
				Assert.AreEqual( "5678", coll["param2"] );

			}

		}

		private void QueryString_Echo( HttpListenerContext context ) {

			var data = context.Request;

			Uri requestUri = context.Request.Url;
			var queryParams = HttpUtility.ParseQueryString( requestUri.Query );

			List<string> temp = new List<string>();
			foreach( string key in queryParams.AllKeys ) {
				temp.Add( String.Format( "{0}={1}", key, queryParams[key] ) );
			}

			context.Response.OutputStream.WriteStringUTF8( String.Join( "|", temp ) );
			
		}

	}

}
