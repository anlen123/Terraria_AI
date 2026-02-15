using System;
using System.Collections.Generic;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Halls
{
	// Token: 0x020004C8 RID: 1224
	public class LegacyDungeonHall : DungeonHall
	{
		// Token: 0x060034AE RID: 13486 RVA: 0x00605D01 File Offset: 0x00603F01
		public LegacyDungeonHall(DungeonHallSettings settings) : base(settings)
		{
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x006073BC File Offset: 0x006055BC
		public override void CalculatePlatformsAndDoors(DungeonData data)
		{
			if (!base.Processed)
			{
				return;
			}
			DungeonUtils.CalculatePlatformAndDoorsOnHallway(data, this.StartPosition, this.StartDirection.Y, this.settings.ForceStyleForDoorsAndPlatforms ? this.settings.StyleData : null, 0.1);
			DungeonUtils.CalculatePlatformAndDoorsOnHallway(data, this.EndPosition, this.EndDirection.Y, this.settings.ForceStyleForDoorsAndPlatforms ? this.settings.StyleData : null, 0.1);
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x00607448 File Offset: 0x00605648
		public override void CalculateHall(DungeonData data, Vector2D startPoint, Vector2D endPoint)
		{
			this.calculated = false;
			this.OverrideStartPosition = startPoint;
			this.OverrideEndPosition = endPoint;
			this.LegacyHall(data, 0, 0, false);
			this.calculated = true;
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x00607470 File Offset: 0x00605670
		public override void GenerateHall(DungeonData data)
		{
			this.generated = false;
			this.LegacyHall(data, 0, 0, true);
			this.generated = true;
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x0060748A File Offset: 0x0060568A
		public bool GenerateHall(DungeonData data, int x, int y)
		{
			this.generated = false;
			this.LegacyHall(data, x, y, true);
			this.generated = true;
			return true;
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x006074A8 File Offset: 0x006056A8
		public virtual void LegacyHall(DungeonData dungeonData, int i, int j, bool generating = false)
		{
			LegacyDungeonHallSettings legacyDungeonHallSettings = (LegacyDungeonHallSettings)this.settings;
			UnifiedRandom unifiedRandom = new UnifiedRandom(legacyDungeonHallSettings.RandomSeed);
			ushort brickTileType = this.settings.StyleData.BrickTileType;
			ushort brickCrackedTileType = this.settings.StyleData.BrickCrackedTileType;
			ushort brickWallType = this.settings.StyleData.BrickWallType;
			Vector2D vector2D;
			vector2D..ctor((double)i, (double)j);
			Vector2D startPosition = vector2D;
			Vector2D vector2D2 = Vector2D.Zero;
			int num = (int)(4.0 * dungeonData.hallStrengthScalar) + unifiedRandom.Next(2);
			Vector2D zero = Vector2D.Zero;
			Vector2D zero2 = Vector2D.Zero;
			double hallStepScalar = dungeonData.hallStepScalar;
			int k = (int)(35.0 * hallStepScalar) + unifiedRandom.Next(45);
			bool flag = false;
			if (legacyDungeonHallSettings.CrackedBrickChance > 0.0)
			{
				flag = (unifiedRandom.NextDouble() <= legacyDungeonHallSettings.CrackedBrickChance);
			}
			if (legacyDungeonHallSettings.ForceHorizontal)
			{
				k += (int)(20.0 * hallStepScalar);
				dungeonData.lastDungeonHall = Vector2D.Zero;
			}
			else
			{
				if (unifiedRandom.Next(5) == 0)
				{
					num *= 2;
					k /= 2;
				}
				if (WorldGen.SecretSeed.errorWorld.Enabled && unifiedRandom.Next(2) == 0)
				{
					num *= 2;
				}
				if (WorldGen.SecretSeed.errorWorld.Enabled && unifiedRandom.Next(2) == 0)
				{
					k *= 2;
				}
			}
			Vector2D lastHall = dungeonData.lastDungeonHall;
			if (this.calculated)
			{
				startPosition = (vector2D = this.StartPosition);
				vector2D2 = (this.EndPosition - this.StartPosition).SafeNormalize(Vector2D.UnitX);
				num = this.Strength;
				k = this.Steps;
				lastHall = this.LastHall;
			}
			int steps = k;
			int num2 = num;
			double num3 = dungeonData.hallInteriorToExteriorRatio;
			if ((float)legacyDungeonHallSettings.OverrideStrength > 0f)
			{
				num2 = (num = legacyDungeonHallSettings.OverrideStrength);
			}
			if (legacyDungeonHallSettings.OverrideSteps > 0)
			{
				steps = (k = legacyDungeonHallSettings.OverrideSteps);
			}
			if (legacyDungeonHallSettings.OverrideInteriorToExteriorRatio > 0.0)
			{
				num3 = legacyDungeonHallSettings.OverrideInteriorToExteriorRatio;
			}
			bool flag2 = false;
			int num4 = Main.UnderworldLayer - (int)(100.0 * ((dungeonData.HallSizeScalar > dungeonData.RoomSizeScalar) ? dungeonData.HallSizeScalar : dungeonData.RoomSizeScalar));
			bool flag3 = false;
			if (this.OverrideStartPosition != default(Vector2D) && this.OverrideEndPosition != default(Vector2D))
			{
				flag3 = true;
				Vector2D overrideStartPosition = this.OverrideStartPosition;
				Vector2D v = this.OverrideEndPosition - overrideStartPosition;
				Vector2D vector2D3 = v.SafeNormalize(Vector2D.UnitX);
				steps = (k = (int)Math.Ceiling(v.Length() / vector2D3.Length()));
				vector2D = overrideStartPosition;
				startPosition = vector2D;
				zero.X = vector2D3.X;
				zero.Y = vector2D3.Y;
				zero2.X = -vector2D3.X;
				zero2.Y = -vector2D3.Y;
				vector2D2 = vector2D3;
			}
			else
			{
				bool flag4 = false;
				bool flag5 = true;
				while (!flag4)
				{
					bool flag6 = false;
					int num9;
					if (flag5 && !legacyDungeonHallSettings.ForceHorizontal)
					{
						bool flag7 = true;
						bool flag8 = true;
						bool flag9 = true;
						bool flag10 = true;
						bool flag11 = false;
						int num5 = k;
						bool flag12 = false;
						for (int l = j; l > j - num5; l--)
						{
							if (!WorldGen.InWorld(i, l, 50))
							{
								flag7 = false;
								break;
							}
							if (DungeonUtils.IsConsideredDungeonWall((int)Main.tile[i, l].wall, false))
							{
								if (flag12)
								{
									flag7 = false;
									break;
								}
							}
							else
							{
								flag12 = true;
							}
						}
						flag12 = false;
						for (int m = j; m < j + num5; m++)
						{
							if (!WorldGen.InWorld(i, m, 50))
							{
								flag8 = false;
								break;
							}
							if (m >= num4)
							{
								flag11 = true;
								flag8 = false;
								break;
							}
							if (DungeonUtils.IsConsideredDungeonWall((int)Main.tile[i, m].wall, false))
							{
								if (flag12)
								{
									flag8 = false;
									break;
								}
							}
							else
							{
								flag12 = true;
							}
						}
						flag12 = false;
						for (int n = i; n > i - num5; n--)
						{
							if (!WorldGen.InWorld(n, j, 50))
							{
								flag9 = false;
								break;
							}
							if (DungeonUtils.IsConsideredDungeonWall((int)Main.tile[n, j].wall, false))
							{
								if (flag12)
								{
									flag9 = false;
									break;
								}
							}
							else
							{
								flag12 = true;
							}
						}
						flag12 = false;
						for (int num6 = i; num6 < i + num5; num6++)
						{
							if (!WorldGen.InWorld(num6, j, 50))
							{
								flag10 = false;
								break;
							}
							if (DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num6, j].wall, false))
							{
								if (flag12)
								{
									flag10 = false;
									break;
								}
							}
							else
							{
								flag12 = true;
							}
						}
						if (flag9 || flag10 || flag7 || flag8)
						{
							int num7 = 100;
							int num8;
							do
							{
								num7--;
								if (num7 <= 0)
								{
									goto Block_39;
								}
								num8 = unifiedRandom.Next(4);
								if (num8 == 1 && flag11)
								{
									num8 = ((unifiedRandom.Next(2) == 0) ? 2 : 3);
								}
							}
							while ((num8 != 0 || !flag7) && (num8 != 1 || !flag8) && (num8 != 2 || !flag9) && (num8 != 3 || !flag10));
							IL_529:
							if (num8 == 0)
							{
								num9 = -1;
								goto IL_58A;
							}
							if (num8 == 1)
							{
								num9 = 1;
								goto IL_58A;
							}
							flag6 = true;
							if (num8 == 2)
							{
								num9 = -1;
								goto IL_58A;
							}
							num9 = 1;
							goto IL_58A;
							Block_39:
							num8 = 0;
							goto IL_529;
						}
						if (unifiedRandom.Next(2) == 0)
						{
							num9 = -1;
						}
						else
						{
							num9 = 1;
						}
						if (unifiedRandom.Next(2) == 0)
						{
							flag6 = true;
						}
						if (num9 == 1 && !flag6 && flag11)
						{
							num9 = ((unifiedRandom.Next(2) == 0) ? 1 : -1);
							flag6 = true;
						}
					}
					else
					{
						if (unifiedRandom.Next(2) == 0)
						{
							num9 = -1;
						}
						else
						{
							num9 = 1;
						}
						if (unifiedRandom.Next(2) == 0)
						{
							flag6 = true;
						}
						if (num9 == 1 && j + k >= num4)
						{
							num9 = ((unifiedRandom.Next(2) == 0) ? -1 : 1);
							flag6 = true;
						}
					}
					IL_58A:
					flag5 = false;
					if (legacyDungeonHallSettings.ForceHorizontal)
					{
						flag6 = true;
					}
					if (flag6)
					{
						zero.Y = 0.0;
						zero.X = (double)num9;
						zero2.Y = 0.0;
						zero2.X = (double)(-(double)num9);
						vector2D2.Y = 0.0;
						vector2D2.X = (double)num9;
						if (unifiedRandom.Next(3) == 0)
						{
							if (unifiedRandom.Next(2) == 0)
							{
								vector2D2.Y = -0.20000000298023224 * dungeonData.hallSlantVariantScalar;
							}
							else
							{
								vector2D2.Y = 0.20000000298023224 * dungeonData.hallSlantVariantScalar;
							}
						}
					}
					else
					{
						num++;
						vector2D2.Y = (double)num9;
						vector2D2.X = 0.0;
						zero.X = 0.0;
						zero.Y = (double)num9;
						zero2.X = 0.0;
						zero2.Y = (double)(-(double)num9);
						if (legacyDungeonHallSettings.ZigzagChance > 0.0 && unifiedRandom.NextDouble() <= legacyDungeonHallSettings.ZigzagChance)
						{
							flag2 = true;
							if (unifiedRandom.Next(2) == 0)
							{
								vector2D2.X = (double)unifiedRandom.Next(10, 20) * 0.1 * dungeonData.hallSlantVariantScalar;
							}
							else
							{
								vector2D2.X = (double)(-(double)unifiedRandom.Next(10, 20)) * 0.1 * dungeonData.hallSlantVariantScalar;
							}
						}
						else if (unifiedRandom.Next(2) == 0)
						{
							if (unifiedRandom.Next(2) == 0)
							{
								vector2D2.X = (double)unifiedRandom.Next(20, 40) * 0.01 * dungeonData.hallSlantVariantScalar;
							}
							else
							{
								vector2D2.X = (double)(-(double)unifiedRandom.Next(20, 40)) * 0.01 * dungeonData.hallSlantVariantScalar;
							}
						}
						else
						{
							k /= 2;
						}
					}
					if (dungeonData.lastDungeonHall != zero2)
					{
						flag4 = true;
					}
				}
			}
			int num10 = 0;
			float num11 = (float)Main.maxTilesX * 0.25f;
			float num12 = (float)Main.maxTilesX * 0.75f;
			if (WorldGen.SecretSeed.errorWorld.Enabled)
			{
				num11 = (float)Main.maxTilesX * 0.4f;
				num12 = (float)Main.maxTilesX * 0.6f;
			}
			bool flag13 = vector2D.Y < Main.rockLayer + 100.0;
			if (WorldGen.remixWorldGen)
			{
				flag13 = (vector2D.Y < Main.worldSurface + 100.0);
			}
			bool flag14 = vector2D.X < (double)(Main.maxTilesX / 2) && vector2D.X > (double)num11;
			bool flag15 = vector2D.X > (double)(Main.maxTilesX / 2) && vector2D.X < (double)num12;
			if (!flag3 && !legacyDungeonHallSettings.ForceHorizontal)
			{
				if (vector2D.X > (double)(Main.maxTilesX - 200))
				{
					int num9 = -1;
					zero.X = (double)num9;
					zero.Y = 0.0;
					vector2D2.X = (double)num9;
					vector2D2.Y = 0.0;
					if (unifiedRandom.Next(3) == 0)
					{
						if (unifiedRandom.Next(2) == 0)
						{
							vector2D2.Y = -0.20000000298023224 * dungeonData.hallSlantVariantScalar;
						}
						else
						{
							vector2D2.Y = 0.20000000298023224 * dungeonData.hallSlantVariantScalar;
						}
					}
				}
				else if (vector2D.X < 200.0)
				{
					int num9 = 1;
					zero.X = (double)num9;
					zero.Y = 0.0;
					vector2D2.X = (double)num9;
					vector2D2.Y = 0.0;
					if (unifiedRandom.Next(3) == 0)
					{
						if (unifiedRandom.Next(2) == 0)
						{
							vector2D2.Y = -0.20000000298023224 * dungeonData.hallSlantVariantScalar;
						}
						else
						{
							vector2D2.Y = 0.20000000298023224 * dungeonData.hallSlantVariantScalar;
						}
					}
				}
				else if (vector2D.Y >= (double)num4)
				{
					int num9 = -1;
					num++;
					zero.X = 0.0;
					zero.Y = (double)num9;
					vector2D2.X = 0.0;
					vector2D2.Y = (double)num9;
					if (unifiedRandom.Next(2) == 0)
					{
						if (unifiedRandom.Next(2) == 0)
						{
							vector2D2.X = (double)((float)unifiedRandom.Next(20, 50) * 0.01f) * dungeonData.hallSlantVariantScalar;
						}
						else
						{
							vector2D2.X = (double)((float)(-(float)unifiedRandom.Next(20, 50)) * 0.01f) * dungeonData.hallSlantVariantScalar;
						}
					}
				}
				else if (vector2D.Y < 200.0)
				{
					int num9 = 1;
					num++;
					zero.X = 0.0;
					zero.Y = (double)num9;
					vector2D2.X = 0.0;
					vector2D2.Y = (double)num9;
					if (unifiedRandom.Next(2) == 0)
					{
						if (unifiedRandom.Next(2) == 0)
						{
							vector2D2.X = (double)((float)unifiedRandom.Next(20, 50) * 0.01f) * dungeonData.hallSlantVariantScalar;
						}
						else
						{
							vector2D2.X = (double)((float)(-(float)unifiedRandom.Next(20, 50)) * 0.01f) * dungeonData.hallSlantVariantScalar;
						}
					}
				}
				else if (!flag3)
				{
					if (flag13)
					{
						int num9 = 1;
						num++;
						zero.X = 0.0;
						zero.Y = (double)num9;
						vector2D2.X = 0.0;
						vector2D2.Y = (double)num9;
						if (legacyDungeonHallSettings.ZigzagChance > 0.0 && unifiedRandom.NextDouble() <= legacyDungeonHallSettings.ZigzagChance)
						{
							flag2 = true;
							if (unifiedRandom.Next(2) == 0)
							{
								vector2D2.X = (double)unifiedRandom.Next(10, 20) * 0.1 * dungeonData.hallSlantVariantScalar;
							}
							else
							{
								vector2D2.X = (double)(-(double)unifiedRandom.Next(10, 20)) * 0.1 * dungeonData.hallSlantVariantScalar;
							}
						}
						else if (unifiedRandom.Next(2) == 0)
						{
							if (unifiedRandom.Next(2) == 0)
							{
								vector2D2.X = (double)unifiedRandom.Next(20, 50) * 0.01 * dungeonData.hallSlantVariantScalar;
							}
							else
							{
								vector2D2.X = (double)unifiedRandom.Next(20, 50) * 0.01 * dungeonData.hallSlantVariantScalar;
							}
						}
					}
					else if (flag14)
					{
						int num9 = -1;
						zero.Y = 0.0;
						zero.X = (double)num9;
						vector2D2.Y = 0.0;
						vector2D2.X = (double)num9;
						if (unifiedRandom.Next(3) == 0)
						{
							if (unifiedRandom.Next(2) == 0)
							{
								vector2D2.Y = -0.20000000298023224 * dungeonData.hallSlantVariantScalar;
							}
							else
							{
								vector2D2.Y = 0.20000000298023224 * dungeonData.hallSlantVariantScalar;
							}
						}
					}
					else if (flag15)
					{
						int num9 = 1;
						zero.Y = 0.0;
						zero.X = (double)num9;
						vector2D2.Y = 0.0;
						vector2D2.X = (double)num9;
						if (unifiedRandom.Next(3) == 0)
						{
							if (unifiedRandom.Next(2) == 0)
							{
								vector2D2.Y = -0.20000000298023224 * dungeonData.hallSlantVariantScalar;
							}
							else
							{
								vector2D2.Y = 0.20000000298023224 * dungeonData.hallSlantVariantScalar;
							}
						}
					}
				}
			}
			Vector2D startDirection = zero;
			dungeonData.lastDungeonHall = zero;
			if (!this.calculated && !flag3 && Math.Abs(vector2D2.X) > Math.Abs(vector2D2.Y) && unifiedRandom.Next(3) != 0)
			{
				num = (int)((float)num2 * ((float)unifiedRandom.Next(110, 150) * 0.01f));
			}
			if (!base.Processed)
			{
				this.Bounds.SetBounds((int)vector2D.X, (int)vector2D.Y, (int)vector2D.X, (int)vector2D.Y);
			}
			Vector2D startPos = vector2D;
			Vector2D endPos = vector2D + vector2D2 * (double)k;
			DungeonRoomSearchSettings settings = new DungeonRoomSearchSettings
			{
				Fluff = k / 2 + num
			};
			List<DungeonRoom> allRoomsInSpots = DungeonUtils.GetAllRoomsInSpots(dungeonData.dungeonRooms, startPos, endPos, settings);
			while (k > 0)
			{
				num10++;
				if (flag3)
				{
					if (!WorldGen.InWorld((int)(vector2D.X + zero.X), (int)(vector2D.Y + zero.Y), 10))
					{
						k = 0;
					}
				}
				else if (zero.X > 0.0 && vector2D.X > (double)(Main.maxTilesX - 100))
				{
					k = 0;
				}
				else if (zero.X < 0.0 && vector2D.X < 100.0)
				{
					k = 0;
				}
				else if (zero.Y > 0.0 && vector2D.Y >= (double)num4)
				{
					k = 0;
				}
				else if (zero.Y < 0.0 && vector2D.Y < 100.0)
				{
					k = 0;
				}
				else if (WorldGen.remixWorldGen && zero.Y < 0.0 && vector2D.Y < (Main.rockLayer + Main.worldSurface) / 2.0)
				{
					k = 0;
				}
				else if (!WorldGen.remixWorldGen && zero.Y < 0.0 && vector2D.Y < Main.rockLayer + 50.0)
				{
					k = 0;
				}
				k--;
				int num13 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(vector2D.X - (double)num - 4.0 - (double)unifiedRandom.Next(6))));
				int num14 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(vector2D.X + (double)num + 4.0 + (double)unifiedRandom.Next(6))));
				int num15 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(vector2D.Y - (double)num - 4.0 - (double)unifiedRandom.Next(6))));
				int num16 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(vector2D.Y + (double)num + 4.0 + (double)unifiedRandom.Next(6))));
				if (!base.Processed)
				{
					dungeonData.dungeonBounds.UpdateBounds(num13, num15, num14, num16);
					this.Bounds.UpdateBounds(num13, num15, num14, num16);
				}
				if (generating && !this.settings.CarveOnly)
				{
					for (int num17 = num13; num17 < num14; num17++)
					{
						for (int num18 = num15; num18 < num16; num18++)
						{
							bool flag16 = true;
							ProtectionType highestProtectionTypeFromPoint = DungeonUtils.GetHighestProtectionTypeFromPoint(num17, num18, allRoomsInSpots);
							if (highestProtectionTypeFromPoint != ProtectionType.TilesAndWalls)
							{
								if (highestProtectionTypeFromPoint == ProtectionType.Tiles)
								{
									flag16 = false;
								}
								Tile tile = Main.tile[num17, num18];
								tile.liquid = 0;
								if (flag16 && num18 <= Main.UnderworldLayer + 7 && this.CanPlaceTileAt(dungeonData, tile, (int)brickTileType, (int)brickCrackedTileType))
								{
									DungeonUtils.ChangeTileType(tile, brickTileType, true, this.settings.OverridePaintTile);
								}
							}
						}
					}
					for (int num19 = num13 + 1; num19 < num14 - 1; num19++)
					{
						for (int num20 = num15 + 1; num20 < num16 - 1; num20++)
						{
							if (num20 < Main.UnderworldLayer + 7)
							{
								bool flag17 = true;
								ProtectionType highestProtectionTypeFromPoint2 = DungeonUtils.GetHighestProtectionTypeFromPoint(num19, num20, allRoomsInSpots);
								if (highestProtectionTypeFromPoint2 != ProtectionType.TilesAndWalls)
								{
									if (highestProtectionTypeFromPoint2 == ProtectionType.Walls && DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num19, num20].wall, false))
									{
										flag17 = false;
									}
									if (flag17)
									{
										DungeonUtils.ChangeWallType(Main.tile[num19, num20], brickWallType, false, this.settings.OverridePaintWall);
									}
								}
							}
						}
					}
				}
				if (generating)
				{
					int num21 = 0;
					if (vector2D2.Y == 0.0 && unifiedRandom.Next(num + 1) == 0)
					{
						num21 = unifiedRandom.Next(1, 3);
					}
					else if (vector2D2.X == 0.0 && unifiedRandom.Next(num - 1) == 0)
					{
						num21 = unifiedRandom.Next(1, 3);
					}
					else if (unifiedRandom.Next(num * 3) == 0)
					{
						num21 = unifiedRandom.Next(1, 3);
					}
					num13 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(vector2D.X - (double)num * num3 - (double)num21)));
					num14 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(vector2D.X + (double)num * num3 + (double)num21)));
					num15 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(vector2D.Y - (double)num * num3 - (double)num21)));
					num16 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(vector2D.Y + (double)num * num3 + (double)num21)));
					for (int num22 = num13; num22 < num14; num22++)
					{
						for (int num23 = num15; num23 < num16; num23++)
						{
							bool flag18 = true;
							bool flag19 = true;
							ProtectionType highestProtectionTypeFromPoint3 = DungeonUtils.GetHighestProtectionTypeFromPoint(num22, num23, allRoomsInSpots);
							if (highestProtectionTypeFromPoint3 != ProtectionType.TilesAndWalls)
							{
								if (highestProtectionTypeFromPoint3 == ProtectionType.Tiles)
								{
									flag18 = false;
								}
								if (highestProtectionTypeFromPoint3 == ProtectionType.Walls && DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num22, num23].wall, false))
								{
									flag19 = false;
								}
								if (this.CanRemoveTileAt(dungeonData, Main.tile[num22, num23], (int)brickCrackedTileType))
								{
									if (flag)
									{
										if ((Main.tile[num22, num23].active() || !DungeonUtils.IsConsideredDungeonWall((int)Main.tile[num22, num23].wall, false)) && num23 < Main.UnderworldLayer)
										{
											if (this.settings.CarveOnly)
											{
												Main.tile[num22, num23].ClearTile();
											}
											else
											{
												Main.tile[num22, num23].ClearTile();
												if (flag18)
												{
													DungeonUtils.ChangeTileType(Main.tile[num22, num23], brickCrackedTileType, false, this.settings.OverridePaintTile);
												}
											}
										}
									}
									else
									{
										Main.tile[num22, num23].ClearTile();
									}
									if (flag19 && num23 < Main.UnderworldLayer && !this.settings.CarveOnly)
									{
										DungeonUtils.ChangeWallType(Main.tile[num22, num23], brickWallType, false, this.settings.OverridePaintWall);
									}
								}
							}
						}
					}
				}
				vector2D += vector2D2;
				if (!flag3 && flag2 && num10 > unifiedRandom.Next(10, 20))
				{
					num10 = 0;
					vector2D2.X *= -1.0;
				}
			}
			dungeonData.genVars.generatingDungeonPositionX = (int)vector2D.X;
			dungeonData.genVars.generatingDungeonPositionY = (int)vector2D.Y;
			this.StartPosition = startPosition;
			this.EndPosition = vector2D;
			this.StartDirection = startDirection;
			this.EndDirection = zero;
			this.Strength = num2;
			this.Steps = steps;
			this.LastHall = lastHall;
			this.CrackedBrick = flag;
			if (!base.Processed)
			{
				this.Bounds.CalculateHitbox();
			}
		}

		// Token: 0x04005A38 RID: 23096
		public Vector2D LastHall;

		// Token: 0x04005A39 RID: 23097
		public int Strength;

		// Token: 0x04005A3A RID: 23098
		public int Steps;

		// Token: 0x04005A3B RID: 23099
		protected Vector2D OverrideStartPosition;

		// Token: 0x04005A3C RID: 23100
		protected Vector2D OverrideEndPosition;
	}
}
