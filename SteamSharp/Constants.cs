using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

	/// <summary>
	/// Standard HTTP methods used for making requests to the Steam API
	/// </summary>
	public enum HttpMethod {
		GET,
		POST,
		PUT,
		DELETE,
		HEAD,
		OPTIONS,
		PATCH
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
