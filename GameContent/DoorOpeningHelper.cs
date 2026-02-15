using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameInput;

namespace Terraria.GameContent
{
	// Token: 0x0200024E RID: 590
	public class DoorOpeningHelper
	{
		// Token: 0x06002309 RID: 8969 RVA: 0x0053B8A3 File Offset: 0x00539AA3
		public void AllowOpeningDoorsByVelocityAloneForATime(int timeInFramesToAllow)
		{
			this._timeWeCanOpenDoorsUsingVelocityAlone = timeInFramesToAllow;
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x0053B8AC File Offset: 0x00539AAC
		public void Update(Player player)
		{
			this.LookForDoorsToClose(player);
			if (this.ShouldTryOpeningDoors())
			{
				this.LookForDoorsToOpen(player);
			}
			if (this._timeWeCanOpenDoorsUsingVelocityAlone > 0)
			{
				this._timeWeCanOpenDoorsUsingVelocityAlone--;
			}
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x0053B8DC File Offset: 0x00539ADC
		private bool ShouldTryOpeningDoors()
		{
			switch (DoorOpeningHelper.PreferenceSettings)
			{
			default:
				return false;
			case DoorOpeningHelper.DoorAutoOpeningPreference.EnabledForGamepadOnly:
				return PlayerInput.UsingGamepad;
			case DoorOpeningHelper.DoorAutoOpeningPreference.EnabledForEverything:
				return true;
			}
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x0053B90C File Offset: 0x00539B0C
		public static void CyclePreferences()
		{
			switch (DoorOpeningHelper.PreferenceSettings)
			{
			case DoorOpeningHelper.DoorAutoOpeningPreference.Disabled:
				DoorOpeningHelper.PreferenceSettings = DoorOpeningHelper.DoorAutoOpeningPreference.EnabledForEverything;
				return;
			case DoorOpeningHelper.DoorAutoOpeningPreference.EnabledForGamepadOnly:
				DoorOpeningHelper.PreferenceSettings = DoorOpeningHelper.DoorAutoOpeningPreference.Disabled;
				return;
			case DoorOpeningHelper.DoorAutoOpeningPreference.EnabledForEverything:
				DoorOpeningHelper.PreferenceSettings = DoorOpeningHelper.DoorAutoOpeningPreference.EnabledForGamepadOnly;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x0053B948 File Offset: 0x00539B48
		public void LookForDoorsToClose(Player player)
		{
			DoorOpeningHelper.PlayerInfoForClosingDoors playerInfoForClosingDoor = this.GetPlayerInfoForClosingDoor(player);
			for (int i = this._ongoingOpenDoors.Count - 1; i >= 0; i--)
			{
				DoorOpeningHelper.DoorOpenCloseTogglingInfo doorOpenCloseTogglingInfo = this._ongoingOpenDoors[i];
				DoorOpeningHelper.DoorCloseAttemptResult doorCloseAttemptResult = doorOpenCloseTogglingInfo.handler.TryCloseDoor(doorOpenCloseTogglingInfo, playerInfoForClosingDoor);
				if (doorCloseAttemptResult != DoorOpeningHelper.DoorCloseAttemptResult.StillInDoorArea)
				{
					this._ongoingOpenDoors.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x0053B9A0 File Offset: 0x00539BA0
		private DoorOpeningHelper.PlayerInfoForClosingDoors GetPlayerInfoForClosingDoor(Player player)
		{
			return new DoorOpeningHelper.PlayerInfoForClosingDoors
			{
				hitboxToNotCloseDoor = player.Hitbox
			};
		}

		// Token: 0x0600230F RID: 8975 RVA: 0x0053B9C4 File Offset: 0x00539BC4
		public void LookForDoorsToOpen(Player player)
		{
			DoorOpeningHelper.PlayerInfoForOpeningDoors playerInfoForOpeningDoor = this.GetPlayerInfoForOpeningDoor(player);
			if (playerInfoForOpeningDoor.intendedOpeningDirection == 0 && player.velocity.X == 0f)
			{
				return;
			}
			Point tileCoords = default(Point);
			for (int i = playerInfoForOpeningDoor.tileCoordSpaceForCheckingForDoors.Left; i <= playerInfoForOpeningDoor.tileCoordSpaceForCheckingForDoors.Right; i++)
			{
				for (int j = playerInfoForOpeningDoor.tileCoordSpaceForCheckingForDoors.Top; j <= playerInfoForOpeningDoor.tileCoordSpaceForCheckingForDoors.Bottom; j++)
				{
					tileCoords.X = i;
					tileCoords.Y = j;
					this.TryAutoOpeningDoor(tileCoords, playerInfoForOpeningDoor);
				}
			}
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x0053BA58 File Offset: 0x00539C58
		private DoorOpeningHelper.PlayerInfoForOpeningDoors GetPlayerInfoForOpeningDoor(Player player)
		{
			int num = player.controlRight.ToInt() - player.controlLeft.ToInt();
			int playerGravityDirection = (int)player.gravDir;
			Rectangle hitbox = player.Hitbox;
			hitbox.Y -= -1;
			hitbox.Height += -2;
			float num2 = player.GetAutoDoorVelocityContribution();
			if (num == 0 && this._timeWeCanOpenDoorsUsingVelocityAlone == 0)
			{
				num2 = 0f;
			}
			float value = (float)num + num2;
			int num3 = Math.Sign(value) * (int)Math.Ceiling((double)Math.Abs(value));
			hitbox.X += num3;
			if (num == 0)
			{
				num = Math.Sign(value);
			}
			Rectangle hitbox2;
			Rectangle value2 = hitbox2 = player.Hitbox;
			hitbox2.X += num3;
			Rectangle r = Rectangle.Union(value2, hitbox2);
			Point point = r.TopLeft().ToTileCoordinates();
			Point point2 = r.BottomRight().ToTileCoordinates();
			Rectangle tileCoordSpaceForCheckingForDoors = new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
			return new DoorOpeningHelper.PlayerInfoForOpeningDoors
			{
				hitboxToOpenDoor = hitbox,
				intendedOpeningDirection = num,
				playerGravityDirection = playerGravityDirection,
				tileCoordSpaceForCheckingForDoors = tileCoordSpaceForCheckingForDoors
			};
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x0053BB88 File Offset: 0x00539D88
		private void TryAutoOpeningDoor(Point tileCoords, DoorOpeningHelper.PlayerInfoForOpeningDoors playerInfo)
		{
			DoorOpeningHelper.DoorAutoHandler doorAutoHandler;
			if (!this.TryGetHandler(tileCoords, out doorAutoHandler))
			{
				return;
			}
			DoorOpeningHelper.DoorOpenCloseTogglingInfo doorOpenCloseTogglingInfo = doorAutoHandler.ProvideInfo(tileCoords);
			if (!doorAutoHandler.TryOpenDoor(doorOpenCloseTogglingInfo, playerInfo))
			{
				return;
			}
			this._ongoingOpenDoors.Add(doorOpenCloseTogglingInfo);
		}

		// Token: 0x06002312 RID: 8978 RVA: 0x0053BBC0 File Offset: 0x00539DC0
		private bool TryGetHandler(Point tileCoords, out DoorOpeningHelper.DoorAutoHandler infoProvider)
		{
			infoProvider = null;
			if (!WorldGen.InWorld(tileCoords.X, tileCoords.Y, 3))
			{
				return false;
			}
			Tile tile = Main.tile[tileCoords.X, tileCoords.Y];
			return tile != null && this._handlerByTileType.TryGetValue((int)tile.type, out infoProvider);
		}

		// Token: 0x04004D23 RID: 19747
		public static DoorOpeningHelper.DoorAutoOpeningPreference PreferenceSettings = DoorOpeningHelper.DoorAutoOpeningPreference.EnabledForEverything;

		// Token: 0x04004D24 RID: 19748
		private Dictionary<int, DoorOpeningHelper.DoorAutoHandler> _handlerByTileType = new Dictionary<int, DoorOpeningHelper.DoorAutoHandler>
		{
			{
				10,
				new DoorOpeningHelper.CommonDoorOpeningInfoProvider()
			},
			{
				388,
				new DoorOpeningHelper.TallGateOpeningInfoProvider()
			}
		};

		// Token: 0x04004D25 RID: 19749
		private List<DoorOpeningHelper.DoorOpenCloseTogglingInfo> _ongoingOpenDoors = new List<DoorOpeningHelper.DoorOpenCloseTogglingInfo>();

		// Token: 0x04004D26 RID: 19750
		private int _timeWeCanOpenDoorsUsingVelocityAlone;

		// Token: 0x020007D4 RID: 2004
		public enum DoorAutoOpeningPreference
		{
			// Token: 0x040070D8 RID: 28888
			Disabled,
			// Token: 0x040070D9 RID: 28889
			EnabledForGamepadOnly,
			// Token: 0x040070DA RID: 28890
			EnabledForEverything
		}

		// Token: 0x020007D5 RID: 2005
		private enum DoorCloseAttemptResult
		{
			// Token: 0x040070DC RID: 28892
			StillInDoorArea,
			// Token: 0x040070DD RID: 28893
			ClosedDoor,
			// Token: 0x040070DE RID: 28894
			FailedToCloseDoor,
			// Token: 0x040070DF RID: 28895
			DoorIsInvalidated
		}

		// Token: 0x020007D6 RID: 2006
		private struct DoorOpenCloseTogglingInfo
		{
			// Token: 0x040070E0 RID: 28896
			public Point tileCoordsForToggling;

			// Token: 0x040070E1 RID: 28897
			public DoorOpeningHelper.DoorAutoHandler handler;
		}

		// Token: 0x020007D7 RID: 2007
		private struct PlayerInfoForOpeningDoors
		{
			// Token: 0x040070E2 RID: 28898
			public Rectangle hitboxToOpenDoor;

			// Token: 0x040070E3 RID: 28899
			public int intendedOpeningDirection;

			// Token: 0x040070E4 RID: 28900
			public int playerGravityDirection;

			// Token: 0x040070E5 RID: 28901
			public Rectangle tileCoordSpaceForCheckingForDoors;
		}

		// Token: 0x020007D8 RID: 2008
		private struct PlayerInfoForClosingDoors
		{
			// Token: 0x040070E6 RID: 28902
			public Rectangle hitboxToNotCloseDoor;
		}

		// Token: 0x020007D9 RID: 2009
		private interface DoorAutoHandler
		{
			// Token: 0x06004233 RID: 16947
			DoorOpeningHelper.DoorOpenCloseTogglingInfo ProvideInfo(Point tileCoords);

			// Token: 0x06004234 RID: 16948
			bool TryOpenDoor(DoorOpeningHelper.DoorOpenCloseTogglingInfo info, DoorOpeningHelper.PlayerInfoForOpeningDoors playerInfo);

			// Token: 0x06004235 RID: 16949
			DoorOpeningHelper.DoorCloseAttemptResult TryCloseDoor(DoorOpeningHelper.DoorOpenCloseTogglingInfo info, DoorOpeningHelper.PlayerInfoForClosingDoors playerInfo);
		}

		// Token: 0x020007DA RID: 2010
		private class CommonDoorOpeningInfoProvider : DoorOpeningHelper.DoorAutoHandler
		{
			// Token: 0x06004236 RID: 16950 RVA: 0x006BCAB4 File Offset: 0x006BACB4
			public DoorOpeningHelper.DoorOpenCloseTogglingInfo ProvideInfo(Point tileCoords)
			{
				Tile tile = Main.tile[tileCoords.X, tileCoords.Y];
				Point tileCoordsForToggling = tileCoords;
				tileCoordsForToggling.Y -= (int)(tile.frameY % 54 / 18);
				return new DoorOpeningHelper.DoorOpenCloseTogglingInfo
				{
					handler = this,
					tileCoordsForToggling = tileCoordsForToggling
				};
			}

			// Token: 0x06004237 RID: 16951 RVA: 0x006BCB0C File Offset: 0x006BAD0C
			public bool TryOpenDoor(DoorOpeningHelper.DoorOpenCloseTogglingInfo doorInfo, DoorOpeningHelper.PlayerInfoForOpeningDoors playerInfo)
			{
				Point tileCoordsForToggling = doorInfo.tileCoordsForToggling;
				int intendedOpeningDirection = playerInfo.intendedOpeningDirection;
				Rectangle rectangle = new Rectangle(doorInfo.tileCoordsForToggling.X * 16, doorInfo.tileCoordsForToggling.Y * 16, 16, 48);
				int playerGravityDirection = playerInfo.playerGravityDirection;
				if (playerGravityDirection != -1)
				{
					if (playerGravityDirection == 1)
					{
						rectangle.Height += 16;
					}
				}
				else
				{
					rectangle.Y -= 16;
					rectangle.Height += 16;
				}
				if (!rectangle.Intersects(playerInfo.hitboxToOpenDoor))
				{
					return false;
				}
				if (playerInfo.hitboxToOpenDoor.Top < rectangle.Top || playerInfo.hitboxToOpenDoor.Bottom > rectangle.Bottom)
				{
					return false;
				}
				WorldGen.OpenDoor(tileCoordsForToggling.X, tileCoordsForToggling.Y, intendedOpeningDirection);
				if (Main.tile[tileCoordsForToggling.X, tileCoordsForToggling.Y].type != 10)
				{
					NetMessage.SendData(19, -1, -1, null, 0, (float)tileCoordsForToggling.X, (float)tileCoordsForToggling.Y, (float)intendedOpeningDirection, 0, 0, 0);
					return true;
				}
				WorldGen.OpenDoor(tileCoordsForToggling.X, tileCoordsForToggling.Y, -intendedOpeningDirection);
				if (Main.tile[tileCoordsForToggling.X, tileCoordsForToggling.Y].type != 10)
				{
					NetMessage.SendData(19, -1, -1, null, 0, (float)tileCoordsForToggling.X, (float)tileCoordsForToggling.Y, (float)(-(float)intendedOpeningDirection), 0, 0, 0);
					return true;
				}
				return false;
			}

			// Token: 0x06004238 RID: 16952 RVA: 0x006BCC6C File Offset: 0x006BAE6C
			public DoorOpeningHelper.DoorCloseAttemptResult TryCloseDoor(DoorOpeningHelper.DoorOpenCloseTogglingInfo info, DoorOpeningHelper.PlayerInfoForClosingDoors playerInfo)
			{
				Point tileCoordsForToggling = info.tileCoordsForToggling;
				Tile tile = Main.tile[tileCoordsForToggling.X, tileCoordsForToggling.Y];
				if (!tile.active() || tile.type != 11)
				{
					return DoorOpeningHelper.DoorCloseAttemptResult.DoorIsInvalidated;
				}
				int num = (int)(tile.frameX % 72 / 18);
				Rectangle value = new Rectangle(tileCoordsForToggling.X * 16, tileCoordsForToggling.Y * 16, 16, 48);
				if (num != 1)
				{
					if (num == 2)
					{
						value.X += 16;
					}
				}
				else
				{
					value.X -= 16;
				}
				value.Inflate(1, 0);
				Rectangle rectangle = Rectangle.Intersect(value, playerInfo.hitboxToNotCloseDoor);
				if (rectangle.Width > 0 || rectangle.Height > 0)
				{
					return DoorOpeningHelper.DoorCloseAttemptResult.StillInDoorArea;
				}
				if (WorldGen.CloseDoor(tileCoordsForToggling.X, tileCoordsForToggling.Y, false))
				{
					NetMessage.SendData(13, -1, -1, null, Main.myPlayer, 0f, 0f, 0f, 0, 0, 0);
					NetMessage.SendData(19, -1, -1, null, 1, (float)tileCoordsForToggling.X, (float)tileCoordsForToggling.Y, 1f, 0, 0, 0);
					return DoorOpeningHelper.DoorCloseAttemptResult.ClosedDoor;
				}
				return DoorOpeningHelper.DoorCloseAttemptResult.FailedToCloseDoor;
			}
		}

		// Token: 0x020007DB RID: 2011
		private class TallGateOpeningInfoProvider : DoorOpeningHelper.DoorAutoHandler
		{
			// Token: 0x0600423A RID: 16954 RVA: 0x006BCD84 File Offset: 0x006BAF84
			public DoorOpeningHelper.DoorOpenCloseTogglingInfo ProvideInfo(Point tileCoords)
			{
				Tile tile = Main.tile[tileCoords.X, tileCoords.Y];
				Point tileCoordsForToggling = tileCoords;
				tileCoordsForToggling.Y -= (int)(tile.frameY % 90 / 18);
				return new DoorOpeningHelper.DoorOpenCloseTogglingInfo
				{
					handler = this,
					tileCoordsForToggling = tileCoordsForToggling
				};
			}

			// Token: 0x0600423B RID: 16955 RVA: 0x006BCDDC File Offset: 0x006BAFDC
			public bool TryOpenDoor(DoorOpeningHelper.DoorOpenCloseTogglingInfo doorInfo, DoorOpeningHelper.PlayerInfoForOpeningDoors playerInfo)
			{
				Point tileCoordsForToggling = doorInfo.tileCoordsForToggling;
				Rectangle rectangle = new Rectangle(doorInfo.tileCoordsForToggling.X * 16, doorInfo.tileCoordsForToggling.Y * 16, 16, 80);
				int playerGravityDirection = playerInfo.playerGravityDirection;
				if (playerGravityDirection != -1)
				{
					if (playerGravityDirection == 1)
					{
						rectangle.Height += 16;
					}
				}
				else
				{
					rectangle.Y -= 16;
					rectangle.Height += 16;
				}
				if (!rectangle.Intersects(playerInfo.hitboxToOpenDoor))
				{
					return false;
				}
				if (playerInfo.hitboxToOpenDoor.Top < rectangle.Top || playerInfo.hitboxToOpenDoor.Bottom > rectangle.Bottom)
				{
					return false;
				}
				bool flag = false;
				if (WorldGen.ShiftTallGate(tileCoordsForToggling.X, tileCoordsForToggling.Y, flag, false))
				{
					NetMessage.SendData(19, -1, -1, null, 4 + flag.ToInt(), (float)tileCoordsForToggling.X, (float)tileCoordsForToggling.Y, 0f, 0, 0, 0);
					return true;
				}
				return false;
			}

			// Token: 0x0600423C RID: 16956 RVA: 0x006BCED0 File Offset: 0x006BB0D0
			public DoorOpeningHelper.DoorCloseAttemptResult TryCloseDoor(DoorOpeningHelper.DoorOpenCloseTogglingInfo info, DoorOpeningHelper.PlayerInfoForClosingDoors playerInfo)
			{
				Point tileCoordsForToggling = info.tileCoordsForToggling;
				Tile tile = Main.tile[tileCoordsForToggling.X, tileCoordsForToggling.Y];
				if (!tile.active() || tile.type != 389)
				{
					return DoorOpeningHelper.DoorCloseAttemptResult.DoorIsInvalidated;
				}
				short num = tile.frameY % 90 / 18;
				Rectangle value = new Rectangle(tileCoordsForToggling.X * 16, tileCoordsForToggling.Y * 16, 16, 80);
				value.Inflate(1, 0);
				Rectangle rectangle = Rectangle.Intersect(value, playerInfo.hitboxToNotCloseDoor);
				if (rectangle.Width > 0 || rectangle.Height > 0)
				{
					return DoorOpeningHelper.DoorCloseAttemptResult.StillInDoorArea;
				}
				bool flag = true;
				if (WorldGen.ShiftTallGate(tileCoordsForToggling.X, tileCoordsForToggling.Y, flag, false))
				{
					NetMessage.SendData(13, -1, -1, null, Main.myPlayer, 0f, 0f, 0f, 0, 0, 0);
					NetMessage.SendData(19, -1, -1, null, 4 + flag.ToInt(), (float)tileCoordsForToggling.X, (float)tileCoordsForToggling.Y, 0f, 0, 0, 0);
					return DoorOpeningHelper.DoorCloseAttemptResult.ClosedDoor;
				}
				return DoorOpeningHelper.DoorCloseAttemptResult.FailedToCloseDoor;
			}
		}
	}
}
