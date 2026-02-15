using System;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.Tile_Entities
{
	// Token: 0x02000413 RID: 1043
	public class TETeleportationPylon : TileEntityType<TETeleportationPylon>
	{
		// Token: 0x06002FC7 RID: 12231 RVA: 0x005B49D0 File Offset: 0x005B2BD0
		public override void NetPlaceEntityAttempt(int x, int y)
		{
			TeleportPylonType pylonType;
			if (!this.TryGetPylonTypeFromTileCoords(x, y, out pylonType))
			{
				TETeleportationPylon.RejectPlacementFromNet(x, y);
				return;
			}
			if (Main.PylonSystem.HasPylonOfType(pylonType))
			{
				TETeleportationPylon.RejectPlacementFromNet(x, y);
				return;
			}
			base.NetPlaceEntityAttempt(x, y);
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x005B4A0E File Offset: 0x005B2C0E
		public bool TryGetPylonType(out TeleportPylonType pylonType)
		{
			return this.TryGetPylonTypeFromTileCoords((int)this.Position.X, (int)this.Position.Y, out pylonType);
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x005B4A30 File Offset: 0x005B2C30
		private static void RejectPlacementFromNet(int x, int y)
		{
			WorldGen.KillTile(x, y, false, false, false);
			if (Main.netMode == 2)
			{
				NetMessage.SendData(17, -1, -1, null, 0, (float)x, (float)y, 0f, 0, 0, 0);
			}
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x005B4A66 File Offset: 0x005B2C66
		public override void OnPlaced()
		{
			Main.PylonSystem.RequestImmediateUpdate();
		}

		// Token: 0x06002FCB RID: 12235 RVA: 0x005B4A66 File Offset: 0x005B2C66
		public override void OnRemoved()
		{
			Main.PylonSystem.RequestImmediateUpdate();
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x005B4A74 File Offset: 0x005B2C74
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.Position.X,
				"x  ",
				this.Position.Y,
				"y"
			});
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x005B4AC4 File Offset: 0x005B2CC4
		public static void Framing_CheckTile(int callX, int callY)
		{
			if (WorldGen.destroyObject)
			{
				return;
			}
			Tile tileSafely = Framing.GetTileSafely(callX, callY);
			int num = callX - (int)(tileSafely.frameX / 18 % 3);
			int num2 = callY - (int)(tileSafely.frameY / 18 % 4);
			int pylonStyleFromTile = TETeleportationPylon.GetPylonStyleFromTile(tileSafely);
			bool flag = false;
			for (int i = num; i < num + 3; i++)
			{
				for (int j = num2; j < num2 + 4; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile == null)
					{
						return;
					}
					if (!tile.active() || tile.type != 597)
					{
						flag = true;
					}
				}
			}
			if (!WorldGen.SolidTileAllowBottomSlope(num, num2 + 4) || !WorldGen.SolidTileAllowBottomSlope(num + 1, num2 + 4) || !WorldGen.SolidTileAllowBottomSlope(num + 2, num2 + 4))
			{
				flag = true;
			}
			if (flag)
			{
				TileEntityType<TETeleportationPylon>.Kill(num, num2);
				int pylonItemTypeFromTileStyle = TETeleportationPylon.GetPylonItemTypeFromTileStyle(pylonStyleFromTile);
				Item.NewItem(new EntitySource_TileBreak(num, num2), num * 16, num2 * 16, 48, 64, pylonItemTypeFromTileStyle, 1, false, 0, false);
				WorldGen.destroyObject = true;
				for (int k = num; k < num + 3; k++)
				{
					for (int l = num2; l < num2 + 4; l++)
					{
						if (Main.tile[k, l].active() && Main.tile[k, l].type == 597)
						{
							WorldGen.KillTile(k, l, false, false, false);
						}
					}
				}
				WorldGen.destroyObject = false;
			}
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x005B4C26 File Offset: 0x005B2E26
		public static int GetPylonStyleFromTile(Tile tile)
		{
			return (int)(tile.frameX / 54);
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x005B4C34 File Offset: 0x005B2E34
		public static int GetPylonItemTypeFromTileStyle(int style)
		{
			switch (style)
			{
			case 1:
				return 4875;
			case 2:
				return 4916;
			case 3:
				return 4917;
			case 4:
				return 4918;
			case 5:
				return 4919;
			case 6:
				return 4920;
			case 7:
				return 4921;
			case 8:
				return 4951;
			case 9:
				return 5652;
			case 10:
				return 5653;
			default:
				return 4876;
			}
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x005B4CB4 File Offset: 0x005B2EB4
		public override bool IsTileValidForEntity(int x, int y)
		{
			return Main.tile[x, y].active() && Main.tile[x, y].type == 597 && Main.tile[x, y].frameY == 0 && Main.tile[x, y].frameX % 54 == 0;
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x005B4D18 File Offset: 0x005B2F18
		public static int PlacementPreviewHook_AfterPlacement(int x, int y, int type = 597, int style = 0, int direction = 1, int alternate = 0)
		{
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(Main.myPlayer, x - 1, y - 3, 3, 4, TileChangeType.None);
				NetMessage.SendData(87, -1, -1, null, x + -1, (float)(y + -3), (float)TileEntityType<TETeleportationPylon>.EntityTypeID, 0f, 0, 0, 0);
				return -1;
			}
			return TileEntityType<TETeleportationPylon>.Place(x + -1, y + -3);
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x005B4D70 File Offset: 0x005B2F70
		public static int PlacementPreviewHook_CheckIfCanPlace(int x, int y, int type = 597, int style = 0, int direction = 1, int alternate = 0)
		{
			TeleportPylonType pylonTypeFromPylonTileStyle = TETeleportationPylon.GetPylonTypeFromPylonTileStyle(style);
			if (Main.PylonSystem.HasPylonOfType(pylonTypeFromPylonTileStyle))
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x005B4D94 File Offset: 0x005B2F94
		private bool TryGetPylonTypeFromTileCoords(int x, int y, out TeleportPylonType pylonType)
		{
			pylonType = TeleportPylonType.SurfacePurity;
			Tile tile = Main.tile[x, y];
			if (tile == null || !tile.active() || tile.type != 597)
			{
				return false;
			}
			int pylonStyle = (int)(tile.frameX / 54);
			pylonType = TETeleportationPylon.GetPylonTypeFromPylonTileStyle(pylonStyle);
			return true;
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x005B4DDE File Offset: 0x005B2FDE
		private static TeleportPylonType GetPylonTypeFromPylonTileStyle(int pylonStyle)
		{
			return (TeleportPylonType)pylonStyle;
		}

		// Token: 0x04005670 RID: 22128
		private const int MyTileID = 597;

		// Token: 0x04005671 RID: 22129
		public const int entityTileWidth = 3;

		// Token: 0x04005672 RID: 22130
		public const int entityTileHeight = 4;
	}
}
