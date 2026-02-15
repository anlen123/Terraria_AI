using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.Entrances
{
	// Token: 0x020004F2 RID: 1266
	public class DomeDungeonEntrance : DungeonEntrance
	{
		// Token: 0x0600352E RID: 13614 RVA: 0x0061348C File Offset: 0x0061168C
		public DomeDungeonEntrance(DungeonEntranceSettings settings) : base(settings)
		{
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x006146B9 File Offset: 0x006128B9
		public override void CalculateEntrance(DungeonData data, int x, int y)
		{
			this.calculated = false;
			this.DomeEntrance(data, x, y, false);
			this.calculated = true;
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x006146D3 File Offset: 0x006128D3
		public override bool GenerateEntrance(DungeonData data, int x, int y)
		{
			this.generated = false;
			this.DomeEntrance(data, x, y, true);
			this.generated = true;
			return true;
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x006134CA File Offset: 0x006116CA
		public override bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
		{
			return !(feature is DungeonGlobalBookshelves) && !(feature is DungeonGlobalPaintings) && !(feature is DungeonGlobalSpikes) && base.CanGenerateFeatureAt(data, feature, x, y);
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x006146F0 File Offset: 0x006128F0
		public void DomeEntrance(DungeonData data, int i, int j, bool generating)
		{
			UnifiedRandom unifiedRandom = new UnifiedRandom(((DomeDungeonEntranceSettings)this.settings).RandomSeed);
			ushort brickTileType = this.settings.StyleData.BrickTileType;
			ushort brickWallType = this.settings.StyleData.BrickWallType;
			bool dungeonEntranceIsBuried = SpecialSeedFeatures.DungeonEntranceIsBuried;
			bool dungeonEntranceIsUnderground = SpecialSeedFeatures.DungeonEntranceIsUnderground;
			bool flag = data.genVars.dungeonSide == (int)DungeonSide.Left;
			if (Main.drunkWorld)
			{
				flag = !flag;
			}
			bool flag2 = unifiedRandom.Next(4) != 0;
			WindowType mosaicType;
			switch (unifiedRandom.Next(3))
			{
			default:
				mosaicType = WindowType.RegularWindows;
				break;
			case 1:
				mosaicType = WindowType.SkeletronMosaic;
				break;
			case 2:
				mosaicType = WindowType.MoonLordMosaic;
				break;
			}
			this.Bounds.SetBounds(i, j, i, j);
			if (generating)
			{
				int num = 60;
				for (int k = i - num; k < i + num; k++)
				{
					for (int l = j - num; l < j + num; l++)
					{
						if (WorldGen.InWorld(k, l, 0))
						{
							Main.tile[k, l].liquid = 0;
							Main.tile[k, l].lava(false);
							Main.tile[k, l].Clear(TileDataType.Slope);
						}
					}
				}
			}
			int num2 = 5;
			int num3 = 35;
			int num4 = num3 + num2;
			int num5 = 100;
			int num6 = 30;
			int num7 = j - num6;
			int m = 10;
			int num8 = 50;
			if (data.Type == DungeonType.DualDungeon)
			{
				num5 = DungeonUtils.GetDualDungeonBrickSupportCutoffY(data) - num7;
			}
			else if (dungeonEntranceIsUnderground)
			{
				num5 = num8 - m + 5;
			}
			if (generating && !dungeonEntranceIsBuried && !dungeonEntranceIsUnderground)
			{
				int x = i - num4 + 1;
				if (flag)
				{
					x = i + num4 - 1;
				}
				int num9 = 20;
				WorldUtils.Gen(new Point(x, num7 - num9), new Shapes.Circle(num9, num9), Actions.Chain(new GenAction[]
				{
					new Actions.Clear()
				}));
			}
			this.Bounds.UpdateBounds(i - num4, num7 - num4, i + num4 + 1, num7 + 10);
			if (generating)
			{
				int num10 = -5;
				int num11 = num5;
				for (int n = -num4; n <= num4; n++)
				{
					for (int num12 = num10; num12 < num11; num12++)
					{
						int num13 = i + n;
						int num14 = num7 + num12;
						if (WorldGen.InWorld(num13, num14, 0))
						{
							Tile tile = Main.tile[num13, num14];
							bool flag3 = tile.active() && !this.settings.StyleData.TileIsInStyle((int)tile.type, true);
							bool flag4 = !this.settings.StyleData.WallIsInStyle((int)tile.wall, false);
							bool flag5 = DungeonUtils.IsConsideredDungeonWall((int)tile.wall, false);
							if (num12 < 0)
							{
								tile.ClearEverything();
							}
							else if (num12 >= 0 && num12 < 5)
							{
								if ((n >= -num3 + num2 && n <= -num3 + num2 * 2) || (n >= num3 - num2 * 2 && n <= num3 - num2))
								{
									tile.ClearEverything();
									if (!flag5)
									{
										tile.wall = brickWallType;
									}
								}
								else if (!flag5)
								{
									tile.liquid = 0;
									tile.active(true);
									tile.type = brickTileType;
									if (n != -num4 && n != num4)
									{
										tile.wall = brickWallType;
									}
								}
							}
							else if (num12 >= 5 && num12 < 10)
							{
								if (n >= -num3 + num2 && n <= num3 - num2)
								{
									tile.ClearEverything();
									tile.wall = brickWallType;
								}
								else if (!flag5)
								{
									tile.liquid = 0;
									tile.active(true);
									tile.type = brickTileType;
									if (n != -num4 && n != num4)
									{
										tile.wall = brickWallType;
									}
								}
							}
							else if ((tile.active() && flag3) || !flag5)
							{
								tile.liquid = 0;
								tile.active(true);
								tile.type = brickTileType;
								if (n != -num4 && n != num4)
								{
									tile.wall = brickWallType;
								}
							}
							else if (flag4)
							{
								tile.liquid = 0;
								if (n != -num4 && n != num4)
								{
									tile.wall = brickWallType;
								}
							}
							if (num12 == 1 && (n == -num3 + num2 || n == num3 - num2 * 2))
							{
								DungeonPlatformData item = new DungeonPlatformData
								{
									Position = new Point(num13, num14),
									OverrideHeightFluff = new int?(0),
									ForcePlacement = true,
									PlacePotsChance = 0.33000001311302185
								};
								data.dungeonPlatformData.Add(item);
							}
							if (num12 == 10 && n == 0)
							{
								DungeonPlatformData item2 = new DungeonPlatformData
								{
									Position = new Point(num13, num14),
									OverrideHeightFluff = new int?(0),
									ForcePlacement = true,
									PlacePotsChance = 0.33000001311302185
								};
								data.dungeonPlatformData.Add(item2);
							}
						}
					}
				}
				int num15 = -1;
				int num16 = 6;
				while (m < num8)
				{
					Tile tile2 = Main.tile[i, num7 + m];
					if (num15 == -1 && !tile2.active())
					{
						num15 = 15;
					}
					if (num15 > 0)
					{
						num15--;
						if (num15 <= 0)
						{
							break;
						}
						if (num15 <= 5)
						{
							num16--;
						}
					}
					for (int num17 = -num16; num17 <= num16; num17++)
					{
						Tile tile3 = Main.tile[i + num17, num7 + m];
						tile3.ClearEverything();
						if (!DungeonUtils.IsConsideredDungeonWall((int)tile3.wall, false))
						{
							tile3.wall = brickWallType;
						}
					}
					m++;
				}
			}
			int num18 = num7 + 1;
			if (generating)
			{
				WorldUtils.Gen(new Point(i, num7), new Shapes.Slime(num4, 1.0, 1.0), Actions.Chain(new GenAction[]
				{
					new Modifiers.IsAboveHeight(num18, false),
					new Modifiers.SkipWalls(new ushort[]
					{
						brickWallType
					}),
					new Actions.UpdateBounds(data.dungeonBounds),
					new Actions.Clear(),
					new Actions.SetTile(brickTileType, false, false, false)
				}));
				WorldUtils.Gen(new Point(i, num7), new Shapes.Slime(num4 - 2, 1.0, 1.0), Actions.Chain(new GenAction[]
				{
					new Modifiers.IsAboveHeight(num18 + 1, false),
					new Actions.SetWall(brickWallType, false, false, false)
				}));
			}
			if (generating)
			{
				ushort num19 = 0;
				int num20 = 2;
				if (WorldGen.SecretSeed.surfaceIsDesert.Enabled)
				{
					num19 = 53;
					num20 = -1;
				}
				WorldUtils.Gen(new Point(i, num7 - num2 + 1), new Shapes.Slime(num4, 0.8999999761581421, 1.100000023841858), Actions.Chain(new GenAction[]
				{
					new Modifiers.IsAboveHeight(num18 - 2, false),
					new Modifiers.SkipTiles(new ushort[]
					{
						brickTileType
					}),
					new Modifiers.SkipWalls(new ushort[]
					{
						brickWallType
					}),
					new Actions.Clear(),
					new Actions.SetTile(num19, false, false, false)
				}));
				if (!dungeonEntranceIsUnderground && num20 > -1)
				{
					WorldUtils.Gen(new Point(i, num7 - num2 + 1), new Shapes.Slime(num4, 0.8999999761581421, 1.100000023841858), Actions.Chain(new GenAction[]
					{
						new Modifiers.IsAboveHeight(num18 - 2, false),
						new Modifiers.OnlyTiles(new ushort[]
						{
							num19
						}),
						new Modifiers.IsTouchingAir(true),
						new Actions.SetTile((ushort)num20, false, false, false)
					}));
				}
			}
			if (generating)
			{
				WorldUtils.Gen(new Point(i, num7), new Shapes.Slime(num3, 1.0, 1.0), Actions.Chain(new GenAction[]
				{
					new Modifiers.IsAboveHeight(num18, false),
					new Actions.ClearTile(false)
				}));
			}
			if (generating)
			{
				this.DomeEntrance_Door(data, i, num7, num4, num3, flag, dungeonEntranceIsBuried);
				if (dungeonEntranceIsBuried || dungeonEntranceIsUnderground)
				{
					this.DomeEntrance_Door(data, i, num7, num4, num3, !flag, dungeonEntranceIsBuried);
				}
			}
			if (generating)
			{
				DungeonWindowBasicSettings dungeonWindowBasicSettings = new DungeonWindowBasicSettings
				{
					Style = this.settings.StyleData,
					Width = 5,
					Height = 24,
					Closed = dungeonEntranceIsUnderground
				};
				DungeonWindowMosaicSettings dungeonWindowMosaicSettings = new DungeonWindowMosaicSettings
				{
					Style = this.settings.StyleData,
					Closed = dungeonEntranceIsUnderground,
					MosaicType = mosaicType
				};
				switch (mosaicType)
				{
				case WindowType.RegularWindows:
					new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i - 8, num7 - 16);
					new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i + 8, num7 - 16);
					dungeonWindowBasicSettings.Height = 28;
					new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i, num7 - 17);
					dungeonWindowBasicSettings.Height = 10;
					new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i - num3 + 6, num7 - 8);
					new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i + num3 - 6, num7 - 8);
					dungeonWindowBasicSettings.Height = 11;
					new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i - num3 + 15, num7 - 11);
					new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i + num3 - 15, num7 - 11);
					break;
				case WindowType.SkeletronMosaic:
					if (!dungeonEntranceIsUnderground)
					{
						dungeonWindowMosaicSettings.OverrideGlassType = 89;
					}
					dungeonWindowMosaicSettings.OverrideGlassPaint = 26;
					new DungeonWindowMosaic(dungeonWindowMosaicSettings).GenerateFeature(data, i, num7 - 19);
					dungeonWindowBasicSettings.OverrideGlassPaint = 26;
					dungeonWindowBasicSettings.Height = 10;
					new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i - num3 + 6, num7 - 8);
					new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i + num3 - 6, num7 - 8);
					dungeonWindowBasicSettings.Height = 11;
					new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i - num3 + 15, num7 - 11);
					new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i + num3 - 15, num7 - 11);
					break;
				case WindowType.MoonLordMosaic:
					if (!dungeonEntranceIsUnderground)
					{
						dungeonWindowMosaicSettings.OverrideGlassType = 91;
					}
					new DungeonWindowMosaic(dungeonWindowMosaicSettings).GenerateFeature(data, i, num7 - 17);
					dungeonWindowBasicSettings.Height = 10;
					if (!dungeonEntranceIsUnderground)
					{
						dungeonWindowBasicSettings.OverrideGlassType = 241;
					}
					new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i - num3 + 6, num7 - 8);
					dungeonWindowBasicSettings.OverrideGlassType = 91;
					new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i + num3 - 6, num7 - 8);
					dungeonWindowBasicSettings.Height = 11;
					if (!dungeonEntranceIsUnderground)
					{
						dungeonWindowBasicSettings.OverrideGlassType = 90;
					}
					new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i - num3 + 15, num7 - 11);
					if (!dungeonEntranceIsUnderground)
					{
						dungeonWindowBasicSettings.OverrideGlassType = 88;
					}
					new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i + num3 - 15, num7 - 11);
					break;
				}
			}
			DungeonPillarSettings dungeonPillarSettings = new DungeonPillarSettings
			{
				Style = this.settings.StyleData,
				PillarType = PillarType.BlockActuated,
				Width = 3,
				Height = 0,
				CrowningOnTop = true,
				CrowningOnBottom = true,
				CrowningStopsAtPillar = false,
				AlwaysPlaceEntirePillar = true
			};
			if (generating)
			{
				new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, i - num3 + 21, num7);
				new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, i + num3 - 21, num7);
				DungeonPlatformData item3 = new DungeonPlatformData
				{
					Position = new Point(i - num3 + 15, num7 - 25),
					OverrideHeightFluff = new int?(0),
					ForcePlacement = true,
					PlacePotsChance = 0.33000001311302185,
					PlaceBooksChance = 0.75,
					PlacePotionBottlesChance = 0.10000000149011612,
					NoWaterbolt = true
				};
				data.dungeonPlatformData.Add(item3);
				item3 = new DungeonPlatformData
				{
					Position = new Point(i + num3 - 15, num7 - 25),
					OverrideHeightFluff = new int?(0),
					ForcePlacement = true,
					PlacePotsChance = 0.33000001311302185,
					PlaceBooksChance = 0.75,
					PlacePotionBottlesChance = 0.10000000149011612,
					NoWaterbolt = true
				};
				data.dungeonPlatformData.Add(item3);
				item3 = new DungeonPlatformData
				{
					Position = new Point(i - num3 + 15, num7 - 20),
					OverrideHeightFluff = new int?(0),
					ForcePlacement = true,
					PlacePotsChance = 0.33000001311302185,
					PlaceBooksChance = 0.75,
					PlacePotionBottlesChance = 0.10000000149011612
				};
				data.dungeonPlatformData.Add(item3);
				item3 = new DungeonPlatformData
				{
					Position = new Point(i + num3 - 15, num7 - 20),
					OverrideHeightFluff = new int?(0),
					ForcePlacement = true,
					PlacePotsChance = 0.33000001311302185,
					PlaceBooksChance = 0.75,
					PlacePotionBottlesChance = 0.10000000149011612
				};
				data.dungeonPlatformData.Add(item3);
			}
			if (generating)
			{
				int num21 = 16;
				dungeonPillarSettings.PillarType = PillarType.Block;
				dungeonPillarSettings.CrowningOnTop = false;
				dungeonPillarSettings.CrowningOnBottom = false;
				dungeonPillarSettings.Width = 5;
				dungeonPillarSettings.Height = num21;
				new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, i - num4 + 2, num7 - 10);
				new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, i + num4 - 2, num7 - 10);
				dungeonPillarSettings.Width = 4;
				dungeonPillarSettings.Height = num21 - 2;
				new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, i - num3 + 8, num7 - 28);
				new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, i + num3 - 8, num7 - 28);
				dungeonPillarSettings.Width = 3;
				dungeonPillarSettings.Height = num21 - 3;
				new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, i - num3 + 21, num7 - 37);
				new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, i + num3 - 21, num7 - 37);
				if (flag2)
				{
					this.DomeEntrance_TreeOnPillar(unifiedRandom, i - num4 + 2, num7 - 10 - num21 + 1);
					this.DomeEntrance_TreeOnPillar(unifiedRandom, i - num3 + 8, num7 - 28 - num21 + 2 + 1);
					this.DomeEntrance_TreeOnPillar(unifiedRandom, i - num3 + 21, num7 - 37 - num21 + 3 + 1);
					this.DomeEntrance_TreeOnPillar(unifiedRandom, i + num4 - 2, num7 - 10 - num21 + 1);
					this.DomeEntrance_TreeOnPillar(unifiedRandom, i + num3 - 8, num7 - 28 - num21 + 2 + 1);
					this.DomeEntrance_TreeOnPillar(unifiedRandom, i + num3 - 21, num7 - 37 - num21 + 3 + 1);
				}
			}
			this.OldManSpawn = DungeonUtils.SetOldManSpawnAndSpawnOldManIfDefaultDungeon(i, num7, generating);
			if (generating && SpecialSeedFeatures.DungeonEntranceHasATree)
			{
				DungeonUtils.GenerateDungeonTree(data, i, (int)Main.worldSurface, num7 - num3 + 5, false);
			}
			if (generating && SpecialSeedFeatures.DungeonEntranceHasStairs)
			{
				int i2 = flag ? (i + num4) : (i - num4);
				DungeonUtils.GenerateDungeonStairs(data, i2, num7, flag ? 1 : -1, brickTileType, brickWallType, num5);
			}
			this.Bounds.CalculateHitbox();
		}

		// Token: 0x06003533 RID: 13619 RVA: 0x006155DC File Offset: 0x006137DC
		public void DomeEntrance_Door(DungeonData data, int i, int entranceFloor, int outerSize, int innerSize, bool leftDungeonDoor, bool buried)
		{
			int num = leftDungeonDoor ? (innerSize - 1) : (-outerSize - 2);
			int num2 = leftDungeonDoor ? (outerSize + 2) : (-innerSize + 1);
			if (buried)
			{
				num += 2 * (leftDungeonDoor ? 0 : 1);
				num2 += 2 * (leftDungeonDoor ? -1 : 0);
			}
			Point point = new Point(i + (leftDungeonDoor ? (outerSize - 1) : (-outerSize + 1)), entranceFloor);
			Point point2 = new Point(i + (leftDungeonDoor ? (innerSize + 1) : (-innerSize - 1)), entranceFloor);
			for (int j = num; j <= num2; j++)
			{
				for (int k = -3; k <= 1; k++)
				{
					int num3 = j + i;
					int num4 = k + entranceFloor;
					Tile tile = Main.tile[num3, num4];
					if (!buried && ((leftDungeonDoor && num3 >= point.X) || (!leftDungeonDoor && num3 <= point.X)))
					{
						tile.wall = 0;
					}
					if (k >= -2 && k <= 0)
					{
						tile.ClearTile();
					}
				}
			}
			WorldGen.PlaceTile(point.X, point.Y, 10, true, true, -1, 13);
			WorldGen.PlaceTile(point2.X, point2.Y, 10, true, true, -1, 13);
		}

		// Token: 0x06003534 RID: 13620 RVA: 0x00615704 File Offset: 0x00613904
		public void DomeEntrance_TreeOnPillar(UnifiedRandom genRand, int pillarX, int pillarY)
		{
			if (!WorldGen.InWorld(pillarX, pillarY, 5))
			{
				return;
			}
			if (Main.tile[pillarX, pillarY - 1].active())
			{
				return;
			}
			ushort num = 0;
			int num2 = 2;
			if (WorldGen.SecretSeed.surfaceIsDesert.Enabled)
			{
				num = 53;
				num2 = -1;
			}
			int num3 = 5;
			int num4 = num3 / 2;
			for (int i = 0; i < num3; i++)
			{
				int num5 = pillarX + i - num4;
				for (int j = 0; j <= 3; j++)
				{
					int num6 = pillarY + j;
					Tile tile = Main.tile[num5, num6];
					if (tile.wall != this.settings.StyleData.BrickWallType)
					{
						tile.wall = 0;
					}
					if ((j != 1 || genRand.Next(2) != 0) && (j != 2 || genRand.Next(3) == 0) && (j != 3 || genRand.Next(4) == 0))
					{
						if (num2 > -1 && WorldGen.TileIsExposedToAir(num5, num6))
						{
							tile.type = (ushort)num2;
						}
						else
						{
							tile.type = num;
						}
					}
				}
			}
			if (num == 53)
			{
				WorldGen.TryGrowingTreeByType(323, pillarX, pillarY, 0, true);
				return;
			}
			WorldGen.TryGrowingTreeByType(5, pillarX, pillarY, 0, true);
		}
	}
}
