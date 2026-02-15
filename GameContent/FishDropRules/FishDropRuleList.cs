using System;
using System.Collections.Generic;

namespace Terraria.GameContent.FishDropRules
{
	// Token: 0x02000479 RID: 1145
	public class FishDropRuleList
	{
		// Token: 0x0600330D RID: 13069 RVA: 0x005F2A80 File Offset: 0x005F0C80
		public int TryGetItemDropType(FishingContext context)
		{
			int result = 0;
			for (int i = 0; i < this._rules.Count; i++)
			{
				if (this._rules[i].Attempt(context, out result))
				{
					return result;
				}
			}
			return 0;
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x005F2AC0 File Offset: 0x005F0CC0
		public void GetDisplayableDrops(FishingContext context, List<FishPossibilityEntry> resultTypes)
		{
			for (int i = 0; i < this._rules.Count; i++)
			{
				FishDropRule fishDropRule = this._rules[i];
				if (fishDropRule.MeetsConditions(context, true))
				{
					int itemType = 0;
					if (fishDropRule.PossibleItems.Length != 0)
					{
						itemType = context.Random.NextFromList(fishDropRule.PossibleItems);
					}
					resultTypes.Add(new FishPossibilityEntry
					{
						ItemType = itemType,
						Frequency = fishDropRule.Rarity.FrequencyOfAppearanceForVisuals
					});
					if (fishDropRule.IsStopper)
					{
						break;
					}
				}
			}
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x005F2B48 File Offset: 0x005F0D48
		public void Add(FishDropRule rule)
		{
			this.Validate(rule);
			this._rules.Add(rule);
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x005F2B5D File Offset: 0x005F0D5D
		private void Validate(FishDropRule rule)
		{
			if (rule.ChanceDenominator <= 0)
			{
				throw new ArgumentOutOfRangeException("FishDropRule.ChanceDenominator", "Chance Denominator must be positive non-zero number");
			}
		}

		// Token: 0x04005865 RID: 22629
		private List<FishDropRule> _rules = new List<FishDropRule>();
	}
}
