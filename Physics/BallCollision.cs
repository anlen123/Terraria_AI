using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.Physics
{
	// Token: 0x02000076 RID: 118
	public static class BallCollision
	{
		// Token: 0x06001547 RID: 5447 RVA: 0x004C30CC File Offset: 0x004C12CC
		public static BallStepResult Step(PhysicsProperties physicsProperties, Entity entity, ref float entityAngularVelocity, IBallContactListener listener)
		{
			Vector2 vector = entity.position;
			Vector2 vector2 = entity.velocity;
			Vector2 size = entity.Size;
			float num = entityAngularVelocity;
			float num2 = size.X * 0.5f;
			num *= physicsProperties.Drag;
			vector2 *= physicsProperties.Drag;
			float num3 = vector2.Length();
			if (num3 > 1000f)
			{
				vector2 = 1000f * Vector2.Normalize(vector2);
				num3 = 1000f;
			}
			int num4 = Math.Max(1, (int)Math.Ceiling((double)(num3 / 2f)));
			float num5 = 1f / (float)num4;
			vector2 *= num5;
			num *= num5;
			float num6 = physicsProperties.Gravity / (float)(num4 * num4);
			bool flag = false;
			for (int i = 0; i < num4; i++)
			{
				vector2.Y += num6;
				BallPassThroughType ballPassThroughType;
				Tile tile;
				if (BallCollision.CheckForPassThrough(vector + size * 0.5f, out ballPassThroughType, out tile))
				{
					if (ballPassThroughType == BallPassThroughType.Tile && Main.tileSolid[(int)tile.type] && !Main.tileSolidTop[(int)tile.type])
					{
						vector2 *= 0f;
						num *= 0f;
						flag = true;
					}
					else
					{
						BallPassThroughEvent ballPassThroughEvent = new BallPassThroughEvent(num5, tile, entity, ballPassThroughType);
						listener.OnPassThrough(physicsProperties, ref vector, ref vector2, ref num, ref ballPassThroughEvent);
					}
				}
				vector += vector2;
				if (!BallCollision.IsBallInWorld(vector, size))
				{
					return BallStepResult.OutOfBounds();
				}
				Vector2 vector3;
				if (BallCollision.GetClosestEdgeToCircle(vector, size, vector2, out vector3, out tile))
				{
					Vector2 vector4 = Vector2.Normalize(vector + size * 0.5f - vector3);
					vector = vector3 + vector4 * (num2 + 0.0001f) - size * 0.5f;
					BallCollisionEvent ballCollisionEvent = new BallCollisionEvent(num5, vector4, vector3, tile, entity);
					flag = true;
					vector2 = Vector2.Reflect(vector2, ballCollisionEvent.Normal);
					listener.OnCollision(physicsProperties, ref vector, ref vector2, ref ballCollisionEvent);
					num = (ballCollisionEvent.Normal.X * vector2.Y - ballCollisionEvent.Normal.Y * vector2.X) / num2;
				}
			}
			vector2 /= num5;
			num /= num5;
			BallStepResult result = BallStepResult.Moving();
			if (flag && vector2.X > -0.01f && vector2.X < 0.01f && vector2.Y <= 0f && vector2.Y > -physicsProperties.Gravity)
			{
				result = BallStepResult.Resting();
			}
			entity.position = vector;
			entity.velocity = vector2;
			entityAngularVelocity = num;
			return result;
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x004C334C File Offset: 0x004C154C
		private static bool CheckForPassThrough(Vector2 center, out BallPassThroughType type, out Tile contactTile)
		{
			Point point = center.ToTileCoordinates();
			Tile tile = Main.tile[point.X, point.Y];
			contactTile = tile;
			type = BallPassThroughType.None;
			if (tile == null)
			{
				return false;
			}
			if (tile.nactive())
			{
				type = BallPassThroughType.Tile;
				return BallCollision.IsPositionInsideTile(center, point, tile);
			}
			if (tile.liquid > 0)
			{
				float num = (float)(point.Y + 1) * 16f - (float)tile.liquid / 255f * 16f;
				byte b = tile.liquidType();
				if (b != 1)
				{
					if (b != 2)
					{
						type = BallPassThroughType.Water;
					}
					else
					{
						type = BallPassThroughType.Honey;
					}
				}
				else
				{
					type = BallPassThroughType.Lava;
				}
				return num < center.Y;
			}
			return false;
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x004C33EC File Offset: 0x004C15EC
		private static bool IsPositionInsideTile(Vector2 position, Point tileCoordinates, Tile tile)
		{
			if (tile.slope() == 0 && !tile.halfBrick())
			{
				return true;
			}
			Vector2 vector = position / 16f - new Vector2((float)tileCoordinates.X, (float)tileCoordinates.Y);
			switch (tile.slope())
			{
			case 0:
				return vector.Y > 0.5f;
			case 1:
				return vector.Y > vector.X;
			case 2:
				return vector.Y > 1f - vector.X;
			case 3:
				return vector.Y < 1f - vector.X;
			case 4:
				return vector.Y < vector.X;
			default:
				return false;
			}
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x004C34AC File Offset: 0x004C16AC
		private static bool IsBallInWorld(Vector2 position, Vector2 size)
		{
			return position.X > 32f && position.Y > 32f && position.X + size.X < (float)Main.maxTilesX * 16f - 32f && position.Y + size.Y < (float)Main.maxTilesY * 16f - 32f;
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x004C3518 File Offset: 0x004C1718
		private static bool GetClosestEdgeToCircle(Vector2 position, Vector2 size, Vector2 velocity, out Vector2 collisionPoint, out Tile collisionTile)
		{
			Rectangle tileBounds = BallCollision.GetTileBounds(position, size);
			Vector2 vector = position + size * 0.5f;
			BallCollision.TileEdges tileEdges = BallCollision.TileEdges.None;
			if (velocity.Y < 0f)
			{
				tileEdges |= BallCollision.TileEdges.Bottom;
			}
			else
			{
				tileEdges |= BallCollision.TileEdges.Top;
			}
			if (velocity.X < 0f)
			{
				tileEdges |= BallCollision.TileEdges.Right;
			}
			else
			{
				tileEdges |= BallCollision.TileEdges.Left;
			}
			if (velocity.Y > velocity.X)
			{
				tileEdges |= BallCollision.TileEdges.BottomLeftSlope;
			}
			else
			{
				tileEdges |= BallCollision.TileEdges.TopRightSlope;
			}
			if (velocity.Y > -velocity.X)
			{
				tileEdges |= BallCollision.TileEdges.BottomRightSlope;
			}
			else
			{
				tileEdges |= BallCollision.TileEdges.TopLeftSlope;
			}
			collisionPoint = Vector2.Zero;
			collisionTile = null;
			float num = float.MaxValue;
			Vector2 vector2 = default(Vector2);
			float num2 = 0f;
			for (int i = tileBounds.Left; i < tileBounds.Right; i++)
			{
				for (int j = tileBounds.Top; j < tileBounds.Bottom; j++)
				{
					if (BallCollision.GetCollisionPointForTile(tileEdges, i, j, vector, ref vector2, ref num2) && num2 < num && Vector2.Dot(velocity, vector - vector2) <= 0f)
					{
						num = num2;
						collisionPoint = vector2;
						collisionTile = Main.tile[i, j];
					}
				}
			}
			float num3 = size.X / 2f;
			return num < num3 * num3;
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x004C3660 File Offset: 0x004C1860
		private static bool GetCollisionPointForTile(BallCollision.TileEdges edgesToTest, int x, int y, Vector2 center, ref Vector2 closestPointOut, ref float distanceSquaredOut)
		{
			Tile tile = Main.tile[x, y];
			if (tile == null || !tile.nactive() || (!Main.tileSolid[(int)tile.type] && !Main.tileSolidTop[(int)tile.type]))
			{
				return false;
			}
			if (!Main.tileSolid[(int)tile.type] && Main.tileSolidTop[(int)tile.type] && tile.frameY != 0)
			{
				return false;
			}
			if (Main.tileSolidTop[(int)tile.type])
			{
				edgesToTest &= (BallCollision.TileEdges.Top | BallCollision.TileEdges.BottomLeftSlope | BallCollision.TileEdges.BottomRightSlope);
			}
			Vector2 tilePosition = new Vector2((float)x * 16f, (float)y * 16f);
			bool flag = false;
			LineSegment lineSegment = default(LineSegment);
			if (BallCollision.GetSlopeEdge(ref edgesToTest, tile, tilePosition, ref lineSegment))
			{
				closestPointOut = BallCollision.ClosestPointOnLineSegment(center, lineSegment);
				distanceSquaredOut = Vector2.DistanceSquared(closestPointOut, center);
				flag = true;
			}
			if (BallCollision.GetTopOrBottomEdge(edgesToTest, x, y, tilePosition, ref lineSegment))
			{
				Vector2 vector = BallCollision.ClosestPointOnLineSegment(center, lineSegment);
				float num = Vector2.DistanceSquared(vector, center);
				if (!flag || num < distanceSquaredOut)
				{
					distanceSquaredOut = num;
					closestPointOut = vector;
				}
				flag = true;
			}
			if (BallCollision.GetLeftOrRightEdge(edgesToTest, x, y, tilePosition, ref lineSegment))
			{
				Vector2 vector2 = BallCollision.ClosestPointOnLineSegment(center, lineSegment);
				float num2 = Vector2.DistanceSquared(vector2, center);
				if (!flag || num2 < distanceSquaredOut)
				{
					distanceSquaredOut = num2;
					closestPointOut = vector2;
				}
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x004C37A4 File Offset: 0x004C19A4
		private static bool GetSlopeEdge(ref BallCollision.TileEdges edgesToTest, Tile tile, Vector2 tilePosition, ref LineSegment edge)
		{
			switch (tile.slope())
			{
			case 0:
				return false;
			case 1:
				edgesToTest &= (BallCollision.TileEdges.Bottom | BallCollision.TileEdges.Left | BallCollision.TileEdges.BottomLeftSlope);
				if ((edgesToTest & BallCollision.TileEdges.BottomLeftSlope) == BallCollision.TileEdges.None)
				{
					return false;
				}
				edge.Start = tilePosition;
				edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y + 16f);
				return true;
			case 2:
				edgesToTest &= (BallCollision.TileEdges.Bottom | BallCollision.TileEdges.Right | BallCollision.TileEdges.BottomRightSlope);
				if ((edgesToTest & BallCollision.TileEdges.BottomRightSlope) == BallCollision.TileEdges.None)
				{
					return false;
				}
				edge.Start = new Vector2(tilePosition.X, tilePosition.Y + 16f);
				edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y);
				return true;
			case 3:
				edgesToTest &= (BallCollision.TileEdges.Top | BallCollision.TileEdges.Left | BallCollision.TileEdges.TopLeftSlope);
				if ((edgesToTest & BallCollision.TileEdges.TopLeftSlope) == BallCollision.TileEdges.None)
				{
					return false;
				}
				edge.Start = new Vector2(tilePosition.X, tilePosition.Y + 16f);
				edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y);
				return true;
			case 4:
				edgesToTest &= (BallCollision.TileEdges.Top | BallCollision.TileEdges.Right | BallCollision.TileEdges.TopRightSlope);
				if ((edgesToTest & BallCollision.TileEdges.TopRightSlope) == BallCollision.TileEdges.None)
				{
					return false;
				}
				edge.Start = tilePosition;
				edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y + 16f);
				return true;
			default:
				return false;
			}
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x004C38F0 File Offset: 0x004C1AF0
		private static bool GetTopOrBottomEdge(BallCollision.TileEdges edgesToTest, int x, int y, Vector2 tilePosition, ref LineSegment edge)
		{
			if ((edgesToTest & BallCollision.TileEdges.Bottom) != BallCollision.TileEdges.None)
			{
				Tile tile = Main.tile[x, y + 1];
				if (BallCollision.IsNeighborSolid(tile) && tile.slope() != 1 && tile.slope() != 2 && !tile.halfBrick())
				{
					return false;
				}
				edge.Start = new Vector2(tilePosition.X, tilePosition.Y + 16f);
				edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y + 16f);
				return true;
			}
			else
			{
				if ((edgesToTest & BallCollision.TileEdges.Top) == BallCollision.TileEdges.None)
				{
					return false;
				}
				Tile tile2 = Main.tile[x, y - 1];
				if (!Main.tile[x, y].halfBrick() && BallCollision.IsNeighborSolid(tile2) && tile2.slope() != 3 && tile2.slope() != 4)
				{
					return false;
				}
				if (Main.tile[x, y].halfBrick())
				{
					tilePosition.Y += 8f;
				}
				edge.Start = new Vector2(tilePosition.X, tilePosition.Y);
				edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y);
				return true;
			}
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x004C3A24 File Offset: 0x004C1C24
		private static bool GetLeftOrRightEdge(BallCollision.TileEdges edgesToTest, int x, int y, Vector2 tilePosition, ref LineSegment edge)
		{
			if ((edgesToTest & BallCollision.TileEdges.Left) != BallCollision.TileEdges.None)
			{
				Tile tile = Main.tile[x, y];
				Tile tile2 = Main.tile[x - 1, y];
				if (BallCollision.IsNeighborSolid(tile2) && tile2.slope() != 1 && tile2.slope() != 3 && (!tile2.halfBrick() || tile.halfBrick()))
				{
					return false;
				}
				edge.Start = new Vector2(tilePosition.X, tilePosition.Y);
				edge.End = new Vector2(tilePosition.X, tilePosition.Y + 16f);
				if (tile.halfBrick())
				{
					edge.Start.Y = edge.Start.Y + 8f;
				}
				return true;
			}
			else
			{
				if ((edgesToTest & BallCollision.TileEdges.Right) == BallCollision.TileEdges.None)
				{
					return false;
				}
				Tile tile3 = Main.tile[x, y];
				Tile tile4 = Main.tile[x + 1, y];
				if (BallCollision.IsNeighborSolid(tile4) && tile4.slope() != 2 && tile4.slope() != 4 && (!tile4.halfBrick() || tile3.halfBrick()))
				{
					return false;
				}
				edge.Start = new Vector2(tilePosition.X + 16f, tilePosition.Y);
				edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y + 16f);
				if (tile3.halfBrick())
				{
					edge.Start.Y = edge.Start.Y + 8f;
				}
				return true;
			}
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x004C3B9C File Offset: 0x004C1D9C
		private static Rectangle GetTileBounds(Vector2 position, Vector2 size)
		{
			int num = (int)Math.Floor((double)(position.X / 16f));
			int num2 = (int)Math.Floor((double)(position.Y / 16f));
			int num3 = (int)Math.Floor((double)((position.X + size.X) / 16f));
			int num4 = (int)Math.Floor((double)((position.Y + size.Y) / 16f));
			return new Rectangle(num, num2, num3 - num + 1, num4 - num2 + 1);
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x004C3C18 File Offset: 0x004C1E18
		private static bool IsNeighborSolid(Tile tile)
		{
			return tile != null && tile.nactive() && Main.tileSolid[(int)tile.type] && !Main.tileSolidTop[(int)tile.type];
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x004C3C44 File Offset: 0x004C1E44
		private static Vector2 ClosestPointOnLineSegment(Vector2 point, LineSegment lineSegment)
		{
			Vector2 value = point - lineSegment.Start;
			Vector2 vector = lineSegment.End - lineSegment.Start;
			float num = vector.LengthSquared();
			float num2 = Vector2.Dot(value, vector) / num;
			if (num2 < 0f)
			{
				return lineSegment.Start;
			}
			if (num2 > 1f)
			{
				return lineSegment.End;
			}
			return lineSegment.Start + vector * num2;
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x00009E06 File Offset: 0x00008006
		[Conditional("DEBUG")]
		private static void DrawEdge(LineSegment edge)
		{
		}

		// Token: 0x0200066A RID: 1642
		[Flags]
		private enum TileEdges : uint
		{
			// Token: 0x0400666C RID: 26220
			None = 0U,
			// Token: 0x0400666D RID: 26221
			Top = 1U,
			// Token: 0x0400666E RID: 26222
			Bottom = 2U,
			// Token: 0x0400666F RID: 26223
			Left = 4U,
			// Token: 0x04006670 RID: 26224
			Right = 8U,
			// Token: 0x04006671 RID: 26225
			TopLeftSlope = 16U,
			// Token: 0x04006672 RID: 26226
			TopRightSlope = 32U,
			// Token: 0x04006673 RID: 26227
			BottomLeftSlope = 64U,
			// Token: 0x04006674 RID: 26228
			BottomRightSlope = 128U
		}
	}
}
