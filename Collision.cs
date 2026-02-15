using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria
{
	// Token: 0x02000042 RID: 66
	public class Collision
	{
		// Token: 0x060006B6 RID: 1718 RVA: 0x002730F8 File Offset: 0x002712F8
		public static Vector2[] CheckLinevLine(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
		{
			if (a1.Equals(a2) && b1.Equals(b2))
			{
				if (a1.Equals(b1))
				{
					return new Vector2[]
					{
						a1
					};
				}
				return new Vector2[0];
			}
			else if (b1.Equals(b2))
			{
				if (Collision.PointOnLine(b1, a1, a2))
				{
					return new Vector2[]
					{
						b1
					};
				}
				return new Vector2[0];
			}
			else if (a1.Equals(a2))
			{
				if (Collision.PointOnLine(a1, b1, b2))
				{
					return new Vector2[]
					{
						a1
					};
				}
				return new Vector2[0];
			}
			else
			{
				float num = (b2.X - b1.X) * (a1.Y - b1.Y) - (b2.Y - b1.Y) * (a1.X - b1.X);
				float num2 = (a2.X - a1.X) * (a1.Y - b1.Y) - (a2.Y - a1.Y) * (a1.X - b1.X);
				float num3 = (b2.Y - b1.Y) * (a2.X - a1.X) - (b2.X - b1.X) * (a2.Y - a1.Y);
				if (-Collision.Epsilon >= num3 || num3 >= Collision.Epsilon)
				{
					float num4 = num / num3;
					float num5 = num2 / num3;
					if (0f <= num4 && num4 <= 1f && 0f <= num5 && num5 <= 1f)
					{
						return new Vector2[]
						{
							new Vector2(a1.X + num4 * (a2.X - a1.X), a1.Y + num4 * (a2.Y - a1.Y))
						};
					}
					return new Vector2[0];
				}
				else
				{
					if ((-Collision.Epsilon >= num || num >= Collision.Epsilon) && (-Collision.Epsilon >= num2 || num2 >= Collision.Epsilon))
					{
						return new Vector2[0];
					}
					if (a1.Equals(a2))
					{
						return Collision.OneDimensionalIntersection(b1, b2, a1, a2);
					}
					return Collision.OneDimensionalIntersection(a1, a2, b1, b2);
				}
			}
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00273300 File Offset: 0x00271500
		private static double DistFromSeg(Vector2 p, Vector2 q0, Vector2 q1, double radius, ref float u)
		{
			double num = (double)(q1.X - q0.X);
			double num2 = (double)(q1.Y - q0.Y);
			double num3 = (double)(q0.X - p.X);
			double num4 = (double)(q0.Y - p.Y);
			double num5 = Math.Sqrt(num * num + num2 * num2);
			if (num5 < (double)Collision.Epsilon)
			{
				throw new Exception("Expected line segment, not point.");
			}
			return Math.Abs(num * num4 - num3 * num2) / num5;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00273378 File Offset: 0x00271578
		private static bool PointOnLine(Vector2 p, Vector2 a1, Vector2 a2)
		{
			float num = 0f;
			return Collision.DistFromSeg(p, a1, a2, (double)Collision.Epsilon, ref num) < (double)Collision.Epsilon;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x002733A4 File Offset: 0x002715A4
		private static Vector2[] OneDimensionalIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
		{
			float num = a2.X - a1.X;
			float num2 = a2.Y - a1.Y;
			float relativePoint;
			float relativePoint2;
			if (Math.Abs(num) > Math.Abs(num2))
			{
				relativePoint = (b1.X - a1.X) / num;
				relativePoint2 = (b2.X - a1.X) / num;
			}
			else
			{
				relativePoint = (b1.Y - a1.Y) / num2;
				relativePoint2 = (b2.Y - a1.Y) / num2;
			}
			List<Vector2> list = new List<Vector2>();
			foreach (float num3 in Collision.FindOverlapPoints(relativePoint, relativePoint2))
			{
				float x = a2.X * num3 + a1.X * (1f - num3);
				float y = a2.Y * num3 + a1.Y * (1f - num3);
				list.Add(new Vector2(x, y));
			}
			return list.ToArray();
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00273494 File Offset: 0x00271694
		private static float[] FindOverlapPoints(float relativePoint1, float relativePoint2)
		{
			float val = Math.Min(relativePoint1, relativePoint2);
			float val2 = Math.Max(relativePoint1, relativePoint2);
			float num = Math.Max(0f, val);
			float num2 = Math.Min(1f, val2);
			if (num > num2)
			{
				return new float[0];
			}
			if (num == num2)
			{
				return new float[]
				{
					num
				};
			}
			return new float[]
			{
				num,
				num2
			};
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x002734F4 File Offset: 0x002716F4
		public static bool CheckAABBvAABBCollision(Vector2 position1, Vector2 dimensions1, Vector2 position2, Vector2 dimensions2)
		{
			return position1.X < position2.X + dimensions2.X && position1.Y < position2.Y + dimensions2.Y && position1.X + dimensions1.X > position2.X && position1.Y + dimensions1.Y > position2.Y;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00273558 File Offset: 0x00271758
		private static int collisionOutcode(Vector2 aabbPosition, Vector2 aabbDimensions, Vector2 point)
		{
			float num = aabbPosition.X + aabbDimensions.X;
			float num2 = aabbPosition.Y + aabbDimensions.Y;
			int num3 = 0;
			if (aabbDimensions.X <= 0f)
			{
				num3 |= 5;
			}
			else if (point.X < aabbPosition.X)
			{
				num3 |= 1;
			}
			else if (point.X - num > 0f)
			{
				num3 |= 4;
			}
			if (aabbDimensions.Y <= 0f)
			{
				num3 |= 10;
			}
			else if (point.Y < aabbPosition.Y)
			{
				num3 |= 2;
			}
			else if (point.Y - num2 > 0f)
			{
				num3 |= 8;
			}
			return num3;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x002735FC File Offset: 0x002717FC
		public static bool CheckAABBvLineCollision(Vector2 aabbPosition, Vector2 aabbDimensions, Vector2 lineStart, Vector2 lineEnd)
		{
			int num;
			if ((num = Collision.collisionOutcode(aabbPosition, aabbDimensions, lineEnd)) == 0)
			{
				return true;
			}
			int num2;
			while ((num2 = Collision.collisionOutcode(aabbPosition, aabbDimensions, lineStart)) != 0)
			{
				if ((num2 & num) != 0)
				{
					return false;
				}
				if ((num2 & 5) != 0)
				{
					float num3 = aabbPosition.X;
					if ((num2 & 4) != 0)
					{
						num3 += aabbDimensions.X;
					}
					lineStart.Y += (num3 - lineStart.X) * (lineEnd.Y - lineStart.Y) / (lineEnd.X - lineStart.X);
					lineStart.X = num3;
				}
				else
				{
					float num4 = aabbPosition.Y;
					if ((num2 & 8) != 0)
					{
						num4 += aabbDimensions.Y;
					}
					lineStart.X += (num4 - lineStart.Y) * (lineEnd.X - lineStart.X) / (lineEnd.Y - lineStart.Y);
					lineStart.Y = num4;
				}
			}
			return true;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x002736D0 File Offset: 0x002718D0
		public static bool CheckAABBvLineCollision2(Vector2 aabbPosition, Vector2 aabbDimensions, Vector2 lineStart, Vector2 lineEnd)
		{
			float num = 0f;
			return Utils.RectangleLineCollision(aabbPosition, aabbPosition + aabbDimensions, lineStart, lineEnd) || Collision.CheckAABBvLineCollision(aabbPosition, aabbDimensions, lineStart, lineEnd, 0.0001f, ref num);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00273708 File Offset: 0x00271908
		public static bool CheckAABBvLineCollision(Vector2 objectPosition, Vector2 objectDimensions, Vector2 lineStart, Vector2 lineEnd, float lineWidth, ref float collisionPoint)
		{
			float num = lineWidth * 0.5f;
			Vector2 position = lineStart;
			Vector2 vector = lineEnd - lineStart;
			if (vector.X > 0f)
			{
				vector.X += lineWidth;
				position.X -= num;
			}
			else
			{
				position.X += vector.X - num;
				vector.X = -vector.X + lineWidth;
			}
			if (vector.Y > 0f)
			{
				vector.Y += lineWidth;
				position.Y -= num;
			}
			else
			{
				position.Y += vector.Y - num;
				vector.Y = -vector.Y + lineWidth;
			}
			if (!Collision.CheckAABBvAABBCollision(objectPosition, objectDimensions, position, vector))
			{
				return false;
			}
			Vector2 vector2 = objectPosition - lineStart;
			Vector2 vector3 = vector2 + objectDimensions;
			Vector2 spinningpoint = new Vector2(vector2.X, vector3.Y);
			Vector2 spinningpoint2 = new Vector2(vector3.X, vector2.Y);
			Vector2 vector4 = lineEnd - lineStart;
			float num2 = vector4.Length();
			float num3 = (float)Math.Atan2((double)vector4.Y, (double)vector4.X);
			Vector2[] array = new Vector2[]
			{
				vector2.RotatedBy((double)(-(double)num3), default(Vector2)),
				spinningpoint2.RotatedBy((double)(-(double)num3), default(Vector2)),
				vector3.RotatedBy((double)(-(double)num3), default(Vector2)),
				spinningpoint.RotatedBy((double)(-(double)num3), default(Vector2))
			};
			collisionPoint = num2;
			bool result = false;
			for (int i = 0; i < array.Length; i++)
			{
				if (Math.Abs(array[i].Y) < num && array[i].X < collisionPoint && array[i].X >= 0f)
				{
					collisionPoint = array[i].X;
					result = true;
				}
			}
			Vector2 vector5 = new Vector2(0f, num);
			Vector2 value = new Vector2(num2, num);
			Vector2 vector6 = new Vector2(0f, -num);
			Vector2 value2 = new Vector2(num2, -num);
			for (int j = 0; j < array.Length; j++)
			{
				int num4 = (j + 1) % array.Length;
				Vector2 vector7 = value - vector5;
				Vector2 vector8 = array[num4] - array[j];
				float num5 = vector7.X * vector8.Y - vector7.Y * vector8.X;
				if (num5 != 0f)
				{
					Vector2 vector9 = array[j] - vector5;
					float num6 = (vector9.X * vector8.Y - vector9.Y * vector8.X) / num5;
					if (num6 >= 0f && num6 <= 1f)
					{
						float num7 = (vector9.X * vector7.Y - vector9.Y * vector7.X) / num5;
						if (num7 >= 0f && num7 <= 1f)
						{
							result = true;
							collisionPoint = Math.Min(collisionPoint, vector5.X + num6 * vector7.X);
						}
					}
				}
				vector7 = value2 - vector6;
				num5 = vector7.X * vector8.Y - vector7.Y * vector8.X;
				if (num5 != 0f)
				{
					Vector2 vector10 = array[j] - vector6;
					float num8 = (vector10.X * vector8.Y - vector10.Y * vector8.X) / num5;
					if (num8 >= 0f && num8 <= 1f)
					{
						float num9 = (vector10.X * vector7.Y - vector10.Y * vector7.X) / num5;
						if (num9 >= 0f && num9 <= 1f)
						{
							result = true;
							collisionPoint = Math.Min(collisionPoint, vector6.X + num8 * vector7.X);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00273B33 File Offset: 0x00271D33
		public static bool CanHit(Entity source, Entity target)
		{
			return Collision.CanHit(source.position, source.width, source.height, target.position, target.width, target.height);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00273B5E File Offset: 0x00271D5E
		public static bool CanHit(Entity source, NPCAimedTarget target)
		{
			return Collision.CanHit(source.position, source.width, source.height, target.Position, target.Width, target.Height);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00273B89 File Offset: 0x00271D89
		public static bool CanHit(Vector2 Position1, int Width1, int Height1, Vector2 Position2, int Width2, int Height2)
		{
			return Collision.CanHit(Position1.ToPoint(), Width1, Height1, Position2.ToPoint(), Width2, Height2);
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00273BA4 File Offset: 0x00271DA4
		public static bool CanHit(Point Position1, int Width1, int Height1, Point Position2, int Width2, int Height2)
		{
			int num = (Position1.X + Width1 / 2) / 16;
			int num2 = (Position1.Y + Height1 / 2) / 16;
			int num3 = (Position2.X + Width2 / 2) / 16;
			int num4 = (Position2.Y + Height2 / 2) / 16;
			if (num <= 1)
			{
				num = 1;
			}
			if (num >= Main.maxTilesX)
			{
				num = Main.maxTilesX - 1;
			}
			if (num3 <= 1)
			{
				num3 = 1;
			}
			if (num3 >= Main.maxTilesX)
			{
				num3 = Main.maxTilesX - 1;
			}
			if (num2 <= 1)
			{
				num2 = 1;
			}
			if (num2 >= Main.maxTilesY - 40)
			{
				num2 = Main.maxTilesY - 40;
			}
			if (num4 <= 1)
			{
				num4 = 1;
			}
			if (num4 >= Main.maxTilesY - 40)
			{
				num4 = Main.maxTilesY - 40;
			}
			bool result;
			try
			{
				for (;;)
				{
					int num5 = Math.Abs(num - num3);
					int num6 = Math.Abs(num2 - num4);
					if (num == num3 && num2 == num4)
					{
						break;
					}
					if (num5 > num6)
					{
						if (num < num3)
						{
							num++;
						}
						else
						{
							num--;
						}
						if (Main.tile[num, num2 - 1] == null)
						{
							goto Block_14;
						}
						if (Main.tile[num, num2 + 1] == null)
						{
							goto Block_15;
						}
						if (!Main.tile[num, num2 - 1].inActive() && Main.tile[num, num2 - 1].active() && Main.tileSolid[(int)Main.tile[num, num2 - 1].type] && !Main.tileSolidTop[(int)Main.tile[num, num2 - 1].type] && Main.tile[num, num2 - 1].slope() == 0 && !Main.tile[num, num2 - 1].halfBrick() && !Main.tile[num, num2 + 1].inActive() && Main.tile[num, num2 + 1].active() && Main.tileSolid[(int)Main.tile[num, num2 + 1].type] && !Main.tileSolidTop[(int)Main.tile[num, num2 + 1].type] && Main.tile[num, num2 + 1].slope() == 0 && !Main.tile[num, num2 + 1].halfBrick())
						{
							goto Block_27;
						}
					}
					else
					{
						if (num2 < num4)
						{
							num2++;
						}
						else
						{
							num2--;
						}
						if (Main.tile[num - 1, num2] == null)
						{
							goto Block_29;
						}
						if (Main.tile[num + 1, num2] == null)
						{
							goto Block_30;
						}
						if (!Main.tile[num - 1, num2].inActive() && Main.tile[num - 1, num2].active() && Main.tileSolid[(int)Main.tile[num - 1, num2].type] && !Main.tileSolidTop[(int)Main.tile[num - 1, num2].type] && Main.tile[num - 1, num2].slope() == 0 && !Main.tile[num - 1, num2].halfBrick() && !Main.tile[num + 1, num2].inActive() && Main.tile[num + 1, num2].active() && Main.tileSolid[(int)Main.tile[num + 1, num2].type] && !Main.tileSolidTop[(int)Main.tile[num + 1, num2].type] && Main.tile[num + 1, num2].slope() == 0 && !Main.tile[num + 1, num2].halfBrick())
						{
							goto Block_42;
						}
					}
					if (Main.tile[num, num2] == null)
					{
						goto Block_43;
					}
					if (!Main.tile[num, num2].inActive() && Main.tile[num, num2].active() && Main.tileSolid[(int)Main.tile[num, num2].type] && !Main.tileSolidTop[(int)Main.tile[num, num2].type])
					{
						goto Block_47;
					}
				}
				return true;
				Block_14:
				return false;
				Block_15:
				return false;
				Block_27:
				return false;
				Block_29:
				return false;
				Block_30:
				return false;
				Block_42:
				return false;
				Block_43:
				return false;
				Block_47:
				result = false;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00274004 File Offset: 0x00272204
		public static bool CanHitWithCheck(Entity source, Entity target, Utils.TileActionAttempt check)
		{
			return Collision.CanHitWithCheck(source.position, source.width, source.height, target.position, target.width, target.height, check);
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00274030 File Offset: 0x00272230
		public static bool CanHit(Entity source, NPCAimedTarget target, Utils.TileActionAttempt check)
		{
			return Collision.CanHitWithCheck(source.position, source.width, source.height, target.Position, target.Width, target.Height, check);
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0027405C File Offset: 0x0027225C
		public static bool CanHitWithCheck(Vector2 Position1, int Width1, int Height1, Vector2 Position2, int Width2, int Height2, Utils.TileActionAttempt check)
		{
			int num = (int)((Position1.X + (float)(Width1 / 2)) / 16f);
			int num2 = (int)((Position1.Y + (float)(Height1 / 2)) / 16f);
			int num3 = (int)((Position2.X + (float)(Width2 / 2)) / 16f);
			int num4 = (int)((Position2.Y + (float)(Height2 / 2)) / 16f);
			if (num <= 1)
			{
				num = 1;
			}
			if (num >= Main.maxTilesX)
			{
				num = Main.maxTilesX - 1;
			}
			if (num3 <= 1)
			{
				num3 = 1;
			}
			if (num3 >= Main.maxTilesX)
			{
				num3 = Main.maxTilesX - 1;
			}
			if (num2 <= 1)
			{
				num2 = 1;
			}
			if (num2 >= Main.maxTilesY - 40)
			{
				num2 = Main.maxTilesY - 40;
			}
			if (num4 <= 1)
			{
				num4 = 1;
			}
			if (num4 >= Main.maxTilesY - 40)
			{
				num4 = Main.maxTilesY - 40;
			}
			bool result;
			try
			{
				for (;;)
				{
					int num5 = Math.Abs(num - num3);
					int num6 = Math.Abs(num2 - num4);
					if (num == num3 && num2 == num4)
					{
						break;
					}
					if (num5 > num6)
					{
						if (num < num3)
						{
							num++;
						}
						else
						{
							num--;
						}
						if (Main.tile[num, num2 - 1] == null)
						{
							goto Block_14;
						}
						if (Main.tile[num, num2 + 1] == null)
						{
							goto Block_15;
						}
						if (!Main.tile[num, num2 - 1].inActive() && Main.tile[num, num2 - 1].active() && Main.tileSolid[(int)Main.tile[num, num2 - 1].type] && !Main.tileSolidTop[(int)Main.tile[num, num2 - 1].type] && Main.tile[num, num2 - 1].slope() == 0 && !Main.tile[num, num2 - 1].halfBrick() && !Main.tile[num, num2 + 1].inActive() && Main.tile[num, num2 + 1].active() && Main.tileSolid[(int)Main.tile[num, num2 + 1].type] && !Main.tileSolidTop[(int)Main.tile[num, num2 + 1].type] && Main.tile[num, num2 + 1].slope() == 0 && !Main.tile[num, num2 + 1].halfBrick())
						{
							goto Block_27;
						}
					}
					else
					{
						if (num2 < num4)
						{
							num2++;
						}
						else
						{
							num2--;
						}
						if (Main.tile[num - 1, num2] == null)
						{
							goto Block_29;
						}
						if (Main.tile[num + 1, num2] == null)
						{
							goto Block_30;
						}
						if (!Main.tile[num - 1, num2].inActive() && Main.tile[num - 1, num2].active() && Main.tileSolid[(int)Main.tile[num - 1, num2].type] && !Main.tileSolidTop[(int)Main.tile[num - 1, num2].type] && Main.tile[num - 1, num2].slope() == 0 && !Main.tile[num - 1, num2].halfBrick() && !Main.tile[num + 1, num2].inActive() && Main.tile[num + 1, num2].active() && Main.tileSolid[(int)Main.tile[num + 1, num2].type] && !Main.tileSolidTop[(int)Main.tile[num + 1, num2].type] && Main.tile[num + 1, num2].slope() == 0 && !Main.tile[num + 1, num2].halfBrick())
						{
							goto Block_42;
						}
					}
					if (Main.tile[num, num2] == null)
					{
						goto Block_43;
					}
					if (!Main.tile[num, num2].inActive() && Main.tile[num, num2].active() && Main.tileSolid[(int)Main.tile[num, num2].type] && !Main.tileSolidTop[(int)Main.tile[num, num2].type])
					{
						goto Block_47;
					}
					if (!check(num, num2))
					{
						goto Block_48;
					}
				}
				return true;
				Block_14:
				return false;
				Block_15:
				return false;
				Block_27:
				return false;
				Block_29:
				return false;
				Block_30:
				return false;
				Block_42:
				return false;
				Block_43:
				return false;
				Block_47:
				return false;
				Block_48:
				result = false;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x002744D8 File Offset: 0x002726D8
		public static bool CanHitLine(Vector2 Position1, int Width1, int Height1, Vector2 Position2, int Width2, int Height2)
		{
			int num = (int)((Position1.X + (float)(Width1 / 2)) / 16f);
			int num2 = (int)((Position1.Y + (float)(Height1 / 2)) / 16f);
			int num3 = (int)((Position2.X + (float)(Width2 / 2)) / 16f);
			int num4 = (int)((Position2.Y + (float)(Height2 / 2)) / 16f);
			if (num <= 1)
			{
				num = 1;
			}
			if (num >= Main.maxTilesX)
			{
				num = Main.maxTilesX - 1;
			}
			if (num3 <= 1)
			{
				num3 = 1;
			}
			if (num3 >= Main.maxTilesX)
			{
				num3 = Main.maxTilesX - 1;
			}
			if (num2 <= 1)
			{
				num2 = 1;
			}
			if (num2 >= Main.maxTilesY - 40)
			{
				num2 = Main.maxTilesY - 40;
			}
			if (num4 <= 1)
			{
				num4 = 1;
			}
			if (num4 >= Main.maxTilesY - 40)
			{
				num4 = Main.maxTilesY - 40;
			}
			float num5 = (float)Math.Abs(num - num3);
			float num6 = (float)Math.Abs(num2 - num4);
			if (num5 == 0f && num6 == 0f)
			{
				return true;
			}
			float num7 = 1f;
			float num8 = 1f;
			if (num5 == 0f || num6 == 0f)
			{
				if (num5 == 0f)
				{
					num7 = 0f;
				}
				if (num6 == 0f)
				{
					num8 = 0f;
				}
			}
			else if (num5 > num6)
			{
				num7 = num5 / num6;
			}
			else
			{
				num8 = num6 / num5;
			}
			float num9 = 0f;
			float num10 = 0f;
			int num11 = 1;
			if (num2 < num4)
			{
				num11 = 2;
			}
			int num12 = (int)num5;
			int num13 = (int)num6;
			int num14 = Math.Sign(num3 - num);
			int num15 = Math.Sign(num4 - num2);
			bool flag = false;
			bool flag2 = false;
			bool result;
			try
			{
				for (;;)
				{
					if (num11 == 2)
					{
						num9 += num7;
						int num16 = (int)num9;
						num9 -= (float)num16;
						for (int i = 0; i < num16; i++)
						{
							if (Main.tile[num, num2 - 1] == null)
							{
								goto Block_18;
							}
							if (Main.tile[num, num2] == null)
							{
								goto Block_19;
							}
							if (Main.tile[num, num2 + 1] == null)
							{
								goto Block_20;
							}
							Tile tile = Main.tile[num, num2 - 1];
							Tile tile2 = Main.tile[num, num2 + 1];
							Tile tile3 = Main.tile[num, num2];
							if ((!tile.inActive() && tile.active() && Main.tileSolid[(int)tile.type] && !Main.tileSolidTop[(int)tile.type]) || (!tile2.inActive() && tile2.active() && Main.tileSolid[(int)tile2.type] && !Main.tileSolidTop[(int)tile2.type]) || (!tile3.inActive() && tile3.active() && Main.tileSolid[(int)tile3.type] && !Main.tileSolidTop[(int)tile3.type]))
							{
								goto IL_294;
							}
							if (num12 == 0 && num13 == 0)
							{
								flag = true;
								break;
							}
							num += num14;
							num12--;
							if (num12 == 0 && num13 == 0 && num16 == 1)
							{
								flag2 = true;
							}
						}
						if (num13 != 0)
						{
							num11 = 1;
						}
					}
					else if (num11 == 1)
					{
						num10 += num8;
						int num17 = (int)num10;
						num10 -= (float)num17;
						for (int j = 0; j < num17; j++)
						{
							if (Main.tile[num - 1, num2] == null)
							{
								goto Block_37;
							}
							if (Main.tile[num, num2] == null)
							{
								goto Block_38;
							}
							if (Main.tile[num + 1, num2] == null)
							{
								goto Block_39;
							}
							Tile tile4 = Main.tile[num - 1, num2];
							Tile tile5 = Main.tile[num + 1, num2];
							Tile tile6 = Main.tile[num, num2];
							if ((!tile4.inActive() && tile4.active() && Main.tileSolid[(int)tile4.type] && !Main.tileSolidTop[(int)tile4.type]) || (!tile5.inActive() && tile5.active() && Main.tileSolid[(int)tile5.type] && !Main.tileSolidTop[(int)tile5.type]) || (!tile6.inActive() && tile6.active() && Main.tileSolid[(int)tile6.type] && !Main.tileSolidTop[(int)tile6.type]))
							{
								goto IL_40A;
							}
							if (num12 == 0 && num13 == 0)
							{
								flag = true;
								break;
							}
							num2 += num15;
							num13--;
							if (num12 == 0 && num13 == 0 && num17 == 1)
							{
								flag2 = true;
							}
						}
						if (num12 != 0)
						{
							num11 = 2;
						}
					}
					if (Main.tile[num, num2] == null)
					{
						goto Block_55;
					}
					Tile tile7 = Main.tile[num, num2];
					if (!tile7.inActive() && tile7.active() && Main.tileSolid[(int)tile7.type] && !Main.tileSolidTop[(int)tile7.type])
					{
						goto Block_59;
					}
					if (flag || flag2)
					{
						goto Block_60;
					}
				}
				Block_18:
				return false;
				Block_19:
				return false;
				Block_20:
				return false;
				IL_294:
				return false;
				Block_37:
				return false;
				Block_38:
				return false;
				Block_39:
				return false;
				IL_40A:
				return false;
				Block_55:
				return false;
				Block_59:
				return false;
				Block_60:
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x002749C0 File Offset: 0x00272BC0
		public static bool HitLine(int x1, int y1, int x2, int y2, int ignoreX, int ignoreY, List<Point> ignoreTargets, out Point col)
		{
			int num = Utils.Clamp<int>(x1, 1, Main.maxTilesX - 1);
			int num2 = Utils.Clamp<int>(x2, 1, Main.maxTilesX - 1);
			int num3 = Utils.Clamp<int>(y1, 1, Main.maxTilesY - 40);
			int num4 = Utils.Clamp<int>(y2, 1, Main.maxTilesY - 40);
			float num5 = (float)Math.Abs(num - num2);
			float num6 = (float)Math.Abs(num3 - num4);
			if (num5 == 0f && num6 == 0f)
			{
				col = new Point(num, num3);
				return true;
			}
			float num7 = 1f;
			float num8 = 1f;
			if (num5 == 0f || num6 == 0f)
			{
				if (num5 == 0f)
				{
					num7 = 0f;
				}
				if (num6 == 0f)
				{
					num8 = 0f;
				}
			}
			else if (num5 > num6)
			{
				num7 = num5 / num6;
			}
			else
			{
				num8 = num6 / num5;
			}
			float num9 = 0f;
			float num10 = 0f;
			int num11 = 1;
			if (num3 < num4)
			{
				num11 = 2;
			}
			int num12 = (int)num5;
			int num13 = (int)num6;
			int num14 = Math.Sign(num2 - num);
			int num15 = Math.Sign(num4 - num3);
			bool flag = false;
			bool flag2 = false;
			bool result;
			try
			{
				for (;;)
				{
					if (num11 == 2)
					{
						num9 += num7;
						int num16 = (int)num9;
						num9 -= (float)num16;
						for (int i = 0; i < num16; i++)
						{
							if (Main.tile[num, num3 - 1] == null)
							{
								goto Block_10;
							}
							if (Main.tile[num, num3 + 1] == null)
							{
								goto Block_11;
							}
							Tile tile = Main.tile[num, num3 - 1];
							Tile tile2 = Main.tile[num, num3 + 1];
							Tile tile3 = Main.tile[num, num3];
							if (!ignoreTargets.Contains(new Point(num, num3)) && !ignoreTargets.Contains(new Point(num, num3 - 1)) && !ignoreTargets.Contains(new Point(num, num3 + 1)))
							{
								if (ignoreY != -1 && num15 < 0 && !tile.inActive() && tile.active() && Main.tileSolid[(int)tile.type] && !Main.tileSolidTop[(int)tile.type])
								{
									goto Block_20;
								}
								if (ignoreY != 1 && num15 > 0 && !tile2.inActive() && tile2.active() && Main.tileSolid[(int)tile2.type] && !Main.tileSolidTop[(int)tile2.type])
								{
									goto Block_26;
								}
								if (!tile3.inActive() && tile3.active() && Main.tileSolid[(int)tile3.type] && !Main.tileSolidTop[(int)tile3.type])
								{
									goto Block_30;
								}
							}
							if (num12 == 0 && num13 == 0)
							{
								flag = true;
								break;
							}
							num += num14;
							num12--;
							if (num12 == 0 && num13 == 0 && num16 == 1)
							{
								flag2 = true;
							}
						}
						if (num13 != 0)
						{
							num11 = 1;
						}
					}
					else if (num11 == 1)
					{
						num10 += num8;
						int num17 = (int)num10;
						num10 -= (float)num17;
						for (int j = 0; j < num17; j++)
						{
							if (Main.tile[num - 1, num3] == null)
							{
								goto Block_38;
							}
							if (Main.tile[num + 1, num3] == null)
							{
								goto Block_39;
							}
							Tile tile4 = Main.tile[num - 1, num3];
							Tile tile5 = Main.tile[num + 1, num3];
							Tile tile6 = Main.tile[num, num3];
							if (!ignoreTargets.Contains(new Point(num, num3)) && !ignoreTargets.Contains(new Point(num - 1, num3)) && !ignoreTargets.Contains(new Point(num + 1, num3)))
							{
								if (ignoreX != -1 && num14 < 0 && !tile4.inActive() && tile4.active() && Main.tileSolid[(int)tile4.type] && !Main.tileSolidTop[(int)tile4.type])
								{
									goto Block_48;
								}
								if (ignoreX != 1 && num14 > 0 && !tile5.inActive() && tile5.active() && Main.tileSolid[(int)tile5.type] && !Main.tileSolidTop[(int)tile5.type])
								{
									goto Block_54;
								}
								if (!tile6.inActive() && tile6.active() && Main.tileSolid[(int)tile6.type] && !Main.tileSolidTop[(int)tile6.type])
								{
									goto Block_58;
								}
							}
							if (num12 == 0 && num13 == 0)
							{
								flag = true;
								break;
							}
							num3 += num15;
							num13--;
							if (num12 == 0 && num13 == 0 && num17 == 1)
							{
								flag2 = true;
							}
						}
						if (num12 != 0)
						{
							num11 = 2;
						}
					}
					if (Main.tile[num, num3] == null)
					{
						goto Block_65;
					}
					Tile tile7 = Main.tile[num, num3];
					if (!ignoreTargets.Contains(new Point(num, num3)) && !tile7.inActive() && tile7.active() && Main.tileSolid[(int)tile7.type] && !Main.tileSolidTop[(int)tile7.type])
					{
						goto Block_70;
					}
					if (flag || flag2)
					{
						goto Block_71;
					}
				}
				Block_10:
				col = new Point(num, num3 - 1);
				return false;
				Block_11:
				col = new Point(num, num3 + 1);
				return false;
				Block_20:
				col = new Point(num, num3 - 1);
				return true;
				Block_26:
				col = new Point(num, num3 + 1);
				return true;
				Block_30:
				col = new Point(num, num3);
				return true;
				Block_38:
				col = new Point(num - 1, num3);
				return false;
				Block_39:
				col = new Point(num + 1, num3);
				return false;
				Block_48:
				col = new Point(num - 1, num3);
				return true;
				Block_54:
				col = new Point(num + 1, num3);
				return true;
				Block_58:
				col = new Point(num, num3);
				return true;
				Block_65:
				col = new Point(num, num3);
				return false;
				Block_70:
				col = new Point(num, num3);
				return true;
				Block_71:
				col = new Point(num, num3);
				result = true;
			}
			catch
			{
				col = new Point(x1, y1);
				result = false;
			}
			return result;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00274FCC File Offset: 0x002731CC
		public static bool AnyWallOfTypeOnLine(int x1, int y1, int x2, int y2, ushort wallId)
		{
			int num = x1;
			int num2 = y1;
			int num3 = x2;
			int num4 = y2;
			if (num <= 1)
			{
				num = 1;
			}
			if (num >= Main.maxTilesX)
			{
				num = Main.maxTilesX - 1;
			}
			if (num3 <= 1)
			{
				num3 = 1;
			}
			if (num3 >= Main.maxTilesX)
			{
				num3 = Main.maxTilesX - 1;
			}
			if (num2 <= 1)
			{
				num2 = 1;
			}
			if (num2 >= Main.maxTilesY - 40)
			{
				num2 = Main.maxTilesY - 40;
			}
			if (num4 <= 1)
			{
				num4 = 1;
			}
			if (num4 >= Main.maxTilesY - 40)
			{
				num4 = Main.maxTilesY - 40;
			}
			float num5 = (float)Math.Abs(num - num3);
			float num6 = (float)Math.Abs(num2 - num4);
			if (num5 == 0f && num6 == 0f)
			{
				return false;
			}
			float num7 = 1f;
			float num8 = 1f;
			if (num5 == 0f || num6 == 0f)
			{
				if (num5 == 0f)
				{
					num7 = 0f;
				}
				if (num6 == 0f)
				{
					num8 = 0f;
				}
			}
			else if (num5 > num6)
			{
				num7 = num5 / num6;
			}
			else
			{
				num8 = num6 / num5;
			}
			float num9 = 0f;
			float num10 = 0f;
			int num11 = 1;
			if (num2 < num4)
			{
				num11 = 2;
			}
			int num12 = (int)num5;
			int num13 = (int)num6;
			int num14 = Math.Sign(num3 - num);
			int num15 = Math.Sign(num4 - num2);
			bool flag = false;
			bool flag2 = false;
			bool result;
			try
			{
				for (;;)
				{
					if (num11 == 2)
					{
						num9 += num7;
						int num16 = (int)num9;
						num9 -= (float)num16;
						for (int i = 0; i < num16; i++)
						{
							Main.tile[num, num2];
							if (Collision.HitSpecificWallSubstep(num, num2, wallId))
							{
								goto Block_18;
							}
							if (num12 == 0 && num13 == 0)
							{
								flag = true;
								break;
							}
							num += num14;
							num12--;
							if (num12 == 0 && num13 == 0 && num16 == 1)
							{
								flag2 = true;
							}
						}
						if (num13 != 0)
						{
							num11 = 1;
						}
					}
					else if (num11 == 1)
					{
						num10 += num8;
						int num17 = (int)num10;
						num10 -= (float)num17;
						for (int j = 0; j < num17; j++)
						{
							Main.tile[num, num2];
							if (Collision.HitSpecificWallSubstep(num, num2, wallId))
							{
								goto Block_26;
							}
							if (num12 == 0 && num13 == 0)
							{
								flag = true;
								break;
							}
							num2 += num15;
							num13--;
							if (num12 == 0 && num13 == 0 && num17 == 1)
							{
								flag2 = true;
							}
						}
						if (num12 != 0)
						{
							num11 = 2;
						}
					}
					if (Main.tile[num, num2] == null)
					{
						goto Block_33;
					}
					Main.tile[num, num2];
					if (Collision.HitSpecificWallSubstep(num, num2, wallId))
					{
						goto Block_34;
					}
					if (flag || flag2)
					{
						goto Block_35;
					}
				}
				Block_18:
				return true;
				Block_26:
				return true;
				Block_33:
				return false;
				Block_34:
				return true;
				Block_35:
				result = false;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00275258 File Offset: 0x00273458
		public static bool HitSpecificWallSubstep(int x, int y, ushort wallId)
		{
			return Main.tile[x, y].wall == wallId;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00275274 File Offset: 0x00273474
		public static Point HitLineWall(int x1, int y1, int x2, int y2)
		{
			int num = x1;
			int num2 = y1;
			int num3 = x2;
			int num4 = y2;
			if (num <= 1)
			{
				num = 1;
			}
			if (num >= Main.maxTilesX)
			{
				num = Main.maxTilesX - 1;
			}
			if (num3 <= 1)
			{
				num3 = 1;
			}
			if (num3 >= Main.maxTilesX)
			{
				num3 = Main.maxTilesX - 1;
			}
			if (num2 <= 1)
			{
				num2 = 1;
			}
			if (num2 >= Main.maxTilesY - 40)
			{
				num2 = Main.maxTilesY - 40;
			}
			if (num4 <= 1)
			{
				num4 = 1;
			}
			if (num4 >= Main.maxTilesY - 40)
			{
				num4 = Main.maxTilesY - 40;
			}
			float num5 = (float)Math.Abs(num - num3);
			float num6 = (float)Math.Abs(num2 - num4);
			if (num5 == 0f && num6 == 0f)
			{
				return new Point(num, num2);
			}
			float num7 = 1f;
			float num8 = 1f;
			if (num5 == 0f || num6 == 0f)
			{
				if (num5 == 0f)
				{
					num7 = 0f;
				}
				if (num6 == 0f)
				{
					num8 = 0f;
				}
			}
			else if (num5 > num6)
			{
				num7 = num5 / num6;
			}
			else
			{
				num8 = num6 / num5;
			}
			float num9 = 0f;
			float num10 = 0f;
			int num11 = 1;
			if (num2 < num4)
			{
				num11 = 2;
			}
			int num12 = (int)num5;
			int num13 = (int)num6;
			int num14 = Math.Sign(num3 - num);
			int num15 = Math.Sign(num4 - num2);
			bool flag = false;
			bool flag2 = false;
			Point result;
			try
			{
				for (;;)
				{
					if (num11 == 2)
					{
						num9 += num7;
						int num16 = (int)num9;
						num9 -= (float)num16;
						for (int i = 0; i < num16; i++)
						{
							Main.tile[num, num2];
							if (Collision.HitWallSubstep(num, num2))
							{
								goto Block_18;
							}
							if (num12 == 0 && num13 == 0)
							{
								flag = true;
								break;
							}
							num += num14;
							num12--;
							if (num12 == 0 && num13 == 0 && num16 == 1)
							{
								flag2 = true;
							}
						}
						if (num13 != 0)
						{
							num11 = 1;
						}
					}
					else if (num11 == 1)
					{
						num10 += num8;
						int num17 = (int)num10;
						num10 -= (float)num17;
						for (int j = 0; j < num17; j++)
						{
							Main.tile[num, num2];
							if (Collision.HitWallSubstep(num, num2))
							{
								goto Block_26;
							}
							if (num12 == 0 && num13 == 0)
							{
								flag = true;
								break;
							}
							num2 += num15;
							num13--;
							if (num12 == 0 && num13 == 0 && num17 == 1)
							{
								flag2 = true;
							}
						}
						if (num12 != 0)
						{
							num11 = 2;
						}
					}
					if (Main.tile[num, num2] == null)
					{
						goto Block_33;
					}
					Main.tile[num, num2];
					if (Collision.HitWallSubstep(num, num2))
					{
						goto Block_34;
					}
					if (flag || flag2)
					{
						goto Block_35;
					}
				}
				Block_18:
				return new Point(num, num2);
				Block_26:
				return new Point(num, num2);
				Block_33:
				return new Point(-1, -1);
				Block_34:
				return new Point(num, num2);
				Block_35:
				result = new Point(num, num2);
			}
			catch
			{
				result = new Point(-1, -1);
			}
			return result;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00275528 File Offset: 0x00273728
		public static bool HitWallSubstep(int x, int y)
		{
			if (Main.tile[x, y].wall == 0)
			{
				return false;
			}
			bool flag = false;
			if (Main.wallHouse[(int)Main.tile[x, y].wall])
			{
				flag = true;
			}
			if (!flag)
			{
				for (int i = -1; i < 2; i++)
				{
					for (int j = -1; j < 2; j++)
					{
						if ((i != 0 || j != 0) && Main.tile[x + i, y + j].wall == 0)
						{
							flag = true;
						}
					}
				}
			}
			if (Main.tile[x, y].active() && flag)
			{
				bool flag2 = true;
				for (int k = -1; k < 2; k++)
				{
					for (int l = -1; l < 2; l++)
					{
						if (k != 0 || l != 0)
						{
							Tile tile = Main.tile[x + k, y + l];
							if (!tile.active() || !Main.tileSolid[(int)tile.type] || Main.tileSolidTop[(int)tile.type])
							{
								flag2 = false;
							}
						}
					}
				}
				if (flag2)
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00275624 File Offset: 0x00273824
		public static bool EmptyTile(int i, int j, bool ignoreTiles = false)
		{
			Rectangle rectangle = new Rectangle(i * 16, j * 16, 16, 16);
			if (Main.tile[i, j].active() && !ignoreTiles)
			{
				return false;
			}
			for (int k = 0; k < 255; k++)
			{
				if (Main.player[k].active && !Main.player[k].dead && !Main.player[k].ghost && rectangle.Intersects(new Rectangle((int)Main.player[k].position.X, (int)Main.player[k].position.Y, Main.player[k].width, Main.player[k].height)))
				{
					return false;
				}
			}
			for (int l = 0; l < Main.maxNPCs; l++)
			{
				if (Main.npc[l].active && rectangle.Intersects(new Rectangle((int)Main.npc[l].position.X, (int)Main.npc[l].position.Y, Main.npc[l].width, Main.npc[l].height)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00275750 File Offset: 0x00273950
		public static bool DrownCollision(Vector2 Position, int Width, int Height, float gravDir = -1f, bool includeSlopes = false)
		{
			Vector2 vector = new Vector2(Position.X + (float)(Width / 2), Position.Y + (float)(Height / 2));
			int num = 10;
			int num2 = 12;
			if (num > Width)
			{
				num = Width;
			}
			if (num2 > Height)
			{
				num2 = Height;
			}
			vector = new Vector2(vector.X - (float)(num / 2), Position.Y + -2f);
			if (gravDir == -1f)
			{
				vector.Y += (float)(Height / 2 - 6);
			}
			int value = (int)(Position.X / 16f) - 1;
			int num3 = (int)((Position.X + (float)Width) / 16f) + 2;
			int num4 = (int)(Position.Y / 16f) - 1;
			int num5 = (int)((Position.Y + (float)Height) / 16f) + 2;
			int num6 = Utils.Clamp<int>(value, 0, Main.maxTilesX - 1);
			num3 = Utils.Clamp<int>(num3, 0, Main.maxTilesX - 1);
			num4 = Utils.Clamp<int>(num4, 0, Main.maxTilesY - 40);
			num5 = Utils.Clamp<int>(num5, 0, Main.maxTilesY - 40);
			int num7 = (gravDir == 1f) ? num4 : (num5 - 1);
			for (int i = num6; i < num3; i++)
			{
				for (int j = num4; j < num5; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile != null && tile.liquid > 0 && !tile.lava() && !tile.shimmer() && (j != num7 || !tile.active() || !Main.tileSolid[(int)tile.type] || Main.tileSolidTop[(int)tile.type] || (includeSlopes && tile.blockType() != 0)))
					{
						Vector2 vector2;
						vector2.X = (float)(i * 16);
						vector2.Y = (float)(j * 16);
						int num8 = 16;
						float num9 = (float)(256 - (int)Main.tile[i, j].liquid);
						num9 /= 32f;
						vector2.Y += num9 * 2f;
						num8 -= (int)(num9 * 2f);
						if (vector.X + (float)num > vector2.X && vector.X < vector2.X + 16f && vector.Y + (float)num2 > vector2.Y && vector.Y < vector2.Y + (float)num8)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x002759BC File Offset: 0x00273BBC
		public static bool IsWorldPointSolid(Vector2 pos, bool treatPlatformsAsNonSolid = false)
		{
			Point point = pos.ToTileCoordinates();
			if (!WorldGen.InWorld(point.X, point.Y, 1))
			{
				return false;
			}
			Tile tile = Main.tile[point.X, point.Y];
			if (tile == null || !tile.active() || tile.inActive() || !Main.tileSolid[(int)tile.type])
			{
				return false;
			}
			if (treatPlatformsAsNonSolid && tile.type > 0 && tile.type <= TileID.Count && (TileID.Sets.Platforms[(int)tile.type] || tile.type == 380))
			{
				return false;
			}
			int num = tile.blockType();
			switch (num)
			{
			case 0:
				return pos.X >= (float)(point.X * 16) && pos.X <= (float)(point.X * 16 + 16) && pos.Y >= (float)(point.Y * 16) && pos.Y <= (float)(point.Y * 16 + 16);
			case 1:
				return pos.X >= (float)(point.X * 16) && pos.X <= (float)(point.X * 16 + 16) && pos.Y >= (float)(point.Y * 16 + 8) && pos.Y <= (float)(point.Y * 16 + 16);
			case 2:
			case 3:
			case 4:
			case 5:
			{
				if (pos.X < (float)(point.X * 16) && pos.X > (float)(point.X * 16 + 16) && pos.Y < (float)(point.Y * 16) && pos.Y > (float)(point.Y * 16 + 16))
				{
					return false;
				}
				float num2 = pos.X % 16f;
				float num3 = pos.Y % 16f;
				switch (num)
				{
				case 2:
					return num3 >= num2;
				case 3:
					return num2 + num3 >= 16f;
				case 4:
					return num2 + num3 <= 16f;
				case 5:
					return num3 <= num2;
				}
				break;
			}
			}
			return false;
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00275BE0 File Offset: 0x00273DE0
		public static bool GetWaterLine(Point pt, out float waterLineHeight)
		{
			return Collision.GetWaterLine(pt.X, pt.Y, out waterLineHeight);
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00275BF4 File Offset: 0x00273DF4
		public static bool GetWaterLine(int X, int Y, out float waterLineHeight)
		{
			waterLineHeight = 0f;
			if (!WorldGen.InWorld(X, Y, 10))
			{
				return false;
			}
			if (Main.tile[X, Y - 2] == null)
			{
				Main.tile[X, Y - 2] = new Tile();
			}
			if (Main.tile[X, Y - 1] == null)
			{
				Main.tile[X, Y - 1] = new Tile();
			}
			if (Main.tile[X, Y] == null)
			{
				Main.tile[X, Y] = new Tile();
			}
			if (Main.tile[X, Y + 1] == null)
			{
				Main.tile[X, Y + 1] = new Tile();
			}
			if (Main.tile[X, Y - 2].liquid > 0)
			{
				return false;
			}
			if (Main.tile[X, Y - 1].liquid > 0)
			{
				waterLineHeight = (float)(Y * 16);
				waterLineHeight -= (float)(Main.tile[X, Y - 1].liquid / 16);
				return true;
			}
			if (Main.tile[X, Y].liquid > 0)
			{
				waterLineHeight = (float)((Y + 1) * 16);
				waterLineHeight -= (float)(Main.tile[X, Y].liquid / 16);
				return true;
			}
			if (Main.tile[X, Y + 1].liquid > 0)
			{
				waterLineHeight = (float)((Y + 2) * 16);
				waterLineHeight -= (float)(Main.tile[X, Y + 1].liquid / 16);
				return true;
			}
			return false;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00275D67 File Offset: 0x00273F67
		public static bool GetWaterLineIterate(Point pt, out float waterLineHeight)
		{
			return Collision.GetWaterLineIterate(pt.X, pt.Y, out waterLineHeight);
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00275D7C File Offset: 0x00273F7C
		public static bool GetWaterLineIterate(int X, int Y, out float waterLineHeight)
		{
			waterLineHeight = 0f;
			while (Y > 0 && Framing.GetTileSafely(X, Y).liquid > 0)
			{
				Y--;
			}
			Y++;
			if (Main.tile[X, Y] == null)
			{
				Main.tile[X, Y] = new Tile();
			}
			if (Main.tile[X, Y].liquid > 0)
			{
				waterLineHeight = (float)(Y * 16);
				waterLineHeight -= (float)(Main.tile[X, Y - 1].liquid / 16);
				return true;
			}
			return false;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00275E08 File Offset: 0x00274008
		public static bool WetCollision(Vector2 Position, int Width, int Height)
		{
			Collision.honey = false;
			Collision.shimmer = false;
			Vector2 vector = new Vector2(Position.X + (float)(Width / 2), Position.Y + (float)(Height / 2));
			int num = 10;
			int num2 = Height / 2;
			if (num > Width)
			{
				num = Width;
			}
			if (num2 > Height)
			{
				num2 = Height;
			}
			vector = new Vector2(vector.X - (float)(num / 2), vector.Y - (float)(num2 / 2));
			int value = (int)(Position.X / 16f) - 1;
			int num3 = (int)((Position.X + (float)Width) / 16f) + 2;
			int num4 = (int)(Position.Y / 16f) - 1;
			int num5 = (int)((Position.Y + (float)Height) / 16f) + 2;
			int num6 = Utils.Clamp<int>(value, 0, Main.maxTilesX - 1);
			num3 = Utils.Clamp<int>(num3, 0, Main.maxTilesX - 1);
			num4 = Utils.Clamp<int>(num4, 0, Main.maxTilesY - 40);
			num5 = Utils.Clamp<int>(num5, 0, Main.maxTilesY - 40);
			for (int i = num6; i < num3; i++)
			{
				for (int j = num4; j < num5; j++)
				{
					if (Main.tile[i, j] != null)
					{
						if (Main.tile[i, j].liquid > 0)
						{
							Vector2 vector2;
							vector2.X = (float)(i * 16);
							vector2.Y = (float)(j * 16);
							int num7 = 16;
							float num8 = (float)(256 - (int)Main.tile[i, j].liquid);
							num8 /= 32f;
							vector2.Y += num8 * 2f;
							num7 -= (int)(num8 * 2f);
							if (vector.X + (float)num > vector2.X && vector.X < vector2.X + 16f && vector.Y + (float)num2 > vector2.Y && vector.Y < vector2.Y + (float)num7)
							{
								if (Main.tile[i, j].honey())
								{
									Collision.honey = true;
								}
								if (Main.tile[i, j].shimmer())
								{
									Collision.shimmer = true;
								}
								return true;
							}
						}
						else if (Main.tile[i, j].active() && Main.tile[i, j].slope() != 0 && j > 0 && Main.tile[i, j - 1] != null && Main.tile[i, j - 1].liquid > 0)
						{
							Vector2 vector2;
							vector2.X = (float)(i * 16);
							vector2.Y = (float)(j * 16);
							int num9 = 16;
							if (vector.X + (float)num > vector2.X && vector.X < vector2.X + 16f && vector.Y + (float)num2 > vector2.Y && vector.Y < vector2.Y + (float)num9)
							{
								if (Main.tile[i, j - 1].honey())
								{
									Collision.honey = true;
								}
								else if (Main.tile[i, j - 1].shimmer())
								{
									Collision.shimmer = true;
								}
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00276150 File Offset: 0x00274350
		public static bool LavaCollision(Vector2 Position, int Width, int Height)
		{
			int value = (int)(Position.X / 16f) - 1;
			int num = (int)((Position.X + (float)Width) / 16f) + 2;
			int num2 = (int)(Position.Y / 16f) - 1;
			int num3 = (int)((Position.Y + (float)Height) / 16f) + 2;
			int num4 = Utils.Clamp<int>(value, 0, Main.maxTilesX - 1);
			num = Utils.Clamp<int>(num, 0, Main.maxTilesX - 1);
			num2 = Utils.Clamp<int>(num2, 0, Main.maxTilesY - 40);
			num3 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 40);
			for (int i = num4; i < num; i++)
			{
				for (int j = num2; j < num3; j++)
				{
					if (Main.tile[i, j] != null && Main.tile[i, j].liquid > 0 && Main.tile[i, j].lava())
					{
						Vector2 vector;
						vector.X = (float)(i * 16);
						vector.Y = (float)(j * 16);
						int num5 = 16;
						float num6 = (float)(256 - (int)Main.tile[i, j].liquid);
						num6 /= 32f;
						vector.Y += num6 * 2f;
						num5 -= (int)(num6 * 2f);
						if (Position.X + (float)Width > vector.X && Position.X < vector.X + 16f && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + (float)num5)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00276304 File Offset: 0x00274504
		public static Vector4 WalkDownSlope(Vector2 Position, Vector2 Velocity, int Width, int Height, float gravity = 0f)
		{
			if (Velocity.Y != gravity)
			{
				return new Vector4(Position, Velocity.X, Velocity.Y);
			}
			int num = (int)(Position.X / 16f);
			int num2 = (int)((Position.X + (float)Width) / 16f);
			int num3 = (int)((Position.Y + (float)Height + 4f) / 16f);
			num = Utils.Clamp<int>(num, 0, Main.maxTilesX - 1);
			num2 = Utils.Clamp<int>(num2, 0, Main.maxTilesX - 1);
			num3 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 2 - 40);
			float num4 = (float)((num3 + 3) * 16);
			int num5 = -1;
			int num6 = -1;
			int num7 = 1;
			if (Velocity.X < 0f)
			{
				num7 = 2;
			}
			for (int i = num; i <= num2; i++)
			{
				for (int j = num3; j <= num3 + 1; j++)
				{
					if (Main.tile[i, j] == null)
					{
						Main.tile[i, j] = new Tile();
					}
					if (Main.tile[i, j].nactive() && (Main.tileSolid[(int)Main.tile[i, j].type] || Main.tileSolidTop[(int)Main.tile[i, j].type]))
					{
						int num8 = j * 16;
						if (Main.tile[i, j].halfBrick())
						{
							num8 += 8;
						}
						Rectangle rectangle = new Rectangle(i * 16, j * 16 - 17, 16, 16);
						if (rectangle.Intersects(new Rectangle((int)Position.X, (int)Position.Y, Width, Height)) && (float)num8 <= num4)
						{
							if (num4 == (float)num8)
							{
								if (Main.tile[i, j].slope() != 0)
								{
									if (num5 != -1 && num6 != -1 && Main.tile[num5, num6] != null && Main.tile[num5, num6].slope() != 0)
									{
										if ((int)Main.tile[i, j].slope() == num7)
										{
											num4 = (float)num8;
											num5 = i;
											num6 = j;
										}
									}
									else
									{
										num4 = (float)num8;
										num5 = i;
										num6 = j;
									}
								}
							}
							else
							{
								num4 = (float)num8;
								num5 = i;
								num6 = j;
							}
						}
					}
				}
			}
			int num9 = num5;
			int num10 = num6;
			if (num5 != -1 && num6 != -1 && Main.tile[num9, num10] != null && Main.tile[num9, num10].slope() > 0)
			{
				int num11 = (int)Main.tile[num9, num10].slope();
				Vector2 vector;
				vector.X = (float)(num9 * 16);
				vector.Y = (float)(num10 * 16);
				if (num11 == 2)
				{
					float num12 = vector.X + 16f - (Position.X + (float)Width);
					if (Position.Y + (float)Height >= vector.Y + num12 && Velocity.X < 0f)
					{
						Velocity.Y += Math.Abs(Velocity.X);
					}
				}
				else if (num11 == 1)
				{
					float num12 = Position.X - vector.X;
					if (Position.Y + (float)Height >= vector.Y + num12 && Velocity.X > 0f)
					{
						Velocity.Y += Math.Abs(Velocity.X);
					}
				}
			}
			return new Vector4(Position, Velocity.X, Velocity.Y);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00276670 File Offset: 0x00274870
		public static Vector4 SlopeCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, float gravity = 0f, bool fall = false, bool ignoreAetheriumPlatforms = false)
		{
			Collision.stair = false;
			Collision.stairFall = false;
			BitsByte bitsByte = 0;
			float y = Position.Y;
			float y2 = Position.Y;
			Collision.sloping = false;
			Vector2 vector = Position;
			Vector2 vector2 = Velocity;
			int value = (int)(Position.X / 16f) - 1;
			int num = (int)((Position.X + (float)Width) / 16f) + 2;
			int num2 = (int)(Position.Y / 16f) - 1;
			int num3 = (int)((Position.Y + (float)Height) / 16f) + 2;
			int num4 = Utils.Clamp<int>(value, 0, Main.maxTilesX - 1);
			num = Utils.Clamp<int>(num, 0, Main.maxTilesX - 1);
			num2 = Utils.Clamp<int>(num2, 0, Main.maxTilesY - 40);
			num3 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 40);
			for (int i = num4; i < num; i++)
			{
				for (int j = num2; j < num3; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile != null && tile.active() && !tile.inActive())
					{
						bool flag = Main.tileSolid[(int)tile.type];
						if (Main.tileSolidTop[(int)tile.type] && tile.frameY == 0)
						{
							flag = true;
						}
						if (ignoreAetheriumPlatforms && tile.type == 19 && tile.frameY / 18 == 50)
						{
							flag = false;
						}
						if (flag)
						{
							Vector2 vector3;
							vector3.X = (float)(i * 16);
							vector3.Y = (float)(j * 16);
							int num5 = 16;
							if (Main.tile[i, j].halfBrick())
							{
								vector3.Y += 8f;
								num5 -= 8;
							}
							if (Position.X + (float)Width > vector3.X && Position.X < vector3.X + 16f && Position.Y + (float)Height > vector3.Y && Position.Y < vector3.Y + (float)num5)
							{
								bool flag2 = true;
								if (TileID.Sets.Platforms[(int)Main.tile[i, j].type])
								{
									if (Velocity.Y < 0f)
									{
										flag2 = false;
									}
									if (Position.Y + (float)Height < (float)(j * 16) || Position.Y + (float)Height - (1f + Math.Abs(Velocity.X)) > (float)(j * 16 + 16))
									{
										flag2 = false;
									}
									if (((Main.tile[i, j].slope() == 1 && Velocity.X >= 0f) || (Main.tile[i, j].slope() == 2 && Velocity.X <= 0f)) && (Position.Y + (float)Height) / 16f - 1f == (float)j)
									{
										flag2 = false;
									}
								}
								if (flag2)
								{
									bool flag3 = false;
									if (fall && TileID.Sets.Platforms[(int)Main.tile[i, j].type])
									{
										flag3 = true;
									}
									int num6 = (int)Main.tile[i, j].slope();
									vector3.X = (float)(i * 16);
									vector3.Y = (float)(j * 16);
									if (Position.X + (float)Width > vector3.X && Position.X < vector3.X + 16f && Position.Y + (float)Height > vector3.Y && Position.Y < vector3.Y + 16f)
									{
										float num7 = 0f;
										if (num6 == 3 || num6 == 4)
										{
											if (num6 == 3)
											{
												num7 = Position.X - vector3.X;
											}
											if (num6 == 4)
											{
												num7 = vector3.X + 16f - (Position.X + (float)Width);
											}
											if (num7 >= 0f)
											{
												if (Position.Y <= vector3.Y + 16f - num7)
												{
													float num8 = vector3.Y + 16f - Position.Y - num7;
													if (Position.Y + num8 > y2)
													{
														vector.Y = Position.Y + num8;
														y2 = vector.Y;
														if (vector2.Y < 0.0101f)
														{
															vector2.Y = 0.0101f;
														}
														bitsByte[num6] = true;
													}
												}
											}
											else if (Position.Y > vector3.Y)
											{
												float num9 = vector3.Y + 16f;
												if (vector.Y < num9)
												{
													vector.Y = num9;
													if (vector2.Y < 0.0101f)
													{
														vector2.Y = 0.0101f;
													}
												}
											}
										}
										if (num6 == 1 || num6 == 2)
										{
											if (num6 == 1)
											{
												num7 = Position.X - vector3.X;
											}
											if (num6 == 2)
											{
												num7 = vector3.X + 16f - (Position.X + (float)Width);
											}
											if (num7 >= 0f)
											{
												if (Position.Y + (float)Height >= vector3.Y + num7)
												{
													float num10 = vector3.Y - (Position.Y + (float)Height) + num7;
													if (Position.Y + num10 < y)
													{
														if (flag3)
														{
															Collision.stairFall = true;
														}
														else
														{
															if (TileID.Sets.Platforms[(int)Main.tile[i, j].type])
															{
																Collision.stair = true;
															}
															else
															{
																Collision.stair = false;
															}
															vector.Y = Position.Y + num10;
															y = vector.Y;
															if (vector2.Y > 0f)
															{
																vector2.Y = 0f;
															}
															bitsByte[num6] = true;
														}
													}
												}
											}
											else if (TileID.Sets.Platforms[(int)Main.tile[i, j].type] && Position.Y + (float)Height - 4f - Math.Abs(Velocity.X) > vector3.Y)
											{
												if (flag3)
												{
													Collision.stairFall = true;
												}
											}
											else
											{
												float num11 = vector3.Y - (float)Height;
												if (vector.Y > num11)
												{
													if (flag3)
													{
														Collision.stairFall = true;
													}
													else
													{
														if (TileID.Sets.Platforms[(int)Main.tile[i, j].type])
														{
															Collision.stair = true;
														}
														else
														{
															Collision.stair = false;
														}
														vector.Y = num11;
														if (vector2.Y > 0f)
														{
															vector2.Y = 0f;
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			Vector2 vector4 = vector - Position;
			Vector2 vector5 = Collision.TileCollision(Position, vector4, Width, Height, false, false, 1, false, false, true);
			if (vector5.Y > vector4.Y)
			{
				float num12 = vector4.Y - vector5.Y;
				vector.Y = Position.Y + vector5.Y;
				if (bitsByte[1])
				{
					vector.X = Position.X - num12;
				}
				if (bitsByte[2])
				{
					vector.X = Position.X + num12;
				}
				vector2.X = 0f;
				vector2.Y = 0f;
				Collision.up = false;
			}
			else if (vector5.Y < vector4.Y)
			{
				float num13 = vector5.Y - vector4.Y;
				vector.Y = Position.Y + vector5.Y;
				if (bitsByte[3])
				{
					vector.X = Position.X - num13;
				}
				if (bitsByte[4])
				{
					vector.X = Position.X + num13;
				}
				vector2.X = 0f;
				vector2.Y = 0f;
			}
			return new Vector4(vector, vector2.X, vector2.Y);
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00276E24 File Offset: 0x00275024
		public static Vector2 noSlopeCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false)
		{
			Collision.up = false;
			Collision.down = false;
			Vector2 result = Velocity;
			Vector2 vector = Position + Velocity;
			int value = (int)(Position.X / 16f) - 1;
			int num = (int)((Position.X + (float)Width) / 16f) + 2;
			int num2 = (int)(Position.Y / 16f) - 1;
			int num3 = (int)((Position.Y + (float)Height) / 16f) + 2;
			int num4 = -1;
			int num5 = -1;
			int num6 = -1;
			int num7 = -1;
			int num8 = Utils.Clamp<int>(value, 0, Main.maxTilesX - 1);
			num = Utils.Clamp<int>(num, 0, Main.maxTilesX - 1);
			num2 = Utils.Clamp<int>(num2, 0, Main.maxTilesY - 40);
			num3 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 40);
			float num9 = (float)((num3 + 3) * 16);
			for (int i = num8; i < num; i++)
			{
				for (int j = num2; j < num3; j++)
				{
					if (Main.tile[i, j] != null && Main.tile[i, j].active() && (Main.tileSolid[(int)Main.tile[i, j].type] || (Main.tileSolidTop[(int)Main.tile[i, j].type] && Main.tile[i, j].frameY == 0)))
					{
						Vector2 vector2;
						vector2.X = (float)(i * 16);
						vector2.Y = (float)(j * 16);
						int num10 = 16;
						if (Main.tile[i, j].halfBrick())
						{
							vector2.Y += 8f;
							num10 -= 8;
						}
						if (vector.X + (float)Width > vector2.X && vector.X < vector2.X + 16f && vector.Y + (float)Height > vector2.Y && vector.Y < vector2.Y + (float)num10)
						{
							if (Position.Y + (float)Height <= vector2.Y)
							{
								Collision.down = true;
								if ((!Main.tileSolidTop[(int)Main.tile[i, j].type] || !fallThrough || (Velocity.Y > 1f && !fall2)) && num9 > vector2.Y)
								{
									num6 = i;
									num7 = j;
									if (num10 < 16)
									{
										num7++;
									}
									if (num6 != num4)
									{
										result.Y = vector2.Y - (Position.Y + (float)Height);
										num9 = vector2.Y;
									}
								}
							}
							else if (Position.X + (float)Width <= vector2.X && !Main.tileSolidTop[(int)Main.tile[i, j].type])
							{
								num4 = i;
								num5 = j;
								if (num5 != num7)
								{
									result.X = vector2.X - (Position.X + (float)Width);
								}
								if (num6 == num4)
								{
									result.Y = Velocity.Y;
								}
							}
							else if (Position.X >= vector2.X + 16f && !Main.tileSolidTop[(int)Main.tile[i, j].type])
							{
								num4 = i;
								num5 = j;
								if (num5 != num7)
								{
									result.X = vector2.X + 16f - Position.X;
								}
								if (num6 == num4)
								{
									result.Y = Velocity.Y;
								}
							}
							else if (Position.Y >= vector2.Y + (float)num10 && !Main.tileSolidTop[(int)Main.tile[i, j].type])
							{
								Collision.up = true;
								num6 = i;
								num7 = j;
								result.Y = vector2.Y + (float)num10 - Position.Y + 0.01f;
								if (num7 == num5)
								{
									result.X = Velocity.X;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00277220 File Offset: 0x00275420
		public static void BuildTileContacts(Vector2 Position, int Width, int Height, List<Collision.TileContact> contactTiles)
		{
			Position.X = (float)((int)Position.X);
			Position.Y = (float)((int)Position.Y);
			contactTiles.Clear();
			int value = (int)((Position.X - 1f) / 16f) - 1;
			int num = (int)((Position.X + 1f + (float)Width) / 16f) + 1;
			int num2 = (int)((Position.Y - 1f) / 16f) - 1;
			int num3 = (int)((Position.Y + 3f + (float)Height) / 16f) + 1;
			int num4 = Utils.Clamp<int>(value, 0, Main.maxTilesX - 1);
			num = Utils.Clamp<int>(num, 0, Main.maxTilesX - 1);
			num2 = Utils.Clamp<int>(num2, 0, Main.maxTilesY - 40);
			num3 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 40);
			for (int i = num4; i < num; i++)
			{
				for (int j = num2; j < num3; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile != null && tile.active() && !tile.inActive())
					{
						ushort type = tile.type;
						bool flag = Main.tileSolid[(int)type];
						if (Main.tileSolidTop[(int)type] && tile.frameY == 0)
						{
							flag = true;
						}
						if (flag)
						{
							Vector2 vector;
							vector.X = (float)(i * 16);
							vector.Y = (float)(j * 16);
							int num5 = 16;
							if (tile.halfBrick())
							{
								vector.Y += 8f;
								num5 -= 8;
							}
							byte b = tile.slope();
							if (Math.Abs(Position.X - (vector.X + 16f)) < 0.1f && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + (float)num5 && b != 3 && b != 1)
							{
								float num6 = Math.Max(Math.Min(Position.Y + (float)Height, vector.Y + (float)num5) - Math.Max(Position.Y, vector.Y) + 0.5f, 1f);
								contactTiles.Add(new Collision.TileContact(Collision.TileContactSide.Left, i, j, (int)type, (int)b, (int)num6));
							}
							if (Math.Abs(Position.X + (float)Width - vector.X) < 0.1f && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + (float)num5 && b != 4 && b != 2)
							{
								float num7 = Math.Max(Math.Min(Position.Y + (float)Height, vector.Y + (float)num5) - Math.Max(Position.Y, vector.Y) + 0.5f, 1f);
								contactTiles.Add(new Collision.TileContact(Collision.TileContactSide.Right, i, j, (int)type, (int)b, (int)num7));
							}
							if (Position.Y + 3f + (float)Height > vector.Y && Position.Y - 1f < vector.Y + (float)num5)
							{
								if (Position.X + (float)Width > vector.X && Position.X < vector.X + 16f)
								{
									float num8 = Math.Max(Math.Min(Position.X + (float)Width, vector.X + 16f) - Math.Max(Position.X, vector.X) + 0.5f, 1f);
									switch (b)
									{
									case 0:
										if (Math.Abs(Position.Y - (vector.Y + (float)num5)) < 0.1f)
										{
											contactTiles.Add(new Collision.TileContact(Collision.TileContactSide.Top, i, j, (int)type, (int)b, (int)num8));
										}
										if (Math.Abs(Position.Y + (float)Height - vector.Y) < 0.1f)
										{
											contactTiles.Add(new Collision.TileContact(Collision.TileContactSide.Bottom, i, j, (int)type, (int)b, (int)num8));
										}
										break;
									case 1:
									{
										if (Math.Abs(Position.Y - (vector.Y + (float)num5)) < 0.1f)
										{
											contactTiles.Add(new Collision.TileContact(Collision.TileContactSide.Top, i, j, (int)type, (int)b, (int)num8));
										}
										float num9 = Math.Max(Position.X - vector.X, 0f);
										float num10 = Position.Y + (float)Height;
										if (num10 - vector.Y > -0.1f && num10 - (vector.Y + num9) < 0.1f)
										{
											contactTiles.Add(new Collision.TileContact(Collision.TileContactSide.Bottom, i, j, (int)type, (int)b, (int)num8));
										}
										break;
									}
									case 2:
									{
										if (Math.Abs(Position.Y - (vector.Y + (float)num5)) < 0.1f)
										{
											contactTiles.Add(new Collision.TileContact(Collision.TileContactSide.Top, i, j, (int)type, (int)b, (int)num8));
										}
										float num11 = Math.Max(vector.X + 16f - (Position.X + (float)Width), 0f);
										float num12 = Position.Y + (float)Height;
										if (num12 - vector.Y > -0.1f && num12 - (vector.Y + num11) < 0.1f)
										{
											contactTiles.Add(new Collision.TileContact(Collision.TileContactSide.Bottom, i, j, (int)type, (int)b, (int)num8));
										}
										break;
									}
									case 3:
									{
										float num13 = Math.Max(Position.X - vector.X, 0f);
										if (Math.Abs(Position.Y - (vector.Y + (float)num5 - num13)) < 0.1f)
										{
											contactTiles.Add(new Collision.TileContact(Collision.TileContactSide.Top, i, j, (int)type, (int)b, (int)num8));
										}
										if (Math.Abs(Position.Y + (float)Height - vector.Y) < 0.1f)
										{
											contactTiles.Add(new Collision.TileContact(Collision.TileContactSide.Bottom, i, j, (int)type, (int)b, (int)num8));
										}
										break;
									}
									case 4:
									{
										float num14 = Math.Max(vector.X + 16f - (Position.X + (float)Width), 0f);
										if (Math.Abs(Position.Y - (vector.Y + (float)num5 - num14)) < 0.1f)
										{
											contactTiles.Add(new Collision.TileContact(Collision.TileContactSide.Top, i, j, (int)type, (int)b, (int)num8));
										}
										if (Math.Abs(Position.Y + (float)Height - vector.Y) < 0.1f)
										{
											contactTiles.Add(new Collision.TileContact(Collision.TileContactSide.Bottom, i, j, (int)type, (int)b, (int)num8));
										}
										break;
									}
									}
								}
								else if (Position.X + 3f + (float)Width > vector.X && Position.X - 3f < vector.X + 16f && Position.Y < vector.Y)
								{
									Collision.TileContactSide tileContactSide = (vector.X < Position.X) ? Collision.TileContactSide.BottomLeft : Collision.TileContactSide.BottomRight;
									switch (b)
									{
									case 0:
									case 3:
									case 4:
										contactTiles.Add(new Collision.TileContact(tileContactSide, i, j, (int)type, (int)b, 0));
										break;
									case 1:
										if (tileContactSide == Collision.TileContactSide.BottomRight)
										{
											contactTiles.Add(new Collision.TileContact(tileContactSide, i, j, (int)type, (int)b, 0));
										}
										break;
									case 2:
										if (tileContactSide == Collision.TileContactSide.BottomLeft)
										{
											contactTiles.Add(new Collision.TileContact(tileContactSide, i, j, (int)type, (int)b, 0));
										}
										break;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00277954 File Offset: 0x00275B54
		public static Vector2 TileCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1, bool ignoreDoors = false, bool ignoreAetheriumPlatforms = false, bool hoik = true)
		{
			Collision.up = false;
			Collision.down = false;
			Vector2 result = Velocity;
			Vector2 vector = Position + Velocity;
			int value = (int)(Position.X / 16f) - 1;
			int num = (int)((Position.X + (float)Width) / 16f) + 2;
			int num2 = (int)(Position.Y / 16f) - 1;
			int num3 = (int)((Position.Y + (float)Height) / 16f) + 2;
			int num4 = -1;
			int num5 = -1;
			int num6 = -1;
			int num7 = -1;
			int num8 = Utils.Clamp<int>(value, 0, Main.maxTilesX - 1);
			num = Utils.Clamp<int>(num, 0, Main.maxTilesX - 1);
			num2 = Utils.Clamp<int>(num2, 0, Main.maxTilesY - 40);
			num3 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 40);
			float num9 = (float)((num3 + 3) * 16);
			for (int i = num8; i < num; i++)
			{
				for (int j = num2; j < num3; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile != null && tile.active() && !tile.inActive())
					{
						bool flag = Main.tileSolid[(int)tile.type];
						if (Main.tileSolidTop[(int)tile.type] && tile.frameY == 0)
						{
							flag = true;
						}
						if (ignoreDoors && TileID.Sets.ForAdvancedCollision.ClosedDoors[(int)tile.type])
						{
							flag = false;
						}
						if (ignoreAetheriumPlatforms && tile.type == 19 && tile.frameY / 18 == 50)
						{
							flag = false;
						}
						if (flag)
						{
							Vector2 vector2;
							vector2.X = (float)(i * 16);
							vector2.Y = (float)(j * 16);
							int num10 = 16;
							if (Main.tile[i, j].halfBrick())
							{
								vector2.Y += 8f;
								num10 -= 8;
							}
							if (vector.X + (float)Width > vector2.X && vector.X < vector2.X + 16f && vector.Y + (float)Height > vector2.Y && vector.Y < vector2.Y + (float)num10)
							{
								bool flag2 = false;
								bool flag3 = false;
								if (Main.tile[i, j].slope() > 2)
								{
									if (Main.tile[i, j].slope() == 3 && Position.Y + Math.Abs(Velocity.X) >= vector2.Y && Position.X >= vector2.X)
									{
										flag3 = true;
									}
									if (Main.tile[i, j].slope() == 4 && Position.Y + Math.Abs(Velocity.X) >= vector2.Y && Position.X + (float)Width <= vector2.X + 16f)
									{
										flag3 = true;
									}
								}
								else if (Main.tile[i, j].slope() > 0)
								{
									flag2 = true;
									if (Main.tile[i, j].slope() == 1 && Position.Y + (float)Height - Math.Abs(Velocity.X) <= vector2.Y + (float)num10 && Position.X >= vector2.X)
									{
										flag3 = true;
									}
									if (Main.tile[i, j].slope() == 2 && Position.Y + (float)Height - Math.Abs(Velocity.X) <= vector2.Y + (float)num10 && Position.X + (float)Width <= vector2.X + 16f)
									{
										flag3 = true;
									}
								}
								if (!flag3)
								{
									if (Position.Y + (float)Height <= vector2.Y)
									{
										Collision.down = true;
										if ((!Main.tileSolidTop[(int)Main.tile[i, j].type] || !fallThrough || (Velocity.Y > 1f && !fall2)) && num9 > vector2.Y)
										{
											num6 = i;
											num7 = j;
											if (num10 < 16)
											{
												num7++;
											}
											if (num6 != num4 && !flag2)
											{
												result.Y = vector2.Y - (Position.Y + (float)Height) + ((gravDir == -1) ? -0.01f : 0f);
												num9 = vector2.Y;
											}
										}
									}
									else if (Position.X + (float)Width <= vector2.X && !Main.tileSolidTop[(int)Main.tile[i, j].type])
									{
										if (i >= 1 && Main.tile[i - 1, j] == null)
										{
											Main.tile[i - 1, j] = new Tile();
										}
										if (!hoik || i < 1 || (Main.tile[i - 1, j].slope() != 2 && Main.tile[i - 1, j].slope() != 4))
										{
											num4 = i;
											num5 = j;
											if (num5 != num7)
											{
												result.X = vector2.X - (Position.X + (float)Width);
											}
											if (num6 == num4)
											{
												result.Y = Velocity.Y;
											}
										}
									}
									else if (Position.X >= vector2.X + 16f && !Main.tileSolidTop[(int)Main.tile[i, j].type])
									{
										if (Main.tile[i + 1, j] == null)
										{
											Main.tile[i + 1, j] = new Tile();
										}
										if (!hoik || (Main.tile[i + 1, j].slope() != 1 && Main.tile[i + 1, j].slope() != 3))
										{
											num4 = i;
											num5 = j;
											if (num5 != num7)
											{
												result.X = vector2.X + 16f - Position.X;
											}
											if (num6 == num4)
											{
												result.Y = Velocity.Y;
											}
										}
									}
									else if (Position.Y >= vector2.Y + (float)num10 && !Main.tileSolidTop[(int)Main.tile[i, j].type])
									{
										Collision.up = true;
										num6 = i;
										num7 = j;
										result.Y = vector2.Y + (float)num10 - Position.Y + ((gravDir == 1) ? 0.01f : 0f);
										if (num7 == num5)
										{
											result.X = Velocity.X;
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00277FE4 File Offset: 0x002761E4
		public static float TileCollisionInStepsOf16(Vector2 Position, Vector2 normalizedDirection, float amount, int Width, int Height)
		{
			float num = 16f;
			Vector2 value = new Vector2(Position.X + 1f, Position.Y);
			int width = Width - 1;
			for (float num2 = 0f; num2 < amount; num2 += num)
			{
				Vector2 value2 = normalizedDirection * num2;
				Vector2 position = Position + value2;
				Vector2 vector = value + value2;
				float num3 = Math.Min(num, amount - num2);
				Vector2 vector2 = normalizedDirection * num3;
				Vector4 vec = Collision.SlopeCollision(vector, vector2, width, Height, 0f, false, false);
				if (vec.XY() != vector || vec.ZW() != vector2)
				{
					float num4 = (vec.XY() - vector).Length();
					return num2 - num4;
				}
				vec = Collision.SlopeCollision(vector + vector2, vector2, width, Height, 0f, false, false);
				if (vec.XY() != vector + vector2 || vec.ZW() != vector2)
				{
					float num5 = (vec.XY() - vector - vector2).Length();
					return num2 + num3 - num5;
				}
				Vector2 value3 = Collision.TileCollision(position, vector2, Width, Height, false, false, 1, false, false, true);
				if (value3 != vector2)
				{
					return num2 + value3.Length();
				}
			}
			return amount;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00278144 File Offset: 0x00276344
		public static bool TryChangingSizeFromBottomCenter(Rectangle hitbox, int targetWidth, int targetHeight, out Rectangle changedHitbox)
		{
			changedHitbox = hitbox;
			int num = targetHeight - hitbox.Height;
			if (num > 0)
			{
				int num2 = (int)Collision.TileCollisionInStepsOf16(changedHitbox.TopLeft(), Vector2.UnitY, (float)num, changedHitbox.Width, changedHitbox.Height);
				int num3 = (int)Collision.TileCollisionInStepsOf16(changedHitbox.TopLeft(), -Vector2.UnitY, (float)num, changedHitbox.Width, changedHitbox.Height);
				if (num3 + num2 < num)
				{
					return false;
				}
				int num4 = Math.Min(num, num3);
				int num5 = 0;
				int val = num - num4 - num5;
				num4 += Math.Min(val, num3 - num4);
				val = num - num4 - num5;
				num5 += Math.Min(val, num2 - num5);
				changedHitbox.Offset(0, -num4);
				changedHitbox.Height += num4 + num5;
			}
			else
			{
				changedHitbox.Offset(0, -num);
				changedHitbox.Height = targetHeight;
			}
			int num6 = targetWidth - hitbox.Width;
			if (num6 > 0)
			{
				int num7 = (int)Collision.TileCollisionInStepsOf16(changedHitbox.TopLeft(), Vector2.UnitX, (float)num6, changedHitbox.Width, changedHitbox.Height);
				int num8 = (int)Collision.TileCollisionInStepsOf16(changedHitbox.TopLeft(), -Vector2.UnitX, (float)num6, changedHitbox.Width, changedHitbox.Height);
				if (num8 + num7 < num6)
				{
					return false;
				}
				int num10;
				int num9 = num10 = Math.Min(num6 / 2, Math.Min(num8, num7));
				int val2 = num6 - num9 - num10;
				num9 += Math.Min(val2, num8 - num9);
				val2 = num6 - num9 - num10;
				num10 += Math.Min(val2, num7 - num10);
				changedHitbox.Offset(-num9, 0);
				changedHitbox.Width += num9 + num10;
			}
			else
			{
				changedHitbox.Offset(num6 / 2, 0);
				changedHitbox.Width = targetWidth;
			}
			return true;
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0027830C File Offset: 0x0027650C
		public static bool IsClearSpotTest(Vector2 position, float testMagnitude, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1, bool checkCardinals = true, bool checkSlopes = false)
		{
			if (checkCardinals)
			{
				Vector2 vector = Vector2.UnitX * testMagnitude;
				if (Collision.TileCollision(position - vector, vector, Width, Height, fallThrough, fall2, gravDir, false, false, true) != vector)
				{
					return false;
				}
				vector = -Vector2.UnitX * testMagnitude;
				if (Collision.TileCollision(position - vector, vector, Width, Height, fallThrough, fall2, gravDir, false, false, true) != vector)
				{
					return false;
				}
				vector = Vector2.UnitY * testMagnitude;
				if (Collision.TileCollision(position - vector, vector, Width, Height, fallThrough, fall2, gravDir, false, false, true) != vector)
				{
					return false;
				}
				vector = -Vector2.UnitY * testMagnitude;
				if (Collision.TileCollision(position - vector, vector, Width, Height, fallThrough, fall2, gravDir, false, false, true) != vector)
				{
					return false;
				}
			}
			if (checkSlopes)
			{
				Vector2 vector = Vector2.UnitX * testMagnitude;
				Vector4 value = new Vector4(position, testMagnitude, 0f);
				if (Collision.SlopeCollision(position, vector, Width, Height, (float)gravDir, fallThrough, false) != value)
				{
					return false;
				}
				vector = -Vector2.UnitX * testMagnitude;
				value = new Vector4(position, -testMagnitude, 0f);
				if (Collision.SlopeCollision(position, vector, Width, Height, (float)gravDir, fallThrough, false) != value)
				{
					return false;
				}
				vector = Vector2.UnitY * testMagnitude;
				value = new Vector4(position, 0f, testMagnitude);
				if (Collision.SlopeCollision(position, vector, Width, Height, (float)gravDir, fallThrough, false) != value)
				{
					return false;
				}
				vector = -Vector2.UnitY * testMagnitude;
				value = new Vector4(position, 0f, -testMagnitude);
				if (Collision.SlopeCollision(position, vector, Width, Height, (float)gravDir, fallThrough, false) != value)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x002784C4 File Offset: 0x002766C4
		public static List<Point> FindCollisionTile(int Direction, Vector2 position, float testMagnitude, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1, bool checkCardinals = true, bool checkSlopes = false)
		{
			List<Point> list = new List<Point>();
			if (Direction > 1)
			{
				if (Direction - 2 <= 1)
				{
					Vector2 vector = (Direction == 2) ? (Vector2.UnitY * testMagnitude) : (-Vector2.UnitY * testMagnitude);
					Vector4 vec = new Vector4(position, vector.X, vector.Y);
					int num = (int)(position.Y + (float)((Direction == 2) ? Height : 0)) / 16;
					float num2 = Math.Min(16f - position.X % 16f, (float)Width);
					float num3 = num2;
					if (checkCardinals && Collision.TileCollision(position - vector, vector, (int)num2, Height, fallThrough, fall2, gravDir, false, false, true) != vector)
					{
						list.Add(new Point((int)position.X / 16, num));
					}
					else if (checkSlopes && Collision.SlopeCollision(position, vector, (int)num2, Height, (float)gravDir, fallThrough, false).YZW() != vec.YZW())
					{
						list.Add(new Point((int)position.X / 16, num));
					}
					while (num3 + 16f <= (float)(Width - 16))
					{
						if (checkCardinals && Collision.TileCollision(position - vector + Vector2.UnitX * num3, vector, 16, Height, fallThrough, fall2, gravDir, false, false, true) != vector)
						{
							list.Add(new Point((int)(position.X + num3) / 16, num));
						}
						else if (checkSlopes && Collision.SlopeCollision(position + Vector2.UnitX * num3, vector, 16, Height, (float)gravDir, fallThrough, false).YZW() != vec.YZW())
						{
							list.Add(new Point((int)(position.X + num3) / 16, num));
						}
						num3 += 16f;
					}
					int width = Width - (int)num3;
					if (checkCardinals && Collision.TileCollision(position - vector + Vector2.UnitX * num3, vector, width, Height, fallThrough, fall2, gravDir, false, false, true) != vector)
					{
						list.Add(new Point((int)(position.X + num3) / 16, num));
					}
					else if (checkSlopes && Collision.SlopeCollision(position + Vector2.UnitX * num3, vector, width, Height, (float)gravDir, fallThrough, false).YZW() != vec.YZW())
					{
						list.Add(new Point((int)(position.X + num3) / 16, num));
					}
				}
			}
			else
			{
				Vector2 vector = (Direction == 0) ? (Vector2.UnitX * testMagnitude) : (-Vector2.UnitX * testMagnitude);
				Vector4 vec = new Vector4(position, vector.X, vector.Y);
				int num = (int)(position.X + (float)((Direction == 0) ? Width : 0)) / 16;
				float num4 = Math.Min(16f - position.Y % 16f, (float)Height);
				float num5 = num4;
				if (checkCardinals && Collision.TileCollision(position - vector, vector, Width, (int)num4, fallThrough, fall2, gravDir, false, false, true) != vector)
				{
					list.Add(new Point(num, (int)position.Y / 16));
				}
				else if (checkSlopes && Collision.SlopeCollision(position, vector, Width, (int)num4, (float)gravDir, fallThrough, false).XZW() != vec.XZW())
				{
					list.Add(new Point(num, (int)position.Y / 16));
				}
				while (num5 + 16f <= (float)(Height - 16))
				{
					if (checkCardinals && Collision.TileCollision(position - vector + Vector2.UnitY * num5, vector, Width, 16, fallThrough, fall2, gravDir, false, false, true) != vector)
					{
						list.Add(new Point(num, (int)(position.Y + num5) / 16));
					}
					else if (checkSlopes && Collision.SlopeCollision(position + Vector2.UnitY * num5, vector, Width, 16, (float)gravDir, fallThrough, false).XZW() != vec.XZW())
					{
						list.Add(new Point(num, (int)(position.Y + num5) / 16));
					}
					num5 += 16f;
				}
				int height = Height - (int)num5;
				if (checkCardinals && Collision.TileCollision(position - vector + Vector2.UnitY * num5, vector, Width, height, fallThrough, fall2, gravDir, false, false, true) != vector)
				{
					list.Add(new Point(num, (int)(position.Y + num5) / 16));
				}
				else if (checkSlopes && Collision.SlopeCollision(position + Vector2.UnitY * num5, vector, Width, height, (float)gravDir, fallThrough, false).XZW() != vec.XZW())
				{
					list.Add(new Point(num, (int)(position.Y + num5) / 16));
				}
			}
			return list;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x002789B0 File Offset: 0x00276BB0
		public static bool FindCollisionDirection(out int Direction, Vector2 position, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1)
		{
			Vector2 vector = Vector2.UnitX * 16f;
			if (Collision.TileCollision(position - vector, vector, Width, Height, fallThrough, fall2, gravDir, false, false, true) != vector)
			{
				Direction = 0;
				return true;
			}
			vector = -Vector2.UnitX * 16f;
			if (Collision.TileCollision(position - vector, vector, Width, Height, fallThrough, fall2, gravDir, false, false, true) != vector)
			{
				Direction = 1;
				return true;
			}
			vector = Vector2.UnitY * 16f;
			if (Collision.TileCollision(position - vector, vector, Width, Height, fallThrough, fall2, gravDir, false, false, true) != vector)
			{
				Direction = 2;
				return true;
			}
			vector = -Vector2.UnitY * 16f;
			if (Collision.TileCollision(position - vector, vector, Width, Height, fallThrough, fall2, gravDir, false, false, true) != vector)
			{
				Direction = 3;
				return true;
			}
			Direction = -1;
			return false;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00278AA0 File Offset: 0x00276CA0
		public static bool SolidCollision(Vector2 Position, int Width, int Height)
		{
			int value = (int)(Position.X / 16f) - 1;
			int num = (int)((Position.X + (float)Width) / 16f) + 2;
			int num2 = (int)(Position.Y / 16f) - 1;
			int num3 = (int)((Position.Y + (float)Height) / 16f) + 2;
			int num4 = Utils.Clamp<int>(value, 0, Main.maxTilesX - 1);
			num = Utils.Clamp<int>(num, 0, Main.maxTilesX - 1);
			num2 = Utils.Clamp<int>(num2, 0, Main.maxTilesY - 40);
			num3 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 40);
			for (int i = num4; i < num; i++)
			{
				for (int j = num2; j < num3; j++)
				{
					if (Main.tile[i, j] != null && !Main.tile[i, j].inActive() && Main.tile[i, j].active() && Main.tileSolid[(int)Main.tile[i, j].type] && !Main.tileSolidTop[(int)Main.tile[i, j].type])
					{
						Vector2 vector;
						vector.X = (float)(i * 16);
						vector.Y = (float)(j * 16);
						int num5 = 16;
						if (Main.tile[i, j].halfBrick())
						{
							vector.Y += 8f;
							num5 -= 8;
						}
						if (Position.X + (float)Width > vector.X && Position.X < vector.X + 16f && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + (float)num5)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00278C6C File Offset: 0x00276E6C
		public static bool SolidCollision(Vector2 Position, int Width, int Height, bool acceptTopSurfaces)
		{
			int value = (int)(Position.X / 16f) - 1;
			int num = (int)((Position.X + (float)Width) / 16f) + 2;
			int num2 = (int)(Position.Y / 16f) - 1;
			int num3 = (int)((Position.Y + (float)Height) / 16f) + 2;
			int num4 = Utils.Clamp<int>(value, 0, Main.maxTilesX - 1);
			num = Utils.Clamp<int>(num, 0, Main.maxTilesX - 1);
			num2 = Utils.Clamp<int>(num2, 0, Main.maxTilesY - 40);
			num3 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 40);
			for (int i = num4; i < num; i++)
			{
				for (int j = num2; j < num3; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile != null && tile.active() && !tile.inActive())
					{
						bool flag = Main.tileSolid[(int)tile.type] && !Main.tileSolidTop[(int)tile.type];
						if (acceptTopSurfaces)
						{
							if (TileID.Sets.Platforms[(int)tile.type])
							{
								flag |= WorldGen.PlatformProperTopFrame(tile.frameX);
							}
							else
							{
								flag |= (Main.tileSolidTop[(int)tile.type] && tile.frameY == 0);
							}
						}
						if (flag)
						{
							Vector2 vector;
							vector.X = (float)(i * 16);
							vector.Y = (float)(j * 16);
							int num5 = 16;
							if (tile.halfBrick())
							{
								vector.Y += 8f;
								num5 -= 8;
							}
							if (Position.X + (float)Width > vector.X && Position.X < vector.X + 16f && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + (float)num5)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00278E50 File Offset: 0x00277050
		public static Vector2 WaterCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false, bool lavaWalk = true)
		{
			Vector2 result = Velocity;
			Vector2 vector = Position + Velocity;
			int value = (int)(Position.X / 16f) - 1;
			int num = (int)((Position.X + (float)Width) / 16f) + 2;
			int num2 = (int)(Position.Y / 16f) - 1;
			int num3 = (int)((Position.Y + (float)Height) / 16f) + 2;
			int num4 = Utils.Clamp<int>(value, 0, Main.maxTilesX - 1);
			num = Utils.Clamp<int>(num, 0, Main.maxTilesX - 1);
			num2 = Utils.Clamp<int>(num2, 0, Main.maxTilesY - 40);
			num3 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 40);
			for (int i = num4; i < num; i++)
			{
				for (int j = num2; j < num3; j++)
				{
					if (Main.tile[i, j] != null && Main.tile[i, j].liquid > 0 && Main.tile[i, j - 1].liquid == 0 && (!Main.tile[i, j].lava() || lavaWalk))
					{
						int num5 = (int)(Main.tile[i, j].liquid / 32 * 2 + 2);
						Vector2 vector2;
						vector2.X = (float)(i * 16);
						vector2.Y = (float)(j * 16 + 16 - num5);
						if (vector.X + (float)Width > vector2.X && vector.X < vector2.X + 16f && vector.Y + (float)Height > vector2.Y && vector.Y < vector2.Y + (float)num5 && Position.Y + (float)Height <= vector2.Y && !fallThrough)
						{
							result.Y = vector2.Y - (Position.Y + (float)Height);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00279034 File Offset: 0x00277234
		public static Vector2 AnyCollisionWithSpecificTiles(Vector2 Position, Vector2 Velocity, int Width, int Height, bool[] tilesWeCanCollideWithByType, bool evenActuated = false)
		{
			Vector2 result = Velocity;
			Vector2 vector = Position + Velocity;
			int num = (int)(Position.X / 16f) - 1;
			int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
			int num3 = (int)(Position.Y / 16f) - 1;
			int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
			int num5 = -1;
			int num6 = -1;
			int num7 = -1;
			int num8 = -1;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 > Main.maxTilesX)
			{
				num2 = Main.maxTilesX;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num4 > Main.maxTilesY - 40)
			{
				num4 = Main.maxTilesY - 40;
			}
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile != null && tile.active() && (evenActuated || !tile.inActive()) && tilesWeCanCollideWithByType[(int)tile.type])
					{
						Vector2 vector2;
						vector2.X = (float)(i * 16);
						vector2.Y = (float)(j * 16);
						int num9 = 16;
						if (tile.halfBrick())
						{
							vector2.Y += 8f;
							num9 -= 8;
						}
						if (vector.X + (float)Width > vector2.X && vector.X < vector2.X + 16f && vector.Y + (float)Height > vector2.Y && vector.Y < vector2.Y + (float)num9)
						{
							if (Position.Y + (float)Height <= vector2.Y)
							{
								num7 = i;
								num8 = j;
								if (num7 != num5)
								{
									result.Y = vector2.Y - (Position.Y + (float)Height);
								}
							}
							else if (Position.X + (float)Width <= vector2.X && !Main.tileSolidTop[(int)tile.type])
							{
								num5 = i;
								num6 = j;
								if (num6 != num8)
								{
									result.X = vector2.X - (Position.X + (float)Width);
								}
								if (num7 == num5)
								{
									result.Y = Velocity.Y;
								}
							}
							else if (Position.X >= vector2.X + 16f && !Main.tileSolidTop[(int)tile.type])
							{
								num5 = i;
								num6 = j;
								if (num6 != num8)
								{
									result.X = vector2.X + 16f - Position.X;
								}
								if (num7 == num5)
								{
									result.Y = Velocity.Y;
								}
							}
							else if (Position.Y >= vector2.Y + (float)num9 && !Main.tileSolidTop[(int)tile.type])
							{
								num7 = i;
								num8 = j;
								result.Y = vector2.Y + (float)num9 - Position.Y + 0.01f;
								if (num8 == num6)
								{
									result.X = Velocity.X + 0.01f;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00279348 File Offset: 0x00277548
		public static Vector2 AnyCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool evenActuated = false)
		{
			Vector2 result = Velocity;
			Vector2 vector = Position + Velocity;
			int num = (int)(Position.X / 16f) - 1;
			int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
			int num3 = (int)(Position.Y / 16f) - 1;
			int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
			int num5 = -1;
			int num6 = -1;
			int num7 = -1;
			int num8 = -1;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 > Main.maxTilesX)
			{
				num2 = Main.maxTilesX;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num4 > Main.maxTilesY - 40)
			{
				num4 = Main.maxTilesY - 40;
			}
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					if (Main.tile[i, j] != null && Main.tile[i, j].active() && (evenActuated || !Main.tile[i, j].inActive()))
					{
						Vector2 vector2;
						vector2.X = (float)(i * 16);
						vector2.Y = (float)(j * 16);
						int num9 = 16;
						if (Main.tile[i, j].halfBrick())
						{
							vector2.Y += 8f;
							num9 -= 8;
						}
						if (vector.X + (float)Width > vector2.X && vector.X < vector2.X + 16f && vector.Y + (float)Height > vector2.Y && vector.Y < vector2.Y + (float)num9)
						{
							if (Position.Y + (float)Height <= vector2.Y)
							{
								num7 = i;
								num8 = j;
								if (num7 != num5)
								{
									result.Y = vector2.Y - (Position.Y + (float)Height);
								}
							}
							else if (Position.X + (float)Width <= vector2.X && !Main.tileSolidTop[(int)Main.tile[i, j].type])
							{
								num5 = i;
								num6 = j;
								if (num6 != num8)
								{
									result.X = vector2.X - (Position.X + (float)Width);
								}
								if (num7 == num5)
								{
									result.Y = Velocity.Y;
								}
							}
							else if (Position.X >= vector2.X + 16f && !Main.tileSolidTop[(int)Main.tile[i, j].type])
							{
								num5 = i;
								num6 = j;
								if (num6 != num8)
								{
									result.X = vector2.X + 16f - Position.X;
								}
								if (num7 == num5)
								{
									result.Y = Velocity.Y;
								}
							}
							else if (Position.Y >= vector2.Y + (float)num9 && !Main.tileSolidTop[(int)Main.tile[i, j].type])
							{
								num7 = i;
								num8 = j;
								result.Y = vector2.Y + (float)num9 - Position.Y + 0.01f;
								if (num8 == num6)
								{
									result.X = Velocity.X + 0.01f;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00279690 File Offset: 0x00277890
		public static void HitTilesInACircle(Vector2 Position, Vector2 Velocity, int Width, int Height)
		{
			Vector2 vector = Position + Velocity;
			int num = (int)(Position.X / 16f) - 1;
			int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
			int num3 = (int)(Position.Y / 16f) - 1;
			int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 > Main.maxTilesX)
			{
				num2 = Main.maxTilesX;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num4 > Main.maxTilesY - 40)
			{
				num4 = Main.maxTilesY - 40;
			}
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					if (Main.tile[i, j] != null && !Main.tile[i, j].inActive() && Main.tile[i, j].active() && (Main.tileSolid[(int)Main.tile[i, j].type] || (Main.tileSolidTop[(int)Main.tile[i, j].type] && Main.tile[i, j].frameY == 0)))
					{
						Vector2 vector2;
						vector2.X = (float)(i * 16);
						vector2.Y = (float)(j * 16);
						int num5 = 16;
						if (Main.tile[i, j].halfBrick())
						{
							vector2.Y += 8f;
							num5 -= 8;
						}
						if (vector.X + (float)Width >= vector2.X && vector.X <= vector2.X + 16f && vector.Y + (float)Height >= vector2.Y && vector.Y <= vector2.Y + (float)num5 && (new Vector2(vector.X + (float)(Width / 2), vector.Y + (float)(Height / 2)) - new Vector2(vector2.X + 8f, vector2.Y + 8f)).Length() < (float)((Width + Height) / 4))
						{
							WorldGen.KillTile(i, j, true, true, false);
						}
					}
				}
			}
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x002798D0 File Offset: 0x00277AD0
		public static void HitTiles(Vector2 Position, Vector2 Velocity, int Width, int Height)
		{
			Vector2 vector = Position + Velocity;
			int num = (int)(Position.X / 16f) - 1;
			int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
			int num3 = (int)(Position.Y / 16f) - 1;
			int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 > Main.maxTilesX)
			{
				num2 = Main.maxTilesX;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num4 > Main.maxTilesY - 40)
			{
				num4 = Main.maxTilesY - 40;
			}
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					if (Main.tile[i, j] != null && !Main.tile[i, j].inActive() && Main.tile[i, j].active() && (Main.tileSolid[(int)Main.tile[i, j].type] || (Main.tileSolidTop[(int)Main.tile[i, j].type] && Main.tile[i, j].frameY == 0)))
					{
						Vector2 vector2;
						vector2.X = (float)(i * 16);
						vector2.Y = (float)(j * 16);
						int num5 = 16;
						if (Main.tile[i, j].halfBrick())
						{
							vector2.Y += 8f;
							num5 -= 8;
						}
						if (vector.X + (float)Width >= vector2.X && vector.X <= vector2.X + 16f && vector.Y + (float)Height >= vector2.Y && vector.Y <= vector2.Y + (float)num5)
						{
							WorldGen.KillTile(i, j, true, true, false);
						}
					}
				}
			}
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00279ABE File Offset: 0x00277CBE
		public static bool AnyHurtingTiles(Vector2 Position, int Width, int Height)
		{
			return Collision.HurtTiles(Position, Width, Height, null).type >= 0;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00279AD4 File Offset: 0x00277CD4
		public static Collision.HurtTile HurtTiles(Vector2 Position, int Width, int Height, Player player)
		{
			int num = (int)(Position.X / 16f) - 1;
			int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
			int num3 = (int)(Position.Y / 16f) - 1;
			int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 > Main.maxTilesX)
			{
				num2 = Main.maxTilesX;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num4 > Main.maxTilesY - 40)
			{
				num4 = Main.maxTilesY - 40;
			}
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile != null && !tile.inActive() && tile.active())
					{
						Vector2 vector;
						vector.X = (float)(i * 16);
						vector.Y = (float)(j * 16);
						int num5 = 16;
						if (tile.halfBrick())
						{
							vector.Y += 8f;
							num5 -= 8;
						}
						int num6 = 0;
						if (TileID.Sets.Suffocate[(int)tile.type])
						{
							num6 = 2;
						}
						if (Position.X + (float)Width - (float)num6 >= vector.X && Position.X + (float)num6 <= vector.X + 16f && Position.Y + (float)Height - (float)num6 >= vector.Y - 0.5f && Position.Y + (float)num6 <= vector.Y + (float)num5 + 0.5f && Collision.CanTileHurt(tile.type, i, j, player))
						{
							if (tile.slope() > 0)
							{
								if (num6 > 0)
								{
									goto IL_25F;
								}
								int num7 = 0;
								if (tile.rightSlope() && Position.X > vector.X)
								{
									num7++;
								}
								if (tile.leftSlope() && Position.X + (float)Width < vector.X + 16f)
								{
									num7++;
								}
								if (tile.bottomSlope() && Position.Y > vector.Y)
								{
									num7++;
								}
								if (tile.topSlope() && Position.Y + (float)Height < vector.Y + (float)num5)
								{
									num7++;
								}
								if (num7 == 2)
								{
									goto IL_25F;
								}
							}
							return new Collision.HurtTile
							{
								type = (int)tile.type,
								x = i,
								y = j
							};
						}
					}
					IL_25F:;
				}
			}
			return new Collision.HurtTile
			{
				type = -1
			};
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00279D70 File Offset: 0x00277F70
		public static bool CanTileHurt(ushort type, int i, int j, Player player)
		{
			return (type != 230 || Main.getGoodWorld) && (type != 80 || Main.dontStarveWorld) && (TileID.Sets.TouchDamageBleeding[(int)type] || TileID.Sets.Suffocate[(int)type] || TileID.Sets.TouchDamageImmediate[(int)type] > 0 || (TileID.Sets.TouchDamageHot[(int)type] && (player == null || !player.fireWalk)));
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00279DD4 File Offset: 0x00277FD4
		public static bool SwitchTiles(Entity entity, Vector2 Position, int Width, int Height, Vector2 oldPosition, int objType)
		{
			int num = (int)(Position.X / 16f) - 1;
			int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
			int num3 = (int)(Position.Y / 16f) - 1;
			int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 > Main.maxTilesX)
			{
				num2 = Main.maxTilesX;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num4 > Main.maxTilesY - 40)
			{
				num4 = Main.maxTilesY - 40;
			}
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					if (Main.tile[i, j] != null)
					{
						int type = (int)Main.tile[i, j].type;
						if (Main.tile[i, j].active() && (type == 135 || type == 210 || type == 443 || type == 442))
						{
							Vector2 vector;
							vector.X = (float)(i * 16);
							vector.Y = (float)(j * 16 + 12);
							bool flag = false;
							if (type == 442)
							{
								if (objType == 4)
								{
									float r1StartX = 0f;
									float r1StartY = 0f;
									float r1Width = 0f;
									float r1Height = 0f;
									switch (Main.tile[i, j].frameX / 22)
									{
									case 0:
										r1StartX = (float)(i * 16);
										r1StartY = (float)(j * 16 + 16 - 10);
										r1Width = 16f;
										r1Height = 10f;
										break;
									case 1:
										r1StartX = (float)(i * 16);
										r1StartY = (float)(j * 16);
										r1Width = 16f;
										r1Height = 10f;
										break;
									case 2:
										r1StartX = (float)(i * 16);
										r1StartY = (float)(j * 16);
										r1Width = 10f;
										r1Height = 16f;
										break;
									case 3:
										r1StartX = (float)(i * 16 + 16 - 10);
										r1StartY = (float)(j * 16);
										r1Width = 10f;
										r1Height = 16f;
										break;
									}
									if (Utils.FloatIntersect(r1StartX, r1StartY, r1Width, r1Height, Position.X, Position.Y, (float)Width, (float)Height) && !Utils.FloatIntersect(r1StartX, r1StartY, r1Width, r1Height, oldPosition.X, oldPosition.Y, (float)Width, (float)Height))
									{
										Wiring.HitSwitch(i, j);
										NetMessage.SendData(59, -1, -1, null, i, (float)j, 0f, 0f, 0, 0, 0);
										return true;
									}
								}
								flag = true;
							}
							if (!flag && Position.X + (float)Width > vector.X && Position.X < vector.X + 16f && Position.Y + (float)Height > vector.Y && (double)Position.Y < (double)vector.Y + 4.01 && (oldPosition.X + (float)Width <= vector.X || oldPosition.X >= vector.X + 16f || oldPosition.Y + (float)Height <= vector.Y || (double)oldPosition.Y >= (double)vector.Y + 16.01))
							{
								if (type == 210)
								{
									Wiring.HitSwitch(i, j);
									NetMessage.SendData(59, -1, -1, null, i, (float)j, 0f, 0f, 0, 0, 0);
								}
								else if (type == 443)
								{
									if (objType == 1 || objType == 5)
									{
										Wiring.HitSwitch(i, j);
										NetMessage.SendData(59, -1, -1, null, i, (float)j, 0f, 0f, 0, 0, 0);
									}
								}
								else
								{
									int num5 = (int)(Main.tile[i, j].frameY / 18);
									bool flag2 = true;
									if ((num5 == 4 || num5 == 2 || num5 == 3 || num5 == 6 || num5 == 7) && objType != 5)
									{
										flag2 = false;
									}
									if (num5 == 5 && (objType == 1 || objType == 4 || objType == 5))
									{
										flag2 = false;
									}
									if (flag2)
									{
										if (Main.netMode == 1 && objType == 5)
										{
											NetMessage.SendData(13, -1, -1, null, Main.myPlayer, 0f, 0f, 0f, 0, 0, 0);
										}
										Wiring.HitSwitch(i, j);
										NetMessage.SendData(59, -1, -1, null, i, (float)j, 0f, 0f, 0, 0, 0);
										if (num5 == 7)
										{
											WorldGen.KillTile(i, j, false, false, false);
											if (Main.netMode == 1)
											{
												NetMessage.SendData(17, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
											}
										}
										return true;
									}
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0027A274 File Offset: 0x00278474
		public static Vector2 StickyTiles(Vector2 Position, Vector2 Velocity, int Width, int Height)
		{
			int num = (int)(Position.X / 16f) - 1;
			int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
			int num3 = (int)(Position.Y / 16f) - 1;
			int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 > Main.maxTilesX)
			{
				num2 = Main.maxTilesX;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num4 > Main.maxTilesY - 40)
			{
				num4 = Main.maxTilesY - 40;
			}
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					if (Main.tile[i, j] != null && Main.tile[i, j].active() && !Main.tile[i, j].inActive())
					{
						if (Main.tile[i, j].type == 51)
						{
							int num5 = 0;
							Vector2 vector;
							vector.X = (float)(i * 16);
							vector.Y = (float)(j * 16);
							if (Position.X + (float)Width > vector.X - (float)num5 && Position.X < vector.X + 16f + (float)num5 && Position.Y + (float)Height > vector.Y && (double)Position.Y < (double)vector.Y + 16.01)
							{
								if (Main.tile[i, j].type == 51 && (double)(Math.Abs(Velocity.X) + Math.Abs(Velocity.Y)) > 0.7 && Main.rand.Next(30) == 0)
								{
									Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 30, 0f, 0f, 0, default(Color), 1f);
								}
								return new Vector2((float)i, (float)j);
							}
						}
						else if (Main.tile[i, j].type == 229 && Main.tile[i, j].slope() == 0)
						{
							int num6 = 1;
							Vector2 vector;
							vector.X = (float)(i * 16);
							vector.Y = (float)(j * 16);
							float num7 = 16.01f;
							if (Main.tile[i, j].halfBrick())
							{
								vector.Y += 8f;
								num7 -= 8f;
							}
							if (Position.X + (float)Width > vector.X - (float)num6 && Position.X < vector.X + 16f + (float)num6 && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + num7)
							{
								if (Main.tile[i, j].type == 51 && (double)(Math.Abs(Velocity.X) + Math.Abs(Velocity.Y)) > 0.7 && Main.rand.Next(30) == 0)
								{
									Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 30, 0f, 0f, 0, default(Color), 1f);
								}
								return new Vector2((float)i, (float)j);
							}
						}
					}
				}
			}
			return new Vector2(-1f, -1f);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0027A611 File Offset: 0x00278811
		public static bool SolidTilesVersatile(int startX, int endX, int startY, int endY)
		{
			if (startX > endX)
			{
				Utils.Swap<int>(ref startX, ref endX);
			}
			if (startY > endY)
			{
				Utils.Swap<int>(ref startY, ref endY);
			}
			return Collision.SolidTiles(startX, endX, startY, endY);
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0027A638 File Offset: 0x00278838
		public static bool SolidTiles(Vector2 position, int width, int height)
		{
			return Collision.SolidTiles((int)(position.X / 16f), (int)((position.X + (float)width) / 16f), (int)(position.Y / 16f), (int)((position.Y + (float)height) / 16f));
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0027A684 File Offset: 0x00278884
		public static bool SolidTiles(int startX, int endX, int startY, int endY)
		{
			if (startX < 0)
			{
				return true;
			}
			if (endX >= Main.maxTilesX)
			{
				return true;
			}
			if (startY < 0)
			{
				return true;
			}
			if (endY >= Main.maxTilesY - 40)
			{
				return true;
			}
			for (int i = startX; i < endX + 1; i++)
			{
				for (int j = startY; j < endY + 1; j++)
				{
					if (Main.tile[i, j] == null)
					{
						return false;
					}
					if (Main.tile[i, j].active() && !Main.tile[i, j].inActive() && Main.tileSolid[(int)Main.tile[i, j].type] && !Main.tileSolidTop[(int)Main.tile[i, j].type])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0027A740 File Offset: 0x00278940
		public static bool SolidTiles(Vector2 position, int width, int height, bool allowTopSurfaces)
		{
			return Collision.SolidTiles((int)(position.X / 16f), (int)((position.X + (float)width) / 16f), (int)(position.Y / 16f), (int)((position.Y + (float)height) / 16f), allowTopSurfaces);
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0027A790 File Offset: 0x00278990
		public static bool SolidTiles(int startX, int endX, int startY, int endY, bool allowTopSurfaces)
		{
			if (startX < 0)
			{
				return true;
			}
			if (endX >= Main.maxTilesX)
			{
				return true;
			}
			if (startY < 0)
			{
				return true;
			}
			if (endY >= Main.maxTilesY - 40)
			{
				return true;
			}
			for (int i = startX; i < endX + 1; i++)
			{
				for (int j = startY; j < endY + 1; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile == null)
					{
						return false;
					}
					if (tile.active() && !Main.tile[i, j].inActive())
					{
						ushort type = tile.type;
						bool flag = Main.tileSolid[(int)type] && !Main.tileSolidTop[(int)type];
						if (allowTopSurfaces)
						{
							flag |= (Main.tileSolidTop[(int)type] && tile.frameY == 0);
						}
						if (flag)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0027A854 File Offset: 0x00278A54
		public static bool SolidFullTiles(Vector2 pos, Vector2 size)
		{
			int val = (int)(pos.X / 16f);
			int num = (int)(pos.Y / 16f);
			int num2 = (int)Math.Ceiling((double)((pos.X + size.X) / 16f));
			int num3 = (int)Math.Ceiling((double)((pos.Y + size.Y) / 16f));
			int num4 = Math.Max(val, 0);
			num = Math.Max(num, 0);
			num2 = Math.Min(num2, Main.maxTilesX - 1);
			num3 = Math.Min(num3, Main.maxTilesY - 1);
			for (int i = num4; i < num2; i++)
			{
				for (int j = num; j < num3; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile != null && tile.nactive() && tile.blockType() == 0 && Main.tileSolid[(int)tile.type] && !Main.tileSolidTop[(int)tile.type])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0027A940 File Offset: 0x00278B40
		public static void StepDown(ref Vector2 position, ref Vector2 velocity, int width, int height, ref float stepSpeed, ref float gfxOffY, int gravDir = 1, bool waterWalk = false)
		{
			Vector2 vector = position;
			vector.X += velocity.X;
			vector.Y = (float)Math.Floor((double)((vector.Y + (float)height) / 16f)) * 16f - (float)height;
			bool flag = false;
			int num = (int)(vector.X / 16f);
			int num2 = (int)((vector.X + (float)width) / 16f);
			int num3 = (int)((vector.Y + (float)height + 4f) / 16f);
			int num4 = height / 16 + ((height % 16 == 0) ? 0 : 1);
			float num5 = (float)((num3 + num4) * 16);
			float num6 = Main.bottomWorld / 16f - 42f;
			for (int i = num; i <= num2; i++)
			{
				for (int j = num3; j <= num3 + 1; j++)
				{
					if (WorldGen.InWorld(i, j, 1))
					{
						if (Main.tile[i, j] == null)
						{
							Main.tile[i, j] = new Tile();
						}
						if (Main.tile[i, j - 1] == null)
						{
							Main.tile[i, j - 1] = new Tile();
						}
						if (waterWalk && Main.tile[i, j].liquid > 0 && Main.tile[i, j - 1].liquid == 0)
						{
							int num7 = (int)(Main.tile[i, j].liquid / 32 * 2 + 2);
							int num8 = j * 16 + 16 - num7;
							Rectangle rectangle = new Rectangle(i * 16, j * 16 - 17, 16, 16);
							if (rectangle.Intersects(new Rectangle((int)position.X, (int)position.Y, width, height)) && (float)num8 < num5)
							{
								num5 = (float)num8;
							}
						}
						if ((float)j >= num6 || (Main.tile[i, j].nactive() && (Main.tileSolid[(int)Main.tile[i, j].type] || Main.tileSolidTop[(int)Main.tile[i, j].type])))
						{
							int num9 = j * 16;
							if (Main.tile[i, j].halfBrick())
							{
								num9 += 8;
							}
							if (Utils.FloatIntersect((float)(i * 16), (float)(j * 16 - 17), 16f, 16f, position.X, position.Y, (float)width, (float)height) && (float)num9 < num5)
							{
								num5 = (float)num9;
							}
						}
					}
				}
			}
			float num10 = num5 - (position.Y + (float)height);
			if (num10 > 7f && num10 < 17f && !flag)
			{
				stepSpeed = 1.5f;
				if (num10 > 9f)
				{
					stepSpeed = 2.5f;
				}
				gfxOffY += position.Y + (float)height - num5;
				position.Y = num5 - (float)height;
			}
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0027AC28 File Offset: 0x00278E28
		public static void StepUp(ref Vector2 position, ref Vector2 velocity, int width, int height, ref float stepSpeed, ref float gfxOffY, int gravDir = 1, bool holdsMatching = false, int specialChecksMode = 0)
		{
			int num = 0;
			if (velocity.X < 0f)
			{
				num = -1;
			}
			if (velocity.X > 0f)
			{
				num = 1;
			}
			Vector2 vector = position;
			vector.X += velocity.X;
			int num2 = (int)((vector.X + (float)(width / 2) + (float)((width / 2 + 1) * num)) / 16f);
			int num3 = (int)(((double)vector.Y + 0.1) / 16.0);
			if (gravDir == 1)
			{
				num3 = (int)((vector.Y + (float)height - 1f) / 16f);
			}
			int num4 = height / 16 + ((height % 16 == 0) ? 0 : 1);
			if (!WorldGen.InWorld(num2, num3, 1))
			{
				return;
			}
			if (num3 >= Main.maxTilesY - 40)
			{
				return;
			}
			bool flag = true;
			bool flag2 = true;
			if (Main.tile[num2, num3] == null)
			{
				return;
			}
			for (int i = 1; i < num4 + 2; i++)
			{
				if (!WorldGen.InWorld(num2, num3 - i * gravDir, 0) || Main.tile[num2, num3 - i * gravDir] == null)
				{
					return;
				}
			}
			if (!WorldGen.InWorld(num2 - num, num3 - num4 * gravDir, 0) || Main.tile[num2 - num, num3 - num4 * gravDir] == null)
			{
				return;
			}
			Tile tile;
			for (int j = 2; j < num4 + 1; j++)
			{
				if (!WorldGen.InWorld(num2, num3 - j * gravDir, 0))
				{
					return;
				}
				if (Main.tile[num2, num3 - j * gravDir] == null)
				{
					return;
				}
				tile = Main.tile[num2, num3 - j * gravDir];
				flag = (flag && (!tile.nactive() || !Main.tileSolid[(int)tile.type] || Main.tileSolidTop[(int)tile.type]));
			}
			tile = Main.tile[num2 - num, num3 - num4 * gravDir];
			flag2 = (flag2 && (!tile.nactive() || !Main.tileSolid[(int)tile.type] || Main.tileSolidTop[(int)tile.type]));
			bool flag3 = true;
			bool flag4 = true;
			bool flag5 = true;
			Tile tile2;
			if (gravDir == 1)
			{
				if (Main.tile[num2, num3 - gravDir] == null)
				{
					return;
				}
				if (Main.tile[num2, num3 - (num4 + 1) * gravDir] == null)
				{
					return;
				}
				tile = Main.tile[num2, num3 - gravDir];
				tile2 = Main.tile[num2, num3 - (num4 + 1) * gravDir];
				flag3 = (flag3 && (!tile.nactive() || !Main.tileSolid[(int)tile.type] || Main.tileSolidTop[(int)tile.type] || (tile.slope() == 1 && position.X + (float)(width / 2) > (float)(num2 * 16)) || (tile.slope() == 2 && position.X + (float)(width / 2) < (float)(num2 * 16 + 16)) || (tile.halfBrick() && (!tile2.nactive() || !Main.tileSolid[(int)tile2.type] || Main.tileSolidTop[(int)tile2.type]))));
				tile = Main.tile[num2, num3];
				tile2 = Main.tile[num2, num3 - 1];
				if (specialChecksMode == 1)
				{
					flag5 = !TileID.Sets.IgnoredByNpcStepUp[(int)tile.type];
				}
				flag4 = (flag4 && ((tile.nactive() && (!tile.topSlope() || (tile.slope() == 1 && position.X + (float)(width / 2) < (float)(num2 * 16)) || (tile.slope() == 2 && position.X + (float)(width / 2) > (float)(num2 * 16 + 16))) && (!tile.topSlope() || position.Y + (float)height > (float)(num3 * 16)) && ((Main.tileSolid[(int)tile.type] && !Main.tileSolidTop[(int)tile.type]) || (holdsMatching && ((Main.tileSolidTop[(int)tile.type] && tile.frameY == 0) || TileID.Sets.Platforms[(int)tile.type] || tile.type == 380) && (!Main.tileSolid[(int)tile2.type] || !tile2.nactive()) && flag5))) || (tile2.halfBrick() && tile2.nactive())));
				flag4 &= (!Main.tileSolidTop[(int)tile.type] || !Main.tileSolidTop[(int)tile2.type]);
			}
			else
			{
				tile = Main.tile[num2, num3 - gravDir];
				tile2 = Main.tile[num2, num3 - (num4 + 1) * gravDir];
				flag3 = (flag3 && (!tile.nactive() || !Main.tileSolid[(int)tile.type] || Main.tileSolidTop[(int)tile.type] || tile.slope() != 0 || (tile.halfBrick() && (!tile2.nactive() || !Main.tileSolid[(int)tile2.type] || Main.tileSolidTop[(int)tile2.type]))));
				tile = Main.tile[num2, num3];
				tile2 = Main.tile[num2, num3 + 1];
				flag4 = (flag4 && ((tile.nactive() && ((Main.tileSolid[(int)tile.type] && !Main.tileSolidTop[(int)tile.type]) || (holdsMatching && Main.tileSolidTop[(int)tile.type] && tile.frameY == 0 && (!Main.tileSolid[(int)tile2.type] || !tile2.nactive())))) || (tile2.halfBrick() && tile2.nactive())));
			}
			if ((float)(num2 * 16) < vector.X + (float)width && (float)(num2 * 16 + 16) > vector.X)
			{
				if (gravDir == 1)
				{
					if (flag4 && flag3 && flag && flag2)
					{
						float num5 = (float)(num3 * 16);
						if (Main.tile[num2, num3 - 1].halfBrick())
						{
							num5 -= 8f;
						}
						else if (Main.tile[num2, num3].halfBrick())
						{
							num5 += 8f;
						}
						if (num5 < vector.Y + (float)height)
						{
							float num6 = vector.Y + (float)height - num5;
							if ((double)num6 <= 16.1)
							{
								gfxOffY += position.Y + (float)height - num5;
								position.Y = num5 - (float)height;
								if (num6 < 9f)
								{
									stepSpeed = 1f;
									return;
								}
								stepSpeed = 2f;
								return;
							}
						}
					}
				}
				else if (flag4 && flag3 && flag && flag2 && !Main.tile[num2, num3].bottomSlope() && !TileID.Sets.Platforms[(int)tile2.type])
				{
					float num7 = (float)(num3 * 16 + 16);
					if (num7 > vector.Y)
					{
						float num8 = num7 - vector.Y;
						if ((double)num8 <= 16.1)
						{
							gfxOffY -= num7 - position.Y;
							position.Y = num7;
							velocity.Y = 0f;
							if (num8 < 9f)
							{
								stepSpeed = 1f;
								return;
							}
							stepSpeed = 2f;
						}
					}
				}
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0027B354 File Offset: 0x00279554
		public static bool InTileBounds(int x, int y, int lx, int ly, int hx, int hy)
		{
			return x >= lx && x <= hx && y >= ly && y <= hy;
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0027B36C File Offset: 0x0027956C
		public static float GetTileRotation(Vector2 position)
		{
			float num = position.Y % 16f;
			int num2 = (int)(position.X / 16f);
			int num3 = (int)(position.Y / 16f);
			Tile tile = Main.tile[num2, num3];
			bool flag = false;
			for (int i = 2; i >= 0; i--)
			{
				if (tile.active())
				{
					if (Main.tileSolid[(int)tile.type])
					{
						int num4 = tile.blockType();
						if (tile.type == 19)
						{
							int num5 = (int)(tile.frameX / 18);
							if (((num5 >= 0 && num5 <= 7) || (num5 >= 12 && num5 <= 16)) && (num == 0f || flag))
							{
								return 0f;
							}
							switch (num5)
							{
							case 8:
							case 19:
							case 21:
							case 23:
								return -0.7853982f;
							case 10:
							case 20:
							case 22:
							case 24:
								return 0.7853982f;
							case 25:
							case 26:
								if (flag)
								{
									return 0f;
								}
								if (num4 == 2)
								{
									return 0.7853982f;
								}
								if (num4 == 3)
								{
									return -0.7853982f;
								}
								break;
							}
						}
						else
						{
							if (num4 == 1)
							{
								return 0f;
							}
							if (num4 == 2)
							{
								return 0.7853982f;
							}
							if (num4 == 3)
							{
								return -0.7853982f;
							}
							return 0f;
						}
					}
					else if (Main.tileSolidTop[(int)tile.type] && tile.frameY == 0 && flag)
					{
						return 0f;
					}
				}
				num3++;
				tile = Main.tile[num2, num3];
				flag = true;
			}
			return 0f;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0027B518 File Offset: 0x00279718
		public static void GetEntityEdgeTiles(List<Point> p, Entity entity, bool left = true, bool right = true, bool up = true, bool down = true)
		{
			int num = (int)entity.position.X;
			int num2 = (int)entity.position.Y;
			int num3 = num % 16;
			int num4 = num2 % 16;
			int num5 = (int)entity.Right.X;
			int num6 = (int)entity.Bottom.Y;
			if (num % 16 == 0)
			{
				num--;
			}
			if (num2 % 16 == 0)
			{
				num2--;
			}
			if (num5 % 16 == 0)
			{
				num5++;
			}
			if (num6 % 16 == 0)
			{
				num6++;
			}
			int num7 = num5 / 16 - num / 16;
			int num8 = num6 / 16 - num2 / 16;
			num /= 16;
			num2 /= 16;
			for (int i = num; i <= num + num7; i++)
			{
				if (up)
				{
					p.Add(new Point(i, num2));
				}
				if (down)
				{
					p.Add(new Point(i, num2 + num8));
				}
			}
			for (int j = num2; j < num2 + num8; j++)
			{
				if (left)
				{
					p.Add(new Point(num, j));
				}
				if (right)
				{
					p.Add(new Point(num + num7, j));
				}
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0027B620 File Offset: 0x00279820
		public static bool ApplyConveyorBeltMovementToVelocity(WorldItem item, ref Vector2 velocity)
		{
			bool flag = false;
			Collision.BuildTileContacts(item.position, item.width, item.height, Collision.contacts);
			if (Collision.contacts.Count > 0)
			{
				int num = -1;
				int num2 = -1;
				bool flag2 = false;
				Collision.TileContactSide tileContactSide = Collision.TileContactSide.Top;
				Collision.TileContactSide tileContactSide2 = Collision.TileContactSide.Top;
				Vector2 zero = Vector2.Zero;
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				for (int i = 0; i < Collision.contacts.Count; i++)
				{
					int num3 = TileID.Sets.ConveyorDirection[Collision.contacts[i].Type];
					switch (Collision.contacts[i].Side)
					{
					case Collision.TileContactSide.Left:
						zero.Y += (float)(num3 * Collision.contacts[i].Overlap);
						flag3 |= (num3 != 0);
						break;
					case Collision.TileContactSide.Right:
						zero.Y += (float)(-(float)num3 * Collision.contacts[i].Overlap);
						flag3 |= (num3 != 0);
						break;
					case Collision.TileContactSide.Top:
						zero.X += (float)(-(float)num3 * Collision.contacts[i].Overlap);
						flag4 = (num3 != 0);
						break;
					case Collision.TileContactSide.Bottom:
						flag5 = true;
						zero.X += (float)(num3 * Collision.contacts[i].Overlap);
						flag4 = (num3 != 0);
						if (Collision.contacts[i].Slope == 1)
						{
							if ((float)(Collision.contacts[i].X * 16) < item.position.X)
							{
								Tile tile = Main.tile[Collision.contacts[i].X, Collision.contacts[i].Y - 1];
								if (tile != null && tile.active() && tile.slope() == 3)
								{
									num2 = i;
								}
							}
						}
						else if (Collision.contacts[i].Slope == 2 && (float)(Collision.contacts[i].X * 16 + 16) > item.Right.X)
						{
							Tile tile2 = Main.tile[Collision.contacts[i].X, Collision.contacts[i].Y - 1];
							if (tile2 != null && tile2.active() && tile2.slope() == 4)
							{
								num = i;
							}
						}
						break;
					case Collision.TileContactSide.BottomLeft:
						if (num3 == -1)
						{
							int x = Collision.contacts[i].X;
							int y = Collision.contacts[i].Y;
							Tile tile3 = Main.tile[x, y - 1];
							byte b = tile3.slope();
							if (!tile3.active() || (b == 1 && TileID.Sets.ConveyorDirection[(int)tile3.type] == -1))
							{
								tileContactSide = Collision.TileContactSide.BottomLeft;
								flag2 = Main.tile[x, y].halfBrick();
							}
						}
						if (num3 == 1)
						{
							tileContactSide2 = Collision.TileContactSide.BottomLeft;
						}
						break;
					case Collision.TileContactSide.BottomRight:
						if (num3 == 1)
						{
							int x2 = Collision.contacts[i].X;
							int y2 = Collision.contacts[i].Y;
							Tile tile4 = Main.tile[x2, y2 - 1];
							byte b2 = tile4.slope();
							if (!tile4.active() || (b2 == 2 && TileID.Sets.ConveyorDirection[(int)tile4.type] == 1))
							{
								tileContactSide = Collision.TileContactSide.BottomRight;
								flag2 = Main.tile[x2, y2].halfBrick();
							}
						}
						if (num3 == -1)
						{
							tileContactSide2 = Collision.TileContactSide.BottomRight;
						}
						break;
					}
				}
				if (zero.X < 0f)
				{
					if (zero.X < -8f)
					{
						zero.X = -2.5f;
					}
					else if (zero.X < -4f)
					{
						zero.X = -1.25f;
					}
					else
					{
						zero.X = -0.75f;
					}
				}
				else if (zero.X > 0f)
				{
					if (zero.X > 8f)
					{
						zero.X = 2.5f;
					}
					else if (zero.X > 4f)
					{
						zero.X = 1.25f;
					}
					else
					{
						zero.X = 0.75f;
					}
				}
				if (zero.Y < 0f)
				{
					if (zero.Y < 8f)
					{
						zero.Y = -2.5f;
					}
					else
					{
						zero.Y = -1.25f;
					}
				}
				else if (zero.Y > 0f)
				{
					if (zero.Y > 8f)
					{
						zero.Y = 2.5f;
					}
					else
					{
						zero.Y = 1.25f;
					}
				}
				else if (flag3 && velocity.Y <= 1f)
				{
					velocity.Y = 0f;
				}
				if (zero.Y == 0f)
				{
					if (velocity.Y < 0.11f && tileContactSide != Collision.TileContactSide.Top && !flag5)
					{
						if (tileContactSide == Collision.TileContactSide.BottomLeft)
						{
							flag = true;
							Vector2 bottom = item.Bottom;
							int num4 = (int)bottom.Y;
							num4 = (num4 + 8) / 16;
							num4 *= 16;
							if (flag2)
							{
								num4 += 8;
							}
							bottom.Y = (float)num4;
							item.Bottom = bottom;
							velocity.Y = 0f;
							velocity.X = -1.25f;
							zero.X = -2.5f;
						}
						if (tileContactSide == Collision.TileContactSide.BottomRight)
						{
							flag = true;
							Vector2 bottom2 = item.Bottom;
							int num5 = (int)bottom2.Y;
							num5 = (num5 + 8) / 16;
							num5 *= 16;
							if (flag2)
							{
								num5 += 8;
							}
							bottom2.Y = (float)num5;
							item.Bottom = bottom2;
							velocity.Y = 0f;
							velocity.X = 1.25f;
							zero.X = 2.5f;
						}
					}
					else if ((double)velocity.Y < 0.3 && !flag5)
					{
						if (tileContactSide2 == Collision.TileContactSide.BottomRight)
						{
							if (velocity.X <= 0.75f && velocity.X >= -2.5f && !flag3)
							{
								flag = true;
								int num6 = (int)item.position.X;
								num6 = (num6 + 8) / 16;
								num6 *= 16;
								item.position.X = (float)num6;
								velocity.X = 0f;
								velocity.Y = 0.75f;
								zero.Y = 2.5f;
							}
						}
						else if (tileContactSide2 == Collision.TileContactSide.BottomLeft && velocity.X >= -0.75f && velocity.X <= 2.5f && !flag3)
						{
							flag = true;
							Vector2 right = item.Right;
							int num7 = (int)right.X;
							num7 = (num7 + 8) / 16;
							num7 *= 16;
							right.X = (float)num7;
							item.Right = right;
							velocity.X = 0f;
							velocity.Y = 0.75f;
							zero.Y = 2.5f;
						}
					}
				}
				flag |= (flag4 || flag3);
				if (zero.Y < 0f)
				{
					velocity.Y = Math.Max(velocity.Y + zero.Y * 6f / 60f, zero.Y);
				}
				else if (zero.Y > 0f)
				{
					velocity.Y = Math.Min(velocity.Y + zero.Y * 6f / 60f, zero.Y);
				}
				if (zero.X < 0f && velocity.X > zero.X)
				{
					velocity.X = Math.Max(velocity.X + zero.X * 6f / 60f, zero.X);
				}
				if (zero.X > 0f && velocity.X < zero.X)
				{
					velocity.X = Math.Min(velocity.X + zero.X * 6f / 60f, zero.X);
				}
				if (num != -1 && velocity.X > 0f)
				{
					int num8 = Collision.contacts[num].X * 16 + Math.Max(16 - item.height / 2, 0);
					Vector2 right2 = item.Right;
					if (right2.X + velocity.X > (float)num8)
					{
						if (right2.X >= (float)num8)
						{
							right2.X = (float)num8;
							velocity.X = 0f;
							item.Right = right2;
						}
						else
						{
							velocity.X = (float)num8 - right2.X;
						}
					}
				}
				if (num2 != -1 && velocity.X < 0f)
				{
					int num9 = Collision.contacts[num2].X * 16 + Math.Min(item.height / 2, 16);
					Vector2 position = item.position;
					if (position.X + velocity.X < (float)num9)
					{
						if (position.X <= (float)num9)
						{
							position.X = (float)num9;
							velocity.X = 0f;
							item.position = position;
						}
						else
						{
							velocity.X = (float)num9 - position.X;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0027BF50 File Offset: 0x0027A150
		public static void StepConveyorBelt(Entity entity, float gravDir, bool artificialRising = false)
		{
			Player player = null;
			if (entity is Player)
			{
				player = (Player)entity;
				if (Math.Abs(player.gfxOffY) > 2f || player.grapCount > 0 || player.pulley)
				{
					return;
				}
				entity.height -= 5;
				entity.position.Y = entity.position.Y + 5f;
			}
			int num = 0;
			int num2 = 0;
			bool flag = false;
			int num3 = (int)entity.position.Y + entity.height;
			entity.Hitbox.Inflate(2, 2);
			Vector2 topLeft = entity.TopLeft;
			Vector2 topRight = entity.TopRight;
			Vector2 bottomLeft = entity.BottomLeft;
			Vector2 bottomRight = entity.BottomRight;
			List<Point> cacheForConveyorBelts = Collision._cacheForConveyorBelts;
			cacheForConveyorBelts.Clear();
			Collision.GetEntityEdgeTiles(cacheForConveyorBelts, entity, false, false, true, true);
			Vector2 vector = new Vector2(0.0001f);
			for (int i = 0; i < cacheForConveyorBelts.Count; i++)
			{
				Point point = cacheForConveyorBelts[i];
				if (WorldGen.InWorld(point.X, point.Y, 0) && (player == null || !player.onTrack || point.Y >= num3))
				{
					Tile tile = Main.tile[point.X, point.Y];
					if (tile != null && tile.active() && tile.nactive())
					{
						int num4 = TileID.Sets.ConveyorDirection[(int)tile.type];
						if (num4 != 0)
						{
							Vector2 lineStart;
							Vector2 lineStart2;
							lineStart.X = (lineStart2.X = (float)(point.X * 16));
							Vector2 lineEnd;
							Vector2 lineEnd2;
							lineEnd.X = (lineEnd2.X = (float)(point.X * 16 + 16));
							switch (tile.slope())
							{
							case 1:
								lineStart2.Y = (float)(point.Y * 16);
								lineEnd2.Y = (lineEnd.Y = (lineStart.Y = (float)(point.Y * 16 + 16)));
								break;
							case 2:
								lineEnd2.Y = (float)(point.Y * 16);
								lineStart2.Y = (lineEnd.Y = (lineStart.Y = (float)(point.Y * 16 + 16)));
								break;
							case 3:
								lineEnd.Y = (lineStart2.Y = (lineEnd2.Y = (float)(point.Y * 16)));
								lineStart.Y = (float)(point.Y * 16 + 16);
								break;
							case 4:
								lineStart.Y = (lineStart2.Y = (lineEnd2.Y = (float)(point.Y * 16)));
								lineEnd.Y = (float)(point.Y * 16 + 16);
								break;
							default:
								if (tile.halfBrick())
								{
									lineStart2.Y = (lineEnd2.Y = (float)(point.Y * 16 + 8));
								}
								else
								{
									lineStart2.Y = (lineEnd2.Y = (float)(point.Y * 16));
								}
								lineStart.Y = (lineEnd.Y = (float)(point.Y * 16 + 16));
								break;
							}
							int num5 = 0;
							if (!TileID.Sets.Platforms[(int)tile.type] && Collision.CheckAABBvLineCollision2(entity.position - vector, entity.Size + vector * 2f, lineStart, lineEnd))
							{
								num5--;
							}
							if (Collision.CheckAABBvLineCollision2(entity.position - vector, entity.Size + vector * 2f, lineStart2, lineEnd2))
							{
								num5++;
							}
							if (num5 != 0)
							{
								flag = true;
								num += num4 * num5 * (int)gravDir;
								if (tile.leftSlope())
								{
									num2 += (int)gravDir * -num4;
								}
								if (tile.rightSlope())
								{
									num2 -= (int)gravDir * -num4;
								}
							}
						}
					}
				}
			}
			if (entity is Player)
			{
				entity.height += 5;
				entity.position.Y = entity.position.Y - 5f;
			}
			if (!flag)
			{
				return;
			}
			if (artificialRising)
			{
				num2 = -1;
			}
			if (num != 0 || artificialRising)
			{
				num = Math.Sign(num);
				num2 = Math.Sign(num2);
				Vector2 vector2 = Vector2.Normalize(new Vector2((float)num * gravDir, (float)num2)) * 2.5f;
				Vector2 value = Collision.TileCollision(entity.position, vector2, entity.width, entity.height, false, false, (int)gravDir, false, false, true);
				entity.position += value;
				if (!artificialRising)
				{
					vector2 = new Vector2(0f, 2.5f * gravDir);
					value = Collision.TileCollision(entity.position, vector2, entity.width, entity.height, false, false, (int)gravDir, false, false, true);
					entity.position += value;
				}
				if (artificialRising)
				{
					vector2 = new Vector2((float)num, (float)num2);
					value = Collision.TileCollision(entity.position - vector2, vector2, entity.width, entity.height, false, false, (int)gravDir, false, false, true);
					entity.position += value;
				}
			}
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0027C48C File Offset: 0x0027A68C
		public static bool TryFindingConveyorBeltRising(Entity entity, float gravDir)
		{
			if (entity is WorldItem)
			{
				Point point = (entity.BottomLeft + new Vector2(-0.5f, -0.01f)).ToTileCoordinates();
				float num = 0f;
				if (WorldGen.InWorld(point, 0))
				{
					Tile tile = Main.tile[point.X, point.Y];
					if (tile != null && tile.active() && tile.nactive() && tile.slope() == 0)
					{
						int num2 = TileID.Sets.ConveyorDirection[(int)tile.type];
						num += (float)num2 * gravDir;
					}
				}
				Point point2 = (entity.BottomRight + new Vector2(0.5f, -0.01f)).ToTileCoordinates();
				if (WorldGen.InWorld(point2, 0))
				{
					Tile tile2 = Main.tile[point2.X, point2.Y];
					if (tile2 != null && tile2.active() && tile2.nactive() && tile2.slope() == 0)
					{
						int num3 = TileID.Sets.ConveyorDirection[(int)tile2.type];
						num += (float)num3 * gravDir * -1f;
					}
				}
				return num < 0f;
			}
			return false;
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0027C5A4 File Offset: 0x0027A7A4
		public static List<Point> GetTilesIn(Vector2 TopLeft, Vector2 BottomRight)
		{
			List<Point> list = new List<Point>();
			Point point = TopLeft.ToTileCoordinates();
			Point point2 = BottomRight.ToTileCoordinates();
			int num = Utils.Clamp<int>(point.X, 0, Main.maxTilesX - 1);
			int num2 = Utils.Clamp<int>(point.Y, 0, Main.maxTilesY - 40);
			int num3 = Utils.Clamp<int>(point2.X, 0, Main.maxTilesX - 1);
			int num4 = Utils.Clamp<int>(point2.Y, 0, Main.maxTilesY - 40);
			for (int i = num; i <= num3; i++)
			{
				for (int j = num2; j <= num4; j++)
				{
					if (Main.tile[i, j] != null)
					{
						list.Add(new Point(i, j));
					}
				}
			}
			return list;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0027C65C File Offset: 0x0027A85C
		public static void ExpandVertically(int startX, int startY, out int topY, out int bottomY, int maxExpandUp = 100, int maxExpandDown = 100)
		{
			topY = startY;
			bottomY = startY;
			if (!WorldGen.InWorld(startX, startY, 10))
			{
				return;
			}
			int num = 0;
			while (num < maxExpandUp && topY > 0 && topY >= 10 && Main.tile[startX, topY] != null && !WorldGen.SolidTile3(startX, topY))
			{
				topY--;
				num++;
			}
			int num2 = 0;
			while (num2 < maxExpandDown && bottomY < Main.maxTilesY - 10 && bottomY <= Main.maxTilesY - 10 && Main.tile[startX, bottomY] != null && !WorldGen.SolidTile3(startX, bottomY))
			{
				bottomY++;
				num2++;
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0027C6F8 File Offset: 0x0027A8F8
		public static Vector2 AdvancedTileCollision(bool[] forcedIgnoredTiles, Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1)
		{
			Collision.up = false;
			Collision.down = false;
			Vector2 result = Velocity;
			Vector2 vector = Position + Velocity;
			int value = (int)(Position.X / 16f) - 1;
			int num = (int)((Position.X + (float)Width) / 16f) + 2;
			int num2 = (int)(Position.Y / 16f) - 1;
			int num3 = (int)((Position.Y + (float)Height) / 16f) + 2;
			int num4 = -1;
			int num5 = -1;
			int num6 = -1;
			int num7 = -1;
			int num8 = Utils.Clamp<int>(value, 0, Main.maxTilesX - 1);
			num = Utils.Clamp<int>(num, 0, Main.maxTilesX - 1);
			num2 = Utils.Clamp<int>(num2, 0, Main.maxTilesY - 40);
			num3 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 40);
			float num9 = (float)((num3 + 3) * 16);
			for (int i = num8; i < num; i++)
			{
				for (int j = num2; j < num3; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile != null && tile.active() && !tile.inActive() && !forcedIgnoredTiles[(int)tile.type] && (Main.tileSolid[(int)tile.type] || (Main.tileSolidTop[(int)tile.type] && tile.frameY == 0)))
					{
						Vector2 vector2;
						vector2.X = (float)(i * 16);
						vector2.Y = (float)(j * 16);
						int num10 = 16;
						if (tile.halfBrick())
						{
							vector2.Y += 8f;
							num10 -= 8;
						}
						if (vector.X + (float)Width > vector2.X && vector.X < vector2.X + 16f && vector.Y + (float)Height > vector2.Y && vector.Y < vector2.Y + (float)num10)
						{
							bool flag = false;
							bool flag2 = false;
							if (tile.slope() > 2)
							{
								if (tile.slope() == 3 && Position.Y + Math.Abs(Velocity.X) >= vector2.Y && Position.X >= vector2.X)
								{
									flag2 = true;
								}
								if (tile.slope() == 4 && Position.Y + Math.Abs(Velocity.X) >= vector2.Y && Position.X + (float)Width <= vector2.X + 16f)
								{
									flag2 = true;
								}
							}
							else if (tile.slope() > 0)
							{
								flag = true;
								if (tile.slope() == 1 && Position.Y + (float)Height - Math.Abs(Velocity.X) <= vector2.Y + (float)num10 && Position.X >= vector2.X)
								{
									flag2 = true;
								}
								if (tile.slope() == 2 && Position.Y + (float)Height - Math.Abs(Velocity.X) <= vector2.Y + (float)num10 && Position.X + (float)Width <= vector2.X + 16f)
								{
									flag2 = true;
								}
							}
							if (!flag2)
							{
								if (Position.Y + (float)Height <= vector2.Y)
								{
									Collision.down = true;
									if ((!Main.tileSolidTop[(int)tile.type] || !fallThrough || (Velocity.Y > 1f && !fall2)) && num9 > vector2.Y)
									{
										num6 = i;
										num7 = j;
										if (num10 < 16)
										{
											num7++;
										}
										if (num6 != num4 && !flag)
										{
											result.Y = vector2.Y - (Position.Y + (float)Height) + ((gravDir == -1) ? -0.01f : 0f);
											num9 = vector2.Y;
										}
									}
								}
								else if (Position.X + (float)Width <= vector2.X && !Main.tileSolidTop[(int)tile.type])
								{
									if (Main.tile[i - 1, j] == null)
									{
										Main.tile[i - 1, j] = new Tile();
									}
									if (Main.tile[i - 1, j].slope() != 2 && Main.tile[i - 1, j].slope() != 4)
									{
										num4 = i;
										num5 = j;
										if (num5 != num7)
										{
											result.X = vector2.X - (Position.X + (float)Width);
										}
										if (num6 == num4)
										{
											result.Y = Velocity.Y;
										}
									}
								}
								else if (Position.X >= vector2.X + 16f && !Main.tileSolidTop[(int)tile.type])
								{
									if (Main.tile[i + 1, j] == null)
									{
										Main.tile[i + 1, j] = new Tile();
									}
									if (Main.tile[i + 1, j].slope() != 1 && Main.tile[i + 1, j].slope() != 3)
									{
										num4 = i;
										num5 = j;
										if (num5 != num7)
										{
											result.X = vector2.X + 16f - Position.X;
										}
										if (num6 == num4)
										{
											result.Y = Velocity.Y;
										}
									}
								}
								else if (Position.Y >= vector2.Y + (float)num10 && !Main.tileSolidTop[(int)tile.type])
								{
									Collision.up = true;
									num6 = i;
									num7 = j;
									result.Y = vector2.Y + (float)num10 - Position.Y + ((gravDir == 1) ? 0.01f : 0f);
									if (num7 == num5)
									{
										result.X = Velocity.X;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0027CCCC File Offset: 0x0027AECC
		public static void LaserScan(Vector2 samplingPoint, Vector2 directionUnit, float samplingWidth, float maxDistance, float[] samples)
		{
			for (int i = 0; i < samples.Length; i++)
			{
				float num = (float)i / (float)(samples.Length - 1);
				Vector2 vector = samplingPoint + directionUnit.RotatedBy(1.5707963705062866, default(Vector2)) * (num - 0.5f) * samplingWidth;
				int num2 = (int)vector.X / 16;
				int num3 = (int)vector.Y / 16;
				Vector2 vector2 = vector + directionUnit * maxDistance;
				int num4 = (int)vector2.X / 16;
				int num5 = (int)vector2.Y / 16;
				Point point;
				float num6;
				if (!Collision.HitLine(num2, num3, num4, num5, 0, 0, new List<Point>(), out point))
				{
					num6 = new Vector2((float)Math.Abs(num2 - point.X), (float)Math.Abs(num3 - point.Y)).Length() * 16f;
				}
				else if (point.X == num4 && point.Y == num5)
				{
					num6 = maxDistance;
				}
				else
				{
					num6 = new Vector2((float)Math.Abs(num2 - point.X), (float)Math.Abs(num3 - point.Y)).Length() * 16f;
				}
				samples[i] = num6;
			}
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0027CE0B File Offset: 0x0027B00B
		public static void AimingLaserScan(Vector2 startPoint, Vector2 endPoint, float samplingWidth, int samplesToTake, out Vector2 vectorTowardsTarget, out float[] samples)
		{
			samples = new float[samplesToTake];
			vectorTowardsTarget = endPoint - startPoint;
			Collision.LaserScan(startPoint, vectorTowardsTarget.SafeNormalize(Vector2.Zero), samplingWidth, vectorTowardsTarget.Length(), samples);
		}

		// Token: 0x04000449 RID: 1097
		public static bool stair;

		// Token: 0x0400044A RID: 1098
		public static bool stairFall;

		// Token: 0x0400044B RID: 1099
		public static bool honey;

		// Token: 0x0400044C RID: 1100
		public static bool shimmer;

		// Token: 0x0400044D RID: 1101
		public static bool sloping;

		// Token: 0x0400044E RID: 1102
		public static bool landMine = false;

		// Token: 0x0400044F RID: 1103
		public static bool up;

		// Token: 0x04000450 RID: 1104
		public static bool down;

		// Token: 0x04000451 RID: 1105
		public static float Epsilon = 2.7182817f;

		// Token: 0x04000452 RID: 1106
		private const int bottomFluff = 40;

		// Token: 0x04000453 RID: 1107
		private static List<Collision.TileContact> contacts = new List<Collision.TileContact>();

		// Token: 0x04000454 RID: 1108
		private static List<Point> _cacheForConveyorBelts = new List<Point>();

		// Token: 0x02000612 RID: 1554
		public enum TileContactSide
		{
			// Token: 0x04006457 RID: 25687
			Left,
			// Token: 0x04006458 RID: 25688
			Right,
			// Token: 0x04006459 RID: 25689
			Top,
			// Token: 0x0400645A RID: 25690
			Bottom,
			// Token: 0x0400645B RID: 25691
			BottomLeft,
			// Token: 0x0400645C RID: 25692
			BottomRight
		}

		// Token: 0x02000613 RID: 1555
		public struct TileContact
		{
			// Token: 0x06003C0A RID: 15370 RVA: 0x0066BD9F File Offset: 0x00669F9F
			public TileContact(Collision.TileContactSide side, int x, int y, int type, int slope, int overlap)
			{
				this.Side = side;
				this.X = x;
				this.Y = y;
				this.Slope = slope;
				this.Type = type;
				this.Overlap = overlap;
			}

			// Token: 0x0400645D RID: 25693
			public Collision.TileContactSide Side;

			// Token: 0x0400645E RID: 25694
			public int Overlap;

			// Token: 0x0400645F RID: 25695
			public int X;

			// Token: 0x04006460 RID: 25696
			public int Y;

			// Token: 0x04006461 RID: 25697
			public int Slope;

			// Token: 0x04006462 RID: 25698
			public int Type;
		}

		// Token: 0x02000614 RID: 1556
		public struct HurtTile
		{
			// Token: 0x04006463 RID: 25699
			public int type;

			// Token: 0x04006464 RID: 25700
			public int x;

			// Token: 0x04006465 RID: 25701
			public int y;
		}
	}
}
