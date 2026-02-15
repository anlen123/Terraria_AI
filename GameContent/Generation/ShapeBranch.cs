using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation
{
	// Token: 0x02000488 RID: 1160
	public class ShapeBranch : GenShape
	{
		// Token: 0x0600334C RID: 13132 RVA: 0x005F5635 File Offset: 0x005F3835
		public ShapeBranch()
		{
			this._offset = new Point(10, -5);
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x005F564C File Offset: 0x005F384C
		public ShapeBranch(Point offset)
		{
			this._offset = offset;
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x005F565B File Offset: 0x005F385B
		public ShapeBranch(double angle, double distance)
		{
			this._offset = new Point((int)(Math.Cos(angle) * distance), (int)(Math.Sin(angle) * distance));
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x005F5680 File Offset: 0x005F3880
		private bool PerformSegment(Point origin, GenAction action, Point start, Point end, int size)
		{
			size = Math.Max(1, size);
			Utils.TileActionAttempt <>9__0;
			for (int i = -(size >> 1); i < size - (size >> 1); i++)
			{
				for (int j = -(size >> 1); j < size - (size >> 1); j++)
				{
					Point p = new Point(start.X + i, start.Y + j);
					Utils.TileActionAttempt plot;
					if ((plot = <>9__0) == null)
					{
						plot = (<>9__0 = ((int tileX, int tileY) => this.UnitApply(action, origin, tileX, tileY, new object[0]) || !this._quitOnFail));
					}
					if (!Utils.PlotLine(p, end, plot, false))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x005F5720 File Offset: 0x005F3920
		public override bool Perform(Point origin, GenAction action)
		{
			Vector2D vector2D;
			vector2D..ctor((double)this._offset.X, (double)this._offset.Y);
			double num = vector2D.Length();
			int num2 = (int)(num / 6.0);
			if (this._endPoints != null)
			{
				this._endPoints.Add(new Point(origin.X + this._offset.X, origin.Y + this._offset.Y));
			}
			if (!this.PerformSegment(origin, action, origin, new Point(origin.X + this._offset.X, origin.Y + this._offset.Y), num2))
			{
				return false;
			}
			int num3 = (int)(num / 8.0);
			for (int i = 0; i < num3; i++)
			{
				double num4 = ((double)i + 1.0) / ((double)num3 + 1.0);
				Point point = new Point((int)(num4 * (double)this._offset.X), (int)(num4 * (double)this._offset.Y));
				Vector2D vector2D2;
				vector2D2..ctor((double)(this._offset.X - point.X), (double)(this._offset.Y - point.Y));
				vector2D2 = vector2D2.RotatedBy((GenBase._random.NextDouble() * 0.5 + 1.0) * (double)((GenBase._random.Next(2) == 0) ? -1 : 1), default(Vector2D)) * 0.75;
				Point point2 = new Point((int)vector2D2.X + point.X, (int)vector2D2.Y + point.Y);
				if (this._endPoints != null)
				{
					this._endPoints.Add(new Point(point2.X + origin.X, point2.Y + origin.Y));
				}
				if (!this.PerformSegment(origin, action, new Point(point.X + origin.X, point.Y + origin.Y), new Point(point2.X + origin.X, point2.Y + origin.Y), num2 - 1))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x005F5968 File Offset: 0x005F3B68
		public ShapeBranch OutputEndpoints(List<Point> endpoints)
		{
			this._endPoints = endpoints;
			return this;
		}

		// Token: 0x040058A4 RID: 22692
		private Point _offset;

		// Token: 0x040058A5 RID: 22693
		private List<Point> _endPoints;
	}
}
