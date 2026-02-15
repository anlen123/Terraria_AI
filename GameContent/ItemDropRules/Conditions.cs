using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x02000313 RID: 787
	public class Conditions
	{
		// Token: 0x06002701 RID: 9985 RVA: 0x005600F8 File Offset: 0x0055E2F8
		public static bool SoulOfWhateverConditionCanDrop(DropAttemptInfo info)
		{
			if (info.npc.boss)
			{
				return false;
			}
			if (NPCID.Sets.DontDropDungeonKeysOrSouls[info.npc.type])
			{
				return false;
			}
			int type = info.npc.type;
			if (type <= 15)
			{
				if (type != 1 && type - 13 > 2)
				{
					goto IL_51;
				}
			}
			else if (type != 121 && type != 535)
			{
				goto IL_51;
			}
			return false;
			IL_51:
			if (Main.remixWorld)
			{
				if (!Main.hardMode || info.npc.lifeMax <= 1 || info.npc.friendly || info.npc.value < 1f)
				{
					return false;
				}
			}
			else if (!Main.hardMode || info.npc.lifeMax <= 1 || info.npc.friendly || (double)info.npc.position.Y <= Main.rockLayer * 16.0 || info.npc.value < 1f)
			{
				return false;
			}
			return true;
		}

		// Token: 0x02000830 RID: 2096
		public class NeverTrue : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004344 RID: 17220 RVA: 0x001DA9FB File Offset: 0x001D8BFB
			public bool CanDrop(DropAttemptInfo info)
			{
				return false;
			}

			// Token: 0x06004345 RID: 17221 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x06004346 RID: 17222 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x02000831 RID: 2097
		public class IsUsingSpecificAIValues : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004348 RID: 17224 RVA: 0x006BF6B5 File Offset: 0x006BD8B5
			public IsUsingSpecificAIValues(int aislot, float valueToMatch)
			{
				this.aiSlotToCheck = aislot;
				this.valueToMatch = valueToMatch;
			}

			// Token: 0x06004349 RID: 17225 RVA: 0x006BF6CB File Offset: 0x006BD8CB
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.ai[this.aiSlotToCheck] == this.valueToMatch;
			}

			// Token: 0x0600434A RID: 17226 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x0600434B RID: 17227 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}

			// Token: 0x04007245 RID: 29253
			public int aiSlotToCheck;

			// Token: 0x04007246 RID: 29254
			public float valueToMatch;
		}

		// Token: 0x02000832 RID: 2098
		public class FrostMoonDropGatingChance : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x0600434C RID: 17228 RVA: 0x006BF6E8 File Offset: 0x006BD8E8
			public bool CanDrop(DropAttemptInfo info)
			{
				if (!Main.snowMoon)
				{
					return false;
				}
				int num = NPC.waveNumber;
				if (Main.expertMode)
				{
					num += 5;
				}
				int num2 = (int)((double)(28 - num) / 2.5);
				if (Main.expertMode)
				{
					num2 -= 2;
				}
				if (num2 < 1)
				{
					num2 = 1;
				}
				return info.player.RollLuck(num2) == 0;
			}

			// Token: 0x0600434D RID: 17229 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x0600434E RID: 17230 RVA: 0x006BF740 File Offset: 0x006BD940
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.WaveBasedDrop");
			}
		}

		// Token: 0x02000833 RID: 2099
		public class PumpkinMoonDropGatingChance : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004350 RID: 17232 RVA: 0x006BF74C File Offset: 0x006BD94C
			public bool CanDrop(DropAttemptInfo info)
			{
				if (!Main.pumpkinMoon)
				{
					return false;
				}
				int num = NPC.waveNumber;
				if (Main.expertMode)
				{
					num += 5;
				}
				int num2 = (int)((double)(24 - num) / 2.5);
				if (Main.expertMode)
				{
					num2--;
				}
				if (num2 < 1)
				{
					num2 = 1;
				}
				return info.player.RollLuck(num2) == 0;
			}

			// Token: 0x06004351 RID: 17233 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x06004352 RID: 17234 RVA: 0x006BF740 File Offset: 0x006BD940
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.WaveBasedDrop");
			}
		}

		// Token: 0x02000834 RID: 2100
		public class FrostMoonDropGateForTrophies : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004354 RID: 17236 RVA: 0x006BF7A4 File Offset: 0x006BD9A4
			public bool CanDrop(DropAttemptInfo info)
			{
				if (!Main.snowMoon)
				{
					return false;
				}
				int waveNumber = NPC.waveNumber;
				if (NPC.waveNumber < 15)
				{
					return false;
				}
				int num = 4;
				if (waveNumber == 16)
				{
					num = 4;
				}
				if (waveNumber == 17)
				{
					num = 3;
				}
				if (waveNumber == 18)
				{
					num = 3;
				}
				if (waveNumber == 19)
				{
					num = 2;
				}
				if (waveNumber >= 20)
				{
					num = 2;
				}
				if (Main.expertMode && Main.rand.Next(3) == 0)
				{
					num--;
				}
				return info.rng.Next(num) == 0;
			}

			// Token: 0x06004355 RID: 17237 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x06004356 RID: 17238 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x02000835 RID: 2101
		public class PumpkinMoonDropGateForTrophies : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004358 RID: 17240 RVA: 0x006BF818 File Offset: 0x006BDA18
			public bool CanDrop(DropAttemptInfo info)
			{
				if (!Main.pumpkinMoon)
				{
					return false;
				}
				int waveNumber = NPC.waveNumber;
				if (NPC.waveNumber < 15)
				{
					return false;
				}
				int num = 4;
				if (waveNumber == 16)
				{
					num = 4;
				}
				if (waveNumber == 17)
				{
					num = 3;
				}
				if (waveNumber == 18)
				{
					num = 3;
				}
				if (waveNumber == 19)
				{
					num = 2;
				}
				if (waveNumber >= 20)
				{
					num = 2;
				}
				if (Main.expertMode && Main.rand.Next(3) == 0)
				{
					num--;
				}
				return info.rng.Next(num) == 0;
			}

			// Token: 0x06004359 RID: 17241 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x0600435A RID: 17242 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x02000836 RID: 2102
		public class IsPumpkinMoon : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x0600435C RID: 17244 RVA: 0x006BF88B File Offset: 0x006BDA8B
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.pumpkinMoon;
			}

			// Token: 0x0600435D RID: 17245 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x0600435E RID: 17246 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x02000837 RID: 2103
		public class FromCertainWaveAndAbove : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004360 RID: 17248 RVA: 0x006BF892 File Offset: 0x006BDA92
			public FromCertainWaveAndAbove(int neededWave)
			{
				this.neededWave = neededWave;
			}

			// Token: 0x06004361 RID: 17249 RVA: 0x006BF8A1 File Offset: 0x006BDAA1
			public bool CanDrop(DropAttemptInfo info)
			{
				return NPC.waveNumber >= this.neededWave;
			}

			// Token: 0x06004362 RID: 17250 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x06004363 RID: 17251 RVA: 0x006BF8B3 File Offset: 0x006BDAB3
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.PastWaveBasedDrop", this.neededWave);
			}

			// Token: 0x04007247 RID: 29255
			public int neededWave;
		}

		// Token: 0x02000838 RID: 2104
		public class IsBloodMoonAndNotFromStatue : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004364 RID: 17252 RVA: 0x006BF8CA File Offset: 0x006BDACA
			public bool CanDrop(DropAttemptInfo info)
			{
				return !Main.dayTime && Main.bloodMoon && !info.npc.SpawnedFromStatue && !info.IsInSimulation;
			}

			// Token: 0x06004365 RID: 17253 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x06004366 RID: 17254 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x02000839 RID: 2105
		public class DownedAllMechBosses : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004368 RID: 17256 RVA: 0x006BF8F2 File Offset: 0x006BDAF2
			public bool CanDrop(DropAttemptInfo info)
			{
				return NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3;
			}

			// Token: 0x06004369 RID: 17257 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x0600436A RID: 17258 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x0200083A RID: 2106
		public class DownedPlantera : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x0600436C RID: 17260 RVA: 0x006BF909 File Offset: 0x006BDB09
			public bool CanDrop(DropAttemptInfo info)
			{
				return NPC.downedPlantBoss;
			}

			// Token: 0x0600436D RID: 17261 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x0600436E RID: 17262 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x0200083B RID: 2107
		public class IsHardmode : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004370 RID: 17264 RVA: 0x004DD56B File Offset: 0x004DB76B
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.hardMode;
			}

			// Token: 0x06004371 RID: 17265 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x06004372 RID: 17266 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x0200083C RID: 2108
		public class FirstTimeKillingPlantera : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004374 RID: 17268 RVA: 0x006BF910 File Offset: 0x006BDB10
			public bool CanDrop(DropAttemptInfo info)
			{
				return !NPC.downedPlantBoss;
			}

			// Token: 0x06004375 RID: 17269 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x06004376 RID: 17270 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x0200083D RID: 2109
		public class MechanicalBossesDummyCondition : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004378 RID: 17272 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanDrop(DropAttemptInfo info)
			{
				return true;
			}

			// Token: 0x06004379 RID: 17273 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x0600437A RID: 17274 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x0200083E RID: 2110
		public class PirateMap : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x0600437C RID: 17276 RVA: 0x006BF91C File Offset: 0x006BDB1C
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.value > 0f && Main.hardMode && (double)(info.npc.position.Y / 16f) < Main.worldSurface + 10.0 && (info.npc.Center.X / 16f < 380f || info.npc.Center.X / 16f > (float)(Main.maxTilesX - 380)) && !info.IsInSimulation;
			}

			// Token: 0x0600437D RID: 17277 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x0600437E RID: 17278 RVA: 0x006BF9B6 File Offset: 0x006BDBB6
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.PirateMap");
			}
		}

		// Token: 0x0200083F RID: 2111
		public class IsChristmas : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004380 RID: 17280 RVA: 0x006BF9C2 File Offset: 0x006BDBC2
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.xMas;
			}

			// Token: 0x06004381 RID: 17281 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x06004382 RID: 17282 RVA: 0x006BF9C9 File Offset: 0x006BDBC9
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.IsChristmas");
			}
		}

		// Token: 0x02000840 RID: 2112
		public class NotExpert : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004384 RID: 17284 RVA: 0x006BF9D5 File Offset: 0x006BDBD5
			public bool CanDrop(DropAttemptInfo info)
			{
				return !Main.expertMode;
			}

			// Token: 0x06004385 RID: 17285 RVA: 0x006BF9D5 File Offset: 0x006BDBD5
			public bool CanShowItemDropInUI()
			{
				return !Main.expertMode;
			}

			// Token: 0x06004386 RID: 17286 RVA: 0x006BF9DF File Offset: 0x006BDBDF
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.NotExpert");
			}
		}

		// Token: 0x02000841 RID: 2113
		public class DropExtraGel : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004388 RID: 17288 RVA: 0x006BF9EB File Offset: 0x006BDBEB
			public bool CanDrop(DropAttemptInfo info)
			{
				return SpecialSeedFeatures.ShouldDropExtraGel;
			}

			// Token: 0x06004389 RID: 17289 RVA: 0x006BF9EB File Offset: 0x006BDBEB
			public bool CanShowItemDropInUI()
			{
				return SpecialSeedFeatures.ShouldDropExtraGel;
			}

			// Token: 0x0600438A RID: 17290 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x02000842 RID: 2114
		public class NotDropExtraGel : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x0600438C RID: 17292 RVA: 0x006BF9F2 File Offset: 0x006BDBF2
			public bool CanDrop(DropAttemptInfo info)
			{
				return !SpecialSeedFeatures.ShouldDropExtraGel;
			}

			// Token: 0x0600438D RID: 17293 RVA: 0x006BF9F2 File Offset: 0x006BDBF2
			public bool CanShowItemDropInUI()
			{
				return !SpecialSeedFeatures.ShouldDropExtraGel;
			}

			// Token: 0x0600438E RID: 17294 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x02000843 RID: 2115
		public class NotMasterMode : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004390 RID: 17296 RVA: 0x006BF9FC File Offset: 0x006BDBFC
			public bool CanDrop(DropAttemptInfo info)
			{
				return !Main.masterMode;
			}

			// Token: 0x06004391 RID: 17297 RVA: 0x006BF9FC File Offset: 0x006BDBFC
			public bool CanShowItemDropInUI()
			{
				return !Main.masterMode;
			}

			// Token: 0x06004392 RID: 17298 RVA: 0x006BFA06 File Offset: 0x006BDC06
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.NotMasterMode");
			}
		}

		// Token: 0x02000844 RID: 2116
		public class MissingTwin : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004394 RID: 17300 RVA: 0x006BFA14 File Offset: 0x006BDC14
			public bool CanDrop(DropAttemptInfo info)
			{
				int type = 125;
				if (info.npc.type == 125)
				{
					type = 126;
				}
				return !NPC.AnyNPCs(type);
			}

			// Token: 0x06004395 RID: 17301 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x06004396 RID: 17302 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x02000845 RID: 2117
		public class EmpressOfLightIsGenuinelyEnraged : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004398 RID: 17304 RVA: 0x006BFA3F File Offset: 0x006BDC3F
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.AI_120_HallowBoss_IsGenuinelyEnraged();
			}

			// Token: 0x06004399 RID: 17305 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x0600439A RID: 17306 RVA: 0x006BFA4C File Offset: 0x006BDC4C
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.EmpressOfLightOnlyTookDamageWhileEnraged");
			}
		}

		// Token: 0x02000846 RID: 2118
		public class RedHatSkeletron : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x0600439C RID: 17308 RVA: 0x006BFA58 File Offset: 0x006BDC58
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.RedHatSkeletronAdjustmentsEnabled();
			}

			// Token: 0x0600439D RID: 17309 RVA: 0x006BFA65 File Offset: 0x006BDC65
			public bool CanShowItemDropInUI()
			{
				return Main.Difficulty >= GameDifficultyLevel.Legendary;
			}

			// Token: 0x0600439E RID: 17310 RVA: 0x006BFA76 File Offset: 0x006BDC76
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.RedHatSkeletron");
			}
		}

		// Token: 0x02000847 RID: 2119
		public class PlayerNeedsHealing : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043A0 RID: 17312 RVA: 0x006BFA82 File Offset: 0x006BDC82
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.player.statLife < info.player.statLifeMax2;
			}

			// Token: 0x060043A1 RID: 17313 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x060043A2 RID: 17314 RVA: 0x006BFA9C File Offset: 0x006BDC9C
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.PlayerNeedsHealing");
			}
		}

		// Token: 0x02000848 RID: 2120
		public class MechdusaKill : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043A4 RID: 17316 RVA: 0x006BFAA8 File Offset: 0x006BDCA8
			public bool CanDrop(DropAttemptInfo info)
			{
				if (!SpecialSeedFeatures.Mechdusa)
				{
					return false;
				}
				for (int i = 0; i < Conditions.MechdusaKill._targetList.Length; i++)
				{
					if (Conditions.MechdusaKill._targetList[i] != info.npc.type && NPC.AnyNPCs(Conditions.MechdusaKill._targetList[i]))
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x060043A5 RID: 17317 RVA: 0x006BFAF5 File Offset: 0x006BDCF5
			public bool CanShowItemDropInUI()
			{
				return SpecialSeedFeatures.Mechdusa;
			}

			// Token: 0x060043A6 RID: 17318 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}

			// Token: 0x04007248 RID: 29256
			private static int[] _targetList = new int[]
			{
				127,
				126,
				125,
				134
			};
		}

		// Token: 0x02000849 RID: 2121
		public class LegacyHack_IsBossAndExpert : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043A9 RID: 17321 RVA: 0x006BFB14 File Offset: 0x006BDD14
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.boss && Main.expertMode;
			}

			// Token: 0x060043AA RID: 17322 RVA: 0x006BFB2A File Offset: 0x006BDD2A
			public bool CanShowItemDropInUI()
			{
				return Main.expertMode;
			}

			// Token: 0x060043AB RID: 17323 RVA: 0x006BFB31 File Offset: 0x006BDD31
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.LegacyHack_IsBossAndExpert");
			}
		}

		// Token: 0x0200084A RID: 2122
		public class LegacyHack_IsBossAndNotExpert : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043AD RID: 17325 RVA: 0x006BFB3D File Offset: 0x006BDD3D
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.boss && !Main.expertMode;
			}

			// Token: 0x060043AE RID: 17326 RVA: 0x006BF9D5 File Offset: 0x006BDBD5
			public bool CanShowItemDropInUI()
			{
				return !Main.expertMode;
			}

			// Token: 0x060043AF RID: 17327 RVA: 0x006BFB56 File Offset: 0x006BDD56
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.LegacyHack_IsBossAndNotExpert");
			}
		}

		// Token: 0x0200084B RID: 2123
		public class LegacyHack_IsABoss : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043B1 RID: 17329 RVA: 0x006BFB62 File Offset: 0x006BDD62
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.boss;
			}

			// Token: 0x060043B2 RID: 17330 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x060043B3 RID: 17331 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x0200084C RID: 2124
		public class IsExpert : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043B5 RID: 17333 RVA: 0x006BFB2A File Offset: 0x006BDD2A
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.expertMode;
			}

			// Token: 0x060043B6 RID: 17334 RVA: 0x006BFB2A File Offset: 0x006BDD2A
			public bool CanShowItemDropInUI()
			{
				return Main.expertMode;
			}

			// Token: 0x060043B7 RID: 17335 RVA: 0x006BFB6F File Offset: 0x006BDD6F
			public string GetConditionDescription()
			{
				if (Main.masterMode)
				{
					return Language.GetTextValue("Bestiary_ItemDropConditions.IsMasterMode");
				}
				return Language.GetTextValue("Bestiary_ItemDropConditions.IsExpert");
			}
		}

		// Token: 0x0200084D RID: 2125
		public class IsMasterMode : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043B9 RID: 17337 RVA: 0x006BFB8D File Offset: 0x006BDD8D
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.masterMode;
			}

			// Token: 0x060043BA RID: 17338 RVA: 0x006BFB8D File Offset: 0x006BDD8D
			public bool CanShowItemDropInUI()
			{
				return Main.masterMode;
			}

			// Token: 0x060043BB RID: 17339 RVA: 0x006BFB94 File Offset: 0x006BDD94
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.IsMasterMode");
			}
		}

		// Token: 0x0200084E RID: 2126
		public class IsCrimson : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043BD RID: 17341 RVA: 0x006BFBA0 File Offset: 0x006BDDA0
			public bool CanDrop(DropAttemptInfo info)
			{
				return WorldGen.crimson;
			}

			// Token: 0x060043BE RID: 17342 RVA: 0x006BFBA0 File Offset: 0x006BDDA0
			public bool CanShowItemDropInUI()
			{
				return WorldGen.crimson;
			}

			// Token: 0x060043BF RID: 17343 RVA: 0x006BFBA7 File Offset: 0x006BDDA7
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.IsCrimson");
			}
		}

		// Token: 0x0200084F RID: 2127
		public class IsCorruption : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043C1 RID: 17345 RVA: 0x006BFBB3 File Offset: 0x006BDDB3
			public bool CanDrop(DropAttemptInfo info)
			{
				return !WorldGen.crimson;
			}

			// Token: 0x060043C2 RID: 17346 RVA: 0x006BFBB3 File Offset: 0x006BDDB3
			public bool CanShowItemDropInUI()
			{
				return !WorldGen.crimson;
			}

			// Token: 0x060043C3 RID: 17347 RVA: 0x006BFBBD File Offset: 0x006BDDBD
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.IsCorruption");
			}
		}

		// Token: 0x02000850 RID: 2128
		public class IsCrimsonAndNotExpert : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043C5 RID: 17349 RVA: 0x006BFBC9 File Offset: 0x006BDDC9
			public bool CanDrop(DropAttemptInfo info)
			{
				return WorldGen.crimson && !Main.expertMode;
			}

			// Token: 0x060043C6 RID: 17350 RVA: 0x006BFBC9 File Offset: 0x006BDDC9
			public bool CanShowItemDropInUI()
			{
				return WorldGen.crimson && !Main.expertMode;
			}

			// Token: 0x060043C7 RID: 17351 RVA: 0x006BFBDC File Offset: 0x006BDDDC
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.IsCrimsonAndNotExpert");
			}
		}

		// Token: 0x02000851 RID: 2129
		public class IsCorruptionAndNotExpert : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043C9 RID: 17353 RVA: 0x006BFBE8 File Offset: 0x006BDDE8
			public bool CanDrop(DropAttemptInfo info)
			{
				return !WorldGen.crimson && !Main.expertMode;
			}

			// Token: 0x060043CA RID: 17354 RVA: 0x006BFBE8 File Offset: 0x006BDDE8
			public bool CanShowItemDropInUI()
			{
				return !WorldGen.crimson && !Main.expertMode;
			}

			// Token: 0x060043CB RID: 17355 RVA: 0x006BFBFB File Offset: 0x006BDDFB
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.IsCorruptionAndNotExpert");
			}
		}

		// Token: 0x02000852 RID: 2130
		public class HalloweenWeapons : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043CD RID: 17357 RVA: 0x006BFC08 File Offset: 0x006BDE08
			public bool CanDrop(DropAttemptInfo info)
			{
				float num = 500f * GameDifficultyData.EnemyMoneyDropMultiplier.Sample(Main.Difficulty);
				float num2 = 40f * GameDifficultyData.EnemyDamageMultiplier.Sample(Main.Difficulty);
				float num3 = 20f;
				return Main.halloween && info.npc.value > 0f && info.npc.value < num && (float)info.npc.damage < num2 && (float)info.npc.defense < num3 && !info.IsInSimulation;
			}

			// Token: 0x060043CE RID: 17358 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x060043CF RID: 17359 RVA: 0x006BFC9D File Offset: 0x006BDE9D
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.HalloweenWeapons");
			}
		}

		// Token: 0x02000853 RID: 2131
		public class SoulOfNight : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043D1 RID: 17361 RVA: 0x006BFCA9 File Offset: 0x006BDEA9
			public bool CanDrop(DropAttemptInfo info)
			{
				return Conditions.SoulOfWhateverConditionCanDrop(info) && (info.player.ZoneCorrupt || info.player.ZoneCrimson);
			}

			// Token: 0x060043D2 RID: 17362 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x060043D3 RID: 17363 RVA: 0x006BFCCF File Offset: 0x006BDECF
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.SoulOfNight");
			}
		}

		// Token: 0x02000854 RID: 2132
		public class SoulOfLight : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043D5 RID: 17365 RVA: 0x006BFCDB File Offset: 0x006BDEDB
			public bool CanDrop(DropAttemptInfo info)
			{
				return Conditions.SoulOfWhateverConditionCanDrop(info) && info.player.ZoneHallow;
			}

			// Token: 0x060043D6 RID: 17366 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x060043D7 RID: 17367 RVA: 0x006BFCF2 File Offset: 0x006BDEF2
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.SoulOfLight");
			}
		}

		// Token: 0x02000855 RID: 2133
		public class NotFromStatue : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043D9 RID: 17369 RVA: 0x006BFCFE File Offset: 0x006BDEFE
			public bool CanDrop(DropAttemptInfo info)
			{
				return !info.npc.SpawnedFromStatue;
			}

			// Token: 0x060043DA RID: 17370 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x060043DB RID: 17371 RVA: 0x006BFD0E File Offset: 0x006BDF0E
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.NotFromStatue");
			}
		}

		// Token: 0x02000856 RID: 2134
		public class HalloweenGoodieBagDrop : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043DD RID: 17373 RVA: 0x006BFD1C File Offset: 0x006BDF1C
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.halloween && info.npc.lifeMax > 1 && info.npc.damage > 0 && !info.npc.friendly && info.npc.type != 121 && info.npc.type != 23 && info.npc.value > 0f && !info.IsInSimulation;
			}

			// Token: 0x060043DE RID: 17374 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x060043DF RID: 17375 RVA: 0x006BFD94 File Offset: 0x006BDF94
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.HalloweenGoodieBagDrop");
			}
		}

		// Token: 0x02000857 RID: 2135
		public class XmasPresentDrop : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043E1 RID: 17377 RVA: 0x006BFDA0 File Offset: 0x006BDFA0
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.xMas && info.npc.lifeMax > 1 && info.npc.damage > 0 && !info.npc.friendly && info.npc.type != 121 && info.npc.type != 23 && info.npc.value > 0f && !info.IsInSimulation;
			}

			// Token: 0x060043E2 RID: 17378 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x060043E3 RID: 17379 RVA: 0x006BFE18 File Offset: 0x006BE018
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.XmasPresentDrop");
			}
		}

		// Token: 0x02000858 RID: 2136
		public class LivingFlames : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043E5 RID: 17381 RVA: 0x006BFE24 File Offset: 0x006BE024
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.lifeMax > 5 && info.npc.value > 0f && !info.npc.friendly && Main.hardMode && info.npc.position.Y / 16f > (float)Main.UnderworldLayer && !info.IsInSimulation;
			}

			// Token: 0x060043E6 RID: 17382 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x060043E7 RID: 17383 RVA: 0x006BFE8E File Offset: 0x006BE08E
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.LivingFlames");
			}
		}

		// Token: 0x02000859 RID: 2137
		public class NamedNPC : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043E9 RID: 17385 RVA: 0x006BFE9A File Offset: 0x006BE09A
			public NamedNPC(string neededName)
			{
				this.neededName = neededName;
			}

			// Token: 0x060043EA RID: 17386 RVA: 0x006BFEA9 File Offset: 0x006BE0A9
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.HasGivenName && info.npc.GivenName == Language.GetTextValue(this.neededName);
			}

			// Token: 0x060043EB RID: 17387 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x060043EC RID: 17388 RVA: 0x006BFED5 File Offset: 0x006BE0D5
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.NamedNPC");
			}

			// Token: 0x04007249 RID: 29257
			public string neededName;
		}

		// Token: 0x0200085A RID: 2138
		public class HallowKeyCondition : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043ED RID: 17389 RVA: 0x006BFEE4 File Offset: 0x006BE0E4
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.value > 0f && !NPCID.Sets.DontDropDungeonKeysOrSouls[info.npc.type] && Main.hardMode && !info.IsInSimulation && info.player.ZoneHallow;
			}

			// Token: 0x060043EE RID: 17390 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x060043EF RID: 17391 RVA: 0x006BFF32 File Offset: 0x006BE132
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.HallowKeyCondition");
			}
		}

		// Token: 0x0200085B RID: 2139
		public class JungleKeyCondition : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043F1 RID: 17393 RVA: 0x006BFF40 File Offset: 0x006BE140
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.value > 0f && !NPCID.Sets.DontDropDungeonKeysOrSouls[info.npc.type] && Main.hardMode && !info.IsInSimulation && info.player.ZoneJungle;
			}

			// Token: 0x060043F2 RID: 17394 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x060043F3 RID: 17395 RVA: 0x006BFF8E File Offset: 0x006BE18E
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.JungleKeyCondition");
			}
		}

		// Token: 0x0200085C RID: 2140
		public class CorruptKeyCondition : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043F5 RID: 17397 RVA: 0x006BFF9C File Offset: 0x006BE19C
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.value > 0f && !NPCID.Sets.DontDropDungeonKeysOrSouls[info.npc.type] && Main.hardMode && !info.IsInSimulation && info.player.ZoneCorrupt;
			}

			// Token: 0x060043F6 RID: 17398 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x060043F7 RID: 17399 RVA: 0x006BFFEA File Offset: 0x006BE1EA
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.CorruptKeyCondition");
			}
		}

		// Token: 0x0200085D RID: 2141
		public class CrimsonKeyCondition : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043F9 RID: 17401 RVA: 0x006BFFF8 File Offset: 0x006BE1F8
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.value > 0f && !NPCID.Sets.DontDropDungeonKeysOrSouls[info.npc.type] && Main.hardMode && !info.IsInSimulation && info.player.ZoneCrimson;
			}

			// Token: 0x060043FA RID: 17402 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x060043FB RID: 17403 RVA: 0x006C0046 File Offset: 0x006BE246
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.CrimsonKeyCondition");
			}
		}

		// Token: 0x0200085E RID: 2142
		public class FrozenKeyCondition : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x060043FD RID: 17405 RVA: 0x006C0054 File Offset: 0x006BE254
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.value > 0f && !NPCID.Sets.DontDropDungeonKeysOrSouls[info.npc.type] && Main.hardMode && !info.IsInSimulation && info.player.ZoneSnow;
			}

			// Token: 0x060043FE RID: 17406 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x060043FF RID: 17407 RVA: 0x006C00A2 File Offset: 0x006BE2A2
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.FrozenKeyCondition");
			}
		}

		// Token: 0x0200085F RID: 2143
		public class DesertKeyCondition : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004401 RID: 17409 RVA: 0x006C00B0 File Offset: 0x006BE2B0
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.value > 0f && !NPCID.Sets.DontDropDungeonKeysOrSouls[info.npc.type] && Main.hardMode && !info.IsInSimulation && info.player.ZoneDesert && !info.player.ZoneBeach;
			}

			// Token: 0x06004402 RID: 17410 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x06004403 RID: 17411 RVA: 0x006C010E File Offset: 0x006BE30E
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.DesertKeyCondition");
			}
		}

		// Token: 0x02000860 RID: 2144
		public class BeatAnyMechBoss : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004405 RID: 17413 RVA: 0x006C011A File Offset: 0x006BE31A
			public bool CanDrop(DropAttemptInfo info)
			{
				return NPC.downedMechBossAny;
			}

			// Token: 0x06004406 RID: 17414 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x06004407 RID: 17415 RVA: 0x006C0121 File Offset: 0x006BE321
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.BeatAnyMechBoss");
			}
		}

		// Token: 0x02000861 RID: 2145
		public class YoyoCascade : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004409 RID: 17417 RVA: 0x006C0130 File Offset: 0x006BE330
			public bool CanDrop(DropAttemptInfo info)
			{
				return !Main.hardMode && info.npc.HasPlayerTarget && info.npc.lifeMax > 5 && !info.npc.friendly && info.npc.value > 0f && info.npc.position.Y / 16f > (float)(Main.maxTilesY - 350) && NPC.downedBoss3 && !info.IsInSimulation;
			}

			// Token: 0x0600440A RID: 17418 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x0600440B RID: 17419 RVA: 0x006C01B4 File Offset: 0x006BE3B4
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.YoyoCascade");
			}
		}

		// Token: 0x02000862 RID: 2146
		public class YoyosAmarok : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x0600440D RID: 17421 RVA: 0x006C01C0 File Offset: 0x006BE3C0
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.hardMode && info.npc.HasPlayerTarget && info.player.ZoneSnow && info.npc.lifeMax > 5 && !info.npc.friendly && info.npc.value > 0f && !info.IsInSimulation;
			}

			// Token: 0x0600440E RID: 17422 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x0600440F RID: 17423 RVA: 0x006C0226 File Offset: 0x006BE426
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.YoyosAmarok");
			}
		}

		// Token: 0x02000863 RID: 2147
		public class YoyosYelets : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004411 RID: 17425 RVA: 0x006C0234 File Offset: 0x006BE434
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.hardMode && info.player.ZoneJungle && NPC.downedMechBossAny && info.npc.lifeMax > 5 && info.npc.HasPlayerTarget && !info.npc.friendly && info.npc.value > 0f && !info.IsInSimulation;
			}

			// Token: 0x06004412 RID: 17426 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x06004413 RID: 17427 RVA: 0x006C02A1 File Offset: 0x006BE4A1
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.YoyosYelets");
			}
		}

		// Token: 0x02000864 RID: 2148
		public class YoyosKraken : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004415 RID: 17429 RVA: 0x006C02B0 File Offset: 0x006BE4B0
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.hardMode && info.player.ZoneDungeon && NPC.downedPlantBoss && info.npc.lifeMax > 5 && info.npc.HasPlayerTarget && !info.npc.friendly && info.npc.value > 0f && !info.IsInSimulation;
			}

			// Token: 0x06004416 RID: 17430 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x06004417 RID: 17431 RVA: 0x006C031D File Offset: 0x006BE51D
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.YoyosKraken");
			}
		}

		// Token: 0x02000865 RID: 2149
		public class YoyosHelFire : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004419 RID: 17433 RVA: 0x006C032C File Offset: 0x006BE52C
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.hardMode && !info.player.ZoneDungeon && (double)(info.npc.position.Y / 16f) > (Main.rockLayer + (double)(Main.maxTilesY * 2)) / 3.0 && info.npc.lifeMax > 5 && info.npc.HasPlayerTarget && !info.npc.friendly && info.npc.value > 0f && !info.IsInSimulation;
			}

			// Token: 0x0600441A RID: 17434 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x0600441B RID: 17435 RVA: 0x006C03C6 File Offset: 0x006BE5C6
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.YoyosHelFire");
			}
		}

		// Token: 0x02000866 RID: 2150
		public class WindyEnoughForKiteDrops : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x0600441D RID: 17437 RVA: 0x006C03D2 File Offset: 0x006BE5D2
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.WindyEnoughForKiteDrops;
			}

			// Token: 0x0600441E RID: 17438 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CanShowItemDropInUI()
			{
				return true;
			}

			// Token: 0x0600441F RID: 17439 RVA: 0x006C03D9 File Offset: 0x006BE5D9
			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.IsItAHappyWindyDay");
			}
		}

		// Token: 0x02000867 RID: 2151
		public class Easymode : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004421 RID: 17441 RVA: 0x006C03E5 File Offset: 0x006BE5E5
			public bool CanDrop(DropAttemptInfo info)
			{
				return !Main.hardMode;
			}

			// Token: 0x06004422 RID: 17442 RVA: 0x006C03E5 File Offset: 0x006BE5E5
			public bool CanShowItemDropInUI()
			{
				return !Main.hardMode;
			}

			// Token: 0x06004423 RID: 17443 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x02000868 RID: 2152
		public class RemixSeed : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004425 RID: 17445 RVA: 0x006C03EF File Offset: 0x006BE5EF
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.remixWorld;
			}

			// Token: 0x06004426 RID: 17446 RVA: 0x006C03EF File Offset: 0x006BE5EF
			public bool CanShowItemDropInUI()
			{
				return Main.remixWorld;
			}

			// Token: 0x06004427 RID: 17447 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x02000869 RID: 2153
		public class NotRemixSeed : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004429 RID: 17449 RVA: 0x006C03F6 File Offset: 0x006BE5F6
			public bool CanDrop(DropAttemptInfo info)
			{
				return !Main.remixWorld;
			}

			// Token: 0x0600442A RID: 17450 RVA: 0x006C03F6 File Offset: 0x006BE5F6
			public bool CanShowItemDropInUI()
			{
				return !Main.remixWorld;
			}

			// Token: 0x0600442B RID: 17451 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x0200086A RID: 2154
		public class RemixSeedEasymode : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x0600442D RID: 17453 RVA: 0x006C0400 File Offset: 0x006BE600
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.remixWorld && !Main.hardMode;
			}

			// Token: 0x0600442E RID: 17454 RVA: 0x006C0400 File Offset: 0x006BE600
			public bool CanShowItemDropInUI()
			{
				return Main.remixWorld && !Main.hardMode;
			}

			// Token: 0x0600442F RID: 17455 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x0200086B RID: 2155
		public class RemixSeedHardmode : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004431 RID: 17457 RVA: 0x006C0413 File Offset: 0x006BE613
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.remixWorld && Main.hardMode;
			}

			// Token: 0x06004432 RID: 17458 RVA: 0x006C0413 File Offset: 0x006BE613
			public bool CanShowItemDropInUI()
			{
				return Main.remixWorld && Main.hardMode;
			}

			// Token: 0x06004433 RID: 17459 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x0200086C RID: 2156
		public class NotRemixSeedEasymode : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004435 RID: 17461 RVA: 0x006C0423 File Offset: 0x006BE623
			public bool CanDrop(DropAttemptInfo info)
			{
				return !Main.remixWorld && !Main.hardMode;
			}

			// Token: 0x06004436 RID: 17462 RVA: 0x006C0423 File Offset: 0x006BE623
			public bool CanShowItemDropInUI()
			{
				return !Main.remixWorld && !Main.hardMode;
			}

			// Token: 0x06004437 RID: 17463 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x0200086D RID: 2157
		public class NotRemixSeedHardmode : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004439 RID: 17465 RVA: 0x006C0436 File Offset: 0x006BE636
			public bool CanDrop(DropAttemptInfo info)
			{
				return !Main.remixWorld && Main.hardMode;
			}

			// Token: 0x0600443A RID: 17466 RVA: 0x006C0436 File Offset: 0x006BE636
			public bool CanShowItemDropInUI()
			{
				return !Main.remixWorld && Main.hardMode;
			}

			// Token: 0x0600443B RID: 17467 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x0200086E RID: 2158
		public class EyeOfCthulhuDefeatedAndNoAltarsInWorld : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x0600443D RID: 17469 RVA: 0x006C0446 File Offset: 0x006BE646
			public bool CanDrop(DropAttemptInfo info)
			{
				return NPC.downedBoss1 && WorldGen.Skyblock.noAltars;
			}

			// Token: 0x0600443E RID: 17470 RVA: 0x001DA9FB File Offset: 0x001D8BFB
			public bool CanShowItemDropInUI()
			{
				return false;
			}

			// Token: 0x0600443F RID: 17471 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x0200086F RID: 2159
		public class TenthAnniversaryIsUp : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004441 RID: 17473 RVA: 0x006C0456 File Offset: 0x006BE656
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.tenthAnniversaryWorld;
			}

			// Token: 0x06004442 RID: 17474 RVA: 0x006C0456 File Offset: 0x006BE656
			public bool CanShowItemDropInUI()
			{
				return Main.tenthAnniversaryWorld;
			}

			// Token: 0x06004443 RID: 17475 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x02000870 RID: 2160
		public class TenthAnniversaryIsNotUp : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004445 RID: 17477 RVA: 0x006C045D File Offset: 0x006BE65D
			public bool CanDrop(DropAttemptInfo info)
			{
				return !Main.tenthAnniversaryWorld;
			}

			// Token: 0x06004446 RID: 17478 RVA: 0x006C045D File Offset: 0x006BE65D
			public bool CanShowItemDropInUI()
			{
				return !Main.tenthAnniversaryWorld;
			}

			// Token: 0x06004447 RID: 17479 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x02000871 RID: 2161
		public class DontStarveIsUp : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004449 RID: 17481 RVA: 0x006C0467 File Offset: 0x006BE667
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.dontStarveWorld;
			}

			// Token: 0x0600444A RID: 17482 RVA: 0x006C0467 File Offset: 0x006BE667
			public bool CanShowItemDropInUI()
			{
				return Main.dontStarveWorld;
			}

			// Token: 0x0600444B RID: 17483 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x02000872 RID: 2162
		public class DontStarveIsNotUp : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x0600444D RID: 17485 RVA: 0x006C046E File Offset: 0x006BE66E
			public bool CanDrop(DropAttemptInfo info)
			{
				return !Main.dontStarveWorld;
			}

			// Token: 0x0600444E RID: 17486 RVA: 0x006C046E File Offset: 0x006BE66E
			public bool CanShowItemDropInUI()
			{
				return !Main.dontStarveWorld;
			}

			// Token: 0x0600444F RID: 17487 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x02000873 RID: 2163
		public class SkyblockIsUp : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004451 RID: 17489 RVA: 0x006C0478 File Offset: 0x006BE678
			public bool CanDrop(DropAttemptInfo info)
			{
				return WorldGen.Skyblock.lowTiles;
			}

			// Token: 0x06004452 RID: 17490 RVA: 0x006C0478 File Offset: 0x006BE678
			public bool CanShowItemDropInUI()
			{
				return WorldGen.Skyblock.lowTiles;
			}

			// Token: 0x06004453 RID: 17491 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x02000874 RID: 2164
		public class SkyblockIsNotUp : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004455 RID: 17493 RVA: 0x006C047F File Offset: 0x006BE67F
			public bool CanDrop(DropAttemptInfo info)
			{
				return !WorldGen.Skyblock.lowTiles;
			}

			// Token: 0x06004456 RID: 17494 RVA: 0x006C047F File Offset: 0x006BE67F
			public bool CanShowItemDropInUI()
			{
				return !WorldGen.Skyblock.lowTiles;
			}

			// Token: 0x06004457 RID: 17495 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}

		// Token: 0x02000875 RID: 2165
		public class SkyblockIsUpNoSickle : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			// Token: 0x06004459 RID: 17497 RVA: 0x006C0489 File Offset: 0x006BE689
			public bool CanDrop(DropAttemptInfo info)
			{
				return WorldGen.Skyblock.lowTiles && !info.player.HasItemInInventoryOrOpenVoidBag(1786);
			}

			// Token: 0x0600445A RID: 17498 RVA: 0x001DA9FB File Offset: 0x001D8BFB
			public bool CanShowItemDropInUI()
			{
				return false;
			}

			// Token: 0x0600445B RID: 17499 RVA: 0x000762F3 File Offset: 0x000744F3
			public string GetConditionDescription()
			{
				return null;
			}
		}
	}
}
