using System;

namespace SteamSharp.Helpers.Cryptography {

	/// <summary>
	/// Padding Providers for use in RSA.
	/// </summary>
	public class PaddingProviders {

		/// <summary>
		/// Base interface that must be implemented by all hash providers.
		/// </summary>
		public interface IPaddingProvider {

			/// <summary>
			/// Adds padding to the input data and returns the padded data.
			/// </summary>
			/// <param name="dataBytes">Data to be padded prior to encryption</param>
			/// <param name="parameters">RSA Parameters used for padding computation</param>
			/// <returns>Padded message</returns>
			byte[] EncodeMessage( byte[] dataBytes, RSAParameters parameters );
			/// <summary>
			/// Removes padding that was added to the unencrypted data prior to encryption.
			/// </summary>
			/// <param name="dataBytes">Data to have padding removed</param>
			/// <param name="parameters">RSA Parameters used for padding computation</param>
			/// <returns>Unpadded message</returns>
			byte[] DecodeMessage( byte[] dataBytes, RSAParameters parameters );
			/// <summary>
			/// Gets the maximum message length for this padding provider.
			/// </summary>
			/// <param name="parameters">RSA Parameters used for padding computation</param>
			/// <returns>Max message length</returns>
			int GetMaxMessageLength( RSAParameters parameters );

		}

		/// <summary>
		/// Padding provider which uses no padding on message encrypt and decrypt. 
		/// </summary>
		public class NoPadding : IPaddingProvider {

			/// <summary>
			/// Adds padding to the input data and returns the padded data.
			/// </summary>
			/// <param name="dataBytes">Data to be padded prior to encryption</param>
			/// <param name="parameters">RSA Parameters</param>
			public byte[] EncodeMessage( byte[] dataBytes, RSAParameters parameters ) {
				return dataBytes;
			}

			/// <summary>
			/// Removes padding that was added to the unencrypted data prior to encryption.
			/// </summary>
			/// <param name="dataBytes">Data to have padding removed</param>
			/// <param name="parameters">RSA Parameters used for padding computation</param>
			/// <returns>Unpadded message</returns>
			public byte[] DecodeMessage( byte[] dataBytes, RSAParameters parameters ) {
				return dataBytes;
			}

			/// <summary>
			/// Gets the maximum message length for this padding provider.
			/// </summary>
			/// <param name="parameters">RSA Parameters used for padding computation</param>
			/// <returns>Max message length</returns>
			public int GetMaxMessageLength( RSAParameters parameters ) {
				return parameters.N.Length;
			}

		}

		/// <summary>
		/// Padding provider based on the OAEP standard.
		/// </summary>
		public class OAEP : IPaddingProvider {

			private HashProviders.IHashProvider m_hashProvider;
			//Hash length.  For SHA1, the length is 20 bytes.
			private int m_hLen = 20;
			//Length of message (dataBytes)
			private int m_mLen;
			//Number of bytes in the public key modulus
			private int m_k;

			/// <summary>
			/// Default constructor.  Uses the default SHA1 Hash for OAEP hash calculation.
			/// </summary>
			public OAEP() {
				m_hashProvider = new HashProviders.SHA1();
			}

			/// <summary>
			/// Internal constructor (used to perform OAEP with a different hash and hash output length
			/// </summary>
			/// <param name="ohashProvider"></param>
			/// <param name="hashLength"></param>
			internal OAEP( HashProviders.IHashProvider ohashProvider, int hashLength ) {
				m_hashProvider = ohashProvider;
				m_hLen = hashLength;
			}

