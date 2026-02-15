using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent
{
	// Token: 0x02000240 RID: 576
	public class NPCInteractions
	{
		// Token: 0x06002293 RID: 8851 RVA: 0x0053884C File Offset: 0x00536A4C
		public static void Initialize()
		{
			NPCInteractions.Shop(17, 1, null);
			NPCInteractions.Shop(19, 2, null);
			NPCInteractions.Shop(20, 3, null);
			NPCInteractions.Shop(38, 4, null);
			NPCInteractions.Shop(54, 5, null);
			NPCInteractions.Shop(107, 6, null);
			NPCInteractions.Shop(108, 7, null);
			NPCInteractions.Shop(124, 8, null);
			NPCInteractions.Shop(142, 9, null);
			NPCInteractions.Shop(160, 10, null);
			NPCInteractions.Shop(178, 11, null);
			NPCInteractions.Shop(207, 12, null);
			NPCInteractions.Shop(208, 13, null);
			NPCInteractions.Shop(209, 14, null);
			NPCInteractions.Shop(227, 15, null);
			NPCInteractions.Shop(228, 16, null);
			NPCInteractions.Shop(229, 17, null);
			NPCInteractions.Shop(353, 18, null);
			NPCInteractions.Shop(368, 19, null);
			NPCInteractions.Shop(453, 20, null);
			NPCInteractions.Shop(550, 21, null);
			NPCInteractions.Shop(588, 22, null);
			NPCInteractions.Shop(633, 23, null);
			NPCInteractions.Shop(663, 24, null);
			NPCInteractions.Shop(227, 25, "GameUI.PainterDecor");
			NPCInteractions.Register(new NPCInteractions.Actions.TaxCollectorCollectTaxes());
			NPCInteractions.Register(new NPCInteractions.Actions.NurseHeal());
			NPCInteractions.Register(new NPCInteractions.Actions.CloseChat());
			NPCInteractions.Register(new NPCInteractions.Actions.OpenSign());
			NPCInteractions.Register(new NPCInteractions.Actions.StardewValleyBit());
			NPCInteractions.Register(new NPCInteractions.Actions.DryadPurification());
			NPCInteractions.Register(new NPCInteractions.Actions.AnglerQuest());
			NPCInteractions.Register(new NPCInteractions.Actions.PetAnimal());
			NPCInteractions.Register(new NPCInteractions.Actions.OldManCurse());
			NPCInteractions.Register(new NPCInteractions.Actions.GuideTip());
			NPCInteractions.Register(new NPCInteractions.Actions.PartyGirlMusicSwap());
			NPCInteractions.Register(new NPCInteractions.Actions.GuideReverseCrafting());
			NPCInteractions.Register(new NPCInteractions.Actions.TinkererReforge());
			NPCInteractions.Register(new NPCInteractions.Actions.StylistHairWindow());
			NPCInteractions.Register(new NPCInteractions.Actions.DyeTraderRarePlant());
			NPCInteractions.Register(new NPCInteractions.Actions.TavernkeepAdvice());
			NPCInteractions.Register(new NPCInteractions.Actions.ReportHappiness());
			NPCInteractions.Register(new NPCInteractions.Actions.RequestHome());
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x00538A36 File Offset: 0x00536C36
		private static void Shop(int npcType, int shopIndex, string customTextKey = null)
		{
			NPCInteractions.Register(new NPCInteractions.Actions.OpenShop(npcType, shopIndex, customTextKey));
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x00538A45 File Offset: 0x00536C45
		private static void Register(NPCInteraction interaction)
		{
			NPCInteractions.All.Add(interaction);
		}

		// Token: 0x04004CE9 RID: 19689
		public static List<NPCInteraction> All = new List<NPCInteraction>();

		// Token: 0x020007C9 RID: 1993
		public static class Actions
		{
			// Token: 0x02000AB5 RID: 2741
			public class OpenSign : NPCInteraction
			{
				// Token: 0x06004C16 RID: 19478 RVA: 0x006D90DA File Offset: 0x006D72DA
				public override bool Condition()
				{
					return base.LocalPlayer.sign > -1;
				}

				// Token: 0x06004C17 RID: 19479 RVA: 0x006D90EA File Offset: 0x006D72EA
				public override string GetText()
				{
					if (Main.editSign)
					{
						return Lang.inter[47].Value;
					}
					return Lang.inter[48].Value;
				}

				// Token: 0x06004C18 RID: 19480 RVA: 0x006D910E File Offset: 0x006D730E
				public override void Interact()
				{
					if (Main.editSign)
					{
						Main.SubmitSignText();
						return;
					}
					IngameFancyUI.OpenVirtualKeyboard(1);
				}
			}

			// Token: 0x02000AB6 RID: 2742
			public class OpenShop : NPCInteraction
			{
				// Token: 0x06004C1A RID: 19482 RVA: 0x006D912B File Offset: 0x006D732B
				public OpenShop(int npcType, int shopIndex, string customTextKey = null)
				{
					this._npcType = npcType;
					this._shopIndex = shopIndex;
					this._customTextKey = customTextKey;
				}

				// Token: 0x06004C1B RID: 19483 RVA: 0x006D9148 File Offset: 0x006D7348
				public override bool Condition()
				{
					return base.TalkNPCType == this._npcType;
				}

				// Token: 0x06004C1C RID: 19484 RVA: 0x006D9158 File Offset: 0x006D7358
				public override string GetText()
				{
					if (this._customTextKey != null)
					{
						return Language.GetTextValue(this._customTextKey);
					}
					return Lang.inter[28].Value;
				}

				// Token: 0x06004C1D RID: 19485 RVA: 0x006D917B File Offset: 0x006D737B
				public override void Interact()
				{
					Main.instance.OpenShop(this._shopIndex);
				}

				// Token: 0x0400783A RID: 30778
				private int _shopIndex;

				// Token: 0x0400783B RID: 30779
				private int _npcType;

				// Token: 0x0400783C RID: 30780
				private string _customTextKey;
			}

			// Token: 0x02000AB7 RID: 2743
			public class StardewValleyBit : NPCInteraction
			{
				// Token: 0x170005C1 RID: 1473
				// (get) Token: 0x06004C1E RID: 19486 RVA: 0x000379F1 File Offset: 0x00035BF1
				public override bool ShowExcalmation
				{
					get
					{
						return true;
					}
				}

				// Token: 0x06004C1F RID: 19487 RVA: 0x006D918D File Offset: 0x006D738D
				public override bool Condition()
				{
					return base.TalkNPCType == 20 && Main.CanDryadPlayStardewAnimation(base.LocalPlayer, base.TalkNPC);
				}

				// Token: 0x06004C20 RID: 19488 RVA: 0x006D91AC File Offset: 0x006D73AC
				public override string GetText()
				{
					return Language.GetTextValue("StardewTalk.GiveColaButtonText");
				}

				// Token: 0x06004C21 RID: 19489 RVA: 0x006D91B8 File Offset: 0x006D73B8
				public override void Interact()
				{
					Main.DoNPCPortraitHop();
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					Main.DryadText_Do_StardewValleyBit();
				}
			}

			// Token: 0x02000AB8 RID: 2744
			public class DryadPurification : NPCInteraction
			{
				// Token: 0x06004C23 RID: 19491 RVA: 0x006D91D9 File Offset: 0x006D73D9
				public override bool Condition()
				{
					return base.TalkNPCType == 20 && !Main.CanDryadPlayStardewAnimation(base.LocalPlayer, base.TalkNPC);
				}

				// Token: 0x06004C24 RID: 19492 RVA: 0x006D91FB File Offset: 0x006D73FB
				public override string GetText()
				{
					return Lang.inter[49].Value;
				}

				// Token: 0x06004C25 RID: 19493 RVA: 0x006D920C File Offset: 0x006D740C
				public override void Interact()
				{
					Main.DoNPCPortraitHop();
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					bool flag;
					Main.npcChatText = Lang.GetDryadWorldStatusDialog(out flag);
					if (flag)
					{
						AchievementsHelper.HandleSpecialEvent(base.LocalPlayer, 27);
					}
				}
			}

			// Token: 0x02000AB9 RID: 2745
			public class AnglerQuest : NPCInteraction
			{
				// Token: 0x170005C2 RID: 1474
				// (get) Token: 0x06004C27 RID: 19495 RVA: 0x000379F1 File Offset: 0x00035BF1
				public override bool ShowExcalmation
				{
					get
					{
						return true;
					}
				}

				// Token: 0x06004C28 RID: 19496 RVA: 0x006D924F File Offset: 0x006D744F
				public override bool Condition()
				{
					return base.TalkNPCType == 369;
				}

				// Token: 0x06004C29 RID: 19497 RVA: 0x006D925E File Offset: 0x006D745E
				public override string GetText()
				{
					return Lang.inter[64].Value;
				}

				// Token: 0x06004C2A RID: 19498 RVA: 0x006D926D File Offset: 0x006D746D
				public override void Interact()
				{
					Main.NPCChatText_DoAnglerQuest();
				}
			}

			// Token: 0x02000ABA RID: 2746
			public class PetAnimal : NPCInteraction
			{
				// Token: 0x06004C2C RID: 19500 RVA: 0x006D9274 File Offset: 0x006D7474
				public override bool Condition()
				{
					return NPCID.Sets.IsTownPet[base.TalkNPCType];
				}

				// Token: 0x06004C2D RID: 19501 RVA: 0x006D9282 File Offset: 0x006D7482
				public override string GetText()
				{
					return Language.GetTextValue("UI.PetTheAnimal");
				}

				// Token: 0x06004C2E RID: 19502 RVA: 0x006D928E File Offset: 0x006D748E
				public override void Interact()
				{
					base.LocalPlayer.PetAnimal(Main.npc[base.LocalPlayer.talkNPC].GetPettingInfo(base.LocalPlayer));
				}
			}

			// Token: 0x02000ABB RID: 2747
			public class OldManCurse : NPCInteraction
			{
				// Token: 0x06004C30 RID: 19504 RVA: 0x006D92B7 File Offset: 0x006D74B7
				public override bool Condition()
				{
					return base.TalkNPCType == 37 && !Main.IsItDay();
				}

				// Token: 0x06004C31 RID: 19505 RVA: 0x006D92CD File Offset: 0x006D74CD
				public override string GetText()
				{
					return Lang.inter[50].Value;
				}

				// Token: 0x06004C32 RID: 19506 RVA: 0x006D92DC File Offset: 0x006D74DC
				public override void Interact()
				{
					if (Main.netMode == 0)
					{
						NPC.SpawnSkeletron(Main.myPlayer, false);
					}
					else
					{
						NetMessage.SendData(51, -1, -1, null, Main.myPlayer, 1f, 0f, 0f, 0, 0, 0);
					}
					Main.npcChatText = "";
				}
			}

			// Token: 0x02000ABC RID: 2748
			public class GuideTip : NPCInteraction
			{
				// Token: 0x06004C34 RID: 19508 RVA: 0x006D9328 File Offset: 0x006D7528
				public override bool Condition()
				{
					return base.TalkNPCType == 22;
				}

				// Token: 0x06004C35 RID: 19509 RVA: 0x006D9334 File Offset: 0x006D7534
				public override string GetText()
				{
					return Lang.inter[51].Value;
				}

				// Token: 0x06004C36 RID: 19510 RVA: 0x006D9343 File Offset: 0x006D7543
				public override void Interact()
				{
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					Main.HelpText();
					Main.DoNPCPortraitHop();
				}
			}

			// Token: 0x02000ABD RID: 2749
			public class TaxCollectorCollectTaxes : NPCInteraction
			{
				// Token: 0x06004C38 RID: 19512 RVA: 0x006D9364 File Offset: 0x006D7564
				public override bool Condition()
				{
					return base.TalkNPCType == 441;
				}

				// Token: 0x06004C39 RID: 19513 RVA: 0x006D9373 File Offset: 0x006D7573
				public override string GetText()
				{
					return Lang.inter[89].Value;
				}

				// Token: 0x06004C3A RID: 19514 RVA: 0x006D9382 File Offset: 0x006D7582
				public override void Interact()
				{
					Main.NPCChatText_DoTaxCollector();
				}

				// Token: 0x06004C3B RID: 19515 RVA: 0x006D9389 File Offset: 0x006D7589
				public override bool TryAddCoins(ref Color chatColor, out int coinValue)
				{
					coinValue = 0;
					Main.GetCoinValueText_TaxCollector(ref chatColor, ref coinValue);
					return coinValue > 0;
				}
			}

			// Token: 0x02000ABE RID: 2750
			public class NurseHeal : NPCInteraction
			{
				// Token: 0x06004C3D RID: 19517 RVA: 0x006D939B File Offset: 0x006D759B
				public override bool Condition()
				{
					return base.TalkNPCType == 18;
				}

				// Token: 0x06004C3E RID: 19518 RVA: 0x006D93A7 File Offset: 0x006D75A7
				public override string GetText()
				{
					return Lang.inter[54].Value;
				}

				// Token: 0x06004C3F RID: 19519 RVA: 0x006D93B6 File Offset: 0x006D75B6
				public override void Interact()
				{
					Main.NPCChatText_DoNurseHeal(Main.GetNurseHealCost());
				}

				// Token: 0x06004C40 RID: 19520 RVA: 0x006D93C2 File Offset: 0x006D75C2
				public override bool TryAddCoins(ref Color chatColor, out int coinValue)
				{
					coinValue = Main.GetNurseHealCost();
					Main.GetCoinValueText_Nurse(ref chatColor, ref coinValue);
					return coinValue > 0;
				}
			}

			// Token: 0x02000ABF RID: 2751
			public class CloseChat : NPCInteraction
			{
				// Token: 0x06004C42 RID: 19522 RVA: 0x000379F1 File Offset: 0x00035BF1
				public override bool Condition()
				{
					return true;
				}

				// Token: 0x06004C43 RID: 19523 RVA: 0x006D93D8 File Offset: 0x006D75D8
				public override string GetText()
				{
					return Lang.inter[52].Value;
				}

				// Token: 0x06004C44 RID: 19524 RVA: 0x006D93E7 File Offset: 0x006D75E7
				public override void Interact()
				{
					Main.CloseNPCChatOrSign(false);
				}
			}

			// Token: 0x02000AC0 RID: 2752
			public class ReportHappiness : NPCInteraction
			{
				// Token: 0x06004C46 RID: 19526 RVA: 0x006D93EF File Offset: 0x006D75EF
				public override bool Condition()
				{
					return !NPC.CanShowHomelessText(Main.LocalPlayer.talkNPC) && base.LocalPlayer.currentShoppingSettings.HappinessReport != "";
				}

				// Token: 0x06004C47 RID: 19527 RVA: 0x006D941E File Offset: 0x006D761E
				public override string GetText()
				{
					return Language.GetTextValue("UI.NPCCheckHappiness");
				}

				// Token: 0x06004C48 RID: 19528 RVA: 0x006D942A File Offset: 0x006D762A
				public override void Interact()
				{
					Main.npcChatCornerItem = 0;
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					Main.npcChatText = base.LocalPlayer.currentShoppingSettings.HappinessReport;
					Main.DoNPCPortraitHop();
				}
			}

			// Token: 0x02000AC1 RID: 2753
			public class RequestHome : NPCInteraction
			{
				// Token: 0x170005C3 RID: 1475
				// (get) Token: 0x06004C4A RID: 19530 RVA: 0x000379F1 File Offset: 0x00035BF1
				public override bool ShowExcalmation
				{
					get
					{
						return true;
					}
				}

				// Token: 0x06004C4B RID: 19531 RVA: 0x006D9461 File Offset: 0x006D7661
				public override bool Condition()
				{
					return NPC.CanShowHomelessText(Main.LocalPlayer.talkNPC);
				}

				// Token: 0x06004C4C RID: 19532 RVA: 0x006D9472 File Offset: 0x006D7672
				public override string GetText()
				{
					return Language.GetTextValue("UI.NPCHousing");
				}

				// Token: 0x06004C4D RID: 19533 RVA: 0x006D9480 File Offset: 0x006D7680
				public override void Interact()
				{
					Main.npcChatCornerItem = -1;
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					Main.DoNPCPortraitHop();
					NPC talkNPC = base.TalkNPC;
					string str = "TownNPCMood_" + NPCID.Search.GetName(talkNPC.netID);
					if (talkNPC.type == 633 && talkNPC.altTexture == 2)
					{
						str += "Transformed";
					}
					if (talkNPC.type == 638)
					{
						str = "DogChatter";
					}
					else if (talkNPC.type == 637)
					{
						str = "CatChatter";
					}
					else if (talkNPC.type == 656)
					{
						str = "BunnyChatter";
					}
					else if (NPCID.Sets.IsTownSlime[talkNPC.type])
					{
						string slimeType = Lang.GetSlimeType(talkNPC);
						str = "Slime" + slimeType + "Chatter";
					}
					Main.npcChatText = Language.GetTextValue(str + ".NoHome");
					Main.npcChatText += "\n\n";
					if (talkNPC.type == 160)
					{
						Main.npcChatText += Language.GetTextValueWith("HousingText.HousingRequirements_Truffle", new
						{
							NPCName = talkNPC.FullName
						});
						return;
					}
					Main.npcChatText += Language.GetTextValue("HousingText.HousingRequirements");
				}
			}

			// Token: 0x02000AC2 RID: 2754
			public class PartyGirlMusicSwap : NPCInteraction
			{
				// Token: 0x06004C4F RID: 19535 RVA: 0x006D95CA File Offset: 0x006D77CA
				public override bool Condition()
				{
					return base.TalkNPCType == 208;
				}

				// Token: 0x06004C50 RID: 19536 RVA: 0x006D95D9 File Offset: 0x006D77D9
				public override string GetText()
				{
					return Language.GetTextValue("GameUI.Music");
				}

				// Token: 0x06004C51 RID: 19537 RVA: 0x006D95E5 File Offset: 0x006D77E5
				public override void Interact()
				{
					Main.NPCChatText_PartyGirlSwapMusic();
				}
			}

			// Token: 0x02000AC3 RID: 2755
			public class GuideReverseCrafting : NPCInteraction
			{
				// Token: 0x06004C53 RID: 19539 RVA: 0x006D9328 File Offset: 0x006D7528
				public override bool Condition()
				{
					return base.TalkNPCType == 22;
				}

				// Token: 0x06004C54 RID: 19540 RVA: 0x006D95EC File Offset: 0x006D77EC
				public override string GetText()
				{
					return Lang.inter[25].Value;
				}

				// Token: 0x06004C55 RID: 19541 RVA: 0x006D95FB File Offset: 0x006D77FB
				public override void Interact()
				{
					Main.NPCChatText_GuideReverseCrafting();
				}
			}

			// Token: 0x02000AC4 RID: 2756
			public class TinkererReforge : NPCInteraction
			{
				// Token: 0x06004C57 RID: 19543 RVA: 0x006D9602 File Offset: 0x006D7802
				public override bool Condition()
				{
					return base.TalkNPCType == 107;
				}

				// Token: 0x06004C58 RID: 19544 RVA: 0x006D960E File Offset: 0x006D780E
				public override string GetText()
				{
					return Lang.inter[19].Value;
				}

				// Token: 0x06004C59 RID: 19545 RVA: 0x006D961D File Offset: 0x006D781D
				public override void Interact()
				{
					Main.NPCChatText_TinkererReforge();
				}
			}

			// Token: 0x02000AC5 RID: 2757
			public class StylistHairWindow : NPCInteraction
			{
				// Token: 0x06004C5B RID: 19547 RVA: 0x006D9624 File Offset: 0x006D7824
				public override bool Condition()
				{
					return base.TalkNPCType == 353;
				}

				// Token: 0x06004C5C RID: 19548 RVA: 0x006D9633 File Offset: 0x006D7833
				public override string GetText()
				{
					return Language.GetTextValue("GameUI.HairStyle");
				}

				// Token: 0x06004C5D RID: 19549 RVA: 0x006D963F File Offset: 0x006D783F
				public override void Interact()
				{
					Main.OpenHairWindow();
				}
			}

			// Token: 0x02000AC6 RID: 2758
			public class DyeTraderRarePlant : NPCInteraction
			{
				// Token: 0x06004C5F RID: 19551 RVA: 0x006D9646 File Offset: 0x006D7846
				public override bool Condition()
				{
					return base.TalkNPCType == 207 && Main.hardMode;
				}

				// Token: 0x06004C60 RID: 19552 RVA: 0x006D965C File Offset: 0x006D785C
				public override string GetText()
				{
					return Lang.inter[107].Value;
				}

				// Token: 0x06004C61 RID: 19553 RVA: 0x006D966B File Offset: 0x006D786B
				public override void Interact()
				{
					Main.NPCChatText_DyeTraderRarePlant();
				}
			}

			// Token: 0x02000AC7 RID: 2759
			public class TavernkeepAdvice : NPCInteraction
			{
				// Token: 0x06004C63 RID: 19555 RVA: 0x006D9672 File Offset: 0x006D7872
				public override bool Condition()
				{
					return base.TalkNPCType == 550;
				}

				// Token: 0x06004C64 RID: 19556 RVA: 0x006D9681 File Offset: 0x006D7881
				public override string GetText()
				{
					return Language.GetTextValue("UI.BartenderHelp");
				}

				// Token: 0x06004C65 RID: 19557 RVA: 0x006D968D File Offset: 0x006D788D
				public override void Interact()
				{
					Main.NPCChatText_TavernkeepAdvice();
				}
			}
		}
	}
}
