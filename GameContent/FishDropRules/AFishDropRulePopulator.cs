using System;
using System.Linq;

namespace Terraria.GameContent.FishDropRules
{
	// Token: 0x0200047A RID: 1146
	public abstract class AFishDropRulePopulator
	{
		// Token: 0x06003312 RID: 13074 RVA: 0x005F2B8C File Offset: 0x005F0D8C
		public AFishDropRulePopulator(FishDropRuleList list)
		{
			this._list = list;
		}

		// Token: 0x06003313 RID: 13075 RVA: 0x005F31B8 File Offset: 0x005F13B8
		protected void Add(FishRarityCondition tier, int chanceNominator, int chanceDenominator, int[] itemTypes, params AFishingCondition[] conditions)
		{
			FishDropRule rule = new FishDropRule
			{
				PossibleItems = itemTypes,
				ChanceNumerator = chanceNominator,
				ChanceDenominator = chanceDenominator,
				Rarity = tier,
				Conditions = conditions
			};
			this._list.Add(rule);
		}

		// Token: 0x06003314 RID: 13076 RVA: 0x005F31FC File Offset: 0x005F13FC
		protected void Add(FishRarityCondition tier, int chanceNominator, int chanceDenominator, int itemType, params AFishingCondition[] conditions)
		{
			this.Add(tier, chanceNominator, chanceDenominator, this.Group(new int[]
			{
				itemType
			}), conditions);
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x005F3225 File Offset: 0x005F1425
		protected void Add(FishRarityCondition tier, int chanceDenominator, int[] itemTypes, params AFishingCondition[] conditions)
		{
			this.Add(tier, 1, chanceDenominator, itemTypes, conditions);
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x005F3234 File Offset: 0x005F1434
		protected void Add(FishRarityCondition tier, int chanceDenominator, int itemType, params AFishingCondition[] conditions)
		{
			this.Add(tier, 1, chanceDenominator, this.Group(new int[]
			{
				itemType
			}), conditions);
		}

		// Token: 0x06003317 RID: 13079 RVA: 0x005F325C File Offset: 0x005F145C
		protected void AddQuestFish(FishRarityCondition tier, int chanceDenominator, int itemType, params AFishingCondition[] conditions)
		{
			FishingConditions.QuestFishCondition questFishCondition = new FishingConditions.QuestFishCondition
			{
				CheckedType = itemType
			};
			this.Add(tier, 1, chanceDenominator, this.Group(new int[]
			{
				itemType
			}), this.Join(conditions, new AFishingCondition[]
			{
				questFishCondition
			}));
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x005F32A4 File Offset: 0x005F14A4
		protected void AddQuestFishForRemix(FishRarityCondition tier, int chanceDenominator, int itemType, params AFishingCondition[] conditions)
		{
			FishingConditions.QuestFishConditionRemix questFishConditionRemix = new FishingConditions.QuestFishConditionRemix
			{
				CheckedType = itemType
			};
			this.Add(tier, 1, chanceDenominator, this.Group(new int[]
			{
				itemType
			}), this.Join(conditions, new AFishingCondition[]
			{
				questFishConditionRemix
			}));
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x005F32EC File Offset: 0x005F14EC
		protected void AddWithHardmode(FishRarityCondition tier, int chanceDenominator, int itemTypeEarly, int itemTypeHard, params AFishingCondition[] conditions)
		{
			FishDropRule rule = new FishDropRule
			{
				PossibleItems = new int[]
				{
					itemTypeEarly
				},
				ChanceNumerator = 1,
				ChanceDenominator = chanceDenominator,
				Rarity = tier,
				Conditions = this.Join(conditions, new AFishingCondition[]
				{
					this.EarlyMode
				})
			};
			this._list.Add(rule);
			FishDropRule rule2 = new FishDropRule
			{
				PossibleItems = new int[]
				{
					itemTypeHard
				},
				ChanceNumerator = 1,
				ChanceDenominator = chanceDenominator,
				Rarity = tier,
				Conditions = this.Join(conditions, new AFishingCondition[]
				{
					this.HardMode
				})
			};
			this._list.Add(rule2);
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x005F33A6 File Offset: 0x005F15A6
		protected void AddStopper(AFishingCondition condition)
		{
			this.Add(AFishDropRulePopulator.Rarity.Any, 1, new int[0], new AFishingCondition[]
			{
				condition
			});
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x001FC399 File Offset: 0x001FA599
		public int[] Group(params int[] itemTypes)
		{
			return itemTypes;
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x005F33C4 File Offset: 0x005F15C4
		protected AFishingCondition[] Join(AFishingCondition[] original, params AFishingCondition[] additions)
		{
			return original.Concat(additions).ToArray<AFishingCondition>();
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x001FC399 File Offset: 0x001FA599
		protected AFishingCondition[] Join(params AFishingCondition[] additions)
		{
			return additions;
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x005F33D2 File Offset: 0x005F15D2
		private static bool IsHardmode(bool state)
		{
			return Main.hardMode == state;
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x005F33DC File Offset: 0x005F15DC
		private static bool IsOriginalOcean(FishingContext context)
		{
			return context.Fisher.heightLevel <= 1 && (context.Fisher.X < 380 || context.Fisher.X > Main.maxTilesX - 380) && context.Fisher.waterTilesCount > 1000;
		}

		// Token: 0x04005866 RID: 22630
		private FishDropRuleList _list;

		// Token: 0x04005867 RID: 22631
		protected AFishingCondition HardMode = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => AFishDropRulePopulator.IsHardmode(true));

		// Token: 0x04005868 RID: 22632
		protected AFishingCondition EarlyMode = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => AFishDropRulePopulator.IsHardmode(false));

		// Token: 0x04005869 RID: 22633
		protected AFishingCondition InLava = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Fisher.inLava);

		// Token: 0x0400586A RID: 22634
		protected AFishingCondition InHoney = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Fisher.inHoney);

		// Token: 0x0400586B RID: 22635
		protected AFishingCondition Junk = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Fisher.junk);

		// Token: 0x0400586C RID: 22636
		protected AFishingCondition Crate = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Fisher.crate);

		// Token: 0x0400586D RID: 22637
		protected AFishingCondition AnyEnemies = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Fisher.rolledEnemySpawn > 0);

		// Token: 0x0400586E RID: 22638
		protected AFishingCondition CanFishInLava = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Fisher.CanFishInLava);

		// Token: 0x0400586F RID: 22639
		protected AFishingCondition Dungeon = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Player.ZoneDungeon && NPC.downedBoss3);

		// Token: 0x04005870 RID: 22640
		protected AFishingCondition Beach = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Player.ZoneBeach);

