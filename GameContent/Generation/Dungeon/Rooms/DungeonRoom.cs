using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Rooms
{
	// Token: 0x020004B4 RID: 1204
	public abstract class DungeonRoom
	{
		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x0600344F RID: 13391 RVA: 0x00602CAD File Offset: 0x00600EAD
		public bool Processed
		{
			get
			{
				return this.calculated || this.generated;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06003450 RID: 13392 RVA: 0x00602CBF File Offset: 0x00600EBF
		public Point Center
		{
			get
			{
				return this.InnerBounds.Center;
			}
		}

		// Token: 0x06003451 RID: 13393 RVA: 0x00602CCC File Offset: 0x00600ECC
		public DungeonRoom(DungeonRoomSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06003452 RID: 13394 RVA: 0x00602CF4 File Offset: 0x00600EF4
		public virtual bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
		{
			return (!(feature is DungeonWindow) || data.Type == DungeonType.DualDungeon) && (!(feature is DungeonPitTrap) || ((DungeonPitTrapSettings)((DungeonPitTrap)feature).settings).ConnectedRoom == this) && this.settings.StyleData.CanGenerateFeatureAt(data, this, feature, x, y);
		}

		// Token: 0x06003453 RID: 13395 RVA: 0x00602D4C File Offset: 0x00600F4C
		public virtual void GeneratePreHallwaysDungeonFeaturesInRoom(DungeonData data)
		{
			if ((this.settings.StyleData.Style == 4 || this.settings.StyleData.Style == 5) && this.InnerBounds.Width > 10 && this.InnerBounds.Height > 10)
			{
				DungeonUtils.GenerateSpeleothemsInArea(data, this.settings.StyleData, this.InnerBounds.Left, this.InnerBounds.Top, this.InnerBounds.Width, this.InnerBounds.Height, Math.Max(3, this.InnerBounds.Width / 3), this.settings.StyleData.BrickTileType, this.settings.OverridePaintTile, -1, -1);
			}
		}

		// Token: 0x06003454 RID: 13396 RVA: 0x00602E10 File Offset: 0x00601010
		public virtual void GenerateEarlyDungeonFeaturesInRoom(DungeonData data)
		{
			UnifiedRandom unifiedRandom = new UnifiedRandom(this.settings.RandomSeed);
			if (data.Type != DungeonType.DualDungeon)
			{
				return;
			}
			if (unifiedRandom.Next(3) == 0)
			{
				DungeonWindowBasicSettings dungeonWindowBasicSettings = new DungeonWindowBasicSettings
				{
					Style = this.settings.StyleData,
					Closed = ((double)this.InnerBounds.Bottom > Main.worldSurface)
				};
				int width = this.InnerBounds.Width;
				int height = this.InnerBounds.Height;
				bool flag = true;
				int num = unifiedRandom.Next(3);
				if (num >= 1 && num <= 2 && (width <= 36 || height <= 15))
				{
					num = 0;
				}
				if (num == 0 && (width <= 14 || height <= 10))
				{
					flag = false;
				}
				if (flag)
				{
					Point point = this.InnerBounds.Center;
					if (num == 0 || num - 1 > 1)
					{
						int num2 = Math.Max(3, this.InnerBounds.Width / 3);
						if (num2 % 2 == 0)
						{
							num2++;
						}
						dungeonWindowBasicSettings.Width = Math.Max(3, num2);
						dungeonWindowBasicSettings.Height = Math.Max(5, this.InnerBounds.Height / 3);
						DungeonWindow dungeonWindow = new DungeonWindowBasic(dungeonWindowBasicSettings);
						point = this.GetRoomCenterForDungeonFeature(data, dungeonWindow);
						if (this.CanGenerateFeatureAt(data, dungeonWindow, point.X, point.Y))
						{
							dungeonWindow.GenerateFeature(data, point.X, point.Y);
						}
					}
					else
					{
						int num2 = Math.Min(7, Math.Max(3, this.InnerBounds.Width / 5));
						if (num2 % 2 == 0)
						{
							num2++;
						}
						dungeonWindowBasicSettings.Width = Math.Max(3, num2);
						dungeonWindowBasicSettings.Height = Math.Max(5, this.InnerBounds.Height / 3);
						DungeonWindow dungeonWindow = new DungeonWindowBasic(dungeonWindowBasicSettings);
						point = this.GetRoomCenterForDungeonFeature(data, dungeonWindow);
						if (this.CanGenerateFeatureAt(data, dungeonWindow, point.X, point.Y))
						{
							dungeonWindow.GenerateFeature(data, point.X, point.Y);
						}
						dungeonWindowBasicSettings.Height -= 2;
						dungeonWindow = new DungeonWindowBasic(dungeonWindowBasicSettings);
						if (this.CanGenerateFeatureAt(data, dungeonWindow, point.X - num2 - 2, point.Y))
						{
							dungeonWindow.GenerateFeature(data, point.X - num2 - 2, point.Y);
						}
						dungeonWindow = new DungeonWindowBasic(dungeonWindowBasicSettings);
						if (this.CanGenerateFeatureAt(data, dungeonWindow, point.X + num2 + 2, point.Y))
						{
							dungeonWindow.GenerateFeature(data, point.X + num2 + 2, point.Y);
						}
					}
				}
			}
			int liquidType = this.settings.StyleData.LiquidType;
			if (liquidType >= 0)
			{
				this.FloodRoom((byte)liquidType);
			}
		}

		// Token: 0x06003455 RID: 13397 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void GenerateLateDungeonFeaturesInRoom(DungeonData data)
		{
		}

		// Token: 0x06003456 RID: 13398 RVA: 0x006030AF File Offset: 0x006012AF
		public virtual Point GetRoomCenterForDungeonFeature(DungeonData data, DungeonFeature feature)
		{
			return this.Center;
		}

		// Token: 0x06003457 RID: 13399 RVA: 0x006030AF File Offset: 0x006012AF
		public virtual Point GetRoomCenterForHallway(Vector2D otherRoomPos)
		{
			return this.Center;
		}

		// Token: 0x06003458 RID: 13400
		public abstract void CalculateRoom(DungeonData data);

		// Token: 0x06003459 RID: 13401 RVA: 0x006030B7 File Offset: 0x006012B7
		public virtual void CalculatePlatformsAndDoors(DungeonData data)
		{
			DungeonUtils.CalculatePlatformsAndDoorsOnEdgesOfRoom(data, this.InnerBounds, this.settings.ForceStyleForDoorsAndPlatforms ? this.settings.StyleData : null, new int?(3), new int?(3));
		}

		// Token: 0x0600345A RID: 13402 RVA: 0x006030EC File Offset: 0x006012EC
		public virtual ConnectionPointQuality GetHallwayConnectionPoint(Vector2D otherRoomPos, out Vector2D connectionPoint)
		{
			if (this.settings.HallwayConnectionPointOverride != null)
			{
				ConnectionPointQuality result = this.settings.HallwayConnectionPointOverride(this, otherRoomPos, out connectionPoint);
				if (this.settings.HallwayPointAdjuster != null)
				{
					Vector2D vector2D = (otherRoomPos - connectionPoint).SafeNormalize(Vector2D.UnitX);
					connectionPoint -= vector2D * (double)this.settings.HallwayPointAdjuster.Value;
				}
				return result;
			}
			connectionPoint = this.GetRoomCenterForHallway(otherRoomPos);
			Vector2D vector2D2 = (otherRoomPos - connectionPoint).SafeNormalize(Vector2D.UnitX);
			if (-0.5 < vector2D2.Y && vector2D2.Y < 0.7 && WorldGen.genRand.Next(2) == 0)
			{
				while (this.IsInsideRoom(connectionPoint.ToPoint()))
				{
					connectionPoint.Y += 1.0;
				}
				connectionPoint.Y -= 3.0;
			}
			else if (-0.7 < vector2D2.Y && vector2D2.Y < 0.5 && WorldGen.genRand.Next(3) == 0)
			{
				while (this.IsInsideRoom(connectionPoint.ToPoint()))
				{
					connectionPoint.Y -= 1.0;
				}
				connectionPoint.Y += 3.0;
			}
			else
			{
				connectionPoint += WorldGen.genRand.NextVector2DCircularEdge(4.0, 4.0);
			}
			vector2D2 = (otherRoomPos - connectionPoint).SafeNormalize(Vector2D.UnitX);
			while (this.IsInsideRoom(connectionPoint.ToPoint()))
			{
				connectionPoint += vector2D2;
			}
			if (this.settings.HallwayPointAdjuster != null)
			{
				connectionPoint -= vector2D2 * (double)this.settings.HallwayPointAdjuster.Value;
			}
			return ConnectionPointQuality.Good;
		}

		// Token: 0x0600345B RID: 13403
		public abstract bool GenerateRoom(DungeonData data);

		// Token: 0x0600345C RID: 13404 RVA: 0x00603315 File Offset: 0x00601515
		public virtual bool TryGenerateChestInRoom(DungeonData data, DungeonGlobalBasicChests feature)
		{
			return DungeonUtils.GenerateDungeonRegularChest(data, feature, this.settings.StyleData, this.InnerBounds);
		}

		// Token: 0x0600345D RID: 13405 RVA: 0x0060332F File Offset: 0x0060152F
		public virtual bool DualDungeons_TryGenerateBiomeChestInRoom(DungeonData data, DungeonGlobalBiomeChests feature)
		{
			return DungeonUtils.GenerateDungeonBiomeChest(data, feature, this.settings.StyleData, this.InnerBounds, true);
		}

		// Token: 0x0600345E RID: 13406 RVA: 0x0060334A File Offset: 0x0060154A
		public virtual ProtectionType GetProtectionTypeFromPoint(int x, int y)
		{
			if (!this.OuterBounds.Contains(x, y))
			{
				return ProtectionType.None;
			}
			return ProtectionType.Walls;
		}

		// Token: 0x0600345F RID: 13407 RVA: 0x0060335E File Offset: 0x0060155E
		public bool IsInsideRoom(Point point)
		{
			return this.IsInsideRoom(point.X, point.Y);
		}

		// Token: 0x06003460 RID: 13408 RVA: 0x00603372 File Offset: 0x00601572
		public virtual bool IsInsideRoom(int x, int y)
		{
			return this.InnerBounds.Contains(x, y);
		}

		// Token: 0x06003461 RID: 13409 RVA: 0x00603381 File Offset: 0x00601581
		public virtual int GetFloodedRoomTileCount()
		{
			return this.InnerBounds.Width * this.InnerBounds.Height;
		}

		// Token: 0x06003462 RID: 13410 RVA: 0x0060339C File Offset: 0x0060159C
		public virtual void FloodRoom(byte liquidType)
		{
			for (int i = this.InnerBounds.Left; i <= this.InnerBounds.Right; i++)
			{
				for (int j = this.InnerBounds.Center.Y; j <= this.InnerBounds.Bottom; j++)
				{
					Tile tile = Main.tile[i, j];
					if (!tile.active())
					{
						tile.liquid = byte.MaxValue;
						tile.liquidType((int)liquidType);
					}
				}
			}
		}

		// Token: 0x06003463 RID: 13411 RVA: 0x001FC399 File Offset: 0x001FA599
		public virtual int GetFurnitureCount(int defaultCount)
		{
			return defaultCount;
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x00603418 File Offset: 0x00601618
		public void GenerateDungeonSquareRoom(DungeonData data, DungeonBounds innerBounds, DungeonBounds outerBounds, Vector2D currentPoint, ushort tileType, ushort wallType, int innerBoundsSize, int totalBoundsSize, bool genTiles = true, bool genWalls = true)
		{
			for (int i = -totalBoundsSize; i <= totalBoundsSize; i++)
			{
				int num = (int)currentPoint.X + i;
				for (int j = -totalBoundsSize; j <= totalBoundsSize; j++)
				{
					int num2 = (int)currentPoint.Y + j;
					Tile tile = Main.tile[num, num2];
					if (Math.Abs(i) <= innerBoundsSize && Math.Abs(j) <= innerBoundsSize)
					{
						innerBounds.UpdateBounds(num, num2);
						if (genWalls)
						{
							DungeonUtils.ChangeWallType(tile, wallType, true, this.settings.OverridePaintWall);
						}
					}
					else if (!DungeonUtils.IsHigherOrEqualTieredDungeonWall(data, (int)tile.wall, (int)wallType))
					{
						outerBounds.UpdateBounds(num, num2);
						if (genTiles)
						{
							DungeonUtils.ChangeTileType(tile, tileType, true, this.settings.OverridePaintTile);
						}
						if (genWalls && i > -totalBoundsSize && i < totalBoundsSize && j > -totalBoundsSize && j < totalBoundsSize)
						{
							DungeonUtils.ChangeWallType(tile, wallType, false, this.settings.OverridePaintWall);
						}
					}
				}
			}
		}

		// Token: 0x06003465 RID: 13413 RVA: 0x0060350C File Offset: 0x0060170C
		public void GenerateDungeonSquareRoom(DungeonData data, Vector2D currentPoint, ushort tileType, ushort tileCrackedType, ushort wallType, int innerBoundsSize, int outerBoundsSize, bool crackedBricks = false)
		{
			int num = innerBoundsSize + outerBoundsSize;
			for (int i = -num; i <= num; i++)
			{
				int num2 = (int)currentPoint.X + i;
				for (int j = -num; j <= num; j++)
				{
					int num3 = (int)currentPoint.Y + j;
					Tile tile = Main.tile[num2, num3];
					if (Math.Abs(i) <= innerBoundsSize && Math.Abs(j) <= innerBoundsSize)
					{
						if (crackedBricks)
						{
							if ((tile.active() || !DungeonUtils.IsConsideredDungeonWall((int)tile.wall, false)) && num3 < Main.UnderworldLayer)
							{
								tile.ClearTile();
								tile.wall = 0;
								DungeonUtils.ChangeWallType(tile, wallType, false, this.settings.OverridePaintWall);
								DungeonUtils.ChangeTileType(tile, tileCrackedType, false, this.settings.OverridePaintTile);
							}
						}
						else
						{
							tile.ClearTile();
							DungeonUtils.ChangeWallType(tile, wallType, false, this.settings.OverridePaintWall);
						}
					}
					else if ((tile.active() && !DungeonUtils.IsHigherOrEqualTieredDungeonTile(data, (int)tile.type, (int)tileType)) || !DungeonUtils.IsConsideredDungeonWall((int)tile.wall, false))
					{
						tile.ClearTile();
						tile.wall = 0;
						DungeonUtils.ChangeTileType(tile, tileType, false, this.settings.OverridePaintTile);
						if (i > -num && i < num && j > -num && j < num)
						{
							DungeonUtils.ChangeWallType(tile, wallType, false, this.settings.OverridePaintWall);
						}
					}
					tile.liquid = 0;
				}
			}
		}

		// Token: 0x040059FF RID: 23039
		public DungeonRoomSettings settings;

		// Token: 0x04005A00 RID: 23040
		public bool calculated;

		// Token: 0x04005A01 RID: 23041
		public bool generated;

		// Token: 0x04005A02 RID: 23042
		public DungeonBounds InnerBounds = new DungeonBounds();

		// Token: 0x04005A03 RID: 23043
		public DungeonBounds OuterBounds = new DungeonBounds();
	}
}
