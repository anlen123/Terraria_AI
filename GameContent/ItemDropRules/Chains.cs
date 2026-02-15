using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x02000312 RID: 786
	public static class Chains
	{
		// Token: 0x060026FD RID: 9981 RVA: 0x00560068 File Offset: 0x0055E268
		public static void ReportDroprates(List<IItemDropRuleChainAttempt> ChainedRules, float personalDropRate, List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			foreach (IItemDropRuleChainAttempt itemDropRuleChainAttempt in ChainedRules)
			{
				itemDropRuleChainAttempt.ReportDroprates(personalDropRate, drops, ratesInfo);
			}
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x005600B8 File Offset: 0x0055E2B8
		public static IItemDropRule OnFailedRoll(this IItemDropRule rule, IItemDropRule ruleToChain, bool hideLootReport = false)
		{
			rule.ChainedRules.Add(new Chains.TryIfFailedRandomRoll(ruleToChain, hideLootReport));
			return ruleToChain;
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x005600CD File Offset: 0x0055E2CD
		public static IItemDropRule OnSuccess(this IItemDropRule rule, IItemDropRule ruleToChain, bool hideLootReport = false)
		{
			rule.ChainedRules.Add(new Chains.TryIfSucceeded(ruleToChain, hideLootReport));
			return ruleToChain;
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x005600E2 File Offset: 0x0055E2E2
		public static IItemDropRule OnFailedConditions(this IItemDropRule rule, IItemDropRule ruleToChain, bool hideLootReport = false)
		{
			rule.ChainedRules.Add(new Chains.TryIfDoesntFillConditions(ruleToChain, hideLootReport));
			return ruleToChain;
		}

		// Token: 0x0200082D RID: 2093
		public class TryIfFailedRandomRoll : IItemDropRuleChainAttempt
		{
			// Token: 0x17000541 RID: 1345
			// (get) Token: 0x06004335 RID: 17205 RVA: 0x006BF5BC File Offset: 0x006BD7BC
			// (set) Token: 0x06004336 RID: 17206 RVA: 0x006BF5C4 File Offset: 0x006BD7C4
			public IItemDropRule RuleToChain { get; private set; }

			// Token: 0x06004337 RID: 17207 RVA: 0x006BF5CD File Offset: 0x006BD7CD
			public TryIfFailedRandomRoll(IItemDropRule rule, bool hideLootReport = false)
			{
				this.RuleToChain = rule;
				this.hideLootReport = hideLootReport;
			}

			// Token: 0x06004338 RID: 17208 RVA: 0x006BF5E3 File Offset: 0x006BD7E3
			public bool CanChainIntoRule(ItemDropAttemptResult parentResult)
			{
				return parentResult.State == ItemDropAttemptResultState.FailedRandomRoll;
			}

			// Token: 0x06004339 RID: 17209 RVA: 0x006BF5EE File Offset: 0x006BD7EE
			public void ReportDroprates(float personalDropRate, List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
			{
				if (this.hideLootReport)
				{
					return;
				}
				this.RuleToChain.ReportDroprates(drops, ratesInfo.With(1f - personalDropRate));
			}

			// Token: 0x04007240 RID: 29248
			public bool hideLootReport;
		}

		// Token: 0x0200082E RID: 2094
		public class TryIfSucceeded : IItemDropRuleChainAttempt
		{
			// Token: 0x17000542 RID: 1346
			// (get) Token: 0x0600433A RID: 17210 RVA: 0x006BF613 File Offset: 0x006BD813
			// (set) Token: 0x0600433B RID: 17211 RVA: 0x006BF61B File Offset: 0x006BD81B
			public IItemDropRule RuleToChain { get; private set; }

			// Token: 0x0600433C RID: 17212 RVA: 0x006BF624 File Offset: 0x006BD824
			public TryIfSucceeded(IItemDropRule rule, bool hideLootReport = false)
			{
				this.RuleToChain = rule;
				this.hideLootReport = hideLootReport;
			}

			// Token: 0x0600433D RID: 17213 RVA: 0x006BF63A File Offset: 0x006BD83A
			public bool CanChainIntoRule(ItemDropAttemptResult parentResult)
			{
				return parentResult.State == ItemDropAttemptResultState.Success;
			}

			// Token: 0x0600433E RID: 17214 RVA: 0x006BF645 File Offset: 0x006BD845
			public void ReportDroprates(float personalDropRate, List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
			{
				if (this.hideLootReport)
				{
					return;
				}
				this.RuleToChain.ReportDroprates(drops, ratesInfo.With(personalDropRate));
			}

			// Token: 0x04007242 RID: 29250
			public bool hideLootReport;
		}

		// Token: 0x0200082F RID: 2095
		public class TryIfDoesntFillConditions : IItemDropRuleChainAttempt
		{
			// Token: 0x17000543 RID: 1347
			// (get) Token: 0x0600433F RID: 17215 RVA: 0x006BF664 File Offset: 0x006BD864
			// (set) Token: 0x06004340 RID: 17216 RVA: 0x006BF66C File Offset: 0x006BD86C
			public IItemDropRule RuleToChain { get; private set; }

			// Token: 0x06004341 RID: 17217 RVA: 0x006BF675 File Offset: 0x006BD875
			public TryIfDoesntFillConditions(IItemDropRule rule, bool hideLootReport = false)
			{
				this.RuleToChain = rule;
				this.hideLootReport = hideLootReport;
			}

			// Token: 0x06004342 RID: 17218 RVA: 0x006BF68B File Offset: 0x006BD88B
			public bool CanChainIntoRule(ItemDropAttemptResult parentResult)
			{
				return parentResult.State == ItemDropAttemptResultState.DoesntFillConditions;
			}

			// Token: 0x06004343 RID: 17219 RVA: 0x006BF696 File Offset: 0x006BD896
			public void ReportDroprates(float personalDropRate, List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
			{
				if (this.hideLootReport)
				{
					return;
				}
				this.RuleToChain.ReportDroprates(drops, ratesInfo.With(personalDropRate));
			}

			// Token: 0x04007244 RID: 29252
			public bool hideLootReport;
		}
	}
}
