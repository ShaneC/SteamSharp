using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

	/// <summary>
	/// Class allowing for abstracted querying of the ISteamNews interface
	/// </summary>
	public class SteamNews {

		/// <summary>
		/// Returns the latest of a game specified by its AppID.
		/// </summary>
		/// <param name="appID">AppID of the game you want the news of.</param>
		/// <param name="count">How many news enties you want to get returned.</param>
		/// <param name="maxLength">Maximum length of each news entry.</param>
		/// <param name="client">(optional) <see cref="SteamClient"/> instance to use. If none specified, a new instance will be created.</param>
		/// <returns></returns>
		public static ISteamResponse GetNewsForApp( int appID, int count, int maxLength, SteamClient client = null ) {
			return GetNewsForAppAsync( appID, count, maxLength, client ).Result;
		}

		/// <summary>
		/// Asynchronously returns the latest of a game specified by its AppID.
		/// </summary>
		/// <param name="appID">AppID of the game you want the news of.</param>
		/// <param name="count">How many news enties you want to get returned.</param>
		/// <param name="maxLength">Maximum length of each news entry.</param>
		/// <param name="client">(optional) <see cref="SteamClient"/> instance to use. If none specified, a new instance will be created.</param>
		/// <returns></returns>
		public async static Task<ISteamResponse> GetNewsForAppAsync( int appID, int count, int maxLength, SteamClient client = null ) {

			SteamRequest request = new SteamRequest( SteamInterface.ISteamNews, "GetNewsForApp", SteamMethodVersion.v0002 );
			request.AddParameter( "appid", appID );
			request.AddParameter( "count", count );
			request.AddParameter( "maxlength", maxLength );

			var steamClient = ( client == null ) ? new SteamClient() : client;
			return await steamClient.ExecuteAsync( request );

		}

	}

}
