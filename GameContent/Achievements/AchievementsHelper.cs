using System;

namespace Terraria.GameContent.Achievements
{
	// Token: 0x02000283 RID: 643
	public class AchievementsHelper
	{
		// Token: 0x14000046 RID: 70
		// (add) Token: 0x060024AD RID: 9389 RVA: 0x00550DC8 File Offset: 0x0054EFC8
		// (remove) Token: 0x060024AE RID: 9390 RVA: 0x00550DFC File Offset: 0x0054EFFC
		public static event AchievementsHelper.ItemPickupEvent OnItemPickup;

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x060024AF RID: 9391 RVA: 0x00550E30 File Offset: 0x0054F030
		// (remove) Token: 0x060024B0 RID: 9392 RVA: 0x00550E64 File Offset: 0x0054F064
		public static event AchievementsHelper.ItemCraftEvent OnItemCraft;

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x060024B1 RID: 9393 RVA: 0x00550E98 File Offset: 0x0054F098
		// (remove) Token: 0x060024B2 RID: 9394 RVA: 0x00550ECC File Offset: 0x0054F0CC
		public static event AchievementsHelper.TileDestroyedEvent OnTileDestroyed;

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x060024B3 RID: 9395 RVA: 0x00550F00 File Offset: 0x0054F100
		// (remove) Token: 0x060024B4 RID: 9396 RVA: 0x00550F34 File Offset: 0x0054F134
		public static event AchievementsHelper.NPCKilledEvent OnNPCKilled;

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x060024B5 RID: 9397 RVA: 0x00550F68 File Offset: 0x0054F168
		// (remove) Token: 0x060024B6 RID: 9398 RVA: 0x00550F9C File Offset: 0x0054F19C
		public static event AchievementsHelper.ProgressionEventEvent OnProgressionEvent;

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060024B7 RID: 9399 RVA: 0x00550FCF File Offset: 0x0054F1CF
		// (set) Token: 0x060024B8 RID: 9400 RVA: 0x00550FD6 File Offset: 0x0054F1D6
		public static bool CurrentlyMining
		{
			get
			{
				return AchievementsHelper._isMining;
			}
			set
			{
				AchievementsHelper._isMining = value;
			}
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x00550FDE File Offset: 0x0054F1DE
		public static void NotifyTileDestroyed(Player player, ushort tile)
		{
			if (Main.gameMenu || !AchievementsHelper._isMining)
			{
				return;
			}
			if (AchievementsHelper.OnTileDestroyed != null)
			{
				AchievementsHelper.OnTileDestroyed(player, tile);
			}
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x00551002 File Offset: 0x0054F202
		public static void NotifyItemPickup(Player player, Item item)
		{
			if (AchievementsHelper.OnItemPickup != null)
			{
				AchievementsHelper.OnItemPickup(player, (short)item.type, item.stack);
			}
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x00551023 File Offset: 0x0054F223
		public static void NotifyItemPickup(Player player, Item item, int customStack)
		{
			if (AchievementsHelper.OnItemPickup != null)
			{
				AchievementsHelper.OnItemPickup(player, (short)item.type, customStack);
			}
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x0055103F File Offset: 0x0054F23F
		public static void NotifyItemCraft(Recipe recipe)
		{
			if (AchievementsHelper.OnItemCraft != null)
			{
				AchievementsHelper.OnItemCraft((short)recipe.createItem.type, recipe.createItem.stack);
			}
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x0055106C File Offset: 0x0054F26C
		public static void TryGrantingBestiary100PercentAchievement()
		{
			if (Main.GetBestiaryProgressReport().CompletionPercent >= 1f)
			{
				AchievementsHelper.NotifyProgressionEvent(29);
			}
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x00551094 File Offset: 0x0054F294
		public static void Initialize()
		{
			Player.Hooks.OnEnterWorld += AchievementsHelper.OnPlayerEnteredWorld;
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x005510A8 File Offset: 0x0054F2A8
		internal static void OnPlayerEnteredWorld(Player player)
		{
			if (AchievementsHelper.OnItemPickup != null)
			{
				for (int i = 0; i < 58; i++)
				{
					AchievementsHelper.OnItemPickup(player, (short)player.inventory[i].type, player.inventory[i].stack);
				}
				for (int j = 0; j < player.armor.Length; j++)
				{
					AchievementsHelper.OnItemPickup(player, (short)player.armor[j].type, player.armor[j].stack);
				}
				for (int k = 0; k < player.dye.Length; k++)
				{
					AchievementsHelper.OnItemPickup(player, (short)player.dye[k].type, player.dye[k].stack);
				}
				for (int l = 0; l < player.miscEquips.Length; l++)
				{
					AchievementsHelper.OnItemPickup(player, (short)player.miscEquips[l].type, player.miscEquips[l].stack);
				}
				for (int m = 0; m < player.miscDyes.Length; m++)
				{
					AchievementsHelper.OnItemPickup(player, (short)player.miscDyes[m].type, player.miscDyes[m].stack);
				}
				for (int n = 0; n < player.bank.item.Length; n++)
				{
					AchievementsHelper.OnItemPickup(player, (short)player.bank.item[n].type, player.bank.item[n].stack);
				}
				for (int num = 0; num < player.bank2.item.Length; num++)
				{
					AchievementsHelper.OnItemPickup(player, (short)player.bank2.item[num].type, player.bank2.item[num].stack);
				}
				for (int num2 = 0; num2 < player.bank3.item.Length; num2++)
				{
					AchievementsHelper.OnItemPickup(player, (short)player.bank3.item[num2].type, player.bank3.item[num2].stack);
				}
				for (int num3 = 0; num3 < player.bank4.item.Length; num3++)
				{
					AchievementsHelper.OnItemPickup(player, (short)player.bank4.item[num3].type, player.bank4.item[num3].stack);
				}
				for (int num4 = 0; num4 < player.Loadouts.Length; num4++)
				{
					Item[] array = player.Loadouts[num4].Armor;
					for (int num5 = 0; num5 < array.Length; num5++)
					{
						AchievementsHelper.OnItemPickup(player, (short)array[num5].type, array[num5].stack);
					}
					array = player.Loadouts[num4].Dye;
					for (int num6 = 0; num6 < array.Length; num6++)
					{
						AchievementsHelper.OnItemPickup(player, (short)array[num6].type, array[num6].stack);
					}
				}
			}
			if (player.statManaMax > 20)
			{
				Main.Achievements.GetCondition("STAR_POWER", "Use").Complete();
			}
			if (player.statLifeMax == 500 && player.statManaMax == 200)
			{
				Main.Achievements.GetCondition("TOPPED_OFF", "Use").Complete();
			}
			if (player.miscEquips[4].type > 0)
			{
				Main.Achievements.GetCondition("HOLD_ON_TIGHT", "Equip").Complete();
			}
			if (player.miscEquips[3].type > 0)
			{
				Main.Achievements.GetCondition("THE_CAVALRY", "Equip").Complete();
			}
			for (int num7 = 0; num7 < player.armor.Length; num7++)
			{
				if (player.armor[num7].wingSlot > 0)
				{
					Main.Achievements.GetCondition("HEAD_IN_THE_CLOUDS", "Equip").Complete();
					break;
				}
			}
			if (player.armor[0].stack > 0 && player.armor[1].stack > 0 && player.armor[2].stack > 0)
			{
				Main.Achievements.GetCondition("MATCHING_ATTIRE", "Equip").Complete();
			}
			if (player.armor[10].stack > 0 && player.armor[11].stack > 0 && player.armor[12].stack > 0)
			{
				Main.Achievements.GetCondition("FASHION_STATEMENT", "Equip").Complete();
			}
			bool flag = true;
			for (int num8 = 0; num8 < 10; num8++)
			{
				if (player.IsItemSlotUnlockedAndUsable(num8) && (player.dye[num8].type < 1 || player.dye[num8].stack < 1))
				{
					flag = false;
				}
			}
			if (flag)
			{
				Main.Achievements.GetCondition("DYE_HARD", "Equip").Complete();
			}
			if (player.unlockedBiomeTorches)
			{
				Main.Achievements.GetCondition("GAIN_TORCH_GODS_FAVOR", "Use").Complete();
			}
			WorldGen.CheckAchievement_RealEstateAndTownSlimes();
			AchievementsHelper.TryGrantingBestiary100PercentAchievement();
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x005515C8 File Offset: 0x0054F7C8
		public static void NotifyNPCKilled(NPC npc)
		{
			if (Main.netMode == 0)
			{
				if (npc.playerInteraction[Main.myPlayer])
				{
					AchievementsHelper.NotifyNPCKilledDirect(Main.player[Main.myPlayer], npc.netID);
					return;
				}
			}
			else
			{
				for (int i = 0; i < 255; i++)
				{
					if (npc.playerInteraction[i])
					{
						NetMessage.SendData(97, i, -1, null, npc.netID, 0f, 0f, 0f, 0, 0, 0);
					}
				}
			}
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x0055163D File Offset: 0x0054F83D
		public static void NotifyNPCKilledDirect(Player player, int npcNetID)
		{
			if (AchievementsHelper.OnNPCKilled != null)
			{
				AchievementsHelper.OnNPCKilled(player, (short)npcNetID);
			}
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x00551654 File Offset: 0x0054F854
		public static void NotifyProgressionEvent(int eventID)
		{
			if (Main.netMode == 2)
			{
				NetMessage.SendData(98, -1, -1, null, eventID, 0f, 0f, 0f, 0, 0, 0);
				return;
			}
			if (AchievementsHelper.OnProgressionEvent != null)
			{
				AchievementsHelper.OnProgressionEvent(eventID);
			}
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x0055169C File Offset: 0x0054F89C
		public static void HandleOnEquip(Player player, Item item, int context)
		{
			if (context == 16)
			{
				Main.Achievements.GetCondition("HOLD_ON_TIGHT", "Equip").Complete();
			}
			if (context == 17)
			{
				Main.Achievements.GetCondition("THE_CAVALRY", "Equip").Complete();
			}
			if ((context == 10 || context == 11) && item.wingSlot > 0)
			{
				Main.Achievements.GetCondition("HEAD_IN_THE_CLOUDS", "Equip").Complete();
			}
			if (context == 8 && player.armor[0].stack > 0 && player.armor[1].stack > 0 && player.armor[2].stack > 0)
			{
				Main.Achievements.GetCondition("MATCHING_ATTIRE", "Equip").Complete();
			}
			if (context == 9 && player.armor[10].stack > 0 && player.armor[11].stack > 0 && player.armor[12].stack > 0)
			{
				Main.Achievements.GetCondition("FASHION_STATEMENT", "Equip").Complete();
			}
			if (context == 12 || context == 33)
			{
				for (int i = 0; i < 10; i++)
				{
					if (player.IsItemSlotUnlockedAndUsable(i) && (player.dye[i].type < 1 || player.dye[i].stack < 1))
					{
						return;
					}
				}
				for (int j = 0; j < player.miscDyes.Length; j++)
				{
					if (player.miscDyes[j].type < 1 || player.miscDyes[j].stack < 1)
					{
						return;
					}
				}
				Main.Achievements.GetCondition("DYE_HARD", "Equip").Complete();
			}
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x00551840 File Offset: 0x0054FA40
		public static void HandleSpecialEvent(Player player, int eventID)
		{
			if (player.whoAmI != Main.myPlayer)
			{
				return;
			}
			switch (eventID)
			{
			case 1:
				Main.Achievements.GetCondition("STAR_POWER", "Use").Complete();
				if (player.statLifeMax == 500 && player.statManaMax == 200)
				{
					Main.Achievements.GetCondition("TOPPED_OFF", "Use").Complete();
					return;
				}
				break;
			case 2:
				Main.Achievements.GetCondition("GET_A_LIFE", "Use").Complete();
				if (player.statLifeMax == 500 && player.statManaMax == 200)
				{
					Main.Achievements.GetCondition("TOPPED_OFF", "Use").Complete();
					return;
				}
				break;
			case 3:
				Main.Achievements.GetCondition("NOT_THE_BEES", "Use").Complete();
				return;
			case 4:
				Main.Achievements.GetCondition("WATCH_YOUR_STEP", "Hit").Complete();
				return;
			case 5:
				Main.Achievements.GetCondition("RAINBOWS_AND_UNICORNS", "Use").Complete();
				return;
			case 6:
				Main.Achievements.GetCondition("YOU_AND_WHAT_ARMY", "Spawn").Complete();
				return;
			case 7:
				Main.Achievements.GetCondition("THROWING_LINES", "Use").Complete();
				return;
			case 8:
				Main.Achievements.GetCondition("LUCKY_BREAK", "Hit").Complete();
				return;
			case 9:
				Main.Achievements.GetCondition("VEHICULAR_MANSLAUGHTER", "Hit").Complete();
				return;
			case 10:
				Main.Achievements.GetCondition("ROCK_BOTTOM", "Reach").Complete();
				return;
			case 11:
				Main.Achievements.GetCondition("INTO_ORBIT", "Reach").Complete();
				return;
			case 12:
				Main.Achievements.GetCondition("WHERES_MY_HONEY", "Reach").Complete();
				return;
			case 13:
				Main.Achievements.GetCondition("JEEPERS_CREEPERS", "Reach").Complete();
				return;
			case 14:
				Main.Achievements.GetCondition("ITS_GETTING_HOT_IN_HERE", "Reach").Complete();
				return;
			case 15:
				Main.Achievements.GetCondition("FUNKYTOWN", "Reach").Complete();
				return;
			case 16:
				Main.Achievements.GetCondition("I_AM_LOOT", "Peek").Complete();
				return;
			case 17:
				Main.Achievements.GetCondition("FLY_A_KITE_ON_A_WINDY_DAY", "Use").Complete();
				return;
			case 18:
				Main.Achievements.GetCondition("FOUND_GRAVEYARD", "Reach").Complete();
				return;
			case 19:
				Main.Achievements.GetCondition("GO_LAVA_FISHING", "Do").Complete();
				return;
			case 20:
				Main.Achievements.GetCondition("TALK_TO_NPC_AT_MAX_HAPPINESS", "Do").Complete();
				return;
			case 21:
				Main.Achievements.GetCondition("PET_THE_PET", "Do").Complete();
				return;
			case 22:
				Main.Achievements.GetCondition("FIND_A_FAIRY", "Do").Complete();
				return;
			case 23:
				Main.Achievements.GetCondition("DIE_TO_DEAD_MANS_CHEST", "Do").Complete();
				return;
			case 24:
				Main.Achievements.GetCondition("GAIN_TORCH_GODS_FAVOR", "Use").Complete();
				return;
			case 25:
				Main.Achievements.GetCondition("DRINK_BOTTLED_WATER_WHILE_DROWNING", "Use").Complete();
				return;
			case 26:
				Main.Achievements.GetCondition("PLAY_ON_A_SPECIAL_SEED", "Do").Complete();
				return;
			case 27:
				Main.Achievements.GetCondition("PURIFY_ENTIRE_WORLD", "Do").Complete();
				break;
			default:
				return;
			}
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x00551BFF File Offset: 0x0054FDFF
		public static void DoClassicTitleScreenAchievement()
		{
			Main.Achievements.GetCondition("GOING_OLDSCHOOL", "Do").Complete();
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x00551C1C File Offset: 0x0054FE1C
		public static void CheckResearchAchievement(bool forced = false)
		{
			int lastEditId = Main.LocalPlayerCreativeTracker.ItemSacrifices.LastEditId;
			if (forced || AchievementsHelper._lastResearchVersion != lastEditId)
			{
				AchievementsHelper._lastResearchVersion = lastEditId;
				int num;
				int num2;
				Main.LocalPlayerCreativeTracker.ItemSacrifices.CountFullyResearchedItems(out num, out num2);
				if (num >= num2 / 2)
				{
					AchievementsHelper.NotifyProgressionEvent(45);
				}
			}
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x00551C6C File Offset: 0x0054FE6C
		public static void PlantedAcorn()
		{
			CustomIntCondition customIntCondition = (CustomIntCondition)Main.Achievements.GetCondition("CONSERVATIONIST", "Do");
			int value = customIntCondition.Value;
			customIntCondition.Value = value + 1;
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x00551CA1 File Offset: 0x0054FEA1
		public static void HandleNurseService(int coinsSpent)
		{
			((CustomFloatCondition)Main.Achievements.GetCondition("FREQUENT_FLYER", "Pay")).Value += (float)coinsSpent;
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x00551CCC File Offset: 0x0054FECC
		public static void HandleAnglerService()
		{
			Main.Achievements.GetCondition("SERVANT_IN_TRAINING", "Finish").Complete();
			CustomIntCondition customIntCondition = (CustomIntCondition)Main.Achievements.GetCondition("GOOD_LITTLE_SLAVE", "Finish");
			int value = customIntCondition.Value;
			customIntCondition.Value = value + 1;
			CustomIntCondition customIntCondition2 = (CustomIntCondition)Main.Achievements.GetCondition("TROUT_MONKEY", "Finish");
			value = customIntCondition2.Value;
			customIntCondition2.Value = value + 1;
			CustomIntCondition customIntCondition3 = (CustomIntCondition)Main.Achievements.GetCondition("FAST_AND_FISHIOUS", "Finish");
			value = customIntCondition3.Value;
			customIntCondition3.Value = value + 1;
			CustomIntCondition customIntCondition4 = (CustomIntCondition)Main.Achievements.GetCondition("SUPREME_HELPER_MINION", "Finish");
			value = customIntCondition4.Value;
			customIntCondition4.Value = value + 1;
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x00551D92 File Offset: 0x0054FF92
		public static void HandleRunning(float pixelsMoved)
		{
			((CustomFloatCondition)Main.Achievements.GetCondition("MARATHON_MEDALIST", "Move")).Value += pixelsMoved;
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x00551DBC File Offset: 0x0054FFBC
		public static void HandleMining()
		{
			CustomIntCondition customIntCondition = (CustomIntCondition)Main.Achievements.GetCondition("BULLDOZER", "Pick");
			int value = customIntCondition.Value;
			customIntCondition.Value = value + 1;
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x00551DF4 File Offset: 0x0054FFF4
		public static void MechaMayhem_Clear()
		{
			bool flag;
			bool flag2;
			bool flag3;
			AchievementsHelper.ScanForMechs(out flag, out flag2, out flag3);
			if (!flag && !flag2 && !flag3)
			{
				AchievementsHelper.mayhem1down = false;
				AchievementsHelper.mayhem2down = false;
				AchievementsHelper.mayhem3down = false;
			}
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x00551E28 File Offset: 0x00550028
		public static void MechaMayhem_Start()
		{
			bool flag;
			bool flag2;
			bool flag3;
			AchievementsHelper.ScanForMechs(out flag, out flag2, out flag3);
			AchievementsHelper.mayhemOK = (flag && flag2 && flag3);
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x00551E4C File Offset: 0x0055004C
		public static void MechaMayhem_Kill(int justKilled)
		{
			if (!AchievementsHelper.mayhemOK)
			{
				return;
			}
			if (justKilled == 125 || justKilled == 126)
			{
				AchievementsHelper.mayhem1down = true;
			}
			else if (!NPC.AnyNPCs(125) && !NPC.AnyNPCs(126) && !AchievementsHelper.mayhem1down)
			{
				AchievementsHelper.mayhemOK = false;
				return;
			}
			if (justKilled == 134)
			{
				AchievementsHelper.mayhem2down = true;
			}
			else if (!NPC.AnyNPCs(134) && !AchievementsHelper.mayhem2down)
			{
				AchievementsHelper.mayhemOK = false;
				return;
			}
			if (justKilled == 127)
			{
				AchievementsHelper.mayhem3down = true;
			}
			else if (!NPC.AnyNPCs(127) && !AchievementsHelper.mayhem3down)
			{
				AchievementsHelper.mayhemOK = false;
				return;
			}
			if (AchievementsHelper.mayhem1down && AchievementsHelper.mayhem2down && AchievementsHelper.mayhem3down)
			{
				AchievementsHelper.NotifyProgressionEvent(21);
			}
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x00551F00 File Offset: 0x00550100
		private static void ScanForMechs(out bool foundSkeletronPrime, out bool foundDestroyer, out bool foundTwins)
		{
			foundSkeletronPrime = false;
			foundDestroyer = false;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active)
				{
					int type = npc.type;
					foundSkeletronPrime |= (type == 127);
					foundDestroyer |= (type == 134);
					flag |= (type == 126);
					flag2 |= (type == 125);
				}
			}
			foundTwins = (flag && flag2);
		}

		// Token: 0x04004F41 RID: 20289
		private static bool _isMining;

		// Token: 0x04004F42 RID: 20290
		private static int _lastResearchVersion;

		// Token: 0x04004F43 RID: 20291
		private static bool mayhemOK;

		// Token: 0x04004F44 RID: 20292
		private static bool mayhem1down;

		// Token: 0x04004F45 RID: 20293
		private static bool mayhem2down;

		// Token: 0x04004F46 RID: 20294
		private static bool mayhem3down;

		// Token: 0x02000809 RID: 2057
		// (Invoke) Token: 0x060042CA RID: 17098
		public delegate void ItemPickupEvent(Player player, short itemId, int count);

		// Token: 0x0200080A RID: 2058
		// (Invoke) Token: 0x060042CE RID: 17102
		public delegate void ItemCraftEvent(short itemId, int count);

		// Token: 0x0200080B RID: 2059
		// (Invoke) Token: 0x060042D2 RID: 17106
		public delegate void TileDestroyedEvent(Player player, ushort tileId);

		// Token: 0x0200080C RID: 2060
		// (Invoke) Token: 0x060042D6 RID: 17110
		public delegate void NPCKilledEvent(Player player, short npcId);

		// Token: 0x0200080D RID: 2061
		// (Invoke) Token: 0x060042DA RID: 17114
		public delegate void ProgressionEventEvent(int eventID);
	}
}
