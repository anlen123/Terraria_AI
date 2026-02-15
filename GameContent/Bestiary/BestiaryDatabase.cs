using System;
using System.Collections.Generic;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x0200032C RID: 812
	public class BestiaryDatabase
	{
		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x060027D7 RID: 10199 RVA: 0x00568BB9 File Offset: 0x00566DB9
		public List<BestiaryEntry> Entries
		{
			get
			{
				return this._entries;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x060027D8 RID: 10200 RVA: 0x00568BC1 File Offset: 0x00566DC1
		public List<IBestiaryEntryFilter> Filters
		{
			get
			{
				return this._filters;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x060027D9 RID: 10201 RVA: 0x00568BC9 File Offset: 0x00566DC9
		public List<IBestiarySortStep> SortSteps
		{
			get
			{
				return this._sortSteps;
			}
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x00568BD4 File Offset: 0x00566DD4
		public BestiaryEntry Register(BestiaryEntry entry)
		{
			this._entries.Add(entry);
			for (int i = 0; i < entry.Info.Count; i++)
			{
				NPCNetIdBestiaryInfoElement npcnetIdBestiaryInfoElement = entry.Info[i] as NPCNetIdBestiaryInfoElement;
				if (npcnetIdBestiaryInfoElement != null)
				{
					this._byNpcId[npcnetIdBestiaryInfoElement.NetId] = entry;
				}
			}
			return entry;
		}

		// Token: 0x060027DB RID: 10203 RVA: 0x00568C2B File Offset: 0x00566E2B
		public IBestiaryEntryFilter Register(IBestiaryEntryFilter filter)
		{
			this._filters.Add(filter);
			return filter;
		}

		// Token: 0x060027DC RID: 10204 RVA: 0x00568C3A File Offset: 0x00566E3A
		public IBestiarySortStep Register(IBestiarySortStep sortStep)
		{
			this._sortSteps.Add(sortStep);
			return sortStep;
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x00568C4C File Offset: 0x00566E4C
		public BestiaryEntry FindEntryByNPCID(int npcNetId)
		{
			BestiaryEntry result;
			if (this._byNpcId.TryGetValue(npcNetId, out result))
			{
				return result;
			}
			this._trashEntry.Info.Clear();
			return this._trashEntry;
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x00568C84 File Offset: 0x00566E84
		public void Merge(ItemDropDatabase dropsDatabase)
		{
			for (int i = -65; i < (int)NPCID.Count; i++)
			{
				this.ExtractDropsForNPC(dropsDatabase, i);
			}
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x00568CAC File Offset: 0x00566EAC
		private void ExtractDropsForNPC(ItemDropDatabase dropsDatabase, int npcId)
		{
			BestiaryEntry bestiaryEntry = this.FindEntryByNPCID(npcId);
			if (bestiaryEntry == null)
			{
				return;
			}
			List<IItemDropRule> rulesForNPCID = dropsDatabase.GetRulesForNPCID(npcId, false);
			List<DropRateInfo> list = new List<DropRateInfo>();
			DropRateInfoChainFeed ratesInfo = new DropRateInfoChainFeed(1f);
			foreach (IItemDropRule itemDropRule in rulesForNPCID)
			{
				itemDropRule.ReportDroprates(list, ratesInfo);
			}
			foreach (DropRateInfo info in list)
			{
				bestiaryEntry.Info.Add(new ItemDropBestiaryInfoElement(info));
			}
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x00568D68 File Offset: 0x00566F68
		public void ApplyPass(BestiaryDatabase.BestiaryEntriesPass pass)
		{
			for (int i = 0; i < this._entries.Count; i++)
			{
				pass(this._entries[i]);
			}
		}

		// Token: 0x040050EB RID: 20715
		private List<BestiaryEntry> _entries = new List<BestiaryEntry>();

		// Token: 0x040050EC RID: 20716
		private List<IBestiaryEntryFilter> _filters = new List<IBestiaryEntryFilter>();

		// Token: 0x040050ED RID: 20717
		private List<IBestiarySortStep> _sortSteps = new List<IBestiarySortStep>();

		// Token: 0x040050EE RID: 20718
		private Dictionary<int, BestiaryEntry> _byNpcId = new Dictionary<int, BestiaryEntry>();

		// Token: 0x040050EF RID: 20719
		private BestiaryEntry _trashEntry = new BestiaryEntry();

		// Token: 0x020008AA RID: 2218
		// (Invoke) Token: 0x060045BF RID: 17855
		public delegate void BestiaryEntriesPass(BestiaryEntry entry);
	}
}
