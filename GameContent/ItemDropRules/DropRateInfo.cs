using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x020002F3 RID: 755
	public struct DropRateInfo
	{
		// Token: 0x06002678 RID: 9848 RVA: 0x0055E5AC File Offset: 0x0055C7AC
		public DropRateInfo(int itemId, int stackMin, int stackMax, float dropRate, List<IItemDropRuleCondition> conditions = null)
		{
			this.itemId = itemId;
			this.stackMin = stackMin;
			this.stackMax = stackMax;
			this.dropRate = dropRate;
			this.conditions = null;
			if (conditions != null && conditions.Count > 0)
			{
				this.conditions = new List<IItemDropRuleCondition>(conditions);
			}
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x0055E5F8 File Offset: 0x0055C7F8
		public void AddCondition(IItemDropRuleCondition condition)
		{
			if (this.conditions == null)
			{
				this.conditions = new List<IItemDropRuleCondition>();
			}
			this.conditions.Add(condition);
		}

		// Token: 0x04005069 RID: 20585
		public int itemId;

		// Token: 0x0400506A RID: 20586
		public int stackMin;

		// Token: 0x0400506B RID: 20587
		public int stackMax;

		// Token: 0x0400506C RID: 20588
		public float dropRate;

		// Token: 0x0400506D RID: 20589
		public List<IItemDropRuleCondition> conditions;
	}
}
