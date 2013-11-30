using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp.Authenticators {

	public class CredentialAuthenticator : ISteamAuthenticator {

		private static string _accessToken;

		public CredentialAuthenticator( string accessToken ) {
			_accessToken = accessToken;
		}

		public void Authenticate( SteamClient client, ISteamRequest request ) {
			
		}

	}

}

/*
public LoginStatus Authenticate( String username, String password, String emailauthcode = "" )
        {
            String response = steamRequest( "ISteamOAuth2/GetTokenWithCredentials/v0001",
                "client_id=DE45CD61&grant_type=password&username=" + Uri.EscapeDataString( username ) + "&password=" + Uri.EscapeDataString( password ) + "&x_emailauthcode=" + emailauthcode + "&scope=read_profile%20write_profile%20read_client%20write_client" );

            if ( response != null )
            {
                JObject data = JObject.Parse( response );

                if ( data["access_token"] != null )
                {
                    accessToken = (String)data["access_token"];

                    return login() ? LoginStatus.LoginSuccessful : LoginStatus.LoginFailed;
                }
                else if ( ( (string)data["x_errorcode"] ).Equals( "steamguard_code_required" ) )
                    return LoginStatus.SteamGuard;
                else
                    return LoginStatus.LoginFailed;
            }
            else
            {
                return LoginStatus.LoginFailed;
            }
        }
*/