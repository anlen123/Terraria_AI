using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;

namespace Terraria.GameContent.LootSimulation
{
	// Token: 0x020002EA RID: 746
	public class LootSimulationItemCounter
	{
		// Token: 0x06002648 RID: 9800 RVA: 0x0055DE74 File Offset: 0x0055C074
		public void AddItem(int itemId, int amount, bool expert)
		{
			if (expert)
			{
				this._itemCountsObtainedExpert[itemId] += (long)amount;
				return;
			}
			this._itemCountsObtained[itemId] += (long)amount;
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x0055DEA0 File Offset: 0x0055C0A0
		public void Exclude(params int[] itemIds)
		{
			foreach (int num in itemIds)
			{
				this._itemCountsObtained[num] = 0L;
				this._itemCountsObtainedExpert[num] = 0L;
			}
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x0055DED5 File Offset: 0x0055C0D5
		public void IncreaseTimesAttempted(int amount, bool expert)
		{
			if (expert)
			{
				this._totalTimesAttemptedExpert += (long)amount;
				return;
			}
			this._totalTimesAttempted += (long)amount;
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x0055DEFC File Offset: 0x0055C0FC
		public string PrintCollectedItems(bool expert)
		{
			long[] collectionToUse = this._itemCountsObtained;
			long totalDropsAttempted = this._totalTimesAttempted;
			if (expert)
			{
				collectionToUse = this._itemCountsObtainedExpert;
				this._totalTimesAttempted = this._totalTimesAttemptedExpert;
			}
			IEnumerable<string> values = from entry in collectionToUse.Select((long count, int itemId) => new
			{
				itemId,
				count
			})
			where entry.count > 0L
			select entry.itemId into itemId
			select string.Format("new ItemDropInfo(ItemID.{0}, {1}, {2})", ItemID.Search.GetName(itemId), collectionToUse[itemId], totalDropsAttempted);
			return string.Join(",\n", values);
		}

		// Token: 0x0400504B RID: 20555
		private long[] _itemCountsObtained = new long[(int)ItemID.Count];

		// Token: 0x0400504C RID: 20556
		private long[] _itemCountsObtainedExpert = new long[(int)ItemID.Count];

		// Token: 0x0400504D RID: 20557
		private long _totalTimesAttempted;

		// Token: 0x0400504E RID: 20558
		private long _totalTimesAttemptedExpert;
	}
}
