using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteamSharp.Helpers.Cryptography;
using SteamSharp.Test.TestHelpers;
using System;
using System.Text;

namespace SteamSharp.Test {

	[TestClass]
	public class Cryptography {

		[TestMethod]
		[TestCategory( "RSA" )]
		public void RSA_Encrypt_NoKeys() {

			RSAHelper rsa = new RSAHelper();
			AssertException.Throws<ArgumentNullException>( () => {
				byte[] encodedPassword = rsa.Encrypt( Encoding.UTF8.GetBytes( "Shouldn't work" ) );
			} );

		}

		[TestMethod]
		[TestCategory( "RSA" )]
		public void RSA_Encrypt() {

			string modulus = "E41855047AC427ABD0480E7DFA8396AAC3FC9167F5C8735519250D5A7C633B36B65065F945DAF0EA4964F314EC6E7A3E52511A5B7FEBE275138706417970D208D55BFDD8B112266F5FDDBA2A3C3FE27D82A575CC0C91733740B175233FBAC43A2EB6121AB2868C5204AA173B50DA9093279370D8E1A72BDD0D064D081E96BB100D9C6878E97EADA7CB931EB04011BE57DF16E10C57EA8DE1263618F1DE84540AF4BA24966AB60F8ADFE1D877B220E9AA414223E2D75B20E64A9D322898052F294A8F9F12B9123B60033A3786F0E0C803D6D7DDF2A5A2C3DCEA75DC3C7B39118FF6FA40E7BE7CED1FA02EE53F9D0B77D7BDCA40CC6C74AC66402F68555B426D1B";
			string exponent = "010001";

			RSAHelper rsa = new RSAHelper();

			rsa.ImportParameters( new RSAParameters {
				E = exponent.HexToByteArray(),
				N = modulus.HexToByteArray()
			} );

			string password = "Howdy do! foo ba$r/b@rs.";
			byte[] bytePassword = Encoding.UTF8.GetBytes( password );
			byte[] encodedPassword = rsa.Encrypt( bytePassword );
			string encryptedBase64Password = Convert.ToBase64String( encodedPassword );

		}

		//[TestMethod]
		[TestCategory( "RSA" )]
		public void RSA_EncryptDecrypt() {

			string modulus = "d2c0a39114f3f9bc6a638f04d9872178c2c2006f5a4c5151e930071df72a30a06434ed37623321323076f39a94d5755a815a7c1cd2066a85789ab5ccf64692126055781a05e09436ab5ad9f61cb0e779ae03902dcfcb213d0a7cc85c7e7cc551a11544a9b8331451a7f5f4d5ad88641c5d6939f4407c7bcc3c279b6d46630ba3";
			string exponent = "010001";
			string privateKey = "067fc44b840ee603a6703d87d3c17409ca4fbb3db3d628a7d2fe152a1a6625abbc8b59495cf0e0b430846a8cb8cc405b3323fc31d3543952b65e66fed4156709b5b556d3b048294f86045d93aba21974525676c71fe1048905f91d9ee610e10df930e2ffa1c80e973b78bd9a66f107437df3a7fdc21de86a6be0d293f464da69";

			RSAHelper rsa = new RSAHelper();

			rsa.ImportParameters( new RSAParameters {
				E = exponent.HexToByteArray(),
				N = modulus.HexToByteArray(),
				D = privateKey.HexToByteArray()
			} );

			string password = "Howdy do! foo ba$r/b@rs.";
			byte[] bytePassword = Encoding.UTF8.GetBytes( password );
			byte[] encodedPassword = rsa.Encrypt( bytePassword );
			string encryptedBase64Password = Convert.ToBase64String( encodedPassword );

			byte[] decrypted = rsa.Decrypt( Convert.FromBase64String( encryptedBase64Password ), privateKey.HexToByteArray() );

		}

		[TestMethod]
		[TestCategory( "RSA" )]
		public void SHA1_Hash() {

			string password = "Howdy do! foo ba$r/b@rs.";

			HashProviders.IHashProvider sha1 = new HashProviders.SHA1();
			byte[] hash = sha1.ComputeHash( Encoding.UTF8.GetBytes( password ) );
			string hashStr = BitConverter.ToString( hash ).Replace( "-", "" ).ToLower();

			Assert.AreEqual( "48627a6e9e0e4484ede4b545f30f13ba2ce27799", hashStr );

		}

	}

}
