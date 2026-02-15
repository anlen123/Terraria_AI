using System;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x020002FA RID: 762
	public interface IItemDropRuleCondition : IProvideItemConditionDescription
	{
		// Token: 0x06002685 RID: 9861
		bool CanDrop(DropAttemptInfo info);

		// Token: 0x06002686 RID: 9862
		bool CanShowItemDropInUI();
	}
}
