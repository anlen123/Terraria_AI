using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent.Items
{
	// Token: 0x02000476 RID: 1142
	public static class ItemVariants
	{
		// Token: 0x06003303 RID: 13059 RVA: 0x005F24B8 File Offset: 0x005F06B8
		public static IEnumerable<ItemVariants.VariantEntry> GetVariants(int itemId)
		{
			if (!ItemVariants._variants.IndexInRange(itemId))
			{
				return Enumerable.Empty<ItemVariants.VariantEntry>();
			}
			IEnumerable<ItemVariants.VariantEntry> enumerable = ItemVariants._variants[itemId];
			return enumerable ?? Enumerable.Empty<ItemVariants.VariantEntry>();
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x005F24EC File Offset: 0x005F06EC
		private static ItemVariants.VariantEntry GetEntry(int itemId, ItemVariant variant)
		{
			return ItemVariants.GetVariants(itemId).SingleOrDefault((ItemVariants.VariantEntry v) => v.Variant == variant);
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x005F2520 File Offset: 0x005F0720
		public static void AddVariant(int itemId, ItemVariant variant, params ItemVariantCondition[] conditions)
		{
			ItemVariants.VariantEntry variantEntry = ItemVariants.GetEntry(itemId, variant);
			if (variantEntry == null)
			{
				List<ItemVariants.VariantEntry> list = ItemVariants._variants[itemId];
				if (list == null)
				{
					list = (ItemVariants._variants[itemId] = new List<ItemVariants.VariantEntry>());
				}
				list.Add(variantEntry = new ItemVariants.VariantEntry(variant));
			}
			variantEntry.AddConditions(conditions);
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x005F2566 File Offset: 0x005F0766
		public static bool HasVariant(int itemId, ItemVariant variant)
		{
			return ItemVariants.GetEntry(itemId, variant) != null;
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x005F2574 File Offset: 0x005F0774
		public static ItemVariant SelectVariant(int itemId)
		{
			if (!ItemVariants._variants.IndexInRange(itemId))
			{
				return null;
			}
			List<ItemVariants.VariantEntry> list = ItemVariants._variants[itemId];
			if (list == null)
			{
				return null;
			}
			foreach (ItemVariants.VariantEntry variantEntry in list)
			{
				if (variantEntry.AnyConditionMet())
				{
					return variantEntry.Variant;
				}
			}
			return null;
		}

		// Token: 0x06003308 RID: 13064 RVA: 0x005F25EC File Offset: 0x005F07EC
		static ItemVariants()
		{
			ItemVariants.AddVariant(112, ItemVariants.StrongerVariant, new ItemVariantCondition[]
			{
				ItemVariants.RemixWorld
			});
			ItemVariants.AddVariant(157, ItemVariants.StrongerVariant, new ItemVariantCondition[]
			{
				ItemVariants.RemixWorld
			});
			ItemVariants.AddVariant(1319, ItemVariants.StrongerVariant, new ItemVariantCondition[]
			{
				ItemVariants.RemixWorld
			});
			ItemVariants.AddVariant(1325, ItemVariants.StrongerVariant, new ItemVariantCondition[]
			{
				ItemVariants.RemixWorld
			});
			ItemVariants.AddVariant(3069, ItemVariants.StrongerVariant, new ItemVariantCondition[]
			{
				ItemVariants.RemixWorld
			});
			ItemVariants.AddVariant(5147, ItemVariants.StrongerVariant, new ItemVariantCondition[]
			{
				ItemVariants.RemixWorld
			});
			ItemVariants.AddVariant(517, ItemVariants.WeakerVariant, new ItemVariantCondition[]
			{
				ItemVariants.RemixWorld
			});
			ItemVariants.AddVariant(683, ItemVariants.WeakerVariant, new ItemVariantCondition[]
			{
				ItemVariants.RemixWorld
			});
			ItemVariants.AddVariant(725, ItemVariants.WeakerVariant, new ItemVariantCondition[]
			{
				ItemVariants.RemixWorld
			});
			ItemVariants.AddVariant(1314, ItemVariants.WeakerVariant, new ItemVariantCondition[]
			{
				ItemVariants.RemixWorld
			});
			ItemVariants.AddVariant(2623, ItemVariants.WeakerVariant, new ItemVariantCondition[]
			{
				ItemVariants.RemixWorld
			});
			ItemVariants.AddVariant(5279, ItemVariants.WeakerVariant, new ItemVariantCondition[]
			{
				ItemVariants.RemixWorld
			});
			ItemVariants.AddVariant(5280, ItemVariants.WeakerVariant, new ItemVariantCondition[]
			{
				ItemVariants.RemixWorld
			});
			ItemVariants.AddVariant(5281, ItemVariants.WeakerVariant, new ItemVariantCondition[]
			{
				ItemVariants.RemixWorld
			});
			ItemVariants.AddVariant(5282, ItemVariants.WeakerVariant, new ItemVariantCondition[]
			{
				ItemVariants.RemixWorld
			});
			ItemVariants.AddVariant(5283, ItemVariants.WeakerVariant, new ItemVariantCondition[]
			{
				ItemVariants.RemixWorld
			});
			ItemVariants.AddVariant(5284, ItemVariants.WeakerVariant, new ItemVariantCondition[]
			{
				ItemVariants.RemixWorld
			});
			ItemVariants.AddVariant(197, ItemVariants.RebalancedVariant, new ItemVariantCondition[]
			{
				ItemVariants.GetGoodWorld
			});
			ItemVariants.AddVariant(4060, ItemVariants.RebalancedVariant, new ItemVariantCondition[]
			{
				ItemVariants.GetGoodWorld
			});
			ItemVariants.AddVariant(556, ItemVariants.DisabledBossSummonVariant, new ItemVariantCondition[]
			{
				ItemVariants.MechdusaWorld
			});
			ItemVariants.AddVariant(557, ItemVariants.DisabledBossSummonVariant, new ItemVariantCondition[]
			{
				ItemVariants.MechdusaWorld
			});
			ItemVariants.AddVariant(544, ItemVariants.DisabledBossSummonVariant, new ItemVariantCondition[]
			{
				ItemVariants.MechdusaWorld
			});
			ItemVariants.AddVariant(5334, ItemVariants.EnabledVariant, new ItemVariantCondition[]
			{
				ItemVariants.MechdusaWorld
			});
		}

		// Token: 0x04005855 RID: 22613
		private static List<ItemVariants.VariantEntry>[] _variants = new List<ItemVariants.VariantEntry>[(int)ItemID.Count];

		// Token: 0x04005856 RID: 22614
		public static ItemVariant StrongerVariant = new ItemVariant(NetworkText.FromKey("ItemVariant.Stronger", new object[0]));

		// Token: 0x04005857 RID: 22615
		public static ItemVariant WeakerVariant = new ItemVariant(NetworkText.FromKey("ItemVariant.Weaker", new object[0]));

		// Token: 0x04005858 RID: 22616
		public static ItemVariant RebalancedVariant = new ItemVariant(NetworkText.FromKey("ItemVariant.Rebalanced", new object[0]));

		// Token: 0x04005859 RID: 22617
		public static ItemVariant EnabledVariant = new ItemVariant(NetworkText.FromKey("ItemVariant.Enabled", new object[0]));

		// Token: 0x0400585A RID: 22618
		public static ItemVariant DisabledBossSummonVariant = new ItemVariant(NetworkText.FromKey("ItemVariant.DisabledBossSummon", new object[0]));

		// Token: 0x0400585B RID: 22619
		public static ItemVariantCondition RemixWorld = new ItemVariantCondition(NetworkText.FromKey("ItemVariantCondition.RemixWorld", new object[0]), () => Main.remixWorld);

		// Token: 0x0400585C RID: 22620
		public static ItemVariantCondition GetGoodWorld = new ItemVariantCondition(NetworkText.FromKey("ItemVariantCondition.GetGoodWorld", new object[0]), () => Main.getGoodWorld);

		// Token: 0x0400585D RID: 22621
		public static ItemVariantCondition MechdusaWorld = new ItemVariantCondition(NetworkText.FromKey("ItemVariantCondition.MechdusaWorld", new object[0]), () => SpecialSeedFeatures.Mechdusa);

		// Token: 0x0200096F RID: 2415
		public class VariantEntry
		{
			// Token: 0x1700058B RID: 1419
			// (get) Token: 0x060048F3 RID: 18675 RVA: 0x006CF1BF File Offset: 0x006CD3BF
			public IEnumerable<ItemVariantCondition> Conditions
			{
				get
				{
					return this._conditions;
				}
			}

			// Token: 0x060048F4 RID: 18676 RVA: 0x006CF1C7 File Offset: 0x006CD3C7
			public VariantEntry(ItemVariant variant)
			{
				this.Variant = variant;
			}

			// Token: 0x060048F5 RID: 18677 RVA: 0x006CF1E1 File Offset: 0x006CD3E1
			internal void AddConditions(IEnumerable<ItemVariantCondition> conditions)
			{
				this._conditions.AddRange(conditions);
			}

			// Token: 0x060048F6 RID: 18678 RVA: 0x006CF1EF File Offset: 0x006CD3EF
			public bool AnyConditionMet()
			{
				return this.Conditions.Any((ItemVariantCondition c) => c.IsMet());
			}

			// Token: 0x040075A8 RID: 30120
			public readonly ItemVariant Variant;

			// Token: 0x040075A9 RID: 30121
			private readonly List<ItemVariantCondition> _conditions = new List<ItemVariantCondition>();
		}
	}
}
