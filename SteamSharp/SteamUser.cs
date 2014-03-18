using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

	/// <summary>
	/// Object representing a Steam User. This class includes authentication data for use in authorizing a user.
	/// </summary>
	public class SteamUser : IComparable<SteamUser> {

		/// <summary>
		/// User's SteamID.
		/// </summary>
		public SteamID SteamID { get; set; }

		/// <summary>
		/// Obtained via <see cref="UserAuthenticator"/>. Access Token to be passed on the Steam API for requests requiring first part authentication.
		/// </summary>
		public string OAuthAccessToken { get; set; }

		/// <summary>
		/// Obtained via <see cref="AuthCookieAuthenticator"/>. Transfer Token to be passed on Steam Store transactions.
		/// </summary>
		public string TransferToken { get; set; }

		/// <summary>
		/// Obtained via <see cref="AuthCookieAuthenticator"/>. Cookie which contains the authentication token for the user.
		/// </summary>
		public Cookie AuthCookie { get; set; }

		/// <summary>
		/// Obtained via <see cref="AuthCookieAuthenticator"/>. Login Key obtained from the Authentication Cookie.
		/// </summary>
		public string AuthCookieLoginKey { get; set; }

		/// <summary>
		/// Community and profile information for the given Steam User.
		/// </summary>
		public SteamCommunity.PlayerInfo PlayerInfo { get; set; }

		/// <summary>
		/// Returns true if the user object is capable of UserAuthentication to OAuth APIs. False otherwise.
		/// </summary>
		/// <returns></returns>
		public bool IsAuthenticated() {
			return ( !String.IsNullOrEmpty( this.OAuthAccessToken ) );
		}

		/// <summary>
		/// (Async) If the user object is User Authenticated (IsAuthenticated() == true), creates a SteamClient and pulls the latest PlayerInfo data for the current user.
		/// Throws <see cref="SteamSharp.SteamAuthenticationException"/> if the user is not authenticated or is no longer able to authenticate.
		/// </summary>
		public async Task GetProfileDataAsync() {
			if( !this.IsAuthenticated() )
				throw new SteamAuthenticationException( "User object is not capable of authenticating with the UserAuthenticator object." );
			SteamClient client = new SteamClient();
			client.Authenticator = Authenticators.UserAuthenticator.ForProtectedResource( this.OAuthAccessToken );
			await GetProfileDataAsync( client );
		}

		/// <summary>
		/// (Async) (Requires client with <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/> or <see cref="SteamSharp.Authenticators.UserAuthenticator"/>)
		/// Pulls the latest PlayerInfo data for the current user from the Steam service.
		/// </summary>
		/// <param name="client">SteamClient for use in executing the request.</param>
		public async Task GetProfileDataAsync( SteamClient client ) {
			var freshUser = await SteamCommunity.GetUserAsync( client, this.SteamID );
			this.PlayerInfo = freshUser.PlayerInfo;
		}

		/// <summary>
		/// Sorts of a list of <see cref="SteamUser"/> into alphabetical order based on their Persona Name (display name).
		/// </summary>
		/// <param name="target">Target for comparison</param>
		/// <returns>Alphabetical order sorted by PersonaName.</returns>
		public int CompareTo( SteamUser target ) {
			if( target == null )
				return 1;
			return this.PlayerInfo.PersonaName.CompareTo( target.PlayerInfo.PersonaName );
		}

	}

}
