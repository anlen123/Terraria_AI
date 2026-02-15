using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x02000306 RID: 774
	public class DropBasedOnMasterMode : IItemDropRule, INestedItemDropRule
	{
		// Token: 0x1700038F RID: 911
		// (get) Token: 0x060026B4 RID: 9908 RVA: 0x0055F15B File Offset: 0x0055D35B
		// (set) Token: 0x060026B5 RID: 9909 RVA: 0x0055F163 File Offset: 0x0055D363
		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		// Token: 0x060026B6 RID: 9910 RVA: 0x0055F16C File Offset: 0x0055D36C
		public DropBasedOnMasterMode(IItemDropRule ruleForDefault, IItemDropRule ruleForMasterMode)
		{
			this.ruleForDefault = ruleForDefault;
			this.ruleForMasterMode = ruleForMasterMode;
			this.ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x0055F18D File Offset: 0x0055D38D
		public bool CanDrop(DropAttemptInfo info)
		{
			if (info.IsMasterMode)
			{
				return this.ruleForMasterMode.CanDrop(info);
			}
			return this.ruleForDefault.CanDrop(info);
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x0055F1B0 File Offset: 0x0055D3B0
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			return new ItemDropAttemptResult
			{
				State = ItemDropAttemptResultState.DidNotRunCode
			};
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x0055F1CE File Offset: 0x0055D3CE
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info, ItemDropRuleResolveAction resolveAction)
		{
			if (info.IsMasterMode)
			{
				return resolveAction(this.ruleForMasterMode, info);
			}
			return resolveAction(this.ruleForDefault, info);
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x0055F1F4 File Offset: 0x0055D3F4
		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			DropRateInfoChainFeed ratesInfo2 = ratesInfo.With(1f);
			ratesInfo2.AddCondition(new Conditions.IsMasterMode());
			this.ruleForMasterMode.ReportDroprates(drops, ratesInfo2);
			DropRateInfoChainFeed ratesInfo3 = ratesInfo.With(1f);
			ratesInfo3.AddCondition(new Conditions.NotMasterMode());
			this.ruleForDefault.ReportDroprates(drops, ratesInfo3);
			Chains.ReportDroprates(this.ChainedRules, 1f, drops, ratesInfo);
		}

		// Token: 0x04005086 RID: 20614
		public IItemDropRule ruleForDefault;

		// Token: 0x04005087 RID: 20615
		public IItemDropRule ruleForMasterMode;
	}
}
