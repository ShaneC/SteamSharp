﻿using Newtonsoft.Json;
using SteamSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

	/// <summary>
	/// Main transport class for all requests and responses to and from the Steam API.
	/// This class is used to execute all requests, as well as provide authentication and transaction control.
	/// </summary>
	public partial class SteamClient {

		/// <summary>
		/// Steam API endpoint. This could be subject to change and thus needs to be overrideable.
		/// </summary>
		public string BaseAPIEndpoint {
			get { return _apiEndpoint; }
			private set { _apiEndpoint = value; }
		}
		private string _apiEndpoint = "https://api.steampowered.com";

		/// <summary>
		/// Timeout, in milliseconds, for requests executed by this client instance. Defaults to 10 seconds.
		/// </summary>
		public int Timeout {
			get { return _timeout; }
			set { _timeout = value; }
		}
		private int _timeout = 10000;

		/// <summary>
		/// Default User-Agent for use in the client. If not set, it will take the form of "SteamSharp/{SteamSharp_Version}".
		/// </summary>
		public string DefaultUserAgent { get; set; }

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

		/// <summary>
		/// Validates that the required ISteamAuthenticator object is attached to the client prior to initiating execution
		/// </summary>
		/// <param name="validAuthMethods">List of valid authenticators for API access.</param>
		/// <returns>Returns 'true' if the proper authenticator is attached. Throws <see cref="SteamAuthenticationException"/> otherwise.</returns>
		internal bool IsAuthorizedCall( Type[] validAuthMethods ) {
			if( Authenticator == null || !validAuthMethods.Contains( Authenticator.GetType() ) )
				throw new SteamAuthenticationException( "API call has not been properly authenticated. Please ensure the proper ISteamAuthenticator object is added to the SteamClient (SteamClient.Authenticator)." );
			return true;
		}

		/// <summary>
		/// Validates that the required ISteamAuthenticator object is attached to the client prior to initiating execution
		/// </summary>
		/// <param name="validAuthMethod">Required authenticator.</param>
		/// <returns>Returns 'true' if the proper authenticator is attached. Throws <see cref="SteamAuthenticationException"/> otherwise.</returns>
		internal bool IsAuthorizedCall( Type validAuthMethod ) {
			if( validAuthMethod != Authenticator.GetType() )
				throw new SteamAuthenticationException( "API call has not been properly authenticated. Please ensure a " + validAuthMethod.FullName + " object is added to the SteamClient (SteamClient.Authenticator)." );
			return true;
		}

		/// <summary>
		/// Creates a SteamClient which facilitates requests to and responses from the Steam API.
		/// All Steam API transactions are conducted through this object.
		/// </summary>
		public SteamClient() {

			DefaultParameters = new List<SteamRequestParameter> {
				new SteamRequestParameter { Name = "format", Value = "json", Type = ParameterType.QueryString }
			};

		}

		/// <summary>
		/// Creates a SteamClient with a Steam API endpoint at the specified value.
		/// Using this constructor is not recommended unless mitigating a known change in the Steam API endpoint or during execution of a test case.
		/// </summary>
		/// <param name="customApiEndpoint">Base URL for the Steam API Endpoint</param>
		public SteamClient( string customApiEndpoint )
			: this()
		{
			BaseAPIEndpoint = customApiEndpoint;
		}

		/// <summary>
		/// Helper method which creates the final Uri used in the HTTP request.
		/// </summary>
		/// <param name="request">Request for execution.</param>
		/// <returns>Well formed Uri for use in an <see cref="System.Net.Http.HttpRequestMessage"/>.</returns>
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
			if( request.DataFormat != PostDataFormat.Raw && ( request.Method == HttpMethod.Post || request.Method == HttpMethod.Put ) ) {
				// This conforms to a POST-style request and the output will be serialized (i.e. not Raw)
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
					string appendValue = ( p.IsUrlEncoded ) ? p.Value.ToString() : Uri.EscapeDataString( p.Value.ToString() );
					queryString.AppendFormat( "{0}={1}", Uri.EscapeDataString( p.Name ), appendValue );
				}

				destination = destination + "?" + queryString;
			}

			return new Uri( destination );

		}

		/// <summary>
		/// Constructs the <see cref="HttpWebRequest" /> object which will be used to execute the web request. 
		/// </summary>
		/// <param name="request">Request for execution.</param>
		protected HttpRequestMessage BuildHttpRequest( ISteamRequest request ) {

			// Add any Default client parameters (if it exists in the request, the request wins)
			foreach( var param in DefaultParameters ) {
				if( !request.Parameters.Any( p => p.Name == param.Name && p.Type == param.Type ) )
					request.AddParameter( param );
			}

			HttpRequestMessage httpRequest = new HttpRequestMessage( request.Method, BuildUri( request ) );

			// HEADERS
			// -- Add UserAgent header, if it does not already exist in both the request and the standard request message (shouldn't overwrite valuable system/platform data)
			if( !request.Parameters.Any( p => p.Name == "User-Agent" && p.Type == ParameterType.HttpHeader ) && !httpRequest.Headers.Any( h => h.Key == "User-Agent" ) )
				request.Parameters.Add( new SteamRequestParameter { Name = "User-Agent", Value = ( ( DefaultUserAgent == null ) ? "SteamSharp/" + AssemblyVersion : DefaultUserAgent ), Type = ParameterType.HttpHeader } );

			// -- Specify cookies which indicate this is an OAuth call
			//    Resolves Steam API change made in mid-2015
			request.Cookies.Add(new Cookie("forceMobile", "1"));
			request.Cookies.Add(new Cookie("mobileClient", "Steam App / SteamSharp / " + AssemblyVersion));

			// -- Currently we only accept and deserialize JSON responses
			request.Parameters.Add( new SteamRequestParameter { Name = "Accept", Value = "application/json", Type = ParameterType.HttpHeader } );

			IEnumerable<SteamRequestParameter> headers = request.Parameters.Where( p => p.Type == ParameterType.HttpHeader );
			foreach( var header in headers ) {
				if( httpRequest.Headers.Contains( header.Name ) )
					httpRequest.Headers.Remove( header.Name );
				httpRequest.Headers.Add( header.Name, header.Value.ToString() );
			}

			// BODY -- Only evaluate if post
			if( request.Method == HttpMethod.Post ) {
				var body = request.Parameters.FirstOrDefault( p => p.Type == ParameterType.RequestBody );

				HttpContent content;

				switch( request.DataFormat ) {
					case PostDataFormat.Json:
						content = new StringContent( SerializeBodyWithParameters( request, body, PostDataFormat.Json ) );
						content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse( "application/json" );
						break;
					case PostDataFormat.FormUrlEncoded:
						content = new StringContent( SerializeBodyWithParameters( request, body, PostDataFormat.FormUrlEncoded ) );
						content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse( "application/x-www-form-urlencoded" );
						break;
					default:
						content = new StringContent( ( ( body != null ) ? body.Value.ToString() : String.Empty ) );
						break;
				}

				httpRequest.Content = content;
			}

			return httpRequest;

		}

		protected SteamResponse CreateErrorResponse( ISteamRequest request, Exception ex ) {

			SteamResponse response = new SteamResponse();
			response.Request = request;
			response.IsSuccessful = false;
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

		protected SteamResponse ConvertToResponse( ISteamRequest request, HttpResponseMessage response, CookieContainer cookies ) {

			var cookieJar = new Dictionary<string,Cookie>();

			if( cookies != null ) {
				foreach( Cookie cookie in cookies.GetCookies( response.RequestMessage.RequestUri ).Cast<Cookie>() ) {
					cookieJar.Add( cookie.Name, cookie );
				}
			}

			SteamResponse steamResponse = new SteamResponse {
				HttpResponse = response,
				Request = request,
				RequestUri = response.RequestMessage.RequestUri,
				Cookies = cookieJar,
				ResponseStatus = SteamSharp.ResponseStatus.Completed,
				StatusCode = response.StatusCode,
				StatusDescription = response.StatusCode.ToString(),
				IsSuccessful = response.IsSuccessStatusCode
			};

			if( !steamResponse.IsSuccessful && response.ReasonPhrase != null )
				steamResponse.ErrorMessage = response.ReasonPhrase;

			StreamReader stream = new StreamReader( response.Content.ReadAsStreamAsync().Result, System.Text.Encoding.UTF8 );
			steamResponse.Content = stream.ReadToEnd();

			return steamResponse;

		}

		/// <summary>
		/// Utility method for POST requests. We must produce an object array such that it can be serialized (non-Raw POST style requests).
		/// </summary>
		/// <param name="request">Request to be evaluated.</param>
		/// <param name="body">Current parameter intended for addition to the body of the request.</param>
		/// <returns>Content which can be sent over the HTTP Request, including all parameters (both body and GetOrPost).</returns>
		private string SerializeBodyWithParameters( ISteamRequest request, SteamRequestParameter body, PostDataFormat format ) {
			
			Dictionary<string, object> output = new Dictionary<string, object>();
			
			IEnumerable<SteamRequestParameter> parameters = request.Parameters.Where( p => p.Type == ParameterType.GetOrPost );
			foreach( var p in parameters ) {
				output.Add( p.Name, p.Value );
			}

			if( format == PostDataFormat.Json ) {

				if( body != null && output.Count < 1 )
					return JsonConvert.SerializeObject( body.Value );
				else if( body != null ) {
					output.Add( body.Name, body.Value );
					return JsonConvert.SerializeObject( output );
				} else {
					return JsonConvert.SerializeObject( output );
				}

			} else {

				var payload = new StringBuilder();
				foreach( SteamRequestParameter p in parameters ) {
					if( payload.Length > 1 )
						payload.Append( "&" );
					string appendValue = ( p.IsUrlEncoded ) ? p.Value.ToString() : Uri.EscapeDataString( p.Value.ToString() );
					payload.AppendFormat( "{0}={1}", Uri.EscapeDataString( p.Name ), appendValue );
				}

				if( body != null && body.Value != null )
					payload.AppendFormat( "{0}={1}", body.Name, body.Value.ToString() );

				return payload.ToString();

			}

		}

		/// <summary>
		/// Pulls the current version of SteamSharp. This is the recommended way in .NET 4.5+.
		/// Note: Assembly.GetExecutingAssembly() does not work in Windows Store (and is expensive in its execution).
		/// </summary>
		//private static readonly Version _version = new AssemblyName( typeof( SteamClient ).GetTypeInfo().Assembly.FullName ).Version;
		private static readonly Version _version = new AssemblyName( typeof( SteamClient ).GetTypeInfo().Assembly.FullName ).Version;

		/// <summary>
		/// Version of the SteamSharp library currently being run.
		/// </summary>
		public Version AssemblyVersion {
			get { return _version; }
		}

	}

}
