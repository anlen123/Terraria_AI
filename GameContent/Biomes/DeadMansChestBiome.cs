using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	// Token: 0x02000504 RID: 1284
	public class DeadMansChestBiome : MicroBiome
	{
		// Token: 0x060035EA RID: 13802 RVA: 0x0061E550 File Offset: 0x0061C750
		public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
		{
			if (!DeadMansChestBiome.IsAGoodSpot(origin))
			{
				return false;
			}
			this.ClearCaches();
			Point position = new Point(origin.X, origin.Y + 1);
			this.FindBoulderTrapSpots(position);
			this.FindDartTrapSpots(position);
			this.FindExplosiveTrapSpots(position);
			if (!this.AreThereEnoughTraps())
			{
				return false;
			}
			this.TurnGoldChestIntoDeadMansChest(origin);
			foreach (DeadMansChestBiome.DartTrapPlacementAttempt dartTrapPlacementAttempt in this._dartTrapPlacementSpots)
			{
				this.ActuallyPlaceDartTrap(dartTrapPlacementAttempt.position, dartTrapPlacementAttempt.directionX, dartTrapPlacementAttempt.x, dartTrapPlacementAttempt.y, dartTrapPlacementAttempt.xPush, dartTrapPlacementAttempt.t);
			}
			foreach (DeadMansChestBiome.WirePlacementAttempt wirePlacementAttempt in this._wirePlacementSpots)
			{
				this.PlaceWireLine(wirePlacementAttempt.position, wirePlacementAttempt.dirX, wirePlacementAttempt.dirY, wirePlacementAttempt.steps);
			}
			foreach (DeadMansChestBiome.BoulderPlacementAttempt boulderPlacementAttempt in this._boulderPlacementSpots)
			{
				this.ActuallyPlaceBoulderTrap(boulderPlacementAttempt.position, boulderPlacementAttempt.yPush, boulderPlacementAttempt.requiredHeight, boulderPlacementAttempt.bestType);
			}
			foreach (DeadMansChestBiome.ExplosivePlacementAttempt explosivePlacementAttempt in this._explosivePlacementAttempt)
			{
				this.ActuallyPlaceExplosive(explosivePlacementAttempt.position);
			}
			this.PlaceWiresForExplosives(origin);
			return true;
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x0061E720 File Offset: 0x0061C920
		private void PlaceWiresForExplosives(Point origin)
		{
			if (this._explosivePlacementAttempt.Count > 0)
			{
				this.PlaceWireLine(origin, 0, 1, this._explosivePlacementAttempt[0].position.Y - origin.Y);
				int num = this._explosivePlacementAttempt[0].position.X;
				int num2 = this._explosivePlacementAttempt[0].position.X;
				int y = this._explosivePlacementAttempt[0].position.Y;
				for (int i = 1; i < this._explosivePlacementAttempt.Count; i++)
				{
					int x = this._explosivePlacementAttempt[i].position.X;
					if (num > x)
					{
						num = x;
					}
					if (num2 < x)
					{
						num2 = x;
					}
				}
				this.PlaceWireLine(new Point(num, y), 1, 0, num2 - num);
			}
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x0061E7F9 File Offset: 0x0061C9F9
		private bool AreThereEnoughTraps()
		{
			return (this._boulderPlacementSpots.Count >= 1 || this._explosivePlacementAttempt.Count >= 1) && this._dartTrapPlacementSpots.Count >= 1;
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x0061E82A File Offset: 0x0061CA2A
		private void ClearCaches()
		{
			this._dartTrapPlacementSpots.Clear();
			this._wirePlacementSpots.Clear();
			this._boulderPlacementSpots.Clear();
			this._explosivePlacementAttempt.Clear();
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x0061E858 File Offset: 0x0061CA58
		private void FindBoulderTrapSpots(Point position)
		{
			int num = position.X;
			int num2 = GenBase._random.Next(this._numberOfBoulderTraps);
			int num3 = GenBase._random.Next(this._numberOfStepsBetweenBoulderTraps);
			num -= num2 / 2 * num3;
			int num4 = position.Y - 6;
			for (int i = 0; i <= num2; i++)
			{
				this.FindBoulderTrapSpot(new Point(num, num4));
				num += num3;
			}
			if (this._boulderPlacementSpots.Count > 0)
			{
				int num5 = this._boulderPlacementSpots[0].position.X;
				int num6 = this._boulderPlacementSpots[0].position.X;
				for (int j = 1; j < this._boulderPlacementSpots.Count; j++)
				{
					int x = this._boulderPlacementSpots[j].position.X;
					if (num5 > x)
					{
						num5 = x;
					}
					if (num6 < x)
					{
						num6 = x;
					}
				}
				if (num5 > position.X)
				{
					num5 = position.X;
				}
				if (num6 < position.X)
				{
					num6 = position.X;
				}
				this._wirePlacementSpots.Add(new DeadMansChestBiome.WirePlacementAttempt(new Point(num5, num4 - 1), 1, 0, num6 - num5));
				this._wirePlacementSpots.Add(new DeadMansChestBiome.WirePlacementAttempt(position, 0, -1, 7));
			}
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x0061E9A4 File Offset: 0x0061CBA4
		private void FindBoulderTrapSpot(Point position)
		{
			int x = position.X;
			int y = position.Y;
			for (int i = 0; i < 50; i++)
			{
				if (Main.tile[x, y - i].active())
				{
					this.PlaceBoulderTrapSpot(new Point(x, y - i), i);
					return;
				}
			}
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x0061E9F4 File Offset: 0x0061CBF4
		private void PlaceBoulderTrapSpot(Point position, int yPush)
		{
			int[] array = new int[(int)TileID.Count];
			for (int i = position.X; i < position.X + 2; i++)
			{
				for (int j = position.Y - 4; j <= position.Y; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.active() && !Main.tileFrameImportant[(int)tile.type] && Main.tileSolid[(int)tile.type])
					{
						array[(int)tile.type]++;
					}
					if (tile.active() && !WorldGen.CanBeClearedDuringGeneration((int)tile.type, i, j, false))
					{
						return;
					}
					if (tile.active() && TileID.Sets.IsAContainer[(int)tile.type])
					{
						return;
					}
				}
			}
			for (int k = position.X - 1; k < position.X + 2 + 1; k++)
			{
				for (int l = position.Y - 4 - 1; l <= position.Y - 4 + 2; l++)
				{
					Tile tile2 = Main.tile[k, l];
					if (!tile2.active())
					{
						return;
					}
					if (TileID.Sets.IsAContainer[(int)tile2.type])
					{
						return;
					}
				}
			}
			int num = 2;
			int num2 = position.X - num;
			int num3 = position.Y - 4 - num;
			int num4 = position.X + num + 1;
			int num5 = position.Y - 4 + num + 1;
			for (int m = num2; m <= num4; m++)
			{
				for (int n = num3; n <= num5; n++)
				{
					Tile tile3 = Main.tile[m, n];
					if (tile3.active() && (TileID.Sets.IsAContainer[(int)tile3.type] || tile3.type == 12 || tile3.type == 665 || tile3.type == 639))
					{
						return;
					}
				}
			}
			int num6 = -1;
			for (int num7 = 0; num7 < array.Length; num7++)
			{
				if (num6 == -1 || array[num6] < array[num7])
				{
					num6 = num7;
				}
			}
			this._boulderPlacementSpots.Add(new DeadMansChestBiome.BoulderPlacementAttempt(position, yPush - 1, 4, num6));
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x0061EC18 File Offset: 0x0061CE18
		private void FindDartTrapSpots(Point position)
		{
			int num = GenBase._random.Next(this._numberOfDartTraps);
			int num2 = (GenBase._random.Next(2) == 0) ? -1 : 1;
			int steps = -1;
			for (int i = 0; i < num; i++)
			{
				bool flag = this.FindDartTrapSpotSingle(position, num2);
				num2 *= -1;
				position.Y--;
				if (flag)
				{
					steps = i;
				}
			}
			this._wirePlacementSpots.Add(new DeadMansChestBiome.WirePlacementAttempt(new Point(position.X, position.Y + num), 0, -1, steps));
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x0061EC98 File Offset: 0x0061CE98
		private bool FindDartTrapSpotSingle(Point position, int directionX)
		{
			int x = position.X;
			int y = position.Y;
			int i = 0;
			while (i < 20)
			{
				Tile tile = Main.tile[x + i * directionX, y];
				if ((!tile.active() || tile.type < 0 || tile.type >= TileID.Count || !TileID.Sets.IsAContainer[(int)tile.type]) && tile.active() && Main.tileSolid[(int)tile.type])
				{
					if (i >= 5 && !tile.actuator() && !Main.tileFrameImportant[(int)tile.type] && WorldGen.CanBeClearedDuringGeneration((int)tile.type, x + i * directionX, y, false))
					{
						this._dartTrapPlacementSpots.Add(new DeadMansChestBiome.DartTrapPlacementAttempt(position, directionX, x, y, i, tile));
						return true;
					}
					return false;
				}
				else
				{
					i++;
				}
			}
			return false;
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x0061ED64 File Offset: 0x0061CF64
		private void FindExplosiveTrapSpots(Point position)
		{
			int num = position.X;
			int y = position.Y + 3;
			List<int> list = new List<int>();
			if (this.IsGoodSpotsForExplosive(num, y))
			{
				list.Add(num);
			}
			num++;
			if (this.IsGoodSpotsForExplosive(num, y))
			{
				list.Add(num);
			}
			int num2 = -1;
			if (list.Count > 0)
			{
				num2 = list[GenBase._random.Next(list.Count)];
			}
			list.Clear();
			num += GenBase._random.Next(2, 6);
			int num3 = 4;
			for (int i = num; i < num + num3; i++)
			{
				if (this.IsGoodSpotsForExplosive(i, y))
				{
					list.Add(i);
				}
			}
			int num4 = -1;
			if (list.Count > 0)
			{
				num4 = list[GenBase._random.Next(list.Count)];
			}
			num = position.X - num3 - GenBase._random.Next(2, 6);
			for (int j = num; j < num + num3; j++)
			{
				if (this.IsGoodSpotsForExplosive(j, y))
				{
					list.Add(j);
				}
			}
			int num5 = -1;
			if (list.Count > 0)
			{
				num5 = list[GenBase._random.Next(list.Count)];
			}
			if (num5 != -1)
			{
				this._explosivePlacementAttempt.Add(new DeadMansChestBiome.ExplosivePlacementAttempt(new Point(num5, y)));
			}
			if (num2 != -1)
			{
				this._explosivePlacementAttempt.Add(new DeadMansChestBiome.ExplosivePlacementAttempt(new Point(num2, y)));
			}
			if (num4 != -1)
			{
				this._explosivePlacementAttempt.Add(new DeadMansChestBiome.ExplosivePlacementAttempt(new Point(num4, y)));
			}
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x0061EEE8 File Offset: 0x0061D0E8
		private bool IsGoodSpotsForExplosive(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			return (!tile.active() || tile.type < 0 || tile.type >= TileID.Count || !TileID.Sets.IsAContainer[(int)tile.type]) && (tile.active() && Main.tileSolid[(int)tile.type] && !Main.tileFrameImportant[(int)tile.type] && !Main.tileSolidTop[(int)tile.type]);
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x0061EF68 File Offset: 0x0061D168
		public List<int> GetPossibleChestsToTrapify(StructureMap structures)
		{
			List<int> list = new List<int>();
			bool[] array = new bool[TileID.Sets.GeneralPlacementTiles.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = TileID.Sets.GeneralPlacementTiles[i];
			}
			array[21] = true;
			array[467] = true;
			array[138] = true;
			array[664] = true;
			array[712] = true;
			array[713] = true;
			array[714] = true;
			array[715] = true;
			for (int j = 0; j < 8000; j++)
			{
				Chest chest = Main.chest[j];
				if (chest != null)
				{
					Point point = new Point(chest.x, chest.y);
					if (DeadMansChestBiome.IsAGoodSpot(point))
					{
						this.ClearCaches();
						Point position = new Point(point.X, point.Y + 1);
						this.FindBoulderTrapSpots(position);
						this.FindDartTrapSpots(position);
						if (this.AreThereEnoughTraps() && (structures == null || structures.CanPlace(new Rectangle(point.X, point.Y, 1, 1), array, 10)))
						{
							list.Add(j);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x0061F080 File Offset: 0x0061D280
		private static bool IsAGoodSpot(Point position)
		{
			if (!WorldGen.InWorld(position.X, position.Y, 50))
			{
				return false;
			}
			if (WorldGen.oceanDepths(position.X, position.Y))
			{
				return false;
			}
			Tile tile = Main.tile[position.X, position.Y];
			if (tile.type != 21)
			{
				return false;
			}
			if (tile.frameX / 36 != 1)
			{
				return false;
			}
			tile = Main.tile[position.X, position.Y + 2];
			return WorldGen.CanBeClearedDuringGeneration((int)tile.type, position.X, position.Y + 2, false) && WorldGen.countWires(position.X, position.Y, 20) <= 0 && WorldGen.countTiles(position.X, position.Y, false, true) >= 40;
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x0061F154 File Offset: 0x0061D354
		private void TurnGoldChestIntoDeadMansChest(Point position)
		{
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					int num = position.X + i;
					int num2 = position.Y + j;
					Tile tile = Main.tile[num, num2];
					tile.type = 467;
					tile.frameX = (short)(144 + i * 18);
					tile.frameY = (short)(j * 18);
				}
			}
			if (GenBase._random.Next(3) == 0)
			{
				int num3 = Chest.FindChest(position.X, position.Y);
				if (num3 > -1)
				{
					Item[] item = Main.chest[num3].item;
					for (int k = item.Length - 2; k > 0; k--)
					{
						Item item2 = item[k];
						if (item2.stack != 0)
						{
							item[k + 1] = item2.DeepClone();
						}
					}
					item[1] = new Item();
					item[1].SetDefaults(5007, null);
					Main.chest[num3].item = item;
				}
			}
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x0061F250 File Offset: 0x0061D450
		private void ActuallyPlaceDartTrap(Point position, int directionX, int x, int y, int xPush, Tile t)
		{
			t.type = 137;
			t.frameY = 0;
			if (directionX == -1)
			{
				t.frameX = 18;
			}
			else
			{
				t.frameX = 0;
			}
			t.slope(0);
			t.halfBrick(false);
			WorldGen.TileFrame(x, y, true, false);
			this.PlaceWireLine(position, directionX, 0, xPush);
		}

		// Token: 0x060035F9 RID: 13817 RVA: 0x0061F2B0 File Offset: 0x0061D4B0
		private void PlaceWireLine(Point start, int offsetX, int offsetY, int steps)
		{
			for (int i = 0; i <= steps; i++)
			{
				Main.tile[start.X + offsetX * i, start.Y + offsetY * i].wire(true);
			}
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x0061F2F0 File Offset: 0x0061D4F0
		private void ActuallyPlaceBoulderTrap(Point position, int yPush, int requiredHeight, int bestType)
		{
			for (int i = position.X; i < position.X + 2; i++)
			{
				for (int j = position.Y - requiredHeight; j <= position.Y + 2; j++)
				{
					Tile tile = Main.tile[i, j];
					if (j < position.Y - requiredHeight + 2)
					{
						tile.ClearTile();
					}
					else if (j <= position.Y)
					{
						bool flag = false;
						do
						{
							if (!tile.active())
							{
								tile.active(true);
								tile.type = (ushort)bestType;
							}
							tile.slope(0);
							tile.halfBrick(false);
							WorldGen.TileFrame(i, j, true, false);
							if (flag)
							{
								break;
							}
							flag = true;
						}
						while (!tile.active());
						tile.wire(true);
						if (Main.tileSolid[(int)tile.type])
						{
							tile.actuator(true);
						}
					}
					else
					{
						tile.ClearTile();
					}
				}
			}
			int num = position.X + 1;
			int num2 = position.Y - requiredHeight + 1;
			int num3 = 3;
			int num4 = num - num3;
			int num5 = num2 - num3;
			int num6 = num + num3 - 1;
			int num7 = num2 + num3 - 1;
			for (int k = num4; k <= num6; k++)
			{
				for (int l = num5; l <= num7; l++)
				{
					Tile tile2 = Main.tile[k, l];
					if (tile2.type >= 0 && !TileID.Sets.Boulders[(int)tile2.type])
					{
						tile2.type = 1;
						if (tile2.wire())
						{
							tile2.actuator(true);
						}
					}
				}
			}
			WorldGen.PlaceTile(num, num2, 138, false, false, -1, 0);
			this.PlaceWireLine(position, 0, 1, yPush);
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x0061F494 File Offset: 0x0061D694
		private void ActuallyPlaceExplosive(Point position)
		{
			Tile tile = Main.tile[position.X, position.Y];
			tile.type = 141;
			tile.frameX = (tile.frameY = 0);
			tile.slope(0);
			tile.halfBrick(false);
			WorldGen.TileFrame(position.X, position.Y, true, false);
		}

		// Token: 0x04005ADA RID: 23258
		private List<DeadMansChestBiome.DartTrapPlacementAttempt> _dartTrapPlacementSpots = new List<DeadMansChestBiome.DartTrapPlacementAttempt>();

		// Token: 0x04005ADB RID: 23259
		private List<DeadMansChestBiome.WirePlacementAttempt> _wirePlacementSpots = new List<DeadMansChestBiome.WirePlacementAttempt>();

		// Token: 0x04005ADC RID: 23260
		private List<DeadMansChestBiome.BoulderPlacementAttempt> _boulderPlacementSpots = new List<DeadMansChestBiome.BoulderPlacementAttempt>();

		// Token: 0x04005ADD RID: 23261
		private List<DeadMansChestBiome.ExplosivePlacementAttempt> _explosivePlacementAttempt = new List<DeadMansChestBiome.ExplosivePlacementAttempt>();

		// Token: 0x04005ADE RID: 23262
		[JsonProperty("NumberOfDartTraps")]
		private IntRange _numberOfDartTraps = new IntRange(3, 6);

		// Token: 0x04005ADF RID: 23263
		[JsonProperty("NumberOfBoulderTraps")]
		private IntRange _numberOfBoulderTraps = new IntRange(2, 4);

		// Token: 0x04005AE0 RID: 23264
		[JsonProperty("NumberOfStepsBetweenBoulderTraps")]
		private IntRange _numberOfStepsBetweenBoulderTraps = new IntRange(2, 4);

		// Token: 0x0200098E RID: 2446
		private class DartTrapPlacementAttempt
		{
			// Token: 0x06004972 RID: 18802 RVA: 0x006D0B2A File Offset: 0x006CED2A
			public DartTrapPlacementAttempt(Point position, int directionX, int x, int y, int xPush, Tile t)
			{
				this.position = position;
				this.directionX = directionX;
				this.x = x;
				this.y = y;
				this.xPush = xPush;
				this.t = t;
			}

			// Token: 0x04007619 RID: 30233
			public int directionX;

			// Token: 0x0400761A RID: 30234
			public int xPush;

			// Token: 0x0400761B RID: 30235
			public int x;

			// Token: 0x0400761C RID: 30236
			public int y;

			// Token: 0x0400761D RID: 30237
			public Point position;

			// Token: 0x0400761E RID: 30238
			public Tile t;
		}

		// Token: 0x0200098F RID: 2447
		private class BoulderPlacementAttempt
		{
			// Token: 0x06004973 RID: 18803 RVA: 0x006D0B5F File Offset: 0x006CED5F
			public BoulderPlacementAttempt(Point position, int yPush, int requiredHeight, int bestType)
			{
				this.position = position;
				this.yPush = yPush;
				this.requiredHeight = requiredHeight;
				this.bestType = bestType;
			}

			// Token: 0x0400761F RID: 30239
			public Point position;

			// Token: 0x04007620 RID: 30240
			public int yPush;

			// Token: 0x04007621 RID: 30241
			public int requiredHeight;

			// Token: 0x04007622 RID: 30242
			public int bestType;
		}

		// Token: 0x02000990 RID: 2448
		private class WirePlacementAttempt
		{
			// Token: 0x06004974 RID: 18804 RVA: 0x006D0B84 File Offset: 0x006CED84
			public WirePlacementAttempt(Point position, int dirX, int dirY, int steps)
			{
				this.position = position;
				this.dirX = dirX;
				this.dirY = dirY;
				this.steps = steps;
			}

			// Token: 0x04007623 RID: 30243
			public Point position;

			// Token: 0x04007624 RID: 30244
			public int dirX;

			// Token: 0x04007625 RID: 30245
			public int dirY;

			// Token: 0x04007626 RID: 30246
			public int steps;
		}

		// Token: 0x02000991 RID: 2449
		private class ExplosivePlacementAttempt
		{
			// Token: 0x06004975 RID: 18805 RVA: 0x006D0BA9 File Offset: 0x006CEDA9
			public ExplosivePlacementAttempt(Point position)
			{
				this.position = position;
			}

			// Token: 0x04007627 RID: 30247
			public Point position;
		}
	}
}
