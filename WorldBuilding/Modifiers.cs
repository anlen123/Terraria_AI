using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000A9 RID: 169
	public static class Modifiers
	{
		// Token: 0x020006B9 RID: 1721
		public class ShapeScale : GenAction
		{
			// Token: 0x06003EDA RID: 16090 RVA: 0x00697638 File Offset: 0x00695838
			public ShapeScale(int scale)
			{
				this._scale = scale;
			}

			// Token: 0x06003EDB RID: 16091 RVA: 0x00697648 File Offset: 0x00695848
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				bool flag = false;
				for (int i = 0; i < this._scale; i++)
				{
					for (int j = 0; j < this._scale; j++)
					{
						flag |= !base.UnitApply(origin, (x - origin.X << 1) + i + origin.X, (y - origin.Y << 1) + j + origin.Y, new object[0]);
					}
				}
				return !flag;
			}

			// Token: 0x04006762 RID: 26466
			private int _scale;
		}

		// Token: 0x020006BA RID: 1722
		public class Expand : GenAction
		{
			// Token: 0x06003EDC RID: 16092 RVA: 0x006976B6 File Offset: 0x006958B6
			public Expand(int expansion)
			{
				this._xExpansion = expansion;
				this._yExpansion = expansion;
			}

			// Token: 0x06003EDD RID: 16093 RVA: 0x006976CC File Offset: 0x006958CC
			public Expand(int xExpansion, int yExpansion)
			{
				this._xExpansion = xExpansion;
				this._yExpansion = yExpansion;
			}

			// Token: 0x06003EDE RID: 16094 RVA: 0x006976E4 File Offset: 0x006958E4
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				bool flag = false;
				for (int i = -this._xExpansion; i <= this._xExpansion; i++)
				{
					for (int j = -this._yExpansion; j <= this._yExpansion; j++)
					{
						flag |= !base.UnitApply(origin, x + i, y + j, args);
					}
				}
				return !flag;
			}

			// Token: 0x04006763 RID: 26467
			private int _xExpansion;

			// Token: 0x04006764 RID: 26468
			private int _yExpansion;
		}

		// Token: 0x020006BB RID: 1723
		public class RadialDither : GenAction
		{
			// Token: 0x06003EDF RID: 16095 RVA: 0x0069773A File Offset: 0x0069593A
			public RadialDither(double innerRadius, double outerRadius)
			{
				this._innerRadius = innerRadius;
				this._outerRadius = outerRadius;
			}

			// Token: 0x06003EE0 RID: 16096 RVA: 0x00697750 File Offset: 0x00695950
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				Vector2D vector2D;
				vector2D..ctor((double)origin.X, (double)origin.Y);
				double num = Vector2D.Distance(new Vector2D((double)x, (double)y), vector2D);
				double num2 = Math.Max(0.0, Math.Min(1.0, (num - this._innerRadius) / (this._outerRadius - this._innerRadius)));
				if (GenBase._random.NextDouble() > num2)
				{
					return base.UnitApply(origin, x, y, args);
				}
				return base.Fail();
			}

			// Token: 0x04006765 RID: 26469
			private double _innerRadius;

			// Token: 0x04006766 RID: 26470
			private double _outerRadius;
		}

		// Token: 0x020006BC RID: 1724
		public class Blotches : GenAction
		{
			// Token: 0x06003EE1 RID: 16097 RVA: 0x006977D4 File Offset: 0x006959D4
			public Blotches(int scale = 2, double chance = 0.3)
			{
				this._minX = scale;
				this._minY = scale;
				this._maxX = scale;
				this._maxY = scale;
				this._chance = chance;
			}

			// Token: 0x06003EE2 RID: 16098 RVA: 0x006977FF File Offset: 0x006959FF
			public Blotches(int xScale, int yScale, double chance = 0.3)
			{
				this._minX = xScale;
				this._maxX = xScale;
				this._minY = yScale;
				this._maxY = yScale;
				this._chance = chance;
			}

			// Token: 0x06003EE3 RID: 16099 RVA: 0x0069782A File Offset: 0x00695A2A
			public Blotches(int leftScale, int upScale, int rightScale, int downScale, double chance = 0.3)
			{
				this._minX = leftScale;
				this._maxX = rightScale;
				this._minY = upScale;
				this._maxY = downScale;
				this._chance = chance;
			}

			// Token: 0x06003EE4 RID: 16100 RVA: 0x00697858 File Offset: 0x00695A58
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._random.NextDouble();
				if (GenBase._random.NextDouble() < this._chance)
				{
					bool flag = false;
					int num = GenBase._random.Next(1 - this._minX, 1);
					int num2 = GenBase._random.Next(0, this._maxX);
					int num3 = GenBase._random.Next(1 - this._minY, 1);
					int num4 = GenBase._random.Next(0, this._maxY);
					for (int i = num; i <= num2; i++)
					{
						for (int j = num3; j <= num4; j++)
						{
							flag |= !base.UnitApply(origin, x + i, y + j, args);
						}
					}
					return !flag;
				}
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x04006767 RID: 26471
			private int _minX;

			// Token: 0x04006768 RID: 26472
			private int _minY;

			// Token: 0x04006769 RID: 26473
			private int _maxX;

			// Token: 0x0400676A RID: 26474
			private int _maxY;

			// Token: 0x0400676B RID: 26475
			private double _chance;
		}

		// Token: 0x020006BD RID: 1725
		public class InShape : GenAction
		{
			// Token: 0x06003EE5 RID: 16101 RVA: 0x00697918 File Offset: 0x00695B18
			public InShape(ShapeData shapeData)
			{
				this._shapeData = shapeData;
			}

			// Token: 0x06003EE6 RID: 16102 RVA: 0x00697927 File Offset: 0x00695B27
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (!this._shapeData.Contains(x - origin.X, y - origin.Y))
				{
					return base.Fail();
				}
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x0400676C RID: 26476
			private readonly ShapeData _shapeData;
		}

		// Token: 0x020006BE RID: 1726
		public class NotInShape : GenAction
		{
			// Token: 0x06003EE7 RID: 16103 RVA: 0x00697958 File Offset: 0x00695B58
			public NotInShape(ShapeData shapeData)
			{
				this._shapeData = shapeData;
			}

			// Token: 0x06003EE8 RID: 16104 RVA: 0x00697967 File Offset: 0x00695B67
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (this._shapeData.Contains(x - origin.X, y - origin.Y))
				{
					return base.Fail();
				}
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x0400676D RID: 26477
			private readonly ShapeData _shapeData;
		}

		// Token: 0x020006BF RID: 1727
		public class Conditions : GenAction
		{
			// Token: 0x06003EE9 RID: 16105 RVA: 0x00697998 File Offset: 0x00695B98
			public Conditions(params GenCondition[] conditions)
			{
				this._conditions = conditions;
			}

			// Token: 0x06003EEA RID: 16106 RVA: 0x006979A8 File Offset: 0x00695BA8
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				bool flag = true;
				for (int i = 0; i < this._conditions.Length; i++)
				{
					flag &= this._conditions[i].IsValid(x, y);
				}
				if (flag)
				{
					return base.UnitApply(origin, x, y, args);
				}
				return base.Fail();
			}

			// Token: 0x0400676E RID: 26478
			private readonly GenCondition[] _conditions;
		}

		// Token: 0x020006C0 RID: 1728
		public class OnlyWalls : GenAction
		{
			// Token: 0x06003EEB RID: 16107 RVA: 0x006979F1 File Offset: 0x00695BF1
			public OnlyWalls(params ushort[] types)
			{
				this._types = types;
			}

			// Token: 0x06003EEC RID: 16108 RVA: 0x00697A00 File Offset: 0x00695C00
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				for (int i = 0; i < this._types.Length; i++)
				{
					if (GenBase._tiles[x, y].wall == this._types[i])
					{
						return base.UnitApply(origin, x, y, args);
					}
				}
				return base.Fail();
			}

			// Token: 0x0400676F RID: 26479
			private ushort[] _types;
		}

		// Token: 0x020006C1 RID: 1729
		public class OnlyTiles : GenAction
		{
			// Token: 0x06003EED RID: 16109 RVA: 0x00697A4D File Offset: 0x00695C4D
			public OnlyTiles(params ushort[] types)
			{
				this._types = types;
			}

			// Token: 0x06003EEE RID: 16110 RVA: 0x00697A5C File Offset: 0x00695C5C
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (!GenBase._tiles[x, y].active())
				{
					return base.Fail();
				}
				for (int i = 0; i < this._types.Length; i++)
				{
					if (GenBase._tiles[x, y].type == this._types[i])
					{
						return base.UnitApply(origin, x, y, args);
					}
				}
				return base.Fail();
			}

			// Token: 0x04006770 RID: 26480
			private ushort[] _types;
		}

		// Token: 0x020006C2 RID: 1730
		public class Checkerboard : GenAction
		{
			// Token: 0x06003EEF RID: 16111 RVA: 0x00697AC3 File Offset: 0x00695CC3
			public Checkerboard(int percentile)
			{
				this._percentile = percentile;
			}

			// Token: 0x06003EF0 RID: 16112 RVA: 0x00697AD2 File Offset: 0x00695CD2
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (x % this._percentile == 0 && y % this._percentile == 0)
				{
					return base.UnitApply(origin, x, y, args);
				}
				return base.Fail();
			}

			// Token: 0x04006771 RID: 26481
			private int _percentile;
		}

		// Token: 0x020006C3 RID: 1731
		public class IsTouching : GenAction
		{
			// Token: 0x06003EF1 RID: 16113 RVA: 0x00697AFA File Offset: 0x00695CFA
			public IsTouching(bool useDiagonals, params ushort[] tileIds)
			{
				this._useDiagonals = useDiagonals;
				this._tileIds = tileIds;
			}

			// Token: 0x06003EF2 RID: 16114 RVA: 0x00697B10 File Offset: 0x00695D10
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				int num = this._useDiagonals ? 16 : 8;
				for (int i = 0; i < num; i += 2)
				{
					Tile tile = GenBase._tiles[x + Modifiers.IsTouching.DIRECTIONS[i], y + Modifiers.IsTouching.DIRECTIONS[i + 1]];
					if (tile.active())
					{
						for (int j = 0; j < this._tileIds.Length; j++)
						{
							if (tile.type == this._tileIds[j])
							{
								return base.UnitApply(origin, x, y, args);
							}
						}
					}
				}
				return base.Fail();
			}

			// Token: 0x04006772 RID: 26482
			private static readonly int[] DIRECTIONS = new int[]
			{
				0,
				-1,
				1,
				0,
				-1,
				0,
				0,
				1,
				-1,
				-1,
				1,
				-1,
				-1,
				1,
				1,
				1
			};

			// Token: 0x04006773 RID: 26483
			private bool _useDiagonals;

			// Token: 0x04006774 RID: 26484
			private ushort[] _tileIds;
		}

		// Token: 0x020006C4 RID: 1732
		public class NotTouching : GenAction
		{
			// Token: 0x06003EF4 RID: 16116 RVA: 0x00697BAC File Offset: 0x00695DAC
			public NotTouching(bool useDiagonals, params ushort[] tileIds)
			{
				this._useDiagonals = useDiagonals;
				this._tileIds = tileIds;
			}

			// Token: 0x06003EF5 RID: 16117 RVA: 0x00697BC4 File Offset: 0x00695DC4
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				int num = this._useDiagonals ? 16 : 8;
				for (int i = 0; i < num; i += 2)
				{
					Tile tile = GenBase._tiles[x + Modifiers.NotTouching.DIRECTIONS[i], y + Modifiers.NotTouching.DIRECTIONS[i + 1]];
					if (tile.active())
					{
						for (int j = 0; j < this._tileIds.Length; j++)
						{
							if (tile.type == this._tileIds[j])
							{
								return base.Fail();
							}
						}
					}
				}
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x04006775 RID: 26485
			private static readonly int[] DIRECTIONS = new int[]
			{
				0,
				-1,
				1,
				0,
				-1,
				0,
				0,
				1,
				-1,
				-1,
				1,
				-1,
				-1,
				1,
				1,
				1
			};

			// Token: 0x04006776 RID: 26486
			private bool _useDiagonals;

			// Token: 0x04006777 RID: 26487
			private ushort[] _tileIds;
		}

		// Token: 0x020006C5 RID: 1733
		public class IsTouchingAir : GenAction
		{
			// Token: 0x06003EF7 RID: 16119 RVA: 0x00697C60 File Offset: 0x00695E60
			public IsTouchingAir(bool useDiagonals = false)
			{
				this._useDiagonals = useDiagonals;
			}

			// Token: 0x06003EF8 RID: 16120 RVA: 0x00697C70 File Offset: 0x00695E70
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				int num = this._useDiagonals ? 16 : 8;
				for (int i = 0; i < num; i += 2)
				{
					if (!GenBase._tiles[x + Modifiers.IsTouchingAir.DIRECTIONS[i], y + Modifiers.IsTouchingAir.DIRECTIONS[i + 1]].active())
					{
						return base.UnitApply(origin, x, y, args);
					}
				}
				return base.Fail();
			}

			// Token: 0x04006778 RID: 26488
			private static readonly int[] DIRECTIONS = new int[]
			{
				0,
				-1,
				1,
				0,
				-1,
				0,
				0,
				1,
				-1,
				-1,
				1,
				-1,
				-1,
				1,
				1,
				1
			};

			// Token: 0x04006779 RID: 26489
			private bool _useDiagonals;
		}

		// Token: 0x020006C6 RID: 1734
		public class SkipTiles : GenAction
		{
			// Token: 0x06003EFA RID: 16122 RVA: 0x00697CE7 File Offset: 0x00695EE7
			public SkipTiles(params ushort[] types)
			{
				this._types = types;
			}

			// Token: 0x06003EFB RID: 16123 RVA: 0x00697CF8 File Offset: 0x00695EF8
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (!GenBase._tiles[x, y].active())
				{
					return base.UnitApply(origin, x, y, args);
				}
				for (int i = 0; i < this._types.Length; i++)
				{
					if (GenBase._tiles[x, y].type == this._types[i])
					{
						return base.Fail();
					}
				}
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x0400677A RID: 26490
			private ushort[] _types;
		}

		// Token: 0x020006C7 RID: 1735
		public class HasLiquid : GenAction
		{
			// Token: 0x06003EFC RID: 16124 RVA: 0x00697D64 File Offset: 0x00695F64
			public HasLiquid(int liquidLevel = -1, int liquidType = -1)
			{
				this._liquidType = liquidType;
				this._liquidLevel = liquidLevel;
			}

			// Token: 0x06003EFD RID: 16125 RVA: 0x00697D7C File Offset: 0x00695F7C
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				Tile tile = GenBase._tiles[x, y];
				if ((this._liquidType == -1 || this._liquidType == (int)tile.liquidType()) && ((this._liquidLevel == -1 && tile.liquid != 0) || this._liquidLevel == (int)tile.liquid))
				{
					return base.UnitApply(origin, x, y, args);
				}
				return base.Fail();
			}

			// Token: 0x0400677B RID: 26491
			private int _liquidType;

			// Token: 0x0400677C RID: 26492
			private int _liquidLevel;
		}

		// Token: 0x020006C8 RID: 1736
		public class NoLiquid : GenAction
		{
			// Token: 0x06003EFE RID: 16126 RVA: 0x00697DDE File Offset: 0x00695FDE
			public NoLiquid(int liquidType = -1)
			{
				this._liquidType = liquidType;
			}

			// Token: 0x06003EFF RID: 16127 RVA: 0x00697DF0 File Offset: 0x00695FF0
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				Tile tile = GenBase._tiles[x, y];
				if (tile.liquid > 0 && (this._liquidType == -1 || this._liquidType == (int)tile.liquidType()))
				{
					return base.Fail();
				}
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x0400677D RID: 26493
			private int _liquidType;
		}

		// Token: 0x020006C9 RID: 1737
		public class SkipWalls : GenAction
		{
			// Token: 0x06003F00 RID: 16128 RVA: 0x00697E3C File Offset: 0x0069603C
			public SkipWalls(params ushort[] types)
			{
				this._types = types;
			}

			// Token: 0x06003F01 RID: 16129 RVA: 0x00697E4C File Offset: 0x0069604C
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				for (int i = 0; i < this._types.Length; i++)
				{
					if (GenBase._tiles[x, y].wall == this._types[i])
					{
						return base.Fail();
					}
				}
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x0400677E RID: 26494
			private ushort[] _types;
		}

		// Token: 0x020006CA RID: 1738
		public class SkipUnbreakableWalledTiles : GenAction
		{
			// Token: 0x06003F02 RID: 16130 RVA: 0x00697E99 File Offset: 0x00696099
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (GenBase._tiles[x, y].active() && GenBase._tiles[x, y].wall == 350)
				{
					return base.Fail();
				}
				return base.UnitApply(origin, x, y, args);
			}
		}

		// Token: 0x020006CB RID: 1739
		public class IsAboveHeight : GenAction
		{
			// Token: 0x06003F04 RID: 16132 RVA: 0x00697ED8 File Offset: 0x006960D8
			public IsAboveHeight(int y, bool inclusive = false)
			{
				this._y = y;
				this._inclusive = inclusive;
			}

			// Token: 0x06003F05 RID: 16133 RVA: 0x00697EEE File Offset: 0x006960EE
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (this._inclusive ? (y <= this._y) : (y < this._y))
				{
					return base.UnitApply(origin, x, y, args);
				}
				return base.Fail();
			}

			// Token: 0x0400677F RID: 26495
			private int _y;

			// Token: 0x04006780 RID: 26496
			private bool _inclusive;
		}

		// Token: 0x020006CC RID: 1740
		public class IsBelowHeight : GenAction
		{
			// Token: 0x06003F06 RID: 16134 RVA: 0x00697F23 File Offset: 0x00696123
			public IsBelowHeight(int y, bool inclusive = false)
			{
				this._y = y;
				this._inclusive = inclusive;
			}

			// Token: 0x06003F07 RID: 16135 RVA: 0x00697F39 File Offset: 0x00696139
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (this._inclusive ? (y >= this._y) : (y > this._y))
				{
					return base.UnitApply(origin, x, y, args);
				}
				return base.Fail();
			}

			// Token: 0x04006781 RID: 26497
			private int _y;

			// Token: 0x04006782 RID: 26498
			private bool _inclusive;
		}

		// Token: 0x020006CD RID: 1741
		public class IsEmpty : GenAction
		{
			// Token: 0x06003F08 RID: 16136 RVA: 0x00697F6E File Offset: 0x0069616E
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (!GenBase._tiles[x, y].active())
				{
					return base.UnitApply(origin, x, y, args);
				}
				return base.Fail();
			}
		}

		// Token: 0x020006CE RID: 1742
		public class IsSolid : GenAction
		{
			// Token: 0x06003F0A RID: 16138 RVA: 0x00697F95 File Offset: 0x00696195
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (GenBase._tiles[x, y].active() && WorldGen.SolidOrSlopedTile(x, y))
				{
					return base.UnitApply(origin, x, y, args);
				}
				return base.Fail();
			}
		}

		// Token: 0x020006CF RID: 1743
		public class IsNotSolid : GenAction
		{
			// Token: 0x06003F0C RID: 16140 RVA: 0x00697FC5 File Offset: 0x006961C5
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (!GenBase._tiles[x, y].active() || !WorldGen.SolidOrSlopedTile(x, y))
				{
					return base.UnitApply(origin, x, y, args);
				}
				return base.Fail();
			}
		}

		// Token: 0x020006D0 RID: 1744
		public class RectangleMask : GenAction
		{
			// Token: 0x06003F0E RID: 16142 RVA: 0x00697FF5 File Offset: 0x006961F5
			public RectangleMask(int xMin, int xMax, int yMin, int yMax)
			{
				this._xMin = xMin;
				this._yMin = yMin;
				this._xMax = xMax;
				this._yMax = yMax;
			}

			// Token: 0x06003F0F RID: 16143 RVA: 0x0069801C File Offset: 0x0069621C
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (x >= this._xMin + origin.X && x <= this._xMax + origin.X && y >= this._yMin + origin.Y && y <= this._yMax + origin.Y)
				{
					return base.UnitApply(origin, x, y, args);
				}
				return base.Fail();
			}

			// Token: 0x04006783 RID: 26499
			private int _xMin;

			// Token: 0x04006784 RID: 26500
			private int _yMin;

			// Token: 0x04006785 RID: 26501
			private int _xMax;

			// Token: 0x04006786 RID: 26502
			private int _yMax;
		}

		// Token: 0x020006D1 RID: 1745
		public class Offset : GenAction
		{
			// Token: 0x06003F10 RID: 16144 RVA: 0x0069807B File Offset: 0x0069627B
			public Offset(int x, int y)
			{
				this._xOffset = x;
				this._yOffset = y;
			}

			// Token: 0x06003F11 RID: 16145 RVA: 0x00698091 File Offset: 0x00696291
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				return base.UnitApply(origin, x + this._xOffset, y + this._yOffset, args);
			}

			// Token: 0x04006787 RID: 26503
			private int _xOffset;

			// Token: 0x04006788 RID: 26504
			private int _yOffset;
		}

		// Token: 0x020006D2 RID: 1746
		public class Dither : GenAction
		{
			// Token: 0x06003F12 RID: 16146 RVA: 0x006980AC File Offset: 0x006962AC
			public Dither(double failureChance = 0.5)
			{
				this._failureChance = failureChance;
			}

			// Token: 0x06003F13 RID: 16147 RVA: 0x006980BB File Offset: 0x006962BB
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (GenBase._random.NextDouble() >= this._failureChance)
				{
					return base.UnitApply(origin, x, y, args);
				}
				return base.Fail();
			}

			// Token: 0x04006789 RID: 26505
			private double _failureChance;
		}

		// Token: 0x020006D3 RID: 1747
		public class Flip : GenAction
		{
			// Token: 0x06003F14 RID: 16148 RVA: 0x006980E1 File Offset: 0x006962E1
			public Flip(bool flipX, bool flipY)
			{
				this._flipX = flipX;
				this._flipY = flipY;
			}

			// Token: 0x06003F15 RID: 16149 RVA: 0x006980F7 File Offset: 0x006962F7
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (this._flipX)
				{
					x = origin.X * 2 - x;
				}
				if (this._flipY)
				{
					y = origin.Y * 2 - y;
				}
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x0400678A RID: 26506
			private bool _flipX;

			// Token: 0x0400678B RID: 26507
			private bool _flipY;
		}
	}
}
