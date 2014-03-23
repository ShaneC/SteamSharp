using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

	public partial class SteamChat {

		public class SteamChatMessage : IComparable<SteamChatMessage> {

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
			public int CompareTo( SteamChatMessage target ) {
				if( target == null )
					return 1;
				return this.UTCMessageDateTime.CompareTo( target.UTCMessageDateTime );
			}

		}

	}

	public partial class SteamChatClient {

		private class SteamChatPollResult {

			/// <summary>
			/// ID given to the polling being accessed. Default value is 0.
			/// </summary>
			public int PollID { get; set; }

			public List<SteamChat.SteamChatMessage> Messages { get; set; }

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
