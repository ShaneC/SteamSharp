using SteamSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp.FlowTests.Tests {

	public class PlayerServiceCalls : ITestClass {

		public bool Invoke() {

			SteamUser user = UserAuthentication.Login();
			
			SteamClient client = new SteamClient();
			client.Authenticator = UserAuthenticator.ForProtectedResource( user );

			//var response = PlayerService.GetRecentlyPlayedGames( client, user.SteamID.ToString() );
			var response = SteamCommunity.GetFriendsList( client, user.SteamID );

			if( response == null )
				return false;

			return true;

		}

	}

}
