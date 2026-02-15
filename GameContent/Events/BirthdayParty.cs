using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Achievements;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent.Events
{
	// Token: 0x020004F9 RID: 1273
	public class BirthdayParty
	{
		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x0600355B RID: 13659 RVA: 0x006178E9 File Offset: 0x00615AE9
		public static bool PartyIsUp
		{
			get
			{
				return BirthdayParty.GenuineParty || BirthdayParty.ManualParty;
			}
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x006178F9 File Offset: 0x00615AF9
		public static void CheckMorning()
		{
			BirthdayParty.NaturalAttempt();
		}

		// Token: 0x0600355D RID: 13661 RVA: 0x00617900 File Offset: 0x00615B00
		public static void CheckNight()
		{
			bool flag = false;
			if (BirthdayParty.GenuineParty)
			{
				flag = true;
				BirthdayParty.GenuineParty = false;
				BirthdayParty.CelebratingNPCs.Clear();
			}
			if (BirthdayParty.ManualParty)
			{
				flag = true;
				BirthdayParty.ManualParty = false;
			}
			if (flag)
			{
				Color color = new Color(255, 0, 160);
				WorldGen.BroadcastText(NetworkText.FromKey(Lang.misc[99].Key, new object[0]), color);
			}
		}

		// Token: 0x0600355E RID: 13662 RVA: 0x0061796C File Offset: 0x00615B6C
		private static bool CanNPCParty(NPC n)
		{
			return n.active && n.townNPC && n.aiStyle != 0 && n.type != 37 && n.type != 453 && n.type != 441 && !NPCID.Sets.IsTownPet[n.type];
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x006179C8 File Offset: 0x00615BC8
		private static void NaturalAttempt()
		{
			if (Main.netMode == 1)
			{
				return;
			}
			if (!NPC.AnyNPCs(208))
			{
				return;
			}
			if (BirthdayParty.PartyDaysOnCooldown > 0)
			{
				BirthdayParty.PartyDaysOnCooldown--;
				return;
			}
			int maxValue = 10;
			if (Main.tenthAnniversaryWorld)
			{
				maxValue = 7;
			}
			if (Main.rand.Next(maxValue) != 0)
			{
				return;
			}
			List<NPC> list = new List<NPC>();
			for (int l = 0; l < Main.maxNPCs; l++)
			{
				NPC npc = Main.npc[l];
				if (BirthdayParty.CanNPCParty(npc))
				{
					list.Add(npc);
				}
			}
			if (list.Count < 5)
			{
				return;
			}
			BirthdayParty.GenuineParty = true;
			BirthdayParty.PartyDaysOnCooldown = Main.rand.Next(5, 11);
			NPC.freeCake = true;
			BirthdayParty.CelebratingNPCs.Clear();
			List<int> list2 = new List<int>();
			int num = 1;
			if (Main.rand.Next(5) == 0 && list.Count > 12)
			{
				num = 3;
			}
			else if (Main.rand.Next(3) == 0)
			{
				num = 2;
			}
			list = (from i in list
			orderby Main.rand.Next()
			select i).ToList<NPC>();
			for (int j = 0; j < num; j++)
			{
				list2.Add(j);
			}
			for (int k = 0; k < list2.Count; k++)
			{
				BirthdayParty.CelebratingNPCs.Add(list[list2[k]].whoAmI);
			}
			Color color = new Color(255, 0, 160);
			if (BirthdayParty.CelebratingNPCs.Count == 3)
			{
				WorldGen.BroadcastText(NetworkText.FromKey("Game.BirthdayParty_3", new object[]
				{
					Main.npc[BirthdayParty.CelebratingNPCs[0]].GetGivenOrTypeNetName(),
					Main.npc[BirthdayParty.CelebratingNPCs[1]].GetGivenOrTypeNetName(),
					Main.npc[BirthdayParty.CelebratingNPCs[2]].GetGivenOrTypeNetName()
				}), color);
			}
			else if (BirthdayParty.CelebratingNPCs.Count == 2)
			{
				WorldGen.BroadcastText(NetworkText.FromKey("Game.BirthdayParty_2", new object[]
				{
					Main.npc[BirthdayParty.CelebratingNPCs[0]].GetGivenOrTypeNetName(),
					Main.npc[BirthdayParty.CelebratingNPCs[1]].GetGivenOrTypeNetName()
				}), color);
			}
			else
			{
				WorldGen.BroadcastText(NetworkText.FromKey("Game.BirthdayParty_1", new object[]
				{
					Main.npc[BirthdayParty.CelebratingNPCs[0]].GetGivenOrTypeNetName()
				}), color);
			}
			NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
			BirthdayParty.CheckForAchievement();
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x00617C5C File Offset: 0x00615E5C
		public static void ToggleManualParty()
		{
			bool partyIsUp = BirthdayParty.PartyIsUp;
			if (Main.netMode != 1)
			{
				BirthdayParty.ManualParty = !BirthdayParty.ManualParty;
			}
			else
			{
				NetMessage.SendData(111, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
			}
			if (partyIsUp != BirthdayParty.PartyIsUp)
			{
				if (Main.netMode == 2)
				{
					NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
				}
				BirthdayParty.CheckForAchievement();
			}
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x00617CD2 File Offset: 0x00615ED2
		private static void CheckForAchievement()
		{
			if (BirthdayParty.PartyIsUp)
			{
				AchievementsHelper.NotifyProgressionEvent(25);
			}
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x00617CE2 File Offset: 0x00615EE2
		public static void WorldClear()
		{
			BirthdayParty.ManualParty = false;
			BirthdayParty.GenuineParty = false;
			BirthdayParty.PartyDaysOnCooldown = 0;
			BirthdayParty.CelebratingNPCs.Clear();
			BirthdayParty._wasCelebrating = false;
		}

		// Token: 0x06003563 RID: 13667 RVA: 0x00617D08 File Offset: 0x00615F08
		public static void UpdateTime()
		{
			if (BirthdayParty._wasCelebrating != BirthdayParty.PartyIsUp)
			{
				if (Main.netMode != 2)
				{
					if (BirthdayParty.PartyIsUp)
					{
						SkyManager.Instance.Activate("Party", default(Vector2), new object[0]);
					}
					else
					{
						SkyManager.Instance.Deactivate("Party", new object[0]);
					}
				}
				if (Main.netMode != 1 && BirthdayParty.CelebratingNPCs.Count > 0)
				{
					for (int i = 0; i < BirthdayParty.CelebratingNPCs.Count; i++)
					{
						if (!BirthdayParty.CanNPCParty(Main.npc[BirthdayParty.CelebratingNPCs[i]]))
						{
							BirthdayParty.CelebratingNPCs.RemoveAt(i);
						}
					}
					if (BirthdayParty.CelebratingNPCs.Count == 0)
					{
						BirthdayParty.GenuineParty = false;
						if (!BirthdayParty.ManualParty)
						{
							Color color = new Color(255, 0, 160);
							WorldGen.BroadcastText(NetworkText.FromKey(Lang.misc[99].Key, new object[0]), color);
							NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
						}
					}
				}
			}
			BirthdayParty._wasCelebrating = BirthdayParty.PartyIsUp;
		}

		// Token: 0x04005A92 RID: 23186
		public static bool ManualParty;

		// Token: 0x04005A93 RID: 23187
		public static bool GenuineParty;

		// Token: 0x04005A94 RID: 23188
		public static int PartyDaysOnCooldown;

		// Token: 0x04005A95 RID: 23189
		public static List<int> CelebratingNPCs = new List<int>();

		// Token: 0x04005A96 RID: 23190
		private static bool _wasCelebrating;
	}
}
