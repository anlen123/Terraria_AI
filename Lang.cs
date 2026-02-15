using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.UI;
using Terraria.Utilities;

namespace Terraria
{
	// Token: 0x02000029 RID: 41
	public class Lang
	{
		// Token: 0x060001CC RID: 460 RVA: 0x0001A611 File Offset: 0x00018811
		public static string GetMapObjectName(int id)
		{
			if (Lang._mapLegendCache != null)
			{
				return Lang._mapLegendCache[id].Value;
			}
			return string.Empty;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0001A62C File Offset: 0x0001882C
		public static void RegisterGlobalSubstitution(string key, Func<object> getValue)
		{
			Lang._globalSubstitutions[key] = getValue;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0001A63C File Offset: 0x0001883C
		public static object GetGlobalSubstitution(string key)
		{
			Func<object> func;
			if (!Lang._globalSubstitutions.TryGetValue(key, out func))
			{
				return null;
			}
			return func();
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0001A660 File Offset: 0x00018860
		private static void InitGlobalSubstitutions()
		{
			Lang.RegisterGlobalSubstitution("Nurse", () => NPC.GetFirstNPCNameOrNull(18));
			Lang.RegisterGlobalSubstitution("Merchant", () => NPC.GetFirstNPCNameOrNull(17));
			Lang.RegisterGlobalSubstitution("ArmsDealer", () => NPC.GetFirstNPCNameOrNull(19));
			Lang.RegisterGlobalSubstitution("Dryad", () => NPC.GetFirstNPCNameOrNull(20));
			Lang.RegisterGlobalSubstitution("Demolitionist", () => NPC.GetFirstNPCNameOrNull(38));
			Lang.RegisterGlobalSubstitution("Clothier", () => NPC.GetFirstNPCNameOrNull(54));
			Lang.RegisterGlobalSubstitution("Guide", () => NPC.GetFirstNPCNameOrNull(22));
			Lang.RegisterGlobalSubstitution("Wizard", () => NPC.GetFirstNPCNameOrNull(108));
			Lang.RegisterGlobalSubstitution("GoblinTinkerer", () => NPC.GetFirstNPCNameOrNull(107));
			Lang.RegisterGlobalSubstitution("Mechanic", () => NPC.GetFirstNPCNameOrNull(124));
			Lang.RegisterGlobalSubstitution("Truffle", () => NPC.GetFirstNPCNameOrNull(160));
			Lang.RegisterGlobalSubstitution("Steampunker", () => NPC.GetFirstNPCNameOrNull(178));
			Lang.RegisterGlobalSubstitution("DyeTrader", () => NPC.GetFirstNPCNameOrNull(207));
			Lang.RegisterGlobalSubstitution("PartyGirl", () => NPC.GetFirstNPCNameOrNull(208));
			Lang.RegisterGlobalSubstitution("Cyborg", () => NPC.GetFirstNPCNameOrNull(209));
			Lang.RegisterGlobalSubstitution("Painter", () => NPC.GetFirstNPCNameOrNull(227));
			Lang.RegisterGlobalSubstitution("WitchDoctor", () => NPC.GetFirstNPCNameOrNull(228));
			Lang.RegisterGlobalSubstitution("Pirate", () => NPC.GetFirstNPCNameOrNull(229));
			Lang.RegisterGlobalSubstitution("Stylist", () => NPC.GetFirstNPCNameOrNull(353));
			Lang.RegisterGlobalSubstitution("TravelingMerchant", () => NPC.GetFirstNPCNameOrNull(368));
			Lang.RegisterGlobalSubstitution("Angler", () => NPC.GetFirstNPCNameOrNull(369));
			Lang.RegisterGlobalSubstitution("Bartender", () => NPC.GetFirstNPCNameOrNull(550));
			Lang.RegisterGlobalSubstitution("WorldName", () => Main.worldName);
			Lang.RegisterGlobalSubstitution("Day", () => Main.dayTime);
			Lang.RegisterGlobalSubstitution("BloodMoon", () => Main.bloodMoon);
			Lang.RegisterGlobalSubstitution("Eclipse", () => Main.eclipse);
			Lang.RegisterGlobalSubstitution("MoonLordDefeated", () => NPC.downedMoonlord);
			Lang.RegisterGlobalSubstitution("GolemDefeated", () => NPC.downedGolemBoss);
			Lang.RegisterGlobalSubstitution("DukeFishronDefeated", () => NPC.downedFishron);
			Lang.RegisterGlobalSubstitution("FrostLegionDefeated", () => NPC.downedFrost);
			Lang.RegisterGlobalSubstitution("MartiansDefeated", () => NPC.downedMartians);
			Lang.RegisterGlobalSubstitution("PumpkingDefeated", () => NPC.downedHalloweenKing);
			Lang.RegisterGlobalSubstitution("IceQueenDefeated", () => NPC.downedChristmasIceQueen);
			Lang.RegisterGlobalSubstitution("HardMode", () => Main.hardMode);
			Lang.RegisterGlobalSubstitution("Homeless", () => Main.LocalPlayer.talkNPC >= 0 && Main.npc[Main.LocalPlayer.talkNPC].homeless);
			Lang.RegisterGlobalSubstitution("InventoryKey", () => Main.cInv);
			Lang.RegisterGlobalSubstitution("PlayerName", () => Main.player[Main.myPlayer].name);
			Lang.RegisterGlobalSubstitution("GolfGuy", () => NPC.GetFirstNPCNameOrNull(588));
			Lang.RegisterGlobalSubstitution("TaxCollector", () => NPC.GetFirstNPCNameOrNull(441));
			Lang.RegisterGlobalSubstitution("Rain", () => Main.raining);
			Lang.RegisterGlobalSubstitution("Graveyard", () => Main.LocalPlayer.ZoneGraveyard);
			Lang.RegisterGlobalSubstitution("AnglerCompletedQuestsCount", () => Main.LocalPlayer.anglerQuestsFinished);
			Lang.RegisterGlobalSubstitution("TotalDeathsCount", () => Main.LocalPlayer.numberOfDeathsPVE);
			Lang.RegisterGlobalSubstitution("WorldEvilStone", delegate
			{
				if (!WorldGen.crimson)
				{
					return Language.GetTextValue("Misc.Ebonstone");
				}
				return Language.GetTextValue("Misc.Crimstone");
			});
			Lang.RegisterGlobalSubstitution("InputTriggerUI_BuildFromInventory", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(false, "QuickMount"));
			Lang.RegisterGlobalSubstitution("InputTriggerUI_TrashEnabled", () => !ItemSlot.Options.DisableQuickTrash);
			Lang.RegisterGlobalSubstitution("InputTriggerUI_Trash", delegate
			{
				if (!PlayerInput.UsingGamepad)
				{
					return Language.GetTextValue(ItemSlot.Options.DisableLeftShiftTrashCan ? "Controls.Control" : "Controls.Shift");
				}
				return PlayerInput.GenerateInputTag_ForCurrentGamemode(false, "SmartSelect");
			});
			Lang.RegisterGlobalSubstitution("InputTriggerUI_FavoriteItem", delegate
			{
				if (!PlayerInput.UsingGamepad)
				{
					return Main.FavoriteKey.ToString();
				}
				return PlayerInput.GenerateInputTag_ForCurrentGamemode(false, "SmartCursor");
			});
			Lang.RegisterGlobalSubstitution("InputTrigger_QuickEquip", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(false, "Grapple"));
			Lang.RegisterGlobalSubstitution("InputTrigger_Grapple", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(true, "Grapple"));
			Lang.RegisterGlobalSubstitution("InputTrigger_LockOn", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(true, "LockOn"));
			Lang.RegisterGlobalSubstitution("InputTrigger_RadialQuickbar", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(true, "HotbarMinus"));
			Lang.RegisterGlobalSubstitution("InputTrigger_RadialHotbar", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(true, "HotbarPlus"));
			Lang.RegisterGlobalSubstitution("InputTrigger_SmartCursor", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(true, "SmartCursor"));
			Lang.RegisterGlobalSubstitution("InputTrigger_UseOrAttack", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(true, "MouseLeft"));
			Lang.RegisterGlobalSubstitution("InputTrigger_InteractWithTile", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(true, "MouseRight"));
			Lang.RegisterGlobalSubstitution("InputTrigger_InteractWithTileUI", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(false, "MouseRight"));
			Lang.RegisterGlobalSubstitution("InputTrigger_ToggleOrOpen", delegate
			{
				if (!PlayerInput.UsingGamepad)
				{
					return PlayerInput.GenerateInputTag_ForCurrentGamemode(true, "MouseRight");
				}
				return PlayerInput.GenerateInputTag_ForCurrentGamemode(false, "Grapple");
			});
			Lang.RegisterGlobalSubstitution("InputTrigger_SmartSelect", () => PlayerInput.GenerateInputTag_ForCurrentGamemode(true, "SmartSelect"));
			Lang.RegisterGlobalSubstitution("ToggleArmorSetBonusKey", () => Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN"));
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0001B009 File Offset: 0x00019209
		[Old("dialog is deprecated. Please use Language.GetText instead.")]
		public static string dialog(int l, bool english = false)
		{
			return Language.GetTextValue("LegacyDialog." + l);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0001B020 File Offset: 0x00019220
		public static string GetNPCNameValue(int netID)
		{
			return Lang.GetNPCName(netID).Value;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0001B02D File Offset: 0x0001922D
		public static LocalizedText GetNPCName(int netID)
		{
			if (netID > 0 && netID < (int)NPCID.Count)
			{
				return Lang._npcNameCache[netID];
			}
			if (netID < 0 && -netID - 1 < Lang._negativeNpcNameCache.Length)
			{
				return Lang._negativeNpcNameCache[-netID - 1];
			}
			return LocalizedText.Empty;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0001B064 File Offset: 0x00019264
		public static ItemTooltip GetTooltip(int itemId)
		{
			return Lang._itemTooltipCache[itemId];
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0001B06D File Offset: 0x0001926D
		public static LocalizedText GetItemName(int id)
		{
			if (id < 0)
			{
				id = (int)ItemID.FromNetId((short)id);
			}
			if (id > 0 && id < (int)ItemID.Count && Lang._itemNameCache[id] != null)
			{
				return Lang._itemNameCache[id];
			}
			return LocalizedText.Empty;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0001B09E File Offset: 0x0001929E
		public static LocalizedText GetEmojiName(int id)
		{
			if (id >= 0 && id < EmoteID.Count && Lang._emojiNameCache[id] != null)
			{
				return Lang._emojiNameCache[id];
			}
			return LocalizedText.Empty;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0001B0C2 File Offset: 0x000192C2
		public static string GetItemNameValue(int id)
		{
			return Lang.GetItemName(id).Value;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0001B0D0 File Offset: 0x000192D0
		public static string GetPrefixedItemName(int id, int prefixType)
		{
			LocalizedText itemName = Lang.GetItemName(id);
			LocalizedText localizedText = Lang.prefix[prefixType];
			string prefixName = localizedText.Value;
			string variant;
			string text;
			if (Language.TryGetVariation(itemName.Key, "Gender", out variant) && Language.TryGetVariation(localizedText.Key, variant, out text))
			{
				prefixName = text;
			}
			return Lang._prefixFormatText.FormatWith(new Lang.ItemPrefixCombiner
			{
				PrefixName = prefixName,
				ItemName = itemName.Value
			});
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0001B13C File Offset: 0x0001933C
		public static string GetBuffName(int id)
		{
			return Lang._buffNameCache[id].Value;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0001B14A File Offset: 0x0001934A
		public static string GetBuffDescription(int id)
		{
			return Lang._buffDescriptionCache[id].Value;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0001B158 File Offset: 0x00019358
		public static string GetDryadWorldStatusDialog(out bool worldIsEntirelyPure)
		{
			worldIsEntirelyPure = false;
			int tGood = (int)WorldGen.tGood;
			int tEvil = (int)WorldGen.tEvil;
			int tBlood = (int)WorldGen.tBlood;
			string textValue;
			if (tGood > 0 && tEvil > 0 && tBlood > 0)
			{
				textValue = Language.GetTextValue("DryadSpecialText.WorldStatusAll", new object[]
				{
					Main.worldName,
					tGood,
					tEvil,
					tBlood
				});
			}
			else if (tGood > 0 && tEvil > 0)
			{
				textValue = Language.GetTextValue("DryadSpecialText.WorldStatusHallowCorrupt", Main.worldName, tGood, tEvil);
			}
			else if (tGood > 0 && tBlood > 0)
			{
				textValue = Language.GetTextValue("DryadSpecialText.WorldStatusHallowCrimson", Main.worldName, tGood, tBlood);
			}
			else if (tEvil > 0 && tBlood > 0)
			{
				textValue = Language.GetTextValue("DryadSpecialText.WorldStatusCorruptCrimson", Main.worldName, tEvil, tBlood);
			}
			else if (tEvil > 0)
			{
				textValue = Language.GetTextValue("DryadSpecialText.WorldStatusCorrupt", Main.worldName, tEvil);
			}
			else if (tBlood > 0)
			{
				textValue = Language.GetTextValue("DryadSpecialText.WorldStatusCrimson", Main.worldName, tBlood);
			}
			else
			{
				if (tGood <= 0)
				{
					textValue = Language.GetTextValue("DryadSpecialText.WorldStatusPure", Main.worldName);
					worldIsEntirelyPure = true;
					return textValue;
				}
				textValue = Language.GetTextValue("DryadSpecialText.WorldStatusHallow", Main.worldName, tGood);
			}
			string textValue2;
			if ((double)tGood * 1.2 >= (double)(tEvil + tBlood) && (double)tGood * 0.8 <= (double)(tEvil + tBlood))
			{
				textValue2 = Language.GetTextValue("DryadSpecialText.WorldDescriptionBalanced");
			}
			else if (tGood >= tEvil + tBlood)
			{
				textValue2 = Language.GetTextValue("DryadSpecialText.WorldDescriptionFairyTale");
			}
			else if (tEvil + tBlood > tGood + 20)
			{
				textValue2 = Language.GetTextValue("DryadSpecialText.WorldDescriptionGrim");
			}
			else if (tEvil + tBlood > 5)
			{
				textValue2 = Language.GetTextValue("DryadSpecialText.WorldDescriptionWork");
			}
			else
			{
				textValue2 = Language.GetTextValue("DryadSpecialText.WorldDescriptionClose");
			}
			return string.Format("{0} {1}", textValue, textValue2);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0001B332 File Offset: 0x00019532
		public static string GetRandomGameTitle()
		{
			return Language.RandomFromCategory("GameTitle", null).Value;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0001B344 File Offset: 0x00019544
		public static string DyeTraderQuestChat(bool gotDye = false)
		{
			LocalizedText[] array = Language.FindAll(Lang.CreateDialogFilter(gotDye ? "DyeTraderSpecialText.HasPlant" : "DyeTraderSpecialText.NoPlant", true));
			return array[Main.rand.Next(array.Length)].Value;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0001B380 File Offset: 0x00019580
		public static string AnglerQuestCountChat()
		{
			return Language.SelectRandom(Lang.CreateDialogFilter("AnglerQuestChatter.", true), null).Value;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0001B398 File Offset: 0x00019598
		public static string BartenderHelpText(NPC npc)
		{
			Player player = Main.player[Main.myPlayer];
			if (player.bartenderQuestLog == 0)
			{
				player.bartenderQuestLog++;
				Item item = new Item();
				item.SetDefaults(3817, null);
				item.stack = 10;
				player.QuickSpawnItem(new EntitySource_Gift(npc), item, GetItemSettings.GiftRecieved);
				return Language.GetTextValue("BartenderSpecialText.FirstHelp");
			}
			LocalizedText[] array = Language.FindAll(Lang.CreateDialogFilter("BartenderHelpText.", true));
			if (Main.BartenderHelpTextIndex >= array.Length)
			{
				Main.BartenderHelpTextIndex = 0;
			}
			return array[Main.BartenderHelpTextIndex++].Value;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0001B434 File Offset: 0x00019634
		public static string BartenderChat()
		{
			if (Main.rand.Next(5) == 0)
			{
				string key;
				if (DD2Event.DownedInvasionT3)
				{
					key = "BartenderSpecialText.AfterDD2Tier3";
				}
				else if (DD2Event.DownedInvasionT2)
				{
					key = "BartenderSpecialText.AfterDD2Tier2";
				}
				else if (DD2Event.DownedInvasionT1)
				{
					key = "BartenderSpecialText.AfterDD2Tier1";
				}
				else
				{
					key = "BartenderSpecialText.BeforeDD2Tier1";
				}
				return Language.GetTextValue(key);
			}
			return Language.SelectRandom(Lang.CreateDialogFilter("BartenderChatter.", true), null).Value;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0001B49E File Offset: 0x0001969E
		public static string GolferChat()
		{
			return Language.SelectRandom(Lang.CreateDialogFilter("GolferChatter.", true), null).Value;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0001B4B8 File Offset: 0x000196B8
		public static string BestiaryGirlChat()
		{
			string startsWith = "BestiaryGirlChatter.";
			if (NPC.ShouldBestiaryGirlBeLycantrope())
			{
				startsWith = "BestiaryGirlLycantropeChatter.";
			}
			return Language.SelectRandom(Lang.CreateDialogFilter(startsWith, true), null).Value;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0001B4EA File Offset: 0x000196EA
		public static string PrincessChat()
		{
			return Language.SelectRandom(Lang.CreateDialogFilter("PrincessChatter.", true), null).Value;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0001B502 File Offset: 0x00019702
		public static string CatChat()
		{
			return Language.SelectRandom(Lang.CreateDialogFilter("CatChatter.Chatter", true), null).Value;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0001B51A File Offset: 0x0001971A
		public static string DogChat()
		{
			return Language.SelectRandom(Lang.CreateDialogFilter("DogChatter.Chatter", true), null).Value;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0001B532 File Offset: 0x00019732
		public static string BunnyChat()
		{
			return Language.SelectRandom(Lang.CreateDialogFilter("BunnyChatter.Chatter", true), null).Value;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0001B54C File Offset: 0x0001974C
		public static string GetSlimeType(NPC npc)
		{
			string result = "Blue";
			switch (npc.type)
			{
			case 670:
				result = "Blue";
				break;
			case 678:
				result = "Green";
				break;
			case 679:
				result = "Old";
				break;
			case 680:
				result = "Purple";
				break;
			case 681:
				result = "Rainbow";
				break;
			case 682:
				result = "Red";
				break;
			case 683:
				result = "Yellow";
				break;
			case 684:
				result = "Copper";
				break;
			}
			return result;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0001B5F0 File Offset: 0x000197F0
		public static string SlimeChat(NPC npc)
		{
			string slimeType = Lang.GetSlimeType(npc);
			return Language.SelectRandom(Lang.CreateDialogFilter("Slime" + slimeType + "Chatter.Chatter", true), null).Value;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0001B625 File Offset: 0x00019825
		public static string GetNPCHouseBannerText(NPC npc, int bannerStyle)
		{
			if (bannerStyle == 1)
			{
				return Language.GetTextValue("Game.ReservedForNPC", npc.FullName);
			}
			return npc.FullName;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0001B642 File Offset: 0x00019842
		public static LanguageSearchFilter CreateDialogFilter(string startsWith, object substitutions)
		{
			return (string key, LocalizedText text) => key.StartsWith(startsWith) && text.ConditionsMetWith(substitutions);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0001B662 File Offset: 0x00019862
		public static LanguageSearchFilter CreateDialogFilter(string startsWith, bool checkConditions = true)
		{
			return (string key, LocalizedText text) => key.StartsWith(startsWith) && (!checkConditions || text.ConditionsMet);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0001B684 File Offset: 0x00019884
		public static string AnglerQuestChat(bool turnIn = false)
		{
			if (turnIn)
			{
				return Language.SelectRandom(Lang.CreateDialogFilter("AnglerQuestText.TurnIn_", true), null).Value;
			}
			if (Main.anglerQuestFinished)
			{
				return Language.SelectRandom(Lang.CreateDialogFilter("AnglerQuestText.NoQuest_", true), null).Value;
			}
			int num = Main.anglerQuestItemNetIDs[Main.anglerQuest];
			Main.npcChatCornerItem = num;
			return Language.GetTextValue("AnglerQuestText.Quest_" + ItemID.Search.GetName(num));
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0001B6F5 File Offset: 0x000198F5
		public static LocalizedText GetProjectileName(int type)
		{
			if (type >= 0 && type < Lang._projectileNameCache.Length && Lang._projectileNameCache[type] != null)
			{
				return Lang._projectileNameCache[type];
			}
			return LocalizedText.Empty;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0001B71C File Offset: 0x0001991C
		private static void FillNameCacheArray<IdClass, IdType>(string category, LocalizedText[] nameCache, bool leaveMissingEntriesBlank = false) where IdType : IConvertible
		{
			for (int i = 0; i < nameCache.Length; i++)
			{
				nameCache[i] = LocalizedText.Empty;
			}
			(from f in typeof(IdClass).GetFields(BindingFlags.Static | BindingFlags.Public)
			where f.FieldType == typeof(IdType)
			select f).ToList<FieldInfo>().ForEach(delegate(FieldInfo field)
			{
				long num = Convert.ToInt64((IdType)((object)field.GetValue(null)));
				if (num >= 0L && num < (long)nameCache.Length)
				{
					nameCache[(int)(checked((IntPtr)num))] = ((!leaveMissingEntriesBlank || Language.Exists(category + "." + field.Name)) ? Language.GetText(category + "." + field.Name) : LocalizedText.Empty);
					return;
				}
				if (field.Name == "None")
				{
					nameCache[(int)(checked((IntPtr)num))] = LocalizedText.Empty;
				}
			});
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0001B7B0 File Offset: 0x000199B0
		public static void InitializeLegacyLocalization()
		{
			Lang.FillNameCacheArray<PrefixID, int>("Prefix", Lang.prefix, false);
			for (int i = 0; i < Lang.gen.Length; i++)
			{
				Lang.gen[i] = Language.GetText("LegacyWorldGen." + i);
			}
			for (int j = 0; j < Lang.menu.Length; j++)
			{
				Lang.menu[j] = Language.GetText("LegacyMenu." + j);
			}
			for (int k = 0; k < Lang.inter.Length; k++)
			{
				Lang.inter[k] = Language.GetText("LegacyInterface." + k);
			}
			for (int l = 0; l < Lang.misc.Length; l++)
			{
				Lang.misc[l] = Language.GetText("LegacyMisc." + l);
			}
			for (int m = 0; m < Lang.mp.Length; m++)
			{
				Lang.mp[m] = Language.GetText("LegacyMultiplayer." + m);
			}
			for (int n = 0; n < Lang.tip.Length; n++)
			{
				Lang.tip[n] = Language.GetText("LegacyTooltip." + n);
			}
			for (int num = 0; num < Lang.chestType.Length; num++)
			{
				Lang.chestType[num] = Language.GetText("LegacyChestType." + num);
			}
			for (int num2 = 0; num2 < Lang.chestType2.Length; num2++)
			{
				Lang.chestType2[num2] = Language.GetText("LegacyChestType2." + num2);
			}
			for (int num3 = 0; num3 < Lang.dresserType.Length; num3++)
			{
				Lang.dresserType[num3] = Language.GetText("LegacyDresserType." + num3);
			}
			Lang.FillNameCacheArray<ItemID, short>("ItemName", Lang._itemNameCache, false);
			Lang.FillNameCacheArray<ProjectileID, short>("ProjectileName", Lang._projectileNameCache, false);
			Lang.FillNameCacheArray<NPCID, short>("NPCName", Lang._npcNameCache, false);
			Lang.FillNameCacheArray<BuffID, int>("BuffName", Lang._buffNameCache, false);
			Lang.FillNameCacheArray<BuffID, int>("BuffDescription", Lang._buffDescriptionCache, false);
			Lang.FillNameCacheArray<EmoteID, int>("EmojiName", Lang._emojiNameCache, true);
			for (int num4 = -65; num4 < 0; num4++)
			{
				Lang._negativeNpcNameCache[-num4 - 1] = Lang._npcNameCache[NPCID.FromNetId(num4)];
			}
			Lang._negativeNpcNameCache[0] = Language.GetText("NPCName.Slimeling");
			Lang._negativeNpcNameCache[1] = Language.GetText("NPCName.Slimer2");
			Lang._negativeNpcNameCache[2] = Language.GetText("NPCName.GreenSlime");
			Lang._negativeNpcNameCache[3] = Language.GetText("NPCName.Pinky");
			Lang._negativeNpcNameCache[4] = Language.GetText("NPCName.BabySlime");
			Lang._negativeNpcNameCache[5] = Language.GetText("NPCName.BlackSlime");
			Lang._negativeNpcNameCache[6] = Language.GetText("NPCName.PurpleSlime");
			Lang._negativeNpcNameCache[7] = Language.GetText("NPCName.RedSlime");
			Lang._negativeNpcNameCache[8] = Language.GetText("NPCName.YellowSlime");
			Lang._negativeNpcNameCache[9] = Language.GetText("NPCName.JungleSlime");
			Lang._negativeNpcNameCache[53] = Language.GetText("NPCName.SmallRainZombie");
			Lang._negativeNpcNameCache[54] = Language.GetText("NPCName.BigRainZombie");
			for (int num5 = 0; num5 < Lang._itemTooltipCache.Length; num5++)
			{
				Lang._itemTooltipCache[num5] = ItemTooltip.None;
			}
			(from f in typeof(ItemID).GetFields(BindingFlags.Static | BindingFlags.Public)
			where f.FieldType == typeof(short)
			select f).ToList<FieldInfo>().ForEach(delegate(FieldInfo field)
			{
				short num6 = (short)field.GetValue(null);
				if (num6 > 0 && (int)num6 < Lang._itemTooltipCache.Length)
				{
					Lang._itemTooltipCache[(int)num6] = ItemTooltip.FromLanguageKey("ItemTooltip." + field.Name);
				}
			});
			Lang.InitGlobalSubstitutions();
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0001BB68 File Offset: 0x00019D68
		public static void BuildMapAtlas()
		{
			if (Main.dedServ)
			{
				return;
			}
			Lang._mapLegendCache = new LocalizedText[MapHelper.LookupCount()];
			for (int i = 0; i < Lang._mapLegendCache.Length; i++)
			{
				Lang._mapLegendCache[i] = LocalizedText.Empty;
			}
			for (int j = 0; j < TileID.Sets.IsATreeTrunk.Length; j++)
			{
				if (TileID.Sets.IsATreeTrunk[j])
				{
					Lang._mapLegendCache[MapHelper.TileToLookup(j, 0)] = Language.GetText("MapObject.Tree");
				}
			}
			LocalizedText[] itemNameCache = Lang._itemNameCache;
			Lang._mapLegendCache[MapHelper.TileToLookup(4, 0)] = itemNameCache[8];
			Lang._mapLegendCache[MapHelper.TileToLookup(4, 1)] = itemNameCache[8];
			Lang._mapLegendCache[MapHelper.TileToLookup(6, 0)] = Language.GetText("MapObject.Iron");
			Lang._mapLegendCache[MapHelper.TileToLookup(7, 0)] = Language.GetText("MapObject.Copper");
			Lang._mapLegendCache[MapHelper.TileToLookup(8, 0)] = Language.GetText("MapObject.Gold");
			Lang._mapLegendCache[MapHelper.TileToLookup(9, 0)] = Language.GetText("MapObject.Silver");
			Lang._mapLegendCache[MapHelper.TileToLookup(10, 0)] = Language.GetText("MapObject.Door");
			Lang._mapLegendCache[MapHelper.TileToLookup(11, 0)] = Language.GetText("MapObject.Door");
			Lang._mapLegendCache[MapHelper.TileToLookup(12, 0)] = itemNameCache[29];
			Lang._mapLegendCache[MapHelper.TileToLookup(665, 0)] = itemNameCache[29];
			Lang._mapLegendCache[MapHelper.TileToLookup(711, 0)] = itemNameCache[540];
			Lang._mapLegendCache[MapHelper.TileToLookup(664, 0)] = itemNameCache[540];
			Lang._mapLegendCache[MapHelper.TileToLookup(712, 0)] = itemNameCache[540];
			Lang._mapLegendCache[MapHelper.TileToLookup(713, 0)] = itemNameCache[540];
			Lang._mapLegendCache[MapHelper.TileToLookup(714, 0)] = itemNameCache[540];
			Lang._mapLegendCache[MapHelper.TileToLookup(715, 0)] = itemNameCache[540];
			Lang._mapLegendCache[MapHelper.TileToLookup(716, 0)] = itemNameCache[540];
			Lang._mapLegendCache[MapHelper.TileToLookup(639, 0)] = itemNameCache[109];
			Lang._mapLegendCache[MapHelper.TileToLookup(630, 0)] = itemNameCache[5137];
			Lang._mapLegendCache[MapHelper.TileToLookup(631, 0)] = itemNameCache[5138];
			Lang._mapLegendCache[MapHelper.TileToLookup(13, 0)] = itemNameCache[31];
			Lang._mapLegendCache[MapHelper.TileToLookup(14, 0)] = Language.GetText("MapObject.Table");
			Lang._mapLegendCache[MapHelper.TileToLookup(469, 0)] = Language.GetText("MapObject.Table");
			Lang._mapLegendCache[MapHelper.TileToLookup(486, 0)] = itemNameCache[4063];
			Lang._mapLegendCache[MapHelper.TileToLookup(487, 0)] = itemNameCache[4064];
			Lang._mapLegendCache[MapHelper.TileToLookup(487, 1)] = itemNameCache[4065];
			Lang._mapLegendCache[MapHelper.TileToLookup(489, 0)] = itemNameCache[4074];
			Lang._mapLegendCache[MapHelper.TileToLookup(490, 0)] = itemNameCache[4075];
			Lang._mapLegendCache[MapHelper.TileToLookup(15, 0)] = Language.GetText("MapObject.Chair");
			Lang._mapLegendCache[MapHelper.TileToLookup(15, 1)] = Language.GetText("MapObject.Toilet");
			Lang._mapLegendCache[MapHelper.TileToLookup(16, 0)] = Language.GetText("MapObject.Anvil");
			Lang._mapLegendCache[MapHelper.TileToLookup(17, 0)] = itemNameCache[33];
			Lang._mapLegendCache[MapHelper.TileToLookup(18, 0)] = itemNameCache[36];
			Lang._mapLegendCache[MapHelper.TileToLookup(20, 0)] = Language.GetText("MapObject.Sapling");
			Lang._mapLegendCache[MapHelper.TileToLookup(590, 0)] = Language.GetText("MapObject.Sapling");
			Lang._mapLegendCache[MapHelper.TileToLookup(595, 0)] = Language.GetText("MapObject.Sapling");
			Lang._mapLegendCache[MapHelper.TileToLookup(615, 0)] = Language.GetText("MapObject.Sapling");
			Lang._mapLegendCache[MapHelper.TileToLookup(21, 0)] = itemNameCache[48];
			Lang._mapLegendCache[MapHelper.TileToLookup(467, 0)] = itemNameCache[48];
			Lang._mapLegendCache[MapHelper.TileToLookup(22, 0)] = Language.GetText("MapObject.Demonite");
			Lang._mapLegendCache[MapHelper.TileToLookup(26, 0)] = Language.GetText("MapObject.DemonAltar");
			Lang._mapLegendCache[MapHelper.TileToLookup(26, 1)] = Language.GetText("MapObject.CrimsonAltar");
			Lang._mapLegendCache[MapHelper.TileToLookup(27, 0)] = itemNameCache[63];
			Lang._mapLegendCache[MapHelper.TileToLookup(407, 0)] = Language.GetText("MapObject.Fossil");
			Lang._mapLegendCache[MapHelper.TileToLookup(412, 0)] = itemNameCache[3549];
			Lang._mapLegendCache[MapHelper.TileToLookup(441, 0)] = itemNameCache[48];
			Lang._mapLegendCache[MapHelper.TileToLookup(468, 0)] = itemNameCache[48];
			Lang._mapLegendCache[MapHelper.TileToLookup(404, 0)] = Language.GetText("MapObject.DesertFossil");
			Lang._mapLegendCache[MapHelper.TileToLookup(654, 0)] = itemNameCache[5327];
			Lang._mapLegendCache[MapHelper.TileToLookup(695, 0)] = itemNameCache[5467];
			Lang._mapLegendCache[MapHelper.TileToLookup(695, 1)] = itemNameCache[5468];
			Lang._mapLegendCache[MapHelper.TileToLookup(696, 0)] = itemNameCache[5469];
			Lang._mapLegendCache[MapHelper.TileToLookup(696, 1)] = itemNameCache[5470];
			for (int k = 0; k < 9; k++)
			{
				Lang._mapLegendCache[MapHelper.TileToLookup(28, k)] = Language.GetText("MapObject.Pot");
				Lang._mapLegendCache[MapHelper.TileToLookup(653, k)] = Language.GetText("MapObject.Pot");
			}
			Lang._mapLegendCache[MapHelper.TileToLookup(37, 0)] = itemNameCache[116];
			Lang._mapLegendCache[MapHelper.TileToLookup(29, 0)] = itemNameCache[87];
			Lang._mapLegendCache[MapHelper.TileToLookup(31, 0)] = itemNameCache[115];
			Lang._mapLegendCache[MapHelper.TileToLookup(31, 1)] = itemNameCache[3062];
			Lang._mapLegendCache[MapHelper.TileToLookup(32, 0)] = Language.GetText("MapObject.Thorns");
			Lang._mapLegendCache[MapHelper.TileToLookup(33, 0)] = itemNameCache[105];
			Lang._mapLegendCache[MapHelper.TileToLookup(34, 0)] = Language.GetText("MapObject.Chandelier");
			Lang._mapLegendCache[MapHelper.TileToLookup(35, 0)] = itemNameCache[1813];
			Lang._mapLegendCache[MapHelper.TileToLookup(36, 0)] = itemNameCache[1869];
			Lang._mapLegendCache[MapHelper.TileToLookup(476, 0)] = itemNameCache[4040];
			Lang._mapLegendCache[MapHelper.TileToLookup(42, 0)] = Language.GetText("MapObject.Lantern");
			Lang._mapLegendCache[MapHelper.TileToLookup(48, 0)] = itemNameCache[147];
			Lang._mapLegendCache[MapHelper.TileToLookup(49, 0)] = itemNameCache[148];
			Lang._mapLegendCache[MapHelper.TileToLookup(50, 0)] = itemNameCache[149];
			Lang._mapLegendCache[MapHelper.TileToLookup(707, 0)] = itemNameCache[149];
			Lang._mapLegendCache[MapHelper.TileToLookup(51, 0)] = Language.GetText("MapObject.Web");
			Lang._mapLegendCache[MapHelper.TileToLookup(697, 0)] = Language.GetText("MapObject.Web");
			Lang._mapLegendCache[MapHelper.TileToLookup(55, 0)] = itemNameCache[171];
			Lang._mapLegendCache[MapHelper.TileToLookup(454, 0)] = itemNameCache[3746];
			Lang._mapLegendCache[MapHelper.TileToLookup(455, 0)] = itemNameCache[3747];
			Lang._mapLegendCache[MapHelper.TileToLookup(452, 0)] = itemNameCache[3742];
			Lang._mapLegendCache[MapHelper.TileToLookup(456, 0)] = itemNameCache[3748];
			Lang._mapLegendCache[MapHelper.TileToLookup(453, 0)] = itemNameCache[3744];
			Lang._mapLegendCache[MapHelper.TileToLookup(453, 1)] = itemNameCache[3745];
			Lang._mapLegendCache[MapHelper.TileToLookup(453, 2)] = itemNameCache[3743];
			Lang._mapLegendCache[MapHelper.TileToLookup(573, 0)] = itemNameCache[4710];
			Lang._mapLegendCache[MapHelper.TileToLookup(63, 0)] = itemNameCache[177];
			Lang._mapLegendCache[MapHelper.TileToLookup(64, 0)] = itemNameCache[178];
			Lang._mapLegendCache[MapHelper.TileToLookup(65, 0)] = itemNameCache[179];
			Lang._mapLegendCache[MapHelper.TileToLookup(66, 0)] = itemNameCache[180];
			Lang._mapLegendCache[MapHelper.TileToLookup(67, 0)] = itemNameCache[181];
			Lang._mapLegendCache[MapHelper.TileToLookup(68, 0)] = itemNameCache[182];
			Lang._mapLegendCache[MapHelper.TileToLookup(566, 0)] = itemNameCache[999];
			Lang._mapLegendCache[MapHelper.TileToLookup(69, 0)] = Language.GetText("MapObject.Thorn");
			Lang._mapLegendCache[MapHelper.TileToLookup(72, 0)] = Language.GetText("MapObject.GiantMushroom");
			Lang._mapLegendCache[MapHelper.TileToLookup(77, 0)] = itemNameCache[221];
			Lang._mapLegendCache[MapHelper.TileToLookup(78, 0)] = itemNameCache[222];
			Lang._mapLegendCache[MapHelper.TileToLookup(79, 0)] = itemNameCache[224];
			Lang._mapLegendCache[MapHelper.TileToLookup(80, 0)] = itemNameCache[276];
			Lang._mapLegendCache[MapHelper.TileToLookup(81, 0)] = itemNameCache[275];
			Lang._mapLegendCache[MapHelper.TileToLookup(82, 0)] = itemNameCache[313];
			Lang._mapLegendCache[MapHelper.TileToLookup(82, 1)] = itemNameCache[314];
			Lang._mapLegendCache[MapHelper.TileToLookup(82, 2)] = itemNameCache[315];
			Lang._mapLegendCache[MapHelper.TileToLookup(82, 3)] = itemNameCache[316];
			Lang._mapLegendCache[MapHelper.TileToLookup(82, 4)] = itemNameCache[317];
			Lang._mapLegendCache[MapHelper.TileToLookup(82, 5)] = itemNameCache[318];
			Lang._mapLegendCache[MapHelper.TileToLookup(82, 6)] = itemNameCache[2358];
			Lang._mapLegendCache[MapHelper.TileToLookup(83, 0)] = itemNameCache[313];
			Lang._mapLegendCache[MapHelper.TileToLookup(83, 1)] = itemNameCache[314];
			Lang._mapLegendCache[MapHelper.TileToLookup(83, 2)] = itemNameCache[315];
			Lang._mapLegendCache[MapHelper.TileToLookup(83, 3)] = itemNameCache[316];
			Lang._mapLegendCache[MapHelper.TileToLookup(83, 4)] = itemNameCache[317];
			Lang._mapLegendCache[MapHelper.TileToLookup(83, 5)] = itemNameCache[318];
			Lang._mapLegendCache[MapHelper.TileToLookup(83, 6)] = itemNameCache[2358];
			Lang._mapLegendCache[MapHelper.TileToLookup(84, 0)] = itemNameCache[313];
			Lang._mapLegendCache[MapHelper.TileToLookup(84, 1)] = itemNameCache[314];
			Lang._mapLegendCache[MapHelper.TileToLookup(84, 2)] = itemNameCache[315];
			Lang._mapLegendCache[MapHelper.TileToLookup(84, 3)] = itemNameCache[316];
			Lang._mapLegendCache[MapHelper.TileToLookup(84, 4)] = itemNameCache[317];
			Lang._mapLegendCache[MapHelper.TileToLookup(84, 5)] = itemNameCache[318];
			Lang._mapLegendCache[MapHelper.TileToLookup(84, 6)] = itemNameCache[2358];
			Lang._mapLegendCache[MapHelper.TileToLookup(85, 0)] = itemNameCache[321];
			Lang._mapLegendCache[MapHelper.TileToLookup(86, 0)] = itemNameCache[332];
			Lang._mapLegendCache[MapHelper.TileToLookup(87, 0)] = itemNameCache[333];
			Lang._mapLegendCache[MapHelper.TileToLookup(88, 0)] = itemNameCache[334];
			Lang._mapLegendCache[MapHelper.TileToLookup(89, 0)] = itemNameCache[335];
			Lang._mapLegendCache[MapHelper.TileToLookup(89, 1)] = itemNameCache[2397];
			Lang._mapLegendCache[MapHelper.TileToLookup(89, 2)] = itemNameCache[4993];
			Lang._mapLegendCache[MapHelper.TileToLookup(90, 0)] = itemNameCache[336];
			Lang._mapLegendCache[MapHelper.TileToLookup(91, 0)] = Language.GetText("MapObject.Banner");
			Lang._mapLegendCache[MapHelper.TileToLookup(92, 0)] = itemNameCache[341];
			Lang._mapLegendCache[MapHelper.TileToLookup(93, 0)] = Language.GetText("MapObject.FloorLamp");
			Lang._mapLegendCache[MapHelper.TileToLookup(94, 0)] = itemNameCache[352];
			Lang._mapLegendCache[MapHelper.TileToLookup(95, 0)] = itemNameCache[344];
			Lang._mapLegendCache[MapHelper.TileToLookup(96, 0)] = itemNameCache[345];
			Lang._mapLegendCache[MapHelper.TileToLookup(97, 0)] = itemNameCache[346];
			Lang._mapLegendCache[MapHelper.TileToLookup(98, 0)] = itemNameCache[347];
			Lang._mapLegendCache[MapHelper.TileToLookup(100, 0)] = itemNameCache[349];
			Lang._mapLegendCache[MapHelper.TileToLookup(101, 0)] = itemNameCache[354];
			Lang._mapLegendCache[MapHelper.TileToLookup(102, 0)] = itemNameCache[355];
			Lang._mapLegendCache[MapHelper.TileToLookup(103, 0)] = itemNameCache[356];
			Lang._mapLegendCache[MapHelper.TileToLookup(104, 0)] = itemNameCache[359];
			Lang._mapLegendCache[MapHelper.TileToLookup(105, 0)] = Language.GetText("MapObject.Statue");
			Lang._mapLegendCache[MapHelper.TileToLookup(105, 2)] = Language.GetText("MapObject.Vase");
			Lang._mapLegendCache[MapHelper.TileToLookup(106, 0)] = itemNameCache[363];
			Lang._mapLegendCache[MapHelper.TileToLookup(107, 0)] = Language.GetText("MapObject.Cobalt");
			Lang._mapLegendCache[MapHelper.TileToLookup(108, 0)] = Language.GetText("MapObject.Mythril");
			Lang._mapLegendCache[MapHelper.TileToLookup(111, 0)] = Language.GetText("MapObject.Adamantite");
			Lang._mapLegendCache[MapHelper.TileToLookup(114, 0)] = itemNameCache[398];
			Lang._mapLegendCache[MapHelper.TileToLookup(125, 0)] = itemNameCache[487];
			Lang._mapLegendCache[MapHelper.TileToLookup(128, 0)] = itemNameCache[498];
			Lang._mapLegendCache[MapHelper.TileToLookup(129, 0)] = itemNameCache[502];
			Lang._mapLegendCache[MapHelper.TileToLookup(129, 1)] = itemNameCache[4988];
			Lang._mapLegendCache[MapHelper.TileToLookup(132, 0)] = itemNameCache[513];
			Lang._mapLegendCache[MapHelper.TileToLookup(411, 0)] = itemNameCache[3545];
			Lang._mapLegendCache[MapHelper.TileToLookup(133, 0)] = itemNameCache[524];
			Lang._mapLegendCache[MapHelper.TileToLookup(133, 1)] = itemNameCache[1221];
			Lang._mapLegendCache[MapHelper.TileToLookup(134, 0)] = itemNameCache[525];
			Lang._mapLegendCache[MapHelper.TileToLookup(134, 1)] = itemNameCache[1220];
			Lang._mapLegendCache[MapHelper.TileToLookup(136, 0)] = itemNameCache[538];
			Lang._mapLegendCache[MapHelper.TileToLookup(137, 0)] = Language.GetText("MapObject.Trap");
			Lang._mapLegendCache[MapHelper.TileToLookup(138, 0)] = itemNameCache[540];
			Lang._mapLegendCache[MapHelper.TileToLookup(139, 0)] = itemNameCache[576];
			Lang._mapLegendCache[MapHelper.TileToLookup(142, 0)] = itemNameCache[581];
			Lang._mapLegendCache[MapHelper.TileToLookup(143, 0)] = itemNameCache[582];
			Lang._mapLegendCache[MapHelper.TileToLookup(144, 0)] = Language.GetText("MapObject.Timer");
			Lang._mapLegendCache[MapHelper.TileToLookup(149, 0)] = Language.GetText("MapObject.ChristmasLight");
			Lang._mapLegendCache[MapHelper.TileToLookup(166, 0)] = Language.GetText("MapObject.Tin");
			Lang._mapLegendCache[MapHelper.TileToLookup(167, 0)] = Language.GetText("MapObject.Lead");
			Lang._mapLegendCache[MapHelper.TileToLookup(168, 0)] = Language.GetText("MapObject.Tungsten");
			Lang._mapLegendCache[MapHelper.TileToLookup(169, 0)] = Language.GetText("MapObject.Platinum");
			Lang._mapLegendCache[MapHelper.TileToLookup(170, 0)] = Language.GetText("MapObject.PineTree");
			Lang._mapLegendCache[MapHelper.TileToLookup(171, 0)] = itemNameCache[1873];
			Lang._mapLegendCache[MapHelper.TileToLookup(172, 0)] = Language.GetText("MapObject.Sink");
			Lang._mapLegendCache[MapHelper.TileToLookup(173, 0)] = itemNameCache[349];
			Lang._mapLegendCache[MapHelper.TileToLookup(174, 0)] = itemNameCache[713];
			Lang._mapLegendCache[MapHelper.TileToLookup(178, 0)] = itemNameCache[181];
			Lang._mapLegendCache[MapHelper.TileToLookup(178, 1)] = itemNameCache[180];
			Lang._mapLegendCache[MapHelper.TileToLookup(178, 2)] = itemNameCache[177];
			Lang._mapLegendCache[MapHelper.TileToLookup(178, 3)] = itemNameCache[179];
			Lang._mapLegendCache[MapHelper.TileToLookup(178, 4)] = itemNameCache[178];
			Lang._mapLegendCache[MapHelper.TileToLookup(178, 5)] = itemNameCache[182];
			Lang._mapLegendCache[MapHelper.TileToLookup(178, 6)] = itemNameCache[999];
			Lang._mapLegendCache[MapHelper.TileToLookup(191, 0)] = Language.GetText("MapObject.LivingWood");
			Lang._mapLegendCache[MapHelper.TileToLookup(204, 0)] = Language.GetText("MapObject.Crimtane");
			Lang._mapLegendCache[MapHelper.TileToLookup(207, 0)] = Language.GetText("MapObject.WaterFountain");
			Lang._mapLegendCache[MapHelper.TileToLookup(209, 0)] = itemNameCache[928];
			Lang._mapLegendCache[MapHelper.TileToLookup(211, 0)] = Language.GetText("MapObject.Chlorophyte");
			Lang._mapLegendCache[MapHelper.TileToLookup(212, 0)] = Language.GetText("MapObject.Turret");
			Lang._mapLegendCache[MapHelper.TileToLookup(213, 0)] = itemNameCache[965];
			Lang._mapLegendCache[MapHelper.TileToLookup(214, 0)] = itemNameCache[85];
			Lang._mapLegendCache[MapHelper.TileToLookup(215, 0)] = itemNameCache[966];
			Lang._mapLegendCache[MapHelper.TileToLookup(216, 0)] = Language.GetText("MapObject.Rocket");
			Lang._mapLegendCache[MapHelper.TileToLookup(217, 0)] = itemNameCache[995];
			Lang._mapLegendCache[MapHelper.TileToLookup(218, 0)] = itemNameCache[996];
			Lang._mapLegendCache[MapHelper.TileToLookup(219, 0)] = Language.GetText("MapObject.SiltExtractinator");
			Lang._mapLegendCache[MapHelper.TileToLookup(642, 0)] = Language.GetText("MapObject.ChlorophyteExtractinator");
			Lang._mapLegendCache[MapHelper.TileToLookup(220, 0)] = itemNameCache[998];
			Lang._mapLegendCache[MapHelper.TileToLookup(221, 0)] = Language.GetText("MapObject.Palladium");
			Lang._mapLegendCache[MapHelper.TileToLookup(222, 0)] = Language.GetText("MapObject.Orichalcum");
			Lang._mapLegendCache[MapHelper.TileToLookup(223, 0)] = Language.GetText("MapObject.Titanium");
			Lang._mapLegendCache[MapHelper.TileToLookup(227, 0)] = itemNameCache[1107];
			Lang._mapLegendCache[MapHelper.TileToLookup(227, 1)] = itemNameCache[1108];
			Lang._mapLegendCache[MapHelper.TileToLookup(227, 2)] = itemNameCache[1109];
			Lang._mapLegendCache[MapHelper.TileToLookup(227, 3)] = itemNameCache[1110];
			Lang._mapLegendCache[MapHelper.TileToLookup(227, 4)] = itemNameCache[1111];
			Lang._mapLegendCache[MapHelper.TileToLookup(227, 5)] = itemNameCache[1112];
			Lang._mapLegendCache[MapHelper.TileToLookup(227, 6)] = itemNameCache[1113];
			Lang._mapLegendCache[MapHelper.TileToLookup(227, 7)] = itemNameCache[1114];
			Lang._mapLegendCache[MapHelper.TileToLookup(227, 8)] = itemNameCache[3385];
			Lang._mapLegendCache[MapHelper.TileToLookup(227, 9)] = itemNameCache[3386];
			Lang._mapLegendCache[MapHelper.TileToLookup(227, 10)] = itemNameCache[3387];
			Lang._mapLegendCache[MapHelper.TileToLookup(227, 11)] = itemNameCache[3388];
			Lang._mapLegendCache[MapHelper.TileToLookup(228, 0)] = itemNameCache[1120];
			Lang._mapLegendCache[MapHelper.TileToLookup(231, 0)] = Language.GetText("MapObject.Larva");
			Lang._mapLegendCache[MapHelper.TileToLookup(232, 0)] = itemNameCache[1150];
			Lang._mapLegendCache[MapHelper.TileToLookup(235, 0)] = itemNameCache[1263];
			Lang._mapLegendCache[MapHelper.TileToLookup(624, 0)] = itemNameCache[5114];
			Lang._mapLegendCache[MapHelper.TileToLookup(700, 0)] = itemNameCache[5114];
			Lang._mapLegendCache[MapHelper.TileToLookup(656, 0)] = itemNameCache[5333];
			Lang._mapLegendCache[MapHelper.TileToLookup(701, 0)] = itemNameCache[5333];
			Lang._mapLegendCache[MapHelper.TileToLookup(236, 0)] = itemNameCache[1291];
			Lang._mapLegendCache[MapHelper.TileToLookup(237, 0)] = itemNameCache[1292];
			Lang._mapLegendCache[MapHelper.TileToLookup(238, 0)] = Language.GetText("MapObject.PlanterasBulb");
			Lang._mapLegendCache[MapHelper.TileToLookup(239, 0)] = Language.GetText("MapObject.MetalBar");
			Lang._mapLegendCache[MapHelper.TileToLookup(240, 0)] = Language.GetText("MapObject.Trophy");
			Lang._mapLegendCache[MapHelper.TileToLookup(240, 1)] = Language.GetText("MapObject.Painting");
			Lang._mapLegendCache[MapHelper.TileToLookup(240, 2)] = Lang._npcNameCache[21];
			Lang._mapLegendCache[MapHelper.TileToLookup(240, 3)] = Language.GetText("MapObject.ItemRack");
			Lang._mapLegendCache[MapHelper.TileToLookup(240, 4)] = itemNameCache[2442];
			Lang._mapLegendCache[MapHelper.TileToLookup(241, 0)] = itemNameCache[1417];
			Lang._mapLegendCache[MapHelper.TileToLookup(242, 0)] = Language.GetText("MapObject.Painting");
			Lang._mapLegendCache[MapHelper.TileToLookup(242, 1)] = Language.GetText("MapObject.AnimalSkin");
			Lang._mapLegendCache[MapHelper.TileToLookup(243, 0)] = itemNameCache[1430];
			Lang._mapLegendCache[MapHelper.TileToLookup(244, 0)] = itemNameCache[1449];
			Lang._mapLegendCache[MapHelper.TileToLookup(245, 0)] = Language.GetText("MapObject.Picture");
			Lang._mapLegendCache[MapHelper.TileToLookup(246, 0)] = Language.GetText("MapObject.Picture");
			Lang._mapLegendCache[MapHelper.TileToLookup(247, 0)] = itemNameCache[1551];
			Lang._mapLegendCache[MapHelper.TileToLookup(254, 0)] = itemNameCache[1725];
			Lang._mapLegendCache[MapHelper.TileToLookup(269, 0)] = itemNameCache[1989];
			Lang._mapLegendCache[MapHelper.TileToLookup(475, 0)] = itemNameCache[3977];
			Lang._mapLegendCache[MapHelper.TileToLookup(597, 0)] = itemNameCache[4876];
			Lang._mapLegendCache[MapHelper.TileToLookup(597, 1)] = itemNameCache[4875];
			Lang._mapLegendCache[MapHelper.TileToLookup(597, 2)] = itemNameCache[4916];
			Lang._mapLegendCache[MapHelper.TileToLookup(597, 3)] = itemNameCache[4917];
			Lang._mapLegendCache[MapHelper.TileToLookup(597, 4)] = itemNameCache[4918];
			Lang._mapLegendCache[MapHelper.TileToLookup(597, 5)] = itemNameCache[4919];
			Lang._mapLegendCache[MapHelper.TileToLookup(597, 6)] = itemNameCache[4920];
			Lang._mapLegendCache[MapHelper.TileToLookup(597, 7)] = itemNameCache[4921];
			Lang._mapLegendCache[MapHelper.TileToLookup(597, 8)] = itemNameCache[4951];
			Lang._mapLegendCache[MapHelper.TileToLookup(597, 9)] = itemNameCache[5652];
			Lang._mapLegendCache[MapHelper.TileToLookup(597, 10)] = itemNameCache[5653];
			Lang._mapLegendCache[MapHelper.TileToLookup(617, 0)] = Language.GetText("MapObject.Relic");
			Lang._mapLegendCache[MapHelper.TileToLookup(621, 0)] = itemNameCache[3750];
			Lang._mapLegendCache[MapHelper.TileToLookup(622, 0)] = itemNameCache[5008];
			Lang._mapLegendCache[MapHelper.TileToLookup(270, 0)] = itemNameCache[1993];
			Lang._mapLegendCache[MapHelper.TileToLookup(271, 0)] = itemNameCache[2005];
			Lang._mapLegendCache[MapHelper.TileToLookup(581, 0)] = itemNameCache[4848];
			Lang._mapLegendCache[MapHelper.TileToLookup(660, 0)] = itemNameCache[5351];
			Lang._mapLegendCache[MapHelper.TileToLookup(275, 0)] = itemNameCache[2162];
			Lang._mapLegendCache[MapHelper.TileToLookup(276, 0)] = itemNameCache[2163];
			Lang._mapLegendCache[MapHelper.TileToLookup(277, 0)] = itemNameCache[2164];
			Lang._mapLegendCache[MapHelper.TileToLookup(278, 0)] = itemNameCache[2165];
			Lang._mapLegendCache[MapHelper.TileToLookup(279, 0)] = itemNameCache[2166];
			Lang._mapLegendCache[MapHelper.TileToLookup(280, 0)] = itemNameCache[2167];
			Lang._mapLegendCache[MapHelper.TileToLookup(281, 0)] = itemNameCache[2168];
			Lang._mapLegendCache[MapHelper.TileToLookup(632, 0)] = itemNameCache[5213];
			Lang._mapLegendCache[MapHelper.TileToLookup(640, 0)] = itemNameCache[5301];
			Lang._mapLegendCache[MapHelper.TileToLookup(643, 0)] = itemNameCache[5314];
			Lang._mapLegendCache[MapHelper.TileToLookup(644, 0)] = itemNameCache[5315];
			Lang._mapLegendCache[MapHelper.TileToLookup(645, 0)] = itemNameCache[5316];
			Lang._mapLegendCache[MapHelper.TileToLookup(282, 0)] = itemNameCache[250];
			Lang._mapLegendCache[MapHelper.TileToLookup(543, 0)] = itemNameCache[4398];
			Lang._mapLegendCache[MapHelper.TileToLookup(598, 0)] = itemNameCache[4880];
			Lang._mapLegendCache[MapHelper.TileToLookup(413, 0)] = itemNameCache[3565];
			Lang._mapLegendCache[MapHelper.TileToLookup(710, 0)] = itemNameCache[5512];
			Lang._mapLegendCache[MapHelper.TileToLookup(283, 0)] = itemNameCache[2172];
			Lang._mapLegendCache[MapHelper.TileToLookup(285, 0)] = itemNameCache[2174];
			Lang._mapLegendCache[MapHelper.TileToLookup(286, 0)] = itemNameCache[2175];
			Lang._mapLegendCache[MapHelper.TileToLookup(582, 0)] = itemNameCache[4850];
			Lang._mapLegendCache[MapHelper.TileToLookup(287, 0)] = itemNameCache[2177];
			Lang._mapLegendCache[MapHelper.TileToLookup(288, 0)] = itemNameCache[2178];
			Lang._mapLegendCache[MapHelper.TileToLookup(289, 0)] = itemNameCache[2179];
			Lang._mapLegendCache[MapHelper.TileToLookup(290, 0)] = itemNameCache[2180];
			Lang._mapLegendCache[MapHelper.TileToLookup(291, 0)] = itemNameCache[2181];
			Lang._mapLegendCache[MapHelper.TileToLookup(292, 0)] = itemNameCache[2182];
			Lang._mapLegendCache[MapHelper.TileToLookup(293, 0)] = itemNameCache[2183];
			Lang._mapLegendCache[MapHelper.TileToLookup(294, 0)] = itemNameCache[2184];
			Lang._mapLegendCache[MapHelper.TileToLookup(295, 0)] = itemNameCache[2185];
			Lang._mapLegendCache[MapHelper.TileToLookup(580, 0)] = itemNameCache[4846];
			Lang._mapLegendCache[MapHelper.TileToLookup(620, 0)] = itemNameCache[4964];
			Lang._mapLegendCache[MapHelper.TileToLookup(619, 0)] = itemNameCache[4963];
			Lang._mapLegendCache[MapHelper.TileToLookup(296, 0)] = itemNameCache[2186];
			Lang._mapLegendCache[MapHelper.TileToLookup(297, 0)] = itemNameCache[2187];
			Lang._mapLegendCache[MapHelper.TileToLookup(298, 0)] = itemNameCache[2190];
			Lang._mapLegendCache[MapHelper.TileToLookup(299, 0)] = itemNameCache[2191];
			Lang._mapLegendCache[MapHelper.TileToLookup(300, 0)] = itemNameCache[2192];
			Lang._mapLegendCache[MapHelper.TileToLookup(301, 0)] = itemNameCache[2193];
			Lang._mapLegendCache[MapHelper.TileToLookup(302, 0)] = itemNameCache[2194];
			Lang._mapLegendCache[MapHelper.TileToLookup(303, 0)] = itemNameCache[2195];
			Lang._mapLegendCache[MapHelper.TileToLookup(304, 0)] = itemNameCache[2196];
			Lang._mapLegendCache[MapHelper.TileToLookup(305, 0)] = itemNameCache[2197];
			Lang._mapLegendCache[MapHelper.TileToLookup(306, 0)] = itemNameCache[2198];
			Lang._mapLegendCache[MapHelper.TileToLookup(307, 0)] = itemNameCache[2203];
			Lang._mapLegendCache[MapHelper.TileToLookup(308, 0)] = itemNameCache[2204];
			Lang._mapLegendCache[MapHelper.TileToLookup(309, 0)] = itemNameCache[2206];
			Lang._mapLegendCache[MapHelper.TileToLookup(310, 0)] = itemNameCache[2207];
			Lang._mapLegendCache[MapHelper.TileToLookup(391, 0)] = itemNameCache[3254];
			Lang._mapLegendCache[MapHelper.TileToLookup(316, 0)] = itemNameCache[2439];
			Lang._mapLegendCache[MapHelper.TileToLookup(317, 0)] = itemNameCache[2440];
			Lang._mapLegendCache[MapHelper.TileToLookup(318, 0)] = itemNameCache[2441];
			Lang._mapLegendCache[MapHelper.TileToLookup(319, 0)] = itemNameCache[2490];
			Lang._mapLegendCache[MapHelper.TileToLookup(320, 0)] = itemNameCache[2496];
			Lang._mapLegendCache[MapHelper.TileToLookup(323, 0)] = Language.GetText("MapObject.PalmTree");
			Lang._mapLegendCache[MapHelper.TileToLookup(314, 0)] = itemNameCache[2340];
			Lang._mapLegendCache[MapHelper.TileToLookup(353, 0)] = itemNameCache[2996];
			Lang._mapLegendCache[MapHelper.TileToLookup(354, 0)] = itemNameCache[2999];
			Lang._mapLegendCache[MapHelper.TileToLookup(355, 0)] = itemNameCache[3000];
			Lang._mapLegendCache[MapHelper.TileToLookup(464, 0)] = itemNameCache[3814];
			Lang._mapLegendCache[MapHelper.TileToLookup(699, 0)] = itemNameCache[5482];
			Lang._mapLegendCache[MapHelper.TileToLookup(356, 0)] = itemNameCache[3064];
			Lang._mapLegendCache[MapHelper.TileToLookup(663, 0)] = itemNameCache[5381];
			Lang._mapLegendCache[MapHelper.TileToLookup(567, 0)] = itemNameCache[4609];
			Lang._mapLegendCache[MapHelper.TileToLookup(358, 0)] = itemNameCache[3070];
			Lang._mapLegendCache[MapHelper.TileToLookup(359, 0)] = itemNameCache[3071];
			Lang._mapLegendCache[MapHelper.TileToLookup(360, 0)] = itemNameCache[3072];
			Lang._mapLegendCache[MapHelper.TileToLookup(361, 0)] = itemNameCache[3073];
			Lang._mapLegendCache[MapHelper.TileToLookup(362, 0)] = itemNameCache[3074];
			Lang._mapLegendCache[MapHelper.TileToLookup(363, 0)] = itemNameCache[3075];
			Lang._mapLegendCache[MapHelper.TileToLookup(364, 0)] = itemNameCache[3076];
			Lang._mapLegendCache[MapHelper.TileToLookup(414, 0)] = itemNameCache[3566];
			Lang._mapLegendCache[MapHelper.TileToLookup(521, 0)] = itemNameCache[4327];
			Lang._mapLegendCache[MapHelper.TileToLookup(522, 0)] = itemNameCache[4328];
			Lang._mapLegendCache[MapHelper.TileToLookup(523, 0)] = itemNameCache[4329];
			Lang._mapLegendCache[MapHelper.TileToLookup(524, 0)] = itemNameCache[4330];
			Lang._mapLegendCache[MapHelper.TileToLookup(525, 0)] = itemNameCache[4331];
			Lang._mapLegendCache[MapHelper.TileToLookup(526, 0)] = itemNameCache[4332];
			Lang._mapLegendCache[MapHelper.TileToLookup(527, 0)] = itemNameCache[4333];
			Lang._mapLegendCache[MapHelper.TileToLookup(542, 0)] = itemNameCache[4396];
			Lang._mapLegendCache[MapHelper.TileToLookup(365, 0)] = itemNameCache[3077];
			Lang._mapLegendCache[MapHelper.TileToLookup(366, 0)] = itemNameCache[3078];
			Lang._mapLegendCache[MapHelper.TileToLookup(373, 0)] = Language.GetText("MapObject.DrippingWater");
			Lang._mapLegendCache[MapHelper.TileToLookup(374, 0)] = Language.GetText("MapObject.DrippingLava");
			Lang._mapLegendCache[MapHelper.TileToLookup(375, 0)] = Language.GetText("MapObject.DrippingHoney");
			Lang._mapLegendCache[MapHelper.TileToLookup(709, 0)] = Language.GetText("MapObject.DrippingShimmer");
			Lang._mapLegendCache[MapHelper.TileToLookup(461, 0)] = Language.GetText("MapObject.SandFlow");
			Lang._mapLegendCache[MapHelper.TileToLookup(461, 1)] = Language.GetText("MapObject.SandFlow");
			Lang._mapLegendCache[MapHelper.TileToLookup(461, 2)] = Language.GetText("MapObject.SandFlow");
			Lang._mapLegendCache[MapHelper.TileToLookup(461, 3)] = Language.GetText("MapObject.SandFlow");
			Lang._mapLegendCache[MapHelper.TileToLookup(377, 0)] = itemNameCache[3198];
			Lang._mapLegendCache[MapHelper.TileToLookup(372, 0)] = itemNameCache[3117];
			Lang._mapLegendCache[MapHelper.TileToLookup(646, 0)] = itemNameCache[5322];
			Lang._mapLegendCache[MapHelper.TileToLookup(425, 0)] = itemNameCache[3617];
			Lang._mapLegendCache[MapHelper.TileToLookup(420, 0)] = itemNameCache[3603];
			Lang._mapLegendCache[MapHelper.TileToLookup(420, 1)] = itemNameCache[3604];
			Lang._mapLegendCache[MapHelper.TileToLookup(420, 2)] = itemNameCache[3605];
			Lang._mapLegendCache[MapHelper.TileToLookup(420, 3)] = itemNameCache[3606];
			Lang._mapLegendCache[MapHelper.TileToLookup(420, 4)] = itemNameCache[3607];
			Lang._mapLegendCache[MapHelper.TileToLookup(420, 5)] = itemNameCache[3608];
			Lang._mapLegendCache[MapHelper.TileToLookup(423, 0)] = itemNameCache[3613];
			Lang._mapLegendCache[MapHelper.TileToLookup(423, 1)] = itemNameCache[3614];
			Lang._mapLegendCache[MapHelper.TileToLookup(423, 2)] = itemNameCache[3615];
			Lang._mapLegendCache[MapHelper.TileToLookup(423, 3)] = itemNameCache[3726];
			Lang._mapLegendCache[MapHelper.TileToLookup(423, 4)] = itemNameCache[3727];
			Lang._mapLegendCache[MapHelper.TileToLookup(423, 5)] = itemNameCache[3728];
			Lang._mapLegendCache[MapHelper.TileToLookup(423, 6)] = itemNameCache[3729];
			Lang._mapLegendCache[MapHelper.TileToLookup(440, 0)] = itemNameCache[3644];
			Lang._mapLegendCache[MapHelper.TileToLookup(440, 1)] = itemNameCache[3645];
			Lang._mapLegendCache[MapHelper.TileToLookup(440, 2)] = itemNameCache[3646];
			Lang._mapLegendCache[MapHelper.TileToLookup(440, 3)] = itemNameCache[3647];
			Lang._mapLegendCache[MapHelper.TileToLookup(440, 4)] = itemNameCache[3648];
			Lang._mapLegendCache[MapHelper.TileToLookup(440, 5)] = itemNameCache[3649];
			Lang._mapLegendCache[MapHelper.TileToLookup(440, 6)] = itemNameCache[3650];
			Lang._mapLegendCache[MapHelper.TileToLookup(443, 0)] = itemNameCache[3722];
			Lang._mapLegendCache[MapHelper.TileToLookup(429, 0)] = itemNameCache[3629];
			Lang._mapLegendCache[MapHelper.TileToLookup(424, 0)] = itemNameCache[3616];
			Lang._mapLegendCache[MapHelper.TileToLookup(444, 0)] = Language.GetText("MapObject.BeeHive");
			Lang._mapLegendCache[MapHelper.TileToLookup(466, 0)] = itemNameCache[3816];
			Lang._mapLegendCache[MapHelper.TileToLookup(463, 0)] = itemNameCache[3813];
			Lang._mapLegendCache[MapHelper.TileToLookup(491, 0)] = itemNameCache[4076];
			Lang._mapLegendCache[MapHelper.TileToLookup(494, 0)] = itemNameCache[4089];
			Lang._mapLegendCache[MapHelper.TileToLookup(499, 0)] = itemNameCache[4142];
			Lang._mapLegendCache[MapHelper.TileToLookup(488, 0)] = Language.GetText("MapObject.FallenLog");
			Lang._mapLegendCache[MapHelper.TileToLookup(704, 0)] = Language.GetText("MapObject.FallenLog");
			Lang._mapLegendCache[MapHelper.TileToLookup(505, 0)] = itemNameCache[4275];
			Lang._mapLegendCache[MapHelper.TileToLookup(521, 0)] = itemNameCache[4327];
			Lang._mapLegendCache[MapHelper.TileToLookup(522, 0)] = itemNameCache[4328];
			Lang._mapLegendCache[MapHelper.TileToLookup(523, 0)] = itemNameCache[4329];
			Lang._mapLegendCache[MapHelper.TileToLookup(524, 0)] = itemNameCache[4330];
			Lang._mapLegendCache[MapHelper.TileToLookup(525, 0)] = itemNameCache[4331];
			Lang._mapLegendCache[MapHelper.TileToLookup(526, 0)] = itemNameCache[4332];
			Lang._mapLegendCache[MapHelper.TileToLookup(527, 0)] = itemNameCache[4333];
			Lang._mapLegendCache[MapHelper.TileToLookup(531, 0)] = Language.GetText("MapObject.Statue");
			Lang._mapLegendCache[MapHelper.TileToLookup(349, 0)] = Language.GetText("MapObject.Statue");
			Lang._mapLegendCache[MapHelper.TileToLookup(532, 0)] = itemNameCache[4364];
			Lang._mapLegendCache[MapHelper.TileToLookup(538, 0)] = itemNameCache[4380];
			Lang._mapLegendCache[MapHelper.TileToLookup(544, 0)] = itemNameCache[4399];
			Lang._mapLegendCache[MapHelper.TileToLookup(629, 0)] = itemNameCache[5133];
			Lang._mapLegendCache[MapHelper.TileToLookup(506, 0)] = itemNameCache[4276];
			Lang._mapLegendCache[MapHelper.TileToLookup(545, 0)] = itemNameCache[4420];
			Lang._mapLegendCache[MapHelper.TileToLookup(550, 0)] = itemNameCache[4461];
			Lang._mapLegendCache[MapHelper.TileToLookup(551, 0)] = itemNameCache[4462];
			Lang._mapLegendCache[MapHelper.TileToLookup(533, 0)] = itemNameCache[4376];
			Lang._mapLegendCache[MapHelper.TileToLookup(553, 0)] = itemNameCache[4473];
			Lang._mapLegendCache[MapHelper.TileToLookup(554, 0)] = itemNameCache[4474];
			Lang._mapLegendCache[MapHelper.TileToLookup(555, 0)] = itemNameCache[4475];
			Lang._mapLegendCache[MapHelper.TileToLookup(556, 0)] = itemNameCache[4476];
			Lang._mapLegendCache[MapHelper.TileToLookup(558, 0)] = itemNameCache[4481];
			Lang._mapLegendCache[MapHelper.TileToLookup(559, 0)] = itemNameCache[4483];
			Lang._mapLegendCache[MapHelper.TileToLookup(599, 0)] = itemNameCache[4882];
			Lang._mapLegendCache[MapHelper.TileToLookup(600, 0)] = itemNameCache[4883];
			Lang._mapLegendCache[MapHelper.TileToLookup(601, 0)] = itemNameCache[4884];
			Lang._mapLegendCache[MapHelper.TileToLookup(602, 0)] = itemNameCache[4885];
			Lang._mapLegendCache[MapHelper.TileToLookup(603, 0)] = itemNameCache[4886];
			Lang._mapLegendCache[MapHelper.TileToLookup(604, 0)] = itemNameCache[4887];
			Lang._mapLegendCache[MapHelper.TileToLookup(605, 0)] = itemNameCache[4888];
			Lang._mapLegendCache[MapHelper.TileToLookup(606, 0)] = itemNameCache[4889];
			Lang._mapLegendCache[MapHelper.TileToLookup(607, 0)] = itemNameCache[4890];
			Lang._mapLegendCache[MapHelper.TileToLookup(608, 0)] = itemNameCache[4891];
			Lang._mapLegendCache[MapHelper.TileToLookup(609, 0)] = itemNameCache[4892];
			Lang._mapLegendCache[MapHelper.TileToLookup(610, 0)] = itemNameCache[4893];
			Lang._mapLegendCache[MapHelper.TileToLookup(611, 0)] = itemNameCache[4894];
			Lang._mapLegendCache[MapHelper.TileToLookup(612, 0)] = itemNameCache[4895];
			Lang._mapLegendCache[MapHelper.TileToLookup(560, 0)] = itemNameCache[4599];
			Lang._mapLegendCache[MapHelper.TileToLookup(560, 1)] = itemNameCache[4600];
			Lang._mapLegendCache[MapHelper.TileToLookup(560, 2)] = itemNameCache[4601];
			Lang._mapLegendCache[MapHelper.TileToLookup(568, 0)] = itemNameCache[4655];
			Lang._mapLegendCache[MapHelper.TileToLookup(569, 0)] = itemNameCache[4656];
			Lang._mapLegendCache[MapHelper.TileToLookup(570, 0)] = itemNameCache[4657];
			Lang._mapLegendCache[MapHelper.TileToLookup(572, 0)] = itemNameCache[4695];
			Lang._mapLegendCache[MapHelper.TileToLookup(572, 1)] = itemNameCache[4696];
			Lang._mapLegendCache[MapHelper.TileToLookup(572, 2)] = itemNameCache[4697];
			Lang._mapLegendCache[MapHelper.TileToLookup(572, 3)] = itemNameCache[4698];
			Lang._mapLegendCache[MapHelper.TileToLookup(572, 4)] = itemNameCache[4699];
			Lang._mapLegendCache[MapHelper.TileToLookup(572, 5)] = itemNameCache[4700];
			Lang._mapLegendCache[MapHelper.TileToLookup(497, 0)] = Language.GetText("MapObject.Toilet");
			Lang._mapLegendCache[MapHelper.TileToLookup(751, 0)] = itemNameCache[5667];
			Lang._mapLegendCache[MapHelper.TileToLookup(752, 0)] = itemNameCache[6142];
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0001E418 File Offset: 0x0001C618
		public static NetworkText CreateDeathMessage(string deadPlayerName, int plr = -1, int npc = -1, int proj = -1, int other = -1, int projType = 0, int plrItemType = 0)
		{
			NetworkText networkText = NetworkText.Empty;
			NetworkText networkText2 = NetworkText.Empty;
			NetworkText networkText3 = NetworkText.Empty;
			NetworkText networkText4 = NetworkText.Empty;
			if (proj >= 0)
			{
				networkText = NetworkText.FromKey(Lang.GetProjectileName(projType).Key, new object[0]);
			}
			if (npc >= 0)
			{
				networkText2 = Main.npc[npc].GetGivenOrTypeNetName();
			}
			if (plr >= 0 && plr < 255)
			{
				networkText3 = NetworkText.FromLiteral(Main.player[plr].name);
			}
			if (plrItemType >= 0)
			{
				networkText4 = NetworkText.FromKey(Lang.GetItemName(plrItemType).Key, new object[0]);
			}
			bool flag = networkText != NetworkText.Empty;
			bool flag2 = plr >= 0 && plr < 255;
			bool flag3 = networkText2 != NetworkText.Empty;
			NetworkText result = NetworkText.Empty;
			NetworkText networkText5 = NetworkText.Empty;
			networkText5 = NetworkText.FromKey(Language.RandomFromCategory("DeathTextGeneric", null).Key, new object[]
			{
				deadPlayerName,
				Main.worldName
			});
			if (flag2)
			{
				result = NetworkText.FromKey("DeathSource.Player", new object[]
				{
					networkText5,
					networkText3,
					flag ? networkText : networkText4
				});
			}
			else if (flag3)
			{
				result = NetworkText.FromKey("DeathSource.NPC", new object[]
				{
					networkText5,
					networkText2
				});
			}
			else if (flag)
			{
				result = NetworkText.FromKey("DeathSource.Projectile", new object[]
				{
					networkText5,
					networkText
				});
			}
			else if (other >= 0)
			{
				if (other == 0)
				{
					result = NetworkText.FromKey("DeathText.Fell_" + (Main.rand.Next(9) + 1), new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 1)
				{
					result = NetworkText.FromKey("DeathText.Drowned_" + (Main.rand.Next(7) + 1), new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 2)
				{
					result = NetworkText.FromKey("DeathText.Lava_" + (Main.rand.Next(5) + 1), new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 3)
				{
					result = NetworkText.FromKey("DeathText.Default", new object[]
					{
						networkText5
					});
				}
				else if (other == 4)
				{
					result = NetworkText.FromKey("DeathText.Slain", new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 5)
				{
					result = NetworkText.FromKey("DeathText.Petrified_" + (Main.rand.Next(4) + 1), new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 6)
				{
					result = NetworkText.FromKey("DeathText.Stabbed", new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 7)
				{
					result = NetworkText.FromKey("DeathText.Suffocated_" + (Main.rand.Next(2) + 1), new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 8)
				{
					result = NetworkText.FromKey("DeathText.Burned_" + (Main.rand.Next(4) + 1), new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 9)
				{
					result = NetworkText.FromKey("DeathText.Poisoned", new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 10)
				{
					result = NetworkText.FromKey("DeathText.Electrocuted_" + (Main.rand.Next(4) + 1), new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 11)
				{
					result = NetworkText.FromKey("DeathText.TriedToEscape", new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 12)
				{
					result = NetworkText.FromKey("DeathText.WasLicked_" + (Main.rand.Next(2) + 1), new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 13)
				{
					result = NetworkText.FromKey("DeathText.Teleport_1", new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 14)
				{
					result = NetworkText.FromKey("DeathText.Teleport_2_Male", new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 15)
				{
					result = NetworkText.FromKey("DeathText.Teleport_2_Female", new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 16)
				{
					result = NetworkText.FromKey("DeathText.Inferno", new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 17)
				{
					result = NetworkText.FromKey("DeathText.DiedInTheDark", new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 18)
				{
					result = NetworkText.FromKey("DeathText.Starved_" + (Main.rand.Next(3) + 1), new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 19)
				{
					result = NetworkText.FromKey("DeathText.Space_" + (Main.rand.Next(5) + 1), new object[]
					{
						deadPlayerName,
						Main.worldName
					});
				}
				else if (other == 20)
				{
					result = NetworkText.FromKey("DeathText.TeamTank", new object[]
					{
						deadPlayerName
					});
				}
				else if (other == 21)
				{
					result = NetworkText.FromKey("DeathText.Underground_" + (Main.rand.Next(5) + 1), new object[]
					{
						deadPlayerName,
						Main.worldName
					});
				}
				else if (other == 22)
				{
					result = NetworkText.FromKey("DeathText.VampireBurningInDaylight_" + (Main.rand.Next(6) + 1), new object[]
					{
						deadPlayerName,
						Main.worldName
					});
				}
				else if (other == 255)
				{
					result = NetworkText.FromKey("DeathText.Slain", new object[]
					{
						deadPlayerName
					});
				}
			}
			return result;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0001E9BC File Offset: 0x0001CBBC
		public static NetworkText GetInvasionWaveText(int wave, params short[] npcIds)
		{
			NetworkText[] array = new NetworkText[npcIds.Length + 1];
			for (int i = 0; i < npcIds.Length; i++)
			{
				array[i + 1] = NetworkText.FromKey(Lang.GetNPCName((int)npcIds[i]).Key, new object[0]);
			}
			if (wave == -1)
			{
				array[0] = NetworkText.FromKey("Game.FinalWave", new object[0]);
			}
			else if (wave == 1)
			{
				array[0] = NetworkText.FromKey("Game.FirstWave", new object[0]);
			}
			else
			{
				array[0] = NetworkText.FromKey("Game.Wave", new object[]
				{
					wave
				});
			}
			string key = "Game.InvasionWave_Type" + npcIds.Length;
			object[] substitutions = array;
			return NetworkText.FromKey(key, substitutions);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0001EA68 File Offset: 0x0001CC68
		public static string LocalizedDuration(TimeSpan time, bool abbreviated, bool showAllAvailableUnits)
		{
			string text = "";
			abbreviated |= !GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive;
			if (time.Days > 0)
			{
				int num = time.Days;
				if (!showAllAvailableUnits && time > TimeSpan.FromDays(1.0))
				{
					num++;
				}
				text = text + num + (abbreviated ? (" " + Language.GetTextValue("Misc.ShortDays")) : ((num == 1) ? " day" : " days"));
				if (!showAllAvailableUnits)
				{
					return text;
				}
				text += " ";
			}
			if (time.Hours > 0)
			{
				int num2 = time.Hours;
				if (!showAllAvailableUnits && time > TimeSpan.FromHours(1.0))
				{
					num2++;
				}
				text = text + num2 + (abbreviated ? (" " + Language.GetTextValue("Misc.ShortHours")) : ((num2 == 1) ? " hour" : " hours"));
				if (!showAllAvailableUnits)
				{
					return text;
				}
				text += " ";
			}
			if (time.Minutes > 0)
			{
				int num3 = time.Minutes;
				if (!showAllAvailableUnits && time > TimeSpan.FromMinutes(1.0))
				{
					num3++;
				}
				text = text + num3 + (abbreviated ? (" " + Language.GetTextValue("Misc.ShortMinutes")) : ((num3 == 1) ? " minute" : " minutes"));
				if (!showAllAvailableUnits)
				{
					return text;
				}
				text += " ";
			}
			return text + time.Seconds + (abbreviated ? (" " + Language.GetTextValue("Misc.ShortSeconds")) : ((time.Seconds == 1) ? " second" : " seconds"));
		}

		// Token: 0x0400015A RID: 346
		[Old("Lang arrays have been replaced with the new Language.GetText system.")]
		public static LocalizedText[] menu = new LocalizedText[254];

		// Token: 0x0400015B RID: 347
		[Old("Lang arrays have been replaced with the new Language.GetText system.")]
		public static LocalizedText[] gen = new LocalizedText[94];

		// Token: 0x0400015C RID: 348
		[Old("Lang arrays have been replaced with the new Language.GetText system.")]
		public static LocalizedText[] misc = new LocalizedText[201];

		// Token: 0x0400015D RID: 349
		[Old("Lang arrays have been replaced with the new Language.GetText system.")]
		public static LocalizedText[] inter = new LocalizedText[129];

		// Token: 0x0400015E RID: 350
		[Old("Lang arrays have been replaced with the new Language.GetText system.")]
		public static LocalizedText[] tip = new LocalizedText[62];

		// Token: 0x0400015F RID: 351
		[Old("Lang arrays have been replaced with the new Language.GetText system.")]
		public static LocalizedText[] mp = new LocalizedText[27];

		// Token: 0x04000160 RID: 352
		[Old("Lang arrays have been replaced with the new Language.GetText system.")]
		public static LocalizedText[] chestType = new LocalizedText[52];

		// Token: 0x04000161 RID: 353
		[Old("Lang arrays have been replaced with the new Language.GetText system.")]
		public static LocalizedText[] dresserType = new LocalizedText[65];

		// Token: 0x04000162 RID: 354
		[Old("Lang arrays have been replaced with the new Language.GetText system.")]
		public static LocalizedText[] chestType2 = new LocalizedText[38];

		// Token: 0x04000163 RID: 355
		public static LocalizedText[] prefix = new LocalizedText[PrefixID.Count];

		// Token: 0x04000164 RID: 356
		public static LocalizedText[] _mapLegendCache;

		// Token: 0x04000165 RID: 357
		private static LocalizedText[] _itemNameCache = new LocalizedText[(int)ItemID.Count];

		// Token: 0x04000166 RID: 358
		private static LocalizedText[] _projectileNameCache = new LocalizedText[(int)ProjectileID.Count];

		// Token: 0x04000167 RID: 359
		private static LocalizedText[] _npcNameCache = new LocalizedText[(int)NPCID.Count];

		// Token: 0x04000168 RID: 360
		private static LocalizedText[] _negativeNpcNameCache = new LocalizedText[65];

		// Token: 0x04000169 RID: 361
		private static LocalizedText[] _buffNameCache = new LocalizedText[BuffID.Count];

		// Token: 0x0400016A RID: 362
		private static LocalizedText[] _buffDescriptionCache = new LocalizedText[BuffID.Count];

		// Token: 0x0400016B RID: 363
		private static ItemTooltip[] _itemTooltipCache = new ItemTooltip[(int)ItemID.Count];

		// Token: 0x0400016C RID: 364
		private static LocalizedText[] _emojiNameCache = new LocalizedText[EmoteID.Count];

		// Token: 0x0400016D RID: 365
		private static Dictionary<string, Func<object>> _globalSubstitutions = new Dictionary<string, Func<object>>();

		// Token: 0x0400016E RID: 366
		private static LocalizedText _prefixFormatText = Language.GetText("CombineFormat.Prefix");

		// Token: 0x020005F2 RID: 1522
		private class ItemPrefixCombiner
		{
			// Token: 0x170004C1 RID: 1217
			// (get) Token: 0x06003B48 RID: 15176 RVA: 0x00659753 File Offset: 0x00657953
			// (set) Token: 0x06003B49 RID: 15177 RVA: 0x0065975B File Offset: 0x0065795B
			public string ItemName { get; set; }

			// Token: 0x170004C2 RID: 1218
			// (get) Token: 0x06003B4A RID: 15178 RVA: 0x00659764 File Offset: 0x00657964
			// (set) Token: 0x06003B4B RID: 15179 RVA: 0x0065976C File Offset: 0x0065796C
			public string PrefixName { get; set; }
		}
	}
}
