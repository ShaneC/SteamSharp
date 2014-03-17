using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {
	public partial class SteamCommunity {

		#region ISteamUserStats interface
		#region GetGlobalAchievementPercentagesForApp
		/// <summary>
		/// Object for executing the GetGlobalAchievementPercentagesForApp API call.
		/// </summary>
		private class GetGlobalAchievementPercentagesForAppResponse {

			/// <summary>
			/// Containing object for the response of the GetGlobalAchievementPercentagesForApp API call.
			/// </summary>
			public AchievementPercentages AchievementPercentages { get; set; }

		}

		/// <summary>
		/// Containing object for the response of the GetGlobalAchievementPercentagesForApp API call.
		/// </summary>
		private class AchievementPercentages {

			/// <summary>
			/// List of global achievements for the specified GameID.
			/// </summary>
			public List<GlobalAchievement> Achievements { get; set; }

		}

		/// <summary>
		/// Individual achievement for the specified GameID, providing information on its completion.
		/// </summary>
		public class GlobalAchievement {

			/// <summary>
			/// API Name of the achievement (i.e. TF_SCOUT_LONG_DISTANCE_RUNNER, TF_HEAVY_DAMAGE_TAKEN).
			/// </summary>
			[JsonProperty( "name" )]
			public string APIName { get; set; }

			/// <summary>
			/// Percentage of that achievement's global completion.
			/// </summary>
			public double Percent { get; set; }

		}
		#endregion

		#region GetPlayerAchievements
		/// <summary>
		/// Object for executing the GetPlayerAchievements API call.
		/// </summary>
		private class GetPlayerAchievementsResponse {

			/// <summary>
			/// Achievement status of player's 
			/// </summary>
			[JsonProperty( "playerstats" )]
			public PlayerAchievements PlayerAchievements { get; set; }

		}

		/// <summary>
		/// Containing object for executing the GetPlayerAchievements API call.
		/// </summary>
		public class PlayerAchievements {

			/// <summary>
			/// 64bit SteamID of the user.
			/// </summary>
			public string SteamID { get; set; }

			/// <summary>
			/// Friendly name of the game specified (i.e. "Team Fortress 2")
			/// </summary>
			public string GameName { get; set; }

			/// <summary>
			/// List of possible <see cref="Achievement"/>s for the game specified. <see cref="Achievement"/> contains data about the specified user's progress.
			/// </summary>
			public List<Achievement> Achievements { get; set; }

		}

		/// <summary>
		/// Information about a possible achievement for the game specified, containing data about the specified user's progress.
		/// </summary>
		public class Achievement {

			/// <summary>
			/// API Name of the achievement (i.e. TF_SCOUT_LONG_DISTANCE_RUNNER, TF_HEAVY_DAMAGE_TAKEN).
			/// </summary>
			public string APIName { get; set; }

			/// <summary>
			/// Flag indicating whether or not the specified user has completed this achievement.
			/// </summary>
			[JsonProperty( "achieved" )]
			public bool IsAchieved { get; set; }

			/// <summary>
			/// Friendly name of the achievement (in the language specified, defaults to English). In example, "Race for the Pennant."
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Description of the achievement (in the language specified, defaults to English). In example, "Run 25 kilometers."
			/// </summary>
			public string Description { get; set; }

		}
		#endregion

		#region GetUserStatsForGame
		/// <summary>
		/// Object for executing the GetPlayerAchievements API call.
		/// </summary>
		private class GetUserStatsForGameResponse {

			/// <summary>
			/// Achievement status of player's 
			/// </summary>
			public PlayerStats PlayerStats { get; set; }

		}

		/// <summary>
		/// Containing object for executing the GetPlayerAchievements API call.
		/// </summary>
		public class PlayerStats {

			/// <summary>
			/// 64bit SteamID of the user.
			/// </summary>
			public string SteamID { get; set; }

			/// <summary>
			/// Friendly name of the game specified (i.e. "Team Fortress 2")
			/// </summary>
			public string GameName { get; set; }

			/// <summary>
			/// List of possible <see cref="Stat"/>s for the game specified. <see cref="Stat"/> contains data about the user's specific accomplishment.
			/// </summary>
			public List<Stat> Stats { get; set; }

		}

		/// <summary>
		/// Information about a possible stat for the game specified, containing data about the specified user's progress.
		/// </summary>
		public class Stat {

			/// <summary>
			/// API name of the stat (i.e. Scout.accum.iBuildingsDestroyed, Soldier.accum.iDominations)
			/// </summary>
			[JsonProperty( "name" )]
			public string APIName { get; set; }

			/// <summary>
			/// Value of the stat for the specified user.
			/// </summary>
			public double Value { get; set; }


		}
		#endregion
		#endregion

		#region ISteamUserOAuth interface
		/// <summary>
		/// Specified user's Friend's List.
		/// </summary>
		public class SteamFriendsList {

			/// <summary>
			/// List of Friends for specified user.
			/// </summary>
			public List<SteamFriend> Friends { get; set; }

			public int GetFriendCount() {
				return ( ( Friends == null ) ? 0 : Friends.Count );
			}

		}

		/// <summary>
		/// Object representing a single Friend of the specified user.
		/// Additional information can be pulled on the users by making a GetPlayerSummaries call with all SteamIDs contained in the List enumeration.
		/// </summary>
		public class SteamFriend {

			/// 64bit SteamID of the user.
			/// </summary>
			[JsonConverter( typeof( SteamInterfaceHelpers.SteamIDConverter ) )]
			public SteamID SteamID { get; set; }

			/// <summary>
			/// Relationship filter for profile/friend's list filtering.
			/// </summary>
			[JsonProperty( "friend_since" )]
			[JsonConverter( typeof( SteamInterfaceHelpers.UnixDateTimeConverter ) )]
			public DateTime FriendSince { get; set; }

			/// <summary>
			/// DateTime of when the relationship was created.
			/// </summary>
			[JsonProperty( "relationship" )]
			[JsonConverter( typeof( SteamInterfaceHelpers.RelationshipTypeConverter ) )]
			public PlayerRelationshipType RelationshipType { get; set; }

		}
		#endregion

		#region SteamUserInterface
		#region GetPlayerSummaries
		/// <summary>
		/// Object for executing the GetPlayerSummaries API call.
		/// </summary>
		private class GetPlayerSummariesResponse {

			/// <summary>
			/// Containing object for the data of the GetPlayerSummaries API call.
			/// </summary>
			public GetPlayerSummariesContainer Response { get; set; }

		}

		/// <summary>
		/// Containing object for the data of the GetPlayerSummaries API call.
		/// </summary>
		private class GetPlayerSummariesContainer {

			/// <summary>
			/// List containing the Player objects selected via the SteamIDs specified.
			/// </summary>
			public List<PlayerInfo> Players { get; set; }

		}

		public class PlayerInfo {

			/// <summary>
			/// User's SteamID.
			/// </summary>
			[JsonConverter( typeof( SteamInterfaceHelpers.SteamIDConverter ) )]
			public SteamID SteamID { get; set; }

			/// <summary>
			/// This represents whether the profile is visible or not, and if it is visible, why you are allowed to see it.
			/// </summary>
			public CommunityVisibilityState CommunityVisibilityState { get; set; }

			/// <summary>
			/// Indicates if the user has set their public profile. Maps to the "profilestate" API property.
			/// </summary>
			[JsonProperty( "profilestate" )]
			public bool IsProfileSet {
				get { return _profileState; }
				set { _profileState = value; }
			}

			private bool _profileState = false;

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
			[JsonProperty( "loccountrycode" )]
			public string CountryCode { get; set; }

			/// <summary>
			/// If set on the user's Steam Community profile, The user's state of residence.
			/// </summary>
			[JsonProperty( "locstatecode" )]
			public string StateCode { get; set; }

			/// <summary>
			/// An internal code indicating the user's city of residence. A future update will provide this data in a more useful way.
			/// </summary>
			[JsonProperty( "loccityid" )]
			public int CityID { get; set; }

		}
		#endregion

		#region GetFriendsList
		/// <summary>
		/// Response object for the data of the GetFriendsList API call.
		/// </summary>
		private class GetFriendsListResponse {

			/// <summary>
			/// Container for the SteamFriendsList object response.
			/// </summary>
			public SteamFriendsList FriendsList { get; set; }

		}
		#endregion
		#endregion

	}
}
