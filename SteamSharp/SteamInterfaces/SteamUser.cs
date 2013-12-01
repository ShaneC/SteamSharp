using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {
	
	public partial class SteamUser : SteamInterface {

		#region GetPlayerSummaries (and overload GetPlayerSummary)
		/// <summary>
		/// (Requires <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/>)
		/// Returns basic profile information for a given 64-bit Steam ID.
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetPlayerSummaries_.28v0002.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamIDs">SteamID to return profile information for.</param>
		/// <returns>
		///	Returns profile data for the requested user in the form of a <see cref="Player"/> object. 
		/// Some data associated with a Steam account may be hidden if the user has their profile visibility set to "Friends Only" or "Private". In that case, only public data will be returned.
		/// </returns>
		public static Player GetPlayerSummary( SteamClient client, string steamID ) {
			try {
				return GetPlayerSummaryAsync( client, steamID ).Result;
			} catch( AggregateException e ) {
				if( e.InnerException != null )
					throw e.InnerException;
				throw e;
			}
		}

		/// <summary>
		/// (Requires <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/>)
		/// Returns basic profile information for a list of 64-bit Steam IDs.
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetPlayerSummaries_.28v0002.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamIDs">List of 64 bit Steam IDs to return profile information for. Up to 100 Steam IDs can be requested.</param>
		/// <returns>
		///	Returns a large amount of profile data for the requested users in the form of a <see cref="Player"/> object. 
		/// Some data associated with a Steam account may be hidden if the user has their profile visibility set to "Friends Only" or "Private". In that case, only public data will be returned.
		/// </returns>
		public static List<Player> GetPlayerSummaries( SteamClient client, string[] steamIDs ) {
			try {
				return GetPlayerSummariesAsync( client, steamIDs ).Result;
			} catch( AggregateException e ) {
				if( e.InnerException != null )
					throw e.InnerException;
				throw e;
			}
		}

		/// <summary>
		/// (Requires <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/>)
		/// (Async) Returns basic profile information for a given 64-bit Steam ID.
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetPlayerSummaries_.28v0002.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamIDs">SteamID to return profile information for.</param>
		/// <returns>
		///	Returns profile data for the requested user in the form of a <see cref="Player"/> object. 
		/// Some data associated with a Steam account may be hidden if the user has their profile visibility set to "Friends Only" or "Private". In that case, only public data will be returned.
		/// </returns>
		public async static Task<Player> GetPlayerSummaryAsync( SteamClient client, string steamID ) {

			SteamRequest request = new SteamRequest( SteamAPIInterface.ISteamUser, "GetPlayerSummaries", SteamMethodVersion.v0002 );
			request.AddParameter( "steamids", steamID );

			return VerifyAndDeserialize<GetPlayerSummariesResponse>( ( await client.ExecuteAsync( request ) ) ).response.players.FirstOrDefault();

		}

		/// <summary>
		/// (Requires <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/>)
		/// (Async) Returns basic profile information for a list of 64-bit Steam IDs.
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetPlayerSummaries_.28v0002.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamIDs">List of 64 bit Steam IDs to return profile information for. Up to 100 Steam IDs can be requested.</param>
		/// <returns>
		///	Returns a large amount of profile data for the requested users in the form of a <see cref="Player"/> object. 
		/// Some data associated with a Steam account may be hidden if the user has their profile visibility set to "Friends Only" or "Private". In that case, only public data will be returned.
		/// </returns>
		public async static Task<List<Player>> GetPlayerSummariesAsync( SteamClient client, string[] steamIDs ) {

			if( steamIDs.Length > 100 )
				throw new SteamRequestException( "You can specify a maximum of 100 SteamIDs per call." );

			SteamRequest request = new SteamRequest( SteamAPIInterface.ISteamUser, "GetPlayerSummaries", SteamMethodVersion.v0002 );
			request.AddParameter( "steamids", String.Join( ",", steamIDs ) );

			return VerifyAndDeserialize<GetPlayerSummariesResponse>( ( await client.ExecuteAsync( request ) ) ).response.players;

		}
		#endregion

		#region GetFriendList
		/// <summary>
		/// (Requires <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/>)
		/// Returns the friend list of any Steam user, provided the user's Steam Community profile visibility is set to "Public."
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetFriendList_.28v0001.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID to return friend's list for.</param>
		/// <param name="relationship">Relationship filter. Possibles values: all, friend.</param>
		/// <returns>List of <see cref="Friend"/> objects mapping to the Friend's list of the target user.</returns>
		public static List<Friend> GetFriendList( SteamClient client, string steamID, PlayerRelationshipType relationship ) {
			try {
				return GetFriendListAsync( client, steamID, relationship ).Result;
			} catch( AggregateException e ) {
				if( e.InnerException != null )
					throw e.InnerException;
				throw e;
			}
		}

		/// <summary>
		/// (Requires <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/>)
		/// (Async) Returns the friend list of any Steam user, provided the user's Steam Community profile visibility is set to "Public."
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetFriendList_.28v0001.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID to return friend's list for.</param>
		/// <param name="relationship">Relationship filter. Possibles values: all, friend.</param>
		/// <returns>List of <see cref="Friend"/> objects mapping to the Friend's list of the target user.</returns>
		public async static Task<List<Friend>> GetFriendListAsync( SteamClient client, string steamID, PlayerRelationshipType relationship ) {

			SteamRequest request = new SteamRequest( SteamAPIInterface.ISteamUser, "GetFriendList", SteamMethodVersion.v0001 );

			string relationshipType = "";
			switch( relationship ) {
				case PlayerRelationshipType.Friend :
					relationshipType = "friend";
					break;
				default :
					relationshipType = "all";
					break;
			}

			request.AddParameter( "steamid", steamID );
			request.AddParameter( "relationship", relationshipType );

			return VerifyAndDeserialize<GetFriendsListResponse>( ( await client.ExecuteAsync( request ) ) ).friendslist.friends;

		}
		#endregion

		#region Interface Specific Enums
		/// <summary>
		/// Relationship filter for profile/friend's list filtering. Possible values: All, Friend
		/// </summary>
		public enum PlayerRelationshipType {
			All,
			Friend
		}
		#endregion

	}

}
