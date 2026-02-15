using System;
using Microsoft.Xna.Framework;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Rooms
{
	// Token: 0x020004A4 RID: 1188
	public class BiomeSquareDungeonRoom : BiomeDungeonRoom
	{
		// Token: 0x06003410 RID: 13328 RVA: 0x005FFCD7 File Offset: 0x005FDED7
		public BiomeSquareDungeonRoom(DungeonRoomSettings settings) : base(settings)
		{
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x005FFCE0 File Offset: 0x005FDEE0
		public override void CalculateRoom(DungeonData data)
		{
			this.calculated = false;
			int x = this.settings.RoomPosition.X;
			int y = this.settings.RoomPosition.Y;
			this.BiomeRoom(data, x, y, false);
			this.calculated = true;
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x005FFD28 File Offset: 0x005FDF28
		public override bool GenerateRoom(DungeonData data)
		{
			this.generated = false;
			int x = this.settings.RoomPosition.X;
			int y = this.settings.RoomPosition.Y;
			this.BiomeRoom(data, x, y, true);
			this.generated = true;
			return true;
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x005FFD70 File Offset: 0x005FDF70
		public void BiomeRoom(DungeonData data, int i, int j, bool generating)
		{
			UnifiedRandom genRand = new UnifiedRandom(this.settings.RandomSeed);
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
			this.InnerBounds.SetBounds(num5, num7, num6, num8);
			this.OuterBounds.SetBounds(num9, num11, num10, num12);
			data.dungeonBounds.UpdateBounds(num9, num11, num10, num12);
			if (generating)
			{
				Point center = this.OuterBounds.Center;
				for (int k = num9; k < num10; k++)
				{
					int num13 = k;
					for (int l = num11; l < num12; l++)
					{
						bool flag = false;
						bool flag2 = false;
						int num14 = l;
						Tile tile = Main.tile[num13, num14];
						Main.tile[num13, num14 - 1];
						Main.tile[num13, num14 + 1];
						Main.tile[num13, num14 + 2];
						if (tile.type == 484 || tile.type == 485)
						{
							tile.active(false);
						}
						if (this.InnerBounds.Contains(num13, num14))
						{
							if (tile.liquid > 0)
							{
								if (flag2)
								{
									tile.liquid = 0;
								}
								tile.liquidType(0);
							}
							if (flag)
							{
								DungeonUtils.ChangeWallType(tile, 0, false, biomeDungeonRoomSettings.OverridePaintWall);
							}
							else
							{
								DungeonUtils.ChangeWallType(tile, brickWallType, false, biomeDungeonRoomSettings.OverridePaintWall);
							}
							tile.active(false);
						}
						else
						{
							bool flag3 = k == num9 || k == num10 - 1 || l == num11 || l == num12 - 1 || k == num5 - 1 || k == num6 || l == num7 - 1 || l == num8;
							if (tile.liquid > 0)
							{
								tile.liquid = 0;
							}
							DungeonUtils.ChangeTileType(tile, brickTileType, true, biomeDungeonRoomSettings.OverridePaintTile);
							if (!flag3)
							{
								DungeonUtils.ChangeWallType(tile, brickWallType, false, biomeDungeonRoomSettings.OverridePaintWall);
							}
						}
					}
				}
				base.BiomeRoom_FinishRoom(genRand, num9, num10, num11, num12, false);
			}
			this.RoomInnerSize = num;
			this.RoomOuterSize = num3;
			this.WallDepth = num2;
			this.Position = position;
			this.InnerBounds.CalculateHitbox();
			this.OuterBounds.CalculateHitbox();
		}

		// Token: 0x040059A9 RID: 22953
		public Vector2 Position;

		// Token: 0x040059AA RID: 22954
		public int RoomInnerSize;

		// Token: 0x040059AB RID: 22955
		public int RoomOuterSize;

		// Token: 0x040059AC RID: 22956
		public int WallDepth;
	}
}
