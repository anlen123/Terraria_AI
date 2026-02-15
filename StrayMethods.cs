using System;
using Microsoft.Xna.Framework;

namespace Terraria
{
	// Token: 0x02000021 RID: 33
	public class StrayMethods
	{
		// Token: 0x06000173 RID: 371 RVA: 0x0001153C File Offset: 0x0000F73C
		public static bool CountSandHorizontally(int i, int j, bool[] fittingTypes, int requiredTotalSpread = 4, int spreadInEachAxis = 5)
		{
			if (!WorldGen.InWorld(i, j, 2))
			{
				return false;
			}
			int num = 0;
			int num2 = 0;
			int num3 = i - 1;
			while (num < spreadInEachAxis && num3 > 0)
			{
				Tile tile = Main.tile[num3, j];
				if (tile.active() && fittingTypes[(int)tile.type] && !WorldGen.SolidTileAllowBottomSlope(num3, j - 1))
				{
					num++;
				}
				else if (!tile.active())
				{
					break;
				}
				num3--;
			}
			num3 = i + 1;
			while (num2 < spreadInEachAxis && num3 < Main.maxTilesX - 1)
			{
				Tile tile2 = Main.tile[num3, j];
				if (tile2.active() && fittingTypes[(int)tile2.type] && !WorldGen.SolidTileAllowBottomSlope(num3, j - 1))
				{
					num2++;
				}
				else if (!tile2.active())
				{
					break;
				}
				num3++;
			}
			return num + num2 + 1 >= requiredTotalSpread;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00011604 File Offset: 0x0000F804
		public static bool CanSpawnSandstormHostile(Vector2 position, int expandUp, int expandDown)
		{
			bool result = true;
			Point point = position.ToTileCoordinates();
			for (int i = -1; i <= 1; i++)
			{
				int num;
				int num2;
				Collision.ExpandVertically(point.X + i, point.Y, out num, out num2, expandUp, expandDown);
				num++;
				num2--;
				if (num2 - num < 20)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00011658 File Offset: 0x0000F858
		public static bool CanSpawnSandstormFriendly(Vector2 position, int expandUp, int expandDown)
		{
			bool result = true;
			Point point = position.ToTileCoordinates();
			for (int i = -1; i <= 1; i++)
			{
				int num;
				int num2;
				Collision.ExpandVertically(point.X + i, point.Y, out num, out num2, expandUp, expandDown);
				num++;
				num2--;
				if (num2 - num < 10)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000116AC File Offset: 0x0000F8AC
		public static void CheckArenaScore(Vector2 arenaCenter, out Point xLeftEnd, out Point xRightEnd, int walkerWidthInTiles = 5, int walkerHeightInTiles = 10)
		{
			bool flag = false;
			int maxDistance = Main.MaxWorldViewSize.X / 16;
			Point point = arenaCenter.ToTileCoordinates();
			xLeftEnd = (xRightEnd = point);
			int num;
			int y;
			Collision.ExpandVertically(point.X, point.Y, out num, out y, 0, 4);
			point.Y = y;
			if (flag)
			{
				Dust.QuickDust(point, Color.Blue).scale = 5f;
			}
			int num2;
			Point point2;
			StrayMethods.SendWalker(point, walkerHeightInTiles, -1, out num2, out point2, maxDistance, flag);
			int num3;
			Point point3;
			StrayMethods.SendWalker(point, walkerHeightInTiles, 1, out num3, out point3, maxDistance, flag);
			point2.X++;
			point3.X--;
			if (flag)
			{
				Dust.QuickDustLine(point2.ToWorldCoordinates(8f, 8f), point3.ToWorldCoordinates(8f, 8f), 50f, Color.Pink);
			}
			xLeftEnd = point2;
			xRightEnd = point3;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00011794 File Offset: 0x0000F994
		public static void SendWalker(Point startFloorPosition, int height, int direction, out int distanceCoveredInTiles, out Point lastIteratedFloorSpot, int maxDistance = 100, bool showDebug = false)
		{
			distanceCoveredInTiles = 0;
			startFloorPosition.Y--;
			lastIteratedFloorSpot = startFloorPosition;
			for (int i = 0; i < maxDistance; i++)
			{
				int num = 0;
				while (num < 3 && WorldGen.SolidTile3(startFloorPosition.X, startFloorPosition.Y))
				{
					startFloorPosition.Y--;
					num++;
				}
				int num2;
				int num3;
				Collision.ExpandVertically(startFloorPosition.X, startFloorPosition.Y, out num2, out num3, height, 2);
				num2++;
				num3--;
				if (!WorldGen.SolidTile3(startFloorPosition.X, num3 + 1))
				{
					int num4;
					int num5;
					Collision.ExpandVertically(startFloorPosition.X, num3, out num4, out num5, 0, 6);
					if (showDebug)
					{
						Dust.QuickBox(new Vector2((float)(startFloorPosition.X * 16 + 8), (float)(num4 * 16)), new Vector2((float)(startFloorPosition.X * 16 + 8), (float)(num5 * 16)), 1, Color.Blue, 1f, 1f, null);
					}
					if (!WorldGen.SolidTile3(startFloorPosition.X, num5))
					{
						break;
					}
				}
				if (num3 - num2 < height - 1)
				{
					break;
				}
				if (showDebug)
				{
					Dust.QuickDust(startFloorPosition, Color.Green).scale = 1f;
					Dust.QuickBox(new Vector2((float)(startFloorPosition.X * 16 + 8), (float)(num2 * 16)), new Vector2((float)(startFloorPosition.X * 16 + 8), (float)(num3 * 16 + 16)), 1, Color.Red, 1f, 1f, null);
				}
				distanceCoveredInTiles += direction;
				startFloorPosition.X += direction;
				startFloorPosition.Y = num3;
				lastIteratedFloorSpot = startFloorPosition;
				if (Math.Abs(distanceCoveredInTiles) >= maxDistance)
				{
					break;
				}
			}
			distanceCoveredInTiles = Math.Abs(distanceCoveredInTiles);
		}
	}
}
