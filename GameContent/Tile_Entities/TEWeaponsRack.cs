using System;
using System.IO;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.Tile_Entities
{
	// Token: 0x02000419 RID: 1049
	public class TEWeaponsRack : TileEntityType<TEWeaponsRack>, IFixLoadedData
	{
		// Token: 0x06003012 RID: 12306 RVA: 0x005B7663 File Offset: 0x005B5863
		public TEWeaponsRack()
		{
			this.item = new Item();
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x005B7676 File Offset: 0x005B5876
		public override void NetPlaceEntityAttempt(int x, int y)
		{
			TEWeaponsRack.NetPlaceEntity(x, y);
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x005B7680 File Offset: 0x005B5880
		public static void NetPlaceEntity(int x, int y)
		{
			int number = TileEntityType<TEWeaponsRack>.Place(x, y);
			NetMessage.SendData(86, -1, -1, null, number, (float)x, (float)y, 0f, 0, 0, 0);
		}

		// Token: 0x06003015 RID: 12309 RVA: 0x005B76AC File Offset: 0x005B58AC
		public override bool IsTileValidForEntity(int x, int y)
		{
			return TEWeaponsRack.ValidTile(x, y);
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x005B76B8 File Offset: 0x005B58B8
		public static bool ValidTile(int x, int y)
		{
			return Main.tile[x, y].active() && Main.tile[x, y].type == 471 && Main.tile[x, y].frameY == 0 && Main.tile[x, y].frameX % 54 == 0;
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x005B771C File Offset: 0x005B591C
		public static int Hook_AfterPlacement(int x, int y, int type = 471, int style = 0, int direction = 1, int alternate = 0)
		{
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(Main.myPlayer, x, y, 3, 3, TileChangeType.None);
				NetMessage.SendData(87, -1, -1, null, x, (float)y, (float)TileEntityType<TEWeaponsRack>.EntityTypeID, 0f, 0, 0, 0);
				return -1;
			}
			return TileEntityType<TEWeaponsRack>.Place(x, y);
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x005B7764 File Offset: 0x005B5964
		public override void WriteExtraData(BinaryWriter writer, bool networkSend)
		{
			writer.Write((short)this.item.type);
			writer.Write(this.item.prefix);
			writer.Write((short)this.item.stack);
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x005B779C File Offset: 0x005B599C
		public override void ReadExtraData(BinaryReader reader, int gameVersion, bool networkSend)
		{
			this.item = new Item();
			this.item.netDefaults((int)reader.ReadInt16());
			this.item.Prefix((int)reader.ReadByte());
			this.item.stack = (int)reader.ReadInt16();
		}

		// Token: 0x0600301A RID: 12314 RVA: 0x005B77E8 File Offset: 0x005B59E8
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

		// Token: 0x0600301B RID: 12315 RVA: 0x005B7840 File Offset: 0x005B5A40
		public static void Framing_CheckTile(int callX, int callY)
		{
			int num = 3;
			int num2 = 3;
			if (WorldGen.destroyObject)
			{
				return;
			}
			Tile tileSafely = Framing.GetTileSafely(callX, callY);
			int num3 = callX - (int)(tileSafely.frameX / 18) % num;
			int num4 = callY - (int)(tileSafely.frameY / 18) % num2;
			bool flag = false;
			for (int i = num3; i < num3 + num; i++)
			{
				for (int j = num4; j < num4 + num2; j++)
				{
					Tile tile = Main.tile[i, j];
					if (!tile.active() || tile.type != 471 || tile.wall == 0)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				TEWeaponsRack teweaponsRack;
				if (TileEntity.TryGetAt<TEWeaponsRack>(num3, num4, out teweaponsRack) && teweaponsRack.item.stack > 0)
				{
					teweaponsRack.DropItem();
					if (Main.netMode != 2)
					{
						Main.LocalPlayer.InterruptItemUsageIfOverTile(471);
					}
				}
				WorldGen.destroyObject = true;
				for (int k = num3; k < num3 + num; k++)
				{
					for (int l = num4; l < num4 + num2; l++)
					{
						if (Main.tile[k, l].active() && Main.tile[k, l].type == 471)
						{
							WorldGen.KillTile(k, l, false, false, false);
						}
					}
				}
				Item.NewItem(new EntitySource_TileBreak(num3, num4), num3 * 16, num4 * 16, 48, 48, 2699, 1, false, 0, false);
				TileEntityType<TEWeaponsRack>.Kill(num3, num4);
				WorldGen.destroyObject = false;
			}
		}

		// Token: 0x0600301C RID: 12316 RVA: 0x005B79B4 File Offset: 0x005B5BB4
		public void DropItem()
		{
			if (Main.netMode != 1)
			{
				Item.NewItem(new EntitySource_TileBreak((int)this.Position.X, (int)this.Position.Y), (int)(this.Position.X * 16), (int)(this.Position.Y * 16), 32, 32, this.item.type, 1, false, (int)this.item.prefix, false);
			}
			this.item = new Item();
		}

		// Token: 0x0600301D RID: 12317 RVA: 0x005B7A30 File Offset: 0x005B5C30
		public static void TryPlacing(int x, int y, int type, int prefix, int stack)
		{
			WorldGen.RangeFrame(x, y, x + 3, y + 3);
			TEWeaponsRack teweaponsRack;
			if (!TileEntity.TryGetAt<TEWeaponsRack>(x, y, out teweaponsRack))
			{
				int num = Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 32, 32, 1, 1, false, 0, false);
				Main.item[num].SetDefaults(type);
				Main.item[num].Prefix(prefix);
				Main.item[num].stack = stack;
				NetMessage.SendData(21, -1, -1, null, num, 0f, 0f, 0f, 0, 0, 0);
				return;
			}
			if (teweaponsRack.item.stack > 0)
			{
				teweaponsRack.DropItem();
			}
			teweaponsRack.item = new Item();
			teweaponsRack.item.SetDefaults(type, null);
			teweaponsRack.item.Prefix(prefix);
			teweaponsRack.item.stack = stack;
			NetMessage.SendData(86, -1, -1, null, teweaponsRack.ID, (float)x, (float)y, 0f, 0, 0, 0);
		}

		// Token: 0x0600301E RID: 12318 RVA: 0x005B7B1C File Offset: 0x005B5D1C
		public static void OnPlayerInteraction(Player player, int clickX, int clickY)
		{
			if (TEWeaponsRack.FitsWeaponFrame(player.inventory[player.selectedItem]) && !player.inventory[player.selectedItem].favorited)
			{
				player.GamepadEnableGrappleCooldown();
				TEWeaponsRack.PlaceItemInFrame(player, clickX, clickY);
				return;
			}
			int num = clickX - (int)(Main.tile[clickX, clickY].frameX % 54 / 18);
			int num2 = clickY - (int)(Main.tile[num, clickY].frameY % 54 / 18);
			TEWeaponsRack teweaponsRack;
			if (TileEntity.TryGetAt<TEWeaponsRack>(num, num2, out teweaponsRack) && teweaponsRack.item.stack > 0)
			{
				player.GamepadEnableGrappleCooldown();
				WorldGen.KillTile(num, num2, true, false, false);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(17, -1, -1, null, 0, (float)num, (float)num2, 1f, 0, 0, 0);
				}
			}
		}

		// Token: 0x0600301F RID: 12319 RVA: 0x005B7BE0 File Offset: 0x005B5DE0
		public static bool FitsWeaponFrame(Item i)
		{
			return (!i.IsAir && (i.fishingPole > 0 || ItemID.Sets.CanBePlacedOnWeaponRacks[i.type])) || (i.damage > 0 && i.useStyle != 0 && i.stack > 0);
		}

		// Token: 0x06003020 RID: 12320 RVA: 0x005B7C20 File Offset: 0x005B5E20
		private static void PlaceItemInFrame(Player player, int x, int y)
		{
			if (!player.ItemTimeIsZero)
			{
				return;
			}
			x -= (int)(Main.tile[x, y].frameX % 54 / 18);
			y -= (int)(Main.tile[x, y].frameY % 54 / 18);
			TEWeaponsRack teweaponsRack;
			if (!TileEntity.TryGetAt<TEWeaponsRack>(x, y, out teweaponsRack))
			{
				return;
			}
			if (teweaponsRack.item.stack > 0)
			{
				WorldGen.KillTile(x, y, true, false, false);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(17, -1, -1, null, 0, (float)Player.tileTargetX, (float)y, 1f, 0, 0, 0);
				}
			}
			if (Main.netMode == 1)
			{
				NetMessage.SendData(123, -1, -1, null, x, (float)y, (float)player.selectedItem, (float)player.whoAmI, 1, 0, 0);
			}
			else
			{
				TEWeaponsRack.TryPlacing(x, y, player.inventory[player.selectedItem].type, (int)player.inventory[player.selectedItem].prefix, 1);
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
			WorldGen.RangeFrame(x, y, x + 3, y + 3);
		}

		// Token: 0x06003021 RID: 12321 RVA: 0x005B7D9A File Offset: 0x005B5F9A
		public void FixLoadedData()
		{
			this.item.FixAgainstExploit();
		}

		// Token: 0x040056A0 RID: 22176
		public Item item;

		// Token: 0x040056A1 RID: 22177
		private const int MyTileID = 471;
	}
}
