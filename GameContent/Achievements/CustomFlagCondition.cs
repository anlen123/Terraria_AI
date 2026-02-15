using System;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
	// Token: 0x02000284 RID: 644
	public class CustomFlagCondition : AchievementCondition
	{
		// Token: 0x060024D1 RID: 9425 RVA: 0x00551F6F File Offset: 0x0055016F
		private CustomFlagCondition(string name) : base(name)
		{
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x00551F78 File Offset: 0x00550178
		public static AchievementCondition Create(string name)
		{
			return new CustomFlagCondition(name);
		}
	}
}
