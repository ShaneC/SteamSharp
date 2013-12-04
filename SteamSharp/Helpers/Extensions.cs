using System;
using System.IO;

namespace SteamSharp.Helpers {

	/// <summary>
	/// Class containing extension methods for SteamSharp.
	/// </summary>
	public static class Extensions {

		/// <summary>
		/// Converts a valid hexadecimal string into is byte array equivalent.
		/// </summary>
		/// <param name="hex">Valid hexadecimal string of even length.</param>
		/// <returns>Byte array equivalent of entered hex string.</returns>
		public static byte[] HexToByteArray( this string hex ) {

			if( hex.Length % 2 == 1 )
				throw new FormatException( "String provided is not a valid hex string (the length is non-even)." );

			int numChars = hex.Length / 2;

			byte[] bytes = new byte[numChars];

			using( var reader = new StringReader( hex ) ) {
				for( int i = 0; i < numChars; i++ )
					bytes[i] = Convert.ToByte( new string( new char[2] { (char)reader.Read(), (char)reader.Read() } ), 16 );
			}

			return bytes;

		}

	}

}
