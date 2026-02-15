using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;

namespace Terraria.UI
{
	// Token: 0x020000EA RID: 234
	public class ItemSorting
	{
		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060018DE RID: 6366 RVA: 0x004E5350 File Offset: 0x004E3550
		public static int LayerCount
		{
			get
			{
				return ItemSorting._layerCount;
			}
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x004E5358 File Offset: 0x004E3558
		public static void SetupWhiteLists()
		{
			ItemSorting._layerWhiteLists.Clear();
			List<ItemSorting.ItemSortingLayer> list = new List<ItemSorting.ItemSortingLayer>();
			List<Item> list2 = new List<Item>();
			List<int> list3 = new List<int>();
			list.Add(ItemSorting.ItemSortingLayers.WeaponsMelee);
			list.Add(ItemSorting.ItemSortingLayers.WeaponsRanged);
			list.Add(ItemSorting.ItemSortingLayers.WeaponsMagic);
			list.Add(ItemSorting.ItemSortingLayers.WeaponsMinions);
			list.Add(ItemSorting.ItemSortingLayers.WeaponsAssorted);
			list.Add(ItemSorting.ItemSortingLayers.WeaponsAmmo);
			list.Add(ItemSorting.ItemSortingLayers.ToolsPicksaws);
			list.Add(ItemSorting.ItemSortingLayers.ToolsHamaxes);
			list.Add(ItemSorting.ItemSortingLayers.ToolsPickaxes);
			list.Add(ItemSorting.ItemSortingLayers.ToolsAxes);
			list.Add(ItemSorting.ItemSortingLayers.ToolsHammers);
			list.Add(ItemSorting.ItemSortingLayers.ToolsTerraforming);
			list.Add(ItemSorting.ItemSortingLayers.ToolsFishing);
			list.Add(ItemSorting.ItemSortingLayers.ToolsGolf);
			list.Add(ItemSorting.ItemSortingLayers.ToolsInstruments);
			list.Add(ItemSorting.ItemSortingLayers.ToolsKeys);
			list.Add(ItemSorting.ItemSortingLayers.ToolsKites);
			list.Add(ItemSorting.ItemSortingLayers.ToolsAmmoLeftovers);
			list.Add(ItemSorting.ItemSortingLayers.ToolsMisc);
			list.Add(ItemSorting.ItemSortingLayers.ArmorCombat);
			list.Add(ItemSorting.ItemSortingLayers.ArmorVanity);
			list.Add(ItemSorting.ItemSortingLayers.ArmorAccessories);
			list.Add(ItemSorting.ItemSortingLayers.EquipGrapple);
			list.Add(ItemSorting.ItemSortingLayers.EquipMount);
			list.Add(ItemSorting.ItemSortingLayers.EquipCart);
			list.Add(ItemSorting.ItemSortingLayers.EquipLightPet);
			list.Add(ItemSorting.ItemSortingLayers.EquipVanityPet);
			list.Add(ItemSorting.ItemSortingLayers.PotionsDyes);
			list.Add(ItemSorting.ItemSortingLayers.PotionsHairDyes);
			list.Add(ItemSorting.ItemSortingLayers.PotionsLife);
			list.Add(ItemSorting.ItemSortingLayers.PotionsJustTheMushroom);
			list.Add(ItemSorting.ItemSortingLayers.PotionsMana);
			list.Add(ItemSorting.ItemSortingLayers.PotionsElixirs);
			list.Add(ItemSorting.ItemSortingLayers.PotionsBuffs);
			list.Add(ItemSorting.ItemSortingLayers.PotionsFood);
			list.Add(ItemSorting.ItemSortingLayers.MiscValuables);
			list.Add(ItemSorting.ItemSortingLayers.MiscPainting);
			list.Add(ItemSorting.ItemSortingLayers.MiscWiring);
			list.Add(ItemSorting.ItemSortingLayers.MiscMaterials);
			list.Add(ItemSorting.ItemSortingLayers.MiscJustTheGlowingMushroom);
			list.Add(ItemSorting.ItemSortingLayers.MiscRopes);
			list.Add(ItemSorting.ItemSortingLayers.MiscHerbsAndSeeds);
			list.Add(ItemSorting.ItemSortingLayers.MiscAcorns);
			list.Add(ItemSorting.ItemSortingLayers.MiscGems);
			list.Add(ItemSorting.ItemSortingLayers.MiscBossBags);
			list.Add(ItemSorting.ItemSortingLayers.MiscCritters);
			list.Add(ItemSorting.ItemSortingLayers.MiscExtractinator);
			list.Add(ItemSorting.ItemSortingLayers.LastMaterials);
			list.Add(ItemSorting.ItemSortingLayers.LastTilesImportant);
			list.Add(ItemSorting.ItemSortingLayers.LastTilesCommon);
			list.Add(ItemSorting.ItemSortingLayers.LastNotTrash);
			list.Add(ItemSorting.ItemSortingLayers.LastTrash);
			for (int i = -48; i < (int)ItemID.Count; i++)
			{
				Item item = new Item();
				item.netDefaults(i);
				list2.Add(item);
				list3.Add(i + 48);
			}
			Item[] array = list2.ToArray();
			ItemSorting._layerCount = list.Count;
			ItemSorting._layerIndexForItemType = new int[(int)ItemID.Count];
			for (int j = 0; j < list.Count; j++)
			{
				ItemSorting.ItemSortingLayer itemSortingLayer = list[j];
				List<int> list4 = itemSortingLayer.SortingMethod(itemSortingLayer, array, list3);
				List<int> list5 = new List<int>();
				for (int k = 0; k < list4.Count; k++)
				{
					Item item2 = array[list4[k]];
					list5.Add(item2.type);
					ItemSorting._layerIndexForItemType[item2.type] = j;
				}
				ItemSorting._layerWhiteLists.Add(itemSortingLayer.Name, list5);
			}
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x004E56A4 File Offset: 0x004E38A4
		private static void AddSortingPrioritiesBasedOnPlayerDamage(List<ItemSorting.ItemSortingLayer> list)
		{
			Player player = Main.player[Main.myPlayer];
			ItemSorting._damageRankings.Clear();
			ItemSorting._damageRankings.Add(new ItemSorting.DamageTypeSortingLayerEntry(player.meleeDamage, ItemSorting.ItemSortingLayers.WeaponsMelee, 0));
			ItemSorting._damageRankings.Add(new ItemSorting.DamageTypeSortingLayerEntry(player.rangedDamage, ItemSorting.ItemSortingLayers.WeaponsRanged, 1));
			ItemSorting._damageRankings.Add(new ItemSorting.DamageTypeSortingLayerEntry(player.magicDamage, ItemSorting.ItemSortingLayers.WeaponsMagic, 2));
			ItemSorting._damageRankings.Add(new ItemSorting.DamageTypeSortingLayerEntry(player.minionDamage, ItemSorting.ItemSortingLayers.WeaponsMinions, 3));
			ItemSorting._damageRankings.Sort(new Comparison<ItemSorting.DamageTypeSortingLayerEntry>(ItemSorting.Descending));
			foreach (ItemSorting.DamageTypeSortingLayerEntry damageTypeSortingLayerEntry in ItemSorting._damageRankings)
			{
				list.Add(damageTypeSortingLayerEntry.Layer);
			}
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x004E5794 File Offset: 0x004E3994
		private static int Descending(ItemSorting.DamageTypeSortingLayerEntry x, ItemSorting.DamageTypeSortingLayerEntry y)
		{
			int num = y.Multiplier.CompareTo(x.Multiplier);
			if (num == 0)
			{
				num = x.Index.CompareTo(y.Index);
			}
			return num;
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x004E57CC File Offset: 0x004E39CC
		private static void SetupSortingPriorities()
		{
			Player player = Main.player[Main.myPlayer];
			ItemSorting._layerList.Clear();
			ItemSorting.AddSortingPrioritiesBasedOnPlayerDamage(ItemSorting._layerList);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.WeaponsAssorted);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.WeaponsAmmo);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.ToolsPicksaws);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.ToolsHamaxes);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.ToolsPickaxes);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.ToolsAxes);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.ToolsHammers);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.ToolsTerraforming);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.ToolsFishing);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.ToolsGolf);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.ToolsInstruments);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.ToolsKeys);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.ToolsKites);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.ToolsAmmoLeftovers);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.ToolsMisc);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.ArmorCombat);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.ArmorVanity);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.ArmorAccessories);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.EquipGrapple);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.EquipMount);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.EquipCart);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.EquipLightPet);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.EquipVanityPet);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.PotionsDyes);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.PotionsHairDyes);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.PotionsLife);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.PotionsJustTheMushroom);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.PotionsMana);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.PotionsElixirs);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.PotionsBuffs);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.PotionsFood);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.MiscValuables);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.MiscPainting);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.MiscWiring);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.MiscMaterials);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.MiscJustTheGlowingMushroom);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.MiscRopes);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.MiscHerbsAndSeeds);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.MiscAcorns);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.MiscGems);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.MiscBossBags);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.MiscCritters);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.MiscExtractinator);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.LastMaterials);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.LastTilesImportant);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.LastTilesCommon);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.LastNotTrash);
			ItemSorting._layerList.Add(ItemSorting.ItemSortingLayers.LastTrash);
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x004E5ACC File Offset: 0x004E3CCC
		private static void Sort(bool withFeedback, Item[] inv, params int[] ignoreSlots)
		{
			ItemSorting.SetupSortingPriorities();
			ItemSorting._sort_itemsToSort.Clear();
			ItemSorting._sort_sortedItemIndexes.Clear();
			ItemSorting._sort_counts.Clear();
			ItemSorting._sort_itemsCache.Clear();
			ItemSorting._sort_availableSortingSlots.Clear();
			for (int i = 0; i < inv.Length; i++)
			{
				if (!ignoreSlots.Contains(i))
				{
					Item item = inv[i];
					if (item != null && item.stack != 0 && item.type != 0 && !item.favorited)
					{
						ItemSorting._sort_itemsToSort.Add(i);
					}
				}
			}
			for (int j = 0; j < ItemSorting._sort_itemsToSort.Count; j++)
			{
				Item item2 = inv[ItemSorting._sort_itemsToSort[j]];
				if (item2.stack < item2.maxStack)
				{
					int num = item2.maxStack - item2.stack;
					for (int k = j; k < ItemSorting._sort_itemsToSort.Count; k++)
					{
						if (j != k)
						{
							Item item3 = inv[ItemSorting._sort_itemsToSort[k]];
							if (Item.CanStack(item2, item3) && item3.stack != item3.maxStack)
							{
								int num2 = item3.stack;
								if (num < num2)
								{
									num2 = num;
								}
								item2.stack += num2;
								item3.stack -= num2;
								num -= num2;
								if (item3.stack == 0)
								{
									inv[ItemSorting._sort_itemsToSort[k]] = new Item();
									ItemSorting._sort_itemsToSort.Remove(ItemSorting._sort_itemsToSort[k]);
									j--;
									k--;
									break;
								}
								if (num == 0)
								{
									break;
								}
							}
						}
					}
				}
			}
			ItemSorting._sort_availableSortingSlots.AddRange(ItemSorting._sort_itemsToSort);
			for (int l = 0; l < inv.Length; l++)
			{
				if (!ignoreSlots.Contains(l) && !ItemSorting._sort_availableSortingSlots.Contains(l))
				{
					Item item4 = inv[l];
					if (item4 == null || item4.stack == 0 || item4.type == 0)
					{
						ItemSorting._sort_availableSortingSlots.Add(l);
					}
				}
			}
			ItemSorting._sort_availableSortingSlots.Sort();
			foreach (ItemSorting.ItemSortingLayer itemSortingLayer in ItemSorting._layerList)
			{
				List<int> list = itemSortingLayer.SortingMethod(itemSortingLayer, inv, ItemSorting._sort_itemsToSort);
				if (list.Count > 0)
				{
					ItemSorting._sort_counts.Add(list.Count);
				}
				ItemSorting._sort_sortedItemIndexes.AddRange(list);
			}
			ItemSorting._sort_sortedItemIndexes.AddRange(ItemSorting._sort_itemsToSort);
			foreach (int num3 in ItemSorting._sort_sortedItemIndexes)
			{
				ItemSorting._sort_itemsCache.Add(inv[num3]);
				inv[num3] = new Item();
			}
			float num4 = 1f / (float)ItemSorting._sort_counts.Count;
			float num5 = num4 / 2f;
			for (int m = 0; m < ItemSorting._sort_itemsCache.Count; m++)
			{
				int num6 = ItemSorting._sort_availableSortingSlots[0];
				if (withFeedback)
				{
					ItemSlot.SetGlow(num6, num5, Main.player[Main.myPlayer].chest != -1);
				}
				List<int> sort_counts = ItemSorting._sort_counts;
				int num7 = sort_counts[0];
				sort_counts[0] = num7 - 1;
				if (ItemSorting._sort_counts[0] == 0)
				{
					ItemSorting._sort_counts.RemoveAt(0);
					num5 += num4;
				}
				inv[num6] = ItemSorting._sort_itemsCache[m];
				ItemSorting._sort_availableSortingSlots.Remove(num6);
			}
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x004E5E7C File Offset: 0x004E407C
		public static string GetSortingLayer(int itemType)
		{
			foreach (KeyValuePair<string, List<int>> keyValuePair in ItemSorting._layerWhiteLists)
			{
				if (keyValuePair.Value.Contains(itemType))
				{
					return keyValuePair.Key;
				}
			}
			return null;
		}

		// Token: 0x060018E5 RID: 6373 RVA: 0x004E5EE4 File Offset: 0x004E40E4
		public static int GetSortingLayerIndex(int itemType)
		{
			return ItemSorting._layerIndexForItemType[itemType];
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x004E5EF0 File Offset: 0x004E40F0
		public static void SortInventory()
		{
			if (Main.LocalPlayer.HasLockedInventory())
			{
				return;
			}
			if (!Main.LocalPlayer.HasItem(905))
			{
				ItemSorting.SortCoins();
			}
			ItemSorting.SortAmmo();
			ItemSorting.Sort(true, Main.player[Main.myPlayer].inventory, new int[]
			{
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				50,
				51,
				52,
				53,
				54,
				55,
				56,
				57,
				58
			});
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x004E5F50 File Offset: 0x004E4150
		public static void SortChest()
		{
			int chest = Main.player[Main.myPlayer].chest;
			if (chest == -1)
			{
				return;
			}
			Item[] item = Main.player[Main.myPlayer].bank.item;
			if (chest == -3)
			{
				Item[] item2 = Main.player[Main.myPlayer].bank2.item;
			}
			if (chest == -4)
			{
				Item[] item3 = Main.player[Main.myPlayer].bank3.item;
			}
			if (chest == -5)
			{
				Item[] item4 = Main.player[Main.myPlayer].bank4.item;
			}
			if (chest > -1)
			{
				Item[] item5 = Main.chest[chest].item;
			}
			ItemSorting.SortInventory(Main.LocalPlayer.GetCurrentContainer(), true, true);
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x004E5FFC File Offset: 0x004E41FC
		public static void SortInventory(Chest chest, bool withSync, bool withFeedback)
		{
			Item[] item = chest.item;
			Array.Resize<ItemSorting.MemoryStamp>(ref ItemSorting._sortInventory_preStamps, chest.maxItems);
			Array.Resize<ItemSorting.MemoryStamp>(ref ItemSorting._sortInventory_postStamps, chest.maxItems);
			for (int i = 0; i < chest.maxItems; i++)
			{
				ItemSorting._sortInventory_preStamps[i] = new ItemSorting.MemoryStamp(item[i]);
			}
			ItemSorting.Sort(withFeedback, item, new int[0]);
			for (int j = 0; j < chest.maxItems; j++)
			{
				ItemSorting._sortInventory_postStamps[j] = new ItemSorting.MemoryStamp(item[j]);
			}
			if (withSync && Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
			{
				for (int k = 0; k < chest.maxItems; k++)
				{
					if (ItemSorting._sortInventory_postStamps[k] != ItemSorting._sortInventory_preStamps[k])
					{
						NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, (float)k, 0f, 0f, 0, 0, 0);
					}
				}
			}
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x004E60FA File Offset: 0x004E42FA
		public static void SortAmmo()
		{
			ItemSorting.ClearAmmoSlotSpaces();
			ItemSorting.FillAmmoFromInventory();
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x004E6108 File Offset: 0x004E4308
		public static void FillAmmoFromInventory()
		{
			ItemSorting._fillAmmoFromInventory_acceptedAmmoTypes.Clear();
			ItemSorting._fillAmmoFromInventory_emptyAmmoSlots.Clear();
			Item[] inventory = Main.player[Main.myPlayer].inventory;
			for (int i = 54; i < 58; i++)
			{
				ItemSlot.SetGlow(i, 0.31f, false);
				Item item = inventory[i];
				if (item.IsAir)
				{
					ItemSorting._fillAmmoFromInventory_emptyAmmoSlots.Add(i);
				}
				else if (item.ammo != AmmoID.None)
				{
					if (!ItemSorting._fillAmmoFromInventory_acceptedAmmoTypes.Contains(item.type))
					{
						ItemSorting._fillAmmoFromInventory_acceptedAmmoTypes.Add(item.type);
					}
					ItemSorting.RefillItemStack(inventory, inventory[i], 0, 50);
				}
			}
			if (ItemSorting._fillAmmoFromInventory_emptyAmmoSlots.Count < 1)
			{
				return;
			}
			for (int j = 0; j < 50; j++)
			{
				Item item2 = inventory[j];
				if (item2.stack >= 1 && item2.CanFillEmptyAmmoSlot() && ItemSorting._fillAmmoFromInventory_acceptedAmmoTypes.Contains(item2.type) && !item2.favorited)
				{
					int num = ItemSorting._fillAmmoFromInventory_emptyAmmoSlots[0];
					ItemSorting._fillAmmoFromInventory_emptyAmmoSlots.Remove(num);
					Utils.Swap<Item>(ref inventory[j], ref inventory[num]);
					ItemSorting.RefillItemStack(inventory, inventory[num], 0, 50);
					if (ItemSorting._fillAmmoFromInventory_emptyAmmoSlots.Count == 0)
					{
						break;
					}
				}
			}
			if (ItemSorting._fillAmmoFromInventory_emptyAmmoSlots.Count < 1)
			{
				return;
			}
			for (int k = 0; k < 50; k++)
			{
				Item item3 = inventory[k];
				if (item3.stack >= 1 && item3.CanFillEmptyAmmoSlot() && item3.FitsAmmoSlot() && !item3.favorited)
				{
					int num2 = ItemSorting._fillAmmoFromInventory_emptyAmmoSlots[0];
					ItemSorting._fillAmmoFromInventory_emptyAmmoSlots.Remove(num2);
					Utils.Swap<Item>(ref inventory[k], ref inventory[num2]);
					ItemSorting.RefillItemStack(inventory, inventory[num2], 0, 50);
					if (ItemSorting._fillAmmoFromInventory_emptyAmmoSlots.Count == 0)
					{
						break;
					}
				}
			}
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x004E62D4 File Offset: 0x004E44D4
		public static void ClearAmmoSlotSpaces()
		{
			Item[] inventory = Main.player[Main.myPlayer].inventory;
			for (int i = 54; i < 58; i++)
			{
				Item item = inventory[i];
				if (!item.IsAir && item.ammo != AmmoID.None && item.stack < item.maxStack)
				{
					int loopStartIndex = item.favorited ? 54 : (i + 1);
					ItemSorting.RefillItemStack(inventory, item, loopStartIndex, 58);
				}
			}
			for (int j = 54; j < 58; j++)
			{
				if (inventory[j].type > 0 && !inventory[j].favorited)
				{
					ItemSorting.TrySlidingUp(inventory, j, 54);
				}
			}
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x004E6374 File Offset: 0x004E4574
		private static void SortCoins()
		{
			Item[] inventory = Main.LocalPlayer.inventory;
			bool flag;
			long count = Utils.CoinsCount(out flag, inventory, new int[]
			{
				58
			});
			int commonMaxStack = Item.CommonMaxStack;
			if (flag)
			{
				return;
			}
			int[] array = Utils.CoinsSplit(count);
			int num = 0;
			for (int i = 0; i < 3; i++)
			{
				int j = array[i];
				while (j > 0)
				{
					j -= 99;
					num++;
				}
			}
			int k = array[3];
			while (k > commonMaxStack)
			{
				k -= commonMaxStack;
				num++;
			}
			int num2 = 0;
			for (int l = 0; l < 58; l++)
			{
				if (inventory[l].type >= 71 && inventory[l].type <= 74 && inventory[l].stack > 0)
				{
					num2++;
				}
			}
			if (num2 < num)
			{
				return;
			}
			for (int m = 0; m < 58; m++)
			{
				if (inventory[m].type >= 71 && inventory[m].type <= 74 && inventory[m].stack > 0)
				{
					inventory[m].TurnToAir(false);
				}
			}
			int num3 = 100;
			do
			{
				int num4 = -1;
				for (int n = 3; n >= 0; n--)
				{
					if (array[n] > 0)
					{
						num4 = n;
						break;
					}
				}
				if (num4 == -1)
				{
					return;
				}
				int num5 = array[num4];
				if (num4 == 3 && num5 > commonMaxStack)
				{
					num5 = commonMaxStack;
				}
				bool flag2 = false;
				if (!flag2)
				{
					for (int num6 = 50; num6 < 54; num6++)
					{
						if (inventory[num6].IsAir)
						{
							inventory[num6].SetDefaults(71 + num4, null);
							inventory[num6].stack = num5;
							array[num4] -= num5;
							flag2 = true;
							break;
						}
					}
				}
				if (!flag2)
				{
					for (int num7 = 0; num7 < 50; num7++)
					{
						if (inventory[num7].IsAir)
						{
							inventory[num7].SetDefaults(71 + num4, null);
							inventory[num7].stack = num5;
							array[num4] -= num5;
							break;
						}
					}
				}
				num3--;
			}
			while (num3 > 0);
			for (int num8 = 3; num8 >= 0; num8--)
			{
				if (array[num8] > 0)
				{
					Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetItemSource_InventoryOverflow(), 71 + num8, array[num8]);
				}
			}
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x004E65A8 File Offset: 0x004E47A8
		private static void RefillItemStack(Item[] inv, Item itemToRefill, int loopStartIndex, int loopEndIndex)
		{
			int num = itemToRefill.maxStack - itemToRefill.stack;
			if (num <= 0)
			{
				return;
			}
			for (int i = loopStartIndex; i < loopEndIndex; i++)
			{
				Item item = inv[i];
				if (item.stack >= 1 && item.type == itemToRefill.type && !item.favorited)
				{
					int num2 = item.stack;
					if (num2 > num)
					{
						num2 = num;
					}
					num -= num2;
					itemToRefill.stack += num2;
					item.stack -= num2;
					if (item.stack <= 0)
					{
						item.TurnToAir(false);
					}
					if (num <= 0)
					{
						break;
					}
				}
			}
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x004E6638 File Offset: 0x004E4838
		private static void TrySlidingUp(Item[] inv, int slot, int minimumIndex)
		{
			for (int i = minimumIndex; i < slot; i++)
			{
				if (inv[i].IsAir)
				{
					Utils.Swap<Item>(ref inv[i], ref inv[slot]);
					return;
				}
			}
		}

		// Token: 0x04001300 RID: 4864
		private static List<ItemSorting.ItemSortingLayer> _layerList = new List<ItemSorting.ItemSortingLayer>();

		// Token: 0x04001301 RID: 4865
		private static Dictionary<string, List<int>> _layerWhiteLists = new Dictionary<string, List<int>>();

		// Token: 0x04001302 RID: 4866
		private static int[] _layerIndexForItemType;

		// Token: 0x04001303 RID: 4867
		private static int _layerCount;

		// Token: 0x04001304 RID: 4868
		private static List<ItemSorting.DamageTypeSortingLayerEntry> _damageRankings = new List<ItemSorting.DamageTypeSortingLayerEntry>();

		// Token: 0x04001305 RID: 4869
		private static readonly List<int> _sort_itemsToSort = new List<int>();

		// Token: 0x04001306 RID: 4870
		private static readonly List<int> _sort_sortedItemIndexes = new List<int>();

		// Token: 0x04001307 RID: 4871
		private static readonly List<int> _sort_counts = new List<int>();

		// Token: 0x04001308 RID: 4872
		private static readonly List<Item> _sort_itemsCache = new List<Item>();

		// Token: 0x04001309 RID: 4873
		private static readonly List<int> _sort_availableSortingSlots = new List<int>();

		// Token: 0x0400130A RID: 4874
		private static ItemSorting.MemoryStamp[] _sortInventory_preStamps = new ItemSorting.MemoryStamp[0];

		// Token: 0x0400130B RID: 4875
		private static ItemSorting.MemoryStamp[] _sortInventory_postStamps = new ItemSorting.MemoryStamp[0];

		// Token: 0x0400130C RID: 4876
		private static readonly List<int> _fillAmmoFromInventory_acceptedAmmoTypes = new List<int>();

		// Token: 0x0400130D RID: 4877
		private static readonly List<int> _fillAmmoFromInventory_emptyAmmoSlots = new List<int>();

		// Token: 0x02000700 RID: 1792
		private class ItemSortingLayer
		{
			// Token: 0x06003FC5 RID: 16325 RVA: 0x0069AA8D File Offset: 0x00698C8D
			public ItemSortingLayer(string name, Func<ItemSorting.ItemSortingLayer, Item[], List<int>, List<int>> method)
			{
				this.Name = name;
				this.SortingMethod = method;
			}

			// Token: 0x06003FC6 RID: 16326 RVA: 0x0069AAA4 File Offset: 0x00698CA4
			public void Validate(ref List<int> indexesSortable, Item[] inv)
			{
				List<int> list;
				if (ItemSorting._layerWhiteLists.TryGetValue(this.Name, out list))
				{
					indexesSortable = (from i in indexesSortable
					where list.Contains(inv[i].type)
					select i).ToList<int>();
				}
			}

			// Token: 0x06003FC7 RID: 16327 RVA: 0x0069AAF0 File Offset: 0x00698CF0
			public override string ToString()
			{
				return this.Name;
			}

			// Token: 0x04006829 RID: 26665
			public readonly string Name;

			// Token: 0x0400682A RID: 26666
			public readonly Func<ItemSorting.ItemSortingLayer, Item[], List<int>, List<int>> SortingMethod;
		}

		// Token: 0x02000701 RID: 1793
		private class ItemSortingLayers
		{
			// Token: 0x06003FC8 RID: 16328 RVA: 0x0069AAF8 File Offset: 0x00698CF8
			private static void SortIndicesStable(List<int> list, Comparison<int> comparison)
			{
				list.Sort(delegate(int x, int y)
				{
					int num = comparison(x, y);
					if (num == 0)
					{
						num = x.CompareTo(y);
					}
					return num;
				});
			}

			// Token: 0x06003FC9 RID: 16329 RVA: 0x0069AB24 File Offset: 0x00698D24
			public static int CompareWithPrioritySet(int[] prioritySet, int typeOne, int typeTwo)
			{
				if (typeOne < 0 || typeTwo < 0)
				{
					return 0;
				}
				if (prioritySet[typeOne] >= 0 && prioritySet[typeTwo] < 0)
				{
					return -1;
				}
				if (prioritySet[typeOne] < 0 && prioritySet[typeTwo] >= 0)
				{
					return 1;
				}
				if (prioritySet[typeOne] < 0 && prioritySet[typeTwo] < 0)
				{
					return 0;
				}
				return prioritySet[typeOne].CompareTo(prioritySet[typeTwo]);
			}

			// Token: 0x0400682B RID: 26667
			public static ItemSorting.ItemSortingLayer WeaponsMelee = new ItemSorting.ItemSortingLayer("Weapons - Melee", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].damage > 0 && !inv[i].consumable && inv[i].ammo == 0 && inv[i].melee && inv[i].pick < 1 && inv[i].hammer < 1 && inv[i].axe < 1
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num == 0)
					{
						num = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400682C RID: 26668
			public static ItemSorting.ItemSortingLayer WeaponsRanged = new ItemSorting.ItemSortingLayer("Weapons - Ranged", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where (inv[i].damage > 0 && !inv[i].consumable && inv[i].ammo == 0 && inv[i].ranged) || (inv[i].type >= 0 && ItemID.Sets.SortingPriorityWeaponsRanged[inv[i].type] > -1)
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemSorting.ItemSortingLayers.CompareWithPrioritySet(ItemID.Sets.SortingPriorityWeaponsRanged, inv[x].type, inv[y].type);
					if (num == 0)
					{
						num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					}
					if (num == 0)
					{
						num = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400682D RID: 26669
			public static ItemSorting.ItemSortingLayer WeaponsMagic = new ItemSorting.ItemSortingLayer("Weapons - Magic", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].damage > 0 && !inv[i].consumable && inv[i].ammo == 0 && inv[i].magic
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num == 0)
					{
						num = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400682E RID: 26670
			public static ItemSorting.ItemSortingLayer WeaponsMinions = new ItemSorting.ItemSortingLayer("Weapons - Minions", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].damage > 0 && !inv[i].consumable && inv[i].summon
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num == 0)
					{
						num = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400682F RID: 26671
			public static ItemSorting.ItemSortingLayer WeaponsAssorted = new ItemSorting.ItemSortingLayer("Weapons - Assorted", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].damage > 0 && inv[i].ammo == 0 && inv[i].pick == 0 && inv[i].axe == 0 && inv[i].hammer == 0
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num == 0)
					{
						num = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006830 RID: 26672
			public static ItemSorting.ItemSortingLayer WeaponsAmmo = new ItemSorting.ItemSortingLayer("Weapons - Ammo", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].ammo > 0 && inv[i].damage > 0
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num == 0)
					{
						num = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006831 RID: 26673
			public static ItemSorting.ItemSortingLayer ToolsPicksaws = new ItemSorting.ItemSortingLayer("Tools - Picksaws", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].pick > 0 && inv[i].axe > 0
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, (int x, int y) => inv[x].pick.CompareTo(inv[y].pick));
				return list;
			});

			// Token: 0x04006832 RID: 26674
			public static ItemSorting.ItemSortingLayer ToolsHamaxes = new ItemSorting.ItemSortingLayer("Tools - Hamaxes", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].hammer > 0 && inv[i].axe > 0
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, (int x, int y) => inv[x].axe.CompareTo(inv[y].axe));
				return list;
			});

			// Token: 0x04006833 RID: 26675
			public static ItemSorting.ItemSortingLayer ToolsPickaxes = new ItemSorting.ItemSortingLayer("Tools - Pickaxes", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].pick > 0
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, (int x, int y) => inv[x].pick.CompareTo(inv[y].pick));
				return list;
			});

			// Token: 0x04006834 RID: 26676
			public static ItemSorting.ItemSortingLayer ToolsAxes = new ItemSorting.ItemSortingLayer("Tools - Axes", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].axe > 0
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, (int x, int y) => inv[x].axe.CompareTo(inv[y].axe));
				return list;
			});

			// Token: 0x04006835 RID: 26677
			public static ItemSorting.ItemSortingLayer ToolsHammers = new ItemSorting.ItemSortingLayer("Tools - Hammers", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].hammer > 0
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, (int x, int y) => inv[x].hammer.CompareTo(inv[y].hammer));
				return list;
			});

			// Token: 0x04006836 RID: 26678
			public static ItemSorting.ItemSortingLayer ToolsTerraforming = new ItemSorting.ItemSortingLayer("Tools - Terraforming", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].type > 0 && ItemID.Sets.SortingPriorityTerraforming[inv[i].type] > -1
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemID.Sets.SortingPriorityTerraforming[inv[x].type].CompareTo(ItemID.Sets.SortingPriorityTerraforming[inv[y].type]);
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006837 RID: 26679
			public static ItemSorting.ItemSortingLayer ToolsFishing = new ItemSorting.ItemSortingLayer("Tools - Fishing", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].fishingPole > 0 || inv[i].bait > 0 || inv[i].questItem || (inv[i].type > 0 && (ItemID.Sets.IsFishingCrate[inv[i].type] || ItemID.Sets.IsBasicFish[inv[i].type] || ItemID.Sets.SortingPriorityToolsFishing[inv[i].type] > -1))
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemSorting.ItemSortingLayers.CompareWithPrioritySet(ItemID.Sets.SortingPriorityToolsFishing, inv[x].type, inv[y].type);
					if (num == 0)
					{
						num = inv[y].fishingPole.CompareTo(inv[x].fishingPole);
					}
					if (num == 0)
					{
						num = inv[y].bait.CompareTo(inv[x].bait);
					}
					if (num == 0)
					{
						num = inv[y].questItem.CompareTo(inv[x].questItem);
					}
					if (num == 0 && inv[y].type >= 0 && inv[x].type >= 0)
					{
						if (num == 0)
						{
							num = ItemID.Sets.IsFishingCrate[inv[y].type].CompareTo(ItemID.Sets.IsFishingCrate[inv[x].type]);
						}
						if (num == 0)
						{
							num = ItemID.Sets.IsBasicFish[inv[y].type].CompareTo(ItemID.Sets.IsBasicFish[inv[x].type]);
						}
					}
					if (num == 0)
					{
						num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					}
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006838 RID: 26680
			public static ItemSorting.ItemSortingLayer ToolsGolf = new ItemSorting.ItemSortingLayer("Tools - Golf", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].type > 0 && ItemID.Sets.SortingPriorityToolsGolf[inv[i].type] > -1
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemID.Sets.SortingPriorityToolsGolf[inv[x].type].CompareTo(ItemID.Sets.SortingPriorityToolsGolf[inv[y].type]);
					if (num == 0)
					{
						num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					}
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006839 RID: 26681
			public static ItemSorting.ItemSortingLayer ToolsInstruments = new ItemSorting.ItemSortingLayer("Tools - Instruments", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].type > 0 && ItemID.Sets.SortingPriorityToolsInstruments[inv[i].type] > -1
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemID.Sets.SortingPriorityToolsInstruments[inv[x].type].CompareTo(ItemID.Sets.SortingPriorityToolsInstruments[inv[y].type]);
					if (num == 0)
					{
						num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					}
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400683A RID: 26682
			public static ItemSorting.ItemSortingLayer ToolsKeys = new ItemSorting.ItemSortingLayer("Tools - Keys", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].type > 0 && ItemID.Sets.SortingPriorityToolsKeys[inv[i].type] > -1
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemID.Sets.SortingPriorityToolsKeys[inv[x].type].CompareTo(ItemID.Sets.SortingPriorityToolsKeys[inv[y].type]);
					if (num == 0)
					{
						num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					}
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400683B RID: 26683
			public static ItemSorting.ItemSortingLayer ToolsKites = new ItemSorting.ItemSortingLayer("Tools - Kites", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].type > 0 && ItemID.Sets.SortingPriorityToolsKites[inv[i].type] > -1
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemID.Sets.SortingPriorityToolsKites[inv[x].type].CompareTo(ItemID.Sets.SortingPriorityToolsKites[inv[y].type]);
					if (num == 0)
					{
						num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					}
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400683C RID: 26684
			public static ItemSorting.ItemSortingLayer ToolsAmmoLeftovers = new ItemSorting.ItemSortingLayer("Weapons - Ammo Leftovers", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].ammo > 0 && inv[i].type >= 0 && inv[i].type < (int)ItemID.Count && !ItemID.Sets.IsFood[inv[i].type] && ItemID.Sets.SortingPriorityMiscAcorns[inv[i].type] == -1
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num == 0)
					{
						num = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400683D RID: 26685
			public static ItemSorting.ItemSortingLayer ToolsMisc = new ItemSorting.ItemSortingLayer("Tools - Misc", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].type > 0 && ItemID.Sets.SortingPriorityToolsMisc[inv[i].type] > -1
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemID.Sets.SortingPriorityToolsMisc[inv[x].type].CompareTo(ItemID.Sets.SortingPriorityToolsMisc[inv[y].type]);
					if (num == 0)
					{
						num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					}
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400683E RID: 26686
			public static ItemSorting.ItemSortingLayer ArmorCombat = new ItemSorting.ItemSortingLayer("Armor - Combat", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where (inv[i].bodySlot >= 0 || inv[i].headSlot >= 0 || inv[i].legSlot >= 0) && !inv[i].vanity
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num == 0)
					{
						num = inv[y].OriginalDefense.CompareTo(inv[x].OriginalDefense);
					}
					if (num == 0)
					{
						num = inv[x].type.CompareTo(inv[y].type);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400683F RID: 26687
			public static ItemSorting.ItemSortingLayer ArmorVanity = new ItemSorting.ItemSortingLayer("Armor - Vanity", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where (inv[i].bodySlot >= 0 || inv[i].headSlot >= 0 || inv[i].legSlot >= 0) && inv[i].vanity
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num == 0)
					{
						num = inv[x].type.CompareTo(inv[y].type);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006840 RID: 26688
			public static ItemSorting.ItemSortingLayer ArmorAccessories = new ItemSorting.ItemSortingLayer("Armor - Accessories", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].accessory
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[x].vanity.CompareTo(inv[y].vanity);
					if (num == 0)
					{
						num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					}
					if (num == 0)
					{
						num = inv[y].OriginalDefense.CompareTo(inv[x].OriginalDefense);
					}
					if (num == 0)
					{
						num = inv[x].type.CompareTo(inv[y].type);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006841 RID: 26689
			public static ItemSorting.ItemSortingLayer EquipGrapple = new ItemSorting.ItemSortingLayer("Equip - Grapple", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where Main.projHook[inv[i].shoot]
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num == 0)
					{
						num = inv[x].type.CompareTo(inv[y].type);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006842 RID: 26690
			public static ItemSorting.ItemSortingLayer EquipMount = new ItemSorting.ItemSortingLayer("Equip - Mount", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].mountType != -1 && !MountID.Sets.Cart[inv[i].mountType]
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num == 0)
					{
						num = inv[x].type.CompareTo(inv[y].type);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006843 RID: 26691
			public static ItemSorting.ItemSortingLayer EquipCart = new ItemSorting.ItemSortingLayer("Equip - Cart", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].mountType != -1 && MountID.Sets.Cart[inv[i].mountType]
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num == 0)
					{
						num = inv[x].type.CompareTo(inv[y].type);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006844 RID: 26692
			public static ItemSorting.ItemSortingLayer EquipLightPet = new ItemSorting.ItemSortingLayer("Equip - Light Pet", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].buffType > 0 && Main.lightPet[inv[i].buffType]
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num == 0)
					{
						num = inv[x].type.CompareTo(inv[y].type);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006845 RID: 26693
			public static ItemSorting.ItemSortingLayer EquipVanityPet = new ItemSorting.ItemSortingLayer("Equip - Vanity Pet", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].buffType > 0 && Main.vanityPet[inv[i].buffType]
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num == 0)
					{
						num = inv[x].type.CompareTo(inv[y].type);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006846 RID: 26694
			public static ItemSorting.ItemSortingLayer PotionsLife = new ItemSorting.ItemSortingLayer("Potions - Life", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].consumable && inv[i].healLife > 0 && inv[i].healMana < 1 && inv[i].type != 5
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].healLife.CompareTo(inv[x].healLife);
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006847 RID: 26695
			public static ItemSorting.ItemSortingLayer PotionsJustTheMushroom = new ItemSorting.ItemSortingLayer("Potions - Just The Mushroom", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].type == 5
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, (int x, int y) => inv[y].stack.CompareTo(inv[x].stack));
				return list;
			});

			// Token: 0x04006848 RID: 26696
			public static ItemSorting.ItemSortingLayer PotionsMana = new ItemSorting.ItemSortingLayer("Potions - Mana", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].consumable && inv[i].healLife < 1 && inv[i].healMana > 0
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].healMana.CompareTo(inv[x].healMana);
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006849 RID: 26697
			public static ItemSorting.ItemSortingLayer PotionsElixirs = new ItemSorting.ItemSortingLayer("Potions - Elixirs", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].consumable && inv[i].healLife > 0 && inv[i].healMana > 0
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].healLife.CompareTo(inv[x].healLife);
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400684A RID: 26698
			public static ItemSorting.ItemSortingLayer PotionsBuffs = new ItemSorting.ItemSortingLayer("Potions - Buffs", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where (inv[i].consumable && inv[i].buffType > 0 && inv[i].type >= 0 && inv[i].type < (int)ItemID.Count && !ItemID.Sets.IsFood[inv[i].type]) || (inv[i].type >= 0 && ItemID.Sets.SortingPriorityPotionsBuffs[inv[i].type] > -1)
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemSorting.ItemSortingLayers.CompareWithPrioritySet(ItemID.Sets.SortingPriorityPotionsBuffs, inv[x].type, inv[y].type);
					if (num == 0)
					{
						num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					}
					if (num == 0)
					{
						num = inv[x].type.CompareTo(inv[y].type);
					}
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400684B RID: 26699
			public static ItemSorting.ItemSortingLayer PotionsFood = new ItemSorting.ItemSortingLayer("Potions - Food", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].consumable && inv[i].buffType > 0 && inv[i].type >= 0 && inv[i].type < (int)ItemID.Count && ItemID.Sets.IsFood[inv[i].type]
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = (inv[y].buffType < 0 || inv[y].buffType >= BuffID.Count) ? 0 : BuffID.Sets.SortingPriorityFoodBuffs[inv[y].buffType].CompareTo(BuffID.Sets.SortingPriorityFoodBuffs[inv[x].buffType]);
					if (num == 0)
					{
						num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					}
					if (num == 0)
					{
						num = inv[x].type.CompareTo(inv[y].type);
					}
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400684C RID: 26700
			public static ItemSorting.ItemSortingLayer PotionsDyes = new ItemSorting.ItemSortingLayer("Potions - Dyes", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].dye > 0 || (inv[i].type >= 0 && ItemID.Sets.SortingPriorityPotionsDyeMaterial[inv[i].type] > -1)
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num == 0)
					{
						num = inv[y].dye.CompareTo(inv[x].dye);
					}
					if (num == 0)
					{
						num = ItemSorting.ItemSortingLayers.CompareWithPrioritySet(ItemID.Sets.SortingPriorityPotionsDyeMaterial, inv[x].type, inv[y].type);
					}
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400684D RID: 26701
			public static ItemSorting.ItemSortingLayer PotionsHairDyes = new ItemSorting.ItemSortingLayer("Potions - Hair Dyes", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].hairDye >= 0
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num == 0)
					{
						num = inv[y].hairDye.CompareTo(inv[x].hairDye);
					}
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400684E RID: 26702
			public static ItemSorting.ItemSortingLayer MiscValuables = new ItemSorting.ItemSortingLayer("Misc - Importants", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].type > 0 && ItemID.Sets.SortingPriorityMiscImportants[inv[i].type] > -1
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemID.Sets.SortingPriorityMiscImportants[inv[x].type].CompareTo(ItemID.Sets.SortingPriorityMiscImportants[inv[y].type]);
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400684F RID: 26703
			public static ItemSorting.ItemSortingLayer MiscWiring = new ItemSorting.ItemSortingLayer("Misc - Wiring", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where (inv[i].type > 0 && ItemID.Sets.SortingPriorityWiring[inv[i].type] > -1) || inv[i].mech
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemID.Sets.SortingPriorityWiring[inv[y].type].CompareTo(ItemID.Sets.SortingPriorityWiring[inv[x].type]);
					if (num == 0)
					{
						num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					}
					if (num == 0)
					{
						num = inv[y].type.CompareTo(inv[x].type);
					}
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006850 RID: 26704
			public static ItemSorting.ItemSortingLayer MiscMaterials = new ItemSorting.ItemSortingLayer("Misc - Materials", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].type > 0 && ItemID.Sets.SortingPriorityMaterials[inv[i].type] > -1
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemID.Sets.SortingPriorityMaterials[inv[y].type].CompareTo(ItemID.Sets.SortingPriorityMaterials[inv[x].type]);
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006851 RID: 26705
			public static ItemSorting.ItemSortingLayer MiscJustTheGlowingMushroom = new ItemSorting.ItemSortingLayer("Misc - Just The Glowing Mushroom", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].type == 183
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, (int x, int y) => inv[y].stack.CompareTo(inv[x].stack));
				return list;
			});

			// Token: 0x04006852 RID: 26706
			public static ItemSorting.ItemSortingLayer MiscExtractinator = new ItemSorting.ItemSortingLayer("Misc - Extractinator", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].type > 0 && ItemID.Sets.SortingPriorityExtractibles[inv[i].type] > -1
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemID.Sets.SortingPriorityExtractibles[inv[y].type].CompareTo(ItemID.Sets.SortingPriorityExtractibles[inv[x].type]);
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006853 RID: 26707
			public static ItemSorting.ItemSortingLayer MiscPainting = new ItemSorting.ItemSortingLayer("Misc - Painting", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where (inv[i].type > 0 && ItemID.Sets.SortingPriorityPainting[inv[i].type] > -1) || inv[i].paint > 0
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemID.Sets.SortingPriorityPainting[inv[y].type].CompareTo(ItemID.Sets.SortingPriorityPainting[inv[x].type]);
					if (num == 0)
					{
						num = inv[x].paint.CompareTo(inv[y].paint);
					}
					if (num == 0)
					{
						num = inv[x].paintCoating.CompareTo(inv[y].paintCoating);
					}
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006854 RID: 26708
			public static ItemSorting.ItemSortingLayer MiscRopes = new ItemSorting.ItemSortingLayer("Misc - Ropes", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].type > 0 && ItemID.Sets.SortingPriorityRopes[inv[i].type] > -1
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemID.Sets.SortingPriorityRopes[inv[y].type].CompareTo(ItemID.Sets.SortingPriorityRopes[inv[x].type]);
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006855 RID: 26709
			public static ItemSorting.ItemSortingLayer MiscHerbsAndSeeds = new ItemSorting.ItemSortingLayer("Misc - Herbs And Seeds", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].type > 0 && ItemID.Sets.SortingPriorityMiscHerbsAndSeeds[inv[i].type] > -1
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemID.Sets.SortingPriorityMiscHerbsAndSeeds[inv[y].type].CompareTo(ItemID.Sets.SortingPriorityMiscHerbsAndSeeds[inv[x].type]);
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006856 RID: 26710
			public static ItemSorting.ItemSortingLayer MiscGems = new ItemSorting.ItemSortingLayer("Misc - Gems", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].type > 0 && ItemID.Sets.SortingPriorityMiscGems[inv[i].type] > -1
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemID.Sets.SortingPriorityMiscGems[inv[y].type].CompareTo(ItemID.Sets.SortingPriorityMiscGems[inv[x].type]);
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006857 RID: 26711
			public static ItemSorting.ItemSortingLayer MiscAcorns = new ItemSorting.ItemSortingLayer("Misc - Acorns", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].type > 0 && ItemID.Sets.SortingPriorityMiscAcorns[inv[i].type] > -1
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemID.Sets.SortingPriorityMiscAcorns[inv[y].type].CompareTo(ItemID.Sets.SortingPriorityMiscAcorns[inv[x].type]);
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006858 RID: 26712
			public static ItemSorting.ItemSortingLayer MiscBossBags = new ItemSorting.ItemSortingLayer("Misc - Boss Bags", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].type > 0 && ItemID.Sets.SortingPriorityMiscBossBags[inv[i].type] > -1
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = ItemID.Sets.SortingPriorityMiscBossBags[inv[x].type].CompareTo(ItemID.Sets.SortingPriorityMiscBossBags[inv[y].type]);
					if (num == 0)
					{
						num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					}
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x04006859 RID: 26713
			public static ItemSorting.ItemSortingLayer MiscCritters = new ItemSorting.ItemSortingLayer("Misc - Critters", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].makeNPC > 0
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[x].makeNPC.CompareTo(inv[y].makeNPC);
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400685A RID: 26714
			public static ItemSorting.ItemSortingLayer LastMaterials = new ItemSorting.ItemSortingLayer("Last - Materials", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].createTile < 0 && inv[i].createWall < 1 && inv[i].rare != -1
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num == 0)
					{
						num = inv[y].value.CompareTo(inv[x].value);
					}
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400685B RID: 26715
			public static ItemSorting.ItemSortingLayer LastTilesImportant = new ItemSorting.ItemSortingLayer("Last - Tiles (Frame Important)", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].createTile >= 0 && Main.tileFrameImportant[inv[i].createTile]
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = string.Compare(inv[x].Name, inv[y].Name, StringComparison.OrdinalIgnoreCase);
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400685C RID: 26716
			public static ItemSorting.ItemSortingLayer LastTilesCommon = new ItemSorting.ItemSortingLayer("Last - Tiles (Common), Walls", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].createWall > 0 || inv[i].createTile >= 0
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = string.Compare(inv[x].Name, inv[y].Name, StringComparison.OrdinalIgnoreCase);
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400685D RID: 26717
			public static ItemSorting.ItemSortingLayer LastNotTrash = new ItemSorting.ItemSortingLayer("Last - Not Trash", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = (from i in itemsToSort
				where inv[i].OriginalRarity >= 0
				select i).ToList<int>();
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num == 0)
					{
						num = string.Compare(inv[x].Name, inv[y].Name, StringComparison.OrdinalIgnoreCase);
					}
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});

			// Token: 0x0400685E RID: 26718
			public static ItemSorting.ItemSortingLayer LastTrash = new ItemSorting.ItemSortingLayer("Last - Trash", delegate(ItemSorting.ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> list = new List<int>(itemsToSort);
				layer.Validate(ref list, inv);
				foreach (int item in list)
				{
					itemsToSort.Remove(item);
				}
				ItemSorting.ItemSortingLayers.SortIndicesStable(list, delegate(int x, int y)
				{
					int num = inv[y].value.CompareTo(inv[x].value);
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					return num;
				});
				return list;
			});
		}

		// Token: 0x02000702 RID: 1794
		private struct DamageTypeSortingLayerEntry
		{
			// Token: 0x06003FCC RID: 16332 RVA: 0x0069B1CD File Offset: 0x006993CD
			public DamageTypeSortingLayerEntry(float multiplier, ItemSorting.ItemSortingLayer layer, int index)
			{
				this.Multiplier = multiplier;
				this.Layer = layer;
				this.Index = index;
			}

			// Token: 0x0400685F RID: 26719
			public float Multiplier;

			// Token: 0x04006860 RID: 26720
			public ItemSorting.ItemSortingLayer Layer;

			// Token: 0x04006861 RID: 26721
			public int Index;
		}

		// Token: 0x02000703 RID: 1795
		private struct MemoryStamp
		{
			// Token: 0x06003FCD RID: 16333 RVA: 0x0069B1E4 File Offset: 0x006993E4
			public MemoryStamp(int itemType, int stack, int prefix)
			{
				this.ItemType = itemType;
				this.Stack = stack;
				this.Prefix = prefix;
			}

			// Token: 0x06003FCE RID: 16334 RVA: 0x0069B1FB File Offset: 0x006993FB
			public MemoryStamp(Item item)
			{
				this.ItemType = item.type;
				this.Stack = item.stack;
				this.Prefix = (int)item.prefix;
			}

			// Token: 0x06003FCF RID: 16335 RVA: 0x0069B221 File Offset: 0x00699421
			public override bool Equals(object obj)
			{
				return obj != null && obj is ItemSorting.MemoryStamp && this.Equals((ItemSorting.MemoryStamp)obj);
			}

			// Token: 0x06003FD0 RID: 16336 RVA: 0x0069B23C File Offset: 0x0069943C
			public bool Equals(ItemSorting.MemoryStamp other)
			{
				return this.ItemType == other.ItemType && this.Stack == other.Stack && this.Prefix == other.Prefix;
			}

			// Token: 0x06003FD1 RID: 16337 RVA: 0x0069B26A File Offset: 0x0069946A
			public override int GetHashCode()
			{
				return (this.ItemType * 397 ^ this.Stack) * 397 ^ this.Prefix;
			}

			// Token: 0x06003FD2 RID: 16338 RVA: 0x0069B28C File Offset: 0x0069948C
			public static bool operator ==(ItemSorting.MemoryStamp left, ItemSorting.MemoryStamp right)
			{
				return left.Equals(right);
			}

			// Token: 0x06003FD3 RID: 16339 RVA: 0x0069B296 File Offset: 0x00699496
			public static bool operator !=(ItemSorting.MemoryStamp left, ItemSorting.MemoryStamp right)
			{
				return !left.Equals(right);
			}

			// Token: 0x04006862 RID: 26722
			public int ItemType;

			// Token: 0x04006863 RID: 26723
			public int Stack;

			// Token: 0x04006864 RID: 26724
			public int Prefix;
		}
	}
}
