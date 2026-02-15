using System;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004D3 RID: 1235
	public class DungeonPillar : DungeonFeature
	{
		// Token: 0x060034CB RID: 13515 RVA: 0x00609E5F File Offset: 0x0060805F
		public DungeonPillar(DungeonFeatureSettings settings) : base(settings)
		{
			DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x00609EA4 File Offset: 0x006080A4
		public override bool GenerateFeature(DungeonData data, int x, int y)
		{
			this.generated = false;
			DungeonGenerationStyleData style = ((DungeonPillarSettings)this.settings).Style;
			if (this.Pillar(data, x, y, style.BrickTileType, style.BrickWallType, true))
			{
				this.generated = true;
				return true;
			}
			return false;
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x000379F1 File Offset: 0x00035BF1
		public override bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
		{
			return true;
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x00609EEC File Offset: 0x006080EC
		public bool Pillar(DungeonData data, int i, int j, ushort tileType, ushort wallType, bool generating = false)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			DungeonPillarSettings dungeonPillarSettings = (DungeonPillarSettings)this.settings;
			int width = dungeonPillarSettings.Width;
			int height = dungeonPillarSettings.Height;
			bool crowningOnTop = dungeonPillarSettings.CrowningOnTop;
			bool crowningOnBottom = dungeonPillarSettings.CrowningOnBottom;
			bool crowningStopsAtPillar = dungeonPillarSettings.CrowningStopsAtPillar;
			int num = 3;
			int num2 = 0;
			this.Bounds.SetBounds(i, j, i, j);
			int num3 = width / 2;
			for (int k = 0; k < width; k++)
			{
				int num4 = i + k - width / 2;
				int num5 = j;
				int num6 = j;
				DungeonPillar.GenerateTileStrip(dungeonPillarSettings, true, out num5, out num6, num4, j, height, (int)tileType, (int)wallType, false, false);
				this.Bounds.UpdateBounds(num4, num5, num4, num6);
				if (crowningOnTop)
				{
					int pillarHeight = crowningStopsAtPillar ? (num + 1) : 0;
					if (k == 0)
					{
						DungeonPillar.GenerateTileStrip(dungeonPillarSettings, true, out num2, out num2, num4 - 1, num5 + num, pillarHeight, (int)tileType, (int)wallType, false, true);
					}
					else if (k == width - 1)
					{
						DungeonPillar.GenerateTileStrip(dungeonPillarSettings, true, out num2, out num2, num4 + 1, num5 + num, pillarHeight, (int)tileType, (int)wallType, false, true);
					}
				}
				if (crowningOnBottom)
				{
					int pillarHeight2 = crowningStopsAtPillar ? (num + 1) : 0;
					if (k == 0)
					{
						DungeonPillar.GenerateTileStrip(dungeonPillarSettings, false, out num2, out num2, num4 - 1, num6 - num, pillarHeight2, (int)tileType, (int)wallType, true, false);
					}
					else if (k == width - 1)
					{
						DungeonPillar.GenerateTileStrip(dungeonPillarSettings, false, out num2, out num2, num4 + 1, num6 - num, pillarHeight2, (int)tileType, (int)wallType, true, false);
					}
				}
			}
			this.Bounds.CalculateHitbox();
			return true;
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x0060A050 File Offset: 0x00608250
		public static void GenerateTileStrip(DungeonPillarSettings pillarSettings, bool upwards, out int topY, out int bottomY, int placeX, int placeY, int pillarHeight, int tileType, int wallType, bool smoothTop, bool smoothBottom)
		{
			PillarType pillarType = pillarSettings.PillarType;
			ushort num = (ushort)((pillarType == PillarType.Wall) ? wallType : tileType);
			bool flag = pillarType == PillarType.Wall;
			int num2 = flag ? pillarSettings.OverridePaintWall : pillarSettings.OverridePaintTile;
			bool flag2 = pillarType == PillarType.BlockActuatedSolidTop || pillarType == PillarType.BlockActuatedSolidTopAndBottom;
			bool flag3 = pillarType == PillarType.BlockActuatedSolidBottom || pillarType == PillarType.BlockActuatedSolidTopAndBottom;
			bool flag4 = pillarType == PillarType.BlockActuated || pillarType == PillarType.BlockActuatedSolidTop || pillarType == PillarType.BlockActuatedSolidBottom || pillarType == PillarType.BlockActuatedSolidTopAndBottom;
			int num3 = pillarHeight;
			if (num3 == 0)
			{
				int num4 = 0;
				if (upwards)
				{
					while (num4 > -100 && WorldGen.InWorld(placeX, placeY + num4, 10) && !Main.tile[placeX, placeY + num4].active())
					{
						num4--;
					}
					num3 = -num4;
				}
				else
				{
					while (num4 < 100 && WorldGen.InWorld(placeX, placeY + num4, 10) && !Main.tile[placeX, placeY + num4].active())
					{
						num4++;
					}
					num3 = num4;
					placeY += num3 - 1;
				}
			}
			topY = placeY;
			bottomY = placeY;
			if (num3 == 0)
			{
				return;
			}
			int num5 = -num3 + 1;
			int num6 = 0;
			if (upwards)
			{
				for (int i = num5; i <= num6; i++)
				{
					int num7 = placeY + i;
					if (num7 <= 10)
					{
						return;
					}
					Tile tile = Main.tile[placeX, num7];
					if (!pillarSettings.AlwaysPlaceEntirePillar && tile.active())
					{
						return;
					}
					if (flag)
					{
						tile.wall = num;
						if (num2 >= 0)
						{
							tile.wallColor((byte)num2);
						}
					}
					else
					{
						tile.ClearTile();
						tile.active(true);
						tile.type = num;
						if (num2 >= 0)
						{
							tile.color((byte)num2);
						}
						if ((i == num5 && smoothTop) || (i == num6 && smoothBottom))
						{
							Tile.SmoothSlope(placeX, num7, false, false);
						}
						if ((!flag2 || i >= num5 + 2) && (!flag3 || i <= num6 - 2) && flag4)
						{
							tile.inActive(true);
						}
					}
					if (num7 < topY)
					{
						topY = num7;
					}
					if (num7 > bottomY)
					{
						bottomY = num7;
					}
				}
				return;
			}
			for (int j = num6; j >= num5; j--)
			{
				int num8 = placeY + j;
				if (num8 >= Main.maxTilesY - 10)
				{
					break;
				}
				Tile tile2 = Main.tile[placeX, num8];
				if (!pillarSettings.AlwaysPlaceEntirePillar && tile2.active())
				{
					break;
				}
				if (flag)
				{
					tile2.wall = num;
					if (num2 >= 0)
					{
						tile2.wallColor((byte)num2);
					}
				}
				else
				{
					tile2.ClearTile();
					tile2.active(true);
					tile2.type = num;
					if (num2 >= 0)
					{
						tile2.color((byte)num2);
					}
					if ((j == num5 && smoothTop) || (j == num6 && smoothBottom))
					{
						Tile.SmoothSlope(placeX, num8, false, false);
					}
					if ((!flag2 || j >= num5 + 2) && (!flag3 || j <= num6 - 2) && flag4)
					{
						tile2.inActive(true);
					}
				}
				if (num8 < topY)
				{
					topY = num8;
				}
				if (num8 > bottomY)
				{
					bottomY = num8;
				}
			}
		}
	}
}
