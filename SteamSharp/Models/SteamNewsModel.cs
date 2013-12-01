using System.Collections.Generic;

namespace SteamSharp {

	public class SteamNewsModel {

		#region AppNews
		public class AppNewsResponse {
			public AppNews appnews { get; set; }
		}

		public class AppNews {
			public int appid { get; set; }
			public List<NewsItem> newsitems { get; set; }
		}

		public class NewsItem {
			public string gid { get; set; }
			public string title { get; set; }
			public string url { get; set; }
			public bool is_external_url { get; set; }
			public string author { get; set; }
			public string contents { get; set; }
			public string feedlabel { get; set; }
			public int date { get; set; }
			public string feedname { get; set; }
		}
		#endregion

	}

}
