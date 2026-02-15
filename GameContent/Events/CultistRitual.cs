using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.Events
{
	// Token: 0x020004FF RID: 1279
	public class CultistRitual
	{
		// Token: 0x060035C1 RID: 13761 RVA: 0x0061CB80 File Offset: 0x0061AD80
		public static void UpdateTime()
		{
			if (Main.netMode == 1)
			{
				return;
			}
			CultistRitual.delay -= Main.dayRate;
			if (CultistRitual.delay < 0)
			{
				CultistRitual.delay = 0;
			}
			CultistRitual.recheck -= Main.dayRate;
			if (CultistRitual.recheck < 0)
			{
				CultistRitual.recheck = 0;
			}
			if (CultistRitual.delay == 0 && CultistRitual.recheck == 0)
			{
				CultistRitual.recheck = 600;
				if (NPC.AnyDanger(false, false))
				{
					CultistRitual.recheck *= 6;
					return;
				}
				CultistRitual.TrySpawning(Main.dungeonX, Main.dungeonY, false);
			}
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x0061CC11 File Offset: 0x0061AE11
		public static void CultistSlain()
		{
			CultistRitual.delay -= 3600;
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x0061CC23 File Offset: 0x0061AE23
		public static void TabletDestroyed()
		{
			CultistRitual.delay = 43200;
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x0061CC30 File Offset: 0x0061AE30
		public static bool TrySpawning(int x, int y, bool force = false)
		{
			if (x < 0 || y < 0 || x >= Main.maxTilesX || y >= Main.maxTilesY)
			{
				return false;
			}
			if (!force && (WorldGen.PlayerLOS(x - 6, y) || WorldGen.PlayerLOS(x + 6, y)))
			{
				return false;
			}
			if (!CultistRitual.CheckRitual(x, y, force))
			{
				return false;
			}
			NPC.NewNPC(new EntitySource_WorldEvent(), x * 16 + 8, (y - 4) * 16 - 8, 437, 0, 0f, 0f, 0f, 0f, 255);
			return true;
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x0061CCB8 File Offset: 0x0061AEB8
		private static bool CheckRitual(int x, int y, bool force = false)
		{
			if (!force && (CultistRitual.delay != 0 || !Main.hardMode || !NPC.downedGolemBoss || !NPC.downedBoss3))
			{
				return false;
			}
			if (y < 7 || WorldGen.SolidTile(Main.tile[x, y - 7]))
			{
				return false;
			}
			if (!force && NPC.AnyNPCs(437))
			{
				return false;
			}
			Vector2 center = new Vector2((float)(x * 16 + 8), (float)(y * 16 - 64 - 8 - 27));
			Point[] array = null;
			return CultistRitual.CheckFloor(center, out array);
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x0061CD3C File Offset: 0x0061AF3C
		public static bool CheckFloor(Vector2 Center, out Point[] spawnPoints)
		{
			Point[] array = new Point[4];
			int num = 0;
			Point point = Center.ToTileCoordinates();
			for (int i = -5; i <= 5; i += 2)
			{
				if (i != -1 && i != 1)
				{
					for (int j = -5; j < 12; j++)
					{
						int num2 = point.X + i * 2;
						int num3 = point.Y + j;
						if ((WorldGen.SolidTile(num2, num3, false) || TileID.Sets.Platforms[(int)Framing.GetTileSafely(num2, num3).type]) && (!Collision.SolidTiles(num2 - 1, num2 + 1, num3 - 3, num3 - 1) || (!Collision.SolidTiles(num2, num2, num3 - 3, num3 - 1) && !Collision.SolidTiles(num2 + 1, num2 + 1, num3 - 3, num3 - 2) && !Collision.SolidTiles(num2 - 1, num2 - 1, num3 - 3, num3 - 2))))
						{
							array[num++] = new Point(num2, num3);
							break;
						}
					}
				}
			}
			if (num != 4)
			{
				spawnPoints = null;
				return false;
			}
			spawnPoints = array;
			return true;
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x0061CE48 File Offset: 0x0061B048
		public static bool CheckFloor2(Vector2 Center, out Point[] spawnPoints)
		{
			Point[] array = new Point[2];
			int num = 0;
			Point point = Center.ToTileCoordinates();
			for (int i = -3; i <= 3; i += 2)
			{
				if (i != -1 && i != 1)
				{
					for (int j = -5; j < 12; j++)
					{
						int num2 = point.X + i * 2;
						int num3 = point.Y + j;
						if ((WorldGen.SolidTile(num2, num3, false) || TileID.Sets.Platforms[(int)Framing.GetTileSafely(num2, num3).type]) && (!Collision.SolidTiles(num2 - 1, num2 + 1, num3 - 3, num3 - 1) || (!Collision.SolidTiles(num2, num2, num3 - 3, num3 - 1) && !Collision.SolidTiles(num2 + 1, num2 + 1, num3 - 3, num3 - 2) && !Collision.SolidTiles(num2 - 1, num2 - 1, num3 - 3, num3 - 2))))
						{
							array[num++] = new Point(num2, num3);
							break;
						}
					}
				}
			}
			if (num != 2)
			{
				spawnPoints = null;
				return false;
			}
			spawnPoints = array;
			return true;
		}

		// Token: 0x04005ABD RID: 23229
		public const int delayStart = 86400;

		// Token: 0x04005ABE RID: 23230
		public const int respawnDelay = 43200;

		// Token: 0x04005ABF RID: 23231
		private const int timePerCultist = 3600;

		// Token: 0x04005AC0 RID: 23232
		private const int recheckStart = 600;

		// Token: 0x04005AC1 RID: 23233
		public static int delay;

		// Token: 0x04005AC2 RID: 23234
		public static int recheck;
	}
}
