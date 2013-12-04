using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteamSharp.Helpers;
using System.Text;

namespace SteamSharp.Test {

	[TestClass]
	public class Helpers {

		[TestMethod]
		[TestCategory( "RSA" )]
		public void RSA_Encrypt_Decrypt() {

			string modulus = "a5261939975948bb7a58dffe5ff54e65f0498f9175f5a09288810b8975871e99af3b5dd94057b0fc07535f5f97444504fa35169d461d0d30cf0192e307727c065168c788771c561a9400fb49175e9e6aa4e23fe11af69e9412dd23b0cb6684c4c2429bce139e848ab26d0829073351f4acd36074eafd036a5eb83359d2a698d3";
			string exponent = "010001";
			string privateKey = "8e9912f6d3645894e8d38cb58c0db81ff516cf4c7e5a14c7f1eddb1459d2cded4d8d293fc97aee6aefb861859c8b6a3d1dfe710463e1f9ddc72048c09751971c4a580aa51eb523357a3cc48d31cfad1d4a165066ed92d4748fb6571211da5cb14bc11b6e2df7c1a559e6d5ac1cd5c94703a22891464fba23d0d965086277a161";

			RSAHelper rsa = new RSAHelper( modulus.HexToByteArray(), exponent.HexToByteArray() );

			string message = "Howdy do! foo ba$r/b@rs.";

			byte[] cipherText = rsa.Encrypt( Encoding.UTF8.GetBytes( message ) );

			byte[] roundTrip = rsa.Decrypt( cipherText, privateKey.HexToByteArray() );
			string decodedRoundTrip = Encoding.UTF8.GetString( roundTrip );

			Assert.AreEqual( message, decodedRoundTrip );

		}

	}

}
