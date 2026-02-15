using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI
{
	// Token: 0x02000373 RID: 883
	public class CustomCurrencySystem
	{
		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06002937 RID: 10551 RVA: 0x00579786 File Offset: 0x00577986
		public long CurrencyCap
		{
			get
			{
				return this._currencyCap;
			}
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x0057978E File Offset: 0x0057798E
		public void Include(int coin, int howMuchIsItWorth)
		{
			this._valuePerUnit[coin] = howMuchIsItWorth;
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x0057979D File Offset: 0x0057799D
		public void SetCurrencyCap(long cap)
		{
			this._currencyCap = cap;
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x005797A8 File Offset: 0x005779A8
		public virtual long CountCurrency(out bool overFlowing, Item[] inv, params int[] ignoreSlots)
		{
			List<int> list = new List<int>(ignoreSlots);
			long num = 0L;
			for (int i = 0; i < inv.Length; i++)
			{
				if (!list.Contains(i))
				{
					int num2;
					if (this._valuePerUnit.TryGetValue(inv[i].type, out num2))
					{
						num += (long)(num2 * inv[i].stack);
					}
					if (num >= this.CurrencyCap)
					{
						overFlowing = true;
						return this.CurrencyCap;
					}
				}
			}
			overFlowing = false;
			return num;
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x00579814 File Offset: 0x00577A14
		public virtual long CombineStacks(out bool overFlowing, params long[] coinCounts)
		{
			long num = 0L;
			foreach (long num2 in coinCounts)
			{
				num += num2;
				if (num >= this.CurrencyCap)
				{
					overFlowing = true;
					return this.CurrencyCap;
				}
			}
			overFlowing = false;
			return num;
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x00579854 File Offset: 0x00577A54
		public virtual bool TryPurchasing(long price, List<Item[]> inv, List<Point> slotCoins, List<Point> slotsEmpty, List<Point> slotEmptyBank, List<Point> slotEmptyBank2, List<Point> slotEmptyBank3, List<Point> slotEmptyBank4)
		{
			long num = price;
			Dictionary<Point, Item> dictionary = new Dictionary<Point, Item>();
			bool result = true;
			while (num > 0L)
			{
				long num2 = 1000000L;
				for (int i = 0; i < 4; i++)
				{
					if (num >= num2)
					{
						foreach (Point point in slotCoins)
						{
							if (inv[point.X][point.Y].type == 74 - i)
							{
								long num3 = num / num2;
								dictionary[point] = inv[point.X][point.Y].Clone();
								if (num3 < (long)inv[point.X][point.Y].stack)
								{
									inv[point.X][point.Y].stack -= (int)num3;
								}
								else
								{
									inv[point.X][point.Y].SetDefaults(0, null);
									slotsEmpty.Add(point);
								}
								num -= num2 * (long)(dictionary[point].stack - inv[point.X][point.Y].stack);
							}
						}
					}
					num2 /= 100L;
				}
				if (num > 0L)
				{
					if (slotsEmpty.Count <= 0)
					{
						foreach (KeyValuePair<Point, Item> keyValuePair in dictionary)
						{
							inv[keyValuePair.Key.X][keyValuePair.Key.Y] = keyValuePair.Value.Clone();
						}
						result = false;
						break;
					}
					slotsEmpty.Sort(new Comparison<Point>(DelegateMethods.CompareYReverse));
					Point point2 = new Point(-1, -1);
					for (int j = 0; j < inv.Count; j++)
					{
						num2 = 10000L;
						for (int k = 0; k < 3; k++)
						{
							if (num >= num2)
							{
								foreach (Point point3 in slotCoins)
								{
									if (point3.X == j && inv[point3.X][point3.Y].type == 74 - k && inv[point3.X][point3.Y].stack >= 1)
									{
										List<Point> list = slotsEmpty;
										if (j == 1 && slotEmptyBank.Count > 0)
										{
											list = slotEmptyBank;
										}
										if (j == 2 && slotEmptyBank2.Count > 0)
										{
											list = slotEmptyBank2;
										}
										if (j == 3 && slotEmptyBank3.Count > 0)
										{
											list = slotEmptyBank3;
										}
										if (j == 4 && slotEmptyBank4.Count > 0)
										{
											list = slotEmptyBank4;
										}
										Item item = inv[point3.X][point3.Y];
										int num4 = item.stack - 1;
										item.stack = num4;
										if (num4 <= 0)
										{
											inv[point3.X][point3.Y].SetDefaults(0, null);
											list.Add(point3);
										}
										dictionary[list[0]] = inv[list[0].X][list[0].Y].Clone();
										inv[list[0].X][list[0].Y].SetDefaults(73 - k, null);
										inv[list[0].X][list[0].Y].stack = 100;
										point2 = list[0];
										list.RemoveAt(0);
										break;
									}
								}
							}
							if (point2.X != -1 || point2.Y != -1)
							{
								break;
							}
							num2 /= 100L;
						}
						for (int l = 0; l < 2; l++)
						{
							if (point2.X == -1 && point2.Y == -1)
							{
								foreach (Point point4 in slotCoins)
								{
									if (point4.X == j && inv[point4.X][point4.Y].type == 73 + l && inv[point4.X][point4.Y].stack >= 1)
									{
										List<Point> list2 = slotsEmpty;
										if (j == 1 && slotEmptyBank.Count > 0)
										{
											list2 = slotEmptyBank;
										}
										if (j == 2 && slotEmptyBank2.Count > 0)
										{
											list2 = slotEmptyBank2;
										}
										if (j == 3 && slotEmptyBank3.Count > 0)
										{
											list2 = slotEmptyBank3;
										}
										if (j == 4 && slotEmptyBank4.Count > 0)
										{
											list2 = slotEmptyBank4;
										}
										Item item2 = inv[point4.X][point4.Y];
										int num4 = item2.stack - 1;
										item2.stack = num4;
										if (num4 <= 0)
										{
											inv[point4.X][point4.Y].SetDefaults(0, null);
											list2.Add(point4);
										}
										dictionary[list2[0]] = inv[list2[0].X][list2[0].Y].Clone();
										inv[list2[0].X][list2[0].Y].SetDefaults(72 + l, null);
										inv[list2[0].X][list2[0].Y].stack = 100;
										point2 = list2[0];
										list2.RemoveAt(0);
										break;
									}
								}
							}
						}
						if (point2.X != -1 && point2.Y != -1)
						{
							slotCoins.Add(point2);
							break;
						}
					}
					slotsEmpty.Sort(new Comparison<Point>(DelegateMethods.CompareYReverse));
					slotEmptyBank.Sort(new Comparison<Point>(DelegateMethods.CompareYReverse));
					slotEmptyBank2.Sort(new Comparison<Point>(DelegateMethods.CompareYReverse));
					slotEmptyBank3.Sort(new Comparison<Point>(DelegateMethods.CompareYReverse));
					slotEmptyBank4.Sort(new Comparison<Point>(DelegateMethods.CompareYReverse));
				}
			}
			return result;
		}

		// Token: 0x0600293D RID: 10557 RVA: 0x00579F30 File Offset: 0x00578130
		public virtual bool Accepts(Item item)
		{
			return this._valuePerUnit.ContainsKey(item.type);
		}

		// Token: 0x0600293E RID: 10558 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void DrawSavingsMoney(SpriteBatch sb, string text, float shopx, float shopy, long totalCoins, bool horizontal = false)
		{
		}

		// Token: 0x0600293F RID: 10559 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void GetPriceText(string[] lines, ref int currentLine, long price)
		{
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x00579F43 File Offset: 0x00578143
		protected int SortByHighest(Tuple<int, int> valueA, Tuple<int, int> valueB)
		{
			if (valueA.Item2 > valueB.Item2)
			{
				return -1;
			}
			if (valueA.Item2 == valueB.Item2)
			{
				return 0;
			}
			return -1;
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x00579F68 File Offset: 0x00578168
		protected List<Tuple<Point, Item>> ItemCacheCreate(List<Item[]> inventories)
		{
			List<Tuple<Point, Item>> list = new List<Tuple<Point, Item>>();
			for (int i = 0; i < inventories.Count; i++)
			{
				for (int j = 0; j < inventories[i].Length; j++)
				{
					Item item = inventories[i][j];
					list.Add(new Tuple<Point, Item>(new Point(i, j), item.DeepClone()));
				}
			}
			return list;
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x00579FC4 File Offset: 0x005781C4
		protected void ItemCacheRestore(List<Tuple<Point, Item>> cache, List<Item[]> inventories)
		{
			foreach (Tuple<Point, Item> tuple in cache)
			{
				inventories[tuple.Item1.X][tuple.Item1.Y] = tuple.Item2;
			}
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x0057A030 File Offset: 0x00578230
		public virtual void GetItemExpectedPrice(Item item, out long calcForSelling, out long calcForBuying)
		{
			int storeValue = item.GetStoreValue();
			calcForSelling = (long)storeValue;
			calcForBuying = (long)storeValue;
		}

		// Token: 0x040051BE RID: 20926
		protected Dictionary<int, int> _valuePerUnit = new Dictionary<int, int>();

		// Token: 0x040051BF RID: 20927
		private long _currencyCap = 999999999L;
	}
}
