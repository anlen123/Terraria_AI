using System;
using System.Collections.Generic;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Rooms;

namespace Terraria.GameContent.Generation.Dungeon.Halls
{
	// Token: 0x020004C4 RID: 1220
	public abstract class DungeonHall
	{
		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x0600349F RID: 13471 RVA: 0x006065C1 File Offset: 0x006047C1
		public bool Processed
		{
			get
			{
				return this.calculated || this.generated;
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x060034A0 RID: 13472 RVA: 0x006065D3 File Offset: 0x006047D3
		public Vector2D CenterPosition
		{
			get
			{
				return (this.StartPosition + this.EndPosition) / 2.0;
			}
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x006065F4 File Offset: 0x006047F4
		public DungeonHall(DungeonHallSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x060034A2 RID: 13474
		public abstract void CalculateHall(DungeonData data, Vector2D startPoint, Vector2D endPoint);

		// Token: 0x060034A3 RID: 13475
		public abstract void CalculatePlatformsAndDoors(DungeonData data);

		// Token: 0x060034A4 RID: 13476
		public abstract void GenerateHall(DungeonData data);

		// Token: 0x060034A5 RID: 13477 RVA: 0x001FC399 File Offset: 0x001FA599
		public virtual int GetFurnitureCount(int defaultCount)
		{
			return defaultCount;
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x00606610 File Offset: 0x00604810
		public void GenerateDungeonSquareHall(DungeonData data, List<DungeonRoom> roomsInArea, Vector2D currentPoint, ushort tileType, ushort tileCrackedType, ushort wallType, int innerBoundsSize, int outerBoundsSize, bool placeOverProtectedBricks = false, bool crackedBricks = false, bool clearPaintFirst = false)
		{
			int num = innerBoundsSize + outerBoundsSize;
			for (int i = -num; i <= num; i++)
			{
				int num2 = (int)currentPoint.X + i;
				for (int j = -num; j <= num; j++)
				{
					int num3 = (int)currentPoint.Y + j;
					bool flag = true;
					bool flag2 = true;
					ProtectionType highestProtectionTypeFromPoint = DungeonUtils.GetHighestProtectionTypeFromPoint(num2, num3, roomsInArea);
					if (highestProtectionTypeFromPoint != ProtectionType.TilesAndWalls)
					{
						if (highestProtectionTypeFromPoint == ProtectionType.Tiles)
						{
							flag = false;
						}
						if (highestProtectionTypeFromPoint == ProtectionType.Walls && DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num2, num3].wall, false))
						{
							flag2 = false;
						}
						Tile tile = Main.tile[num2, num3];
						if (Math.Abs(i) <= innerBoundsSize && Math.Abs(j) <= innerBoundsSize)
						{
							if (!this.CanRemoveTileAt(data, tile, (int)tileCrackedType))
							{
								goto IL_23B;
							}
							if (crackedBricks)
							{
								if ((tile.active() || !DungeonUtils.IsConsideredDungeonWall((int)tile.wall, false)) && num3 < Main.UnderworldLayer)
								{
									if (this.settings.CarveOnly)
									{
										tile.ClearTile();
									}
									else
									{
										if (flag)
										{
											tile.ClearTile();
										}
										if (flag2)
										{
											tile.wall = 0;
										}
										if (flag2)
										{
											if (clearPaintFirst)
											{
												WorldGen.paintWall(num2, num3, 0, false, false);
											}
											DungeonUtils.ChangeWallType(tile, wallType, false, this.settings.OverridePaintWall);
										}
										if (flag)
										{
											if (clearPaintFirst)
											{
												WorldGen.paintTile(num2, num3, 0, false, false);
											}
											DungeonUtils.ChangeTileType(tile, tileCrackedType, false, this.settings.OverridePaintTile);
										}
									}
								}
							}
							else
							{
								tile.ClearTile();
								if (!this.settings.CarveOnly && flag2)
								{
									if (clearPaintFirst)
									{
										WorldGen.paintWall(num2, num3, 0, false, false);
									}
									DungeonUtils.ChangeWallType(tile, wallType, false, this.settings.OverridePaintWall);
								}
							}
						}
						else if (this.CanPlaceTileAt(data, tile, (int)tileType, (int)tileCrackedType))
						{
							if (flag)
							{
								tile.ClearTile();
							}
							if (flag2)
							{
								tile.wall = 0;
							}
							if (flag)
							{
								if (clearPaintFirst)
								{
									WorldGen.paintTile(num2, num3, 0, false, false);
								}
								DungeonUtils.ChangeTileType(tile, tileType, false, this.settings.OverridePaintTile);
							}
							if (flag2 && i > -num && i < num && j > -num && j < num)
							{
								if (clearPaintFirst)
								{
									WorldGen.paintWall(num2, num3, 0, false, false);
								}
								DungeonUtils.ChangeWallType(tile, wallType, false, this.settings.OverridePaintWall);
							}
						}
						tile.liquid = 0;
					}
					IL_23B:;
				}
			}
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x00606870 File Offset: 0x00604A70
		public virtual bool CanPlaceTileAt(DungeonData data, Tile tile, int tileType, int tileCrackedType)
		{
			return !this.settings.CarveOnly && (!DungeonUtils.IsConsideredDungeonWall((int)tile.wall, false) || (tile.active() && !DungeonUtils.IsHigherOrEqualTieredDungeonTile(data, (int)tile.type, tileType) && (int)tile.type != tileCrackedType));
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x006068C4 File Offset: 0x00604AC4
		public virtual bool CanRemoveTileAt(DungeonData data, Tile tile, int tileCrackedType)
		{
			return !tile.active() || data.Type != DungeonType.DualDungeon || (int)tile.type != tileCrackedType;
		}

		// Token: 0x04005A2D RID: 23085
		public DungeonHallSettings settings;

		// Token: 0x04005A2E RID: 23086
		public bool calculated;

		// Token: 0x04005A2F RID: 23087
		public bool generated;

		// Token: 0x04005A30 RID: 23088
		public DungeonBounds Bounds = new DungeonBounds();

		// Token: 0x04005A31 RID: 23089
		public Vector2D StartPosition;

		// Token: 0x04005A32 RID: 23090
		public Vector2D EndPosition;

		// Token: 0x04005A33 RID: 23091
		public Vector2D StartDirection;

		// Token: 0x04005A34 RID: 23092
		public Vector2D EndDirection;

		// Token: 0x04005A35 RID: 23093
		public bool CrackedBrick;
	}
}
