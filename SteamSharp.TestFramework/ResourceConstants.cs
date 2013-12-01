
namespace SteamSharp.TestFramework {

	public class ResourceConstants {

		/// <summary>
		/// This should be the Steam API Key for use with testing
		/// </summary>
		public const string AccessToken = "";

		/// <summary>
		/// Address of the simulated web server for handling response call tests.
		/// The only time you will likely need to change this is if there is an active local service on the same port.
		/// </summary>
		public const string SimulatedServerUrl = "http://localhost:8080/";

	}

}
