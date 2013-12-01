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
		public const string AccessToken = "";

		/// <summary>
		/// Address of the simulated web server for handling response call tests.
		/// Only time you will likely to need a change this is if there is an active local service on the same port.
		/// </summary>
		public const string SimulatedServerUrl = "http://localhost:8080/";

	}

}
