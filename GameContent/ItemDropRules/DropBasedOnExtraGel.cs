using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x02000304 RID: 772
	public class DropBasedOnExtraGel : IItemDropRule, INestedItemDropRule
	{
		// Token: 0x1700038D RID: 909
		// (get) Token: 0x060026A6 RID: 9894 RVA: 0x0055EF52 File Offset: 0x0055D152
		// (set) Token: 0x060026A7 RID: 9895 RVA: 0x0055EF5A File Offset: 0x0055D15A
		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		// Token: 0x060026A8 RID: 9896 RVA: 0x0055EF63 File Offset: 0x0055D163
		public DropBasedOnExtraGel(IItemDropRule ruleForNormal, IItemDropRule ruleForExtraGel)
		{
			this.ruleForNormal = ruleForNormal;
			this.ruleForExtraGel = ruleForExtraGel;
			this.ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x0055EF84 File Offset: 0x0055D184
		public bool CanDrop(DropAttemptInfo info)
		{
			if (SpecialSeedFeatures.ShouldDropExtraGel)
			{
				return this.ruleForExtraGel.CanDrop(info);
			}
			return this.ruleForNormal.CanDrop(info);
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x0055EFA8 File Offset: 0x0055D1A8
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			return new ItemDropAttemptResult
			{
				State = ItemDropAttemptResultState.DidNotRunCode
			};
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x0055EFC6 File Offset: 0x0055D1C6
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info, ItemDropRuleResolveAction resolveAction)
		{
			if (SpecialSeedFeatures.ShouldDropExtraGel)
			{
				return resolveAction(this.ruleForExtraGel, info);
			}
			return resolveAction(this.ruleForNormal, info);
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x0055EFEC File Offset: 0x0055D1EC
		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			DropRateInfoChainFeed ratesInfo2 = ratesInfo.With(1f);
			ratesInfo2.AddCondition(new Conditions.DropExtraGel());
			this.ruleForExtraGel.ReportDroprates(drops, ratesInfo2);
			DropRateInfoChainFeed ratesInfo3 = ratesInfo.With(1f);
			ratesInfo3.AddCondition(new Conditions.NotDropExtraGel());
			this.ruleForNormal.ReportDroprates(drops, ratesInfo3);
			Chains.ReportDroprates(this.ChainedRules, 1f, drops, ratesInfo);
		}

		// Token: 0x04005080 RID: 20608
		public IItemDropRule ruleForNormal;

		// Token: 0x04005081 RID: 20609
		public IItemDropRule ruleForExtraGel;
	}
}
