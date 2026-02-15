using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000553 RID: 1363
	public class MinionSpawnFromInventoryItem : MinionSpawnInfo
	{
		// Token: 0x06003776 RID: 14198 RVA: 0x0062E545 File Offset: 0x0062C745
		public MinionSpawnFromInventoryItem(Item item)
		{
			this.ItemType = item.type;
			this.ItemPrefix = (int)item.prefix;
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x0062E565 File Offset: 0x0062C765
		protected virtual bool ItemMatches(Item item)
		{
			return item.type == this.ItemType && (int)item.prefix == this.ItemPrefix;
		}

		// Token: 0x06003778 RID: 14200 RVA: 0x0062E588 File Offset: 0x0062C788
		public override void TryRespawn(Player player)
		{
			Item item = this.FindMatchingItem(player);
			if (item != null)
			{
				if (item.buffType > 0)
				{
					int num = item.buffTime;
					if (num == 0)
					{
						num = 3600;
					}
					player.AddBuff(item.buffType, num, false);
				}
				player.SilentlyShootItem(item);
			}
		}

		// Token: 0x06003779 RID: 14201 RVA: 0x0062E5D0 File Offset: 0x0062C7D0
		protected Item FindMatchingItem(Player player)
		{
			Item[] inventory = player.inventory;
			for (int i = 0; i < 50; i++)
			{
				Item item = inventory[i];
				if (this.ItemMatches(item))
				{
					return item;
				}
			}
			return null;
		}

		// Token: 0x04005B91 RID: 23441
		public int ItemType;

		// Token: 0x04005B92 RID: 23442
		public int ItemPrefix;
	}
}
