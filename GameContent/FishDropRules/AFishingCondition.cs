using System;

namespace Terraria.GameContent.FishDropRules
{
	// Token: 0x0200047E RID: 1150
	public abstract class AFishingCondition
	{
		// Token: 0x06003336 RID: 13110
		public abstract bool Matches(FishingContext context);

		// Token: 0x04005898 RID: 22680
		public bool CanBeSkippedForDisplay;
	}
}
