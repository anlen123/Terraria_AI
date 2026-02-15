using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation.Dungeon;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation
{
	// Token: 0x0200048B RID: 1163
	public class TrackGenerator
	{
		// Token: 0x06003357 RID: 13143 RVA: 0x005F5CC4 File Offset: 0x005F3EC4
		public bool Place(Point origin, int minLength, int maxLength)
		{
			if (!TrackGenerator.FindSuitableOrigin(ref origin))
			{
				return false;
			}
			this.CreateTrackStart(origin);
			if (!this.FindPath(minLength, maxLength))
			{
				return false;
			}
			this.PlacePath();
			return true;
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x005F5CEC File Offset: 0x005F3EEC
		private void PlacePath()
		{
			bool[] array = new bool[this._length];
			for (int i = 0; i < this._length; i++)
			{
				if (WorldGen.genRand.Next(7) == 0)
				{
					this.playerHeight = WorldGen.genRand.Next(5, 9);
				}
				for (int j = 0; j < this.playerHeight; j++)
				{
					TrackGenerator.TrackHistory trackHistory = this._history[i];
					if (Main.tile[(int)trackHistory.X, (int)trackHistory.Y - j - 1].wall == 244)
					{
						Main.tile[(int)trackHistory.X, (int)trackHistory.Y - j - 1].wall = 0;
					}
					if (Main.tile[(int)trackHistory.X, (int)trackHistory.Y - j].wall == 244)
					{
						Main.tile[(int)trackHistory.X, (int)trackHistory.Y - j].wall = 0;
					}
					if (Main.tile[(int)trackHistory.X, (int)trackHistory.Y - j + 1].wall == 244)
					{
						Main.tile[(int)trackHistory.X, (int)trackHistory.Y - j + 1].wall = 0;
					}
					if (Main.tile[(int)trackHistory.X, (int)trackHistory.Y - j].active() && Main.tile[(int)trackHistory.X, (int)trackHistory.Y - j].type == 135)
					{
						array[i] = true;
					}
					if (Main.tile[(int)trackHistory.X, (int)trackHistory.Y - j].type != 379)
					{
						WorldGen.KillTile((int)trackHistory.X, (int)trackHistory.Y - j, false, false, true);
					}
				}
			}
			for (int k = 0; k < this._length; k++)
			{
				if (WorldGen.genRand.Next(7) == 0)
				{
					this.playerHeight = WorldGen.genRand.Next(5, 9);
				}
				TrackGenerator.TrackHistory trackHistory2 = this._history[k];
				Tile.SmoothSlope((int)trackHistory2.X, (int)(trackHistory2.Y + 1), true, false);
				if (!Main.tile[(int)trackHistory2.X, (int)trackHistory2.Y - this.playerHeight].active() || Main.tile[(int)trackHistory2.X, (int)trackHistory2.Y - this.playerHeight].type != 379)
				{
					Tile.SmoothSlope((int)trackHistory2.X, (int)trackHistory2.Y - this.playerHeight, true, false);
				}
				bool wire = Main.tile[(int)trackHistory2.X, (int)trackHistory2.Y].wire();
				bool wire2 = Main.tile[(int)trackHistory2.X, (int)trackHistory2.Y].wire2();
				bool wire3 = Main.tile[(int)trackHistory2.X, (int)trackHistory2.Y].wire3();
				bool wire4 = Main.tile[(int)trackHistory2.X, (int)trackHistory2.Y].wire4();
				if (array[k] && k < this._length && k > 0 && this._history[k - 1].Y == trackHistory2.Y && this._history[k + 1].Y == trackHistory2.Y)
				{
					Main.tile[(int)trackHistory2.X, (int)trackHistory2.Y].ClearEverything();
					WorldGen.PlaceTile((int)trackHistory2.X, (int)trackHistory2.Y, 314, false, true, -1, 1);
				}
				else
				{
					Main.tile[(int)trackHistory2.X, (int)trackHistory2.Y].ResetToType(314);
				}
				Main.tile[(int)trackHistory2.X, (int)trackHistory2.Y].wire(wire);
				Main.tile[(int)trackHistory2.X, (int)trackHistory2.Y].wire2(wire2);
				Main.tile[(int)trackHistory2.X, (int)trackHistory2.Y].wire3(wire3);
				Main.tile[(int)trackHistory2.X, (int)trackHistory2.Y].wire4(wire4);
				if (k != 0)
				{
					for (int l = 0; l < 8; l++)
					{
						WorldUtils.TileFrame((int)this._history[k - 1].X, (int)this._history[k - 1].Y - l, true);
					}
					if (k == this._length - 1)
					{
						for (int m = 0; m < this.playerHeight; m++)
						{
							WorldUtils.TileFrame((int)trackHistory2.X, (int)trackHistory2.Y - m, true);
						}
					}
				}
			}
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x005F61A4 File Offset: 0x005F43A4
		private void CreateTrackStart(Point origin)
		{
			this._xDirection = ((origin.X > Main.maxTilesX / 2) ? -1 : 1);
			this._length = 1;
			for (int i = 0; i < this._history.Length; i++)
			{
				this._history[i] = new TrackGenerator.TrackHistory(origin.X + i * this._xDirection, origin.Y + i, TrackGenerator.TrackSlope.Down);
			}
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x005F6210 File Offset: 0x005F4410
		private bool FindPath(int minLength, int maxLength)
		{
			int length = this._length;
			while (this._length < this._history.Length - 100)
			{
				TrackGenerator.TrackSlope slope = (this._history[this._length - 1].Slope == TrackGenerator.TrackSlope.Up) ? TrackGenerator.TrackSlope.Straight : TrackGenerator.TrackSlope.Down;
				this.AppendToHistory(slope, TrackGenerator.TrackMode.Normal);
				TrackGenerator.TrackPlacementState trackPlacementState = this.TryRewriteHistoryToAvoidTiles();
				if (trackPlacementState == TrackGenerator.TrackPlacementState.Invalid)
				{
					break;
				}
				length = this._length;
				TrackGenerator.TrackPlacementState trackPlacementState2 = trackPlacementState;
				while (trackPlacementState2 != TrackGenerator.TrackPlacementState.Available)
				{
					trackPlacementState2 = this.CreateTunnel();
					if (trackPlacementState2 == TrackGenerator.TrackPlacementState.Invalid)
					{
						break;
					}
					length = this._length;
				}
				if (this._length >= maxLength)
				{
					break;
				}
			}
			this._length = Math.Min(maxLength, length);
			if (this._length < minLength)
			{
				return false;
			}
			this.SmoothTrack();
			return this.GetHistorySegmentPlacementState(0, this._length) != TrackGenerator.TrackPlacementState.Invalid;
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x005F62C8 File Offset: 0x005F44C8
		private TrackGenerator.TrackPlacementState CreateTunnel()
		{
			TrackGenerator.TrackSlope trackSlope = TrackGenerator.TrackSlope.Straight;
			int num = 10;
			TrackGenerator.TrackPlacementState trackPlacementState = TrackGenerator.TrackPlacementState.Invalid;
			int x = (int)this._history[this._length - 1].X;
			int y = (int)this._history[this._length - 1].Y;
			for (TrackGenerator.TrackSlope trackSlope2 = TrackGenerator.TrackSlope.Up; trackSlope2 <= TrackGenerator.TrackSlope.Down; trackSlope2 += 1)
			{
				TrackGenerator.TrackPlacementState trackPlacementState2 = TrackGenerator.TrackPlacementState.Invalid;
				for (int i = 1; i < num; i++)
				{
					trackPlacementState2 = TrackGenerator.CalculateStateForLocation(x + i * this._xDirection, y + i * (int)trackSlope2);
					if (trackPlacementState2 == TrackGenerator.TrackPlacementState.Invalid)
					{
						break;
					}
					if (trackPlacementState2 != TrackGenerator.TrackPlacementState.Obstructed)
					{
						trackSlope = trackSlope2;
						num = i;
						trackPlacementState = trackPlacementState2;
						break;
					}
				}
				if (trackPlacementState != TrackGenerator.TrackPlacementState.Available && trackPlacementState2 == TrackGenerator.TrackPlacementState.Obstructed && (trackPlacementState != TrackGenerator.TrackPlacementState.Obstructed || trackSlope != TrackGenerator.TrackSlope.Straight))
				{
					trackSlope = trackSlope2;
					num = 10;
					trackPlacementState = trackPlacementState2;
				}
			}
			if (this._length == 0 || !TrackGenerator.CanSlopesTouch(this._history[this._length - 1].Slope, trackSlope))
			{
				this.RewriteSlopeDirection(this._length - 1, TrackGenerator.TrackSlope.Straight);
			}
			this._history[this._length - 1].Mode = TrackGenerator.TrackMode.Tunnel;
			for (int j = 1; j < num; j++)
			{
				this.AppendToHistory(trackSlope, TrackGenerator.TrackMode.Tunnel);
			}
			return trackPlacementState;
		}

		// Token: 0x0600335C RID: 13148 RVA: 0x005F63E4 File Offset: 0x005F45E4
		private void AppendToHistory(TrackGenerator.TrackSlope slope, TrackGenerator.TrackMode mode = TrackGenerator.TrackMode.Normal)
		{
			this._history[this._length] = new TrackGenerator.TrackHistory((int)this._history[this._length - 1].X + this._xDirection, (int)((sbyte)this._history[this._length - 1].Y + slope), slope);
			this._history[this._length].Mode = mode;
			this._length++;
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x005F6468 File Offset: 0x005F4668
		private TrackGenerator.TrackPlacementState TryRewriteHistoryToAvoidTiles()
		{
			int i = this._length - 1;
			int num = Math.Min(this._length, this._rewriteHistory.Length);
			for (int j = 0; j < num; j++)
			{
				this._rewriteHistory[j] = this._history[i - j];
			}
			while (i >= this._length - num)
			{
				if (this._history[i].Slope == TrackGenerator.TrackSlope.Down)
				{
					TrackGenerator.TrackPlacementState historySegmentPlacementState = this.GetHistorySegmentPlacementState(i, this._length - i);
					if (historySegmentPlacementState == TrackGenerator.TrackPlacementState.Available)
					{
						return historySegmentPlacementState;
					}
					this.RewriteSlopeDirection(i, TrackGenerator.TrackSlope.Straight);
				}
				i--;
			}
			if (this.GetHistorySegmentPlacementState(i + 1, this._length - (i + 1)) == TrackGenerator.TrackPlacementState.Available)
			{
				return TrackGenerator.TrackPlacementState.Available;
			}
			for (i = this._length - 1; i >= this._length - num + 1; i--)
			{
				if (this._history[i].Slope == TrackGenerator.TrackSlope.Straight)
				{
					TrackGenerator.TrackPlacementState historySegmentPlacementState2 = this.GetHistorySegmentPlacementState(this._length - num, num);
					if (historySegmentPlacementState2 == TrackGenerator.TrackPlacementState.Available)
					{
						return historySegmentPlacementState2;
					}
					this.RewriteSlopeDirection(i, TrackGenerator.TrackSlope.Up);
				}
			}
			for (int k = 0; k < num; k++)
			{
				this._history[this._length - 1 - k] = this._rewriteHistory[k];
			}
			this.RewriteSlopeDirection(this._length - 1, TrackGenerator.TrackSlope.Straight);
			return this.GetHistorySegmentPlacementState(i + 1, this._length - (i + 1));
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x005F65B8 File Offset: 0x005F47B8
		private void RewriteSlopeDirection(int index, TrackGenerator.TrackSlope slope)
		{
			int num = (int)(slope - this._history[index].Slope);
			this._history[index].Slope = slope;
			for (int i = index; i < this._length; i++)
			{
				TrackGenerator.TrackHistory[] history = this._history;
				int num2 = i;
				history[num2].Y = history[num2].Y + (short)num;
			}
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x005F6618 File Offset: 0x005F4818
		private TrackGenerator.TrackPlacementState GetHistorySegmentPlacementState(int startIndex, int length)
		{
			TrackGenerator.TrackPlacementState result = TrackGenerator.TrackPlacementState.Available;
			for (int i = startIndex; i < startIndex + length; i++)
			{
				TrackGenerator.TrackPlacementState trackPlacementState = TrackGenerator.CalculateStateForLocation((int)this._history[i].X, (int)this._history[i].Y);
				if (trackPlacementState != TrackGenerator.TrackPlacementState.Obstructed)
				{
					if (trackPlacementState == TrackGenerator.TrackPlacementState.Invalid)
					{
						return trackPlacementState;
					}
				}
				else if (this._history[i].Mode != TrackGenerator.TrackMode.Tunnel)
				{
					result = trackPlacementState;
				}
			}
			return result;
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x005F6680 File Offset: 0x005F4880
		private void SmoothTrack()
		{
			int num = this._length - 1;
			bool flag = false;
			for (int i = this._length - 1; i >= 0; i--)
			{
				if (flag)
				{
					num = Math.Min(i + 15, num);
					if (this._history[i].Y >= this._history[num].Y)
					{
						int num2 = i + 1;
						while (this._history[num2].Y > this._history[i].Y)
						{
							this._history[num2].Y = this._history[i].Y;
							this._history[num2].Slope = TrackGenerator.TrackSlope.Straight;
							num2++;
						}
						if (this._history[i].Y == this._history[num].Y)
						{
							flag = false;
						}
					}
				}
				else if (this._history[i].Y > this._history[num].Y)
				{
					flag = true;
				}
				else
				{
					num = i;
				}
			}
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x005F679D File Offset: 0x005F499D
		private static bool CanSlopesTouch(TrackGenerator.TrackSlope leftSlope, TrackGenerator.TrackSlope rightSlope)
		{
			return leftSlope == rightSlope || leftSlope == TrackGenerator.TrackSlope.Straight || rightSlope == TrackGenerator.TrackSlope.Straight;
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x005F67AC File Offset: 0x005F49AC
		private static bool FindSuitableOrigin(ref Point origin)
		{
			TrackGenerator.TrackPlacementState trackPlacementState;
			while ((trackPlacementState = TrackGenerator.CalculateStateForLocation(origin.X, origin.Y)) != TrackGenerator.TrackPlacementState.Obstructed)
			{
				origin.Y++;
				if (trackPlacementState == TrackGenerator.TrackPlacementState.Invalid)
				{
					return false;
				}
			}
			origin.Y--;
			return TrackGenerator.CalculateStateForLocation(origin.X, origin.Y) == TrackGenerator.TrackPlacementState.Available;
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x005F6804 File Offset: 0x005F4A04
		private static TrackGenerator.TrackPlacementState CalculateStateForLocation(int x, int y)
		{
			for (int i = 0; i < 6; i++)
			{
				if (TrackGenerator.IsLocationInvalid(x, y - i))
				{
					return TrackGenerator.TrackPlacementState.Invalid;
				}
			}
			for (int j = 0; j < 6; j++)
			{
				if (TrackGenerator.IsMinecartTrack(x, y + j))
				{
					return TrackGenerator.TrackPlacementState.Invalid;
				}
			}
			for (int k = 0; k < 6; k++)
			{
				if (WorldGen.SolidTile(x, y - k, false))
				{
					return TrackGenerator.TrackPlacementState.Obstructed;
				}
			}
			if (WorldGen.IsTileNearby(x, y, 314, 30))
			{
				return TrackGenerator.TrackPlacementState.Invalid;
			}
			return TrackGenerator.TrackPlacementState.Available;
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x005F6870 File Offset: 0x005F4A70
		private static bool IsMinecartTrack(int x, int y)
		{
			return Main.tile[x, y].active() && Main.tile[x, y].type == 314;
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x005F68A0 File Offset: 0x005F4AA0
		private static bool IsLocationInvalid(int x, int y)
		{
			if (y > Main.UnderworldLayer || x < 5 || y < (int)Main.worldSurface || x > Main.maxTilesX - 5)
			{
				return true;
			}
			if (Math.Abs((double)x - GenVars.shimmerPosition.X) < (double)(WorldGen.shimmerSafetyDistance / 2) && Math.Abs((double)y - GenVars.shimmerPosition.Y) < (double)(WorldGen.shimmerSafetyDistance / 2))
			{
				return true;
			}
			if (WorldGen.oceanDepths(x, y))
			{
				return true;
			}
			if (WorldGen.SecretSeed.dualDungeons.Enabled && DungeonUtils.InAnyPotentialDungeonBounds(x, y, 0, false))
			{
				return true;
			}
			ushort wall = Main.tile[x, y].wall;
			for (int i = 0; i < TrackGenerator.InvalidWalls.Length; i++)
			{
				if (wall == TrackGenerator.InvalidWalls[i] && (!WorldGen.notTheBees || wall != 108))
				{
					return true;
				}
			}
			int num = Main.tile[x, y].active() ? ((int)Main.tile[x, y].type) : -1;
			for (int j = 0; j < TrackGenerator.InvalidTiles.Length; j++)
			{
				if (num == (int)TrackGenerator.InvalidTiles[j])
				{
					return true;
				}
			}
			for (int k = -1; k <= 1; k++)
			{
				if (Main.tile[x + k, y].active() && (Main.tile[x + k, y].type == 314 || !TileID.Sets.GeneralPlacementTiles[(int)Main.tile[x + k, y].type]) && (!WorldGen.notTheBees || Main.tile[x + k, y].type != 225))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x00009E06 File Offset: 0x00008006
		[Conditional("DEBUG")]
		private void DrawPause()
		{
		}

		// Token: 0x040058AB RID: 22699
		private static readonly ushort[] InvalidWalls = new ushort[]
		{
			7,
			94,
			95,
			8,
			98,
			99,
			9,
			96,
			97,
			3,
			83,
			68,
			62,
			78,
			87,
			86,
			42,
			74,
			27,
			149
		};

		// Token: 0x040058AC RID: 22700
		private static readonly ushort[] InvalidTiles = new ushort[]
		{
			383,
			384,
			15,
			304,
			30,
			321,
			245,
			246,
			240,
			241,
			242,
			16,
			34,
			158,
			377,
			94,
			10,
			19,
			86,
			219,
			484,
			190,
			664,
			665,
			41,
			43,
			44,
			226,
			237,
			711,
			712,
			713,
			714,
			715,
			716,
			379,
			314
		};

		// Token: 0x040058AD RID: 22701
		private readonly TrackGenerator.TrackHistory[] _history = new TrackGenerator.TrackHistory[4096];

		// Token: 0x040058AE RID: 22702
		private readonly TrackGenerator.TrackHistory[] _rewriteHistory = new TrackGenerator.TrackHistory[25];

		// Token: 0x040058AF RID: 22703
		private int _xDirection;

		// Token: 0x040058B0 RID: 22704
		private int _length;

		// Token: 0x040058B1 RID: 22705
		private int playerHeight = 6;

		// Token: 0x02000979 RID: 2425
		private enum TrackPlacementState
		{
			// Token: 0x040075E3 RID: 30179
			Available,
			// Token: 0x040075E4 RID: 30180
			Obstructed,
			// Token: 0x040075E5 RID: 30181
			Invalid
		}

		// Token: 0x0200097A RID: 2426
		private enum TrackSlope : sbyte
		{
			// Token: 0x040075E7 RID: 30183
			Up = -1,
			// Token: 0x040075E8 RID: 30184
			Straight,
			// Token: 0x040075E9 RID: 30185
			Down
		}

		// Token: 0x0200097B RID: 2427
		private enum TrackMode : byte
		{
			// Token: 0x040075EB RID: 30187
			Normal,
			// Token: 0x040075EC RID: 30188
			Tunnel
		}

		// Token: 0x0200097C RID: 2428
		[DebuggerDisplay("X = {X}, Y = {Y}, Slope = {Slope}")]
		private struct TrackHistory
		{
			// Token: 0x06004931 RID: 18737 RVA: 0x006CF625 File Offset: 0x006CD825
			public TrackHistory(int x, int y, TrackGenerator.TrackSlope slope)
			{
				this.X = (short)x;
				this.Y = (short)y;
				this.Slope = slope;
				this.Mode = TrackGenerator.TrackMode.Normal;
			}

			// Token: 0x040075ED RID: 30189
			public short X;

			// Token: 0x040075EE RID: 30190
			public short Y;

			// Token: 0x040075EF RID: 30191
			public TrackGenerator.TrackSlope Slope;

			// Token: 0x040075F0 RID: 30192
			public TrackGenerator.TrackMode Mode;
		}
	}
}
