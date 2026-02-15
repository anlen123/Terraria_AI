using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent
{
	// Token: 0x0200023C RID: 572
	public static class NearbyChests
	{
		// Token: 0x06002275 RID: 8821 RVA: 0x00538278 File Offset: 0x00536478
		public static List<PositionedChest> GetChestsInRangeOf(Vector2 position, float range = 0f)
		{
			if (range <= 0f)
			{
				range = 600f;
			}
			List<PositionedChest> scratch = NearbyChests._scratch;
			scratch.Clear();
			for (int i = 0; i < 8000; i++)
			{
				Chest chest = Main.chest[i];
				if (chest != null)
				{
					Vector2 vector = new Vector2((float)(chest.x * 16 + 16), (float)(chest.y * 16 + 16));
					if (Vector2.Distance(vector, position) <= range)
					{
						scratch.Add(new PositionedChest(chest, vector));
					}
				}
			}
			return scratch;
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x005382F4 File Offset: 0x005364F4
		public static List<PositionedChest> GetBanksInRangeOf(Player player, float range = 0f)
		{
			if (range <= 0f)
			{
				range = 600f;
			}
			List<PositionedChest> scratch = NearbyChests._scratch;
			scratch.Clear();
			int num = (int)(range / 16f + 2f);
			Point point = player.Center.ToTileCoordinates();
			Rectangle rectangle = new Rectangle(point.X - num, point.Y - num, num * 2 + 1, num * 2 + 1);
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active)
				{
					int container = -1;
					if (projectile.TryGetContainerIndex(out container))
					{
						Vector2 vec = projectile.Hitbox.ClosestPointInRect(player.Center);
						Chest chest;
						if (rectangle.Contains(vec.ToTileCoordinates()) && NearbyChests.ContainerIndexToPlayerBank(player, container, out chest) && !scratch.Contains(chest))
						{
							scratch.Add(new PositionedChest(chest, projectile.Center));
						}
					}
				}
			}
			for (int j = rectangle.Left; j < rectangle.Right; j++)
			{
				for (int k = rectangle.Top; k < rectangle.Bottom; k++)
				{
					if (WorldGen.InWorld(j, k, 0))
					{
						int container2 = 0;
						int type = (int)Main.tile[j, k].type;
						if (type == 29)
						{
							container2 = -2;
						}
						else if (type == 97)
						{
							container2 = -3;
						}
						else if (type == 463)
						{
							container2 = -4;
						}
						else if (type == 491)
						{
							container2 = -5;
						}
						Chest chest2;
						if (NearbyChests.ContainerIndexToPlayerBank(player, container2, out chest2) && !scratch.Contains(chest2))
						{
							scratch.Add(new PositionedChest(chest2, new Vector2((float)(j * 16 + 16), (float)(k * 16 + 16))));
						}
					}
				}
			}
			return scratch;
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x005384B8 File Offset: 0x005366B8
		private static bool Contains(this List<PositionedChest> list, Chest chest)
		{
			using (List<PositionedChest>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.chest == chest)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x00538510 File Offset: 0x00536710
		private static bool ContainerIndexToPlayerBank(Player player, int container, out Chest bank)
		{
			bank = null;
			if (container == -2)
			{
				bank = player.bank;
				return true;
			}
			if (container == -3)
			{
				bank = player.bank2;
				return true;
			}
			if (container == -4)
			{
				bank = player.bank3;
				return true;
			}
			if (container == -5)
			{
				bank = player.bank4;
				for (int i = 0; i < 58; i++)
				{
					if (player.inventory[i].stack > 0 && player.inventory[i].type == 5325)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x04004CE3 RID: 19683
		private static List<PositionedChest> _scratch = new List<PositionedChest>();
	}
}
