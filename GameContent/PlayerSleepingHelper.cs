using System;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent
{
	// Token: 0x02000262 RID: 610
	public struct PlayerSleepingHelper
	{
		// Token: 0x17000376 RID: 886
		// (get) Token: 0x0600237D RID: 9085 RVA: 0x0053E849 File Offset: 0x0053CA49
		public bool FullyFallenAsleep
		{
			get
			{
				return this.isSleeping && this.timeSleeping >= 120;
			}
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x0053E864 File Offset: 0x0053CA64
		public void GetSleepingOffsetInfo(Player player, out Vector2 posOffset)
		{
			if (this.isSleeping)
			{
				posOffset = this.visualOffsetOfBedBase * player.Directions + new Vector2(0f, (float)this.sleepingIndex * player.gravDir * -4f);
				return;
			}
			posOffset = Vector2.Zero;
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x0053E8BF File Offset: 0x0053CABF
		private bool DoesPlayerHaveReasonToActUpInBed(Player player)
		{
			return NPC.AnyDanger(true, false) || (Main.bloodMoon && !Main.dayTime) || (Main.eclipse && Main.dayTime) || player.itemAnimation > 0;
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x0053E8F8 File Offset: 0x0053CAF8
		public void SetIsSleepingAndAdjustPlayerRotation(Player player, bool state)
		{
			if (this.isSleeping == state)
			{
				return;
			}
			this.isSleeping = state;
			if (state)
			{
				player.fullRotation = 1.5707964f * (float)(-(float)player.direction);
				player.fullRotationOrigin = player.Size / 2f;
				return;
			}
			player.fullRotation = 0f;
			player.fullRotationOrigin = Vector2.Zero;
			this.visualOffsetOfBedBase = default(Vector2);
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x0053E968 File Offset: 0x0053CB68
		public void UpdateState(Player player)
		{
			if (!this.isSleeping)
			{
				this.timeSleeping = 0;
				return;
			}
			this.timeSleeping++;
			if (this.DoesPlayerHaveReasonToActUpInBed(player))
			{
				this.timeSleeping = 0;
			}
			Point point = (player.Bottom + new Vector2(0f, -2f)).ToTileCoordinates();
			int num;
			Vector2 vector;
			Vector2 vector2;
			if (!PlayerSleepingHelper.GetSleepingTargetInfo(point.X, point.Y, out num, out vector, out vector2))
			{
				this.StopSleeping(player, true);
				return;
			}
			if (player.controlLeft || player.controlRight || player.controlUp || player.controlDown || player.controlJump || player.pulley || player.mount.Active || num != player.direction)
			{
				this.StopSleeping(player, true);
			}
			bool flag = false;
			if (player.itemAnimation > 0)
			{
				Item heldItem = player.HeldItem;
				if (heldItem.damage > 0 && !heldItem.noMelee)
				{
					flag = true;
				}
				if (heldItem.fishingPole > 0)
				{
					flag = true;
				}
				bool? flag2 = ItemID.Sets.ForcesBreaksSleeping[heldItem.type];
				if (flag2 != null)
				{
					flag = flag2.Value;
				}
			}
			if (flag)
			{
				this.StopSleeping(player, true);
			}
			if (Main.sleepingManager.GetNextPlayerStackIndexInCoords(point) >= 2)
			{
				this.StopSleeping(player, true);
			}
			if (!this.isSleeping)
			{
				return;
			}
			this.visualOffsetOfBedBase = vector2;
			Main.sleepingManager.AddPlayerAndGetItsStackedIndexInCoords(player.whoAmI, point, out this.sleepingIndex);
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x0053EAE0 File Offset: 0x0053CCE0
		public void StopSleeping(Player player, bool multiplayerBroadcast = true)
		{
			if (!this.isSleeping)
			{
				return;
			}
			this.SetIsSleepingAndAdjustPlayerRotation(player, false);
			this.timeSleeping = 0;
			this.sleepingIndex = -1;
			this.visualOffsetOfBedBase = default(Vector2);
			if (multiplayerBroadcast && Main.myPlayer == player.whoAmI)
			{
				NetMessage.SendData(13, -1, -1, null, player.whoAmI, 0f, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x0053EB4C File Offset: 0x0053CD4C
		public void StartSleeping(Player player, int x, int y)
		{
			int dir;
			Vector2 vector;
			Vector2 vector2;
			PlayerSleepingHelper.GetSleepingTargetInfo(x, y, out dir, out vector, out vector2);
			Vector2 offset = vector - player.Bottom;
			bool flag = player.CanSnapToPosition(offset);
			if (flag)
			{
				flag &= (Main.sleepingManager.GetNextPlayerStackIndexInCoords((vector + new Vector2(0f, -2f)).ToTileCoordinates()) < 2);
			}
			if (!flag)
			{
				return;
			}
			if (this.isSleeping && player.Bottom == vector)
			{
				this.StopSleeping(player, true);
				return;
			}
			player.StopVanityActions(true);
			player.RemoveAllGrapplingHooks();
			player.RemoveAllFishingBobbers();
			if (player.mount.Active)
			{
				player.mount.TryDismount(player);
			}
			player.Bottom = vector;
			player.ChangeDir(dir);
			Main.sleepingManager.AddPlayerAndGetItsStackedIndexInCoords(player.whoAmI, new Point(x, y), out this.sleepingIndex);
			player.velocity = Vector2.Zero;
			player.gravDir = 1f;
			this.SetIsSleepingAndAdjustPlayerRotation(player, true);
			this.visualOffsetOfBedBase = vector2;
			if (Main.myPlayer == player.whoAmI)
			{
				NetMessage.SendData(13, -1, -1, null, player.whoAmI, 0f, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x0053EC80 File Offset: 0x0053CE80
		public static bool GetSleepingTargetInfo(int x, int y, out int targetDirection, out Vector2 anchorPosition, out Vector2 visualoffset)
		{
			Tile tileSafely = Framing.GetTileSafely(x, y);
			if (!TileID.Sets.CanBeSleptIn[(int)tileSafely.type] || !tileSafely.active())
			{
				targetDirection = 1;
				anchorPosition = default(Vector2);
				visualoffset = default(Vector2);
				return false;
			}
			int num = y;
			int num2 = x - (int)(tileSafely.frameX % 72 / 18);
			if (tileSafely.frameY % 36 != 0)
			{
				num--;
			}
			targetDirection = 1;
			int num3 = (int)(tileSafely.frameX / 72);
			int num4 = num2;
			if (num3 != 0)
			{
				if (num3 == 1)
				{
					num4 += 2;
				}
			}
			else
			{
				targetDirection = -1;
				num4++;
			}
			anchorPosition = new Point(num4, num + 1).ToWorldCoordinates(8f, 16f);
			visualoffset = PlayerSleepingHelper.SetOffsetbyBed((int)(tileSafely.frameY / 36));
			return true;
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x0053ED38 File Offset: 0x0053CF38
		private static Vector2 SetOffsetbyBed(int bedStyle)
		{
			switch (bedStyle)
			{
			case 8:
				return new Vector2(-11f, 1f);
			default:
				return new Vector2(-9f, 1f);
			case 10:
				return new Vector2(-9f, -1f);
			case 11:
				return new Vector2(-11f, 1f);
			case 13:
				return new Vector2(-11f, -3f);
			case 15:
			case 16:
			case 17:
				return new Vector2(-7f, -3f);
			case 18:
				return new Vector2(-9f, -3f);
			case 19:
				return new Vector2(-3f, -1f);
			case 20:
				return new Vector2(-9f, -5f);
			case 21:
				return new Vector2(-9f, 5f);
			case 22:
				return new Vector2(-7f, 1f);
			case 23:
				return new Vector2(-5f, -1f);
			case 24:
			case 25:
				return new Vector2(-7f, 1f);
			case 27:
				return new Vector2(-9f, 3f);
			case 28:
				return new Vector2(-9f, 5f);
			case 29:
				return new Vector2(-11f, -1f);
			case 30:
				return new Vector2(-9f, 3f);
			case 31:
				return new Vector2(-7f, 5f);
			case 32:
				return new Vector2(-7f, -1f);
			case 34:
			case 35:
			case 36:
			case 37:
				return new Vector2(-13f, 1f);
			case 38:
				return new Vector2(-11f, -3f);
			}
		}

		// Token: 0x04004D6B RID: 19819
		public const int BedSleepingMaxDistance = 96;

		// Token: 0x04004D6C RID: 19820
		public const int TimeToFullyFallAsleep = 120;

		// Token: 0x04004D6D RID: 19821
		public bool isSleeping;

		// Token: 0x04004D6E RID: 19822
		public int sleepingIndex;

		// Token: 0x04004D6F RID: 19823
		public int timeSleeping;

		// Token: 0x04004D70 RID: 19824
		public Vector2 visualOffsetOfBedBase;
	}
}
