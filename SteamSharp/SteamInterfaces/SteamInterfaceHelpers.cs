using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace SteamSharp {

	/// <summary>
	/// Utility class used in the Steam Interface classes.
	/// </summary>
	public static class SteamInterfaceHelpers {

		/// <summary>
		/// Json.Net converter class for reading/writing string/long to/from SteamID.
		/// </summary>
		public class SteamIDConverter : JsonConverter {

			/// <summary>
			/// Writes the JSON representation of the converted object (SteamID --> String).
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param>
			/// <param name="value">The value.</param>
			/// <param name="serializer">The calling serializer.</param>
			/// <returns>The string equivalent of the specified SteamID object.</returns>
			public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
				if( !( value is SteamID ) )
					throw new JsonReaderException( "Specified object must be of type SteamID." );
				writer.WriteValue( value.ToString() );
			}

			/// <summary>
			/// Read the JSON representation of the converted object (String --> SteamID)
			/// </summary>
			/// <param name = "reader">The <see cref = "JsonReader" /> to read from.</param>
			/// <param name = "objectType">Type of the object.</param>
			/// <param name = "existingValue">The existing value of object being read.</param>
			/// <param name = "serializer">The calling serializer.</param>
			/// <returns>SteamID object representing the input string.</returns>
			public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
				if( reader.TokenType == JsonToken.String )
					return new SteamID( (string)reader.Value );
				else if( reader.TokenType == JsonToken.Integer )
					return new SteamID( Int64.Parse( (string)reader.Value ).ToString() );
				throw new JsonReaderException( "Token does not coorespond to the correct datatype (long or int)." );
			}

			public override bool CanConvert( Type objectType ) {
				return ( objectType == typeof( SteamID ) || objectType == typeof( string ) || objectType == typeof( int ) );
			}

		}

		/// <summary>
		/// Json.Net converter class for PlayerRelationshipType.
		/// </summary>
		public class RelationshipTypeConverter : JsonConverter {

			/// <summary>
			/// Writes the string representation of the converted object (PlayerRelationshipType --> String).
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param>
			/// <param name="value">The value.</param>
			/// <param name="serializer">The calling serializer.</param>
			/// <returns>The string equivalent of the specified PlayerRelationshipType object.</returns>
			public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
				if( !( value is PlayerRelationshipType ) )
					throw new JsonReaderException( "Specified object must be of type PlayerRelationshipType." );
				writer.WriteValue( Enum.GetName( typeof( PlayerRelationshipType ), value ).ToLower() );
			}

			/// <summary>
			/// Read the JSON representation of the converted object (String --> PlayerRelationshipType)
			/// </summary>
			/// <param name = "reader">The <see cref = "JsonReader" /> to read from.</param>
			/// <param name = "objectType">Type of the object.</param>
			/// <param name = "existingValue">The existing value of object being read.</param>
			/// <param name = "serializer">The calling serializer.</param>
			/// <returns>PlayerRelationshipType object representing the input string.</returns>
			public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
				if( reader.TokenType != JsonToken.String )
					throw new JsonReaderException( "Token does not coorespond to the correct datatype (string)." );
				switch( ( (string)reader.Value ).ToLower() ) {
					case "all": return PlayerRelationshipType.All;
					case "friend": return PlayerRelationshipType.Friend;
					case "requestinitiator": return PlayerRelationshipType.RequestInitiator;
					case "requestrecipient": return PlayerRelationshipType.RequestRecipient;
					default: return PlayerRelationshipType.Unknown;
				}
			}

			public override bool CanConvert( Type objectType ) {
				return ( objectType == typeof( PlayerRelationshipType ) || objectType == typeof( string ) );
			}

		}

		/// <summary>
		/// Json.Net converter class for ChatMessageType.
		/// </summary>
		public class SteamChatMessageTypeConverter : JsonConverter {

			/// <summary>
			/// Writes the string representation of the converted object (ChatMessageType --> String).
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param>
			/// <param name="value">The value.</param>
			/// <param name="serializer">The calling serializer.</param>
			/// <returns>The string equivalent of the specified PlayerRelationshipType object.</returns>
			public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
				if( !( value is ChatMessageType ) )
					throw new JsonReaderException( "Specified object must be of type ChatMessageType." );
				switch( (ChatMessageType)value ) {
					case ChatMessageType.MessageText: writer.WriteValue( "saytext" ); break;
					case ChatMessageType.PersonaStateChange: writer.WriteValue( "personastate" ); break;
					case ChatMessageType.Typing: writer.WriteValue( "typing" ); break;
					default: writer.WriteValue( "unknown" ); break;
				}
			}

			/// <summary>
			/// Read the JSON representation of the converted object (String --> ChatMessageType)
			/// </summary>
			/// <param name = "reader">The <see cref = "JsonReader" /> to read from.</param>
			/// <param name = "objectType">Type of the object.</param>
			/// <param name = "existingValue">The existing value of object being read.</param>
			/// <param name = "serializer">The calling serializer.</param>
			/// <returns>PlayerRelationshipType object representing the input string.</returns>
			public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
				if( reader.TokenType != JsonToken.String )
					throw new JsonReaderException( "Token does not coorespond to the correct data type (string)." );
				switch( ( (string)reader.Value ).ToLower() ) {
					case "personastate": return ChatMessageType.PersonaStateChange;
					case "saytext": return ChatMessageType.MessageText;
					case "typing": return ChatMessageType.Typing;
					default: return ChatMessageType.Unknown;
				}
			}

			public override bool CanConvert( Type objectType ) {
				return ( objectType == typeof( ChatMessageType ) || objectType == typeof( string ) );
			}

		}

		/// <summary>
		/// Json.Net converter class for ChatPollStatus.
		/// </summary>
		public class SteamChatPollStatusTypeConverter : JsonConverter {

			/// <summary>
			/// Writes the string representation of the converted object (ChatPollStatus --> String).
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param>
			/// <param name="value">The value.</param>
			/// <param name="serializer">The calling serializer.</param>
			/// <returns>The string equivalent of the specified PlayerRelationshipType object.</returns>
			public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
				if( !( value is ChatPollStatus ) )
					throw new JsonReaderException( "Specified object must be of type ChatPollStatus." );
				switch( (ChatPollStatus)value ) {
					case ChatPollStatus.OK: writer.WriteValue( "OK" ); break;
					case ChatPollStatus.TimedOut: writer.WriteValue( "Timeout" ); break;
					default: writer.WriteValue( "Unknown" ); break;
				}
			}

			/// <summary>
			/// Read the JSON representation of the converted object (String --> ChatPollStatus)
			/// </summary>
			/// <param name = "reader">The <see cref = "JsonReader" /> to read from.</param>
			/// <param name = "objectType">Type of the object.</param>
			/// <param name = "existingValue">The existing value of object being read.</param>
			/// <param name = "serializer">The calling serializer.</param>
			/// <returns>PlayerRelationshipType object representing the input string.</returns>
			public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
				if( reader.TokenType != JsonToken.String )
					throw new JsonReaderException( "Token does not coorespond to the correct data type (string)." );
				switch( ( (string)reader.Value ).ToLower() ) {
					case "ok": return ChatPollStatus.OK;
					case "timeout": return ChatPollStatus.TimedOut;
					default: return ChatPollStatus.Unknown;
				}
			}

			public override bool CanConvert( Type objectType ) {
				return ( objectType == typeof( ChatPollStatus ) || objectType == typeof( string ) );
			}

		}

		/// <summary>
		/// Json.Net converter class for reading/writing Unix Time from/to JSON.
		/// </summary>
		public class UnixDateTimeConverter : DateTimeConverterBase {

			/// <summary>
			/// Writes the JSON representation of the converted object (DateTime --> 64-bit integer).
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param>
			/// <param name="value">The value.</param>
			/// <param name="serializer">The calling serializer.</param>
			/// <returns>The 64-bit integer equivalent of the specified DateTime object.</returns>
			public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
				if( !( value is DateTime ) )
					throw new JsonReaderException( "Specified object must be of type DateTime." );
				writer.WriteValue( ( (DateTime)value ).ToUnixTime() ); 
			}

			/// <summary>
			/// Read the JSON representation of the converted object (64-bit integer --> DateTime)
			/// </summary>
			/// <param name = "reader">The <see cref = "JsonReader" /> to read from.</param>
			/// <param name = "objectType">Type of the object.</param>
			/// <param name = "existingValue">The existing value of object being read.</param>
			/// <param name = "serializer">The calling serializer.</param>
			/// <returns>DateTime object representing the 64-bit integer Unix timestamp.</returns>
			public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
				if( reader.TokenType != JsonToken.Integer )
					throw new JsonReaderException( "Token does not coorespond to the correct datatype (long or int)." );
				return ( (long)reader.Value ).FromUnixTime();
			}

		}

		/// <summary>
		/// Creates DateTime value (assumes UTC timezone) from a Unix based timestamp
		/// </summary>
		/// <param name="target">Unix timestamp to convert</param>
		/// <returns>DateTime object representing the UTC DateTime of the target</returns>
		public static DateTime FromUnixTime( this Int64 target ) {
			return new DateTime( 1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc ).AddSeconds( target );
		}

		/// <summary>
		/// Converts a DateTime object into its Unix based timestamp equivalent. Uses the timezone of the initial DateTime object.
		/// </summary>
		/// <param name="target">DateTime object to convert.</param>
		/// <returns>On success, returns 64-bit integer conforming to the Unix timestamp format. On failure, returns -1.</returns>
		public static Int64 ToUnixTime( this DateTime target ) {
			if( target == DateTime.MinValue )
				return 0;
			var delta = target - ( new DateTime( 1970, 1, 1, 0, 0, 0, target.Kind ) );
			if( delta.TotalSeconds < 0 )
				return -1;
			return (long)delta.TotalSeconds;
		}

	}

}
