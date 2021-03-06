﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace SteamSharp {

	/// <summary>
	/// (Interface) Container for data received from Steam API requests.
	/// </summary>
	public interface ISteamResponse {

		/// <summary>
		/// Reference to the initial request which produced this response.
		/// </summary>
		ISteamRequest Request { get; set; }

		/// <summary>
		/// Contains the raw <see cref="HttpResponseMessage" /> object returned by the web request.
		/// </summary>
		HttpResponseMessage HttpResponse { get; set; }

		/// <summary>
		/// Cookie container for all cookies received back from the transaction call.
		/// </summary>
		Dictionary<string, Cookie> Cookies { get; set; }

		/// <summary>
		/// If an error was encountered, this records the exception text. Null otherwise.
		/// </summary>
		string ErrorMessage { get; set; }

		/// <summary>
		/// If an error was encountered, this records the relevant exception. Null otherwise.
		/// </summary>
		Exception ErrorException { get; set; }

		/// <summary>
		/// Contains the state of the response. See <see cref="ResponseStatus" /> for possible values.
		/// </summary>
		ResponseStatus ResponseStatus { get; set; }

		/// <summary>
		/// HTTP response status code
		/// </summary>
		HttpStatusCode StatusCode { get; set; }

		/// <summary>
		/// HTTP response status description
		/// </summary>
		string StatusDescription { get; set; }

		/// <summary>
		/// Indicates whether or not the response was a success.
		/// </summary>
		bool IsSuccessful { get; set;  }

		/// <summary>
		/// Response content
		/// </summary>
		string Content { get; set; }

	}

}
