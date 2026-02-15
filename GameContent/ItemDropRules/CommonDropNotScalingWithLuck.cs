using System;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x020002FE RID: 766
	public class CommonDropNotScalingWithLuck : CommonDrop
	{
		// Token: 0x06002692 RID: 9874 RVA: 0x0055EB8C File Offset: 0x0055CD8C
		public CommonDropNotScalingWithLuck(int itemId, int chanceDenominator, int amountDroppedMinimum, int amountDroppedMaximum) : base(itemId, chanceDenominator, amountDroppedMinimum, amountDroppedMaximum, 1)
		{
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x0055EB9C File Offset: 0x0055CD9C
		public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			if (info.rng.Next(this.chanceDenominator) < this.chanceNumerator)
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
