using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x020002F5 RID: 757
	public interface IItemDropRule
	{
		// Token: 0x17000389 RID: 905
		// (get) Token: 0x0600267D RID: 9853
		List<IItemDropRuleChainAttempt> ChainedRules { get; }

		// Token: 0x0600267E RID: 9854
		bool CanDrop(DropAttemptInfo info);

		// Token: 0x0600267F RID: 9855
		void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo);

		// Token: 0x06002680 RID: 9856
		ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info);
	}
}