		// Token: 0x04005871 RID: 22641
		protected AFishingCondition Hallow = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Player.ZoneHallow);

		// Token: 0x04005872 RID: 22642
		protected AFishingCondition GlowingMushrooms = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Player.ZoneGlowshroom);

		// Token: 0x04005873 RID: 22643
		protected AFishingCondition TrueDesert = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Player.ZoneDesert);

		// Token: 0x04005874 RID: 22644
		protected AFishingCondition TrueSnow = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Player.ZoneSnow);

		// Token: 0x04005875 RID: 22645
		protected AFishingCondition Remix = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => Main.remixWorld);

		// Token: 0x04005876 RID: 22646
		protected AFishingCondition Height1 = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel == 1);

		// Token: 0x04005877 RID: 22647
		protected AFishingCondition Height1And2 = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel == 1 || context.Fisher.heightLevel == 2);

		// Token: 0x04005878 RID: 22648
		protected AFishingCondition HeightAbove1 = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel > 1);

		// Token: 0x04005879 RID: 22649
		protected AFishingCondition HeightAboveAnd1 = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel >= 1);

		// Token: 0x0400587A RID: 22650
		protected AFishingCondition HeightUnder2 = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel < 2);

		// Token: 0x0400587B RID: 22651
		protected AFishingCondition HeightAbove2 = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel > 2);

		// Token: 0x0400587C RID: 22652
		protected AFishingCondition Height0 = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel == 0);

		// Token: 0x0400587D RID: 22653
		protected AFishingCondition Height2 = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel == 2);

		// Token: 0x0400587E RID: 22654
		protected AFishingCondition Height3 = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Fisher.heightLevel == 3);

		// Token: 0x0400587F RID: 22655
		protected AFishingCondition UnderRockLayer = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => (double)context.Fisher.Y >= Main.rockLayer);

		// Token: 0x04005880 RID: 22656
		protected AFishingCondition Corruption = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.RolledCorruption);

		// Token: 0x04005881 RID: 22657
		protected AFishingCondition Crimson = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.RolledCrimson);

		// Token: 0x04005882 RID: 22658
		protected AFishingCondition Jungle = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.RolledJungle);

		// Token: 0x04005883 RID: 22659
		protected AFishingCondition Snow = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.RolledSnow);

		// Token: 0x04005884 RID: 22660
		protected AFishingCondition Desert = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.RolledDesert);

		// Token: 0x04005885 RID: 22661
		protected AFishingCondition RolledHallowDesert = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.RolledInfectedDesert && context.Player.ZoneHallow);

		// Token: 0x04005886 RID: 22662
		protected AFishingCondition OriginalOcean = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => AFishDropRulePopulator.IsOriginalOcean(context));

		// Token: 0x04005887 RID: 22663
		protected AFishingCondition RemixOcean = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.RolledRemixOcean);

		// Token: 0x04005888 RID: 22664
		protected AFishingCondition Ocean = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.RolledRemixOcean || AFishDropRulePopulator.IsOriginalOcean(context));

		// Token: 0x04005889 RID: 22665
		protected AFishingCondition Water1000 = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => context.Fisher.waterTilesCount > 1000);

		// Token: 0x0400588A RID: 22666
		protected AFishingCondition BloodMoon = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => Main.bloodMoon);

		// Token: 0x0400588B RID: 22667
		protected AFishingCondition DidNotUseCombatBook = new AFishDropRulePopulator.DelegateFishingCondition((FishingContext context) => !NPC.combatBookWasUsed);

		// Token: 0x02000972 RID: 2418
		private class DelegateFishingCondition : AFishingCondition
		{
			// Token: 0x060048FE RID: 18686 RVA: 0x006CF23E File Offset: 0x006CD43E
			public DelegateFishingCondition(AFishDropRulePopulator.DelegateFishingCondition.MatchCondition innerCondition)
			{
				this._condition = innerCondition;
			}

			// Token: 0x060048FF RID: 18687 RVA: 0x006CF24D File Offset: 0x006CD44D
			public override bool Matches(FishingContext context)
			{
				return this._condition(context);
			}

			// Token: 0x040075AC RID: 30124
			private AFishDropRulePopulator.DelegateFishingCondition.MatchCondition _condition;

			// Token: 0x02000AE6 RID: 2790
			// (Invoke) Token: 0x06004CF3 RID: 19699
			public delegate bool MatchCondition(FishingContext context);
		}

		// Token: 0x02000973 RID: 2419
		private class DelegateFishingRarityCondition : FishRarityCondition
		{
			// Token: 0x06004900 RID: 18688 RVA: 0x006CF25B File Offset: 0x006CD45B
			public DelegateFishingRarityCondition(AFishDropRulePopulator.DelegateFishingRarityCondition.MatchCondition innerCondition)
			{
				this._condition = innerCondition;
			}

			// Token: 0x06004901 RID: 18689 RVA: 0x006CF26A File Offset: 0x006CD46A
			public override bool Matches(FishingContext context)
			{
				return this._condition(context);
			}

			// Token: 0x040075AD RID: 30125
			private AFishDropRulePopulator.DelegateFishingRarityCondition.MatchCondition _condition;

			// Token: 0x02000AE7 RID: 2791
			// (Invoke) Token: 0x06004CF7 RID: 19703
			public delegate bool MatchCondition(FishingContext context);
		}

		// Token: 0x02000974 RID: 2420
		protected class Rarity
		{
			// Token: 0x040075AE RID: 30126
			public static FishRarityCondition Any = new AFishDropRulePopulator.DelegateFishingRarityCondition((FishingContext context) => true)
			{
				HackedIsAny = true,
				FrequencyOfAppearanceForVisuals = 1f
			};

			// Token: 0x040075AF RID: 30127
			public static FishRarityCondition Legendary = new AFishDropRulePopulator.DelegateFishingRarityCondition((FishingContext context) => context.Fisher.legendary)
			{
				FrequencyOfAppearanceForVisuals = 0.1f
			};

			// Token: 0x040075B0 RID: 30128
			public static FishRarityCondition VeryRare = new AFishDropRulePopulator.DelegateFishingRarityCondition((FishingContext context) => context.Fisher.veryrare)
			{
				FrequencyOfAppearanceForVisuals = 0.25f
			};

			// Token: 0x040075B1 RID: 30129
			public static FishRarityCondition Rare = new AFishDropRulePopulator.DelegateFishingRarityCondition((FishingContext context) => context.Fisher.rare)
			{
				FrequencyOfAppearanceForVisuals = 0.4f
			};

			// Token: 0x040075B2 RID: 30130
			public static FishRarityCondition Uncommon = new AFishDropRulePopulator.DelegateFishingRarityCondition((FishingContext context) => context.Fisher.uncommon)
			{
				FrequencyOfAppearanceForVisuals = 0.8f
			};

			// Token: 0x040075B3 RID: 30131
			public static FishRarityCondition Common = new AFishDropRulePopulator.DelegateFishingRarityCondition((FishingContext context) => context.Fisher.common)
			{
				FrequencyOfAppearanceForVisuals = 1f
			};

			// Token: 0x040075B4 RID: 30132
			public static FishRarityCondition BombRarityOfNotLegendaryAndNotVeryRareAndUncommon = new AFishDropRulePopulator.DelegateFishingRarityCondition((FishingContext context) => !context.Fisher.legendary && !context.Fisher.veryrare && context.Fisher.uncommon)
			{
				FrequencyOfAppearanceForVisuals = 0.6f
			};

			// Token: 0x040075B5 RID: 30133
			public static FishRarityCondition UncommonOrCommon = new AFishDropRulePopulator.DelegateFishingRarityCondition((FishingContext context) => context.Fisher.uncommon || context.Fisher.common)
			{
				FrequencyOfAppearanceForVisuals = 1f
			};
		}
	}
}
