using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace SteamSharp {

	/// <summary>
	/// (Interface) Container for data received from Steam API requests.
	/// </summary>
	public class SteamResponse : ISteamResponse {

		/// <summary>
		/// Reference to the initial request which produced this response.
		/// </summary>
		public ISteamRequest Request { get; set; }

		/// <summary>
		/// Contains the raw <see cref="HttpResponseMessage" /> object returned by the web request.
		/// </summary>
		public HttpResponseMessage HttpResponse { get; set; }

		/// <summary>
		/// Cookie container for all cookies received back from the transaction call.
		/// </summary>
		public IEnumerable<Cookie> Cookies { get; set; }

		/// <summary>
		/// If an error was encountered, this records the exception text. Null otherwise.
		/// </summary>
		public string ErrorMessage { get; set; }

		/// <summary>
		/// If an error was encountered, this records the relevant exception. Null otherwise.
		/// </summary>
		public Exception ErrorException { get; set; }

		/// <summary>
		/// Contains the state of the response. See <see cref="ResponseStatus" /> for possible values.
		/// </summary>
		public ResponseStatus ResponseStatus {
			get { return _responseStatus; }
			set { _responseStatus = value; }
		}

		private ResponseStatus _responseStatus = ResponseStatus.None;

		/// <summary>
		/// HTTP response status code
		/// </summary>
		public HttpStatusCode StatusCode { get; set; }

		/// <summary>
		/// HTTP response status description
		/// </summary>
		public string StatusDescription { get; set; }

		/// <summary>
		/// Indicates whether or not the response was a success.
		/// </summary>
		public bool IsSuccessful { get; set; }

		/// <summary>
		/// Response content
		/// </summary>
		public string Content { get; set; }

	}

}
