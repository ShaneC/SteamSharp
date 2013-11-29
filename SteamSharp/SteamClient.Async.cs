using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

	public partial class SteamClient {

		/// <summary>
		/// Executes the provided request asynchronously, returning the response object.
		/// </summary>
		/// <param name="request"><see cref="ISteamRequest"/> object for execution.</param>
		/// <returns><see cref="ISteamResponse object"/> containing the result of the request.</returns>
		public async Task<ISteamResponse> ExecuteAsync( ISteamRequest request ) {

			HttpRequestMessage httpRequest = BuildHttpRequest( request );

			using( var httpClient = new HttpClient() ){

				httpClient.Timeout = TimeSpan.FromMilliseconds( ( ( request.Timeout > 0 ) ? request.Timeout : this.Timeout ) );

				HttpResponseMessage response;
				try {
					response = await httpClient.SendAsync( httpRequest );
				}catch( Exception ex ) {
					if( ex.InnerException != null && ex.InnerException is WebException )
						return CreateErrorResponse( request, ex.InnerException );
					return CreateErrorResponse( request, ex );
				}

				return ConvertToResponse( request, response );

			}

		}

	}

}
