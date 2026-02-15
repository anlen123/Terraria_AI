using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Generation.Dungeon.Halls;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004DF RID: 1247
	public class DungeonGlobalGroundFurniture : GlobalDungeonFeature
	{
		// Token: 0x060034F7 RID: 13559 RVA: 0x0060A332 File Offset: 0x00608532
		public DungeonGlobalGroundFurniture(DungeonFeatureSettings settings) : base(settings)
		{
			DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
		}

		// Token: 0x060034F8 RID: 13560 RVA: 0x0060F629 File Offset: 0x0060D829
		public override bool GenerateFeature(DungeonData data)
		{
			this.generated = false;
			if (data.Type == DungeonType.DualDungeon)
			{
				this.GroundFurniture_DualDungeons(data);
			}
			else
			{
				this.GroundFurniture(data);
			}
			this.generated = true;
			return true;
		}

		// Token: 0x060034F9 RID: 13561 RVA: 0x0060F654 File Offset: 0x0060D854
		public void GroundFurniture_DualDungeons(DungeonData data)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			float num = (float)Main.maxTilesX / 4200f;
			int num2 = (int)((double)(1f + (float)((int)num)) * data.globalFeatureScalar);
			int num3 = (int)((double)(1f + (float)((int)num)) * data.globalFeatureScalar);
			bool flag = false;
			for (int i = 0; i < data.genVars.dungeonGenerationStyles.Count; i++)
			{
				if (data.genVars.dungeonGenerationStyles[i].Style == 0)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				num2 = 0;
				num3 = 0;
			}
			int num4 = -1;
			if (data.Type == DungeonType.DualDungeon)
			{
				switch (WorldGen.GetWorldSize())
				{
				default:
					num4 = 5;
					break;
				case 1:
					num4 = 10;
					break;
				case 2:
					num4 = 15;
					break;
				}
			}
			int defaultCount = 4;
			int defaultCount2 = 6;
			int num5 = 0;
			for (int j = 0; j < data.dungeonRooms.Count; j++)
			{
				DungeonRoom dungeonRoom = data.dungeonRooms[j];
				if (dungeonRoom.generated)
				{
					DungeonGenerationStyleData styleData = dungeonRoom.settings.StyleData;
					DungeonBounds innerBounds = dungeonRoom.InnerBounds;
					int k = dungeonRoom.GetFurnitureCount(defaultCount);
					bool flag2 = styleData.Style == 0 && (num2 > 0 || num3 > 0);
					int num6 = 50;
					while (k > 0)
					{
						num6--;
						if (num6 <= 0)
						{
							break;
						}
						Point point = innerBounds.RandomPointInBounds(genRand);
						Tile tile = Main.tile[point.X, point.Y];
						if (DungeonUtils.IsConsideredDungeonWall((int)tile.wall, false) && !tile.active())
						{
							point = DungeonUtils.FirstSolid(false, point, null);
							point.Y--;
							tile = Main.tile[point.X, point.Y];
							int num7 = num2;
							int num8 = num3;
							int num9 = 0;
							bool flag3;
							if (flag2)
							{
								flag3 = this.GroundFurniture_ActuallyGenerateFurniture(data, genRand, point.X, point.Y, tile.wall, ref num2, ref num3, ref num4, true, false);
								if (!flag3)
								{
									flag3 = this.GroundFurniture_ActuallyGenerateFurniture(data, genRand, point.X, point.Y, tile.wall, ref num9, ref num9, ref num4, false, num6 > 25);
								}
							}
							else
							{
								flag3 = this.GroundFurniture_ActuallyGenerateFurniture(data, genRand, point.X, point.Y, tile.wall, ref num9, ref num9, ref num4, false, num6 > 25);
								if (!flag3)
								{
									flag3 = this.GroundFurniture_ActuallyGenerateFurniture(data, genRand, point.X, point.Y, tile.wall, ref num9, ref num9, ref num4, false, num6 > 25);
								}
							}
							if ((flag2 && num7 != num2) || num8 != num3)
							{
								flag2 = false;
							}
							if (flag3)
							{
								k--;
								num5++;
							}
						}
					}
				}
			}
			for (int l = 0; l < data.dungeonHalls.Count; l++)
			{
				DungeonHall dungeonHall = data.dungeonHalls[l];
				if (dungeonHall.generated)
				{
					DungeonGenerationStyleData styleData2 = dungeonHall.settings.StyleData;
					DungeonBounds bounds = dungeonHall.Bounds;
					int m = dungeonHall.GetFurnitureCount(defaultCount2);
					bool flag4 = styleData2.Style == 0 && (num2 > 0 || num3 > 0);
					int num10 = 50;
					while (m > 0)
					{
						num10--;
						if (num10 <= 0)
						{
							break;
						}
						Point point2 = bounds.RandomPointInBounds(genRand);
						Tile tile2 = Main.tile[point2.X, point2.Y];
						if (DungeonUtils.IsConsideredDungeonWall((int)tile2.wall, false) && !tile2.active())
						{
							point2 = DungeonUtils.FirstSolid(false, point2, bounds);
							point2.Y--;
							tile2 = Main.tile[point2.X, point2.Y];
							int num11 = num2;
							int num12 = num3;
							int num13 = 0;
							bool flag5;
							if (flag4)
							{
								flag5 = this.GroundFurniture_ActuallyGenerateFurniture(data, genRand, point2.X, point2.Y, tile2.wall, ref num2, ref num3, ref num4, true, false);
								if (!flag5)
								{
									flag5 = this.GroundFurniture_ActuallyGenerateFurniture(data, genRand, point2.X, point2.Y, tile2.wall, ref num13, ref num13, ref num4, true, true);
								}
							}
							else
							{
								flag5 = this.GroundFurniture_ActuallyGenerateFurniture(data, genRand, point2.X, point2.Y, tile2.wall, ref num13, ref num13, ref num4, true, false);
								if (!flag5)
								{
									flag5 = this.GroundFurniture_ActuallyGenerateFurniture(data, genRand, point2.X, point2.Y, tile2.wall, ref num13, ref num13, ref num4, true, true);
								}
							}
							if ((flag4 && num11 != num2) || num12 != num3)
							{
								flag4 = false;
							}
							if (flag5)
							{
								m--;
								num5++;
							}
						}
					}
				}
			}
		}

		// Token: 0x060034FA RID: 13562 RVA: 0x0060FAF4 File Offset: 0x0060DCF4
		public void GroundFurniture(DungeonData data)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			ushort wallType = (ushort)data.wallVariants[0];
			float num = (float)Main.maxTilesX / 4200f;
			int num2 = (int)((double)(2000f * num) * data.globalFeatureScalar);
			int num3 = (int)((double)(1f + (float)((int)num)) * data.globalFeatureScalar);
			int num4 = (int)((double)(1f + (float)((int)num)) * data.globalFeatureScalar);
			bool flag = false;
			for (int i = 0; i < data.genVars.dungeonGenerationStyles.Count; i++)
			{
				if (data.genVars.dungeonGenerationStyles[i].Style == 0)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				num3 = 0;
				num4 = 0;
			}
			int num5 = -1;
			if (data.Type == DungeonType.DualDungeon)
			{
				switch (WorldGen.GetWorldSize())
				{
				default:
					num5 = 5;
					break;
				case 1:
					num5 = 10;
					break;
				case 2:
					num5 = 15;
					break;
				}
			}
			int num6 = 2000;
			for (int j = 0; j < num2; j++)
			{
				if (num3 > 0 || num4 > 0)
				{
					j--;
					num6--;
					if (num6 <= 0)
					{
						break;
					}
				}
				int num7 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
				int num8 = genRand.Next(Math.Max(data.dungeonBounds.Top, (int)Main.worldSurface + 10), data.dungeonBounds.Bottom);
				int num9 = 1000;
				while (!DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num7, num8].wall, false) || Main.tile[num7, num8].active())
				{
					num9--;
					if (num9 <= 0)
					{
						break;
					}
					num7 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
					num8 = genRand.Next(Math.Max(data.dungeonBounds.Top, (int)Main.worldSurface + 10), data.dungeonBounds.Bottom);
				}
				if (DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num7, num8].wall, false))
				{
					if (!Main.tile[num7, num8].active())
					{
						while (!WorldGen.SolidTile(num7, num8, false) && num8 < Main.UnderworldLayer)
						{
							num8++;
						}
						num8--;
						this.GroundFurniture_ActuallyGenerateFurniture(data, genRand, num7, num8, wallType, ref num3, ref num4, ref num5, j < num2 / 2, false);
					}
				}
			}
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x0060FD60 File Offset: 0x0060DF60
		private bool GroundFurniture_ActuallyGenerateFurniture(DungeonData data, UnifiedRandom genRand, int i, int j, ushort wallType, ref int alchTableCount, ref int bewitchTableCount, ref int minimumWaterCandles, bool stricterSpecialCheck = true, bool noRegularFurnitureAreaChecks = false)
		{
			int num = i;
			int num2 = i;
			while (!Main.tile[num, j].active() && WorldGen.SolidTile(num, j + 1, false))
			{
				num--;
			}
			num++;
			while (!Main.tile[num2, j].active() && WorldGen.SolidTile(num2, j + 1, false))
			{
				num2++;
			}
			num2--;
			int num3 = num2 - num;
			int num4 = (num2 + num) / 2;
			if (!data.CanGenerateFeatureAt(this, num4, j))
			{
				return false;
			}
			if (!Main.tile[num4, j].active() && DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num4, j].wall, false) && WorldGen.SolidTile(num4, j + 1, false) && Main.tile[num4, j + 1].type != 48)
			{
				int num5 = 1396;
				int num6 = 1397;
				int num7 = 1398;
				int num8 = 1405;
				int num9 = 1408;
				int num10 = 1414;
				int num11 = 1470;
				int num12 = 2376;
				int num13 = 2386;
				int num14 = 2402;
				int num15 = 2658;
				int num16 = 2664;
				int num17 = 2645;
				int num18 = 3900;
				if (wallType == 8)
				{
					num5 = 1399;
					num6 = 1400;
					num7 = 1401;
					num8 = 1406;
					num9 = 1409;
					num10 = 1415;
					num11 = 1471;
					num12 = 2377;
					num13 = 2387;
					num14 = 2403;
					num15 = 2659;
					num16 = 2665;
					num17 = 2646;
					num18 = 3901;
				}
				else if (wallType == 9)
				{
					num5 = 1402;
					num6 = 1403;
					num7 = 1404;
					num8 = 1407;
					num9 = 1410;
					num10 = 1416;
					num11 = 1472;
					num12 = 2378;
					num13 = 2388;
					num14 = 2404;
					num15 = 2660;
					num16 = 2666;
					num17 = 2647;
					num18 = 3902;
				}
				if (Main.tile[num4, j].wall >= 94 && Main.tile[num4, j].wall <= 105)
				{
					num5 = 1509;
					num6 = 1510;
					num7 = 1511;
					num8 = 5743;
					num9 = -1;
					num10 = 1512;
					num11 = 5740;
					num12 = 5750;
					num13 = 5741;
					num14 = 5753;
					num15 = 5739;
					num16 = 5742;
					num17 = 5748;
					num18 = 5746;
				}
				bool flag = true;
				bool flag2 = true;
				DungeonGenerationStyleData styleForWall = DungeonGenerationStyles.GetStyleForWall(data.genVars.dungeonGenerationStyles, (int)Main.tile[num4, j].wall);
				if (styleForWall != null)
				{
					flag = (styleForWall.Style == 0);
					flag2 = flag;
					num6 = this.GroundFurniture_GetFurnitureItem(styleForWall, genRand, num6, styleForWall.TableItemTypes);
					num7 = this.GroundFurniture_GetFurnitureItem(styleForWall, genRand, num7, styleForWall.WorkbenchItemTypes);
					num8 = this.GroundFurniture_GetFurnitureItem(styleForWall, genRand, num8, styleForWall.CandleItemTypes);
					num9 = this.GroundFurniture_GetFurnitureItem(styleForWall, genRand, num9, styleForWall.VaseOrStatueItemTypes);
					num10 = this.GroundFurniture_GetFurnitureItem(styleForWall, genRand, num10, styleForWall.BookcaseItemTypes);
					num5 = this.GroundFurniture_GetFurnitureItem(styleForWall, genRand, num5, styleForWall.ChairItemTypes);
					num11 = this.GroundFurniture_GetFurnitureItem(styleForWall, genRand, num11, styleForWall.BedItemTypes);
					num12 = this.GroundFurniture_GetFurnitureItem(styleForWall, genRand, num12, styleForWall.PianoItemTypes);
					num13 = this.GroundFurniture_GetFurnitureItem(styleForWall, genRand, num13, styleForWall.DresserItemTypes);
					num14 = this.GroundFurniture_GetFurnitureItem(styleForWall, genRand, num14, styleForWall.SofaItemTypes);
					num15 = this.GroundFurniture_GetFurnitureItem(styleForWall, genRand, num15, styleForWall.BathtubItemTypes);
					num17 = this.GroundFurniture_GetFurnitureItem(styleForWall, genRand, num17, styleForWall.LampItemTypes);
					num16 = this.GroundFurniture_GetFurnitureItem(styleForWall, genRand, num16, styleForWall.CandelabraItemTypes);
					num18 = this.GroundFurniture_GetFurnitureItem(styleForWall, genRand, num18, styleForWall.ClockItemTypes);
				}
				int num19 = genRand.Next(13);
				if ((num19 == 10 || num19 == 11 || num19 == 12) && genRand.Next(4) != 0)
				{
					num19 = genRand.Next(13);
				}
				while ((num19 == 2 && num9 == -1) || (num19 == 5 && num11 == -1) || (num19 == 6 && num12 == -1) || (num19 == 7 && num13 == -1) || (num19 == 8 && num14 == -1) || (num19 == 9 && num15 == -1) || (num19 == 10 && num16 == -1) || (num19 == 11 && num17 == -1) || (num19 == 12 && num18 == -1))
				{
					num19 = genRand.Next(13);
				}
				int num20 = 0;
				int num21 = 0;
				if (num19 == 0)
				{
					num20 = 5;
					num21 = 4;
				}
				if (num19 == 1)
				{
					num20 = 4;
					num21 = 3;
				}
				if (num19 == 2)
				{
					num20 = 3;
					num21 = 5;
				}
				if (num19 == 3)
				{
					num20 = 4;
					num21 = 6;
				}
				if (num19 == 4)
				{
					num20 = 3;
					num21 = 3;
				}
				if (num19 == 5)
				{
					num20 = 5;
					num21 = 3;
				}
				if (num19 == 6)
				{
					num20 = 5;
					num21 = 4;
				}
				if (num19 == 7)
				{
					num20 = 5;
					num21 = 4;
				}
				if (num19 == 8)
				{
					num20 = 5;
					num21 = 4;
				}
				if (num19 == 9)
				{
					num20 = 5;
					num21 = 3;
				}
				if (num19 == 10)
				{
					num20 = 2;
					num21 = 4;
				}
				if (num19 == 11)
				{
					num20 = 3;
					num21 = 3;
				}
				if (num19 == 12)
				{
					num20 = 2;
					num21 = 5;
				}
				if (noRegularFurnitureAreaChecks)
				{
					if (num19 == 0)
					{
						num20 = 3;
						num21 = 4;
					}
					if (num19 == 1)
					{
						num20 = 2;
						num21 = 3;
					}
					if (num19 == 2)
					{
						num20 = 3;
						num21 = 5;
					}
					if (num19 == 3)
					{
						num20 = 3;
						num21 = 6;
					}
					if (num19 == 4)
					{
						num20 = 1;
						num21 = 3;
					}
					if (num19 == 5)
					{
						num20 = 4;
						num21 = 3;
					}
					if (num19 == 6)
					{
						num20 = 4;
						num21 = 4;
					}
					if (num19 == 7)
					{
						num20 = 4;
						num21 = 4;
					}
					if (num19 == 8)
					{
						num20 = 4;
						num21 = 4;
					}
					if (num19 == 9)
					{
						num20 = 4;
						num21 = 3;
					}
					if (num19 == 10)
					{
						num20 = 1;
						num21 = 4;
					}
					if (num19 == 11)
					{
						num20 = 2;
						num21 = 3;
					}
					if (num19 == 12)
					{
						num20 = 2;
						num21 = 5;
					}
				}
				bool flag3 = false;
				bool flag4 = false;
				int num22 = 0;
				if (alchTableCount > 0 || bewitchTableCount > 0)
				{
					num22 = 15;
				}
				for (int k = num4 - num20 - num22; k <= num4 + num20 + num22; k++)
				{
					for (int l = j - num21 - num22; l <= j + num22; l++)
					{
						if (WorldGen.InWorld(k, l, 0))
						{
							Tile tile = Main.tile[k, l];
							if (k >= num4 - num20 && k <= num4 + num20 && l >= j - num21 && l <= j)
							{
								if (!data.CanGenerateFeatureAt(this, k, l))
								{
									flag3 = true;
									break;
								}
								if (!noRegularFurnitureAreaChecks && tile.active())
								{
									num19 = -1;
									break;
								}
							}
							if (stricterSpecialCheck && (alchTableCount > 0 || bewitchTableCount > 0) && tile.active() && (tile.type == 355 || tile.type == 354))
							{
								flag4 = true;
							}
						}
					}
				}
				if (flag3)
				{
					return false;
				}
				float num23 = (float)num20 * 1.75f;
				if (noRegularFurnitureAreaChecks)
				{
					num23 = (float)num20;
				}
				if ((float)num3 < num23)
				{
					num19 = -1;
				}
				if (!flag4 && flag2 && (alchTableCount > 0 || bewitchTableCount > 0))
				{
					if (alchTableCount > 0)
					{
						WorldGen.PlaceTile(num4, j, 355, true, false, -1, 0);
						if (Main.tile[num4, j].active() && Main.tile[num4, j].type == 355)
						{
							alchTableCount--;
							return true;
						}
					}
					else if (bewitchTableCount > 0)
					{
						WorldGen.PlaceTile(num4, j, 354, true, false, -1, 0);
						if (Main.tile[num4, j].active() && Main.tile[num4, j].type == 354)
						{
							bewitchTableCount--;
							return true;
						}
					}
				}
				else if (num6 > -1 && num19 == 0)
				{
					PlacementDetails placementDetails = ItemID.Sets.DerivedPlacementDetails[num6];
					WorldGen.PlaceTile(num4, j, placementDetails.tileType, true, false, -1, (int)placementDetails.tileStyle);
					if (Main.tile[num4, j].active() && (int)Main.tile[num4, j].type == placementDetails.tileType)
					{
						if (num5 > -1)
						{
							PlacementDetails placementDetails2 = ItemID.Sets.DerivedPlacementDetails[num5];
							if (!Main.tile[num4 - 2, j].active())
							{
								WorldGen.PlaceTile(num4 - 2, j, placementDetails2.tileType, true, false, -1, (int)placementDetails2.tileStyle);
								if (Main.tile[num4 - 2, j].active())
								{
									Tile tile2 = Main.tile[num4 - 2, j];
									tile2.frameX += 18;
									Tile tile3 = Main.tile[num4 - 2, j - 1];
									tile3.frameX += 18;
								}
							}
							if (!Main.tile[num4 + 2, j].active())
							{
								WorldGen.PlaceTile(num4 + 2, j, placementDetails2.tileType, true, false, -1, (int)placementDetails2.tileStyle);
							}
						}
						for (int m = num4 - 1; m <= num4 + 1; m++)
						{
							if (genRand.Next(2) == 0 && !Main.tile[m, j - 2].active())
							{
								if (flag)
								{
									int num24 = genRand.Next(5);
									if (minimumWaterCandles > 0)
									{
										num24 = 2;
									}
									if (num8 > -1 && num24 <= 1 && !Main.tileLighted[(int)Main.tile[m - 1, j - 2].type])
									{
										PlacementDetails placementDetails3 = ItemID.Sets.DerivedPlacementDetails[num8];
										WorldGen.PlaceTile(m, j - 2, placementDetails3.tileType, true, false, -1, (int)placementDetails3.tileStyle);
									}
									if (num24 == 2 && !Main.tileLighted[(int)Main.tile[m - 1, j - 2].type])
									{
										WorldGen.PlaceTile(m, j - 2, 49, true, false, -1, 0);
										if (Main.tile[m, j - 2].active() && Main.tile[m, j - 2].type == 49)
										{
											minimumWaterCandles--;
										}
									}
									else if (num24 == 3)
									{
										WorldGen.PlaceTile(m, j - 2, 50, true, false, -1, 0);
									}
									else if (num24 == 4)
									{
										WorldGen.PlaceTile(m, j - 2, 103, true, false, -1, 0);
									}
								}
								else
								{
									int num25 = genRand.Next(3);
									if (num8 > -1 && num25 <= 1 && !Main.tileLighted[(int)Main.tile[m - 1, j - 2].type])
									{
										PlacementDetails placementDetails4 = ItemID.Sets.DerivedPlacementDetails[num8];
										WorldGen.PlaceTile(m, j - 2, placementDetails4.tileType, true, false, -1, (int)placementDetails4.tileStyle);
									}
									else if (num25 == 2)
									{
										WorldGen.PlaceTile(m, j - 2, 103, true, false, -1, 0);
									}
								}
							}
						}
						return true;
					}
				}
				else if (num7 > -1 && num19 == 1)
				{
					PlacementDetails placementDetails5 = ItemID.Sets.DerivedPlacementDetails[num7];
					PlacementDetails placementDetails6 = ItemID.Sets.DerivedPlacementDetails[num5];
					WorldGen.PlaceTile(num4, j, placementDetails5.tileType, true, false, -1, (int)placementDetails5.tileStyle);
					if (Main.tile[num4, j].active() && (int)Main.tile[num4, j].type == placementDetails5.tileType)
					{
						if (num5 > -1)
						{
							if (genRand.Next(2) == 0)
							{
								if (!Main.tile[num4 - 1, j].active())
								{
									WorldGen.PlaceTile(num4 - 1, j, placementDetails6.tileType, true, false, -1, (int)placementDetails6.tileStyle);
									if (Main.tile[num4 - 1, j].active())
									{
										Tile tile4 = Main.tile[num4 - 1, j];
										tile4.frameX += 18;
										Tile tile5 = Main.tile[num4 - 1, j - 1];
										tile5.frameX += 18;
									}
								}
							}
							else if (!Main.tile[num4 + 2, j].active())
							{
								WorldGen.PlaceTile(num4 + 2, j, placementDetails6.tileType, true, false, -1, (int)placementDetails6.tileStyle);
							}
						}
						for (int n = num4; n <= num4 + 1; n++)
						{
							if (genRand.Next(2) == 0 && !Main.tile[n, j - 1].active())
							{
								if (flag)
								{
									int num26 = genRand.Next(5);
									if (minimumWaterCandles > 0)
									{
										num26 = 2;
									}
									if (num8 != -1 && num26 <= 1 && !Main.tileLighted[(int)Main.tile[n - 1, j - 1].type])
									{
										PlacementDetails placementDetails7 = ItemID.Sets.DerivedPlacementDetails[num8];
										WorldGen.PlaceTile(n, j - 1, placementDetails7.tileType, true, false, -1, (int)placementDetails7.tileStyle);
									}
									else if (num26 == 2 && !Main.tileLighted[(int)Main.tile[n - 1, j - 1].type])
									{
										WorldGen.PlaceTile(n, j - 1, 49, true, false, -1, 0);
										if (Main.tile[n, j - 1].active() && Main.tile[n, j - 1].type == 49)
										{
											minimumWaterCandles--;
										}
									}
									else if (num26 == 3)
									{
										WorldGen.PlaceTile(n, j - 1, 50, true, false, -1, 0);
									}
									else if (num26 == 4)
									{
										WorldGen.PlaceTile(n, j - 1, 103, true, false, -1, 0);
									}
								}
								else
								{
									int num27 = genRand.Next(3);
									if (num8 != -1 && num27 <= 1 && !Main.tileLighted[(int)Main.tile[n - 1, j - 1].type])
									{
										PlacementDetails placementDetails8 = ItemID.Sets.DerivedPlacementDetails[num8];
										WorldGen.PlaceTile(n, j - 1, placementDetails8.tileType, true, false, -1, (int)placementDetails8.tileStyle);
									}
									else if (num27 == 2)
									{
										WorldGen.PlaceTile(n, j - 1, 103, true, false, -1, 0);
									}
								}
							}
						}
						return true;
					}
				}
				else if (num9 > -1 && num19 == 2)
				{
					PlacementDetails placementDetails9 = ItemID.Sets.DerivedPlacementDetails[num9];
					WorldGen.PlaceTile(num4, j, placementDetails9.tileType, true, false, -1, (int)placementDetails9.tileStyle);
					if (Main.tile[num4, j].active() && (int)Main.tile[num4, j].type == placementDetails9.tileType)
					{
						return true;
					}
				}
				else if (num10 > -1 && num19 == 3)
				{
					PlacementDetails placementDetails10 = ItemID.Sets.DerivedPlacementDetails[num10];
					WorldGen.PlaceTile(num4, j, placementDetails10.tileType, true, false, -1, (int)placementDetails10.tileStyle);
				}
				else if (num5 > -1 && num19 == 4)
				{
					PlacementDetails placementDetails11 = ItemID.Sets.DerivedPlacementDetails[num5];
					if (genRand.Next(2) == 0)
					{
						WorldGen.PlaceTile(num4, j, placementDetails11.tileType, true, false, -1, (int)placementDetails11.tileStyle);
						Tile tile6 = Main.tile[num4, j];
						tile6.frameX += 18;
						Tile tile7 = Main.tile[num4, j - 1];
						tile7.frameX += 18;
					}
					else
					{
						WorldGen.PlaceTile(num4, j, placementDetails11.tileType, true, false, -1, (int)placementDetails11.tileStyle);
					}
					if (Main.tile[num4, j].active() && (int)Main.tile[num4, j].type == placementDetails11.tileType)
					{
						return true;
					}
				}
				else if (num11 > -1 && num19 == 5)
				{
					PlacementDetails placementDetails12 = ItemID.Sets.DerivedPlacementDetails[num11];
					if (placementDetails12.tileType >= 0)
					{
						if (genRand.Next(2) == 0)
						{
							WorldGen.Place4x2(num4, j, (ushort)placementDetails12.tileType, 1, (int)placementDetails12.tileStyle);
						}
						else
						{
							WorldGen.Place4x2(num4, j, (ushort)placementDetails12.tileType, -1, (int)placementDetails12.tileStyle);
						}
						if (Main.tile[num4, j].active() && (int)Main.tile[num4, j].type == placementDetails12.tileType)
						{
							return true;
						}
					}
				}
				else if (num12 > -1 && num19 == 6)
				{
					PlacementDetails placementDetails13 = ItemID.Sets.DerivedPlacementDetails[num12];
					WorldGen.PlaceTile(num4, j, placementDetails13.tileType, true, false, -1, (int)placementDetails13.tileStyle);
					if (Main.tile[num4, j].active() && (int)Main.tile[num4, j].type == placementDetails13.tileType)
					{
						return true;
					}
				}
				else if (num13 > -1 && num19 == 7)
				{
					PlacementDetails placementDetails14 = ItemID.Sets.DerivedPlacementDetails[num13];
					WorldGen.PlaceTile(num4, j, placementDetails14.tileType, true, false, -1, (int)placementDetails14.tileStyle);
					if (Main.tile[num4, j].active() && (int)Main.tile[num4, j].type == placementDetails14.tileType)
					{
						return true;
					}
				}
				else if (num14 > -1 && num19 == 8)
				{
					PlacementDetails placementDetails15 = ItemID.Sets.DerivedPlacementDetails[num14];
					WorldGen.PlaceTile(num4, j, placementDetails15.tileType, true, false, -1, (int)placementDetails15.tileStyle);
					if (Main.tile[num4, j].active() && (int)Main.tile[num4, j].type == placementDetails15.tileType)
					{
						return true;
					}
				}
				else if (num15 > -1 && num19 == 9)
				{
					PlacementDetails placementDetails16 = ItemID.Sets.DerivedPlacementDetails[num15];
					if (placementDetails16.tileType >= 0)
					{
						if (genRand.Next(2) == 0)
						{
							WorldGen.Place4x2(num4, j, (ushort)placementDetails16.tileType, 1, (int)placementDetails16.tileStyle);
						}
						else
						{
							WorldGen.Place4x2(num4, j, (ushort)placementDetails16.tileType, -1, (int)placementDetails16.tileStyle);
						}
						if (Main.tile[num4, j].active() && (int)Main.tile[num4, j].type == placementDetails16.tileType)
						{
							return true;
						}
					}
				}
				else if (num17 > -1 && num19 == 10)
				{
					PlacementDetails placementDetails17 = ItemID.Sets.DerivedPlacementDetails[num17];
					WorldGen.PlaceTile(num4, j, placementDetails17.tileType, true, false, -1, (int)placementDetails17.tileStyle);
					if (Main.tile[num4, j].active() && (int)Main.tile[num4, j].type == placementDetails17.tileType)
					{
						return true;
					}
				}
				else if (num16 > -1 && num19 == 11)
				{
					PlacementDetails placementDetails18 = ItemID.Sets.DerivedPlacementDetails[num16];
					WorldGen.PlaceTile(num4, j, placementDetails18.tileType, true, false, -1, (int)placementDetails18.tileStyle);
					if (Main.tile[num4, j].active() && (int)Main.tile[num4, j].type == placementDetails18.tileType)
					{
						return true;
					}
				}
				else if (num18 > -1 && num19 == 12)
				{
					PlacementDetails placementDetails19 = ItemID.Sets.DerivedPlacementDetails[num18];
					WorldGen.PlaceTile(num4, j, placementDetails19.tileType, true, false, -1, (int)placementDetails19.tileStyle);
					if (Main.tile[num4, j].active() && (int)Main.tile[num4, j].type == placementDetails19.tileType)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x0061104F File Offset: 0x0060F24F
		private int GroundFurniture_GetFurnitureItem(DungeonGenerationStyleData styleData, UnifiedRandom genRand, int defaultItem, int[] items)
		{
			if (items == null)
			{
				return -1;
			}
			if (items.Length == 0 || styleData.Style == 0)
			{
				return defaultItem;
			}
			return items[genRand.Next(items.Length)];
		}
	}
}
