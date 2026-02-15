using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000B7 RID: 183
	public static class Shapes
	{
		// Token: 0x020006DE RID: 1758
		public class Circle : GenShape
		{
			// Token: 0x06003F2D RID: 16173 RVA: 0x00698722 File Offset: 0x00696922
			public Circle(int radius)
			{
				this._verticalRadius = radius;
				this._horizontalRadius = radius;
			}

			// Token: 0x06003F2E RID: 16174 RVA: 0x00698738 File Offset: 0x00696938
			public Circle(int horizontalRadius, int verticalRadius)
			{
				this._horizontalRadius = horizontalRadius;
				this._verticalRadius = verticalRadius;
			}

			// Token: 0x06003F2F RID: 16175 RVA: 0x0069874E File Offset: 0x0069694E
			public void SetRadius(int radius)
			{
				this._verticalRadius = radius;
				this._horizontalRadius = radius;
			}

			// Token: 0x06003F30 RID: 16176 RVA: 0x00698760 File Offset: 0x00696960
			public override bool Perform(Point origin, GenAction action)
			{
				int num = (this._horizontalRadius + 1) * (this._horizontalRadius + 1);
				for (int i = origin.Y - this._verticalRadius; i <= origin.Y + this._verticalRadius; i++)
				{
					double num2 = (double)this._horizontalRadius / (double)this._verticalRadius * (double)(i - origin.Y);
					int num3 = Math.Min(this._horizontalRadius, (int)Math.Sqrt((double)num - num2 * num2));
					for (int j = origin.X - num3; j <= origin.X + num3; j++)
					{
						if (!base.UnitApply(action, origin, j, i, new object[0]) && this._quitOnFail)
						{
							return false;
						}
					}
				}
				return true;
			}

			// Token: 0x04006799 RID: 26521
			private int _verticalRadius;

			// Token: 0x0400679A RID: 26522
			private int _horizontalRadius;
		}

		// Token: 0x020006DF RID: 1759
		public class HalfCircle : GenShape
		{
			// Token: 0x06003F31 RID: 16177 RVA: 0x00698814 File Offset: 0x00696A14
			public HalfCircle(int radius, bool bottomHalf = false)
			{
				this._radius = radius;
				this._bottomHalf = bottomHalf;
			}

			// Token: 0x06003F32 RID: 16178 RVA: 0x0069882C File Offset: 0x00696A2C
			public override bool Perform(Point origin, GenAction action)
			{
				int num = (this._radius + 1) * (this._radius + 1);
				int num2 = origin.Y - this._radius;
				int num3 = origin.Y;
				int num4 = 0;
				if (this._bottomHalf)
				{
					num2 = origin.Y;
					num3 = origin.Y + this._radius;
					num4 = -this._radius;
				}
				for (int i = num2; i <= num3; i++)
				{
					int num5 = Math.Min(this._radius, (int)Math.Sqrt((double)(num - (i - origin.Y) * (i - origin.Y))));
					int y = i + num4;
					for (int j = origin.X - num5; j <= origin.X + num5; j++)
					{
						if (!base.UnitApply(action, origin, j, y, new object[0]) && this._quitOnFail)
						{
							return false;
						}
					}
				}
				return true;
			}

			// Token: 0x0400679B RID: 26523
			private int _radius;

			// Token: 0x0400679C RID: 26524
			private bool _bottomHalf;
		}

		// Token: 0x020006E0 RID: 1760
		public class Slime : GenShape
		{
			// Token: 0x06003F33 RID: 16179 RVA: 0x00698905 File Offset: 0x00696B05
			public Slime(int radius)
			{
				this._radius = radius;
				this._xScale = 1.0;
				this._yScale = 1.0;
			}

			// Token: 0x06003F34 RID: 16180 RVA: 0x00698932 File Offset: 0x00696B32
			public Slime(int radius, double xScale, double yScale)
			{
				this._radius = radius;
				this._xScale = xScale;
				this._yScale = yScale;
			}

			// Token: 0x06003F35 RID: 16181 RVA: 0x00698950 File Offset: 0x00696B50
			public override bool Perform(Point origin, GenAction action)
			{
				double num = (double)this._radius;
				int num2 = (this._radius + 1) * (this._radius + 1);
				for (int i = origin.Y - (int)(num * this._yScale); i <= origin.Y; i++)
				{
					double num3 = (double)(i - origin.Y) / this._yScale;
					int num4 = (int)Math.Min((double)this._radius * this._xScale, this._xScale * Math.Sqrt((double)num2 - num3 * num3));
					for (int j = origin.X - num4; j <= origin.X + num4; j++)
					{
						if (!base.UnitApply(action, origin, j, i, new object[0]) && this._quitOnFail)
						{
							return false;
						}
					}
				}
				for (int k = origin.Y + 1; k <= origin.Y + (int)(num * this._yScale * 0.5) - 1; k++)
				{
					double num5 = (double)(k - origin.Y) * (2.0 / this._yScale);
					int num6 = (int)Math.Min((double)this._radius * this._xScale, this._xScale * Math.Sqrt((double)num2 - num5 * num5));
					for (int l = origin.X - num6; l <= origin.X + num6; l++)
					{
						if (!base.UnitApply(action, origin, l, k, new object[0]) && this._quitOnFail)
						{
							return false;
						}
					}
				}
				return true;
			}

			// Token: 0x0400679D RID: 26525
			private int _radius;

			// Token: 0x0400679E RID: 26526
			private double _xScale;

			// Token: 0x0400679F RID: 26527
			private double _yScale;
		}

		// Token: 0x020006E1 RID: 1761
		public class Rectangle : GenShape
		{
			// Token: 0x06003F36 RID: 16182 RVA: 0x00698ACE File Offset: 0x00696CCE
			public Rectangle(Microsoft.Xna.Framework.Rectangle area)
			{
				this._area = area;
			}

			// Token: 0x06003F37 RID: 16183 RVA: 0x00698ADD File Offset: 0x00696CDD
			public Rectangle(int width, int height)
			{
				this._area = new Microsoft.Xna.Framework.Rectangle(0, 0, width, height);
			}

			// Token: 0x06003F38 RID: 16184 RVA: 0x00698AF4 File Offset: 0x00696CF4
			public void SetArea(Microsoft.Xna.Framework.Rectangle area)
			{
				this._area = area;
			}

			// Token: 0x06003F39 RID: 16185 RVA: 0x00698B00 File Offset: 0x00696D00
			public override bool Perform(Point origin, GenAction action)
			{
				for (int i = origin.X + this._area.Left; i < origin.X + this._area.Right; i++)
				{
					for (int j = origin.Y + this._area.Top; j < origin.Y + this._area.Bottom; j++)
					{
						if (!base.UnitApply(action, origin, i, j, new object[0]) && this._quitOnFail)
						{
							return false;
						}
					}
				}
				return true;
			}

			// Token: 0x040067A0 RID: 26528
			private Microsoft.Xna.Framework.Rectangle _area;
		}

		// Token: 0x020006E2 RID: 1762
		public class Tail : GenShape
		{
			// Token: 0x06003F3A RID: 16186 RVA: 0x00698B86 File Offset: 0x00696D86
			public Tail(double width, Vector2D endOffset)
			{
				this._width = width * 16.0;
				this._endOffset = endOffset * 16.0;
			}

			// Token: 0x06003F3B RID: 16187 RVA: 0x00698BB4 File Offset: 0x00696DB4
			public override bool Perform(Point origin, GenAction action)
			{
				Vector2D vector2D = new Vector2D((double)(origin.X << 4), (double)(origin.Y << 4));
				return Utils.PlotTileTale(vector2D, vector2D + this._endOffset, this._width, (int x, int y) => this.UnitApply(action, origin, x, y, new object[0]) || !this._quitOnFail);
			}

			// Token: 0x040067A1 RID: 26529
			private double _width;

			// Token: 0x040067A2 RID: 26530
			private Vector2D _endOffset;
		}

		// Token: 0x020006E3 RID: 1763
		public class Mound : GenShape
		{
			// Token: 0x06003F3C RID: 16188 RVA: 0x00698C20 File Offset: 0x00696E20
			public Mound(int halfWidth, int height)
			{
				this._halfWidth = halfWidth;
				this._height = height;
			}

			// Token: 0x06003F3D RID: 16189 RVA: 0x00698C38 File Offset: 0x00696E38
			public override bool Perform(Point origin, GenAction action)
			{
				int height = this._height;
				double num = (double)this._halfWidth;
				for (int i = -this._halfWidth; i <= this._halfWidth; i++)
				{
					int num2 = Math.Min(this._height, (int)(-((double)(this._height + 1) / (num * num)) * ((double)i + num) * ((double)i - num)));
					for (int j = 0; j < num2; j++)
					{
						if (!base.UnitApply(action, origin, i + origin.X, origin.Y - j, new object[0]) && this._quitOnFail)
						{
							return false;
						}
					}
				}
				return true;
			}

			// Token: 0x040067A3 RID: 26531
			private int _halfWidth;

			// Token: 0x040067A4 RID: 26532
			private int _height;
		}
	}
}
