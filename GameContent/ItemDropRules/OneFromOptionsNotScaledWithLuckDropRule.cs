using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x0200030E RID: 782
	public class OneFromOptionsNotScaledWithLuckDropRule : IItemDropRule
	{
		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060026E4 RID: 9956 RVA: 0x0055FB96 File Offset: 0x0055DD96
		// (set) Token: 0x060026E5 RID: 9957 RVA: 0x0055FB9E File Offset: 0x0055DD9E
		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		// Token: 0x060026E6 RID: 9958 RVA: 0x0055FBA7 File Offset: 0x0055DDA7
		public OneFromOptionsNotScaledWithLuckDropRule(int chanceDenominator, int chanceNumerator, params int[] options)
		{
			this.chanceDenominator = chanceDenominator;
			this.dropIds = options;
			this.chanceNumerator = chanceNumerator;
			this.ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x000379F1 File Offset: 0x00035BF1
		public bool CanDrop(DropAttemptInfo info)
		{
			return true;
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x0055FBD0 File Offset: 0x0055DDD0
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			if (info.rng.Next(this.chanceDenominator) < this.chanceNumerator)
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

		// Token: 0x060026E9 RID: 9961 RVA: 0x0055FC40 File Offset: 0x0055DE40
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

		// Token: 0x04005097 RID: 20631
		public int[] dropIds;

		// Token: 0x04005098 RID: 20632
		public int chanceDenominator;

		// Token: 0x04005099 RID: 20633
		public int chanceNumerator;
	}
}
