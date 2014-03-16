
namespace SteamSharp {

	/// <summary>
	/// Available Steam Web APIs
	/// </summary>
	public enum SteamAPIInterface {
		/// <summary>
		/// Unknown Steam interface.
		/// </summary>
		Unknown,
		/// <summary>
		/// ISteamNews interface.
		/// </summary>
		ISteamNews,
		/// <summary>
		/// ISteamUserStats interface.
		/// </summary>
		ISteamUserStats,
		/// <summary>
		/// ISteamUser interface.
		/// </summary>
		ISteamUser,
		/// <summary>
		/// IPlayerService interface.
		/// </summary>
		IPlayerService,
		/// <summary>
		/// ISteamUserOAuth interface.
		/// </summary>
		ISteamUserOAuth
	}

	/// <summary>
	/// Possible values for API version. NOTE: Not all APIs contain all versions listed.
	/// </summary>
	public enum SteamMethodVersion {
		/// <summary>
		/// Unknown version of the Method API.
		/// </summary>
		Unknown,
		/// <summary>
		/// Version 1 of the Steam API's method.
		/// </summary>
		v0001,
		/// <summary>
		/// Version 2 of the Steam API's method.
		/// </summary>
		v0002,
		/// <summary>
		/// Version 3 of the Steam API's method.
		/// </summary>
		v0003,
		/// <summary>
		/// Version 4 of the Steam API's method.
		/// </summary>
		v0004,
		/// <summary>
		/// Version 5 of the Steam API's method.
		/// </summary>
		v0005,
		/// <summary>
		/// Version 6 of the Steam API's method.
		/// </summary>
		v0006,
		/// <summary>
		/// Version 7 of the Steam API's method.
		/// </summary>
		v0007,
		/// <summary>
		/// Version 8 of the Steam API's method.
		/// </summary>
		v0008,
		/// <summary>
		/// Version 9 of the Steam API's method.
		/// </summary>
		v0009,
		/// <summary>
		/// Version 10 of the Steam API's method.
		/// </summary>
		v0010
	}

	/// <summary>
	/// HTTP Response States
	/// </summary>
	public enum ResponseStatus {
		/// <summary>
		/// No response has been received from Steam.
		/// </summary>
		None,
		/// <summary>
		/// The transaction with the Steam API has been completed, full response text is received.
		/// </summary>
		Completed,
		/// <summary>
		/// The transaction with the Steam API completed but resulted in an error state.
		/// </summary>
		Error,
		/// <summary>
		/// The transaction with the Steam API timed out before it could be completed.
		/// </summary>
		TimedOut,
		/// <summary>
		/// The request to the Steam API was aborted before it could be completed.
		/// </summary>
		Aborted
	}

	///<summary>
	/// The valid types of parameters that can be added to requests
	///</summary>
	public enum ParameterType {
		/// <summary>
		/// Parameter to add to either the URI or request body, depending on which <see cref="System.Net.Http.HttpMethod"/> is invoked.
		/// </summary>
		GetOrPost,
		/// <summary>
		/// Specifies the parameter to be a find-and-replace section of the URI's query string (i.e. /resource/{foo} --> /resource/bar).
		/// </summary>
		UrlSegment,
		/// <summary>
		/// Indicates the parameter is intended to be added as an HTTP header on the request.
		/// </summary>
		HttpHeader,
		/// <summary>
		/// Specifies this data should be added to the body of the POST message. If the HTTPMethod is chosen to be GET, this data will be dropped.
		/// </summary>
		RequestBody,
		/// <summary>
		/// Indicates the parameter should be added to the query string, regardless of <see cref="System.Net.Http.HttpMethod"/> type.
		/// </summary>
		QueryString
	}

	/// <summary>
	/// Data format for SteamRequests.
	/// Raw will be sent without Content-Type and without serialization.
	/// Json will be serialized and sent via Content-Type application/json.
	/// </summary>
	public enum PostDataFormat {
		/// <summary>
		/// Will not serialize the request into any format. Data added to the body will be sent as raw text, and will be sent without a Content-Type header.
		/// </summary>
		Raw,
		/// <summary>
		/// Content-Type: application/x-www-form-urlencoded
		/// </summary>
		FormUrlEncoded,
		/// <summary>
		/// Data added to the POST body will be serialized into JSON and the Content-Type of application/json will be added to the request.
		/// </summary>
		Json
	}

	/// <summary>
	/// Steam Universe identifiers.
	/// </summary>
	public enum SteamUniverse {
		/// <summary>
		/// Unknown or invalid universe.
		/// </summary>
		Unknown,
		/// <summary>
		/// Public universe.
		/// </summary>
		Public,
		/// <summary>
		/// Beta universe.
		/// </summary>
		Beta,
		/// <summary>
		/// Internal universe.
		/// </summary>
		Internal,
		/// <summary>
		/// Dev universe.
		/// </summary>
		Dev,
		/// <summary>
		/// RC universe.
		/// </summary>
		RC
	}

	/* Interface Specific Enums */
	#region Interface Specific Enums
	/// <summary>
	/// Relationship filter for profile/friend's list filtering. Possible values: All, Friend
	/// </summary>
	public enum PlayerRelationshipType {
		/// <summary>
		/// Filter to retrieve all users from a user's profile, regardless of friendship affiliation.
		/// </summary>
		All,
		/// <summary>
		/// Filter to retrieve only friends of a particular user from their profile.
		/// </summary>
		Friend
	}

	/// <summary>
	/// Certain Steam APIs allow a language to be provided. In those instances, use this enum in order to specify the return language.
	/// The languages the Steam API supports are limited.
	/// </summary>
	public enum RequestedLangage {
		/// <summary>
		/// English language.
		/// </summary>
		English,
		/// <summary>
		/// Spanish language.
		/// </summary>
		Spanish,
		/// <summary>
		/// Swedish language.
		/// </summary>
		Swedish,
		/// <summary>
		/// Russian language.
		/// </summary>
		Russian,
		/// <summary>
		/// Portuguese language.
		/// </summary>
		Portuguese,
		/// <summary>
		/// German language.
		/// </summary>
		German,
		/// <summary>
		/// Dutch language.
		/// </summary>
		Dutch,
		/// <summary>
		/// Japanese language.
		/// </summary>
		Japanese
	}

	/// <summary>
	/// The user's current status.
	/// If the player's profile is private, this will always be "Offline", except if the user has set their status to looking to trade or looking to play (due to a bug, not long term behavior!).
	/// </summary>
	public enum PersonaState {
		/// <summary>
		/// User is offline.
		/// </summary>
		Offline = 0,
		/// <summary>
		/// User is online.
		/// </summary>
		Online = 1,
		/// <summary>
		/// User is online but busy.
		/// </summary>
		Busy = 2,
		/// <summary>
		/// User is only but away.
		/// </summary>
		Away = 3,
		/// <summary>
		/// User is snoozing.
		/// </summary>
		Snooze = 4,
		/// <summary>
		/// User is online and looking to trade.
		/// </summary>
		LookingToTrade = 5,
		/// <summary>
		/// User is online and looking to play.
		/// </summary>
		LookingToPlay = 6
	}

	/// <summary>
	/// This represents whether the profile is visible or not, and if it is visible, why you are allowed to see it.
	/// </summary>
	public enum CommunityVisibilityState {
		/// <summary>
		/// User's profile is private.
		/// </summary>
		Private = 1,
		/// <summary>
		/// User's profile is public.
		/// </summary>
		Public = 3
	}
	#endregion

}
