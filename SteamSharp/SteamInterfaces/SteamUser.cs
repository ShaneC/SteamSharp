using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {
	
	public partial class SteamUser : SteamInterface {

		/// <summary>
		/// (Requires <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/>) Returns basic profile information for a given 64-bit Steam ID.
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
		/// (Requires <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/>) Returns basic profile information for a list of 64-bit Steam IDs.
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
		/// (Requires <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/>) (Async) Returns basic profile information for a given 64-bit Steam ID.
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
		/// (Requires <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/>) (Async) Returns basic profile information for a list of 64-bit Steam IDs.
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

	}

}