			/// <summary>
			/// Adds padding to the input data and returns the padded data.
			/// </summary>
			/// <param name="dataBytes">Data to be padded prior to encryption</param>
			/// <param name="parameters">RSA Parameters used for padding computation</param>
			/// <returns>Padded message</returns>
			public byte[] EncodeMessage( byte[] dataBytes, RSAParameters parameters ) {
				//Iterator
				int i = 0;

				//Get the size of the data to be encrypted
				m_mLen = dataBytes.Length;

				//Get the size of the public modulus (will serve as max length for cipher text)
				m_k = parameters.N.Length;

				if( m_mLen > GetMaxMessageLength( parameters ) ) {
					throw new Exception( "Bad Data." );
				}

				//Generate the random octet seed (same length as hash)
				BigInteger biSeed = new BigInteger();
				biSeed.genRandomBits( m_hLen * 8, new Random() );
				byte[] bytSeed = biSeed.getBytesRaw();

				//Make sure all of the bytes are greater than 0.
				for( i = 0; i <= bytSeed.Length - 1; i++ ) {
					if( bytSeed[i] == 0x00 ) {
						//Replacing with the prime byte 17, no real reason...just picked at random.
						bytSeed[i] = 0x17;
					}
				}

				//Mask the seed with MFG Function(SHA1 Hash)
				//This is the mask to be XOR'd with the DataBlock below.
				byte[] dbMask = CryptoMathematics.OAEPMGF( bytSeed, m_k - m_hLen - 1, m_hLen, m_hashProvider );

				//Compute the length needed for PS (zero padding) and 
				//fill a byte array to the computed length
				int psLen = GetMaxMessageLength( parameters ) - m_mLen;

				//Generate the SHA1 hash of an empty L (Label).  Label is not used for this 
				//application of padding in the RSA specification.
				byte[] lHash = m_hashProvider.ComputeHash( System.Text.Encoding.UTF8.GetBytes( string.Empty.ToCharArray() ) );

				//Create a dataBlock which will later be masked.  The 
				//data block includes the concatenated hash(L), PS, 
				//a 0x01 byte, and the message.
				int dbLen = m_hLen + psLen + 1 + m_mLen;
				byte[] dataBlock = new byte[dbLen];

				int cPos = 0;
				//Current position

				//Add the L Hash to the data blcok
				for( i = 0; i <= lHash.Length - 1; i++ ) {
					dataBlock[cPos] = lHash[i];
					cPos += 1;
				}

				//Add the zero padding
				for( i = 0; i <= psLen - 1; i++ ) {
					dataBlock[cPos] = 0x00;
					cPos += 1;
				}

				//Add the 0x01 byte
				dataBlock[cPos] = 0x01;
				cPos += 1;

				//Add the message
				for( i = 0; i <= dataBytes.Length - 1; i++ ) {
					dataBlock[cPos] = dataBytes[i];
					cPos += 1;
				}

				//Create the masked data block.
				byte[] maskedDB = CryptoMathematics.BitwiseXOR( dbMask, dataBlock );

				//Create the seed mask
				byte[] seedMask = CryptoMathematics.OAEPMGF( maskedDB, m_hLen, m_hLen, m_hashProvider );

				//Create the masked seed
				byte[] maskedSeed = CryptoMathematics.BitwiseXOR( bytSeed, seedMask );

				//Create the resulting cipher - starting with a 0 byte.
				byte[] result = new byte[parameters.N.Length];
				result[0] = 0x00;

				//Add the masked seed
				maskedSeed.CopyTo( result, 1 );

				//Add the masked data block
				maskedDB.CopyTo( result, maskedSeed.Length + 1 );

				return result;
			}

			/// <summary>
			/// Removes padding that was added to the unencrypted data prior to encryption.
			/// </summary>
			/// <param name="dataBytes">Data to have padding removed</param>
			/// <param name="parameters">RSA Parameters used for padding computation</param>
			/// <returns>Unpadded message</returns>
			public byte[] DecodeMessage( byte[] dataBytes, RSAParameters parameters ) {

				m_k = parameters.D.Length;
				if( !( m_k == dataBytes.Length ) ) {
					throw new Exception( "Bad Data." );
				}

				//Length of the datablock
				int iDBLen = dataBytes.Length - m_hLen - 1;

				//Starting index for the data block.  This will be equal to m_hLen + 1.  The 
				//index is zero based, and the dataBytes should start with a single leading byte, 
				//plus the maskedSeed (equal to hash length m_hLen).
				int iDBidx = m_hLen + 1;

				//Single byte for leading byte
				byte bytY = 0;

				//Byte array matching the length of the hashing algorithm.
				//This array will hold the masked seed.
				byte[] maskedSeed = new byte[m_hLen];

				//Byte array matching the length of the following:
				//Private Exponent D minus Hash Length, minus 1 (for the leading byte)
				byte[] maskedDB = new byte[iDBLen];

				//Copy the leading byte
				bytY = dataBytes[0];

				//Copy the mask
				Array.Copy( dataBytes, 1, maskedSeed, 0, m_hLen );

				//Copy the data block
				Array.Copy( dataBytes, iDBidx, maskedDB, 0, iDBLen );

				//Reproduce the seed mask from the masked data block using the mask generation function
				byte[] seedMask = CryptoMathematics.OAEPMGF( maskedDB, m_hLen, m_hLen, m_hashProvider );

				//Reproduce the Seed from the Seed Mask.
				byte[] seed = CryptoMathematics.BitwiseXOR( maskedSeed, seedMask );

				//Reproduce the data block bask from the seed using the mask generation function
				byte[] dbMask = CryptoMathematics.OAEPMGF( seed, m_k - m_hLen - 1, m_hLen, m_hashProvider );

				//Reproduce the data block from the masked data block and the seed
				byte[] dataBlock = CryptoMathematics.BitwiseXOR( maskedDB, dbMask );

				//Pull the message from the data block.  First m_hLen bytes are the lHash, 
				//followed by padding of 0x00's, followed by a single 0x01, then the message.
				//So we're going to start and index m_hLen and work forward.
				if( !( dataBlock[m_hLen] == 0x00 ) ) {
					throw new Exception( "Decryption Error. Bad Data." );
				}

				//If we passed the 0x00 first byte test, iterate through the 
				//data block and find the terminating character.
				int iDataIdx = 0;


				for( int i = m_hLen; i <= dataBlock.Length - 1; i++ ) {
					if( dataBlock[i] == 0x01 ) {
						iDataIdx = i + 1;
						break;
					}
				}

				//Now find the length of the data and copy it to a byte array.
				int iDataLen = dataBlock.Length - iDataIdx;
				byte[] result = new byte[iDataLen];
				Array.Copy( dataBlock, iDataIdx, result, 0, iDataLen );

				return result;

			}

