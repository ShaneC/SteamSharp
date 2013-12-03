using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace SteamSharp {

	public class SteamInterfaceHelpers {

		public class UnixDateTimeConverter : DateTimeConverterBase {

			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
				writer.WriteValue( (long)value );
			}

			/// <summary>
			/// Reads the JSON representation of the object.
			/// </summary>
			/// <param name = "reader">The <see cref = "JsonReader" /> to read from.</param>
			/// <param name = "objectType">Type of the object.</param>
			/// <param name = "existingValue">The existing value of object being read.</param>
			/// <param name = "serializer">The calling serializer.</param>
			/// <returns>The object value.</returns>
			public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
				if( reader.TokenType != JsonToken.Integer )
					throw new JsonReaderException( "Token does not coorespond to the correct datatype (long or int)." );
				return FromUnixTime( (long)reader.Value );
			}

			/// <summary>
			/// Creates DateTime value (Timezone UTC) from a Unix based timestamp
			/// </summary>
			/// <param name="target">Unix timestamp to convert</param>
			/// <returns>DateTime object representing the UTC DateTime of the target</returns>
			public static DateTime FromUnixTime( Int64 target ) {
				return new DateTime( 1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc ).AddSeconds( target );
			}

		}

	}

}
