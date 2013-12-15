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
	public class SteamUser {

		/// <summary>
		/// User's SteamID.
		/// </summary>
		public SteamID SteamID { get; set; }

		/// <summary>
		/// Access Token to be passed on the Steam API for requests requiring first part authentication.
		/// </summary>
		public string TransferToken { get; set; }

		/// <summary>
		/// Cookie which contains the authentication token for the user.
		/// </summary>
		public Cookie AuthCookie { get; set; }

	}

}
