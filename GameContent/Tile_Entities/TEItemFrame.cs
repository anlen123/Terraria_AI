using System;
using System.IO;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.Tile_Entities
{
	// Token: 0x0200041B RID: 1051
	public class TEItemFrame : TileEntityType<TEItemFrame>, IFixLoadedData
	{
		// Token: 0x06003037 RID: 12343 RVA: 0x005B86C0 File Offset: 0x005B68C0
		public TEItemFrame()
		{
			this.item = new Item();
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x005B86D3 File Offset: 0x005B68D3
		public override bool IsTileValidForEntity(int x, int y)
		{
			return TEItemFrame.ValidTile(x, y);
		}

		// Token: 0x06003039 RID: 12345 RVA: 0x005B86DC File Offset: 0x005B68DC
		public static int Hook_AfterPlacement(int x, int y, int type = 395, int style = 0, int direction = 1, int alternate = 0)
		{
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(Main.myPlayer, x, y, 2, 2, TileChangeType.None);
				NetMessage.SendData(87, -1, -1, null, x, (float)y, (float)TileEntityType<TEItemFrame>.EntityTypeID, 0f, 0, 0, 0);
				return -1;
			}
			return TileEntityType<TEItemFrame>.Place(x, y);
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x005B8724 File Offset: 0x005B6924
		public static bool ValidTile(int x, int y)
		{
			return Main.tile[x, y].active() && Main.tile[x, y].type == 395 && Main.tile[x, y].frameY == 0 && Main.tile[x, y].frameX % 36 == 0;
		}

		// Token: 0x0600303B RID: 12347 RVA: 0x005B8788 File Offset: 0x005B6988
		public override void WriteExtraData(BinaryWriter writer, bool networkSend)
		{
			writer.Write((short)this.item.type);
			writer.Write(this.item.prefix);
			writer.Write((short)this.item.stack);
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x005B87C0 File Offset: 0x005B69C0
		public override void ReadExtraData(BinaryReader reader, int gameVersion, bool networkSend)
		{
			this.item = new Item();
			this.item.netDefaults((int)reader.ReadInt16());
			this.item.Prefix((int)reader.ReadByte());
			this.item.stack = (int)reader.ReadInt16();
		}

		// Token: 0x0600303D RID: 12349 RVA: 0x005B880C File Offset: 0x005B6A0C
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

		// Token: 0x0600303E RID: 12350 RVA: 0x005B8864 File Offset: 0x005B6A64
		public void DropItem()
		{
			if (Main.netMode != 1)
			{
				Item.NewItem(new EntitySource_TileBreak((int)this.Position.X, (int)this.Position.Y), (int)(this.Position.X * 16), (int)(this.Position.Y * 16), 32, 32, this.item.type, 1, false, (int)this.item.prefix, false);
			}
			this.item = new Item();
		}

		// Token: 0x0600303F RID: 12351 RVA: 0x005B88E0 File Offset: 0x005B6AE0
		public static void TryPlacing(int x, int y, int type, int prefix, int stack)
		{
			WorldGen.RangeFrame(x, y, x + 2, y + 2);
			TEItemFrame teitemFrame;
			if (!TileEntity.TryGetAt<TEItemFrame>(x, y, out teitemFrame))
			{
				int num = Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 32, 32, 1, 1, false, 0, false);
				Main.item[num].SetDefaults(type);
				Main.item[num].Prefix(prefix);
				Main.item[num].stack = stack;
				NetMessage.SendData(21, -1, -1, null, num, 0f, 0f, 0f, 0, 0, 0);
				return;
			}
			if (teitemFrame.item.stack > 0)
			{
				teitemFrame.DropItem();
			}
			teitemFrame.item = new Item();
			teitemFrame.item.SetDefaults(type, null);
			teitemFrame.item.Prefix(prefix);
			teitemFrame.item.stack = stack;
			NetMessage.SendData(86, -1, -1, null, teitemFrame.ID, (float)x, (float)y, 0f, 0, 0, 0);
		}

		// Token: 0x06003040 RID: 12352 RVA: 0x005B89CC File Offset: 0x005B6BCC
		public static void OnPlayerInteraction(Player player, int clickX, int clickY)
		{
			if (TEItemFrame.FitsItemFrame(player.inventory[player.selectedItem]) && !player.inventory[player.selectedItem].favorited)
			{
				player.GamepadEnableGrappleCooldown();
				TEItemFrame.PlaceItemInFrame(player, clickX, clickY);
				return;
			}
			int num = clickX;
			int num2 = clickY;
			if (Main.tile[num, num2].frameX % 36 != 0)
			{
				num--;
			}
			if (Main.tile[num, num2].frameY % 36 != 0)
			{
				num2--;
			}
			TEItemFrame teitemFrame;
			if (TileEntity.TryGetAt<TEItemFrame>(num, num2, out teitemFrame) && teitemFrame.item.stack > 0)
			{
				player.GamepadEnableGrappleCooldown();
				WorldGen.KillTile(clickX, clickY, true, false, false);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(17, -1, -1, null, 0, (float)num, (float)num2, 1f, 0, 0, 0);
				}
			}
		}

		// Token: 0x06003041 RID: 12353 RVA: 0x005B3D30 File Offset: 0x005B1F30
		public static bool FitsItemFrame(Item i)
		{
			return i.stack > 0;
		}

		// Token: 0x06003042 RID: 12354 RVA: 0x005B8A90 File Offset: 0x005B6C90
		public static void PlaceItemInFrame(Player player, int x, int y)
		{
			if (!player.ItemTimeIsZero)
			{
				return;
			}
			if (Main.tile[x, y].frameX % 36 != 0)
			{
				x--;
			}
			if (Main.tile[x, y].frameY % 36 != 0)
			{
				y--;
			}
			TEItemFrame teitemFrame;
			if (!TileEntity.TryGetAt<TEItemFrame>(x, y, out teitemFrame))
			{
				return;
			}
			if (teitemFrame.item.stack > 0)
			{
				WorldGen.KillTile(x, y, true, false, false);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(17, -1, -1, null, 0, (float)Player.tileTargetX, (float)y, 1f, 0, 0, 0);
				}
			}
			if (Main.netMode == 1)
			{
				NetMessage.SendData(89, -1, -1, null, x, (float)y, (float)player.selectedItem, (float)player.whoAmI, 1, 0, 0);
			}
			else
			{
				TEItemFrame.TryPlacing(x, y, player.inventory[player.selectedItem].type, (int)player.inventory[player.selectedItem].prefix, 1);
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
			WorldGen.RangeFrame(x, y, x + 2, y + 2);
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x005B8C0A File Offset: 0x005B6E0A
		public void FixLoadedData()
		{
			this.item.FixAgainstExploit();
		}

		// Token: 0x040056AA RID: 22186
		public Item item;
	}
}
