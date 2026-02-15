using System;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	// Token: 0x02000537 RID: 1335
	public static class ActiveSections
	{
		// Token: 0x14000058 RID: 88
		// (add) Token: 0x06003731 RID: 14129 RVA: 0x0062D83C File Offset: 0x0062BA3C
		// (remove) Token: 0x06003732 RID: 14130 RVA: 0x0062D870 File Offset: 0x0062BA70
		public static event Action<Point> SectionActivated;

		// Token: 0x06003733 RID: 14131 RVA: 0x0062D8A4 File Offset: 0x0062BAA4
		public static void CheckSection(Vector2 position, int fluff = 1)
		{
			int sectionX = Netplay.GetSectionX((int)(position.X / 16f));
			int sectionY = Netplay.GetSectionY((int)(position.Y / 16f));
			for (int i = sectionX - fluff; i < sectionX + fluff + 1; i++)
			{
				for (int j = sectionY - fluff; j < sectionY + fluff + 1; j++)
				{
					if (i >= 0 && i < Main.maxSectionsX && j >= 0 && j < Main.maxSectionsY)
					{
						bool flag = ActiveSections.IsSectionActive(new Point(i, j));
						ActiveSections.LastActiveTime[i, j] = Main.GameUpdateCount;
						if (!flag)
						{
							ActiveSections.SectionActivated(new Point(i, j));
						}
					}
				}
			}
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x0062D943 File Offset: 0x0062BB43
		public static bool IsSectionActive(Point sectionCoords)
		{
			sectionCoords = sectionCoords.ClampSectionCoords();
			return ActiveSections.LastActiveTime[sectionCoords.X, sectionCoords.Y] + ActiveSections.SectionInactiveTime >= Main.GameUpdateCount;
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x0062D973 File Offset: 0x0062BB73
		public static int TimeTillInactive(Point sectionCoords)
		{
			sectionCoords = sectionCoords.ClampSectionCoords();
			return (int)Math.Max(0L, (long)((ulong)(ActiveSections.LastActiveTime[sectionCoords.X, sectionCoords.Y] + ActiveSections.SectionInactiveTime) - (ulong)Main.GameUpdateCount));
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x0062D9A9 File Offset: 0x0062BBA9
		public static void Reset()
		{
			Array.Clear(ActiveSections.LastActiveTime, 0, ActiveSections.LastActiveTime.Length);
		}

		// Token: 0x06003737 RID: 14135 RVA: 0x0062D9C0 File Offset: 0x0062BBC0
		public static Point ClampSectionCoords(this Point point)
		{
			return new Point(Utils.Clamp<int>(point.X, 0, Main.maxSectionsX), Utils.Clamp<int>(point.Y, 0, Main.maxSectionsY));
		}

		// Token: 0x04005B55 RID: 23381
		public static readonly uint SectionInactiveTime = 60U;

		// Token: 0x04005B56 RID: 23382
		private static uint[,] LastActiveTime = new uint[Main.maxTilesX / 200 + 1, Main.maxTilesY / 150 + 1];
	}
}
