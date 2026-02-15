using System;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	// Token: 0x0200053B RID: 1339
	public class MultiPointHitbox
	{
		// Token: 0x06003746 RID: 14150 RVA: 0x0062DBE8 File Offset: 0x0062BDE8
		public MultiPointHitbox(Point pointSize, Vector2[] points)
		{
			this.PointSize = pointSize;
			this.Points = points;
			Rectangle rectangle = Utils.CenteredRectangle(points[0], Vector2.Zero);
			foreach (Vector2 v in points)
			{
				rectangle = rectangle.Including(v.ToPoint());
			}
			this.BoundingRect = rectangle;
		}

		// Token: 0x06003747 RID: 14151 RVA: 0x0062DC48 File Offset: 0x0062BE48
		public bool Intersects(Rectangle targetRect)
		{
			targetRect.Inflate(this.PointSize.X / 2, this.PointSize.Y / 2);
			if (!this.BoundingRect.Intersects(targetRect))
			{
				return false;
			}
			foreach (Vector2 v in this.Points)
			{
				if (targetRect.Contains(v.ToPoint()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005B61 RID: 23393
		public readonly Point PointSize;

		// Token: 0x04005B62 RID: 23394
		public readonly Vector2[] Points;

		// Token: 0x04005B63 RID: 23395
		public readonly Rectangle BoundingRect;
	}
}
