using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {
	
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
		private ResponseStatus _responseStatus = ResponseStatus.None;
		public ResponseStatus ResponseStatus {
			get { return _responseStatus; }
			set { _responseStatus = value; }
		}

		/// <summary>
		/// HTTP response status code
		/// </summary>
		public HttpStatusCode StatusCode { get; set; }

	}

}
