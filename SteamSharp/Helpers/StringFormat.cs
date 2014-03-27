using System;
using System.Text;

namespace SteamSharp.Helpers {

	/// <summary>
	/// Helper methods for use in formatting strings.
	/// </summary>
	public class StringFormat {

		/// <summary>
		/// Encodes a URL string. These method overloads can be used to encode the entire URL, including query-string values.
		/// </summary>
		/// <remarks>
		/// (c) Microsoft Corporation 2013. Code ported from .NET 4.5 4.5.50709.0.
		/// </remarks>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static string UrlEncode( string str ) {
			if( str == null )
				return null;
			byte[] bytes = Encoding.UTF8.GetBytes( str );
			byte[] output = UrlEncode( bytes, 0, bytes.Length );
			return Encoding.UTF8.GetString( output, 0, output.Length );
		}

		/// <summary>
		/// Encodes a URL string. These method overloads can be used to encode the entire URL, including query-string values.
		/// </summary>
		/// <remarks>
		/// (c) Microsoft Corporation 2013. Code ported from .NET 4.5 4.5.50709.0.
		/// </remarks>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static string UrlEncode( byte[] bytes ) {
			if( bytes == null )
				return null;
			byte[] output = UrlEncode( bytes, 0, bytes.Length );
			return Encoding.UTF8.GetString( output, 0, output.Length );
		}

		/// <summary>
		/// Encodes a URL string. These method overloads can be used to encode the entire URL, including query-string values.
		/// </summary>
		/// <remarks>
		/// (c) Microsoft Corporation 2013. Code ported from .NET 4.5 4.5.50709.0.
		/// </remarks>
		/// <param name="bytes"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public static byte[] UrlEncode( byte[] bytes, int offset, int count ) {

			if( !ValidateUrlEncodingParameters( bytes, offset, count ) ) {
				return null;
			}

			int cSpaces = 0;
			int cUnsafe = 0;

			// count them first 
			for( int i = 0; i < count; i++ ) {
				char ch = (char)bytes[offset + i];

				if( ch == ' ' )
					cSpaces++;
				else if( !HttpEncoderUtility.IsUrlSafeChar( ch ) )
					cUnsafe++;
			}

			// nothing to expand? 
			if( cSpaces == 0 && cUnsafe == 0 )
				return bytes;

			// expand not 'safe' characters into %XX, spaces to +s
			byte[] expandedBytes = new byte[count + cUnsafe * 2];
			int pos = 0;

			for( int i = 0; i < count; i++ ) {
				byte b = bytes[offset + i];
				char ch = (char)b;

				if( HttpEncoderUtility.IsUrlSafeChar( ch ) ) {
					expandedBytes[pos++] = b;
				} else if( ch == ' ' ) {
					expandedBytes[pos++] = (byte)'+';
				} else {
					expandedBytes[pos++] = (byte)'%';
					expandedBytes[pos++] = (byte)HttpEncoderUtility.IntToHex( ( b >> 4 ) & 0xf );
					expandedBytes[pos++] = (byte)HttpEncoderUtility.IntToHex( b & 0x0f );
				}
			}

			return expandedBytes;

		}

		/// <summary>
		/// URL Encodes a byte array consistent with the behavior of HttpUtility.UrlTokenEncode( byte[] ).
		/// </summary>
		/// <remarks>
		/// (c) Microsoft Corporation 2013. Code ported from .NET 4.5 4.5.50709.0.
		/// </remarks>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string UrlTokenEncode( byte[] input ) {

			if( input == null )
				throw new ArgumentNullException( "input" );
			if( input.Length < 1 )
				return String.Empty;

			string base64Str = null;
			int endPos = 0;
			char[] base64Chars = null;

			//////////////////////////////////////////////////////// 
			// Step 1: Do a Base64 encoding
			base64Str = Convert.ToBase64String( input );
			if( base64Str == null )
				return null;

			//////////////////////////////////////////////////////// 
			// Step 2: Find how many padding chars are present in the end 
			for( endPos = base64Str.Length; endPos > 0; endPos-- ) {
				if( base64Str[endPos - 1] != '=' ) // Found a non-padding char! 
				{
					break; // Stop here
				}
			}

			//////////////////////////////////////////////////////// 
			// Step 3: Create char array to store all non-padding chars, 
			//      plus a char to indicate how many padding chars are needed
			base64Chars = new char[endPos + 1];
			base64Chars[endPos] = (char)( (int)'0' + base64Str.Length - endPos ); // Store a char at the end, to indicate how many padding chars are needed

			////////////////////////////////////////////////////////
			// Step 3: Copy in the other chars. Transform the "+" to "-", and "/" to "_" 
			for( int iter = 0; iter < endPos; iter++ ) {
				char c = base64Str[iter];

				switch( c ) {
					case '+':
						base64Chars[iter] = '-';
						break;

					case '/':
						base64Chars[iter] = '_';
						break;

					case '=':
						base64Chars[iter] = c;
						break;

					default:
						base64Chars[iter] = c;
						break;
				}
			}
			return new string( base64Chars );

		}

		internal static bool ValidateUrlEncodingParameters( byte[] bytes, int offset, int count ) {
			if( bytes == null && count == 0 )
				return false;
			if( bytes == null ) {
				throw new ArgumentNullException( "bytes" );
			}
			if( offset < 0 || offset > bytes.Length ) {
				throw new ArgumentOutOfRangeException( "offset" );
			}
			if( count < 0 || offset + count > bytes.Length ) {
				throw new ArgumentOutOfRangeException( "count" );
			}

			return true;
		}

	}

	//------------------------------------------------------------------------------ 
	// <copyright file="HttpEncoderUtility.cs" company="Microsoft">
	//     Copyright (c) Microsoft Corporation.  All rights reserved.
	// </copyright>
	//----------------------------------------------------------------------------- 
	internal static class HttpEncoderUtility {

		public static int HexToInt( char h ) {
			return ( h >= '0' && h <= '9' ) ? h - '0' :
			( h >= 'a' && h <= 'f' ) ? h - 'a' + 10 :
			( h >= 'A' && h <= 'F' ) ? h - 'A' + 10 :
			-1;
		}

		public static char IntToHex( int n ) {
			if( n <= 9 )
				return (char)( n + (int)'0' );
			else
				return (char)( n - 10 + (int)'a' );
		}

		// Set of safe chars, from RFC 1738.4 minus '+' 
		public static bool IsUrlSafeChar( char ch ) {
			if( ( ch >= 'a' && ch <= 'z' ) || ( ch >= 'A' && ch <= 'Z' ) || ( ch >= '0' && ch <= '9' ) )
				return true;

			switch( ch ) {
				case '-':
				case '_':
				case '.':
				case '!':
				case '*':
				case '(':
				case ')':
					return true;
			}

			return false;
		}

		//  Helper to encode spaces only
		internal static String UrlEncodeSpaces( string str ) {
			if( str != null && str.IndexOf( ' ' ) >= 0 )
				str = str.Replace( " ", "%20" );
			return str;
		}

	} 

}
