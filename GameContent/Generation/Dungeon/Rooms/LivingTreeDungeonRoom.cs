using System;
using Microsoft.Xna.Framework;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.Rooms
{
	// Token: 0x020004A8 RID: 1192
	public class LivingTreeDungeonRoom : DungeonRoom
	{
		// Token: 0x06003423 RID: 13347 RVA: 0x006012CA File Offset: 0x005FF4CA
		public LivingTreeDungeonRoom(DungeonRoomSettings settings) : base(settings)
		{
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x006012EC File Offset: 0x005FF4EC
		public override void CalculateRoom(DungeonData data)
		{
			this.calculated = false;
			int x = this.settings.RoomPosition.X;
			int y = this.settings.RoomPosition.Y;
			this.LivingTreeRoom(data, x, y, false);
			this.calculated = true;
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x00601334 File Offset: 0x005FF534
		public override bool GenerateRoom(DungeonData data)
		{
			this.generated = false;
			int x = this.settings.RoomPosition.X;
			int y = this.settings.RoomPosition.Y;
			this.LivingTreeRoom(data, x, y, true);
			this.generated = true;
			return true;
		}

		// Token: 0x06003426 RID: 13350 RVA: 0x0060137C File Offset: 0x005FF57C
		public override int GetFloodedRoomTileCount()
		{
			return this._floodedTileCount;
		}

		// Token: 0x06003427 RID: 13351 RVA: 0x00601384 File Offset: 0x005FF584
		public override void FloodRoom(byte liquidType)
		{
			if (this._innerShapeData == null)
			{
				base.FloodRoom(liquidType);
				return;
			}
			WormlikeDungeonRoomSettings wormlikeDungeonRoomSettings = (WormlikeDungeonRoomSettings)this.settings;
			WorldUtils.Gen(this.BasePosition, new ModShapes.All(this._innerShapeData), Actions.Chain(new GenAction[]
			{
				new Modifiers.IsBelowHeight(base.Center.Y, true),
				new Modifiers.IsNotSolid(),
				new Actions.SetLiquid((int)liquidType, byte.MaxValue)
			}));
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x006013FC File Offset: 0x005FF5FC
		public override ProtectionType GetProtectionTypeFromPoint(int x, int y)
		{
			if (this._innerShapeData == null || this._outerShapeData == null || (this.calculated && !this.OuterBounds.Contains(x, y)))
			{
				return base.GetProtectionTypeFromPoint(x, y);
			}
			Point basePosition = this.BasePosition;
			if (!this._outerShapeData.Contains(x - basePosition.X, y - basePosition.Y))
			{
				return ProtectionType.None;
			}
			return ProtectionType.Walls;
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x00601460 File Offset: 0x005FF660
		public override bool IsInsideRoom(int x, int y)
		{
			Point basePosition = this.BasePosition;
			return base.IsInsideRoom(x, y) && this._innerShapeData.Contains(x - basePosition.X, y - basePosition.Y);
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x0060149C File Offset: 0x005FF69C
		public override void GenerateEarlyDungeonFeaturesInRoom(DungeonData data)
		{
			UnifiedRandom unifiedRandom = new UnifiedRandom(this.settings.RandomSeed);
			ushort brickTileType = this.settings.StyleData.BrickTileType;
			ushort brickCrackedTileType = this.settings.StyleData.BrickCrackedTileType;
			int growthLength = (int)((float)this.InnerBounds.Height * 0.1f) + unifiedRandom.Next(4);
			int branchDensity = 2 + unifiedRandom.Next(2);
			int leafDensity = 3 + unifiedRandom.Next(4);
			Point startPoint = new Point(this.InnerBounds.Center.X, this.InnerBounds.Top);
			DungeonUtils.GenerateHangingLeafCluster(data, unifiedRandom, this.OuterBounds, startPoint, growthLength, branchDensity, leafDensity, brickCrackedTileType, brickTileType, this.settings.OverridePaintTile, this.settings.OverridePaintTile, true, true);
			growthLength = (int)((float)this.InnerBounds.Height * 0.15f) + unifiedRandom.Next(5);
			branchDensity = 3 + unifiedRandom.Next(2);
			leafDensity = 4 + unifiedRandom.Next(4);
			startPoint = new Point(this.InnerBounds.Left + 2 + unifiedRandom.Next(3), this.InnerBounds.Top);
			DungeonUtils.GenerateHangingLeafCluster(data, unifiedRandom, this.OuterBounds, startPoint, growthLength, branchDensity, leafDensity, brickCrackedTileType, brickTileType, this.settings.OverridePaintTile, this.settings.OverridePaintTile, true, true);
			growthLength = (int)((float)this.InnerBounds.Height * 0.15f) + unifiedRandom.Next(5);
			branchDensity = 3 + unifiedRandom.Next(2);
			leafDensity = 4 + unifiedRandom.Next(4);
			startPoint = new Point(this.InnerBounds.Right - 2 - unifiedRandom.Next(3), this.InnerBounds.Top);
			DungeonUtils.GenerateHangingLeafCluster(data, unifiedRandom, this.OuterBounds, startPoint, growthLength, branchDensity, leafDensity, brickCrackedTileType, brickTileType, this.settings.OverridePaintTile, this.settings.OverridePaintTile, true, true);
			base.GenerateEarlyDungeonFeaturesInRoom(data);
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x00601674 File Offset: 0x005FF874
		public override void GenerateLateDungeonFeaturesInRoom(DungeonData data)
		{
			UnifiedRandom unifiedRandom = new UnifiedRandom(this.settings.RandomSeed);
			LivingTreeDungeonRoomSettings livingTreeDungeonRoomSettings = (LivingTreeDungeonRoomSettings)this.settings;
			ushort brickTileType = this.settings.StyleData.BrickTileType;
			ushort brickCrackedTileType = this.settings.StyleData.BrickCrackedTileType;
			ushort brickWallType = this.settings.StyleData.BrickWallType;
			for (int i = 0; i < 50; i++)
			{
				int num = unifiedRandom.Next(this.InnerBounds.Left + 1, this.InnerBounds.Right);
				int num2 = unifiedRandom.Next(this.InnerBounds.Top + 1, this.InnerBounds.Bottom);
				Point point = DungeonUtils.FirstSolid(false, new Point(num, num2), this.InnerBounds);
				num = point.X;
				num2 = point.Y - 1;
				Tile tile = Main.tile[num, num2];
				if (!tile.active() && tile.wall == brickWallType)
				{
					if (unifiedRandom.Next(2) == 0)
					{
						WorldGen.PlaceTile(num, num2, 187, true, false, -1, unifiedRandom.Next(47, 50));
					}
					else
					{
						int num3 = unifiedRandom.Next(2);
						int pileStyle = 72;
						if (num3 == 1)
						{
							pileStyle = unifiedRandom.Next(59, 62);
						}
						WorldGen.PlaceSmallPile(num, num2, pileStyle, num3, 185);
					}
				}
			}
			for (int j = 0; j < 10; j++)
			{
				int num4 = unifiedRandom.Next(this.InnerBounds.Left + 1, this.InnerBounds.Right);
				int num5 = unifiedRandom.Next(this.InnerBounds.Top + 1, this.InnerBounds.Bottom);
				Point point2 = DungeonUtils.FirstSolid(true, new Point(num4, num5), this.InnerBounds);
				num4 = point2.X;
				num5 = point2.Y + 1;
				Tile tile2 = Main.tile[num4, num5];
				Tile tile3 = Main.tile[num4, num5 - 1];
				if (!tile2.active() && tile2.wall == brickWallType && tile3.active() && tile3.type == brickCrackedTileType)
				{
					ushort type = 52;
					if (brickTileType == 383)
					{
						type = 62;
					}
					for (int k = unifiedRandom.Next(3, 12); k > 0; k--)
					{
						Tile tile4 = Main.tile[num4, num5];
						if (tile4.active())
						{
							break;
						}
						tile4.ClearTile();
						tile4.active(true);
						tile4.type = type;
						if (livingTreeDungeonRoomSettings.OverridePaintTile > -1)
						{
							WorldGen.paintTile(num4, num5, (byte)livingTreeDungeonRoomSettings.OverridePaintTile, false, false);
						}
						num5++;
					}
				}
			}
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x00601918 File Offset: 0x005FFB18
		public void LivingTreeRoom(DungeonData data, int i, int j, bool generating)
		{
			UnifiedRandom unifiedRandom = new UnifiedRandom(this.settings.RandomSeed);
			LivingTreeDungeonRoomSettings livingTreeDungeonRoomSettings = (LivingTreeDungeonRoomSettings)this.settings;
			ushort brickTileType = this.settings.StyleData.BrickTileType;
			ushort brickCrackedTileType = this.settings.StyleData.BrickCrackedTileType;
			ushort brickWallType = this.settings.StyleData.BrickWallType;
			Point basePosition = new Point(i, j);
			if (this.calculated)
			{
				basePosition = this.BasePosition;
			}
			Point point = new Point(basePosition.X, basePosition.Y + livingTreeDungeonRoomSettings.InnerHeight / 2);
			int num = point.Y - livingTreeDungeonRoomSettings.InnerHeight;
			int innerWidth = livingTreeDungeonRoomSettings.InnerWidth;
			int depth = livingTreeDungeonRoomSettings.Depth;
			int num2 = innerWidth;
			int num3 = num2 + depth;
			this.OuterBounds.SetBounds(basePosition.X, basePosition.Y, basePosition.X, basePosition.Y);
			this.InnerBounds.SetBounds(basePosition.X, basePosition.Y, basePosition.X, basePosition.Y);
			while (point.Y > num)
			{
				this.OuterBounds.UpdateBounds(point.X - num3, point.Y - num3, point.X + num3, point.Y + num3);
				this.InnerBounds.UpdateBounds(point.X - num2, point.Y - num2, point.X + num2, point.Y + num2);
				this._outerShapeData.AddBounds(point.X - num3 - basePosition.X, point.Y - num3 - basePosition.Y, point.X + num3 - basePosition.X, point.Y + num3 - basePosition.Y);
				this._innerShapeData.AddBounds(point.X - num2 - basePosition.X, point.Y - num2 - basePosition.Y, point.X + num2 - basePosition.X, point.Y + num2 - basePosition.Y);
				if (generating)
				{
					base.GenerateDungeonSquareRoom(data, point, brickTileType, brickCrackedTileType, brickWallType, livingTreeDungeonRoomSettings.InnerWidth, livingTreeDungeonRoomSettings.Depth, false);
				}
				if (point.Y % 4 == 0)
				{
					point.X += ((unifiedRandom.Next(2) == 0) ? -1 : 1);
				}
				point.Y--;
			}
			this.InnerBounds.CalculateHitbox();
			this.OuterBounds.CalculateHitbox();
			this.BasePosition = basePosition;
			this._floodedTileCount = DungeonUtils.CalculateFloodedTileCountFromShapeData(this.InnerBounds, this._innerShapeData);
		}

		// Token: 0x040059BE RID: 22974
		private ShapeData _innerShapeData = new ShapeData();

		// Token: 0x040059BF RID: 22975
		private ShapeData _outerShapeData = new ShapeData();

		// Token: 0x040059C0 RID: 22976
		private int _floodedTileCount;

		// Token: 0x040059C1 RID: 22977
		private Point BasePosition;
	}
}
