using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x02000311 RID: 785
	public class OneFromRulesRule : IItemDropRule, INestedItemDropRule
	{
		// Token: 0x17000399 RID: 921
		// (get) Token: 0x060026F6 RID: 9974 RVA: 0x0055FF46 File Offset: 0x0055E146
		// (set) Token: 0x060026F7 RID: 9975 RVA: 0x0055FF4E File Offset: 0x0055E14E
		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		// Token: 0x060026F8 RID: 9976 RVA: 0x0055FF57 File Offset: 0x0055E157
		public OneFromRulesRule(int chanceDenominator, params IItemDropRule[] options)
		{
			this.chanceDenominator = chanceDenominator;
			this.options = options;
			this.ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x000379F1 File Offset: 0x00035BF1
		public bool CanDrop(DropAttemptInfo info)
		{
			return true;
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x0055FF78 File Offset: 0x0055E178
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			return new ItemDropAttemptResult
			{
				State = ItemDropAttemptResultState.DidNotRunCode
			};
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x0055FF98 File Offset: 0x0055E198
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info, ItemDropRuleResolveAction resolveAction)
		{
			if (info.rng.Next(this.chanceDenominator) == 0)
			{
				int num = info.rng.Next(this.options.Length);
				resolveAction(this.options[num], info);
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

		// Token: 0x060026FC RID: 9980 RVA: 0x00560004 File Offset: 0x0055E204
		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			float num = 1f / (float)this.chanceDenominator;
			float multiplier = 1f / (float)this.options.Length * num;
			for (int i = 0; i < this.options.Length; i++)
			{
				this.options[i].ReportDroprates(drops, ratesInfo.With(multiplier));
			}
			Chains.ReportDroprates(this.ChainedRules, num, drops, ratesInfo);
		}

		// Token: 0x040050A3 RID: 20643
		public IItemDropRule[] options;

		// Token: 0x040050A4 RID: 20644
		public int chanceDenominator;
	}
}
