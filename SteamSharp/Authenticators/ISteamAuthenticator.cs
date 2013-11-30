
namespace SteamSharp.Authenticators {

	public interface ISteamAuthenticator {

		void Authenticate( SteamClient client, ISteamRequest request );

	}

}
