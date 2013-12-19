using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace SteamSharp.Helpers.Cryptography {

	/// <summary>
	/// Symmetric encryption/decryption with AES/CBC/PKCS7.
	/// </summary>
	public class AESHelper {

		/// <summary>
		/// Symmetric encryption with AES/CBC/PKCS7.
		/// </summary>
		/// <param name="message">Message to be encrypted.</param>
		/// <param name="key">Private key for use with encryption.</param>
		/// <returns>Message in cipher text.</returns>
		public static byte[] Encrypt( byte[] message, byte[] key ){
			PaddedBufferedBlockCipher cipher = new PaddedBufferedBlockCipher( new CbcBlockCipher( new AesEngine() ), new Pkcs7Padding() );
			cipher.Init( true, new KeyParameter( key ) );
			return cipher.DoFinal( message );
		}

		/// <summary>
		/// Symmetric decryption with AES/CBC/PKCS7.
		/// </summary>
		/// <param name="cipherText">Message to be decrypted.</param>
		/// <param name="key">Private key for use with decryption.</param>
		/// <returns>Plain text message.</returns>
		public static byte[] Decrypt( byte[] cipherText, byte[] key ) {
			PaddedBufferedBlockCipher cipher = new PaddedBufferedBlockCipher( new CbcBlockCipher( new AesEngine() ), new Pkcs7Padding() );
			cipher.Init( false, new KeyParameter( key ) );
			return cipher.DoFinal( cipherText );
		}

	}

}
