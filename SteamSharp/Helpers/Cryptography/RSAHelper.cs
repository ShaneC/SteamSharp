using System;

namespace SteamSharp.Helpers.Cryptography {

	/// <summary>
	/// RSA Encryption/Decryption.
	/// </summary>
	public class RSAHelper {

		private RSAParameters RSAParams = new RSAParameters();
		private PaddingProviders.IPaddingProvider paddingProvider;

		/// <summary>
		/// Initializes the RSA Helper class with the necessary keys for encryption and decryption.
		/// </summary>
		public RSAHelper() : this( new PaddingProviders.PKCS1v1_5() )
		{
		}

		/// <summary>
		/// Initializes the RSA Helper class with the necessary keys for encryption and decryption.
		/// </summary>
		/// <param name="useOAEP">Set to true if you wish to use OAEP as the padding provider for encryption/decryption.</param>
		public RSAHelper( bool useOAEP ) : this( new PaddingProviders.OAEP() )
		{
		}

		/// <summary>
		/// Initializes the RSA Helper class with the necessary keys for encryption and decryption.
		/// </summary>
		/// <param name="padding">Padding Provider to use for encoding and decoding of the message.</param>
		public RSAHelper( PaddingProviders.IPaddingProvider padding ) {
			paddingProvider = padding;
		}

		/// <summary>
		/// Import an existing set of RSA Parameters.  If only the public key is to be loaded, 
		/// Do not set the P, Q, DP, DQ, IQ or D values.  If P, Q or D are set, the parameters 
		/// will automatically be validated for existence of private key.
		/// </summary>
		/// <param name="parameters">RSAParameters object containing key data.</param>
		public void ImportParameters( RSAParameters parameters ) {

			if( ValidateKeyData( parameters ) ) {
				RSAParams.D = parameters.D;
				RSAParams.N = parameters.N;
				RSAParams.E = parameters.E;
			}

		}

		/// <summary>
		/// Encrypts a message using the set RSA modulus and exponent. Uses UTF-8 encoding on the message.
		/// </summary>
		/// <param name="message">Message to be encrypted.</param>
		/// <returns>Byte array containing the cipher text</returns>
		public byte[] Encrypt( byte[] message ) {
			return DoEncrypt( AddEncryptionPadding( message ) );
		}

		private byte[] DoEncrypt( byte[] message ) {
			// Generate message's BigInteger representation
			BigInteger biMessage = new BigInteger( message, message.Length );
			// Perform RSA -- Cipher text is equal to ( message^exponent ) mod n.
			BigInteger cipherText = biMessage.modPow( new BigInteger( RSAParams.E ), new BigInteger( RSAParams.N ) );
			return cipherText.getBytesRaw();
		}

		private byte[] AddEncryptionPadding( byte[] dataBytes ) {
			return paddingProvider.EncodeMessage( dataBytes, RSAParams );
		}

		/// <summary>
		/// Encrypts a message using the set RSA modulus and a given private key. Uses UTF-8 encoding on the message.
		/// </summary>
		/// <param name="cipherText">Cipher text.</param>
		/// <param name="privateKey">Private key which is paired with the generation of the initial RSA keys.</param>
		/// <returns>Byte array containing the plain text message.</returns>
		/// 
		public byte[] Decrypt( byte[] cipherText, byte[] privateKey ) {
			if( RSAParams.D == null )
				throw new ArgumentException( "Value for the Private Exponent (D) is missing or invalid." );
			return DoDecrypt( RemoveEncryptionPadding( cipherText ), privateKey );
		}

		private byte[] DoDecrypt( byte[] cipherText, byte[] privateKey ) {
			// Generate cipher text and private key's BigInteger representation
			BigInteger biCipherText = new BigInteger( cipherText, cipherText.Length );
			BigInteger biPK = new BigInteger( privateKey, privateKey.Length );
			// Perform RSA -- Message text is equal to ( cipherText^privateKey ) mod n.
			BigInteger message = biCipherText.modPow( biPK, new BigInteger( RSAParams.N ) );
			return message.getBytesRaw();
		}

		private byte[] RemoveEncryptionPadding( byte[] dataBytes ) {
			return paddingProvider.DecodeMessage( dataBytes, RSAParams );
		}

		private bool ValidateKeyData( RSAParameters parameters ) {

			if( parameters.N == null )
				throw new ArgumentException( "Value for Modulus (N) is missing or invalid." );
			if( parameters.E == null )
				throw new ArgumentException( "Value for Public Exponent (E) is missing or invalid." );

			return true;

		}

	}

	/// <summary>
	/// RSAParameters for Import / Export
	/// </summary>
	public class RSAParameters {

		private byte[] m_E;
		private byte[] m_N;
		private byte[] m_D;

		/// <summary>
		/// Parameter value E
		/// </summary>
		public byte[] E {
			get { return m_E; }
			set { m_E = value; }
		}

		/// <summary>
		/// Parameter value N
		/// </summary>
		public byte[] N {
			get { return m_N; }
			set { m_N = value; }
		}

		/// <summary>
		/// Parameter value D
		/// </summary>
		public byte[] D {
			get { return m_D; }
			set { m_D = value; }
		}

	}

}
