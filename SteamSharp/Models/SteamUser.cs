using System.Collections.Generic;

namespace SteamSharp {

	public partial class SteamUser {

		public class GetPlayerSummariesResponse {
			public GetPlayerSummariesContainer response { get; set; }
		}

		public class GetPlayerSummariesContainer {
			public List<Player> players { get; set; }
		}

		public class Player {
			public string steamid { get; set; }
			public int communityvisibilitystate { get; set; }
			public bool profilestate { get; set; }
			public string personaname { get; set; }
			public int lastlogoff { get; set; }
			public string profileurl { get; set; }
			public string avatar { get; set; }
			public string avatarmedium { get; set; }
			public string avatarfull { get; set; }
			public int personastate { get; set; }
			public string realname { get; set; }
			public string primaryclanid { get; set; }
			public int timecreated { get; set; }
			public string personastateflags { get; set; }
			public string gameextrainfo { get; set; }
			public string gameid { get; set; }
			public string loccountrycode { get; set; }
			public string locstatecode { get; set; }
			public int loccityid { get; set; }
		}

		public class GetFriendsListResponse {
			public FriendsList friendslist { get; set; }
		}

		public class FriendsList {
			public List<Friend> friends { get; set; }
		}

		public class Friend {
			public string steamid { get; set; }
			public string relationship { get; set; }
			public int friend_since { get; set; }
		}

	}

}
