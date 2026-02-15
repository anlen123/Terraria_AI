using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Terraria.GameContent.UI
{
	// Token: 0x02000375 RID: 885
	public class CustomCurrencyManager
	{
		// Token: 0x06002949 RID: 10569 RVA: 0x0057A401 File Offset: 0x00578601
		public static void Initialize()
		{
			CustomCurrencyManager._nextCurrencyIndex = 0;
			CustomCurrencyID.DefenderMedals = CustomCurrencyManager.RegisterCurrency(new CustomCurrencySingleCoin(3817, 999L));
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x0057A424 File Offset: 0x00578624
		public static int RegisterCurrency(CustomCurrencySystem collection)
		{
			int nextCurrencyIndex = CustomCurrencyManager._nextCurrencyIndex;
			CustomCurrencyManager._nextCurrencyIndex++;
			CustomCurrencyManager._currencies[nextCurrencyIndex] = collection;
			return nextCurrencyIndex;
		}

		// Token: 0x0600294B RID: 10571 RVA: 0x0057A450 File Offset: 0x00578650
		public static void DrawSavings(SpriteBatch sb, int currencyIndex, float shopx, float shopy, bool horizontal = false)
		{
			CustomCurrencySystem customCurrencySystem = CustomCurrencyManager._currencies[currencyIndex];
			Player player = Main.player[Main.myPlayer];
			bool flag;
			long num = customCurrencySystem.CountCurrency(out flag, player.bank.item, new int[0]);
			long num2 = customCurrencySystem.CountCurrency(out flag, player.bank2.item, new int[0]);
			long num3 = customCurrencySystem.CountCurrency(out flag, player.bank3.item, new int[0]);
			long num4 = customCurrencySystem.CountCurrency(out flag, player.bank4.item, new int[0]);
			long num5 = customCurrencySystem.CombineStacks(out flag, new long[]
			{
				num,
				num2,
				num3,
				num4
			});
			if (num5 > 0L)
			{
				Texture2D texture;
				Rectangle r;
				Main.GetItemDrawFrame(4076, out texture, out r);
				Texture2D texture2;
				Rectangle r2;
				Main.GetItemDrawFrame(3813, out texture2, out r2);
				Texture2D texture3;
				Rectangle r3;
				Main.GetItemDrawFrame(346, out texture3, out r3);
				Texture2D texture4;
				Rectangle r4;
				Main.GetItemDrawFrame(87, out texture4, out r4);
				if (num4 > 0L)
				{
					sb.Draw(texture, Utils.CenteredRectangle(new Vector2(shopx + 96f, shopy + 50f), r.Size() * 0.65f), null, Color.White);
				}
				if (num3 > 0L)
				{
					sb.Draw(texture2, Utils.CenteredRectangle(new Vector2(shopx + 80f, shopy + 50f), r2.Size() * 0.65f), null, Color.White);
				}
				if (num2 > 0L)
				{
					sb.Draw(texture3, Utils.CenteredRectangle(new Vector2(shopx + 80f, shopy + 50f), r3.Size() * 0.65f), null, Color.White);
				}
				if (num > 0L)
				{
					sb.Draw(texture4, Utils.CenteredRectangle(new Vector2(shopx + 70f, shopy + 60f), r4.Size() * 0.65f), null, Color.White);
				}
				Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value, Lang.inter[66].Value, shopx, shopy + 40f, Color.White * ((float)Main.mouseTextColor / 255f), Color.Black, Vector2.Zero, 1f);
				customCurrencySystem.DrawSavingsMoney(sb, Lang.inter[66].Value, shopx, shopy, num5, horizontal);
			}
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x0057A6BC File Offset: 0x005788BC
		public static void GetPriceText(int currencyIndex, string[] lines, ref int currentLine, long price)
		{
			CustomCurrencyManager._currencies[currencyIndex].GetPriceText(lines, ref currentLine, price);
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x0057A6D4 File Offset: 0x005788D4
		public static bool BuyItem(Player player, long price, int currencyIndex)
		{
			CustomCurrencySystem customCurrencySystem = CustomCurrencyManager._currencies[currencyIndex];
			bool flag;
			long num = customCurrencySystem.CountCurrency(out flag, player.inventory, new int[]
			{
				58,
				57,
				56,
				55,
				54
			});
			long num2 = customCurrencySystem.CountCurrency(out flag, player.bank.item, new int[0]);
			long num3 = customCurrencySystem.CountCurrency(out flag, player.bank2.item, new int[0]);
			long num4 = customCurrencySystem.CountCurrency(out flag, player.bank3.item, new int[0]);
			long num5 = customCurrencySystem.CountCurrency(out flag, player.bank4.item, new int[0]);
			if (customCurrencySystem.CombineStacks(out flag, new long[]
			{
				num,
				num2,
				num3,
				num4,
				num5
			}) < price)
			{
				return false;
			}
			List<Item[]> list = new List<Item[]>();
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			List<Point> list2 = new List<Point>();
			List<Point> list3 = new List<Point>();
			List<Point> list4 = new List<Point>();
			List<Point> list5 = new List<Point>();
			List<Point> list6 = new List<Point>();
			List<Point> list7 = new List<Point>();
			list.Add(player.inventory);
			list.Add(player.bank.item);
			list.Add(player.bank2.item);
			list.Add(player.bank3.item);
			list.Add(player.bank4.item);
			for (int i = 0; i < list.Count; i++)
			{
				dictionary[i] = new List<int>();
			}
			dictionary[0] = new List<int>
			{
				58,
				57,
				56,
				55,
				54
			};
			for (int j = 0; j < list.Count; j++)
			{
				for (int k = 0; k < list[j].Length; k++)
				{
					if (!dictionary[j].Contains(k) && customCurrencySystem.Accepts(list[j][k]))
					{
						list3.Add(new Point(j, k));
					}
				}
			}
			CustomCurrencyManager.FindEmptySlots(list, dictionary, list2, 0);
			CustomCurrencyManager.FindEmptySlots(list, dictionary, list4, 1);
			CustomCurrencyManager.FindEmptySlots(list, dictionary, list5, 2);
			CustomCurrencyManager.FindEmptySlots(list, dictionary, list6, 3);
			CustomCurrencyManager.FindEmptySlots(list, dictionary, list7, 4);
			return customCurrencySystem.TryPurchasing(price, list, list3, list2, list4, list5, list6, list7);
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x0057A944 File Offset: 0x00578B44
		private static void FindEmptySlots(List<Item[]> inventories, Dictionary<int, List<int>> slotsToIgnore, List<Point> emptySlots, int currentInventoryIndex)
		{
			for (int i = inventories[currentInventoryIndex].Length - 1; i >= 0; i--)
			{
				if (!slotsToIgnore[currentInventoryIndex].Contains(i) && (inventories[currentInventoryIndex][i].type == 0 || inventories[currentInventoryIndex][i].stack == 0))
				{
					emptySlots.Add(new Point(currentInventoryIndex, i));
				}
			}
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x0057A9A4 File Offset: 0x00578BA4
		public static bool IsCustomCurrency(Item item)
		{
			foreach (KeyValuePair<int, CustomCurrencySystem> keyValuePair in CustomCurrencyManager._currencies)
			{
				if (keyValuePair.Value.Accepts(item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x0057AA08 File Offset: 0x00578C08
		public static void GetPrices(Item item, out long calcForSelling, out long calcForBuying)
		{
			CustomCurrencyManager._currencies[item.shopSpecialCurrency].GetItemExpectedPrice(item, out calcForSelling, out calcForBuying);
		}

		// Token: 0x040051C3 RID: 20931
		private static int _nextCurrencyIndex;

		// Token: 0x040051C4 RID: 20932
		private static Dictionary<int, CustomCurrencySystem> _currencies = new Dictionary<int, CustomCurrencySystem>();
	}
}
