using System;
using System.Collections;
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

		/// <summary>
		/// Converts a BitArray into its byte[] equivalent.
		/// </summary>
		/// <param name="bits">Target BitArray</param>
		/// <returns>byte[] representing the BitArray object.</returns>
		public static byte[] ToByteArray( this BitArray bits ) {

			int numBytes = bits.Length / 8;
			if( bits.Length % 8 != 0 ) numBytes++;

			byte[] bytes = new byte[numBytes];
			int byteIndex = 0, bitIndex = 0;

			for( int i = 0; i < bits.Length; i++ ) {
				if( bits[i] )
					bytes[byteIndex] |= (byte)( 1 << ( 7 - bitIndex ) );

				bitIndex++;
				if( bitIndex == 8 ) {
					bitIndex = 0;
					byteIndex++;
				}
			}

			return bytes;

		}

	}

}
