using System;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent
{
	// Token: 0x0200024C RID: 588
	public class DontStarveDarknessDamageDealer
	{
		// Token: 0x060022F9 RID: 8953 RVA: 0x0053B585 File Offset: 0x00539785
		public static void Reset()
		{
			DontStarveDarknessDamageDealer.ResetTimer();
			DontStarveDarknessDamageDealer.saidMessage = false;
			DontStarveDarknessDamageDealer.lastFrameWasTooBright = true;
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x0053B598 File Offset: 0x00539798
		private static void ResetTimer()
		{
			DontStarveDarknessDamageDealer.darknessTimer = -1;
			DontStarveDarknessDamageDealer.darknessHitTimer = 0;
		}

		// Token: 0x060022FB RID: 8955 RVA: 0x0053B5A6 File Offset: 0x005397A6
		private static int GetDarknessDamagePerHit()
		{
			return 250;
		}

		// Token: 0x060022FC RID: 8956 RVA: 0x0053B5AD File Offset: 0x005397AD
		private static int GetDarknessTimeBeforeStartingHits()
		{
			return 120;
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x0053B5B1 File Offset: 0x005397B1
		private static int GetDarknessTimeForMessage()
		{
			return 60;
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x0053B5B8 File Offset: 0x005397B8
		public static void Update(Player player)
		{
			if (player.dead || !FocusHelper.AllowDontStarveDarknessDamage || player.shimmering || Main.disableDontStarveDarknessDamage)
			{
				DontStarveDarknessDamageDealer.ResetTimer();
				return;
			}
			DontStarveDarknessDamageDealer.UpdateDarknessState(player);
			int darknessTimeBeforeStartingHits = DontStarveDarknessDamageDealer.GetDarknessTimeBeforeStartingHits();
			if (DontStarveDarknessDamageDealer.darknessTimer >= darknessTimeBeforeStartingHits)
			{
				DontStarveDarknessDamageDealer.darknessTimer = darknessTimeBeforeStartingHits;
				DontStarveDarknessDamageDealer.darknessHitTimer++;
				if (DontStarveDarknessDamageDealer.darknessHitTimer > 60 && !player.immune)
				{
					int darknessDamagePerHit = DontStarveDarknessDamageDealer.GetDarknessDamagePerHit();
					SoundEngine.PlaySound(SoundID.Item1, player.Center, 0f, 1f);
					player.Hurt(PlayerDeathReason.ByOther(17), darknessDamagePerHit, 0, false, false, false, -1, true);
					DontStarveDarknessDamageDealer.darknessHitTimer = 0;
				}
			}
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x0053B65C File Offset: 0x0053985C
		private static void UpdateDarknessState(Player player)
		{
			if (DontStarveDarknessDamageDealer.IsPlayerSafe(player))
			{
				if (DontStarveDarknessDamageDealer.saidMessage)
				{
					if (!Main.getGoodWorld)
					{
						Main.NewText(Language.GetTextValue("Game.DarknessSafe"), 50, 200, 50);
					}
					DontStarveDarknessDamageDealer.saidMessage = false;
				}
				DontStarveDarknessDamageDealer.ResetTimer();
				return;
			}
			int darknessTimeForMessage = DontStarveDarknessDamageDealer.GetDarknessTimeForMessage();
			if (DontStarveDarknessDamageDealer.darknessTimer >= darknessTimeForMessage && !DontStarveDarknessDamageDealer.saidMessage)
			{
				if (!Main.getGoodWorld)
				{
					Main.NewText(Language.GetTextValue("Game.DarknessDanger"), 200, 50, 50);
				}
				DontStarveDarknessDamageDealer.saidMessage = true;
			}
			DontStarveDarknessDamageDealer.darknessTimer++;
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x0053B6EC File Offset: 0x005398EC
		private static bool IsPlayerSafe(Player player)
		{
			return Lighting.GetColor((int)player.Center.X / 16, (int)player.Center.Y / 16).ToVector3().Length() >= 0.1f;
		}

		// Token: 0x04004D1E RID: 19742
		public const int DARKNESS_HIT_TIMER_MAX_BEFORE_HIT = 60;

		// Token: 0x04004D1F RID: 19743
		public static int darknessTimer = -1;

		// Token: 0x04004D20 RID: 19744
		public static int darknessHitTimer = 0;

		// Token: 0x04004D21 RID: 19745
		public static bool saidMessage = false;

		// Token: 0x04004D22 RID: 19746
		public static bool lastFrameWasTooBright = true;
	}
}
