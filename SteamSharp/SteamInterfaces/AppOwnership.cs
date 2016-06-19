using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp
{
    public class AppOwnership : SteamInterface
    {
        public async static Task<PlayerService.AppContainer> GetOwnedApplicationsAsync(SteamClient client, string steamID)
        {
            client.IsAuthorizedCall(new Type[] {
				typeof( Authenticators.APIKeyAuthenticator ) // Executes in API context (public)
			});

            SteamRequest request = new SteamRequest(SteamAPIInterface.ISteamUser, "GetPublisherAppOwnership", SteamMethodVersion.v0001);
            request.AddParameter("steamid", steamID, ParameterType.QueryString);

            return VerifyAndDeserialize<PlayerService.GetOwnedGamesOwnershipResponseContainer>((await client.ExecuteAsync(request))).Response;

        }
    }
}
