using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.Rooms
{
	// Token: 0x020004A6 RID: 1190
	public class BiomeStructuredDungeonRoom : BiomeDungeonRoom
	{
		// Token: 0x0600341A RID: 13338 RVA: 0x00600133 File Offset: 0x005FE333
		public BiomeStructuredDungeonRoom(DungeonRoomSettings settings) : base(settings)
		{
			this._innerShapeData = new ShapeData();
			this._outerShapeData = new ShapeData();
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x006008A0 File Offset: 0x005FEAA0
		public override void CalculateRoom(DungeonData data)
		{
			this.calculated = false;
			int x = this.settings.RoomPosition.X;
			int y = this.settings.RoomPosition.Y;
			this.BiomeRoom(data, x, y, false);
			this.calculated = true;
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x006008E8 File Offset: 0x005FEAE8
		public override bool GenerateRoom(DungeonData data)
		{
			this.generated = false;
			int x = this.settings.RoomPosition.X;
			int y = this.settings.RoomPosition.Y;
			this.BiomeRoom(data, x, y, true);
			this.generated = true;
			return true;
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x00600930 File Offset: 0x005FEB30
		public override void GenerateEarlyDungeonFeaturesInRoom(DungeonData data)
		{
			UnifiedRandom unifiedRandom = new UnifiedRandom(this.settings.RandomSeed);
			BiomeDungeonRoomSettings biomeDungeonRoomSettings = (BiomeDungeonRoomSettings)this.settings;
			byte style = this.settings.StyleData.Style;
			int x = this.InnerBounds.Center.X;
			int y = this.InnerBounds.Center.Y;
			float width = (float)this.InnerBounds.Width;
			int height = this.InnerBounds.Height;
			int bottom = this.InnerBounds.Bottom;
			int num = (int)(width * 0.25f);
			int num2 = unifiedRandom.Next(2);
			if (num2 == 1 && !biomeDungeonRoomSettings.StyleData.CanGenerateFeatureAt(data, this, null, x, y))
			{
				num2 = 0;
			}
			if (num2 == 0 || num2 != 1)
			{
				DungeonWindowBasicSettings dungeonWindowBasicSettings = new DungeonWindowBasicSettings
				{
					Style = this.settings.StyleData,
					Width = 9,
					Height = height / 5,
					Closed = (unifiedRandom.Next(3) != 0)
				};
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, x, y);
				dungeonWindowBasicSettings.Width = 7;
				dungeonWindowBasicSettings.Height = height / 5 - 4;
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, x - num, y + 3);
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, x + num, y + 3);
				return;
			}
			int num3 = 4;
			DungeonPillarSettings dungeonPillarSettings = new DungeonPillarSettings();
			dungeonPillarSettings.Style = this.settings.StyleData;
			dungeonPillarSettings.OverridePaintTile = this.settings.OverridePaintTile;
			dungeonPillarSettings.OverridePaintWall = this.settings.OverridePaintWall;
			dungeonPillarSettings.PillarType = PillarType.BlockActuatedSolidTop;
			dungeonPillarSettings.Width = 10;
			dungeonPillarSettings.Height = bottom - y + 5;
			dungeonPillarSettings.CrowningOnTop = true;
			dungeonPillarSettings.CrowningOnBottom = false;
			dungeonPillarSettings.CrowningStopsAtPillar = true;
			dungeonPillarSettings.AlwaysPlaceEntirePillar = false;
			new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, x + 1, bottom + num3);
			dungeonPillarSettings.Width = 7;
			dungeonPillarSettings.Height = bottom - y;
			new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, x - num + 1, bottom + num3);
			new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, x + num, bottom + num3);
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x00600B40 File Offset: 0x005FED40
		public void BiomeRoom(DungeonData data, int i, int j, bool generating)
		{
			UnifiedRandom unifiedRandom = new UnifiedRandom(this.settings.RandomSeed);
			BiomeDungeonRoomSettings biomeDungeonRoomSettings = (BiomeDungeonRoomSettings)this.settings;
			ushort brickTileType = this.settings.StyleData.BrickTileType;
			ushort brickWallType = this.settings.StyleData.BrickWallType;
			Vector2 position = new Vector2((float)i, (float)j);
			int num = BiomeDungeonRoom.GetBiomeRoomInnerSize(biomeDungeonRoomSettings.StyleData);
			int num2 = 8;
			int num3 = BiomeDungeonRoom.GetBiomeRoomOuterSize(biomeDungeonRoomSettings.StyleData);
			if (this.calculated)
			{
				position = this.Position;
				num = this.RoomInnerSize;
				num3 = this.RoomOuterSize;
				num2 = this.WallDepth;
			}
			int num4 = 20;
			int num5 = Math.Max(num4 + num2, Math.Min(Main.maxTilesX - num4 - num2, (int)position.X - num));
			int num6 = Math.Max(num4 + num2, Math.Min(Main.maxTilesX - num4 - num2, (int)position.X + num));
			int num7 = Math.Max(num4 + num2, Math.Min(Main.maxTilesY - num4 - num2, (int)position.Y - num));
			int num8 = Math.Max(num4 + num2, Math.Min(Main.maxTilesY - num4 - num2, (int)position.Y + num));
			int num9 = Math.Max(num4, Math.Min(Main.maxTilesX - num4, (int)position.X - num3));
			int num10 = Math.Max(num4, Math.Min(Main.maxTilesX - num4, (int)position.X + num3));
			int num11 = Math.Max(num4, Math.Min(Main.maxTilesY - num4, (int)position.Y - num3));
			int num12 = Math.Max(num4, Math.Min(Main.maxTilesY - num4, (int)position.Y + num3));
			this.InnerBounds.SetBounds(num5, num7, num6 - 1, num8 - 1);
			this.OuterBounds.SetBounds(num9, num11, num10 - 1, num12 - 1);
			data.dungeonBounds.UpdateBounds(num9, num11, num10 - 1, num12 - 1);
			int num13 = num10 - num9;
			int num14 = num12 - num11;
			Point center = this.OuterBounds.Center;
			Point center2 = this.OuterBounds.Center;
			int variant = unifiedRandom.Next(4);
			int num15 = 1;
			for (int k = num9; k < num10; k++)
			{
				int num16 = k;
				float percentile = Math.Max(0f, Math.Min(1f, (float)(k - num9) / Math.Max(1f, (float)(num10 - 1 - num9))));
				float num17 = this.BiomeRoom_GetYPercent(unifiedRandom, variant, percentile);
				float num18 = (float)Math.Max(1, num14 / 16) + (float)Math.Max(1, num14 / 4) * num17;
				float num19 = (float)num7 + num18;
				float num20 = num19 - (float)num2;
				float num21 = (float)num8 - num18 - 1f;
				float num22 = num21 + (float)num2;
				for (int l = num11; l < num12; l++)
				{
					int num23 = l;
					float percentile2 = Math.Max(0f, Math.Min(1f, (float)(l - num11) / Math.Max(1f, (float)(num12 - 1 - num11))));
					float num24 = this.BiomeRoom_GetXPercent(unifiedRandom, variant, percentile2);
					float num25 = (float)Math.Max(1, num13 / 4) * num24;
					float num26 = (float)num5 + num25;
					float num27 = num26 - (float)num2;
					float num28 = (float)num6 - num25 - 1f;
					float num29 = num28 + (float)num2;
					Tile tile = Main.tile[num16, num23];
					Main.tile[num16, num23 - 1];
					Main.tile[num16, num23 + 1];
					Main.tile[num16, num23 + 2];
					if (generating && (tile.type == 484 || tile.type == 485))
					{
						tile.active(false);
					}
					if ((float)l >= num20 && (float)l <= num22 && (float)k >= num27 && (float)k <= num29)
					{
						if (k <= num9 + num2 - 1 || k >= num10 - num2 + 1 || ((float)l >= num20 && (float)l <= num19) || ((float)l >= num21 && (float)l <= num22) || ((float)k >= num27 && (float)k <= num26) || ((float)k >= num28 && (float)k <= num29))
						{
							if (!generating)
							{
								this._outerShapeData.Add(num16 - (int)position.X + num15, num23 - (int)position.Y + num15);
							}
							else
							{
								DungeonUtils.ChangeTileType(tile, brickTileType, false, biomeDungeonRoomSettings.OverridePaintTile);
								if (tile.liquid > 0)
								{
									tile.liquid = 0;
									tile.liquidType(0);
								}
								DungeonUtils.ChangeWallType(tile, brickWallType, false, biomeDungeonRoomSettings.OverridePaintWall);
							}
						}
						else if (this.InnerBounds.Contains(num16, num23))
						{
							if (!generating)
							{
								this._innerShapeData.Add(num16 - (int)position.X + num15, num23 - (int)position.Y + num15);
								this._outerShapeData.Add(num16 - (int)position.X + num15, num23 - (int)position.Y + num15);
							}
							else
							{
								if (tile.liquid > 0)
								{
									tile.liquid = 0;
									tile.liquidType(0);
								}
								DungeonUtils.ChangeWallType(tile, brickWallType, false, biomeDungeonRoomSettings.OverridePaintWall);
								tile.active(false);
								tile.ClearBlockPaintAndCoating();
							}
						}
						else if (!generating)
						{
							this._outerShapeData.Add(num16 - (int)position.X + num15, num23 - (int)position.Y + num15);
						}
						else
						{
							bool flag = k == num9 || k == num10 - 1 || l == num11 || l == num12 - 1 || k == num5 - 1 || k == num6 || l == num7 - 1 || l == num8;
							if (tile.liquid > 0)
							{
								tile.liquid = 0;
								tile.liquidType(0);
							}
							DungeonUtils.ChangeTileType(tile, brickTileType, true, biomeDungeonRoomSettings.OverridePaintTile);
							if (!flag)
							{
								DungeonUtils.ChangeWallType(tile, brickWallType, false, biomeDungeonRoomSettings.OverridePaintWall);
							}
						}
					}
				}
			}
			base.BiomeRoom_AddHallwaySpace(position, num5, num6, num7, num8, num15, brickWallType, this.settings.OverridePaintWall, generating);
			base.BiomeRoom_FinishRoom(unifiedRandom, num9, num10, num11, num12, false);
			this.RoomInnerSize = num;
			this.RoomOuterSize = num3;
			this.WallDepth = num2;
			this.Position = position;
			this.InnerBounds.CalculateHitbox();
			this.OuterBounds.CalculateHitbox();
		}

		// Token: 0x0600341F RID: 13343 RVA: 0x006011A0 File Offset: 0x005FF3A0
		public float BiomeRoom_GetXPercent(UnifiedRandom genRand, int variant, float percentile)
		{
			switch (variant)
			{
			default:
				return Utils.MultiLerp(Utils.WrappedLerp(0f, 1f, percentile), new float[]
				{
					0.1f,
					0.5f,
					0.1f
				});
			case 1:
			{
				float percent = Utils.WrappedLerp(0f, 1f, percentile);
				float[] array = new float[4];
				array[1] = 0.9f;
				array[2] = 0.25f;
				return Utils.MultiLerp(percent, array);
			}
			case 2:
				return 0f;
			case 3:
				return 0f;
			}
		}

		// Token: 0x06003420 RID: 13344 RVA: 0x00601224 File Offset: 0x005FF424
		public float BiomeRoom_GetYPercent(UnifiedRandom genRand, int variant, float percentile)
		{
			switch (variant)
			{
			default:
				return Utils.MultiLerp(Utils.WrappedLerp(0f, 1f, percentile), new float[]
				{
					1f,
					0f,
					0.5f,
					0.5f,
					0f,
					0f,
					0.5f
				});
			case 1:
				return 0f;
			case 2:
				return Utils.MultiLerp(Utils.WrappedLerp(0f, 1f, percentile), new float[]
				{
					1f,
					1f,
					0.5f,
					0f,
					0.5f,
					0.25f,
					0.1f
				});
			case 3:
				return Utils.MultiLerp(Utils.WrappedLerp(0f, 1f, percentile), new float[]
				{
					0f,
					0.25f,
					0.6f,
					0.25f,
					0f,
					0f,
					0.3f,
					0.4f,
					0.3f,
					0f,
					0f
				});
			}
		}

		// Token: 0x040059B1 RID: 22961
		public const int VARIANT_DOUBLEDIAMOND = 0;

		// Token: 0x040059B2 RID: 22962
		public const int VARIANT_ROUNDED = 1;

		// Token: 0x040059B3 RID: 22963
		public const int VARIANT_CANDY = 2;

		// Token: 0x040059B4 RID: 22964
		public const int VARIANT_WIGGLED = 3;

		// Token: 0x040059B5 RID: 22965
		public const int MAX_VARIANTS = 4;

		// Token: 0x040059B6 RID: 22966
		public Vector2 Position;

		// Token: 0x040059B7 RID: 22967
		public int RoomInnerSize;

		// Token: 0x040059B8 RID: 22968
		public int RoomOuterSize;

		// Token: 0x040059B9 RID: 22969
		public int WallDepth;
	}
}
