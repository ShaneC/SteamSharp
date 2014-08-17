using System;
using System.Net;

namespace SteamSharp {

	/// <summary>
	/// Exception thrown when Steam encounters an error while attempting to execute a request.
	/// </summary>
	public class SteamRequestException : Exception {

		/// <summary>
		/// Exception encountered causing this exception to be thrown. May be a deserialization exception or the exception from an HTTP request.
		/// </summary>
		public new Exception InnerException { get; set; }

		/// <summary>
		/// Indicates whether or not this exception was caused from the inability to deserialize the Steam API response.
		/// </summary>
		public bool IsDeserializationIssue {
			get { return _isDeserializationIssue; }
			set { _isDeserializationIssue = value; }
		}
		
		private bool _isDeserializationIssue = false;

		/// <summary>
		/// Indiciates whether or not this exception was caused from an invalid HTTP request.
		/// </summary>
		public bool IsRequestIssue {
			get { return _isRequestIssue; }
			set { _isRequestIssue = value; }
		}

		private bool _isRequestIssue = false;

		public bool IsAuthenticationIssue {
			get { return _isAuthenticationIssue; }
			set { _isAuthenticationIssue = value; }
		}

		private bool _isAuthenticationIssue = false;

		/// <summary>
		/// The feedback from a bad HTTP Request. If none, this value will be null.
		/// </summary>
		public string RequestErrorMessage { get; set; }

		/// <summary>
		/// Object containing the processed HTTP response, as well as the initial <see cref="ISteamRequest"/> object and raw HTTPResponseMessage.
		/// </summary>
		public ISteamResponse Response { get; private set; }

		/// <summary>
		/// HTTP Status Code of the transaction. If no HTTP request was made, this value will be null.
		/// </summary>
		public HttpStatusCode StatusCode { get; private set; }

		/// <summary>
		/// Exception thrown when Steam encounters an error while attempting to execute a request.
		/// </summary>
		/// <param name="message">Exception message</param>
		public SteamRequestException( string message )
			: base( message ) 
		{
		}

		/// <summary>
		/// Exception thrown when Steam encounters an error while attempting to execute a request.
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="exception">Exception which identifies the error.</param>
		public SteamRequestException( string message, Exception exception )
			: base( message, exception )
		{
		}

		/// <summary>
		/// Exception thrown when Steam encounters an error while attempting to execute a request.
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="response">Response received from Steam which is either in an error state or delivered data which is in error.</param>
		public SteamRequestException( string message, ISteamResponse response )
			: base( message )
		{
			ProcessResponse( response );
		}

		private void ProcessResponse( ISteamResponse response ) {

			Response = response;
			StatusCode = response.StatusCode;

			this.Data.Add( "HttpErrorMessage", Response.ErrorMessage );
			this.Data.Add( "HttpStatusDescription", Response.StatusDescription );
			this.Data.Add( "HttpStatusCode", StatusCode );

			if( !response.IsSuccessful ) {

				RequestErrorMessage = response.ErrorMessage;
				if( response.ErrorException != null )
					InnerException = response.ErrorException;

			}
			
		}

	}

}
