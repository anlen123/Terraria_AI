using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent
{
	// Token: 0x02000242 RID: 578
	public class QuickStacking
	{
		// Token: 0x0600229C RID: 8860 RVA: 0x00538B84 File Offset: 0x00536D84
		private static void AddToListyArray<T>(ref T[] arr, ref int count, T elem)
		{
			if (count == arr.Length)
			{
				Array.Resize<T>(ref arr, arr.Length * 2);
			}
			T[] array = arr;
			int num = count;
			count = num + 1;
			array[num] = elem;
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x00538BB6 File Offset: 0x00536DB6
		private static int GetCategory(int type)
		{
			return ItemSorting.GetSortingLayerIndex(type);
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x00538BBE File Offset: 0x00536DBE
		public static void QuickStackToNearbyInventories(Player player, bool smartStack = false)
		{
			QuickStacking.QuickStackToNearbyBanks(player);
			QuickStacking.QuickStackToNearbyChests(player, smartStack);
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x00538BD0 File Offset: 0x00536DD0
		private static void QuickStackToNearbyBanks(Player player)
		{
			List<PositionedChest> banksInRangeOf = NearbyChests.GetBanksInRangeOf(player, 0f);
			foreach (PositionedChest positionedChest in banksInRangeOf)
			{
				long coinsMoved = ChestUI.MoveCoins(player.inventory, positionedChest.chest);
				Chest.VisualizeChestTransfer_CoinsBatch(player.Center, positionedChest.position, coinsMoved, Chest.ItemTransferVisualizationSettings.PlayerToChest);
			}
			QuickStacking.SourceInventory sourceInventory = QuickStacking.PackQuickStackableItems(player, false);
			List<int> list;
			QuickStacking.Transfer(sourceInventory, banksInRangeOf, out list, false);
			QuickStacking.RestoreToPlayer(player, sourceInventory);
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x00538C6C File Offset: 0x00536E6C
		public static void QuickStackToNearbyChests(Player player, bool smartStack = false)
		{
			QuickStacking.QuickStackToNearbyChests(player, QuickStacking.PackQuickStackableItems(player, true), smartStack);
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x00538C7C File Offset: 0x00536E7C
		internal static void QuickStackToNearbyChests(Player player, QuickStacking.SourceInventory inventory, bool smartStack)
		{
			if (Main.netMode == 1)
			{
				QuickStacking.SendQuickStackToNearbyChests(player, inventory, smartStack);
				return;
			}
			List<PositionedChest> chestsInRangeOf = NearbyChests.GetChestsInRangeOf(player.position, 0f);
			List<int> chests;
			QuickStacking.Transfer(inventory, chestsInRangeOf, out chests, smartStack);
			QuickStacking.IndicateBlockedChests(player, chests);
			QuickStacking.RestoreToPlayer(player, inventory);
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x00538CC4 File Offset: 0x00536EC4
		internal static void IndicateBlockedChests(Player player, List<int> chests)
		{
			if (!chests.Any<int>())
			{
				return;
			}
			if (Main.netMode == 2)
			{
				NetMessage.SendData(85, player.whoAmI, -1, null, player.whoAmI, 0f, 0f, 0f, 0, 0, 0);
				return;
			}
			foreach (int chestIndex in chests)
			{
				Chest.IndicateBlockedChest(chestIndex);
			}
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x00538D48 File Offset: 0x00536F48
		private static void SendQuickStackToNearbyChests(Player player, QuickStacking.SourceInventory inventory, bool smartStack)
		{
			QuickStacking.netInv = inventory;
			for (int i = 0; i < inventory.numItems; i++)
			{
				int slotId = inventory.slots[i].SlotId;
				player.LockNetSlot(slotId);
				NetMessage.SendData(5, -1, -1, null, player.whoAmI, (float)slotId, 0f, 0f, 0, 0, 0);
			}
			NetMessage.SendData(85, -1, -1, null, smartStack ? 1 : 0, 0f, 0f, 0f, 0, 0, 0);
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x00538DC8 File Offset: 0x00536FC8
		internal static void WriteNetInventorySlots(BinaryWriter writer)
		{
			writer.Write(QuickStacking.netInv.numItems);
			for (int i = 0; i < QuickStacking.netInv.numItems; i++)
			{
				writer.Write((short)QuickStacking.netInv.slots[i].SlotId);
			}
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x00538E18 File Offset: 0x00537018
		internal static QuickStacking.SourceInventory ReadNetInventory(Player player, BinaryReader reader)
		{
			QuickStacking.SourceInventory scratchInventory = QuickStacking.GetScratchInventory(player);
			Array.Clear(scratchInventory.transferBlocked, 0, scratchInventory.transferBlocked.Length);
			scratchInventory.numItems = reader.ReadInt32();
			for (int i = 0; i < scratchInventory.numItems; i++)
			{
				PlayerItemSlotID.SlotReference slotReference = new PlayerItemSlotID.SlotReference(player, (int)reader.ReadInt16());
				scratchInventory.slots[i] = slotReference;
				Item item = slotReference.Item;
				scratchInventory.items[i] = item;
			}
			return scratchInventory;
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x00538E8C File Offset: 0x0053708C
		internal static void WriteBlockedChestList(BinaryWriter writer)
		{
			writer.Write(QuickStacking._blockedChests.Count);
			for (int i = 0; i < QuickStacking._blockedChests.Count; i++)
			{
				writer.Write((ushort)QuickStacking._blockedChests[i]);
			}
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x00538ED0 File Offset: 0x005370D0
		internal static List<int> ReadBlockedChestList(BinaryReader reader)
		{
			QuickStacking._blockedChests.Clear();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				QuickStacking._blockedChests.Add((int)reader.ReadUInt16());
			}
			return QuickStacking._blockedChests;
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x00538F10 File Offset: 0x00537110
		private static void RestoreToPlayer(Player player, QuickStacking.SourceInventory inventory)
		{
			for (int i = 0; i < inventory.numItems; i++)
			{
				Item item = inventory.items[i];
				PlayerItemSlotID.SlotReference slotReference = inventory.slots[i];
				bool flag = inventory.transferBlocked[i];
				if (!false)
				{
					slotReference.Item = item;
				}
				if (Main.netMode == 2)
				{
					NetMessage.SendData(5, -1, -1, null, player.whoAmI, (float)slotReference.SlotId, (float)(flag ? 1 : 0), 0f, 0, 0, 0);
				}
				else if (flag)
				{
					ItemSlot.IndicateBlockedSlot(slotReference);
				}
			}
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x00538F94 File Offset: 0x00537194
		private static QuickStacking.SourceInventory GetScratchInventory(Player player)
		{
			return new QuickStacking.SourceInventory
			{
				items = QuickStacking.inventoryItemsScratch,
				numItems = 0,
				slots = QuickStacking.slotsScratch,
				transferBlocked = QuickStacking.blockedSlotsScratch,
				position = player.Center
			};
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x00538FE4 File Offset: 0x005371E4
		private static QuickStacking.SourceInventory PackQuickStackableItems(Player player, bool includeVoidBag)
		{
			QuickStacking.SourceInventory scratchInventory = QuickStacking.GetScratchInventory(player);
			Array.Clear(scratchInventory.transferBlocked, 0, scratchInventory.transferBlocked.Length);
			QuickStacking.AddQuickStackableItems(player, ref scratchInventory, PlayerItemSlotID.Inventory0 + 10, 40);
			if (player.useVoidBag() && includeVoidBag)
			{
				QuickStacking.AddQuickStackableItems(player, ref scratchInventory, PlayerItemSlotID.Bank4_0, player.bank4.maxItems);
			}
			return scratchInventory;
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x00539044 File Offset: 0x00537244
		private static void AddQuickStackableItems(Player player, ref QuickStacking.SourceInventory inventory, int startSlot, int count)
		{
			for (int i = 0; i < count; i++)
			{
				PlayerItemSlotID.SlotReference slotReference = new PlayerItemSlotID.SlotReference(player, startSlot + i);
				Item item = slotReference.Item;
				if (!item.IsAir && !item.favorited && !item.IsACoin)
				{
					int numItems = inventory.numItems;
					inventory.numItems = numItems + 1;
					int num = numItems;
					inventory.slots[num] = slotReference;
					inventory.items[num] = item;
				}
			}
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x005390B0 File Offset: 0x005372B0
		private static void Transfer(QuickStacking.SourceInventory source, List<PositionedChest> destinations, out List<int> blockedChests, bool smartStack = false)
		{
			QuickStacking.nextDestHelper = 0;
			List<QuickStacking.DestinationHelper> list = QuickStacking.destHelperListScratch;
			list.Clear();
			QuickStacking.MatchingItemTypeDestinationList matchingItemTypeDestinationList = QuickStacking.matchingItemTypeScratch;
			matchingItemTypeDestinationList.Reset();
			foreach (PositionedChest positionedChest in destinations)
			{
				if (!positionedChest.chest.IsEmpty())
				{
					QuickStacking.DestinationHelper destHelperFromPool = QuickStacking.GetDestHelperFromPool();
					destHelperFromPool.Reset(positionedChest);
					list.Add(destHelperFromPool);
					QuickStacking.BuildDestinationMetricsAndStackItems(source, destHelperFromPool, matchingItemTypeDestinationList);
				}
			}
			for (int i = 0; i < source.numItems; i++)
			{
				Item item = source.items[i];
				QuickStacking.DestinationHelper dest;
				if (!item.IsAir && matchingItemTypeDestinationList.Lookup(item.type, out dest))
				{
					QuickStacking.Consolidate(source, i);
					QuickStacking.InsertIntoFreeSlot(ref source.items[i], dest, source.position);
				}
			}
			if (smartStack)
			{
				for (int j = 0; j < source.numItems; j++)
				{
					Item item2 = source.items[j];
					QuickStacking.DestinationHelper destinationHelper;
					if (!item2.IsAir && !source.transferBlocked[j] && QuickStacking.TryGetBestDestinationForCategory(QuickStacking.GetCategory(item2.type), list, out destinationHelper))
					{
						if (destinationHelper.locked)
						{
							source.transferBlocked[j] = true;
							destinationHelper.transferBlocked = true;
						}
						else
						{
							QuickStacking.Consolidate(source, j);
							QuickStacking.InsertIntoFreeSlot(ref source.items[j], destinationHelper, source.position);
						}
					}
				}
			}
			blockedChests = QuickStacking._blockedChests;
			blockedChests.Clear();
			foreach (QuickStacking.DestinationHelper destinationHelper2 in list)
			{
				if (destinationHelper2.transferBlocked)
				{
					blockedChests.Add(destinationHelper2.ChestIndex);
				}
			}
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x00539290 File Offset: 0x00537490
		private static QuickStacking.DestinationHelper GetDestHelperFromPool()
		{
			if (QuickStacking.nextDestHelper == QuickStacking.destHelperPool.Length)
			{
				Array.Resize<QuickStacking.DestinationHelper>(ref QuickStacking.destHelperPool, QuickStacking.destHelperPool.Length * 2);
			}
			if (QuickStacking.destHelperPool[QuickStacking.nextDestHelper] == null)
			{
				QuickStacking.destHelperPool[QuickStacking.nextDestHelper] = new QuickStacking.DestinationHelper();
			}
			return QuickStacking.destHelperPool[QuickStacking.nextDestHelper++];
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x005392F0 File Offset: 0x005374F0
		private static void BuildDestinationMetricsAndStackItems(QuickStacking.SourceInventory source, QuickStacking.DestinationHelper dest, QuickStacking.MatchingItemTypeDestinationList destinationsForItemTypes)
		{
			for (int i = 0; i < dest.itemCount; i++)
			{
				Item item = dest.items[i];
				if (item.IsAir)
				{
					dest.AddFreeSlot(i);
				}
				else
				{
					dest.AddCategoryScore(QuickStacking.GetCategory(item.type));
					for (int j = 0; j < source.numItems; j++)
					{
						Item item2 = source.items[j];
						if (item2.type == item.type && Item.CanStack(item2, item))
						{
							int num = Math.Min(item.maxStack - item.stack, item2.stack);
							if (num != 0)
							{
								if (dest.locked)
								{
									source.transferBlocked[j] = true;
									dest.transferBlocked = true;
								}
								else
								{
									QuickStacking.FillStack(item2, num, dest, i, source.position);
									if (!item2.IsAir)
									{
										destinationsForItemTypes.Add(item.type, dest);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x005393D8 File Offset: 0x005375D8
		private static bool TryGetBestDestinationForCategory(int category, List<QuickStacking.DestinationHelper> destinations, out QuickStacking.DestinationHelper dest)
		{
			dest = null;
			int num = int.MinValue;
			foreach (QuickStacking.DestinationHelper destinationHelper in destinations)
			{
				int num2;
				if (destinationHelper.HasFreeSlots && destinationHelper.TryGetCategoryScore(category, out num2) && num2 > num)
				{
					dest = destinationHelper;
					num = num2;
				}
			}
			return dest != null;
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x0053944C File Offset: 0x0053764C
		private static void FillStack(Item item, int numToTransfer, QuickStacking.DestinationHelper dest, int slotIndex, Vector2 srcPosition)
		{
			Item item2 = dest.items[slotIndex];
			Chest.VisualizeChestTransfer(srcPosition, dest.position, item.type, Chest.ItemTransferVisualizationSettings.PlayerToChest);
			item2.stack += numToTransfer;
			item.stack -= numToTransfer;
			if (item.stack == 0)
			{
				item.TurnToAir(false);
			}
			dest.SyncSlot(slotIndex);
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x005394AC File Offset: 0x005376AC
		private static void Consolidate(QuickStacking.SourceInventory source, int i)
		{
			Item item = source.items[i++];
			while (i < source.numItems)
			{
				Item item2 = source.items[i++];
				if (Item.CanStack(item, item2))
				{
					int num = Math.Min(item.maxStack - item.stack, item2.stack);
					item.stack += num;
					item2.stack -= num;
					if (item2.stack == 0)
					{
						item2.TurnToAir(false);
					}
				}
			}
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x00539530 File Offset: 0x00537730
		private static void InsertIntoFreeSlot(ref Item item, QuickStacking.DestinationHelper dest, Vector2 srcPosition)
		{
			Chest.VisualizeChestTransfer(srcPosition, dest.position, item.type, Chest.ItemTransferVisualizationSettings.PlayerToChest);
			int num = dest.ConsumeFreeSlot();
			Utils.Swap<Item>(ref item, ref dest.items[num]);
			dest.SyncSlot(num);
		}

		// Token: 0x04004CF1 RID: 19697
		private static QuickStacking.SourceInventory netInv;

		// Token: 0x04004CF2 RID: 19698
		private static Item[] inventoryItemsScratch = new Item[400];

		// Token: 0x04004CF3 RID: 19699
		private static PlayerItemSlotID.SlotReference[] slotsScratch = new PlayerItemSlotID.SlotReference[400];

		// Token: 0x04004CF4 RID: 19700
		private static bool[] blockedSlotsScratch = new bool[400];

		// Token: 0x04004CF5 RID: 19701
		private static List<QuickStacking.DestinationHelper> destHelperListScratch = new List<QuickStacking.DestinationHelper>();

		// Token: 0x04004CF6 RID: 19702
		private static QuickStacking.MatchingItemTypeDestinationList matchingItemTypeScratch = new QuickStacking.MatchingItemTypeDestinationList();

		// Token: 0x04004CF7 RID: 19703
		private static List<int> _blockedChests = new List<int>();

		// Token: 0x04004CF8 RID: 19704
		private static QuickStacking.DestinationHelper[] destHelperPool = new QuickStacking.DestinationHelper[100];

		// Token: 0x04004CF9 RID: 19705
		private static int nextDestHelper = 0;

		// Token: 0x020007CA RID: 1994
		private class DestinationHelper
		{
			// Token: 0x17000534 RID: 1332
			// (get) Token: 0x06004211 RID: 16913 RVA: 0x006BC616 File Offset: 0x006BA816
			public Vector2 position
			{
				get
				{
					return this._chest.position;
				}
			}

			// Token: 0x06004212 RID: 16914 RVA: 0x006BC624 File Offset: 0x006BA824
			public void Reset(PositionedChest inventory)
			{
				this._chest = inventory;
				this.items = inventory.chest.item;
				this.itemCount = inventory.chest.maxItems;
				this.locked = inventory.chest.IsLockedOrInUse();
				this.transferBlocked = false;
				if (this.freeSlots.Length < this.itemCount)
				{
					Array.Resize<int>(ref this.freeSlots, this.itemCount);
				}
				Array.Clear(this.freeSlots, 0, this.freeSlots.Length);
				this.freeSlotStart = 0;
				this.freeSlotCount = 0;
				Array.Clear(this.categoryScores, 0, this.categoryScores.Length);
			}

			// Token: 0x17000535 RID: 1333
			// (get) Token: 0x06004213 RID: 16915 RVA: 0x006BC6C9 File Offset: 0x006BA8C9
			public int ChestIndex
			{
				get
				{
					return this._chest.chest.index;
				}
			}

			// Token: 0x17000536 RID: 1334
			// (get) Token: 0x06004214 RID: 16916 RVA: 0x006BC6DB File Offset: 0x006BA8DB
			public bool IsEmpty
			{
				get
				{
					return this.freeSlotCount == this.itemCount;
				}
			}

			// Token: 0x17000537 RID: 1335
			// (get) Token: 0x06004215 RID: 16917 RVA: 0x006BC6EB File Offset: 0x006BA8EB
			public bool HasFreeSlots
			{
				get
				{
					return this.freeSlotStart < this.freeSlotCount;
				}
			}

			// Token: 0x06004216 RID: 16918 RVA: 0x006BC6FB File Offset: 0x006BA8FB
			public void AddCategoryScore(int category)
			{
				this.categoryScores[category]++;
			}

			// Token: 0x06004217 RID: 16919 RVA: 0x006BC70E File Offset: 0x006BA90E
			public void AddFreeSlot(int i)
			{
				QuickStacking.AddToListyArray<int>(ref this.freeSlots, ref this.freeSlotCount, i);
			}

			// Token: 0x06004218 RID: 16920 RVA: 0x006BC722 File Offset: 0x006BA922
			public bool TryGetCategoryScore(int category, out int score)
			{
				score = this.categoryScores[category];
				return score != 0;
			}

			// Token: 0x06004219 RID: 16921 RVA: 0x006BC738 File Offset: 0x006BA938
			public int ConsumeFreeSlot()
			{
				int[] array = this.freeSlots;
				int num = this.freeSlotStart;
				this.freeSlotStart = num + 1;
				return array[num];
			}

			// Token: 0x0600421A RID: 16922 RVA: 0x006BC760 File Offset: 0x006BA960
			public void SyncSlot(int slot)
			{
				if (this._chest.chest.index >= 0)
				{
					NetMessage.SendData(32, -1, -1, null, this._chest.chest.index, (float)slot, 0f, 0f, 0, 0, 0);
				}
			}

			// Token: 0x040070B6 RID: 28854
			private PositionedChest _chest;

			// Token: 0x040070B7 RID: 28855
			public Item[] items;

			// Token: 0x040070B8 RID: 28856
			public int itemCount;

			// Token: 0x040070B9 RID: 28857
			public bool locked;

			// Token: 0x040070BA RID: 28858
			public bool transferBlocked;

			// Token: 0x040070BB RID: 28859
			private int[] freeSlots = new int[200];

			// Token: 0x040070BC RID: 28860
			private int freeSlotStart;

			// Token: 0x040070BD RID: 28861
			private int freeSlotCount;

			// Token: 0x040070BE RID: 28862
			private int[] categoryScores = new int[ItemSorting.LayerCount];
		}

		// Token: 0x020007CB RID: 1995
		private class MatchingItemTypeDestinationList
		{
			// Token: 0x0600421C RID: 16924 RVA: 0x006BC7D1 File Offset: 0x006BA9D1
			public MatchingItemTypeDestinationList()
			{
				this.Reset();
			}

			// Token: 0x0600421D RID: 16925 RVA: 0x006BC7FF File Offset: 0x006BA9FF
			public void Reset()
			{
				Array.Clear(this.firstEntryForType, 0, this.firstEntryForType.Length);
				Array.Clear(this.entries, 0, this.entries.Length);
				this.numEntries = 1;
			}

			// Token: 0x0600421E RID: 16926 RVA: 0x006BC830 File Offset: 0x006BAA30
			private int Tail(int type)
			{
				int num = this.firstEntryForType[type];
				if (num == 0)
				{
					return 0;
				}
				while (this.entries[num].next != 0)
				{
					num = this.entries[num].next;
				}
				return num;
			}

			// Token: 0x0600421F RID: 16927 RVA: 0x006BC874 File Offset: 0x006BAA74
			internal void Add(int type, QuickStacking.DestinationHelper value)
			{
				int num = this.Tail(type);
				if (num == 0)
				{
					this.firstEntryForType[type] = this.AddEntry(value);
					return;
				}
				if (this.entries[num].value == value)
				{
					return;
				}
				this.entries[num].next = this.AddEntry(value);
			}

			// Token: 0x06004220 RID: 16928 RVA: 0x006BC8CC File Offset: 0x006BAACC
			private int AddEntry(QuickStacking.DestinationHelper value)
			{
				if (this.numEntries == this.entries.Length)
				{
					Array.Resize<QuickStacking.MatchingItemTypeDestinationList.LinkedEntry>(ref this.entries, this.entries.Length * 2);
				}
				int result = this.numEntries;
				QuickStacking.AddToListyArray<QuickStacking.MatchingItemTypeDestinationList.LinkedEntry>(ref this.entries, ref this.numEntries, new QuickStacking.MatchingItemTypeDestinationList.LinkedEntry
				{
					value = value
				});
				return result;
			}

			// Token: 0x06004221 RID: 16929 RVA: 0x006BC928 File Offset: 0x006BAB28
			public bool Lookup(int type, out QuickStacking.DestinationHelper value)
			{
				value = null;
				int i = this.firstEntryForType[type];
				while (i > 0)
				{
					QuickStacking.MatchingItemTypeDestinationList.LinkedEntry linkedEntry = this.entries[i];
					value = linkedEntry.value;
					if (value.HasFreeSlots)
					{
						return true;
					}
					i = linkedEntry.next;
					this.firstEntryForType[type] = i;
				}
				return false;
			}

			// Token: 0x040070BF RID: 28863
			private QuickStacking.MatchingItemTypeDestinationList.LinkedEntry[] entries = new QuickStacking.MatchingItemTypeDestinationList.LinkedEntry[1000];

			// Token: 0x040070C0 RID: 28864
			private int numEntries;

			// Token: 0x040070C1 RID: 28865
			private int[] firstEntryForType = new int[(int)ItemID.Count];

			// Token: 0x02000AC8 RID: 2760
			private struct LinkedEntry
			{
				// Token: 0x0400783D RID: 30781
				public QuickStacking.DestinationHelper value;

				// Token: 0x0400783E RID: 30782
				public int next;
			}
		}

		// Token: 0x020007CC RID: 1996
		internal struct SourceInventory
		{
			// Token: 0x040070C2 RID: 28866
			public Item[] items;

			// Token: 0x040070C3 RID: 28867
			public int numItems;

			// Token: 0x040070C4 RID: 28868
			public PlayerItemSlotID.SlotReference[] slots;

			// Token: 0x040070C5 RID: 28869
			public bool[] transferBlocked;

			// Token: 0x040070C6 RID: 28870
			public Vector2 position;
		}
	}
}
