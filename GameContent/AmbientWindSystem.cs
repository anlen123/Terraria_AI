using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Utilities;

namespace Terraria.GameContent
{
	// Token: 0x02000275 RID: 629
	public class AmbientWindSystem
	{
		// Token: 0x06002420 RID: 9248 RVA: 0x0054AA58 File Offset: 0x00548C58
		public void Update()
		{
			if (!Main.LocalPlayer.ZoneGraveyard)
			{
				return;
			}
			this._updatesCounter++;
			Rectangle tileWorkSpace = this.GetTileWorkSpace();
			int num = tileWorkSpace.X + tileWorkSpace.Width;
			int num2 = tileWorkSpace.Y + tileWorkSpace.Height;
			for (int i = tileWorkSpace.X; i < num; i++)
			{
				for (int j = tileWorkSpace.Y; j < num2; j++)
				{
					this.TrySpawningWind(i, j);
				}
			}
			if (this._updatesCounter % 30 == 0)
			{
				this.SpawnAirborneWind();
			}
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x0054AAE4 File Offset: 0x00548CE4
		private void SpawnAirborneWind()
		{
			foreach (Point point in this._spotsForAirboneWind)
			{
				this.SpawnAirborneCloud(point.X, point.Y);
			}
			this._spotsForAirboneWind.Clear();
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x0054AB50 File Offset: 0x00548D50
		private Rectangle GetTileWorkSpace()
		{
			Point point = Main.LocalPlayer.Center.ToTileCoordinates();
			int num = 120;
			int num2 = 30;
			return new Rectangle(point.X - num / 2, point.Y - num2 / 2, num, num2);
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x0054AB90 File Offset: 0x00548D90
		private void TrySpawningWind(int x, int y)
		{
			if (!WorldGen.InWorld(x, y, 10))
			{
				return;
			}
			if (Main.tile[x, y] == null)
			{
				return;
			}
			this.TestAirCloud(x, y);
			Tile tile = Main.tile[x, y];
			if (!tile.active() || tile.slope() > 0 || tile.halfBrick() || !Main.tileSolid[(int)tile.type])
			{
				return;
			}
			tile = Main.tile[x, y - 1];
			if (WorldGen.SolidTile(tile))
			{
				return;
			}
			if (this._random.Next(120) != 0)
			{
				return;
			}
			this.SpawnFloorCloud(x, y);
			if (this._random.Next(3) == 0)
			{
				this.SpawnFloorCloud(x, y - 1);
			}
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x0054AC40 File Offset: 0x00548E40
		private void SpawnAirborneCloud(int x, int y)
		{
			int num = this._random.Next(2, 6);
			float num2 = 1.1f;
			float num3 = 2.2f;
			float num4 = 0.023561945f * this._random.NextFloatDirection();
			float num5 = 0.023561945f * this._random.NextFloatDirection();
			while (num5 > -0.011780973f && num5 < 0.011780973f)
			{
				num5 = 0.023561945f * this._random.NextFloatDirection();
			}
			if (this._random.Next(4) == 0)
			{
				num = this._random.Next(9, 16);
				num2 = 1.1f;
				num3 = 1.2f;
			}
			else if (this._random.Next(4) == 0)
			{
				num = this._random.Next(9, 16);
				num2 = 1.1f;
				num3 = 0.2f;
			}
			Vector2 value = new Vector2(-10f, 0f);
			Vector2 value2 = new Point(x, y).ToWorldCoordinates(8f, 8f);
			num4 -= num5 * (float)num * 0.5f;
			float num6 = num4;
			for (int i = 0; i < num; i++)
			{
				if (Main.rand.Next(10) == 0)
				{
					num5 *= this._random.NextFloatDirection();
				}
				Vector2 value3 = this._random.NextVector2Circular(4f, 4f);
				int type = 1091 + this._random.Next(2) * 2;
				float scaleFactor = 1.4f;
				float num7 = num2 + this._random.NextFloat() * num3;
				float num8 = num6 + num5;
				Vector2 value4 = Vector2.UnitX.RotatedBy((double)num8, default(Vector2)) * scaleFactor;
				Gore.NewGorePerfect(value2 + value3 - value, value4 * Main.WindForVisuals, type, num7);
				value2 += value4 * 6.5f * num7;
				num6 = num8;
			}
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x0054AE2C File Offset: 0x0054902C
		private void SpawnFloorCloud(int x, int y)
		{
			Vector2 position = new Point(x, y - 1).ToWorldCoordinates(8f, 8f);
			int type = this._random.Next(1087, 1090);
			float num = 16f * this._random.NextFloat();
			position.Y -= num;
			if (num < 4f)
			{
				type = 1090;
			}
			float scaleFactor = 0.4f;
			float scale = 0.8f + this._random.NextFloat() * 0.2f;
			Gore.NewGorePerfect(position, Vector2.UnitX * scaleFactor * Main.WindForVisuals, type, scale);
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x0054AED4 File Offset: 0x005490D4
		private void TestAirCloud(int x, int y)
		{
			if (this._random.Next(120000) != 0)
			{
				return;
			}
			for (int i = -2; i <= 2; i++)
			{
				if (i != 0)
				{
					Tile t = Main.tile[x + i, y];
					if (!this.DoesTileAllowWind(t))
					{
						return;
					}
					t = Main.tile[x, y + i];
					if (!this.DoesTileAllowWind(t))
					{
						return;
					}
				}
			}
			this._spotsForAirboneWind.Add(new Point(x, y));
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x0054AF48 File Offset: 0x00549148
		private bool DoesTileAllowWind(Tile t)
		{
			return !t.active() || !Main.tileSolid[(int)t.type];
		}

		// Token: 0x04004DBE RID: 19902
		private UnifiedRandom _random = new UnifiedRandom();

		// Token: 0x04004DBF RID: 19903
		private List<Point> _spotsForAirboneWind = new List<Point>();

		// Token: 0x04004DC0 RID: 19904
		private int _updatesCounter;
	}
}
