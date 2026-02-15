using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon;

namespace Terraria.GameContent.Biomes
{
	// Token: 0x02000500 RID: 1280
	public class DungeonControlLine
	{
		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060035C9 RID: 13769 RVA: 0x0061CF52 File Offset: 0x0061B152
		public Vector2D Center
		{
			get
			{
				return (this.End + this.Start) / 2.0;
			}
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x0000357B File Offset: 0x0000177B
		[JsonConstructor]
		private DungeonControlLine()
		{
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x0061CF74 File Offset: 0x0061B174
		public DungeonControlLine(Vector2D start, Vector2D end, double startRadius, double endRadius, int progressionStage, DungeonGenerationStyleData style)
		{
			this.Start = start;
			this.End = end;
			this.StartRadius = startRadius;
			this.EndRadius = endRadius;
			this.ProgressionStage = progressionStage;
			this.Style = style;
			Vector2D v = this.End - this.Start;
			this.LineLength = v.Length();
			this.NormalizedLineDirection = v.SafeNormalize(Vector2D.UnitX);
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x0061CFE4 File Offset: 0x0061B1E4
		private void CacheNormals()
		{
			this.StartNormal = new Vector2D(this.StartTangent.Y, -this.StartTangent.X);
			this.EndNormal = new Vector2D(-this.EndTangent.Y, this.EndTangent.X);
			this.CrossTangent = Vector2D.Cross(this.StartTangent, this.EndTangent);
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x0061D04C File Offset: 0x0061B24C
		public bool CanPaint(int x, int y, out double distance, out double normalizedLineProgress)
		{
			distance = 0.0;
			normalizedLineProgress = 0.0;
			Vector2D vector2D;
			vector2D..ctor((double)x, (double)y);
			Vector2D vector2D2 = vector2D - this.Start;
			double num = Vector2D.Dot(vector2D2, this.StartTangent);
			if (num < 0.0)
			{
				if (this.Prev != null)
				{
					return false;
				}
				normalizedLineProgress = 0.0;
				distance = vector2D2.Length();
				return true;
			}
			else
			{
				Vector2D vector2D3 = vector2D - this.End;
				double num2 = Vector2D.Dot(vector2D3, this.EndTangent);
				if (num2 >= 0.0)
				{
					double num3 = Vector2D.Dot(vector2D2, this.StartNormal);
					double num4 = Vector2D.Dot(vector2D3, this.EndNormal);
					double num5 = (num + num2) / 2.0;
					num *= num;
					num2 *= num2;
					double num6 = num / (num + num2);
					double value = num3 * (1.0 - num6) + num4 * num6 - num5 * this.CrossTangent * num6 * (1.0 - num6);
					distance = Math.Abs(value);
					normalizedLineProgress = num6;
					return true;
				}
				if (this.Next != null)
				{
					return false;
				}
				normalizedLineProgress = 1.0;
				distance = vector2D3.Length();
				return true;
			}
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x0061D188 File Offset: 0x0061B388
		public void Paint(int x, int y)
		{
			double num;
			double num2;
			if (!this.CanPaint(x, y, out num, out num2))
			{
				return;
			}
			double num3 = Utils.Lerp(this.StartRadius, this.EndRadius, num2);
			double num4 = num / num3;
			if (num4 > 1.0)
			{
				return;
			}
			DungeonGenerationStyleData styleWithDitheredTransition = this.GetStyleWithDitheredTransition(x, y, num2);
			if (DungeonControlLine.SkipPaintForEdge(x, y, styleWithDitheredTransition, num4))
			{
				return;
			}
			Tile tile = Main.tile[x, y];
			tile.ClearEverything();
			tile.active(true);
			tile.type = styleWithDitheredTransition.BrickTileType;
			tile.wall = styleWithDitheredTransition.BrickWallType;
			if (styleWithDitheredTransition.UnbreakableWallProgressionTier > DualDungeonUnbreakableWallTiers.EarlyGame)
			{
				int num5 = (int)(num3 * DungeonControlLine.NormalizedDistanceSafeFromDither);
				double num6 = num - (double)num5;
				if (num6 >= -4.0 && num6 <= 0.0)
				{
					int num7 = styleWithDitheredTransition.UnbreakableWallProgressionTier;
					if (num6 <= -2.0)
					{
						num7 += 16;
					}
					tile.wall = 350;
					tile.wallColor((byte)num7);
				}
			}
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x0061D288 File Offset: 0x0061B488
		public DungeonGenerationStyleData GetStyleWithDitheredTransition(int x, int y, double normalizedLineProgress)
		{
			if (normalizedLineProgress < 0.25)
			{
				if (this.Prev != null && this.Prev.Style != this.Style && Utils.Remap(normalizedLineProgress, 0.0, 0.25, 0.5, 1.0, true) <= DitherSnakePass._bayerDither[x % 4, y % 4])
				{
					return this.Prev.Style;
				}
			}
			else if (normalizedLineProgress > 0.75 && this.Next != null && this.Next.Style != this.Style && Utils.Remap(normalizedLineProgress, 0.75, 1.0, 0.0, 0.5, true) >= DitherSnakePass._bayerDither[x % 4, y % 4])
			{
				return this.Next.Style;
			}
			return this.Style;
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x0061D384 File Offset: 0x0061B584
		public static bool SkipPaintForEdge(int x, int y, DungeonGenerationStyleData style, double normalizedDistanceForPoint)
		{
			if (normalizedDistanceForPoint <= DungeonControlLine.NormalizedDistanceSafeFromDither)
			{
				return false;
			}
			double num = 1.0 - (normalizedDistanceForPoint - DungeonControlLine.NormalizedDistanceSafeFromDither) / (1.0 - DungeonControlLine.NormalizedDistanceSafeFromDither);
			if (!style.EdgeDither)
			{
				return num < 0.25;
			}
			if (!WorldGen.InWorld(x, y, 5))
			{
				return false;
			}
			Tile tile = Main.tile[x, y];
			if (tile != null && !tile.active())
			{
				return true;
			}
			if (num <= DitherSnakePass._bayerDither[x % 4, y % 4])
			{
				return true;
			}
			double num2 = Utils.Lerp(0.0, 0.949999988079071, 1.0 - num);
			return (double)WorldGen.genRand.NextFloat() <= num2 || (num <= 0.09375 && WorldGen.genRand.Next(3) != 0) || (num <= 0.125 && WorldGen.genRand.Next(2) == 0) || (num <= 0.15625 && WorldGen.genRand.Next(4) == 0);
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x0061D494 File Offset: 0x0061B694
		public void Paint(Rectangle dungeonBounds)
		{
			this.CacheNormals();
			double num = Utils.Max<double>(new double[]
			{
				this.StartRadius,
				this.EndRadius
			});
			Point point = this.Start.ToPoint();
			Point point2 = this.End.ToPoint();
			Rectangle value = Rectangle.Union(new Rectangle(point.X, point.Y, 1, 1), new Rectangle(point2.X, point2.Y, 1, 1));
			value.Inflate((int)num, (int)num);
			Rectangle rectangle = Rectangle.Intersect(value, dungeonBounds);
			for (int i = rectangle.Left; i <= rectangle.Right; i++)
			{
				for (int j = rectangle.Top; j <= rectangle.Bottom; j++)
				{
					this.Paint(i, j);
				}
			}
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x0061D560 File Offset: 0x0061B760
		public bool IsSelfIntersecting()
		{
			this.CacheNormals();
			double num = Vector2D.Cross(this.StartNormal, this.EndNormal);
			Vector2D vector2D = this.End - this.Start;
			double value = Vector2D.Cross(vector2D, this.EndNormal) / num;
			double value2 = Vector2D.Cross(vector2D, this.StartNormal) / num;
			return Math.Abs(value) < this.StartRadius || Math.Abs(value2) < this.EndRadius;
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x0061D5D4 File Offset: 0x0061B7D4
		public bool AdjustTangentsToPreventSelfIntersection()
		{
			if (!this.IsSelfIntersecting())
			{
				return false;
			}
			Vector2D startTangent = (this.StartTangent - this.EndTangent / 2.0).SafeNormalize(default(Vector2D));
			Vector2D endTangent = (this.EndTangent - this.StartTangent / 2.0).SafeNormalize(default(Vector2D));
			if (this.Prev != null)
			{
				this.StartTangent = startTangent;
				this.Prev.EndTangent = -this.StartTangent;
			}
			if (this.Next != null)
			{
				this.EndTangent = endTangent;
				this.Next.StartTangent = -this.EndTangent;
			}
			return true;
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x0061D694 File Offset: 0x0061B894
		public bool IsInsideBorder(Point point)
		{
			double num;
			double amount;
			return this.CanPaint(point.X, point.Y, out num, out amount) && num < Utils.Lerp(this.StartRadius, this.EndRadius, amount) * DungeonControlLine.NormalizedDistanceSafeFromDither - 4.0;
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x0061D6E0 File Offset: 0x0061B8E0
		public Vector2D GetPotentialRoomPosition(double normalizedDistanceAlong, double normalizedOffset, int roomRadius)
		{
			Vector2D vector2D = Vector2D.Lerp(this.StartNormal, this.EndNormal, normalizedDistanceAlong).SafeNormalize(default(Vector2D));
			double num = Utils.Lerp(this.StartRadius, this.EndRadius, normalizedDistanceAlong) * DungeonControlLine.NormalizedDistanceSafeFromDither - 4.0 - (double)roomRadius;
			return Vector2D.Lerp(this.Start, this.End, normalizedDistanceAlong) + vector2D * num * normalizedOffset;
		}

		// Token: 0x04005AC3 RID: 23235
		public int Index;

		// Token: 0x04005AC4 RID: 23236
		public DungeonControlLine Next;

		// Token: 0x04005AC5 RID: 23237
		public DungeonControlLine Prev;

		// Token: 0x04005AC6 RID: 23238
		public Vector2D Start;

		// Token: 0x04005AC7 RID: 23239
		public Vector2D End;

		// Token: 0x04005AC8 RID: 23240
		public Vector2D StartTangent;

		// Token: 0x04005AC9 RID: 23241
		public Vector2D EndTangent;

		// Token: 0x04005ACA RID: 23242
		public Vector2D StartNormal;

		// Token: 0x04005ACB RID: 23243
		public Vector2D EndNormal;

		// Token: 0x04005ACC RID: 23244
		public double CrossTangent;

		// Token: 0x04005ACD RID: 23245
		public double StartRadius;

		// Token: 0x04005ACE RID: 23246
		public double EndRadius;

		// Token: 0x04005ACF RID: 23247
		public static double NormalizedDistanceSafeFromDither;

		// Token: 0x04005AD0 RID: 23248
		private const double StyleTransitionDitherWidth = 0.5;

		// Token: 0x04005AD1 RID: 23249
		private const int BorderWidth = 4;

		// Token: 0x04005AD2 RID: 23250
		public Vector2D NormalizedLineDirection;

		// Token: 0x04005AD3 RID: 23251
		public double LineLength;

		// Token: 0x04005AD4 RID: 23252
		public DungeonGenerationStyleData Style;

		// Token: 0x04005AD5 RID: 23253
		public int ProgressionStage;

		// Token: 0x04005AD6 RID: 23254
		public bool CurveLine;
	}
}
