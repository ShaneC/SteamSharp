using System;
using Xunit;
using SteamSharp;

namespace SteamSharp.TestFramework {

	public class ProfileTests {

		[Fact]
		public void Can_Get_Profile() {

			// Did you remember to set your API Key?
			Assert.False( String.IsNullOrEmpty( ResourceConstants.AccessToken ) );



		}

	}

}
