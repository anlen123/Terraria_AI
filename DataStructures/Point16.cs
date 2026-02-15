using System;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	// Token: 0x020005AA RID: 1450
	public struct Point16
	{
		// Token: 0x0600394B RID: 14667 RVA: 0x00650A36 File Offset: 0x0064EC36
		public Point16(Point point)
		{
			this.X = (short)point.X;
			this.Y = (short)point.Y;
		}

		// Token: 0x0600394C RID: 14668 RVA: 0x00650A52 File Offset: 0x0064EC52
		public Point16(int X, int Y)
		{
			this.X = (short)X;
			this.Y = (short)Y;
		}

		// Token: 0x0600394D RID: 14669 RVA: 0x00650A64 File Offset: 0x0064EC64
		public Point16(short X, short Y)
		{
			this.X = X;
			this.Y = Y;
		}

		// Token: 0x0600394E RID: 14670 RVA: 0x00650A74 File Offset: 0x0064EC74
		public static Point16 Max(int firstX, int firstY, int secondX, int secondY)
		{
			return new Point16((firstX > secondX) ? firstX : secondX, (firstY > secondY) ? firstY : secondY);
		}

		// Token: 0x0600394F RID: 14671 RVA: 0x00650A8B File Offset: 0x0064EC8B
		public Point16 Max(int compareX, int compareY)
		{
			return new Point16(((int)this.X > compareX) ? ((int)this.X) : compareX, ((int)this.Y > compareY) ? ((int)this.Y) : compareY);
		}

		// Token: 0x06003950 RID: 14672 RVA: 0x00650AB6 File Offset: 0x0064ECB6
		public Point16 Max(Point16 compareTo)
		{
			return new Point16((this.X > compareTo.X) ? this.X : compareTo.X, (this.Y > compareTo.Y) ? this.Y : compareTo.Y);
		}

		// Token: 0x06003951 RID: 14673 RVA: 0x00650AF5 File Offset: 0x0064ECF5
		public static bool operator ==(Point16 first, Point16 second)
		{
			return first.X == second.X && first.Y == second.Y;
		}

		// Token: 0x06003952 RID: 14674 RVA: 0x00650B15 File Offset: 0x0064ED15
		public static bool operator !=(Point16 first, Point16 second)
		{
			return first.X != second.X || first.Y != second.Y;
		}

		// Token: 0x06003953 RID: 14675 RVA: 0x00650B38 File Offset: 0x0064ED38
		public override bool Equals(object obj)
		{
			Point16 point = (Point16)obj;
			return this.X == point.X && this.Y == point.Y;
		}

		// Token: 0x06003954 RID: 14676 RVA: 0x00650B6B File Offset: 0x0064ED6B
		public override int GetHashCode()
		{
			return (int)this.X << 16 | (int)((ushort)this.Y);
		}

		// Token: 0x06003955 RID: 14677 RVA: 0x00650B7E File Offset: 0x0064ED7E
		public override string ToString()
		{
			return string.Format("{{{0}, {1}}}", this.X, this.Y);
		}

		// Token: 0x06003956 RID: 14678 RVA: 0x00650BA0 File Offset: 0x0064EDA0
		public static implicit operator Point(Point16 p)
		{
			return new Point((int)p.X, (int)p.Y);
		}

		// Token: 0x04005D6A RID: 23914
		public short X;

		// Token: 0x04005D6B RID: 23915
		public short Y;

		// Token: 0x04005D6C RID: 23916
		public static Point16 Zero = new Point16(0, 0);

		// Token: 0x04005D6D RID: 23917
		public static Point16 NegativeOne = new Point16(-1, -1);
	}
}
