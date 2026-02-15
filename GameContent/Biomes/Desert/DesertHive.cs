using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.Desert
{
	// Token: 0x02000517 RID: 1303
	public static class DesertHive
	{
		// Token: 0x06003669 RID: 13929 RVA: 0x0062639C File Offset: 0x0062459C
		public static void Place(DesertDescription description, GenerationProgress progress, float progressMin, float progressMax)
		{
			DesertHive.ClusterGroup clusters = DesertHive.ClusterGroup.FromDescription(description);
			DesertHive.PlaceClusters(description, clusters, progress, progressMin, progressMax);
			DesertHive.AddTileVariance(description);
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x006263C0 File Offset: 0x006245C0
		private static void PlaceClusters(DesertDescription description, DesertHive.ClusterGroup clusters, GenerationProgress progress, float progressMin, float progressMax)
		{
			Rectangle hive = description.Hive;
			hive.Inflate(20, 20);
			DesertHive.PostPlacementEffect[,] array = new DesertHive.PostPlacementEffect[hive.Width, hive.Height];
			DesertHive.PlaceClustersArea(description, clusters, hive, array, Point.Zero, progress, progressMin, progressMax);
			for (int i = hive.Left; i < hive.Right; i++)
			{
				for (int j = hive.Top; j < hive.Bottom; j++)
				{
					if ((array[i - hive.Left, j - hive.Top] & DesertHive.PostPlacementEffect.Smooth) != DesertHive.PostPlacementEffect.None)
					{
						Tile.SmoothSlope(i, j, false, false);
					}
				}
			}
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x00626458 File Offset: 0x00624658
		private static void PlaceClustersArea(DesertDescription description, DesertHive.ClusterGroup clusters, Rectangle area, DesertHive.PostPlacementEffect[,] postEffectMap, Point postEffectMapOffset, GenerationProgress progress, float progressMin, float progressMax)
		{
			FastRandom fastRandom = new FastRandom(Main.ActiveWorldFileData.Seed).WithModifier(57005UL);
			Vector2D vector2D;
			vector2D..ctor((double)description.Hive.Width, (double)description.Hive.Height);
			Vector2D vector2D2;
			vector2D2..ctor((double)clusters.Width, (double)clusters.Height);
			Vector2D vector2D3 = description.BlockScale / 2.0;
			for (int i = area.Left; i < area.Right; i++)
			{
				progress.Set((double)((float)(i - area.Left) / (float)(area.Right - area.Left)), (double)progressMin, (double)progressMax);
				for (int j = area.Top; j < area.Bottom; j++)
				{
					if (WorldGen.InWorld(i, j, 1))
					{
						byte liquid = Main.tile[i, j].liquid;
						double num = 0.0;
						int num2 = -1;
						double num3 = 0.0;
						ushort type = 53;
						if (fastRandom.Next(3) == 0)
						{
							type = 397;
						}
						int num4 = i - description.Hive.X;
						int num5 = j - description.Hive.Y;
						Vector2D vector2D4 = (new Vector2D((double)num4, (double)num5) - vector2D3) / vector2D * vector2D2;
						for (int k = 0; k < clusters.Count; k++)
						{
							DesertHive.Cluster cluster = clusters[k];
							if (Math.Abs(cluster[0].Position.X - vector2D4.X) <= 10.0 && Math.Abs(cluster[0].Position.Y - vector2D4.Y) <= 10.0)
							{
								double num6 = 0.0;
								foreach (DesertHive.Block block in cluster)
								{
									num6 += 1.0 / Vector2D.DistanceSquared(block.Position, vector2D4);
								}
								if (num6 > num)
								{
									if (num > num3)
									{
										num3 = num;
									}
									num = num6;
									num2 = k;
								}
								else if (num6 > num3)
								{
									num3 = num6;
								}
							}
						}
						double num7 = num + num3;
						Tile tile = Main.tile[i, j];
						bool flag = ((new Vector2D((double)num4, (double)num5) - vector2D3) / vector2D * 2.0 - Vector2D.One).Length() >= 0.8;
						DesertHive.PostPlacementEffect postPlacementEffect = DesertHive.PostPlacementEffect.None;
						bool flag2 = true;
						if (num7 > 3.5)
						{
							postPlacementEffect = DesertHive.PostPlacementEffect.Smooth;
							tile.ClearEverything();
							if (!WorldGen.remixWorldGen || (double)j <= Main.rockLayer + (double)WorldGen.genRand.Next(-1, 2) || WorldGen.SecretSeed.Variations.surfaceIsDesertSwapDesertAndSnowBiomes)
							{
								tile.wall = 187;
								if (num2 % 15 == 2)
								{
									tile.ResetToType(404);
								}
							}
						}
						else if (num7 > 1.8)
						{
							if (!WorldGen.remixWorldGen || (double)j <= Main.rockLayer + (double)WorldGen.genRand.Next(-1, 2) || WorldGen.SecretSeed.Variations.surfaceIsDesertSwapDesertAndSnowBiomes)
							{
								tile.wall = 187;
							}
							if (!WorldGen.SecretSeed.Variations.surfaceIsDesertSwapDesertAndSnowBiomes)
							{
								if ((double)j < Main.worldSurface)
								{
									tile.liquid = 0;
								}
								else if (!WorldGen.remixWorldGen)
								{
									tile.lava(true);
								}
							}
							if (!flag || tile.active())
							{
								tile.ResetToType(396);
								postPlacementEffect = DesertHive.PostPlacementEffect.Smooth;
							}
						}
						else if (num7 > 0.7 || !flag)
						{
							if (!WorldGen.remixWorldGen || (double)j <= Main.rockLayer + (double)WorldGen.genRand.Next(-1, 2) || WorldGen.SecretSeed.Variations.surfaceIsDesertSwapDesertAndSnowBiomes)
							{
								tile.wall = 216;
								if (!WorldGen.SecretSeed.Variations.surfaceIsDesertSwapDesertAndSnowBiomes)
								{
									tile.liquid = 0;
								}
							}
							if (!flag || tile.active())
							{
								tile.ResetToType(type);
								postPlacementEffect = DesertHive.PostPlacementEffect.Smooth;
							}
						}
						else if (num7 > 0.25)
						{
							FastRandom fastRandom2 = fastRandom.WithModifier(num4, num5);
							double num8 = (num7 - 0.25) / 0.45;
							if (fastRandom2.NextDouble() < num8)
							{
								if (!WorldGen.remixWorldGen || (double)j <= Main.rockLayer + (double)WorldGen.genRand.Next(-1, 2) || WorldGen.SecretSeed.Variations.surfaceIsDesertSwapDesertAndSnowBiomes)
								{
									tile.wall = 187;
								}
								if (!WorldGen.SecretSeed.Variations.surfaceIsDesertSwapDesertAndSnowBiomes)
								{
									if ((double)j < Main.worldSurface)
									{
										tile.liquid = 0;
									}
									else if (!WorldGen.remixWorldGen)
									{
										tile.lava(true);
									}
								}
								if (tile.active())
								{
									tile.ResetToType(type);
									postPlacementEffect = DesertHive.PostPlacementEffect.Smooth;
								}
							}
						}
						else
						{
							flag2 = false;
						}
						if (flag2)
						{
							WorldGen.UpdateDesertHiveBounds(i, j);
						}
						postEffectMap[i - area.X + postEffectMapOffset.X, j - area.Y + postEffectMapOffset.Y] = postPlacementEffect;
						if (WorldGen.remixWorldGen)
						{
							Main.tile[i, j].liquid = liquid;
						}
						else if (WorldGen.SecretSeed.Variations.surfaceIsDesertSwapDesertAndSnowBiomes)
						{
							Main.tile[i, j].liquid = liquid;
						}
					}
				}
			}
		}

		// Token: 0x0600366C RID: 13932 RVA: 0x006269C8 File Offset: 0x00624BC8
		private static void AddTileVariance(DesertDescription description)
		{
			for (int i = -20; i < description.Hive.Width + 20; i++)
			{
				for (int j = -20; j < description.Hive.Height + 20; j++)
				{
					int num = i + description.Hive.X;
					int num2 = j + description.Hive.Y;
					if (WorldGen.InWorld(num, num2, 1))
					{
						Tile tile = Main.tile[num, num2];
						Tile testTile = Main.tile[num, num2 + 1];
						Tile testTile2 = Main.tile[num, num2 + 2];
						if (tile.type == 53 && (!WorldGen.SolidTile(testTile) || !WorldGen.SolidTile(testTile2)))
						{
							tile.type = 397;
						}
					}
				}
			}
			for (int k = -20; k < description.Hive.Width + 20; k++)
			{
				for (int l = -20; l < description.Hive.Height + 20; l++)
				{
					int num3 = k + description.Hive.X;
					int num4 = l + description.Hive.Y;
					if (WorldGen.InWorld(num3, num4, 5))
					{
						Tile tile2 = Main.tile[num3, num4];
						if (tile2.active() && tile2.type == 396)
						{
							bool flag = true;
							for (int m = -1; m >= -3; m--)
							{
								Tile tile3 = Main.tile[num3, num4 + m];
								Tile tile4 = Main.tile[num3 + 1, num4 + m];
								if (tile3.active() || tile4.active())
								{
									flag = false;
									break;
								}
							}
							bool flag2 = true;
							for (int n = 1; n <= 3; n++)
							{
								Tile tile5 = Main.tile[num3, num4 + n];
								Tile tile6 = Main.tile[num3 + 1, num4 + n];
								if (tile5.active() || tile6.active())
								{
									flag2 = false;
									break;
								}
							}
							if (!WorldGen.remixWorldGen || (double)num4 <= Main.rockLayer)
							{
								if (flag && WorldGen.genRand.Next(20) == 0)
								{
									ushort type = 485;
									int style = WorldGen.genRand.Next(4);
									if (WorldGen.genRand.Next(30) == 0)
									{
										type = 751;
										style = 0;
									}
									WorldGen.PlaceTile(num3, num4 - 1, (int)type, true, true, -1, style);
								}
								else if (flag && WorldGen.genRand.Next(5) == 0)
								{
									WorldGen.PlaceTile(num3, num4 - 1, 484, true, true, -1, 0);
								}
								else if ((flag ^ flag2) && WorldGen.genRand.Next(5) == 0)
								{
									WorldGen.PlaceTile(num3, num4 + (flag ? -1 : 1), 165, true, true, -1, 0);
								}
								else if (flag && WorldGen.genRand.Next(5) == 0)
								{
									WorldGen.PlaceTile(num3, num4 - 1, 187, true, true, -1, 29 + WorldGen.genRand.Next(6));
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0200099B RID: 2459
		private struct Block
		{
			// Token: 0x06004999 RID: 18841 RVA: 0x006D0DBE File Offset: 0x006CEFBE
			public Block(double x, double y)
			{
				this.Position = new Vector2D(x, y);
			}

			// Token: 0x0400763E RID: 30270
			public Vector2D Position;
		}

		// Token: 0x0200099C RID: 2460
		private class Cluster : List<DesertHive.Block>
		{
		}

		// Token: 0x0200099D RID: 2461
		private class ClusterGroup : List<DesertHive.Cluster>
		{
			// Token: 0x0600499B RID: 18843 RVA: 0x006D0DD5 File Offset: 0x006CEFD5
			private ClusterGroup(int width, int height)
			{
				this.Width = width;
				this.Height = height;
				this.Generate();
			}

			// Token: 0x0600499C RID: 18844 RVA: 0x006D0DF1 File Offset: 0x006CEFF1
			public static DesertHive.ClusterGroup FromDescription(DesertDescription description)
			{
				return new DesertHive.ClusterGroup(description.BlockColumnCount, description.BlockRowCount);
			}

			// Token: 0x0600499D RID: 18845 RVA: 0x006D0E04 File Offset: 0x006CF004
			private static void SearchForCluster(bool[,] blockMap, List<Point> pointCluster, int x, int y, int level = 2)
			{
				pointCluster.Add(new Point(x, y));
				blockMap[x, y] = false;
				level--;
				if (level == -1)
				{
					return;
				}
				if (x > 0 && blockMap[x - 1, y])
				{
					DesertHive.ClusterGroup.SearchForCluster(blockMap, pointCluster, x - 1, y, level);
				}
				if (x < blockMap.GetLength(0) - 1 && blockMap[x + 1, y])
				{
					DesertHive.ClusterGroup.SearchForCluster(blockMap, pointCluster, x + 1, y, level);
				}
				if (y > 0 && blockMap[x, y - 1])
				{
					DesertHive.ClusterGroup.SearchForCluster(blockMap, pointCluster, x, y - 1, level);
				}
				if (y < blockMap.GetLength(1) - 1 && blockMap[x, y + 1])
				{
					DesertHive.ClusterGroup.SearchForCluster(blockMap, pointCluster, x, y + 1, level);
				}
			}

			// Token: 0x0600499E RID: 18846 RVA: 0x006D0EB8 File Offset: 0x006CF0B8
			private static void AttemptClaim(int x, int y, int[,] clusterIndexMap, List<List<Point>> pointClusters, int index)
			{
				int num = clusterIndexMap[x, y];
				if (num != -1 && num != index)
				{
					int num2 = (WorldGen.genRand.Next(2) == 0) ? -1 : index;
					foreach (Point point in pointClusters[num])
					{
						clusterIndexMap[point.X, point.Y] = num2;
					}
				}
			}

			// Token: 0x0600499F RID: 18847 RVA: 0x006D0F40 File Offset: 0x006CF140
			private void Generate()
			{
				base.Clear();
				bool[,] array = new bool[this.Width, this.Height];
				int num = this.Width / 2 - 1;
				int num2 = this.Height / 2 - 1;
				int num3 = (num + 1) * (num + 1);
				Point point = new Point(num, num2);
				for (int i = point.Y - num2; i <= point.Y + num2; i++)
				{
					double num4 = (double)num / (double)num2 * (double)(i - point.Y);
					int num5 = Math.Min(num, (int)Math.Sqrt((double)num3 - num4 * num4));
					for (int j = point.X - num5; j <= point.X + num5; j++)
					{
						array[j, i] = (WorldGen.genRand.Next(2) == 0);
					}
				}
				List<List<Point>> list = new List<List<Point>>();
				for (int k = 0; k < array.GetLength(0); k++)
				{
					for (int l = 0; l < array.GetLength(1); l++)
					{
						if (array[k, l] && WorldGen.genRand.Next(2) == 0)
						{
							List<Point> list2 = new List<Point>();
							DesertHive.ClusterGroup.SearchForCluster(array, list2, k, l, 2);
							if (list2.Count > 2)
							{
								list.Add(list2);
							}
						}
					}
				}
				int[,] array2 = new int[array.GetLength(0), array.GetLength(1)];
				for (int m = 0; m < array2.GetLength(0); m++)
				{
					for (int n = 0; n < array2.GetLength(1); n++)
					{
						array2[m, n] = -1;
					}
				}
				for (int num6 = 0; num6 < list.Count; num6++)
				{
					foreach (Point point2 in list[num6])
					{
						array2[point2.X, point2.Y] = num6;
					}
				}
				for (int num7 = 0; num7 < list.Count; num7++)
				{
					foreach (Point point3 in list[num7])
					{
						int x = point3.X;
						int y = point3.Y;
						if (array2[x, y] == -1)
						{
							break;
						}
						int index = array2[x, y];
						if (x > 0)
						{
							DesertHive.ClusterGroup.AttemptClaim(x - 1, y, array2, list, index);
						}
						if (x < array2.GetLength(0) - 1)
						{
							DesertHive.ClusterGroup.AttemptClaim(x + 1, y, array2, list, index);
						}
						if (y > 0)
						{
							DesertHive.ClusterGroup.AttemptClaim(x, y - 1, array2, list, index);
						}
						if (y < array2.GetLength(1) - 1)
						{
							DesertHive.ClusterGroup.AttemptClaim(x, y + 1, array2, list, index);
						}
					}
				}
				foreach (List<Point> list3 in list)
				{
					list3.Clear();
				}
				for (int num8 = 0; num8 < array2.GetLength(0); num8++)
				{
					for (int num9 = 0; num9 < array2.GetLength(1); num9++)
					{
						if (array2[num8, num9] != -1)
						{
							list[array2[num8, num9]].Add(new Point(num8, num9));
						}
					}
				}
				foreach (List<Point> list4 in list)
				{
					if (list4.Count < 4)
					{
						list4.Clear();
					}
				}
				foreach (List<Point> list5 in list)
				{
					DesertHive.Cluster cluster = new DesertHive.Cluster();
					if (list5.Count > 0)
					{
						foreach (Point point4 in list5)
						{
							cluster.Add(new DesertHive.Block((double)point4.X + (WorldGen.genRand.NextDouble() - 0.5) * 0.5, (double)point4.Y + (WorldGen.genRand.NextDouble() - 0.5) * 0.5));
						}
						base.Add(cluster);
					}
				}
			}

			// Token: 0x0400763F RID: 30271
			public readonly int Width;

			// Token: 0x04007640 RID: 30272
			public readonly int Height;
		}

		// Token: 0x0200099E RID: 2462
		[Flags]
		private enum PostPlacementEffect : byte
		{
			// Token: 0x04007642 RID: 30274
			None = 0,
			// Token: 0x04007643 RID: 30275
			Smooth = 1
		}
	}
}
