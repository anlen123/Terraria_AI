using System;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.Rooms
{
	// Token: 0x020004B6 RID: 1206
	public class LegacyDungeonRoom : DungeonRoom
	{
		// Token: 0x06003467 RID: 13415 RVA: 0x0060368F File Offset: 0x0060188F
		public LegacyDungeonRoom(DungeonRoomSettings settings) : base(settings)
		{
		}

		// Token: 0x06003468 RID: 13416 RVA: 0x006036B0 File Offset: 0x006018B0
		public override void CalculateRoom(DungeonData data)
		{
			this.calculated = false;
			int x = this.settings.RoomPosition.X;
			int y = this.settings.RoomPosition.Y;
			this.LegacyRoom(data, x, y, false);
			this.calculated = true;
		}

		// Token: 0x06003469 RID: 13417 RVA: 0x006036F8 File Offset: 0x006018F8
		public override bool GenerateRoom(DungeonData data)
		{
			this.generated = false;
			int x = this.settings.RoomPosition.X;
			int y = this.settings.RoomPosition.Y;
			this.LegacyRoom(data, x, y, true);
			this.generated = true;
			return true;
		}

		// Token: 0x0600346A RID: 13418 RVA: 0x00603740 File Offset: 0x00601940
		public override int GetFloodedRoomTileCount()
		{
			return this._floodedTileCount;
		}

		// Token: 0x0600346B RID: 13419 RVA: 0x00603748 File Offset: 0x00601948
		public override void FloodRoom(byte liquidType)
		{
			if (!this.generated || this._innerShapeData == null)
			{
				return;
			}
			WorldUtils.Gen(this.StartPosition.ToPoint(), new ModShapes.All(this._innerShapeData), Actions.Chain(new GenAction[]
			{
				new Modifiers.IsBelowHeight(this.InnerBounds.Center.Y, true),
				new Modifiers.IsNotSolid(),
				new Actions.SetLiquid((int)liquidType, byte.MaxValue)
			}));
		}

		// Token: 0x0600346C RID: 13420 RVA: 0x006037BC File Offset: 0x006019BC
		public override ProtectionType GetProtectionTypeFromPoint(int x, int y)
		{
			if (this._innerShapeData == null || this._outerShapeData == null || (this.calculated && !this.OuterBounds.Contains(x, y)))
			{
				return base.GetProtectionTypeFromPoint(x, y);
			}
			if (!this._outerShapeData.Contains(x - (int)this.StartPosition.X, y - (int)this.StartPosition.Y))
			{
				return ProtectionType.None;
			}
			return ProtectionType.Walls;
		}

		// Token: 0x0600346D RID: 13421 RVA: 0x00603825 File Offset: 0x00601A25
		public override bool IsInsideRoom(int x, int y)
		{
			return base.IsInsideRoom(x, y) && this._innerShapeData.Contains(x - (int)this.StartPosition.X, y - (int)this.StartPosition.Y);
		}

		// Token: 0x0600346E RID: 13422 RVA: 0x0060385C File Offset: 0x00601A5C
		public override bool TryGenerateChestInRoom(DungeonData data, DungeonGlobalBasicChests feature)
		{
			Vector2D endPosition = this.EndPosition;
			int num = (int)((float)this.Strength * 0.4f);
			return DungeonUtils.GenerateDungeonRegularChest(data, feature, this.settings.StyleData, (int)endPosition.X - num, (int)endPosition.Y - num, (int)endPosition.X + num, (int)endPosition.Y + num);
		}

		// Token: 0x0600346F RID: 13423 RVA: 0x006038B8 File Offset: 0x00601AB8
		public override bool DualDungeons_TryGenerateBiomeChestInRoom(DungeonData data, DungeonGlobalBiomeChests feature)
		{
			Vector2D endPosition = this.EndPosition;
			int num = (int)((float)this.Strength * 0.4f);
			return DungeonUtils.GenerateDungeonBiomeChest(data, feature, this.settings.StyleData, (int)endPosition.X - num, (int)endPosition.Y - num, (int)endPosition.X + num, (int)endPosition.Y + num, true);
		}

		// Token: 0x06003470 RID: 13424 RVA: 0x00603914 File Offset: 0x00601B14
		public void LegacyRoom(DungeonData data, int i, int j, bool generating)
		{
			LegacyDungeonRoomSettings legacyDungeonRoomSettings = (LegacyDungeonRoomSettings)this.settings;
			UnifiedRandom unifiedRandom = new UnifiedRandom(legacyDungeonRoomSettings.RandomSeed);
			ushort brickTileType = this.settings.StyleData.BrickTileType;
			ushort brickWallType = this.settings.StyleData.BrickWallType;
			double num = data.roomStrengthScalar;
			if (legacyDungeonRoomSettings.StartingRoom)
			{
				num = 1.0;
			}
			double num2 = (double)((int)(15.0 * num) + unifiedRandom.Next(15));
			Vector2D vector2D;
			vector2D.X = (double)((float)unifiedRandom.Next(-10, 11) * 0.1f) * data.roomSlantVariantScalar;
			vector2D.Y = (double)((float)unifiedRandom.Next(-10, 11) * 0.1f) * data.roomSlantVariantScalar;
			if (vector2D.X == 0.0 && vector2D.Y == 0.0)
			{
				if (unifiedRandom.Next(2) == 0)
				{
					vector2D.X = (double)((unifiedRandom.Next(2) == 0) ? -1 : 1);
				}
				else
				{
					vector2D.Y = (double)((unifiedRandom.Next(2) == 0) ? -1 : 1);
				}
			}
			Vector2D vector2D2;
			vector2D2.X = (double)i;
			vector2D2.Y = (double)j - num2 / 2.0;
			if (this.calculated)
			{
				vector2D2 = this.StartPosition;
			}
			Vector2D vector2D3 = vector2D2;
			double num3 = data.roomStepScalar;
			if (legacyDungeonRoomSettings.StartingRoom)
			{
				num3 = 1.0;
			}
			int k = (int)(10.0 * num3) + unifiedRandom.Next(10);
			double num4 = num2;
			double num5 = data.roomInteriorToExteriorRatio;
			if (legacyDungeonRoomSettings.OverrideStartPosition != default(Vector2D) && legacyDungeonRoomSettings.OverrideEndPosition != default(Vector2D))
			{
				vector2D3 = (vector2D2 = legacyDungeonRoomSettings.OverrideStartPosition);
				Vector2D v = legacyDungeonRoomSettings.OverrideEndPosition - vector2D2;
				vector2D = v.SafeNormalize(Vector2D.UnitX);
				k = (int)Math.Ceiling(v.Length() / vector2D.Length());
			}
			else if (legacyDungeonRoomSettings.OverrideVelocity != default(Vector2D))
			{
				vector2D = legacyDungeonRoomSettings.OverrideVelocity;
			}
			if (legacyDungeonRoomSettings.OverrideStrength > 0)
			{
				num4 = (num2 = (double)legacyDungeonRoomSettings.OverrideStrength);
			}
			if (legacyDungeonRoomSettings.OverrideSteps > 0)
			{
				k = legacyDungeonRoomSettings.OverrideSteps;
			}
			if (legacyDungeonRoomSettings.OverrideInteriorToExteriorRatio > 0.0)
			{
				num5 = legacyDungeonRoomSettings.OverrideInteriorToExteriorRatio;
			}
			this.InnerBounds.SetBounds((int)vector2D2.X, (int)vector2D2.Y, (int)vector2D2.X, (int)vector2D2.Y);
			this.OuterBounds.SetBounds((int)vector2D2.X, (int)vector2D2.Y, (int)vector2D2.X, (int)vector2D2.Y);
			while (k > 0)
			{
				k--;
				int num6 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(vector2D2.X - num2 * 0.800000011920929 - 5.0)));
				int num7 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(vector2D2.X + num2 * 0.800000011920929 + 5.0)));
				int num8 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(vector2D2.Y - num2 * 0.800000011920929 - 5.0)));
				int num9 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(vector2D2.Y + num2 * 0.800000011920929 + 5.0)));
				if (legacyDungeonRoomSettings.IsEntranceRoom && data.Type == DungeonType.DualDungeon)
				{
					num9 = Math.Max(num9, DungeonUtils.GetDualDungeonBrickSupportCutoffY(data));
				}
				data.dungeonBounds.UpdateBounds(num6, num8, num7 - 1, num9 - 1);
				this.OuterBounds.UpdateBounds(num6, num8, num7 - 1, num9 - 1);
				int num10 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(vector2D2.X - num2 * num5)));
				int num11 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(vector2D2.X + num2 * num5)));
				int num12 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(vector2D2.Y - num2 * num5)));
				int num13 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(vector2D2.Y + num2 * num5)));
				this.InnerBounds.UpdateBounds(num10, num12, num11 - 1, num13 - 1);
				for (int l = num6; l < num7; l++)
				{
					for (int m = num8; m < num9; m++)
					{
						if (!generating)
						{
							this._outerShapeData.Add(l - (int)vector2D3.X, m - (int)vector2D3.Y);
							if (l >= num10 && l <= num11 && m >= num12 && m <= num13)
							{
								this._innerShapeData.Add(l - (int)vector2D3.X, m - (int)vector2D3.Y);
							}
						}
						else
						{
							Main.tile[l, m].liquid = 0;
							if (!DungeonUtils.IsHigherOrEqualTieredDungeonWall(data, (int)Main.tile[l, m].wall, (int)brickWallType))
							{
								DungeonUtils.ChangeTileType(Main.tile[l, m], brickTileType, true, legacyDungeonRoomSettings.OverridePaintTile);
							}
						}
					}
				}
				if (generating)
				{
					for (int n = num6 + 1; n < num7 - 1; n++)
					{
						for (int num14 = num8 + 1; num14 < num9 - 1; num14++)
						{
							DungeonUtils.ChangeWallType(Main.tile[n, num14], brickWallType, false, legacyDungeonRoomSettings.OverridePaintWall);
						}
					}
				}
				num6 = num10;
				num7 = num11;
				num8 = num12;
				num9 = num13;
				if (generating)
				{
					for (int num15 = num6; num15 < num7; num15++)
					{
						for (int num16 = num8; num16 < num9; num16++)
						{
							DungeonUtils.ChangeWallType(Main.tile[num15, num16], brickWallType, true, legacyDungeonRoomSettings.OverridePaintWall);
						}
					}
				}
				vector2D2 += vector2D;
				vector2D.X = Math.Max(-1.0, Math.Min(1.0, vector2D.X + (double)((float)unifiedRandom.Next(-10, 11) * 0.05f) * data.roomSlantVariantScalar));
				vector2D.Y = Math.Max(-1.0, Math.Min(1.0, vector2D.Y + (double)((float)unifiedRandom.Next(-10, 11) * 0.05f) * data.roomSlantVariantScalar));
			}
			this.StartPosition = vector2D3;
			this.EndPosition = vector2D2;
			this.Strength = (int)num4;
			this.InnerBounds.CalculateHitbox();
			this.OuterBounds.CalculateHitbox();
			this._floodedTileCount = DungeonUtils.CalculateFloodedTileCountFromShapeData(this.InnerBounds, this._innerShapeData);
		}

		// Token: 0x04005A05 RID: 23045
		private ShapeData _innerShapeData = new ShapeData();

		// Token: 0x04005A06 RID: 23046
		private ShapeData _outerShapeData = new ShapeData();

		// Token: 0x04005A07 RID: 23047
		private int _floodedTileCount;

		// Token: 0x04005A08 RID: 23048
		public Vector2D StartPosition;

		// Token: 0x04005A09 RID: 23049
		public Vector2D EndPosition;

		// Token: 0x04005A0A RID: 23050
		public int Strength;
	}
}
