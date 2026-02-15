using System;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x020002FF RID: 767
	public class CommonDropScalingWithOnlyBadLuck : CommonDrop
	{
		// Token: 0x06002694 RID: 9876 RVA: 0x0055EC10 File Offset: 0x0055CE10
		public CommonDropScalingWithOnlyBadLuck(int itemId, int chanceDenominator, int amountDroppedMinimum = 1, int amountDroppedMaximum = 1, int chanceNumerator = 1) : base(itemId, chanceDenominator, amountDroppedMinimum, amountDroppedMaximum, chanceNumerator)
		{
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x0055EB8C File Offset: 0x0055CD8C
		public CommonDropScalingWithOnlyBadLuck(int itemId, int chanceDenominator, int amountDroppedMinimum, int amountDroppedMaximum) : base(itemId, chanceDenominator, amountDroppedMinimum, amountDroppedMaximum, 1)
		{
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x0055EC20 File Offset: 0x0055CE20
		public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			if (info.player.RollOnlyBadLuck(this.chanceDenominator) < this.chanceNumerator)
			{
				CommonCode.DropItemFromNPC(info.npc, this.itemId, info.rng.Next(this.amountDroppedMinimum, this.amountDroppedMaximum + 1), false);
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
	}
}
