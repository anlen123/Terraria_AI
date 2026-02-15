using System;
using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
	// Token: 0x02000288 RID: 648
	public class ItemPickupCondition : AchievementCondition
	{
		// Token: 0x060024EB RID: 9451 RVA: 0x00552324 File Offset: 0x00550524
		private ItemPickupCondition(short itemId) : base("ITEM_PICKUP_" + itemId)
		{
			this._itemIds = new short[]
			{
				itemId
			};
			ItemPickupCondition.ListenForPickup(this);
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x00552352 File Offset: 0x00550552
		private ItemPickupCondition(short[] itemIds) : base("ITEM_PICKUP_" + itemIds[0])
		{
			this._itemIds = itemIds;
			ItemPickupCondition.ListenForPickup(this);
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x0055237C File Offset: 0x0055057C
		private static void ListenForPickup(ItemPickupCondition condition)
		{
			if (!ItemPickupCondition._isListenerHooked)
			{
				AchievementsHelper.OnItemPickup += ItemPickupCondition.ItemPickupListener;
				ItemPickupCondition._isListenerHooked = true;
			}
			for (int i = 0; i < condition._itemIds.Length; i++)
			{
				if (!ItemPickupCondition._listeners.ContainsKey(condition._itemIds[i]))
				{
					ItemPickupCondition._listeners[condition._itemIds[i]] = new List<ItemPickupCondition>();
				}
				ItemPickupCondition._listeners[condition._itemIds[i]].Add(condition);
			}
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x00552400 File Offset: 0x00550600
		private static void ItemPickupListener(Player player, short itemId, int count)
		{
			if (player.whoAmI != Main.myPlayer)
			{
				return;
			}
			if (ItemPickupCondition._listeners.ContainsKey(itemId))
			{
				foreach (ItemPickupCondition itemPickupCondition in ItemPickupCondition._listeners[itemId])
				{
					itemPickupCondition.Complete();
				}
			}
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x00552470 File Offset: 0x00550670
		public static AchievementCondition Create(params short[] items)
		{
			return new ItemPickupCondition(items);
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x00552478 File Offset: 0x00550678
		public static AchievementCondition Create(short item)
		{
			return new ItemPickupCondition(item);
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x00552480 File Offset: 0x00550680
		public static AchievementCondition[] CreateMany(params short[] items)
		{
			AchievementCondition[] array = new AchievementCondition[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				array[i] = new ItemPickupCondition(items[i]);
			}
			return array;
		}

		// Token: 0x04004F4F RID: 20303
		private const string Identifier = "ITEM_PICKUP";

		// Token: 0x04004F50 RID: 20304
		private static Dictionary<short, List<ItemPickupCondition>> _listeners = new Dictionary<short, List<ItemPickupCondition>>();

		// Token: 0x04004F51 RID: 20305
		private static bool _isListenerHooked;

		// Token: 0x04004F52 RID: 20306
		private short[] _itemIds;
	}
}
