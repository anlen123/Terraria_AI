using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.Desert
{
	// Token: 0x02000516 RID: 1302
	public class DesertDescription
	{
		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06003654 RID: 13908 RVA: 0x006260A5 File Offset: 0x006242A5
		// (set) Token: 0x06003655 RID: 13909 RVA: 0x006260AD File Offset: 0x006242AD
		public Rectangle CombinedArea { get; private set; }

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06003656 RID: 13910 RVA: 0x006260B6 File Offset: 0x006242B6
		// (set) Token: 0x06003657 RID: 13911 RVA: 0x006260BE File Offset: 0x006242BE
		public Rectangle Desert { get; private set; }

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06003658 RID: 13912 RVA: 0x006260C7 File Offset: 0x006242C7
		// (set) Token: 0x06003659 RID: 13913 RVA: 0x006260CF File Offset: 0x006242CF
		public Rectangle Hive { get; private set; }

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x0600365A RID: 13914 RVA: 0x006260D8 File Offset: 0x006242D8
		// (set) Token: 0x0600365B RID: 13915 RVA: 0x006260E0 File Offset: 0x006242E0
		public Vector2D BlockScale { get; private set; }

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x0600365C RID: 13916 RVA: 0x006260E9 File Offset: 0x006242E9
		// (set) Token: 0x0600365D RID: 13917 RVA: 0x006260F1 File Offset: 0x006242F1
		public int BlockColumnCount { get; private set; }

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x0600365E RID: 13918 RVA: 0x006260FA File Offset: 0x006242FA
		// (set) Token: 0x0600365F RID: 13919 RVA: 0x00626102 File Offset: 0x00624302
		public int BlockRowCount { get; private set; }

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06003660 RID: 13920 RVA: 0x0062610B File Offset: 0x0062430B
		// (set) Token: 0x06003661 RID: 13921 RVA: 0x00626113 File Offset: 0x00624313
		public bool IsValid { get; private set; }

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06003662 RID: 13922 RVA: 0x0062611C File Offset: 0x0062431C
		// (set) Token: 0x06003663 RID: 13923 RVA: 0x00626124 File Offset: 0x00624324
		public SurfaceMap Surface { get; private set; }

		// Token: 0x06003664 RID: 13924 RVA: 0x0000357B File Offset: 0x0000177B
		private DesertDescription()
		{
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x00626130 File Offset: 0x00624330
		public void UpdateSurfaceMap()
		{
			this.Surface = SurfaceMap.FromArea(this.CombinedArea.Left - 5, this.CombinedArea.Width + 10);
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x00626168 File Offset: 0x00624368
		public static DesertDescription CreateFromPlacement(Point origin)
		{
			Vector2D defaultBlockScale = DesertDescription.DefaultBlockScale;
			double num = (double)Main.maxTilesX / 4200.0;
			int num2 = (int)(80.0 * num);
			int num3 = (int)((WorldGen.genRand.NextDouble() * 0.5 + 1.5) * 170.0 * num);
			if (WorldGen.remixWorldGen)
			{
				num3 = (int)(340.0 * num);
			}
			int num4 = (int)(defaultBlockScale.X * (double)num2);
			int num5 = (int)(defaultBlockScale.Y * (double)num3);
			origin.X -= num4 / 2;
			SurfaceMap surfaceMap = SurfaceMap.FromArea(origin.X - 5, num4 + 10);
			if (DesertDescription.RowHasInvalidTiles(origin.X, surfaceMap.Bottom, num4))
			{
				return DesertDescription.Invalid;
			}
			int num6 = (int)(surfaceMap.Average + (double)surfaceMap.Bottom) / 2;
			origin.Y = num6 + WorldGen.genRand.Next(40, 60);
			int num7 = 0;
			if (Main.tenthAnniversaryWorld)
			{
				num7 = (int)(20.0 * num);
			}
			return new DesertDescription
			{
				CombinedArea = new Rectangle(origin.X, num6, num4, origin.Y + num5 - num6),
				Hive = new Rectangle(origin.X, origin.Y + num7, num4, num5 - num7),
				Desert = new Rectangle(origin.X, num6, num4, origin.Y + num5 / 2 - num6 + num7),
				BlockScale = defaultBlockScale,
				BlockColumnCount = num2,
				BlockRowCount = num3,
				Surface = surfaceMap,
				IsValid = true
			};
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x00626308 File Offset: 0x00624508
		private static bool RowHasInvalidTiles(int startX, int startY, int width)
		{
			if (GenVars.skipDesertTileCheck)
			{
				return false;
			}
			for (int i = startX; i < startX + width; i++)
			{
				ushort type = Main.tile[i, startY].type;
				if ((!WorldGen.notTheBees || WorldGen.remixWorldGen) && (type == 59 || type == 60))
				{
					return true;
				}
				if (type == 161 || type == 147)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005AF6 RID: 23286
		public static readonly DesertDescription Invalid = new DesertDescription
		{
			IsValid = false
		};

		// Token: 0x04005AF7 RID: 23287
		private static readonly Vector2D DefaultBlockScale = new Vector2D(4.0, 2.0);

		// Token: 0x04005AF8 RID: 23288
		private const int SCAN_PADDING = 5;
	}
}
