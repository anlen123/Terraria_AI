using System;

namespace Terraria.GameContent.FishDropRules
{
	// Token: 0x0200047F RID: 1151
	public abstract class FishRarityCondition
	{
		// Token: 0x06003338 RID: 13112
		public abstract bool Matches(FishingContext context);

		// Token: 0x04005899 RID: 22681
		public float FrequencyOfAppearanceForVisuals;

		// Token: 0x0400589A RID: 22682
		public bool HackedIsAny;
	}
}
