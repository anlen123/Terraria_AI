using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x020002F4 RID: 756
	public struct DropRateInfoChainFeed
	{
		// Token: 0x0600267A RID: 9850 RVA: 0x0055E619 File Offset: 0x0055C819
		public void AddCondition(IItemDropRuleCondition condition)
		{
			if (this.conditions == null)
			{
				this.conditions = new List<IItemDropRuleCondition>();
			}
			this.conditions.Add(condition);
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x0055E63A File Offset: 0x0055C83A
		public DropRateInfoChainFeed(float droprate)
		{
			this.parentDroprateChance = droprate;
			this.conditions = null;
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x0055E64C File Offset: 0x0055C84C
		public DropRateInfoChainFeed With(float multiplier)
		{
			DropRateInfoChainFeed result = new DropRateInfoChainFeed(this.parentDroprateChance * multiplier);
			if (this.conditions != null)
			{
				result.conditions = new List<IItemDropRuleCondition>(this.conditions);
			}
			return result;
		}

		// Token: 0x0400506E RID: 20590
		public float parentDroprateChance;

		// Token: 0x0400506F RID: 20591
		public List<IItemDropRuleCondition> conditions;
	}
}
