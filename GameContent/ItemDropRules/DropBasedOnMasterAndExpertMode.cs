using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x02000307 RID: 775
	public class DropBasedOnMasterAndExpertMode : IItemDropRule, INestedItemDropRule
	{
		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060026BB RID: 9915 RVA: 0x0055F25F File Offset: 0x0055D45F
		// (set) Token: 0x060026BC RID: 9916 RVA: 0x0055F267 File Offset: 0x0055D467
		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		// Token: 0x060026BD RID: 9917 RVA: 0x0055F270 File Offset: 0x0055D470
		public DropBasedOnMasterAndExpertMode(IItemDropRule ruleForDefault, IItemDropRule ruleForExpertMode, IItemDropRule ruleForMasterMode)
		{
			this.ruleForDefault = ruleForDefault;
			this.ruleForExpertmode = ruleForExpertMode;
			this.ruleForMasterMode = ruleForMasterMode;
			this.ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x0055F298 File Offset: 0x0055D498
		public bool CanDrop(DropAttemptInfo info)
		{
			if (info.IsMasterMode)
			{
				return this.ruleForMasterMode.CanDrop(info);
			}
			if (info.IsExpertMode)
			{
				return this.ruleForExpertmode.CanDrop(info);
			}
			return this.ruleForDefault.CanDrop(info);
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x0055F2D0 File Offset: 0x0055D4D0
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			return new ItemDropAttemptResult
			{
				State = ItemDropAttemptResultState.DidNotRunCode
			};
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x0055F2EE File Offset: 0x0055D4EE
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info, ItemDropRuleResolveAction resolveAction)
		{
			if (info.IsMasterMode)
			{
				return resolveAction(this.ruleForMasterMode, info);
			}
			if (info.IsExpertMode)
			{
				return resolveAction(this.ruleForExpertmode, info);
			}
			return resolveAction(this.ruleForDefault, info);
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x0055F32C File Offset: 0x0055D52C
		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			DropRateInfoChainFeed ratesInfo2 = ratesInfo.With(1f);
			ratesInfo2.AddCondition(new Conditions.IsMasterMode());
			this.ruleForMasterMode.ReportDroprates(drops, ratesInfo2);
			DropRateInfoChainFeed ratesInfo3 = ratesInfo.With(1f);
			ratesInfo3.AddCondition(new Conditions.NotMasterMode());
			ratesInfo3.AddCondition(new Conditions.IsExpert());
			this.ruleForExpertmode.ReportDroprates(drops, ratesInfo3);
			DropRateInfoChainFeed ratesInfo4 = ratesInfo.With(1f);
			ratesInfo4.AddCondition(new Conditions.NotMasterMode());
			ratesInfo4.AddCondition(new Conditions.NotExpert());
			this.ruleForDefault.ReportDroprates(drops, ratesInfo4);
			Chains.ReportDroprates(this.ChainedRules, 1f, drops, ratesInfo);
		}

		// Token: 0x04005089 RID: 20617
		public IItemDropRule ruleForDefault;

		// Token: 0x0400508A RID: 20618
		public IItemDropRule ruleForExpertmode;

		// Token: 0x0400508B RID: 20619
		public IItemDropRule ruleForMasterMode;
	}
}
