using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

	/// <summary>
	/// Object representing a Steam ID.
	/// </summary>
	public class SteamID {

		private string _steamID;

		/// <summary>
		/// Initialize SteamID from string.
		/// </summary>
		/// <param name="steamID"></param>
		public SteamID( string steamID ) {
			_steamID = steamID;
		}

		public override string ToString() {
			return _steamID;
		}

	}

}
