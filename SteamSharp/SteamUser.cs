using System;
using System.Collections.Generic;
using System.Linq;
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
		public string AccessToken { get; set; }


	}

}
