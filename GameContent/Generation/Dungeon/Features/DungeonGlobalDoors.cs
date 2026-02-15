using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004D8 RID: 1240
	public class DungeonGlobalDoors : GlobalDungeonFeature
	{
		// Token: 0x060034D8 RID: 13528 RVA: 0x0060A332 File Offset: 0x00608532
		public DungeonGlobalDoors(DungeonFeatureSettings settings) : base(settings)
		{
			DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x0060AF28 File Offset: 0x00609128
		public override bool GenerateFeature(DungeonData data)
		{
			this.generated = false;
			this.Doors(data);
			this.generated = true;
			return true;
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x0060AF40 File Offset: 0x00609140
		public void Doors(DungeonData data)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			ushort brickTileType = data.genVars.brickTileType;
			ushort brickWallType = data.genVars.brickWallType;
			PlacementDetails placementDetails = ItemID.Sets.DerivedPlacementDetails[data.doorItemType];
			for (int i = 0; i < data.dungeonDoorData.Count; i++)
			{
				DungeonDoorData dungeonDoorData = data.dungeonDoorData[i];
				if (WorldGen.InWorld(dungeonDoorData.Position, 30))
				{
					ushort num = brickTileType;
					if (dungeonDoorData.OverrideBrickTileType != null)
					{
						num = dungeonDoorData.OverrideBrickTileType.Value;
					}
					ushort wall = brickWallType;
					if (dungeonDoorData.OverrideBrickWallType != null)
					{
						wall = dungeonDoorData.OverrideBrickWallType.Value;
					}
					int style = 13;
					if (genRand.Next(3) == 0)
					{
						style = (int)placementDetails.tileStyle;
					}
					if (dungeonDoorData.OverrideStyle != null)
					{
						style = dungeonDoorData.OverrideStyle.Value;
					}
					int num2 = 20;
					int num3 = num2 + 5;
					int num4 = 10;
					if (dungeonDoorData.OverrideWidthFluff != null)
					{
						num4 = dungeonDoorData.OverrideWidthFluff.Value;
					}
					int num5 = Math.Max(num3, Math.Min(Main.maxTilesX - num3, dungeonDoorData.Position.X - num4));
					int num6 = Math.Max(num3, Math.Min(Main.maxTilesX - num3, Math.Max(num5, dungeonDoorData.Position.X + num4 - 1)));
					int num7 = 100;
					int num8 = 0;
					for (int j = num5; j <= num6; j++)
					{
						bool flag = true;
						int num9 = dungeonDoorData.Position.Y;
						while (num9 > 10 && !Main.tile[j, num9].active())
						{
							num9--;
						}
						if (!DungeonUtils.IsConsideredDungeonTile((int)Main.tile[j, num9].type, false))
						{
							flag = false;
						}
						int num10 = num9;
						num9 = dungeonDoorData.Position.Y;
						while (!Main.tile[j, num9].active())
						{
							num9++;
						}
						if (!DungeonUtils.IsConsideredDungeonTile((int)Main.tile[j, num9].type, false))
						{
							flag = false;
						}
						int num11 = num9;
						if (num11 - num10 >= 3)
						{
							if (!dungeonDoorData.SkipOtherDoorsCheck)
							{
								int num12 = j - 20;
								int num13 = j + 20;
								int num14 = num11 - 10;
								int num15 = num11 + 10;
								for (int k = num12; k < num13; k++)
								{
									for (int l = num14; l < num15; l++)
									{
										if (Main.tile[k, l].active() && Main.tile[k, l].type == 10)
										{
											flag = false;
											break;
										}
									}
								}
							}
							if (flag && !dungeonDoorData.SkipSpaceCheck)
							{
								for (int m = num11 - 3; m < num11; m++)
								{
									for (int n = j - 3; n <= j + 3; n++)
									{
										if (Main.tile[n, m].active())
										{
											flag = false;
											break;
										}
									}
								}
							}
							if (flag && num11 - num10 < num2)
							{
								bool flag2 = false;
								if (dungeonDoorData.Direction == 0 && num11 - num10 < num7)
								{
									flag2 = true;
								}
								if (dungeonDoorData.Direction == -1 && j > num8)
								{
									flag2 = true;
								}
								if (dungeonDoorData.Direction == 1 && (j < num8 || num8 == 0))
								{
									flag2 = true;
								}
								if (flag2)
								{
									num8 = j;
									num7 = num11 - num10;
								}
							}
						}
					}
					if (num7 < num2)
					{
						int num16 = num8;
						int num17 = dungeonDoorData.Position.Y;
						int num18 = num17;
						while (!Main.tile[num16, num17].active())
						{
							Main.tile[num16, num17].active(false);
							num17++;
						}
						while (!Main.tile[num16, num18].active())
						{
							num18--;
						}
						num17--;
						num18++;
						for (int num19 = num18; num19 < num17 - 2; num19++)
						{
							Main.tile[num16, num19].Clear(TileDataType.Slope);
							Main.tile[num16, num19].active(true);
							Main.tile[num16, num19].type = num;
							if (Main.tile[num16 - 1, num19].active() && WorldGen.CanKillTile(num16 - 1, num19))
							{
								Main.tile[num16 - 1, num19].active(false);
								Main.tile[num16 - 1, num19].ClearEverything();
								Main.tile[num16 - 1, num19].wall = wall;
							}
							if (Main.tile[num16 - 2, num19].active() && WorldGen.CanKillTile(num16 - 2, num19))
							{
								Main.tile[num16 - 2, num19].active(false);
								Main.tile[num16 - 2, num19].ClearEverything();
								Main.tile[num16 - 2, num19].wall = wall;
							}
							if (Main.tile[num16 + 1, num19].active() && WorldGen.CanKillTile(num16 + 1, num19))
							{
								Main.tile[num16 + 1, num19].active(false);
								Main.tile[num16 + 1, num19].ClearEverything();
								Main.tile[num16 + 1, num19].wall = wall;
							}
							if (Main.tile[num16 + 2, num19].active() && WorldGen.CanKillTile(num16 + 2, num19))
							{
								Main.tile[num16 + 2, num19].active(false);
								Main.tile[num16 + 2, num19].ClearEverything();
								Main.tile[num16 + 2, num19].wall = wall;
							}
						}
						WorldGen.PlaceTile(num16, num17, 10, true, false, -1, style);
						num16--;
						int num20 = num17 - 3;
						while (!Main.tile[num16, num20].active())
						{
							num20--;
						}
						bool flag3 = num17 - num20 < num17 - num18 + 5 && DungeonUtils.IsConsideredDungeonTile((int)Main.tile[num16, num20].type, false);
						if (dungeonDoorData.AlwaysClearArea || flag3)
						{
							for (int num21 = num17 - 4 - genRand.Next(3); num21 > num20; num21--)
							{
								if (flag3)
								{
									Main.tile[num16, num21].Clear(TileDataType.Slope);
									Main.tile[num16, num21].active(true);
									Main.tile[num16, num21].type = num;
								}
								if (dungeonDoorData.AlwaysClearArea || Main.tile[num16 - 1, num21].type == num)
								{
									Main.tile[num16 - 1, num21].active(false);
									Main.tile[num16 - 1, num21].ClearEverything();
									Main.tile[num16 - 1, num21].wall = wall;
								}
								if (dungeonDoorData.AlwaysClearArea || Main.tile[num16 - 2, num21].type == num)
								{
									Main.tile[num16 - 2, num21].active(false);
									Main.tile[num16 - 2, num21].ClearEverything();
									Main.tile[num16 - 2, num21].wall = wall;
								}
							}
						}
						num16 += 2;
						num20 = num17 - 3;
						while (!Main.tile[num16, num20].active())
						{
							num20--;
						}
						flag3 = (num17 - num20 < num17 - num18 + 5 && DungeonUtils.IsConsideredDungeonTile((int)Main.tile[num16, num20].type, false));
						if (dungeonDoorData.AlwaysClearArea || flag3)
						{
							for (int num22 = num17 - 4 - genRand.Next(3); num22 > num20; num22--)
							{
								if (flag3)
								{
									Main.tile[num16, num22].active(true);
									Main.tile[num16, num22].Clear(TileDataType.Slope);
									Main.tile[num16, num22].type = num;
								}
								if (dungeonDoorData.AlwaysClearArea || Main.tile[num16 + 1, num22].type == num)
								{
									Main.tile[num16 + 1, num22].active(false);
									Main.tile[num16 + 1, num22].ClearEverything();
									Main.tile[num16 + 1, num22].wall = wall;
								}
								if (dungeonDoorData.AlwaysClearArea || Main.tile[num16 + 2, num22].type == num)
								{
									Main.tile[num16 + 2, num22].active(false);
									Main.tile[num16 + 2, num22].ClearEverything();
									Main.tile[num16 + 2, num22].wall = wall;
								}
							}
						}
						num17++;
						num16--;
						for (int num23 = num17 - 8; num23 < num17; num23++)
						{
							if (dungeonDoorData.AlwaysClearArea || Main.tile[num16 + 2, num23].type == num)
							{
								Main.tile[num16 + 2, num23].active(false);
								Main.tile[num16 + 2, num23].ClearEverything();
								Main.tile[num16 + 2, num23].wall = wall;
							}
							if (dungeonDoorData.AlwaysClearArea || Main.tile[num16 + 3, num23].type == num)
							{
								Main.tile[num16 + 3, num23].active(false);
								Main.tile[num16 + 3, num23].ClearEverything();
								Main.tile[num16 + 3, num23].wall = wall;
							}
							if (dungeonDoorData.AlwaysClearArea || Main.tile[num16 - 2, num23].type == num)
							{
								Main.tile[num16 - 2, num23].active(false);
								Main.tile[num16 - 2, num23].ClearEverything();
								Main.tile[num16 - 2, num23].wall = wall;
							}
							if (dungeonDoorData.AlwaysClearArea || Main.tile[num16 - 3, num23].type == num)
							{
								Main.tile[num16 - 3, num23].active(false);
								Main.tile[num16 - 3, num23].ClearEverything();
								Main.tile[num16 - 3, num23].wall = wall;
							}
						}
						Main.tile[num16 - 1, num17].active(true);
						Main.tile[num16 - 1, num17].type = num;
						Main.tile[num16 - 1, num17].Clear(TileDataType.Slope);
						Main.tile[num16 + 1, num17].active(true);
						Main.tile[num16 + 1, num17].type = num;
						Main.tile[num16 + 1, num17].Clear(TileDataType.Slope);
					}
				}
			}
		}
	}
}
