using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000AB RID: 171
	public static class ModShapes
	{
		// Token: 0x020006D4 RID: 1748
		public class All : GenModShape
		{
			// Token: 0x06003F16 RID: 16150 RVA: 0x0069812C File Offset: 0x0069632C
			public All(ShapeData data) : base(data)
			{
			}

			// Token: 0x06003F17 RID: 16151 RVA: 0x00698138 File Offset: 0x00696338
			public override bool Perform(Point origin, GenAction action)
			{
				foreach (Point16 point in this._data.GetData())
				{
					if (!base.UnitApply(action, origin, (int)point.X + origin.X, (int)point.Y + origin.Y, new object[0]) && this._quitOnFail)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x020006D5 RID: 1749
		public class OuterOutline : GenModShape
		{
			// Token: 0x06003F18 RID: 16152 RVA: 0x006981C4 File Offset: 0x006963C4
			public OuterOutline(ShapeData data, bool useDiagonals = true, bool useInterior = false) : base(data)
			{
				this._useDiagonals = useDiagonals;
				this._useInterior = useInterior;
			}

			// Token: 0x06003F19 RID: 16153 RVA: 0x006981DC File Offset: 0x006963DC
			public override bool Perform(Point origin, GenAction action)
			{
				int num = this._useDiagonals ? 16 : 8;
				foreach (Point16 point in this._data.GetData())
				{
					if (this._useInterior && !base.UnitApply(action, origin, (int)point.X + origin.X, (int)point.Y + origin.Y, new object[0]) && this._quitOnFail)
					{
						return false;
					}
					for (int i = 0; i < num; i += 2)
					{
						if (!this._data.Contains((int)point.X + ModShapes.OuterOutline.POINT_OFFSETS[i], (int)point.Y + ModShapes.OuterOutline.POINT_OFFSETS[i + 1]) && !base.UnitApply(action, origin, origin.X + (int)point.X + ModShapes.OuterOutline.POINT_OFFSETS[i], origin.Y + (int)point.Y + ModShapes.OuterOutline.POINT_OFFSETS[i + 1], new object[0]) && this._quitOnFail)
						{
							return false;
						}
					}
				}
				return true;
			}

			// Token: 0x0400678C RID: 26508
			private static readonly int[] POINT_OFFSETS = new int[]
			{
				1,
				0,
				-1,
				0,
				0,
				1,
				0,
				-1,
				1,
				1,
				1,
				-1,
				-1,
				1,
				-1,
				-1
			};

			// Token: 0x0400678D RID: 26509
			private bool _useDiagonals;

			// Token: 0x0400678E RID: 26510
			private bool _useInterior;
		}

		// Token: 0x020006D6 RID: 1750
		public class InnerOutline : GenModShape
		{
			// Token: 0x06003F1B RID: 16155 RVA: 0x00698329 File Offset: 0x00696529
			public InnerOutline(ShapeData data, bool useDiagonals = true) : base(data)
			{
				this._useDiagonals = useDiagonals;
			}

			// Token: 0x06003F1C RID: 16156 RVA: 0x0069833C File Offset: 0x0069653C
			public override bool Perform(Point origin, GenAction action)
			{
				int num = this._useDiagonals ? 16 : 8;
				foreach (Point16 point in this._data.GetData())
				{
					bool flag = false;
					for (int i = 0; i < num; i += 2)
					{
						if (!this._data.Contains((int)point.X + ModShapes.InnerOutline.POINT_OFFSETS[i], (int)point.Y + ModShapes.InnerOutline.POINT_OFFSETS[i + 1]))
						{
							flag = true;
							break;
						}
					}
					if (flag && !base.UnitApply(action, origin, (int)point.X + origin.X, (int)point.Y + origin.Y, new object[0]) && this._quitOnFail)
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x0400678F RID: 26511
			private static readonly int[] POINT_OFFSETS = new int[]
			{
				1,
				0,
				-1,
				0,
				0,
				1,
				0,
				-1,
				1,
				1,
				1,
				-1,
				-1,
				1,
				-1,
				-1
			};

			// Token: 0x04006790 RID: 26512
			private bool _useDiagonals;
		}
	}
}
