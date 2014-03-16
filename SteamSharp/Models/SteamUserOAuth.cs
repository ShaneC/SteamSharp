using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

	public partial class SteamUserOAuth {

		public class SteamFriendsList {

			public List<SteamFriend> Friends { get; set; }

			public int GetFriendCount() {
				return ( ( Friends == null ) ? 0 : Friends.Count );
			}

		}

		public class SteamFriend {

			[JsonProperty( "steamid" )]
			[JsonConverter( typeof( SteamInterfaceHelpers.SteamIDConverter ) )]
			public SteamID SteamID { get; set; }

			[JsonProperty( "friend_since" )]
			[JsonConverter( typeof( SteamInterfaceHelpers.UnixDateTimeConverter ) )]
			public DateTime FriendSince { get; set; }

			[JsonProperty( "relationship" )]
			[JsonConverter( typeof( SteamInterfaceHelpers.RelationshipTypeConverter ) )]
			public PlayerRelationshipType RelationshipType { get; set; }

		}

	}

}
