using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

	/// <summary>
	/// Class allowing for abstracted querying of the ISteamNews interface
	/// </summary>
	public partial class SteamNews {

		/// <summary>
		/// Returns the latest of a game specified by its AppID.
		/// </summary>
		/// <param name="client">(optional) <see cref="SteamClient"/> instance to use.</param>
		/// <param name="appID">AppID of the game you want the news of.</param>
		/// <param name="count">How many news enties you want to get returned.</param>
		/// <param name="maxLength">Maximum length of each news entry.</param>
		/// <returns></returns>
		public static AppNews GetNewsForApp( SteamClient client, int appID, int count, int maxLength ) {
			return GetNewsForAppAsync( client, appID, count, maxLength ).Result;
		}

		/// <summary>
		/// Asynchronously returns the latest of a game specified by its AppID.
		/// </summary>
		/// <param name="client">(optional) <see cref="SteamClient"/> instance to use.</param>
		/// <param name="appID">AppID of the game you want the news of.</param>
		/// <param name="count">How many news enties you want to get returned.</param>
		/// <param name="maxLength">Maximum length of each news entry.</param>
		/// <returns></returns>
		public async static Task<AppNews> GetNewsForAppAsync( SteamClient client, int appID, int count, int maxLength ) {

			SteamRequest request = new SteamRequest( SteamInterface.ISteamNews, "GetNewsForApp", SteamMethodVersion.v0002 );
			request.AddParameter( "appid", appID );
			request.AddParameter( "count", count );
			request.AddParameter( "maxlength", maxLength );

			AppNewsResponse responseObj = JsonConvert.DeserializeObject<AppNewsResponse>( ( await client.ExecuteAsync( request ) ).Content );

			return responseObj.appnews;

		}

	}

}
