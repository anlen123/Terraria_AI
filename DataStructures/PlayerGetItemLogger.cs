using System;
using System.Collections.Generic;

namespace Terraria.DataStructures
{
	// Token: 0x02000558 RID: 1368
	public class PlayerGetItemLogger
	{
		// Token: 0x06003790 RID: 14224 RVA: 0x0062EA2A File Offset: 0x0062CC2A
		public void Clear()
		{
			this.Entries.Clear();
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x0062EA38 File Offset: 0x0062CC38
		public void Add(Item[] array, int slot, int itemSlotContext, int stack)
		{
			if (!this._enabled)
			{
				return;
			}
			this.Entries.Add(new PlayerGetItemLogger.GetItemLoggerEntry
			{
				TargetArray = array,
				TargetSlot = slot,
				TargetItemSlotContext = itemSlotContext,
				Stack = stack
			});
		}

		// Token: 0x06003792 RID: 14226 RVA: 0x0062EA83 File Offset: 0x0062CC83
		public void Start()
		{
			this.Clear();
			this._enabled = true;
		}

		// Token: 0x06003793 RID: 14227 RVA: 0x0062EA92 File Offset: 0x0062CC92
		public void Stop()
		{
			this._enabled = false;
		}

		// Token: 0x04005B9D RID: 23453
		public List<PlayerGetItemLogger.GetItemLoggerEntry> Entries = new List<PlayerGetItemLogger.GetItemLoggerEntry>();

		// Token: 0x04005B9E RID: 23454
		private bool _enabled;

		// Token: 0x020009B7 RID: 2487
		public struct GetItemLoggerEntry
		{
			// Token: 0x0400768D RID: 30349
			public Item[] TargetArray;

			// Token: 0x0400768E RID: 30350
			public int TargetSlot;

			// Token: 0x0400768F RID: 30351
			public int TargetItemSlotContext;

			// Token: 0x04007690 RID: 30352
			public int Stack;
		}
	}
}
