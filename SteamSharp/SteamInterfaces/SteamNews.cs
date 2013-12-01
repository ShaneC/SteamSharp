using System.Threading.Tasks;

namespace SteamSharp {

	/// <summary>
	/// Class allowing for abstracted querying of the ISteamNews interface
	/// </summary>
	public static class SteamNews {

		/// <summary>
		/// Returns the latest of a game specified by its AppID.
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// </summary>
		/// <param name="client">(optional) <see cref="SteamClient"/> instance to use.</param>
		/// <param name="appID">AppID of the game you want the news of.</param>
		/// <param name="count">How many news enties you want to get returned.</param>
		/// <param name="maxLength">Maximum length of each news entry.</param>
		/// <returns>An <see cref="AppNews"/> object.</returns>
		public static SteamNewsModel.AppNews GetNewsForApp( this SteamClient client, int appID, int count, int maxLength ) {
			return GetNewsForAppAsync( client, appID, count, maxLength ).Result;
		}

		/// <summary>
		/// Asynchronously returns the latest of a game specified by its AppID.
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// </summary>
		/// <param name="client">(optional) <see cref="SteamClient"/> instance to use.</param>
		/// <param name="appID">AppID of the game you want the news of.</param>
		/// <param name="count">How many news enties you want to get returned.</param>
		/// <param name="maxLength">Maximum length of each news entry.</param>
		/// <returns>An <see cref="AppNews"/> object.</returns>
		public async static Task<SteamNewsModel.AppNews> GetNewsForAppAsync( this SteamClient client, int appID, int count, int maxLength ) {

			SteamRequest request = new SteamRequest( SteamAPIInterface.ISteamNews, "GetNewsForApp", SteamMethodVersion.v0002 );
			request.AddParameter( "appid", appID );
			request.AddParameter( "count", count );
			request.AddParameter( "maxlength", maxLength );

			return SteamInterfaceHelper.VerifyAndDeserialize<SteamNewsModel.AppNewsResponse>( ( await client.ExecuteAsync( request ) ) ).appnews;

		}

		

	}

}
