using System;
using Terraria.GameContent.Generation.Dungeon.Halls;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004E0 RID: 1248
	public class DungeonGlobalTraps : GlobalDungeonFeature
	{
		// Token: 0x060034FD RID: 13565 RVA: 0x0060A332 File Offset: 0x00608532
		public DungeonGlobalTraps(DungeonFeatureSettings settings) : base(settings)
		{
			DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
		}

		// Token: 0x060034FE RID: 13566 RVA: 0x00611073 File Offset: 0x0060F273
		public override bool GenerateFeature(DungeonData data)
		{
			this.generated = false;
			this.Traps(data);
			this.generated = true;
			return true;
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x0061108C File Offset: 0x0060F28C
		public void Traps(DungeonData data)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			float num = (float)Main.maxTilesX / 4200f;
			int num2 = 0;
			int num3 = 1000;
			int i = 0;
			int num4 = (int)((double)(8.4f * num) * data.globalFeatureScalar);
			Main.tileSolid[379] = false;
			while (i < num4)
			{
				num2++;
				int num5 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
				int num6 = genRand.Next((int)Main.worldSurface, data.dungeonBounds.Bottom);
				if (data.Type == DungeonType.DualDungeon)
				{
					if (DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num5, num6].wall, false) && data.CanGenerateFeatureAt(this, num5, num6) && !DungeonGenerationStyles.Temple.WallIsInStyle((int)Main.tile[num5, num6].wall, false) && WorldGen.placeTrap(num5, num6, 0))
					{
						num2 = num3;
					}
				}
				else if (DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num5, num6].wall, false) && WorldGen.placeTrap(num5, num6, 0))
				{
					num2 = num3;
				}
				if (num2 > num3)
				{
					i++;
					num2 = 0;
				}
			}
			if (data.Type == DungeonType.DualDungeon)
			{
				int num7 = 30;
				switch (WorldGen.GetWorldSize())
				{
				case 0:
					num7 = 30 + genRand.Next(11);
					break;
				case 1:
					num7 = 50 + genRand.Next(16);
					break;
				case 2:
					num7 = 70 + genRand.Next(21);
					break;
				}
				if (WorldGen.SecretSeed.Variations.actuallyNoTrapsForRealIMeanIt)
				{
					num7 = 0;
				}
				if (num7 > 0)
				{
					int j = num7;
					for (int k = 0; k < data.dungeonHalls.Count; k++)
					{
						DungeonHall dungeonHall = data.dungeonHalls[k];
						if (dungeonHall.generated && dungeonHall.settings.StyleData.Style == 10)
						{
							DungeonBounds bounds = dungeonHall.Bounds;
							int num8 = bounds.Left + genRand.Next(bounds.Width);
							int num9 = bounds.Top + genRand.Next(bounds.Height);
							Tile tile = Main.tile[num8, num9];
							if (!tile.active() && DungeonGenerationStyles.Temple.WallIsInStyle((int)tile.wall, false) && WorldGen.mayanTrap(num8, num9))
							{
								j--;
								if (j <= 0)
								{
									break;
								}
							}
						}
					}
					if (j > 0)
					{
						for (int l = 0; l < data.dungeonRooms.Count; l++)
						{
							DungeonRoom dungeonRoom = data.dungeonRooms[l];
							if (dungeonRoom.generated && dungeonRoom.settings.StyleData.Style == 10)
							{
								DungeonBounds innerBounds = dungeonRoom.InnerBounds;
								int num10 = innerBounds.Left + genRand.Next(innerBounds.Width);
								int num11 = innerBounds.Top + genRand.Next(innerBounds.Height);
								Tile tile2 = Main.tile[num10, num11];
								if (!tile2.active() && DungeonGenerationStyles.Temple.WallIsInStyle((int)tile2.wall, false) && WorldGen.mayanTrap(num10, num11))
								{
									j--;
									if (j <= 0)
									{
										break;
									}
								}
							}
						}
					}
					if (j > 0)
					{
						for (int m = 0; m < data.genVars.dungeonGenerationStyles.Count; m++)
						{
							DungeonGenerationStyleData dungeonGenerationStyleData = data.genVars.dungeonGenerationStyles[m];
							int style = (int)dungeonGenerationStyleData.Style;
							DungeonBounds dungeonBounds = data.outerProgressionBounds[m];
							if (style == 10)
							{
								int num12 = 1000;
								while (j > 0)
								{
									num12--;
									if (num12 <= 0)
									{
										break;
									}
									int num13 = dungeonBounds.Left + genRand.Next(dungeonBounds.Width);
									int num14 = dungeonBounds.Top + genRand.Next(dungeonBounds.Height);
									Tile tile3 = Main.tile[num13, num14];
									if (!tile3.active() && dungeonGenerationStyleData.WallIsInStyle((int)tile3.wall, false) && WorldGen.mayanTrap(num13, num14))
									{
										j--;
									}
								}
								break;
							}
						}
					}
				}
			}
			Main.tileSolid[379] = true;
		}
	}
}
