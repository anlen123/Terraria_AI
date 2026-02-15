using System;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.Desert
{
	// Token: 0x02000519 RID: 1305
	public static class PitEntrance
	{
		// Token: 0x0600366F RID: 13935 RVA: 0x00626F68 File Offset: 0x00625168
		public static void Place(DesertDescription description, GenerationProgress progress, float progressMin, float progressMax)
		{
			int holeRadius = WorldGen.genRand.Next(6, 9);
			Point center = description.CombinedArea.Center;
			center.Y = (int)description.Surface[center.X];
			PitEntrance.PlaceAt(description, center, holeRadius, progress, progressMin, progressMax);
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x00626FB8 File Offset: 0x006251B8
		private static void PlaceAt(DesertDescription description, Point position, int holeRadius, GenerationProgress progress, float progressMin, float progressMax)
		{
			int num = holeRadius + 3;
			int num2 = num + holeRadius + 3;
			for (int i = -holeRadius - 3; i < holeRadius + 3; i++)
			{
				progress.Set((double)((float)(i + num) / (float)num2), (double)progressMin, (double)progressMax);
				for (int j = (int)description.Surface[i + position.X]; j <= description.Hive.Top + 10; j++)
				{
					double num3 = (double)(j - (int)description.Surface[i + position.X]) / (double)(description.Hive.Top - description.Desert.Top);
					num3 = Utils.Clamp<double>(num3, 0.0, 1.0);
					int num4 = (int)(PitEntrance.GetHoleRadiusScaleAt(num3) * (double)holeRadius);
					if (Math.Abs(i) < num4)
					{
						Main.tile[i + position.X, j].ClearEverything();
					}
					else if (Math.Abs(i) < num4 + 3 && num3 > 0.35)
					{
						Main.tile[i + position.X, j].ResetToType(397);
					}
					double num5 = Math.Abs((double)i / (double)holeRadius);
					num5 *= num5;
					if (Math.Abs(i) < num4 + 3 && (double)(j - position.Y) > 15.0 - 3.0 * num5)
					{
						Main.tile[i + position.X, j].wall = 187;
						WorldGen.SquareWallFrame(i + position.X, j - 1, true);
						WorldGen.SquareWallFrame(i + position.X, j, true);
					}
				}
			}
			holeRadius += 4;
			for (int k = -holeRadius; k < holeRadius; k++)
			{
				int num6 = holeRadius - Math.Abs(k);
				num6 = Math.Min(10, num6 * num6);
				for (int l = 0; l < num6; l++)
				{
					Main.tile[k + position.X, l + (int)description.Surface[k + position.X]].ClearEverything();
				}
			}
		}

		// Token: 0x06003671 RID: 13937 RVA: 0x006271D8 File Offset: 0x006253D8
		private static double GetHoleRadiusScaleAt(double yProgress)
		{
			if (yProgress < 0.6)
			{
				return 1.0;
			}
			return (1.0 - PitEntrance.SmootherStep((yProgress - 0.6) / 0.4)) * 0.5 + 0.5;
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x00627234 File Offset: 0x00625434
		private static double SmootherStep(double delta)
		{
			delta = Utils.Clamp<double>(delta, 0.0, 1.0);
			return 1.0 - Math.Cos(delta * 3.1415927410125732) * 0.5 - 0.5;
		}
	}
}
