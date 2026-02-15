using System;
using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
	// Token: 0x02000287 RID: 647
	public class ItemCraftCondition : AchievementCondition
	{
		// Token: 0x060024E3 RID: 9443 RVA: 0x00552199 File Offset: 0x00550399
		private ItemCraftCondition(short itemId) : base("ITEM_PICKUP_" + itemId)
		{
			this._itemIds = new short[]
			{
				itemId
			};
			ItemCraftCondition.ListenForCraft(this);
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x005521C7 File Offset: 0x005503C7
		private ItemCraftCondition(short[] itemIds) : base("ITEM_PICKUP_" + itemIds[0])
		{
			this._itemIds = itemIds;
			ItemCraftCondition.ListenForCraft(this);
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x005521F0 File Offset: 0x005503F0
		private static void ListenForCraft(ItemCraftCondition condition)
		{
			if (!ItemCraftCondition._isListenerHooked)
			{
				AchievementsHelper.OnItemCraft += ItemCraftCondition.ItemCraftListener;
				ItemCraftCondition._isListenerHooked = true;
			}
			for (int i = 0; i < condition._itemIds.Length; i++)
			{
				if (!ItemCraftCondition._listeners.ContainsKey(condition._itemIds[i]))
				{
					ItemCraftCondition._listeners[condition._itemIds[i]] = new List<ItemCraftCondition>();
				}
				ItemCraftCondition._listeners[condition._itemIds[i]].Add(condition);
			}
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x00552274 File Offset: 0x00550474
		private static void ItemCraftListener(short itemId, int count)
		{
			if (ItemCraftCondition._listeners.ContainsKey(itemId))
			{
				foreach (ItemCraftCondition itemCraftCondition in ItemCraftCondition._listeners[itemId])
				{
					itemCraftCondition.Complete();
				}
			}
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x005522D8 File Offset: 0x005504D8
		public static AchievementCondition Create(params short[] items)
		{
			return new ItemCraftCondition(items);
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x005522E0 File Offset: 0x005504E0
		public static AchievementCondition Create(short item)
		{
			return new ItemCraftCondition(item);
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x005522E8 File Offset: 0x005504E8
		public static AchievementCondition[] CreateMany(params short[] items)
		{
			AchievementCondition[] array = new AchievementCondition[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				array[i] = new ItemCraftCondition(items[i]);
			}
			return array;
		}

		// Token: 0x04004F4B RID: 20299
		private const string Identifier = "ITEM_PICKUP";

		// Token: 0x04004F4C RID: 20300
		private static Dictionary<short, List<ItemCraftCondition>> _listeners = new Dictionary<short, List<ItemCraftCondition>>();

		// Token: 0x04004F4D RID: 20301
		private static bool _isListenerHooked;

		// Token: 0x04004F4E RID: 20302
		private short[] _itemIds;
	}
}
