using System;
using System.Collections.Generic;

namespace Terraria.GameContent
{
	// Token: 0x02000246 RID: 582
	public class ItemTrader
	{
		// Token: 0x060022DA RID: 8922 RVA: 0x0053AB68 File Offset: 0x00538D68
		public void AddOption_Interchangable(int itemType1, int itemType2)
		{
			this.AddOption_OneWay(itemType1, 1, itemType2, 1);
			this.AddOption_OneWay(itemType2, 1, itemType1, 1);
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x0053AB80 File Offset: 0x00538D80
		public void AddOption_CyclicLoop(params int[] typesInOrder)
		{
			for (int i = 0; i < typesInOrder.Length - 1; i++)
			{
				this.AddOption_OneWay(typesInOrder[i], 1, typesInOrder[i + 1], 1);
			}
			this.AddOption_OneWay(typesInOrder[typesInOrder.Length - 1], 1, typesInOrder[0], 1);
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x0053ABC0 File Offset: 0x00538DC0
		public void AddOption_FromAny(int givingItemType, params int[] takingItemTypes)
		{
			for (int i = 0; i < takingItemTypes.Length; i++)
			{
				this.AddOption_OneWay(takingItemTypes[i], 1, givingItemType, 1);
			}
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x0053ABE7 File Offset: 0x00538DE7
		public void AddOption_OneWay(int takingItemType, int takingItemStack, int givingItemType, int givingItemStack)
		{
			this._options.Add(new ItemTrader.TradeOption
			{
				TakingItemType = takingItemType,
				TakingItemStack = takingItemStack,
				GivingItemType = givingItemType,
				GivingItemStack = givingItemStack
			});
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x0053AC18 File Offset: 0x00538E18
		public bool TryGetTradeOption(Item item, out ItemTrader.TradeOption option)
		{
			option = null;
			int type = item.type;
			int stack = item.stack;
			for (int i = 0; i < this._options.Count; i++)
			{
				ItemTrader.TradeOption tradeOption = this._options[i];
				if (tradeOption.WillTradeFor(type, stack))
				{
					option = tradeOption;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x0053AC6C File Offset: 0x00538E6C
		public static ItemTrader CreateChlorophyteExtractinator()
		{
			ItemTrader itemTrader = new ItemTrader();
			itemTrader.AddOption_Interchangable(12, 699);
			itemTrader.AddOption_Interchangable(11, 700);
			itemTrader.AddOption_Interchangable(14, 701);
			itemTrader.AddOption_Interchangable(13, 702);
			itemTrader.AddOption_Interchangable(56, 880);
			itemTrader.AddOption_Interchangable(364, 1104);
			itemTrader.AddOption_Interchangable(365, 1105);
			itemTrader.AddOption_Interchangable(366, 1106);
			itemTrader.AddOption_CyclicLoop(new int[]
			{
				134,
				137,
				139
			});
			itemTrader.AddOption_Interchangable(20, 703);
			itemTrader.AddOption_Interchangable(22, 704);
			itemTrader.AddOption_Interchangable(21, 705);
			itemTrader.AddOption_Interchangable(19, 706);
			itemTrader.AddOption_Interchangable(57, 1257);
			itemTrader.AddOption_Interchangable(381, 1184);
			itemTrader.AddOption_Interchangable(382, 1191);
			itemTrader.AddOption_Interchangable(391, 1198);
			itemTrader.AddOption_Interchangable(86, 1329);
			itemTrader.AddOption_FromAny(3, new int[]
			{
				61,
				836,
				409
			});
			itemTrader.AddOption_FromAny(169, new int[]
			{
				370,
				1246,
				408
			});
			itemTrader.AddOption_FromAny(664, new int[]
			{
				833,
				835,
				834
			});
			itemTrader.AddOption_FromAny(3271, new int[]
			{
				3276,
				3277,
				3339
			});
			itemTrader.AddOption_FromAny(3272, new int[]
			{
				3274,
				3275,
				3338
			});
			return itemTrader;
		}

		// Token: 0x04004D16 RID: 19734
		public static ItemTrader ChlorophyteExtractinator = ItemTrader.CreateChlorophyteExtractinator();

		// Token: 0x04004D17 RID: 19735
		private List<ItemTrader.TradeOption> _options = new List<ItemTrader.TradeOption>();

		// Token: 0x020007D0 RID: 2000
		public class TradeOption
		{
			// Token: 0x0600422A RID: 16938 RVA: 0x006BCA05 File Offset: 0x006BAC05
			public bool WillTradeFor(int offeredItemType, int offeredItemStack)
			{
				return offeredItemType == this.TakingItemType && offeredItemStack >= this.TakingItemStack;
			}

			// Token: 0x040070CE RID: 28878
			public int TakingItemType;

			// Token: 0x040070CF RID: 28879
			public int TakingItemStack;

			// Token: 0x040070D0 RID: 28880
			public int GivingItemType;

			// Token: 0x040070D1 RID: 28881
			public int GivingItemStack;
		}
	}
}
