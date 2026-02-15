using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004DB RID: 1243
	public class DungeonGlobalEarlyDualDungeonFeatures : GlobalDungeonFeature
	{
		// Token: 0x060034E5 RID: 13541 RVA: 0x0060A332 File Offset: 0x00608532
		public DungeonGlobalEarlyDualDungeonFeatures(DungeonFeatureSettings settings) : base(settings)
		{
			DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x0060C466 File Offset: 0x0060A666
		public override bool GenerateFeature(DungeonData data)
		{
			this.generated = false;
			this.EarlyDungeonFeatures(data);
			this.generated = true;
			return true;
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x0060C480 File Offset: 0x0060A680
		public void EarlyDungeonFeatures(DungeonData data)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			int num = 20;
			int num2 = 8;
			int num3 = 8;
			int num4 = 6;
			int num5 = 4;
			int num6 = 4;
			int num7 = 40;
			int num8 = 40;
			switch (WorldGen.GetWorldSize())
			{
			case 0:
				num = 20;
				num2 = 8;
				num3 = 8;
				num4 = 6;
				num5 = 4;
				num6 = 4;
				num7 = 40;
				num8 = 40;
				break;
			case 1:
				num = 30;
				num2 = 14;
				num3 = 12;
				num4 = 10;
				num5 = 6;
				num6 = 8;
				num7 = 60;
				num8 = 60;
				break;
			case 2:
				num = 40;
				num2 = 18;
				num3 = 16;
				num4 = 14;
				num5 = 8;
				num6 = 12;
				num7 = 80;
				num8 = 80;
				break;
			}
			if (WorldGen.SecretSeed.Variations.actuallyNoTrapsForRealIMeanIt)
			{
				num3 = 0;
				num4 = 0;
				num5 = 0;
			}
			for (int i = 0; i < data.genVars.dungeonGenerationStyles.Count; i++)
			{
				DungeonGenerationStyleData dungeonGenerationStyleData = data.genVars.dungeonGenerationStyles[i];
				byte style = dungeonGenerationStyleData.Style;
				DungeonBounds dungeonBounds = data.outerProgressionBounds[i];
				if (style == 4 || style == 5)
				{
					bool flag = style == 5;
					int j = num;
					int num9 = 1000;
					while (j > 0)
					{
						num9--;
						if (num9 <= 0)
						{
							break;
						}
						int num10 = dungeonBounds.Left + genRand.Next(dungeonBounds.Width);
						int num11 = dungeonBounds.Top + genRand.Next(dungeonBounds.Height);
						Tile tile = Main.tile[num10, num11];
						Tile tile2 = Main.tile[num10, num11 + 1];
						while (!tile.active() && num11 < Main.maxTilesY - 10)
						{
							num11++;
							tile = Main.tile[num10, num11];
						}
						num11--;
						tile = Main.tile[num10, num11];
						tile2 = Main.tile[num10, num11 + 1];
						if (!tile.active() && tile.wall == dungeonGenerationStyleData.BrickWallType)
						{
							DungeonGenerationStyleData styleForTile = DungeonGenerationStyles.GetStyleForTile(data.genVars.dungeonGenerationStyles, (int)tile2.type);
							if (styleForTile != null && styleForTile.Style == (flag ? 5 : 4) && tile2.type != styleForTile.BrickCrackedTileType && tile2.type != styleForTile.PitTrapTileType)
							{
								WorldGen.Place3x2(num10, num11, 26, flag ? 1 : 0);
								tile = Main.tile[num10, num11];
								if (tile.active() && tile.type == 26)
								{
									j--;
								}
							}
						}
					}
				}
			}
			Dictionary<int, List<DungeonRoom>> dictionary = new Dictionary<int, List<DungeonRoom>>();
			BiomeDungeonRoom biomeDungeonRoom = null;
			for (int k = 0; k < data.dungeonRooms.Count; k++)
			{
				DungeonRoom dungeonRoom = data.dungeonRooms[k];
				byte style2 = dungeonRoom.settings.StyleData.Style;
				if (!dictionary.ContainsKey((int)style2))
				{
					dictionary.Add((int)style2, new List<DungeonRoom>());
				}
				dictionary[(int)style2].Add(dungeonRoom);
				if (dungeonRoom is BiomeDungeonRoom && dungeonRoom.settings.StyleData.Style == 10)
				{
					biomeDungeonRoom = (BiomeDungeonRoom)dungeonRoom;
				}
			}
			if (dictionary.ContainsKey(4))
			{
				int num12 = num2;
				List<DungeonRoom> list = dictionary[4].ToList<DungeonRoom>();
				while (list.Count > 0 && num12 > 0)
				{
					DungeonRoom dungeonRoom2 = list[genRand.Next(list.Count)];
					Point center = dungeonRoom2.InnerBounds.Center;
					Main.tile[center.X, center.Y];
					WorldGen.AddShadowOrb(center.X, center.Y, false);
					if (Main.tile[center.X, center.Y].type == 31)
					{
						num12--;
					}
					list.Remove(dungeonRoom2);
				}
			}
			if (dictionary.ContainsKey(5))
			{
				int num13 = num2;
				List<DungeonRoom> list2 = dictionary[5].ToList<DungeonRoom>();
				while (list2.Count > 0 && num13 > 0)
				{
					DungeonRoom dungeonRoom3 = list2[genRand.Next(list2.Count)];
					Point center2 = dungeonRoom3.InnerBounds.Center;
					Main.tile[center2.X, center2.Y];
					WorldGen.AddShadowOrb(center2.X, center2.Y, true);
					if (Main.tile[center2.X, center2.Y].type == 31)
					{
						num13--;
					}
					list2.Remove(dungeonRoom3);
				}
			}
			if (dictionary.ContainsKey(9))
			{
				List<DungeonRoom> list3 = dictionary[9].ToList<DungeonRoom>();
				while (list3.Count > 0)
				{
					DungeonRoom dungeonRoom4 = list3[0];
					Point center3 = dungeonRoom4.InnerBounds.Center;
					WorldGen.AddBeeLarva(center3.X - 1, center3.Y - 3);
					list3.Remove(dungeonRoom4);
				}
			}
			if (data.Type == DungeonType.DualDungeon)
			{
				for (int l = 0; l < data.genVars.dungeonGenerationStyles.Count; l++)
				{
					DungeonGenerationStyleData dungeonGenerationStyleData2 = data.genVars.dungeonGenerationStyles[l];
					List<DungeonRoom> list4 = dictionary[(int)dungeonGenerationStyleData2.Style];
					int num14 = num6;
					int num15;
					switch (WorldGen.GetWorldSize())
					{
					default:
						num15 = 2;
						break;
					case 1:
						num15 = 4;
						break;
					case 2:
						num15 = 6;
						break;
					}
					if (list4 != null)
					{
						while (list4.Count > 0 && num14 > 0)
						{
							DungeonRoom dungeonRoom5 = list4[genRand.Next(list4.Count)];
							if (dungeonRoom5 is BiomeDungeonRoom)
							{
								list4.Remove(dungeonRoom5);
							}
							else
							{
								int x = dungeonRoom5.InnerBounds.Center.X;
								int y = dungeonRoom5.InnerBounds.Bottom - 5;
								int width = dungeonRoom5.InnerBounds.Width / 2;
								int height = (int)((float)dungeonRoom5.InnerBounds.Height * 0.75f);
								bool flag2 = num15 > 0 || genRand.Next(8) == 0;
								DungeonGenerationStyleData styleData = dungeonRoom5.settings.StyleData;
								DungeonPitTrap dungeonPitTrap = new DungeonPitTrap(new DungeonPitTrapSettings
								{
									Style = styleData,
									Width = width,
									Height = height,
									EdgeWidth = 2,
									EdgeHeight = 2,
									TopDensity = 8,
									ConnectedRoom = dungeonRoom5,
									Flooded = flag2
								}, false);
								if (!dungeonRoom5.settings.StyleData.CanGenerateFeatureAt(data, dungeonRoom5, dungeonPitTrap, x, y))
								{
									list4.Remove(dungeonRoom5);
								}
								else
								{
									if (dungeonPitTrap.GenerateFeature(data, x, y))
									{
										DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(dungeonPitTrap);
										if (flag2 && num15 > 0)
										{
											num15--;
										}
										num14--;
									}
									else
									{
										height = Math.Max(10, (int)((float)dungeonRoom5.InnerBounds.Height * 0.5f));
										dungeonPitTrap = new DungeonPitTrap(new DungeonPitTrapSettings
										{
											Style = styleData,
											Width = width,
											Height = height,
											EdgeWidth = 2,
											EdgeHeight = 2,
											TopDensity = 8,
											ConnectedRoom = dungeonRoom5,
											Flooded = flag2
										}, false);
										if (!dungeonRoom5.settings.StyleData.CanGenerateFeatureAt(data, dungeonRoom5, dungeonPitTrap, x, y))
										{
											list4.Remove(dungeonRoom5);
											continue;
										}
										if (dungeonPitTrap.GenerateFeature(data, x, y))
										{
											DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(dungeonPitTrap);
											if (flag2 && num15 > 0)
											{
												num15--;
											}
											num14--;
										}
										else
										{
											width = (int)((float)(dungeonRoom5.InnerBounds.Width / 2) * 0.75f);
											dungeonPitTrap = new DungeonPitTrap(new DungeonPitTrapSettings
											{
												Style = styleData,
												Width = width,
												Height = height,
												EdgeWidth = 2,
												EdgeHeight = 2,
												TopDensity = 8,
												ConnectedRoom = dungeonRoom5,
												Flooded = flag2
											}, false);
											if (!dungeonRoom5.settings.StyleData.CanGenerateFeatureAt(data, dungeonRoom5, dungeonPitTrap, x, y))
											{
												list4.Remove(dungeonRoom5);
												continue;
											}
											if (dungeonPitTrap.GenerateFeature(data, x, y))
											{
												DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(dungeonPitTrap);
												if (flag2 && num15 > 0)
												{
													num15--;
												}
												num14--;
											}
										}
									}
									list4.Remove(dungeonRoom5);
								}
							}
						}
					}
				}
			}
			if (dictionary.ContainsKey(3))
			{
				List<DungeonRoom> list5 = dictionary[3].ToList<DungeonRoom>();
				while (list5.Count > 0 && num3 > 0)
				{
					DungeonRoom dungeonRoom6 = list5[genRand.Next(list5.Count)];
					int num16 = 20;
					while (num16 > 0 && num3 > 0)
					{
						num16--;
						int num17 = dungeonRoom6.InnerBounds.Left + genRand.Next(dungeonRoom6.InnerBounds.Width);
						int num18 = dungeonRoom6.InnerBounds.Top + genRand.Next(dungeonRoom6.InnerBounds.Height);
						if (WorldGen.InWorld(num17, num18, 25))
						{
							Tile tile3 = Main.tile[num17, num18];
							while (num18 < Main.UnderworldLayer - 10 && !tile3.active())
							{
								num18++;
								tile3 = Main.tile[num17, num18];
							}
							if (tile3.active() && tile3.type == DungeonGenerationStyles.Desert.BrickTileType)
							{
								DungeonDropTrap dungeonDropTrap = new DungeonDropTrap(new DungeonDropTrapSettings
								{
									StyleData = DungeonGenerationStyles.Desert,
									DropTrapType = ((genRand.Next(2) == 0) ? DungeonDropTrapType.Sand : DungeonDropTrapType.Lava)
								}, false);
								if (dungeonDropTrap.GenerateFeature(data, num17, num18))
								{
									DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(dungeonDropTrap);
									num3--;
								}
							}
						}
					}
					list5.Remove(dungeonRoom6);
				}
			}
			if (dictionary.ContainsKey(2))
			{
				List<DungeonRoom> list6 = dictionary[2].ToList<DungeonRoom>();
				while (list6.Count > 0 && num4 > 0)
				{
					DungeonRoom dungeonRoom7 = list6[genRand.Next(list6.Count)];
					int num19 = 20;
					while (num19 > 0 && num4 > 0)
					{
						num19--;
						int num20 = dungeonRoom7.InnerBounds.Left + genRand.Next(dungeonRoom7.InnerBounds.Width);
						int num21 = dungeonRoom7.InnerBounds.Top + genRand.Next(dungeonRoom7.InnerBounds.Height);
						if (WorldGen.InWorld(num20, num21, 25))
						{
							Tile tile4 = Main.tile[num20, num21];
							while (num21 < Main.UnderworldLayer - 10 && !tile4.active())
							{
								num21++;
								tile4 = Main.tile[num20, num21];
							}
							if (tile4.active() && tile4.type == DungeonGenerationStyles.Snow.BrickTileType)
							{
								DungeonDropTrap dungeonDropTrap2 = new DungeonDropTrap(new DungeonDropTrapSettings
								{
									StyleData = DungeonGenerationStyles.Snow,
									DropTrapType = DungeonDropTrapType.Slush
								}, false);
								if (dungeonDropTrap2.GenerateFeature(data, num20, num21))
								{
									DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(dungeonDropTrap2);
									num4--;
								}
							}
						}
					}
					list6.Remove(dungeonRoom7);
				}
			}
			if (dictionary.ContainsKey(1))
			{
				List<DungeonRoom> list7 = dictionary[1].ToList<DungeonRoom>();
				while (list7.Count > 0 && num5 > 0)
				{
					DungeonRoom dungeonRoom8 = list7[genRand.Next(list7.Count)];
					int num22 = 20;
					while (num22 > 0 && num5 > 0)
					{
						num22--;
						int num23 = dungeonRoom8.InnerBounds.Left + genRand.Next(dungeonRoom8.InnerBounds.Width);
						int num24 = dungeonRoom8.InnerBounds.Top + genRand.Next(dungeonRoom8.InnerBounds.Height);
						if (WorldGen.InWorld(num23, num24, 25))
						{
							Tile tile5 = Main.tile[num23, num24];
							while (num24 < Main.UnderworldLayer - 10 && !tile5.active())
							{
								num24++;
								tile5 = Main.tile[num23, num24];
							}
							if (tile5.active() && tile5.type == DungeonGenerationStyles.Cavern.BrickTileType)
							{
								DungeonDropTrap dungeonDropTrap3 = new DungeonDropTrap(new DungeonDropTrapSettings
								{
									StyleData = DungeonGenerationStyles.Cavern,
									DropTrapType = DungeonDropTrapType.Silt
								}, false);
								if (dungeonDropTrap3.GenerateFeature(data, num23, num24))
								{
									DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(dungeonDropTrap3);
									num5--;
								}
							}
						}
					}
					list7.Remove(dungeonRoom8);
				}
			}
			for (int m = 0; m < data.genVars.dungeonGenerationStyles.Count; m++)
			{
				DungeonGenerationStyleData dungeonGenerationStyleData3 = data.genVars.dungeonGenerationStyles[m];
				byte style3 = dungeonGenerationStyleData3.Style;
				DungeonBounds dungeonBounds2 = data.outerProgressionBounds[m];
				if (style3 == 3)
				{
					int num25 = 1000;
					int n = num7;
					while (n > 0)
					{
						num25--;
						if (num25 <= 0)
						{
							break;
						}
						int num26 = dungeonBounds2.Left + genRand.Next(dungeonBounds2.Width);
						int num27 = dungeonBounds2.Top + genRand.Next(dungeonBounds2.Height);
						Tile tile6 = Main.tile[num26, num27];
						if (tile6.wall == dungeonGenerationStyleData3.BrickWallType)
						{
							DungeonGenerationStyleData styleForTile2 = DungeonGenerationStyles.GetStyleForTile(data.genVars.dungeonGenerationStyles, (int)tile6.type);
							if (styleForTile2 != null && styleForTile2.Style == 3)
							{
								new DungeonTileClump(new DungeonTileClumpSettings
								{
									RandomSeed = genRand.Next(),
									Strength = (double)(25 + genRand.Next(10)),
									Steps = 25 + genRand.Next(10),
									TileType = 53,
									WallType = 216,
									AreaToGenerateIn = null,
									OnlyReplaceThisTileType = new ushort?(styleForTile2.BrickTileType),
									OnlyReplaceThisWallType = new ushort?(styleForTile2.BrickWallType)
								}, false).GenerateFeature(data, num26, num27);
								n--;
							}
						}
					}
					num25 = 1000;
					n = num7;
					while (n > 0)
					{
						num25--;
						if (num25 <= 0)
						{
							break;
						}
						int num28 = dungeonBounds2.Left + genRand.Next(dungeonBounds2.Width);
						int num29 = dungeonBounds2.Top + genRand.Next(dungeonBounds2.Height);
						Tile tile7 = Main.tile[num28, num29];
						if (tile7.wall == dungeonGenerationStyleData3.BrickWallType)
						{
							DungeonGenerationStyleData styleForTile3 = DungeonGenerationStyles.GetStyleForTile(data.genVars.dungeonGenerationStyles, (int)tile7.type);
							if (styleForTile3 != null && styleForTile3.Style == 3)
							{
								new DungeonTileClump(new DungeonTileClumpSettings
								{
									RandomSeed = genRand.Next(),
									Strength = (double)(25 + genRand.Next(10)),
									Steps = 25 + genRand.Next(10),
									TileType = 397,
									WallType = 216,
									AreaToGenerateIn = null,
									OnlyReplaceThisTileType = new ushort?(styleForTile3.BrickTileType),
									OnlyReplaceThisWallType = new ushort?(styleForTile3.BrickWallType)
								}, false).GenerateFeature(data, num28, num29);
								n--;
							}
						}
					}
					num25 = 1000;
					n = num7 * 2;
					while (n > 0)
					{
						num25--;
						if (num25 <= 0)
						{
							break;
						}
						int num30 = dungeonBounds2.Left + genRand.Next(dungeonBounds2.Width);
						int num31 = dungeonBounds2.Top + genRand.Next(dungeonBounds2.Height);
						Tile tile8 = Main.tile[num30, num31];
						if (tile8.wall == dungeonGenerationStyleData3.BrickWallType)
						{
							DungeonGenerationStyleData styleForTile4 = DungeonGenerationStyles.GetStyleForTile(data.genVars.dungeonGenerationStyles, (int)tile8.type);
							if (styleForTile4 != null && styleForTile4.Style == 3)
							{
								new DungeonTileClump(new DungeonTileClumpSettings
								{
									RandomSeed = genRand.Next(),
									Strength = (double)(15 + genRand.Next(5)),
									Steps = 15 + genRand.Next(5),
									TileType = 404,
									WallType = 223,
									AreaToGenerateIn = null,
									OnlyReplaceThisTileType = new ushort?(styleForTile4.BrickTileType),
									OnlyReplaceThisWallType = new ushort?(styleForTile4.BrickWallType)
								}, false).GenerateFeature(data, num30, num31);
								n--;
							}
						}
					}
				}
				if (style3 == 2)
				{
					int num32 = 1000;
					int num33 = num8;
					while (num33 > 0)
					{
						num32--;
						if (num32 <= 0)
						{
							break;
						}
						int num34 = dungeonBounds2.Left + genRand.Next(dungeonBounds2.Width);
						int num35 = dungeonBounds2.Top + genRand.Next(dungeonBounds2.Height);
						Tile tile9 = Main.tile[num34, num35];
						if (tile9.wall == dungeonGenerationStyleData3.BrickWallType)
						{
							DungeonGenerationStyleData styleForTile5 = DungeonGenerationStyles.GetStyleForTile(data.genVars.dungeonGenerationStyles, (int)tile9.type);
							if (styleForTile5 != null && styleForTile5.Style == 2)
							{
								new DungeonTileClump(new DungeonTileClumpSettings
								{
									RandomSeed = genRand.Next(),
									Strength = (double)(25 + genRand.Next(10)),
									Steps = 25 + genRand.Next(10),
									TileType = 147,
									WallType = 40,
									AreaToGenerateIn = null,
									OnlyReplaceThisTileType = new ushort?(styleForTile5.BrickTileType),
									OnlyReplaceThisWallType = new ushort?(styleForTile5.BrickWallType)
								}, false).GenerateFeature(data, num34, num35);
								num33--;
							}
						}
					}
					num32 = 1000;
					num33 = num8;
					while (num33 > 0)
					{
						num32--;
						if (num32 <= 0)
						{
							break;
						}
						int num36 = dungeonBounds2.Left + genRand.Next(dungeonBounds2.Width);
						int num37 = dungeonBounds2.Top + genRand.Next(dungeonBounds2.Height);
						Tile tile10 = Main.tile[num36, num37];
						if (tile10.wall == dungeonGenerationStyleData3.BrickWallType)
						{
							DungeonGenerationStyleData styleForTile6 = DungeonGenerationStyles.GetStyleForTile(data.genVars.dungeonGenerationStyles, (int)tile10.type);
							if (styleForTile6 != null && styleForTile6.Style == 2)
							{
								new DungeonTileClump(new DungeonTileClumpSettings
								{
									RandomSeed = genRand.Next(),
									Strength = (double)(25 + genRand.Next(10)),
									Steps = 25 + genRand.Next(10),
									TileType = 224,
									WallType = 40,
									AreaToGenerateIn = null,
									OnlyReplaceThisTileType = new ushort?(styleForTile6.BrickTileType),
									OnlyReplaceThisWallType = new ushort?(styleForTile6.BrickWallType)
								}, false).GenerateFeature(data, num36, num37);
								num33--;
							}
						}
					}
				}
			}
			for (int num38 = 0; num38 < data.dungeonRooms.Count; num38++)
			{
				data.dungeonRooms[num38].GenerateEarlyDungeonFeaturesInRoom(data);
			}
			if (biomeDungeonRoom != null)
			{
				int x2 = biomeDungeonRoom.InnerBounds.Center.X;
				int num39 = (biomeDungeonRoom.InnerBounds.Top + biomeDungeonRoom.InnerBounds.Center.Y) / 2;
				Tile tile11 = Main.tile[x2, num39];
				while (!tile11.active())
				{
					num39++;
					tile11 = Main.tile[x2, num39];
				}
				for (int num40 = -1; num40 <= 1; num40++)
				{
					int num41 = x2 + num40;
					int num42 = num39;
					Tile tile12 = Main.tile[num41, num42];
					while (!tile12.active())
					{
						tile12.ClearTile();
						tile12.active(true);
						tile12.type = 226;
						num42++;
						tile12 = Main.tile[num41, num42];
					}
				}
				WorldGen.AddLihzahrdAltar(x2 - 1, num39 - 2);
			}
			if (data.Type == DungeonType.Default)
			{
				num6 = (int)((double)Main.maxTilesX * 2.0 * data.dungeonStepScalar);
				for (int num43 = 0; num43 < num6; num43++)
				{
					int x3 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
					int num44 = data.dungeonBounds.Top;
					if (num44 < Main.dungeonY + 25)
					{
						num44 = Main.dungeonY + 25;
					}
					if ((double)num44 < Main.worldSurface)
					{
						num44 = (int)Main.worldSurface;
					}
					int y2 = genRand.Next(num44, data.dungeonBounds.Bottom);
					bool flag3 = data.makeNextPitTrapFlooded || genRand.Next(8) == 0;
					int num45 = genRand.Next(6, 10);
					if (new DungeonPitTrap(new DungeonPitTrapSettings
					{
						Style = data.genVars.dungeonStyle,
						Width = genRand.Next(8, 19),
						Height = genRand.Next(19, 46),
						EdgeWidth = genRand.Next(6, 10),
						EdgeHeight = num45,
						TopDensity = num45,
						Flooded = flag3
					}, true).GenerateFeature(data, x3, y2))
					{
						if (flag3)
						{
							data.makeNextPitTrapFlooded = false;
						}
						num43 += 1500;
					}
					else
					{
						num43++;
					}
				}
			}
		}
	}
}
