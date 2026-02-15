using System;
using System.IO;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.Tile_Entities
{
	// Token: 0x02000412 RID: 1042
	public class TEFoodPlatter : TileEntityType<TEFoodPlatter>, IFixLoadedData
	{
		// Token: 0x06002FB9 RID: 12217 RVA: 0x005B44D5 File Offset: 0x005B26D5
		public TEFoodPlatter()
		{
			this.item = new Item();
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x005B44E8 File Offset: 0x005B26E8
		public override bool IsTileValidForEntity(int x, int y)
		{
			return TEFoodPlatter.ValidTile(x, y);
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x005B44F4 File Offset: 0x005B26F4
		public static int Hook_AfterPlacement(int x, int y, int type = 520, int style = 0, int direction = 1, int alternate = 0)
		{
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(Main.myPlayer, x, y, TileChangeType.None);
				NetMessage.SendData(87, -1, -1, null, x, (float)y, (float)TileEntityType<TEFoodPlatter>.EntityTypeID, 0f, 0, 0, 0);
				return -1;
			}
			return TileEntityType<TEFoodPlatter>.Place(x, y);
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x005B453C File Offset: 0x005B273C
		public static bool ValidTile(int x, int y)
		{
			return Main.tile[x, y].active() && Main.tile[x, y].type == 520 && Main.tile[x, y].frameY == 0;
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x005B458A File Offset: 0x005B278A
		public override void WriteExtraData(BinaryWriter writer, bool networkSend)
		{
			writer.Write((short)this.item.type);
			writer.Write(this.item.prefix);
			writer.Write((short)this.item.stack);
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x005B45C4 File Offset: 0x005B27C4
		public override void ReadExtraData(BinaryReader reader, int gameVersion, bool networkSend)
		{
			this.item = new Item();
			this.item.netDefaults((int)reader.ReadInt16());
			this.item.Prefix((int)reader.ReadByte());
			this.item.stack = (int)reader.ReadInt16();
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x005B4610 File Offset: 0x005B2810
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

		// Token: 0x06002FC0 RID: 12224 RVA: 0x005B4668 File Offset: 0x005B2868
		public void DropItem()
		{
			if (Main.netMode != 1)
			{
				Item.NewItem(new EntitySource_TileBreak((int)this.Position.X, (int)this.Position.Y), (int)(this.Position.X * 16), (int)(this.Position.Y * 16), 16, 16, this.item.type, 1, false, (int)this.item.prefix, false);
			}
			this.item = new Item();
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x005B46E4 File Offset: 0x005B28E4
		public static void TryPlacing(int x, int y, int type, int prefix, int stack)
		{
			WorldGen.RangeFrame(x, y, x + 1, y + 1);
			TEFoodPlatter tefoodPlatter;
			if (!TileEntity.TryGetAt<TEFoodPlatter>(x, y, out tefoodPlatter))
			{
				int num = Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 16, 16, 1, 1, false, 0, false);
				Main.item[num].SetDefaults(type);
				Main.item[num].Prefix(prefix);
				Main.item[num].stack = stack;
				NetMessage.SendData(21, -1, -1, null, num, 0f, 0f, 0f, 0, 0, 0);
				return;
			}
			if (tefoodPlatter.item.stack > 0)
			{
				tefoodPlatter.DropItem();
			}
			tefoodPlatter.item = new Item();
			tefoodPlatter.item.SetDefaults(type, null);
			tefoodPlatter.item.Prefix(prefix);
			tefoodPlatter.item.stack = stack;
			NetMessage.SendData(86, -1, -1, null, tefoodPlatter.ID, (float)x, (float)y, 0f, 0, 0, 0);
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x005B47D0 File Offset: 0x005B29D0
		public static void OnPlayerInteraction(Player player, int clickX, int clickY)
		{
			if (TEFoodPlatter.FitsFoodPlatter(player.inventory[player.selectedItem]) && !player.inventory[player.selectedItem].favorited)
			{
				player.GamepadEnableGrappleCooldown();
				TEFoodPlatter.PlaceItemInFrame(player, clickX, clickY);
				return;
			}
			TEFoodPlatter tefoodPlatter;
			if (TileEntity.TryGetAt<TEFoodPlatter>(clickX, clickY, out tefoodPlatter) && tefoodPlatter.item.stack > 0)
			{
				player.GamepadEnableGrappleCooldown();
				WorldGen.KillTile(clickX, clickY, true, false, false);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(17, -1, -1, null, 0, (float)clickX, (float)clickY, 1f, 0, 0, 0);
				}
			}
		}

		// Token: 0x06002FC3 RID: 12227 RVA: 0x005B4860 File Offset: 0x005B2A60
		public static bool FitsFoodPlatter(Item i)
		{
			return i.stack > 0 && ItemID.Sets.IsFood[i.type];
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x005B487C File Offset: 0x005B2A7C
		public static void PlaceItemInFrame(Player player, int x, int y)
		{
			if (!player.ItemTimeIsZero)
			{
				return;
			}
			TEFoodPlatter tefoodPlatter;
			if (!TileEntity.TryGetAt<TEFoodPlatter>(x, y, out tefoodPlatter))
			{
				return;
			}
			if (tefoodPlatter.item.stack > 0)
			{
				WorldGen.KillTile(x, y, true, false, false);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(17, -1, -1, null, 0, (float)Player.tileTargetX, (float)y, 1f, 0, 0, 0);
				}
			}
			if (Main.netMode == 1)
			{
				NetMessage.SendData(133, -1, -1, null, x, (float)y, (float)player.selectedItem, (float)player.whoAmI, 1, 0, 0);
			}
			else
			{
				TEFoodPlatter.TryPlacing(x, y, player.inventory[player.selectedItem].type, (int)player.inventory[player.selectedItem].prefix, 1);
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
			WorldGen.RangeFrame(x, y, x + 1, y + 1);
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x005B49BB File Offset: 0x005B2BBB
		public void FixLoadedData()
		{
			this.item.FixAgainstExploit();
		}

		// Token: 0x0400566F RID: 22127
		public Item item;
	}
}
