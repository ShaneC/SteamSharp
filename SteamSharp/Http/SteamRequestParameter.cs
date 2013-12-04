using System;

namespace SteamSharp {

	/// <summary>
	/// Containing object for data intended to be sent as request parameters.
	/// </summary>
	public class SteamRequestParameter {

		/// <summary>
		/// Parameter's name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Parameter's value
		/// </summary>
		public object Value { get; set; }

		/// <summary>
		/// Type of the parameter
		/// </summary>
		public ParameterType Type { get; set; }

		/// <summary>
		/// Return a query-string formatted representation of this parameter
		/// </summary>
		/// <returns>String</returns>
		public override string ToString() {
			return String.Format( "{0}={1}", Name, Value );
		}

	}

}
