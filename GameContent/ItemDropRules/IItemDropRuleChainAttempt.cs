using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x020002F9 RID: 761
	public interface IItemDropRuleChainAttempt
	{
		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06002682 RID: 9858
		IItemDropRule RuleToChain { get; }

		// Token: 0x06002683 RID: 9859
		bool CanChainIntoRule(ItemDropAttemptResult parentResult);

		// Token: 0x06002684 RID: 9860
		void ReportDroprates(float personalDropRate, List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo);
	}
}