			/// <summary>
			/// Gets the maximum message length for this padding provider.
			/// </summary>
			/// <param name="parameters">RSA Parameters used for padding computation</param>
			/// <returns>Max message length</returns>
			public int GetMaxMessageLength( RSAParameters parameters ) {
				return parameters.N.Length - ( 2 * m_hLen ) - 2;
			}

		}

		/// <summary>
		/// Padding provider based on the PKCS#1 v1.5 standard.
		/// </summary>
		public sealed class PKCS1v1_5 : IPaddingProvider {

			/// <summary>
			/// Default constructor.
			/// </summary>
			public PKCS1v1_5() { }

			/// <summary>
			/// Adds padding to the input data and returns the padded data.
			/// </summary>
			/// <param name="dataBytes">Data to be padded prior to encryption</param>
			/// <param name="parameters">RSA Parameters used for padding computation</param>
			/// <returns>Padded message</returns>
			public byte[] EncodeMessage( byte[] dataBytes, RSAParameters parameters ) {

				//Determine if we can add padding.
				if( dataBytes.Length > GetMaxMessageLength( parameters ) ) {
					throw new Exception( "Data length is too long. Increase your key size or consider encrypting less data." );
				}

				int padLength = parameters.N.Length - dataBytes.Length - 3;
				BigInteger biRnd = new BigInteger();
				biRnd.genRandomBits( padLength * 8, new Random( DateTime.Now.Millisecond ) );

				byte[] bytRandom = null;
				bytRandom = biRnd.getBytes();

				int z1 = bytRandom.Length;

				//Make sure the bytes are all > 0.
				for( int i = 0; i <= bytRandom.Length - 1; i++ ) {
					if( bytRandom[i] == 0x00 ) {
						bytRandom[i] = 0x01;
					}
				}

				byte[] result = new byte[parameters.N.Length];


				//Add the starting 0x00 byte
				result[0] = 0x00;

				//Add the version code 0x02 byte
				result[1] = 0x02;

				for( int i = 0; i <= bytRandom.Length - 1; i++ ) {
					z1 = i + 2;
					result[z1] = bytRandom[i];
				}

				//Add the trailing 0 byte after the padding.
				result[bytRandom.Length + 2] = 0x00;

				//This starting index for the unpadded data.
				int idx = bytRandom.Length + 3;

				//Copy the unpadded data to the padded byte array.
				dataBytes.CopyTo( result, idx );

				return result;

			}

			/// <summary>
			/// Removes padding that was added to the unencrypted data prior to encryption.
			/// </summary>
			/// <param name="dataBytes">Data to have padding removed</param>
			/// <param name="parameters">RSA Parameters used for padding computation</param>
			/// <returns>Unpadded message</returns>
			public byte[] DecodeMessage( byte[] dataBytes, RSAParameters parameters ) {

				byte[] bytDec = new byte[parameters.N.Length];

				int lenDiff = 0;

				dataBytes.CopyTo( bytDec, lenDiff );

				if( ( bytDec[0] != 0x0 ) && ( bytDec[1] != 0x02 ) ) {
					throw new Exception( "Invalid padding. Supplied data does not contain valid PKCS#1 v1.5 padding. Padding could not be removed." );
				}

				//Find out where the padding ends.
				int idxEnd = 0;
				int dataLength = 0;

				for( int i = 2; i < bytDec.Length; i++ ) {
					if( bytDec[i] == 0x00 ) {
						idxEnd = i;
						break;
					}
				}

				//Calculate the length of the unpadded data
				dataLength = bytDec.Length - idxEnd - 2;

				byte[] result = new byte[dataLength + 1];

				int idxRslt = 0;

				//Put the unpadded data into the result array
				for( int i = idxEnd + 1; i <= bytDec.Length - 1; i++ ) {
					result[idxRslt] = bytDec[i];
					idxRslt += 1;
				}

				return result;

			}

			/// <summary>
			/// Gets the maximum message length for this padding provider.
			/// </summary>
			/// <param name="parameters">RSA Parameters used for padding computation</param>
			/// <returns>Max message length</returns>
			public int GetMaxMessageLength( RSAParameters parameters ) {
				return parameters.N.Length - 11;
			}

		}

	}

}
