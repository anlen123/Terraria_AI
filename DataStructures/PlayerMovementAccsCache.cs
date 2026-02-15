using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000562 RID: 1378
	public struct PlayerMovementAccsCache
	{
		// Token: 0x060037CC RID: 14284 RVA: 0x0062F2C8 File Offset: 0x0062D4C8
		public void CopyFrom(Player player)
		{
			if (this._readyToPaste)
			{
				return;
			}
			this._readyToPaste = true;
			this._mountPreventedFlight = !player.mount.CanUseWings;
			this._mountPreventedExtraJumps = player.mount.BlockExtraJumps;
			this.rocketTime = player.rocketTime;
			this.rocketDelay = player.rocketDelay;
			this.rocketDelay2 = player.rocketDelay2;
			this.wingTime = player.wingTime;
			this.jumpAgainCloud = player.canJumpAgain_Cloud;
			this.jumpAgainSandstorm = player.canJumpAgain_Sandstorm;
			this.jumpAgainBlizzard = player.canJumpAgain_Blizzard;
			this.jumpAgainFart = player.canJumpAgain_Fart;
			this.jumpAgainSail = player.canJumpAgain_Sail;
			this.jumpAgainUnicorn = player.canJumpAgain_Unicorn;
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x0062F384 File Offset: 0x0062D584
		public void PasteInto(Player player)
		{
			if (!this._readyToPaste)
			{
				return;
			}
			this._readyToPaste = false;
			if (this._mountPreventedFlight)
			{
				player.rocketTime = this.rocketTime;
				player.rocketDelay = this.rocketDelay;
				player.rocketDelay2 = this.rocketDelay2;
				player.wingTime = this.wingTime;
			}
			if (this._mountPreventedExtraJumps)
			{
				player.canJumpAgain_Cloud = this.jumpAgainCloud;
				player.canJumpAgain_Sandstorm = this.jumpAgainSandstorm;
				player.canJumpAgain_Blizzard = this.jumpAgainBlizzard;
				player.canJumpAgain_Fart = this.jumpAgainFart;
				player.canJumpAgain_Sail = this.jumpAgainSail;
				player.canJumpAgain_Unicorn = this.jumpAgainUnicorn;
			}
		}

		// Token: 0x04005BDB RID: 23515
		private bool _readyToPaste;

		// Token: 0x04005BDC RID: 23516
		private bool _mountPreventedFlight;

		// Token: 0x04005BDD RID: 23517
		private bool _mountPreventedExtraJumps;

		// Token: 0x04005BDE RID: 23518
		private int rocketTime;

		// Token: 0x04005BDF RID: 23519
		private float wingTime;

		// Token: 0x04005BE0 RID: 23520
		private int rocketDelay;

		// Token: 0x04005BE1 RID: 23521
		private int rocketDelay2;

		// Token: 0x04005BE2 RID: 23522
		private bool jumpAgainCloud;

		// Token: 0x04005BE3 RID: 23523
		private bool jumpAgainSandstorm;

		// Token: 0x04005BE4 RID: 23524
		private bool jumpAgainBlizzard;

		// Token: 0x04005BE5 RID: 23525
		private bool jumpAgainFart;

		// Token: 0x04005BE6 RID: 23526
		private bool jumpAgainSail;

		// Token: 0x04005BE7 RID: 23527
		private bool jumpAgainUnicorn;
	}
}
