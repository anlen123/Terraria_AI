using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000337 RID: 823
	public class BestiaryEntry
	{
		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06002822 RID: 10274 RVA: 0x00571764 File Offset: 0x0056F964
		// (set) Token: 0x06002823 RID: 10275 RVA: 0x0057176C File Offset: 0x0056F96C
		public List<IBestiaryInfoElement> Info { get; private set; }

		// Token: 0x06002824 RID: 10276 RVA: 0x00571775 File Offset: 0x0056F975
		public BestiaryEntry()
		{
			this.Info = new List<IBestiaryInfoElement>();
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x00571788 File Offset: 0x0056F988
		public static BestiaryEntry Enemy(int npcNetId)
		{
			NPC npc = ContentSamples.NpcsByNetId[npcNetId];
			List<IBestiaryInfoElement> list = new List<IBestiaryInfoElement>
			{
				new NPCNetIdBestiaryInfoElement(npcNetId),
				new NamePlateInfoElement(Lang.GetNPCName(npcNetId).Key, npcNetId),
				new NPCPortraitInfoElement(new int?(ContentSamples.NpcBestiaryRarityStars[npcNetId])),
				new NPCKillCounterInfoElement(npcNetId)
			};
			list.Add(new NPCStatsReportInfoElement(npcNetId));
			if (npc.rarity != 0)
			{
				list.Add(new RareSpawnBestiaryInfoElement(npc.rarity));
			}
			IBestiaryUICollectionInfoProvider uiinfoProvider;
			if (npc.boss || NPCID.Sets.ShouldBeCountedAsBossForBestiary[npc.type])
			{
				list.Add(new BossBestiaryInfoElement());
				uiinfoProvider = new CommonEnemyUICollectionInfoProvider(npc.GetBestiaryCreditId(), true);
			}
			else
			{
				uiinfoProvider = new CommonEnemyUICollectionInfoProvider(npc.GetBestiaryCreditId(), false);
			}
			string text = Lang.GetNPCName(npc.netID).Key;
			text = text.Replace("NPCName.", "");
			string text2 = "Bestiary_FlavorText.npc_" + text;
			if (Language.Exists(text2))
			{
				list.Add(new FlavorTextBestiaryInfoElement(text2));
			}
			return new BestiaryEntry
			{
				Icon = new UnlockableNPCEntryIcon(npcNetId, 0f, 0f, 0f, 0f, null),
				Info = list,
				UIInfoProvider = uiinfoProvider
			};
		}

		// Token: 0x06002826 RID: 10278 RVA: 0x005718CC File Offset: 0x0056FACC
		public static BestiaryEntry TownNPC(int npcNetId)
		{
			NPC npc = ContentSamples.NpcsByNetId[npcNetId];
			List<IBestiaryInfoElement> list = new List<IBestiaryInfoElement>
			{
				new NPCNetIdBestiaryInfoElement(npcNetId),
				new NamePlateInfoElement(Lang.GetNPCName(npcNetId).Key, npcNetId),
				new NPCPortraitInfoElement(new int?(ContentSamples.NpcBestiaryRarityStars[npcNetId])),
				new NPCKillCounterInfoElement(npcNetId)
			};
			string text = Lang.GetNPCName(npc.netID).Key;
			text = text.Replace("NPCName.", "");
			string text2 = "Bestiary_FlavorText.npc_" + text;
			if (Language.Exists(text2))
			{
				list.Add(new FlavorTextBestiaryInfoElement(text2));
			}
			return new BestiaryEntry
			{
				Icon = new UnlockableNPCEntryIcon(npcNetId, 0f, 0f, 0f, 0f, null),
				Info = list,
				UIInfoProvider = new TownNPCUICollectionInfoProvider(npc.GetBestiaryCreditId())
			};
		}

		// Token: 0x06002827 RID: 10279 RVA: 0x005719B8 File Offset: 0x0056FBB8
		public static BestiaryEntry Critter(int npcNetId)
		{
			NPC npc = ContentSamples.NpcsByNetId[npcNetId];
			List<IBestiaryInfoElement> list = new List<IBestiaryInfoElement>
			{
				new NPCNetIdBestiaryInfoElement(npcNetId),
				new NamePlateInfoElement(Lang.GetNPCName(npcNetId).Key, npcNetId),
				new NPCPortraitInfoElement(new int?(ContentSamples.NpcBestiaryRarityStars[npcNetId])),
				new NPCKillCounterInfoElement(npcNetId)
			};
			string text = Lang.GetNPCName(npc.netID).Key;
			text = text.Replace("NPCName.", "");
			string text2 = "Bestiary_FlavorText.npc_" + text;
			if (Language.Exists(text2))
			{
				list.Add(new FlavorTextBestiaryInfoElement(text2));
			}
			return new BestiaryEntry
			{
				Icon = new UnlockableNPCEntryIcon(npcNetId, 0f, 0f, 0f, 0f, null),
				Info = list,
				UIInfoProvider = new CritterUICollectionInfoProvider(npc.GetBestiaryCreditId())
			};
		}

		// Token: 0x06002828 RID: 10280 RVA: 0x00571AA1 File Offset: 0x0056FCA1
		public static BestiaryEntry Biome(string nameLanguageKey, string texturePath, Func<bool> unlockCondition)
		{
			return new BestiaryEntry
			{
				Icon = new CustomEntryIcon(nameLanguageKey, texturePath, unlockCondition),
				Info = new List<IBestiaryInfoElement>()
			};
		}

		// Token: 0x06002829 RID: 10281 RVA: 0x00571AC1 File Offset: 0x0056FCC1
		public void AddTags(params IBestiaryInfoElement[] elements)
		{
			this.Info.AddRange(elements);
		}

		// Token: 0x040050FF RID: 20735
		public IEntryIcon Icon;

		// Token: 0x04005101 RID: 20737
		public IBestiaryUICollectionInfoProvider UIInfoProvider;
	}
}
