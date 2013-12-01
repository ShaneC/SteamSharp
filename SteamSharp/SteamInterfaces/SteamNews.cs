using System.Threading.Tasks;

namespace SteamSharp {

	/// <summary>
	/// Class allowing for abstracted querying of the ISteamNews interface
	/// </summary>
	public partial class SteamNews : SteamInterface {

		/// <summary>
		/// Returns the latest of a game specified by its AppID.
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetNewsForApp_.28v0002.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="appID">AppID of the game you want the news of.</param>
		/// <param name="count">How many news enties you want to get returned.</param>
		/// <param name="maxLength">Maximum length of each news entry.</param>
		/// <returns>An <see cref="AppNews"/> object.</returns>
		public static AppNews GetNewsForApp( SteamClient client, int appID, int count, int maxLength ) {
			return GetNewsForAppAsync( client, appID, count, maxLength ).Result;
		}

		/// <summary>
		/// (Asynchronous) Returns the latest of a game specified by its AppID.
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetNewsForApp_.28v0002.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="appID">AppID of the game you want the news of.</param>
		/// <param name="count">How many news enties you want to get returned.</param>
		/// <param name="maxLength">Maximum length of each news entry.</param>
		/// <returns>An <see cref="AppNews"/> object.</returns>
		public async static Task<AppNews> GetNewsForAppAsync( SteamClient client, int appID, int count, int maxLength ) {

			SteamRequest request = new SteamRequest( SteamAPIInterface.ISteamNews, "GetNewsForApp", SteamMethodVersion.v0002 );
			request.AddParameter( "appid", appID );
			request.AddParameter( "count", count );
			request.AddParameter( "maxlength", maxLength );

			return VerifyAndDeserialize<AppNewsResponse>( ( await client.ExecuteAsync( request ) ) ).appnews;

		}

	}

}
