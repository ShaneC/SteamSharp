using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

	public class SteamRequestException : Exception {

		public bool IsDeserializationIssue { get; private set; }
	
	}

}
