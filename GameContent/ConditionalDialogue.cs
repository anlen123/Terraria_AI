using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent
{
	// Token: 0x0200023D RID: 573
	public abstract class ConditionalDialogue
	{
		// Token: 0x0600227A RID: 8826 RVA: 0x0053859C File Offset: 0x0053679C
		private static void Register(int npcType, ConditionalDialogue dialogue)
		{
			List<ConditionalDialogue> list = ConditionalDialogue._registry[npcType];
			if (list == null)
			{
				list = (ConditionalDialogue._registry[npcType] = new List<ConditionalDialogue>());
			}
			list.Add(dialogue);
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x005385CC File Offset: 0x005367CC
		public static bool TryGetPendingDialogue(NPC npc, out ConditionalDialogue dialogue)
		{
			dialogue = null;
			List<ConditionalDialogue> list = ConditionalDialogue._registry[npc.type];
			if (list == null)
			{
				return false;
			}
			foreach (ConditionalDialogue conditionalDialogue in list)
			{
				if (conditionalDialogue.ConditionsMet(npc))
				{
					dialogue = conditionalDialogue;
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x0600227C RID: 8828 RVA: 0x00538640 File Offset: 0x00536840
		// (set) Token: 0x0600227D RID: 8829 RVA: 0x00538648 File Offset: 0x00536848
		public bool ShowIndicator { get; private set; }

		// Token: 0x0600227E RID: 8830 RVA: 0x00538651 File Offset: 0x00536851
		public ConditionalDialogue(Predicate<NPC> condition = null)
		{
			this.ShowIndicator = true;
			Predicate<NPC> conditionsMet = condition;
			if (condition == null && (conditionsMet = ConditionalDialogue.<>c.<>9__9_0) == null)
			{
				conditionsMet = (ConditionalDialogue.<>c.<>9__9_0 = ((NPC _) => true));
			}
			this.ConditionsMet = conditionsMet;
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x0053868A File Offset: 0x0053688A
		public void HideIndicator()
		{
			this.ShowIndicator = false;
		}

		// Token: 0x06002280 RID: 8832
		public abstract string GetChatAndClearCondition(NPC npc);

		// Token: 0x06002281 RID: 8833 RVA: 0x00538693 File Offset: 0x00536893
		public void Register(int npcType)
		{
			ConditionalDialogue.Register(npcType, this);
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x0053869C File Offset: 0x0053689C
		internal static void Init()
		{
			new ConditionalDialogue.FreeCakeDialogue().Register(208);
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x005386AD File Offset: 0x005368AD
		public static Predicate<NPC> CreateInventoryCondition(RecipeGroup item, int stack)
		{
			return ConditionalDialogue.CreateInventoryCondition(new Recipe.RequiredItemEntry[]
			{
				new Recipe.RequiredItemEntry(item, stack)
			});
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x005386C8 File Offset: 0x005368C8
		public static Predicate<NPC> CreateInventoryCondition(params Recipe.RequiredItemEntry[] requiredItems)
		{
			return (NPC _) => Recipe.CollectedEnoughItemsToCraft(requiredItems);
		}

		// Token: 0x04004CE4 RID: 19684
		private static List<ConditionalDialogue>[] _registry = new List<ConditionalDialogue>[(int)NPCID.Count];

		// Token: 0x04004CE6 RID: 19686
		public readonly Predicate<NPC> ConditionsMet;

		// Token: 0x020007C4 RID: 1988
		public static class ItemGroups
		{
			// Token: 0x06004206 RID: 16902 RVA: 0x006BC3C8 File Offset: 0x006BA5C8
			internal static void PostSetupContent()
			{
				foreach (Item item in ContentSamples.ItemsByType.Values)
				{
					if (ProjectileID.Sets.IsAWhip[item.shoot])
					{
						ConditionalDialogue.ItemGroups.Whips.Add(item.type, null);
					}
				}
				foreach (Item item2 in ContentSamples.ItemsByType.Values)
				{
					if (item2.mountType != -1)
					{
						ConditionalDialogue.ItemGroups.Mounts.Add(item2.type, null);
					}
				}
			}

			// Token: 0x040070AC RID: 28844
			public static RecipeGroup Ore = new RecipeGroup("RecipeGroups.Ore", new int[]
			{
				699,
				12,
				11,
				700,
				14,
				701,
				13,
				702
			});

			// Token: 0x040070AD RID: 28845
			public static RecipeGroup Bars = new RecipeGroup("RecipeGroups.Bar", new int[]
			{
				703,
				20,
				22,
				704,
				21,
				705,
				19,
				706
			});

			// Token: 0x040070AE RID: 28846
			public static RecipeGroup Anvils = new RecipeGroup("ItemName.IronAnvil", new int[]
			{
				35,
				716
			});

			// Token: 0x040070AF RID: 28847
			public static RecipeGroup Whips = new RecipeGroup("RecipeGroups.Whip", new int[0]);

			// Token: 0x040070B0 RID: 28848
			public static RecipeGroup Mounts = new RecipeGroup("RecipeGroups.Mount", new int[0]);
		}

		// Token: 0x020007C5 RID: 1989
		private class FreeCakeDialogue : ConditionalDialogue
		{
			// Token: 0x06004208 RID: 16904 RVA: 0x006BC52D File Offset: 0x006BA72D
			public FreeCakeDialogue() : base((NPC _) => NPC.freeCake)
			{
			}

			// Token: 0x06004209 RID: 16905 RVA: 0x006BC554 File Offset: 0x006BA754
			public override string GetChatAndClearCondition(NPC npc)
			{
				NPC.freeCake = false;
				NetMessage.SendData(51, -1, -1, null, 0, 10f, 0f, 0f, 0, 0, 0);
				Item item = new Item();
				item.SetDefaults(3750, null);
				Main.LocalPlayer.QuickSpawnItem(new EntitySource_Gift(npc), item, GetItemSettings.GiftRecieved);
				return Language.GetTextValue("PartyGirlSpecialText.Cake" + Main.rand.Next(1, 4));
			}
		}
	}
}
