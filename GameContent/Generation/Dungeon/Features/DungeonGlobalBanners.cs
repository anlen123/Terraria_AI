using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004DD RID: 1245
	public class DungeonGlobalBanners : GlobalDungeonFeature
	{
		// Token: 0x060034EC RID: 13548 RVA: 0x0060A332 File Offset: 0x00608532
		public DungeonGlobalBanners(DungeonFeatureSettings settings) : base(settings)
		{
			DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x0060E4D4 File Offset: 0x0060C6D4
		public override bool GenerateFeature(DungeonData data)
		{
			this.generated = false;
			this.Banners(data);
			this.generated = true;
			return true;
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x0060E4EC File Offset: 0x0060C6EC
		public void Banners(DungeonData data)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			float num = (float)Main.maxTilesX / 4200f;
			double num2 = Math.Max(1.0, data.globalFeatureScalar * 0.75);
			int num3 = (int)((double)(200f * num) * num2);
			for (int i = 0; i < num3; i++)
			{
				int num4 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
				int num5 = genRand.Next(data.dungeonBounds.Top, data.dungeonBounds.Bottom);
				int num6 = 1000;
				while (!DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num4, num5].wall, false) || Main.tile[num4, num5].active())
				{
					num6--;
					if (num6 <= 0)
					{
						break;
					}
					num4 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
					num5 = genRand.Next(data.dungeonBounds.Top, data.dungeonBounds.Bottom);
				}
				num6 = 1000;
				while (!WorldGen.SolidTile(num4, num5, false) && num5 > 10)
				{
					num6--;
					if (num6 <= 0)
					{
						break;
					}
					num5--;
				}
				num5++;
				if (data.CanGenerateFeatureAt(this, num4, num5) && DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num4, num5].wall, false) && Main.tile[num4, num5 - 1].type != 48 && !Main.tile[num4, num5].active() && !Main.tile[num4, num5 + 1].active() && !Main.tile[num4, num5 + 2].active() && !Main.tile[num4, num5 + 3].active())
				{
					bool flag = true;
					for (int j = num4 - 1; j <= num4 + 1; j++)
					{
						for (int k = num5; k <= num5 + 3; k++)
						{
							if (Main.tile[j, k].active() && (Main.tile[j, k].type == 10 || Main.tile[j, k].type == 11 || Main.tile[j, k].type == 91))
							{
								flag = false;
							}
						}
					}
					if (flag)
					{
						ushort type = 91;
						DungeonGenerationStyleData styleForWall = DungeonGenerationStyles.GetStyleForWall(data.genVars.dungeonGenerationStyles, (int)Main.tile[num4, num5].wall);
						if (styleForWall == null || styleForWall.BannerItemTypes != null)
						{
							int style;
							if (styleForWall == null || styleForWall.Style == 0 || styleForWall.BannerItemTypes.Length == 0)
							{
								int num7 = 0;
								if ((int)Main.tile[num4, num5].wall == data.wallVariants[1])
								{
									num7 = 1;
								}
								if ((int)Main.tile[num4, num5].wall == data.wallVariants[2])
								{
									num7 = 2;
								}
								num7 *= 2;
								num7 += genRand.Next(2);
								style = data.bannerStyles[num7];
							}
							else
							{
								PlacementDetails placementDetails = ItemID.Sets.DerivedPlacementDetails[styleForWall.BannerItemTypes[genRand.Next(styleForWall.BannerItemTypes.Length)]];
								type = (ushort)placementDetails.tileType;
								style = (int)placementDetails.tileStyle;
							}
							WorldGen.PlaceTile(num4, num5, (int)type, true, false, -1, style);
						}
					}
				}
			}
		}
	}
}
