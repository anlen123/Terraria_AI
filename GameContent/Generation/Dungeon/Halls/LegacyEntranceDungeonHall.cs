using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Halls
{
	// Token: 0x020004C6 RID: 1222
	public class LegacyEntranceDungeonHall : LegacyDungeonHall
	{
		// Token: 0x060034AA RID: 13482 RVA: 0x006068ED File Offset: 0x00604AED
		public LegacyEntranceDungeonHall(DungeonHallSettings settings) : base(settings)
		{
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x00009E06 File Offset: 0x00008006
		public override void CalculatePlatformsAndDoors(DungeonData data)
		{
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x006068F8 File Offset: 0x00604AF8
		public override void LegacyHall(DungeonData dungeonData, int i, int j, bool generating = false)
		{
			LegacyEntranceDungeonHallSettings legacyEntranceDungeonHallSettings = (LegacyEntranceDungeonHallSettings)this.settings;
			UnifiedRandom unifiedRandom = new UnifiedRandom(legacyEntranceDungeonHallSettings.RandomSeed);
			ushort brickTileType = this.settings.StyleData.BrickTileType;
			ushort brickCrackedTileType = this.settings.StyleData.BrickCrackedTileType;
			ushort brickWallType = this.settings.StyleData.BrickWallType;
			Vector2D vector2D = Vector2D.Zero;
			Vector2D vector2D2 = Vector2D.Zero;
			int num = Main.maxTilesX / 2;
			int num2 = unifiedRandom.Next(5, 9);
			vector2D.X = (double)i;
			vector2D.Y = (double)j;
			Vector2D startPosition = vector2D;
			int k = unifiedRandom.Next(10, 30);
			int num3 = (i > dungeonData.genVars.generatingDungeonTopX) ? -1 : 1;
			if (i > Main.maxTilesX - 400)
			{
				num3 = -1;
			}
			else if (i < 400)
			{
				num3 = 1;
			}
			vector2D2.Y = -1.0;
			vector2D2.X = (double)num3;
			if (unifiedRandom.Next(3) != 0)
			{
				vector2D2.X *= (double)(1f + (float)unifiedRandom.Next(0, 200) * 0.01f);
			}
			else if (unifiedRandom.Next(3) == 0)
			{
				vector2D2.X *= (double)((float)unifiedRandom.Next(50, 76) * 0.01f);
			}
			else if (unifiedRandom.Next(6) == 0)
			{
				vector2D2.Y *= 2.0;
			}
			if (dungeonData.useSkewedDungeonEntranceHalls)
			{
				if (dungeonData.genVars.generatingDungeonPositionX < num && vector2D2.X < 0.0 && vector2D2.X < -0.5)
				{
					vector2D2.X = 0.5;
				}
				if (dungeonData.genVars.generatingDungeonPositionX > num && vector2D2.X > 0.0 && vector2D2.X > 0.5)
				{
					vector2D2.X = -0.5;
				}
			}
			else
			{
				if (dungeonData.genVars.generatingDungeonPositionX < num && vector2D2.X < -0.5)
				{
					vector2D2.X = -0.5;
				}
				if (dungeonData.genVars.generatingDungeonPositionX > num && vector2D2.X > 0.5)
				{
					vector2D2.X = 0.5;
				}
			}
			if (WorldGen.drunkWorldGen || WorldGen.SecretSeed.noSurface.Enabled)
			{
				num3 *= -1;
				vector2D2.X *= -1.0;
			}
			if (this.calculated)
			{
				startPosition = (vector2D = this.StartPosition);
				vector2D2 = (this.EndPosition - this.StartPosition).SafeNormalize(Vector2D.UnitX);
				num3 = this.Direction;
				num2 = this.Strength;
				k = this.Steps;
			}
			int strength = num2;
			int steps = k;
			double num4 = dungeonData.hallInteriorToExteriorRatio;
			if ((float)legacyEntranceDungeonHallSettings.OverrideStrength > 0f)
			{
				strength = (num2 = legacyEntranceDungeonHallSettings.OverrideStrength);
			}
			if (legacyEntranceDungeonHallSettings.OverrideSteps > 0)
			{
				steps = (k = legacyEntranceDungeonHallSettings.OverrideSteps);
			}
			if (legacyEntranceDungeonHallSettings.OverrideInteriorToExteriorRatio > 0.0)
			{
				num4 = legacyEntranceDungeonHallSettings.OverrideInteriorToExteriorRatio;
			}
			bool flag = false;
			if (this.OverrideStartPosition != default(Vector2D) && this.OverrideEndPosition != default(Vector2D))
			{
				flag = true;
				Vector2D overrideStartPosition = this.OverrideStartPosition;
				Vector2D v = this.OverrideEndPosition - overrideStartPosition;
				Vector2D vector2D3 = v.SafeNormalize(Vector2D.UnitX);
				steps = (k = (int)Math.Ceiling(v.Length() / vector2D3.Length()));
				startPosition = (vector2D = overrideStartPosition);
				vector2D2 = vector2D3;
				num3 = ((vector2D3.X > 0.0) ? 1 : -1);
			}
			this.Bounds.SetBounds((int)vector2D.X, (int)vector2D.Y, (int)vector2D.X, (int)vector2D.Y);
			Vector2D startPos = vector2D;
			Vector2D endPos = vector2D + vector2D2 * (double)k;
			DungeonRoomSearchSettings settings = new DungeonRoomSearchSettings
			{
				Fluff = k / 2 + num2
			};
			List<DungeonRoom> allRoomsInSpots = DungeonUtils.GetAllRoomsInSpots(dungeonData.dungeonRooms, startPos, endPos, settings);
			Vector2D vector2D4 = vector2D2;
			int num5 = 30;
			int num6 = 10;
			int num7 = 0;
			while (k > 0)
			{
				k--;
				if (!WorldGen.InWorld((int)vector2D.X, (int)vector2D.Y, num5 + 5))
				{
					break;
				}
				int num8 = Math.Max(num5, Math.Min(Main.maxTilesX - num5 - 1, (int)(vector2D.X - (double)num2 - 4.0 - (double)unifiedRandom.Next(6))));
				int num9 = Math.Max(num5, Math.Min(Main.maxTilesX - num5 - 1, (int)(vector2D.X + (double)num2 + 4.0 + (double)unifiedRandom.Next(6))));
				int num10 = Math.Max(num5, Math.Min(Main.maxTilesY - num5 - 1, (int)(vector2D.Y - (double)num2 - 4.0)));
				int num11 = Math.Max(num5, Math.Min(Main.maxTilesY - num5 - 1, (int)(vector2D.Y + (double)num2 + 4.0 + (double)unifiedRandom.Next(6))));
				if (!base.Processed)
				{
					dungeonData.dungeonBounds.UpdateBounds(num8, num10, num9, num11);
					this.Bounds.UpdateBounds(num8, num10, num9, num11);
				}
				int num12 = 1;
				if (vector2D.X > (double)num)
				{
					num12 = -1;
				}
				int num13 = (int)(vector2D.X + dungeonData.dungeonEntranceStrengthX * 0.6 * (double)num12 + dungeonData.dungeonEntranceStrengthX2 * (double)num12);
				int num14 = (int)(dungeonData.dungeonEntranceStrengthY2 * 0.5);
				if (!legacyEntranceDungeonHallSettings.UsePrecalculatedEntrance && vector2D.Y < Main.worldSurface - 5.0 && ((Main.tile[num13, (int)(vector2D.Y - (double)num2 - 6.0 + (double)num14)].wall == 0 && Main.tile[num13, (int)(vector2D.Y - (double)num2 - 7.0 + (double)num14)].wall == 0 && Main.tile[num13, (int)(vector2D.Y - (double)num2 - 8.0 + (double)num14)].wall == 0) || WorldGen.SecretSeed.surfaceIsDesert.Enabled))
				{
					dungeonData.createdDungeonEntranceOnSurface = true;
					if (generating)
					{
						WorldGen.TileRunner(num13, (int)(vector2D.Y - (double)num2 - 6.0 + (double)num14), (double)unifiedRandom.Next(25, 35), unifiedRandom.Next(10, 20), -1, false, 0.0, -1.0, false, true, -1);
					}
				}
				if (generating && !this.settings.CarveOnly)
				{
					for (int l = num8; l < num9; l++)
					{
						for (int m = num10; m < num11; m++)
						{
							bool flag2 = true;
							ProtectionType highestProtectionTypeFromPoint = DungeonUtils.GetHighestProtectionTypeFromPoint(l, m, allRoomsInSpots);
							if (highestProtectionTypeFromPoint != ProtectionType.TilesAndWalls)
							{
								if (highestProtectionTypeFromPoint == ProtectionType.Tiles)
								{
									flag2 = false;
								}
								Tile tile = Main.tile[l, m];
								tile.liquid = 0;
								if (flag2 && this.CanPlaceTileAt(dungeonData, tile, (int)brickTileType, (int)brickCrackedTileType))
								{
									WorldGen.paintTile(l, m, 0, false, false);
									DungeonUtils.ChangeTileType(Main.tile[l, m], brickTileType, true, legacyEntranceDungeonHallSettings.OverridePaintTile);
								}
							}
						}
					}
					for (int n = num8 + 1; n < num9 - 1; n++)
					{
						for (int num15 = num10 + 1; num15 < num11 - 1; num15++)
						{
							bool flag3 = true;
							ProtectionType highestProtectionTypeFromPoint2 = DungeonUtils.GetHighestProtectionTypeFromPoint(n, num15, allRoomsInSpots);
							if (highestProtectionTypeFromPoint2 != ProtectionType.TilesAndWalls)
							{
								if (highestProtectionTypeFromPoint2 == ProtectionType.Walls && DungeonUtils.IsConsideredDungeonWall((int)Main.tile[n, num15].wall, false))
								{
									flag3 = false;
								}
								if (flag3)
								{
									WorldGen.paintWall(n, num15, 0, false, false);
									DungeonUtils.ChangeWallType(Main.tile[n, num15], brickWallType, false, legacyEntranceDungeonHallSettings.OverridePaintWall);
								}
							}
						}
					}
				}
				int num16 = 0;
				if (unifiedRandom.Next(num2) == 0)
				{
					num16 = unifiedRandom.Next(1, 3);
				}
				num8 = Math.Max(num5, Math.Min(Main.maxTilesX - num5 - 1, (int)(vector2D.X - (double)num2 * num4 - (double)num16)));
				num9 = Math.Max(num5, Math.Min(Main.maxTilesX - num5 - 1, (int)(vector2D.X + (double)num2 * num4 + (double)num16)));
				num10 = Math.Max(num5, Math.Min(Main.maxTilesY - num5 - 1, (int)(vector2D.Y - (double)num2 * num4 - (double)num16)));
				num11 = Math.Max(num5, Math.Min(Main.maxTilesY - num5 - 1, (int)(vector2D.Y + (double)num2 * num4 + (double)num16)));
				if (generating)
				{
					if (flag)
					{
						num7--;
						if (num7 <= 0)
						{
							num7 = num6;
							DungeonPlatformData item = new DungeonPlatformData
							{
								Position = new Point((int)vector2D.X, (int)vector2D.Y),
								PlacePotsChance = 0.25,
								InAHallway = true
							};
							dungeonData.dungeonPlatformData.Add(item);
						}
					}
					for (int num17 = num8; num17 < num9; num17++)
					{
						for (int num18 = num10; num18 < num11; num18++)
						{
							bool flag4 = true;
							ProtectionType highestProtectionTypeFromPoint3 = DungeonUtils.GetHighestProtectionTypeFromPoint(num17, num18, allRoomsInSpots);
							if (highestProtectionTypeFromPoint3 != ProtectionType.TilesAndWalls)
							{
								if (highestProtectionTypeFromPoint3 == ProtectionType.Walls && DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num17, num18].wall, false))
								{
									flag4 = false;
								}
								Main.tile[num17, num18].ClearTile();
								if (flag4 && !this.settings.CarveOnly)
								{
									DungeonUtils.ChangeWallType(Main.tile[num17, num18], brickWallType, false, legacyEntranceDungeonHallSettings.OverridePaintWall);
								}
							}
						}
					}
				}
				if (!legacyEntranceDungeonHallSettings.UsePrecalculatedEntrance && dungeonData.createdDungeonEntranceOnSurface)
				{
					k = 0;
				}
				vector2D += vector2D2;
				if (!flag && vector2D.Y < Main.worldSurface)
				{
					vector2D2.Y *= 0.9800000190734863;
				}
			}
			dungeonData.genVars.generatingDungeonPositionX = (int)vector2D.X;
			dungeonData.genVars.generatingDungeonPositionY = (int)vector2D.Y;
			this.StartPosition = startPosition;
			this.EndPosition = vector2D;
			this.StartDirection = new Vector2D(vector2D4.X, vector2D4.Y);
			this.EndDirection = new Vector2D(vector2D2.X, vector2D2.Y);
			this.Strength = strength;
			this.Steps = steps;
			this.Direction = num3;
			if (!base.Processed)
			{
				this.Bounds.CalculateHitbox();
			}
		}

		// Token: 0x04005A37 RID: 23095
		public int Direction;
	}
}
