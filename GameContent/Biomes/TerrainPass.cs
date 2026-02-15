using System;
using Terraria.ID;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	// Token: 0x02000506 RID: 1286
	public class TerrainPass : GenPass
	{
		// Token: 0x06003606 RID: 13830 RVA: 0x00620344 File Offset: 0x0061E544
		public TerrainPass() : base(GenPassNameID.Terrain, 449.3721923828125)
		{
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x0062035C File Offset: 0x0061E55C
		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			int num = configuration.Get<int>("FlatBeachPadding");
			progress.Message = Lang.gen[0].Value;
			TerrainPass.TerrainFeatureType terrainFeatureType = TerrainPass.TerrainFeatureType.Plateau;
			double num2 = (double)Main.maxTilesY * 0.3;
			num2 *= (double)GenBase._random.Next(90, 110) * 0.005;
			double num3 = num2 + (double)Main.maxTilesY * 0.2;
			num3 *= (double)GenBase._random.Next(90, 110) * 0.01;
			if (WorldGen.remixWorldGen)
			{
				num3 = (double)Main.maxTilesY * 0.5;
				if (Main.maxTilesX > 2500)
				{
					num3 = (double)Main.maxTilesY * 0.6;
				}
				num3 *= (double)GenBase._random.Next(95, 106) * 0.01;
			}
			double num4 = num2;
			double num5 = num2;
			double num6 = num3;
			double num7 = num3;
			if (WorldGen.SecretSeed.noSurface.Enabled)
			{
				num2 = 25.0;
				num3 = (double)Main.maxTilesY * 0.4;
				num3 *= (double)GenBase._random.Next(90, 110) * 0.01;
			}
			double num8 = (double)Main.maxTilesY * 0.23;
			TerrainPass.SurfaceHistory surfaceHistory = new TerrainPass.SurfaceHistory(500);
			int num9 = GenVars.leftBeachEnd + num;
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				progress.Set((double)i / (double)Main.maxTilesX);
				num4 = Math.Min(num2, num4);
				num5 = Math.Max(num2, num5);
				num6 = Math.Min(num3, num6);
				num7 = Math.Max(num3, num7);
				if (num9 <= 0)
				{
					terrainFeatureType = (TerrainPass.TerrainFeatureType)GenBase._random.Next(0, 5);
					num9 = GenBase._random.Next(5, 40);
					if (terrainFeatureType == TerrainPass.TerrainFeatureType.Plateau)
					{
						num9 *= (int)((double)GenBase._random.Next(5, 30) * 0.2);
					}
				}
				num9--;
				if ((double)i > (double)Main.maxTilesX * 0.45 && (double)i < (double)Main.maxTilesX * 0.55 && (terrainFeatureType == TerrainPass.TerrainFeatureType.Mountain || terrainFeatureType == TerrainPass.TerrainFeatureType.Valley))
				{
					terrainFeatureType = (TerrainPass.TerrainFeatureType)GenBase._random.Next(3);
				}
				if ((double)i > (double)Main.maxTilesX * 0.48 && (double)i < (double)Main.maxTilesX * 0.52)
				{
					terrainFeatureType = TerrainPass.TerrainFeatureType.Plateau;
				}
				if (!WorldGen.SecretSeed.noSurface.Enabled)
				{
					num2 += TerrainPass.GenerateWorldSurfaceOffset(terrainFeatureType);
				}
				double num10 = 0.17;
				double num11 = 0.26;
				if (WorldGen.SecretSeed.surfaceIsInSpace.Enabled)
				{
					num11 = 0.2199999988079071;
				}
				else if (WorldGen.drunkWorldGen)
				{
					num10 = 0.15;
					num11 = 0.28;
				}
				if (WorldGen.GetWorldSize() == 0)
				{
					num10 += 0.02;
				}
				if (!WorldGen.SecretSeed.noSurface.Enabled)
				{
					if (i < GenVars.leftBeachEnd + num || i > GenVars.rightBeachStart - num)
					{
						num2 = Utils.Clamp<double>(num2, (double)Main.maxTilesY * num10, num8);
					}
					else if (num2 < (double)Main.maxTilesY * num10)
					{
						num2 = (double)Main.maxTilesY * num10;
						num9 = 0;
					}
					else if (num2 > (double)Main.maxTilesY * num11)
					{
						num2 = (double)Main.maxTilesY * num11;
						num9 = 0;
					}
				}
				while (GenBase._random.Next(0, 3) == 0)
				{
					num3 += (double)GenBase._random.Next(-2, 3);
				}
				if (WorldGen.SecretSeed.noSurface.Enabled)
				{
					if (num3 < num2 + (double)Main.maxTilesY * 0.35)
					{
						num3 += 1.0;
					}
					if (num3 > num2 + (double)Main.maxTilesY * 0.45)
					{
						num3 -= 1.0;
					}
				}
				else if (WorldGen.remixWorldGen)
				{
					if (Main.maxTilesX > 2500)
					{
						if (num3 > (double)Main.maxTilesY * 0.7)
						{
							num3 -= 1.0;
						}
					}
					else if (num3 > (double)Main.maxTilesY * 0.6)
					{
						num3 -= 1.0;
					}
				}
				else
				{
					if (num3 < num2 + (double)Main.maxTilesY * 0.06)
					{
						num3 += 1.0;
					}
					if (num3 > num2 + (double)Main.maxTilesY * 0.35)
					{
						num3 -= 1.0;
					}
				}
				surfaceHistory.Record(num2);
				if (WorldGen.SecretSeed.surfaceIsInSpace.Enabled && !WorldGen.SecretSeed.noSurface.Enabled)
				{
					TerrainPass.FillColumn(i, num2 - (double)Main.maxTilesY * 0.08, num3);
				}
				else
				{
					TerrainPass.FillColumn(i, num2, num3);
					if (i == GenVars.rightBeachStart - num)
					{
						if (num2 > num8)
						{
							TerrainPass.RetargetSurfaceHistory(surfaceHistory, i, num8);
						}
						terrainFeatureType = TerrainPass.TerrainFeatureType.Plateau;
						num9 = Main.maxTilesX - i;
					}
				}
			}
			Main.worldSurface = (double)((int)(num5 + 25.0));
			if (WorldGen.SecretSeed.noSurface.Enabled)
			{
				Main.worldSurface = 25.0;
			}
			Main.rockLayer = num7;
			double num12 = (double)((int)((Main.rockLayer - Main.worldSurface) / 6.0) * 6);
			Main.rockLayer = (double)((int)(Main.worldSurface + num12));
			int num13 = (int)(Main.rockLayer + (double)Main.maxTilesY) / 2 + GenBase._random.Next(-100, 20);
			int lavaLine = num13 + GenBase._random.Next(50, 80);
			if (WorldGen.remixWorldGen)
			{
				lavaLine = (int)(Main.worldSurface * 4.0 + num3) / 5;
			}
			int num14 = 20;
			if (num6 < num5 + (double)num14)
			{
				double num15 = (num6 + num5) / 2.0;
				double num16 = Math.Abs(num6 - num5);
				if (num16 < (double)num14)
				{
					num16 = (double)num14;
				}
				num6 = num15 + num16 / 2.0;
				num5 = num15 - num16 / 2.0;
			}
			GenVars.rockLayer = num3;
			GenVars.rockLayerHigh = num7;
			GenVars.rockLayerLow = num6;
			GenVars.worldSurface = num2;
			GenVars.worldSurfaceHigh = num5;
			GenVars.worldSurfaceLow = num4;
			GenVars.waterLine = num13;
			GenVars.lavaLine = lavaLine;
			GenVars.remixMushroomLayerLow = Main.maxTilesY - 350;
			GenVars.remixMushroomLayerHigh = Main.UnderworldLayer;
			GenVars.remixSurfaceLayerLow = (int)GenVars.rockLayerLow;
			GenVars.remixSurfaceLayerHigh = GenVars.remixMushroomLayerLow;
		}

		// Token: 0x06003608 RID: 13832 RVA: 0x0062098C File Offset: 0x0061EB8C
		private static void FillColumn(int x, double worldSurface, double rockLayer)
		{
			int num = 0;
			while ((double)num < worldSurface)
			{
				Main.tile[x, num].active(false);
				Main.tile[x, num].frameX = -1;
				Main.tile[x, num].frameY = -1;
				num++;
			}
			for (int i = (int)worldSurface; i < Main.maxTilesY; i++)
			{
				if ((double)i < rockLayer)
				{
					Main.tile[x, i].active(true);
					Main.tile[x, i].type = 0;
					Main.tile[x, i].frameX = -1;
					Main.tile[x, i].frameY = -1;
				}
				else
				{
					Main.tile[x, i].active(true);
					Main.tile[x, i].type = 1;
					Main.tile[x, i].frameX = -1;
					Main.tile[x, i].frameY = -1;
				}
			}
		}

		// Token: 0x06003609 RID: 13833 RVA: 0x00620A8C File Offset: 0x0061EC8C
		private static void RetargetColumn(int x, double worldSurface)
		{
			int num = 0;
			while ((double)num < worldSurface)
			{
				Main.tile[x, num].active(false);
				Main.tile[x, num].frameX = -1;
				Main.tile[x, num].frameY = -1;
				num++;
			}
			for (int i = (int)worldSurface; i < Main.maxTilesY; i++)
			{
				if (Main.tile[x, i].type != 1 || !Main.tile[x, i].active())
				{
					Main.tile[x, i].active(true);
					Main.tile[x, i].type = 0;
					Main.tile[x, i].frameX = -1;
					Main.tile[x, i].frameY = -1;
				}
			}
		}

		// Token: 0x0600360A RID: 13834 RVA: 0x00620B5C File Offset: 0x0061ED5C
		private static double GenerateWorldSurfaceOffset(TerrainPass.TerrainFeatureType featureType)
		{
			double num = 0.0;
			if ((WorldGen.drunkWorldGen || WorldGen.getGoodWorldGen || WorldGen.remixWorldGen) && WorldGen.genRand.Next(2) == 0)
			{
				switch (featureType)
				{
				case TerrainPass.TerrainFeatureType.Plateau:
					while (GenBase._random.Next(0, 6) == 0)
					{
						num += (double)GenBase._random.Next(-1, 2);
					}
					break;
				case TerrainPass.TerrainFeatureType.Hill:
					while (GenBase._random.Next(0, 3) == 0)
					{
						num -= 1.0;
					}
					while (GenBase._random.Next(0, 10) == 0)
					{
						num += 1.0;
					}
					break;
				case TerrainPass.TerrainFeatureType.Dale:
					while (GenBase._random.Next(0, 3) == 0)
					{
						num += 1.0;
					}
					while (GenBase._random.Next(0, 10) == 0)
					{
						num -= 1.0;
					}
					break;
				case TerrainPass.TerrainFeatureType.Mountain:
					while (GenBase._random.Next(0, 3) != 0)
					{
						num -= 1.0;
					}
					while (GenBase._random.Next(0, 6) == 0)
					{
						num += 1.0;
					}
					break;
				case TerrainPass.TerrainFeatureType.Valley:
					while (GenBase._random.Next(0, 3) != 0)
					{
						num += 1.0;
					}
					while (GenBase._random.Next(0, 5) == 0)
					{
						num -= 1.0;
					}
					break;
				}
			}
			else
			{
				switch (featureType)
				{
				case TerrainPass.TerrainFeatureType.Plateau:
					while (GenBase._random.Next(0, 7) == 0)
					{
						num += (double)GenBase._random.Next(-1, 2);
					}
					break;
				case TerrainPass.TerrainFeatureType.Hill:
					while (GenBase._random.Next(0, 4) == 0)
					{
						num -= 1.0;
					}
					while (GenBase._random.Next(0, 10) == 0)
					{
						num += 1.0;
					}
					break;
				case TerrainPass.TerrainFeatureType.Dale:
					while (GenBase._random.Next(0, 4) == 0)
					{
						num += 1.0;
					}
					while (GenBase._random.Next(0, 10) == 0)
					{
						num -= 1.0;
					}
					break;
				case TerrainPass.TerrainFeatureType.Mountain:
					while (GenBase._random.Next(0, 2) == 0)
					{
						num -= 1.0;
					}
					while (GenBase._random.Next(0, 6) == 0)
					{
						num += 1.0;
					}
					break;
				case TerrainPass.TerrainFeatureType.Valley:
					while (GenBase._random.Next(0, 2) == 0)
					{
						num += 1.0;
					}
					while (GenBase._random.Next(0, 5) == 0)
					{
						num -= 1.0;
					}
					break;
				}
			}
			return num;
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x00620DF4 File Offset: 0x0061EFF4
		private static void RetargetSurfaceHistory(TerrainPass.SurfaceHistory history, int targetX, double targetHeight)
		{
			int num = 0;
			while (num < history.Length / 2 && history[history.Length - 1] > targetHeight)
			{
				for (int i = 0; i < history.Length - num * 2; i++)
				{
					double num2 = history[history.Length - i - 1];
					num2 -= 1.0;
					history[history.Length - i - 1] = num2;
					if (num2 <= targetHeight)
					{
						break;
					}
				}
				num++;
			}
			for (int j = 0; j < history.Length; j++)
			{
				double worldSurface = history[history.Length - j - 1];
				TerrainPass.RetargetColumn(targetX - j, worldSurface);
			}
		}

		// Token: 0x02000992 RID: 2450
		private enum TerrainFeatureType
		{
			// Token: 0x04007629 RID: 30249
			Plateau,
			// Token: 0x0400762A RID: 30250
			Hill,
			// Token: 0x0400762B RID: 30251
			Dale,
			// Token: 0x0400762C RID: 30252
			Mountain,
			// Token: 0x0400762D RID: 30253
			Valley
		}

		// Token: 0x02000993 RID: 2451
		private class SurfaceHistory
		{
			// Token: 0x17000593 RID: 1427
			public double this[int index]
			{
				get
				{
					return this._heights[(index + this._index) % this._heights.Length];
				}
				set
				{
					this._heights[(index + this._index) % this._heights.Length] = value;
				}
			}

			// Token: 0x17000594 RID: 1428
			// (get) Token: 0x06004978 RID: 18808 RVA: 0x006D0BED File Offset: 0x006CEDED
			public int Length
			{
				get
				{
					return this._heights.Length;
				}
			}

			// Token: 0x06004979 RID: 18809 RVA: 0x006D0BF7 File Offset: 0x006CEDF7
			public SurfaceHistory(int size)
			{
				this._heights = new double[size];
			}

			// Token: 0x0600497A RID: 18810 RVA: 0x006D0C0B File Offset: 0x006CEE0B
			public void Record(double height)
			{
				this._heights[this._index] = height;
				this._index = (this._index + 1) % this._heights.Length;
			}

			// Token: 0x0400762E RID: 30254
			private readonly double[] _heights;

			// Token: 0x0400762F RID: 30255
			private int _index;
		}
	}
}
