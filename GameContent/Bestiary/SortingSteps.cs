using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000332 RID: 818
	public static class SortingSteps
	{
		// Token: 0x020008B4 RID: 2228
		public class ByNetId : IBestiarySortStep, IEntrySortStep<BestiaryEntry>, IComparer<BestiaryEntry>
		{
			// Token: 0x17000561 RID: 1377
			// (get) Token: 0x060045EC RID: 17900 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool HiddenFromSortOptions
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060045ED RID: 17901 RVA: 0x006C4AB4 File Offset: 0x006C2CB4
			public int Compare(BestiaryEntry x, BestiaryEntry y)
			{
				NPCNetIdBestiaryInfoElement npcnetIdBestiaryInfoElement = x.Info.FirstOrDefault((IBestiaryInfoElement element) => element is NPCNetIdBestiaryInfoElement) as NPCNetIdBestiaryInfoElement;
				NPCNetIdBestiaryInfoElement npcnetIdBestiaryInfoElement2 = y.Info.FirstOrDefault((IBestiaryInfoElement element) => element is NPCNetIdBestiaryInfoElement) as NPCNetIdBestiaryInfoElement;
				if (npcnetIdBestiaryInfoElement == null && npcnetIdBestiaryInfoElement2 != null)
				{
					return 1;
				}
				if (npcnetIdBestiaryInfoElement2 == null && npcnetIdBestiaryInfoElement != null)
				{
					return -1;
				}
				if (npcnetIdBestiaryInfoElement == null || npcnetIdBestiaryInfoElement2 == null)
				{
					return 0;
				}
				return npcnetIdBestiaryInfoElement.NetId.CompareTo(npcnetIdBestiaryInfoElement2.NetId);
			}

			// Token: 0x060045EE RID: 17902 RVA: 0x006C4B4D File Offset: 0x006C2D4D
			public string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_ID";
			}
		}

		// Token: 0x020008B5 RID: 2229
		public class ByUnlockState : IBestiarySortStep, IEntrySortStep<BestiaryEntry>, IComparer<BestiaryEntry>
		{
			// Token: 0x17000562 RID: 1378
			// (get) Token: 0x060045F0 RID: 17904 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool HiddenFromSortOptions
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060045F1 RID: 17905 RVA: 0x006C4B54 File Offset: 0x006C2D54
			public int Compare(BestiaryEntry x, BestiaryEntry y)
			{
				BestiaryUICollectionInfo entryUICollectionInfo = x.UIInfoProvider.GetEntryUICollectionInfo();
				BestiaryUICollectionInfo entryUICollectionInfo2 = y.UIInfoProvider.GetEntryUICollectionInfo();
				return y.Icon.GetUnlockState(entryUICollectionInfo2).CompareTo(x.Icon.GetUnlockState(entryUICollectionInfo));
			}

			// Token: 0x060045F2 RID: 17906 RVA: 0x006C4B99 File Offset: 0x006C2D99
			public string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_Unlocks";
			}
		}

		// Token: 0x020008B6 RID: 2230
		public class ByBestiarySortingId : IBestiarySortStep, IEntrySortStep<BestiaryEntry>, IComparer<BestiaryEntry>
		{
			// Token: 0x17000563 RID: 1379
			// (get) Token: 0x060045F4 RID: 17908 RVA: 0x001DA9FB File Offset: 0x001D8BFB
			public bool HiddenFromSortOptions
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060045F5 RID: 17909 RVA: 0x006C4BA0 File Offset: 0x006C2DA0
			public int Compare(BestiaryEntry x, BestiaryEntry y)
			{
				NPCNetIdBestiaryInfoElement npcnetIdBestiaryInfoElement = x.Info.FirstOrDefault((IBestiaryInfoElement element) => element is NPCNetIdBestiaryInfoElement) as NPCNetIdBestiaryInfoElement;
				NPCNetIdBestiaryInfoElement npcnetIdBestiaryInfoElement2 = y.Info.FirstOrDefault((IBestiaryInfoElement element) => element is NPCNetIdBestiaryInfoElement) as NPCNetIdBestiaryInfoElement;
				if (npcnetIdBestiaryInfoElement == null && npcnetIdBestiaryInfoElement2 != null)
				{
					return 1;
				}
				if (npcnetIdBestiaryInfoElement2 == null && npcnetIdBestiaryInfoElement != null)
				{
					return -1;
				}
				if (npcnetIdBestiaryInfoElement == null || npcnetIdBestiaryInfoElement2 == null)
				{
					return 0;
				}
				int num = ContentSamples.NpcBestiarySortingId[npcnetIdBestiaryInfoElement.NetId];
				int value = ContentSamples.NpcBestiarySortingId[npcnetIdBestiaryInfoElement2.NetId];
				return num.CompareTo(value);
			}

			// Token: 0x060045F6 RID: 17910 RVA: 0x006C4C4F File Offset: 0x006C2E4F
			public string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_BestiaryID";
			}
		}

		// Token: 0x020008B7 RID: 2231
		public class ByBestiaryRarity : IBestiarySortStep, IEntrySortStep<BestiaryEntry>, IComparer<BestiaryEntry>
		{
			// Token: 0x17000564 RID: 1380
			// (get) Token: 0x060045F8 RID: 17912 RVA: 0x001DA9FB File Offset: 0x001D8BFB
			public bool HiddenFromSortOptions
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060045F9 RID: 17913 RVA: 0x006C4C58 File Offset: 0x006C2E58
			public int Compare(BestiaryEntry x, BestiaryEntry y)
			{
				NPCNetIdBestiaryInfoElement npcnetIdBestiaryInfoElement = x.Info.FirstOrDefault((IBestiaryInfoElement element) => element is NPCNetIdBestiaryInfoElement) as NPCNetIdBestiaryInfoElement;
				NPCNetIdBestiaryInfoElement npcnetIdBestiaryInfoElement2 = y.Info.FirstOrDefault((IBestiaryInfoElement element) => element is NPCNetIdBestiaryInfoElement) as NPCNetIdBestiaryInfoElement;
				if (npcnetIdBestiaryInfoElement == null && npcnetIdBestiaryInfoElement2 != null)
				{
					return 1;
				}
				if (npcnetIdBestiaryInfoElement2 == null && npcnetIdBestiaryInfoElement != null)
				{
					return -1;
				}
				if (npcnetIdBestiaryInfoElement == null || npcnetIdBestiaryInfoElement2 == null)
				{
					return 0;
				}
				int value = ContentSamples.NpcBestiaryRarityStars[npcnetIdBestiaryInfoElement.NetId];
				return ContentSamples.NpcBestiaryRarityStars[npcnetIdBestiaryInfoElement2.NetId].CompareTo(value);
			}

			// Token: 0x060045FA RID: 17914 RVA: 0x006C4D07 File Offset: 0x006C2F07
			public string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_Rarity";
			}
		}

		// Token: 0x020008B8 RID: 2232
		public class Alphabetical : IBestiarySortStep, IEntrySortStep<BestiaryEntry>, IComparer<BestiaryEntry>
		{
			// Token: 0x17000565 RID: 1381
			// (get) Token: 0x060045FC RID: 17916 RVA: 0x001DA9FB File Offset: 0x001D8BFB
			public bool HiddenFromSortOptions
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060045FD RID: 17917 RVA: 0x006C4D10 File Offset: 0x006C2F10
			public int Compare(BestiaryEntry x, BestiaryEntry y)
			{
				NPCNetIdBestiaryInfoElement npcnetIdBestiaryInfoElement = x.Info.FirstOrDefault((IBestiaryInfoElement element) => element is NPCNetIdBestiaryInfoElement) as NPCNetIdBestiaryInfoElement;
				NPCNetIdBestiaryInfoElement npcnetIdBestiaryInfoElement2 = y.Info.FirstOrDefault((IBestiaryInfoElement element) => element is NPCNetIdBestiaryInfoElement) as NPCNetIdBestiaryInfoElement;
				if (npcnetIdBestiaryInfoElement == null && npcnetIdBestiaryInfoElement2 != null)
				{
					return 1;
				}
				if (npcnetIdBestiaryInfoElement2 == null && npcnetIdBestiaryInfoElement != null)
				{
					return -1;
				}
				if (npcnetIdBestiaryInfoElement == null || npcnetIdBestiaryInfoElement2 == null)
				{
					return 0;
				}
				string textValue = Language.GetTextValue(ContentSamples.NpcsByNetId[npcnetIdBestiaryInfoElement.NetId].TypeName);
				string textValue2 = Language.GetTextValue(ContentSamples.NpcsByNetId[npcnetIdBestiaryInfoElement2.NetId].TypeName);
				return textValue.CompareTo(textValue2);
			}

			// Token: 0x060045FE RID: 17918 RVA: 0x006C4DD0 File Offset: 0x006C2FD0
			public string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_Alphabetical";
			}
		}

		// Token: 0x020008B9 RID: 2233
		public abstract class ByStat : IBestiarySortStep, IEntrySortStep<BestiaryEntry>, IComparer<BestiaryEntry>
		{
			// Token: 0x17000566 RID: 1382
			// (get) Token: 0x06004600 RID: 17920 RVA: 0x001DA9FB File Offset: 0x001D8BFB
			public bool HiddenFromSortOptions
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06004601 RID: 17921 RVA: 0x006C4DD8 File Offset: 0x006C2FD8
			public int Compare(BestiaryEntry x, BestiaryEntry y)
			{
				NPCStatsReportInfoElement npcstatsReportInfoElement = x.Info.FirstOrDefault((IBestiaryInfoElement element) => this.IsAStatsCardINeed(element)) as NPCStatsReportInfoElement;
				NPCStatsReportInfoElement npcstatsReportInfoElement2 = y.Info.FirstOrDefault((IBestiaryInfoElement element) => this.IsAStatsCardINeed(element)) as NPCStatsReportInfoElement;
				if (npcstatsReportInfoElement == null && npcstatsReportInfoElement2 != null)
				{
					return 1;
				}
				if (npcstatsReportInfoElement2 == null && npcstatsReportInfoElement != null)
				{
					return -1;
				}
				if (npcstatsReportInfoElement == null || npcstatsReportInfoElement2 == null)
				{
					return 0;
				}
				return this.Compare(npcstatsReportInfoElement, npcstatsReportInfoElement2);
			}

			// Token: 0x06004602 RID: 17922
			public abstract int Compare(NPCStatsReportInfoElement cardX, NPCStatsReportInfoElement cardY);

			// Token: 0x06004603 RID: 17923
			public abstract string GetDisplayNameKey();

			// Token: 0x06004604 RID: 17924 RVA: 0x006C4E3F File Offset: 0x006C303F
			private bool IsAStatsCardINeed(IBestiaryInfoElement element)
			{
				return element is NPCStatsReportInfoElement;
			}
		}

		// Token: 0x020008BA RID: 2234
		public class ByAttack : SortingSteps.ByStat
		{
			// Token: 0x06004608 RID: 17928 RVA: 0x006C4E55 File Offset: 0x006C3055
			public override int Compare(NPCStatsReportInfoElement cardX, NPCStatsReportInfoElement cardY)
			{
				return cardY.Damage.CompareTo(cardX.Damage);
			}

			// Token: 0x06004609 RID: 17929 RVA: 0x006C4E68 File Offset: 0x006C3068
			public override string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_Attack";
			}
		}

		// Token: 0x020008BB RID: 2235
		public class ByDefense : SortingSteps.ByStat
		{
			// Token: 0x0600460B RID: 17931 RVA: 0x006C4E77 File Offset: 0x006C3077
			public override int Compare(NPCStatsReportInfoElement cardX, NPCStatsReportInfoElement cardY)
			{
				return cardY.Defense.CompareTo(cardX.Defense);
			}

			// Token: 0x0600460C RID: 17932 RVA: 0x006C4E8A File Offset: 0x006C308A
			public override string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_Defense";
			}
		}

		// Token: 0x020008BC RID: 2236
		public class ByCoins : SortingSteps.ByStat
		{
			// Token: 0x0600460E RID: 17934 RVA: 0x006C4E91 File Offset: 0x006C3091
			public override int Compare(NPCStatsReportInfoElement cardX, NPCStatsReportInfoElement cardY)
			{
				return cardY.MonetaryValue.CompareTo(cardX.MonetaryValue);
			}

			// Token: 0x0600460F RID: 17935 RVA: 0x006C4EA4 File Offset: 0x006C30A4
			public override string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_Coins";
			}
		}

		// Token: 0x020008BD RID: 2237
		public class ByHP : SortingSteps.ByStat
		{
			// Token: 0x06004611 RID: 17937 RVA: 0x006C4EAB File Offset: 0x006C30AB
			public override int Compare(NPCStatsReportInfoElement cardX, NPCStatsReportInfoElement cardY)
			{
				return cardY.LifeMax.CompareTo(cardX.LifeMax);
			}

			// Token: 0x06004612 RID: 17938 RVA: 0x006C4EBE File Offset: 0x006C30BE
			public override string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_HitPoints";
			}
		}
	}
}
