using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SteamSharp {

	public partial class PlayerService {

		#region GetOwnedGames
		/// <summary>
		/// Object for executing the GetOwnedGames API call.
		/// </summary>
		private class GetOwnedGamesResponse {

			/// <summary>
			/// Information about the specified Steam user's games library.
			/// </summary>
			[JsonProperty( "response" )]
			public OwnedGames OwnedGames { get; set; }

		}

		/// <summary>
		/// Information about the specified Steam user's games library.
		/// </summary>
		public class OwnedGames {

			/// <summary>
			/// The total number of games the user owns (including free games they've played, if getPlayedFreeGames is set).
			/// </summary>
			[JsonProperty("game_count")]
			public int GameCount { get; set; }

			/// <summary>
			/// List of <see cref="Game"/>s that are owned by the specified user.
			/// </summary>
			public List<Game> Games { get; set; }

		}
		#endregion

		#region GetRecentlyPlayedGames
		/// <summary>
		/// Object for executing the GetOwnedGames API call.
		/// </summary>
		private class GetRecentlyPlayedGamesResponse {

			/// <summary>
			/// Information about the games the specified Steam user has played recently.
			/// </summary>
			[JsonProperty( "response" )]
			public PlayedGames PlayedGames { get; set; }
		}

		/// <summary>
		/// Information about the games the specified Steam user has played recently.
		/// </summary>
		public class PlayedGames {

			/// <summary>
			/// The total number of unique games the user has played in the last two weeks.
			/// This is mostly significant if you opted to return a limited number of games with the count input parameter.
			/// </summary>
			[JsonProperty( "total_count" )]
			public int TotalCount { get; set; }

			/// <summary>
			/// List of <see cref="Game"/>s that have been recently played by the user.
			/// </summary>
			public List<Game> Games { get; set; }

		}
		#endregion

		#region IsPlayingSharedGame
		/// <summary>
		/// Object for executing the IsPlayingSharedGame API call.
		/// </summary>
		private class IsPlayingSharedGameResponse {

			/// <summary>
			/// Containing object for the data of the IsPlayingSharedGame API call.
			/// </summary>
			[JsonProperty( "response" )]
			public IsPlayingSharedGameObject IsPlayingSharedGame { get; set; }

		}

		/// <summary>
		/// Containing object for the data of the IsPlayingSharedGame API call.
		/// </summary>
		private class IsPlayingSharedGameObject {

			/// <summary>
			/// The SteamID of the original owner if the given account currently plays this game and it's borrowed. In all other cases the result is 0.
			/// </summary>
			public string LenderSteamID { get; set; }

		}

		/// <summary>
		/// Object containing a flag indicating if the requested game is currently being played by the requested user, and the IDs of both parties.
		/// </summary>
		public class SharedGameData {

			/// <summary>
			/// Flag indicating if the user specified is in fact currently playing the game specified.
			/// </summary>
			public bool IsUserPlayingSharedGame { get; set; }

			/// <summary>
			/// SteamID of the user who's borrowing the game.
			/// If the game is being borrowed, this will always be the value of the SteamID specified in the query.
			/// If IsUserPlayingSharedGame is false, this will be null.
			/// </summary>
			public string GameBorrowerSteamID { get; set; }

			/// <summary>
			/// SteamID of the original owner of the game being borrowed.
			/// If IsUserPlayingSharedGame is false, this will be null. 
			/// </summary>
			public string GameOwnerSteamID { get; set; }

		}
		#endregion

		#region Shared Models
		/// <summary>
		/// News information for the specified GameID/AppID.
		/// </summary>
		public class Game {

			/// <summary>
			/// Unique ID of the Game (synonymous with AppID).
			/// </summary>
			[JsonProperty( "appid" )]
			public int GameID { get; set; }

			/// <summary>
			/// Name of the game.
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// The total number of minutes played "on record", since Steam began tracking total playtime in early 2009.
			/// </summary>
			[JsonProperty( "playtime_forever" )]
			public int PlaytimeForever { get; set; }

			/// <summary>
			/// The total number of minutes played in the last 2 weeks.
			/// </summary>
			[JsonProperty( "playtime_2weeks" )]
			public int PlaytimeTwoWeeks { get; set; }

			/// <summary>
			/// Full URL for the Game's icon image.
			/// </summary>
			[JsonProperty( "img_icon_url" )]
			public string ImgIconURL { get; set; }

			/// <summary>
			/// Fully URL for the Game's logo image.
			/// </summary>
			[JsonProperty( "img_logo_url" )]
			public string ImgLogoURL { get; set; }

			/// <summary>
			/// (Set on GetOwnedGames calls only)
			/// Indicates there is a stats page with achievements or other game stats available for this game.
			/// </summary>
			[JsonProperty( "has_community_visible_stats" )]
			public bool HasCommunityVisibileStats { get; set; }

		}
        #endregion


        /// <summary>
        /// Information about the application you can get the owner of the application
        /// </summary>
        public class App
        {
            /// <summary>
            /// Unique ID of the Application (synonymous with AppID).
            /// </summary>
            [JsonProperty("appid")]
            public int ApplicationId { get; set; }

            [JsonProperty("ownsapp")]
            public bool IsApplicationOwned { get; set; }

            [JsonProperty("permanent")]
            public bool IsApplicationPermanent { get; set; }

            [JsonProperty("timestamp")]
            public DateTime OwnedTimestamp { get; set; }

            [JsonProperty("ownersteamid")]
            public long OwnerSteamId { get; set; }
        }

        /// <summary>
        /// A container for a response, which contains the container application
        /// </summary>
        public class GetOwnedGamesOwnershipResponseContainer
        {
            /// <summary>
            /// Information about the specified Steam user's games library.
            /// </summary>
            [JsonProperty("appownership")]
            public AppContainer Response { get; set; }
        }

        /// <summary>
        /// The container for the application, which contains an array of application property
        /// </summary>
        public class AppContainer
        {
            [JsonProperty("apps")]
            public AppCollectionContainer Apps { get; set; }
        }

        /// <summary>
        /// The container for the application, which contains an array of application property
        /// </summary>
        public class AppCollectionContainer
        {
            [JsonProperty("app")]
            public List<App> Application { get; set; }
            public int Count => Application.Count;
        }



    }

}
