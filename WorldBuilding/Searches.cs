using System;
using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000AF RID: 175
	public static class Searches
	{
		// Token: 0x06001752 RID: 5970 RVA: 0x004DD4C3 File Offset: 0x004DB6C3
		public static GenSearch Chain(GenSearch search, params GenCondition[] conditions)
		{
			return search.Conditions(conditions);
		}

		// Token: 0x020006D9 RID: 1753
		public class Left : GenSearch
		{
			// Token: 0x06003F23 RID: 16163 RVA: 0x00698529 File Offset: 0x00696729
			public Left(int maxDistance)
			{
				this._maxDistance = maxDistance;
			}

			// Token: 0x06003F24 RID: 16164 RVA: 0x00698538 File Offset: 0x00696738
			public override Point Find(Point origin)
			{
				for (int i = 0; i < this._maxDistance; i++)
				{
					if (base.Check(origin.X - i, origin.Y))
					{
						return new Point(origin.X - i, origin.Y);
					}
				}
				return GenSearch.NOT_FOUND;
			}

			// Token: 0x04006793 RID: 26515
			private int _maxDistance;
		}

		// Token: 0x020006DA RID: 1754
		public class Right : GenSearch
		{
			// Token: 0x06003F25 RID: 16165 RVA: 0x00698585 File Offset: 0x00696785
			public Right(int maxDistance)
			{
				this._maxDistance = maxDistance;
			}

			// Token: 0x06003F26 RID: 16166 RVA: 0x00698594 File Offset: 0x00696794
			public override Point Find(Point origin)
			{
				for (int i = 0; i < this._maxDistance; i++)
				{
					if (base.Check(origin.X + i, origin.Y))
					{
						return new Point(origin.X + i, origin.Y);
					}
				}
				return GenSearch.NOT_FOUND;
			}

			// Token: 0x04006794 RID: 26516
			private int _maxDistance;
		}

		// Token: 0x020006DB RID: 1755
		public class Down : GenSearch
		{
			// Token: 0x06003F27 RID: 16167 RVA: 0x006985E1 File Offset: 0x006967E1
			public Down(int maxDistance)
			{
				this._maxDistance = maxDistance;
			}

			// Token: 0x06003F28 RID: 16168 RVA: 0x006985F0 File Offset: 0x006967F0
			public override Point Find(Point origin)
			{
				int num = 0;
				while (num < this._maxDistance && origin.Y + num < Main.maxTilesY)
				{
					if (base.Check(origin.X, origin.Y + num))
					{
						return new Point(origin.X, origin.Y + num);
					}
					num++;
				}
				return GenSearch.NOT_FOUND;
			}

			// Token: 0x04006795 RID: 26517
			private int _maxDistance;
		}

		// Token: 0x020006DC RID: 1756
		public class Up : GenSearch
		{
			// Token: 0x06003F29 RID: 16169 RVA: 0x0069864C File Offset: 0x0069684C
			public Up(int maxDistance)
			{
				this._maxDistance = maxDistance;
			}

			// Token: 0x06003F2A RID: 16170 RVA: 0x0069865C File Offset: 0x0069685C
			public override Point Find(Point origin)
			{
				for (int i = 0; i < this._maxDistance; i++)
				{
					if (base.Check(origin.X, origin.Y - i))
					{
						return new Point(origin.X, origin.Y - i);
					}
				}
				return GenSearch.NOT_FOUND;
			}

			// Token: 0x04006796 RID: 26518
			private int _maxDistance;
		}

		// Token: 0x020006DD RID: 1757
		public class Rectangle : GenSearch
		{
			// Token: 0x06003F2B RID: 16171 RVA: 0x006986A9 File Offset: 0x006968A9
			public Rectangle(int width, int height)
			{
				this._width = width;
				this._height = height;
			}

			// Token: 0x06003F2C RID: 16172 RVA: 0x006986C0 File Offset: 0x006968C0
			public override Point Find(Point origin)
			{
				for (int i = 0; i < this._width; i++)
				{
					for (int j = 0; j < this._height; j++)
					{
						if (base.Check(origin.X + i, origin.Y + j))
						{
							return new Point(origin.X + i, origin.Y + j);
						}
					}
				}
				return GenSearch.NOT_FOUND;
			}

			// Token: 0x04006797 RID: 26519
			private int _width;

			// Token: 0x04006798 RID: 26520
			private int _height;
		}
	}
}
