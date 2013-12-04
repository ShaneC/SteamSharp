
namespace SteamSharp {

	public partial class SteamClient {

		/// <summary>
		/// Executes the provided request synchronously, returning the response object.
		/// </summary>
		/// <param name="request"><see cref="ISteamRequest"/> object for execution.</param>
		/// <returns><see cref="ISteamResponse"/> object containing the result of the request.</returns>
		public ISteamResponse Execute( ISteamRequest request ) {
			return ExecuteAsync( request ).Result;
		}

	}

}
