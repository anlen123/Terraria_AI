using System;
using Terraria.DataStructures;
using Terraria.GameContent.Generation.Dungeon.Entrances;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004E2 RID: 1250
	public class DungeonGlobalBookshelves : GlobalDungeonFeature
	{
		// Token: 0x06003504 RID: 13572 RVA: 0x0060A332 File Offset: 0x00608532
		public DungeonGlobalBookshelves(DungeonFeatureSettings settings) : base(settings)
		{
			DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x00611B98 File Offset: 0x0060FD98
		public override bool GenerateFeature(DungeonData data)
		{
			this.generated = false;
			this.Bookshelves(data);
			this.generated = true;
			return true;
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x00611BB0 File Offset: 0x0060FDB0
		public void Bookshelves(DungeonData data)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			DungeonEntrance dungeonEntrance = data.dungeonEntrance;
			int num = 0;
			int num2 = 1000;
			int i = 0;
			int num3 = -1;
			if (data.Type == DungeonType.DualDungeon)
			{
				switch (WorldGen.GetWorldSize())
				{
				default:
					num3 = 5;
					break;
				case 1:
					num3 = 10;
					break;
				case 2:
					num3 = 15;
					break;
				}
			}
			while (i < Main.maxTilesX / 20)
			{
				num++;
				int num4 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
				int num5 = genRand.Next(data.dungeonBounds.Top, data.dungeonBounds.Bottom);
				bool flag = true;
				if (DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num4, num5].wall, false) && !Main.tile[num4, num5].active())
				{
					int num6 = 1;
					if (genRand.Next(2) == 0)
					{
						num6 = -1;
					}
					while (flag && !Main.tile[num4, num5].active())
					{
						num4 -= num6;
						if (num4 < 5 || num4 > Main.maxTilesX - 5)
						{
							flag = false;
						}
						else if (Main.tile[num4, num5].active() && !DungeonUtils.IsConsideredDungeonTile((int)Main.tile[num4, num5].type, false))
						{
							flag = false;
						}
					}
					if (flag && Main.tile[num4, num5].active() && DungeonUtils.IsConsideredDungeonTile((int)Main.tile[num4, num5].type, false) && Main.tile[num4, num5 - 1].active() && DungeonUtils.IsConsideredDungeonTile((int)Main.tile[num4, num5 - 1].type, false) && Main.tile[num4, num5 + 1].active() && DungeonUtils.IsConsideredDungeonTile((int)Main.tile[num4, num5 + 1].type, false))
					{
						num4 += num6;
						for (int j = num4 - 3; j <= num4 + 3; j++)
						{
							for (int k = num5 - 3; k <= num5 + 3; k++)
							{
								if (Main.tile[j, k].active() && Main.tile[j, k].type == 19)
								{
									flag = false;
									break;
								}
							}
						}
						if (flag && (!Main.tile[num4, num5 - 1].active() & !Main.tile[num4, num5 - 2].active() & !Main.tile[num4, num5 - 3].active()))
						{
							if (!data.CanGenerateFeatureAt(this, num4, num5))
							{
								continue;
							}
							int num7 = num4;
							int num8 = num4;
							while (num7 > data.dungeonBounds.Left && num7 < data.dungeonBounds.Right && !Main.tile[num7, num5].active() && !Main.tile[num7, num5 - 1].active() && !Main.tile[num7, num5 + 1].active())
							{
								num7 += num6;
							}
							num7 = Math.Abs(num4 - num7);
							bool flag2 = true;
							bool flag3 = genRand.Next(2) == 0;
							if (num7 > 5)
							{
								int num9 = -1;
								int num10 = 1;
								int num11 = 4;
								DungeonGenerationStyleData styleForWall = DungeonGenerationStyles.GetStyleForWall(data.genVars.dungeonGenerationStyles, (int)Main.tile[num4, num5].wall);
								if (styleForWall != null)
								{
									flag2 = (styleForWall.Style == 0);
									if (!flag2)
									{
										flag3 = false;
									}
									num9 = styleForWall.GetPlatformStyle(genRand);
									styleForWall.GetBookshelfMinMaxSizes(num10, num11, out num10, out num11);
								}
								for (int l = genRand.Next(num10, num11); l > 0; l--)
								{
									Tile tile = Main.tile[num4, num5];
									tile.active(true);
									tile.Clear(TileDataType.Slope);
									tile.type = 19;
									int num12 = data.shelfStyles[0];
									if ((int)tile.wall == data.wallVariants[1])
									{
										num12 = data.shelfStyles[1];
									}
									if ((int)tile.wall == data.wallVariants[2])
									{
										num12 = data.shelfStyles[2];
									}
									if (num9 > -1)
									{
										num12 = num9;
									}
									tile.frameY = (short)(18 * num12);
									WorldGen.TileFrame(num4, num5, false, false);
									if (flag3)
									{
										short frameX = 90;
										WorldGen.PlaceTile(num4, num5 - 1, 50, true, false, -1, 0);
										if (genRand.Next(50) == 0 && (double)num5 > (Main.worldSurface + Main.rockLayer) / 2.0 && Main.tile[num4, num5 - 1].type == 50)
										{
											Main.tile[num4, num5 - 1].frameX = frameX;
										}
									}
									num4 += num6;
								}
								num = 0;
								i++;
								if (!flag3 && genRand.Next(2) == 0)
								{
									num4 = num8;
									num5--;
									if (flag2)
									{
										int num13 = (genRand.Next(4) == 0) ? 1 : 0;
										if (num3 > 0)
										{
											num13 = 1;
										}
										if (num13 == 0)
										{
											num13 = 13;
										}
										else if (num13 == 1)
										{
											num13 = 49;
										}
										WorldGen.PlaceTile(num4, num5, num13, true, false, -1, 0);
										if (Main.tile[num4, num5].type == 13)
										{
											if (genRand.Next(2) == 0)
											{
												Main.tile[num4, num5].frameX = 18;
											}
											else
											{
												Main.tile[num4, num5].frameX = 36;
											}
										}
										if (Main.tile[num4, num5].active() && Main.tile[num4, num5].type == 49)
										{
											num3--;
										}
									}
									else
									{
										ushort type = 13;
										WorldGen.PlaceTile(num4, num5, (int)type, true, false, -1, 0);
										if (Main.tile[num4, num5].type == 13)
										{
											if (genRand.Next(2) == 0)
											{
												Main.tile[num4, num5].frameX = 18;
											}
											else
											{
												Main.tile[num4, num5].frameX = 36;
											}
										}
									}
								}
							}
						}
					}
				}
				if (num > num2)
				{
					num = 0;
					i++;
				}
			}
		}
	}
}
