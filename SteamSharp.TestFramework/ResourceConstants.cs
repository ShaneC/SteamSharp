using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp.TestFramework {

	public class ResourceConstants {

		/// <summary>
		/// This should be the Steam API Key for use with testing
		/// </summary>
		public const string AccessToken = "8675309";

		/// <summary>
		/// Address of the simulated web server for handling response call tests.
		/// Only time likely to need a change is if there is an active service on the same port.
		/// </summary>
		public const string SimulatedServerUrl = "http://localhost:8080/";

	}

}
