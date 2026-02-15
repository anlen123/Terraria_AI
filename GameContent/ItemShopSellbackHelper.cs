using System;
using System.Collections.Generic;

namespace Terraria.GameContent
{
	// Token: 0x0200027C RID: 636
	public class ItemShopSellbackHelper
	{
		// Token: 0x06002455 RID: 9301 RVA: 0x0054C65C File Offset: 0x0054A85C
		public void Add(Item item)
		{
			ItemShopSellbackHelper.ItemMemo itemMemo = this._memos.Find((ItemShopSellbackHelper.ItemMemo x) => x.Matches(item));
			if (itemMemo != null)
			{
				itemMemo.stack += item.stack;
				return;
			}
			this._memos.Add(new ItemShopSellbackHelper.ItemMemo(item));
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x0054C6C0 File Offset: 0x0054A8C0
		public void Clear()
		{
			this._memos.Clear();
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x0054C6D0 File Offset: 0x0054A8D0
		public int GetAmount(Item item)
		{
			ItemShopSellbackHelper.ItemMemo itemMemo = this._memos.Find((ItemShopSellbackHelper.ItemMemo x) => x.Matches(item));
			if (itemMemo != null)
			{
				return itemMemo.stack;
			}
			return 0;
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x0054C710 File Offset: 0x0054A910
		public int Remove(Item item)
		{
			ItemShopSellbackHelper.ItemMemo itemMemo = this._memos.Find((ItemShopSellbackHelper.ItemMemo x) => x.Matches(item));
			if (itemMemo == null)
			{
				return 0;
			}
			int stack = itemMemo.stack;
			itemMemo.stack -= item.stack;
			if (itemMemo.stack <= 0)
			{
				this._memos.Remove(itemMemo);
				return stack;
			}
			return stack - itemMemo.stack;
		}

		// Token: 0x04004DEA RID: 19946
		private List<ItemShopSellbackHelper.ItemMemo> _memos = new List<ItemShopSellbackHelper.ItemMemo>();

		// Token: 0x020007FE RID: 2046
		private class ItemMemo
		{
			// Token: 0x060042BB RID: 17083 RVA: 0x006BE78D File Offset: 0x006BC98D
			public ItemMemo(Item item)
			{
				this.type = item.type;
				this.prefix = (int)item.prefix;
				this.stack = item.stack;
			}

			// Token: 0x060042BC RID: 17084 RVA: 0x006BE7B9 File Offset: 0x006BC9B9
			public bool Matches(Item item)
			{
				return item.IsConsideredSameItemAsType(this.type) && (int)item.prefix == this.prefix;
			}

			// Token: 0x04007179 RID: 29049
			public readonly int type;

			// Token: 0x0400717A RID: 29050
			public readonly int prefix;

			// Token: 0x0400717B RID: 29051
			public int stack;
		}
	}
}
