using System;
using System.IO;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.Tile_Entities
{
	// Token: 0x0200040D RID: 1037
	public class TEDeadCellsDisplayJar : TileEntityType<TEDeadCellsDisplayJar>, IFixLoadedData
	{
		// Token: 0x06002F88 RID: 12168 RVA: 0x005B395D File Offset: 0x005B1B5D
		public TEDeadCellsDisplayJar()
		{
			this.item = new Item();
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x005B3970 File Offset: 0x005B1B70
		public override bool IsTileValidForEntity(int x, int y)
		{
			return TEDeadCellsDisplayJar.ValidTile(x, y);
		}

		// Token: 0x06002F8A RID: 12170 RVA: 0x005B397C File Offset: 0x005B1B7C
		public static int Hook_AfterPlacement(int x, int y, int type = 698, int style = 0, int direction = 1, int alternate = 0)
		{
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(Main.myPlayer, x, y, 2, 2, TileChangeType.None);
				NetMessage.SendData(87, -1, -1, null, x, (float)y, (float)TileEntityType<TEDeadCellsDisplayJar>.EntityTypeID, 0f, 0, 0, 0);
				return -1;
			}
			return TileEntityType<TEDeadCellsDisplayJar>.Place(x, y);
		}

		// Token: 0x06002F8B RID: 12171 RVA: 0x005B39C4 File Offset: 0x005B1BC4
		public static bool ValidTile(int x, int y)
		{
			return Main.tile[x, y].active() && Main.tile[x, y].type == 698 && Main.tile[x, y].frameY == 0 && Main.tile[x, y].frameX % 18 == 0;
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x005B3A28 File Offset: 0x005B1C28
		public override void WriteExtraData(BinaryWriter writer, bool networkSend)
		{
			writer.Write((short)this.item.type);
			writer.Write(this.item.prefix);
			writer.Write((short)this.item.stack);
		}

		// Token: 0x06002F8D RID: 12173 RVA: 0x005B3A60 File Offset: 0x005B1C60
		public override void ReadExtraData(BinaryReader reader, int gameVersion, bool networkSend)
		{
			this.item = new Item();
			this.item.netDefaults((int)reader.ReadInt16());
			this.item.Prefix((int)reader.ReadByte());
			this.item.stack = (int)reader.ReadInt16();
		}

		// Token: 0x06002F8E RID: 12174 RVA: 0x005B3AAC File Offset: 0x005B1CAC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.Position.X,
				"x  ",
				this.Position.Y,
				"y item: ",
				this.item
			});
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x005B3B04 File Offset: 0x005B1D04
		public void DropItem()
		{
			if (Main.netMode != 1)
			{
				Item.NewItem(new EntitySource_TileBreak((int)this.Position.X, (int)this.Position.Y), (int)(this.Position.X * 16), (int)(this.Position.Y * 16), 32, 32, this.item.type, 1, false, (int)this.item.prefix, false);
			}
			this.item = new Item();
		}

		// Token: 0x06002F90 RID: 12176 RVA: 0x005B3B80 File Offset: 0x005B1D80
		public static void TryPlacing(int x, int y, int type, int prefix, int stack)
		{
			WorldGen.RangeFrame(x, y, x + 1, y + 2);
			TEDeadCellsDisplayJar tedeadCellsDisplayJar;
			if (!TileEntity.TryGetAt<TEDeadCellsDisplayJar>(x, y, out tedeadCellsDisplayJar))
			{
				int num = Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 32, 32, 1, 1, false, 0, false);
				Main.item[num].SetDefaults(type);
				Main.item[num].Prefix(prefix);
				Main.item[num].stack = stack;
				NetMessage.SendData(21, -1, -1, null, num, 0f, 0f, 0f, 0, 0, 0);
				return;
			}
			if (tedeadCellsDisplayJar.item.stack > 0)
			{
				tedeadCellsDisplayJar.DropItem();
			}
			tedeadCellsDisplayJar.item = new Item();
			tedeadCellsDisplayJar.item.SetDefaults(type, null);
			tedeadCellsDisplayJar.item.Prefix(prefix);
			tedeadCellsDisplayJar.item.stack = stack;
			NetMessage.SendData(86, -1, -1, null, tedeadCellsDisplayJar.ID, (float)x, (float)y, 0f, 0, 0, 0);
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x005B3C6C File Offset: 0x005B1E6C
		public static void OnPlayerInteraction(Player player, int clickX, int clickY)
		{
			if (TEDeadCellsDisplayJar.FitsJar(player.inventory[player.selectedItem]) && !player.inventory[player.selectedItem].favorited)
			{
				player.GamepadEnableGrappleCooldown();
				TEDeadCellsDisplayJar.PlaceItemInJar(player, clickX, clickY);
				return;
			}
			int num = clickX;
			int num2 = clickY;
			if (Main.tile[num, num2].frameX % 18 != 0)
			{
				num--;
			}
			if (Main.tile[num, num2].frameY % 36 != 0)
			{
				num2--;
			}
			TEDeadCellsDisplayJar tedeadCellsDisplayJar;
			if (TileEntity.TryGetAt<TEDeadCellsDisplayJar>(num, num2, out tedeadCellsDisplayJar) && tedeadCellsDisplayJar.item.stack > 0)
			{
				player.GamepadEnableGrappleCooldown();
				WorldGen.KillTile(clickX, clickY, true, false, false);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(17, -1, -1, null, 0, (float)num, (float)num2, 1f, 0, 0, 0);
				}
			}
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x005B3D30 File Offset: 0x005B1F30
		public static bool FitsJar(Item i)
		{
			return i.stack > 0;
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x005B3D3C File Offset: 0x005B1F3C
		public static void PlaceItemInJar(Player player, int x, int y)
		{
			if (!player.ItemTimeIsZero)
			{
				return;
			}
			if (Main.tile[x, y].frameX % 18 != 0)
			{
				x--;
			}
			if (Main.tile[x, y].frameY % 36 != 0)
			{
				y--;
			}
			TEDeadCellsDisplayJar tedeadCellsDisplayJar;
			if (!TileEntity.TryGetAt<TEDeadCellsDisplayJar>(x, y, out tedeadCellsDisplayJar))
			{
				return;
			}
			if (tedeadCellsDisplayJar.item.stack > 0)
			{
				WorldGen.KillTile(x, y, true, false, false);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(17, -1, -1, null, 0, (float)Player.tileTargetX, (float)y, 1f, 0, 0, 0);
				}
			}
			if (Main.netMode == 1)
			{
				NetMessage.SendData(149, -1, -1, null, x, (float)y, (float)player.selectedItem, (float)player.whoAmI, 1, 0, 0);
			}
			else
			{
				TEDeadCellsDisplayJar.TryPlacing(x, y, player.inventory[player.selectedItem].type, (int)player.inventory[player.selectedItem].prefix, 1);
			}
			player.inventory[player.selectedItem].stack--;
			if (player.inventory[player.selectedItem].stack <= 0)
			{
				player.inventory[player.selectedItem].SetDefaults(0, null);
				Main.mouseItem.SetDefaults(0, null);
			}
			if (player.selectedItem == 58)
			{
				Main.mouseItem = player.inventory[player.selectedItem].Clone();
			}
			player.releaseUseItem = false;
			player.mouseInterface = true;
			player.PlayDroppedItemAnimation(20);
			WorldGen.RangeFrame(x, y, x + 1, y + 2);
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x005B3EB9 File Offset: 0x005B20B9
		public void FixLoadedData()
		{
			this.item.FixAgainstExploit();
		}

		// Token: 0x04005669 RID: 22121
		public Item item;
	}
}
