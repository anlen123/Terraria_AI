using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.Modules
{
	// Token: 0x02000068 RID: 104
	public class TileObjectCoordinatesModule
	{
		// Token: 0x06001454 RID: 5204 RVA: 0x004BA9F0 File Offset: 0x004B8BF0
		public TileObjectCoordinatesModule(TileObjectCoordinatesModule copyFrom = null, int[] drawHeight = null, Rectangle[,] drawFrameOffs = null)
		{
			if (copyFrom == null)
			{
				this.width = 0;
				this.padding = 0;
				this.paddingFix = Point16.Zero;
				this.styleWidth = 0;
				this.drawStyleOffset = 0;
				this.styleHeight = 0;
				this.calculated = false;
				this.heights = drawHeight;
				this.drawFrameOffsets = drawFrameOffs;
				return;
			}
			this.width = copyFrom.width;
			this.padding = copyFrom.padding;
			this.paddingFix = copyFrom.paddingFix;
			this.drawStyleOffset = copyFrom.drawStyleOffset;
			this.styleWidth = copyFrom.styleWidth;
			this.styleHeight = copyFrom.styleHeight;
			this.calculated = copyFrom.calculated;
			if (drawHeight == null)
			{
				if (copyFrom.heights == null)
				{
					this.heights = null;
				}
				else
				{
					this.heights = new int[copyFrom.heights.Length];
					Array.Copy(copyFrom.heights, this.heights, this.heights.Length);
				}
			}
			else
			{
				this.heights = drawHeight;
			}
			if (drawFrameOffs != null)
			{
				this.drawFrameOffsets = drawFrameOffs;
				return;
			}
			if (copyFrom.drawFrameOffsets == null)
			{
				this.drawFrameOffsets = null;
				return;
			}
			this.drawFrameOffsets = new Rectangle[copyFrom.drawFrameOffsets.GetLength(0), copyFrom.drawFrameOffsets.GetLength(1)];
			Array.Copy(copyFrom.drawFrameOffsets, this.drawFrameOffsets, this.drawFrameOffsets.Length);
		}

		// Token: 0x04001057 RID: 4183
		public int width;

		// Token: 0x04001058 RID: 4184
		public int[] heights;

		// Token: 0x04001059 RID: 4185
		public int padding;

		// Token: 0x0400105A RID: 4186
		public Point16 paddingFix;

		// Token: 0x0400105B RID: 4187
		public int styleWidth;

		// Token: 0x0400105C RID: 4188
		public int styleHeight;

		// Token: 0x0400105D RID: 4189
		public bool calculated;

		// Token: 0x0400105E RID: 4190
		public int drawStyleOffset;

		// Token: 0x0400105F RID: 4191
		public Rectangle[,] drawFrameOffsets;
	}
}
