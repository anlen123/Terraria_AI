using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x02000303 RID: 771
	public class DropNothing : IItemDropRule
	{
		// Token: 0x1700038C RID: 908
		// (get) Token: 0x060026A0 RID: 9888 RVA: 0x0055EEF9 File Offset: 0x0055D0F9
		// (set) Token: 0x060026A1 RID: 9889 RVA: 0x0055EF01 File Offset: 0x0055D101
		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		// Token: 0x060026A2 RID: 9890 RVA: 0x0055EF0A File Offset: 0x0055D10A
		public DropNothing()
		{
			this.ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public bool CanDrop(DropAttemptInfo info)
		{
			return false;
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x0055EF20 File Offset: 0x0055D120
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			return new ItemDropAttemptResult
			{
				State = ItemDropAttemptResultState.DoesntFillConditions
			};
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x0055EF3E File Offset: 0x0055D13E
		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			Chains.ReportDroprates(this.ChainedRules, 1f, drops, ratesInfo);
		}
	}
}
