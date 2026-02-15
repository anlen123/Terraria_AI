using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x0200030C RID: 780
	public class LeadingConditionRule : IItemDropRule
	{
		// Token: 0x17000394 RID: 916
		// (get) Token: 0x060026D8 RID: 9944 RVA: 0x0055F976 File Offset: 0x0055DB76
		// (set) Token: 0x060026D9 RID: 9945 RVA: 0x0055F97E File Offset: 0x0055DB7E
		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		// Token: 0x060026DA RID: 9946 RVA: 0x0055F987 File Offset: 0x0055DB87
		public LeadingConditionRule(IItemDropRuleCondition condition)
		{
			this.condition = condition;
			this.ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		// Token: 0x060026DB RID: 9947 RVA: 0x0055F9A1 File Offset: 0x0055DBA1
		public bool CanDrop(DropAttemptInfo info)
		{
			return this.condition.CanDrop(info);
		}

		// Token: 0x060026DC RID: 9948 RVA: 0x0055F9AF File Offset: 0x0055DBAF
		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			ratesInfo.AddCondition(this.condition);
			Chains.ReportDroprates(this.ChainedRules, 1f, drops, ratesInfo);
		}

		// Token: 0x060026DD RID: 9949 RVA: 0x0055F9D0 File Offset: 0x0055DBD0
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			return new ItemDropAttemptResult
			{
				State = ItemDropAttemptResultState.Success
			};
		}

		// Token: 0x04005092 RID: 20626
		public IItemDropRuleCondition condition;
	}
}
