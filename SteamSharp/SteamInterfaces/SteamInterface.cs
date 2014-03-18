using Newtonsoft.Json;
using SteamSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net;

namespace SteamSharp {

	/// <summary>
	/// Base class for abstraction classes of Steam API interfaces.
	/// </summary>
	public abstract class SteamInterface {

		/// <summary>
		/// Utility class to validate the Steam Response for common issues and, if none are found, attempt to deserialize the response's content.
		/// </summary>
		/// <typeparam name="T">Object type for the <see cref="ISteamResponse"/> to be deserialized into.</typeparam>
		/// <param name="response">Response object provided by the SteamClient's execution method.</param>
		/// <returns>Deserialized representation of <see cref="ISteamResponse"/>'s server-recevied content.</returns>
		public static T VerifyAndDeserialize<T>( ISteamResponse response ) {

			if( !response.IsSuccessful ) {

				string message;

				switch( response.StatusCode ) {
					case HttpStatusCode.Unauthorized:
						message = "Steam Request Failed. Unauthorized to access this resource. Typically this means the credentials provided (user token or API key) are no longer valid.";
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

				var requestException = new SteamRequestException( message, response ) {
					IsRequestIssue = true,
					IsAuthenticationIssue = ( response.StatusCode == HttpStatusCode.Unauthorized )
				};

				// SPECIAL CASE: If the call is unauthorized, throw a SteamAuthenticationException so the client can take appropriate re-auth action.
				if( response.StatusCode == HttpStatusCode.Unauthorized ) {

				}
				throw new SteamRequestException( message, response ) {
					IsRequestIssue = true,
					IsAuthenticationIssue = ( response.StatusCode == HttpStatusCode.Unauthorized )
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

		/// <summary>
		/// Utility method providing translation of valid language possibilities for certain Steam APIs.
		/// </summary>
		/// <param name="language">Target language</param>
		/// <returns>Steam-understandable string representation of the target language.</returns>
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
