using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace SteamSharp {

	public static class SteamInterfaceHelpers {

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
