using System;
using System.Collections.Generic;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Halls
{
	// Token: 0x020004C0 RID: 1216
	public class RegularDungeonHall : DungeonHall
	{
		// Token: 0x06003498 RID: 13464 RVA: 0x00605D01 File Offset: 0x00603F01
		public RegularDungeonHall(DungeonHallSettings settings) : base(settings)
		{
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x00606230 File Offset: 0x00604430
		public override void CalculatePlatformsAndDoors(DungeonData data)
		{
			if (!base.Processed)
			{
				return;
			}
			DungeonUtils.CalculatePlatformAndDoorsOnHallway(data, this.StartPosition, this.StartDirection.Y, this.settings.ForceStyleForDoorsAndPlatforms ? this.settings.StyleData : null, 0.1);
			DungeonUtils.CalculatePlatformAndDoorsOnHallway(data, this.EndPosition, this.EndDirection.Y, this.settings.ForceStyleForDoorsAndPlatforms ? this.settings.StyleData : null, 0.1);
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x006062BC File Offset: 0x006044BC
		public override void CalculateHall(DungeonData data, Vector2D startPoint, Vector2D endPoint)
		{
			this.calculated = false;
			this.RegularHall(data, startPoint, endPoint, false);
			this.calculated = true;
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x006062D6 File Offset: 0x006044D6
		public override void GenerateHall(DungeonData data)
		{
			this.generated = false;
			this.RegularHall(data, this.StartPosition, this.EndPosition, true);
			this.generated = true;
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x006062FC File Offset: 0x006044FC
		public void RegularHall(DungeonData data, Vector2D startPoint, Vector2D endPoint, bool generating = false)
		{
			RegularDungeonHallSettings regularDungeonHallSettings = (RegularDungeonHallSettings)this.settings;
			UnifiedRandom unifiedRandom = new UnifiedRandom(regularDungeonHallSettings.RandomSeed);
			ushort brickTileType = this.settings.StyleData.BrickTileType;
			ushort brickCrackedTileType = this.settings.StyleData.BrickCrackedTileType;
			ushort brickWallType = this.settings.StyleData.BrickWallType;
			Vector2D vector2D = startPoint;
			bool flag = false;
			if (regularDungeonHallSettings.CrackedBrickChance > 0.0)
			{
				flag = (unifiedRandom.NextDouble() <= regularDungeonHallSettings.CrackedBrickChance);
			}
			int num = 3;
			int num2 = 8;
			if (regularDungeonHallSettings.OverrideInnerBoundsSize > 0)
			{
				num = regularDungeonHallSettings.OverrideInnerBoundsSize;
			}
			if (regularDungeonHallSettings.OverrideOuterBoundsSize > 0)
			{
				num2 = regularDungeonHallSettings.OverrideOuterBoundsSize;
			}
			int num3 = num + num2;
			Vector2D v = endPoint - startPoint;
			Vector2D vector2D2 = v.SafeNormalize(Vector2D.UnitX);
			int num4 = (int)Math.Ceiling(v.Length() / vector2D2.Length());
			this.Bounds.SetBounds((int)startPoint.X, (int)startPoint.Y, (int)startPoint.X, (int)startPoint.Y);
			DungeonRoomSearchSettings settings = new DungeonRoomSearchSettings
			{
				Fluff = num3
			};
			List<DungeonRoom> allRoomsInSpots = DungeonUtils.GetAllRoomsInSpots(data.dungeonRooms, startPoint, endPoint, settings);
			Vector2D vector2D3 = vector2D2;
			while (num4 > 0 && WorldGen.InWorld((int)(vector2D.X + vector2D2.X), (int)(vector2D.Y + vector2D2.Y), 10))
			{
				if (!base.Processed)
				{
					data.dungeonBounds.UpdateBounds((int)vector2D.X - num3, (int)vector2D.Y - num3, (int)vector2D.Y + num3, (int)vector2D.Y + num3);
					this.Bounds.UpdateBounds((int)vector2D.X - num3, (int)vector2D.Y - num3, (int)vector2D.Y + num3, (int)vector2D.Y + num3);
				}
				if (generating)
				{
					base.GenerateDungeonSquareHall(data, allRoomsInSpots, vector2D, brickTileType, brickCrackedTileType, brickWallType, num, num2, regularDungeonHallSettings.PlaceOverProtectedBricks, flag, false);
				}
				vector2D += vector2D2;
				num4--;
			}
			data.genVars.generatingDungeonPositionX = (int)endPoint.X;
			data.genVars.generatingDungeonPositionY = (int)endPoint.Y;
			this.StartPosition = startPoint;
			this.EndPosition = endPoint;
			this.StartDirection = new Vector2D(vector2D3.X, vector2D3.Y);
			this.EndDirection = new Vector2D(vector2D2.X, vector2D2.Y);
			this.CrackedBrick = flag;
			this.Bounds.CalculateHitbox();
		}
	}
}
