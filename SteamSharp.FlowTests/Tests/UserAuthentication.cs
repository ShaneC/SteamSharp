using SteamSharp.Authenticators;
using System;

namespace SteamSharp.FlowTests.Tests {

	public class UserAuthentication : ITestClass {

		public static SteamUser Login() {

			string Username = WriteConsole.Prompt( "Please enter target username:" );
			string Password = WriteConsole.Prompt( "Please enter password for " + Username + ":" );

			SteamClient client = new SteamClient();
			var result = SteamSharp.Authenticators.UserAuthenticator.GetAccessTokenForUser( Username, Password );

			UserAuthenticator.CaptchaAnswer captcha = null;
			UserAuthenticator.SteamGuardAnswer steamGuard = null;

			while( true ) {

				if( result.IsCaptchaNeeded ) {
					WriteConsole.Information( "CAPTCHA has been required by the transaction." );
					WriteConsole.Information( "Opening CAPTCHA prompt in browser..." );
					System.Diagnostics.Process.Start( result.CaptchaURL );
					captcha = new UserAuthenticator.CaptchaAnswer {
						GID = result.CaptchaGID,
						SolutionText = WriteConsole.Prompt( "Please enter CAPTCHA solution:" )
					};
				} else
					captcha = null;

				if( result.IsSteamGuardNeeded ) {
					WriteConsole.Information( "SteamGuard has been required by the transaction." );
					steamGuard = new UserAuthenticator.SteamGuardAnswer {
						ID = result.SteamGuardID,
						SolutionText = WriteConsole.Prompt( "Please enter SteamGuard solution from your @" + result.SteamGuardEmailDomain + " e-mail:" )
					};
				} else
					steamGuard = null;

				result = SteamSharp.Authenticators.UserAuthenticator.GetAccessTokenForUser( Username, Password, steamGuard, captcha );

				if( result.IsSuccessful ) {
					WriteConsole.Success( "Yay, authentication was successful!" );
					return result.User;
				} else if( result.SteamResponseMessage == "Incorrect login" ) {
					throw new Exception( "Authentication failied, either due to bad password or a failure of the code." );
				} else if( result.IsCaptchaNeeded || result.IsSteamGuardNeeded ) {
					WriteConsole.Information( "Additional information is needed, going through the loop again.\nSteam Message: " + result.SteamResponseMessage );
					continue;
				} else {
					throw new Exception( "Authentication failed, no additional data was needed (so unknown reason for failure).\nSteam Message: " + result.SteamResponseMessage );
				}

			}

		}

		public bool Invoke() {

			try {

				SteamUser user = Login();
				if( user == null )
					return false;

				WriteConsole.Information( "Now attempting basic protected API call..." );

				SteamClient client = new SteamClient();
				client.Authenticator = UserAuthenticator.ForProtectedResource( user );

				// Validate basic protected API call
				var response = SteamUserOAuth.GetFriendsList( client, user.SteamID );
				if( response.Friends == null )
					throw new Exception( "Unable to get protected data!" );

				WriteConsole.Success( "Protected data retrieved!" );

				return true;

			} catch( Exception e ) {
				WriteConsole.Error( e.Message + "\n" + e.ToString() );
				return false;
			}

		}

	}

}
