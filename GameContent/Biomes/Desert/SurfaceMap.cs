using System;
using Terraria.ID;

namespace Terraria.GameContent.Biomes.Desert
{
	// Token: 0x0200051B RID: 1307
	public class SurfaceMap
	{
		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06003675 RID: 13941 RVA: 0x00627584 File Offset: 0x00625784
		public int Width
		{
			get
			{
				return this._heights.Length;
			}
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x00627590 File Offset: 0x00625790
		private SurfaceMap(short[] heights, int x)
		{
			this._heights = heights;
			this.X = x;
			int num = 0;
			int num2 = int.MaxValue;
			int num3 = 0;
			for (int i = 0; i < heights.Length; i++)
			{
				num3 += (int)heights[i];
				num = Math.Max(num, (int)heights[i]);
				num2 = Math.Min(num2, (int)heights[i]);
			}
			if ((double)num > Main.worldSurface - 10.0)
			{
				num = (int)Main.worldSurface - 10;
			}
			this.Bottom = num;
			this.Top = num2;
			this.Average = (double)num3 / (double)this._heights.Length;
		}

		// Token: 0x17000453 RID: 1107
		public short this[int absoluteX]
		{
			get
			{
				return this._heights[absoluteX - this.X];
			}
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x00627634 File Offset: 0x00625834
		public static SurfaceMap FromArea(int startX, int width)
		{
			int num = Main.maxTilesY / 2;
			short[] array = new short[width];
			for (int i = startX; i < startX + width; i++)
			{
				bool flag = false;
				int num2 = 0;
				for (int j = 50; j < 50 + num; j++)
				{
					if (Main.tile[i, j].active())
					{
						if (TileID.Sets.Clouds[(int)Main.tile[i, j].type])
						{
							flag = false;
						}
						else if (!flag)
						{
							num2 = j;
							flag = true;
						}
					}
					if (!flag)
					{
						num2 = num + 50;
					}
				}
				array[i - startX] = (short)num2;
			}
			return new SurfaceMap(array, startX);
		}

		// Token: 0x04005B01 RID: 23297
		public readonly double Average;

		// Token: 0x04005B02 RID: 23298
		public readonly int Bottom;

		// Token: 0x04005B03 RID: 23299
		public readonly int Top;

		// Token: 0x04005B04 RID: 23300
		public readonly int X;

		// Token: 0x04005B05 RID: 23301
		private readonly short[] _heights;
	}
}
