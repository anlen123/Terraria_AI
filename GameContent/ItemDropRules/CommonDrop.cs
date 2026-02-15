using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x020002FD RID: 765
	public class CommonDrop : IItemDropRule
	{
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x0600268C RID: 9868 RVA: 0x0055EA74 File Offset: 0x0055CC74
		// (set) Token: 0x0600268D RID: 9869 RVA: 0x0055EA7C File Offset: 0x0055CC7C
		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		// Token: 0x0600268E RID: 9870 RVA: 0x0055EA85 File Offset: 0x0055CC85
		public CommonDrop(int itemId, int chanceDenominator, int amountDroppedMinimum = 1, int amountDroppedMaximum = 1, int chanceNumerator = 1)
		{
			this.itemId = itemId;
			this.chanceDenominator = chanceDenominator;
			this.amountDroppedMinimum = amountDroppedMinimum;
			this.amountDroppedMaximum = amountDroppedMaximum;
			this.chanceNumerator = chanceNumerator;
			this.ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x000379F1 File Offset: 0x00035BF1
		public virtual bool CanDrop(DropAttemptInfo info)
		{
			return true;
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x0055EAC0 File Offset: 0x0055CCC0
		public virtual ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			if (info.player.RollLuck(this.chanceDenominator) < this.chanceNumerator)
			{
				CommonCode.DropItemFromNPC(info.npc, this.itemId, info.rng.Next(this.amountDroppedMinimum, this.amountDroppedMaximum + 1), false);
				return new ItemDropAttemptResult
				{
					State = ItemDropAttemptResultState.Success
				};
			}
			return new ItemDropAttemptResult
			{
				State = ItemDropAttemptResultState.FailedRandomRoll
			};
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x0055EB34 File Offset: 0x0055CD34
		public virtual void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			float num = (float)this.chanceNumerator / (float)this.chanceDenominator;
			float dropRate = num * ratesInfo.parentDroprateChance;
			drops.Add(new DropRateInfo(this.itemId, this.amountDroppedMinimum, this.amountDroppedMaximum, dropRate, ratesInfo.conditions));
			Chains.ReportDroprates(this.ChainedRules, num, drops, ratesInfo);
		}

		// Token: 0x04005076 RID: 20598
		public int itemId;

		// Token: 0x04005077 RID: 20599
		public int chanceDenominator;

		// Token: 0x04005078 RID: 20600
		public int amountDroppedMinimum;

		// Token: 0x04005079 RID: 20601
		public int amountDroppedMaximum;

		// Token: 0x0400507A RID: 20602
		public int chanceNumerator;
	}
}
