using System;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon
{
	// Token: 0x0200049F RID: 1183
	public class DungeonShapes
	{
		// Token: 0x02000983 RID: 2435
		public class CircleRoom : GenShape
		{
			// Token: 0x1700058C RID: 1420
			// (get) Token: 0x06004941 RID: 18753 RVA: 0x006CF6FF File Offset: 0x006CD8FF
			public int VerticalRadius
			{
				get
				{
					return this._verticalRadius;
				}
			}

			// Token: 0x1700058D RID: 1421
			// (get) Token: 0x06004942 RID: 18754 RVA: 0x006CF707 File Offset: 0x006CD907
			public int HorizontalRadius
			{
				get
				{
					return this._horizontalRadius;
				}
			}

			// Token: 0x06004943 RID: 18755 RVA: 0x006CF70F File Offset: 0x006CD90F
			public CircleRoom(int radius)
			{
				this._verticalRadius = radius;
				this._horizontalRadius = radius;
			}

			// Token: 0x06004944 RID: 18756 RVA: 0x006CF725 File Offset: 0x006CD925
			public CircleRoom(int horizontalRadius, int verticalRadius)
			{
				this._horizontalRadius = horizontalRadius;
				this._verticalRadius = verticalRadius;
			}

			// Token: 0x06004945 RID: 18757 RVA: 0x006CF73B File Offset: 0x006CD93B
			public void SetRadius(int radius)
			{
				this._verticalRadius = radius;
				this._horizontalRadius = radius;
			}

			// Token: 0x06004946 RID: 18758 RVA: 0x006CF74C File Offset: 0x006CD94C
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

			// Token: 0x040075F6 RID: 30198
			private int _verticalRadius;

			// Token: 0x040075F7 RID: 30199
			private int _horizontalRadius;
		}

		// Token: 0x02000984 RID: 2436
		public class MoundRoom : GenShape
		{
			// Token: 0x06004947 RID: 18759 RVA: 0x006CF800 File Offset: 0x006CDA00
			public MoundRoom(int halfWidth, int height)
			{
				this._halfWidth = halfWidth;
				this._height = height;
			}

			// Token: 0x06004948 RID: 18760 RVA: 0x006CF818 File Offset: 0x006CDA18
			public override bool Perform(Point origin, GenAction action)
			{
				int height = this._height;
				float num = (float)this._halfWidth;
				int num2 = this._height / 2;
				for (int i = -this._halfWidth; i <= this._halfWidth; i++)
				{
					int num3 = Math.Min(this._height, (int)(-((float)(this._height + 1) / (num * num)) * ((float)i + num) * ((float)i - num)));
					for (int j = 0; j < num3; j++)
					{
						if (!base.UnitApply(action, origin, i + origin.X, origin.Y - j + num2, new object[0]) && this._quitOnFail)
						{
							return false;
						}
					}
				}
				return true;
			}

			// Token: 0x040075F8 RID: 30200
			private int _halfWidth;

			// Token: 0x040075F9 RID: 30201
			private int _height;
		}

		// Token: 0x02000985 RID: 2437
		public class HourglassRoom : GenShape
		{
			// Token: 0x06004949 RID: 18761 RVA: 0x006CF8B7 File Offset: 0x006CDAB7
			public HourglassRoom(int width, int height, float percentileAddon)
			{
				this._width = width;
				this._height = height;
				this._percentileAddon = percentileAddon;
			}

			// Token: 0x0600494A RID: 18762 RVA: 0x006CF8D4 File Offset: 0x006CDAD4
			public override bool Perform(Point origin, GenAction action)
			{
				int num = this._height / 2;
				for (int i = -num; i <= num; i++)
				{
					int y = origin.Y + i;
					float percent = ((float)i + (float)num) / (float)this._height;
					float num2 = Math.Max(0f, Math.Min(1f, Utils.MultiLerp(Utils.WrappedLerp(0f, 1f, percent), new float[]
					{
						1f,
						1f,
						0.75f,
						0.65f,
						0.45f,
						0.4f,
						0.35f,
						0.35f
					}) + this._percentileAddon));
					int num3 = (int)((float)this._width * num2) / 2;
					for (int j = -num3; j <= num3; j++)
					{
						int x = origin.X + j;
						if (!base.UnitApply(action, origin, x, y, new object[0]) && this._quitOnFail)
						{
							return false;
						}
					}
				}
				return true;
			}

			// Token: 0x040075FA RID: 30202
			private int _width;

			// Token: 0x040075FB RID: 30203
			private int _height;

			// Token: 0x040075FC RID: 30204
			private float _percentileAddon;
		}

		// Token: 0x02000986 RID: 2438
		public class QuadCircleRoom : GenShape
		{
			// Token: 0x1700058E RID: 1422
			// (get) Token: 0x0600494B RID: 18763 RVA: 0x006CF9A2 File Offset: 0x006CDBA2
			public int Radius
			{
				get
				{
					return this._radius;
				}
			}

			// Token: 0x0600494C RID: 18764 RVA: 0x006CF9AA File Offset: 0x006CDBAA
			public QuadCircleRoom(int radius, int distanceBetweenSpheres)
			{
				this._radius = radius;
				this._distanceBetweenSpheres = distanceBetweenSpheres;
			}

			// Token: 0x0600494D RID: 18765 RVA: 0x006CF9C0 File Offset: 0x006CDBC0
			public void SetRadius(int radius)
			{
				this._radius = radius;
			}

			// Token: 0x0600494E RID: 18766 RVA: 0x006CF9CC File Offset: 0x006CDBCC
			public override bool Perform(Point origin, GenAction action)
			{
				int num = (this._radius + 1) * (this._radius + 1);
				int num2 = 3;
				for (int i = 0; i < 5; i++)
				{
					Point point;
					switch (i)
					{
					default:
						point = new Vector2((float)origin.X, (float)(origin.Y - this._distanceBetweenSpheres + num2)).ToPoint();
						break;
					case 1:
						point = new Vector2((float)origin.X, (float)(origin.Y + this._distanceBetweenSpheres - num2)).ToPoint();
						break;
					case 2:
						point = new Vector2((float)(origin.X - this._distanceBetweenSpheres + num2), (float)origin.Y).ToPoint();
						break;
					case 3:
						point = new Vector2((float)(origin.X + this._distanceBetweenSpheres - num2), (float)origin.Y).ToPoint();
						break;
					case 4:
						point = origin;
						break;
					}
					for (int j = point.Y - this._radius; j <= point.Y + this._radius; j++)
					{
						double num3 = (double)this._radius / (double)this._radius * (double)(j - point.Y);
						int num4 = Math.Min(this._radius, (int)Math.Sqrt((double)num - num3 * num3));
						for (int k = point.X - num4; k <= point.X + num4; k++)
						{
							if (!base.UnitApply(action, origin, k, j, new object[0]) && this._quitOnFail)
							{
								return false;
							}
						}
					}
				}
				return true;
			}

			// Token: 0x040075FD RID: 30205
			private int _radius;

			// Token: 0x040075FE RID: 30206
			private int _distanceBetweenSpheres;
		}
	}
}
