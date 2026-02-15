using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;

namespace Terraria.GameContent.Events
{
	// Token: 0x020004F7 RID: 1271
	public class LanternNight
	{
		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06003545 RID: 13637 RVA: 0x00617236 File Offset: 0x00615436
		public static bool LanternsUp
		{
			get
			{
				return LanternNight.GenuineLanterns || LanternNight.ManualLanterns;
			}
		}

		// Token: 0x06003546 RID: 13638 RVA: 0x00617248 File Offset: 0x00615448
		public static void CheckMorning()
		{
			if (LanternNight.GenuineLanterns)
			{
				LanternNight.GenuineLanterns = false;
			}
			if (LanternNight.ManualLanterns)
			{
				LanternNight.ManualLanterns = false;
			}
		}

		// Token: 0x06003547 RID: 13639 RVA: 0x00617277 File Offset: 0x00615477
		public static void CheckNight()
		{
			LanternNight.NaturalAttempt();
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x0061727E File Offset: 0x0061547E
		public static bool LanternsCanPersist()
		{
			return !Main.dayTime && LanternNight.LanternsCanStart();
		}

		// Token: 0x06003549 RID: 13641 RVA: 0x0061728E File Offset: 0x0061548E
		public static bool LanternsCanStart()
		{
			return !WorldGen.spawnMeteor && !Main.bloodMoon && !Main.pumpkinMoon && !Main.snowMoon && Main.invasionType == 0 && NPC.MoonLordCountdown == 0 && !LanternNight.BossIsActive();
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x006172C4 File Offset: 0x006154C4
		private static bool BossIsActive()
		{
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active && (npc.boss || (npc.type >= 13 && npc.type <= 15)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600354B RID: 13643 RVA: 0x00617310 File Offset: 0x00615510
		private static void NaturalAttempt()
		{
			if (Main.netMode == 1)
			{
				return;
			}
			if (!LanternNight.LanternsCanStart())
			{
				return;
			}
			bool flag = false;
			if (LanternNight.LanternNightsOnCooldown > 0)
			{
				LanternNight.LanternNightsOnCooldown--;
			}
			if (LanternNight.LanternNightsOnCooldown == 0 && NPC.downedMoonlord && Main.rand.Next(14) == 0)
			{
				flag = true;
			}
			if (!flag && LanternNight.NextNightIsLanternNight)
			{
				LanternNight.NextNightIsLanternNight = false;
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			LanternNight.GenuineLanterns = true;
			LanternNight.LanternNightsOnCooldown = Main.rand.Next(5, 11);
		}

		// Token: 0x0600354C RID: 13644 RVA: 0x00617390 File Offset: 0x00615590
		public static void ToggleManualLanterns()
		{
			bool lanternsUp = LanternNight.LanternsUp;
			if (Main.netMode != 1)
			{
				LanternNight.ManualLanterns = !LanternNight.ManualLanterns;
			}
			if (lanternsUp != LanternNight.LanternsUp && Main.netMode == 2)
			{
				NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x006173E2 File Offset: 0x006155E2
		public static void WorldClear()
		{
			LanternNight.ManualLanterns = false;
			LanternNight.GenuineLanterns = false;
			LanternNight.LanternNightsOnCooldown = 0;
			LanternNight._wasLanternNight = false;
		}

		// Token: 0x0600354E RID: 13646 RVA: 0x006173FC File Offset: 0x006155FC
		public static void UpdateTime()
		{
			if (LanternNight.GenuineLanterns && !LanternNight.LanternsCanPersist())
			{
				LanternNight.GenuineLanterns = false;
			}
			if (LanternNight._wasLanternNight != LanternNight.LanternsUp)
			{
				if (Main.netMode != 2)
				{
					if (LanternNight.LanternsUp)
					{
						SkyManager.Instance.Activate("Lantern", default(Vector2), new object[0]);
					}
					else
					{
						SkyManager.Instance.Deactivate("Lantern", new object[0]);
					}
				}
				else
				{
					NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
				}
			}
			LanternNight._wasLanternNight = LanternNight.LanternsUp;
		}

		// Token: 0x04005A89 RID: 23177
		public static bool ManualLanterns;

		// Token: 0x04005A8A RID: 23178
		public static bool GenuineLanterns;

		// Token: 0x04005A8B RID: 23179
		public static bool NextNightIsLanternNight;

		// Token: 0x04005A8C RID: 23180
		public static int LanternNightsOnCooldown;

		// Token: 0x04005A8D RID: 23181
		private static bool _wasLanternNight;
	}
}
