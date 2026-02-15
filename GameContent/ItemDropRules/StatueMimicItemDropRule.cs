using System;
using System.Collections.Generic;
using Terraria.ID;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x02000309 RID: 777
	public class StatueMimicItemDropRule : IItemDropRule
	{
		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060026C8 RID: 9928 RVA: 0x0055F59A File Offset: 0x0055D79A
		// (set) Token: 0x060026C9 RID: 9929 RVA: 0x0055F5A2 File Offset: 0x0055D7A2
		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		// Token: 0x060026CA RID: 9930 RVA: 0x0055F5AB File Offset: 0x0055D7AB
		public StatueMimicItemDropRule()
		{
			this.ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x0055F5BE File Offset: 0x0055D7BE
		public bool CanDrop(DropAttemptInfo info)
		{
			return info.npc.ai[1] > 0f && info.npc.ai[1] < (float)ItemID.Count;
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x0055F5EB File Offset: 0x0055D7EB
		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			Chains.ReportDroprates(this.ChainedRules, 1f, drops, ratesInfo);
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x0055F600 File Offset: 0x0055D800
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			int itemId = WorldGen.StatueStyleToItem((int)info.npc.ai[1]);
			CommonCode.DropItemFromNPC(info.npc, itemId, 1, false);
			return new ItemDropAttemptResult
			{
				State = ItemDropAttemptResultState.Success
			};
		}
	}
}
