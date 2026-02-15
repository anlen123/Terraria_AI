using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.DataStructures;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Entrances
{
	// Token: 0x020004F4 RID: 1268
	public class LegacyDungeonEntrance : DungeonEntrance
	{
		// Token: 0x06003536 RID: 13622 RVA: 0x0061348C File Offset: 0x0061168C
		public LegacyDungeonEntrance(DungeonEntranceSettings settings) : base(settings)
		{
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x00615824 File Offset: 0x00613A24
		public override void CalculateEntrance(DungeonData data, int x, int y)
		{
			this.calculated = false;
			this.LegacyEntrance(data, x, y, false);
			this.calculated = true;
		}

		// Token: 0x06003538 RID: 13624 RVA: 0x0061583E File Offset: 0x00613A3E
		public override bool GenerateEntrance(DungeonData data, int x, int y)
		{
			this.generated = false;
			this.LegacyEntrance(data, x, y, true);
			this.generated = true;
			return true;
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x0061585C File Offset: 0x00613A5C
		public void LegacyEntrance(DungeonData data, int i, int j, bool generating)
		{
			UnifiedRandom unifiedRandom = new UnifiedRandom(((LegacyDungeonEntranceSettings)this.settings).RandomSeed);
			ushort brickTileType = this.settings.StyleData.BrickTileType;
			ushort brickWallType = this.settings.StyleData.BrickWallType;
			bool dungeonEntranceIsBuried = SpecialSeedFeatures.DungeonEntranceIsBuried;
			bool dungeonEntranceIsUnderground = SpecialSeedFeatures.DungeonEntranceIsUnderground;
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
			Vector2D zero = Vector2D.Zero;
			double num2 = data.dungeonEntranceStrengthX;
			double num3 = data.dungeonEntranceStrengthY;
			zero.X = (double)i;
			zero.Y = (double)j - num3 / 2.0;
			data.dungeonBounds.Top = (int)zero.Y;
			int num4 = 1;
			if (i > Main.maxTilesX / 2)
			{
				num4 = -1;
			}
			if (WorldGen.drunkWorldGen || WorldGen.getGoodWorldGen)
			{
				num4 *= -1;
			}
			this.Bounds.SetBounds((int)zero.X, (int)zero.Y, (int)zero.X, (int)zero.Y);
			int num5 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X - num2 * 0.6000000238418579 - (double)unifiedRandom.Next(2, 5))));
			int num6 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X + num2 * 0.6000000238418579 + (double)unifiedRandom.Next(2, 5))));
			int num7 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y - num3 * 0.6000000238418579 - (double)unifiedRandom.Next(2, 5))));
			int num8 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y + num3 * 0.6000000238418579 + (double)unifiedRandom.Next(8, 16))));
			this.Bounds.UpdateBounds(num5, num7, num6, num8);
			if (generating)
			{
				for (int m = num5; m < num6; m++)
				{
					for (int n = num7; n < num8; n++)
					{
						Main.tile[m, n].liquid = 0;
						if (Main.tile[m, n].wall != brickWallType)
						{
							Main.tile[m, n].wall = 0;
							if (m > num5 + 1 && m < num6 - 2 && n > num7 + 1 && n < num8 - 2)
							{
								Main.tile[m, n].wall = brickWallType;
							}
							Main.tile[m, n].active(true);
							Main.tile[m, n].type = brickTileType;
							Main.tile[m, n].Clear(TileDataType.Slope);
						}
					}
				}
			}
			int num9 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num5));
			int num10 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num5 + 5 + unifiedRandom.Next(4)));
			int num11 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num7 - 3 - unifiedRandom.Next(3)));
			int num12 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num7));
			this.Bounds.UpdateBounds(num9, num11, num10, num12);
			if (generating)
			{
				for (int num13 = num9; num13 < num10; num13++)
				{
					for (int num14 = num11; num14 < num12; num14++)
					{
						Main.tile[num13, num14].liquid = 0;
						if (Main.tile[num13, num14].wall != brickWallType)
						{
							Main.tile[num13, num14].active(true);
							Main.tile[num13, num14].type = brickTileType;
							Main.tile[num13, num14].Clear(TileDataType.Slope);
						}
					}
				}
			}
			num9 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num6 - 5 - unifiedRandom.Next(4)));
			num10 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num6));
			num11 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num7 - 3 - unifiedRandom.Next(3)));
			num12 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num7));
			this.Bounds.UpdateBounds(num9, num11, num10, num12);
			if (generating)
			{
				for (int num15 = num9; num15 < num10; num15++)
				{
					for (int num16 = num11; num16 < num12; num16++)
					{
						Main.tile[num15, num16].liquid = 0;
						if (Main.tile[num15, num16].wall != brickWallType)
						{
							Main.tile[num15, num16].active(true);
							Main.tile[num15, num16].type = brickTileType;
							Main.tile[num15, num16].Clear(TileDataType.Slope);
						}
					}
				}
			}
			int num17 = 2 + unifiedRandom.Next(4);
			int num18 = 1 + unifiedRandom.Next(2);
			int num19 = 0;
			int num20 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num7 - num18));
			data.dungeonBounds.UpdateBounds(num5, num20, num6, num7);
			if (generating)
			{
				for (int num21 = num5; num21 < num6; num21++)
				{
					for (int num22 = num20; num22 < num7; num22++)
					{
						this.Bounds.UpdateBounds(num21, num22);
						Main.tile[num21, num22].liquid = 0;
						if (Main.tile[num21, num22].wall != brickWallType)
						{
							Main.tile[num21, num22].active(true);
							Main.tile[num21, num22].type = brickTileType;
							Main.tile[num21, num22].Clear(TileDataType.Slope);
						}
					}
					num19++;
					if (num19 >= num17)
					{
						num21 += num17;
						num19 = 0;
					}
				}
			}
			if (generating)
			{
				double num23 = Main.worldSurface;
				if (data.Type == DungeonType.DualDungeon)
				{
					num23 = (double)DungeonUtils.GetDualDungeonBrickSupportCutoffY(data);
				}
				for (int num24 = num5; num24 < num6; num24++)
				{
					int num25 = num7;
					while ((double)num25 < num23)
					{
						Main.tile[num24, num25].liquid = 0;
						if (!DungeonUtils.InAnyPotentialDungeonBounds(num24, num25 - 5, 0, false))
						{
							Tile tile = Main.tile[num24, num25];
							bool flag = tile.active() && !this.settings.StyleData.TileIsInStyle((int)tile.type, true);
							bool flag2 = !this.settings.StyleData.WallIsInStyle((int)tile.wall, false);
							bool flag3 = DungeonUtils.IsConsideredDungeonWall((int)tile.wall, false);
							if ((tile.active() && flag) || !flag3)
							{
								Main.tile[num24, num25].active(true);
								Main.tile[num24, num25].type = brickTileType;
								if (num24 > num5 && num24 < num6 - 1)
								{
									Main.tile[num24, num25].wall = brickWallType;
								}
								Main.tile[num24, num25].Clear(TileDataType.Slope);
							}
							else if (flag2 && num24 > num5 && num24 < num6 - 1)
							{
								Main.tile[num24, num25].wall = brickWallType;
							}
						}
						num25++;
					}
				}
			}
			num5 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X - num2 * 0.5)));
			num6 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X + num2 * 0.5)));
			num7 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y - num3 * 0.5)));
			num8 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y + num3 * 0.5)));
			this.Bounds.UpdateBounds(num5, num7, num6, num8);
			if (generating)
			{
				for (int num26 = num5; num26 < num6; num26++)
				{
					for (int num27 = num7; num27 < num8; num27++)
					{
						Main.tile[num26, num27].liquid = 0;
						Main.tile[num26, num27].active(false);
						Main.tile[num26, num27].wall = brickWallType;
					}
				}
			}
			int num28 = (int)zero.X;
			int num29 = num8;
			for (int num30 = 0; num30 < 20; num30++)
			{
				num28 = (int)zero.X - num30;
				if (num28 <= 0)
				{
					break;
				}
				if (!Main.tile[num28, num29].active() && Main.wallDungeon[(int)Main.tile[num28, num29].wall])
				{
					DungeonPlatformData item = new DungeonPlatformData
					{
						Position = new Point(num28, num29),
						InAHallway = false
					};
					data.dungeonPlatformData.Add(item);
					break;
				}
				num28 = (int)zero.X + num30;
				if (num28 >= Main.maxTilesX)
				{
					break;
				}
				if (!Main.tile[num28, num29].active() && Main.wallDungeon[(int)Main.tile[num28, num29].wall])
				{
					DungeonPlatformData item2 = new DungeonPlatformData
					{
						Position = new Point(num28, num29),
						InAHallway = false
					};
					data.dungeonPlatformData.Add(item2);
					break;
				}
			}
			zero.X += num2 * 0.6000000238418579 * (double)num4;
			zero.Y += num3 * 0.5;
			num2 = data.dungeonEntranceStrengthX2;
			num3 = data.dungeonEntranceStrengthY2;
			zero.X += num2 * 0.550000011920929 * (double)num4;
			zero.Y -= num3 * 0.5;
			num5 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X - num2 * 0.6000000238418579 - (double)unifiedRandom.Next(1, 3))));
			num6 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X + num2 * 0.6000000238418579 + (double)unifiedRandom.Next(1, 3))));
			num7 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y - num3 * 0.6000000238418579 - (double)unifiedRandom.Next(1, 3))));
			num8 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y + num3 * 0.6000000238418579 + (double)unifiedRandom.Next(6, 16))));
			this.Bounds.UpdateBounds(num5, num7, num6, num8);
			if (generating)
			{
				for (int num31 = num5; num31 < num6; num31++)
				{
					for (int num32 = num7; num32 < num8; num32++)
					{
						Tile tile2 = Main.tile[num31, num32];
						if (!tile2.active() || tile2.type != brickTileType)
						{
							tile2.liquid = 0;
							bool flag4 = true;
							if (num4 < 0)
							{
								if ((double)num31 < zero.X - num2 * 0.5)
								{
									flag4 = false;
								}
							}
							else if ((double)num31 > zero.X + num2 * 0.5 - 1.0)
							{
								flag4 = false;
							}
							if (flag4)
							{
								tile2.wall = 0;
								tile2.active(true);
								tile2.type = brickTileType;
								tile2.Clear(TileDataType.Slope);
							}
						}
					}
				}
			}
			this.Bounds.UpdateBounds(num5, num7, num6, (int)Main.worldSurface);
			if (generating)
			{
				double num33 = Main.worldSurface;
				if (data.Type == DungeonType.DualDungeon)
				{
					num33 = (double)(DungeonCrawler.CurrentDungeonData.genVars.outerPotentialDungeonBounds.Top - 5);
				}
				for (int num34 = num5; num34 < num6; num34++)
				{
					int num35 = num8;
					while ((double)num35 < num33)
					{
						Main.tile[num34, num35].liquid = 0;
						if (!DungeonUtils.InAnyPotentialDungeonBounds(num34, num35 - 5, 0, false))
						{
							Tile tile3 = Main.tile[num34, num35];
							bool flag5 = tile3.active() && !this.settings.StyleData.TileIsInStyle((int)tile3.type, true);
							bool flag6 = !this.settings.StyleData.WallIsInStyle((int)tile3.wall, false);
							bool flag7 = DungeonUtils.IsConsideredDungeonWall((int)tile3.wall, false);
							if ((tile3.active() && flag5) || !flag7)
							{
								Main.tile[num34, num35].active(true);
								Main.tile[num34, num35].type = brickTileType;
								if (num34 > num5 && num34 < num6 - 1)
								{
									Main.tile[num34, num35].wall = brickWallType;
								}
								Main.tile[num34, num35].Clear(TileDataType.Slope);
							}
							else if (flag6 && num34 > num5 && num34 < num6 - 1)
							{
								Main.tile[num34, num35].wall = brickWallType;
							}
						}
						num35++;
					}
				}
			}
			num5 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X - num2 * 0.5)));
			num6 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X + num2 * 0.5)));
			num9 = num5;
			if (num4 < 0)
			{
				Math.Max(0, Math.Min(Main.maxTilesX - 1, num9++));
			}
			num10 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num9 + 5 + unifiedRandom.Next(4)));
			num11 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num7 - 3 - unifiedRandom.Next(3)));
			num12 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num7));
			this.Bounds.UpdateBounds(num9, num11, num10, num12);
			if (generating)
			{
				for (int num36 = num9; num36 < num10; num36++)
				{
					for (int num37 = num11; num37 < num12; num37++)
					{
						Main.tile[num36, num37].liquid = 0;
						if (Main.tile[num36, num37].wall != brickWallType)
						{
							Main.tile[num36, num37].active(true);
							Main.tile[num36, num37].type = brickTileType;
							Main.tile[num36, num37].Clear(TileDataType.Slope);
						}
					}
				}
			}
			num9 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num6 - 5 - unifiedRandom.Next(4)));
			num10 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num6));
			num11 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num7 - 3 - unifiedRandom.Next(3)));
			num12 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num7));
			this.Bounds.UpdateBounds(num9, num11, num10, num12);
			if (generating)
			{
				for (int num38 = num9; num38 < num10; num38++)
				{
					for (int num39 = num11; num39 < num12; num39++)
					{
						Main.tile[num38, num39].liquid = 0;
						if (Main.tile[num38, num39].wall != brickWallType)
						{
							Main.tile[num38, num39].active(true);
							Main.tile[num38, num39].type = brickTileType;
							Main.tile[num38, num39].Clear(TileDataType.Slope);
						}
					}
				}
			}
			if (num4 < 0)
			{
				num6++;
			}
			num18 = 1 + unifiedRandom.Next(2);
			num17 = 2 + unifiedRandom.Next(4);
			num19 = 0;
			num20 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num7 - num18));
			if (generating)
			{
				for (int num40 = num5 + 1; num40 < num6 - 1; num40++)
				{
					for (int num41 = num20; num41 < num7; num41++)
					{
						Main.tile[num40, num41].liquid = 0;
						if (Main.tile[num40, num41].wall != brickWallType)
						{
							Main.tile[num40, num41].active(true);
							Main.tile[num40, num41].type = brickTileType;
							Main.tile[num40, num41].Clear(TileDataType.Slope);
						}
					}
					num19++;
					if (num19 >= num17)
					{
						num40 += num17;
						num19 = 0;
					}
				}
			}
			if (!dungeonEntranceIsUnderground && !dungeonEntranceIsBuried)
			{
				num5 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X - num2 * 0.6)));
				num6 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X + num2 * 0.6)));
				num7 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.Y - num3 * 0.6)));
				num8 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.Y + num3 * 0.6)));
				this.Bounds.UpdateBounds(num5, num7, num6, num8);
				if (generating)
				{
					for (int num42 = num5; num42 < num6; num42++)
					{
						for (int num43 = num7; num43 < num8; num43++)
						{
							Main.tile[num42, num43].liquid = 0;
							Main.tile[num42, num43].wall = 0;
						}
					}
				}
			}
			num5 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X - num2 * 0.5)));
			num6 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X + num2 * 0.5)));
			num7 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y - num3 * 0.5)));
			num8 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y + num3 * 0.5)));
			if ((dungeonEntranceIsUnderground || dungeonEntranceIsBuried) && num4 == -1)
			{
				num5 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num5 + 1));
				num6 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num6 + 1));
			}
			this.Bounds.UpdateBounds(num5, num7, num6, num8);
			if (generating)
			{
				for (int num44 = num5; num44 < num6; num44++)
				{
					for (int num45 = num7; num45 < num8; num45++)
					{
						Main.tile[num44, num45].liquid = 0;
						Main.tile[num44, num45].active(false);
						Main.tile[num44, num45].wall = 0;
					}
				}
			}
			this.OldManSpawn = DungeonUtils.SetOldManSpawnAndSpawnOldManIfDefaultDungeon((int)zero.X, num8, generating);
			if (generating && SpecialSeedFeatures.DungeonEntranceHasATree)
			{
				DungeonUtils.GenerateDungeonTree(data, data.genVars.generatingDungeonPositionX, (int)Main.worldSurface, data.genVars.generatingDungeonPositionY, true);
			}
			if (generating && SpecialSeedFeatures.DungeonEntranceHasStairs)
			{
				int i2 = (num4 == 1) ? num6 : num5;
				int depth = DungeonUtils.GetDualDungeonBrickSupportCutoffY(data) - num8 + 5;
				DungeonUtils.GenerateDungeonStairs(data, i2, num8, num4, brickTileType, brickWallType, depth);
			}
			num18 = 1 + unifiedRandom.Next(2);
			num17 = 2 + unifiedRandom.Next(4);
			num19 = 0;
			num5 = (int)(zero.X - num2 * 0.5);
			num6 = (int)(zero.X + num2 * 0.5);
			if (dungeonEntranceIsUnderground || dungeonEntranceIsBuried)
			{
				if (num4 == -1)
				{
					num5++;
					num6++;
				}
			}
			else
			{
				num5 += 2;
				num6 -= 2;
			}
			num5 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num5));
			num6 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num6));
			if (generating)
			{
				for (int num46 = num5; num46 < num6; num46++)
				{
					for (int num47 = num7; num47 < num8 + 1; num47++)
					{
						WorldGen.PlaceWall(num46, num47, (int)brickWallType, true);
					}
					if (!dungeonEntranceIsUnderground && !dungeonEntranceIsBuried)
					{
						num19++;
						if (num19 >= num17)
						{
							num46 += num17 * 2;
							num19 = 0;
						}
					}
				}
			}
			if (WorldGen.drunkWorldGen && !WorldGen.SecretSeed.noSurface.Enabled)
			{
				num5 = (int)(zero.X - num2 * 0.5);
				num6 = (int)(zero.X + num2 * 0.5);
				if (num4 == 1)
				{
					num5 = num6 - 3;
				}
				else
				{
					num6 = num5 + 3;
				}
				num5 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num5));
				num6 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num6));
				this.Bounds.UpdateBounds(num5, num7, num6, num8);
				if (generating)
				{
					for (int num48 = num5; num48 < num6; num48++)
					{
						for (int num49 = num7; num49 < num8 + 1; num49++)
						{
							Main.tile[num48, num49].active(true);
							Main.tile[num48, num49].type = brickTileType;
							Main.tile[num48, num49].Clear(TileDataType.Slope);
						}
					}
				}
			}
			zero.X -= num2 * 0.6000000238418579 * (double)num4;
			zero.Y += num3 * 0.5;
			num2 = 15.0;
			num3 = 3.0;
			zero.Y -= num3 * 0.5;
			num5 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X - num2 * 0.5)));
			num6 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X + num2 * 0.5)));
			num7 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y - num3 * 0.5)));
			num8 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y + num3 * 0.5)));
			this.Bounds.UpdateBounds(num5, num7, num6, num8);
			if (num4 < 0)
			{
				zero.X -= 1.0;
			}
			Vector2D vector2D = zero;
			vector2D.Y += 1.0;
			if (generating)
			{
				for (int num50 = num5; num50 < num6; num50++)
				{
					for (int num51 = num7; num51 < num8; num51++)
					{
						Main.tile[num50, num51].active(false);
						if ((num4 > 0 && (double)num50 < vector2D.X) || (num4 < 0 && (double)num50 > vector2D.X) || (dungeonEntranceIsUnderground || dungeonEntranceIsBuried))
						{
							Main.tile[num50, num51].wall = brickWallType;
						}
					}
				}
			}
			if (generating)
			{
				WorldGen.PlaceTile((int)vector2D.X, (int)vector2D.Y, 10, true, false, -1, 13);
			}
			this.Bounds.CalculateHitbox();
		}
	}
}
