using System;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent
{
	// Token: 0x02000261 RID: 609
	public struct PlayerSittingHelper
	{
		// Token: 0x06002377 RID: 9079 RVA: 0x0053DFA4 File Offset: 0x0053C1A4
		public void GetSittingOffsetInfo(Player player, out Vector2 posOffset, out float seatAdjustment)
		{
			if (this.isSitting)
			{
				posOffset = new Vector2((float)(this.sittingIndex * player.direction * 8), (float)this.sittingIndex * player.gravDir * -4f);
				seatAdjustment = -4f;
				seatAdjustment += (float)((int)this.offsetForSeat.Y);
				posOffset += this.offsetForSeat * player.Directions;
				return;
			}
			posOffset = Vector2.Zero;
			seatAdjustment = 0f;
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x0053E038 File Offset: 0x0053C238
		public bool TryGetSittingBlock(Player player, out Tile tile)
		{
			tile = null;
			if (!this.isSitting)
			{
				return false;
			}
			Point point = (player.Bottom + new Vector2(0f, -2f)).ToTileCoordinates();
			int num;
			Vector2 vector;
			Vector2 vector2;
			ExtraSeatInfo extraSeatInfo;
			if (!PlayerSittingHelper.GetSittingTargetInfo(player, point.X, point.Y, out num, out vector, out vector2, out extraSeatInfo))
			{
				return false;
			}
			tile = Framing.GetTileSafely(point);
			return true;
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x0053E09C File Offset: 0x0053C29C
		public void UpdateSitting(Player player)
		{
			if (!this.isSitting)
			{
				return;
			}
			Point point = (player.Bottom + new Vector2(0f, -2f)).ToTileCoordinates();
			int num;
			Vector2 vector;
			Vector2 vector2;
			ExtraSeatInfo extraSeatInfo;
			if (!PlayerSittingHelper.GetSittingTargetInfo(player, point.X, point.Y, out num, out vector, out vector2, out extraSeatInfo))
			{
				this.SitUp(player, true);
				return;
			}
			if (player.controlLeft || player.controlRight || player.controlUp || player.controlDown || player.controlJump || player.pulley || player.mount.Active || num != player.direction)
			{
				this.SitUp(player, true);
			}
			if (Main.sittingManager.GetNextPlayerStackIndexInCoords(point) >= 2)
			{
				this.SitUp(player, true);
			}
			if (!this.isSitting)
			{
				return;
			}
			if (Main.netMode != 1 && !Main.IsItDay())
			{
				int num2 = 2322;
				int num3 = 2358;
				Tile tile = Main.tile[point.X, point.Y];
				if (tile.type == 89 && (int)tile.frameX >= num2 && (int)tile.frameX <= num3)
				{
					NPC.RedHatSkeletron(player.whoAmI);
				}
			}
			this.offsetForSeat = vector2;
			this.details = extraSeatInfo;
			Main.sittingManager.AddPlayerAndGetItsStackedIndexInCoords(player.whoAmI, point, out this.sittingIndex);
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x0053E1F0 File Offset: 0x0053C3F0
		public void SitUp(Player player, bool multiplayerBroadcast = true)
		{
			if (!this.isSitting)
			{
				return;
			}
			this.isSitting = false;
			this.offsetForSeat = Vector2.Zero;
			this.sittingIndex = -1;
			this.details = default(ExtraSeatInfo);
			if (multiplayerBroadcast && Main.myPlayer == player.whoAmI)
			{
				NetMessage.SendData(13, -1, -1, null, player.whoAmI, 0f, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x0053E260 File Offset: 0x0053C460
		public void SitDown(Player player, int x, int y)
		{
			int dir;
			Vector2 vector;
			Vector2 vector2;
			ExtraSeatInfo extraSeatInfo;
			if (!PlayerSittingHelper.GetSittingTargetInfo(player, x, y, out dir, out vector, out vector2, out extraSeatInfo))
			{
				return;
			}
			Vector2 offset = vector - player.Bottom;
			bool flag = player.CanSnapToPosition(offset);
			if (flag)
			{
				flag &= (Main.sittingManager.GetNextPlayerStackIndexInCoords((vector + new Vector2(0f, -2f)).ToTileCoordinates()) < 2);
			}
			if (flag)
			{
				if (this.isSitting && player.Bottom == vector)
				{
					this.SitUp(player, true);
					return;
				}
				player.StopVanityActions(true);
				player.RemoveAllGrapplingHooks();
				if (player.mount.Active)
				{
					player.mount.TryDismount(player);
				}
				player.Bottom = vector;
				player.ChangeDir(dir);
				this.isSitting = true;
				this.details = extraSeatInfo;
				this.offsetForSeat = vector2;
				Main.sittingManager.AddPlayerAndGetItsStackedIndexInCoords(player.whoAmI, new Point(x, y), out this.sittingIndex);
				player.velocity = Vector2.Zero;
				player.gravDir = 1f;
				if (Main.myPlayer == player.whoAmI)
				{
					NetMessage.SendData(13, -1, -1, null, player.whoAmI, 0f, 0f, 0f, 0, 0, 0);
				}
			}
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x0053E39C File Offset: 0x0053C59C
		public static bool GetSittingTargetInfo(Player player, int x, int y, out int targetDirection, out Vector2 playerSittingPosition, out Vector2 seatDownOffset, out ExtraSeatInfo extraInfo)
		{
			extraInfo = default(ExtraSeatInfo);
			Tile tileSafely = Framing.GetTileSafely(x, y);
			if (!TileID.Sets.CanBeSatOnForPlayers[(int)tileSafely.type] || !tileSafely.active())
			{
				targetDirection = 1;
				seatDownOffset = Vector2.Zero;
				playerSittingPosition = default(Vector2);
				return false;
			}
			int num = x;
			int num2 = y;
			targetDirection = 1;
			seatDownOffset = Vector2.Zero;
			int num3 = 6;
			Vector2 zero = Vector2.Zero;
			ushort type = tileSafely.type;
			if (type <= 89)
			{
				if (type != 15)
				{
					if (type != 89)
					{
						goto IL_45B;
					}
					targetDirection = player.direction;
					num3 = 0;
					Vector2 vector = new Vector2(-4f, 2f);
					Vector2 vector2 = new Vector2(4f, 2f);
					Vector2 vector3 = new Vector2(0f, 2f);
					Vector2 zero2 = Vector2.Zero;
					zero2.X = 1f;
					zero.X = -1f;
					switch (tileSafely.frameX / 54)
					{
					case 0:
						vector3.Y = (vector.Y = (vector2.Y = 1f));
						break;
					case 1:
						vector3.Y = 1f;
						break;
					case 2:
					case 14:
					case 15:
					case 17:
					case 20:
					case 21:
					case 22:
					case 23:
					case 25:
					case 26:
					case 27:
					case 28:
					case 35:
					case 37:
					case 38:
					case 39:
					case 40:
					case 41:
					case 42:
						vector3.Y = (vector.Y = (vector2.Y = 1f));
						break;
					case 3:
					case 4:
					case 5:
					case 7:
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 13:
					case 16:
					case 18:
					case 19:
					case 36:
						vector3.Y = (vector.Y = (vector2.Y = 0f));
						break;
					case 6:
						vector3.Y = (vector.Y = (vector2.Y = -1f));
						break;
					case 24:
						vector3.Y = 0f;
						vector.Y = -4f;
						vector.X = 0f;
						vector2.X = 0f;
						vector2.Y = -4f;
						break;
					}
					if (tileSafely.frameY % 40 != 0)
					{
						num2--;
					}
					if ((tileSafely.frameX % 54 == 0 && targetDirection == -1) || (tileSafely.frameX % 54 == 36 && targetDirection == 1))
					{
						seatDownOffset = vector;
					}
					else if ((tileSafely.frameX % 54 == 0 && targetDirection == 1) || (tileSafely.frameX % 54 == 36 && targetDirection == -1))
					{
						seatDownOffset = vector2;
					}
					else
					{
						seatDownOffset = vector3;
					}
					seatDownOffset += zero2;
					goto IL_45B;
				}
			}
			else
			{
				if (type == 102)
				{
					short num4 = tileSafely.frameX / 18;
					if (num4 == 0)
					{
						num++;
					}
					if (num4 == 2)
					{
						num--;
					}
					short num5 = tileSafely.frameY / 18;
					if (num5 == 0)
					{
						num2 += 2;
					}
					if (num5 == 1)
					{
						num2++;
					}
					if (num5 == 3)
					{
						num2--;
					}
					targetDirection = player.direction;
					num3 = 0;
					goto IL_45B;
				}
				if (type == 487)
				{
					int num6 = (int)(tileSafely.frameX % 72 / 18);
					if (num6 == 1)
					{
						num--;
					}
					if (num6 == 2)
					{
						num++;
					}
					if (tileSafely.frameY / 18 != 0)
					{
						num2--;
					}
					targetDirection = (num6 <= 1).ToDirectionInt();
					num3 = 0;
					seatDownOffset.Y -= 1f;
					goto IL_45B;
				}
				if (type != 497)
				{
					goto IL_45B;
				}
			}
			bool flag = tileSafely.type == 15 && (tileSafely.frameY / 40 == 1 || tileSafely.frameY / 40 == 20);
			bool value = tileSafely.type == 15 && tileSafely.frameY / 40 == 27;
			seatDownOffset.Y = (float)(value.ToInt() * 4);
			if (tileSafely.frameY % 40 != 0)
			{
				num2--;
			}
			targetDirection = -1;
			if (tileSafely.frameX != 0)
			{
				targetDirection = 1;
			}
			if (flag || tileSafely.type == 497)
			{
				extraInfo.IsAToilet = true;
			}
			IL_45B:
			playerSittingPosition = new Point(num, num2 + 1).ToWorldCoordinates(8f, 16f);
			playerSittingPosition.X += (float)(targetDirection * num3);
			playerSittingPosition += zero;
			return true;
		}

		// Token: 0x04004D66 RID: 19814
		public const int ChairSittingMaxDistance = 40;

		// Token: 0x04004D67 RID: 19815
		public bool isSitting;

		// Token: 0x04004D68 RID: 19816
		public ExtraSeatInfo details;

		// Token: 0x04004D69 RID: 19817
		public Vector2 offsetForSeat;

		// Token: 0x04004D6A RID: 19818
		public int sittingIndex;
	}
}
