using System;

namespace SteamSharp {
	
	public class SteamAuthenticationException : Exception {

		public SteamAuthenticationException( string message )
			: base( message )
		{
		}

		public SteamAuthenticationException( string message, Exception exception )
			: base( message, exception ) {
		}

	}

}
