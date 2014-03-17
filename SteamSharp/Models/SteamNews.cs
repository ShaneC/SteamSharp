using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SteamSharp {

	public partial class SteamNews {

		#region GetAppNews
		/// <summary>
		/// Object for executing the GetNewsForApp API call.
		/// </summary>
		protected class GetNewsForAppResponse {

			/// <summary>
			/// Containing object for the data of the GetNewsForApp API call.
			/// </summary>
			public AppNews AppNews { get; set; }

		}

		/// <summary>
		/// Containing object for the data of the GetNewsForApp API call.
		/// </summary>
		public class AppNews {

			/// <summary>
			/// AppID of the game you want the news of.
			/// </summary>
			public int AppID { get; set; }

			/// <summary>
			/// A list of <see cref="NewsItem"/>s, providing information on the specific application.
			/// </summary>
			public List<NewsItem> NewsItems { get; set; }

		}

		/// <summary>
		/// News information for the specified AppID.
		/// </summary>
		public class NewsItem {

			/// <summary>
			/// Unique ID for the given news article.
			/// </summary>
			public string GID { get; set; }

			/// <summary>
			/// Title of the news article.
			/// </summary>
			public string Title { get; set; }

			/// <summary>
			/// Full URL to the article.
			/// </summary>
			public string Url { get; set; }

			/// <summary>
			/// Boolean indicating if the news article is external to the Steam community.
			/// </summary>
			[JsonProperty( "is_external_url" )]
			public bool IsExternalURL { get; set; }

			/// <summary>
			/// Name of the article's author.
			/// </summary>
			public string Author { get; set; }

			/// <summary>
			/// Snippet of the article's text. Character count of this field is selected via the maxLength parameter.
			/// </summary>
			public string Contents { get; set; }

			/// <summary>
			/// Comma-separated string of keywords.
			/// </summary>
			public string FeedLabel { get; set; }

			/// <summary>
			/// DateTime the article was posted.
			/// </summary>
			[JsonConverter( typeof( SteamInterfaceHelpers.UnixDateTimeConverter ) )]
			public DateTime Date { get; set; }

			/// <summary>
			/// Friendly name ID of the specified feed (i.e. rps, steam_updates, tf2_blog).
			/// </summary>
			public string FeedName { get; set; }

		}
		#endregion

	}

}
