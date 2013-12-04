
namespace SteamSharp.Authenticators {

	/// <summary>
	/// Authenticatior allowing access to Steam resources protected by an API key.
	/// </summary>
	public class APIKeyAuthenticator : ISteamAuthenticator {

		internal string ApiKey { get; set; }

		/// <summary>
		/// Invoke method to initialize the authenticator (which should then be added to the Authenticator property of a <see cref="SteamClient"/> instance).
		/// </summary>
		/// <param name="apiKey">Steam API Key (available at http://steamcommunity.com/dev/apikey).</param>
		/// <returns><see cref="APIKeyAuthenticator"/> object for authentication of a <see cref="SteamClient"/> instance.</returns>
		public static APIKeyAuthenticator ForProtectedResource( string apiKey ) {

			return new APIKeyAuthenticator {
				ApiKey = apiKey
			};

		}

		/// <summary>
		/// Method invoked by the library in order to authenticate for a resource.
		/// Should not be called directly by consumer code.
		/// </summary>
		/// <param name="client">SteamClient instance to authenticate.</param>
		/// <param name="request">Request requiring authentication.</param>
		public void Authenticate( SteamClient client, ISteamRequest request ) {
			request.AddParameter( "key", ApiKey, ParameterType.QueryString );
		}

	}

}