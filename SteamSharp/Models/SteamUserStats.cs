using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp {

	public partial class SteamUserStats {

		public class AchievementPercentagesResponse {
			public AchievementPercentages achievementpercentages { get; set; }
		}

		public class AchievementPercentages {
			public List<GlobalAchievement> achievements { get; set; }
		}

		public class GlobalAchievement {
			public string name { get; set; }
			public double percent { get; set; }
		}

		public class GetPlayerAchievementsResponse {
			public PlayerStats playerstats { get; set; }
		}

		public class PlayerStats {
			public string steamID { get; set; }
			public string gameName { get; set; }
			public List<Achievement> achievements { get; set; }
			public bool success { get; set; }
		}

		public class Achievement {
			public string apiname { get; set; }
			public int achieved { get; set; }
			public string name { get; set; }
			public string description { get; set; }
		}

	}

}
