using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

	public class SteamRequestException : Exception {

		/// <summary>
		/// Exception encountered causing this exception to be thrown. May be a deserialization exception or the exception from an HTTP request.
		/// </summary>
		public Exception InnerException { get; set; }

		/// <summary>
		/// Indicates whether or not this exception was caused from the inability to deserialize the Steam API response.
		/// </summary>
		private bool _isDeserializationIssue = false;
		public bool IsDeserializationIssue {
			get { return _isDeserializationIssue; }
			set { _isDeserializationIssue = value; }
		}

		/// <summary>
		/// Indiciates whether or not this exception was caused from an invalid HTTP request.
		/// </summary>
		private bool _isRequestIssue = false;
		public bool IsRequestIssue {
			get { return _isRequestIssue; }
			set { _isRequestIssue = value; }
		}

		/// <summary>
		/// The feedback from a bad HTTP Request. If none, this value will be null.
		/// </summary>
		public string RequestErrorMessage { get; set; }

		/// <summary>
		/// Object containing the processed HTTP response, as well as the initial <see cref="ISteamRequest"/> object and raw <see cref="HTTPResponseMessage"/>.
		/// </summary>
		public ISteamResponse Response { get; set; }

		public SteamRequestException( string message )
			: base( message ) 
		{
		}

		public SteamRequestException( string message, Exception exception )
			: base( message, exception )
		{
		}

		public SteamRequestException( string message, ISteamResponse response )
			: base( message )
		{
			ProcessResponse( response );
		}

		private void ProcessResponse( ISteamResponse response ) {

			Response = response;

			if( !response.IsSuccessful ) {

				RequestErrorMessage = response.ErrorMessage;
				if( response.ErrorException != null )
					InnerException = response.ErrorException;


			}
			
		}

	}

}
