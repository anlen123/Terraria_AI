using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.Rooms
{
	// Token: 0x020004AA RID: 1194
	public class WormlikeDungeonRoom : DungeonRoom
	{
		// Token: 0x0600342F RID: 13359 RVA: 0x00601C03 File Offset: 0x005FFE03
		public WormlikeDungeonRoom(DungeonRoomSettings settings) : base(settings)
		{
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x00601C24 File Offset: 0x005FFE24
		public override void CalculateRoom(DungeonData data)
		{
			this.calculated = false;
			int x = this.settings.RoomPosition.X;
			int y = this.settings.RoomPosition.Y;
			this.WormlikeRoom(data, x, y, false);
			this.calculated = true;
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x00601C6C File Offset: 0x005FFE6C
		public override bool GenerateRoom(DungeonData data)
		{
			this.generated = false;
			int x = this.settings.RoomPosition.X;
			int y = this.settings.RoomPosition.Y;
			this.WormlikeRoom(data, x, y, true);
			this.generated = true;
			return true;
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x00601CB4 File Offset: 0x005FFEB4
		public override int GetFloodedRoomTileCount()
		{
			return this._floodedTileCount;
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x00601CBC File Offset: 0x005FFEBC
		public override void FloodRoom(byte liquidType)
		{
			if (this._innerShapeData == null || this.Positions == null)
			{
				base.FloodRoom(liquidType);
				return;
			}
			WormlikeDungeonRoomSettings wormlikeDungeonRoomSettings = (WormlikeDungeonRoomSettings)this.settings;
			WorldUtils.Gen(this.Positions[0].ToPoint(), new ModShapes.All(this._innerShapeData), Actions.Chain(new GenAction[]
			{
				new Modifiers.IsBelowHeight(base.Center.Y, true),
				new Modifiers.IsNotSolid(),
				new Actions.SetLiquid((int)liquidType, byte.MaxValue)
			}));
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x00601D44 File Offset: 0x005FFF44
		public override ProtectionType GetProtectionTypeFromPoint(int x, int y)
		{
			if (this._innerShapeData == null || this._outerShapeData == null || this.Positions == null || (this.calculated && !this.OuterBounds.Contains(x, y)))
			{
				return base.GetProtectionTypeFromPoint(x, y);
			}
			Point point = this.Positions[0].ToPoint();
			if (!this._outerShapeData.Contains(x - point.X, y - point.Y))
			{
				return ProtectionType.None;
			}
			return ProtectionType.Walls;
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x00601DBC File Offset: 0x005FFFBC
		public override bool IsInsideRoom(int x, int y)
		{
			if (this.Positions == null)
			{
				return base.IsInsideRoom(x, y);
			}
			Point point = this.Positions[0].ToPoint();
			return base.IsInsideRoom(x, y) && this._innerShapeData.Contains(x - point.X, y - point.Y);
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x00601E14 File Offset: 0x00600014
		public void WormlikeRoom(DungeonData data, int i, int j, bool generating)
		{
			UnifiedRandom unifiedRandom = new UnifiedRandom(this.settings.RandomSeed);
			WormlikeDungeonRoomSettings wormlikeDungeonRoomSettings = (WormlikeDungeonRoomSettings)this.settings;
			ushort brickTileType = this.settings.StyleData.BrickTileType;
			ushort brickCrackedTileType = this.settings.StyleData.BrickCrackedTileType;
			ushort brickWallType = this.settings.StyleData.BrickWallType;
			Point point = new Point(i, j);
			if (base.Processed)
			{
				point = this.Positions[0].ToPoint();
			}
			int num = 9 + unifiedRandom.Next(3);
			int num2 = Math.Max(4, num / 5);
			if (base.Processed)
			{
				num = this.InnerBoundsSizeMax;
				num2 = this.InnerBoundsSizeMin;
			}
			int num3 = 8;
			int num4 = num + num3;
			this.InnerBounds.SetBounds(point.X, point.Y, point.X, point.Y);
			this.OuterBounds.SetBounds(point.X, point.Y, point.X, point.Y);
			Vector2 vector = point.ToVector2();
			Vector2 vector2 = vector;
			List<Vector2> list = new List<Vector2>();
			if (base.Processed)
			{
				list.AddRange(this.Positions);
			}
			vector = vector2;
			Vector2 vector3 = unifiedRandom.NextVector2CircularEdge(1f, 1f);
			Vector2 spinningpoint = vector3;
			int num5 = wormlikeDungeonRoomSettings.FirstSideIterations;
			int num6 = 0;
			for (int k = 0; k < num5; k++)
			{
				float num7 = (float)k / (float)num5;
				int num8 = (int)Utils.Lerp((double)num, (double)num2, (double)num7);
				num4 = num8 + num3;
				Point point2 = vector.ToPoint();
				this.OuterBounds.UpdateBounds(point2.X - num4, point2.Y - num4, point2.X + num4, point2.Y + num4);
				this.InnerBounds.UpdateBounds(point2.X - num8, point2.Y - num8, point2.X + num8, point2.Y + num8);
				this._outerShapeData.AddBounds(point2.X - num4 - (int)vector2.X, point2.Y - num4 - (int)vector2.Y, point2.X + num4 - (int)vector2.X, point2.Y + num4 - (int)vector2.Y);
				this._innerShapeData.AddBounds(point2.X - num8 - (int)vector2.X, point2.Y - num8 - (int)vector2.Y, point2.X + num8 - (int)vector2.X, point2.Y + num8 - (int)vector2.Y);
				if (!base.Processed)
				{
					list.Add(vector);
				}
				if (generating)
				{
					base.GenerateDungeonSquareRoom(data, point2, brickTileType, brickCrackedTileType, brickWallType, num8, num3, false);
				}
				if (base.Processed)
				{
					num6++;
					if (num6 < this.Positions.Length)
					{
						vector = this.Positions[num6];
					}
				}
				else
				{
					vector += vector3;
					vector3 = spinningpoint.RotatedBy(Utils.Lerp(0.0, 1.5707963705062866, (double)num7), default(Vector2));
				}
			}
			vector = vector2;
			vector3 = spinningpoint.RotatedBy(3.1415927410125732, Vector2.Zero).RotatedByRandom(0.7853981852531433);
			spinningpoint = vector3;
			num5 = wormlikeDungeonRoomSettings.SecondSideIterations;
			for (int l = 0; l < num5; l++)
			{
				float num9 = (float)l / (float)num5;
				int num8 = (int)Utils.Lerp((double)num, (double)num2, (double)num9);
				num4 = num8 + num3;
				Point point3 = vector.ToPoint();
				this.OuterBounds.UpdateBounds(point3.X - num4, point3.Y - num4, point3.X + num4, point3.Y + num4);
				this.InnerBounds.UpdateBounds(point3.X - num8, point3.Y - num8, point3.X + num8, point3.Y + num8);
				this._outerShapeData.AddBounds(point3.X - num4 - (int)vector2.X, point3.Y - num4 - (int)vector2.Y, point3.X + num4 - (int)vector2.X, point3.Y + num4 - (int)vector2.Y);
				this._innerShapeData.AddBounds(point3.X - num8 - (int)vector2.X, point3.Y - num8 - (int)vector2.Y, point3.X + num8 - (int)vector2.X, point3.Y + num8 - (int)vector2.Y);
				if (!base.Processed)
				{
					list.Add(vector);
				}
				if (generating)
				{
					base.GenerateDungeonSquareRoom(data, point3, brickTileType, brickCrackedTileType, brickWallType, num8, num3, false);
				}
				if (base.Processed)
				{
					num6++;
					if (num6 < this.Positions.Length)
					{
						vector = this.Positions[num6];
					}
				}
				else
				{
					vector += vector3;
					vector3 = spinningpoint.RotatedBy(Utils.Lerp(0.0, 1.5707963705062866, (double)num9), default(Vector2));
				}
			}
			this.Positions = list.ToArray<Vector2>();
			this.InnerBoundsSizeMin = num2;
			this.InnerBoundsSizeMax = num;
			this.InnerBounds.CalculateHitbox();
			this.OuterBounds.CalculateHitbox();
			this._floodedTileCount = DungeonUtils.CalculateFloodedTileCountFromShapeData(this.InnerBounds, this._innerShapeData);
		}

		// Token: 0x040059C4 RID: 22980
		private ShapeData _innerShapeData = new ShapeData();

		// Token: 0x040059C5 RID: 22981
		private ShapeData _outerShapeData = new ShapeData();

		// Token: 0x040059C6 RID: 22982
		private int _floodedTileCount;

		// Token: 0x040059C7 RID: 22983
		public int InnerBoundsSizeMin;

		// Token: 0x040059C8 RID: 22984
		public int InnerBoundsSizeMax;

		// Token: 0x040059C9 RID: 22985
		public Vector2[] Positions;
	}
}
