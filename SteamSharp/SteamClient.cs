using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

    public partial class SteamClient {

		/// <summary>
		/// Steam API endpoint. This could be subject to change and thus needs to be overrideable.
		/// </summary>
		private static string _apiEndpoint = "https://api.steampowered.com";
		public string BaseAPIEndpoint {
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
		/// Parameters that should be included with every request executed via this instance
		/// If specified in both the client and request, the request's parameter is used.
		/// </summary>
		public List<SteamRequestParameter> DefaultParameters { get; private set; }

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

		public SteamClient() {

			DefaultParameters = new List<SteamRequestParameter> {
				new SteamRequestParameter { Name = "format", Value = "json", Type = ParameterType.GetOrPost }
			};

		}

		/// <summary>
		/// Helper method which creates the final Uri used in the HTTP request.
		/// </summary>
		/// <param name="request">Request for execution.</param>
		/// <returns>Well formed Uri for use in an <see cref="HTTPWebRequest"/>.</returns>
		public Uri BuildUri( ISteamRequest request ) {

			string destination = request.Resource;

			// Add trailing slash if none exists
			if( !BaseAPIEndpoint.EndsWith( "/" ) )
				BaseAPIEndpoint = BaseAPIEndpoint + "/";

			if( !Uri.IsWellFormedUriString( BaseAPIEndpoint, UriKind.RelativeOrAbsolute ) )
				throw new FormatException( "BaseAPIEndpoint specified does not conform to a valid Uri format. BaseAPIEndpoint specified: " + BaseAPIEndpoint );

			// URL Segement replacement is only valid if the Resource URI is non-empty
			if( !String.IsNullOrEmpty( destination ) ) {

				// To prefix or not to prefix - that is the question! (The answer is don't. Obviously.)
				if( destination.StartsWith( "/" ) )
					destination = destination.Substring( 1 );

				var urlParams = request.Parameters.Where( p => p.Type == ParameterType.UrlSegment );
				foreach( SteamRequestParameter p in urlParams )
					destination = destination.Replace( "{" + p.Name + "}", p.Value.ToString() );

				destination = BaseAPIEndpoint + destination;

			} else
				destination = BaseAPIEndpoint;

			IEnumerable<SteamRequestParameter> parameters = null;
			if( request.Method == HttpMethod.Post || request.Method == HttpMethod.Put ) {
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

		/// <summary>
		/// Constructs the <see cref="HttpWebRequest" /> object which will be used to execute the web request. 
		/// </summary>
		/// <param name="request">Request for execution.</param>
		private HttpRequestMessage BuildHttpRequest( ISteamRequest request ) {

			// Add any Default client parameters (if it exists in the request, the request wins)
			foreach( var param in DefaultParameters ) {
				if( !request.Parameters.Any( p => p.Name == param.Name && p.Type == param.Type ) )
					request.AddParameter( param );
			}

			HttpRequestMessage httpRequest = new HttpRequestMessage( request.Method, BuildUri( request ) );

			// Add UserAgent header, if it does not already exist in both the request and the standard request message (shouldn't overwrite valuable system/platform data)
			if( !request.Parameters.Any( p => p.Name == "User-Agent" && p.Type == ParameterType.HttpHeader ) && !httpRequest.Headers.Any( h => h.Key == "User-Agent" ) )
				request.Parameters.Add( new SteamRequestParameter { Name = "User-Agent", Value = "SteamSharp/" + AssemblyVersion, Type = ParameterType.HttpHeader } );

			// Currently we only accept and deserialize JSON responses
			request.Parameters.Add( new SteamRequestParameter { Name = "Accept", Value = "application/json", Type = ParameterType.HttpHeader } );

			IEnumerable<SteamRequestParameter> headers = request.Parameters.Where( p => p.Type == ParameterType.HttpHeader );
			foreach( var header in headers ) {
				if( httpRequest.Headers.Contains( header.Name ) )
					httpRequest.Headers.Remove( header.Name );
				httpRequest.Headers.Add( header.Name, header.Value.ToString() );
			}

			var body = request.Parameters.FirstOrDefault( p => p.Type == ParameterType.RequestBody );
			if( body != null ) {
				HttpContent content = new StringContent( body.ToString() );
				content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse( "application/json" );
				httpRequest.Content = content;
			}

			return httpRequest;

		}

		private SteamResponse CreateErrorResponse( ISteamRequest request, Exception ex ) {

			SteamResponse response = new SteamResponse();
			response.Request = request;

			response.ErrorMessage = ex.Message;

			if( ex is WebException ) {

				var webException = ex as WebException;
				if( webException != null ) {

					if( webException.Status == WebExceptionStatus.RequestCanceled )
						response.ResponseStatus = ResponseStatus.Aborted;
					else
						response.ResponseStatus = ResponseStatus.Error;

					response.ErrorException = ex;

					return response;
				}

			} else if( ex is TaskCanceledException ) {

				response.ErrorMessage = "The request timed out.";
				response.ResponseStatus = ResponseStatus.TimedOut;
				return response;

			}

			response.ErrorException = ex;
			response.ResponseStatus = ResponseStatus.Error;

			return response;

		}

		private SteamResponse ConvertToResponse( ISteamRequest request, HttpResponseMessage response ) {

			SteamResponse steamResponse = new SteamResponse {
				HttpResponse = response,
				Request = request,
				ResponseStatus = SteamSharp.ResponseStatus.Completed,
				StatusCode = response.StatusCode,
				Content = response.Content.ReadAsStringAsync().Result
			};

			return steamResponse;

		}

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
