using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

	public interface ISteamRequest {

		/// <summary>
		/// Container of all parameters needing to be passed to the Steam API.
		/// See AddParameter() for adding additional parameters to the request.
		/// </summary>
		List<SteamRequestParameter> Parameters { get; }

		/// <summary>
		/// Indicates the standard HTTP method that should be used for this request.
		/// This value defaults to GET.
		/// </summary>
		HttpMethod Method { get; set; }

		/// <summary>
		/// The URI the request will be made against.
		/// </summary>
		string Resource { get; set; }

		/// <summary>
		/// Timeout in milliseconds to be used for the request. If this time is exceeded the request will fail.
		/// If not set, defaults to Timeout value in the SteamClient executing this request.
		/// </summary>
		int Timeout { get; set; }

		/// <summary>
		/// Serializes object obj into JSON, which is then used as the Body of the HTTP request.
		/// </summary>
		/// <param name="obj">Object to be serialized and used as the Body of the HTTP request.</param>
		/// <returns>This request</returns>
		ISteamRequest AddBody( object obj );

		/// <summary>
		/// Registers a URL Segement with the request. This will replace {name} with value in the specified Resource.
		/// </summary>
		/// <param name="name">Name of the segement to register.</param>
		/// <param name="value">Value to replace the named segement with.</param>
		/// <returns>This request.</returns>
		ISteamRequest AddUrlSegement( string name, string value );

		/// <summary>
		/// Adds an custom HTTP Header to the request.
		/// </summary>
		/// <param name="name">The name of the header (i.e. X-CustomHeader)</param>
		/// <param name="value">The value of the custom header</param>
		/// <returns>This request</returns>
		ISteamRequest AddHeader( string name, string value );

		/// <summary>
		/// Adds a parameter to the request.
		/// </summary>
		/// <param name="name">Name of the parameter</param>
		/// <param name="value">Value of the parameter</param>
		/// <returns>This request</returns>
		ISteamRequest AddParameter( string name, object value );

		/// <summary>
		/// Adds a parameter to the request. 
		/// </summary>
		/// <param name="name">Name of the parameter</param>
		/// <param name="value">Value of the parameter</param>
		/// <param name="type">The type of the parameter</param>
		/// <returns>This request</returns>
		ISteamRequest AddParameter( string name, object value, ParameterType type );

		/// <summary>
		/// Provides the number of times this particular request has been attempted (regardless of success).
		/// </summary>
		int Attempts { get; }

		/// <summary>
		/// Method that allows the request's attempt count to be incremented.
		/// Should only be called by the SteamClient doing an execution operation.
		/// </summary>
		void IncreaseNumAttempts();

	}

}
