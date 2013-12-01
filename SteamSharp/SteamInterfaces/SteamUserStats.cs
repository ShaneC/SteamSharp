using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {
	
	public partial class SteamUserStats : SteamInterface {

		#region GetGlobalAchievementPercentagesForApp
		/// <summary>
		/// Returns global achievements overview for a specific game (in percentages)
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetGlobalAchievementPercentagesForApp_.28v0002.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="gameID">AppID of the game you want the news of.</param>
		/// <returns>A List of <see cref="GlobalAchievement"/> objects containing the achievement name and percentage.</returns>
		public static List<GlobalAchievement> GetGlobalAchievementPercentagesForApp( SteamClient client, int gameID ) {
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
		/// <returns>A List of <see cref="GlobalAchievement"/> objects containing the achievement name and percentage.</returns>
		public async static Task<List<GlobalAchievement>> GetGlobalAchievementPercentagesForAppAsync( SteamClient client, int gameID ) {

			SteamRequest request = new SteamRequest( SteamAPIInterface.ISteamUserStats, "GetGlobalAchievementPercentagesForApp", SteamMethodVersion.v0002 );
			request.AddParameter( "gameid", gameID );

			return VerifyAndDeserialize<AchievementPercentagesResponse>( ( await client.ExecuteAsync( request ) ) ).achievementpercentages.achievements;

		}
		#endregion

		#region GetPlayerAchievementsAsync
		/// <summary>
		/// Returns a list of achievements for the requested user by GameID (AppID)
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetPlayerAchievements_.28v0001.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID to return friend's list for.</param>
		/// <param name="gameID">AppID of the game you want the news of.</param>
		/// <returns><see cref="PlayerStats"/> object containing game name and list of <see cref="Achivement"/> objects.</returns>
		public static PlayerStats GetPlayerAchievements( SteamClient client, string steamID, int gameID ) {
			return GetPlayerAchievements( client, steamID, gameID, RequestedLangage.English );
		}

		/// <summary>
		/// Returns a list of achievements for the requested user by GameID (AppID)
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetPlayerAchievements_.28v0001.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID to return friend's list for.</param>
		/// <param name="gameID">AppID of the game you want the news of.</param>
		/// <param name="returnLanguage">Desired language for the "name" and "description" properties of returned <see cref="Achievement"/> objects.</param>
		/// <returns><see cref="PlayerStats"/> object containing game name and list of <see cref="Achivement"/> objects.</returns>
		public static PlayerStats GetPlayerAchievements( SteamClient client, string steamID, int gameID, RequestedLangage returnLanguage ) {
			try {
				return GetPlayerAchievementsAsync( client, steamID, gameID, returnLanguage ).Result;
			} catch( AggregateException e ) {
				if( e.InnerException != null )
					throw e.InnerException;
				throw e;
			}
		}

		/// <summary>
		/// (Asynchronous) Returns a list of achievements for the requested user by GameID (AppID)
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetPlayerAchievements_.28v0001.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID to return friend's list for.</param>
		/// <param name="gameID">AppID of the game you want the news of.</param>
		/// <returns><see cref="PlayerStats"/> object containing game name and list of <see cref="Achivement"/> objects.</returns>
		public async static Task<PlayerStats> GetPlayerAchievementsAsync( SteamClient client, string steamID, int gameID ) {
			return await GetPlayerAchievementsAsync( client, steamID, gameID, RequestedLangage.English );
		}

		/// <summary>
		/// (Asynchronous) Returns a list of achievements for the requested user by GameID (AppID)
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetPlayerAchievements_.28v0001.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID to return friend's list for.</param>
		/// <param name="gameID">AppID of the game you want the news of.</param>
		/// <param name="returnLanguage">Desired language for the "name" and "description" properties of returned <see cref="Achievement"/> objects.</param>
		/// <returns><see cref="PlayerStats"/> object containing game name and list of <see cref="Achivement"/> objects.</returns>
		public async static Task<PlayerStats> GetPlayerAchievementsAsync( SteamClient client, string steamID, int gameID, RequestedLangage returnLanguage ) {

			SteamRequest request = new SteamRequest( SteamAPIInterface.ISteamUserStats, "GetPlayerAchievements", SteamMethodVersion.v0001 );
			request.AddParameter( "appid", gameID );
			request.AddParameter( "steamid", steamID );

			request.AddParameter( "l", GetLanguageFromEnum( returnLanguage ) );

			return VerifyAndDeserialize<GetPlayerAchievementsResponse>( ( await client.ExecuteAsync( request ) ) ).playerstats;

		}
		#endregion

	}

}
