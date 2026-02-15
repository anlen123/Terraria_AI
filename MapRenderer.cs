using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Map;
using Terraria.WorldBuilding;

namespace Terraria
{
	// Token: 0x02000015 RID: 21
	public class MapRenderer
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600009E RID: 158 RVA: 0x0000A448 File Offset: 0x00008648
		public static bool ChangesQueued
		{
			get
			{
				List<Point16>[,] array = MapRenderer.changeQueues;
				int upperBound = array.GetUpperBound(0);
				int upperBound2 = array.GetUpperBound(1);
				for (int i = array.GetLowerBound(0); i <= upperBound; i++)
				{
					for (int j = array.GetLowerBound(1); j <= upperBound2; j++)
					{
						if (array[i, j].Count > 0)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600009F RID: 159 RVA: 0x0000A4A7 File Offset: 0x000086A7
		private static GraphicsDevice GraphicsDevice
		{
			get
			{
				return Main.instance.GraphicsDevice;
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000A4B4 File Offset: 0x000086B4
		static MapRenderer()
		{
			for (int i = 0; i < MapRenderer.numTargetsX; i++)
			{
				for (int j = 0; j < MapRenderer.numTargetsY; j++)
				{
					MapRenderer.changeQueues[i, j] = new List<Point16>(MapRenderer.ChangeRefreshThreshold);
				}
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000A580 File Offset: 0x00008780
		private static bool checkMap(int i, int j)
		{
			if (MapRenderer.mapTarget[i, j] == null || MapRenderer.mapTarget[i, j].IsDisposed)
			{
				MapRenderer.initMap[i, j] = false;
			}
			if (MapRenderer.initMap[i, j])
			{
				return true;
			}
			bool result;
			try
			{
				int width = MapRenderer.textureMaxWidth;
				int height = MapRenderer.textureMaxHeight;
				if (i == MapRenderer.numTargetsX - 1)
				{
					width = 400;
				}
				if (j == MapRenderer.numTargetsY - 1)
				{
					height = 600;
				}
				MapRenderer.mapTarget[i, j] = new RenderTarget2D(MapRenderer.GraphicsDevice, width, height, false, MapRenderer.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
				MapRenderer.initMap[i, j] = true;
				result = true;
			}
			catch
			{
				Main.mapEnabled = false;
				for (int k = 0; k < MapRenderer.numTargetsX; k++)
				{
					for (int l = 0; l < MapRenderer.numTargetsY; l++)
					{
						try
						{
							MapRenderer.initMap[k, l] = false;
							MapRenderer.mapTarget[k, l].Dispose();
						}
						catch
						{
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000A6A8 File Offset: 0x000088A8
		public static void DrawToMap(Rectangle area)
		{
			if (!Main.mapEnabled)
			{
				return;
			}
			area = WorldUtils.ClampToWorld(area, 0);
			int num = Main.maxTilesX / MapRenderer.textureMaxWidth;
			int num2 = Main.maxTilesY / MapRenderer.textureMaxHeight;
			for (int i = 0; i <= num; i++)
			{
				for (int j = 0; j <= num2; j++)
				{
					if (!MapRenderer.checkMap(i, j))
					{
						return;
					}
				}
			}
			TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
			if (Main.clearMap || Main.refreshMap)
			{
				List<Point16>[,] array = MapRenderer.changeQueues;
				int upperBound = array.GetUpperBound(0);
				int upperBound2 = array.GetUpperBound(1);
				for (int k = array.GetLowerBound(0); k <= upperBound; k++)
				{
					for (int l = array.GetLowerBound(1); l <= upperBound2; l++)
					{
						array[k, l].Clear();
					}
				}
			}
			if (Main.clearMap)
			{
				for (int m = 0; m <= num; m++)
				{
					for (int n = 0; n <= num2; n++)
					{
						MapRenderer.GraphicsDevice.SetRenderTarget(MapRenderer.mapTarget[m, n]);
						MapRenderer.GraphicsDevice.Clear(Color.Transparent);
						MapRenderer.GraphicsDevice.SetRenderTarget(null);
					}
				}
				Main.clearMap = false;
			}
			RenderTarget2D renderTarget2D = null;
			int num3 = 0;
			int num4 = 0;
			while (num4 < MapRenderer.numTargetsX && num3 < Main.maxMapUpdates)
			{
				int num5 = 0;
				while (num5 < MapRenderer.numTargetsY && num3 < Main.maxMapUpdates)
				{
					Rectangle value = new Rectangle(num4 * MapRenderer.textureMaxWidth, num5 * MapRenderer.textureMaxHeight, MapRenderer.textureMaxWidth, MapRenderer.textureMaxHeight);
					MapRenderer.DrawAreaToMap(Rectangle.Intersect(area, value), num4, num5, ref renderTarget2D, ref num3);
					num5++;
				}
				num4++;
			}
			if (renderTarget2D != null)
			{
				Main.spriteBatch.End();
				MapRenderer.GraphicsDevice.SetRenderTarget(null);
			}
			Main.mapReady = true;
			Main.loadMapLastX = 0;
			Main.loadMap = false;
			Main.loadMapLock = false;
			TimeLogger.MapUpdate.AddTime(fromTimestamp);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000A888 File Offset: 0x00008A88
		private static void DrawAreaToMap(Rectangle area, int rX, int rY, ref RenderTarget2D activeTarget, ref int updateCount)
		{
			RenderTarget2D renderTarget2D = MapRenderer.mapTarget[rX, rY];
			if (renderTarget2D == null || renderTarget2D.IsContentLost)
			{
				return;
			}
			for (int i = area.Left; i < area.Right; i++)
			{
				for (int j = area.Top; j < area.Bottom; j++)
				{
					MapTile mapTile = Main.Map[i, j];
					if (mapTile.IsChanged)
					{
						int num = updateCount;
						updateCount = num + 1;
						if (num >= Main.maxMapUpdates)
						{
							return;
						}
						if (Main.loadMap)
						{
							Main.loadMapLastX = i;
						}
						MapRenderer.DrawChangesToMap(rX, rY, ref activeTarget, renderTarget2D, i, ref j, mapTile);
					}
				}
			}
			List<Point16> list = MapRenderer.changeQueues[rX, rY];
			for (int k = 0; k < list.Count; k++)
			{
				Point16 point = list[k];
				MapTile mapTile2 = Main.Map[(int)point.X, (int)point.Y];
				if (mapTile2.IsChanged)
				{
					int num = updateCount;
					updateCount = num + 1;
					if (num >= Main.maxMapUpdates)
					{
						list.RemoveRange(0, k);
						return;
					}
					int y = (int)point.Y;
					MapRenderer.DrawChangesToMap(rX, rY, ref activeTarget, renderTarget2D, (int)point.X, ref y, mapTile2);
				}
			}
			list.Clear();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000A9B8 File Offset: 0x00008BB8
		private static void DrawChangesToMap(int rX, int rY, ref RenderTarget2D activeTarget, RenderTarget2D target, int i, ref int j, MapTile mapTile)
		{
			Main.Map.ConsumeUpdate(i, j);
			if (target != activeTarget)
			{
				if (activeTarget != null)
				{
					Main.spriteBatch.End();
				}
				activeTarget = target;
				MapRenderer.GraphicsDevice.SetRenderTarget(target);
				Main.spriteBatch.Begin();
			}
			int num = i - rX * MapRenderer.textureMaxWidth;
			int num2 = j - rY * MapRenderer.textureMaxHeight;
			Color mapTileXnaColor = MapHelper.GetMapTileXnaColor(mapTile);
			int num3 = 1;
			int num4 = 1;
			int num5 = j + 1;
			for (;;)
			{
				MapTile mapTile2;
				MapTile other = mapTile2 = Main.Map[i, num5];
				if (!mapTile2.IsChanged || !mapTile.Equals(other) || num5 / MapRenderer.textureMaxHeight != rY)
				{
					break;
				}
				Main.Map.ConsumeUpdate(i, num5);
				num3++;
				num5++;
				j++;
			}
			if (num3 == 1)
			{
				num5 = i + 1;
				for (;;)
				{
					MapTile mapTile2;
					MapTile other = mapTile2 = Main.Map[num5, j];
					if (!mapTile2.IsChanged || !mapTile.Equals(other) || num5 / MapRenderer.textureMaxWidth != rX)
					{
						break;
					}
					Main.Map.ConsumeUpdate(num5, j);
					num4++;
					num5++;
				}
			}
			Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Vector2((float)num, (float)num2), new Rectangle?(new Rectangle(0, 0, num4, num3)), mapTileXnaColor, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000AB20 File Offset: 0x00008D20
		public static void QueueChange(int x, int y)
		{
			if (Main.refreshMap)
			{
				return;
			}
			int num = x / MapRenderer.textureMaxWidth;
			int num2 = y / MapRenderer.textureMaxHeight;
			List<Point16> list = MapRenderer.changeQueues[num, num2];
			if (list.Count >= MapRenderer.ChangeRefreshThreshold)
			{
				Main.refreshMap = true;
				return;
			}
			list.Add(new Point16(x, y));
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000AB74 File Offset: 0x00008D74
		public static void DrawToMap_Section(int secX, int secY)
		{
			if (MapRenderer.mapSectionTexture == null)
			{
				MapRenderer.mapSectionTexture = new RenderTarget2D(MapRenderer.GraphicsDevice, 200, 150);
			}
			Stopwatch stopwatch = Stopwatch.StartNew();
			Color[] mapColorCacheArray = MapRenderer._mapColorCacheArray;
			int num = secX * 200;
			int num2 = num + 200;
			int num3 = secY * 150;
			int num4 = num3 + 150;
			int num5 = num / MapRenderer.textureMaxWidth;
			int num6 = num3 / MapRenderer.textureMaxHeight;
			int num7 = num % MapRenderer.textureMaxWidth;
			int num8 = num3 % MapRenderer.textureMaxHeight;
			if (!MapRenderer.checkMap(num5, num6))
			{
				return;
			}
			int num9 = 0;
			Color transparent = Color.Transparent;
			for (int i = num3; i < num4; i++)
			{
				for (int j = num; j < num2; j++)
				{
					MapTile tile = Main.Map[j, i];
					mapColorCacheArray[num9] = MapHelper.GetMapTileXnaColor(tile);
					num9++;
				}
			}
			try
			{
				MapRenderer.GraphicsDevice.SetRenderTarget(MapRenderer.mapTarget[num5, num6]);
			}
			catch (ObjectDisposedException)
			{
				MapRenderer.initMap[num5, num6] = false;
				return;
			}
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			double totalMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
			MapRenderer.mapSectionTexture.SetData<Color>(mapColorCacheArray, 0, mapColorCacheArray.Length);
			double totalMilliseconds2 = stopwatch.Elapsed.TotalMilliseconds;
			totalMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
			Main.spriteBatch.Draw(MapRenderer.mapSectionTexture, new Vector2((float)num7, (float)num8), Color.White);
			Main.spriteBatch.End();
			MapRenderer.GraphicsDevice.SetRenderTarget(null);
			double totalMilliseconds3 = stopwatch.Elapsed.TotalMilliseconds;
			stopwatch.Stop();
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000AD30 File Offset: 0x00008F30
		public static void CheckMapTargets()
		{
			for (int i = 0; i < MapRenderer.numTargetsX; i++)
			{
				for (int j = 0; j < MapRenderer.numTargetsY; j++)
				{
					if (MapRenderer.mapTarget[i, j] != null)
					{
						if (MapRenderer.mapTarget[i, j].IsContentLost && !MapRenderer.mapWasContentLost[i, j])
						{
							MapRenderer.mapWasContentLost[i, j] = true;
							Main.refreshMap = true;
							Main.clearMap = true;
						}
						else if (!MapRenderer.mapTarget[i, j].IsContentLost && MapRenderer.mapWasContentLost[i, j])
						{
							MapRenderer.mapWasContentLost[i, j] = false;
						}
					}
				}
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000ADE0 File Offset: 0x00008FE0
		public static void DrawMap(float X, float Y, float minSizeX, float maxSizeX, float minSizeY, float maxSizeY, float xOff, float yOff, float scale, byte alpha)
		{
			int num = Main.maxTilesX / MapRenderer.textureMaxWidth;
			int num2 = Main.maxTilesY / MapRenderer.textureMaxHeight;
			float num3 = (float)MapRenderer.textureMaxWidth * scale;
			float num4 = (float)MapRenderer.textureMaxHeight * scale;
			float num5 = X;
			float num6 = 0f;
			for (int i = 0; i <= 4; i++)
			{
				if ((float)((i + 1) * MapRenderer.textureMaxWidth) > minSizeX && (float)(i * MapRenderer.textureMaxWidth) < minSizeX + maxSizeX)
				{
					for (int j = 0; j <= num2; j++)
					{
						if ((float)((j + 1) * MapRenderer.textureMaxHeight) > minSizeY && (float)(j * MapRenderer.textureMaxHeight) < minSizeY + maxSizeY)
						{
							float num7 = X + (float)((int)((float)i * num3));
							float num8 = Y + (float)((int)((float)j * num4));
							float num9 = (float)(i * MapRenderer.textureMaxWidth);
							float num10 = (float)(j * MapRenderer.textureMaxHeight);
							float num11 = 0f;
							float num12 = 0f;
							if (num9 < minSizeX)
							{
								num11 = minSizeX - num9;
							}
							else
							{
								num7 -= minSizeX * scale;
							}
							if (num10 < minSizeY)
							{
								num12 = minSizeY - num10;
								num8 = Y;
							}
							else
							{
								num8 -= minSizeY * scale;
							}
							num7 = num5;
							float num13 = (float)MapRenderer.textureMaxWidth;
							float num14 = (float)MapRenderer.textureMaxHeight;
							float num15 = (float)((i + 1) * MapRenderer.textureMaxWidth);
							float num16 = (float)((j + 1) * MapRenderer.textureMaxHeight);
							if (num15 >= maxSizeX)
							{
								num13 -= num15 - maxSizeX;
							}
							if (num16 >= maxSizeY)
							{
								num14 -= num16 - maxSizeY;
							}
							if (num13 > num11)
							{
								Rectangle value = new Rectangle((int)num11, (int)num12, (int)num13 - (int)num11, (int)num14 - (int)num12);
								Main.spriteBatch.Draw(MapRenderer.mapTarget[i, j], new Vector2(num7 + xOff, num8 + yOff), new Rectangle?(value), new Color((int)alpha, (int)alpha, (int)alpha, (int)alpha), 0f, default(Vector2), scale, SpriteEffects.None, 0f);
							}
							num6 = (float)((int)num13 - (int)num11) * scale;
						}
						if (j == num2)
						{
							num5 += num6;
						}
					}
				}
			}
		}

		// Token: 0x04000044 RID: 68
		public static readonly int textureMaxWidth = 2000;

		// Token: 0x04000045 RID: 69
		public static readonly int textureMaxHeight = 1800;

		// Token: 0x04000046 RID: 70
		private static readonly int numTargetsX = 5;

		// Token: 0x04000047 RID: 71
		private static readonly int numTargetsY = 2;

		// Token: 0x04000048 RID: 72
		private static readonly Color[] _mapColorCacheArray = new Color[30000];

		// Token: 0x04000049 RID: 73
		private static RenderTarget2D mapSectionTexture;

		// Token: 0x0400004A RID: 74
		private static readonly RenderTarget2D[,] mapTarget = new RenderTarget2D[MapRenderer.numTargetsX, MapRenderer.numTargetsY];

		// Token: 0x0400004B RID: 75
		private static readonly bool[,] initMap = new bool[MapRenderer.numTargetsX, MapRenderer.numTargetsY];

		// Token: 0x0400004C RID: 76
		private static readonly bool[,] mapWasContentLost = new bool[MapRenderer.numTargetsX, MapRenderer.numTargetsY];

		// Token: 0x0400004D RID: 77
		private static readonly List<Point16>[,] changeQueues = new List<Point16>[MapRenderer.numTargetsX, MapRenderer.numTargetsY];

		// Token: 0x0400004E RID: 78
		private static readonly int ChangeRefreshThreshold = 2048;
	}
}
