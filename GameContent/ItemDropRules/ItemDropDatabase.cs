using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x02000314 RID: 788
	public class ItemDropDatabase
	{
		// Token: 0x06002703 RID: 9987 RVA: 0x005601EC File Offset: 0x0055E3EC
		public void PrepareNPCNetIDsByTypeDictionary()
		{
			this._npcNetIdsByType.Clear();
			foreach (KeyValuePair<int, NPC> keyValuePair in from x in ContentSamples.NpcsByNetId
			where x.Key < 0
			select x)
			{
				if (!this._npcNetIdsByType.ContainsKey(keyValuePair.Value.type))
				{
					this._npcNetIdsByType[keyValuePair.Value.type] = new List<int>();
				}
				this._npcNetIdsByType[keyValuePair.Value.type].Add(keyValuePair.Value.netID);
			}
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x005602C0 File Offset: 0x0055E4C0
		public void TrimDuplicateRulesForNegativeIDs()
		{
			for (int i = -65; i < 0; i++)
			{
				List<IItemDropRule> source;
				if (this._entriesByNpcNetId.TryGetValue(i, out source))
				{
					this._entriesByNpcNetId[i] = source.Distinct<IItemDropRule>().ToList<IItemDropRule>();
				}
			}
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x00560304 File Offset: 0x0055E504
		public List<IItemDropRule> GetRulesForNPCID(int npcNetId, bool includeGlobalDrops = true)
		{
			List<IItemDropRule> list = new List<IItemDropRule>();
			if (includeGlobalDrops)
			{
				list.AddRange(this._globalEntries);
			}
			List<IItemDropRule> collection;
			if (this._entriesByNpcNetId.TryGetValue(npcNetId, out collection))
			{
				list.AddRange(collection);
			}
			return list;
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x0056033E File Offset: 0x0055E53E
		public IItemDropRule RegisterToGlobal(IItemDropRule entry)
		{
			this._globalEntries.Add(entry);
			return entry;
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x00560350 File Offset: 0x0055E550
		public IItemDropRule RegisterToNPC(int type, IItemDropRule entry)
		{
			this.RegisterToNPCNetId(type, entry);
			List<int> list;
			if (type > 0 && this._npcNetIdsByType.TryGetValue(type, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.RegisterToNPCNetId(list[i], entry);
				}
			}
			return entry;
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x00560399 File Offset: 0x0055E599
		private void RegisterToNPCNetId(int npcNetId, IItemDropRule entry)
		{
			if (!this._entriesByNpcNetId.ContainsKey(npcNetId))
			{
				this._entriesByNpcNetId[npcNetId] = new List<IItemDropRule>();
			}
			this._entriesByNpcNetId[npcNetId].Add(entry);
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x005603CC File Offset: 0x0055E5CC
		public IItemDropRule RegisterToMultipleNPCs(IItemDropRule entry, params int[] npcNetIds)
		{
			for (int i = 0; i < npcNetIds.Length; i++)
			{
				this.RegisterToNPC(npcNetIds[i], entry);
			}
			return entry;
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x005603F4 File Offset: 0x0055E5F4
		public IItemDropRule RegisterToMultipleNPCsNotRemixSeed(IItemDropRule entry, params int[] npcNetIds)
		{
			for (int i = 0; i < npcNetIds.Length; i++)
			{
				this.RegisterToNPC(npcNetIds[i], new LeadingConditionRule(new Conditions.NotRemixSeedHardmode())).OnSuccess(entry, false);
			}
			return entry;
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x0056042C File Offset: 0x0055E62C
		public IItemDropRule RegisterToMultipleNPCsRemixSeed(IItemDropRule entry, params int[] npcNetIds)
		{
			for (int i = 0; i < npcNetIds.Length; i++)
			{
				this.RegisterToNPC(npcNetIds[i], new LeadingConditionRule(new Conditions.RemixSeed())).OnSuccess(entry, false);
			}
			return entry;
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x00560463 File Offset: 0x0055E663
		private void RemoveFromNPCNetId(int npcNetId, IItemDropRule entry)
		{
			if (!this._entriesByNpcNetId.ContainsKey(npcNetId))
			{
				return;
			}
			this._entriesByNpcNetId[npcNetId].Remove(entry);
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x00560488 File Offset: 0x0055E688
		public IItemDropRule RemoveFromNPC(int type, IItemDropRule entry)
		{
			this.RemoveFromNPCNetId(type, entry);
			List<int> list;
			if (type > 0 && this._npcNetIdsByType.TryGetValue(type, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.RemoveFromNPCNetId(list[i], entry);
				}
			}
			return entry;
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x005604D4 File Offset: 0x0055E6D4
		public IItemDropRule RemoveFromMultipleNPCs(IItemDropRule entry, params int[] npcNetIds)
		{
			for (int i = 0; i < npcNetIds.Length; i++)
			{
				this.RemoveFromNPC(npcNetIds[i], entry);
			}
			return entry;
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x005604FC File Offset: 0x0055E6FC
		public void Populate()
		{
			this.PrepareNPCNetIDsByTypeDictionary();
			this.RegisterGlobalRules();
			this.RegisterFoodDrops();
			this.RegisterWeirdRules();
			this.RegisterTownNPCDrops();
			this.RegisterDD2EventDrops();
			this.RegisterMiscDrops();
			this.RegisterHardmodeFeathers();
			this.RegisterYoyos();
			this.RegisterStatusImmunityItems();
			this.RegisterPirateDrops();
			this.RegisterBloodMoonFishingEnemies();
			this.RegisterMartianDrops();
			this.RegisterBossTrophies();
			this.RegisterBosses();
			this.RegisterHardmodeDungeonDrops();
			this.RegisterMimic();
			this.RegisterEclipse();
			this.RegisterBloodMoonFishing();
			this.TrimDuplicateRulesForNegativeIDs();
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x00560584 File Offset: 0x0055E784
		private void RegisterBloodMoonFishing()
		{
			this.RegisterToMultipleNPCs(ItemDropRule.Common(4608, 2, 4, 6), new int[]
			{
				587,
				586
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Common(4608, 2, 7, 10), new int[]
			{
				620,
				621,
				618
			});
			this.RegisterToMultipleNPCs(ItemDropRule.OneFromOptions(8, new int[]
			{
				4273
			}), new int[]
			{
				587,
				586
			});
			this.RegisterToMultipleNPCs(ItemDropRule.OneFromOptions(8, new int[]
			{
				4381
			}), new int[]
			{
				587,
				586
			});
			this.RegisterToMultipleNPCs(ItemDropRule.OneFromOptions(8, new int[]
			{
				4325
			}), new int[]
			{
				587,
				586
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Common(3213, 15, 1, 1), new int[]
			{
				587,
				586
			});
			this.RegisterToNPC(620, ItemDropRule.Common(4270, 8, 1, 1));
			this.RegisterToNPC(620, ItemDropRule.Common(4317, 8, 1, 1));
			this.RegisterToNPC(621, ItemDropRule.Common(4272, 8, 1, 1));
			this.RegisterToNPC(621, ItemDropRule.Common(4317, 8, 1, 1));
			this.RegisterToNPC(618, ItemDropRule.NormalvsExpert(4269, 2, 1));
			this.RegisterToNPC(618, ItemDropRule.Common(4054, 10, 1, 1));
			this.RegisterToNPC(618, ItemDropRule.NormalvsExpert(4271, 2, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.ScalingWithOnlyBadLuck(4271, 5, 1, 1), new int[]
			{
				53,
				536
			});
			Conditions.IsBloodMoonAndNotFromStatue condition = new Conditions.IsBloodMoonAndNotFromStatue();
			this.RegisterToMultipleNPCs(ItemDropRule.ByCondition(condition, 4271, 100, 1, 1, 1), new int[]
			{
				489,
				490
			});
			this.RegisterToMultipleNPCs(ItemDropRule.ByCondition(condition, 4271, 25, 1, 1, 1), new int[]
			{
				587,
				586,
				621,
				620
			});
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x005607D0 File Offset: 0x0055E9D0
		private void RegisterEclipse()
		{
			this.RegisterToNPC(461, ItemDropRule.ExpertGetsRerolls(497, 50, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.ExpertGetsRerolls(900, 35, 1), new int[]
			{
				159,
				158
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Common(5597, 40, 1, 1), new int[]
			{
				159,
				158
			});
			this.RegisterToNPC(251, ItemDropRule.ExpertGetsRerolls(1311, 15, 1));
			this.RegisterToNPC(251, ItemDropRule.Common(5239, 15, 1, 1));
			this.RegisterToNPC(251, ItemDropRule.Common(5236, 15, 1, 1));
			this.RegisterToNPC(477, ItemDropRule.Common(5237, 15, 1, 1));
			this.RegisterToNPC(253, ItemDropRule.Common(5223, 60, 1, 1));
			this.RegisterToNPC(460, ItemDropRule.Common(5227, 60, 1, 1));
			this.RegisterToNPC(469, ItemDropRule.Common(5260, 60, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.Common(5261, 450, 1, 1), new int[]
			{
				166,
				162
			});
			this.RegisterToNPC(462, ItemDropRule.Common(5262, 60, 1, 1));
			Conditions.DownedAllMechBosses condition = new Conditions.DownedAllMechBosses();
			Conditions.DownedPlantera condition2 = new Conditions.DownedPlantera();
			IItemDropRule rule = this.RegisterToNPC(477, new LeadingConditionRule(condition));
			IItemDropRule rule2 = rule.OnSuccess(new LeadingConditionRule(condition2), false);
			rule.OnSuccess(ItemDropRule.ExpertGetsRerolls(1570, 4, 1), false);
			rule2.OnSuccess(ItemDropRule.ExpertGetsRerolls(2770, 20, 1), false);
			rule2.OnSuccess(ItemDropRule.ExpertGetsRerolls(3292, 3, 1), false);
			this.RegisterToNPC(253, new LeadingConditionRule(condition)).OnSuccess(ItemDropRule.ExpertGetsRerolls(1327, 40, 1), false);
			this.RegisterToNPC(460, new LeadingConditionRule(condition2)).OnSuccess(ItemDropRule.ExpertGetsRerolls(3098, 40, 1), false);
			this.RegisterToNPC(460, ItemDropRule.ExpertGetsRerolls(4740, 50, 1));
			this.RegisterToNPC(460, ItemDropRule.ExpertGetsRerolls(4741, 50, 1));
			this.RegisterToNPC(460, ItemDropRule.ExpertGetsRerolls(4742, 50, 1));
			this.RegisterToNPC(468, new LeadingConditionRule(condition2)).OnSuccess(ItemDropRule.ExpertGetsRerolls(3105, 40, 1), false);
			this.RegisterToNPC(468, ItemDropRule.ExpertGetsRerolls(4738, 50, 1));
			this.RegisterToNPC(468, ItemDropRule.ExpertGetsRerolls(4739, 50, 1));
			this.RegisterToNPC(466, new LeadingConditionRule(condition2)).OnSuccess(ItemDropRule.ExpertGetsRerolls(3106, 40, 1), false);
			this.RegisterToNPC(467, new LeadingConditionRule(condition2)).OnSuccess(ItemDropRule.ExpertGetsRerolls(3249, 30, 1), false);
			IItemDropRule itemDropRule = ItemDropRule.Common(3107, 25, 1, 1);
			IItemDropRule itemDropRule2 = ItemDropRule.WithRerolls(3107, 1, 25, 1, 1);
			itemDropRule.OnSuccess(ItemDropRule.Common(3108, 1, 100, 200), true);
			itemDropRule2.OnSuccess(ItemDropRule.Common(3108, 1, 100, 200), true);
			this.RegisterToNPC(463, new LeadingConditionRule(condition2)).OnSuccess(new DropBasedOnExpertMode(itemDropRule, itemDropRule2), false);
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x00560B5C File Offset: 0x0055ED5C
		private void RegisterMimic()
		{
			this.RegisterToNPC(85, new LeadingConditionRule(new Conditions.NotRemixSeedHardmode())).OnSuccess(ItemDropRule.OneFromOptions(1, new int[]
			{
				437,
				517,
				535,
				536,
				532,
				554
			}), false);
			this.RegisterToNPC(85, new LeadingConditionRule(new Conditions.RemixSeedHardmode())).OnSuccess(ItemDropRule.OneFromOptions(1, new int[]
			{
				437,
				3069,
				535,
				536,
				532,
				554
			}), false);
			this.RegisterToNPC(85, new LeadingConditionRule(new Conditions.Easymode())).OnSuccess(ItemDropRule.OneFromOptions(1, new int[]
			{
				49,
				50,
				53,
				54,
				5011,
				975
			}), false);
			this.RegisterToNPC(85, new LeadingConditionRule(new Conditions.Easymode())).OnSuccess(ItemDropRule.Common(930, 20, 1, 1), false);
			this.RegisterToNPC(85, new LeadingConditionRule(new Conditions.Easymode())).OnSuccess(ItemDropRule.Common(997, 20, 1, 1), false);
			this.RegisterIceMimic();
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x00560C50 File Offset: 0x0055EE50
		private void RegisterIceMimic()
		{
			IItemDropRule itemDropRule = ItemDropRule.Common(1312, 20, 1, 1);
			itemDropRule.OnFailedRoll(new LeadingConditionRule(new Conditions.NotRemixSeedHardmode()), false).OnSuccess(ItemDropRule.OneFromOptions(1, new int[]
			{
				676,
				725,
				1264
			}), false);
			itemDropRule.OnFailedRoll(new LeadingConditionRule(new Conditions.RemixSeedHardmode()), false).OnSuccess(ItemDropRule.OneFromOptions(1, new int[]
			{
				676,
				1319,
				1264
			}), false);
			itemDropRule.OnFailedRoll(new LeadingConditionRule(new Conditions.RemixSeedEasymode()), false).OnSuccess(ItemDropRule.OneFromOptions(1, this.RegisterIceMimic_GetEasyModeItemPool(true)), false);
			itemDropRule.OnFailedRoll(new LeadingConditionRule(new Conditions.NotRemixSeedEasymode()), false).OnSuccess(ItemDropRule.OneFromOptions(1, this.RegisterIceMimic_GetEasyModeItemPool(false)), false);
			this.RegisterToNPC(629, itemDropRule);
			this.RegisterToNPC(629, new LeadingConditionRule(new Conditions.Easymode())).OnSuccess(ItemDropRule.Common(997, 20, 1, 1), false);
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x00560D4B File Offset: 0x0055EF4B
		private int[] RegisterIceMimic_GetEasyModeItemPool(bool isRemix)
		{
			int[] array = new int[]
			{
				670,
				724,
				950,
				0,
				987,
				1579
			};
			array[3] = (isRemix ? 725 : 1319);
			return array;
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x00560D70 File Offset: 0x0055EF70
		private void RegisterHardmodeDungeonDrops()
		{
			int[] npcNetIds = new int[]
			{
				269,
				270,
				271,
				272,
				273,
				274,
				275,
				276,
				277,
				278,
				279,
				280
			};
			this.RegisterToNPC(290, ItemDropRule.ExpertGetsRerolls(1513, 15, 1));
			this.RegisterToNPC(290, ItemDropRule.ExpertGetsRerolls(938, 10, 1));
			this.RegisterToNPC(287, ItemDropRule.ExpertGetsRerolls(977, 12, 1));
			this.RegisterToNPC(287, ItemDropRule.ExpertGetsRerolls(963, 12, 1));
			this.RegisterToNPC(291, ItemDropRule.ExpertGetsRerolls(1300, 12, 1));
			this.RegisterToNPC(291, ItemDropRule.ExpertGetsRerolls(1254, 12, 1));
			this.RegisterToNPC(292, ItemDropRule.ExpertGetsRerolls(1514, 12, 1));
			this.RegisterToNPC(292, ItemDropRule.ExpertGetsRerolls(679, 12, 1));
			this.RegisterToNPC(293, ItemDropRule.ExpertGetsRerolls(759, 18, 1));
			this.RegisterToNPC(289, ItemDropRule.ExpertGetsRerolls(4789, 25, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.ExpertGetsRerolls(1446, 20, 1), new int[]
			{
				281,
				282
			});
			this.RegisterToMultipleNPCs(ItemDropRule.ExpertGetsRerolls(1444, 20, 1), new int[]
			{
				283,
				284
			});
			this.RegisterToMultipleNPCs(ItemDropRule.ExpertGetsRerolls(1445, 20, 1), new int[]
			{
				285,
				286
			});
			this.RegisterToMultipleNPCs(ItemDropRule.ExpertGetsRerolls(1183, 400, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.ExpertGetsRerolls(1266, 300, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.ExpertGetsRerolls(671, 200, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.ExpertGetsRerolls(4679, 200, 1), npcNetIds);
			this.RegisterToNPC(288, ItemDropRule.Common(1508, 1, 1, 2));
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x00560F84 File Offset: 0x0055F184
		private void RegisterBosses()
		{
			this.RegisterBoss_EOC();
			this.RegisterBoss_BOC();
			this.RegisterBoss_EOW();
			this.RegisterBoss_QueenBee();
			this.RegisterBoss_Skeletron();
			this.RegisterBoss_WOF();
			this.RegisterBoss_AncientCultist();
			this.RegisterBoss_MoonLord();
			this.RegisterBoss_LunarTowers();
			this.RegisterBoss_Betsy();
			this.RegisterBoss_Golem();
			this.RegisterBoss_DukeFishron();
			this.RegisterBoss_SkeletronPrime();
			this.RegisterBoss_TheDestroyer();
			this.RegisterBoss_Twins();
			this.RegisterBoss_Plantera();
			this.RegisterBoss_KingSlime();
			this.RegisterBoss_FrostMoon();
			this.RegisterBoss_PumpkinMoon();
			this.RegisterBoss_HallowBoss();
			this.RegisterBoss_QueenSlime();
			this.RegisterBoss_Deerclops();
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x00561018 File Offset: 0x0055F218
		private void RegisterBoss_QueenSlime()
		{
			short type = 657;
			this.RegisterToNPC((int)type, ItemDropRule.BossBag(4957));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(4950));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeDropOnAllPlayers(4960, this._masterModeDropRng));
			LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.NotExpert());
			this.RegisterToNPC((int)type, leadingConditionRule);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(4986, 1, 25, 75), false);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(4959, 7, 1, 1), false);
			leadingConditionRule.OnSuccess(ItemDropRule.OneFromOptions(1, new int[]
			{
				4982,
				4983,
				4984
			}), false);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(4758, 4, 1, 1), false);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(4981, 4, 1, 1), false);
			leadingConditionRule.OnSuccess(ItemDropRule.NotScalingWithLuck(4980, 3, 1, 1), false);
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x00561108 File Offset: 0x0055F308
		private void RegisterBoss_HallowBoss()
		{
			short type = 636;
			this.RegisterToNPC((int)type, ItemDropRule.BossBag(4782));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(4949));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeDropOnAllPlayers(4811, this._masterModeDropRng));
			LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.NotExpert());
			this.RegisterToNPC((int)type, leadingConditionRule).OnSuccess(ItemDropRule.OneFromOptions(1, new int[]
			{
				4923,
				4952,
				4953,
				4914
			}), false);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(4823, 15, 1, 1), false);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(4778, 4, 3, 3), false);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(4715, 50, 1, 1), false);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(4784, 7, 1, 1), false);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(5075, 20, 1, 1), false);
			LeadingConditionRule entry = new LeadingConditionRule(new Conditions.EmpressOfLightIsGenuinelyEnraged());
			this.RegisterToNPC((int)type, entry).OnSuccess(ItemDropRule.Common(5005, 1, 1, 1), false);
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x0056121C File Offset: 0x0055F41C
		private void RegisterBoss_PumpkinMoon()
		{
			Conditions.PumpkinMoonDropGatingChance condition = new Conditions.PumpkinMoonDropGatingChance();
			Conditions.PumpkinMoonDropGateForTrophies condition2 = new Conditions.PumpkinMoonDropGateForTrophies();
			new Conditions.IsPumpkinMoon();
			new Conditions.FromCertainWaveAndAbove(15);
			this.RegisterToNPC(315, ItemDropRule.ByCondition(condition, 1857, 20, 1, 1, 1));
			int[] npcNetIds = new int[]
			{
				305,
				306,
				307,
				308,
				309,
				310,
				311,
				312,
				313,
				314
			};
			this.RegisterToMultipleNPCs(new LeadingConditionRule(condition), npcNetIds).OnSuccess(ItemDropRule.OneFromOptions(10, new int[]
			{
				1788,
				1789,
				1790
			}), false);
			IItemDropRule rule = this.RegisterToNPC(325, new LeadingConditionRule(condition));
			IItemDropRule itemDropRule = ItemDropRule.Common(1835, 1, 1, 1);
			itemDropRule.OnSuccess(ItemDropRule.Common(1836, 1, 30, 60), true);
			rule.OnSuccess(new OneFromRulesRule(1, new IItemDropRule[]
			{
				ItemDropRule.Common(1829, 1, 1, 1),
				ItemDropRule.Common(1831, 1, 1, 1),
				itemDropRule,
				ItemDropRule.Common(1837, 1, 1, 1),
				ItemDropRule.Common(1845, 1, 1, 1)
			}), false);
			rule.OnSuccess(ItemDropRule.ByCondition(condition2, 1855, 1, 1, 1, 1), false);
			rule.OnSuccess(ItemDropRule.ByCondition(new Conditions.IsExpert(), 4444, 5, 1, 1, 1), false);
			rule.OnSuccess(ItemDropRule.MasterModeCommonDrop(4941), false);
			rule.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(4793, this._masterModeDropRng), false);
			IItemDropRule itemDropRule2 = ItemDropRule.Common(1782, 1, 1, 1);
			itemDropRule2.OnSuccess(ItemDropRule.Common(1783, 1, 50, 100), true);
			IItemDropRule itemDropRule3 = ItemDropRule.Common(1784, 1, 1, 1);
			itemDropRule3.OnSuccess(ItemDropRule.Common(1785, 1, 25, 50), true);
			IItemDropRule rule2 = this.RegisterToNPC(327, new LeadingConditionRule(condition));
			rule2.OnSuccess(new OneFromRulesRule(1, new IItemDropRule[]
			{
				itemDropRule2,
				itemDropRule3,
				ItemDropRule.Common(1811, 1, 1, 1),
				ItemDropRule.Common(1826, 1, 1, 1),
				ItemDropRule.Common(1801, 1, 1, 1),
				ItemDropRule.Common(1802, 1, 1, 1),
				ItemDropRule.Common(4680, 1, 1, 1),
				ItemDropRule.Common(1798, 1, 1, 1)
			}), false);
			rule2.OnSuccess(ItemDropRule.ByCondition(condition2, 1856, 1, 1, 1, 1), false);
			rule2.OnSuccess(ItemDropRule.MasterModeCommonDrop(4942), false);
			rule2.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(4812, this._masterModeDropRng), false);
			this.RegisterToNPC(326, new DropBasedOnMasterAndExpertMode(new CommonDrop(1729, 1, 1, 3, 1), new CommonDrop(1729, 1, 1, 4, 1), new CommonDrop(1729, 1, 2, 4, 1)));
			this.RegisterToNPC(325, new DropBasedOnMasterAndExpertMode(new CommonDrop(1729, 1, 15, 30, 1), new CommonDrop(1729, 1, 25, 40, 1), new CommonDrop(1729, 1, 30, 50, 1)));
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x0056152C File Offset: 0x0055F72C
		private void RegisterBoss_FrostMoon()
		{
			Conditions.FrostMoonDropGatingChance condition = new Conditions.FrostMoonDropGatingChance();
			Conditions.FrostMoonDropGateForTrophies condition2 = new Conditions.FrostMoonDropGateForTrophies();
			Conditions.FromCertainWaveAndAbove condition3 = new Conditions.FromCertainWaveAndAbove(15);
			IItemDropRule rule = this.RegisterToNPC(344, new LeadingConditionRule(condition));
			rule.OnSuccess(ItemDropRule.ByCondition(condition2, 1962, 1, 1, 1, 1), false);
			rule.OnSuccess(ItemDropRule.Common(1871, 15, 1, 1), false).OnFailedRoll(ItemDropRule.OneFromOptions(1, new int[]
			{
				1916,
				1928,
				1930
			}), false);
			rule.OnSuccess(ItemDropRule.MasterModeCommonDrop(4944), false);
			rule.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(4813, this._masterModeDropRng), false);
			IItemDropRule rule2 = this.RegisterToNPC(345, new LeadingConditionRule(condition));
			rule2.OnSuccess(ItemDropRule.ByCondition(condition2, 1960, 1, 1, 1, 1), false);
			rule2.OnSuccess(ItemDropRule.ByCondition(condition3, 1914, 15, 1, 1, 1), false);
			rule2.OnSuccess(ItemDropRule.Common(1959, 15, 1, 1), false).OnFailedRoll(ItemDropRule.OneFromOptions(1, new int[]
			{
				1931,
				1946,
				1947
			}), false);
			rule2.OnSuccess(ItemDropRule.MasterModeCommonDrop(4943), false);
			rule2.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(4814, this._masterModeDropRng), false);
			IItemDropRule rule3 = this.RegisterToNPC(346, new LeadingConditionRule(condition));
			rule3.OnSuccess(ItemDropRule.ByCondition(condition2, 1961, 1, 1, 1, 1), false);
			rule3.OnSuccess(ItemDropRule.OneFromOptions(1, new int[]
			{
				1910,
				1929
			}), false);
			rule3.OnSuccess(ItemDropRule.MasterModeCommonDrop(4945), false);
			rule3.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(4794, this._masterModeDropRng), false);
			int[] npcNetIds = new int[]
			{
				338,
				339,
				340
			};
			this.RegisterToMultipleNPCs(ItemDropRule.OneFromOptions(200, new int[]
			{
				1943,
				1944,
				1945
			}), npcNetIds);
			this.RegisterToNPC(341, ItemDropRule.ByCondition(new Conditions.IsChristmas(), 1869, 1, 1, 1, 1));
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x0056174C File Offset: 0x0055F94C
		private void RegisterBoss_KingSlime()
		{
			short type = 50;
			this.RegisterToNPC((int)type, ItemDropRule.BossBag(3318));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(4929));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeDropOnAllPlayers(4797, this._masterModeDropRng));
			LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.NotExpert());
			this.RegisterToNPC((int)type, leadingConditionRule);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(2430, 4, 1, 1), false);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(2493, 7, 1, 1), false);
			leadingConditionRule.OnSuccess(ItemDropRule.OneFromOptions(1, new int[]
			{
				256,
				257,
				258
			}), false);
			leadingConditionRule.OnSuccess(ItemDropRule.NotScalingWithLuck(2585, 3, 1, 1), false).OnFailedRoll(ItemDropRule.Common(2610, 1, 1, 1), false);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(998, 1, 1, 1), false);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(1309, 30, 1, 1), false);
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x00561848 File Offset: 0x0055FA48
		private void RegisterBoss_Plantera()
		{
			short type = 262;
			this.RegisterToNPC((int)type, ItemDropRule.BossBag(3328));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(4934));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeDropOnAllPlayers(4806, this._masterModeDropRng));
			LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.NotExpert());
			this.RegisterToNPC((int)type, leadingConditionRule);
			LeadingConditionRule leadingConditionRule2 = new LeadingConditionRule(new Conditions.FirstTimeKillingPlantera());
			leadingConditionRule.OnSuccess(leadingConditionRule2, false);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(2109, 7, 1, 1), false);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(1141, 1, 1, 1), false);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(1182, 20, 1, 1), false);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(1305, 50, 1, 1), false);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(1157, 4, 1, 1), false);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(3021, 10, 1, 1), false);
			IItemDropRule itemDropRule = ItemDropRule.Common(758, 1, 1, 1);
			itemDropRule.OnSuccess(ItemDropRule.Common(771, 1, 50, 150), true);
			leadingConditionRule2.OnSuccess(itemDropRule, true);
			leadingConditionRule2.OnFailedConditions(new OneFromRulesRule(1, new IItemDropRule[]
			{
				itemDropRule,
				ItemDropRule.Common(1255, 1, 1, 1),
				ItemDropRule.Common(788, 1, 1, 1),
				ItemDropRule.Common(1178, 1, 1, 1),
				ItemDropRule.Common(1259, 1, 1, 1),
				ItemDropRule.Common(1155, 1, 1, 1),
				ItemDropRule.Common(3018, 1, 1, 1),
				ItemDropRule.Common(5477, 1, 1, 1)
			}), false);
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x005619FC File Offset: 0x0055FBFC
		private void RegisterBoss_SkeletronPrime()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 127;
			this.RegisterToNPC((int)type, ItemDropRule.BossBag(3327));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(4933));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeDropOnAllPlayers(4805, this._masterModeDropRng));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 2107, 7, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 1225, 1, 15, 30, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 547, 1, 25, 40, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(new Conditions.MechdusaKill(), 5382, 1, 1, 1, 1));
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x00561AB4 File Offset: 0x0055FCB4
		private void RegisterBoss_TheDestroyer()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 134;
			this.RegisterToNPC((int)type, ItemDropRule.BossBag(3325));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(4932));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeDropOnAllPlayers(4803, this._masterModeDropRng));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 2113, 7, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 1225, 1, 15, 30, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 548, 1, 25, 40, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(new Conditions.MechdusaKill(), 5382, 1, 1, 1, 1));
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x00561B70 File Offset: 0x0055FD70
		private void RegisterBoss_Twins()
		{
			LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.MissingTwin());
			LeadingConditionRule leadingConditionRule2 = new LeadingConditionRule(new Conditions.NotExpert());
			leadingConditionRule.OnSuccess(ItemDropRule.BossBag(3326), false);
			leadingConditionRule.OnSuccess(leadingConditionRule2, false);
			leadingConditionRule2.OnSuccess(ItemDropRule.Common(2106, 7, 1, 1), false);
			leadingConditionRule2.OnSuccess(ItemDropRule.Common(1225, 1, 15, 30), false);
			leadingConditionRule2.OnSuccess(ItemDropRule.Common(549, 1, 25, 40), false);
			leadingConditionRule.OnSuccess(ItemDropRule.MasterModeCommonDrop(4931), false);
			leadingConditionRule.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(4804, this._masterModeDropRng), false);
			this.RegisterToMultipleNPCs(leadingConditionRule, new int[]
			{
				126,
				125
			});
			this.RegisterToMultipleNPCs(ItemDropRule.ByCondition(new Conditions.MechdusaKill(), 5382, 1, 1, 1, 1), new int[]
			{
				126,
				125
			});
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x00561C60 File Offset: 0x0055FE60
		private void RegisterBoss_EOC()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			Conditions.IsCrimsonAndNotExpert condition2 = new Conditions.IsCrimsonAndNotExpert();
			Conditions.IsCorruptionAndNotExpert condition3 = new Conditions.IsCorruptionAndNotExpert();
			short type = 4;
			this.RegisterToNPC((int)type, ItemDropRule.BossBag(3319));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(4924));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(3763));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeDropOnAllPlayers(4798, this._masterModeDropRng));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 2112, 7, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 1299, 40, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 47, 1, 20, 50, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition2, 880, 1, 30, 90, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition2, 2171, 1, 1, 3, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition3, 56, 1, 30, 90, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition3, 59, 1, 1, 3, 1));
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x00561D70 File Offset: 0x0055FF70
		private void RegisterBoss_BOC()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 266;
			this.RegisterToNPC((int)type, ItemDropRule.BossBag(3321));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(4926));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeDropOnAllPlayers(4800, this._masterModeDropRng));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 880, 1, 40, 90, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 2104, 7, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 3060, 20, 1, 1, 1));
			short type2 = 267;
			this.RegisterToNPC((int)type2, new DropBasedOnMasterAndExpertMode(new CommonDrop(1329, 3, 2, 5, 2), new CommonDrop(1329, 3, 1, 3, 2), new CommonDrop(1329, 4, 1, 2, 2)));
			this.RegisterToNPC((int)type2, new DropBasedOnMasterAndExpertMode(new CommonDrop(880, 3, 5, 12, 2), new CommonDrop(880, 3, 5, 7, 2), new CommonDrop(880, 3, 2, 4, 2)));
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x00561E84 File Offset: 0x00560084
		private void RegisterBoss_EOW()
		{
			Conditions.LegacyHack_IsBossAndExpert condition = new Conditions.LegacyHack_IsBossAndExpert();
			Conditions.LegacyHack_IsBossAndNotExpert condition2 = new Conditions.LegacyHack_IsBossAndNotExpert();
			int[] npcNetIds = new int[]
			{
				13,
				14,
				15
			};
			this.RegisterToMultipleNPCs(new DropBasedOnMasterAndExpertMode(ItemDropRule.Common(86, 2, 1, 2), ItemDropRule.Common(86, 5, 1, 2), ItemDropRule.Common(86, 10, 1, 2)), npcNetIds);
			this.RegisterToMultipleNPCs(new DropBasedOnMasterAndExpertMode(ItemDropRule.Common(56, 2, 2, 5), ItemDropRule.Common(56, 2, 1, 3), ItemDropRule.Common(56, 3, 1, 2)), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.BossBagByCondition(condition, 3320), npcNetIds);
			IItemDropRule rule = this.RegisterToMultipleNPCs(new LeadingConditionRule(new Conditions.LegacyHack_IsABoss()), npcNetIds);
			rule.OnSuccess(ItemDropRule.MasterModeCommonDrop(4925), false);
			rule.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(4799, this._masterModeDropRng), false);
			this.RegisterToMultipleNPCs(ItemDropRule.ByCondition(condition2, 56, 1, 20, 60, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.ByCondition(condition2, 994, 20, 1, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.ByCondition(condition2, 2111, 7, 1, 1, 1), npcNetIds);
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x00561F98 File Offset: 0x00560198
		private void RegisterBoss_Deerclops()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 668;
			this.RegisterToNPC((int)type, ItemDropRule.BossBag(5111));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(5110));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeDropOnAllPlayers(5090, this._masterModeDropRng));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 5109, 7, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 5098, 3, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 5101, 3, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 5113, 3, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 5385, 14, 1, 1, 1));
			this.RegisterToNPC((int)type, new LeadingConditionRule(condition)).OnSuccess(new OneFromRulesRule(1, new IItemDropRule[]
			{
				ItemDropRule.OneFromOptionsNotScalingWithLuck(1, new int[]
				{
					5117,
					5118,
					5119,
					5095
				})
			}), false);
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x0056209C File Offset: 0x0056029C
		private void RegisterBoss_QueenBee()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 222;
			this.RegisterToNPC((int)type, ItemDropRule.BossBag(3322));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(4928));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeDropOnAllPlayers(4802, this._masterModeDropRng));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 2108, 7, 1, 1, 1));
			this.RegisterToNPC((int)type, new DropBasedOnExpertMode(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, new int[]
			{
				1121,
				1123,
				2888
			}), ItemDropRule.DropNothing()));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 1132, 3, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 1170, 15, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 2502, 20, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 5483, 15, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 1129, 3, 1, 1, 1)).OnFailedRoll(ItemDropRule.OneFromOptionsNotScalingWithLuck(2, new int[]
			{
				842,
				843,
				844
			}), false);
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 1130, 4, 10, 30, 3));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 2431, 1, 17, 30, 1));
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x005621F8 File Offset: 0x005603F8
		private void RegisterBoss_Skeletron()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			Conditions.RedHatSkeletron condition2 = new Conditions.RedHatSkeletron();
			short type = 35;
			this.RegisterToNPC((int)type, ItemDropRule.BossBag(3323));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(4927));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeDropOnAllPlayers(4801, this._masterModeDropRng));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 1281, 7, 1, 1, 1)).OnFailedRoll(ItemDropRule.Common(1273, 7, 1, 1), false).OnFailedRoll(ItemDropRule.Common(1313, 7, 1, 1), false);
			this.RegisterToNPC((int)type, ItemDropRule.Common(4993, 7, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition2, 5624, 1, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition2, 5625, 1, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition2, 5626, 1, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition2, 5737, 1, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition2, 5628, 1, 1, 1, 1));
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x00562318 File Offset: 0x00560518
		private void RegisterBoss_WOF()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 113;
			this.RegisterToNPC((int)type, ItemDropRule.BossBag(3324));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(4930));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeDropOnAllPlayers(4795, this._masterModeDropRng));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 2105, 7, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 367, 1, 1, 1, 1));
			this.RegisterToNPC((int)type, new LeadingConditionRule(condition)).OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, new int[]
			{
				490,
				491,
				489,
				2998
			}), false);
			this.RegisterToNPC((int)type, new LeadingConditionRule(condition)).OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, new int[]
			{
				426,
				434,
				514,
				4912
			}), false);
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x005623F0 File Offset: 0x005605F0
		private void RegisterBoss_AncientCultist()
		{
			short type = 439;
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(4937));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeDropOnAllPlayers(4809, this._masterModeDropRng));
			this.RegisterToNPC((int)type, ItemDropRule.Common(3372, 7, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.Common(3549, 1, 1, 1));
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x00562458 File Offset: 0x00560658
		private void RegisterBoss_MoonLord()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 398;
			this.RegisterToNPC((int)type, ItemDropRule.BossBag(3332));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(4938));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeDropOnAllPlayers(4810, this._masterModeDropRng));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 3373, 7, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 4469, 10, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 3384, 1, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 3460, 1, 70, 90, 1));
			this.RegisterToNPC((int)type, new LeadingConditionRule(condition)).OnSuccess(new FromOptionsWithoutRepeatsDropRule(2, new int[]
			{
				3063,
				3389,
				3065,
				1553,
				3930,
				3541,
				3570,
				3571,
				3569,
				5480
			}), false);
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x00562538 File Offset: 0x00560738
		private void RegisterBoss_LunarTowers()
		{
			DropOneByOne.Parameters parameters = new DropOneByOne.Parameters
			{
				MinimumItemDropsCount = 12,
				MaximumItemDropsCount = 20,
				ChanceNumerator = 1,
				ChanceDenominator = 1,
				MinimumStackPerChunkBase = 1,
				MaximumStackPerChunkBase = 3,
				BonusMinDropsPerChunkPerPlayer = 0,
				BonusMaxDropsPerChunkPerPlayer = 0
			};
			DropOneByOne.Parameters parameters2 = parameters;
			parameters2.BonusMinDropsPerChunkPerPlayer = 1;
			parameters2.BonusMaxDropsPerChunkPerPlayer = 1;
			parameters2.MinimumStackPerChunkBase = (int)((float)parameters.MinimumStackPerChunkBase * 1.5f);
			parameters2.MaximumStackPerChunkBase = (int)((float)parameters.MaximumStackPerChunkBase * 1.5f);
			this.RegisterToNPC(517, new DropBasedOnExpertMode(new DropOneByOne(3458, parameters), new DropOneByOne(3458, parameters2)));
			this.RegisterToNPC(422, new DropBasedOnExpertMode(new DropOneByOne(3456, parameters), new DropOneByOne(3456, parameters2)));
			this.RegisterToNPC(507, new DropBasedOnExpertMode(new DropOneByOne(3457, parameters), new DropOneByOne(3457, parameters2)));
			this.RegisterToNPC(493, new DropBasedOnExpertMode(new DropOneByOne(3459, parameters), new DropOneByOne(3459, parameters2)));
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x0056266C File Offset: 0x0056086C
		private void RegisterBoss_Betsy()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 551;
			this.RegisterToNPC((int)type, ItemDropRule.BossBag(3860));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(4948));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeDropOnAllPlayers(4817, this._masterModeDropRng));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 3863, 7, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 3883, 4, 1, 1, 1));
			this.RegisterToNPC((int)type, new LeadingConditionRule(condition)).OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, new int[]
			{
				3827,
				3859,
				3870,
				3858
			}), false);
		}

		// Token: 0x0600272B RID: 10027 RVA: 0x0056271C File Offset: 0x0056091C
		private void RegisterBoss_Golem()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 245;
			this.RegisterToNPC((int)type, ItemDropRule.BossBag(3329));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(4935));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeDropOnAllPlayers(4807, this._masterModeDropRng));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 2110, 7, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 1294, 4, 1, 1, 1));
			IItemDropRule itemDropRule = ItemDropRule.Common(1258, 1, 1, 1);
			itemDropRule.OnSuccess(ItemDropRule.Common(1261, 1, 60, 180), true);
			this.RegisterToNPC((int)type, new LeadingConditionRule(condition)).OnSuccess(new OneFromRulesRule(1, new IItemDropRule[]
			{
				itemDropRule,
				ItemDropRule.Common(1122, 1, 1, 1),
				ItemDropRule.Common(899, 1, 1, 1),
				ItemDropRule.Common(1248, 1, 1, 1),
				ItemDropRule.Common(1295, 1, 1, 1),
				ItemDropRule.Common(1296, 1, 1, 1),
				ItemDropRule.Common(1297, 1, 1, 1)
			}), false);
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 2218, 1, 4, 8, 1));
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x00562864 File Offset: 0x00560A64
		private void RegisterBoss_DukeFishron()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 370;
			this.RegisterToNPC((int)type, ItemDropRule.BossBag(3330));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeCommonDrop(4936));
			this.RegisterToNPC((int)type, ItemDropRule.MasterModeDropOnAllPlayers(4808, this._masterModeDropRng));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 2588, 7, 1, 1, 1));
			this.RegisterToNPC((int)type, ItemDropRule.ByCondition(condition, 2609, 15, 1, 1, 1));
			this.RegisterToNPC((int)type, new LeadingConditionRule(new Conditions.NotRemixSeedHardmode())).OnSuccess(new LeadingConditionRule(condition), false).OnSuccess(ItemDropRule.OneFromOptions(1, new int[]
			{
				5526,
				2624,
				2622,
				2621,
				5478,
				2623
			}), false);
			this.RegisterToNPC((int)type, new LeadingConditionRule(new Conditions.RemixSeed())).OnSuccess(new LeadingConditionRule(condition), false).OnSuccess(ItemDropRule.OneFromOptions(1, new int[]
			{
				5526,
				2624,
				2622,
				2621,
				5478,
				157
			}), false);
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x00562960 File Offset: 0x00560B60
		private void RegisterWeirdRules()
		{
			Conditions.BeatAnyMechBoss condition = new Conditions.BeatAnyMechBoss();
			this.RegisterToMultipleNPCs(ItemDropRule.NormalvsExpert(3260, 40, 30), new int[]
			{
				86
			});
			this.RegisterToNPC(75, ItemDropRule.ByCondition(condition, 5662, 200, 1, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.NormalvsExpert(5488, 200, 150), new int[]
			{
				171,
				475,
				84,
				137,
				138,
				527,
				120
			});
			this.RegisterToMultipleNPCs(ItemDropRule.NormalvsExpert(5489, 200, 150), new int[]
			{
				170,
				180,
				473,
				474,
				83,
				179,
				101,
				98,
				94,
				182,
				268,
				525,
				526,
				529,
				533
			});
		}

		// Token: 0x0600272E RID: 10030 RVA: 0x00562A08 File Offset: 0x00560C08
		private void RegisterGlobalRules()
		{
			this.RegisterToGlobal(new MechBossSpawnersDropRule());
			this.RegisterToGlobal(new SlimeBodyItemDropRule());
			this.RegisterToGlobal(ItemDropRule.ByCondition(new Conditions.HalloweenWeapons(), 1825, 2000, 1, 1, 1)).OnFailedRoll(ItemDropRule.Common(1827, 2000, 1, 1), false);
			this.RegisterToGlobal(new ItemDropWithConditionRule(1533, 2500, 1, 1, new Conditions.JungleKeyCondition(), 1));
			this.RegisterToGlobal(new ItemDropWithConditionRule(1534, 2500, 1, 1, new Conditions.CorruptKeyCondition(), 1));
			this.RegisterToGlobal(new ItemDropWithConditionRule(1535, 2500, 1, 1, new Conditions.CrimsonKeyCondition(), 1));
			this.RegisterToGlobal(new ItemDropWithConditionRule(1536, 2500, 1, 1, new Conditions.HallowKeyCondition(), 1));
			this.RegisterToGlobal(new ItemDropWithConditionRule(1537, 2500, 1, 1, new Conditions.FrozenKeyCondition(), 1));
			this.RegisterToGlobal(new ItemDropWithConditionRule(4714, 2500, 1, 1, new Conditions.DesertKeyCondition(), 1));
			this.RegisterToGlobal(new ItemDropWithConditionRule(1774, 80, 1, 1, new Conditions.HalloweenGoodieBagDrop(), 1));
			this.RegisterToGlobal(new ItemDropWithConditionRule(1869, 13, 1, 1, new Conditions.XmasPresentDrop(), 1));
			this.RegisterToGlobal(new ItemDropWithConditionRule(2701, 50, 20, 50, new Conditions.LivingFlames(), 1));
			this.RegisterToGlobal(new ItemDropWithConditionRule(520, 5, 1, 1, new Conditions.SoulOfLight(), 1));
			this.RegisterToGlobal(new ItemDropWithConditionRule(521, 5, 1, 1, new Conditions.SoulOfNight(), 1));
			this.RegisterToGlobal(ItemDropRule.ByCondition(new Conditions.PirateMap(), 1315, 100, 1, 1, 1));
		}

		// Token: 0x0600272F RID: 10031 RVA: 0x00562BB8 File Offset: 0x00560DB8
		private void RegisterFoodDrops()
		{
			this.RegisterToNPC(48, ItemDropRule.Food(4016, 50, 1, 1));
			this.RegisterToNPC(224, ItemDropRule.Food(4021, 50, 1, 1));
			this.RegisterToNPC(44, ItemDropRule.Food(4037, 10, 1, 1));
			this.RegisterToNPC(469, ItemDropRule.Food(4037, 100, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.Food(4020, 30, 1, 1), new int[]
			{
				163,
				238,
				164,
				165,
				530,
				531
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Food(4029, 50, 1, 1), new int[]
			{
				480,
				481
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Food(4030, 75, 1, 1), new int[]
			{
				498,
				499,
				500,
				501,
				502,
				503,
				504,
				505,
				506,
				496,
				497,
				494,
				495
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Food(4036, 50, 1, 1), new int[]
			{
				482,
				483
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Food(4015, 100, 1, 1), new int[]
			{
				6,
				173
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Food(4026, 150, 1, 1), new int[]
			{
				150,
				147,
				184
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Food(4027, 75, 1, 1), new int[]
			{
				154,
				206
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Food(3532, 15, 1, 1), new int[]
			{
				170,
				180,
				171
			});
			this.RegisterToNPC(289, ItemDropRule.Food(4018, 35, 1, 1));
			this.RegisterToNPC(34, ItemDropRule.Food(4018, 70, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.Food(4013, 21, 1, 1), new int[]
			{
				293,
				291,
				292
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Food(5042, 30, 1, 1), new int[]
			{
				43,
				175,
				56
			});
			this.RegisterToNPC(287, ItemDropRule.Food(5042, 10, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.Food(5041, 150, 1, 1), new int[]
			{
				21,
				201,
				202,
				203,
				322,
				323,
				324,
				635,
				449,
				450,
				451,
				452
			});
			this.RegisterToNPC(290, ItemDropRule.Food(4013, 7, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.Food(4025, 30, 1, 1), new int[]
			{
				39,
				156
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Food(4023, 40, 1, 1), new int[]
			{
				177,
				152
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Food(4012, 50, 1, 1), new int[]
			{
				581,
				509,
				580,
				508,
				69
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Food(4028, 30, 1, 1), new int[]
			{
				546,
				542,
				544,
				543,
				545
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Food(4035, 50, 1, 1), new int[]
			{
				67,
				65,
				692
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Food(4011, 150, 1, 1), new int[]
			{
				120,
				137,
				138
			});
			this.RegisterToNPC(122, ItemDropRule.Food(4017, 75, 1, 1));
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x00562F4C File Offset: 0x0056114C
		private void RegisterTownNPCDrops()
		{
			this.RegisterToNPC(22, new ItemDropWithConditionRule(867, 1, 1, 1, new Conditions.NamedNPC("GuideNames.Andrew"), 1));
			this.RegisterToNPC(178, new ItemDropWithConditionRule(4372, 1, 1, 1, new Conditions.NamedNPC("SteampunkerNames.Whitney"), 1));
			this.RegisterToNPC(227, new ItemDropWithConditionRule(5290, 1, 1, 1, new Conditions.NamedNPC("PainterNames.Jim"), 1));
			this.RegisterToNPC(353, ItemDropRule.Common(3352, 8, 1, 1));
			this.RegisterToNPC(441, ItemDropRule.Common(3351, 8, 1, 1));
			this.RegisterToNPC(227, ItemDropRule.Common(3350, 8, 1, 1));
			this.RegisterToNPC(550, ItemDropRule.Common(3821, 8, 1, 1));
			this.RegisterToNPC(208, ItemDropRule.Common(3548, 4, 30, 60));
			this.RegisterToNPC(207, ItemDropRule.Common(3349, 8, 1, 1));
			this.RegisterToNPC(124, ItemDropRule.Common(4818, 8, 1, 1));
			this.RegisterToNPC(663, ItemDropRule.ByCondition(new Conditions.IsHardmode(), 5065, 8, 1, 1, 1));
			this.RegisterToNPC(54, ItemDropRule.Common(260, 1, 1, 1));
			this.RegisterToNPC(368, ItemDropRule.Common(2222, 1, 1, 1));
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x005630C0 File Offset: 0x005612C0
		private void RegisterDD2EventDrops()
		{
			new Conditions.IsExpert();
			this.RegisterToNPC(576, new DropBasedOnExpertMode(ItemDropRule.NotScalingWithLuck(3814, 4, 1, 1), ItemDropRule.NotScalingWithLuck(3814, 2, 1, 1)));
			this.RegisterToNPC(576, new DropBasedOnExpertMode(ItemDropRule.NotScalingWithLuck(3815, 4, 4, 4), ItemDropRule.NotScalingWithLuck(3815, 2, 4, 4)));
			this.RegisterToNPC(576, new CommonDropNotScalingWithLuck(3865, 7, 1, 1));
			this.RegisterToNPC(576, ItemDropRule.NormalvsExpertOneFromOptionsNotScalingWithLuck(3, 2, new int[]
			{
				3811,
				3812
			}));
			this.RegisterToNPC(576, ItemDropRule.NormalvsExpertOneFromOptionsNotScalingWithLuck(2, 1, new int[]
			{
				3852,
				3854,
				3823,
				3835,
				3836
			}));
			this.RegisterToNPC(576, ItemDropRule.NormalvsExpertNotScalingWithLuck(3856, 5, 4));
			this.RegisterToNPC(577, new DropBasedOnExpertMode(ItemDropRule.NotScalingWithLuck(3814, 8, 1, 1), ItemDropRule.NotScalingWithLuck(3814, 4, 1, 1)));
			this.RegisterToNPC(577, new DropBasedOnExpertMode(ItemDropRule.NotScalingWithLuck(3815, 8, 4, 4), ItemDropRule.NotScalingWithLuck(3815, 4, 4, 4)));
			this.RegisterToNPC(577, new CommonDropNotScalingWithLuck(3865, 14, 1, 1));
			this.RegisterToNPC(577, ItemDropRule.MasterModeCommonDrop(4947));
			this.RegisterToNPC(577, ItemDropRule.MasterModeDropOnAllPlayers(4816, this._masterModeDropRng));
			this.RegisterToNPC(577, ItemDropRule.OneFromOptionsNotScalingWithLuck(6, new int[]
			{
				3811,
				3812
			}));
			this.RegisterToNPC(577, ItemDropRule.OneFromOptionsNotScalingWithLuck(4, new int[]
			{
				3852,
				3854,
				3823,
				3835,
				3836
			}));
			this.RegisterToNPC(577, ItemDropRule.Common(3856, 10, 1, 1));
			this.RegisterToNPC(564, ItemDropRule.Common(3864, 7, 1, 1));
			this.RegisterToNPC(564, ItemDropRule.MasterModeDropOnAllPlayers(4796, this._masterModeDropRng));
			this.RegisterToNPC(564, ItemDropRule.NormalvsExpertOneFromOptionsNotScalingWithLuck(2, 1, new int[]
			{
				3810,
				3809
			}));
			this.RegisterToNPC(564, new DropBasedOnExpertMode(ItemDropRule.NotScalingWithLuck(3814, 2, 1, 1), ItemDropRule.NotScalingWithLuck(3814, 1, 1, 1)));
			this.RegisterToNPC(564, new DropBasedOnExpertMode(ItemDropRule.NotScalingWithLuck(3815, 2, 4, 4), ItemDropRule.NotScalingWithLuck(3815, 1, 4, 4)));
			this.RegisterToNPC(564, ItemDropRule.NormalvsExpertOneFromOptionsNotScalingWithLuck(3, 2, new int[]
			{
				3857,
				3855
			}));
			this.RegisterToNPC(565, ItemDropRule.Common(3864, 14, 1, 1));
			this.RegisterToNPC(565, ItemDropRule.MasterModeCommonDrop(4946));
			this.RegisterToNPC(565, ItemDropRule.MasterModeDropOnAllPlayers(4796, this._masterModeDropRng));
			this.RegisterToNPC(565, ItemDropRule.OneFromOptionsNotScalingWithLuck(6, new int[]
			{
				3810,
				3809
			}));
			this.RegisterToNPC(565, new DropBasedOnExpertMode(ItemDropRule.NotScalingWithLuck(3814, 8, 1, 1), ItemDropRule.NotScalingWithLuck(3814, 4, 1, 1)));
			this.RegisterToNPC(565, new DropBasedOnExpertMode(ItemDropRule.NotScalingWithLuck(3815, 8, 4, 4), ItemDropRule.NotScalingWithLuck(3815, 4, 4, 4)));
			this.RegisterToNPC(565, ItemDropRule.OneFromOptionsNotScalingWithLuck(6, new int[]
			{
				3857,
				3855
			}));
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x00563480 File Offset: 0x00561680
		private void RegisterHardmodeFeathers()
		{
			this.RegisterToNPC(156, ItemDropRule.Common(1518, 50, 1, 1));
			this.RegisterToNPC(243, ItemDropRule.Common(1519, 3, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1517, 300, 1, 1), new int[]
			{
				269,
				270,
				271,
				272,
				273,
				274,
				275,
				276,
				277,
				278,
				279,
				280
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1520, 40, 1, 1), new int[]
			{
				159,
				158
			});
			this.RegisterToNPC(48, ItemDropRule.Common(1516, 150, 1, 1));
			this.RegisterToNPC(176, new ItemDropWithConditionRule(1521, 100, 1, 1, new Conditions.BeatAnyMechBoss(), 1));
			this.RegisterToNPC(205, new ItemDropWithConditionRule(1611, 2, 1, 1, new Conditions.BeatAnyMechBoss(), 1));
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x00563570 File Offset: 0x00561770
		private void RegisterYoyos()
		{
			this.RegisterToGlobal(new ItemDropWithConditionRule(3282, 400, 1, 1, new Conditions.YoyoCascade(), 1));
			this.RegisterToGlobal(new ItemDropWithConditionRule(3289, 300, 1, 1, new Conditions.YoyosAmarok(), 1));
			this.RegisterToGlobal(new ItemDropWithConditionRule(3286, 200, 1, 1, new Conditions.YoyosYelets(), 1));
			this.RegisterToGlobal(new ItemDropWithConditionRule(3291, 400, 1, 1, new Conditions.YoyosKraken(), 1));
			this.RegisterToGlobal(new ItemDropWithConditionRule(3290, 400, 1, 1, new Conditions.YoyosHelFire(), 1));
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x00563614 File Offset: 0x00561814
		private void RegisterStatusImmunityItems()
		{
			this.RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(885, 100), new int[]
			{
				104,
				102,
				269,
				270,
				271,
				272
			});
			this.RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(886, 100), new int[]
			{
				77,
				273,
				274,
				275,
				276
			});
			this.RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(887, 100), new int[]
			{
				141,
				176,
				42,
				231,
				232,
				233,
				234,
				235
			});
			this.RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(888, 100), new int[]
			{
				81,
				79,
				183,
				630
			});
			this.RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(889, 100), new int[]
			{
				78,
				82,
				75
			});
			this.RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(890, 100), new int[]
			{
				103,
				75,
				79,
				630
			});
			this.RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(891, 100), new int[]
			{
				34,
				83,
				84,
				179,
				289
			});
			this.RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(892, 100), new int[]
			{
				94,
				182
			});
			this.RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(893, 100), new int[]
			{
				93,
				109,
				80
			});
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x00563768 File Offset: 0x00561968
		private void RegisterPirateDrops()
		{
			int[] npcNetIds = new int[]
			{
				212,
				213,
				214,
				215
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(905, 4000, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(855, 2000, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(854, 1000, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2584, 1000, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(3033, 500, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(672, 200, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(5460, 200, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1277, 500, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1278, 500, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1279, 500, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1280, 500, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1704, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1705, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1710, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1716, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1720, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2379, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2389, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2405, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2843, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(3885, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2663, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(3904, 150, 80, 130), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(3910, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2238, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2133, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2137, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2143, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2147, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2151, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2155, 300, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(3263, 500, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(3264, 500, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(3265, 500, 1, 1), npcNetIds);
			this.RegisterToNPC(216, ItemDropRule.Common(905, 1000, 1, 1));
			this.RegisterToNPC(216, ItemDropRule.Common(855, 500, 1, 1));
			this.RegisterToNPC(216, ItemDropRule.Common(854, 250, 1, 1));
			this.RegisterToNPC(216, ItemDropRule.Common(2584, 250, 1, 1));
			this.RegisterToNPC(216, ItemDropRule.Common(3033, 125, 1, 1));
			this.RegisterToNPC(216, ItemDropRule.Common(672, 50, 1, 1));
			this.RegisterToNPC(216, ItemDropRule.Common(5460, 50, 1, 1));
			this.RegisterToNPC(491, ItemDropRule.Common(905, 50, 1, 1));
			this.RegisterToNPC(491, ItemDropRule.Common(855, 15, 1, 1));
			this.RegisterToNPC(491, ItemDropRule.Common(854, 15, 1, 1));
			this.RegisterToNPC(491, ItemDropRule.Common(2584, 15, 1, 1));
			this.RegisterToNPC(491, ItemDropRule.Common(3033, 15, 1, 1));
			this.RegisterToNPC(491, ItemDropRule.Common(4471, 20, 1, 1));
			this.RegisterToNPC(491, ItemDropRule.Common(672, 10, 1, 1));
			this.RegisterToNPC(491, ItemDropRule.Common(5460, 10, 1, 1));
			this.RegisterToNPC(491, ItemDropRule.MasterModeCommonDrop(4940));
			this.RegisterToNPC(491, ItemDropRule.MasterModeDropOnAllPlayers(4792, this._masterModeDropRng));
			this.RegisterToNPC(491, ItemDropRule.OneFromOptions(1, new int[]
			{
				1704,
				1705,
				1710,
				1716,
				1720,
				2379,
				2389,
				2405,
				2843,
				3885,
				2663,
				3910,
				2238,
				2133,
				2137,
				2143,
				2147,
				2151,
				2155
			}));
		}

		// Token: 0x06002736 RID: 10038 RVA: 0x00009E06 File Offset: 0x00008006
		private void RegisterBloodMoonFishingEnemies()
		{
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x00563CC8 File Offset: 0x00561EC8
		private void RegisterBossTrophies()
		{
			Conditions.LegacyHack_IsABoss condition = new Conditions.LegacyHack_IsABoss();
			this.RegisterToNPC(4, ItemDropRule.ByCondition(condition, 1360, 10, 1, 1, 1));
			this.RegisterToNPC(13, ItemDropRule.ByCondition(condition, 1361, 10, 1, 1, 1));
			this.RegisterToNPC(14, ItemDropRule.ByCondition(condition, 1361, 10, 1, 1, 1));
			this.RegisterToNPC(15, ItemDropRule.ByCondition(condition, 1361, 10, 1, 1, 1));
			this.RegisterToNPC(266, ItemDropRule.ByCondition(condition, 1362, 10, 1, 1, 1));
			this.RegisterToNPC(35, ItemDropRule.ByCondition(condition, 1363, 10, 1, 1, 1));
			this.RegisterToNPC(222, ItemDropRule.ByCondition(condition, 1364, 10, 1, 1, 1));
			this.RegisterToNPC(113, ItemDropRule.ByCondition(condition, 1365, 10, 1, 1, 1));
			this.RegisterToNPC(134, ItemDropRule.ByCondition(condition, 1366, 10, 1, 1, 1));
			this.RegisterToNPC(127, ItemDropRule.ByCondition(condition, 1367, 10, 1, 1, 1));
			this.RegisterToNPC(262, ItemDropRule.ByCondition(condition, 1370, 10, 1, 1, 1));
			this.RegisterToNPC(245, ItemDropRule.ByCondition(condition, 1371, 10, 1, 1, 1));
			this.RegisterToNPC(50, ItemDropRule.ByCondition(condition, 2489, 10, 1, 1, 1));
			this.RegisterToNPC(370, ItemDropRule.ByCondition(condition, 2589, 10, 1, 1, 1));
			this.RegisterToNPC(439, ItemDropRule.ByCondition(condition, 3357, 10, 1, 1, 1));
			this.RegisterToNPC(395, ItemDropRule.ByCondition(condition, 3358, 10, 1, 1, 1));
			this.RegisterToNPC(398, ItemDropRule.ByCondition(condition, 3595, 10, 1, 1, 1));
			this.RegisterToNPC(636, ItemDropRule.ByCondition(condition, 4783, 10, 1, 1, 1));
			this.RegisterToNPC(657, ItemDropRule.ByCondition(condition, 4958, 10, 1, 1, 1));
			this.RegisterToNPC(668, ItemDropRule.ByCondition(condition, 5108, 10, 1, 1, 1));
			this.RegisterToNPC(125, ItemDropRule.Common(1368, 10, 1, 1));
			this.RegisterToNPC(126, ItemDropRule.Common(1369, 10, 1, 1));
			this.RegisterToNPC(491, ItemDropRule.Common(3359, 10, 1, 1));
			this.RegisterToNPC(551, ItemDropRule.Common(3866, 10, 1, 1));
			this.RegisterToNPC(564, ItemDropRule.Common(3867, 10, 1, 1));
			this.RegisterToNPC(565, ItemDropRule.Common(3867, 10, 1, 1));
			this.RegisterToNPC(576, ItemDropRule.Common(3868, 10, 1, 1));
			this.RegisterToNPC(577, ItemDropRule.Common(3868, 10, 1, 1));
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x00563FBC File Offset: 0x005621BC
		private void RegisterMartianDrops()
		{
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2860, 8, 8, 20), new int[]
			{
				520,
				383,
				389,
				385,
				382,
				381,
				390,
				386
			});
			int[] npcNetIds = new int[]
			{
				520,
				383,
				389,
				385,
				382,
				381,
				390,
				386
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2798, 800, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2800, 800, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2882, 800, 1, 1), npcNetIds);
			int[] npcNetIds2 = new int[]
			{
				383,
				389,
				386
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2806, 200, 1, 1), npcNetIds2);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2807, 200, 1, 1), npcNetIds2);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2808, 200, 1, 1), npcNetIds2);
			int[] npcNetIds3 = new int[]
			{
				385,
				382,
				381,
				390
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2803, 200, 1, 1), npcNetIds3);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2804, 200, 1, 1), npcNetIds3);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2805, 200, 1, 1), npcNetIds3);
			this.RegisterToNPC(395, ItemDropRule.OneFromOptionsNotScalingWithLuck(1, new int[]
			{
				2797,
				2749,
				2795,
				2796,
				2880,
				2769
			}));
			this.RegisterToNPC(395, ItemDropRule.MasterModeCommonDrop(4939));
			this.RegisterToNPC(395, ItemDropRule.MasterModeDropOnAllPlayers(4815, this._masterModeDropRng));
			this.RegisterToNPC(390, ItemDropRule.Common(2771, 30, 1, 1));
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x00564178 File Offset: 0x00562378
		private void RegisterMiscDrops()
		{
			this.RegisterToNPC(68, ItemDropRule.Common(1169, 1, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.Common(3086, 1, 5, 10), new int[]
			{
				483,
				482
			});
			this.RegisterToNPC(77, ItemDropRule.Common(723, 150, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.NormalvsExpert(3102, 2, 1), new int[]
			{
				195,
				196
			});
			this.RegisterToNPC(471, ItemDropRule.NormalvsExpertOneFromOptions(2, 1, new int[]
			{
				3052,
				3053,
				3054
			}));
			this.RegisterToNPC(153, ItemDropRule.Common(1328, 12, 1, 1));
			this.RegisterToNPC(59, new LeadingConditionRule(new Conditions.RemixSeed())).OnSuccess(ItemDropRule.Gel(1, 1, 2), false);
			this.RegisterToNPC(59, new LeadingConditionRule(new Conditions.RemixSeed())).OnSuccess(ItemDropRule.NormalvsExpert(1309, 8000, 5600), false);
			this.RegisterToNPC(120, new LeadingConditionRule(new Conditions.TenthAnniversaryIsUp())).OnSuccess(ItemDropRule.Common(1326, 100, 1, 1), false);
			this.RegisterToNPC(120, new LeadingConditionRule(new Conditions.TenthAnniversaryIsNotUp())).OnSuccess(ItemDropRule.NormalvsExpert(1326, 500, 400), false);
			this.RegisterToNPC(49, new LeadingConditionRule(new Conditions.NotRemixSeed())).OnSuccess(ItemDropRule.Common(1325, 250, 1, 1), false);
			this.RegisterToNPC(49, new LeadingConditionRule(new Conditions.RemixSeed())).OnSuccess(ItemDropRule.Common(1314, 250, 1, 1), false);
			this.RegisterToNPC(109, new LeadingConditionRule(new Conditions.NotRemixSeedHardmode())).OnSuccess(ItemDropRule.Common(1314, 5, 1, 1), false);
			this.RegisterToNPC(109, new LeadingConditionRule(new Conditions.RemixSeed())).OnSuccess(ItemDropRule.Common(1325, 5, 1, 1), false);
			this.RegisterToNPC(156, new LeadingConditionRule(new Conditions.NotRemixSeedHardmode())).OnSuccess(ItemDropRule.Common(683, 30, 1, 1), false);
			this.RegisterToNPC(156, new LeadingConditionRule(new Conditions.RemixSeed())).OnSuccess(ItemDropRule.Common(112, 30, 1, 1), false);
			this.RegisterToNPC(634, ItemDropRule.Common(4764, 40, 1, 1));
			this.RegisterToNPC(185, ItemDropRule.Common(951, 25, 1, 1));
			this.RegisterToNPC(185, new DropBasedOnExpertMode(ItemDropRule.Common(5070, 1, 1, 2), new CommonDrop(5070, 1, 1, 3, 1)));
			this.RegisterToNPC(44, ItemDropRule.Common(1320, 20, 1, 1));
			this.RegisterToNPC(44, ItemDropRule.Common(88, 20, 1, 1));
			this.RegisterToNPC(60, ItemDropRule.Common(1322, 150, 1, 1));
			this.RegisterToNPC(151, ItemDropRule.Common(1322, 50, 1, 1));
			this.RegisterToNPC(24, ItemDropRule.Common(1323, 20, 1, 1));
			this.RegisterToNPC(109, ItemDropRule.Common(1324, 10, 1, 1));
			this.RegisterToNPC(109, ItemDropRule.Common(4271, 10, 1, 1));
			int[] npcNetIds = new int[]
			{
				163,
				238
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1308, 40, 1, 1), npcNetIds);
			this.RegisterToMultipleNPCs(new DropBasedOnExpertMode(ItemDropRule.Common(2607, 2, 1, 3), new CommonDrop(2607, 10, 1, 3, 9)), npcNetIds);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1306, 100, 1, 1), new int[]
			{
				197,
				206,
				169,
				154
			});
			this.RegisterToNPC(301, ItemDropRule.Common(5576, 10, 1, 1));
			this.RegisterToNPC(244, ItemDropRule.Gel(1, 1, 20));
			this.RegisterToNPC(244, ItemDropRule.Common(662, 1, 30, 60));
			this.RegisterToNPC(250, ItemDropRule.Common(1244, 15, 1, 1));
			this.RegisterToNPC(172, ItemDropRule.Common(754, 1, 1, 1));
			this.RegisterToNPC(172, ItemDropRule.Common(755, 1, 1, 1));
			this.RegisterToNPC(110, ItemDropRule.Common(682, 200, 1, 1));
			this.RegisterToNPC(110, ItemDropRule.Common(1321, 40, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.Common(4428, 100, 1, 1), new int[]
			{
				170,
				180,
				171
			});
			this.RegisterToMultipleNPCs(new ItemDropWithConditionRule(4613, 25, 1, 1, new Conditions.WindyEnoughForKiteDrops(), 1), new int[]
			{
				170,
				180,
				171
			});
			this.RegisterToMultipleNPCs(new ItemDropWithConditionRule(5096, 10, 1, 1, new Conditions.DontStarveIsUp(), 1), new int[]
			{
				170,
				180,
				171
			});
			this.RegisterToMultipleNPCs(new ItemDropWithConditionRule(5096, 25, 1, 1, new Conditions.DontStarveIsNotUp(), 1), new int[]
			{
				170,
				180,
				171
			});
			this.RegisterToNPC(154, ItemDropRule.Common(1253, 50, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.Common(726, 50, 1, 1), new int[]
			{
				169,
				206
			});
			this.RegisterToNPC(243, ItemDropRule.Common(2161, 1, 1, 1));
			this.RegisterToNPC(155, ItemDropRule.NormalvsExpert(5130, 30, 25));
			this.RegisterToNPC(480, ItemDropRule.Common(3269, 25, 1, 1));
			this.RegisterToNPC(480, ItemDropRule.NormalvsExpert(3781, 40, 20));
			int[] npcNetIds2 = new int[]
			{
				198,
				199,
				226
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1172, 1000, 1, 1), npcNetIds2);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1293, 50, 1, 1), npcNetIds2);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(2766, 7, 1, 2), npcNetIds2);
			int[] npcNetIds3 = new int[]
			{
				78,
				79,
				80,
				630
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(870, 75, 1, 1), npcNetIds3);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(871, 75, 1, 1), npcNetIds3);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(872, 75, 1, 1), npcNetIds3);
			this.RegisterToNPC(473, ItemDropRule.OneFromOptions(1, new int[]
			{
				3008,
				3014,
				3012,
				3015,
				3023
			}));
			this.RegisterToNPC(474, ItemDropRule.OneFromOptions(1, new int[]
			{
				3006,
				3007,
				3013,
				3016,
				3020
			}));
			this.RegisterToNPC(475, ItemDropRule.OneFromOptions(1, new int[]
			{
				3029,
				3030,
				3051,
				3022
			}));
			this.RegisterToNPC(476, ItemDropRule.Common(52, 3, 1, 1));
			this.RegisterToNPC(476, ItemDropRule.Common(1724, 3, 1, 1));
			this.RegisterToNPC(476, ItemDropRule.Common(2353, 3, 5, 10));
			this.RegisterToNPC(476, ItemDropRule.Common(1922, 3, 1, 1));
			this.RegisterToNPC(476, ItemDropRule.Common(678, 3, 3, 5));
			this.RegisterToNPC(476, ItemDropRule.Common(1336, 3, 1, 1));
			this.RegisterToNPC(476, ItemDropRule.Common(2676, 3, 2, 4));
			this.RegisterToNPC(476, ItemDropRule.Common(2272, 3, 1, 1));
			this.RegisterToNPC(476, ItemDropRule.Common(5395, 3, 1, 1));
			this.RegisterToNPC(476, ItemDropRule.Common(4986, 3, 69, 69));
			int[] npcNetIds4 = new int[]
			{
				473,
				474,
				475
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(499, 1, 5, 10), npcNetIds4);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(500, 1, 5, 15), npcNetIds4);
			this.RegisterToNPC(87, new ItemDropWithConditionRule(4379, 25, 1, 1, new Conditions.WindyEnoughForKiteDrops(), 1));
			this.RegisterToNPC(87, new DropBasedOnExpertMode(ItemDropRule.Common(575, 1, 5, 10), ItemDropRule.Common(575, 1, 10, 20)));
			this.RegisterToMultipleNPCs(ItemDropRule.OneFromOptions(10, new int[]
			{
				803,
				804,
				805
			}), new int[]
			{
				161,
				431
			});
			this.RegisterToNPC(217, ItemDropRule.Common(1115, 1, 1, 1));
			this.RegisterToNPC(218, ItemDropRule.Common(1116, 1, 1, 1));
			this.RegisterToNPC(219, ItemDropRule.Common(1117, 1, 1, 1));
			this.RegisterToNPC(220, ItemDropRule.Common(1118, 1, 1, 1));
			this.RegisterToNPC(221, ItemDropRule.Common(1119, 1, 1, 1));
			this.RegisterToNPC(167, ItemDropRule.Common(879, 50, 1, 1));
			this.RegisterToNPC(628, ItemDropRule.Common(313, 2, 1, 2));
			int[] npcNetIds5 = new int[]
			{
				143,
				144,
				145
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(593, 1, 5, 10), npcNetIds5);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(527, 10, 1, 1), new int[]
			{
				79,
				630
			});
			this.RegisterToNPC(80, ItemDropRule.Common(528, 10, 1, 1));
			this.RegisterToNPC(524, ItemDropRule.Common(3794, 10, 1, 3));
			this.RegisterToNPC(525, ItemDropRule.Common(3794, 10, 1, 1));
			this.RegisterToNPC(525, ItemDropRule.Common(522, 3, 1, 3));
			this.RegisterToNPC(525, ItemDropRule.Common(527, 15, 1, 1));
			this.RegisterToNPC(526, ItemDropRule.Common(3794, 10, 1, 1));
			this.RegisterToNPC(526, ItemDropRule.Common(1332, 3, 1, 3));
			this.RegisterToNPC(526, ItemDropRule.Common(527, 15, 1, 1));
			this.RegisterToNPC(527, ItemDropRule.Common(3794, 10, 1, 1));
			this.RegisterToNPC(527, ItemDropRule.Common(528, 15, 1, 1));
			this.RegisterToNPC(513, ItemDropRule.Common(3380, 2, 1, 2));
			this.RegisterToNPC(532, ItemDropRule.Common(3380, 1, 1, 3));
			this.RegisterToNPC(532, ItemDropRule.Common(3771, 50, 1, 1));
			this.RegisterToNPC(528, ItemDropRule.Common(2802, 25, 1, 1));
			this.RegisterToNPC(529, ItemDropRule.Common(2801, 25, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.OneFromOptions(40, new int[]
			{
				3786,
				3785,
				3784
			}), new int[]
			{
				528,
				529
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Common(18, 200, 1, 1), new int[]
			{
				49,
				51,
				150,
				93,
				634
			});
			this.RegisterToMultipleNPCs(new ItemDropWithConditionRule(5097, 300, 1, 1, new Conditions.DontStarveIsNotUp(), 1), new int[]
			{
				49,
				51,
				150,
				93,
				634,
				151,
				60,
				137,
				152
			});
			this.RegisterToMultipleNPCs(new ItemDropWithConditionRule(5097, 100, 1, 1, new Conditions.DontStarveIsUp(), 1), new int[]
			{
				49,
				51,
				150,
				93,
				634,
				151,
				60,
				137,
				152
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Common(393, 100, 1, 1), new int[]
			{
				16,
				185,
				167,
				197
			});
			this.RegisterToNPC(58, ItemDropRule.Common(393, 75, 1, 1));
			int[] npcNetIds6 = new int[]
			{
				494,
				495,
				496,
				497,
				498,
				499,
				500,
				501,
				502,
				503,
				504,
				505,
				506
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(18, 80, 1, 1), npcNetIds6).OnFailedRoll(ItemDropRule.Common(393, 80, 1, 1), false).OnFailedRoll(ItemDropRule.Common(3285, 15, 1, 1), false);
			int[] npcNetIds7 = new int[]
			{
				21,
				201,
				202,
				203,
				322,
				323,
				324,
				635,
				449,
				450,
				451,
				452
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(954, 100, 1, 1), npcNetIds7).OnFailedRoll(ItemDropRule.Common(955, 200, 1, 1), false).OnFailedRoll(ItemDropRule.Common(1166, 200, 1, 1), false).OnFailedRoll(ItemDropRule.Common(1274, 500, 1, 1), false);
			this.RegisterToNPC(6, ItemDropRule.OneFromOptions(175, new int[]
			{
				956,
				957,
				958
			}));
			int[] npcNetIds8 = new int[]
			{
				42,
				43,
				231,
				232,
				233,
				234,
				235
			};
			this.RegisterToMultipleNPCs(ItemDropRule.OneFromOptions(100, new int[]
			{
				960,
				961,
				962
			}), npcNetIds8);
			int[] npcNetIds9 = new int[]
			{
				31,
				32,
				294,
				295,
				296,
				693
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(959, 450, 1, 1), npcNetIds9);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1307, 300, 1, 1), npcNetIds9);
			this.RegisterToNPC(32, ItemDropRule.Common(5632, 150, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.Common(996, 200, 1, 1), new int[]
			{
				174,
				179,
				182,
				183,
				98,
				83,
				94,
				81,
				101
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Common(522, 1, 2, 5), new int[]
			{
				101,
				98
			});
			this.RegisterToNPC(98, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4611, 25, 1, 1, 1));
			this.RegisterToNPC(86, ItemDropRule.Common(526, 1, 1, 1));
			this.RegisterToNPC(86, ItemDropRule.Common(856, 100, 1, 1));
			this.RegisterToNPC(86, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4684, 25, 1, 1, 1));
			this.RegisterToNPC(224, ItemDropRule.Common(4057, 100, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.Common(40, 1, 1, 9), new int[]
			{
				186,
				432
			});
			this.RegisterToNPC(225, ItemDropRule.Common(1243, 45, 1, 1)).OnFailedRoll(ItemDropRule.Gel(1, 2, 6), false);
			this.RegisterToNPC(537, ItemDropRule.Gel(1, 2, 3));
			this.RegisterToNPC(537, ItemDropRule.NormalvsExpert(1309, 8000, 5600));
			int[] npcNetIds10 = new int[]
			{
				335,
				336,
				333,
				334
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1906, 20, 1, 1), npcNetIds10);
			this.RegisterToNPC(-4, ItemDropRule.Common(3111, 1, 25, 50));
			this.RegisterToNPC(-4, ItemDropRule.NormalvsExpert(1309, 100, 70));
			int[] npcNetIds11 = new int[]
			{
				1,
				16,
				138,
				141,
				147,
				184,
				187,
				433,
				204,
				302,
				333,
				334,
				335,
				336,
				535,
				658,
				659,
				660
			};
			int[] npcNetIds12 = new int[]
			{
				-6,
				-7,
				-8,
				-9,
				676
			};
			int[] npcNetIds13 = new int[]
			{
				-6,
				-7,
				-8,
				-9,
				-4
			};
			IItemDropRule entry = this.RegisterToMultipleNPCs(ItemDropRule.Gel(1, 1, 2), npcNetIds11);
			this.RemoveFromMultipleNPCs(entry, npcNetIds13);
			this.RegisterToMultipleNPCs(ItemDropRule.Gel(1, 2, 5), npcNetIds12);
			IItemDropRule entry2 = this.RegisterToMultipleNPCs(ItemDropRule.NormalvsExpert(1309, 10000, 7000), npcNetIds11);
			this.RemoveFromMultipleNPCs(entry2, npcNetIds13);
			this.RegisterToMultipleNPCs(ItemDropRule.NormalvsExpert(1309, 10000, 7000), npcNetIds12);
			this.RegisterToNPC(75, ItemDropRule.Common(501, 1, 1, 3));
			this.RegisterToMultipleNPCs(ItemDropRule.Gel(1, 2, 4), new int[]
			{
				81,
				183
			});
			this.RegisterToNPC(122, ItemDropRule.Gel(1, 5, 10));
			this.RegisterToNPC(71, ItemDropRule.Common(327, 1, 1, 1));
			int[] npcNetIds14 = new int[]
			{
				2,
				317,
				318,
				190,
				191,
				192,
				193,
				194,
				133
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(236, 100, 1, 1), npcNetIds14).OnFailedRoll(ItemDropRule.Common(38, 3, 1, 1), false);
			this.RegisterToMultipleNPCs(new ItemDropWithConditionRule(43, 50, 1, 1, new Conditions.EyeOfCthulhuDefeatedAndNoAltarsInWorld(), 1), npcNetIds14);
			this.RegisterToNPC(133, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4683, 25, 1, 1, 1));
			this.RegisterToNPC(104, ItemDropRule.Common(485, 60, 1, 1));
			this.RegisterToNPC(58, ItemDropRule.Common(263, 250, 1, 1)).OnFailedRoll(ItemDropRule.Common(118, 30, 1, 1), false);
			this.RegisterToNPC(102, ItemDropRule.Common(263, 250, 1, 1));
			int[] npcNetIds15 = new int[]
			{
				3,
				591,
				590,
				331,
				332,
				132,
				161,
				186,
				187,
				188,
				189,
				200,
				223,
				319,
				320,
				321,
				430,
				431,
				432,
				433,
				434,
				435,
				436
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(216, 50, 1, 1), npcNetIds15);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1304, 250, 1, 1), npcNetIds15);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(5332, 1500, 1, 1), npcNetIds15);
			this.RegisterToMultipleNPCs(new ItemDropWithConditionRule(1786, 15, 1, 1, new Conditions.SkyblockIsUpNoSickle(), 1), npcNetIds15);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(8, 1, 5, 20), new int[]
			{
				590,
				591
			});
			int[] npcNetIds16 = new int[]
			{
				189,
				435,
				188,
				434
			};
			this.RegisterToMultipleNPCs(new ItemDropWithConditionRule(9, 2, 5, 20, new Conditions.SkyblockIsUp(), 1), npcNetIds16);
			this.RegisterToMultipleNPCs(ItemDropRule.NormalvsExpert(3212, 150, 75), new int[]
			{
				489,
				490
			});
			this.RegisterToMultipleNPCs(ItemDropRule.NormalvsExpert(3213, 200, 100), new int[]
			{
				489,
				490
			});
			this.RegisterToNPC(223, ItemDropRule.OneFromOptions(20, new int[]
			{
				1135,
				1136
			}));
			this.RegisterToNPC(66, ItemDropRule.Common(267, 1, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.Common(272, 35, 1, 1), new int[]
			{
				62,
				66
			});
			this.RegisterToNPC(52, ItemDropRule.Common(251, 1, 1, 1));
			this.RegisterToNPC(53, ItemDropRule.Common(239, 1, 1, 1));
			this.RegisterToNPC(536, ItemDropRule.Common(3478, 1, 1, 1));
			this.RegisterToNPC(536, ItemDropRule.Common(3479, 1, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.Common(323, 3, 1, 2), new int[]
			{
				69,
				581,
				580,
				508,
				509
			});
			this.RegisterToNPC(582, ItemDropRule.Common(323, 6, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.Common(3772, 50, 1, 1), new int[]
			{
				581,
				580,
				508,
				509
			});
			this.RegisterToNPC(73, ItemDropRule.Common(362, 1, 1, 2));
			int[] npcNetIds17 = new int[]
			{
				483,
				482
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(3109, 30, 1, 1), npcNetIds17);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(4400, 20, 1, 1), npcNetIds17);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(68, 3, 1, 1), new int[]
			{
				6,
				94
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1330, 3, 1, 1), new int[]
			{
				181,
				173,
				239,
				182,
				240
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Common(68, 3, 1, 2), new int[]
			{
				7,
				8,
				9
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Common(69, 1, 3, 8), new int[]
			{
				7,
				8,
				9
			});
			this.RegisterToMultipleNPCs(new ItemDropWithConditionRule(5094, 100, 1, 1, new Conditions.DontStarveIsUp(), 1), new int[]
			{
				6,
				7,
				8,
				9,
				173,
				181,
				239,
				240
			});
			this.RegisterToMultipleNPCs(new ItemDropWithConditionRule(5094, 525, 1, 1, new Conditions.DontStarveIsNotUp(), 1), new int[]
			{
				6,
				7,
				8,
				9,
				173,
				181,
				239,
				240
			});
			this.RegisterToMultipleNPCs(new ItemDropWithConditionRule(5091, 500, 1, 1, new Conditions.DontStarveIsUp(), 1), new int[]
			{
				6,
				7,
				8,
				9,
				94,
				81,
				101,
				173,
				181,
				239,
				240,
				174,
				183,
				242,
				241,
				268,
				182,
				98,
				99,
				100
			});
			this.RegisterToMultipleNPCs(new ItemDropWithConditionRule(5091, 1500, 1, 1, new Conditions.DontStarveIsNotUp(), 1), new int[]
			{
				6,
				7,
				8,
				9,
				94,
				81,
				101,
				173,
				181,
				239,
				240,
				174,
				183,
				242,
				241,
				268,
				182,
				98,
				99,
				100
			});
			this.RegisterToNPC(690, new StatueMimicItemDropRule());
			this.RegisterToMultipleNPCs(new DropBasedOnExpertMode(ItemDropRule.Common(215, 50, 1, 1), ItemDropRule.WithRerolls(215, 1, 50, 1, 1)), new int[]
			{
				10,
				11,
				12,
				95,
				96,
				97
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Common(243, 75, 1, 1), new int[]
			{
				47,
				464
			});
			this.RegisterToMultipleNPCs(ItemDropRule.OneFromOptions(50, new int[]
			{
				3757,
				3758,
				3759
			}), new int[]
			{
				168,
				470
			});
			this.RegisterToNPC(533, ItemDropRule.Common(3795, 40, 1, 1)).OnFailedRoll(ItemDropRule.Common(3770, 30, 1, 1), false);
			int[] npcNetIds18 = new int[]
			{
				63,
				103,
				64
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(1303, 100, 1, 1), npcNetIds18);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(282, 1, 1, 4), npcNetIds18);
			this.RegisterToMultipleNPCs(ItemDropRule.Common(282, 1, 1, 4), new int[]
			{
				223
			});
			this.RegisterToMultipleNPCs(ItemDropRule.Common(282, 1, 1, 4), new int[]
			{
				224
			});
			this.RegisterToNPC(63, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4649, 50, 1, 1, 1));
			this.RegisterToNPC(64, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4650, 50, 1, 1, 1));
			this.RegisterToNPC(691, ItemDropRule.OneFromOptions(1, new int[]
			{
				4352,
				4350,
				4349,
				4353,
				4351,
				4354,
				5127,
				4378,
				4377,
				4389
			}));
			this.RegisterToNPC(481, ItemDropRule.Common(3094, 2, 40, 80));
			this.RegisterToNPC(481, ItemDropRule.OneFromOptions(7, new int[]
			{
				3187,
				3188,
				3189
			}));
			this.RegisterToNPC(481, ItemDropRule.Common(4463, 20, 1, 1));
			this.RegisterToNPC(481, ItemDropRule.Common(5543, 100, 1, 1));
			int[] npcNetIds19 = new int[]
			{
				21,
				167,
				201,
				202,
				481,
				203,
				322,
				323,
				324,
				449,
				450,
				451,
				452
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(118, 25, 1, 1), npcNetIds19);
			this.RegisterToNPC(44, ItemDropRule.Common(118, 25, 1, 1)).OnFailedRoll(ItemDropRule.OneFromOptions(4, new int[]
			{
				410,
				411
			}), false).OnFailedRoll(ItemDropRule.Common(166, 1, 1, 3), false);
			this.RegisterToNPC(45, ItemDropRule.Common(238, 1, 1, 1));
			this.RegisterToNPC(23, ItemDropRule.Common(116, 50, 1, 1));
			this.RegisterToNPC(23, ItemDropRule.Common(5486, 100, 1, 1));
			this.RegisterToNPC(24, ItemDropRule.Common(244, 250, 1, 1));
			int[] npcNetIds20 = new int[]
			{
				31,
				32,
				34,
				294,
				295,
				296,
				693
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(932, 250, 1, 1), npcNetIds20).OnFailedRoll(ItemDropRule.Common(3095, 100, 1, 1), false).OnFailedRoll(ItemDropRule.Common(327, 65, 1, 1), false).OnFailedRoll(ItemDropRule.ByCondition(new Conditions.NotExpert(), 154, 1, 1, 3, 1), false);
			this.RegisterToMultipleNPCs(ItemDropRule.ByCondition(new Conditions.IsExpert(), 154, 1, 2, 6, 1), npcNetIds20);
			this.RegisterToNPC(694, ItemDropRule.Common(165, 40, 1, 1));
			int[] npcNetIds21 = new int[]
			{
				26,
				27,
				28,
				29,
				111
			};
			this.RegisterToMultipleNPCs(ItemDropRule.Common(160, 200, 1, 1), npcNetIds21).OnFailedRoll(ItemDropRule.Common(161, 2, 1, 5), false);
			this.RegisterToNPC(175, ItemDropRule.Common(1265, 100, 1, 1));
			this.RegisterToNPC(175, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4675, 25, 1, 1, 1));
			this.RegisterToMultipleNPCs(new DropBasedOnExpertMode(new CommonDrop(209, 3, 1, 1, 2), ItemDropRule.Common(209, 1, 1, 1)), new int[]
			{
				42,
				231,
				232,
				233,
				234,
				235
			});
			this.RegisterToNPC(176, ItemDropRule.Common(209, 6, 1, 1));
			this.RegisterToNPC(177, new ItemDropWithConditionRule(5089, 100, 1, 1, new Conditions.DontStarveIsNotUp(), 1));
			this.RegisterToNPC(177, new ItemDropWithConditionRule(5089, 40, 1, 1, new Conditions.DontStarveIsUp(), 1));
			this.RegisterToNPC(204, ItemDropRule.NormalvsExpert(209, 2, 1));
			this.RegisterToNPC(43, ItemDropRule.NormalvsExpert(210, 2, 1));
			this.RegisterToNPC(43, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4648, 25, 1, 1, 1));
			this.RegisterToNPC(39, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4610, 15, 1, 1, 1));
			this.RegisterToNPC(65, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4651, 25, 1, 1, 1));
			this.RegisterToNPC(65, ItemDropRule.Common(268, 20, 1, 1)).OnFailedRoll(ItemDropRule.Common(319, 1, 1, 1), false);
			this.RegisterToNPC(692, ItemDropRule.Common(268, 20, 1, 1)).OnFailedRoll(ItemDropRule.Common(319, 1, 1, 1), false);
			this.RegisterToNPC(48, ItemDropRule.NotScalingWithLuck(320, 2, 1, 1));
			this.RegisterToNPC(541, ItemDropRule.Common(3783, 1, 1, 1));
			this.RegisterToMultipleNPCs(ItemDropRule.Common(319, 8, 1, 1), new int[]
			{
				542,
				543,
				544,
				545
			});
			this.RegisterToMultipleNPCs(ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4669, 25, 1, 1, 1), new int[]
			{
				542,
				543,
				544,
				545
			});
			this.RegisterToNPC(543, ItemDropRule.Common(527, 25, 1, 1));
			this.RegisterToNPC(544, ItemDropRule.Common(527, 25, 1, 1));
			this.RegisterToNPC(545, ItemDropRule.Common(528, 25, 1, 1));
			this.RegisterToNPC(47, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4670, 25, 1, 1, 1));
			this.RegisterToNPC(464, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4671, 25, 1, 1, 1));
			this.RegisterToNPC(268, ItemDropRule.Common(1332, 1, 2, 5));
			this.RegisterToNPC(631, ItemDropRule.Common(3, 1, 10, 20));
			this.RegisterToNPC(631, ItemDropRule.Common(4761, 3, 1, 1));
			int[] npcNetIds22 = new int[]
			{
				594
			};
			LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.NeverTrue());
			int[] options = new int[0];
			IItemDropRule rule = leadingConditionRule.OnSuccess(ItemDropRule.OneFromOptions(8, options), false);
			int chanceDenominator = 9;
			rule.OnSuccess(new CommonDrop(4367, chanceDenominator, 1, 1, 1), false);
			rule.OnSuccess(new CommonDrop(4368, chanceDenominator, 1, 1, 1), false);
			rule.OnSuccess(new CommonDrop(4369, chanceDenominator, 1, 1, 1), false);
			rule.OnSuccess(new CommonDrop(4370, chanceDenominator, 1, 1, 1), false);
			rule.OnSuccess(new CommonDrop(4371, chanceDenominator, 1, 1, 1), false);
			rule.OnSuccess(new CommonDrop(4612, chanceDenominator, 1, 1, 1), false);
			rule.OnSuccess(new CommonDrop(4674, chanceDenominator, 1, 1, 1), false);
			rule.OnSuccess(new CommonDrop(4343, chanceDenominator, 2, 5, 1), false);
			rule.OnSuccess(new CommonDrop(4344, chanceDenominator, 2, 5, 1), false);
			this.RegisterToMultipleNPCs(leadingConditionRule, npcNetIds22);
		}

		// Token: 0x040050A6 RID: 20646
		private List<IItemDropRule> _globalEntries = new List<IItemDropRule>();

		// Token: 0x040050A7 RID: 20647
		private Dictionary<int, List<IItemDropRule>> _entriesByNpcNetId = new Dictionary<int, List<IItemDropRule>>();

		// Token: 0x040050A8 RID: 20648
		private Dictionary<int, List<int>> _npcNetIdsByType = new Dictionary<int, List<int>>();

		// Token: 0x040050A9 RID: 20649
		private int _masterModeDropRng = 4;
	}
}
