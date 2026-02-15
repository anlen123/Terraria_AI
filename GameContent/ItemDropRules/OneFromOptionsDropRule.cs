using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x0200030F RID: 783
	public class OneFromOptionsDropRule : IItemDropRule
	{
		// Token: 0x17000397 RID: 919
		// (get) Token: 0x060026EA RID: 9962 RVA: 0x0055FCB5 File Offset: 0x0055DEB5
		// (set) Token: 0x060026EB RID: 9963 RVA: 0x0055FCBD File Offset: 0x0055DEBD
		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		// Token: 0x060026EC RID: 9964 RVA: 0x0055FCC6 File Offset: 0x0055DEC6
		public OneFromOptionsDropRule(int chanceDenominator, int chanceNumerator, params int[] options)
		{
			this.chanceDenominator = chanceDenominator;
			this.chanceNumerator = chanceNumerator;
			this.dropIds = options;
			this.ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		// Token: 0x060026ED RID: 9965 RVA: 0x000379F1 File Offset: 0x00035BF1
		public bool CanDrop(DropAttemptInfo info)
		{
			return true;
		}

		// Token: 0x060026EE RID: 9966 RVA: 0x0055FCF0 File Offset: 0x0055DEF0
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			if (info.player.RollLuck(this.chanceDenominator) < this.chanceNumerator)
			{
				CommonCode.DropItemFromNPC(info.npc, this.dropIds[info.rng.Next(this.dropIds.Length)], 1, false);
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

		// Token: 0x060026EF RID: 9967 RVA: 0x0055FD60 File Offset: 0x0055DF60
		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			float num = (float)this.chanceNumerator / (float)this.chanceDenominator;
			float num2 = num * ratesInfo.parentDroprateChance;
			float dropRate = 1f / (float)this.dropIds.Length * num2;
			for (int i = 0; i < this.dropIds.Length; i++)
			{
				drops.Add(new DropRateInfo(this.dropIds[i], 1, 1, dropRate, ratesInfo.conditions));
			}
			Chains.ReportDroprates(this.ChainedRules, num, drops, ratesInfo);
		}

		// Token: 0x0400509B RID: 20635
		public int[] dropIds;

		// Token: 0x0400509C RID: 20636
		public int chanceDenominator;

		// Token: 0x0400509D RID: 20637
		public int chanceNumerator;
	}
}
