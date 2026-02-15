using System;
using Terraria.DataStructures;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004DC RID: 1244
	public class DungeonGlobalSpikes : GlobalDungeonFeature
	{
		// Token: 0x060034E8 RID: 13544 RVA: 0x0060A332 File Offset: 0x00608532
		public DungeonGlobalSpikes(DungeonFeatureSettings settings) : base(settings)
		{
			DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x0060D9A7 File Offset: 0x0060BBA7
		public override bool GenerateFeature(DungeonData data)
		{
			this.generated = false;
			this.Spikes(data);
			this.generated = true;
			return true;
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x0060D9C0 File Offset: 0x0060BBC0
		public void Spikes(DungeonData data)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			int num = data.wallVariants[0];
			float num2 = (float)Main.maxTilesX / 4200f;
			int num3 = 0;
			int num4 = 1000;
			int i = 0;
			double num5 = Math.Max(1.0, data.globalFeatureScalar * 0.25);
			int num6 = (int)((double)(42f * num2) * num5);
			if (WorldGen.getGoodWorldGen)
			{
				num6 *= 3;
			}
			while (i < num6)
			{
				num3++;
				int num7 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
				int num8 = genRand.Next((int)Main.worldSurface + 25, data.dungeonBounds.Bottom);
				if (WorldGen.drunkWorldGen || WorldGen.SecretSeed.noSurface.Enabled)
				{
					num8 = genRand.Next(data.genVars.generatingDungeonPositionY + 25, data.dungeonBounds.Bottom);
				}
				int num9 = num7;
				ushort type = 48;
				bool flag = true;
				bool flag2 = (int)Main.tile[num7, num8].wall == num;
				if (data.Type == DungeonType.DualDungeon)
				{
					flag2 = DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num7, num8].wall, false);
					if (Main.tile[num7, num8].wall == 87)
					{
						type = 232;
						flag = false;
					}
				}
				if (flag2 && !Main.tile[num7, num8].active())
				{
					int num10 = 1;
					if (genRand.Next(2) == 0)
					{
						num10 = -1;
					}
					while (!Main.tile[num7, num8].active())
					{
						num8 += num10;
					}
					if (Main.tile[num7 - 1, num8].active() && Main.tile[num7 + 1, num8].active() && this.Spikes_CanSupportSpike(num7 - 1, num8) && !Main.tile[num7 - 1, num8 - num10].active() && !Main.tile[num7 + 1, num8 - num10].active())
					{
						i++;
						int num11 = genRand.Next(5, 13);
						while (Main.tile[num7 - 1, num8].active() && this.Spikes_CanSupportSpike(num7 - 1, num8) && Main.tile[num7, num8 + num10].active() && Main.tile[num7, num8].active() && !Main.tile[num7, num8 - num10].active() && num11 > 0)
						{
							if (!data.CanGenerateFeatureAt(this, num7, num8) || !data.CanGenerateFeatureAt(this, num7, num8 - num10))
							{
								num7--;
								num11 = 0;
							}
							else
							{
								Main.tile[num7, num8].type = type;
								if (!Main.tile[num7 - 1, num8 - num10].active() && !Main.tile[num7 + 1, num8 - num10].active())
								{
									Main.tile[num7, num8 - num10].Clear(TileDataType.Slope);
									Main.tile[num7, num8 - num10].type = type;
									Main.tile[num7, num8 - num10].active(true);
									if (flag)
									{
										Main.tile[num7, num8 - num10 * 2].Clear(TileDataType.Slope);
										Main.tile[num7, num8 - num10 * 2].type = type;
										Main.tile[num7, num8 - num10 * 2].active(true);
									}
								}
								num7--;
								num11--;
							}
						}
						num11 = genRand.Next(5, 13);
						num7 = num9 + 1;
						while (Main.tile[num7 + 1, num8].active() && this.Spikes_CanSupportSpike(num7 + 1, num8) && Main.tile[num7, num8 + num10].active() && Main.tile[num7, num8].active() && !Main.tile[num7, num8 - num10].active() && num11 > 0)
						{
							if (!data.CanGenerateFeatureAt(this, num7, num8) || !data.CanGenerateFeatureAt(this, num7, num8 - num10))
							{
								num7++;
								num11 = 0;
							}
							else
							{
								Main.tile[num7, num8].type = type;
								if (!Main.tile[num7 - 1, num8 - num10].active() && !Main.tile[num7 + 1, num8 - num10].active())
								{
									Main.tile[num7, num8 - num10].Clear(TileDataType.Slope);
									Main.tile[num7, num8 - num10].type = type;
									Main.tile[num7, num8 - num10].active(true);
									if (flag)
									{
										Main.tile[num7, num8 - num10 * 2].Clear(TileDataType.Slope);
										Main.tile[num7, num8 - num10 * 2].type = type;
										Main.tile[num7, num8 - num10 * 2].active(true);
									}
								}
								num7++;
								num11--;
							}
						}
					}
				}
				if (num3 > num4)
				{
					num3 = 0;
					i++;
				}
			}
			num3 = 0;
			num4 = 1000;
			i = 0;
			while (i < num6)
			{
				num3++;
				int num12 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
				int num13 = genRand.Next((int)Main.worldSurface + 25, data.dungeonBounds.Bottom);
				if (WorldGen.SecretSeed.noSurface.Enabled)
				{
					num13 = genRand.Next(data.genVars.generatingDungeonPositionY + 25, data.dungeonBounds.Bottom);
				}
				int num14 = num13;
				ushort type2 = 48;
				bool flag3 = true;
				bool flag4 = (int)Main.tile[num12, num13].wall == num;
				if (data.Type == DungeonType.DualDungeon)
				{
					flag4 = DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num12, num13].wall, false);
					if (Main.tile[num12, num13].wall == 87)
					{
						type2 = 232;
						flag3 = false;
					}
				}
				if (flag4 && !Main.tile[num12, num13].active())
				{
					int num15 = 1;
					if (genRand.Next(2) == 0)
					{
						num15 = -1;
					}
					while (num12 > 5 && num12 < Main.maxTilesX - 5 && !Main.tile[num12, num13].active())
					{
						num12 += num15;
					}
					if (Main.tile[num12, num13 - 1].active() && Main.tile[num12, num13 + 1].active() && this.Spikes_CanSupportSpike(num12, num13 - 1) && !Main.tile[num12 - num15, num13 - 1].active() && !Main.tile[num12 - num15, num13 + 1].active())
					{
						i++;
						int num16 = genRand.Next(5, 13);
						while (Main.tile[num12, num13 - 1].active() && this.Spikes_CanSupportSpike(num12, num13 - 1) && Main.tile[num12 + num15, num13].active() && Main.tile[num12, num13].active() && !Main.tile[num12 - num15, num13].active() && num16 > 0)
						{
							if (!data.CanGenerateFeatureAt(this, num12, num13) || !data.CanGenerateFeatureAt(this, num12 - num15, num13))
							{
								num13--;
								num16 = 0;
							}
							else
							{
								Main.tile[num12, num13].type = type2;
								if (!Main.tile[num12 - num15, num13 - 1].active() && !Main.tile[num12 - num15, num13 + 1].active())
								{
									Main.tile[num12 - num15, num13].type = type2;
									Main.tile[num12 - num15, num13].active(true);
									Main.tile[num12 - num15, num13].Clear(TileDataType.Slope);
									if (flag3)
									{
										Main.tile[num12 - num15 * 2, num13].type = type2;
										Main.tile[num12 - num15 * 2, num13].active(true);
										Main.tile[num12 - num15 * 2, num13].Clear(TileDataType.Slope);
									}
								}
								num13--;
								num16--;
							}
						}
						num16 = genRand.Next(5, 13);
						num13 = num14 + 1;
						while (Main.tile[num12, num13 + 1].active() && this.Spikes_CanSupportSpike(num12, num13 + 1) && Main.tile[num12 + num15, num13].active() && Main.tile[num12, num13].active() && !Main.tile[num12 - num15, num13].active() && num16 > 0)
						{
							if (!data.CanGenerateFeatureAt(this, num12, num13) || !data.CanGenerateFeatureAt(this, num12 - num15, num13))
							{
								num13++;
								num16 = 0;
							}
							else
							{
								Main.tile[num12, num13].type = type2;
								if (!Main.tile[num12 - num15, num13 - 1].active() && !Main.tile[num12 - num15, num13 + 1].active())
								{
									Main.tile[num12 - num15, num13].type = type2;
									Main.tile[num12 - num15, num13].active(true);
									Main.tile[num12 - num15, num13].Clear(TileDataType.Slope);
									if (flag3)
									{
										Main.tile[num12 - num15 * 2, num13].type = type2;
										Main.tile[num12 - num15 * 2, num13].active(true);
										Main.tile[num12 - num15 * 2, num13].Clear(TileDataType.Slope);
									}
								}
								num13++;
								num16--;
							}
						}
					}
				}
				if (num3 > num4)
				{
					num3 = 0;
					i++;
				}
			}
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x0060E478 File Offset: 0x0060C678
		private bool Spikes_CanSupportSpike(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			return tile.active() && (tile.type < 0 || (!Main.tileFrameImportant[(int)tile.type] && !Main.tileCut[(int)tile.type])) && !DungeonUtils.IsConsideredCrackedDungeonTile((int)tile.type, false);
		}
	}
}
