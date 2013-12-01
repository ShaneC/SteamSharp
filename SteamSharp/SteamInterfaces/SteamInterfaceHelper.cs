using Newtonsoft.Json;
using System;

namespace SteamSharp {

	public class SteamInterfaceHelper {

		public static T VerifyAndDeserialize<T>( ISteamResponse response ) {

			if( !response.IsSuccessful )
				throw new SteamRequestException( "The HTTP request to Steam was not successful.", response );

			try {
				return JsonConvert.DeserializeObject<T>( response.Content );
			} catch( Exception e ) {
				throw new SteamRequestException( e.Message ) {
					IsDeserializationIssue = true
				};
			}

		}

	}

}
