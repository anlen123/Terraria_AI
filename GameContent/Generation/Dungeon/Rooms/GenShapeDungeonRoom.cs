using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.Rooms
{
	// Token: 0x020004AD RID: 1197
	public class GenShapeDungeonRoom : DungeonRoom
	{
		// Token: 0x06003439 RID: 13369 RVA: 0x006023C1 File Offset: 0x006005C1
		public GenShapeDungeonRoom(DungeonRoomSettings settings) : base(settings)
		{
			GenShapeDungeonRoomSettings genShapeDungeonRoomSettings = (GenShapeDungeonRoomSettings)settings;
		}

		// Token: 0x0600343A RID: 13370 RVA: 0x006023E8 File Offset: 0x006005E8
		public override void CalculateRoom(DungeonData data)
		{
			this.calculated = false;
			int x = this.settings.RoomPosition.X;
			int y = this.settings.RoomPosition.Y;
			this.GenShapeRoom(data, x, y, false);
			this.calculated = true;
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x00602430 File Offset: 0x00600630
		public override bool GenerateRoom(DungeonData data)
		{
			this.generated = false;
			int x = this.settings.RoomPosition.X;
			int y = this.settings.RoomPosition.Y;
			this.GenShapeRoom(data, x, y, true);
			this.generated = true;
			return true;
		}

		// Token: 0x0600343C RID: 13372 RVA: 0x00602478 File Offset: 0x00600678
		public override bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
		{
			GenShapeType shapeType = ((GenShapeDungeonRoomSettings)this.settings).ShapeType;
			return (shapeType - GenShapeType.Hourglass > 1 || !(feature is DungeonWindow)) && base.CanGenerateFeatureAt(data, feature, x, y);
		}

		// Token: 0x0600343D RID: 13373 RVA: 0x006024B4 File Offset: 0x006006B4
		public override void GenerateEarlyDungeonFeaturesInRoom(DungeonData data)
		{
			GenShapeDungeonRoomSettings genShapeDungeonRoomSettings = (GenShapeDungeonRoomSettings)this.settings;
			if (genShapeDungeonRoomSettings.ShapeType == GenShapeType.Doughnut)
			{
				DungeonShapes.CircleRoom circleRoom = (DungeonShapes.CircleRoom)genShapeDungeonRoomSettings.InnerShape;
				GenAction genAction = new Actions.Blank();
				if (genShapeDungeonRoomSettings.OverridePaintTile == 0)
				{
					genAction = new Actions.ClearTilePaint();
				}
				else if (genShapeDungeonRoomSettings.OverridePaintTile > 0)
				{
					genAction = new Actions.SetTilePaint((byte)genShapeDungeonRoomSettings.OverridePaintTile);
				}
				DungeonShapes.CircleRoom shape = new DungeonShapes.CircleRoom(Math.Max(1, circleRoom.HorizontalRadius / 2), Math.Max(1, circleRoom.VerticalRadius / 2));
				WorldUtils.Gen(this.InnerBounds.Center, shape, Actions.Chain(new GenAction[]
				{
					new Actions.ClearTile(false),
					new Actions.SetTile(genShapeDungeonRoomSettings.StyleData.BrickTileType, false, false, false),
					genAction
				}));
			}
			base.GenerateEarlyDungeonFeaturesInRoom(data);
		}

		// Token: 0x0600343E RID: 13374 RVA: 0x0060257C File Offset: 0x0060077C
		public override Point GetRoomCenterForDungeonFeature(DungeonData data, DungeonFeature feature)
		{
			GenShapeDungeonRoomSettings genShapeDungeonRoomSettings = (GenShapeDungeonRoomSettings)this.settings;
			Point roomCenterForDungeonFeature = base.GetRoomCenterForDungeonFeature(data, feature);
			if (feature is DungeonWindow && genShapeDungeonRoomSettings.ShapeType == GenShapeType.Mound)
			{
				roomCenterForDungeonFeature.Y += this.InnerBounds.Height / 5;
			}
			return roomCenterForDungeonFeature;
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x006025C8 File Offset: 0x006007C8
		public override Point GetRoomCenterForHallway(Vector2D otherRoomPos)
		{
			DungeonRoomSettings dungeonRoomSettings = (GenShapeDungeonRoomSettings)this.settings;
			Vector2D vector2D = base.GetRoomCenterForHallway(otherRoomPos).ToVector2D();
			Vector2D vector2D2 = Vector2D.UnitX;
			DungeonRoomType roomType = dungeonRoomSettings.RoomType;
			if (roomType != DungeonRoomType.GenShapeDoughnut)
			{
				return vector2D.ToPoint();
			}
			vector2D2 = (otherRoomPos - vector2D).SafeNormalize(Vector2D.UnitX);
			Point result = (vector2D + vector2D2 * (double)(this.InnerBounds.Size / 2)).ToPoint();
			result.X = (int)(vector2D.X + vector2D2.X * (double)(this.InnerBounds.Width / 2));
			result.Y = (int)(vector2D.Y + vector2D2.Y * (double)(this.InnerBounds.Height / 2));
			return result;
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x00602680 File Offset: 0x00600880
		public override int GetFloodedRoomTileCount()
		{
			return this._floodedTileCount;
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x00602688 File Offset: 0x00600888
		public override void FloodRoom(byte liquidType)
		{
			GenShapeDungeonRoomSettings genShapeDungeonRoomSettings = (GenShapeDungeonRoomSettings)this.settings;
			WorldUtils.Gen(this.InnerBounds.Center, genShapeDungeonRoomSettings.InnerShape, Actions.Chain(new GenAction[]
			{
				new Modifiers.IsBelowHeight(this.InnerBounds.Center.Y, true),
				new Modifiers.IsNotSolid(),
				new Actions.SetLiquid((int)liquidType, byte.MaxValue)
			}));
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x006026F4 File Offset: 0x006008F4
		public override ProtectionType GetProtectionTypeFromPoint(int x, int y)
		{
			if (this._innerShapeData == null || this._outerShapeData == null || (this.calculated && !this.OuterBounds.Contains(x, y)))
			{
				return base.GetProtectionTypeFromPoint(x, y);
			}
			if (!this._outerShapeData.Contains(x - base.Center.X, y - base.Center.Y))
			{
				return ProtectionType.None;
			}
			return ProtectionType.Walls;
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x0060275B File Offset: 0x0060095B
		public override bool IsInsideRoom(int x, int y)
		{
			return base.IsInsideRoom(x, y) && this._innerShapeData.Contains(x - base.Center.X, y - base.Center.Y);
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x00602790 File Offset: 0x00600990
		public void GenShapeRoom(DungeonData data, int i, int j, bool generating)
		{
			new UnifiedRandom(this.settings.RandomSeed);
			GenShapeDungeonRoomSettings genShapeDungeonRoomSettings = (GenShapeDungeonRoomSettings)this.settings;
			ushort brickTileType = this.settings.StyleData.BrickTileType;
			ushort brickWallType = this.settings.StyleData.BrickWallType;
			Vector2D vector2D;
			vector2D..ctor((double)i, (double)j);
			if (base.Processed)
			{
				vector2D = base.Center;
			}
			this.InnerBounds.SetBounds((int)vector2D.X, (int)vector2D.Y, (int)vector2D.X, (int)vector2D.Y);
			this.OuterBounds.SetBounds((int)vector2D.X, (int)vector2D.Y, (int)vector2D.X, (int)vector2D.Y);
			GenAction genAction = new Actions.Blank();
			if (genShapeDungeonRoomSettings.OverridePaintTile == 0)
			{
				genAction = new Actions.ClearTilePaint();
			}
			else if (genShapeDungeonRoomSettings.OverridePaintTile > 0)
			{
				genAction = new Actions.SetTilePaint((byte)genShapeDungeonRoomSettings.OverridePaintTile);
			}
			GenAction genAction2 = new Actions.Blank();
			if (genShapeDungeonRoomSettings.OverridePaintWall == 0)
			{
				genAction2 = new Actions.ClearWallPaint();
			}
			else if (genShapeDungeonRoomSettings.OverridePaintWall > 0)
			{
				genAction2 = new Actions.SetWallPaint((byte)genShapeDungeonRoomSettings.OverridePaintWall);
			}
			WorldUtils.Gen(vector2D.ToPoint(), genShapeDungeonRoomSettings.OuterShape, Actions.Chain(new GenAction[]
			{
				new Modifiers.Expand(1),
				new Actions.UpdateBounds(data.dungeonBounds).Output(this._outerShapeData),
				new Actions.UpdateBounds(this.OuterBounds),
				new Modifiers.Conditions(new GenCondition[]
				{
					new Conditions.BoolCheck(generating)
				}),
				new Actions.ClearTile(false),
				new Actions.SetTile(brickTileType, false, false, false),
				genAction
			}));
			if (generating)
			{
				WorldUtils.Gen(vector2D.ToPoint(), genShapeDungeonRoomSettings.OuterShape, Actions.Chain(new GenAction[]
				{
					new Actions.SetWall(brickWallType, false, false, false),
					genAction2
				}));
			}
			WorldUtils.Gen(vector2D.ToPoint(), genShapeDungeonRoomSettings.InnerShape, Actions.Chain(new GenAction[]
			{
				new Actions.UpdateBounds(data.dungeonBounds).Output(this._innerShapeData),
				new Actions.UpdateBounds(this.InnerBounds),
				new Modifiers.Conditions(new GenCondition[]
				{
					new Conditions.BoolCheck(generating)
				}),
				new Actions.Clear(),
				new Actions.SetWall(brickWallType, false, false, false),
				genAction2
			}));
			this.InnerBounds.CalculateHitbox();
			this.OuterBounds.CalculateHitbox();
			this._floodedTileCount = DungeonUtils.CalculateFloodedTileCountFromShapeData(this.InnerBounds, this._innerShapeData);
		}

		// Token: 0x040059D4 RID: 22996
		private ShapeData _innerShapeData = new ShapeData();

		// Token: 0x040059D5 RID: 22997
		private ShapeData _outerShapeData = new ShapeData();

		// Token: 0x040059D6 RID: 22998
		private int _floodedTileCount;
	}
}
