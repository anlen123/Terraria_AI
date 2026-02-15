using System;

namespace Terraria.GameContent.FishDropRules
{
	// Token: 0x02000477 RID: 1143
	public class FishDropRule
	{
		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06003309 RID: 13065 RVA: 0x005F29A0 File Offset: 0x005F0BA0
		public bool IsStopper
		{
			get
			{
				return this.PossibleItems.Length == 0 || (this.Rarity.HackedIsAny && this.ChanceDenominator == this.ChanceNumerator);
			}
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x005F29CC File Offset: 0x005F0BCC
		public bool Attempt(FishingContext context, out int resultItemType)
		{
			resultItemType = 0;
			if (!this.MeetsConditions(context, false))
			{
				return false;
			}
			if (context.Random.Next(this.ChanceDenominator) >= this.ChanceNumerator)
			{
				return false;
			}
			if (!this.Rarity.Matches(context))
			{
				return false;
			}
			if (this.PossibleItems != null && this.PossibleItems.Length != 0)
			{
				resultItemType = context.Random.NextFromList(this.PossibleItems);
			}
			return true;
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x005F2A38 File Offset: 0x005F0C38
		public bool MeetsConditions(FishingContext context, bool forDisplay)
		{
			AFishingCondition[] conditions = this.Conditions;
			for (int i = 0; i < conditions.Length; i++)
			{
				if (!conditions[i].Matches(context))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400585E RID: 22622
		public int[] PossibleItems;

		// Token: 0x0400585F RID: 22623
		public int ChanceNumerator = 1;

		// Token: 0x04005860 RID: 22624
		public int ChanceDenominator = 1;

		// Token: 0x04005861 RID: 22625
		public AFishingCondition[] Conditions;

		// Token: 0x04005862 RID: 22626
		public FishRarityCondition Rarity;
	}
}
