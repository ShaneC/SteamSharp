using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
