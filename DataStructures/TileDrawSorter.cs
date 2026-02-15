using System;
using System.Collections.Generic;

namespace Terraria.DataStructures
{
	// Token: 0x02000589 RID: 1417
	public class TileDrawSorter
	{
		// Token: 0x060037FE RID: 14334 RVA: 0x0062F73A File Offset: 0x0062D93A
		public TileDrawSorter()
		{
			this._currentCacheIndex = 0;
			this._holderLength = 9000;
			this.tilesToDraw = new TileDrawSorter.TileTexPoint[this._holderLength];
		}

		// Token: 0x060037FF RID: 14335 RVA: 0x0062F770 File Offset: 0x0062D970
		public void reset()
		{
			this._currentCacheIndex = 0;
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x0062F77C File Offset: 0x0062D97C
		public void Cache(int x, int y, int type)
		{
			int currentCacheIndex = this._currentCacheIndex;
			this._currentCacheIndex = currentCacheIndex + 1;
			int num = currentCacheIndex;
			this.tilesToDraw[num].X = x;
			this.tilesToDraw[num].Y = y;
			this.tilesToDraw[num].TileType = type;
			if (this._currentCacheIndex == this._holderLength)
			{
				this.IncreaseArraySize();
			}
		}

		// Token: 0x06003801 RID: 14337 RVA: 0x0062F7E5 File Offset: 0x0062D9E5
		private void IncreaseArraySize()
		{
			this._holderLength *= 2;
			Array.Resize<TileDrawSorter.TileTexPoint>(ref this.tilesToDraw, this._holderLength);
		}

		// Token: 0x06003802 RID: 14338 RVA: 0x0062F806 File Offset: 0x0062DA06
		public void Sort()
		{
			Array.Sort<TileDrawSorter.TileTexPoint>(this.tilesToDraw, 0, this._currentCacheIndex, this._tileComparer);
		}

		// Token: 0x06003803 RID: 14339 RVA: 0x0062F820 File Offset: 0x0062DA20
		public int GetAmountToDraw()
		{
			return this._currentCacheIndex;
		}

		// Token: 0x04005C16 RID: 23574
		public TileDrawSorter.TileTexPoint[] tilesToDraw;

		// Token: 0x04005C17 RID: 23575
		private int _holderLength;

		// Token: 0x04005C18 RID: 23576
		private int _currentCacheIndex;

		// Token: 0x04005C19 RID: 23577
		private TileDrawSorter.CustomComparer _tileComparer = new TileDrawSorter.CustomComparer();

		// Token: 0x020009BD RID: 2493
		public struct TileTexPoint
		{
			// Token: 0x06004A2E RID: 18990 RVA: 0x006D28D0 File Offset: 0x006D0AD0
			public override string ToString()
			{
				return string.Format("X:{0}, Y:{1}, Type:{2}", this.X, this.Y, this.TileType);
			}

			// Token: 0x0400769A RID: 30362
			public int X;

			// Token: 0x0400769B RID: 30363
			public int Y;

			// Token: 0x0400769C RID: 30364
			public int TileType;
		}

		// Token: 0x020009BE RID: 2494
		public class CustomComparer : Comparer<TileDrawSorter.TileTexPoint>
		{
			// Token: 0x06004A2F RID: 18991 RVA: 0x006D28FD File Offset: 0x006D0AFD
			public override int Compare(TileDrawSorter.TileTexPoint x, TileDrawSorter.TileTexPoint y)
			{
				return x.TileType.CompareTo(y.TileType);
			}
		}
	}
}
