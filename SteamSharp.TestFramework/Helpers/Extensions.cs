using System.IO;
using System.Text;

namespace SteamSharp.TestFramework.Helpers {

	public static class Extensions {

		public static void WriteStringUTF8( this Stream target, string value ) {

			var encoded = Encoding.UTF8.GetBytes( value );
			target.Write( encoded, 0, encoded.Length );

		}

	}

}
