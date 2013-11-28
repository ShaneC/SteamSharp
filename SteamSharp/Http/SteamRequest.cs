using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

	public class SteamRequest : ISteamRequest {

		/// <summary>
		/// Container of all parameters needing to be passed to the Steam API.
		/// See AddParameter() for adding additional parameters to the request.
		/// </summary>
		public List<SteamRequestParameter> Parameters { get; private set; }

		/// <summary>
		/// Indicates the standard HTTP method that should be used for this request.
		/// This value defaults to GET.
		/// </summary>
		private HttpMethod _method;
		public HttpMethod Method {
			get { return _method; }
			set { _method = value; }
		}

		/// <summary>
		/// The URI the request will be made against.
		/// </summary>
		public string Resource { get; set; }

		/// <summary>
		/// Timeout in milliseconds to be used for the request. If this time is exceeded the request will fail.
		/// If not set, defaults to Timeout value in the SteamClient executing this request.
		/// </summary>
		public int Timeout { get; set; }

		/// <summary>
		/// Initializes SteamRequest with the targeted resource URL and the HTTP Method for the request.
		/// </summary>
		/// <param name="resource">Resource to use for this request</param>
		/// <param name="method">HTTP Method to use for this request</param>
		public SteamRequest( string resource, HttpMethod method ) {
			Parameters = new List<SteamRequestParameter>(); 
			Resource = resource;
			Method = method;
		}

		/// <summary>
		/// Initializes SteamRequest with the targeted resource URL.
		/// </summary>
		/// <param name="resource">Resource to use for this request</param>
		public SteamRequest( string resource )
			: this( resource, HttpMethod.GET )
		{
		}

		/// <summary>
		/// Initializes SteamRequest with the targeted resource URL.
		/// </summary>
		/// <param name="resource">Resource to use for this request</param>
		public SteamRequest( Uri resource )
			: this( resource, HttpMethod.GET )
		{
		}

		/// <summary>
		/// Initializes SteamRequest with the targeted resource URL and the HTTP Method for the request.
		/// </summary>
		/// <param name="resource">Resource to use for this request</param>
		/// <param name="method">HTTP Method to use for this request</param>
		public SteamRequest( Uri resource, HttpMethod method )
			: this( ( ( resource.IsAbsoluteUri ) ? ( resource.AbsolutePath + resource.Query ) : resource.OriginalString ), method ) {
		}

		/// <summary>
		/// Serializes object obj into JSON, which is then used as the Body of the HTTP request.
		/// </summary>
		/// <param name="obj">Object to be serialized and used as the Body of the HTTP request.</param>
		/// <returns>This request</returns>
		public ISteamRequest AddBody( object obj ) {
			return AddParameter( "application/json", JsonConvert.SerializeObject( obj ), ParameterType.RequestBody );
		}

		/// <summary>
		/// Registers a URL Segement with the request. This will replace {name} with value in the specified Resource.
		/// </summary>
		/// <param name="name">Name of the segement to register.</param>
		/// <param name="value">Value to replace the named segement with.</param>
		/// <returns>This request.</returns>
		public ISteamRequest AddUrlSegement( string name, string value ) {
			return AddParameter( name, value, ParameterType.UrlSegment );
		}

		/// <summary>
		/// Adds an custom HTTP Header to the request.
		/// </summary>
		/// <param name="name">The name of the header (i.e. X-CustomHeader)</param>
		/// <param name="value">The value of the custom header</param>
		/// <returns>This request</returns>
		public ISteamRequest AddHeader( string name, string value ) {
			return AddParameter( name, value, ParameterType.HttpHeader );
		}

		/// <summary>
		/// Adds a parameter to the request.
		/// </summary>
		/// <param name="name">Name of the parameter</param>
		/// <param name="value">Value of the parameter</param>
		/// <returns>This request</returns>
		public ISteamRequest AddParameter( string name, object value ){
			return AddParameter( name, value, ParameterType.GetOrPost );
		}

		/// <summary>
		/// Adds a parameter to the request. 
		/// </summary>
		/// <param name="name">Name of the parameter</param>
		/// <param name="value">Value of the parameter</param>
		/// <param name="type">The type of the parameter</param>
		/// <returns>This request</returns>
		public ISteamRequest AddParameter( string name, object value, ParameterType type ) {
			Parameters.Add( new SteamRequestParameter { Name = name, Value = value, Type = type } );
			return this;
		}

		/// <summary>
		/// Provides the number of times this particular request has been attempted (regardless of success).
		/// </summary>
		private int _attempts;
		public int Attempts {
			get { return _attempts; }
		}

		/// <summary>
		/// Method that allows the request's attempt count to be incremented.
		/// Should only be called by the SteamClient doing an execution operation.
		/// </summary>
		public void IncreaseNumAttempts(){
			_attempts++;
		}

	}

}
