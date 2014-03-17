using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

	/// <summary>
	/// Steam Players, Friends, and Users
	/// </summary>
	public partial class SteamCommunity : SteamInterface {

		#region ISteamUserStats interface
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

			return VerifyAndDeserialize<GetGlobalAchievementPercentagesForAppResponse>( ( await client.ExecuteAsync( request ) ) ).AchievementPercentages.Achievements;

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
		/// <returns><see cref="PlayerAchievements"/> object containing game name and list of <see cref="Achievement"/> objects.</returns>
		public static PlayerAchievements GetPlayerAchievements( SteamClient client, string steamID, int gameID ) {
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
		/// <returns><see cref="PlayerAchievements"/> object containing game name and list of <see cref="Achievement"/> objects.</returns>
		public static PlayerAchievements GetPlayerAchievements( SteamClient client, string steamID, int gameID, RequestedLangage returnLanguage ) {
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
		/// <returns><see cref="PlayerAchievements"/> object containing game name and list of <see cref="Achievement"/> objects.</returns>
		public async static Task<PlayerAchievements> GetPlayerAchievementsAsync( SteamClient client, string steamID, int gameID ) {
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
		/// <returns><see cref="PlayerAchievements"/> object containing game name and list of <see cref="Achievement"/> objects.</returns>
		public async static Task<PlayerAchievements> GetPlayerAchievementsAsync( SteamClient client, string steamID, int gameID, RequestedLangage returnLanguage ) {

			SteamRequest request = new SteamRequest( SteamAPIInterface.ISteamUserStats, "GetPlayerAchievements", SteamMethodVersion.v0001 );
			request.AddParameter( "appid", gameID );
			request.AddParameter( "steamid", steamID );

			request.AddParameter( "l", GetLanguageFromEnum( returnLanguage ) );

			return VerifyAndDeserialize<GetPlayerAchievementsResponse>( ( await client.ExecuteAsync( request ) ) ).PlayerAchievements;

		}
		#endregion

		#region GetUserStatsForGame
		/// <summary>
		/// Returns a list of stats for this user by GameID (App ID).
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetUserStatsForGame_.28v0002.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID to return friend's list for.</param>
		/// <param name="gameID">AppID of the game you want the news of.</param>
		/// <returns><see cref="PlayerStats"/> object containing game name and list of <see cref="Stat"/> objects.</returns>
		public static PlayerStats GetUserStatsForGame( SteamClient client, string steamID, int gameID ) {
			try {
				return GetUserStatsForGameAsync( client, steamID, gameID ).Result;
			} catch( AggregateException e ) {
				if( e.InnerException != null )
					throw e.InnerException;
				throw e;
			}
		}

		/// <summary>
		/// (Asynchronous) Returns a list of stats for this user by GameID (App ID).
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetUserStatsForGame_.28v0002.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID to return friend's list for.</param>
		/// <param name="gameID">AppID of the game you want the news of.</param>
		/// <returns><see cref="PlayerStats"/> object containing game name and list of <see cref="Stat"/> objects.</returns>
		public async static Task<PlayerStats> GetUserStatsForGameAsync( SteamClient client, string steamID, int gameID ) {

			SteamRequest request = new SteamRequest( SteamAPIInterface.ISteamUserStats, "GetUserStatsForGame", SteamMethodVersion.v0002 );
			request.AddParameter( "appid", gameID );
			request.AddParameter( "steamid", steamID );

			return VerifyAndDeserialize<GetUserStatsForGameResponse>( ( await client.ExecuteAsync( request ) ) ).PlayerStats;

		}
		#endregion
		#endregion

		#region ISteamUserOAuth interface
		#region GetFriendsList
		/// <summary>
		/// (Requires <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/> or <see cref="SteamSharp.Authenticators.UserAuthenticator"/>)
		/// Returns the friend list of any Steam user, provided the user's Steam Community profile visibility is set to "Public."
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID to return friend's list for.</param>
		/// <returns><see cref="SteamFriendsList"/> object containing a list of <see cref="SteamFriend"/> objects mapping to the Friend's list of the target user.</returns>
		public static SteamFriendsList GetFriendsList( SteamClient client, SteamID steamID ) {
			try {
				return GetFriendsListAsync( client, steamID ).Result;
			} catch( AggregateException e ) {
				if( e.InnerException != null )
					throw e.InnerException;
				throw e;
			}
		}

		/// <summary>
		/// (Async) (Requires <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/> or <see cref="SteamSharp.Authenticators.UserAuthenticator"/>)
		/// Returns the friend list of any Steam user, provided the user's Steam Community profile visibility is set to "Public."
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID to return friend's list for.</param>
		/// <returns><see cref="SteamFriendsList"/> object containing a list of <see cref="SteamFriend"/> objects mapping to the Friend's list of the target user.</returns>
		public async static Task<SteamFriendsList> GetFriendsListAsync( SteamClient client, SteamID steamID ) {

			client.IsAuthorizedCall( new Type[] {
				typeof( Authenticators.UserAuthenticator ),
				typeof( Authenticators.APIKeyAuthenticator )
			} );

			SteamRequest request;
			if( client.Authenticator is Authenticators.UserAuthenticator )
				// ISteamUserOAuth provides a higher level of access (with User Authentication), assuming a personal relationship with the target user
				request = new SteamRequest( "ISteamUserOAuth", "GetFriendList", "v0001" );
			else
				request = new SteamRequest( "ISteamUser", "GetFriendList", "v0001" );

			request.AddParameter( "steamID", steamID );

			return VerifyAndDeserialize<SteamFriendsList>( ( await client.ExecuteAsync( request ) ) );

		}
		#endregion
		#endregion

		#region ISteamUserInterface interface
		#region GetPlayerSummaries/GetUserSummaries (and overload GetUser)
		/// <summary>
		/// (Requires <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/> or <see cref="SteamSharp.Authenticators.UserAuthenticator"/>)
		/// Returns basic profile information for a given 64-bit Steam ID.
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID to return profile information for.</param>
		/// <returns>
		///	Returns profile data for the requested user in the form of a <see cref="Player"/> object. 
		/// Some data associated with a Steam account may be hidden if the user has their profile visibility set to "Friends Only" or "Private". In that case, only public data will be returned.
		/// </returns>
		public static SteamUser GetUser( SteamClient client, SteamID steamID ) {
			try {
				return GetUserAsync( client, steamID ).Result;
			} catch( AggregateException e ) {
				if( e.InnerException != null )
					throw e.InnerException;
				throw e;
			}
		}

		/// <summary>
		/// (Requires <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/> or <see cref="SteamSharp.Authenticators.UserAuthenticator"/>)
		/// Returns basic profile information for a list of 64-bit Steam IDs.
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamIDs">List of 64 bit Steam IDs to return profile information for. Up to 100 Steam IDs can be requested.</param>
		/// <returns>
		///	Returns a large amount of profile data for the requested users in the form of a <see cref="Player"/> object. 
		/// Some data associated with a Steam account may be hidden if the user has their profile visibility set to "Friends Only" or "Private". In that case, only public data will be returned.
		/// </returns>
		public static List<SteamUser> GetUsers( SteamClient client, SteamID[] steamIDs ) {
			try {
				return GetUsersAsync( client, steamIDs ).Result;
			} catch( AggregateException e ) {
				if( e.InnerException != null )
					throw e.InnerException;
				throw e;
			}
		}

		/// <summary>
		/// (Async) (Requires <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/> or <see cref="SteamSharp.Authenticators.UserAuthenticator"/>)
		/// Returns basic profile information for a given 64-bit Steam ID.
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID to return profile information for.</param>
		/// <returns>
		///	Returns profile data for the requested user in the form of a <see cref="Player"/> object. 
		/// Some data associated with a Steam account may be hidden if the user has their profile visibility set to "Friends Only" or "Private". In that case, only public data will be returned.
		/// </returns>
		public async static Task<SteamUser> GetUserAsync( SteamClient client, SteamID steamID ) {

			client.IsAuthorizedCall( new Type[] {
				typeof( Authenticators.UserAuthenticator ),
				typeof( Authenticators.APIKeyAuthenticator )
			} );

			SteamRequest request;
			if( client.Authenticator is Authenticators.UserAuthenticator )
				// ISteamUserOAuth provides a higher level of access (with User Authentication), assuming a personal relationship with the target user
				request = new SteamRequest( "ISteamUserOAuth", "GetUserSummaries", "v0001" );
			else
				request = new SteamRequest( "ISteamUser", "GetPlayerSummaries", "v0002" );

			request.AddParameter( "steamids", steamID.ToString() );

			PlayerInfo player = VerifyAndDeserialize<GetPlayerSummariesResponse>( ( await client.ExecuteAsync( request ) ) ).Response.Players.FirstOrDefault();
			
			return new SteamUser() {
				SteamID = player.SteamID,
				PlayerInfo = player
			};

		}

		/// <summary>
		/// (Async) (Requires <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/> or <see cref="SteamSharp.Authenticators.UserAuthenticator"/>)
		/// Returns basic profile information for a list of 64-bit Steam IDs.
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamIDs">List of 64 bit Steam IDs to return profile information for. Up to 100 Steam IDs can be requested.</param>
		/// <returns>
		///	Returns a large amount of profile data for the requested users in the form of a <see cref="Player"/> object. 
		/// Some data associated with a Steam account may be hidden if the user has their profile visibility set to "Friends Only" or "Private". In that case, only public data will be returned.
		/// </returns>
		public async static Task<List<SteamUser>> GetUsersAsync( SteamClient client, SteamID[] steamIDs ) {

			client.IsAuthorizedCall( new Type[] {
				typeof( Authenticators.UserAuthenticator ),
				typeof( Authenticators.APIKeyAuthenticator )
			} );

			if( steamIDs.Length > 100 )
				throw new SteamRequestException( "You can specify a maximum of 100 SteamIDs per call." );

			SteamRequest request;
			if( client.Authenticator is Authenticators.UserAuthenticator )
				// ISteamUserOAuth provides a higher level of access (with User Authentication), assuming a personal relationship with the target user
				request = new SteamRequest( "ISteamUserOAuth", "GetUserSummaries", "v0001" );
			else
				request = new SteamRequest( "ISteamUser", "GetPlayerSummaries", "v0002" );

			request.AddParameter( "steamids", String.Join( ",", steamIDs.ToString() ) );

			List<PlayerInfo> players = VerifyAndDeserialize<GetPlayerSummariesResponse>( ( await client.ExecuteAsync( request ) ) ).Response.Players;

			List<SteamUser> users = new List<SteamUser>();

			foreach( var player in players ) {
				users.Add( new SteamUser {
					SteamID = player.SteamID,
					PlayerInfo = player
				} );
			}

			return users;

		}
		#endregion

		
		#endregion

	}

}
