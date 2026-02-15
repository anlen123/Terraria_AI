using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x02000315 RID: 789
	public class ItemDropResolver
	{
		// Token: 0x0600273B RID: 10043 RVA: 0x00565EC0 File Offset: 0x005640C0
		public ItemDropResolver(ItemDropDatabase database)
		{
			this._database = database;
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x00565ED0 File Offset: 0x005640D0
		public void TryDropping(DropAttemptInfo info)
		{
			List<IItemDropRule> rulesForNPCID = this._database.GetRulesForNPCID(info.npc.netID, true);
			for (int i = 0; i < rulesForNPCID.Count; i++)
			{
				this.ResolveRule(rulesForNPCID[i], info);
			}
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x00565F18 File Offset: 0x00564118
		private ItemDropAttemptResult ResolveRule(IItemDropRule rule, DropAttemptInfo info)
		{
			if (!rule.CanDrop(info))
			{
				ItemDropAttemptResult itemDropAttemptResult = new ItemDropAttemptResult
				{
					State = ItemDropAttemptResultState.DoesntFillConditions
				};
				this.ResolveRuleChains(rule, info, itemDropAttemptResult);
				return itemDropAttemptResult;
			}
			INestedItemDropRule nestedItemDropRule = rule as INestedItemDropRule;
			ItemDropAttemptResult itemDropAttemptResult2;
			if (nestedItemDropRule != null)
			{
				itemDropAttemptResult2 = nestedItemDropRule.TryDroppingItem(info, new ItemDropRuleResolveAction(this.ResolveRule));
			}
			else
			{
				itemDropAttemptResult2 = rule.TryDroppingItem(info);
			}
			this.ResolveRuleChains(rule, info, itemDropAttemptResult2);
			return itemDropAttemptResult2;
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x00565F7D File Offset: 0x0056417D
		private void ResolveRuleChains(IItemDropRule rule, DropAttemptInfo info, ItemDropAttemptResult parentResult)
		{
			this.ResolveRuleChains(ref info, ref parentResult, rule.ChainedRules);
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x00565F90 File Offset: 0x00564190
		private void ResolveRuleChains(ref DropAttemptInfo info, ref ItemDropAttemptResult parentResult, List<IItemDropRuleChainAttempt> ruleChains)
		{
			if (ruleChains == null)
			{
				return;
			}
			for (int i = 0; i < ruleChains.Count; i++)
			{
				IItemDropRuleChainAttempt itemDropRuleChainAttempt = ruleChains[i];
				if (itemDropRuleChainAttempt.CanChainIntoRule(parentResult))
				{
					this.ResolveRule(itemDropRuleChainAttempt.RuleToChain, info);
				}
			}
		}

		// Token: 0x040050AA RID: 20650
		private ItemDropDatabase _database;
	}
}
