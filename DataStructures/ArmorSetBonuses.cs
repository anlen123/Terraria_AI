using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.Achievements;
using Terraria.ID;

namespace Terraria.DataStructures
{
	// Token: 0x02000533 RID: 1331
	public class ArmorSetBonuses
	{
		// Token: 0x0600371C RID: 14108 RVA: 0x0062C34C File Offset: 0x0062A54C
		public static void Initialize()
		{
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Shroomite), "ArmorSetBonus.Shroomite", ArmorSetBonus.PartType.None).Set(1548, 1549, 1550).Set(1546, 1549, 1550).Set(1547, 1549, 1550).Add();
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Wood), "ArmorSetBonus.Wood", ArmorSetBonus.PartType.None).Set(727, 728, 729).Set(733, 734, 735).Set(730, 731, 732).Set(736, 737, 738).Set(924, 925, 926).Set(2509, 2510, 2511).Set(2512, 2513, 2514).Add();
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.AshWood), "ArmorSetBonus.AshWood", 5279, 5280, 5281);
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.MetalTier1), "ArmorSetBonus.MetalTier1", ArmorSetBonus.PartType.None).Set(89, 80, 76).Set(687, 688, 689).Set(90, 81, 77).Set(954, 81, 77).Add();
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.MetalTier2), "ArmorSetBonus.MetalTier2", ArmorSetBonus.PartType.None).Set(91, 82, 78).Set(92, 83, 79).Set(955, 83, 79).Set(690, 691, 692).Set(693, 694, 695).Add();
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Platinum), "ArmorSetBonus.Platinum", 696, 697, 698);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Pumpkin), "ArmorSetBonus.Pumpkin", 1731, 1732, 1733);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Gladiator), "ArmorSetBonus.Gladiator", 3187, 3188, 3189);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Ninja), "ArmorSetBonus.Ninja", 256, 257, 258);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Fossil), "ArmorSetBonus.Fossil", 3374, 3375, 3376);
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Necro), "ArmorSetBonus.Bone", ArmorSetBonus.PartType.None).Set(151, 152, 153).Set(959, 152, 153).Add();
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.BeetleDamage), "ArmorSetBonus.BeetleDamage", ArmorSetBonus.PartType.Body, 2199, 2200, 2202);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.BeetleDefense), "ArmorSetBonus.BeetleDefense", ArmorSetBonus.PartType.Body, 2199, 2201, 2202);
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Wizard), "ArmorSetBonus.Wizard", ArmorSetBonus.PartType.Head).Set(238, 1282, 0).Set(238, 1283, 0).Set(238, 1284, 0).Set(238, 1285, 0).Set(238, 1286, 0).Set(238, 1287, 0).Set(238, 2279, 0).Set(238, 4256, 0).Add();
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.MagicHat), "ArmorSetBonus.MagicHat", ArmorSetBonus.PartType.Head).Set(2275, 1282, 0).Set(2275, 1283, 0).Set(2275, 1284, 0).Set(2275, 1285, 0).Set(2275, 1286, 0).Set(2275, 1287, 0).Set(2275, 2279, 0).Set(2275, 4256, 0).Add();
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.ShadowScale), "ArmorSetBonus.ShadowScale", ArmorSetBonus.PartType.None).Set(new int[]
			{
				102,
				956
			}, new int[]
			{
				101,
				957
			}, new int[]
			{
				100,
				958
			}).Add();
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Crimson), "ArmorSetBonus.Crimson", 792, 793, 794);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.SpectreHealing), "ArmorSetBonus.SpectreHealing", ArmorSetBonus.PartType.Head, 1503, 1504, 1505);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.SpectreDamage), "ArmorSetBonus.SpectreDamage", ArmorSetBonus.PartType.Head, 2189, 1504, 1505);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Meteor), "ArmorSetBonus.Meteor", 123, 124, 125);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Frost), "ArmorSetBonus.Frost", 684, 685, 686);
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Jungle), "ArmorSetBonus.Jungle", ArmorSetBonus.PartType.None).Set(new int[]
			{
				228,
				960
			}, new int[]
			{
				229,
				961
			}, new int[]
			{
				230,
				962
			}).Add();
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Molten), "ArmorSetBonus.Molten", 231, 232, 233);
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Snow), "ArmorSetBonus.Snow", ArmorSetBonus.PartType.None).Set(new int[]
			{
				803,
				978
			}, new int[]
			{
				804,
				979
			}, new int[]
			{
				805,
				980
			}).Add();
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Mining), "ArmorSetBonus.Mining", ArmorSetBonus.PartType.None).Set(new int[]
			{
				88,
				5588,
				4008
			}, new int[]
			{
				410,
				5589
			}, new int[]
			{
				411,
				5590
			}).Add();
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.ChlorophyteMelee), "ArmorSetBonus.ChlorophyteMelee", ArmorSetBonus.PartType.Head, 1001, 1004, 1005);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.ChlorophyteSummon), "ArmorSetBonus.ChlorophyteSummon", ArmorSetBonus.PartType.Head, 5524, 1004, 1005);
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Chlorophyte), "ArmorSetBonus.Chlorophyte", ArmorSetBonus.PartType.Head).Set(1003, 1004, 1005).Set(1002, 1004, 1005).Add();
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Angler), "ArmorSetBonus.Angler", ArmorSetBonus.PartType.None).Set(new int[]
			{
				2367,
				5591
			}, new int[]
			{
				2368,
				5592
			}, new int[]
			{
				2369,
				5593
			}).Add();
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Cactus), "ArmorSetBonus.Cactus", 894, 895, 896);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Turtle), "ArmorSetBonus.Turtle", 1316, 1317, 1318);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.CobaltCaster), "ArmorSetBonus.CobaltCaster", ArmorSetBonus.PartType.Head, 371, 374, 375);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.CobaltMelee), "ArmorSetBonus.CobaltMelee", ArmorSetBonus.PartType.Head, 372, 374, 375);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.CobaltRanged), "ArmorSetBonus.CobaltRanged", ArmorSetBonus.PartType.Head, 373, 374, 375);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.MythrilCaster), "ArmorSetBonus.MythrilCaster", ArmorSetBonus.PartType.Head, 376, 379, 380);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.MythrilMelee), "ArmorSetBonus.MythrilMelee", ArmorSetBonus.PartType.Head, 377, 379, 380);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.MythrilRanged), "ArmorSetBonus.MythrilRanged", ArmorSetBonus.PartType.Head, 378, 379, 380);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.AdamantiteCaster), "ArmorSetBonus.AdamantiteCaster", ArmorSetBonus.PartType.Head, 400, 403, 404);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.AdamantiteMelee), "ArmorSetBonus.AdamantiteMelee", ArmorSetBonus.PartType.Head, 401, 403, 404);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.AdamantiteRanged), "ArmorSetBonus.AdamantiteRanged", ArmorSetBonus.PartType.Head, 402, 403, 404);
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Palladium), "ArmorSetBonus.Palladium", ArmorSetBonus.PartType.None).Set(1205, 1208, 1209).Set(1206, 1208, 1209).Set(1207, 1208, 1209).Add();
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Orichalcum), "ArmorSetBonus.Orichalcum", ArmorSetBonus.PartType.None).Set(1210, 1213, 1214).Set(1211, 1213, 1214).Set(1212, 1213, 1214).Add();
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Titanium), "ArmorSetBonus.Titanium", ArmorSetBonus.PartType.None).Set(1215, 1218, 1219).Set(1216, 1218, 1219).Set(1217, 1218, 1219).Add();
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.HallowedSummoner), "ArmorSetBonus.HallowedSummoner", ArmorSetBonus.PartType.Head).Set(new int[]
			{
				4873,
				4899
			}, new int[]
			{
				551,
				4900
			}, new int[]
			{
				552,
				4901
			}).Add();
			ArmorSetBonuses.Create(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Hallowed), "ArmorSetBonus.Hallowed", ArmorSetBonus.PartType.Head).Set(new int[]
			{
				558,
				553,
				559,
				4898,
				4897,
				4896
			}, new int[]
			{
				551,
				4900
			}, new int[]
			{
				552,
				4901
			}).Add();
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.CrystalAssassin), "ArmorSetBonus.CrystalNinja", 4982, 4983, 4984);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Tiki), "ArmorSetBonus.Tiki", 1159, 1160, 1161);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Spooky), "ArmorSetBonus.Spooky", 1832, 1833, 1834);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Bee), "ArmorSetBonus.Bee", 2361, 2362, 2363);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Spider), "ArmorSetBonus.Spider", 2370, 2371, 2372);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Solar), "ArmorSetBonus.Solar", 2763, 2764, 2765);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Vortex), "ArmorSetBonus.Vortex", 2757, 2758, 2759);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Nebula), "ArmorSetBonus.Nebula", 2760, 2761, 2762);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Stardust), "ArmorSetBonus.Stardust", 3381, 3382, 3383);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.Forbidden), "ArmorSetBonus.Forbidden", 3776, 3777, 3778);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.SquireTier2), "ArmorSetBonus.SquireTier2", 3800, 3801, 3802);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.ApprenticeTier2), "ArmorSetBonus.ApprenticeTier2", 3797, 3798, 3799);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.HuntressTier2), "ArmorSetBonus.HuntressTier2", 3803, 3804, 3805);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.MonkTier2), "ArmorSetBonus.MonkTier2", 3806, 3807, 3808);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.SquireTier3), "ArmorSetBonus.SquireTier3", 3871, 3872, 3873);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.ApprenticeTier3), "ArmorSetBonus.ApprenticeTier3", 3874, 3875, 3876);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.HuntressTier3), "ArmorSetBonus.HuntressTier3", 3877, 3878, 3879);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.MonkTier3), "ArmorSetBonus.MonkTier3", 3880, 3881, 3882);
			ArmorSetBonuses.Add(new ArmorSetBonus.ArmorSetEffect(ArmorSetBonuses.Benefits.ObsidianOutlaw), "ArmorSetBonus.ObsidianOutlaw", 3266, 3267, 3268);
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x0062D12C File Offset: 0x0062B32C
		public static void BuildLookup()
		{
			ArmorSetBonus[] array = new ArmorSetBonus[0];
			ArmorSetBonuses.SetsContaining = new ArmorSetBonus[(int)ItemID.Count][];
			for (int i = 0; i < ArmorSetBonuses.SetsContaining.Length; i++)
			{
				ArmorSetBonuses.SetsContaining[i] = array;
			}
			foreach (IGrouping<int, ArmorSetBonus> grouping in from set in ArmorSetBonuses.All
			group set by set.Head)
			{
				ArmorSetBonuses.SetsContaining[grouping.Key] = grouping.ToArray<ArmorSetBonus>();
			}
			foreach (IGrouping<int, ArmorSetBonus> grouping2 in from set in ArmorSetBonuses.All
			group set by set.Body)
			{
				ArmorSetBonuses.SetsContaining[grouping2.Key] = grouping2.ToArray<ArmorSetBonus>();
			}
			foreach (IGrouping<int, ArmorSetBonus> grouping3 in from set in ArmorSetBonuses.All
			group set by set.Legs)
			{
				ArmorSetBonuses.SetsContaining[grouping3.Key] = grouping3.ToArray<ArmorSetBonus>();
			}
			ArmorSetBonuses.SetsContaining[0] = array;
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x0062D2BC File Offset: 0x0062B4BC
		public static ArmorSetBonus GetCompleteSet(ArmorSetBonus.QueryContext context)
		{
			foreach (ArmorSetBonus armorSetBonus in ArmorSetBonuses.SetsContaining[context.HeadItem])
			{
				if (armorSetBonus.QueryCount(context).Complete)
				{
					return armorSetBonus;
				}
			}
			foreach (ArmorSetBonus armorSetBonus2 in ArmorSetBonuses.SetsContaining[context.BodyItem])
			{
				if (armorSetBonus2.QueryCount(context).Complete)
				{
					return armorSetBonus2;
				}
			}
			return null;
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x0062D331 File Offset: 0x0062B531
		private static ArmorSetBonus.Builder Create(ArmorSetBonus.ArmorSetEffect effect, string textKey, ArmorSetBonus.PartType primaryPart = ArmorSetBonus.PartType.None)
		{
			return ArmorSetBonus.Create(effect, textKey, primaryPart);
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x0062D33B File Offset: 0x0062B53B
		public static void Add(ArmorSetBonus.ArmorSetEffect Effect, string TextKey, int Head, int Body, int Legs)
		{
			ArmorSetBonuses.Create(Effect, TextKey, ArmorSetBonus.PartType.None).Set(Head, Body, Legs).Add();
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x0062D353 File Offset: 0x0062B553
		public static void Add(ArmorSetBonus.ArmorSetEffect Effect, string TextKey, ArmorSetBonus.PartType PrimaryPart, int Head, int Body, int Legs)
		{
			ArmorSetBonuses.Create(Effect, TextKey, PrimaryPart).Set(Head, Body, Legs).Add();
		}

		// Token: 0x04005B3B RID: 23355
		public static List<ArmorSetBonus> All = new List<ArmorSetBonus>();

		// Token: 0x04005B3C RID: 23356
		public static ArmorSetBonus[][] SetsContaining;

		// Token: 0x020009AE RID: 2478
		public static class Benefits
		{
			// Token: 0x060049CA RID: 18890 RVA: 0x006D1EC3 File Offset: 0x006D00C3
			public static void Tiki(Player player)
			{
				player.maxMinions++;
				player.whipRangeMultiplier += 0.2f;
			}

			// Token: 0x060049CB RID: 18891 RVA: 0x006D1EE5 File Offset: 0x006D00E5
			public static void Spooky(Player player)
			{
				player.minionDamage += 0.25f;
			}

			// Token: 0x060049CC RID: 18892 RVA: 0x006D1EF9 File Offset: 0x006D00F9
			public static void Bee(Player player)
			{
				player.minionDamage += 0.1f;
				if (player.itemAnimation > 0 && player.inventory[player.selectedItem].type == 1121)
				{
					AchievementsHelper.HandleSpecialEvent(player, 3);
				}
			}

			// Token: 0x060049CD RID: 18893 RVA: 0x006D1F36 File Offset: 0x006D0136
			public static void Spider(Player player)
			{
				player.minionDamage += 0.12f;
			}

			// Token: 0x060049CE RID: 18894 RVA: 0x006D1F4A File Offset: 0x006D014A
			public static void Solar(Player player)
			{
				player.ApplySetBonus_Solar();
			}

			// Token: 0x060049CF RID: 18895 RVA: 0x006D1F52 File Offset: 0x006D0152
			public static void Vortex(Player player)
			{
				player.setVortex = true;
			}

			// Token: 0x060049D0 RID: 18896 RVA: 0x006D1F5B File Offset: 0x006D015B
			public static void Nebula(Player player)
			{
				if (player.nebulaCD > 0)
				{
					player.nebulaCD--;
				}
				player.setNebula = true;
			}

			// Token: 0x060049D1 RID: 18897 RVA: 0x006D1F7B File Offset: 0x006D017B
			public static void Stardust(Player player)
			{
				player.ApplySetBonus_Stardust();
			}

			// Token: 0x060049D2 RID: 18898 RVA: 0x006D1F83 File Offset: 0x006D0183
			public static void Forbidden(Player player)
			{
				player.setForbidden = true;
				player.UpdateForbiddenSetLock();
				Lighting.AddLight(player.Center, 0.8f, 0.7f, 0.2f);
			}

			// Token: 0x060049D3 RID: 18899 RVA: 0x006D1FAC File Offset: 0x006D01AC
			public static void SquireTier2(Player player)
			{
				player.setSquireT2 = true;
				player.maxTurrets++;
			}

			// Token: 0x060049D4 RID: 18900 RVA: 0x006D1FC3 File Offset: 0x006D01C3
			public static void ApprenticeTier2(Player player)
			{
				player.setApprenticeT2 = true;
				player.maxTurrets++;
			}

			// Token: 0x060049D5 RID: 18901 RVA: 0x006D1FDA File Offset: 0x006D01DA
			public static void HuntressTier2(Player player)
			{
				player.setHuntressT2 = true;
				player.maxTurrets++;
			}

			// Token: 0x060049D6 RID: 18902 RVA: 0x006D1FF1 File Offset: 0x006D01F1
			public static void MonkTier2(Player player)
			{
				player.setMonkT2 = true;
				player.maxTurrets++;
			}

			// Token: 0x060049D7 RID: 18903 RVA: 0x006D2008 File Offset: 0x006D0208
			public static void SquireTier3(Player player)
			{
				player.setSquireT3 = true;
				player.setSquireT2 = true;
				player.maxTurrets++;
			}

			// Token: 0x060049D8 RID: 18904 RVA: 0x006D2026 File Offset: 0x006D0226
			public static void ApprenticeTier3(Player player)
			{
				player.setApprenticeT3 = true;
				player.setApprenticeT2 = true;
				player.maxTurrets++;
			}

			// Token: 0x060049D9 RID: 18905 RVA: 0x006D2044 File Offset: 0x006D0244
			public static void HuntressTier3(Player player)
			{
				player.setHuntressT3 = true;
				player.setHuntressT2 = true;
				player.maxTurrets++;
			}

			// Token: 0x060049DA RID: 18906 RVA: 0x006D2062 File Offset: 0x006D0262
			public static void MonkTier3(Player player)
			{
				player.setMonkT3 = true;
				player.setMonkT2 = true;
				player.maxTurrets++;
			}

			// Token: 0x060049DB RID: 18907 RVA: 0x006D2080 File Offset: 0x006D0280
			public static void ObsidianOutlaw(Player player)
			{
				player.minionDamage += 0.15f;
				player.whipRangeMultiplier += 0.3f;
				float num = 1.15f;
				float num2 = 1f / num;
				player.whipUseTimeMultiplier *= num2;
			}

			// Token: 0x060049DC RID: 18908 RVA: 0x006D20CD File Offset: 0x006D02CD
			public static void ChlorophyteMelee(Player player)
			{
				player.AddBuff(60, 5, false);
				player.setChlorophyte = true;
				player.endurance += 0.05f;
			}

			// Token: 0x060049DD RID: 18909 RVA: 0x006D20F2 File Offset: 0x006D02F2
			public static void ChlorophyteSummon(Player player)
			{
				player.AddBuff(60, 5, false);
				player.setChlorophyte = true;
				player.maxMinions += 2;
			}

			// Token: 0x060049DE RID: 18910 RVA: 0x006D2113 File Offset: 0x006D0313
			public static void Chlorophyte(Player player)
			{
				player.AddBuff(60, 5, false);
				player.setChlorophyte = true;
			}

			// Token: 0x060049DF RID: 18911 RVA: 0x006D2126 File Offset: 0x006D0326
			public static void Angler(Player player)
			{
				player.anglerSetSpawnReduction = true;
			}

			// Token: 0x060049E0 RID: 18912 RVA: 0x006D212F File Offset: 0x006D032F
			public static void Cactus(Player player)
			{
				player.cactusThorns = true;
			}

			// Token: 0x060049E1 RID: 18913 RVA: 0x006D2138 File Offset: 0x006D0338
			public static void Turtle(Player player)
			{
				player.endurance += 0.15f;
				player.thorns = 1f;
				player.turtleThorns = true;
			}

			// Token: 0x060049E2 RID: 18914 RVA: 0x006D215E File Offset: 0x006D035E
			public static void CobaltCaster(Player player)
			{
				player.manaCost -= 0.14f;
			}

			// Token: 0x060049E3 RID: 18915 RVA: 0x006D2172 File Offset: 0x006D0372
			public static void CobaltMelee(Player player)
			{
				player.meleeSpeed += 0.15f;
			}

			// Token: 0x060049E4 RID: 18916 RVA: 0x006D2186 File Offset: 0x006D0386
			public static void CobaltRanged(Player player)
			{
				player.ammoCost80 = true;
			}

			// Token: 0x060049E5 RID: 18917 RVA: 0x006D218F File Offset: 0x006D038F
			public static void MythrilCaster(Player player)
			{
				player.manaCost -= 0.17f;
			}

			// Token: 0x060049E6 RID: 18918 RVA: 0x006D21A3 File Offset: 0x006D03A3
			public static void MythrilMelee(Player player)
			{
				player.meleeCrit += 10;
			}

			// Token: 0x060049E7 RID: 18919 RVA: 0x006D2186 File Offset: 0x006D0386
			public static void MythrilRanged(Player player)
			{
				player.ammoCost80 = true;
			}

			// Token: 0x060049E8 RID: 18920 RVA: 0x006D21B4 File Offset: 0x006D03B4
			public static void AdamantiteCaster(Player player)
			{
				player.manaCost -= 0.19f;
			}

			// Token: 0x060049E9 RID: 18921 RVA: 0x006D21C8 File Offset: 0x006D03C8
			public static void AdamantiteMelee(Player player)
			{
				player.meleeSpeed += 0.2f;
				player.moveSpeed += 0.2f;
			}

			// Token: 0x060049EA RID: 18922 RVA: 0x006D21EE File Offset: 0x006D03EE
			public static void AdamantiteRanged(Player player)
			{
				player.ammoCost75 = true;
			}

			// Token: 0x060049EB RID: 18923 RVA: 0x006D21F7 File Offset: 0x006D03F7
			public static void Palladium(Player player)
			{
				player.onHitRegen = true;
			}

			// Token: 0x060049EC RID: 18924 RVA: 0x006D2200 File Offset: 0x006D0400
			public static void Orichalcum(Player player)
			{
				player.onHitPetal = true;
			}

			// Token: 0x060049ED RID: 18925 RVA: 0x006D2209 File Offset: 0x006D0409
			public static void Titanium(Player player)
			{
				player.onHitTitaniumStorm = true;
			}

			// Token: 0x060049EE RID: 18926 RVA: 0x006D2212 File Offset: 0x006D0412
			public static void HallowedSummoner(Player player)
			{
				player.maxMinions += 2;
				player.onHitDodge = true;
			}

			// Token: 0x060049EF RID: 18927 RVA: 0x006D2229 File Offset: 0x006D0429
			public static void Hallowed(Player player)
			{
				player.onHitDodge = true;
			}

			// Token: 0x060049F0 RID: 18928 RVA: 0x006D2234 File Offset: 0x006D0434
			public static void CrystalAssassin(Player player)
			{
				player.rangedDamage += 0.1f;
				player.meleeDamage += 0.1f;
				player.magicDamage += 0.1f;
				player.minionDamage += 0.1f;
				player.rangedCrit += 10;
				player.meleeCrit += 10;
				player.magicCrit += 10;
				player.dashType = 5;
			}

			// Token: 0x060049F1 RID: 18929 RVA: 0x006D22BD File Offset: 0x006D04BD
			public static void Crimson(Player player)
			{
				player.crimsonRegen = true;
			}

			// Token: 0x060049F2 RID: 18930 RVA: 0x006D22C6 File Offset: 0x006D04C6
			public static void SpectreHealing(Player player)
			{
				player.ghostHeal = true;
				player.magicDamage -= 0.4f;
			}

			// Token: 0x060049F3 RID: 18931 RVA: 0x006D22E1 File Offset: 0x006D04E1
			public static void SpectreDamage(Player player)
			{
				player.ghostHurt = true;
			}

			// Token: 0x060049F4 RID: 18932 RVA: 0x006D22EA File Offset: 0x006D04EA
			public static void Meteor(Player player)
			{
				player.spaceGun = true;
			}

			// Token: 0x060049F5 RID: 18933 RVA: 0x006D22F3 File Offset: 0x006D04F3
			public static void Frost(Player player)
			{
				player.frostBurn = true;
				player.meleeDamage += 0.1f;
				player.rangedDamage += 0.1f;
			}

			// Token: 0x060049F6 RID: 18934 RVA: 0x006D2320 File Offset: 0x006D0520
			public static void Jungle(Player player)
			{
				player.manaCost -= 0.16f;
			}

			// Token: 0x060049F7 RID: 18935 RVA: 0x006D2334 File Offset: 0x006D0534
			public static void Molten(Player player)
			{
				player.meleeDamage += 0.1f;
				player.fireWalk = true;
				if (!player.vampireBurningInSunlight)
				{
					player.buffImmune[24] = true;
				}
			}

			// Token: 0x060049F8 RID: 18936 RVA: 0x006D2361 File Offset: 0x006D0561
			public static void Snow(Player player)
			{
				player.buffImmune[46] = true;
				player.buffImmune[47] = true;
			}

			// Token: 0x060049F9 RID: 18937 RVA: 0x006D2377 File Offset: 0x006D0577
			public static void Mining(Player player)
			{
				player.pickSpeed -= 0.1f;
			}

			// Token: 0x060049FA RID: 18938 RVA: 0x006D238B File Offset: 0x006D058B
			public static void Wizard(Player player)
			{
				player.magicCrit += 10;
			}

			// Token: 0x060049FB RID: 18939 RVA: 0x006D239C File Offset: 0x006D059C
			public static void MagicHat(Player player)
			{
				player.statManaMax2 += 60;
			}

			// Token: 0x060049FC RID: 18940 RVA: 0x006D23AD File Offset: 0x006D05AD
			public static void ShadowScale(Player player)
			{
				player.shadowArmor = true;
			}

			// Token: 0x060049FD RID: 18941 RVA: 0x006D23B6 File Offset: 0x006D05B6
			public static void BeetleDefense(Player player)
			{
				player.ApplySetBonus_BeetleDefense();
			}

			// Token: 0x060049FE RID: 18942 RVA: 0x006D23BE File Offset: 0x006D05BE
			public static void BeetleDamage(Player player)
			{
				player.ApplySetBonus_BeetleDamage();
			}

			// Token: 0x060049FF RID: 18943 RVA: 0x006D23C6 File Offset: 0x006D05C6
			public static void Gladiator(Player player)
			{
				player.noKnockback = true;
			}

			// Token: 0x06004A00 RID: 18944 RVA: 0x006D23CF File Offset: 0x006D05CF
			public static void Ninja(Player player)
			{
				player.moveSpeed += 0.2f;
			}

			// Token: 0x06004A01 RID: 18945 RVA: 0x006D2186 File Offset: 0x006D0386
			public static void Fossil(Player player)
			{
				player.ammoCost80 = true;
			}

			// Token: 0x06004A02 RID: 18946 RVA: 0x006D23E3 File Offset: 0x006D05E3
			public static void Necro(Player player)
			{
				player.rangedCrit += 10;
			}

			// Token: 0x06004A03 RID: 18947 RVA: 0x006D23F4 File Offset: 0x006D05F4
			public static void Pumpkin(Player player)
			{
				player.meleeDamage += 0.1f;
				player.magicDamage += 0.1f;
				player.rangedDamage += 0.1f;
				player.minionDamage += 0.1f;
			}

			// Token: 0x06004A04 RID: 18948 RVA: 0x006D2449 File Offset: 0x006D0649
			public static void Platinum(Player player)
			{
				player.statDefense += 4;
			}

			// Token: 0x06004A05 RID: 18949 RVA: 0x006D2459 File Offset: 0x006D0659
			public static void MetalTier2(Player player)
			{
				player.statDefense += 3;
			}

			// Token: 0x06004A06 RID: 18950 RVA: 0x006D2469 File Offset: 0x006D0669
			public static void MetalTier1(Player player)
			{
				player.statDefense += 2;
			}

			// Token: 0x06004A07 RID: 18951 RVA: 0x006D2479 File Offset: 0x006D0679
			public static void Shroomite(Player player)
			{
				player.shroomiteStealth = true;
			}

			// Token: 0x06004A08 RID: 18952 RVA: 0x006D2482 File Offset: 0x006D0682
			public static void Wood(Player player)
			{
				player.statDefense++;
			}

			// Token: 0x06004A09 RID: 18953 RVA: 0x006D2492 File Offset: 0x006D0692
			public static void AshWood(Player player)
			{
				player.ashWoodBonus = true;
			}
		}
	}
}
