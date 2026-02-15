using System;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x020002F6 RID: 758
	public interface INestedItemDropRule
	{
		// Token: 0x06002681 RID: 9857
		ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info, ItemDropRuleResolveAction resolveAction);
	}
}
