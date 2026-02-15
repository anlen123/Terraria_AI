using System;
using Terraria.GameContent.Skies;
using Terraria.Graphics.Effects;

namespace Terraria.GameContent.Events
{
	// Token: 0x020004F6 RID: 1270
	public class CreditsRollEvent
	{
		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x0600353D RID: 13629 RVA: 0x00617154 File Offset: 0x00615354
		public static bool IsEventOngoing
		{
			get
			{
				return CreditsRollEvent._creditsRollRemainingTime > 0;
			}
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x00617160 File Offset: 0x00615360
		public static void TryStartingCreditsRoll()
		{
			CreditsRollEvent._creditsRollRemainingTime = 28800;
			CreditsRollSky creditsRollSky = SkyManager.Instance["CreditsRoll"] as CreditsRollSky;
			if (creditsRollSky != null)
			{
				CreditsRollEvent._creditsRollRemainingTime = creditsRollSky.AmountOfTimeNeededForFullPlay;
			}
			if (Main.netMode == 2)
			{
				NetMessage.SendData(140, -1, -1, null, 0, (float)CreditsRollEvent._creditsRollRemainingTime, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x006171C4 File Offset: 0x006153C4
		public static void SendCreditsRollRemainingTimeToPlayer(int playerIndex)
		{
			if (CreditsRollEvent._creditsRollRemainingTime == 0)
			{
				return;
			}
			if (Main.netMode == 2)
			{
				NetMessage.SendData(140, playerIndex, -1, null, 0, (float)CreditsRollEvent._creditsRollRemainingTime, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x00617202 File Offset: 0x00615402
		public static void UpdateTime()
		{
			CreditsRollEvent._creditsRollRemainingTime = Utils.Clamp<int>(CreditsRollEvent._creditsRollRemainingTime - 1, 0, 28800);
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x0061721B File Offset: 0x0061541B
		public static void Reset()
		{
			CreditsRollEvent._creditsRollRemainingTime = 0;
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x00617223 File Offset: 0x00615423
		public static void SetRemainingTimeDirect(int time)
		{
			CreditsRollEvent._creditsRollRemainingTime = Utils.Clamp<int>(time, 0, 28800);
		}

		// Token: 0x04005A87 RID: 23175
		private const int MAX_TIME_FOR_CREDITS_ROLL_IN_FRAMES = 28800;

		// Token: 0x04005A88 RID: 23176
		private static int _creditsRollRemainingTime;
	}
}
