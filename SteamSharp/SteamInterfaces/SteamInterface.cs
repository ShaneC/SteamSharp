using Newtonsoft.Json;
using System;
using System.Net;

namespace SteamSharp {

	public class SteamInterface {

		protected static T VerifyAndDeserialize<T>( ISteamResponse response ) {

			if( !response.IsSuccessful ) {

				string message;

				switch( response.StatusCode ) {
					case HttpStatusCode.Unauthorized:
						message = "Steam Request Failed. Unauthorized to access this resource (have you authenticated the client? are your keys valid?).";
						break;
					case HttpStatusCode.BadRequest:
						message = "Steam Request Failed. Bad Request.";
						break;
					case HttpStatusCode.NotFound:
						message = "Steam API returned a 404 (Not Found) for " + response.HttpResponse.RequestMessage.RequestUri.ToString() + ".";
						break;
					default:
						message = 
							"The HTTP request to Steam was not successful. The Steam Service returned HTTP Code " + ((int)response.StatusCode) + 
							( ( String.IsNullOrEmpty( response.StatusDescription ) ) ? "" : " (" + response.StatusDescription + ")" ) + ".";
						break;
				}

				throw new SteamRequestException( message, response ) {
					IsRequestIssue = true
				};

			}

			try {
				return JsonConvert.DeserializeObject<T>( response.Content );
			} catch( Exception e ) {
				throw new SteamRequestException( e.Message ) {
					IsDeserializationIssue = true
				};
			}

		}

		protected static string GetLanguageFromEnum( RequestedLangage language ) {

			switch( Enum.GetName( typeof( RequestedLangage ), language ) ) {
				case "English":
					return "english";
				case "Spanish":
					return "spanish";
				case "Swedish":
					return "swedish";
				case "Russian":
					return "russian";
				case "Portuguese":
					return "portuguese";
				case "German":
					return "german";
				case "Dutch":
					return "dutch";
				case "Japanese":
					return "japanese";
				default:
					return "english";
			}

		}

	}

}
