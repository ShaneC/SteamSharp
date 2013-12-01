
namespace SteamSharp.Authenticators {

	public class APIKeyAuthenticator : ISteamAuthenticator {

		internal string ApiKey { get; set; }

		public static APIKeyAuthenticator ForProtectedResource( string apiKey ) {

			return new APIKeyAuthenticator {
				ApiKey = apiKey
			};

		}

		public void Authenticate( SteamClient client, ISteamRequest request ) {
			request.AddParameter( "key", ApiKey, ParameterType.QueryString );
		}

	}

}