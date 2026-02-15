using System;
using Microsoft.Xna.Framework;
using Terraria.Achievements;

namespace Terraria.UI
{
	// Token: 0x020000E3 RID: 227
	public class AchievementAdvisorCard
	{
		// Token: 0x060018C0 RID: 6336 RVA: 0x004E4318 File Offset: 0x004E2518
		public AchievementAdvisorCard(Achievement achievement, float order)
		{
			this.achievement = achievement;
			this.order = order;
			this.achievementIndex = Main.Achievements.GetIconIndex(achievement.Name);
			this.frame = new Rectangle(this.achievementIndex % 8 * 66, this.achievementIndex / 8 * 66, 64, 64);
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x004E4374 File Offset: 0x004E2574
		public bool IsAchievableInWorld()
		{
			string name = this.achievement.Name;
			if (name == "MASTERMIND")
			{
				return WorldGen.crimson;
			}
			if (!(name == "WORM_FODDER"))
			{
				return !(name == "PLAY_ON_A_SPECIAL_SEED") || Main.specialSeedWorld;
			}
			return !WorldGen.crimson;
		}

		// Token: 0x040012DE RID: 4830
		private const int _iconSize = 64;

		// Token: 0x040012DF RID: 4831
		private const int _iconSizeWithSpace = 66;

		// Token: 0x040012E0 RID: 4832
		private const int _iconsPerRow = 8;

		// Token: 0x040012E1 RID: 4833
		public Achievement achievement;

		// Token: 0x040012E2 RID: 4834
		public float order;

		// Token: 0x040012E3 RID: 4835
		public Rectangle frame;

		// Token: 0x040012E4 RID: 4836
		public int achievementIndex;
	}
}
