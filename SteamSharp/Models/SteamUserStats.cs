using Newtonsoft.Json;
using System.Collections.Generic;

namespace SteamSharp {

	public partial class SteamUserStats {

		#region GetGlobalAchievementPercentagesForApp
		/// <summary>
		/// Object for executing the GetGlobalAchievementPercentagesForApp API call.
		/// </summary>
		public class GetGlobalAchievementPercentagesForAppResponse {

			/// <summary>
			/// Containing object for the response of the GetGlobalAchievementPercentagesForApp API call.
			/// </summary>
			public AchievementPercentages AchievementPercentages { get; set; }

		}

		/// <summary>
		/// Containing object for the response of the GetGlobalAchievementPercentagesForApp API call.
		/// </summary>
		public class AchievementPercentages {

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
		public class GetPlayerAchievementsResponse {

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

	}

}
