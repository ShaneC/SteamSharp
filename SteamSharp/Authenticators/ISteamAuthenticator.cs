
namespace SteamSharp.Authenticators {

	/// <summary>
	/// Interface for classes used in authenticating against the Steam API.
	/// </summary>
	public interface ISteamAuthenticator {

		/// <summary>
		/// Method invoked by the library in order to authenticate for a resource.
		/// Should not be called directly by consumer code.
		/// </summary>
		/// <param name="client">SteamClient instance to authenticate.</param>
		/// <param name="request">Request requiring authentication.</param>
		void Authenticate( SteamClient client, ISteamRequest request );

	}

}
