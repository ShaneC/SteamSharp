using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamSharp {

	/// <summary>
	/// Class allowing for abstracted querying of the ISteamUserOAuth interface
	/// </summary>
	public partial class SteamUserOAuth : SteamInterface {

		#region SteamUserOAuth
		/// <summary>
		/// Gets the Friend's list of a target user.
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID of user to return friend's list for.</param>
		/// <returns>Friends list for the target user.</returns>
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
		/// Gets the Friend's list of a target user.
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID of user to return friend's list for.</param>
		/// <returns>Friends list for the target user.</returns>
		public async static Task<SteamFriendsList> GetFriendsListAsync( SteamClient client, SteamID steamID ) {

			client.IsAuthorizedCall( typeof( Authenticators.UserAuthenticator ) );

			SteamRequest request = new SteamRequest( SteamAPIInterface.ISteamUserOAuth, "GetFriendList", SteamMethodVersion.v0001 );
			request.AddParameter( "steamID", steamID );

			return VerifyAndDeserialize<SteamFriendsList>( ( await client.ExecuteAsync( request ) ) );

		}
		#endregion

	}

}
