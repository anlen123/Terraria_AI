using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x02000308 RID: 776
	public class MechBossSpawnersDropRule : IItemDropRule
	{
		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060026C2 RID: 9922 RVA: 0x0055F3D5 File Offset: 0x0055D5D5
		// (set) Token: 0x060026C3 RID: 9923 RVA: 0x0055F3DD File Offset: 0x0055D5DD
		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		// Token: 0x060026C4 RID: 9924 RVA: 0x0055F3E6 File Offset: 0x0055D5E6
		public MechBossSpawnersDropRule()
		{
			this.ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x0055F404 File Offset: 0x0055D604
		public bool CanDrop(DropAttemptInfo info)
		{
			return info.npc.value > 0f && Main.hardMode && (!NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3) && !info.IsInSimulation;
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x0055F440 File Offset: 0x0055D640
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			if (!NPC.downedMechBoss1 && info.player.RollLuck(2500) == 0)
			{
				CommonCode.DropItemFromNPC(info.npc, 556, 1, false);
				return new ItemDropAttemptResult
				{
					State = ItemDropAttemptResultState.Success
				};
			}
			if (!NPC.downedMechBoss2 && info.player.RollLuck(2500) == 0)
			{
				CommonCode.DropItemFromNPC(info.npc, 544, 1, false);
				return new ItemDropAttemptResult
				{
					State = ItemDropAttemptResultState.Success
				};
			}
			if (!NPC.downedMechBoss3 && info.player.RollLuck(2500) == 0)
			{
				CommonCode.DropItemFromNPC(info.npc, 557, 1, false);
				return new ItemDropAttemptResult
				{
					State = ItemDropAttemptResultState.Success
				};
			}
			return new ItemDropAttemptResult
			{
				State = ItemDropAttemptResultState.FailedRandomRoll
			};
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x0055F518 File Offset: 0x0055D718
		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			ratesInfo.AddCondition(this.dummyCondition);
			float num = 0.0004f;
			float dropRate = num * ratesInfo.parentDroprateChance;
			drops.Add(new DropRateInfo(556, 1, 1, dropRate, ratesInfo.conditions));
			drops.Add(new DropRateInfo(544, 1, 1, dropRate, ratesInfo.conditions));
			drops.Add(new DropRateInfo(557, 1, 1, dropRate, ratesInfo.conditions));
			Chains.ReportDroprates(this.ChainedRules, num, drops, ratesInfo);
		}

		// Token: 0x0400508E RID: 20622
		public Conditions.MechanicalBossesDummyCondition dummyCondition = new Conditions.MechanicalBossesDummyCondition();
	}
}
