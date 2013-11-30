using System;

namespace SteamSharp.Authenticators {

	public class APIKeyAuthenticator : ISteamAuthenticator {

		internal long ApiKey { get; set; }

		public static APIKeyAuthenticator ForProtectedResource( long apiKey ) {

			return new APIKeyAuthenticator {
				ApiKey = apiKey
			};

		}

		public static APIKeyAuthenticator ForProtectedResource( string apiKey ) {
			try {
				return ForProtectedResource( long.Parse( apiKey ) );
			} catch( Exception e ) {
				throw new SteamAuthenticationException( "Specified apiKey does not conform to a valid format (must be parseable to long).", e );
			}
		}

		public void Authenticate( SteamClient client, ISteamRequest request ) {
			request.AddParameter( "key", ApiKey, ParameterType.QueryString );
		}

	}

}