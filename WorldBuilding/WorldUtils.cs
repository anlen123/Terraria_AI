using System;
using Microsoft.Xna.Framework;
using Terraria.Testing;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000C1 RID: 193
	public static class WorldUtils
	{
		// Token: 0x060017C8 RID: 6088 RVA: 0x004DF8F8 File Offset: 0x004DDAF8
		public static Rectangle ClampToWorld(Rectangle tileRectangle, int fluff = 0)
		{
			int num = Math.Max(fluff, Math.Min(tileRectangle.Left, Main.maxTilesX - fluff));
			int num2 = Math.Max(fluff, Math.Min(tileRectangle.Top, Main.maxTilesY - fluff));
			int num3 = Math.Max(fluff, Math.Min(tileRectangle.Right, Main.maxTilesX - fluff));
			int num4 = Math.Max(fluff, Math.Min(tileRectangle.Bottom, Main.maxTilesY - fluff));
			return new Rectangle(num, num2, num3 - num, num4 - num2);
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x004DF97C File Offset: 0x004DDB7C
		public static Rectangle GetWorldPlayArea()
		{
			int num = 640;
			Point point = new Point((int)Main.leftWorld + num, (int)Main.topWorld + num);
			Point point2 = new Point((int)Main.rightWorld - num, (int)Main.bottomWorld - num);
			return new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x004DF9E8 File Offset: 0x004DDBE8
		public static Rectangle ClampToWorldBorders(Rectangle worldRect)
		{
			if (DebugOptions.noLimits)
			{
				return worldRect;
			}
			return Utils.Clamp(worldRect, WorldUtils.GetWorldPlayArea());
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x004DF9FE File Offset: 0x004DDBFE
		public static bool Gen(Point origin, GenShape shape, GenAction action)
		{
			return shape.Perform(origin, action);
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x004DFA08 File Offset: 0x004DDC08
		public static bool Gen(Point origin, GenShapeActionPair pair)
		{
			return pair.Shape.Perform(origin, pair.Action);
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x004DFA1C File Offset: 0x004DDC1C
		public static bool Find(Point origin, GenSearch search, out Point result)
		{
			result = search.Find(origin);
			return !(result == GenSearch.NOT_FOUND);
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x004DFA40 File Offset: 0x004DDC40
		public static void ClearTile(int x, int y, bool frameNeighbors = false)
		{
			Main.tile[x, y].ClearTile();
			if (frameNeighbors)
			{
				WorldGen.TileFrame(x + 1, y, false, false);
				WorldGen.TileFrame(x - 1, y, false, false);
				WorldGen.TileFrame(x, y + 1, false, false);
				WorldGen.TileFrame(x, y - 1, false, false);
			}
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x004DFA8D File Offset: 0x004DDC8D
		public static void ClearWall(int x, int y, bool frameNeighbors = false)
		{
			Main.tile[x, y].wall = 0;
			if (frameNeighbors)
			{
				WorldGen.SquareWallFrame(x + 1, y, true);
				WorldGen.SquareWallFrame(x - 1, y, true);
				WorldGen.SquareWallFrame(x, y + 1, true);
				WorldGen.SquareWallFrame(x, y - 1, true);
			}
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x004DFACC File Offset: 0x004DDCCC
		public static void TileFrame(int x, int y, bool frameNeighbors = false)
		{
			WorldGen.TileFrame(x, y, true, false);
			if (frameNeighbors)
			{
				WorldGen.TileFrame(x + 1, y, true, false);
				WorldGen.TileFrame(x - 1, y, true, false);
				WorldGen.TileFrame(x, y + 1, true, false);
				WorldGen.TileFrame(x, y - 1, true, false);
			}
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x004DFB06 File Offset: 0x004DDD06
		public static void WallFrame(int x, int y, bool frameNeighbors = false)
		{
			Framing.WallFrame(x, y, true);
			if (frameNeighbors)
			{
				Framing.WallFrame(x + 1, y, true);
				Framing.WallFrame(x - 1, y, true);
				Framing.WallFrame(x, y + 1, true);
				Framing.WallFrame(x, y - 1, true);
			}
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x004DFB3B File Offset: 0x004DDD3B
		public static void ClearChestLocation(int x, int y)
		{
			WorldUtils.ClearTile(x, y, true);
			WorldUtils.ClearTile(x - 1, y, true);
			WorldUtils.ClearTile(x, y - 1, true);
			WorldUtils.ClearTile(x - 1, y - 1, true);
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x004DFB68 File Offset: 0x004DDD68
		public static void WireLine(Point start, Point end)
		{
			Point point = start;
			Point point2 = end;
			if (end.X < start.X)
			{
				Utils.Swap<int>(ref end.X, ref start.X);
			}
			if (end.Y < start.Y)
			{
				Utils.Swap<int>(ref end.Y, ref start.Y);
			}
			for (int i = start.X; i <= end.X; i++)
			{
				WorldGen.PlaceWire(i, point.Y);
			}
			for (int j = start.Y; j <= end.Y; j++)
			{
				WorldGen.PlaceWire(point2.X, j);
			}
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x004DFC01 File Offset: 0x004DDE01
		public static void DebugRegen()
		{
			WorldGen.GenerateWorld(null, null);
			Main.NewText("World Regen Complete.", byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x004DFC24 File Offset: 0x004DDE24
		public static void DebugRotate()
		{
			int num = 0;
			int num2 = 0;
			int maxTilesY = Main.maxTilesY;
			for (int i = 0; i < Main.maxTilesX / Main.maxTilesY; i++)
			{
				for (int j = 0; j < maxTilesY / 2; j++)
				{
					for (int k = j; k < maxTilesY - j; k++)
					{
						Tile tile = Main.tile[k + num, j + num2];
						Main.tile[k + num, j + num2] = Main.tile[j + num, maxTilesY - k + num2];
						Main.tile[j + num, maxTilesY - k + num2] = Main.tile[maxTilesY - k + num, maxTilesY - j + num2];
						Main.tile[maxTilesY - k + num, maxTilesY - j + num2] = Main.tile[maxTilesY - j + num, k + num2];
						Main.tile[maxTilesY - j + num, k + num2] = tile;
					}
				}
				num += maxTilesY;
			}
		}
	}
}
