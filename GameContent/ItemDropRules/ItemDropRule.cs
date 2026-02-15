using System;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x020002F1 RID: 753
	public class ItemDropRule
	{
		// Token: 0x06002661 RID: 9825 RVA: 0x0055E429 File Offset: 0x0055C629
		public static IItemDropRule Common(int itemId, int chanceDenominator = 1, int minimumDropped = 1, int maximumDropped = 1)
		{
			return new CommonDrop(itemId, chanceDenominator, minimumDropped, maximumDropped, 1);
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x0055E435 File Offset: 0x0055C635
		public static IItemDropRule BossBag(int itemId)
		{
			return new DropBasedOnExpertMode(ItemDropRule.DropNothing(), new DropLocalPerClientAndResetsNPCMoneyTo0(itemId, 1, 1, 1, null));
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x0055E44B File Offset: 0x0055C64B
		public static IItemDropRule BossBagByCondition(IItemDropRuleCondition condition, int itemId)
		{
			return new DropBasedOnExpertMode(ItemDropRule.DropNothing(), new DropLocalPerClientAndResetsNPCMoneyTo0(itemId, 1, 1, 1, condition));
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x0055E461 File Offset: 0x0055C661
		public static IItemDropRule ExpertGetsRerolls(int itemId, int chanceDenominator, int expertRerolls)
		{
			return new DropBasedOnExpertMode(ItemDropRule.WithRerolls(itemId, 0, chanceDenominator, 1, 1), ItemDropRule.WithRerolls(itemId, expertRerolls, chanceDenominator, 1, 1));
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x0055E47C File Offset: 0x0055C67C
		public static IItemDropRule MasterModeCommonDrop(int itemId)
		{
			return ItemDropRule.ByCondition(new Conditions.IsMasterMode(), itemId, 1, 1, 1, 1);
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x0055E48D File Offset: 0x0055C68D
		public static IItemDropRule MasterModeDropOnAllPlayers(int itemId, int chanceDenominator = 1)
		{
			return new DropBasedOnMasterMode(ItemDropRule.DropNothing(), new DropPerPlayerOnThePlayer(itemId, chanceDenominator, 1, 1, new Conditions.IsMasterMode()));
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x0055E4A7 File Offset: 0x0055C6A7
		public static IItemDropRule WithRerolls(int itemId, int rerolls, int chanceDenominator = 1, int minimumDropped = 1, int maximumDropped = 1)
		{
			return new CommonDropWithRerolls(itemId, chanceDenominator, minimumDropped, maximumDropped, rerolls);
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x0055E4B4 File Offset: 0x0055C6B4
		public static IItemDropRule ByCondition(IItemDropRuleCondition condition, int itemId, int chanceDenominator = 1, int minimumDropped = 1, int maximumDropped = 1, int chanceNumerator = 1)
		{
			return new ItemDropWithConditionRule(itemId, chanceDenominator, minimumDropped, maximumDropped, condition, chanceNumerator);
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x0055E4C3 File Offset: 0x0055C6C3
		public static IItemDropRule ScalingWithOnlyBadLuck(int itemId, int chanceDenominator = 1, int minimumDropped = 1, int maximumDropped = 1)
		{
			return new CommonDropScalingWithOnlyBadLuck(itemId, chanceDenominator, minimumDropped, maximumDropped);
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x0055E4CE File Offset: 0x0055C6CE
		public static IItemDropRule NotScalingWithLuck(int itemId, int chanceDenominator = 1, int minimumDropped = 1, int maximumDropped = 1)
		{
			return new CommonDropNotScalingWithLuck(itemId, chanceDenominator, minimumDropped, maximumDropped);
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x0055E4D9 File Offset: 0x0055C6D9
		public static IItemDropRule OneFromOptionsNotScalingWithLuck(int chanceDenominator, params int[] options)
		{
			return new OneFromOptionsNotScaledWithLuckDropRule(chanceDenominator, 1, options);
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x0055E4E3 File Offset: 0x0055C6E3
		public static IItemDropRule OneFromOptionsNotScalingWithLuckWithX(int chanceDenominator, int chanceNumerator, params int[] options)
		{
			return new OneFromOptionsNotScaledWithLuckDropRule(chanceDenominator, chanceNumerator, options);
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x0055E4ED File Offset: 0x0055C6ED
		public static IItemDropRule OneFromOptions(int chanceDenominator, params int[] options)
		{
			return new OneFromOptionsDropRule(chanceDenominator, 1, options);
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x0055E4F7 File Offset: 0x0055C6F7
		public static IItemDropRule OneFromOptionsWithNumerator(int chanceDenominator, int chanceNumerator, params int[] options)
		{
			return new OneFromOptionsDropRule(chanceDenominator, chanceNumerator, options);
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x0055E501 File Offset: 0x0055C701
		public static IItemDropRule DropNothing()
		{
			return new DropNothing();
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x0055E508 File Offset: 0x0055C708
		public static IItemDropRule Gel(int chanceDenominator = 1, int minimumDropped = 1, int maximumDropped = 1)
		{
			short itemId = 23;
			int num = 2;
			return new DropBasedOnExtraGel(ItemDropRule.Common((int)itemId, chanceDenominator, minimumDropped, maximumDropped), ItemDropRule.Common((int)itemId, chanceDenominator, minimumDropped * num, maximumDropped * num));
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x0055E535 File Offset: 0x0055C735
		public static IItemDropRule NormalvsExpert(int itemId, int chanceDenominatorInNormal, int chanceDenominatorInExpert)
		{
			return new DropBasedOnExpertMode(ItemDropRule.Common(itemId, chanceDenominatorInNormal, 1, 1), ItemDropRule.Common(itemId, chanceDenominatorInExpert, 1, 1));
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x0055E54E File Offset: 0x0055C74E
		public static IItemDropRule NormalvsExpertNotScalingWithLuck(int itemId, int chanceDenominatorInNormal, int chanceDenominatorInExpert)
		{
			return new DropBasedOnExpertMode(ItemDropRule.NotScalingWithLuck(itemId, chanceDenominatorInNormal, 1, 1), ItemDropRule.NotScalingWithLuck(itemId, chanceDenominatorInExpert, 1, 1));
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x0055E567 File Offset: 0x0055C767
		public static IItemDropRule NormalvsExpertOneFromOptionsNotScalingWithLuck(int chanceDenominatorInNormal, int chanceDenominatorInExpert, params int[] options)
		{
			return new DropBasedOnExpertMode(ItemDropRule.OneFromOptionsNotScalingWithLuck(chanceDenominatorInNormal, options), ItemDropRule.OneFromOptionsNotScalingWithLuck(chanceDenominatorInExpert, options));
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x0055E57C File Offset: 0x0055C77C
		public static IItemDropRule NormalvsExpertOneFromOptions(int chanceDenominatorInNormal, int chanceDenominatorInExpert, params int[] options)
		{
			return new DropBasedOnExpertMode(ItemDropRule.OneFromOptions(chanceDenominatorInNormal, options), ItemDropRule.OneFromOptions(chanceDenominatorInExpert, options));
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x0055E591 File Offset: 0x0055C791
		public static IItemDropRule Food(int itemId, int chanceDenominator, int minimumDropped = 1, int maximumDropped = 1)
		{
			return new ItemDropWithConditionRule(itemId, chanceDenominator, minimumDropped, maximumDropped, new Conditions.NotFromStatue(), 1);
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x0055E5A2 File Offset: 0x0055C7A2
		public static IItemDropRule StatusImmunityItem(int itemId, int dropsOutOfX)
		{
			return ItemDropRule.ExpertGetsRerolls(itemId, dropsOutOfX, 1);
		}
	}
}
