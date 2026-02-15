using System;
using Microsoft.Xna.Framework;

namespace Terraria.Utilities
{
	// Token: 0x020000CA RID: 202
	public class BitSet2D
	{
		// Token: 0x060017E9 RID: 6121 RVA: 0x004E00BC File Offset: 0x004DE2BC
		public void Reset(Point center, int maxDist)
		{
			this.size = maxDist * 2 + 1;
			this.offset = new Point(center.X - maxDist, center.Y - maxDist);
			int num = this.size * this.size + 63 >> 6;
			if (this.bits == null || this.bits.Length < num)
			{
				Array.Resize<Bits64>(ref this.bits, num);
			}
			Array.Clear(this.bits, 0, this.bits.Length);
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x004E0138 File Offset: 0x004DE338
		private int Coord(Point p)
		{
			int num = p.X - this.offset.X;
			return (p.Y - this.offset.Y) * this.size + num;
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x004E0174 File Offset: 0x004DE374
		public bool InBounds(Point p)
		{
			int num = p.X - this.offset.X;
			int num2 = p.Y - this.offset.Y;
			return num >= 0 && num < this.size && num2 >= 0 && num2 < this.size;
		}

		// Token: 0x170002A4 RID: 676
		public bool this[Point p]
		{
			get
			{
				int num = this.Coord(p);
				return this.bits[num >> 6][num & 63];
			}
			set
			{
				int num = this.Coord(p);
				this.bits[num >> 6][num & 63] = value;
			}
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x004E0220 File Offset: 0x004DE420
		public bool Add(Point p)
		{
			int num = this.Coord(p);
			if (this.bits[num >> 6][num & 63])
			{
				return false;
			}
			this.bits[num >> 6][num & 63] = true;
			return true;
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x004E026C File Offset: 0x004DE46C
		public bool Remove(Point p)
		{
			int num = this.Coord(p);
			if (!this.bits[num >> 6][num & 63])
			{
				return false;
			}
			this.bits[num >> 6][num & 63] = false;
			return true;
		}

		// Token: 0x0400129E RID: 4766
		private Point offset;

		// Token: 0x0400129F RID: 4767
		private int size;

		// Token: 0x040012A0 RID: 4768
		private Bits64[] bits;
	}
}
