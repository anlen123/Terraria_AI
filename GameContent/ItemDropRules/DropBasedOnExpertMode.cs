using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x02000305 RID: 773
	public class DropBasedOnExpertMode : IItemDropRule, INestedItemDropRule
	{
		// Token: 0x1700038E RID: 910
		// (get) Token: 0x060026AD RID: 9901 RVA: 0x0055F057 File Offset: 0x0055D257
		// (set) Token: 0x060026AE RID: 9902 RVA: 0x0055F05F File Offset: 0x0055D25F
		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		// Token: 0x060026AF RID: 9903 RVA: 0x0055F068 File Offset: 0x0055D268
		public DropBasedOnExpertMode(IItemDropRule ruleForNormalMode, IItemDropRule ruleForExpertMode)
		{
			this.ruleForNormalMode = ruleForNormalMode;
			this.ruleForExpertMode = ruleForExpertMode;
			this.ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x0055F089 File Offset: 0x0055D289
		public bool CanDrop(DropAttemptInfo info)
		{
			if (info.IsExpertMode)
			{
				return this.ruleForExpertMode.CanDrop(info);
			}
			return this.ruleForNormalMode.CanDrop(info);
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x0055F0AC File Offset: 0x0055D2AC
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			return new ItemDropAttemptResult
			{
				State = ItemDropAttemptResultState.DidNotRunCode
			};
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x0055F0CA File Offset: 0x0055D2CA
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info, ItemDropRuleResolveAction resolveAction)
		{
			if (info.IsExpertMode)
			{
				return resolveAction(this.ruleForExpertMode, info);
			}
			return resolveAction(this.ruleForNormalMode, info);
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x0055F0F0 File Offset: 0x0055D2F0
		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			DropRateInfoChainFeed ratesInfo2 = ratesInfo.With(1f);
			ratesInfo2.AddCondition(new Conditions.IsExpert());
			this.ruleForExpertMode.ReportDroprates(drops, ratesInfo2);
			DropRateInfoChainFeed ratesInfo3 = ratesInfo.With(1f);
			ratesInfo3.AddCondition(new Conditions.NotExpert());
			this.ruleForNormalMode.ReportDroprates(drops, ratesInfo3);
			Chains.ReportDroprates(this.ChainedRules, 1f, drops, ratesInfo);
		}

		// Token: 0x04005083 RID: 20611
		public IItemDropRule ruleForNormalMode;

		// Token: 0x04005084 RID: 20612
		public IItemDropRule ruleForExpertMode;
	}
}
