using System;
using System.Collections.Generic;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Halls
{
	// Token: 0x020004BE RID: 1214
	public class StairwellDungeonHall : DungeonHall
	{
		// Token: 0x0600348F RID: 13455 RVA: 0x00605D01 File Offset: 0x00603F01
		public StairwellDungeonHall(StairwellDungeonHallSettings settings) : base(settings)
		{
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x00605D0C File Offset: 0x00603F0C
		public override void CalculatePlatformsAndDoors(DungeonData data)
		{
			if (!base.Processed)
			{
				return;
			}
			DungeonUtils.CalculatePlatformAndDoorsOnHallway(data, this.StartPosition, this.StartDirection.Y, this.settings.ForceStyleForDoorsAndPlatforms ? this.settings.StyleData : null, 0.1);
			DungeonUtils.CalculatePlatformAndDoorsOnHallway(data, this.EndPosition, this.EndDirection.Y, this.settings.ForceStyleForDoorsAndPlatforms ? this.settings.StyleData : null, 0.1);
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x00605D98 File Offset: 0x00603F98
		public override void CalculateHall(DungeonData data, Vector2D startPoint, Vector2D endPoint)
		{
			this.calculated = false;
			this.Stairwell(data, startPoint, endPoint, false);
			this.calculated = true;
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x00605DB2 File Offset: 0x00603FB2
		public override void GenerateHall(DungeonData data)
		{
			this.generated = false;
			this.Stairwell(data, this.StartPosition, this.EndPosition, true);
			this.generated = true;
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x00605DD8 File Offset: 0x00603FD8
		public void Stairwell(DungeonData data, Vector2D startPoint, Vector2D endPoint, bool generating = false)
		{
			StairwellDungeonHallSettings stairwellDungeonHallSettings = (StairwellDungeonHallSettings)this.settings;
			UnifiedRandom unifiedRandom = new UnifiedRandom(stairwellDungeonHallSettings.RandomSeed);
			this.Bounds.SetBounds((int)startPoint.X, (int)startPoint.Y, (int)startPoint.X, (int)startPoint.Y);
			bool flag = false;
			if (stairwellDungeonHallSettings.CrackedBrickChance > 0.0)
			{
				flag = (unifiedRandom.NextDouble() <= stairwellDungeonHallSettings.CrackedBrickChance);
			}
			int innerBoundsSize = stairwellDungeonHallSettings.InnerBoundsSize;
			int outerBoundsSize = stairwellDungeonHallSettings.OuterBoundsSize;
			int num = innerBoundsSize + outerBoundsSize;
			List<DungeonRoom> allRoomsInSpots = DungeonUtils.GetAllRoomsInSpots(data.dungeonRooms, startPoint, endPoint, new DungeonRoomSearchSettings
			{
				Fluff = num + stairwellDungeonHallSettings.MaxDistFromLine
			});
			Vector2D vector2D = endPoint - startPoint;
			Vector2D vector2D2 = this.CalculateZigZagSlope(startPoint, endPoint, stairwellDungeonHallSettings.Gradient);
			double num2 = Math.Ceiling(Math.Abs(Vector2D.Cross(vector2D2, vector2D.SafeNormalize(default(Vector2D)))) / (double)stairwellDungeonHallSettings.MaxDistFromLine);
			vector2D2 /= num2;
			Vector2D startPoint2 = startPoint;
			int num3 = 0;
			while ((double)num3 < num2)
			{
				Vector2D vector2D3 = startPoint + vector2D * (double)num3 / num2;
				Vector2D vector2D4 = startPoint + vector2D * (double)(num3 + 1) / num2;
				Vector2D vector2D5 = vector2D3 + vector2D2 + unifiedRandom.NextVector2DCircular((double)stairwellDungeonHallSettings.PointVariance, (double)stairwellDungeonHallSettings.PointVariance);
				Vector2D vector2D6 = vector2D4 - vector2D2 + unifiedRandom.NextVector2DCircular((double)stairwellDungeonHallSettings.PointVariance, (double)stairwellDungeonHallSettings.PointVariance);
				this.GenerateHallway(data, startPoint2, vector2D5, allRoomsInSpots, flag, generating);
				this.GenerateHallway(data, vector2D5, vector2D6, allRoomsInSpots, flag, generating);
				startPoint2 = vector2D6;
				num3++;
			}
			this.GenerateHallway(data, startPoint2, endPoint, allRoomsInSpots, flag, generating);
			data.genVars.generatingDungeonPositionX = (int)endPoint.X;
			data.genVars.generatingDungeonPositionY = (int)endPoint.Y;
			this.StartPosition = startPoint;
			this.EndPosition = endPoint;
			this.StartDirection = (this.EndDirection = vector2D2.SafeNormalize(default(Vector2D)));
			this.CrackedBrick = flag;
			this.Bounds.CalculateHitbox();
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x00606000 File Offset: 0x00604200
		private Vector2D CalculateZigZagSlope(Vector2D startPoint, Vector2D endPoint, double gradient)
		{
			Vector2D vector2D = (endPoint - startPoint).SafeNormalize(Vector2D.UnitY);
			Vector2D vector2D2 = new Vector2D((double)((vector2D.X > 0.0) ? 1 : -1), gradient).SafeNormalize(default(Vector2D));
			double num = Vector2D.Distance(startPoint, endPoint) * (vector2D.X / vector2D2.X + vector2D.Y / vector2D2.Y) / 4.0;
			return vector2D2 * num;
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x00606080 File Offset: 0x00604280
		private void GenerateHallway(DungeonData data, Vector2D startPoint, Vector2D endPoint, List<DungeonRoom> roomsInArea, bool crackedBricks, bool generating)
		{
			ushort brickTileType = this.settings.StyleData.BrickTileType;
			ushort brickCrackedTileType = this.settings.StyleData.BrickCrackedTileType;
			ushort brickWallType = this.settings.StyleData.BrickWallType;
			StairwellDungeonHallSettings stairwellDungeonHallSettings = (StairwellDungeonHallSettings)this.settings;
			int innerBoundsSize = stairwellDungeonHallSettings.InnerBoundsSize;
			int outerBoundsSize = stairwellDungeonHallSettings.OuterBoundsSize;
			int num = innerBoundsSize + outerBoundsSize;
			Vector2D vector2D = endPoint - startPoint;
			double num2 = Math.Ceiling(Math.Max(Math.Abs(vector2D.X), Math.Abs(vector2D.Y)));
			int num3 = 0;
			while ((double)num3 < num2)
			{
				Vector2D vector2D2 = startPoint + vector2D * ((double)num3 / num2);
				if (!base.Processed)
				{
					data.dungeonBounds.UpdateBounds((int)vector2D2.X - num, (int)vector2D2.Y - num, (int)vector2D2.Y + num, (int)vector2D2.Y + num);
					this.Bounds.UpdateBounds((int)vector2D2.X - num, (int)vector2D2.Y - num, (int)vector2D2.Y + num, (int)vector2D2.Y + num);
				}
				if (generating)
				{
					base.GenerateDungeonSquareHall(data, roomsInArea, vector2D2, brickTileType, brickCrackedTileType, brickWallType, innerBoundsSize, outerBoundsSize, stairwellDungeonHallSettings.PlaceOverProtectedBricks, crackedBricks, stairwellDungeonHallSettings.IsEntranceHall);
				}
				num3++;
			}
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x006061D8 File Offset: 0x006043D8
		public override bool CanPlaceTileAt(DungeonData data, Tile tile, int tileType, int tileCrackedType)
		{
			return (!((StairwellDungeonHallSettings)this.settings).IsEntranceHall || ((!tile.active() || !Main.tileDungeon[(int)tile.type]) && !Main.wallDungeon[(int)tile.wall])) && base.CanPlaceTileAt(data, tile, tileType, tileCrackedType);
		}
	}
}
