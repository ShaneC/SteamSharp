using System;

namespace SteamSharp {
	
	/// <summary>
	/// Exception thrown when Steam encounters an error while attempting to authenticate a request.
	/// </summary>
	public class SteamAuthenticationException : Exception {

		/// <summary>
		/// Exception thrown when Steam encounters an error while attempting to authenticate a request.
		/// </summary>
		/// <param name="message">Exception message</param>
		public SteamAuthenticationException( string message )
			: base( message )
		{
		}

		/// <summary>
		/// Exception thrown when Steam encounters an error while attempting to authenticate a request.
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="exception">Exception which caused the error.</param>
		public SteamAuthenticationException( string message, Exception exception )
			: base( message, exception ) {
		}

	}

}
