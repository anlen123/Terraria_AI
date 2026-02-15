using System;
using System.Collections.Generic;
using Terraria.GameContent.Generation.Dungeon;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ObjectData;
using Terraria.WorldBuilding;

namespace Terraria
{
	// Token: 0x0200002A RID: 42
	public class Liquid
	{
		// Token: 0x060001F5 RID: 501 RVA: 0x0001ED58 File Offset: 0x0001CF58
		public static void NetSendLiquid(int x, int y)
		{
			if (WorldGen.isGeneratingOrLoadingWorld)
			{
				return;
			}
			HashSet<int> netChangeSet = Liquid._netChangeSet;
			lock (netChangeSet)
			{
				Liquid._netChangeSet.Add((x & 65535) << 16 | (y & 65535));
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0001EDB8 File Offset: 0x0001CFB8
		public static void tilesIgnoreWater(bool ignoreSolids)
		{
			WorldGen.SetBoulderSolidity(!ignoreSolids);
			Main.tileSolid[546] = !ignoreSolids;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0001EDD2 File Offset: 0x0001CFD2
		public static void worldGenTilesIgnoreWater(bool ignoreSolids)
		{
			Main.tileSolid[10] = !ignoreSolids;
			Main.tileSolid[192] = !ignoreSolids;
			Main.tileSolid[191] = !ignoreSolids;
			Main.tileSolid[190] = !ignoreSolids;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0001EE10 File Offset: 0x0001D010
		public static void ReInit()
		{
			Liquid.skipCount = 0;
			Liquid.stuckCount = 0;
			Liquid.stuckAmount = 0;
			Liquid.cycles = 10;
			Liquid.curMaxLiquid = Liquid.maxLiquid;
			Liquid.numLiquid = 0;
			Liquid.stuck = false;
			Liquid.quickFall = false;
			Liquid.quickSettle = false;
			Liquid.wetCounter = 0;
			Liquid.panicCounter = 0;
			Liquid.panicMode = false;
			Liquid.panicY = 0;
			if (Main.Setting_UseReducedMaxLiquids)
			{
				Liquid.curMaxLiquid = 5000;
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0001EE84 File Offset: 0x0001D084
		public static void QuickWater(int verbose = 0, int minY = -1, int maxY = -1)
		{
			if (WorldGen.isGeneratingOrLoadingWorld)
			{
				WorldGen.ShimmerRemoveWater();
				if (WorldGen.noTrapsWorldGen)
				{
					WorldGen.SetBoulderSolidity(false);
				}
			}
			Main.tileSolid[379] = true;
			Liquid.tilesIgnoreWater(true);
			if (minY == -1)
			{
				minY = 3;
			}
			if (maxY == -1)
			{
				maxY = Main.maxTilesY - 3;
			}
			for (int i = maxY; i >= minY; i--)
			{
				Liquid.UpdateProgressDisplay(verbose, minY, maxY, i);
				for (int j = 4; j < Main.maxTilesX - 4; j++)
				{
					if (Main.tile[j, i].liquid != 0)
					{
						Liquid.SettleWaterAt(j, i);
					}
				}
			}
			Liquid.tilesIgnoreWater(false);
			if (WorldGen.isGeneratingOrLoadingWorld)
			{
				WorldGen.ShimmerRemoveWater();
				if (WorldGen.noTrapsWorldGen)
				{
					WorldGen.SetBoulderSolidity(true);
				}
			}
			if (WorldGen.generatingWorld && !Main.skyblockWorld)
			{
				WorldGen.LiquidInteractionsCleanup();
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0001EF48 File Offset: 0x0001D148
		private static void SettleWaterAt(int originX, int originY)
		{
			Tile tile = Main.tile[originX, originY];
			Liquid.tilesIgnoreWater(true);
			if (tile.liquid == 0)
			{
				return;
			}
			if (tile.active() && tile.type == 379)
			{
				return;
			}
			int num = originX;
			int num2 = originY;
			bool tileAtXYHasLava = tile.lava();
			bool flag = tile.honey();
			bool flag2 = tile.shimmer();
			int num3 = (int)tile.liquid;
			byte b = tile.liquidType();
			tile.liquid = 0;
			bool flag3 = true;
			for (;;)
			{
				Tile tile2 = Main.tile[num, num2 + 1];
				bool flag4 = false;
				while (num2 < Main.maxTilesY - 5 && tile2.liquid == 0 && (!tile2.nactive() || !Main.tileSolid[(int)tile2.type] || Main.tileSolidTop[(int)tile2.type]))
				{
					num2++;
					flag4 = true;
					flag3 = false;
					tile2 = Main.tile[num, num2 + 1];
				}
				if (WorldGen.isGeneratingOrLoadingWorld && flag4 && !flag && !flag2)
				{
					if (WorldGen.remixWorldGen)
					{
						if (num2 > GenVars.lavaLine && ((double)num2 < Main.rockLayer - 80.0 || num2 > Main.maxTilesY - 350))
						{
							if (WorldGen.oceanDepths(num, num2))
							{
								b = 0;
							}
							else
							{
								b = 1;
							}
						}
						else
						{
							b = 0;
						}
					}
					else if (num2 > GenVars.waterLine)
					{
						b = 1;
					}
					if (WorldGen.generatingWorld && DungeonUtils.InAnyPotentialDungeonBounds(num, num2, 0, false) && DungeonUtils.IsConsideredDungeonWall((int)tile.wall, true))
					{
						b = 0;
					}
				}
				int num4 = -1;
				int num5 = 0;
				int num6 = -1;
				int num7 = 0;
				bool flag5 = false;
				bool flag6 = false;
				bool flag7 = false;
				for (;;)
				{
					if (Main.tile[num + num5 * num4, num2].liquid == 0)
					{
						num6 = num4;
						num7 = num5;
					}
					if (num4 == -1 && num + num5 * num4 < 5)
					{
						flag6 = true;
					}
					else if (num4 == 1 && num + num5 * num4 > Main.maxTilesX - 5)
					{
						flag5 = true;
					}
					tile2 = Main.tile[num + num5 * num4, num2 + 1];
					if (tile2.liquid != 0 && tile2.liquid != 255 && tile2.liquidType() == b)
					{
						int num8 = (int)(byte.MaxValue - tile2.liquid);
						if (num8 > num3)
						{
							num8 = num3;
						}
						Tile tile3 = tile2;
						tile3.liquid += (byte)num8;
						num3 -= num8;
						if (num3 == 0)
						{
							break;
						}
					}
					if (num2 < Main.maxTilesY - 5 && tile2.liquid == 0 && (!tile2.nactive() || !Main.tileSolid[(int)tile2.type] || Main.tileSolidTop[(int)tile2.type]))
					{
						goto IL_267;
					}
					Tile tile4 = Main.tile[num + (num5 + 1) * num4, num2];
					if ((tile4.liquid != 0 && (!flag3 || num4 != 1)) || (tile4.nactive() && Main.tileSolid[(int)tile4.type] && !Main.tileSolidTop[(int)tile4.type]))
					{
						if (num4 == 1)
						{
							flag5 = true;
						}
						else
						{
							flag6 = true;
						}
					}
					if (flag6 && flag5)
					{
						break;
					}
					if (flag5)
					{
						num4 = -1;
						num5++;
					}
					else if (flag6)
					{
						if (num4 == 1)
						{
							num5++;
						}
						num4 = 1;
					}
					else
					{
						if (num4 == 1)
						{
							num5++;
						}
						num4 = -num4;
					}
				}
				IL_310:
				num += num7 * num6;
				if (num3 != 0 && flag7)
				{
					num2++;
					continue;
				}
				break;
				IL_267:
				flag7 = true;
				goto IL_310;
			}
			Main.tile[num, num2].liquid = (byte)num3;
			Main.tile[num, num2].liquidType((int)b);
			if (Main.tile[num, num2].liquid > 0)
			{
				Liquid.AttemptToMoveLava(num, num2, tileAtXYHasLava);
				Liquid.AttemptToMoveHoney(num, num2, flag);
				Liquid.AttemptToMoveShimmer(num, num2, flag2);
			}
			Liquid.tilesIgnoreWater(false);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0001F2DC File Offset: 0x0001D4DC
		private static void AttemptToMoveHoney(int X, int Y, bool tileAtXYHasHoney)
		{
			if (Main.tile[X - 1, Y].liquid > 0 && Main.tile[X - 1, Y].honey() != tileAtXYHasHoney)
			{
				if (tileAtXYHasHoney)
				{
					Liquid.HoneyCheck(X, Y);
					return;
				}
				Liquid.HoneyCheck(X - 1, Y);
				return;
			}
			else if (Main.tile[X + 1, Y].liquid > 0 && Main.tile[X + 1, Y].honey() != tileAtXYHasHoney)
			{
				if (tileAtXYHasHoney)
				{
					Liquid.HoneyCheck(X, Y);
					return;
				}
				Liquid.HoneyCheck(X + 1, Y);
				return;
			}
			else
			{
				if (Main.tile[X, Y - 1].liquid <= 0 || Main.tile[X, Y - 1].honey() == tileAtXYHasHoney)
				{
					if (Main.tile[X, Y + 1].liquid > 0 && Main.tile[X, Y + 1].honey() != tileAtXYHasHoney)
					{
						if (tileAtXYHasHoney)
						{
							Liquid.HoneyCheck(X, Y);
							return;
						}
						Liquid.HoneyCheck(X, Y + 1);
					}
					return;
				}
				if (tileAtXYHasHoney)
				{
					Liquid.HoneyCheck(X, Y);
					return;
				}
				Liquid.HoneyCheck(X, Y - 1);
				return;
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0001F3EC File Offset: 0x0001D5EC
		private static void AttemptToMoveLava(int X, int Y, bool tileAtXYHasLava)
		{
			if (Main.tile[X - 1, Y].liquid > 0 && Main.tile[X - 1, Y].lava() != tileAtXYHasLava)
			{
				if (tileAtXYHasLava)
				{
					Liquid.LavaCheck(X, Y);
					return;
				}
				Liquid.LavaCheck(X - 1, Y);
				return;
			}
			else if (Main.tile[X + 1, Y].liquid > 0 && Main.tile[X + 1, Y].lava() != tileAtXYHasLava)
			{
				if (tileAtXYHasLava)
				{
					Liquid.LavaCheck(X, Y);
					return;
				}
				Liquid.LavaCheck(X + 1, Y);
				return;
			}
			else
			{
				if (Main.tile[X, Y - 1].liquid <= 0 || Main.tile[X, Y - 1].lava() == tileAtXYHasLava)
				{
					if (Main.tile[X, Y + 1].liquid > 0 && Main.tile[X, Y + 1].lava() != tileAtXYHasLava)
					{
						if (tileAtXYHasLava)
						{
							Liquid.LavaCheck(X, Y);
							return;
						}
						Liquid.LavaCheck(X, Y + 1);
					}
					return;
				}
				if (tileAtXYHasLava)
				{
					Liquid.LavaCheck(X, Y);
					return;
				}
				Liquid.LavaCheck(X, Y - 1);
				return;
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0001F4FC File Offset: 0x0001D6FC
		private static void AttemptToMoveShimmer(int X, int Y, bool tileAtXYHasShimmer)
		{
			if (Main.tile[X - 1, Y].liquid > 0 && Main.tile[X - 1, Y].shimmer() != tileAtXYHasShimmer)
			{
				if (tileAtXYHasShimmer)
				{
					Liquid.ShimmerCheck(X, Y);
					return;
				}
				Liquid.ShimmerCheck(X - 1, Y);
				return;
			}
			else if (Main.tile[X + 1, Y].liquid > 0 && Main.tile[X + 1, Y].shimmer() != tileAtXYHasShimmer)
			{
				if (tileAtXYHasShimmer)
				{
					Liquid.ShimmerCheck(X, Y);
					return;
				}
				Liquid.ShimmerCheck(X + 1, Y);
				return;
			}
			else
			{
				if (Main.tile[X, Y - 1].liquid <= 0 || Main.tile[X, Y - 1].shimmer() == tileAtXYHasShimmer)
				{
					if (Main.tile[X, Y + 1].liquid > 0 && Main.tile[X, Y + 1].shimmer() != tileAtXYHasShimmer)
					{
						if (tileAtXYHasShimmer)
						{
							Liquid.ShimmerCheck(X, Y);
							return;
						}
						Liquid.ShimmerCheck(X, Y + 1);
					}
					return;
				}
				if (tileAtXYHasShimmer)
				{
					Liquid.ShimmerCheck(X, Y);
					return;
				}
				Liquid.ShimmerCheck(X, Y - 1);
				return;
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0001F60C File Offset: 0x0001D80C
		private static void UpdateProgressDisplay(int verbose, int minY, int maxY, int y)
		{
			if (verbose > 0)
			{
				float num = (float)(maxY - y) / (float)(maxY - minY + 1);
				num /= (float)verbose;
				Main.statusText = string.Concat(new object[]
				{
					Lang.gen[27].Value,
					" ",
					(int)(num * 100f + 1f),
					"%"
				});
				return;
			}
			if (verbose < 0)
			{
				float num2 = (float)(maxY - y) / (float)(maxY - minY + 1);
				num2 /= (float)(-(float)verbose);
				Main.statusText = string.Concat(new object[]
				{
					Lang.gen[18].Value,
					" ",
					(int)(num2 * 100f + 1f),
					"%"
				});
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0001F6D4 File Offset: 0x0001D8D4
		public void Update()
		{
			Main.tileSolid[379] = true;
			Tile tile = Main.tile[this.x - 1, this.y];
			Tile tile2 = Main.tile[this.x + 1, this.y];
			Tile tile3 = Main.tile[this.x, this.y - 1];
			Tile tile4 = Main.tile[this.x, this.y + 1];
			Tile tile5 = Main.tile[this.x, this.y];
			if (tile5.nactive() && Main.tileSolid[(int)tile5.type] && !Main.tileSolidTop[(int)tile5.type])
			{
				ushort type = tile5.type;
				this.kill = 999;
				return;
			}
			byte liquid = tile5.liquid;
			if (this.y > Main.UnderworldLayer && tile5.liquidType() == 0 && tile5.liquid > 0)
			{
				byte b = 2;
				if (tile5.liquid < b)
				{
					b = tile5.liquid;
				}
				Tile tile6 = tile5;
				tile6.liquid -= b;
			}
			if (tile5.liquid == 0)
			{
				this.kill = 999;
				return;
			}
			if (tile5.lava())
			{
				Liquid.LavaCheck(this.x, this.y);
				if (!Liquid.quickFall)
				{
					if (this.delay < 5)
					{
						this.delay++;
						return;
					}
					this.delay = 0;
				}
			}
			else
			{
				if (tile.lava())
				{
					Liquid.AddWater(this.x - 1, this.y);
				}
				if (tile2.lava())
				{
					Liquid.AddWater(this.x + 1, this.y);
				}
				if (tile3.lava())
				{
					Liquid.AddWater(this.x, this.y - 1);
				}
				if (tile4.lava())
				{
					Liquid.AddWater(this.x, this.y + 1);
				}
				if (tile5.honey())
				{
					Liquid.HoneyCheck(this.x, this.y);
					if (!Liquid.quickFall)
					{
						if (this.delay < 10)
						{
							this.delay++;
							return;
						}
						this.delay = 0;
					}
				}
				else
				{
					if (tile.honey())
					{
						Liquid.AddWater(this.x - 1, this.y);
					}
					if (tile2.honey())
					{
						Liquid.AddWater(this.x + 1, this.y);
					}
					if (tile3.honey())
					{
						Liquid.AddWater(this.x, this.y - 1);
					}
					if (tile4.honey())
					{
						Liquid.AddWater(this.x, this.y + 1);
					}
					if (tile5.shimmer())
					{
						Liquid.ShimmerCheck(this.x, this.y);
					}
					else
					{
						if (tile.shimmer())
						{
							Liquid.AddWater(this.x - 1, this.y);
						}
						if (tile2.shimmer())
						{
							Liquid.AddWater(this.x + 1, this.y);
						}
						if (tile3.shimmer())
						{
							Liquid.AddWater(this.x, this.y - 1);
						}
						if (tile4.shimmer())
						{
							Liquid.AddWater(this.x, this.y + 1);
						}
					}
				}
			}
			if ((!tile4.nactive() || !Main.tileSolid[(int)tile4.type] || Main.tileSolidTop[(int)tile4.type]) && (tile4.liquid <= 0 || tile4.liquidType() == tile5.liquidType()) && tile4.liquid < 255)
			{
				bool flag = false;
				float num = (float)(byte.MaxValue - tile4.liquid);
				if (num > (float)tile5.liquid)
				{
					num = (float)tile5.liquid;
				}
				if (num == 1f && tile5.liquid == 255)
				{
					flag = true;
				}
				if (!flag)
				{
					Tile tile7 = tile5;
					tile7.liquid -= (byte)num;
				}
				Tile tile8 = tile4;
				tile8.liquid += (byte)num;
				tile4.liquidType((int)tile5.liquidType());
				Liquid.AddWater(this.x, this.y + 1);
				tile4.skipLiquid(true);
				tile5.skipLiquid(true);
				if (Liquid.quickSettle && tile5.liquid > 250)
				{
					tile5.liquid = byte.MaxValue;
				}
				else if (!flag)
				{
					Liquid.AddWater(this.x - 1, this.y);
					Liquid.AddWater(this.x + 1, this.y);
				}
			}
			if (tile5.liquid > 0)
			{
				bool flag2 = true;
				bool flag3 = true;
				bool flag4 = true;
				bool flag5 = true;
				if (tile.nactive() && Main.tileSolid[(int)tile.type] && !Main.tileSolidTop[(int)tile.type])
				{
					flag2 = false;
				}
				else if (tile.liquid > 0 && tile.liquidType() != tile5.liquidType())
				{
					flag2 = false;
				}
				else if (Main.tile[this.x - 2, this.y].nactive() && Main.tileSolid[(int)Main.tile[this.x - 2, this.y].type] && !Main.tileSolidTop[(int)Main.tile[this.x - 2, this.y].type])
				{
					flag4 = false;
				}
				else if (Main.tile[this.x - 2, this.y].liquid == 0)
				{
					flag4 = false;
				}
				else if (Main.tile[this.x - 2, this.y].liquid > 0 && Main.tile[this.x - 2, this.y].liquidType() != tile5.liquidType())
				{
					flag4 = false;
				}
				if (tile2.nactive() && Main.tileSolid[(int)tile2.type] && !Main.tileSolidTop[(int)tile2.type])
				{
					flag3 = false;
				}
				else if (tile2.liquid > 0 && tile2.liquidType() != tile5.liquidType())
				{
					flag3 = false;
				}
				else if (Main.tile[this.x + 2, this.y].nactive() && Main.tileSolid[(int)Main.tile[this.x + 2, this.y].type] && !Main.tileSolidTop[(int)Main.tile[this.x + 2, this.y].type])
				{
					flag5 = false;
				}
				else if (Main.tile[this.x + 2, this.y].liquid == 0)
				{
					flag5 = false;
				}
				else if (Main.tile[this.x + 2, this.y].liquid > 0 && Main.tile[this.x + 2, this.y].liquidType() != tile5.liquidType())
				{
					flag5 = false;
				}
				int num2 = 0;
				if (tile5.liquid < 3)
				{
					num2 = -1;
				}
				if (tile5.liquid > 250)
				{
					flag4 = false;
					flag5 = false;
				}
				if (flag2 && flag3)
				{
					if (flag4 && flag5)
					{
						bool flag6 = true;
						bool flag7 = true;
						if (Main.tile[this.x - 3, this.y].nactive() && Main.tileSolid[(int)Main.tile[this.x - 3, this.y].type] && !Main.tileSolidTop[(int)Main.tile[this.x - 3, this.y].type])
						{
							flag6 = false;
						}
						else if (Main.tile[this.x - 3, this.y].liquid == 0)
						{
							flag6 = false;
						}
						else if (Main.tile[this.x - 3, this.y].liquidType() != tile5.liquidType())
						{
							flag6 = false;
						}
						if (Main.tile[this.x + 3, this.y].nactive() && Main.tileSolid[(int)Main.tile[this.x + 3, this.y].type] && !Main.tileSolidTop[(int)Main.tile[this.x + 3, this.y].type])
						{
							flag7 = false;
						}
						else if (Main.tile[this.x + 3, this.y].liquid == 0)
						{
							flag7 = false;
						}
						else if (Main.tile[this.x + 3, this.y].liquidType() != tile5.liquidType())
						{
							flag7 = false;
						}
						if (flag6 && flag7)
						{
							float num = (float)((int)(tile.liquid + tile2.liquid + Main.tile[this.x - 2, this.y].liquid + Main.tile[this.x + 2, this.y].liquid + Main.tile[this.x - 3, this.y].liquid + Main.tile[this.x + 3, this.y].liquid + tile5.liquid) + num2);
							num = (float)Math.Round((double)(num / 7f));
							int num3 = 0;
							tile.liquidType((int)tile5.liquidType());
							if (tile.liquid != (byte)num)
							{
								tile.liquid = (byte)num;
								Liquid.AddWater(this.x - 1, this.y);
							}
							else
							{
								num3++;
							}
							tile2.liquidType((int)tile5.liquidType());
							if (tile2.liquid != (byte)num)
							{
								tile2.liquid = (byte)num;
								Liquid.AddWater(this.x + 1, this.y);
							}
							else
							{
								num3++;
							}
							Main.tile[this.x - 2, this.y].liquidType((int)tile5.liquidType());
							if (Main.tile[this.x - 2, this.y].liquid != (byte)num)
							{
								Main.tile[this.x - 2, this.y].liquid = (byte)num;
								Liquid.AddWater(this.x - 2, this.y);
							}
							else
							{
								num3++;
							}
							Main.tile[this.x + 2, this.y].liquidType((int)tile5.liquidType());
							if (Main.tile[this.x + 2, this.y].liquid != (byte)num)
							{
								Main.tile[this.x + 2, this.y].liquid = (byte)num;
								Liquid.AddWater(this.x + 2, this.y);
							}
							else
							{
								num3++;
							}
							Main.tile[this.x - 3, this.y].liquidType((int)tile5.liquidType());
							if (Main.tile[this.x - 3, this.y].liquid != (byte)num)
							{
								Main.tile[this.x - 3, this.y].liquid = (byte)num;
								Liquid.AddWater(this.x - 3, this.y);
							}
							else
							{
								num3++;
							}
							Main.tile[this.x + 3, this.y].liquidType((int)tile5.liquidType());
							if (Main.tile[this.x + 3, this.y].liquid != (byte)num)
							{
								Main.tile[this.x + 3, this.y].liquid = (byte)num;
								Liquid.AddWater(this.x + 3, this.y);
							}
							else
							{
								num3++;
							}
							if (tile.liquid != (byte)num || tile5.liquid != (byte)num)
							{
								Liquid.AddWater(this.x - 1, this.y);
							}
							if (tile2.liquid != (byte)num || tile5.liquid != (byte)num)
							{
								Liquid.AddWater(this.x + 1, this.y);
							}
							if (Main.tile[this.x - 2, this.y].liquid != (byte)num || tile5.liquid != (byte)num)
							{
								Liquid.AddWater(this.x - 2, this.y);
							}
							if (Main.tile[this.x + 2, this.y].liquid != (byte)num || tile5.liquid != (byte)num)
							{
								Liquid.AddWater(this.x + 2, this.y);
							}
							if (Main.tile[this.x - 3, this.y].liquid != (byte)num || tile5.liquid != (byte)num)
							{
								Liquid.AddWater(this.x - 3, this.y);
							}
							if (Main.tile[this.x + 3, this.y].liquid != (byte)num || tile5.liquid != (byte)num)
							{
								Liquid.AddWater(this.x + 3, this.y);
							}
							if (num3 != 6 || tile3.liquid <= 0)
							{
								tile5.liquid = (byte)num;
							}
						}
						else
						{
							int num4 = 0;
							float num = (float)((int)(tile.liquid + tile2.liquid + Main.tile[this.x - 2, this.y].liquid + Main.tile[this.x + 2, this.y].liquid + tile5.liquid) + num2);
							num = (float)Math.Round((double)(num / 5f));
							tile.liquidType((int)tile5.liquidType());
							if (tile.liquid != (byte)num)
							{
								tile.liquid = (byte)num;
								Liquid.AddWater(this.x - 1, this.y);
							}
							else
							{
								num4++;
							}
							tile2.liquidType((int)tile5.liquidType());
							if (tile2.liquid != (byte)num)
							{
								tile2.liquid = (byte)num;
								Liquid.AddWater(this.x + 1, this.y);
							}
							else
							{
								num4++;
							}
							Main.tile[this.x - 2, this.y].liquidType((int)tile5.liquidType());
							if (Main.tile[this.x - 2, this.y].liquid != (byte)num)
							{
								Main.tile[this.x - 2, this.y].liquid = (byte)num;
								Liquid.AddWater(this.x - 2, this.y);
							}
							else
							{
								num4++;
							}
							Main.tile[this.x + 2, this.y].liquidType((int)tile5.liquidType());
							if (Main.tile[this.x + 2, this.y].liquid != (byte)num)
							{
								Main.tile[this.x + 2, this.y].liquid = (byte)num;
								Liquid.AddWater(this.x + 2, this.y);
							}
							else
							{
								num4++;
							}
							if (tile.liquid != (byte)num || tile5.liquid != (byte)num)
							{
								Liquid.AddWater(this.x - 1, this.y);
							}
							if (tile2.liquid != (byte)num || tile5.liquid != (byte)num)
							{
								Liquid.AddWater(this.x + 1, this.y);
							}
							if (Main.tile[this.x - 2, this.y].liquid != (byte)num || tile5.liquid != (byte)num)
							{
								Liquid.AddWater(this.x - 2, this.y);
							}
							if (Main.tile[this.x + 2, this.y].liquid != (byte)num || tile5.liquid != (byte)num)
							{
								Liquid.AddWater(this.x + 2, this.y);
							}
							if (num4 != 4 || tile3.liquid <= 0)
							{
								tile5.liquid = (byte)num;
							}
						}
					}
					else if (flag4)
					{
						float num = (float)((int)(tile.liquid + tile2.liquid + Main.tile[this.x - 2, this.y].liquid + tile5.liquid) + num2);
						num = (float)Math.Round((double)(num / 4f));
						tile.liquidType((int)tile5.liquidType());
						if (tile.liquid != (byte)num || tile5.liquid != (byte)num)
						{
							tile.liquid = (byte)num;
							Liquid.AddWater(this.x - 1, this.y);
						}
						tile2.liquidType((int)tile5.liquidType());
						if (tile2.liquid != (byte)num || tile5.liquid != (byte)num)
						{
							tile2.liquid = (byte)num;
							Liquid.AddWater(this.x + 1, this.y);
						}
						Main.tile[this.x - 2, this.y].liquidType((int)tile5.liquidType());
						if (Main.tile[this.x - 2, this.y].liquid != (byte)num || tile5.liquid != (byte)num)
						{
							Main.tile[this.x - 2, this.y].liquid = (byte)num;
							Liquid.AddWater(this.x - 2, this.y);
						}
						tile5.liquid = (byte)num;
					}
					else if (flag5)
					{
						float num = (float)((int)(tile.liquid + tile2.liquid + Main.tile[this.x + 2, this.y].liquid + tile5.liquid) + num2);
						num = (float)Math.Round((double)(num / 4f));
						tile.liquidType((int)tile5.liquidType());
						if (tile.liquid != (byte)num || tile5.liquid != (byte)num)
						{
							tile.liquid = (byte)num;
							Liquid.AddWater(this.x - 1, this.y);
						}
						tile2.liquidType((int)tile5.liquidType());
						if (tile2.liquid != (byte)num || tile5.liquid != (byte)num)
						{
							tile2.liquid = (byte)num;
							Liquid.AddWater(this.x + 1, this.y);
						}
						Main.tile[this.x + 2, this.y].liquidType((int)tile5.liquidType());
						if (Main.tile[this.x + 2, this.y].liquid != (byte)num || tile5.liquid != (byte)num)
						{
							Main.tile[this.x + 2, this.y].liquid = (byte)num;
							Liquid.AddWater(this.x + 2, this.y);
						}
						tile5.liquid = (byte)num;
					}
					else
					{
						float num = (float)((int)(tile.liquid + tile2.liquid + tile5.liquid) + num2);
						num = (float)Math.Round((double)(num / 3f));
						if (num == 254f && WorldGen.genRand.Next(30) == 0)
						{
							num = 255f;
						}
						tile.liquidType((int)tile5.liquidType());
						if (tile.liquid != (byte)num)
						{
							tile.liquid = (byte)num;
							Liquid.AddWater(this.x - 1, this.y);
						}
						tile2.liquidType((int)tile5.liquidType());
						if (tile2.liquid != (byte)num)
						{
							tile2.liquid = (byte)num;
							Liquid.AddWater(this.x + 1, this.y);
						}
						tile5.liquid = (byte)num;
					}
				}
				else if (flag2)
				{
					float num = (float)((int)(tile.liquid + tile5.liquid) + num2);
					num = (float)Math.Round((double)(num / 2f));
					if (tile.liquid != (byte)num)
					{
						tile.liquid = (byte)num;
					}
					tile.liquidType((int)tile5.liquidType());
					if (tile5.liquid != (byte)num || tile.liquid != (byte)num)
					{
						Liquid.AddWater(this.x - 1, this.y);
					}
					tile5.liquid = (byte)num;
				}
				else if (flag3)
				{
					float num = (float)((int)(tile2.liquid + tile5.liquid) + num2);
					num = (float)Math.Round((double)(num / 2f));
					if (tile2.liquid != (byte)num)
					{
						tile2.liquid = (byte)num;
					}
					tile2.liquidType((int)tile5.liquidType());
					if (tile5.liquid != (byte)num || tile2.liquid != (byte)num)
					{
						Liquid.AddWater(this.x + 1, this.y);
					}
					tile5.liquid = (byte)num;
				}
			}
			if (tile5.liquid == liquid)
			{
				this.kill++;
				return;
			}
			if (tile5.liquid != 254 || liquid != 255)
			{
				Liquid.AddWater(this.x, this.y - 1);
				this.kill = 0;
				return;
			}
			if (Liquid.quickSettle)
			{
				tile5.liquid = byte.MaxValue;
				this.kill++;
				return;
			}
			this.kill++;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00020BDC File Offset: 0x0001EDDC
		public static void StartPanic()
		{
			if (!Liquid.panicMode)
			{
				GenVars.waterLine = Main.maxTilesY;
				Liquid.numLiquid = 0;
				LiquidBuffer.numLiquidBuffer = 0;
				Liquid.panicCounter = 0;
				Liquid.panicMode = true;
				Liquid.panicY = Main.maxTilesY - 3;
				if (Main.dedServ)
				{
					Console.WriteLine(Language.GetTextValue("Misc.ForceWaterSettling"));
				}
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00020C34 File Offset: 0x0001EE34
		public static void UpdateLiquid()
		{
			int num = 8;
			Liquid.tilesIgnoreWater(true);
			if (Main.netMode == 2 && !WorldGen.isGeneratingOrLoadingWorld)
			{
				int num2 = 0;
				for (int i = 0; i < 15; i++)
				{
					if (Main.player[i].active)
					{
						num2++;
					}
				}
				Liquid.cycles = 10 + num2 / 3;
				Liquid.curMaxLiquid = Liquid.maxLiquid - num2 * 250;
				num = 10 + num2 / 3;
				if (Main.Setting_UseReducedMaxLiquids)
				{
					Liquid.curMaxLiquid = 5000;
				}
			}
			if (!WorldGen.isGeneratingOrLoadingWorld)
			{
				if (!Liquid.panicMode)
				{
					if ((double)LiquidBuffer.numLiquidBuffer >= 45000.0)
					{
						Liquid.panicCounter++;
						if (Liquid.panicCounter > 3600)
						{
							Liquid.StartPanic();
						}
					}
					else
					{
						Liquid.panicCounter = 0;
					}
				}
				if (Liquid.panicMode)
				{
					int num3 = 0;
					while (Liquid.panicY >= 3 && num3 < 5)
					{
						num3++;
						Liquid.QuickWater(0, Liquid.panicY, Liquid.panicY);
						Liquid.panicY--;
						if (Liquid.panicY < 3)
						{
							Console.WriteLine(Language.GetTextValue("Misc.WaterSettled"));
							Liquid.panicCounter = 0;
							Liquid.panicMode = false;
							WorldGen.WaterCheck();
							if (Main.netMode == 2)
							{
								for (int j = 0; j < 255; j++)
								{
									for (int k = 0; k < Main.maxSectionsX; k++)
									{
										for (int l = 0; l < Main.maxSectionsY; l++)
										{
											Netplay.Clients[j].TileSections[k, l] = false;
										}
									}
								}
							}
						}
					}
					return;
				}
			}
			bool flag = Liquid.quickSettle;
			if (Main.Setting_UseReducedMaxLiquids)
			{
				flag |= (Liquid.numLiquid > 2000);
			}
			if (flag)
			{
				Liquid.quickFall = true;
			}
			else
			{
				Liquid.quickFall = false;
			}
			Liquid.wetCounter++;
			int num4 = Liquid.curMaxLiquid / Liquid.cycles;
			int num5 = num4 * (Liquid.wetCounter - 1);
			int num6 = num4 * Liquid.wetCounter;
			if (Liquid.wetCounter == Liquid.cycles)
			{
				num6 = Liquid.numLiquid;
			}
			if (num6 > Liquid.numLiquid)
			{
				num6 = Liquid.numLiquid;
				int netMode = Main.netMode;
				Liquid.wetCounter = Liquid.cycles;
			}
			if (Liquid.quickFall)
			{
				for (int m = num5; m < num6; m++)
				{
					Main.liquid[m].delay = 10;
					Main.liquid[m].Update();
					Main.tile[Main.liquid[m].x, Main.liquid[m].y].skipLiquid(false);
				}
			}
			else
			{
				for (int n = num5; n < num6; n++)
				{
					if (!Main.tile[Main.liquid[n].x, Main.liquid[n].y].skipLiquid())
					{
						Main.liquid[n].Update();
					}
					else
					{
						Main.tile[Main.liquid[n].x, Main.liquid[n].y].skipLiquid(false);
					}
				}
			}
			if (Liquid.wetCounter >= Liquid.cycles)
			{
				Liquid.wetCounter = 0;
				for (int num7 = Liquid.numLiquid - 1; num7 >= 0; num7--)
				{
					if (Main.liquid[num7].kill >= num)
					{
						if (Main.tile[Main.liquid[num7].x, Main.liquid[num7].y].liquid == 254)
						{
							Main.tile[Main.liquid[num7].x, Main.liquid[num7].y].liquid = byte.MaxValue;
						}
						Liquid.DelWater(num7);
					}
				}
				int num8 = Liquid.curMaxLiquid - (Liquid.curMaxLiquid - Liquid.numLiquid);
				if (num8 > LiquidBuffer.numLiquidBuffer)
				{
					num8 = LiquidBuffer.numLiquidBuffer;
				}
				for (int num9 = 0; num9 < num8; num9++)
				{
					Main.tile[Main.liquidBuffer[0].x, Main.liquidBuffer[0].y].checkingLiquid(false);
					Liquid.AddWater(Main.liquidBuffer[0].x, Main.liquidBuffer[0].y);
					LiquidBuffer.DelBuffer(0);
				}
				if (Liquid.numLiquid > 0 && Liquid.numLiquid > Liquid.stuckAmount - 50 && Liquid.numLiquid < Liquid.stuckAmount + 50)
				{
					Liquid.stuckCount++;
					if (Liquid.stuckCount >= 10000)
					{
						Liquid.stuck = true;
						for (int num10 = Liquid.numLiquid - 1; num10 >= 0; num10--)
						{
							Liquid.DelWater(num10);
						}
						Liquid.stuck = false;
						Liquid.stuckCount = 0;
					}
				}
				else
				{
					Liquid.stuckCount = 0;
					Liquid.stuckAmount = Liquid.numLiquid;
				}
			}
			if (!WorldGen.isGeneratingOrLoadingWorld && Main.netMode == 2 && Liquid._netChangeSet.Count > 0)
			{
				Utils.Swap<HashSet<int>>(ref Liquid._netChangeSet, ref Liquid._swapNetChangeSet);
				NetLiquidModule.CreateAndBroadcastByChunk(Liquid._swapNetChangeSet);
				Liquid._swapNetChangeSet.Clear();
			}
			Liquid.tilesIgnoreWater(false);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0002110C File Offset: 0x0001F30C
		public static void AddWater(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			if (Main.tile[x, y] == null)
			{
				return;
			}
			if (tile.checkingLiquid())
			{
				return;
			}
			if (x >= Main.maxTilesX - 5 || y >= Main.maxTilesY - 5)
			{
				return;
			}
			if (x < 5 || y < 5)
			{
				return;
			}
			if (tile.liquid == 0)
			{
				return;
			}
			if (tile.nactive() && Main.tileSolid[(int)tile.type] && tile.type != 546 && !Main.tileSolidTop[(int)tile.type])
			{
				return;
			}
			if (Liquid.numLiquid >= Liquid.curMaxLiquid - 1)
			{
				LiquidBuffer.AddBuffer(x, y);
				return;
			}
			tile.checkingLiquid(true);
			tile.skipLiquid(false);
			Main.liquid[Liquid.numLiquid].kill = 0;
			Main.liquid[Liquid.numLiquid].x = x;
			Main.liquid[Liquid.numLiquid].y = y;
			Main.liquid[Liquid.numLiquid].delay = 0;
			Liquid.numLiquid++;
			if (Main.netMode == 2)
			{
				Liquid.NetSendLiquid(x, y);
			}
			if (tile.active() && !WorldGen.isGeneratingOrLoadingWorld)
			{
				bool flag = false;
				if (tile.lava())
				{
					if (TileObjectData.CheckLavaDeath(tile))
					{
						flag = true;
					}
				}
				else if (TileObjectData.CheckWaterDeath(tile))
				{
					flag = true;
				}
				if (flag)
				{
					WorldGen.KillTile(x, y, false, false, false);
					if (Main.netMode == 2)
					{
						NetMessage.SendData(17, -1, -1, null, 0, (float)x, (float)y, 0f, 0, 0, 0);
					}
				}
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00021278 File Offset: 0x0001F478
		private static bool UndergroundDesertCheck(int x, int y)
		{
			int num = 3;
			for (int i = x - num; i <= x + num; i++)
			{
				for (int j = y - num; j <= y + num; j++)
				{
					if (WorldGen.InWorld(i, j, 0) && (Main.tile[i, j].wall == 187 || Main.tile[i, j].wall == 216))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x000212E4 File Offset: 0x0001F4E4
		public static void LiquidCheck(int x, int y, int thisLiquidType, bool createMergeTilesDuringGen = false)
		{
			if (!WorldGen.InWorld(x, y, 1))
			{
				return;
			}
			if (WorldGen.SolidTile(x, y, false))
			{
				return;
			}
			Tile tile = Main.tile[x - 1, y];
			Tile tile2 = Main.tile[x + 1, y];
			Tile tile3 = Main.tile[x, y - 1];
			Tile tile4 = Main.tile[x, y + 1];
			Tile tile5 = Main.tile[x, y];
			if ((tile.liquid > 0 && (int)tile.liquidType() != thisLiquidType) || (tile2.liquid > 0 && (int)tile2.liquidType() != thisLiquidType) || (tile3.liquid > 0 && (int)tile3.liquidType() != thisLiquidType))
			{
				bool waterNearby = tile.anyWater() || tile2.anyWater() || tile3.anyWater();
				bool lavaNearby = tile.anyLava() || tile2.anyLava() || tile3.anyLava();
				bool honeyNearby = tile.anyHoney() || tile2.anyHoney() || tile3.anyHoney();
				bool shimmerNearby = tile.anyShimmer() || tile2.anyShimmer() || tile3.anyShimmer();
				int num = 0;
				if ((int)tile.liquidType() != thisLiquidType)
				{
					num += (int)tile.liquid;
					tile.liquid = 0;
				}
				if ((int)tile2.liquidType() != thisLiquidType)
				{
					num += (int)tile2.liquid;
					tile2.liquid = 0;
				}
				if ((int)tile3.liquidType() != thisLiquidType)
				{
					num += (int)tile3.liquid;
					tile3.liquid = 0;
				}
				int liquidMergeTileType = 56;
				int num2 = 0;
				Liquid.GetLiquidMergeTypes(thisLiquidType, out liquidMergeTileType, out num2, waterNearby, lavaNearby, honeyNearby, shimmerNearby);
				if (num >= 24 && num2 != thisLiquidType && (!tile5.active() || Main.tileObsidianKill[(int)tile5.type]))
				{
					tile5.liquid = 0;
					Liquid.CreateLiquidMergeTile(x, y, thisLiquidType, num2, liquidMergeTileType, createMergeTilesDuringGen);
					return;
				}
			}
			else if (tile4.liquid > 0 && (int)tile4.liquidType() != thisLiquidType)
			{
				bool flag = false;
				if (tile5.active() && TileID.Sets.IsAContainer[(int)tile5.type] && !TileID.Sets.IsAContainer[(int)tile4.type])
				{
					flag = true;
				}
				if (thisLiquidType != 0 && Main.tileCut[(int)tile4.type])
				{
					WorldGen.KillTile(x, y + 1, false, false, false);
					if (Main.netMode == 2)
					{
						NetMessage.SendData(17, -1, -1, null, 0, (float)x, (float)(y + 1), 0f, 0, 0, 0);
					}
				}
				if (!tile4.active() || Main.tileObsidianKill[(int)tile4.type] || flag)
				{
					if (tile5.liquid < 24)
					{
						tile5.liquid = 0;
						tile5.liquidType(0);
						if (Main.netMode == 2)
						{
							NetMessage.SendTileSquare(-1, x - 1, y, 3, TileChangeType.None);
							return;
						}
					}
					else
					{
						int liquidMergeTileType2 = 56;
						int liquidMergeType = 0;
						bool waterNearby2 = tile4.water();
						bool lavaNearby2 = tile4.lava();
						bool honeyNearby2 = tile4.honey();
						bool shimmerNearby2 = tile4.shimmer();
						Liquid.GetLiquidMergeTypes(thisLiquidType, out liquidMergeTileType2, out liquidMergeType, waterNearby2, lavaNearby2, honeyNearby2, shimmerNearby2);
						tile5.liquid = 0;
						Liquid.CreateLiquidMergeTile(x, y + 1, thisLiquidType, liquidMergeType, liquidMergeTileType2, createMergeTilesDuringGen);
					}
				}
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x000215CC File Offset: 0x0001F7CC
		private static void LiquidOverwriteStrip(int x, int y, int liquidType)
		{
			int num = x;
			while (num >= 0 && Main.tile[num, y].liquid > 0)
			{
				Main.tile[num, y].liquidType(liquidType);
				num--;
			}
			int num2 = x;
			while (num2 < Main.maxTilesX && Main.tile[num2, y].liquid > 0)
			{
				Main.tile[num2, y].liquidType(liquidType);
				num2++;
			}
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00021644 File Offset: 0x0001F844
		private static void CreateLiquidMergeTile(int x, int y, int thisLiquidType, int liquidMergeType, int liquidMergeTileType, bool createMergeTilesDuringGen)
		{
			Tile tile = Main.tile[x, y];
			TileChangeType liquidChangeType = WorldGen.GetLiquidChangeType(thisLiquidType, liquidMergeType);
			if (WorldGen.isGeneratingOrLoadingWorld && !createMergeTilesDuringGen)
			{
				int liquidType = -1;
				if (liquidMergeTileType == 229)
				{
					liquidType = 0;
				}
				else if (liquidMergeTileType == 230)
				{
					liquidType = 1;
				}
				else if (liquidMergeTileType == 56)
				{
					liquidType = 1;
				}
				else if (liquidMergeTileType == 659)
				{
					liquidType = 3;
				}
				if (y >= Main.UnderworldLayer)
				{
					liquidType = 1;
				}
				Liquid.LiquidOverwriteStrip(x, y, liquidType);
				return;
			}
			if (!Main.gameMenu && !WorldGen.isGeneratingOrLoadingWorld)
			{
				WorldGen.PlayLiquidChangeSound(liquidChangeType, x, y, 1);
			}
			bool flag = true;
			Utils.Swap<bool>(ref flag, ref Main.tileSolid[546]);
			if (!tile.active() || !WorldGen.ReplaceTile(x, y, liquidMergeTileType, 0))
			{
				WorldGen.KillTile(x, y, false, false, false);
				WorldGen.PlaceTile(x, y, liquidMergeTileType, true, true, -1, 0);
			}
			WorldGen.SquareTileFrame(x, y, true);
			Main.tileSolid[546] = flag;
			if (Main.netMode == 2)
			{
				NetMessage.SendTileSquare(-1, x - 1, y - 1, 3, liquidChangeType);
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00021740 File Offset: 0x0001F940
		public static void GetLiquidMergeTypes(int thisLiquidType, out int liquidMergeTileType, out int liquidMergeType, bool waterNearby, bool lavaNearby, bool honeyNearby, bool shimmerNearby)
		{
			liquidMergeTileType = 56;
			liquidMergeType = thisLiquidType;
			if (thisLiquidType != 0 && waterNearby)
			{
				switch (thisLiquidType)
				{
				case 1:
					liquidMergeTileType = 56;
					break;
				case 2:
					liquidMergeTileType = 229;
					break;
				case 3:
					liquidMergeTileType = 659;
					break;
				}
				liquidMergeType = 0;
			}
			if (thisLiquidType != 1 && lavaNearby)
			{
				switch (thisLiquidType)
				{
				case 0:
					liquidMergeTileType = 56;
					break;
				case 2:
					liquidMergeTileType = 230;
					break;
				case 3:
					liquidMergeTileType = 659;
					break;
				}
				liquidMergeType = 1;
			}
			if (thisLiquidType != 2 && honeyNearby)
			{
				switch (thisLiquidType)
				{
				case 0:
					liquidMergeTileType = 229;
					break;
				case 1:
					liquidMergeTileType = 230;
					break;
				case 3:
					liquidMergeTileType = 659;
					break;
				}
				liquidMergeType = 2;
			}
			if (thisLiquidType != 3 && shimmerNearby)
			{
				switch (thisLiquidType)
				{
				case 0:
					liquidMergeTileType = 659;
					break;
				case 1:
					liquidMergeTileType = 659;
					break;
				case 2:
					liquidMergeTileType = 659;
					break;
				}
				liquidMergeType = 3;
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00021844 File Offset: 0x0001FA44
		public static void LavaCheck(int x, int y)
		{
			if (!WorldGen.remixWorldGen && !Main.dualDungeonsSeed && WorldGen.generatingWorld && Liquid.UndergroundDesertCheck(x, y))
			{
				for (int i = x - 3; i <= x + 3; i++)
				{
					for (int j = y - 3; j <= y + 3; j++)
					{
						Main.tile[i, j].lava(true);
					}
				}
			}
			Liquid.LiquidCheck(x, y, 1, false);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x000218AA File Offset: 0x0001FAAA
		public static void HoneyCheck(int x, int y)
		{
			Liquid.LiquidCheck(x, y, 2, false);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x000218B5 File Offset: 0x0001FAB5
		public static void ShimmerCheck(int x, int y)
		{
			Liquid.LiquidCheck(x, y, 3, false);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x000218C0 File Offset: 0x0001FAC0
		public static void DelWater(int l)
		{
			int num = Main.liquid[l].x;
			int num2 = Main.liquid[l].y;
			Tile tile = Main.tile[num - 1, num2];
			Tile tile2 = Main.tile[num + 1, num2];
			Tile tile3 = Main.tile[num, num2 + 1];
			Tile tile4 = Main.tile[num, num2];
			byte b = 2;
			if (tile4.liquid < b)
			{
				tile4.liquid = 0;
				if (tile.liquid < b)
				{
					tile.liquid = 0;
				}
				else
				{
					Liquid.AddWater(num - 1, num2);
				}
				if (tile2.liquid < b)
				{
					tile2.liquid = 0;
				}
				else
				{
					Liquid.AddWater(num + 1, num2);
				}
			}
			else if (tile4.liquid < 20)
			{
				if ((tile.liquid < tile4.liquid && (!tile.nactive() || !Main.tileSolid[(int)tile.type] || Main.tileSolidTop[(int)tile.type])) || (tile2.liquid < tile4.liquid && (!tile2.nactive() || !Main.tileSolid[(int)tile2.type] || Main.tileSolidTop[(int)tile2.type])) || (tile3.liquid < 255 && (!tile3.nactive() || !Main.tileSolid[(int)tile3.type] || Main.tileSolidTop[(int)tile3.type])))
				{
					tile4.liquid = 0;
				}
			}
			else if (tile3.liquid < 255 && (!tile3.nactive() || !Main.tileSolid[(int)tile3.type] || Main.tileSolidTop[(int)tile3.type]) && !Liquid.stuck && (!Main.tile[num, num2].nactive() || !Main.tileSolid[(int)Main.tile[num, num2].type] || Main.tileSolidTop[(int)Main.tile[num, num2].type]))
			{
				Main.liquid[l].kill = 0;
				return;
			}
			if (tile4.liquid < 250 && Main.tile[num, num2 - 1].liquid > 0)
			{
				Liquid.AddWater(num, num2 - 1);
			}
			if (tile4.liquid == 0)
			{
				tile4.liquidType(0);
			}
			else
			{
				if (tile2.liquid > 0 && tile2.liquid < 250 && (!tile2.nactive() || !Main.tileSolid[(int)tile2.type] || Main.tileSolidTop[(int)tile2.type]) && tile4.liquid != tile2.liquid)
				{
					Liquid.AddWater(num + 1, num2);
				}
				if (tile.liquid > 0 && tile.liquid < 250 && (!tile.nactive() || !Main.tileSolid[(int)tile.type] || Main.tileSolidTop[(int)tile.type]) && tile4.liquid != tile.liquid)
				{
					Liquid.AddWater(num - 1, num2);
				}
				if (tile4.lava())
				{
					Liquid.LavaCheck(num, num2);
					for (int i = num - 1; i <= num + 1; i++)
					{
						for (int j = num2 - 1; j <= num2 + 1; j++)
						{
							Tile tile5 = Main.tile[i, j];
							if (tile5.active())
							{
								if (tile5.type == 2 || tile5.type == 23 || tile5.type == 109 || tile5.type == 199 || tile5.type == 477 || tile5.type == 492)
								{
									tile5.type = 0;
									WorldGen.SquareTileFrame(i, j, true);
									if (Main.netMode == 2)
									{
										NetMessage.SendTileSquare(-1, num, num2, 3, TileChangeType.None);
									}
								}
								else if (tile5.type == 60 || tile5.type == 70 || tile5.type == 661 || tile5.type == 662)
								{
									tile5.type = 59;
									WorldGen.SquareTileFrame(i, j, true);
									if (Main.netMode == 2)
									{
										NetMessage.SendTileSquare(-1, num, num2, 3, TileChangeType.None);
									}
								}
							}
						}
					}
				}
				else if (tile4.honey())
				{
					Liquid.HoneyCheck(num, num2);
				}
				else if (tile4.shimmer())
				{
					Liquid.ShimmerCheck(num, num2);
				}
			}
			if (Main.netMode == 2)
			{
				Liquid.NetSendLiquid(num, num2);
			}
			Liquid.numLiquid--;
			Main.tile[Main.liquid[l].x, Main.liquid[l].y].checkingLiquid(false);
			Main.liquid[l].x = Main.liquid[Liquid.numLiquid].x;
			Main.liquid[l].y = Main.liquid[Liquid.numLiquid].y;
			Main.liquid[l].kill = Main.liquid[Liquid.numLiquid].kill;
			if (Main.tileAlch[(int)tile4.type])
			{
				WorldGen.CheckAlch(num, num2);
				return;
			}
			if (tile4.type == 518)
			{
				if (Liquid.quickFall)
				{
					WorldGen.CheckLilyPad(num, num2);
					return;
				}
				if (Main.tile[num, num2 + 1].liquid < 255 || Main.tile[num, num2 - 1].liquid > 0)
				{
					WorldGen.SquareTileFrame(num, num2, true);
					return;
				}
				WorldGen.CheckLilyPad(num, num2);
			}
		}

		// Token: 0x0400016F RID: 367
		public const int maxLiquidBuffer = 50000;

		// Token: 0x04000170 RID: 368
		public static int maxLiquid = 25000;

		// Token: 0x04000171 RID: 369
		public static int skipCount;

		// Token: 0x04000172 RID: 370
		public static int stuckCount;

		// Token: 0x04000173 RID: 371
		public static int stuckAmount;

		// Token: 0x04000174 RID: 372
		public static int cycles = 10;

		// Token: 0x04000175 RID: 373
		public static int curMaxLiquid = 0;

		// Token: 0x04000176 RID: 374
		public static int numLiquid;

		// Token: 0x04000177 RID: 375
		public static bool stuck;

		// Token: 0x04000178 RID: 376
		public static bool quickFall;

		// Token: 0x04000179 RID: 377
		public static bool quickSettle;

		// Token: 0x0400017A RID: 378
		private static int wetCounter;

		// Token: 0x0400017B RID: 379
		public static int panicCounter;

		// Token: 0x0400017C RID: 380
		public static bool panicMode;

		// Token: 0x0400017D RID: 381
		public static int panicY;

		// Token: 0x0400017E RID: 382
		public int x;

		// Token: 0x0400017F RID: 383
		public int y;

		// Token: 0x04000180 RID: 384
		public int kill;

		// Token: 0x04000181 RID: 385
		public int delay;

		// Token: 0x04000182 RID: 386
		private static HashSet<int> _netChangeSet = new HashSet<int>();

		// Token: 0x04000183 RID: 387
		private static HashSet<int> _swapNetChangeSet = new HashSet<int>();
	}
}
