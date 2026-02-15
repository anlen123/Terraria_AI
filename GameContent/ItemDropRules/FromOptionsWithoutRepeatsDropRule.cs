using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x02000310 RID: 784
	public class FromOptionsWithoutRepeatsDropRule : IItemDropRule
	{
		// Token: 0x17000398 RID: 920
		// (get) Token: 0x060026F0 RID: 9968 RVA: 0x0055FDD5 File Offset: 0x0055DFD5
		// (set) Token: 0x060026F1 RID: 9969 RVA: 0x0055FDDD File Offset: 0x0055DFDD
		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		// Token: 0x060026F2 RID: 9970 RVA: 0x0055FDE6 File Offset: 0x0055DFE6
		public FromOptionsWithoutRepeatsDropRule(int dropCount, params int[] options)
		{
			this.dropCount = dropCount;
			this.dropIds = options;
			this.ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x000379F1 File Offset: 0x00035BF1
		public bool CanDrop(DropAttemptInfo info)
		{
			return true;
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x0055FE14 File Offset: 0x0055E014
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			this._temporaryAvailableItems.Clear();
			this._temporaryAvailableItems.AddRange(this.dropIds);
			int num = 0;
			while (num < this.dropCount && this._temporaryAvailableItems.Count > 0)
			{
				int index = info.rng.Next(this._temporaryAvailableItems.Count);
				CommonCode.DropItemFromNPC(info.npc, this._temporaryAvailableItems[index], 1, false);
				this._temporaryAvailableItems.RemoveAt(index);
				num++;
			}
			return new ItemDropAttemptResult
			{
				State = ItemDropAttemptResultState.Success
			};
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x0055FEAC File Offset: 0x0055E0AC
		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			float parentDroprateChance = ratesInfo.parentDroprateChance;
			int num = this.dropIds.Length;
			float num2 = 1f;
			int num3 = 0;
			while (num3 < this.dropCount && num > 0)
			{
				num2 *= (float)(num - 1) / (float)num;
				num3++;
				num--;
			}
			float dropRate = (1f - num2) * parentDroprateChance;
			for (int i = 0; i < this.dropIds.Length; i++)
			{
				drops.Add(new DropRateInfo(this.dropIds[i], 1, 1, dropRate, ratesInfo.conditions));
			}
			Chains.ReportDroprates(this.ChainedRules, 1f, drops, ratesInfo);
		}

		// Token: 0x0400509F RID: 20639
		public int[] dropIds;

		// Token: 0x040050A0 RID: 20640
		public int dropCount;

		// Token: 0x040050A2 RID: 20642
		private List<int> _temporaryAvailableItems = new List<int>();
	}
}
