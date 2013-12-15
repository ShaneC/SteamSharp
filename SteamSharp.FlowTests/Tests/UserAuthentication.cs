using SteamSharp.Authenticators;

namespace SteamSharp.FlowTests.Tests {

	public class UserAuthentication : ITestClass {

		private string Username;
		private string Password;

		public bool Invoke() {

			Username = WriteConsole.Prompt( "Please enter target username:" );
			Password = WriteConsole.Prompt( "Please enter password for " + Username + ":" );

			SteamClient client = new SteamClient();
			var result = SteamSharp.Authenticators.UserAuthenticator.GetAccessTokenForUserAsync( Username, Password );

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
						SolutionText = WriteConsole.Prompt( "Please enter SteamGuard solution from your e-mail:" )
					};
				} else
					steamGuard = null;

				result = SteamSharp.Authenticators.UserAuthenticator.GetAccessTokenForUserAsync( Username, Password, steamGuard, captcha );

				if( result.IsSuccessful ) {
					WriteConsole.Success( "Yay, authentication was successful!" );
					return true;
				} else if( result.SteamResponseMessage == "Incorrect login" ) {
					WriteConsole.Error( "Authentication failied, either due to bad password or a failure of the code." );
					return false;
				} else if( result.IsCaptchaNeeded || result.IsSteamGuardNeeded ) {
					WriteConsole.Information( "Additional information is needed, going through the loop again.\nSteam Message: " + result.SteamResponseMessage );
					continue;
				} else {
					WriteConsole.Error( "Authentication failed, no additional data was needed.\nSteam Message: " + result.SteamResponseMessage );
					break;
				}

			}
			
			return false;

		}

	}

}
