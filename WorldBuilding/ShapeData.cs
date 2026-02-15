using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000B9 RID: 185
	public class ShapeData
	{
		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06001779 RID: 6009 RVA: 0x004DD847 File Offset: 0x004DBA47
		public int Count
		{
			get
			{
				return this._points.Count;
			}
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x004DD854 File Offset: 0x004DBA54
		public ShapeData()
		{
			this._points = new HashSet<Point16>();
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x004DD867 File Offset: 0x004DBA67
		public ShapeData(ShapeData original)
		{
			this._points = new HashSet<Point16>(original._points);
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x004DD880 File Offset: 0x004DBA80
		public void Add(int x, int y)
		{
			Point16 item = new Point16(x, y);
			if (!this._points.Contains(item))
			{
				this._points.Add(item);
			}
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x004DD8B4 File Offset: 0x004DBAB4
		public void AddBounds(int minX, int minY, int maxX, int maxY)
		{
			for (int i = minX; i <= maxX; i++)
			{
				for (int j = minY; j <= maxY; j++)
				{
					this.Add(i, j);
				}
			}
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x004DD8E4 File Offset: 0x004DBAE4
		public void Remove(int x, int y)
		{
			Point16 item = new Point16(x, y);
			if (this._points.Contains(item))
			{
				this._points.Remove(item);
			}
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x004DD918 File Offset: 0x004DBB18
		public void RemoveBounds(int minX, int minY, int maxX, int maxY)
		{
			for (int i = minX; i <= maxX; i++)
			{
				for (int j = minY; j <= maxY; j++)
				{
					this.Remove(i, j);
				}
			}
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x004DD946 File Offset: 0x004DBB46
		public HashSet<Point16> GetData()
		{
			return this._points;
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x004DD94E File Offset: 0x004DBB4E
		public void Clear()
		{
			this._points.Clear();
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x004DD95B File Offset: 0x004DBB5B
		public bool Contains(int x, int y)
		{
			return this._points.Contains(new Point16(x, y));
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x004DD970 File Offset: 0x004DBB70
		public void Add(ShapeData shapeData, Point localOrigin, Point remoteOrigin)
		{
			foreach (Point16 point in shapeData.GetData())
			{
				this.Add(remoteOrigin.X - localOrigin.X + (int)point.X, remoteOrigin.Y - localOrigin.Y + (int)point.Y);
			}
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x004DD9EC File Offset: 0x004DBBEC
		public void Subtract(ShapeData shapeData, Point localOrigin, Point remoteOrigin)
		{
			foreach (Point16 point in shapeData.GetData())
			{
				this.Remove(remoteOrigin.X - localOrigin.X + (int)point.X, remoteOrigin.Y - localOrigin.Y + (int)point.Y);
			}
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x004DDA68 File Offset: 0x004DBC68
		public static Rectangle GetBounds(Point origin, params ShapeData[] shapes)
		{
			int num = (int)shapes[0]._points.First<Point16>().X;
			int num2 = num;
			int num3 = (int)shapes[0]._points.First<Point16>().Y;
			int num4 = num3;
			for (int i = 0; i < shapes.Length; i++)
			{
				foreach (Point16 point in shapes[i]._points)
				{
					num = Math.Max(num, (int)point.X);
					num2 = Math.Min(num2, (int)point.X);
					num3 = Math.Max(num3, (int)point.Y);
					num4 = Math.Min(num4, (int)point.Y);
				}
			}
			return new Rectangle(num2 + origin.X, num4 + origin.Y, num - num2, num3 - num4);
		}

		// Token: 0x04001265 RID: 4709
		private HashSet<Point16> _points;
	}
}
