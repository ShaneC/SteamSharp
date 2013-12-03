using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SteamSharp {

	public partial class SteamUser {

		#region GetPlayerSummaries
		/// <summary>
		/// Object for executing the GetPlayerSummaries API call.
		/// </summary>
		public class GetPlayerSummariesResponse {

			/// <summary>
			/// Containing object for the data of the GetPlayerSummaries API call.
			/// </summary>
			public GetPlayerSummariesContainer Response { get; set; }

		}

		/// <summary>
		/// Containing object for the data of the GetPlayerSummaries API call.
		/// </summary>
		public class GetPlayerSummariesContainer {

			/// <summary>
			/// List containing the Player objects selected via the SteamIDs specified.
			/// </summary>
			public List<Player> Players { get; set; }

		}

		/// <summary>
		/// Object representing the Steam information for a specific user.
		/// </summary>
		public class Player {

			/// <summary>
			/// 64bit SteamID of the user.
			/// </summary>
			public string SteamID { get; set; }

			/// <summary>
			/// This represents whether the profile is visible or not, and if it is visible, why you are allowed to see it.
			/// </summary>
			public CommunityVisibilityState CommunityVisibilityState { get; set; }

			/// <summary>
			/// Indicates if the user has set their public profile. Maps to the "profilestate" API property.
			/// </summary>
			private bool _profileState = false;
			[JsonProperty( "profilestate" )]
			public bool IsProfileSet {
				get { return _profileState; }
				set { _profileState = value; }
			}

			/// <summary>
			/// The player's persona name (display name).
			/// </summary>
			public string PersonaName { get; set; }

			/// <summary>
			/// DateTime the user last logged off.
			/// </summary>
			[JsonConverter( typeof( SteamInterfaceHelpers.UnixDateTimeConverter ) )]
			public DateTime LastLogOff { get; set; }

			/// <summary>
			/// The full URL of the user's Steam Community Profile.
			/// </summary>
			public string ProfileURL { get; set; }
			
			/// <summary>
			/// The full URL of the user's 32x32px avatar. If the user hasn't configured an avatar this will be the default ? avatar.
			/// </summary>
			[JsonProperty( "avatar" )]
			public string AvatarURL { get; set; }

			/// <summary>
			/// The full URL of the user's 64x64px avatar. If the user hasn't configured an avatar this will be the default ? avatar.
			/// </summary>
			[JsonProperty( "avatarmedium" )]
			public string AvatarMediumURL { get; set; }

			/// <summary>
			/// The full URL of the user's 184x184px avatar. If the user hasn't configured an avatar this will be the default ? avatar.
			/// </summary>
			[JsonProperty( "avatarfull" )]
			public string AvatarFullURL { get; set; }

			/// <summary>
			/// The user's current status.
			/// If the player's profile is private, this will always be "Offline", except if the user has set their status to looking to trade or looking to play (due to a bug, not long term behavior!).
			/// </summary>
			public PersonaState PersonaState { get; set; }

			/// <summary>
			/// The player's "Real Name", if they have set it.
			/// </summary>
			public string RealName { get; set; }

			/// <summary>
			/// The player's primary group, as configured in their Steam Community profile.
			/// </summary>
			public string PrimaryClanID { get; set; }

			/// <summary>
			/// The DateTime the player's account was created. Maps to the "timecreated" API property.
			/// </summary>
			[JsonProperty( "timecreated" )]
			[JsonConverter( typeof( SteamInterfaceHelpers.UnixDateTimeConverter ) )]
			public DateTime DateTimeCreated { get; set; }

			/// <summary>
			/// This is delivered on the response but is not documented. Object is a string as the full set of deserialization possibilities is unknown.
			/// </summary>
			public string PersonaStateFlags { get; set; }

			/// <summary>
			/// If the user is currently in-game, this will be the name of the game they are playing. If no game is being played this value is null.
			/// </summary>
			public string GameExtraInfo { get; set; }

			/// <summary>
			/// If the user is currently in-game, this value will be returned and set to the GameID of that game. Otherwise value is null.
			/// </summary>
			public string GameID { get; set; }

			/// <summary>
			/// If set on the user's Steam Community profile, The user's country of residence, 2-character ISO country code.
			/// </summary>
			public string LocCountryCode { get; set; }

			/// <summary>
			/// If set on the user's Steam Community profile, The user's state of residence.
			/// </summary>
			public string LocStateCode { get; set; }

			/// <summary>
			/// An internal code indicating the user's city of residence. A future update will provide this data in a more useful way.
			/// </summary>
			public int LocCityID { get; set; }

		}
		#endregion

		#region GetFriendsList
		/// <summary>
		/// Response object for the data of the GetFriendsList API call.
		/// </summary>
		public class GetFriendsListResponse {

			/// <summary>
			/// Container for the FriendsList object response.
			/// </summary>
			public FriendsList FriendsList { get; set; }

		}

		/// <summary>
		/// Containing object for the data of the GetFriendsList API call.
		/// </summary>
		public class FriendsList {

			/// <summary>
			/// List of Friends for specified user.
			/// </summary>
			public List<Friend> Friends { get; set; }

		}

		/// <summary>
		/// Object representing a single Friend of the specified user.
		/// Additional information can be pulled on the users by making a GetPlayerSummaries call with all SteamIDs contained in the List enumeration.
		/// </summary>
		public class Friend {

			/// <summary>
			/// 64bit SteamID of the user.
			/// </summary>
			public string SteamID { get; set; }

			/// <summary>
			/// Relationship filter for profile/friend's list filtering.
			/// </summary>
			public PlayerRelationshipType Relationship { get; set; }

			/// <summary>
			/// DateTime of when the relationship was created.
			/// </summary>
			[JsonProperty( "friend_since" )]
			[JsonConverter( typeof( SteamInterfaceHelpers.UnixDateTimeConverter ) )]
			public DateTime FriendSinceDateTime { get; set; }

		}
		#endregion

	}

}
