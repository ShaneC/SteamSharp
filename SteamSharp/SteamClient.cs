using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace SteamSharp {

    public partial class SteamClient {

		/// <summary>
		/// Steam API endpoint. This could be subject to change and thus needs to be overrideable.
		/// </summary>
		private static string _apiEndpoint = "https://api.steampowered.com/";

		/// <summary>
		/// Public accessor for the _apiEndpoint variable. Sets the Steam API endpoint.
		/// </summary>
		public static string BaseAPIEndpoint {
			get { return _apiEndpoint; }
			set { _apiEndpoint = value; }
		}

		/// <summary>
		/// Timeout, in milliseconds, for requests executed by this client instance.
		/// </summary>
		private int _timeout = 10000;
		public int Timeout {
			get { return _timeout; }
			set { _timeout = value; }
		}

		/// <summary>
		/// Authenticator to use for requests made by this client instance.
		/// </summary>
		public ISteamAuthenticator Authenticator { get; set; }

		/// <summary>
		/// Authenticates the client with the Steam API.
		/// </summary>
		/// <param name="client">SteamClient instance to be authenticated.</param>
		/// <param name="request">Request requiring authentication.</param>
		private void AuthenticateClient( SteamClient client, ISteamRequest request ) {
			if( Authenticator != null ) {
				Authenticator.Authenticate( client, request );
			}
		}

		public Uri BuildUri( ISteamRequest request ) {

			string destination = request.Resource;
			
			// URL Segement replacement is only valid if the Resource URI is non-empty
			if( !String.IsNullOrEmpty( destination ) ) {

				// To prefix or not to prefix - that is the question! (The answer is don't. Obviously.)
				if( destination.StartsWith( "/" ) )
					destination = destination.Substring( 1 );

				var urlParams = request.Parameters.Where( p => p.Type == ParameterType.UrlSegment );
				foreach( SteamRequestParameter p in urlParams )
					destination = destination.Replace( "{" + p.Name + "}", p.Value.ToString() );

				destination = BaseAPIEndpoint + "/" + destination;

			} else
				destination = BaseAPIEndpoint;


			IEnumerable<SteamRequestParameter> parameters = null;
			if( request.Method == HttpMethod.POST || request.Method == HttpMethod.PUT || request.Method == HttpMethod.PATCH ) {
				// This conforms to a POST-style request
				parameters = request.Parameters.Where( p => p.Type == ParameterType.QueryString );
			} else {
				// This conforms to a GET-style request
				parameters = request.Parameters.Where( p => p.Type == ParameterType.GetOrPost || p.Type == ParameterType.QueryString );
			}

			var queryString = new StringBuilder();
			if( parameters != null && parameters.Any() ) {
				foreach( SteamRequestParameter p in parameters ) {
					if( queryString.Length > 1 )
						queryString.Append( "&" );
					queryString.AppendFormat( "{0}={1}", p.Name, p.Value.ToString() );
				}

				destination = destination + "?" + queryString;
			}

			return new Uri( Uri.EscapeUriString( destination ) );

		}

		private SteamResponse ConvertToResponse( ISteamRequest request, HttpResponseMessage response ) {


			SteamResponse steamResponse = new SteamResponse();

			return steamResponse;

		}

		/// <summary>
		/// Object containing the HTTP Request used to query the Steam API.
		/// </summary>
		public HttpWebRequest HttpRequest = null;

		/// <summary>
		/// Pulls the current version of SteamSharp. This is the recommended way in .NET 4.5+.
		/// Note: Assembly.GetExecutingAssembly() does not work in Windows Store (and is expensive in its execution).
		/// </summary>
		//private static readonly Version _version = new AssemblyName( typeof( SteamClient ).GetTypeInfo().Assembly.FullName ).Version;
		private static readonly Version _version = new AssemblyName( typeof( SteamClient ).GetTypeInfo().Assembly.FullName ).Version;

		public Version AssemblyVersion {
			get { return _version; }
		}

	}

}
