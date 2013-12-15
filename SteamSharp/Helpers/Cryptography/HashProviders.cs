using Org.BouncyCastle.Crypto.Digests;

namespace SteamSharp.Helpers.Cryptography {

	/// <summary>
	/// HashProviders for use in the RSAHelper
	/// </summary>
	public class HashProviders {

		/// <summary>
		/// Base interface that must be implemented by all hash providers.
		/// </summary>
		public interface IHashProvider {

			/// <summary>
			/// Compute the hash of the input byte array and return the hashed value as a byte array.
			/// </summary>
			/// <param name="inputData">Input data</param>
			/// <returns>Hashed data.</returns>
			byte[] ComputeHash( byte[] inputData );
		}

		 /// <summary>
		 /// Hash provider based on SHA1
		 /// </summary>
		public sealed class SHA1 : IHashProvider {
			/// <summary>
			/// Default constructor.
			/// </summary>
			public SHA1() { }

			/// <summary>
			/// Compute the hash of the input byte array and return the hashed value as a byte array.
			/// </summary>
			/// <param name="inputData">Input data</param>
			/// <returns>SHA1 Hashed data.</returns>
			byte[] IHashProvider.ComputeHash( byte[] inputData ) {
				Sha1Digest digest = new Sha1Digest();
				digest.BlockUpdate( inputData, 0, inputData.Length );
				byte[] result = new byte[digest.GetDigestSize()];
				digest.DoFinal( result, 0 );
				return result;
			}

		}

	}

}
