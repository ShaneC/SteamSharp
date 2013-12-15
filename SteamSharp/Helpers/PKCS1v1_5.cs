using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp.Helpers {

	/// <summary>
	/// Uses PKCS#1 v 1.5 padding scheme to pad the data.
	/// </summary>
	/// <remarks></remarks>
	public sealed class PKCS1v1_5 {

		/// <summary>
		/// Default constructor.
		/// </summary>
		public PKCS1v1_5() { }

		/// <summary>
		/// Adds padding to the input data and returns the padded data.
		/// </summary>
		/// <param name="dataBytes">Data to be padded prior to encryption</param>
		/// <param name="params">RSA Parameters used for padding computation</param>
		/// <returns>Padded message</returns>
		public byte[] EncodeMessage( byte[] dataBytes, RSAParameters @params ) {

			//Determine if we can add padding.
			if( dataBytes.Length > GetMaxMessageLength( @params ) ) {
				throw new Exception( "Data length is too long. Increase your key size or consider encrypting less data." );
			}

			int padLength = @params.N.Length - dataBytes.Length - 3;
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

			byte[] result = new byte[@params.N.Length];


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
		/// <param name="params">RSA Parameters used for padding computation</param>
		/// <returns>Unpadded message</returns>
		public byte[] DecodeMessage( byte[] dataBytes, RSAParameters @params ) {

			byte[] bytDec = new byte[@params.N.Length];

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
		/// <param name="params">RSA Parameters used for padding computation</param>
		/// <returns>Max message length</returns>
		public int GetMaxMessageLength( RSAParameters @params ) {
			return @params.N.Length - 11;
		}

	}

}
