using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SteamSharp.TestFramework {
	
	public class RequestFormation {

		[Fact]
		public void Creation_Of_Client() {

			SteamClient client = new SteamClient();

			Assert.NotNull( client );
			Assert.NotNull( client.AssemblyVersion );

		}

		public void Creation_Of_Request() {



		}

	}

}
