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
			public List<Achievement> achievements { get; set; }
		}

		public class Achievement {
			public string name { get; set; }
			public double percent { get; set; }
		}

	}

}
