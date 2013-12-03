
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

	/* Interface Specific Enums */
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

	/// <summary>
	/// The user's current status.
	/// If the player's profile is private, this will always be "Offline", except if the user has set their status to looking to trade or looking to play (due to a bug, not long term behavior!).
	/// </summary>
	public enum PersonaState {
		Offline = 0,
		Online = 1,
		Busy = 2,
		Away = 3,
		Snooze = 4,
		LookingToTrade = 5,
		LookingToPlay = 6
	}

	/// <summary>
	/// This represents whether the profile is visible or not, and if it is visible, why you are allowed to see it.
	/// </summary>
	public enum CommunityVisibilityState {
		Private = 1,
		Public = 3
	}
	#endregion

}
