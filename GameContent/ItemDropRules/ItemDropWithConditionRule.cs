using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x0200030B RID: 779
	public class ItemDropWithConditionRule : CommonDrop
	{
		// Token: 0x060026D5 RID: 9941 RVA: 0x0055F8DF File Offset: 0x0055DADF
		public ItemDropWithConditionRule(int itemId, int chanceDenominator, int amountDroppedMinimum, int amountDroppedMaximum, IItemDropRuleCondition condition, int chanceNumerator = 1) : base(itemId, chanceDenominator, amountDroppedMinimum, amountDroppedMaximum, chanceNumerator)
		{
			this.condition = condition;
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x0055F8F6 File Offset: 0x0055DAF6
		public override bool CanDrop(DropAttemptInfo info)
		{
			return this.condition.CanDrop(info);
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x0055F904 File Offset: 0x0055DB04
		public override void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			DropRateInfoChainFeed dropRateInfoChainFeed = ratesInfo.With(1f);
			dropRateInfoChainFeed.AddCondition(this.condition);
			float num = (float)this.chanceNumerator / (float)this.chanceDenominator;
			float dropRate = num * dropRateInfoChainFeed.parentDroprateChance;
			drops.Add(new DropRateInfo(this.itemId, this.amountDroppedMinimum, this.amountDroppedMaximum, dropRate, dropRateInfoChainFeed.conditions));
			Chains.ReportDroprates(base.ChainedRules, num, drops, dropRateInfoChainFeed);
		}

		// Token: 0x04005091 RID: 20625
		public IItemDropRuleCondition condition;
	}
}
