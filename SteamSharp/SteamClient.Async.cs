using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SteamSharp {

	public partial class SteamClient {

		/// <summary>
		/// Executes the provided request asynchronously, returning the response object.
		/// </summary>
		/// <param name="request"><see cref="ISteamRequest"/> object for execution.</param>
		/// <returns><see cref="ISteamResponse"/> object containing the result of the request.</returns>
		public async Task<ISteamResponse> ExecuteAsync( ISteamRequest request ) {

			AuthenticateClient( this, request );

			HttpRequestMessage httpRequest = BuildHttpRequest( request );

			CookieContainer cookieContainer = new CookieContainer();

			if( request.Cookies == null || request.Cookies.Count > 0 ) {
				foreach( Cookie cookie in request.Cookies )
					cookieContainer.Add( httpRequest.RequestUri, cookie );
			}

			HttpClientHandler httpHandler = new HttpClientHandler();
			httpHandler.CookieContainer = cookieContainer;

			using( var httpClient = new HttpClient( httpHandler ) ){

				httpClient.Timeout = TimeSpan.FromMilliseconds( ( ( request.Timeout > 0 ) ? request.Timeout : this.Timeout ) );

				try {
					request.IncreaseNumAttempts();
					return ConvertToResponse( request, await httpClient.SendAsync( httpRequest ), cookieContainer );
				}catch( Exception ex ) {
					if( ex.InnerException != null && ex.InnerException is WebException )
						return CreateErrorResponse( request, ex.InnerException );
					return CreateErrorResponse( request, ex );
				}

			}

		}

	}

}
