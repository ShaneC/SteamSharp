using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace SteamSharp.Test {

	[TestClass]
	public class ISteamNews {

		#region GetNewsForApp
		[TestMethod]
		public void GET_GetNewsForApp_ByUri() {

			SteamClient client = new SteamClient();
			SteamRequest request = new SteamRequest( "ISteamNews/GetNewsForApp/v0002/" );

			request.AddParameter( "appid", 440 );
			request.AddParameter( "count", 2 );
			request.AddParameter( "maxlength", 100 );

			var response = client.Execute( request );

			Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );
			Assert.IsNotNull( response.Content );

		}

		[TestMethod]
		public void GET_GetNewsForApp_ByEnums() {

			SteamClient client = new SteamClient();
			SteamRequest request = new SteamRequest( SteamAPIInterface.ISteamNews, "GetNewsForApp", SteamMethodVersion.v0002 );

			request.AddParameter( "appid", 440 );
			request.AddParameter( "count", 2 );
			request.AddParameter( "maxlength", 100 );

			var response = client.Execute( request );

			Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );
			Assert.IsNotNull( response.Content );

		}

		[TestMethod]
		public void GET_GetNewsForApp_ByClass() {

			SteamClient client = new SteamClient();

			var response = SteamNews.GetNewsForApp( client, 440, 2, 100 );

			Assert.IsNotNull( response );
			Assert.IsInstanceOfType( response, typeof( SteamNews.AppNews ) );

			Assert.IsNotNull( response.NewsItems[0].Date );
			Assert.IsInstanceOfType( response.NewsItems[0].Date, typeof( DateTime ) );

		}

		[TestMethod]
		public void GET_GetNewsForApp_NoValues() {

			SteamClient client = new SteamClient();
			SteamRequest request = new SteamRequest( "ISteamNews/GetNewsForApp/v0002/" );

			var response = client.Execute( request );

			Assert.AreEqual( HttpStatusCode.BadRequest, response.StatusCode );

		}
		#endregion

	}

}
