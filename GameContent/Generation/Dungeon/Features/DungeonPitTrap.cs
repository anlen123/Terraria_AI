using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004EA RID: 1258
	public class DungeonPitTrap : DungeonFeature
	{
		// Token: 0x06003516 RID: 13590 RVA: 0x00608988 File Offset: 0x00606B88
		public DungeonPitTrap(DungeonFeatureSettings settings, bool addToFeatures = true) : base(settings)
		{
			if (addToFeatures)
			{
				DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
			}
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x00612944 File Offset: 0x00610B44
		public override bool GenerateFeature(DungeonData data, int x, int y)
		{
			this.generated = false;
			DungeonGenerationStyleData style = ((DungeonPitTrapSettings)this.settings).Style;
			if (this.PitTrap(data, x, y, style.BrickTileType, style.PitTrapTileType, style.BrickWallType, true))
			{
				this.generated = true;
				return true;
			}
			return false;
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x00612991 File Offset: 0x00610B91
		public override bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
		{
			return feature is DungeonGlobalPaintings || feature is DungeonGlobalWallVariants;
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x006129A8 File Offset: 0x00610BA8
		public bool PitTrap(DungeonData data, int i, int j, ushort tileType, ushort pitTrapTileType, ushort wallType, bool generating = false)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			DungeonPitTrapSettings dungeonPitTrapSettings = (DungeonPitTrapSettings)this.settings;
			bool flag = data.Type == DungeonType.DualDungeon;
			bool flag2 = TileID.Sets.Falling[(int)pitTrapTileType];
			bool flag3 = TileID.Sets.CrackedBricks[(int)pitTrapTileType];
			bool flag4 = !flag2 && !flag3;
			int num = j;
			int num2 = num;
			int width = dungeonPitTrapSettings.Width;
			int num3 = dungeonPitTrapSettings.Height;
			if (width < 1 || num3 < 1)
			{
				return false;
			}
			if (flag && (this.Flooded || dungeonPitTrapSettings.Flooded))
			{
				int num4 = 300;
				for (int k = width * Math.Max(1, num3 - dungeonPitTrapSettings.TopDensity); k < num4; k = width * Math.Max(1, num3 - dungeonPitTrapSettings.TopDensity))
				{
					num3++;
				}
			}
			int num5 = width + dungeonPitTrapSettings.EdgeWidth;
			int num6 = num3 + dungeonPitTrapSettings.EdgeHeight;
			if (!WorldGen.InWorld(i, num, (num5 > num6) ? num5 : num6))
			{
				return false;
			}
			if (!DungeonUtils.IsConsideredDungeonWall((int)Main.tile[i, num].wall, false))
			{
				return false;
			}
			if (Main.tile[i, num].active())
			{
				return false;
			}
			int l = num;
			while (l < Main.maxTilesY)
			{
				if (l > Main.UnderworldLayer)
				{
					return false;
				}
				if (Main.tile[i, l].active() && WorldGen.SolidTile(i, l, false))
				{
					if (Main.tile[i, l].type == 48)
					{
						return false;
					}
					num = l;
					num2 = l;
					break;
				}
				else
				{
					l++;
				}
			}
			if (!DungeonUtils.IsConsideredDungeonWall((int)Main.tile[i - width, num].wall, false) || !DungeonUtils.IsConsideredDungeonWall((int)Main.tile[i + width, num].wall, false))
			{
				return false;
			}
			if (data.Type == DungeonType.DualDungeon)
			{
				for (int m = i - num5; m <= i + num5; m++)
				{
					for (int n = num; n < num + num6 + 2; n++)
					{
						Tile tile = Main.tile[m, n];
						if (tile.active() && tile.type != tileType)
						{
							return false;
						}
						if (tile.wall != wallType)
						{
							return false;
						}
					}
				}
			}
			int num7 = 30;
			for (int num8 = num; num8 < num + num7; num8++)
			{
				bool flag5 = true;
				for (int num9 = i - width; num9 <= i + width; num9++)
				{
					Tile tile2 = Main.tile[num9, num8];
					if (tile2.active() && DungeonUtils.IsConsideredDungeonTile((int)tile2.type, false))
					{
						flag5 = false;
						break;
					}
				}
				if (flag5)
				{
					num = num8;
					break;
				}
			}
			if (num + num6 >= Main.UnderworldLayer)
			{
				return false;
			}
			int[] array = new int[num5 * 2 + 1];
			if (flag)
			{
				for (int num10 = i - num5; num10 <= i + num5; num10++)
				{
					int num11 = num;
					Tile tile3 = Main.tile[num10, num11];
					while (num11 > 10 && tile3.active() && (DungeonUtils.IsConsideredDungeonTile((int)tile3.type, false) || DungeonUtils.IsConsideredCrackedDungeonTile((int)tile3.type, false) || DungeonUtils.IsConsideredPitTrapTile((int)tile3.type, false)))
					{
						num11--;
						tile3 = Main.tile[num10, num11];
					}
					array[num10 - (i - num5)] = num11 + 1;
				}
			}
			this.Bounds.SetBounds(i - num5, num2, i + num5, num + num6);
			this.Bounds.CalculateHitbox();
			if (flag)
			{
				if (!data.CanGenerateFeatureInArea(this, this.Bounds))
				{
					return false;
				}
				if (dungeonPitTrapSettings.ConnectedRoom != null)
				{
					DungeonRoom connectedRoom = dungeonPitTrapSettings.ConnectedRoom;
					for (int num12 = i - num5; num12 <= i + num5; num12++)
					{
						for (int num13 = num2; num13 <= num + num6; num13++)
						{
							if ((num12 < i - width || num12 > i + width || num13 < num || num13 > num + num3) && !connectedRoom.OuterBounds.Contains(num12, num13))
							{
								Tile tile4 = Main.tile[num12, num13];
								if (!tile4.active() && DungeonUtils.IsConsideredDungeonWall((int)tile4.wall, false))
								{
									return false;
								}
							}
						}
					}
				}
			}
			else
			{
				for (int num14 = i - width; num14 <= i + width; num14++)
				{
					for (int num15 = num; num15 <= num + num3; num15++)
					{
						Tile tile5 = Main.tile[num14, num15];
						if (tile5.active() && (DungeonUtils.IsConsideredDungeonTile((int)tile5.type, false) || DungeonUtils.IsConsideredCrackedDungeonTile((int)tile5.type, false) || DungeonUtils.IsConsideredPitTrapTile((int)tile5.type, false)))
						{
							return false;
						}
					}
				}
			}
			if (generating && !flag)
			{
				for (int num16 = i - width; num16 <= i + width; num16++)
				{
					for (int num17 = num2; num17 <= num + num3; num17++)
					{
						Tile tile6 = Main.tile[num16, num17];
						if (tile6.active() && DungeonUtils.IsConsideredDungeonTile((int)tile6.type, false))
						{
							DungeonUtils.ChangeTileType(tile6, pitTrapTileType, true, -1);
							DungeonUtils.ChangeWallType(tile6, wallType, false, -1);
						}
					}
				}
			}
			if (generating)
			{
				for (int num18 = i - num5; num18 <= i + num5; num18++)
				{
					int num19 = num2;
					if (flag)
					{
						num19 = this.GetHeight(array, i, num18 - (i - num5), width, num5, false);
					}
					for (int num20 = num19; num20 <= num + num6; num20++)
					{
						Tile tile7 = Main.tile[num18, num20];
						tile7.liquidType(0);
						tile7.liquid = 0;
						if (!DungeonUtils.IsConsideredDungeonWall((int)tile7.wall, false))
						{
							if (num18 > i - num5 && num18 < i + num5 && num20 < num + num6)
							{
								ushort wall = tile7.wall;
								DungeonUtils.ChangeTileType(tile7, tileType, true, -1);
								if (!DungeonUtils.IsConsideredDungeonWall((int)wall, false))
								{
									DungeonUtils.ChangeWallType(tile7, wallType, false, -1);
								}
							}
							else
							{
								DungeonUtils.ChangeTileType(tile7, tileType, false, -1);
							}
						}
					}
				}
			}
			if (generating)
			{
				for (int num21 = i - width; num21 <= i + width; num21++)
				{
					int num22 = num2;
					if (flag)
					{
						num22 = this.GetHeight(array, i, num21 - (i - width), width, num5, true);
					}
					for (int num23 = num22; num23 <= num + num3; num23++)
					{
						bool flag6;
						if (flag && num23 <= num2 + dungeonPitTrapSettings.TopDensity)
						{
							flag6 = false;
							if (Main.tile[num21, num23].active())
							{
								DungeonUtils.ChangeTileType(Main.tile[num21, num23], pitTrapTileType, false, -1);
							}
							Main.tile[num21, num23].liquidType(0);
							Main.tile[num21, num23].liquid = 0;
						}
						else
						{
							flag6 = (Main.tile[num21, num23].type != pitTrapTileType);
						}
						if (flag6)
						{
							if (dungeonPitTrapSettings.Flooded)
							{
								Main.tile[num21, num23].liquidType(0);
								Main.tile[num21, num23].liquid = byte.MaxValue;
							}
							else
							{
								Main.tile[num21, num23].liquidType(0);
								Main.tile[num21, num23].liquid = 0;
							}
							bool flag7 = num21 == i - width && Main.tile[num21 - 1, num23].active();
							bool flag8 = num21 == i + width && Main.tile[num21 + 1, num23].active();
							bool flag9 = num23 == num + num3 && Main.tile[num21, num23 + 1].active();
							bool flag10 = num21 == i - width + 1 && num23 % 2 == 0 && Main.tile[num21 - 1, num23].active();
							bool flag11 = num21 == i + width - 1 && num23 % 2 == 0 && Main.tile[num21 + 1, num23].active();
							bool flag12 = num23 == num + num3 - 1 && num21 % 2 == 0 && Main.tile[num21, num23 + 1].active();
							if (flag7 || flag8 || flag9)
							{
								DungeonUtils.ChangeTileType(Main.tile[num21, num23], 48, false, -1);
							}
							else if (flag10 || flag11 || flag12)
							{
								DungeonUtils.ChangeTileType(Main.tile[num21, num23], 48, false, -1);
							}
							else if (flag2)
							{
								if (num21 <= i - width + 2 || num21 >= i + width - 2 || num23 >= num + num3 - 2)
								{
									DungeonUtils.ChangeTileType(Main.tile[num21, num23], tileType, false, -1);
									Main.tile[num21, num23].inActive(true);
								}
								else
								{
									Main.tile[num21, num23].active(false);
								}
							}
							else
							{
								Main.tile[num21, num23].active(false);
							}
						}
					}
				}
			}
			if (generating && !flag3)
			{
				Point zero = Point.Zero;
				for (int num24 = i - num5; num24 <= i + num5; num24++)
				{
					if (num24 >= i - width)
					{
						bool flag13 = num24 <= i + width;
					}
					int num25 = num2;
					if (flag)
					{
						num25 = this.GetHeight(array, i, num24 - (i - num5), width, num5, false);
					}
					for (int num26 = num25 - 1; num26 <= num + num6; num26++)
					{
						Tile tile8 = Main.tile[num24, num26];
						if (tile8.active() && tile8.type == pitTrapTileType)
						{
							bool flag14 = false;
							bool flag15 = false;
							if (flag4)
							{
								flag15 = (flag14 = true);
							}
							else if (flag2)
							{
								Tile tile9 = Main.tile[num24, num26 + 1];
								if (num24 == i - width)
								{
									flag14 = true;
								}
								if (!tile9.active() || tile9.type != pitTrapTileType)
								{
									flag15 = (flag14 = true);
									tile8.type = tileType;
								}
							}
							if (flag14)
							{
								tile8.wire(true);
							}
							if (flag15)
							{
								tile8.actuator(true);
							}
							if (tile8.slope() == 0 && !tile8.halfBrick())
							{
								Tile tile10 = Main.tile[num24, num26 - 1];
								if (!tile10.active())
								{
									WorldGen.PlaceTile(num24, num26 - 1, 135, true, false, -1, 7);
									tile10 = Main.tile[num24, num26 - 1];
									if (tile10.active() && tile10.type == 135)
									{
										tile10.wire(true);
										if (zero != Point.Zero)
										{
											WorldGen.AddWireFromPointToPoint(num24, num26 - 1, zero.X, zero.Y, 0, false);
										}
										zero = new Point(num24, num26 - 1);
									}
								}
							}
						}
					}
				}
			}
			this.Flooded = dungeonPitTrapSettings.Flooded;
			return true;
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x00613472 File Offset: 0x00611672
		public int GetHeight(int[] heights, int baseX, int x, int innerWidth, int outerWidth, bool inner)
		{
			if (inner)
			{
				x += outerWidth - innerWidth;
			}
			return heights[x];
		}

		// Token: 0x04005A75 RID: 23157
		public bool Flooded;
	}
}
