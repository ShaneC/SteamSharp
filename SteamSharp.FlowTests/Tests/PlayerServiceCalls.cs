using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp.FlowTests.Tests {

	public class PlayerServiceCalls : ITestClass {

		public bool Invoke() {

			try {
				var response = PlayerService.GetRecentlyPlayedGames( new SteamClient(), "1234" );
			} catch( SteamRequestException ) {
				return true;
			} catch( Exception e ) {
				WriteConsole.Error( "Unexecpted Exception Encountered {" + e.GetType().ToString() + "}" );
			}

			return false;

		}

	}

}
