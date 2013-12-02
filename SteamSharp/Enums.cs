
namespace SteamSharp {

	/// <summary>
	/// Available Steam Web APIs
	/// </summary>
	public enum SteamAPIInterface {
		Unknown,
		ISteamNews,
		ISteamUserStats,
		ISteamUser
	}

	/// <summary>
	/// Possible values for API version. NOTE: Not all APIs contain all versions listed.
	/// </summary>
	public enum SteamMethodVersion {
		Unknown,
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

	/// <summary>
	/// Data format for SteamRequests.
	/// Raw will be sent without Content-Type and without serialization.
	/// Json will be serialized and sent via Content-Type application/json.
	/// </summary>
	public enum PostDataFormat {
		Raw,
		Json
	}

	#region Interface Specific Enums
	/// <summary>
	/// Relationship filter for profile/friend's list filtering. Possible values: All, Friend
	/// </summary>
	public enum PlayerRelationshipType {
		All,
		Friend
	}

	/// <summary>
	/// Certain Steam APIs allow a language to be provided. In those instances, use this enum in order to specify the return language.
	/// The languages the Steam API supports are limited.
	/// </summary>
	public enum RequestedLangage {
		English,
		Spanish,
		Swedish,
		Russian,
		Portuguese,
		German,
		Dutch,
		Japanese
	}
	#endregion

}
