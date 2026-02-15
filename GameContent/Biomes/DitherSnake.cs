using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon;

namespace Terraria.GameContent.Biomes
{
	// Token: 0x02000501 RID: 1281
	public class DitherSnake : List<DungeonControlLine>
	{
		// Token: 0x060035D6 RID: 13782 RVA: 0x0061D758 File Offset: 0x0061B958
		public new void Add(DungeonControlLine line)
		{
			if (base.Count > 0)
			{
				DungeonControlLine dungeonControlLine = this.Last<DungeonControlLine>();
				dungeonControlLine.Next = line;
				line.Prev = dungeonControlLine;
			}
			line.Index = base.Count;
			base.Add(line);
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x0061D798 File Offset: 0x0061B998
		public DungeonControlLine GetClosestLineTo(Vector2D pos)
		{
			DungeonControlLine result = null;
			double num = double.MaxValue;
			foreach (DungeonControlLine dungeonControlLine in this)
			{
				double num2 = dungeonControlLine.Center.Distance(pos);
				if (num2 < num)
				{
					result = dungeonControlLine;
					num = num2;
				}
			}
			return result;
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x0061D804 File Offset: 0x0061BA04
		public DungeonControlLine GetLineContaining(Vector2D pos, DungeonControlLine initialGuess = null, int depth = 0)
		{
			if (initialGuess == null)
			{
				initialGuess = this.GetClosestLineTo(pos);
			}
			if (depth == 3)
			{
				return null;
			}
			if (Vector2D.Dot(pos - initialGuess.Start, initialGuess.StartTangent) < 0.0 && initialGuess.Prev != null)
			{
				return this.GetLineContaining(pos, initialGuess.Prev, depth + 1);
			}
			if (Vector2D.Dot(pos - initialGuess.End, initialGuess.EndTangent) < 0.0 && initialGuess.Next != null)
			{
				return this.GetLineContaining(pos, initialGuess.Next, depth + 1);
			}
			return initialGuess;
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x0061D89C File Offset: 0x0061BA9C
		public double GetPositionAlongSnake(Vector2D pos)
		{
			DungeonControlLine lineContaining = this.GetLineContaining(pos, null, 0);
			double num;
			double num2;
			if (!lineContaining.CanPaint((int)pos.X, (int)pos.Y, out num, out num2))
			{
				num2 = 0.5;
			}
			return (double)lineContaining.Index + num2;
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x0061D8E0 File Offset: 0x0061BAE0
		public bool IsCircleInsideBorderWithStyle(DungeonGenerationStyleData style, Vector2D center, int radius)
		{
			DungeonControlLine closestLineTo = this.GetClosestLineTo(center);
			return closestLineTo.Style == style && this.IsCircleInsideBorderWithMatchingStyle(closestLineTo, center, radius);
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x0061D90C File Offset: 0x0061BB0C
		public bool IsCircleInsideBorderWithMatchingStyle(DungeonControlLine nearbyLine, Vector2D center, int radius)
		{
			double num = (double)radius * DitherSnake.ExtraBuffer;
			foreach (Vector2D vector2D in DitherSnake.CircleTestPoints)
			{
				Vector2D vector2D2 = center + vector2D * num;
				DungeonControlLine lineContaining = this.GetLineContaining(vector2D2, nearbyLine, 0);
				if (lineContaining == null || lineContaining.Style != nearbyLine.Style)
				{
					return false;
				}
				if (!lineContaining.IsInsideBorder(vector2D2.ToPoint()))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x0061D984 File Offset: 0x0061BB84
		public Vector2D GetRoomPositionInsideBorder(DungeonControlLine line, double normalizedDistanceAlong, double normalizedDistanceFrom, int roomRadius, out SnakeOrientation orientation)
		{
			orientation = SnakeOrientation.Unknown;
			Vector2D target = Vector2D.Lerp(line.Start, line.End, normalizedDistanceAlong);
			Vector2D potentialRoomPosition = line.GetPotentialRoomPosition(normalizedDistanceAlong, 0.0, roomRadius);
			Vector2D potentialRoomPosition2 = line.GetPotentialRoomPosition(normalizedDistanceAlong, 1.0, roomRadius);
			Vector2D target2 = (potentialRoomPosition.Y < potentialRoomPosition2.Y) ? potentialRoomPosition : potentialRoomPosition2;
			Vector2D target3 = (potentialRoomPosition.Y > potentialRoomPosition2.Y) ? potentialRoomPosition : potentialRoomPosition2;
			for (int i = 0; i < 4; i++)
			{
				Vector2D potentialRoomPosition3 = line.GetPotentialRoomPosition(normalizedDistanceAlong, normalizedDistanceFrom, roomRadius);
				if (this.IsCircleInsideBorderWithMatchingStyle(line, potentialRoomPosition3, roomRadius))
				{
					double num = potentialRoomPosition3.Distance(target);
					double num2 = potentialRoomPosition3.Distance(target2);
					double num3 = potentialRoomPosition3.Distance(target3);
					if (num < num2 && num < num3)
					{
						orientation = SnakeOrientation.Center;
					}
					else if (num2 < num3)
					{
						orientation = SnakeOrientation.Top;
					}
					else
					{
						orientation = SnakeOrientation.Bottom;
					}
					return potentialRoomPosition3;
				}
				normalizedDistanceFrom *= 0.8;
			}
			orientation = SnakeOrientation.Center;
			return line.Center;
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x0061DA7C File Offset: 0x0061BC7C
		public void SetTangents()
		{
			DungeonControlLine dungeonControlLine = base[0];
			dungeonControlLine.StartTangent = dungeonControlLine.NormalizedLineDirection;
			while (dungeonControlLine.Next != null)
			{
				DungeonControlLine next = dungeonControlLine.Next;
				Vector2D vector2D = (dungeonControlLine.NormalizedLineDirection + next.NormalizedLineDirection).SafeNormalize(default(Vector2D));
				next.StartTangent = vector2D;
				dungeonControlLine.EndTangent = -vector2D;
				dungeonControlLine = next;
			}
			dungeonControlLine.EndTangent = -dungeonControlLine.NormalizedLineDirection;
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x0061DAF4 File Offset: 0x0061BCF4
		public void AdjustTangentsToPreventSelfIntersection()
		{
			for (int i = 0; i < 100; i++)
			{
				bool flag = false;
				using (List<DungeonControlLine>.Enumerator enumerator = base.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.AdjustTangentsToPreventSelfIntersection())
						{
							flag = true;
						}
					}
				}
				if (!flag)
				{
					break;
				}
			}
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x0061DB58 File Offset: 0x0061BD58
		public bool IsLineInsideBorder(Vector2D from, Vector2D to, int halfWidth)
		{
			Vector2D vector2D = (to - from).SafeNormalize(Vector2D.UnitX).RotatedBy(1.5707963267948966, default(Vector2D)) * (double)halfWidth;
			return this.IsLineInsideBorder(from + vector2D, to + vector2D) && this.IsLineInsideBorder(from - vector2D, to - vector2D);
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x0061DBC4 File Offset: 0x0061BDC4
		public bool IsLineInsideBorder(Vector2D from, Vector2D to)
		{
			DungeonControlLine line = this.GetClosestLineTo(from);
			return Utils.PlotLine(from.ToPoint(), to.ToPoint(), delegate(int x, int y)
			{
				line = this.GetLineContaining(new Vector2D((double)x, (double)y), line, 0);
				return line != null && line.IsInsideBorder(new Point(x, y));
			}, true);
		}

		// Token: 0x04005AD7 RID: 23255
		private static readonly Vector2D[] CircleTestPoints = (from i in Enumerable.Range(0, 12)
		select Vector2D.UnitX.RotatedBy(6.283185307179586 * (double)i / 12.0, default(Vector2D))).ToArray<Vector2D>();

		// Token: 0x04005AD8 RID: 23256
		private static readonly double ExtraBuffer = 1.0 / Math.Cos(0.5235987755982988);
	}
}
