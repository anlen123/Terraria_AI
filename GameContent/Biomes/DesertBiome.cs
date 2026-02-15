using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.GameContent.Biomes.Desert;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	// Token: 0x0200050E RID: 1294
	public class DesertBiome : MicroBiome
	{
		// Token: 0x06003641 RID: 13889 RVA: 0x00623EB4 File Offset: 0x006220B4
		public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
		{
			DesertDescription desertDescription = DesertDescription.CreateFromPlacement(origin);
			if (!desertDescription.IsValid)
			{
				return false;
			}
			DesertBiome.ExportDescriptionToEngine(desertDescription);
			SandMound.Place(desertDescription, progress, 0f, 0.1f);
			desertDescription.UpdateSurfaceMap();
			if (!Main.tenthAnniversaryWorld && GenBase._random.NextDouble() <= this.ChanceOfEntrance && !WorldGen.SecretSeed.extraLiquid.Enabled)
			{
				switch (GenBase._random.Next(4))
				{
				case 0:
					ChambersEntrance.Place(desertDescription, progress, 0.1f, 0.2f);
					break;
				case 1:
					AnthillEntrance.Place(desertDescription, progress, 0.1f, 0.2f);
					break;
				case 2:
					LarvaHoleEntrance.Place(desertDescription, progress, 0.1f, 0.2f);
					break;
				case 3:
					PitEntrance.Place(desertDescription, progress, 0.1f, 0.2f);
					break;
				}
			}
			DesertHive.Place(desertDescription, progress, 0.2f, 0.75f);
			DesertBiome.CleanupArea(desertDescription.Hive, progress, 0.75f, 1f);
			Rectangle area = new Rectangle(desertDescription.CombinedArea.X, 50, desertDescription.CombinedArea.Width, desertDescription.CombinedArea.Bottom - 20);
			structures.AddStructure(area, 10);
			return true;
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x00623FE6 File Offset: 0x006221E6
		private static void ExportDescriptionToEngine(DesertDescription description)
		{
			GenVars.UndergroundDesertLocation = description.CombinedArea;
			GenVars.UndergroundDesertLocation.Inflate(10, 10);
			GenVars.UndergroundDesertHiveLocation = description.Hive;
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x0062400C File Offset: 0x0062220C
		private static void CleanupArea(Rectangle area, GenerationProgress progress, float progressMin, float progressMax)
		{
			int num = 20 - area.Left;
			int num2 = num + area.Right + 20;
			for (int i = -20 + area.Left; i < area.Right + 20; i++)
			{
				progress.Set((double)((float)(i + num) / (float)num2), (double)progressMin, (double)progressMax);
				for (int j = -20 + area.Top; j < area.Bottom + 20; j++)
				{
					if (i > 0 && i < Main.maxTilesX - 1 && j > 0 && j < Main.maxTilesY - 1)
					{
						WorldGen.SquareWallFrame(i, j, true);
						WorldUtils.TileFrame(i, j, true);
					}
				}
			}
		}

		// Token: 0x04005AF3 RID: 23283
		[JsonProperty("ChanceOfEntrance")]
		public double ChanceOfEntrance = 0.3333;
	}
}
