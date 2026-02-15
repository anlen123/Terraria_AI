using System;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x02000300 RID: 768
	public class DropLocalPerClientAndResetsNPCMoneyTo0 : CommonDrop
	{
		// Token: 0x06002697 RID: 9879 RVA: 0x0055EC94 File Offset: 0x0055CE94
		public DropLocalPerClientAndResetsNPCMoneyTo0(int itemId, int chanceDenominator, int amountDroppedMinimum, int amountDroppedMaximum, IItemDropRuleCondition optionalCondition) : base(itemId, chanceDenominator, amountDroppedMinimum, amountDroppedMaximum, 1)
		{
			this.condition = optionalCondition;
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x0055ECAA File Offset: 0x0055CEAA
		public override bool CanDrop(DropAttemptInfo info)
		{
			return this.condition == null || this.condition.CanDrop(info);
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x0055ECC4 File Offset: 0x0055CEC4
		public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			if (info.rng.Next(this.chanceDenominator) < this.chanceNumerator)
			{
				CommonCode.DropItemLocalPerClientAndSetNPCMoneyTo0(info.npc, this.itemId, info.rng.Next(this.amountDroppedMinimum, this.amountDroppedMaximum + 1), true);
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

		// Token: 0x0400507C RID: 20604
		public IItemDropRuleCondition condition;
	}
}
