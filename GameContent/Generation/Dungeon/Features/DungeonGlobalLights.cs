using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004E1 RID: 1249
	public class DungeonGlobalLights : GlobalDungeonFeature
	{
		// Token: 0x06003500 RID: 13568 RVA: 0x0060A332 File Offset: 0x00608532
		public DungeonGlobalLights(DungeonFeatureSettings settings) : base(settings)
		{
			DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x006114AA File Offset: 0x0060F6AA
		public override bool GenerateFeature(DungeonData data)
		{
			this.generated = false;
			this.Lights(data);
			this.generated = true;
			return true;
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x006114C4 File Offset: 0x0060F6C4
		public void Lights(DungeonData data)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			float num = (float)Main.maxTilesX / 4200f;
			int num2 = 0;
			int num3 = 1000;
			int i = 0;
			int num4 = (int)((double)(28f * num) * data.globalFeatureScalar);
			while (i < num4)
			{
				num2++;
				int num5 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
				int num6 = genRand.Next(data.dungeonBounds.Top, data.dungeonBounds.Bottom);
				if (DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num5, num6].wall, false))
				{
					for (int j = num6; j > data.dungeonBounds.Top; j--)
					{
						if (Main.tile[num5, j - 1].active() && DungeonUtils.IsConsideredDungeonTile((int)Main.tile[num5, j - 1].type, false) && data.CanGenerateFeatureAt(this, num5, j) && (data.dungeonEntrance.Bounds.Contains(num5, j) || DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num5, j].wall, false)))
						{
							bool flag = false;
							for (int k = num5 - 15; k < num5 + 15; k++)
							{
								for (int l = j - 15; l < j + 15; l++)
								{
									if (k > 0 && k < Main.maxTilesX && l > 0 && l < Main.maxTilesY && (Main.tile[k, l].type == 42 || Main.tile[k, l].type == 34))
									{
										flag = true;
										break;
									}
								}
							}
							if (Main.tile[num5 - 1, j].active() || Main.tile[num5 + 1, j].active() || Main.tile[num5 - 1, j + 1].active() || Main.tile[num5 + 1, j + 1].active() || Main.tile[num5, j + 2].active())
							{
								flag = true;
							}
							if (flag)
							{
								break;
							}
							bool flag2 = false;
							if (!flag2 && genRand.Next(7) == 0)
							{
								bool flag3 = false;
								for (int m = 0; m < 15; m++)
								{
									if (WorldGen.SolidTile(num5, j + m, false))
									{
										flag3 = true;
										break;
									}
								}
								if (!flag3)
								{
									DungeonGenerationStyleData styleForWall = DungeonGenerationStyles.GetStyleForWall(data.genVars.dungeonGenerationStyles, (int)Main.tile[num5, j].wall);
									if (styleForWall != null && styleForWall.ChandelierItemTypes != null)
									{
										int num7 = (styleForWall.ChandelierItemTypes.Length == 0 || styleForWall.Style == 0) ? data.chandelierItemType : styleForWall.ChandelierItemTypes[genRand.Next(styleForWall.ChandelierItemTypes.Length)];
										PlacementDetails placementDetails = ItemID.Sets.DerivedPlacementDetails[num7];
										if (placementDetails.tileType >= 0)
										{
											WorldGen.PlaceChand(num5, j, (ushort)placementDetails.tileType, (int)placementDetails.tileStyle);
											if (Main.tile[num5, j].type == 34)
											{
												flag2 = true;
												num2 = 0;
												i++;
												this.Lights_GenerateSwitch(num5, j);
											}
										}
									}
								}
							}
							if (flag2)
							{
								break;
							}
							DungeonGenerationStyleData styleForWall2 = DungeonGenerationStyles.GetStyleForWall(data.genVars.dungeonGenerationStyles, (int)Main.tile[num5, j].wall);
							ushort num8 = 42;
							if (styleForWall2 == null || styleForWall2.LanternItemTypes != null)
							{
								int style;
								if (styleForWall2 == null || styleForWall2.Style == 0 || styleForWall2.LanternItemTypes.Length == 0)
								{
									style = data.lanternStyles[0];
									if ((int)Main.tile[num5, j].wall == data.wallVariants[1])
									{
										style = data.lanternStyles[1];
									}
									if ((int)Main.tile[num5, j].wall == data.wallVariants[2])
									{
										style = data.lanternStyles[2];
									}
								}
								else
								{
									PlacementDetails placementDetails2 = ItemID.Sets.DerivedPlacementDetails[styleForWall2.LanternItemTypes[genRand.Next(styleForWall2.LanternItemTypes.Length)]];
									num8 = (ushort)placementDetails2.tileType;
									style = (int)placementDetails2.tileStyle;
								}
								WorldGen.Place1x2Top(num5, j, num8, style);
								if (Main.tile[num5, j].type == num8)
								{
									num2 = 0;
									i++;
									this.Lights_GenerateSwitch(num5, j);
									break;
								}
								break;
							}
						}
					}
				}
				if (num2 > num3)
				{
					i++;
					num2 = 0;
				}
			}
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x0061194C File Offset: 0x0060FB4C
		private Point Lights_GenerateSwitch(int x, int y)
		{
			Point zero = Point.Zero;
			for (int i = 0; i < 1000; i++)
			{
				int num = x + WorldGen.genRand.Next(-12, 13);
				int num2 = y + WorldGen.genRand.Next(3, 21);
				if (!Main.tile[num, num2].active() && !Main.tile[num, num2 + 1].active() && DungeonUtils.IsConsideredDungeonTile((int)Main.tile[num - 1, num2].type, false) && DungeonUtils.IsConsideredDungeonTile((int)Main.tile[num + 1, num2].type, false) && Collision.CanHit(new Point(num * 16, num2 * 16), 16, 16, new Point(x * 16, y * 16 + 1), 16, 16))
				{
					if (((WorldGen.SolidTile(num - 1, num2, false) && Main.tile[num - 1, num2].type != 10) || (WorldGen.SolidTile(num + 1, num2, false) && Main.tile[num + 1, num2].type != 10) || WorldGen.SolidTile(num, num2 + 1, false)) && DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num, num2].wall, false) && (DungeonUtils.IsConsideredDungeonTile((int)Main.tile[num - 1, num2].type, false) || DungeonUtils.IsConsideredDungeonTile((int)Main.tile[num + 1, num2].type, false)))
					{
						WorldGen.PlaceTile(num, num2, 136, true, false, -1, 0);
						zero = new Point(num, num2);
					}
					if (Main.tile[num, num2].active())
					{
						while (num != x || num2 != y)
						{
							Main.tile[num, num2].wire(true);
							if (num > x)
							{
								num--;
							}
							if (num < x)
							{
								num++;
							}
							Main.tile[num, num2].wire(true);
							if (num2 > y)
							{
								num2--;
							}
							if (num2 < y)
							{
								num2++;
							}
							Main.tile[num, num2].wire(true);
						}
						if (WorldGen.genRand.Next(3) > 0)
						{
							Main.tile[x, y].frameX = 18;
							Main.tile[x, y + 1].frameX = 18;
							break;
						}
						break;
					}
				}
			}
			return zero;
		}
	}
}
