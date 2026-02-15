using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Localization;

namespace Terraria.GameContent
{
	// Token: 0x0200023E RID: 574
	public class OneTimeDialogue : ConditionalDialogue
	{
		// Token: 0x06002286 RID: 8838 RVA: 0x005386F4 File Offset: 0x005368F4
		public OneTimeDialogue(string key, Predicate<NPC> condition = null) : base((NPC npc) => !Main.LocalPlayer.oneTimeDialoguesSeen.Contains(key) && (condition == null || condition(npc)))
		{
			this.ChatText = Language.GetText(key);
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x00538744 File Offset: 0x00536944
		public override string GetChatAndClearCondition(NPC npc)
		{
			Player localPlayer = Main.LocalPlayer;
			localPlayer.oneTimeDialoguesSeen.Add(this.ChatText.Key);
			foreach (Item item in this.Rewards)
			{
				localPlayer.QuickSpawnItem(new EntitySource_Gift(npc), item, GetItemSettings.GiftRecieved);
			}
			return this.ChatText.Value;
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x005387CC File Offset: 0x005369CC
		public OneTimeDialogue WithReward(int itemId, int stack = 1)
		{
			Item item = new Item();
			item.SetDefaults(itemId, null);
			item.stack = stack;
			return this.WithRewards(new Item[]
			{
				item
			});
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x005387FE File Offset: 0x005369FE
		public OneTimeDialogue WithRewards(params Item[] rewards)
		{
			this.Rewards.AddRange(rewards);
			return this;
		}

		// Token: 0x04004CE7 RID: 19687
		public readonly LocalizedText ChatText;

		// Token: 0x04004CE8 RID: 19688
		public readonly List<Item> Rewards = new List<Item>();
	}
}
