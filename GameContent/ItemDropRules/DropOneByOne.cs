using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x0200030D RID: 781
	public class DropOneByOne : IItemDropRule
	{
		// Token: 0x17000395 RID: 917
		// (get) Token: 0x060026DE RID: 9950 RVA: 0x0055F9EE File Offset: 0x0055DBEE
		// (set) Token: 0x060026DF RID: 9951 RVA: 0x0055F9F6 File Offset: 0x0055DBF6
		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		// Token: 0x060026E0 RID: 9952 RVA: 0x0055F9FF File Offset: 0x0055DBFF
		public DropOneByOne(int itemId, DropOneByOne.Parameters parameters)
		{
			this.ChainedRules = new List<IItemDropRuleChainAttempt>();
			this.parameters = parameters;
			this.itemId = itemId;
		}

		// Token: 0x060026E1 RID: 9953 RVA: 0x0055FA20 File Offset: 0x0055DC20
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			if (info.player.RollLuck(this.parameters.ChanceDenominator) < this.parameters.ChanceNumerator)
			{
				int num = info.rng.Next(this.parameters.MinimumItemDropsCount, this.parameters.MaximumItemDropsCount + 1);
				int activePlayersCount = Main.CurrentFrameFlags.ActivePlayersCount;
				int minValue = this.parameters.MinimumStackPerChunkBase + activePlayersCount * this.parameters.BonusMinDropsPerChunkPerPlayer;
				int num2 = this.parameters.MaximumStackPerChunkBase + activePlayersCount * this.parameters.BonusMaxDropsPerChunkPerPlayer;
				for (int i = 0; i < num; i++)
				{
					CommonCode.DropItemFromNPC(info.npc, this.itemId, info.rng.Next(minValue, num2 + 1), true);
				}
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

		// Token: 0x060026E2 RID: 9954 RVA: 0x0055FB08 File Offset: 0x0055DD08
		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			float personalDropRate = this.parameters.GetPersonalDropRate();
			float dropRate = personalDropRate * ratesInfo.parentDroprateChance;
			drops.Add(new DropRateInfo(this.itemId, this.parameters.MinimumItemDropsCount * (this.parameters.MinimumStackPerChunkBase + this.parameters.BonusMinDropsPerChunkPerPlayer), this.parameters.MaximumItemDropsCount * (this.parameters.MaximumStackPerChunkBase + this.parameters.BonusMaxDropsPerChunkPerPlayer), dropRate, ratesInfo.conditions));
			Chains.ReportDroprates(this.ChainedRules, personalDropRate, drops, ratesInfo);
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x000379F1 File Offset: 0x00035BF1
		public bool CanDrop(DropAttemptInfo info)
		{
			return true;
		}

		// Token: 0x04005094 RID: 20628
		public int itemId;

		// Token: 0x04005095 RID: 20629
		public DropOneByOne.Parameters parameters;

		// Token: 0x0200082C RID: 2092
		public struct Parameters
		{
			// Token: 0x06004334 RID: 17204 RVA: 0x006BF5AB File Offset: 0x006BD7AB
			public float GetPersonalDropRate()
			{
				return (float)this.ChanceNumerator / (float)this.ChanceDenominator;
			}

			// Token: 0x04007237 RID: 29239
			public int ChanceNumerator;

			// Token: 0x04007238 RID: 29240
			public int ChanceDenominator;

			// Token: 0x04007239 RID: 29241
			public int MinimumItemDropsCount;

			// Token: 0x0400723A RID: 29242
			public int MaximumItemDropsCount;

			// Token: 0x0400723B RID: 29243
			public int MinimumStackPerChunkBase;

			// Token: 0x0400723C RID: 29244
			public int MaximumStackPerChunkBase;

			// Token: 0x0400723D RID: 29245
			public int BonusMinDropsPerChunkPerPlayer;

			// Token: 0x0400723E RID: 29246
			public int BonusMaxDropsPerChunkPerPlayer;
		}
	}
}
