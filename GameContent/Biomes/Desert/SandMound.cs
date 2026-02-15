using System;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.Desert
{
	// Token: 0x0200051A RID: 1306
	public static class SandMound
	{
		// Token: 0x06003673 RID: 13939 RVA: 0x0062728C File Offset: 0x0062548C
		public static void Place(DesertDescription description, GenerationProgress progress, float progressMin, float progressMax)
		{
			Rectangle desert = description.Desert;
			desert.Height = Math.Min(description.Desert.Height, description.Hive.Height / 2);
			Rectangle desert2 = description.Desert;
			desert2.Y = desert.Bottom;
			desert2.Height = Math.Max(0, description.Desert.Bottom - desert.Bottom);
			SurfaceMap surface = description.Surface;
			int num = 0;
			int num2 = 0;
			progress.Set((double)progressMin);
			int num3 = desert.Width + 5;
			for (int i = -5; i < num3; i++)
			{
				double num4 = Math.Abs((double)(i + 5) / (double)(desert.Width + 10)) * 2.0 - 1.0;
				num4 = Utils.Clamp<double>(num4, -1.0, 1.0);
				progress.Set((double)((float)(i + 5) / (float)(num3 + 5)), (double)progressMin, (double)progressMax);
				if (i % 3 == 0)
				{
					num += WorldGen.genRand.Next(-1, 2);
					num = Utils.Clamp<int>(num, -10, 10);
				}
				num2 += WorldGen.genRand.Next(-1, 2);
				num2 = Utils.Clamp<int>(num2, -10, 10);
				double num5 = Math.Sqrt(1.0 - num4 * num4 * num4 * num4);
				int num6 = desert.Bottom - (int)(num5 * (double)desert.Height) + num;
				if (Math.Abs(num4) < 1.0)
				{
					double num7 = Utils.UnclampedSmoothStep(0.5, 0.8, Math.Abs(num4));
					num7 = num7 * num7 * num7;
					int num8 = 10 + (int)((double)desert.Top - num7 * 20.0) + num2;
					num8 = Math.Min(num8, num6);
					for (int j = (int)(surface[i + desert.X] - 1); j < num8; j++)
					{
						int num9 = i + desert.X;
						int num10 = j;
						Main.tile[num9, num10].active(false);
						Main.tile[num9, num10].wall = 0;
					}
				}
				SandMound.PlaceSandColumn(i + desert.X, num6, desert2.Bottom - num6);
			}
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x006274DC File Offset: 0x006256DC
		private static void PlaceSandColumn(int startX, int startY, int height)
		{
			for (int i = startY + height - 1; i >= startY; i--)
			{
				int num = i;
				Tile tile = Main.tile[startX, num];
				if (!WorldGen.remixWorldGen && (!WorldGen.SecretSeed.surfaceIsDesert.Enabled || !WorldGen.SecretSeed.noSurface.Enabled))
				{
					tile.liquid = 0;
				}
				Main.tile[startX, num + 1];
				Main.tile[startX, num + 2];
				tile.type = 53;
				tile.slope(0);
				tile.halfBrick(false);
				tile.active(true);
				if (i < startY)
				{
					tile.active(false);
				}
				WorldGen.SquareWallFrame(startX, num, true);
			}
		}
	}
}
