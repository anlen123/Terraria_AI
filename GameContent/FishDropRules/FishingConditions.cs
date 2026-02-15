using System;

namespace Terraria.GameContent.FishDropRules
{
	// Token: 0x02000480 RID: 1152
	public class FishingConditions
	{
		// Token: 0x02000976 RID: 2422
		public class QuestFishCondition : AFishingCondition
		{
			// Token: 0x0600492B RID: 18731 RVA: 0x006CF5B9 File Offset: 0x006CD7B9
			public override bool Matches(FishingContext context)
			{
				return context.Fisher.questFish == this.CheckedType;
			}

			// Token: 0x040075DC RID: 30172
			public int CheckedType;
		}

		// Token: 0x02000977 RID: 2423
		public class QuestFishConditionRemix : AFishingCondition
		{
			// Token: 0x0600492D RID: 18733 RVA: 0x006CF5D6 File Offset: 0x006CD7D6
			public override bool Matches(FishingContext context)
			{
				return context.Fisher.questFish == this.CheckedType && Main.remixWorld;
			}

			// Token: 0x040075DD RID: 30173
			public int CheckedType;
		}
	}
}
