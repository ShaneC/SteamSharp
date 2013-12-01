using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {
	
	public partial class SteamUserStats : SteamInterface {

		/// <summary>
		/// Returns global achievements overview for a specific game (in percentages)
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetGlobalAchievementPercentagesForApp_.28v0002.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="gameID">AppID of the game you want the news of.</param>
		/// <returns>A List of <see cref="Achievement"/> objects containing the achievement name and percentage.</returns>
		public static List<Achievement> GetGlobalAchievementPercentagesForApp( SteamClient client, int gameID ) {
			try {
				return GetGlobalAchievementPercentagesForAppAsync( client, gameID ).Result;
			} catch( AggregateException e ) {
				if( e.InnerException != null )
					throw e.InnerException;
				throw e;
			}
		}

		/// <summary>
		/// (Asynchronous) Returns global achievements overview for a specific game (in percentages)
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetGlobalAchievementPercentagesForApp_.28v0002.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="gameID">AppID of the game you want the news of.</param>
		/// <returns>A List of <see cref="Achievement"/> objects containing the achievement name and percentage.</returns>
		public async static Task<List<Achievement>> GetGlobalAchievementPercentagesForAppAsync( SteamClient client, int gameID ) {

			SteamRequest request = new SteamRequest( SteamAPIInterface.ISteamUserStats, "GetGlobalAchievementPercentagesForApp", SteamMethodVersion.v0002 );
			request.AddParameter( "gameid", gameID );

			return VerifyAndDeserialize<AchievementPercentagesResponse>( ( await client.ExecuteAsync( request ) ) ).achievementpercentages.achievements;

		}

	}

}
