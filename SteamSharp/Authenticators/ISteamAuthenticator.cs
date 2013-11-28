
namespace SteamSharp {

	public interface ISteamAuthenticator {

		void Authenticate( SteamClient client, ISteamRequest request );

	}

}
