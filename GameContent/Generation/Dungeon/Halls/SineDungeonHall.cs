using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Halls
{
	// Token: 0x020004BC RID: 1212
	public class SineDungeonHall : DungeonHall
	{
		// Token: 0x06003489 RID: 13449 RVA: 0x00605837 File Offset: 0x00603A37
		public SineDungeonHall(DungeonHallSettings settings) : base(settings)
		{
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x0060584C File Offset: 0x00603A4C
		public override void CalculatePlatformsAndDoors(DungeonData data)
		{
			if (!base.Processed)
			{
				return;
			}
			DungeonUtils.CalculatePlatformAndDoorsOnHallway(data, this.StartPosition, this.StartDirection.Y, this.settings.ForceStyleForDoorsAndPlatforms ? this.settings.StyleData : null, 0.1);
			DungeonUtils.CalculatePlatformAndDoorsOnHallway(data, this.EndPosition, this.EndDirection.Y, this.settings.ForceStyleForDoorsAndPlatforms ? this.settings.StyleData : null, 0.1);
			float num = 0.65f;
			for (int i = 0; i < this.PotentialPlatformPoints.Count; i++)
			{
				Tuple<Vector2D, Vector2D> tuple = this.PotentialPlatformPoints[i];
				Vector2D item = tuple.Item1;
				Vector2D item2 = tuple.Item2;
				if (item2.Y >= (double)num || item2.Y <= (double)(-(double)num))
				{
					DungeonUtils.CalculatePlatformAndDoorsOnHallway(data, item, item2.Y, this.settings.ForceStyleForDoorsAndPlatforms ? this.settings.StyleData : null, 0.1);
				}
			}
		}

		// Token: 0x0600348B RID: 13451 RVA: 0x00605953 File Offset: 0x00603B53
		public override void CalculateHall(DungeonData data, Vector2D startPoint, Vector2D endPoint)
		{
			this.calculated = false;
			this.SineHall(data, startPoint, endPoint, false);
			this.calculated = true;
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x0060596D File Offset: 0x00603B6D
		public override void GenerateHall(DungeonData data)
		{
			this.generated = false;
			this.SineHall(data, this.StartPosition, this.EndPosition, true);
			this.generated = true;
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x00605994 File Offset: 0x00603B94
		public void SineHall(DungeonData data, Vector2D startPoint, Vector2D endPoint, bool generating = false)
		{
			SineDungeonHallSettings sineDungeonHallSettings = (SineDungeonHallSettings)this.settings;
			UnifiedRandom unifiedRandom = new UnifiedRandom(sineDungeonHallSettings.RandomSeed);
			ushort brickTileType = this.settings.StyleData.BrickTileType;
			ushort brickCrackedTileType = this.settings.StyleData.BrickCrackedTileType;
			ushort brickWallType = this.settings.StyleData.BrickWallType;
			Vector2D vector2D = startPoint;
			bool flag = false;
			if (sineDungeonHallSettings.CrackedBrickChance > 0.0)
			{
				flag = (unifiedRandom.NextDouble() <= sineDungeonHallSettings.CrackedBrickChance);
			}
			int num = 3;
			int num2 = 8;
			int num3 = num + num2;
			Vector2D v = endPoint - startPoint;
			Vector2D vector2D2 = v.SafeNormalize(Vector2D.UnitX);
			int i = (int)Math.Ceiling(v.Length() / vector2D2.Length());
			int num4 = i;
			this.Bounds.SetBounds((int)startPoint.X, (int)startPoint.Y, (int)startPoint.X, (int)startPoint.Y);
			DungeonRoomSearchSettings settings = new DungeonRoomSearchSettings
			{
				Fluff = (int)((float)num * sineDungeonHallSettings.Magnitude) + num2
			};
			List<DungeonRoom> allRoomsInSpots = DungeonUtils.GetAllRoomsInSpots(data.dungeonRooms, startPoint, endPoint, settings);
			Vector2D vector2D3 = vector2D2;
			Vector2D vector2D4 = (Vector2.UnitY * sineDungeonHallSettings.Magnitude).ToVector2D();
			vector2D4 = vector2D4.ToVector2().RotatedBy(vector2D3.ToRotation(), Vector2.Zero).ToVector2D();
			float num5 = 0f;
			float num6 = 0f;
			float num7 = (float)sineDungeonHallSettings.Iterations / (float)num4 * 6.2831855f;
			while (i > 0)
			{
				Vector2D vector2D5 = vector2D4 * (double)((float)Math.Sin((double)num6));
				Vector2D vector2D6 = sineDungeonHallSettings.FlipSine ? (vector2D - vector2D5) : (vector2D + vector2D5);
				if (!WorldGen.InWorld((int)(vector2D6.X + vector2D2.X), (int)(vector2D6.Y + vector2D2.Y), 10))
				{
					break;
				}
				if (!base.Processed)
				{
					data.dungeonBounds.UpdateBounds((int)vector2D6.X - num3, (int)vector2D6.Y - num3, (int)vector2D6.Y + num3, (int)vector2D6.Y + num3);
					this.Bounds.UpdateBounds((int)vector2D6.X - num3, (int)vector2D6.Y - num3, (int)vector2D6.Y + num3, (int)vector2D6.Y + num3);
				}
				if (generating)
				{
					base.GenerateDungeonSquareHall(data, allRoomsInSpots, vector2D6, brickTileType, brickCrackedTileType, brickWallType, num, num2, sineDungeonHallSettings.PlaceOverProtectedBricks, flag, false);
				}
				vector2D += vector2D2;
				num5 += num7;
				num6 += num7;
				if (num5 >= 0.5f)
				{
					num5 = 0f;
					this.PotentialPlatformPoints.Add(new Tuple<Vector2D, Vector2D>(vector2D, vector2D5));
				}
				i--;
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

		// Token: 0x04005A10 RID: 23056
		public List<Tuple<Vector2D, Vector2D>> PotentialPlatformPoints = new List<Tuple<Vector2D, Vector2D>>();
	}
}
