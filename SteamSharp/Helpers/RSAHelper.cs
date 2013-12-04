
namespace SteamSharp.Helpers {

	/// <summary>
	/// Helper class providing encryption and decryption via RSA.
	/// </summary>
	public class RSAHelper {

		private BigInteger Modulus;
		private BigInteger Exponent;

		/// <summary>
		/// Initializes the RSA Helper class with the necessary keys for encryption and decryption.
		/// </summary>
		/// <param name="modulus">Modulus value to be used in the RSA encryption/decryption.</param>
		/// <param name="exponent">Exponent value to be used in the RSA encryption/decryption.</param>
		public RSAHelper( byte[] modulus, byte[] exponent ) {
			Modulus = new BigInteger( modulus );
			Exponent = new BigInteger( exponent );
		}

		/// <summary>
		/// Encrypts a message using the set RSA modulus and exponent. Uses UTF-8 encoding on the message.
		/// </summary>
		/// <param name="message">Message to be encrypted.</param>
		/// <returns>Byte array containing the cipher text</returns>
		public byte[] Encrypt( byte[] message ) {
			// Generate message's BigInteger representation
			BigInteger biMessage = new BigInteger( message );
			// Perform RSA -- Cipher text is equal to ( message^exponent ) mod n.
			BigInteger cipherText = biMessage.modPow( Exponent, Modulus );
			return cipherText.getBytes();
		}

		/// <summary>
		/// Encrypts a message using the set RSA modulus and a given private key. Uses UTF-8 encoding on the message.
		/// </summary>
		/// <param name="cipherText">Cipher text.</param>
		/// <param name="privateKey">Private key which is paired with the generation of the initial RSA keys.</param>
		/// <returns>Byte array containing the plain text message.</returns>
		public byte[] Decrypt( byte[] cipherText, byte[] privateKey ) {
			// Generate cipher text and private key's BigInteger representation
			BigInteger biCipherText = new BigInteger( cipherText );
			BigInteger biPK = new BigInteger( privateKey );
			// Perform RSA -- Message text is equal to ( cipherText^privateKey ) mod n.
			BigInteger message = biCipherText.modPow( biPK, Modulus );
			return message.getBytes();
		}

	}

}
