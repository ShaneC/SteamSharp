using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

	public abstract class SteamInterface {

		protected static T VerifyAndDeserialize<T>( ISteamResponse response ) {

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
