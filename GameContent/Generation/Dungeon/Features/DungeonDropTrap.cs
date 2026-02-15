using System;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004D7 RID: 1239
	public class DungeonDropTrap : DungeonFeature
	{
		// Token: 0x060034D4 RID: 13524 RVA: 0x00608988 File Offset: 0x00606B88
		public DungeonDropTrap(DungeonFeatureSettings settings, bool addToFeatures = true) : base(settings)
		{
			if (addToFeatures)
			{
				DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
			}
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x0060A3AE File Offset: 0x006085AE
		public override bool GenerateFeature(DungeonData data, int x, int y)
		{
			this.generated = false;
			if (this.DropTrap(data, x, y))
			{
				this.generated = true;
				return true;
			}
			return false;
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public override bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
		{
			return false;
		}

		// Token: 0x060034D7 RID: 13527 RVA: 0x0060A3CC File Offset: 0x006085CC
		public bool DropTrap(DungeonData data, int i, int j)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			DungeonDropTrapSettings dungeonDropTrapSettings = (DungeonDropTrapSettings)this.settings;
			bool flag = dungeonDropTrapSettings.StyleData.Style == 0;
			ushort num = flag ? data.genVars.brickTileType : dungeonDropTrapSettings.StyleData.BrickTileType;
			ushort num2 = flag ? data.genVars.brickWallType : dungeonDropTrapSettings.StyleData.BrickWallType;
			ushort num3 = num;
			int num4 = -1;
			int num5 = -1;
			switch (dungeonDropTrapSettings.DropTrapType)
			{
			default:
				num4 = 53;
				num3 = 396;
				break;
			case DungeonDropTrapType.Silt:
				num4 = 123;
				break;
			case DungeonDropTrapType.Slush:
				num4 = 224;
				num3 = 147;
				break;
			case DungeonDropTrapType.Lava:
				num5 = 1;
				break;
			}
			int num6 = 6;
			int num7 = 4;
			int num8 = 25;
			int num9 = j;
			if (!WorldGen.InWorld(i, num9, num8))
			{
				return false;
			}
			while (!Main.tile[i, num9].active() && num9 < Main.UnderworldLayer)
			{
				num9++;
			}
			if (!WorldGen.InWorld(i, num9, num8))
			{
				return false;
			}
			if (Main.tile[i, num9 - 1].active())
			{
				return false;
			}
			if (!Main.tileSolid[(int)Main.tile[i, num9].type] || Main.tile[i, num9].halfBrick() || Main.tile[i, num9].topSlope())
			{
				return false;
			}
			if (((int)Main.tile[i, num9].type != num4 && Main.tile[i, num9].type != num3 && Main.tile[i, num9].type != num) || Main.tile[i, num9].wall != num2)
			{
				return false;
			}
			num9--;
			Main.tile[i, num9];
			int num10 = -1;
			int num11 = genRand.Next(6, 12);
			int num12 = genRand.Next(6, 14);
			for (int k = i - num8; k <= i + num8; k++)
			{
				for (int l = num9 - num8; l < num9 + num8; l++)
				{
					Tile tile = Main.tile[k, l];
					if (tile.wire())
					{
						return false;
					}
					if (TileID.Sets.BasicChest[(int)tile.type])
					{
						return false;
					}
				}
			}
			int m = num9;
			while (m > num9 - 30)
			{
				if (Main.tile[i, m].active())
				{
					if (Main.tile[i, m].type == num)
					{
						num10 = m;
						break;
					}
					return false;
				}
				else
				{
					m--;
				}
			}
			if (num10 <= -1)
			{
				return false;
			}
			if (num9 - num10 < num12 + num7)
			{
				return false;
			}
			int num13 = 0;
			int num14 = (num9 + num10) / 2;
			for (int n = i - num11; n <= i + num11; n++)
			{
				for (int num15 = num10 - num12; num15 <= num10; num15++)
				{
					Tile tile2 = Main.tile[n, num15];
					if (tile2.active() && Main.tileSolid[(int)tile2.type])
					{
						num13++;
					}
				}
			}
			double num16 = (double)((num11 * 2 + 1) * (num12 + 1)) * 0.75;
			if ((double)num13 < num16)
			{
				return false;
			}
			this.Bounds.SetBounds(i - num11 - 1, num10 - num12, i + num11 + 1, num10 + 20);
			this.Bounds.CalculateHitbox();
			if (!data.CanGenerateFeatureInArea(this, this.Bounds))
			{
				return false;
			}
			this.Bounds = new DungeonBounds();
			for (int num17 = i - num11 - 1; num17 <= i + num11 + 1; num17++)
			{
				for (int num18 = num10 - num12; num18 <= num10; num18++)
				{
					bool flag2 = false;
					if (Main.tile[num17, num18].active() && Main.tileSolid[(int)Main.tile[num17, num18].type])
					{
						flag2 = true;
					}
					if (num18 == num10)
					{
						Main.tile[num17, num18].slope(0);
						Main.tile[num17, num18].halfBrick(false);
						if (!flag2)
						{
							Main.tile[num17, num18].active(true);
							Main.tile[num17, num18].type = num;
						}
					}
					else if (num18 == num10 - num12)
					{
						Main.tile[num17, num18].ClearTile();
						Main.tile[num17, num18].active(true);
						if (flag2 && Main.tile[num17, num18 - 1].active() && Main.tileSolid[(int)Main.tile[num17, num18 - 1].type])
						{
							Main.tile[num17, num18].type = num3;
						}
						else
						{
							Main.tile[num17, num18].type = num;
						}
					}
					else if (num17 == i - num11 - 1 || num17 == i + num11 + 1)
					{
						if (!flag2)
						{
							Main.tile[num17, num18].ClearTile();
							Main.tile[num17, num18].active(true);
							Main.tile[num17, num18].type = num;
						}
						else
						{
							Main.tile[num17, num18].slope(0);
							Main.tile[num17, num18].halfBrick(false);
						}
					}
					else
					{
						Main.tile[num17, num18].ClearTile();
						if (num5 > -1)
						{
							Main.tile[num17, num18].liquid = byte.MaxValue;
							Main.tile[num17, num18].liquidType(num5);
						}
						else
						{
							Main.tile[num17, num18].active(true);
							Main.tile[num17, num18].type = (ushort)num4;
						}
					}
				}
			}
			int num19 = (int)((double)num10 - (double)num12 * 0.6600000262260437);
			while ((double)num19 <= (double)num10 - (double)num12 * 0.33000001311302185)
			{
				if ((double)num19 < (double)num10 - (double)num12 * 0.4000000059604645)
				{
					if (Main.tile[i - num11 - 2, num19].bottomSlope())
					{
						Main.tile[i - num11 - 2, num19].slope(0);
					}
				}
				else if ((double)num19 > (double)num10 - (double)num12 * 0.6000000238418579)
				{
					if (Main.tile[i - num11 - 2, num19].topSlope())
					{
						Main.tile[i - num11 - 2, num19].slope(0);
					}
					Main.tile[i - num11 - 2, num19].halfBrick(false);
				}
				else
				{
					Main.tile[i - num11 - 2, num19].halfBrick(false);
					Main.tile[i - num11 - 2, num19].slope(0);
				}
				if (!Main.tile[i - num11 - 2, num19].active() || !Main.tileSolid[(int)Main.tile[i - num11 - 2, num19].type])
				{
					Main.tile[i - num11 - 2, num19].active(true);
					Main.tile[i - num11 - 2, num19].type = num;
				}
				if (!Main.tile[i + num11 + 2, num19].active() || !Main.tileSolid[(int)Main.tile[i + num11 + 2, num19].type])
				{
					Main.tile[i + num11 + 2, num19].active(true);
					Main.tile[i + num11 + 2, num19].type = num;
				}
				num19++;
			}
			for (int num20 = num10 - num12; num20 <= num10; num20++)
			{
				Main.tile[i - num11 - 2, num20].slope(0);
				Main.tile[i - num11 - 2, num20].halfBrick(false);
				Main.tile[i - num11 - 1, num20].slope(0);
				Main.tile[i - num11 - 1, num20].halfBrick(false);
				Main.tile[i - num11 + 1, num20].slope(0);
				Main.tile[i - num11 + 1, num20].halfBrick(false);
				Main.tile[i - num11 + 2, num20].slope(0);
				Main.tile[i - num11 + 2, num20].halfBrick(false);
			}
			for (int num21 = i - num11 - 1; num21 < i + num11 + 1; num21++)
			{
				int num22 = num9 - num12 - 1;
				if (Main.tile[num21, num22].bottomSlope())
				{
					Main.tile[num21, num22].slope(0);
				}
				Main.tile[num21, num22].halfBrick(false);
			}
			WorldGen.KillTile(i - 2, num9, false, false, false);
			WorldGen.KillTile(i - 1, num9, false, false, false);
			WorldGen.KillTile(i + 1, num9, false, false, false);
			WorldGen.KillTile(i + 2, num9, false, false, false);
			WorldGen.PlaceTile(i, num9, 135, true, false, -1, 7);
			for (int num23 = i - num11; num23 <= i + num11; num23++)
			{
				int num24 = num9;
				if ((float)num23 < (float)i - (float)num11 * 0.8f || (float)num23 > (float)i + (float)num11 * 0.8f)
				{
					num24 = num9 - 3;
				}
				else if ((float)num23 < (float)i - (float)num11 * 0.6f || (float)num23 > (float)i + (float)num11 * 0.6f)
				{
					num24 = num9 - 2;
				}
				else if ((float)num23 < (float)i - (float)num11 * 0.4f || (float)num23 > (float)i + (float)num11 * 0.4f)
				{
					num24 = num9 - 1;
				}
				for (int num25 = num10; num25 <= num9; num25++)
				{
					if (num23 == i && num25 <= num9)
					{
						Main.tile[i, num25].wire(true);
					}
					if (Main.tile[num23, num25].active() && Main.tileSolid[(int)Main.tile[num23, num25].type])
					{
						if (num25 < num10 + num6 - 4)
						{
							Main.tile[num23, num25].actuator(true);
							Main.tile[num23, num25].wire(true);
						}
						else if (num25 < num24)
						{
							WorldGen.KillTile(num23, num25, false, false, false);
						}
					}
				}
			}
			int num26 = num9;
			for (int num27 = i - num11; num27 <= i + num11; num27++)
			{
				for (int num28 = num26; num28 < Main.UnderworldLayer; num28++)
				{
					if (num28 >= num26)
					{
						Tile tile3 = Main.tile[num27, num28];
						if (tile3.active() && !TileID.Sets.Platforms[(int)tile3.type] && Main.tileSolid[(int)tile3.type])
						{
							num26 = num28;
							break;
						}
					}
				}
			}
			this.Bounds.SetBounds(i - num11 - 1, num10 - num12, i + num11 + 1, num26);
			this.Bounds.CalculateHitbox();
			return true;
		}
	}
}
