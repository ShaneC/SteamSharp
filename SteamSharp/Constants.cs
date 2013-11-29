using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

	/// <summary>
	/// Available Steam Web APIs
	/// </summary>
	public enum SteamInterface {
		ISteamNews,
		ISteamUserStats,
		ISteamUser
	}

	/// <summary>
	/// Possible values for API version. NOTE: Not all APIs contain all versions listed.
	/// </summary>
	public enum SteamMethodVersion {
		v0001,
		v0002,
		v0003,
		v0004,
		v0005,
		v0006,
		v0007,
		v0008,
		v0009,
		v0010
	}

	/// <summary>
	/// HTTP Response States
	/// </summary>
	public enum ResponseStatus {
		None,
		Completed,
		Error,
		TimedOut,
		Aborted
	}

	///<summary>
	/// The valid types of parameters that can be added to requests
	///</summary>
	public enum ParameterType {
		GetOrPost,
		UrlSegment,
		HttpHeader,
		RequestBody,
		QueryString
	}

}
