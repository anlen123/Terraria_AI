using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.DataStructures;
using Terraria.Net;

namespace Terraria.GameContent
{
	// Token: 0x02000230 RID: 560
	public static class CraftingRequests
	{
		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060021FA RID: 8698 RVA: 0x00532BA0 File Offset: 0x00530DA0
		public static bool HasPendingRequests
		{
			get
			{
				return CraftingRequests._pendingCrafts.Count > 0;
			}
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x00532BAF File Offset: 0x00530DAF
		public static void Clear()
		{
			CraftingRequests._pendingCrafts.Clear();
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x00532BBC File Offset: 0x00530DBC
		public static void CraftItem(Recipe recipe, int qty = 1, bool quickCraft = false)
		{
			Player localPlayer = Main.LocalPlayer;
			List<Chest> chests = Recipe._recipeChests;
			List<Recipe.RequiredItemEntry> list = new List<Recipe.RequiredItemEntry>();
			int num = 0;
			Func<Recipe.RequiredItemEntry, bool> <>9__0;
			while (num < qty && (num <= 0 || (Recipe.CollectedEnoughItemsToCraft(recipe) && Main.CursorHasSpaceToCraftRecipe(recipe))))
			{
				list.Clear();
				recipe.GetIngredientsForOneCraft(localPlayer, list);
				if (Main.netMode == 0)
				{
					goto IL_7C;
				}
				IEnumerable<Recipe.RequiredItemEntry> source = list;
				Func<Recipe.RequiredItemEntry, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((Recipe.RequiredItemEntry req) => CraftingRequests.CanCraftLocally(req, chests)));
				}
				if (source.All(predicate))
				{
					goto IL_7C;
				}
				CraftingRequests.CraftViaRequest(recipe, quickCraft, chests, list);
				IL_9A:
				foreach (Recipe.RequiredItemEntry req2 in list)
				{
					Recipe.SubtractOwnedItem(req2);
				}
				num++;
				continue;
				IL_7C:
				CraftingRequests.CraftLocally(recipe, quickCraft, chests, list);
				goto IL_9A;
			}
			CraftingEffects.OnCraft(recipe, quickCraft);
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x00532CB4 File Offset: 0x00530EB4
		private static Item CreateResult(Recipe recipe)
		{
			Item item = recipe.createItem.Clone();
			item.OnCreated(new RecipeItemCreationContext(recipe));
			if (item.stack <= 1)
			{
				item.Prefix(-1);
			}
			return item;
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x00532CEC File Offset: 0x00530EEC
		private static void CraftLocally(Recipe recipe, bool quickCraft, List<Chest> chests, List<Recipe.RequiredItemEntry> ingredients)
		{
			foreach (Recipe.RequiredItemEntry req in ingredients)
			{
				CraftingRequests.Consume(req, chests, null, true);
			}
			Main.CraftItem_GrantItem(recipe, CraftingRequests.CreateResult(recipe), quickCraft);
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x00532D48 File Offset: 0x00530F48
		private static void CraftViaRequest(Recipe recipe, bool quickCraft, List<Chest> chests, List<Recipe.RequiredItemEntry> ingredients)
		{
			List<Item> list = new List<Item>();
			List<Recipe.RequiredItemEntry> list2 = new List<Recipe.RequiredItemEntry>();
			foreach (Recipe.RequiredItemEntry requiredItemEntry in ingredients)
			{
				int num = CraftingRequests.Consume(requiredItemEntry, chests, list, false);
				if (num > 0)
				{
					list2.Add(new Recipe.RequiredItemEntry
					{
						itemIdOrRecipeGroup = requiredItemEntry.itemIdOrRecipeGroup,
						stack = num
					});
				}
			}
			Item item = CraftingRequests.CreateResult(recipe);
			if (!quickCraft)
			{
				FakeCursorItem.Add(item);
			}
			CraftingRequests._pendingCrafts.Enqueue(new CraftingRequests.RemoteCraftRequest
			{
				recipe = recipe,
				result = item,
				consumed = list,
				requested = list2,
				quickCraft = quickCraft
			});
			NetManager.Instance.SendToServer(CraftingRequests.NetCraftingRequestsModule.WriteRequest(list2, chests));
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x00532E34 File Offset: 0x00531034
		private static bool IsLocallyAccessible(Chest chest)
		{
			return chest.bankChest || chest.index == Main.LocalPlayer.chest;
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x00532E54 File Offset: 0x00531054
		private static bool CanCraftLocally(Recipe.RequiredItemEntry req, List<Chest> chests)
		{
			int num = 0;
			num += CraftingRequests.CountMatches(req, Main.LocalPlayer.inventory, 58);
			foreach (Chest chest in chests)
			{
				if (CraftingRequests.IsLocallyAccessible(chest))
				{
					num += CraftingRequests.CountMatches(req, chest.item, chest.maxItems);
				}
			}
			return num >= req.stack;
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x00532EDC File Offset: 0x005310DC
		private static int CountMatches(Recipe.RequiredItemEntry req, List<Chest> chests)
		{
			int num = 0;
			foreach (Chest chest in chests)
			{
				num += CraftingRequests.CountMatches(req, chest.item, chest.maxItems);
			}
			return num;
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x00532F3C File Offset: 0x0053113C
		private static int CountMatches(Recipe.RequiredItemEntry req, Item[] inv, int maxItems)
		{
			int num = 0;
			for (int i = 0; i < maxItems; i++)
			{
				Item item = inv[i];
				if (req.Matches(item.type))
				{
					num += item.stack;
				}
			}
			return num;
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x00532F74 File Offset: 0x00531174
		private static int Consume(Recipe.RequiredItemEntry req, List<Chest> chests, List<Item> consumedItems, bool fromChests)
		{
			int stack = req.stack;
			if (Main.netMode != 2)
			{
				CraftingRequests.ConsumeItemsFrom(Main.LocalPlayer.inventory, 58, req, ref stack, consumedItems, -1);
			}
			foreach (Chest chest in chests)
			{
				if (chest.bankChest || fromChests)
				{
					CraftingRequests.ConsumeItemsFrom(chest, req, ref stack, consumedItems);
				}
			}
			return stack;
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x00532FF8 File Offset: 0x005311F8
		private static void ConsumeItemsFrom(Chest chest, Recipe.RequiredItemEntry req, ref int toConsume, List<Item> consumedItems = null)
		{
			CraftingRequests.ConsumeItemsFrom(chest.item, chest.maxItems, req, ref toConsume, consumedItems, chest.bankChest ? -1 : chest.index);
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x00533020 File Offset: 0x00531220
		private static void ConsumeItemsFrom(Item[] inventory, int maxItems, Recipe.RequiredItemEntry req, ref int toConsume, List<Item> consumedItems = null, int chestIndex = -1)
		{
			if (toConsume <= 0)
			{
				return;
			}
			int netMode = Main.netMode;
			int netMode2 = Main.netMode;
			for (int i = 0; i < maxItems; i++)
			{
				Item item = inventory[i];
				if (req.Matches(item.type))
				{
					if (item.stack > toConsume)
					{
						if (consumedItems != null)
						{
							Item item2 = item.Clone();
							item2.stack = toConsume;
							consumedItems.Add(item2);
						}
						item.stack -= toConsume;
						toConsume = 0;
					}
					else
					{
						toConsume -= item.stack;
						if (consumedItems != null)
						{
							consumedItems.Add(item);
						}
						inventory[i] = new Item();
					}
					if (chestIndex >= 0)
					{
						NetMessage.SendData(32, -1, -1, null, chestIndex, (float)i, 0f, 0f, 0, 0, 0);
					}
					if (toConsume <= 0)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x005330E8 File Offset: 0x005312E8
		public static bool CanCraftFromChest(Chest chest, int whoAmI)
		{
			if (Chest.IsLocked(chest.x, chest.y))
			{
				return false;
			}
			int num = Chest.UsingChest(chest.index);
			return num < 0 || num == whoAmI;
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x00533124 File Offset: 0x00531324
		private static void HandleRequest(int whoAmI, List<Recipe.RequiredItemEntry> items, List<Chest> chests)
		{
			chests.RemoveAll((Chest chest) => chest == null || !CraftingRequests.CanCraftFromChest(chest, whoAmI));
			if (!items.All((Recipe.RequiredItemEntry req) => CraftingRequests.CountMatches(req, chests) >= req.stack))
			{
				NetManager.Instance.SendToClient(CraftingRequests.NetCraftingRequestsModule.WriteResponse(false), whoAmI);
				return;
			}
			foreach (Recipe.RequiredItemEntry req2 in items)
			{
				CraftingRequests.Consume(req2, chests, null, true);
			}
			NetManager.Instance.SendToClient(CraftingRequests.NetCraftingRequestsModule.WriteResponse(true), whoAmI);
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x005331E8 File Offset: 0x005313E8
		private static void HandleResponse(bool approved)
		{
			CraftingRequests.RemoteCraftRequest remoteCraftRequest = CraftingRequests._pendingCrafts.Dequeue();
			FakeCursorItem.Remove(remoteCraftRequest.result.type, remoteCraftRequest.result.stack);
			if (approved)
			{
				Main.CraftItem_GrantItem(remoteCraftRequest.recipe, remoteCraftRequest.result, remoteCraftRequest.quickCraft);
				return;
			}
			foreach (Item item in remoteCraftRequest.consumed)
			{
				CraftingRequests.Refund(item);
			}
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x0053327C File Offset: 0x0053147C
		public static void Refund(Item item)
		{
			Main.LocalPlayer.GetOrDropItem(item, GetItemSettings.RefundConsumedItem);
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x00533290 File Offset: 0x00531490
		public static void SubtractPendingRequests()
		{
			foreach (CraftingRequests.RemoteCraftRequest remoteCraftRequest in CraftingRequests._pendingCrafts)
			{
				foreach (Recipe.RequiredItemEntry req in remoteCraftRequest.requested)
				{
					Recipe.SubtractOwnedItem(req);
				}
			}
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x00533318 File Offset: 0x00531518
		public static void SavePossibleRefunds(BinaryWriter writer)
		{
			int value = CraftingRequests._pendingCrafts.Sum((CraftingRequests.RemoteCraftRequest c) => c.consumed.Count);
			writer.Write(value);
			foreach (CraftingRequests.RemoteCraftRequest remoteCraftRequest in CraftingRequests._pendingCrafts)
			{
				foreach (Item item in remoteCraftRequest.consumed)
				{
					item.Serialize(writer, ItemSerializationContext.SavingAndLoading);
				}
			}
		}

		// Token: 0x04004CA5 RID: 19621
		private static Queue<CraftingRequests.RemoteCraftRequest> _pendingCrafts = new Queue<CraftingRequests.RemoteCraftRequest>();

		// Token: 0x020007B1 RID: 1969
		public struct RemoteCraftRequest
		{
			// Token: 0x04007079 RID: 28793
			public Recipe recipe;

			// Token: 0x0400707A RID: 28794
			public Item result;

			// Token: 0x0400707B RID: 28795
			public List<Item> consumed;

			// Token: 0x0400707C RID: 28796
			public List<Recipe.RequiredItemEntry> requested;

			// Token: 0x0400707D RID: 28797
			public bool quickCraft;
		}

		// Token: 0x020007B2 RID: 1970
		public class NetCraftingRequestsModule : NetModule
		{
			// Token: 0x060041BE RID: 16830 RVA: 0x006BB3A4 File Offset: 0x006B95A4
			public static NetPacket WriteRequest(List<Recipe.RequiredItemEntry> items, List<Chest> chests)
			{
				NetPacket result = NetModule.CreatePacket<CraftingRequests.NetCraftingRequestsModule>(65530);
				result.Writer.Write7BitEncodedInt(items.Count);
				foreach (Recipe.RequiredItemEntry requiredItemEntry in items)
				{
					result.Writer.Write(requiredItemEntry.itemIdOrRecipeGroup);
					result.Writer.Write7BitEncodedInt(requiredItemEntry.stack);
				}
				result.Writer.Write7BitEncodedInt(chests.Count);
				foreach (Chest chest in chests)
				{
					result.Writer.Write7BitEncodedInt(chest.index);
				}
				return result;
			}

			// Token: 0x060041BF RID: 16831 RVA: 0x006BB48C File Offset: 0x006B968C
			public static NetPacket WriteResponse(bool approved)
			{
				NetPacket result = NetModule.CreatePacket<CraftingRequests.NetCraftingRequestsModule>(65530);
				result.Writer.Write(approved);
				return result;
			}

			// Token: 0x060041C0 RID: 16832 RVA: 0x006BB4B4 File Offset: 0x006B96B4
			public void DeserializeRequest(BinaryReader reader, int userId)
			{
				int num = reader.Read7BitEncodedInt();
				List<Recipe.RequiredItemEntry> list = new List<Recipe.RequiredItemEntry>(num);
				for (int i = 0; i < num; i++)
				{
					list.Add(new Recipe.RequiredItemEntry(reader.ReadInt32(), reader.Read7BitEncodedInt()));
				}
				int num2 = reader.Read7BitEncodedInt();
				List<Chest> list2 = new List<Chest>(num2);
				for (int j = 0; j < num2; j++)
				{
					int num3 = reader.Read7BitEncodedInt();
					list2.Add((num3 < 0) ? null : Main.chest[num3]);
				}
				CraftingRequests.HandleRequest(userId, list, list2);
			}

			// Token: 0x060041C1 RID: 16833 RVA: 0x006BB53A File Offset: 0x006B973A
			public void DeserializeResponse(BinaryReader reader)
			{
				CraftingRequests.HandleResponse(reader.ReadBoolean());
			}

			// Token: 0x060041C2 RID: 16834 RVA: 0x006BB547 File Offset: 0x006B9747
			public override bool Deserialize(BinaryReader reader, int userId)
			{
				if (Main.netMode == 2)
				{
					this.DeserializeRequest(reader, userId);
				}
				else
				{
					this.DeserializeResponse(reader);
				}
				return true;
			}
		}
	}
}
