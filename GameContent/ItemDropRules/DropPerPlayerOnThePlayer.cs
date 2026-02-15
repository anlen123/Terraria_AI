using System;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x02000301 RID: 769
	public class DropPerPlayerOnThePlayer : CommonDrop
	{
		// Token: 0x0600269A RID: 9882 RVA: 0x0055ED38 File Offset: 0x0055CF38
		public DropPerPlayerOnThePlayer(int itemId, int chanceDenominator, int amountDroppedMinimum, int amountDroppedMaximum, IItemDropRuleCondition optionalCondition) : base(itemId, chanceDenominator, amountDroppedMinimum, amountDroppedMaximum, 1)
		{
			this.condition = optionalCondition;
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x0055ED4E File Offset: 0x0055CF4E
		public override bool CanDrop(DropAttemptInfo info)
		{
			return this.condition == null || this.condition.CanDrop(info);
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x0055ED68 File Offset: 0x0055CF68
		public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			CommonCode.DropItemForEachInteractingPlayerOnThePlayer(info.npc, this.itemId, info.rng, this.chanceNumerator, this.chanceDenominator, info.rng.Next(this.amountDroppedMinimum, this.amountDroppedMaximum + 1), true);
			return new ItemDropAttemptResult
			{
				State = ItemDropAttemptResultState.Success
			};
		}

		// Token: 0x0400507D RID: 20605
		public IItemDropRuleCondition condition;
	}
}
