using System;
using Terraria.DataStructures;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004CE RID: 1230
	public class DungeonWindowBasic : DungeonWindow
	{
		// Token: 0x060034C3 RID: 13507 RVA: 0x00608D85 File Offset: 0x00606F85
		public DungeonWindowBasic(DungeonFeatureSettings settings) : base(settings)
		{
		}

		// Token: 0x060034C4 RID: 13508 RVA: 0x00609B3C File Offset: 0x00607D3C
		public override bool GenerateFeature(DungeonData data, int x, int y)
		{
			this.generated = false;
			DungeonGenerationStyleData style = ((DungeonWindowBasicSettings)this.settings).Style;
			if (this.Window(data, x, y, style, true))
			{
				this.generated = true;
				return true;
			}
			return false;
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x00609B78 File Offset: 0x00607D78
		public bool Window(DungeonData data, int placeX, int placeY, DungeonGenerationStyleData style, bool generating = false)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			DungeonWindowBasicSettings dungeonWindowBasicSettings = (DungeonWindowBasicSettings)this.settings;
			int width = dungeonWindowBasicSettings.Width;
			int height = dungeonWindowBasicSettings.Height;
			int overrideGlassPaint = dungeonWindowBasicSettings.OverrideGlassPaint;
			ushort wall = dungeonWindowBasicSettings.Closed ? style.WindowClosedGlassWallType : style.WindowGlassWallType;
			ushort windowEdgeWallType = style.WindowEdgeWallType;
			int num = style.GetWindowPlatformStyle(genRand);
			if (dungeonWindowBasicSettings.OverrideGlassType > 0)
			{
				wall = (ushort)dungeonWindowBasicSettings.OverrideGlassType;
			}
			if (dungeonWindowBasicSettings.OverridePlatformStyle > -1)
			{
				num = dungeonWindowBasicSettings.OverridePlatformStyle;
			}
			this.Bounds.SetBounds(placeX, placeY, placeX, placeY);
			for (int i = 0; i < width; i++)
			{
				int num2 = placeX + i - width / 2;
				for (int j = 0; j < height; j++)
				{
					if (this.Window_ValidWindowSpot(i, j, width, height))
					{
						int num3 = placeY + j - height / 2;
						if (i == width / 2 || j == height / 2)
						{
							Main.tile[num2, num3].wall = windowEdgeWallType;
						}
						else
						{
							Main.tile[num2, num3].wall = wall;
							if (overrideGlassPaint >= 0)
							{
								Main.tile[num2, num3].wallColor((byte)overrideGlassPaint);
							}
						}
						this.Bounds.UpdateBounds(num2, num3);
						if (!this.Window_ValidWindowSpot(i - 1, j, width, height))
						{
							Main.tile[num2 - 1, num3].wall = windowEdgeWallType;
							this.Bounds.UpdateBounds(num2 - 1, num3);
						}
						if (!this.Window_ValidWindowSpot(i + 1, j, width, height))
						{
							Main.tile[num2 + 1, num3].wall = windowEdgeWallType;
							this.Bounds.UpdateBounds(num2 + 1, num3);
						}
						if (!this.Window_ValidWindowSpot(i, j - 1, width, height))
						{
							Main.tile[num2, num3 - 1].wall = windowEdgeWallType;
							this.Bounds.UpdateBounds(num2, num3 - 1);
						}
						if (!this.Window_ValidWindowSpot(i, j + 1, width, height))
						{
							Main.tile[num2, num3 + 1].wall = windowEdgeWallType;
							this.Bounds.UpdateBounds(num2, num3 + 1);
							if (num > -1)
							{
								Main.tile[num2, num3 + 1].active(true);
								Main.tile[num2, num3 + 1].type = 19;
								Main.tile[num2, num3 + 1].Clear(TileDataType.Slope);
								Main.tile[num2, num3 + 1].frameY = (short)(num * 18);
								WorldGen.TileFrame(num2, num3 + 1, false, false);
							}
						}
					}
				}
			}
			this.Bounds.CalculateHitbox();
			return true;
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x00609E25 File Offset: 0x00608025
		private bool Window_ValidWindowSpot(int x, int y, int width, int height)
		{
			return x >= 0 && y >= 0 && x < width && y < height && (y != 0 || (x != 0 && x != width - 1));
		}
	}
}
