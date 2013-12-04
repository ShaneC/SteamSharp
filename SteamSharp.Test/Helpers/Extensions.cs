using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace SteamSharp.Test.Helpers {

	public static class Extensions {

		public static void WriteStringUTF8( this Stream target, string value ) {

			var encoded = Encoding.UTF8.GetBytes( value );
			target.Write( encoded, 0, encoded.Length );

		}

		public static string StreamToString( this Stream target ) {

			var streamReader = new StreamReader( target );
			return streamReader.ReadToEnd();

		}

	}

	public static class AssertException {

		public static T Throws<T>( Action action ) where T : Exception {

			try {
				action();
			} catch( T exception ) {
				return exception;
			} catch( Exception e ) {
				Assert.Fail( String.Format( 
					"Expected Exception of type {0} to be thrown. Exception of type {1} encountered instead.", 
					typeof( T ), 
					e.GetType()
				) );
			}

			Assert.Fail( "Expected Exception of Type {0} to be thrown. No Exception was encountered.", typeof( T ) );

			return null;

		}

	}

}