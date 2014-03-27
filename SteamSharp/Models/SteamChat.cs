using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SteamSharp {

	/// <summary>
	/// Specified user's Friend's List.
	/// </summary>
	public class SteamFriendsList {

		/// <summary>
		/// List of Steam Friends for specified user.
		/// </summary>
		public Dictionary<SteamID, SteamUser> Friends { get; set; }

		public int GetFriendCount() {
			return ( ( Friends == null ) ? 0 : Friends.Count );
		}

	}

	internal class SteamFriendsListResponse {

		/// <summary>
		/// List of Steam Friends for specified user. These are not user objects, but simply a directory of friends.
		/// </summary>
		public List<SteamFriend> Friends { get; set; }

	}

	/// <summary>
	/// Object representing a single Friend of the specified user.
	/// Additional information can be pulled on the users by making a GetPlayerSummaries call with all SteamIDs contained in the List enumeration.
	/// </summary>
	public class SteamFriend {

		/// <summary>
		/// 64bit SteamID of the user.
		/// </summary>
		[JsonConverter( typeof( SteamInterfaceHelpers.SteamIDConverter ) )]
		public SteamID SteamID { get; set; }

		/// <summary>
		/// Length of time this user has been a friend
		/// </summary>
		[JsonProperty( "friend_since" )]
		[JsonConverter( typeof( SteamInterfaceHelpers.UnixDateTimeConverter ) )]
		public DateTime FriendSince { get; set; }

		/// <summary>
		/// DateTime of when the relationship was created.
		/// </summary>
		[JsonProperty( "relationship" )]
		[JsonConverter( typeof( SteamInterfaceHelpers.RelationshipTypeConverter ) )]
		public PlayerRelationshipType RelationshipType { get; set; }

	}

	public class SteamChatMessage : IComparable<SteamChatMessage> {

		/// <summary>
		/// Indicates the type of message received.
		/// </summary>
		public ChatMessageType Type { get; set; }

		/// <summary>
		/// Timestamp the message was sent, in UTC.
		/// </summary>
		public DateTime UTCMessageDateTime { get; set; }

		/// <summary>
		/// 64bit SteamID of the user who sent the message.
		/// </summary>
		public SteamID FromUser { get; set; }

		/// <summary>
		/// If available, text sent from the FromUser.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Initializes a new SteamChatMessage from an existing SteamPollMessage.
		/// </summary>
		/// <param name="message">Poll Message Received which is of Type MessageText or Typing.</param>
		internal static SteamChatMessage CreateFromPollMessage( SteamSharp.SteamChatClient.SteamPollMessage message ) {
			return new SteamChatMessage {
				Type = message.Type,
				UTCMessageDateTime = message.UTCMessageDateTime,
				FromUser = message.FromUser,
				Text = message.Text
			};
		}

		/// <summary>
		/// Compares the value of this message to a specified message.
		/// </summary>
		/// <param name="target">Specified message.</param>
		/// <returns>Returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified Compares the value of this instance to a specified DateTime value and returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified message.</returns>
		public int CompareTo( SteamChatMessage target ) {
			if( target == null )
				return 1;
			return this.UTCMessageDateTime.CompareTo( target.UTCMessageDateTime );
		}

	}

	public class SteamChatRelationshipNotification : IComparable<SteamChatRelationshipNotification> {

		/// <summary>
		/// Indicates the type of message received.
		/// </summary>
		public ChatMessageType Type { get; set; }

		/// <summary>
		/// Timestamp the message was sent, in UTC.
		/// </summary>
		public DateTime UTCMessageDateTime { get; set; }

		/// <summary>
		/// 64bit SteamID of the user who sent the message.
		/// </summary>
		public SteamID FromUser { get; set; }

		/// <summary>
		/// Sorry, no clue.
		/// </summary>
		public int StatusFlags { get; set; }

		/// <summary>
		/// Current state of the user's status.
		/// </summary>
		public PersonaState PersonaState { get; set; }

		/// <summary>
		/// Persona Name of the message author.
		/// </summary>
		public string PersonaName { get; set; }

		/// <summary>
		/// Initializes a new SteamChatMessage from an existing SteamPollMessage.
		/// </summary>
		/// <param name="message">Poll Message Received which is of Type PersonaStateChange or PersonaRelationship.</param>
		internal static SteamChatRelationshipNotification CreateFromPollMessage( SteamSharp.SteamChatClient.SteamPollMessage message ) {
			return new SteamChatRelationshipNotification {
				Type = message.Type,
				UTCMessageDateTime = message.UTCMessageDateTime,
				FromUser = message.FromUser,
				StatusFlags = message.StatusFlags,
				PersonaState = message.PersonaState,
				PersonaName = message.PersonaName
			};
		}

		/// <summary>
		/// Compares the value of this message to a specified message.
		/// </summary>
		/// <param name="target">Specified message.</param>
		/// <returns>Returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified Compares the value of this instance to a specified DateTime value and returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified message.</returns>
		public int CompareTo( SteamChatRelationshipNotification target ) {
			if( target == null )
				return 1;
			return this.UTCMessageDateTime.CompareTo( target.UTCMessageDateTime );
		}

	}

	public partial class SteamChatClient {

		private class SteamChatPollResult {

			/// <summary>
			/// ID given to the polling being accessed. Default value is 0.
			/// </summary>
			public int PollID { get; set; }

			public List<SteamPollMessage> Messages { get; set; }

			/// <summary>
			/// ID of the last message sent via the poll.
			/// </summary>
			[JsonProperty( "messagelast" )]
			public long PollLastMessageSentID { get; set; }

			/// <summary>
			/// Timestamp of the last new message, in UTC
			/// </summary>
			[JsonProperty( "utc_timestamp" )]
			[JsonConverter( typeof( SteamInterfaceHelpers.UnixDateTimeConverter ) )]
			public DateTime UTCTimestamp { get; set; }

			/// <summary>
			/// Starting ID (original LastMessageSentID when the poll was instantiated).
			/// </summary>
			[JsonProperty( "messagebase" )]
			public long MessageBaseID { get; set; }

			/// <summary>
			/// Length of time, in seconds, before the connection times out.
			/// </summary>
			[JsonProperty( "sectimeout" )]
			public int SecondsUntilTimeout { get; set; }

			/// <summary>
			/// State indicating OK for success or an error message in the event of failure. "Timeout" if the poll connection has timed out and requires re-connection.
			/// </summary>
			[JsonProperty( "error" )]
			[JsonConverter( typeof( SteamInterfaceHelpers.SteamChatPollStatusTypeConverter ) )]
			public ChatPollStatus PollStatus { get; set; }

		}

		public class SteamPollMessage : IComparable<SteamPollMessage> {

			/// <summary>
			/// Indicates the type of message received.
			/// </summary>
			[JsonProperty( "type" )]
			[JsonConverter( typeof( SteamInterfaceHelpers.SteamChatMessageTypeConverter ) )]
			public ChatMessageType Type { get; set; }

			/// <summary>
			/// Timestamp the message was sent, in UTC.
			/// </summary>
			[JsonProperty( "utc_timestamp" )]
			[JsonConverter( typeof( SteamInterfaceHelpers.UnixDateTimeConverter ) )]
			public DateTime UTCMessageDateTime { get; set; }

			/// <summary>
			/// 64bit SteamID of the user who sent the message.
			/// </summary>
			[JsonConverter( typeof( SteamInterfaceHelpers.SteamIDConverter ) )]
			[JsonProperty( "steamid_from" )]
			public SteamID FromUser { get; set; }

			/// <summary>
			/// Sorry, no clue.
			/// </summary>
			[JsonProperty( "status_flags" )]
			public int StatusFlags { get; set; }

			/// <summary>
			/// Current state of the user's status.
			/// </summary>
			[JsonProperty( "persona_state" )]
			public PersonaState PersonaState { get; set; }

			/// <summary>
			/// Persona Name of the message author.
			/// </summary>
			[JsonProperty( "persona_name" )]
			public string PersonaName { get; set; }

			/// <summary>
			/// If available, text sent from the FromUser.
			/// </summary>
			public string Text { get; set; }

			/// <summary>
			/// Compares the value of this message to a specified message.
			/// </summary>
			/// <param name="target">Specified message.</param>
			/// <returns>Returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified Compares the value of this instance to a specified DateTime value and returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified message.</returns>
			public int CompareTo( SteamPollMessage target ) {
				if( target == null )
					return 1;
				return this.UTCMessageDateTime.CompareTo( target.UTCMessageDateTime );
			}

		}

		private class SteamChatSendMessageResponse {

			/// <summary>
			/// Timestamp of the last new message, in UTC
			/// </summary>
			[JsonProperty( "utc_timestamp" )]
			[JsonConverter( typeof( SteamInterfaceHelpers.UnixDateTimeConverter ) )]
			public DateTime UTCTimestamp { get; set; }

			/// <summary>
			/// Text indicating OK for success or an error message in the event of failure. "Timeout" if the connection has timed out.
			/// </summary>
			public string Error { get; set; }

		}

		internal class SteamChatSession {

			/// <summary>
			/// 64bit SteamID of the user.
			/// </summary>
			[JsonConverter( typeof( SteamInterfaceHelpers.SteamIDConverter ) )]
			public SteamID SteamID { get; set; }

			/// <summary>
			/// Text indicating OK for success or an error message in the event of failure
			/// </summary>
			public string Error { get; set; }

			/// <summary>
			/// Unique ID which cooresponds to the newly created chat session.
			/// </summary>
			[JsonProperty( "umqid" )]
			public string ChatSessionID { get; set; }

			/// <summary>
			/// Timestamp of the last new message, in user's locale
			/// </summary>
			[JsonConverter( typeof( SteamInterfaceHelpers.UnixDateTimeConverter ) )]
			public DateTime Timestamp { get; set; }

			/// <summary>
			/// Timestamp of the last new message, in UTC
			/// </summary>
			[JsonProperty( "utc_timestamp" )]
			[JsonConverter( typeof( SteamInterfaceHelpers.UnixDateTimeConverter ) )]
			public DateTime UTCTimestamp { get; set; }

			/// <summary>
			/// Last message for use in polling.
			/// </summary>
			[JsonProperty( "message" )]
			public long MessageBaseID { get; set; }

			/// <summary>
			/// Push flag. Exact purpose is unknown.
			/// </summary>
			public long Push { get; set; }

		}

	}

}
