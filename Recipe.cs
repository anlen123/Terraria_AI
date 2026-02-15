using System;
using System.Collections.Generic;
using System.Text;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Map;
using Terraria.UI;

namespace Terraria
{
	// Token: 0x0200003E RID: 62
	public class Recipe
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x0012C187 File Offset: 0x0012A387
		public bool DoesNotNeedTileOrLiquid
		{
			get
			{
				return this.requiredTile < 0 && !this.needHoney && !this.needWater && !this.needLava;
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0012C1B0 File Offset: 0x0012A3B0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.createItem.ToString());
			foreach (Item item in this.requiredItem)
			{
				if (item.IsAir)
				{
					break;
				}
				stringBuilder.AppendLine("  " + item.ToString());
			}
			foreach (int num in this.acceptedGroups)
			{
				if (num < 0)
				{
					break;
				}
				stringBuilder.AppendLine("  " + RecipeGroup.recipeGroups[num]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0012C254 File Offset: 0x0012A454
		public void RequireGroup(RecipeGroup group)
		{
			for (int i = 0; i < Recipe.maxRequirements; i++)
			{
				if (this.acceptedGroups[i] == -1)
				{
					this.acceptedGroups[i] = group.RegisteredId;
					return;
				}
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0012C28C File Offset: 0x0012A48C
		public bool ProcessGroupsForText(int type, out string theText)
		{
			for (int i = 0; i < Recipe.maxRequirements; i++)
			{
				int num = this.acceptedGroups[i];
				if (num == -1)
				{
					break;
				}
				if (RecipeGroup.recipeGroups[num].ValidItems.Contains(type))
				{
					theText = RecipeGroup.recipeGroups[num].GetText();
					return true;
				}
			}
			theText = "";
			return false;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0012C2F0 File Offset: 0x0012A4F0
		public Item AddCustomShimmerResult(int itemType, int itemStack = 1)
		{
			if (this.customShimmerResults == null)
			{
				this.customShimmerResults = new List<Item>();
			}
			Item item = new Item();
			item.SetDefaults(itemType, null);
			item.stack = itemStack;
			this.customShimmerResults.Add(item);
			return item;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0012C334 File Offset: 0x0012A534
		public Recipe()
		{
			for (int i = 0; i < Recipe.maxRequirements; i++)
			{
				this.requiredItem[i] = new Item();
				this.acceptedGroups[i] = -1;
			}
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0012C3B0 File Offset: 0x0012A5B0
		public void GetIngredientsForOneCraft(Player player, List<Recipe.RequiredItemEntry> ingredients)
		{
			for (int i = 0; i < Recipe.maxRequirements; i++)
			{
				Recipe.RequiredItemEntry requiredItemEntry = this.requiredItemQuickLookup[i];
				if (requiredItemEntry.itemIdOrRecipeGroup == 0)
				{
					break;
				}
				requiredItemEntry.stack -= this.GetIngredientCraftingDiscount(player, requiredItemEntry);
				if (requiredItemEntry.stack > 0)
				{
					ingredients.Add(requiredItemEntry);
				}
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0012C408 File Offset: 0x0012A608
		private int GetIngredientCraftingDiscount(Player player, Recipe.RequiredItemEntry req)
		{
			int num = 0;
			if (this.alchemy && player.alchemyTable)
			{
				for (int i = 0; i < req.stack; i++)
				{
					if (Main.rand.Next(3) == 0)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0012C44C File Offset: 0x0012A64C
		public static void UpdateRecipeList()
		{
			int focusRecipe = Main.focusRecipe;
			int oldRecipe = Main.availableRecipe[Main.focusRecipe];
			Recipe.ClearAvailableRecipes();
			if (!Main.guideItem.IsAir && Main.guideItem.Name != "")
			{
				Recipe.CollectGuideRecipes();
				Recipe.TryRefocusingRecipe(oldRecipe);
				Main.craftingUI.VisuallyRepositionRecipes(focusRecipe);
				return;
			}
			Player localPlayer = Main.LocalPlayer;
			Recipe.CollectItemsToCraftWithFrom(localPlayer);
			NewCraftingUI.RecipeFilter recipeFilterHack = CraftingUI.RecipeFilterHack;
			for (int i = 0; i < Recipe.maxRecipes; i++)
			{
				Recipe recipe = Main.recipe[i];
				if (recipe.createItem.type == 0)
				{
					break;
				}
				if ((recipeFilterHack == null || recipeFilterHack.Accepts(recipe)) && recipe.PlayerMeetsEnvironmentConditions(localPlayer, null) && Recipe.CollectedEnoughItemsToCraft(recipe))
				{
					Recipe.AddToAvailableRecipes(i);
				}
			}
			Recipe.TryRefocusingRecipe(oldRecipe);
			Main.craftingUI.VisuallyRepositionRecipes(focusRecipe);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0012C524 File Offset: 0x0012A724
		public static void SubtractOwnedItem(Recipe.RequiredItemEntry req)
		{
			int num;
			if (Recipe._ownedItems.TryGetValue(req.itemIdOrRecipeGroup, out num))
			{
				Recipe._ownedItems[req.itemIdOrRecipeGroup] = Math.Max(num - req.stack, 0);
			}
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0012C563 File Offset: 0x0012A763
		private static void AddToAvailableRecipes(int recipeIndex)
		{
			Main.availableRecipe[Main.numAvailableRecipes] = recipeIndex;
			Main.numAvailableRecipes++;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0012C57D File Offset: 0x0012A77D
		public static bool CollectedEnoughItemsToCraft(Recipe recipe)
		{
			return Recipe.CollectedEnoughItemsToCraft(recipe.requiredItemQuickLookup);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0012C58A File Offset: 0x0012A78A
		public static bool CollectedEnoughItemsToCraft(Recipe.RequiredItemEntry[] requiredItems)
		{
			return Recipe.HowManyTimesCanRecipeBeCrafted(requiredItems) > 0;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0012C595 File Offset: 0x0012A795
		public static int HowManyTimesCanRecipeBeCrafted(Recipe recipe)
		{
			return Recipe.HowManyTimesCanRecipeBeCrafted(recipe.requiredItemQuickLookup);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0012C5A4 File Offset: 0x0012A7A4
		public static int HowManyTimesCanRecipeBeCrafted(Recipe.RequiredItemEntry[] requiredItems)
		{
			int num = int.MaxValue;
			foreach (Recipe.RequiredItemEntry requiredItemEntry in requiredItems)
			{
				if (requiredItemEntry.itemIdOrRecipeGroup == 0)
				{
					break;
				}
				int num2;
				if (!Recipe._ownedItems.TryGetValue(requiredItemEntry.itemIdOrRecipeGroup, out num2))
				{
					return 0;
				}
				num = Math.Min(num, num2 / requiredItemEntry.stack);
				if (num == 0)
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0012C604 File Offset: 0x0012A804
		public static int GetAvailableItemCount(Recipe.RequiredItemEntry item)
		{
			int result;
			if (!Recipe._ownedItems.TryGetValue(item.itemIdOrRecipeGroup, out result))
			{
				return 0;
			}
			return result;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0012C628 File Offset: 0x0012A828
		public bool PlayerMeetsEnvironmentConditions(Player player, List<string> missingObjects = null)
		{
			bool flag = true;
			if (this.requiredTile >= 0 && !player.adjTile[this.requiredTile])
			{
				flag &= Recipe.AddMissing(Recipe.GetRequiredTileName(this.requiredTile), missingObjects);
			}
			if (this.needWater && !player.adjWaterSource)
			{
				flag &= Recipe.AddMissing(Lang.inter[53].Value, missingObjects);
			}
			if (this.needHoney && !player.adjHoney)
			{
				flag &= Recipe.AddMissing(Lang.inter[58].Value, missingObjects);
			}
			if (this.needLava && !player.adjLava)
			{
				flag &= Recipe.AddMissing(Lang.inter[56].Value, missingObjects);
			}
			if (this.needSnowBiome && !player.ZoneSnow)
			{
				flag &= Recipe.AddMissing(Lang.inter[123].Value, missingObjects);
			}
			if (this.needGraveyardBiome && !player.ZoneGraveyard)
			{
				flag &= Recipe.AddMissing(Lang.inter[124].Value, missingObjects);
			}
			if (this.needMechdusa)
			{
				flag &= SpecialSeedFeatures.Mechdusa;
			}
			if (this.needTorchGodsFavor && !player.unlockedBiomeTorches)
			{
				flag &= Recipe.AddMissing(Lang.inter[125].Value, missingObjects);
			}
			return flag;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0012C758 File Offset: 0x0012A958
		private static bool AddMissing(string objectName, List<string> missingObjects)
		{
			if (missingObjects != null)
			{
				missingObjects.Add(objectName);
			}
			return false;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0012C765 File Offset: 0x0012A965
		private static void CollectItemsToCraftWithFrom(Player player)
		{
			Recipe._ownedItems.Clear();
			Recipe.CollectItems(player.inventory, 58);
			Recipe.CollectItemsFromChests(player);
			Recipe.AddFakeCountsForItemGroups();
			CraftingRequests.SubtractPendingRequests();
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0012C790 File Offset: 0x0012A990
		private static void CollectItemsFromChests(Player player)
		{
			Recipe._recipeChests.Clear();
			if (player.chest != -1)
			{
				Recipe._recipeChests.Add(player.GetCurrentContainer());
			}
			if (player.useVoidBag() && player.chest != -5)
			{
				Recipe._recipeChests.Add(player.bank4);
			}
			if (Player.Settings.CraftFromNearbyChests)
			{
				foreach (PositionedChest positionedChest in NearbyChests.GetBanksInRangeOf(player, 0f))
				{
					if (!Recipe._recipeChests.Contains(positionedChest.chest))
					{
						Recipe._recipeChests.Add(positionedChest.chest);
					}
				}
				foreach (PositionedChest positionedChest2 in NearbyChests.GetChestsInRangeOf(player.position, 0f))
				{
					if (!positionedChest2.chest.IsLockedOrInUse())
					{
						Recipe._recipeChests.Add(positionedChest2.chest);
					}
				}
			}
			foreach (Chest chest in Recipe._recipeChests)
			{
				Recipe.CollectItems(chest.item, chest.maxItems);
			}
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0012C904 File Offset: 0x0012AB04
		private static void AddFakeCountsForItemGroups()
		{
			foreach (RecipeGroup recipeGroup in RecipeGroup.recipeGroups.Values)
			{
				int groupFakeItemId = recipeGroup.GetGroupFakeItemId();
				Recipe._ownedItems[groupFakeItemId] = recipeGroup.CountUsableItems(Recipe._ownedItems);
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0012C974 File Offset: 0x0012AB74
		private static void CollectItems(Item[] currentInventory, int slotCap)
		{
			for (int i = 0; i < slotCap; i++)
			{
				Item item = currentInventory[i];
				if (item.stack > 0)
				{
					int num = item.stack;
					int num2;
					if (Recipe._ownedItems.TryGetValue(item.type, out num2))
					{
						num += num2;
					}
					Recipe._ownedItems[item.type] = num;
				}
			}
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0012C9CC File Offset: 0x0012ABCC
		public static void ConsumeOwnedItem(int type, int qty)
		{
			Dictionary<int, int> ownedItems = Recipe._ownedItems;
			ownedItems[type] -= qty;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0012C9F4 File Offset: 0x0012ABF4
		private static void CollectGuideRecipes()
		{
			int type = Main.guideItem.type;
			for (int i = 0; i < Recipe.maxRecipes; i++)
			{
				Recipe recipe = Main.recipe[i];
				if (recipe.createItem.type == 0)
				{
					break;
				}
				for (int j = 0; j < Recipe.maxRequirements; j++)
				{
					Recipe.RequiredItemEntry requiredItemEntry = recipe.requiredItemQuickLookup[j];
					if (requiredItemEntry.itemIdOrRecipeGroup == 0)
					{
						break;
					}
					if (requiredItemEntry.Matches(type))
					{
						Main.availableRecipe[Main.numAvailableRecipes] = i;
						Main.numAvailableRecipes++;
						break;
					}
				}
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0012CA7C File Offset: 0x0012AC7C
		public static void ClearAvailableRecipes()
		{
			for (int i = 0; i < Recipe.maxRecipes; i++)
			{
				Main.availableRecipe[i] = 0;
			}
			Main.numAvailableRecipes = 0;
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0012CAA8 File Offset: 0x0012ACA8
		private static void TryRefocusingRecipe(int oldRecipe)
		{
			for (int i = 0; i < Main.numAvailableRecipes; i++)
			{
				if (oldRecipe == Main.availableRecipe[i])
				{
					Main.focusRecipe = i;
					break;
				}
			}
			if (Main.focusRecipe >= Main.numAvailableRecipes)
			{
				Main.focusRecipe = Main.numAvailableRecipes - 1;
			}
			if (Main.focusRecipe < 0)
			{
				Main.focusRecipe = 0;
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0012CB00 File Offset: 0x0012AD00
		public static void SetupRecipeGroups()
		{
			RecipeGroups.Birds = new RecipeGroup("NPCName.Bird", new int[]
			{
				2015,
				2016,
				2017
			}).Register();
			RecipeGroups.Scorpions = new RecipeGroup("NPCName.Scorpion", new int[]
			{
				2157,
				2156
			}).Register();
			RecipeGroups.Squirrels = new RecipeGroup("NPCName.Squirrel", new int[]
			{
				2018,
				3563
			}).Register();
			RecipeGroups.Bugs = new RecipeGroup(Lang.misc[85].Key, new int[]
			{
				3194,
				3192,
				3193
			}).Register();
			RecipeGroups.Ducks = new RecipeGroup(Lang.misc[86].Key, new int[]
			{
				2123,
				2122
			}).Register();
			RecipeGroups.Butterflies = new RecipeGroup(Lang.misc[87].Key, new int[]
			{
				1998,
				2001,
				1994,
				1995,
				1996,
				1999,
				1997,
				2000
			}).Register();
			RecipeGroups.Fireflies = new RecipeGroup(Lang.misc[88].Key, new int[]
			{
				1992,
				2004
			}).Register();
			RecipeGroups.Snails = new RecipeGroup(Lang.misc[95].Key, new int[]
			{
				2006,
				2007
			}).Register();
			RecipeGroups.Dragonflies = new RecipeGroup(Lang.misc[105].Key, new int[]
			{
				4334,
				4335,
				4336,
				4338,
				4339,
				4337
			}).Register();
			RecipeGroups.Turtles = new RecipeGroup(Lang.GetNPCNameValue(616), new int[]
			{
				4464,
				4465
			}).Register();
			RecipeGroups.Macaws = new RecipeGroup("Misc.Macaw", new int[]
			{
				5212,
				5300
			}).Register();
			RecipeGroups.Cockatiels = new RecipeGroup("Misc.Cockatiel", new int[]
			{
				5312,
				5313
			}).Register();
			RecipeGroups.CloudBalloons = new RecipeGroup("Misc.CloudBalloon", new int[]
			{
				399,
				1250
			}).Register();
			RecipeGroups.BlizzardBalloons = new RecipeGroup("Misc.BlizzardBalloon", new int[]
			{
				1163,
				1251
			}).Register();
			RecipeGroups.SandstormBalloons = new RecipeGroup("Misc.SandstormBalloon", new int[]
			{
				983,
				1252
			}).Register();
			RecipeGroups.CritterGuides = new RecipeGroup("Misc.CritterGuides", new int[]
			{
				4767,
				5453
			}).Register();
			RecipeGroups.NatureGuides = new RecipeGroup("Misc.NatureGuides", new int[]
			{
				5309,
				5454
			}).Register();
			RecipeGroups.Seashells = new RecipeGroup("Misc.Seashells", new int[]
			{
				2625,
				2626
			}).Register();
			RecipeGroups.Fruit = new RecipeGroup("Misc.Fruit", new int[]
			{
				4009,
				4282,
				4283,
				4284,
				4285,
				4286,
				4287,
				4288,
				4289,
				4290,
				4291,
				4292,
				4293,
				4294,
				4295,
				4296,
				4297,
				5277,
				5278
			}).Register();
			RecipeGroups.Balloons = new RecipeGroup("Misc.Balloon", new int[]
			{
				3738,
				3736,
				3737
			}).Register();
			RecipeGroups.CobaltBar = new RecipeGroup("ItemName.CobaltBar", new int[]
			{
				381,
				1184
			}).Register();
			RecipeGroups.MythrilBar = new RecipeGroup("ItemName.MythrilBar", new int[]
			{
				382,
				1191
			}).Register();
			RecipeGroups.AdamantiteBar = new RecipeGroup("ItemName.AdamantiteBar", new int[]
			{
				391,
				1198
			}).Register();
			RecipeGroups.GemCritter = new RecipeGroup("Misc.GemCritter", new int[]
			{
				4838,
				4844,
				4843,
				4841,
				4842,
				4840,
				4839,
				4831,
				4837,
				4836,
				4834,
				4835,
				4833,
				4832
			}).Register();
			RecipeGroups.MagicMirror = new RecipeGroup("ItemName.MagicMirror", new int[]
			{
				50,
				3199
			}).Register();
			RecipeGroups.Wood = new RecipeGroup("ItemName.Wood", new int[]
			{
				9,
				619,
				620,
				621,
				911,
				1729,
				2504,
				2503,
				5215
			}).Register();
			RecipeGroups.Stone = new RecipeGroup("ItemName.StoneBlock", new int[]
			{
				3,
				61,
				836,
				409
			}).Register();
			RecipeGroups.Sand = new RecipeGroup("ItemName.SandBlock", new int[]
			{
				169,
				408,
				1246,
				370,
				3272,
				3338,
				3274,
				3275
			}).Register();
			RecipeGroups.IronBar = new RecipeGroup("ItemName.IronBar", new int[]
			{
				22,
				704
			}).Register();
			RecipeGroups.Fragment = new RecipeGroup(Lang.misc[51].Key, new int[]
			{
				3458,
				3456,
				3457,
				3459
			}).Register();
			RecipeGroups.PressurePlate = new RecipeGroup(Lang.misc[38].Key, new int[]
			{
				542,
				852,
				543,
				541,
				1151,
				529,
				853,
				4261
			}).Register();
			RecipeGroups.Jellyfish = new RecipeGroup("Misc.Jellyfish", new int[]
			{
				2436,
				2437,
				2438
			}).Register();
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0012D050 File Offset: 0x0012B250
		public static void UpdateItemVariants()
		{
			for (int i = 0; i < Recipe.maxRecipes; i++)
			{
				Recipe recipe = Main.recipe[i];
				recipe.createItem.Refresh(true);
				Item[] array = recipe.requiredItem;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].Refresh(true);
				}
			}
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0012D0A0 File Offset: 0x0012B2A0
		public static void AddTileCountsAs(int tile, int equivalent)
		{
			List<int> list = Recipe.TileCountsAs[tile];
			if (list == null)
			{
				list = (Recipe.TileCountsAs[tile] = new List<int>());
			}
			list.Add(equivalent);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0012D0D0 File Offset: 0x0012B2D0
		private static void SetupTileInheritance()
		{
			Array.Clear(Recipe.TileCountsAs, 0, Recipe.TileCountsAs.Length);
			Recipe.AddTileCountsAs(96, 215);
			Recipe.AddTileCountsAs(17, 215);
			Recipe.AddTileCountsAs(302, 17);
			Recipe.AddTileCountsAs(77, 17);
			Recipe.AddTileCountsAs(133, 77);
			Recipe.AddTileCountsAs(134, 16);
			Recipe.AddTileCountsAs(355, 13);
			Recipe.AddTileCountsAs(699, 13);
			Recipe.AddTileCountsAs(304, 86);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0012D158 File Offset: 0x0012B358
		private static bool TileUsedInRecipeInherited(int tileType)
		{
			if (Recipe.TileUsedInRecipes[tileType])
			{
				return true;
			}
			List<int> list = Recipe.TileCountsAs[tileType];
			if (list != null)
			{
				using (List<int>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (Recipe.TileUsedInRecipeInherited(enumerator.Current))
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0012D1C4 File Offset: 0x0012B3C4
		private static void UpdateInheritedTilesUsedInRecipes()
		{
			for (int i = 0; i < Recipe.TileCountsAs.Length; i++)
			{
				if (Recipe.TileCountsAs[i] != null)
				{
					Recipe.TileUsedInRecipes[i] = Recipe.TileUsedInRecipeInherited(i);
				}
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0012D1FC File Offset: 0x0012B3FC
		public static void SetupRecipes()
		{
			Array.Clear(Recipe.TileUsedInRecipes, 0, Recipe.TileUsedInRecipes.Length);
			Recipe.SetupTileInheritance();
			int num = 5;
			int stack = 2;
			Recipe.currentRecipe.createItem.SetDefaults(8, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(23, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(974, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(664, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(974, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(833, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(974, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(834, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(974, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(835, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4383, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3272, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4383, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3274, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4383, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3275, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4383, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3338, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4384, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(275, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4385, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(61, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4385, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(833, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4385, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3274, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4386, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(836, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4386, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(835, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4386, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3275, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4387, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(409, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4387, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(834, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4387, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3338, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4388, null);
			Recipe.currentRecipe.createItem.stack = 25;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(331, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5293, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(183, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5293, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(23, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(183, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3114, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3111, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(433, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(173, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(523, null);
			Recipe.currentRecipe.createItem.stack = 33;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 33;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(522, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1333, null);
			Recipe.currentRecipe.createItem.stack = 33;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 33;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1332, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3045, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(662, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(427, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(177, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(428, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(178, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(429, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(179, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(432, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(180, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(430, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(181, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(431, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(182, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1245, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(999, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5378, null);
			Recipe.currentRecipe.createItem.stack = 33;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(931, null);
			Recipe.currentRecipe.requiredItem[0].stack = 33;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(522, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5379, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(931, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(662, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5377, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(931, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3002, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(966, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3048, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(974, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3047, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(433, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3046, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(523, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3049, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1333, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3050, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3045, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3723, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2274, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3724, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3004, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4689, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4383, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4690, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4384, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4691, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4385, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4692, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4386, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4693, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4387, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4694, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4388, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5299, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5293, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5357, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5353, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2751, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2701, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(522, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2752, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2701, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(433, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2753, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2701, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2754, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2701, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1332, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2755, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2701, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2274, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(985, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(965, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3005, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2996, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3078, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(150, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3080, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3078, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3077, null);
			Recipe.currentRecipe.createItem.stack = 30;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3079, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3077, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2590, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(353, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(23, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(8, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1130, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(168, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2431, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2586, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(168, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(23, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(235, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(166, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(23, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2896, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(167, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(23, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3116, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(168, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3111, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3115, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(166, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3111, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3547, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(167, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3111, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4423, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(166, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3380, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4908, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(166, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2, null);
			Recipe.currentRecipe.requiredItem[1].stack = 25;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4909, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4908, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(23, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4909, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(235, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2, null);
			Recipe.currentRecipe.requiredItem[1].stack = 25;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5594, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				166,
				5,
				381,
				1
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.CobaltBar);
			Recipe.currentRecipe.SetCraftingStation(16);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5595, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5594,
				1,
				23,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5595, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				235,
				5,
				381,
				1
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.CobaltBar);
			Recipe.currentRecipe.SetCraftingStation(16);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1338, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2019, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(167, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(286, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(282, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(23, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3112, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(282, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3111, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3191, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2002, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(75, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2243, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2244, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(351, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(353, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(351, null);
			Recipe.currentRecipe.requiredTile = 94;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(357, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(261, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3195, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3192, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3193, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3194, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1787, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5092, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				68,
				8
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.corruption = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5092, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				1330,
				8
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.crimson = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5093, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2121, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4033, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2121
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4032, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2123
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4032, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2122
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4032, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4374
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4031, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2015
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4031, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2017
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4031, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2016
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4031, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2205
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4031, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4359
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4031, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4395
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4024, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2018
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4024, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3563
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4014, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2019
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5645, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4838
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.GemCritter);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4019, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2006
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4022, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2308
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4022, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2889
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4022, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2890
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4022, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2892
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4022, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2894
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4022, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3564
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4022, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2893
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4022, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2895
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4022, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2891
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4022, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4274
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4022, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4340
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4022, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4362
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4022, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4482
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4022, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4419
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2425, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2290, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2425, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2297, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2425, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2299, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2427, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2300, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2427, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2298, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2427, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2301, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2427, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4401, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2426, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2316, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4403, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4402, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5009, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 622;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4614, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4009, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(31, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4617, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4283, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(593, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4615, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4023, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(31, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4616, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4291, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(31, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4618, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4293, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(31, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4619, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4294, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4287, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(31, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4620, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4292, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4294, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(31, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4621, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4285, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4296, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(31, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4622, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4284, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4289, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(31, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4624, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4009, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(31, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Fruit);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4625, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4009, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(356, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Fruit);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4623, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4297, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4288, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(31, null);
			Recipe.currentRecipe.requiredTile = 96;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4034, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2303,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4034, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2317,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4034, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2305,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4034, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2304,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4034, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2313,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4034, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2318,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4034, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2312,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4034, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2306,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4034, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2319,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4034, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2314,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4034, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2302,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4034, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2315,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4034, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2307,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4034, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2310,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4034, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2309,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4034, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2321,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4034, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2311,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(96);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5537, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2290, null);
			Recipe.currentRecipe.requiredTile = 215;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5537, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2297, null);
			Recipe.currentRecipe.requiredTile = 215;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5537, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2299, null);
			Recipe.currentRecipe.requiredTile = 215;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5537, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4401, null);
			Recipe.currentRecipe.requiredTile = 215;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5537, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2302, null);
			Recipe.currentRecipe.requiredTile = 215;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5537, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2298, null);
			Recipe.currentRecipe.requiredTile = 215;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5537, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2300, null);
			Recipe.currentRecipe.requiredTile = 215;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5537, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2301, null);
			Recipe.currentRecipe.requiredTile = 215;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(968, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(967, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(31, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.needWater = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1134, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.needHoney = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(422, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(501, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(369, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(423, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(370, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(59, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3477, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1246, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(2171, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(28, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(23, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[2].stack = 2;
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(227, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3111, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(31, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(188, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(28, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(183, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(499, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(501, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(502, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5496, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				499,
				3,
				1291,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(13);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3544, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(499, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredItem[4].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(189, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(110, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(183, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2209, null);
			Recipe.currentRecipe.createItem.stack = 15;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(500, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(75, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(502, null);
			Recipe.currentRecipe.requiredItem[2].stack = 3;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(526, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2756, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(313, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(314, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(315, null);
			Recipe.currentRecipe.requiredItem[4].SetDefaults(317, null);
			Recipe.currentRecipe.requiredItem[5].SetDefaults(316, null);
			Recipe.currentRecipe.requiredItem[6].SetDefaults(2358, null);
			Recipe.currentRecipe.requiredItem[7].SetDefaults(318, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5099, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(313, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(314, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(315, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(317, null);
			Recipe.currentRecipe.requiredItem[4].SetDefaults(316, null);
			Recipe.currentRecipe.requiredItem[5].SetDefaults(2358, null);
			Recipe.currentRecipe.requiredItem[6].SetDefaults(318, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(288, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(318, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(317, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(173, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(289, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(313, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(5, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(290, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(315, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(276, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(291, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(317, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(275, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5573, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(313, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(314, null);
			Recipe.currentRecipe.requiredItem[4].SetDefaults(315, null);
			Recipe.currentRecipe.requiredItem[4].SetDefaults(318, null);
			Recipe.currentRecipe.needTorchGodsFavor = true;
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(292, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(313, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(11, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(292, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(313, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(700, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(293, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(314, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(313, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(75, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(294, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(314, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(316, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(75, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(295, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(313, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(315, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(320, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(296, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(315, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(314, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(13, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(296, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(315, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(314, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(702, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(297, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(315, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(314, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(298, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(313, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(183, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(299, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(313, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(315, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(300, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(316, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(68, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.currentRecipe.corruption = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(300, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(316, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1330, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.currentRecipe.crimson = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(301, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(316, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(276, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(302, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(317, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(319, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(303, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(313, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(38, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(304, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(313, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(315, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(319, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(305, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(318, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(316, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(315, null);
			Recipe.currentRecipe.requiredItem[4].SetDefaults(320, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2354, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1127, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(317, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2356, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(999, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(314, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(2358, null);
			Recipe.currentRecipe.requiredItem[4].SetDefaults(317, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2325, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(315, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(2358, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(314, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2326, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(316, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(2358, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2329, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2358, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(150, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2355, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(317, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(275, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2322, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(323, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(315, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2327, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2358, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(317, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2323, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2305, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(313, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2324, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2304, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(313, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2328, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2311, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(314, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2344, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2313, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(314, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2345, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2310, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(314, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(2358, null);
			Recipe.currentRecipe.requiredItem[4].SetDefaults(317, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2346, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2303, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(315, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2348, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2312, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(2315, null);
			Recipe.currentRecipe.requiredItem[2].stack = 2;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(318, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2347, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2319, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(316, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2349, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2318, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(316, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2350, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2309, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(313, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2997, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2309, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(315, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2351, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2317, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(315, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(318, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2352, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2307, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(2358, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2353, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2321, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(316, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2359, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2306, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(2358, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4477, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(317, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(4361, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(4412, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4478, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(317, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(4361, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(4413, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4479, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(317, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(4361, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(4414, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4870, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2350, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2315, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5211, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				126,
				1,
				318,
				1,
				315,
				1,
				314,
				1,
				62,
				5
			});
			Recipe.currentRecipe.SetCraftingStation(13);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1359, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(209, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 243;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1354, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(174, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 243;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1340, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1339, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 243;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1355, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1348, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 243;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1356, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1332, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 243;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1353, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(522, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 243;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1357, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1346, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 243;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1358, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1345, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 243;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3092, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(520, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3091, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(521, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(949, null);
			Recipe.currentRecipe.createItem.stack = 15;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(593, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3103, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(40, null);
			Recipe.currentRecipe.requiredItem[0].stack = 9999;
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3104, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(97, null);
			Recipe.currentRecipe.requiredItem[0].stack = 9999;
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(40, null);
			Recipe.currentRecipe.createItem.stack = 25;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(41, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(40, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(988, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(40, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(974, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(51, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(40, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(75, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(47, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(40, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(69, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(47, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(40, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1330, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(265, null);
			Recipe.currentRecipe.createItem.stack = 100;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(40, null);
			Recipe.currentRecipe.requiredItem[0].stack = 100;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(175, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(545, null);
			Recipe.currentRecipe.createItem.stack = 150;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(40, null);
			Recipe.currentRecipe.requiredItem[0].stack = 150;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(522, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1334, null);
			Recipe.currentRecipe.createItem.stack = 150;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(40, null);
			Recipe.currentRecipe.requiredItem[0].stack = 150;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1332, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(516, null);
			Recipe.currentRecipe.createItem.stack = 200;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(40, null);
			Recipe.currentRecipe.requiredItem[0].stack = 200;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(501, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(526, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1341, null);
			Recipe.currentRecipe.createItem.stack = 35;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(40, null);
			Recipe.currentRecipe.requiredItem[0].stack = 35;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1339, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1235, null);
			Recipe.currentRecipe.createItem.stack = 150;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1310, null);
			Recipe.currentRecipe.createItem.stack = 100;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(209, null);
			Recipe.currentRecipe.SetCraftingStation(16);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3009, null);
			Recipe.currentRecipe.createItem.stack = 100;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(502, null);
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3010, null);
			Recipe.currentRecipe.createItem.stack = 100;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(522, null);
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3011, null);
			Recipe.currentRecipe.createItem.stack = 100;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1332, null);
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(234, null);
			Recipe.currentRecipe.createItem.stack = 70;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(97, null);
			Recipe.currentRecipe.requiredItem[0].stack = 70;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(117, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(278, null);
			Recipe.currentRecipe.createItem.stack = 70;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(97, null);
			Recipe.currentRecipe.requiredItem[0].stack = 70;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(21, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.AddCustomShimmerResult(97, 70);
			Recipe.currentRecipe.AddCustomShimmerResult(14, 1);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4915, null);
			Recipe.currentRecipe.createItem.stack = 70;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(97, null);
			Recipe.currentRecipe.requiredItem[0].stack = 70;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(705, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.AddCustomShimmerResult(97, 70);
			Recipe.currentRecipe.AddCustomShimmerResult(701, 1);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(515, null);
			Recipe.currentRecipe.createItem.stack = 100;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(97, null);
			Recipe.currentRecipe.requiredItem[0].stack = 100;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(502, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(546, null);
			Recipe.currentRecipe.createItem.stack = 150;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(97, null);
			Recipe.currentRecipe.requiredItem[0].stack = 150;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(522, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1335, null);
			Recipe.currentRecipe.createItem.stack = 150;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(97, null);
			Recipe.currentRecipe.requiredItem[0].stack = 150;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1332, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1179, null);
			Recipe.currentRecipe.createItem.stack = 60;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(97, null);
			Recipe.currentRecipe.requiredItem[0].stack = 60;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1302, null);
			Recipe.currentRecipe.createItem.stack = 50;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1432, null);
			Recipe.currentRecipe.requiredItem[0].stack = 50;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1349, null);
			Recipe.currentRecipe.createItem.stack = 50;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1432, null);
			Recipe.currentRecipe.requiredItem[0].stack = 50;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1345, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1350, null);
			Recipe.currentRecipe.createItem.stack = 50;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1432, null);
			Recipe.currentRecipe.requiredItem[0].stack = 50;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1346, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1351, null);
			Recipe.currentRecipe.createItem.stack = 50;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1432, null);
			Recipe.currentRecipe.requiredItem[0].stack = 50;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1347, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1352, null);
			Recipe.currentRecipe.createItem.stack = 50;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1432, null);
			Recipe.currentRecipe.requiredItem[0].stack = 50;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1348, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1342, null);
			Recipe.currentRecipe.createItem.stack = 50;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1432, null);
			Recipe.currentRecipe.requiredItem[0].stack = 50;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1339, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4447, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4459, null);
			Recipe.currentRecipe.needWater = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4448, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4459, null);
			Recipe.currentRecipe.needLava = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4449, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4459, null);
			Recipe.currentRecipe.needHoney = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4459, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4447, null);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4459, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4448, null);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4459, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4449, null);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4824, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4827, null);
			Recipe.currentRecipe.needWater = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4825, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4827, null);
			Recipe.currentRecipe.needLava = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4826, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4827, null);
			Recipe.currentRecipe.needHoney = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4827, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4824, null);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4827, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4825, null);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4827, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4826, null);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4457, null);
			Recipe.currentRecipe.createItem.stack = 100;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(773, null);
			Recipe.currentRecipe.requiredItem[0].stack = 100;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1552, null);
			Recipe.currentRecipe.requiredTile = 247;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4458, null);
			Recipe.currentRecipe.createItem.stack = 100;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(774, null);
			Recipe.currentRecipe.requiredItem[0].stack = 100;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1552, null);
			Recipe.currentRecipe.requiredTile = 247;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(67, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(60, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2886, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2887, null);
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5438, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5395, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(172, null);
			Recipe.currentRecipe.requiredItem[2].stack = 3;
			Recipe.currentRecipe.requiredTile = 13;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(287, null);
			Recipe.currentRecipe.createItem.stack = 50;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(279, null);
			Recipe.currentRecipe.requiredItem[0].stack = 50;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(67, null);
			Recipe.currentRecipe.corruption = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(287, null);
			Recipe.currentRecipe.createItem.stack = 50;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(279, null);
			Recipe.currentRecipe.requiredItem[0].stack = 50;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2886, null);
			Recipe.currentRecipe.crimson = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3610, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3609, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3609, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3610, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(94, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4416, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(129, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2518, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2566, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(632, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(631, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(913, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(633, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2744, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1702, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1796, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1818, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(634, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2549, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2581, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2627, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2628, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(824, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2629, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2630, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1457, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(192, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3144, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3100, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3146, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3145, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2822, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2860, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3903, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3234, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3945, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3905, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3906, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1101, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3907, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(763, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3957, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3955, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3908, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4159, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4180, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4201, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4222, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4311, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4580, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5162, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5183, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				1
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5204, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5292, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4392,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5544, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				751,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1384, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				134,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1386, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				137,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1385, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				139,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5562, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5615, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5703, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5710,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5726, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5733,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5751, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5922,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5770, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				577,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5791, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2793,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5812, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				593,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5833, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5924,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5852, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				1872,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5852, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5930,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5872, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5920,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5912, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5926,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5946, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5953,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5989, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5996,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6012, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				6019,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6035, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				6042,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6058, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				6065,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6081, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				6088,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6103, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				6109,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6125, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				6132,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1389, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1388, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(145, null);
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1418, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1387, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(717, null);
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1431, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(75, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1993, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1992, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2005, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2004, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4848, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4847, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5351, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5350, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4695, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(520, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4696, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(521, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4697, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(575, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4698, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(549, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4699, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(548, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4700, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(547, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1859, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(29, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(85, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(344, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(342, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(341, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(347, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5472, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3782, null);
			Recipe.currentRecipe.createItem.stack = 3;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3186, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(169, null);
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Sand);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3182, null);
			Recipe.currentRecipe.createItem.stack = 1;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3186, null);
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.currentRecipe.needWater = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3184, null);
			Recipe.currentRecipe.createItem.stack = 1;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3186, null);
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.currentRecipe.needLava = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3185, null);
			Recipe.currentRecipe.createItem.stack = 1;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3186, null);
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.currentRecipe.needHoney = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2693, null);
			Recipe.currentRecipe.createItem.stack = 1;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.needWater = true;
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2169, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2693, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2694, null);
			Recipe.currentRecipe.createItem.stack = 1;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.needLava = true;
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2170, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2694, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2787, null);
			Recipe.currentRecipe.createItem.stack = 1;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.needHoney = true;
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2788, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2787, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3754, null);
			Recipe.currentRecipe.createItem.stack = 1;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(169, null);
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3752, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3754, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3755, null);
			Recipe.currentRecipe.createItem.stack = 1;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(593, null);
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3753, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3755, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5494, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5493, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4277, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(75, null);
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4279, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4277, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4278, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(75, null);
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4280, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4278, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5291, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4392, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4490, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4640, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4491, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4641, null);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4492, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4642, null);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4493, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4643, null);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4494, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4644, null);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4495, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4645, null);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4647, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4646, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4496, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4349, null);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4497, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4350, null);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4498, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4351, null);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4499, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4352, null);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4500, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4353, null);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4503, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(150, null);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4529, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(62, null);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4531, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(62, null);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4530, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(195, null);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4532, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(195, null);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3340, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3272,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3341, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3274,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3342, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3275,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3343, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3338,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3344, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3276,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3345, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3277,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3346, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3339,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3348, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3347,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(663, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(662, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2695, null);
			Recipe.currentRecipe.createItem.stack = 1;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1345, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2696, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2695, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2697, null);
			Recipe.currentRecipe.createItem.stack = 1;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1345, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2698, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2697, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3748, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3743, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3744, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3745, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4090, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2625, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4090, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4071, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4090, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4072, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4090, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4073, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4090, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2626, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4090, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(275, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5323, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4767, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5309, null);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.CritterGuides);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.NatureGuides);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(752, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(751, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(222, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(133, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(350, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(133, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(356, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(133, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4326, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(133, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(169, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Sand);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(392, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1267, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(392, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(181, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1268, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(392, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(180, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1269, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(392, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(177, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1270, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(392, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(179, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1271, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(392, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(178, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4260, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(392, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(999, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1272, null);
			Recipe.currentRecipe.createItem.stack = 50;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(392, null);
			Recipe.currentRecipe.requiredItem[0].stack = 50;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(181, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(180, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(177, null);
			Recipe.currentRecipe.requiredItem[4].SetDefaults(179, null);
			Recipe.currentRecipe.requiredItem[5].SetDefaults(178, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1970, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(181, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			int type = 1970;
			int type2 = 2679;
			int type3 = 2680;
			Recipe.currentRecipe.createItem.SetDefaults(type2, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(type3, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1971, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(180, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			type = 1971;
			type2 = 2689;
			type3 = 2690;
			Recipe.currentRecipe.createItem.SetDefaults(type2, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(type3, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1972, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(177, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			type = 1972;
			type2 = 2687;
			type3 = 2688;
			Recipe.currentRecipe.createItem.SetDefaults(type2, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(type3, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1973, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(179, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			type = 1973;
			type2 = 2683;
			type3 = 2684;
			Recipe.currentRecipe.createItem.SetDefaults(type2, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(type3, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1974, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(178, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			type = 1974;
			type2 = 2685;
			type3 = 2686;
			Recipe.currentRecipe.createItem.SetDefaults(type2, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(type3, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1975, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(182, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			type = 1975;
			type2 = 2681;
			type3 = 2682;
			Recipe.currentRecipe.createItem.SetDefaults(type2, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(type3, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1976, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(999, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			type = 1976;
			type2 = 2677;
			type3 = 2678;
			Recipe.currentRecipe.createItem.SetDefaults(type2, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(type3, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4238, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(134, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4239, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(137, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4240, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(139, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(135, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(134, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1379, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(134, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1378, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(134, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(138, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(137, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1383, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(137, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1382, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(137, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(140, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(139, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1381, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(139, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1380, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(139, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2119, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2433, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2119, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4962, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2120, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(169, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3272, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(169, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2, null);
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3271, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(169, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2173, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(12, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2432, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2173, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2692, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(699, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2691, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2692, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(775, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(23, null);
			Recipe.currentRecipe.requiredTile = 217;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1102, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(172, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				9,
				5
			});
			Recipe.currentRecipe.SetCraftingStation(77);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(129, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(130, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(129, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(131, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(133, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(132, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(131, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(145, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(12, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(146, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(145, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3951, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(11, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3952, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3951, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3953, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(700, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3954, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3953, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(144, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(143, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(143, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(14, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(142, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(141, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(141, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(13, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(717, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(699, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(720, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(717, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(721, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(718, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(718, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(701, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(722, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(719, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(719, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(702, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(214, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(174, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3067, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(214, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4533, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				174,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4534, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				174,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4535, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				174,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4536, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				174,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(192, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(173, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(330, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(192, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4507, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				173,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(606, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(577, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(594, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(593, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(595, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(594, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4489, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				593,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(883, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4506, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				664,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(884, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(883, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(587, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(586, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(592, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(591, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(607, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(169, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(608, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(607, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4051, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3271, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4053, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4051, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3273, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3271,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4565, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4564, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4547, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4564, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4564, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4547, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4548, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4547, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(412, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(409, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(417, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(412, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4488, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				409,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4525, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				409,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4526, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				409,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4527, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				409,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4528, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				409,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(609, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(61, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(610, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(609, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4486, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(61, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4513, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				61,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4514, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				61,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4515, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				61,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4516, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				61,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4050, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(836, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4052, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4050, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4509, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				836,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4517, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				836,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4518, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				836,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4519, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				836,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4520, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				836,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(413, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(172, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(418, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(413, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(414, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(176, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(419, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(414, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(611, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(424, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(133, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(615, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(611, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(612, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(424, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(169, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(616, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(612, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(613, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(424, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(255, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(617, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(613, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(614, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(424, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(618, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(614, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3100, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(116, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3101, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2793, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(880, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(836, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2790, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2793, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(134, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(364, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(137, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(365, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(137, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1106, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(139, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1104, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(139, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1105, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(139, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(366, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(415, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(364, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(420, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(415, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(416, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(365, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(421, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(416, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(604, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(366, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(605, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(604, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1589, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1104, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1590, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1589, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1591, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1105, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1592, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1591, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1593, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1106, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1594, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1593, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2792, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(947, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2789, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2792, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2794, null);
			Recipe.currentRecipe.createItem.stack = 25;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1552, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2791, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2794, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3461, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3460, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3472, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3461, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5409, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5401, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5410, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5402, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5411, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5403, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5412, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5404, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5413, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5405, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5414, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5406, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5415, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5407, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5416, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5408, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5418, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5417, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5420, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5419, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5422, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5421, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5424, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5423, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5426, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5425, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5428, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5427, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5436, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5435, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5434, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5433, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5432, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5431, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5430, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5429, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5439, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4354, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(133, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5440, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4389, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(133, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5441, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4377, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(133, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5442, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4378, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(133, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5443, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5127, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(133, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5444, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5128, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(133, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5445, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5439, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5446, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5440, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5447, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5441, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5448, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5442, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5449, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5443, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5450, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5444, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5397, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5349, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5398, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5349, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5399, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5398, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5622, null);
			Recipe.currentRecipe.createItem.stack = 25;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3,
				25,
				75,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(17);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5623, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5710, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				9,
				20,
				5,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(304);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5711, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5710,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5733, null);
			Recipe.currentRecipe.createItem.stack = 25;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				409,
				25,
				1225,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(17);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5734, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5733,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5919, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				1872,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5921, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5920,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5922, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3,
				20,
				150,
				5,
				154,
				5
			});
			Recipe.currentRecipe.SetCraftingStation(300);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5923, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5922,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5924, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5070
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5925, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5924,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5927, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5926,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5953, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				320
			});
			Recipe.currentRecipe.SetCraftingStation(305);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5954, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5953,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5997, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5996,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6019, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				9,
				20,
				149,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(101);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6020, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				6019,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6042, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3,
				5,
				147,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6043, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				6042,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6134, null);
			Recipe.currentRecipe.createItem.stack = 1;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				6042,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6042, null);
			Recipe.currentRecipe.createItem.stack = 1;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				6134,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6065, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				9,
				10,
				23,
				5
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6066, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				6065,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6088, null);
			Recipe.currentRecipe.createItem.stack = 150;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3,
				150,
				3783,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6089, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				6088,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6109, null);
			Recipe.currentRecipe.createItem.stack = 1;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				170,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needWater = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6110, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				6109,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6132, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				540,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(283);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6133, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				6132,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1872, null);
			Recipe.currentRecipe.createItem.stack = 1;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5930
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5930, null);
			Recipe.currentRecipe.createItem.stack = 1;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				1872
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5931, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5930,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3573, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3574, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3575, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3576, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3234, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(502, null);
			Recipe.currentRecipe.requiredTile = 133;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3238, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3234, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2435, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(275, null);
			Recipe.currentRecipe.needWater = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5306, null);
			Recipe.currentRecipe.createItem.stack = 15;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(275, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(2625, null);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Seashells);
			Recipe.currentRecipe.needWater = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5307, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5306, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5396, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5395, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(577, null);
			Recipe.currentRecipe.createItem.stack = 5;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(56, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(61, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(176, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2, null);
			Recipe.currentRecipe.needWater = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5572, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(176, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4487, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				176,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(30, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4501, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4510, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4511, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4521, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4522, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4523, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4524, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(26, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4502, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4512, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4537, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4538, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4539, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4540, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1723, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3584, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(93, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(623, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(622, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(927, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(624, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2505, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2506, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5216, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5215, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(764, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1726, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1728, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1727, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1730, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3751, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2861, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3760, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3736, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3761, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3737, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3762, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3738, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3239, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3240, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(25, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(34, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(48, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2827, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(32, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(36, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(333, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(224, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(334, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(354, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2519, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2520, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2521, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2536, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2522, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2523, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2524, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2525, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2526, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2601, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2528, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2529, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2533, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2530, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2531, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2527, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2850, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4118, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2504,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2532, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2534, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4717, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2552, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2553, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2554, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2555, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(974, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2556, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(974, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2557, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2558, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(974, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2559, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2560, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2561, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2562, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2563, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(974, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2564, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(974, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2565, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(858, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2852, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4119, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2503,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(677, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(673, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4718, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2597, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(651, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(629, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(626, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2829, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(639, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(636, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(642, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(645, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(648, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2026, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2077, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2050, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2038, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2060, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2087, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2098, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2399, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4097, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				620,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(650, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(628, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2593, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(625, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2828, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(638, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(635, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(641, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(644, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(647, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2021, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2073, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2033, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2046, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2056, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2083, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2093, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2398, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4096, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				619,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2604, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(912, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(915, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(914, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2835, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(917, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(916, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(919, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(920, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2127, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2136, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(918, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2142, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2150, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2146, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2132, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2154, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2401, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4105, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				911,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2602, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(652, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(630, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(627, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2830, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(640, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(637, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(643, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(646, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(649, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2027, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2078, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2039, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2051, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2061, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2088, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2099, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2400, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4098, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				621,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.AddAshWoodFurnitureArmorAndItems();
			Recipe.currentRecipe.createItem.SetDefaults(4721, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2537, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2538, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2539, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2540, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2541, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2542, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(810, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2543, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2544, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2599, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(818, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2545, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2547, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2546, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2548, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2413, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2851, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4103, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				183,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2550, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(814, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2567, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2568, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2569, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2570, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2571, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2572, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2573, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2574, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2575, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2576, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2577, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2578, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2579, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2580, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2582, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2853, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4120, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				762,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(220);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2583, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(815, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3159, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3162, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3165, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3168, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3171, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3174, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3177, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3180, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3126, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3129, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3132, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3135, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3138, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3141, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3150, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3147, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4141, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3100,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3153, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3156, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3100, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3081, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3083, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3082, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3081, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4554, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3160, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3163, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3166, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3169, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3172, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3175, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3178, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3181, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3127, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3130, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3133, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3136, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3139, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3142, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3151, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3148, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4123, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3066,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3154, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3157, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3066, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3086, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3089, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3088, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3086, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4719, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3161, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3164, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3167, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3170, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3173, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3176, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3179, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3125, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3128, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3131, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3134, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3137, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3140, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3143, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3152, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3149, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4122, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3087,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3155, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3158, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3087, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2810, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2811, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2817, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2825, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2818, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2812, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2813, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2814, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2809, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2815, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2816, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2819, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2820, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2821, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2823, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2855, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4121, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2860,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2824, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2826, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2860, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			int type4 = 3234;
			Recipe.currentRecipe.createItem.SetDefaults(3895, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[0].stack = 35;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3897, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[0].stack = 40;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3917, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[0].stack = 50;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3893, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3890, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3889, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3894, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3884, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3898, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[1].stack = 25;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3888, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3911, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[0].stack = 40;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3892, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3891, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3915, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[1].stack = 40;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3918, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[0].stack = (int)((float)num * 2.5f);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4124, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3234,
				15
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3896, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3920, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3909, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(type4, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.AddRecipe();
			Recipe.AddSpiderFurniture();
			Recipe.AddLesionFurniture();
			Recipe.AddSolarFurniture();
			Recipe.AddVortexFurniture();
			Recipe.AddNebulaFurniture();
			Recipe.AddStardustFurniture();
			Recipe.AddSandstoneFurniture();
			Recipe.AddBambooFurniture();
			Recipe.AddCoralFurniture();
			Recipe.AddBalloonFurniture();
			Recipe.AddAetheriumFurniture();
			Recipe.AddFallenStarFurniture();
			Recipe.AddFeywoodFurniture();
			Recipe.AddHallowedFurniture();
			Recipe.AddGothicFurniture();
			Recipe.AddDemoniteFurniture();
			Recipe.AddCrimtaneFurniture();
			Recipe.AddSnowFurniture();
			Recipe.AddFlinxFurFurniture();
			Recipe.AddPineFurniture();
			Recipe.AddEasterFurniture();
			Recipe.AddStoneFurniture();
			Recipe.AddJellyfishFurniture();
			Recipe.AddHarpyFurniture();
			Recipe.AddCloudFurniture();
			Recipe.AddMoonplateFurniture();
			Recipe.AddLibrarianFurniture();
			Recipe.AddSpikeFurniture();
			Recipe.AddOfficeFurniture();
			Recipe.AddForbiddenFurniture();
			Recipe.AddWaterFurniture();
			Recipe.AddBoulderFurniture();
			Recipe.currentRecipe.createItem.SetDefaults(5687, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				23,
				50
			});
			Recipe.currentRecipe.SetCraftingStation(220);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5688, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				23,
				50
			});
			Recipe.currentRecipe.SetCraftingStation(220);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(171, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4710, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1447, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2210, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2211, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2212, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2213, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2507, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2508, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5217, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5215, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1448, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(704, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2333, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4424, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4667, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4564, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			for (int i = 3665; i <= 3704; i++)
			{
				Recipe.currentRecipe.createItem.SetDefaults(i, null);
				Recipe.currentRecipe.requiredItem[0].SetDefaults(ItemID.Sets.TextureCopyLoad[i], null);
				Recipe.currentRecipe.requiredItem[1].SetDefaults(530, null);
				Recipe.currentRecipe.requiredItem[1].stack = 10;
				Recipe.currentRecipe.requiredTile = 283;
				Recipe.AddRecipe();
			}
			int[,] array = new int[,]
			{
				{
					3886,
					3884
				},
				{
					3887,
					3885
				},
				{
					3950,
					3939
				},
				{
					3976,
					3965
				},
				{
					4164,
					4153
				},
				{
					4185,
					4174
				},
				{
					4206,
					4195
				},
				{
					4227,
					4216
				},
				{
					4266,
					4265
				},
				{
					4268,
					4267
				},
				{
					4585,
					4574
				},
				{
					4713,
					4712
				},
				{
					5167,
					5156
				},
				{
					5188,
					5177
				},
				{
					5209,
					5198
				},
				{
					5567,
					5556
				},
				{
					5620,
					5609
				},
				{
					5708,
					5697
				},
				{
					5731,
					5720
				},
				{
					5754,
					5745
				},
				{
					5776,
					5763
				},
				{
					5797,
					5784
				},
				{
					5818,
					5805
				},
				{
					5839,
					5826
				},
				{
					5857,
					5846
				},
				{
					5878,
					5865
				},
				{
					5897,
					5886
				},
				{
					5918,
					5905
				},
				{
					5952,
					5939
				},
				{
					5974,
					5962
				},
				{
					5995,
					5982
				},
				{
					6018,
					6005
				},
				{
					6041,
					6028
				},
				{
					6064,
					6051
				},
				{
					6087,
					6074
				},
				{
					6131,
					6118
				}
			};
			for (int j = 0; j < array.GetLength(0); j++)
			{
				Recipe.currentRecipe.createItem.SetDefaults(array[j, 0], null);
				Recipe.currentRecipe.requiredItem[0].SetDefaults(array[j, 1], null);
				Recipe.currentRecipe.requiredItem[1].SetDefaults(530, null);
				Recipe.currentRecipe.requiredItem[1].stack = 10;
				Recipe.currentRecipe.requiredTile = 283;
				Recipe.AddRecipe();
			}
			Recipe.currentRecipe.createItem.SetDefaults(2340, null);
			Recipe.currentRecipe.createItem.stack = 50;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2492, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2340, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(542, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.PressurePlate);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(479, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(480, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3202, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1727, null);
			Recipe.currentRecipe.requiredItem[1].stack = 50;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(498, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1989, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3977, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2699, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3270, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5137, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5132,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5138, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5132,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(343, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 9;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(359, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(352, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5008, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(133, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[1].stack = 12;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(332, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			for (int k = 2114; k <= 2118; k++)
			{
				Recipe.currentRecipe.createItem.SetDefaults(k, null);
				Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
				Recipe.currentRecipe.requiredItem[0].stack = 12;
				Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
				Recipe.currentRecipe.requiredItem[1].stack = 3;
				Recipe.currentRecipe.requiredTile = 106;
				Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
				Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
				Recipe.AddRecipe();
			}
			Recipe.currentRecipe.createItem.SetDefaults(1706, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1714, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1715, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(335, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2397, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(363, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5012, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5011, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 99;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5147, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3069, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(974, null);
			Recipe.currentRecipe.requiredItem[1].stack = 99;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(55, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(284, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(75, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5298, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(55, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4764, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(670, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2289, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(727, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(728, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(729, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(24, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 7;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(196, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(39, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3278, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(150, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3283, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3278, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(502, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(520, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(733, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(734, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(735, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2509, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2510, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2511, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2745, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 7;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2746, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2747, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2503, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2512, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2513, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2514, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2517, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 7;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2516, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2515, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2504, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(656, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 7;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(657, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(658, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(730, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(731, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(732, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(653, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 7;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(654, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(655, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(619, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(924, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(925, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(926, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(921, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 7;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(922, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(923, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(911, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(736, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(737, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(738, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(659, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 7;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(660, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(661, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(621, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1832, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = 200;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1833, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = 300;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1834, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = 250;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2263, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2264, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2265, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2228, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2230, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3916, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3912, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3919, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2849, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4117, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2260,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2259, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2229, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2231, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2237, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2233, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2232, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2226, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2236, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2224, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2225, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2227, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2235, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2234, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2260, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2596, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(806, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(831, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(819, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3914, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2833, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(829, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2139, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2245, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2135, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2126, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2145, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2153, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2141, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2131, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2149, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2636, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2633, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 304;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4099, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				9,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(304);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2239, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 16;
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1703, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2748, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1709, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2639, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2842, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1713, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1719, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2254, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2025, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2075, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2037, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2048, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2065, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2085, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2097, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2414, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4112, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				170,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(302);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2632, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 302;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1127, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1707, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1711, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2844, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1717, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2251, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1721, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2255, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2124, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2249, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2240, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2023, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2035, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2648, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2058, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2095, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2129, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2257, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2395, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2411, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 308;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4113, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				1125,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(308);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(824, null);
			Recipe.currentRecipe.createItem.stack = 25;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(75, null);
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(825, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(824, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(826, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2606, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(838, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3899, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[2].stack = 15;
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(837, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2834, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(830, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2029, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2070, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2080, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2042, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2053, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2063, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2090, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2102, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2384, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2394, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2410, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2631, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(824, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4104, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				824,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(305);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(765, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(751, null);
			Recipe.currentRecipe.needWater = true;
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3756, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(751, null);
			Recipe.currentRecipe.needSnowBiome = true;
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5569, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(751, null);
			Recipe.currentRecipe.needLava = true;
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5570, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(751, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(75, null);
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5571, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(751, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(662, null);
			Recipe.currentRecipe.requiredTile = 305;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2595, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1143, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1137, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2836, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1144, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1142, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1145, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2030, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2069, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2079, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2041, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2052, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2062, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2089, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2101, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2385, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2396, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2416, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1101, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 303;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4106, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				1101,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(303);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2848, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2248, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2635, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2252, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2031, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2247, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2068, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2076, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(681, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2594, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2044, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3913, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2040, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(974, null);
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2049, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(974, null);
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2059, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(974, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2086, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(974, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2100, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(974, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2288, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 306;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4111, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				664,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(306);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1708, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2649, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2655, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1712, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2638, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2845, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1718, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2253, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1722, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2256, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2125, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2250, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2241, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2024, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2036, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2096, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2130, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2412, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 307;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4114, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				1344,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(307);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(894, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(895, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(896, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(881, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(882, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(750, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(816, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(807, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2616, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2592, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(812, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2020, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2066, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2072, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2032, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2045, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2055, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2082, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2092, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2382, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2392, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2408, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2854, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2743, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4100, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				276,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2661, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2669, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2670, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2668, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2603, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1793, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2637, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1792, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2656, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2619, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2671, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2846, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1794, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1795, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1813, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2643, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2641, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1808, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2054, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4115, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				1725,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1812, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2415, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1731, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1732, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1733, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2605, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1815, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1814, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2620, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2847, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1816, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1817, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2028, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2071, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2081, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2043, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2650, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2064, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2091, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2103, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2383, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2393, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2409, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1729, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 106;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4116, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				1729,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(836, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 218;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(770, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(763, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2598, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(817, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2640, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(809, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2617, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(813, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2832, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(828, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2246, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2022, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2067, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2074, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2034, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2047, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2057, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2084, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2094, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2634, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(763, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 301;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4102, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				763,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(301);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(23, null);
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(769, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(767, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(762, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(664, null);
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3113, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3111, null);
			Recipe.currentRecipe.requiredTile = 220;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1126, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1124, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(768, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(820, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2615, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2591, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(808, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(811, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2831, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(827, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2138, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2140, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2128, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2144, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2152, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2134, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2148, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2381, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 19;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(149, null);
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2391, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4101, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				154,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(300);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2407, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = num;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[1].stack = stack;
			Recipe.currentRecipe.requiredTile = 300;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2618, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(173, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(174, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[2].stack = 2;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2840, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(173, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(174, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4110, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				173,
				4,
				174,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2613, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(139, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2839, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(139, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4109, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				139,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2614, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(134, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2837, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(134, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4107, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				134,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2612, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(137, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2838, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(137, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4108, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				137,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(361, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(362, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(150, null);
			Recipe.currentRecipe.requiredItem[0].stack = 7;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(337, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				225,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(86);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(338, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				225,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(86);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(339, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				225,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(86);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(340, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				225,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(86);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5497, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				225,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(86);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5498, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				225,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(86);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(255, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(195, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(247, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(255, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(248, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(255, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(249, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(255, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3773, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3794, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3774, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3794, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3775, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3794, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(240, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(254, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(241, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(254, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4132, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(981, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4133, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(981, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4134, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(981, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4128, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(254, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4129, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(254, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4130, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(254, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4652, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(255, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4653, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(255, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4654, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(255, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5045, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1330, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5046, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1330, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5047, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1330, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5048, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(133, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1992, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5049, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1992, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5050, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1992, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5051, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5052, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(68, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5053, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(68, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5054, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(313, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5055, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5056, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5057, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(275, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(75, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5058, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(75, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5060, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(75, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5061, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5062, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1016, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5063, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1016, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5102, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5103, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5115, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5116, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5457, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(521, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5322, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(68, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.currentRecipe.corruption = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5457, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(521, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5322, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1330, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.currentRecipe.crimson = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5458, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3794, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(216, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5459, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3794, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(362, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5646, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				225,
				20
			});
			Recipe.currentRecipe.SetCraftingStation(86);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5647, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				225,
				20
			});
			Recipe.currentRecipe.SetCraftingStation(86);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5648, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				225,
				20
			});
			Recipe.currentRecipe.SetCraftingStation(86);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5649, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				225,
				20
			});
			Recipe.currentRecipe.SetCraftingStation(86);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5650, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				225,
				20
			});
			Recipe.currentRecipe.SetCraftingStation(86);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(262, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3266, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(173, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(86, null);
			Recipe.currentRecipe.requiredItem[2].stack = 5;
			Recipe.currentRecipe.requiredTile = 77;
			Recipe.currentRecipe.corruption = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3267, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(173, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(86, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 77;
			Recipe.currentRecipe.corruption = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3268, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(173, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(86, null);
			Recipe.currentRecipe.requiredItem[2].stack = 5;
			Recipe.currentRecipe.requiredTile = 77;
			Recipe.currentRecipe.corruption = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3266, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(173, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1329, null);
			Recipe.currentRecipe.requiredItem[2].stack = 5;
			Recipe.currentRecipe.requiredTile = 77;
			Recipe.currentRecipe.crimson = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3267, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(173, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1329, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 77;
			Recipe.currentRecipe.crimson = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3268, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(173, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1329, null);
			Recipe.currentRecipe.requiredItem[2].stack = 5;
			Recipe.currentRecipe.requiredTile = 77;
			Recipe.currentRecipe.crimson = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1282, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(262, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(181, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1283, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(262, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(180, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1284, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(262, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(177, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1285, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(262, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(179, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1286, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(262, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(178, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1287, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(262, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(182, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4256, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(262, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(999, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4242, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3989,
				1,
				1050,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4243, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3989,
				1,
				1015,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4244, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3989,
				1,
				2874,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4245, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3989,
				1,
				1013,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4246, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3989,
				1,
				1011,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4247, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3989,
				1,
				1010,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4248, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3989,
				1,
				1008,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4249, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3989,
				1,
				1018,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4250, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3989,
				1,
				1016,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4251, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3989,
				1,
				1007,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4252, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3989,
				1,
				1014,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4253, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3989,
				1,
				1012,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4254, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3989,
				1,
				1017,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4255, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3989,
				1,
				1009,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3306, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(150, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3293, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3306, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1007, null);
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3294, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3306, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1008, null);
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3295, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3306, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1009, null);
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3296, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3306, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1010, null);
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3297, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3306, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1011, null);
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3298, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3306, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1012, null);
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3299, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3306, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1013, null);
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3300, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3306, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1014, null);
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3301, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3306, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1015, null);
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3302, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3306, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1016, null);
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3303, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3306, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1017, null);
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3304, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3306, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1018, null);
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3308, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3306, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3305, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3306, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2874, null);
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3307, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3306, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1066, null);
			Recipe.currentRecipe.SetCraftingStation(228);
			Recipe.AddRecipe();
			for (int l = 3309; l <= 3314; l++)
			{
				Recipe.currentRecipe.createItem.SetDefaults(5547, null);
				Recipe.currentRecipe.requiredItem[0].SetDefaults(3306, null);
				Recipe.currentRecipe.requiredItem[1].SetDefaults(l, null);
				Recipe.currentRecipe.requiredTile = 114;
				Recipe.AddRecipe();
			}
			Recipe.currentRecipe.createItem.SetDefaults(3366, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3334, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5547, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5541, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5540, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3366, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(259, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(68, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(252, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(259, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(253, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(259, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(978, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(803, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(981, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(979, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(804, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(981, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(980, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(805, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(981, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3365, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(129, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4075, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				22,
				4
			});
			Recipe.currentRecipe.SetCraftingStation(16);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4064, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				9,
				12
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4065, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				9,
				12,
				225,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4859, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4858, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(313, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4860, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4858, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(314, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4861, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4858, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(317, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4862, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4858, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2358, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4863, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4858, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(315, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4864, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4858, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(316, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4865, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4858, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(316, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4866, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4858, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(318, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4867, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4858, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4852, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(27, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(181, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4851, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(27, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(180, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4853, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(27, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(177, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4854, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(27, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(179, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4855, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(27, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(178, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4856, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(27, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(182, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4857, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(27, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(999, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4869, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4868, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3364, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(129, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[2].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(33, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[2].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Stone);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(360, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 50;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			for (int m = 0; m < 36; m++)
			{
				Recipe.currentRecipe.createItem.SetDefaults(2702 + m, null);
				Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
				Recipe.currentRecipe.requiredItem[0].stack = 50;
				Recipe.currentRecipe.requiredTile = 283;
				Recipe.AddRecipe();
			}
			Recipe.AddCritterStatueRecipe(444, 261, null);
			Recipe.AddCritterStatueRecipe(3653, 2002, null);
			Recipe.AddCritterStatueRecipe(3651, 0, RecipeGroups.Squirrels);
			Recipe.AddCritterStatueRecipe(3652, 0, RecipeGroups.Butterflies);
			Recipe.AddCritterStatueRecipe(3654, 0, RecipeGroups.Fireflies);
			Recipe.AddCritterStatueRecipe(3655, 0, RecipeGroups.Scorpions);
			Recipe.AddCritterStatueRecipe(3656, 0, RecipeGroups.Snails);
			Recipe.AddCritterStatueRecipe(3658, 2003, null);
			Recipe.AddCritterStatueRecipe(3659, 0, RecipeGroups.Ducks);
			Recipe.AddCritterStatueRecipe(3660, 2205, null);
			Recipe.AddCritterStatueRecipe(3661, 2121, null);
			Recipe.AddCritterStatueRecipe(3662, 0, RecipeGroups.Bugs);
			Recipe.AddCritterStatueRecipe(445, 2019, null);
			Recipe.AddCritterStatueRecipe(464, 0, RecipeGroups.Birds);
			Recipe.AddCritterStatueRecipe(3657, 2740, null);
			Recipe.AddCritterStatueRecipe(4342, 0, RecipeGroups.Dragonflies);
			Recipe.AddCritterStatueRecipe(4360, 4359, null);
			Recipe.AddCritterStatueRecipe(4397, 4395, null);
			Recipe.AddCritterStatueRecipe(4466, 0, RecipeGroups.Turtles);
			Recipe.AddCritterStatueRecipe(5317, 0, RecipeGroups.Macaws);
			Recipe.AddCritterStatueRecipe(5318, 5311, null);
			Recipe.AddCritterStatueRecipe(5319, 0, RecipeGroups.Cockatiels);
			Recipe.currentRecipe.createItem.SetDefaults(20, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(12, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3509, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(20, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.AddCustomShimmerResult(12, 1);
			Recipe.currentRecipe.AddCustomShimmerResult(9, 1);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3506, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(20, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.AddCustomShimmerResult(12, 1);
			Recipe.currentRecipe.AddCustomShimmerResult(9, 1);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3505, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(20, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3508, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(20, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3507, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(20, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.AddCustomShimmerResult(12, 1);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3504, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(20, null);
			Recipe.currentRecipe.requiredItem[0].stack = 7;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(739, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(20, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(181, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(89, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(20, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(80, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(20, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(76, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(20, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(15, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(20, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(106, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(20, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(703, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(699, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3503, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(703, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3500, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(703, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3499, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(703, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3502, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(703, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3501, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(703, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3498, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(703, null);
			Recipe.currentRecipe.requiredItem[0].stack = 7;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(740, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(703, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(180, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(687, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(703, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(688, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(703, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(689, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(703, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(707, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(703, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(710, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(703, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(11, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(35, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2291, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(10, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(7, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4711, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(6, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(99, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 7;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(90, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(81, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(77, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(704, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(700, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(716, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(704, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3497, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(704, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3494, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(704, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3493, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(704, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3496, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(704, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3495, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(704, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3492, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(704, null);
			Recipe.currentRecipe.requiredItem[0].stack = 7;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(690, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(704, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(691, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(704, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(692, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(704, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(205, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1128, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(205, null);
			Recipe.currentRecipe.requiredTile = 215;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5364, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3031, null);
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5304, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3032, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4872, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(5303, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1140, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1139, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(704, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2172, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2194, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(348, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(336, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(358, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4127, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				182,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4731, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(358, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1570, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2841, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(206, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(345, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(85, null);
			Recipe.currentRecipe.createItem.stack = 15;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5328, null);
			Recipe.currentRecipe.createItem.stack = 1;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(85, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4422, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(21, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(14, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3515, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(21, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3512, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(21, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3511, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(21, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3514, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(21, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3513, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(21, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3510, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(21, null);
			Recipe.currentRecipe.requiredItem[0].stack = 7;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(741, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(21, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(177, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(91, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(21, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(82, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(21, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(78, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(21, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(16, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(21, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(107, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(21, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(705, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(701, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3491, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(705, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3488, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(705, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3487, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(705, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3490, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(705, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3489, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(705, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3486, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(705, null);
			Recipe.currentRecipe.requiredItem[0].stack = 7;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(742, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(705, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(179, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(693, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(705, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(694, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(705, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(695, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(705, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(708, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(705, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(711, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(705, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(13, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3521, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3518, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3517, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3520, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3519, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3516, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[0].stack = 7;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(743, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(178, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(92, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(83, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(79, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(17, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(264, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(178, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(108, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(105, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(148, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(105, null);
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.currentRecipe.needWater = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3117, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3114, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5322, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(57, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.currentRecipe.corruption = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5322, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1257, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.currentRecipe.crimson = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(349, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(702, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3485, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3482, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3481, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3484, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3483, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3480, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[0].stack = 7;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(744, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(182, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(696, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(697, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(698, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(709, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(715, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(178, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(712, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(85, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(713, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(148, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(713, null);
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.currentRecipe.needWater = true;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3117, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3114, null);
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(714, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(8, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(355, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[1].stack = 30;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(355, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[1].stack = 30;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5473, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				150,
				30,
				9,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(86);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5456, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 300;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(194, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(86, null);
			Recipe.currentRecipe.requiredItem[2].stack = 5;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5456, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[0].stack = 300;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(194, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1329, null);
			Recipe.currentRecipe.requiredItem[2].stack = 5;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5068, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5070, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[2].stack = 8;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5068, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5070, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[2].stack = 8;
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5069, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5070, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5069, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5070, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(57, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(56, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2293, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(57, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(44, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(57, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(45, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(57, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(46, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(57, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(102, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(57, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(86, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(101, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(57, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(86, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(100, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(57, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(86, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(103, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(57, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(86, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(104, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(57, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(86, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3279, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(57, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5474, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(57, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5107, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(57, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(180, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.corruption = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1257, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(880, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2421, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1257, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(796, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1257, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(799, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1257, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(795, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1257, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(792, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1257, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1329, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(793, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1257, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1329, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(794, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1257, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1329, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(798, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1257, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1329, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(797, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1257, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1329, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(801, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1257, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1329, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3280, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1257, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5475, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1257, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5107, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1257, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(180, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.crimson = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(84, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(85, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(118, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1236, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(181, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1237, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(180, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1238, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(177, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1239, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(179, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1240, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(178, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1241, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(182, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4257, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(999, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1522, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(181, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1523, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(180, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1524, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(177, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1525, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(179, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1526, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(178, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1527, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(182, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3643, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(999, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3648, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(181, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3647, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(180, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3646, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(179, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3645, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(177, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3644, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(178, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3649, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(182, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3650, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(999, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(116, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(198, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(177, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(199, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(178, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(200, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(179, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(201, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(181, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(202, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(182, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(203, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(180, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4258, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(999, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5535, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4414, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5670, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				117,
				15,
				181,
				3,
				180,
				3,
				177,
				3,
				179,
				3,
				178,
				3,
				182,
				3,
				999,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(16);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3764, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(198, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(502, null);
			Recipe.currentRecipe.requiredItem[1].stack = 25;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3765, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(199, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(502, null);
			Recipe.currentRecipe.requiredItem[1].stack = 25;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3766, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(200, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(502, null);
			Recipe.currentRecipe.requiredItem[1].stack = 25;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3767, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(201, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(502, null);
			Recipe.currentRecipe.requiredItem[1].stack = 25;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3768, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(202, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(502, null);
			Recipe.currentRecipe.requiredItem[1].stack = 25;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3769, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(203, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(502, null);
			Recipe.currentRecipe.requiredItem[1].stack = 25;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4259, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4258, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(502, null);
			Recipe.currentRecipe.requiredItem[1].stack = 25;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5536, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5535, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(502, null);
			Recipe.currentRecipe.requiredItem[1].stack = 25;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5671, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5670, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(502, null);
			Recipe.currentRecipe.requiredItem[1].stack = 25;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(204, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(127, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(197, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(98, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(75, null);
			Recipe.currentRecipe.requiredItem[2].stack = 5;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5476, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(85, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(75, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(123, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(124, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(125, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5510, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3380,
				20,
				999,
				5,
				75,
				5
			});
			Recipe.currentRecipe.SetCraftingStation(16);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4059, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3380,
				12,
				9,
				4
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.SetCraftingStation(16);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3378, null);
			Recipe.currentRecipe.createItem.stack = 20;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3380, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3379, null);
			Recipe.currentRecipe.createItem.stack = 30;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3380, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3377, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3380, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(999, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3374, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3380, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3375, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3380, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3376, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3380, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5074, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 90;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(150, null);
			Recipe.currentRecipe.requiredItem[1].stack = 55;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(151, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 40;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(150, null);
			Recipe.currentRecipe.requiredItem[1].stack = 40;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(152, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 60;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(150, null);
			Recipe.currentRecipe.requiredItem[1].stack = 50;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(153, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 50;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(150, null);
			Recipe.currentRecipe.requiredItem[1].stack = 45;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(190, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(209, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(331, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(210, null);
			Recipe.currentRecipe.requiredItem[2].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(191, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(331, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(209, null);
			Recipe.currentRecipe.requiredItem[1].stack = 9;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(185, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(331, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(210, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5295, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(213, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3506, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(331, null);
			Recipe.currentRecipe.requiredItem[2].stack = 12;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(210, null);
			Recipe.currentRecipe.requiredItem[3].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5295, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(213, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3500, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(331, null);
			Recipe.currentRecipe.requiredItem[2].stack = 12;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(210, null);
			Recipe.currentRecipe.requiredItem[3].stack = 3;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3281, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(620, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(209, null);
			Recipe.currentRecipe.requiredItem[1].stack = 12;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(210, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(331, null);
			Recipe.currentRecipe.requiredItem[3].stack = 9;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4913, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(209, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(210, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(331, null);
			Recipe.currentRecipe.requiredItem[2].stack = 12;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(228, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(331, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(229, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(331, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(209, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(230, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(331, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(210, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2364, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2431, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2361, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2431, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2362, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2431, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2363, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2431, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5294, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2431, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(175, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(174, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(173, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 77;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(119, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(175, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(55, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(120, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(175, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(121, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(175, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(122, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(175, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(217, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(175, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(219, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(175, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(164, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2365, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(175, null);
			Recipe.currentRecipe.requiredItem[0].stack = 17;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(231, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(175, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(232, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(175, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(233, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(175, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4821, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(175, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1991, null);
			Recipe.currentRecipe.requiredTile = 77;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5129, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3484,
				1,
				5132,
				5
			});
			Recipe.currentRecipe.needHoney = true;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5129, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3520,
				1,
				5132,
				5
			});
			Recipe.currentRecipe.needHoney = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(273, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(46, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(155, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(190, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(121, null);
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.currentRecipe.corruption = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(273, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(795, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(155, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(190, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(121, null);
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.currentRecipe.crimson = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(675, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(273, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(547, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(548, null);
			Recipe.currentRecipe.requiredItem[2].stack = 20;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(549, null);
			Recipe.currentRecipe.requiredItem[3].stack = 20;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(674, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(368, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[1].stack = 24;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(757, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(675, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(674, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1570, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4956, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(757, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3063, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3065, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(2880, null);
			Recipe.currentRecipe.requiredItem[4].SetDefaults(1826, null);
			Recipe.currentRecipe.requiredItem[5].SetDefaults(3018, null);
			Recipe.currentRecipe.requiredItem[6].SetDefaults(65, null);
			Recipe.currentRecipe.requiredItem[7].SetDefaults(1123, null);
			Recipe.currentRecipe.requiredItem[8].SetDefaults(989, null);
			Recipe.currentRecipe.requiredItem[9].SetDefaults(3507, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(389, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(527, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(528, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(521, null);
			Recipe.currentRecipe.requiredItem[2].stack = 7;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(520, null);
			Recipe.currentRecipe.requiredItem[3].stack = 7;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5462, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				391,
				24,
				175,
				20
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.AdamantiteBar);
			Recipe.currentRecipe.SetCraftingStation(133);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(381, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(364, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(372, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(381, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(373, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(381, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(371, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(381, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(374, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(381, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(375, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(381, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(385, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(381, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(776, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(381, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(383, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(381, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(991, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(381, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(435, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(381, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(483, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(381, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(537, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(381, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1184, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1104, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1205, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1184, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1206, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1184, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1207, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1184, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1208, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1184, null);
			Recipe.currentRecipe.requiredItem[0].stack = 24;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1209, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1184, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1189, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1184, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1188, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1184, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1190, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1184, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1222, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1184, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1187, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1184, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1185, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1184, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1186, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1184, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(382, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(365, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(377, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(382, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(378, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(382, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(376, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(382, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(379, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(382, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(380, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(382, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(386, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(382, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(777, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(382, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(384, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(382, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(992, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(382, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(436, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(382, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(484, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(382, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(390, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(382, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(525, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(382, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1191, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1105, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1210, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1191, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1211, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1191, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1212, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1191, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1213, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1191, null);
			Recipe.currentRecipe.requiredItem[0].stack = 24;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1214, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1191, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1196, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1191, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1195, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1191, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1197, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1191, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1223, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1191, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1194, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1191, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1192, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1191, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1193, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1191, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1220, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1191, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(391, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(366, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 133;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(401, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(391, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(402, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(391, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(400, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(391, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(403, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(391, null);
			Recipe.currentRecipe.requiredItem[0].stack = 24;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(404, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(391, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(388, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(391, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(778, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(391, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(387, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(391, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(993, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(391, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(481, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(391, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(482, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(391, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(406, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(391, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(524, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(366, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(221, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1198, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1106, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 133;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1215, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1198, null);
			Recipe.currentRecipe.requiredItem[0].stack = 13;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1216, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1198, null);
			Recipe.currentRecipe.requiredItem[0].stack = 13;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1217, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1198, null);
			Recipe.currentRecipe.requiredItem[0].stack = 13;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1218, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1198, null);
			Recipe.currentRecipe.requiredItem[0].stack = 26;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1219, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1198, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1203, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1198, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1202, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1198, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1204, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1198, null);
			Recipe.currentRecipe.requiredItem[0].stack = 13;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1224, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1198, null);
			Recipe.currentRecipe.requiredItem[0].stack = 13;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1201, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1198, null);
			Recipe.currentRecipe.requiredItem[0].stack = 13;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1199, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1198, null);
			Recipe.currentRecipe.requiredItem[0].stack = 13;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1200, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1198, null);
			Recipe.currentRecipe.requiredItem[0].stack = 13;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1221, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1106, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(221, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5588, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				88,
				1,
				382,
				10
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.MythrilBar);
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5589, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				410,
				1,
				382,
				10
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.MythrilBar);
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5590, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				411,
				1,
				382,
				10
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.MythrilBar);
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5596, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				22,
				10,
				382,
				5
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.MythrilBar);
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5591, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2367,
				1,
				382,
				10
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.MythrilBar);
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5592, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2368,
				1,
				382,
				10
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.MythrilBar);
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5593, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2369,
				1,
				382,
				10
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.MythrilBar);
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2551, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2607, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2366, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2607, null);
			Recipe.currentRecipe.requiredItem[0].stack = 24;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2370, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2607, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2371, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2607, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2372, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2607, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(684, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				391,
				10,
				2161,
				1
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.AdamantiteBar);
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(685, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				391,
				20,
				2161,
				1
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.AdamantiteBar);
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(686, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				391,
				16,
				2161,
				1
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.AdamantiteBar);
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4911, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(520, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(521, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(2161, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3788, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(534, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(527, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(521, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3787, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(113, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(528, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(520, null);
			Recipe.currentRecipe.requiredItem[2].stack = 16;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3779, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3795, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3783, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(521, null);
			Recipe.currentRecipe.requiredItem[2].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3776, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				391,
				10,
				3783,
				1
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.AdamantiteBar);
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3777, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				391,
				20,
				3783,
				1
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.AdamantiteBar);
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3778, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				391,
				16,
				3783,
				1
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.AdamantiteBar);
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(559, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(553, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(558, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4873, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5660, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(551, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 24;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(552, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4896, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4897, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4898, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4899, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4900, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 24;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4901, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(579, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(547, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(548, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(549, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(990, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(547, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(548, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(549, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(578, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(368, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(550, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4790, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4678, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4060, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(197, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(947, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredTile = 133;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1002, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				1006,
				12
			});
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1001, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				1006,
				12
			});
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1003, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				1006,
				12
			});
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5524, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				1006,
				12
			});
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1004, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 24;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1005, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1231, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1230, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1232, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1233, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1262, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1234, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1229, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1227, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1226, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1228, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2188, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1308, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[1].stack = 14;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5296, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(997, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1316, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1328, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1317, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 24;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1328, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1318, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1328, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2199, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2218, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1316, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2200, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2218, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1317, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2201, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2218, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1317, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2202, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2218, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1318, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1552, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(183, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredTile = 247;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1546, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1552, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1547, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1552, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1548, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1552, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1549, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1552, null);
			Recipe.currentRecipe.requiredItem[0].stack = 24;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1550, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1552, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2176, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1552, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3261, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1508, null);
			Recipe.currentRecipe.requiredTile = 133;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2189, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3261, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1503, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3261, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1504, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3261, null);
			Recipe.currentRecipe.requiredItem[0].stack = 24;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1505, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3261, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1506, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3261, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1507, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3261, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1543, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3261, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1544, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3261, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1545, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3261, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3460, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3567, null);
			Recipe.currentRecipe.createItem.stack = 333;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3568, null);
			Recipe.currentRecipe.createItem.stack = 333;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4318, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3460, null);
			Recipe.currentRecipe.requiredItem[0].stack = 25;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3572, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredItem[2].stack = 6;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[3].stack = 6;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3539, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2763, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2764, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 16;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2765, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 12;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2786, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2784, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3522, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 12;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3473, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3543, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3536, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2757, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2758, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 16;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2759, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 12;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2776, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2774, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3523, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 12;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3475, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3540, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3537, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2760, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2761, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 16;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2762, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 12;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2781, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2779, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3524, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 12;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3476, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3542, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3459, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3538, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3381, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3382, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 16;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3383, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 12;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3466, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3464, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3525, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 12;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3474, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3531, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5479, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredItem[0].stack = 18;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(533, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(98, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(324, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(319, null);
			Recipe.currentRecipe.requiredItem[2].stack = 5;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(548, null);
			Recipe.currentRecipe.requiredItem[3].stack = 20;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(561, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(520, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(548, null);
			Recipe.currentRecipe.requiredItem[2].stack = 25;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(506, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(324, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(547, null);
			Recipe.currentRecipe.requiredItem[2].stack = 20;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2535, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(236, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(38, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[2].stack = 12;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(549, null);
			Recipe.currentRecipe.requiredItem[3].stack = 20;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(494, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(508, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(502, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(521, null);
			Recipe.currentRecipe.requiredItem[2].stack = 8;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(549, null);
			Recipe.currentRecipe.requiredItem[3].stack = 15;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(425, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(507, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(501, null);
			Recipe.currentRecipe.requiredItem[1].stack = 25;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(520, null);
			Recipe.currentRecipe.requiredItem[2].stack = 8;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(549, null);
			Recipe.currentRecipe.requiredItem[3].stack = 10;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2343, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5125, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2343,
				1,
				215,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(16);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5288, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5125,
				1,
				4731,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(16);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4468, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2343, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2218, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4451, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2343, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1522, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4452, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2343, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1523, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4453, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2343, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1524, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4454, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2343, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1525, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4455, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2343, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1526, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4456, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2343, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1527, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4467, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2343, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3643, null);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4745, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(68, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.currentRecipe.corruption = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4745, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(9, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1330, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.currentRecipe.crimson = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5289, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3354, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3355, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3356, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2768, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[0].stack = 40;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1006, null);
			Recipe.currentRecipe.requiredItem[1].stack = 40;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1552, null);
			Recipe.currentRecipe.requiredItem[2].stack = 40;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(3261, null);
			Recipe.currentRecipe.requiredItem[3].stack = 40;
			Recipe.currentRecipe.requiredItem[4].SetDefaults(175, null);
			Recipe.currentRecipe.requiredItem[4].stack = 40;
			Recipe.currentRecipe.requiredItem[5].SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[5].stack = 40;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5131, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4797,
				1,
				4960,
				1
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5513, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5511,
				5,
				2316,
				10
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(495, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(502, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(526, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(501, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(520, null);
			Recipe.currentRecipe.requiredItem[3].stack = 8;
			Recipe.currentRecipe.requiredItem[4].SetDefaults(549, null);
			Recipe.currentRecipe.requiredItem[4].stack = 15;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(493, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(320, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(575, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(520, null);
			Recipe.currentRecipe.requiredItem[2].stack = 15;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(492, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(320, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(575, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(521, null);
			Recipe.currentRecipe.requiredItem[2].stack = 15;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(761, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(575, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(501, null);
			Recipe.currentRecipe.requiredItem[1].stack = 99;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(785, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(575, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1516, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(749, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(575, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1611, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(786, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(575, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1517, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(821, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(575, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1518, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(822, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(575, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1519, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1165, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(575, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1520, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1515, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(575, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1521, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1797, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(575, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1811, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1830, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(575, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1831, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(823, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(575, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3261, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2280, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(575, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2218, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1866, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(575, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1552, null);
			Recipe.currentRecipe.requiredItem[1].stack = 18;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3468, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3469, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3470, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3471, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredItem[0].stack = 14;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5627, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5737,
				1,
				575,
				20,
				1225,
				12
			});
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(250, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(261, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(126, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4398, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4373, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(126, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2439, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2436, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(126, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2440, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2437, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(126, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2441, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2438, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(126, null);
			Recipe.AddRecipe();
			for (int n = 0; n < 8; n++)
			{
				Recipe.currentRecipe.createItem.SetDefaults(2178 + n, null);
				Recipe.currentRecipe.requiredItem[0].SetDefaults(1994 + n, null);
				Recipe.currentRecipe.requiredItem[0].stack = 1;
				Recipe.currentRecipe.requiredItem[1].SetDefaults(31, null);
				Recipe.AddRecipe();
			}
			for (int num2 = 4327; num2 <= 4332; num2++)
			{
				Recipe.currentRecipe.createItem.SetDefaults(num2, null);
				Recipe.currentRecipe.requiredItem[0].SetDefaults(num2 - 4327 + 4334, null);
				Recipe.currentRecipe.requiredItem[0].stack = 1;
				Recipe.currentRecipe.requiredItem[1].SetDefaults(31, null);
				Recipe.AddRecipe();
			}
			Recipe.currentRecipe.createItem.SetDefaults(5133, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5132, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4846, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4845, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4964, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4961, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4655, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4068, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4656, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4069, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4657, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(31, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4070, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 16;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2162, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2019, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2163, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2018, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3565, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3563, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2206, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2205, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2165, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2123, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2164, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2122, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2166, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2015, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2167, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2016, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2168, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2017, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5213, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5212, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5301, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5300, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5314, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5311, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5315, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5312, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5316, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5313, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2190, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2121, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2174, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2006, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2175, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2007, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2186, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2157, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2187, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2156, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2191, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2003, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4376, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4375, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2207, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2002, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4364, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4363, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2741, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2740, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4380, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4361, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4461, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4464, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4462, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4465, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4473, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4374, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4474, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4359, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4475, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4418, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4481, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4480, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5512, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5511, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4396, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4395, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4850, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4849, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4963, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2673, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4882, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4838, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4883, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4839, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4884, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4840, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4885, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4841, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4886, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4842, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4887, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4843, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4888, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4844, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4889, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4831, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4890, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4832, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4891, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4833, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4892, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4834, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4893, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4835, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4894, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4836, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4895, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4837, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4483, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4482, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4476, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4419, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4275, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4274, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(126, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3072, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2891, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(31, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4333, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4340, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(31, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3070, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2889, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3071, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2890, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3566, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3564, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3073, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2892, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3074, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2893, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3075, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2894, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3076, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2895, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4399, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4362, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3254, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3191, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3255, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3194, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3256, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3192, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3257, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2208, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3193, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1085, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1073, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1086, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1074, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1087, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1075, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1088, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1076, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1089, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1077, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1090, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1078, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1091, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1079, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1092, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1080, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1093, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1081, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1094, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1082, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1095, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1083, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1096, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1084, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1007, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1115, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1008, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1114, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1009, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1110, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1010, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1112, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1011, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1108, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1012, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1107, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1013, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1116, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1014, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1109, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1015, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1111, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1016, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1118, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1017, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1117, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1018, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1113, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1050, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1119, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1031, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1007, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1008, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1009, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1033, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1009, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1010, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1011, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1035, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1013, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1014, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1015, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1063, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1031, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1064, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1033, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1065, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1035, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1032, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1031, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1034, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1033, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1036, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1035, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3550, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1031, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3551, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1033, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3552, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1035, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1068, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1008, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1009, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1010, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1069, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1012, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1013, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1014, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1070, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1016, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1017, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1018, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1066, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1068, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1069, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1070, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1067, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1066, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1019, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1007, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1020, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1008, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1021, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1009, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1022, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1010, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1023, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1011, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1024, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1012, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1025, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1013, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1026, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1014, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1027, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1015, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1028, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1016, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1029, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1017, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1030, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1018, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2875, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2874, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1038, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1007, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1039, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1008, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1040, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1009, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1041, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1010, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1042, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1011, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1043, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1012, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1044, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1013, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1045, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1014, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1046, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1015, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1047, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1016, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1048, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1017, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1049, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1018, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2876, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2874, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1051, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1007, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1052, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1008, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1053, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1009, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1054, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1010, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1055, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1011, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1056, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1012, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1057, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1013, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1058, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1014, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1059, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1015, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1060, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1016, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1061, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1017, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1062, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1018, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2877, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2874, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3559, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3557, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1050, null);
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3558, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1037, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3562, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3561, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3111, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3535, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3533, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(502, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3526, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3528, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3527, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3529, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3530, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(126, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3467, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 228;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2750, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(117, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(501, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(520, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(394, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(187, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(268, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1860, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(394, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1303, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1861, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1860, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(950, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(395, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(17, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(18, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(393, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(395, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(709, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(18, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(393, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3036, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3120, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3037, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3096, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3121, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3102, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3099, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3119, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3122, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3084, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3118, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3095, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3123, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(395, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3036, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3121, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(3122, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(50, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(19, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(182, null);
			Recipe.currentRecipe.requiredItem[2].stack = 3;
			Recipe.currentRecipe.SetCraftingStation(17);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(50, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(170, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(706, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(182, null);
			Recipe.currentRecipe.requiredItem[2].stack = 3;
			Recipe.currentRecipe.SetCraftingStation(17);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3124, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3123, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(50, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3124, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3123, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3199, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5437, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3124, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4263, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(4819, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4000, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				555,
				1,
				2219,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4001, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				555,
				1,
				532,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3996, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2423,
				1,
				976,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3994, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2423,
				1,
				187,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3995, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3994,
				1,
				976,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3995, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3996,
				1,
				187,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3997, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				938,
				1,
				1253,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3998, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				938,
				1,
				3016,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4008, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				88,
				1,
				3109,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3992, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				897,
				1,
				3016,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4006, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				1321,
				1,
				3015,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4005, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				1858,
				1,
				3015,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3991, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				555,
				1,
				3015,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4007, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3212,
				1,
				1132,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3990, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3200,
				1,
				2423,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4002, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				1321,
				1,
				1322,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3993, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				405,
				1,
				3017,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(396, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(158, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(193, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(397, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(156, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(193, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1613, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(397, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1612, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1724, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(53, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(215, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(399, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(53, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(159, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1163, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(987, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(159, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(983, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(857, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(159, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1863, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1724, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(159, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3241, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3201, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3225, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1250, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(399, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(158, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1251, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1163, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(158, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1252, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(983, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(158, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3250, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1863, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(158, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3251, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1249, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(158, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3252, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3241, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(158, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1164, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(399, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1163, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(983, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5331, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1164, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(158, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5331, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(399, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1163, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(983, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(158, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5331, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1250, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1163, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(983, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.BlizzardBalloons);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.SandstormBalloons);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5331, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1251, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(399, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(983, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.CloudBalloons);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.SandstormBalloons);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5331, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1252, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(399, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1163, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.CloudBalloons);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.BlizzardBalloons);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1249, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(159, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1132, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(857, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(53, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3783, null);
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(987, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(53, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2161, null);
			Recipe.currentRecipe.requiredTile = 125;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(405, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(54, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(128, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(405, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1579, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(128, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(405, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3200, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(128, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(405, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4055, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(128, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(898, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(405, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(212, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(285, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1862, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(898, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(950, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5000, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1862, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(908, null);
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(193, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(173, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3999, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				193,
				1,
				906,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4004, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				193,
				1,
				1323,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4003, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4004,
				1,
				906,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4003, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3999,
				1,
				1323,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4003, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3999,
				1,
				4004,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4038, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				906,
				1,
				193,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(907, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(863, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(193, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(908, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(907, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(906, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1323, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(908, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4038,
				1,
				863,
				1,
				1323,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(908, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4038,
				1,
				907,
				1,
				1323,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(908, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4003,
				1,
				907,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(908, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4003,
				1,
				863,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4874, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4822,
				1,
				405,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(555, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(223, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(189, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3034, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3033, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(855, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3035, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3034, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(854, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(897, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(536, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(211, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(936, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(897, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(935, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1343, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1322, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(936, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1864, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1845, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1167, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(982, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(111, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(49, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1595, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(982, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(216, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2221, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2219, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1595, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2220, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2219, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(935, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(860, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(49, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(535, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1865, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(899, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(900, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(861, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(485, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(497, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3110, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1865, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(861, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(862, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(532, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(554, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1247, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1132, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(532, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1578, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1132, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1290, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(976, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(953, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(975, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(984, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(976, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(977, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(963, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3061, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2214, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2215, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(2216, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(2217, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5126, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3061, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4056, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(5010, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(4341, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3721, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2373, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2375, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(2374, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5064, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3721, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4881, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5140, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5139, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(75, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5144, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5140, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4389, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5142, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5140, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4377, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5141, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5140, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4354, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5146, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5140, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5128, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5145, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5140, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(5127, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5143, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5140, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4378, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(901, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(886, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(892, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(902, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(887, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(885, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(903, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(889, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(893, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(904, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(890, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(891, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5354, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(888, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3781, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1612, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(901, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(902, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(903, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(904, null);
			Recipe.currentRecipe.requiredItem[4].SetDefaults(5354, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(935, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(490, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(548, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(549, null);
			Recipe.currentRecipe.requiredItem[2].stack = 5;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(547, null);
			Recipe.currentRecipe.requiredItem[3].stack = 5;
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(935, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(491, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(548, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(549, null);
			Recipe.currentRecipe.requiredItem[2].stack = 5;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(547, null);
			Recipe.currentRecipe.requiredItem[3].stack = 5;
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(935, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(489, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(548, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(549, null);
			Recipe.currentRecipe.requiredItem[2].stack = 5;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(547, null);
			Recipe.currentRecipe.requiredItem[3].stack = 5;
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(935, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2998, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(548, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(549, null);
			Recipe.currentRecipe.requiredItem[2].stack = 5;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(547, null);
			Recipe.currentRecipe.requiredItem[3].stack = 5;
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1301, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(935, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1248, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1858, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1300, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1301, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(518, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(531, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(502, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(520, null);
			Recipe.currentRecipe.requiredItem[2].stack = 15;
			Recipe.currentRecipe.requiredTile = 101;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(519, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(531, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(522, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(521, null);
			Recipe.currentRecipe.requiredItem[2].stack = 15;
			Recipe.currentRecipe.requiredTile = 101;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1336, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(531, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1332, null);
			Recipe.currentRecipe.requiredItem[1].stack = 20;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(521, null);
			Recipe.currentRecipe.requiredItem[2].stack = 15;
			Recipe.currentRecipe.requiredTile = 101;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(37, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(38, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(266, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(324, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(323, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(180, null);
			Recipe.currentRecipe.requiredItem[2].stack = 5;
			Recipe.currentRecipe.requiredTile = 17;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(237, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(236, null);
			Recipe.currentRecipe.requiredItem[0].stack = 2;
			Recipe.currentRecipe.requiredTile = 18;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(109, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(75, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3625, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(509, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(851, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(850, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(3612, null);
			Recipe.currentRecipe.requiredItem[4].SetDefaults(510, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3611, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3625, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3619, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(2799, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[3].stack = 60;
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3620, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(849, null);
			Recipe.currentRecipe.requiredItem[0].stack = 50;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 10;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(511, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(530, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(512, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(26, null);
			Recipe.currentRecipe.requiredItem[0].stack = 4;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(530, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3617, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(171, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 4;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[2].stack = 4;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(581, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(582, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[0].stack = 10;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(583, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(17, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(583, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(709, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(584, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(16, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(584, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(708, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(585, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(15, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(585, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(707, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 16;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3632, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(542, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.PressurePlate);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3630, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(542, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.PressurePlate);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3626, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(542, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.PressurePlate);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3631, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(542, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.PressurePlate);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3613, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(520, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3614, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(521, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3615, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(549, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3726, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3182, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3727, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3184, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3728, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3185, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3729, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1344, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3182, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3184, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(3185, null);
			Recipe.currentRecipe.requiredItem[3].stack = 1;
			Recipe.currentRecipe.requiredItem[4].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[4].stack = 1;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5135, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(539, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(2607, null);
			Recipe.currentRecipe.requiredItem[1].stack = 2;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(580, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(167, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(530, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5327, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(167, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(343, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(540, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5383, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				540,
				1,
				3111,
				5
			});
			Recipe.currentRecipe.SetCraftingStation(283);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5516, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5395,
				20
			});
			Recipe.currentRecipe.SetCraftingStation(283);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5520, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				540,
				1,
				4825,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(283);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5521, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				540,
				1,
				150,
				200
			});
			Recipe.currentRecipe.SetCraftingStation(283);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5522, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				540,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(283);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5514, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				540,
				1,
				75,
				50
			});
			Recipe.currentRecipe.SetCraftingStation(125);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5384, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				540,
				1,
				29,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(283);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4390, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(276, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5066, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1124, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5067, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3271, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(323, null);
			Recipe.currentRecipe.requiredItem[1].stack = 1;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5471, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(150, null);
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(150, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5471, null);
			Recipe.currentRecipe.requiredTile = 86;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5467, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(61, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(57, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(86, null);
			Recipe.currentRecipe.requiredItem[2].stack = 3;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5468, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(836, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1257, null);
			Recipe.currentRecipe.requiredItem[1].stack = 3;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1329, null);
			Recipe.currentRecipe.requiredItem[2].stack = 3;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5469, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(57, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(86, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5470, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1257, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1329, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5286, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				29,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(283);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5287, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				109,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(283);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5320, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				28,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(283);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5321, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				110,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(283);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5345, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4392, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(530, null);
			Recipe.currentRecipe.requiredItem[1].stack = 6;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(129, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3393, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3391,
				1,
				3392,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(86);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3391, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3392,
				1,
				3393,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(86);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3392, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3391,
				1,
				3393,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(86);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4391, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(664, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1290, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				111,
				1,
				29,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(111, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				1290,
				1,
				109,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(114);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2193, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4142,
				1,
				521,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4142, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				2193,
				1,
				521,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4355, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[0].stack = 50;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(540, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4640, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(181, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4641, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(180, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4642, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(177, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4643, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(179, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4644, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(178, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4645, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(182, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4646, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(999, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3, null);
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(565, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(562, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(563, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(564, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(566, null);
			Recipe.currentRecipe.requiredItem[4].SetDefaults(567, null);
			Recipe.currentRecipe.requiredItem[5].SetDefaults(568, null);
			Recipe.currentRecipe.requiredItem[6].SetDefaults(569, null);
			Recipe.currentRecipe.requiredItem[7].SetDefaults(570, null);
			Recipe.currentRecipe.requiredItem[8].SetDefaults(571, null);
			Recipe.currentRecipe.requiredItem[9].SetDefaults(572, null);
			Recipe.currentRecipe.requiredItem[10].SetDefaults(573, null);
			Recipe.currentRecipe.requiredItem[11].SetDefaults(574, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4356, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(4078, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(4080, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(4081, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(4082, null);
			Recipe.currentRecipe.requiredItem[4].SetDefaults(4357, null);
			Recipe.currentRecipe.requiredItem[5].SetDefaults(4358, null);
			Recipe.currentRecipe.requiredItem[6].SetDefaults(4421, null);
			Recipe.currentRecipe.requiredItem[7].SetDefaults(4606, null);
			Recipe.currentRecipe.requiredItem[8].SetDefaults(5006, null);
			Recipe.currentRecipe.requiredItem[9].SetDefaults(4979, null);
			Recipe.currentRecipe.requiredItem[10].SetDefaults(4985, null);
			Recipe.currentRecipe.requiredItem[11].SetDefaults(4990, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4992, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1603, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1602, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(4079, null);
			Recipe.currentRecipe.requiredItem[3].SetDefaults(4077, null);
			Recipe.currentRecipe.requiredItem[4].SetDefaults(1607, null);
			Recipe.currentRecipe.requiredItem[5].SetDefaults(4991, null);
			Recipe.currentRecipe.requiredTile = 114;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4237, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				562,
				1,
				2860,
				25
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(43, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(38, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5120, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5070, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(56, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(38, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.currentRecipe.corruption = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5120, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5070, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(880, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(38, null);
			Recipe.currentRecipe.requiredItem[2].stack = 1;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.currentRecipe.crimson = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4131, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(331, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(86, null);
			Recipe.currentRecipe.requiredItem[2].stack = 30;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.currentRecipe.corruption = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4131, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(331, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1329, null);
			Recipe.currentRecipe.requiredItem[2].stack = 30;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.currentRecipe.crimson = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4076, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(331, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1329, null);
			Recipe.currentRecipe.requiredItem[2].stack = 15;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.currentRecipe.crimson = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4076, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 15;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(331, null);
			Recipe.currentRecipe.requiredItem[1].stack = 8;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(86, null);
			Recipe.currentRecipe.requiredItem[2].stack = 15;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.currentRecipe.corruption = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(70, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(67, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(68, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1331, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2886, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1330, null);
			Recipe.currentRecipe.requiredItem[1].stack = 15;
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1133, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1125, null);
			Recipe.currentRecipe.requiredItem[0].stack = 5;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(209, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1124, null);
			Recipe.currentRecipe.requiredItem[2].stack = 5;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(1134, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(560, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(23, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(264, null);
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(560, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(23, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(715, null);
			Recipe.currentRecipe.requiredTile = 26;
			Recipe.currentRecipe.notDecraftable = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(544, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(38, null);
			Recipe.currentRecipe.requiredItem[0].stack = 3;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(520, null);
			Recipe.currentRecipe.requiredItem[2].stack = 6;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(556, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(68, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(521, null);
			Recipe.currentRecipe.requiredItem[2].stack = 6;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.corruption = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(556, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1330, null);
			Recipe.currentRecipe.requiredItem[0].stack = 6;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(521, null);
			Recipe.currentRecipe.requiredItem[2].stack = 6;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.crimson = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(557, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(154, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(22, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(520, null);
			Recipe.currentRecipe.requiredItem[2].stack = 3;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(521, null);
			Recipe.currentRecipe.requiredItem[3].stack = 3;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5334, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(544, null);
			Recipe.currentRecipe.requiredItem[1].SetDefaults(557, null);
			Recipe.currentRecipe.requiredItem[2].SetDefaults(556, null);
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.currentRecipe.needMechdusa = true;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1844, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(1725, null);
			Recipe.currentRecipe.requiredItem[0].stack = 30;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1508, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(1225, null);
			Recipe.currentRecipe.requiredItem[2].stack = 10;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(1958, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(225, null);
			Recipe.currentRecipe.requiredItem[0].stack = 20;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(1508, null);
			Recipe.currentRecipe.requiredItem[1].stack = 5;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(547, null);
			Recipe.currentRecipe.requiredItem[2].stack = 5;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(2767, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(2766, null);
			Recipe.currentRecipe.requiredItem[0].stack = 8;
			Recipe.currentRecipe.requiredTile = 134;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3601, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(3458, null);
			Recipe.currentRecipe.requiredItem[0].stack = 12;
			Recipe.currentRecipe.requiredItem[1].SetDefaults(3456, null);
			Recipe.currentRecipe.requiredItem[1].stack = 12;
			Recipe.currentRecipe.requiredItem[2].SetDefaults(3457, null);
			Recipe.currentRecipe.requiredItem[2].stack = 12;
			Recipe.currentRecipe.requiredItem[3].SetDefaults(3459, null);
			Recipe.currentRecipe.requiredItem[3].stack = 12;
			Recipe.currentRecipe.requiredTile = 412;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5104, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5105, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5104, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5106, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5105, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(5106, null);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5644, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				50,
				1,
				38,
				2,
				2997,
				4
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.MagicMirror);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5659, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5661,
				1,
				575,
				20,
				1225,
				12
			});
			Recipe.currentRecipe.SetCraftingStation(134);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(71, null);
			Recipe.currentRecipe.createItem.stack = 100;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(72, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(72, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(71, null);
			Recipe.currentRecipe.requiredItem[0].stack = 100;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(72, null);
			Recipe.currentRecipe.createItem.stack = 100;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(73, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(73, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(72, null);
			Recipe.currentRecipe.requiredItem[0].stack = 100;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(73, null);
			Recipe.currentRecipe.createItem.stack = 100;
			Recipe.currentRecipe.requiredItem[0].SetDefaults(74, null);
			Recipe.currentRecipe.requiredItem[0].stack = 1;
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(74, null);
			Recipe.currentRecipe.requiredItem[0].SetDefaults(73, null);
			Recipe.currentRecipe.requiredItem[0].stack = 100;
			Recipe.AddRecipe();
			Recipe.CreateReverseWallRecipes();
			Recipe.CreateReversePlatformRecipes();
			Recipe.CreateRequiredItemQuickLookups();
			Recipe.UpdateWhichItemsAreMaterials();
			Recipe.UpdateWhichItemsAreCrafted();
			Recipe.UpdateInheritedTilesUsedInRecipes();
			Recipe.UpdateMaterialFieldForAllRecipes();
			foreach (RecipeGroup recipeGroup in RecipeGroup.recipeGroups.Values)
			{
				recipeGroup.SortDecraftingEntries();
			}
			ShimmerTransforms.UpdateRecipeSets();
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00167540 File Offset: 0x00165740
		private static void CreateRequiredItemQuickLookups()
		{
			for (int i = 0; i < Recipe.numRecipes; i++)
			{
				Recipe recipe = Main.recipe[i];
				for (int j = 0; j < Recipe.maxRequirements; j++)
				{
					Item item = recipe.requiredItem[j];
					if (item.IsAir)
					{
						break;
					}
					Recipe.RequiredItemEntry requiredItemEntry = new Recipe.RequiredItemEntry(item.type, item.stack);
					foreach (int num in recipe.acceptedGroups)
					{
						if (num < 0)
						{
							break;
						}
						RecipeGroup recipeGroup = RecipeGroup.recipeGroups[num];
						if (recipeGroup.ValidItems.Contains(item.type))
						{
							requiredItemEntry.itemIdOrRecipeGroup = recipeGroup.GetGroupFakeItemId();
						}
					}
					recipe.requiredItemQuickLookup[j] = requiredItemEntry;
				}
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00167610 File Offset: 0x00165810
		private static void UpdateMaterialFieldForAllRecipes()
		{
			for (int i = 0; i < Recipe.numRecipes; i++)
			{
				int num = 0;
				while (Main.recipe[i].requiredItem[num].type > 0)
				{
					Main.recipe[i].requiredItem[num].Refresh(false);
					num++;
				}
				Main.recipe[i].createItem.Refresh(false);
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00167674 File Offset: 0x00165874
		public static void UpdateWhichItemsAreMaterials()
		{
			bool[] array = new bool[RecipeGroup.nextRecipeGroupIndex];
			for (int i = 0; i < Recipe.numRecipes; i++)
			{
				Recipe recipe = Main.recipe[i];
				for (int j = 0; j < Recipe.maxRequirements; j++)
				{
					Recipe.RequiredItemEntry requiredItemEntry = recipe.requiredItemQuickLookup[j];
					if (requiredItemEntry.itemIdOrRecipeGroup == 0)
					{
						break;
					}
					if (requiredItemEntry.IsRecipeGroup)
					{
						array[requiredItemEntry.RecipeGroup.RegisteredId] = true;
					}
					else
					{
						ItemID.Sets.IsAMaterial[requiredItemEntry.itemIdOrRecipeGroup] = true;
					}
				}
			}
			for (int k = 0; k < RecipeGroup.nextRecipeGroupIndex; k++)
			{
				if (array[k])
				{
					foreach (int num in RecipeGroup.recipeGroups[k].Items)
					{
						ItemID.Sets.IsAMaterial[num] = true;
					}
				}
			}
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00167764 File Offset: 0x00165964
		public static void UpdateWhichItemsAreCrafted()
		{
			for (int i = 0; i < Recipe.numRecipes; i++)
			{
				if (!Main.recipe[i].notDecraftable)
				{
					ItemID.Sets.IsCrafted[Main.recipe[i].createItem.type] = i;
				}
				if (Main.recipe[i].crimson)
				{
					ItemID.Sets.IsCraftedCrimson[Main.recipe[i].createItem.type] = i;
				}
				if (Main.recipe[i].corruption)
				{
					ItemID.Sets.IsCraftedCorruption[Main.recipe[i].createItem.type] = i;
				}
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x001677F4 File Offset: 0x001659F4
		private static void AddSolarFurniture()
		{
			Recipe.currentRecipe.createItem.SetDefaults(4229, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3,
				10,
				3458,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4233, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4145, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				14
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4146, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				15,
				225,
				5
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4147, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				20,
				149,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4148, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				16
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4149, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				5,
				8,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4150, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				4,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4151, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				4
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4152, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				4,
				8,
				4,
				85,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4153, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				8,
				22,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4154, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				10,
				22,
				3,
				170,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4155, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4156, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				8,
				1,
				4229,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4157, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				6,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4158, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				15,
				154,
				4,
				149,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4160, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				6,
				206,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4161, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				5,
				225,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4162, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				8
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4163, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4165, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4229,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00167DDC File Offset: 0x00165FDC
		private static void AddVortexFurniture()
		{
			Recipe.currentRecipe.createItem.SetDefaults(4230, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3,
				10,
				3456,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4234, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4166, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				14
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4167, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				15,
				225,
				5
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4168, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				20,
				149,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4169, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				16
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4170, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				5,
				8,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4171, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				4,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4172, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				4
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4173, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				4,
				8,
				4,
				85,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4174, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				8,
				22,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4175, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				10,
				22,
				3,
				170,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4176, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4177, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				8,
				1,
				4230,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4178, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				6,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4179, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				15,
				154,
				4,
				149,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4181, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				6,
				206,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4182, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				5,
				225,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4183, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				8
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4184, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4186, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4230,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x001683C4 File Offset: 0x001665C4
		private static void AddNebulaFurniture()
		{
			Recipe.currentRecipe.createItem.SetDefaults(4231, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3,
				10,
				3457,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4235, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4187, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				14
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4188, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				15,
				225,
				5
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4189, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				20,
				149,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4190, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				16
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4191, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				5,
				8,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4192, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				4,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4193, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				4
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4194, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				4,
				8,
				4,
				85,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4195, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				8,
				22,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4196, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				10,
				22,
				3,
				170,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4197, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4198, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				8,
				1,
				4231,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4199, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				6,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4200, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				15,
				154,
				4,
				149,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4202, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				6,
				206,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4203, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				5,
				225,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4204, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				8
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4205, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4207, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4231,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x001689AC File Offset: 0x00166BAC
		private static void AddStardustFurniture()
		{
			Recipe.currentRecipe.createItem.SetDefaults(4232, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3,
				10,
				3459,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4236, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4208, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				14
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4209, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				15,
				225,
				5
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4210, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				20,
				149,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4211, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				16
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4212, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				5,
				8,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4213, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				4,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4214, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				4
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4215, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				4,
				8,
				4,
				85,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4216, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				8,
				22,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4217, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				10,
				22,
				3,
				170,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4218, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4219, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				8,
				1,
				4232,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4220, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				6,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4221, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				15,
				154,
				4,
				149,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4223, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				6,
				206,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4224, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				5,
				225,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4225, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				8
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4226, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4228, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4232,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(412);
			Recipe.AddRecipe();
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00168F94 File Offset: 0x00167194
		private static void AddSpiderFurniture()
		{
			Recipe.currentRecipe.createItem.SetDefaults(4139, null);
			Recipe.currentRecipe.createItem.stack = 10;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				150,
				10,
				2607,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4140, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3931, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				14
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3932, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				15,
				225,
				5
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3933, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				20,
				149,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3934, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				16
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3935, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				5,
				8,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3936, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				4,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3937, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				4
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3938, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				4,
				8,
				4,
				85,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(16);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3939, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				8,
				22,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3940, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				10,
				22,
				3,
				170,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3941, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3942, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				8,
				1,
				4139,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3943, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				6,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3944, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				15,
				154,
				4,
				149,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3946, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				6,
				206,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3947, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				5,
				225,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3948, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				8
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3949, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				10
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4125, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4139,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00169534 File Offset: 0x00167734
		private static void AddLesionFurniture()
		{
			int num = 3955;
			Recipe.currentRecipe.createItem.SetDefaults(3955, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				61,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(218);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3975, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				num,
				10
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3956, null);
			Recipe.currentRecipe.createItem.stack = 4;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3955,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3967, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				num,
				6
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3963, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				num,
				4
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3965, null);
			Recipe recipe = Recipe.currentRecipe;
			int[] array = new int[]
			{
				0,
				8,
				22,
				2
			};
			array[0] = num;
			recipe.SetIngredients(array);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3974, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				num,
				8
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3972, null);
			Recipe recipe2 = Recipe.currentRecipe;
			int[] array2 = new int[]
			{
				0,
				6,
				206,
				1
			};
			array2[0] = num;
			recipe2.SetIngredients(array2);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3970, null);
			Recipe recipe3 = Recipe.currentRecipe;
			int[] array3 = new int[]
			{
				0,
				6,
				8,
				1
			};
			array3[0] = num;
			recipe3.SetIngredients(array3);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3962, null);
			Recipe recipe4 = Recipe.currentRecipe;
			int[] array4 = new int[]
			{
				0,
				4,
				8,
				1
			};
			array4[0] = num;
			recipe4.SetIngredients(array4);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3969, null);
			Recipe recipe5 = Recipe.currentRecipe;
			int[] array5 = new int[]
			{
				0,
				3,
				8,
				1
			};
			array5[0] = num;
			recipe5.SetIngredients(array5);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3961, null);
			Recipe recipe6 = Recipe.currentRecipe;
			int[] array6 = new int[]
			{
				0,
				5,
				8,
				3
			};
			array6[0] = num;
			recipe6.SetIngredients(array6);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3959, null);
			Recipe recipe7 = Recipe.currentRecipe;
			int[] array7 = new int[]
			{
				0,
				15,
				225,
				5
			};
			array7[0] = num;
			recipe7.SetIngredients(array7);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3968, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				num,
				16
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3960, null);
			Recipe recipe8 = Recipe.currentRecipe;
			int[] array8 = new int[]
			{
				0,
				20,
				149,
				10
			};
			array8[0] = num;
			recipe8.SetIngredients(array8);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3966, null);
			Recipe recipe9 = Recipe.currentRecipe;
			int[] array9 = new int[]
			{
				22,
				3,
				170,
				6,
				0,
				10
			};
			array9[4] = num;
			recipe9.SetIngredients(array9);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3973, null);
			Recipe recipe10 = Recipe.currentRecipe;
			int[] array10 = new int[]
			{
				0,
				5,
				225,
				2
			};
			array10[0] = num;
			recipe10.SetIngredients(array10);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3971, null);
			Recipe recipe11 = Recipe.currentRecipe;
			int[] array11 = new int[]
			{
				154,
				4,
				0,
				15,
				149,
				1
			};
			array11[2] = num;
			recipe11.SetIngredients(array11);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3958, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				num,
				14
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(3964, null);
			Recipe recipe12 = Recipe.currentRecipe;
			int[] array12 = new int[]
			{
				0,
				4,
				8,
				4,
				85,
				1
			};
			array12[0] = num;
			recipe12.SetIngredients(array12);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Wood);
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4126, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3955,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(499);
			Recipe.AddRecipe();
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00169C38 File Offset: 0x00167E38
		private static void AddSandstoneFurniture()
		{
			Recipe.currentRecipe.createItem.SetDefaults(4720, null);
			Recipe.currentRecipe.createItem.stack = 2;
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4298, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				14
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4299, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				15,
				225,
				5
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4300, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				20,
				149,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4301, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				16
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4302, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				5,
				8,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4303, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				4,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4304, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				4
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4305, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				4,
				8,
				4,
				85,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(16);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4267, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				8,
				22,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4306, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				10,
				22,
				3,
				170,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4307, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4308, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				8,
				1,
				4051,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4309, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				6,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4310, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				15,
				154,
				4,
				149,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4312, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				6,
				206,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4313, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				5,
				225,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4314, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				8
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4315, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				10
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4316, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4051,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0016A188 File Offset: 0x00168388
		private static void AddBambooFurniture()
		{
			Recipe.currentRecipe.createItem.SetDefaults(4566, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				14
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4567, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				15,
				225,
				5
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4568, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				20,
				149,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4569, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				16
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4570, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				5,
				8,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4571, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				4,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4572, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				4
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4573, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				4,
				8,
				4,
				85,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(16);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4574, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				8,
				22,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4575, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				10,
				22,
				3,
				170,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4576, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4577, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				8,
				1,
				4564,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4578, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				6,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4579, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				15,
				154,
				4,
				149,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4581, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				6,
				206,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4582, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				5,
				225,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4583, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				8
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4584, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				10
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(4586, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				4564,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0016A684 File Offset: 0x00168884
		private static void AddCoralFurniture()
		{
			Recipe.currentRecipe.createItem.SetDefaults(5148, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				14
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5149, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				15,
				225,
				5
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5150, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				20,
				149,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5151, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				16
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5152, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				5,
				8,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5153, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				4,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5154, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				4
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5155, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				4,
				8,
				4,
				85,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(16);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5156, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				8,
				22,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5157, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				10,
				22,
				3,
				170,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5158, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5159, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				8,
				1,
				5306,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5160, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				6,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5161, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				15,
				154,
				4,
				149,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5163, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				6,
				206,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5164, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				5,
				225,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5165, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				8
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5166, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				10
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5168, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5306,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0016AB80 File Offset: 0x00168D80
		private static void AddBalloonFurniture()
		{
			Recipe.currentRecipe.createItem.SetDefaults(5169, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				14
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5170, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				15,
				225,
				5
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5171, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				20,
				149,
				10
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5172, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				16
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5173, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				5,
				8,
				3
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5174, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				4,
				8,
				1
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5175, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				4
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5176, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				4,
				8,
				4,
				85,
				1
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(16);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5177, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				8,
				22,
				2
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5178, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				10,
				22,
				3,
				170,
				6
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5179, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				6
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5180, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				8,
				1,
				3738,
				3
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5181, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				6,
				8,
				1
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5182, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				15,
				154,
				4,
				149,
				1
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5184, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				6,
				206,
				1
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5185, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				5,
				225,
				2
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5186, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				8
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5187, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				10
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5189, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				3738,
				6
			});
			Recipe.currentRecipe.RequireGroup(RecipeGroups.Balloons);
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0016B19C File Offset: 0x0016939C
		private static void AddAshWoodFurnitureArmorAndItems()
		{
			Recipe.currentRecipe.createItem.SetDefaults(5279, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				20
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5280, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				30
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5281, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				25
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5284, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				7
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5283, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				8
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5282, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5190, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				14
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5191, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				15,
				225,
				5
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5192, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				20,
				149,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5193, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				16
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5194, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				5,
				8,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5195, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				4,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5196, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				4
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5197, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				4,
				8,
				4,
				85,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(16);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5198, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				8,
				22,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5199, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				10,
				22,
				3,
				170,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5200, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5201, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				8,
				1,
				5215,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5202, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				6,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5203, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				15,
				154,
				4,
				149,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5205, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				6,
				206,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5206, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				5,
				225,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5207, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				8
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5208, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				10
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5210, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5215,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0016B828 File Offset: 0x00169A28
		private static void AddAetheriumFurniture()
		{
			Recipe.currentRecipe.createItem.SetDefaults(5548, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				14
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5549, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				15,
				225,
				5
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5550, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				20,
				149,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5551, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				16
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5552, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				5,
				8,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5553, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				4,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5554, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				4
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5555, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				4,
				8,
				4,
				85,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(16);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5556, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				8,
				22,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5557, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				10,
				22,
				3,
				170,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5558, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5559, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				8,
				1,
				5398,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5560, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				6,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5561, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				15,
				154,
				4,
				149,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5563, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				6,
				206,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5564, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				5,
				225,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5565, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				8
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5566, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				10
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5568, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5398,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0016BD24 File Offset: 0x00169F24
		private static void AddFallenStarFurniture()
		{
			Recipe.currentRecipe.createItem.SetDefaults(5601, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				14
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5602, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				15,
				225,
				5
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5603, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				20,
				149,
				10
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5604, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				16
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5605, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				5,
				8,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5606, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				4,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5607, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				4
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5608, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				4,
				8,
				4,
				85,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(16);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5609, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				8,
				22,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5610, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				10,
				22,
				3,
				170,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5611, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5612, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				8,
				1,
				5622,
				3
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5613, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				6,
				8,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5614, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				15,
				154,
				4,
				149,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5616, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				6,
				206,
				1
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5617, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				5,
				225,
				2
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5618, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				8
			});
			Recipe.currentRecipe.SetCraftingStation(18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5619, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				10
			});
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(5621, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				5622,
				6
			});
			Recipe.currentRecipe.SetCraftingStation(106);
			Recipe.AddRecipe();
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0016C220 File Offset: 0x0016A420
		private static void AddFeywoodFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(5710, 5689, 5690, 5691, 5693, 5694, 5695, 5696, 5697, 5698, 5699, 5692, 5700, 5701, 5702, 5704, 5705, 5706, 5709, 5707, 5708, 304);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0016C2A0 File Offset: 0x0016A4A0
		private static void AddHallowedFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(5733, 5712, 5713, 5714, 5716, 5717, 5718, 5719, 5720, 5721, 5722, 5715, 5723, 5724, 5725, 5727, 5728, 5729, 5732, 5730, 5731, -1);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0016C31C File Offset: 0x0016A51C
		private static void AddEasterFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(5920, 5858, 5859, 5860, 5861, 5862, 5863, 5864, 5865, 5866, 5867, 5868, 5869, 5870, 5871, 5873, 5874, 5875, 5876, 5877, 5878, -1);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0016C398 File Offset: 0x0016A598
		private static void AddGothicFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(5922, 5739, 5740, 1512, 5742, 5743, 1509, 5744, 5745, 5746, 5747, 5741, 5748, 5749, 5750, 5752, 5753, 1510, 5755, 1511, 5754, 300);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0016C418 File Offset: 0x0016A618
		private static void AddDemoniteFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(577, 5756, 5757, 5758, 5759, 5760, 5761, 5762, 5763, 5764, 5765, 5766, 5767, 5768, 5769, 5771, 5772, 5773, 5774, 5775, 5776, -1);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0016C494 File Offset: 0x0016A694
		private static void AddCrimtaneFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(2793, 5777, 5778, 5779, 5780, 5781, 5782, 5783, 5784, 5785, 5786, 5787, 5788, 5789, 5790, 5792, 5793, 5794, 5795, 5796, 5797, -1);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0016C510 File Offset: 0x0016A710
		private static void AddSnowFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(593, 5798, 5799, 5800, 5801, 5802, 5803, 5804, 5805, 5806, 5807, 5808, 5809, 5810, 5811, 5813, 5814, 5815, 5816, 5817, 5818, 306);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0016C590 File Offset: 0x0016A790
		private static void AddFlinxFurFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(5924, 5819, 5820, 5821, 5822, 5823, 5824, 5825, 5826, 5827, 5828, 5829, 5830, 5831, 5832, 5834, 5835, 5836, 5837, 5838, 5839, -1);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0016C60C File Offset: 0x0016A80C
		private static void AddPineFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(1872, 5840, 5841, 5842, 5843, 5844, 1925, 5845, 5846, 5847, 1924, 5848, 5849, 5850, 5851, 5853, 5854, 1926, 5855, 5856, 5857, -1);
			Recipe.AddStandardFurnitureSetRecipes(5930, 5840, 5841, 5842, 5843, 5844, 1925, 5845, 5846, 5847, 1924, 5848, 5849, 5850, 5851, 5853, 5854, 1926, 5855, 5856, 5857, -1);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0016C6F8 File Offset: 0x0016A8F8
		private static void AddStoneFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(129, 5879, 5880, 5881, 5882, 5883, 5884, 5885, 5886, 5887, 4415, 5888, 5889, 5890, 5891, 5892, 5893, 5894, 5895, 5896, 5897, 283);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0016C778 File Offset: 0x0016A978
		private static void AddJellyfishFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(5926, 5898, 5899, 5900, 5901, 5902, 5903, 5904, 5905, 5906, 5907, 5908, 5909, 5910, 5911, 5913, 5914, 5915, 5916, 5917, 5918, -1);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0016C7F4 File Offset: 0x0016A9F4
		private static void AddHarpyFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(5953, 5932, 5933, 5934, 5935, 5936, 5937, 5938, 5939, 5940, 5941, 5942, 5943, 5944, 5945, 5947, 5948, 5949, 5950, 5951, 5952, 305);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0016C874 File Offset: 0x0016AA74
		private static void AddCloudFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(751, 5955, 5956, 5957, 5958, 5959, 5960, 5961, 5962, 5963, 5964, 5965, 5966, 5967, 5968, 5969, 5970, 5971, 5972, 5973, 5974, 305);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0016C8F4 File Offset: 0x0016AAF4
		private static void AddMoonplateFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(5996, 5975, 5976, 5977, 5978, 5979, 5980, 5981, 5982, 5983, 5984, 5985, 5986, 5987, 5988, 5990, 5991, 5992, 5993, 5994, 5995, 305);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0016C974 File Offset: 0x0016AB74
		private static void AddLibrarianFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(6019, 5998, 5999, 6000, 6001, 6002, 6003, 6004, 6005, 6006, 6007, 6008, 6009, 6010, 6011, 6013, 6014, 6015, 6016, 6017, 6018, 101);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0016C9F4 File Offset: 0x0016ABF4
		private static void AddSpikeFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(6042, 6021, 6022, 6023, 6024, 6025, 6026, 6027, 6028, 6029, 6030, 6031, 6032, 6033, 6034, 6036, 6037, 6038, 6039, 6040, 6041, -1);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0016CA70 File Offset: 0x0016AC70
		private static void AddOfficeFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(6065, 6044, 6045, 6046, 6047, 6048, 6049, 6050, 6051, 6052, 6053, 6054, 6055, 6056, 6057, 6059, 6060, 6061, 6062, 6063, 6064, -1);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0016CAEC File Offset: 0x0016ACEC
		private static void AddForbiddenFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(6088, 6067, 6068, 6069, 6070, 6071, 6072, 6073, 6074, 6075, 6076, 6077, 6078, 6079, 6080, 6082, 6083, 6084, 6085, 6086, 6087, -1);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0016CB68 File Offset: 0x0016AD68
		private static void AddWaterFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(6109, 6090, 6091, 6092, 6093, 6094, 6095, 6096, 1298, 6097, 6098, 6099, 6100, 6101, 6102, 6104, 6105, 6106, 6107, 6108, 3678, -1);
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0016CBE4 File Offset: 0x0016ADE4
		private static void AddBoulderFurniture()
		{
			Recipe.AddStandardFurnitureSetRecipes(6132, 6111, 6112, 6113, 6114, 6115, 6116, 6117, 6118, 6119, 6120, 6121, 6122, 6123, 6124, 6126, 6127, 6128, 6129, 6130, 6131, 283);
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0016CC64 File Offset: 0x0016AE64
		private static void AddStandardFurnitureSetRecipes(int primaryMaterial, int bathtub, int bed, int bookcase, int candelabra, int candle, int chair, int chandelier, int chest, int clock, int door, int dresser, int lamp, int lantern, int piano, int sink, int sofa, int table, int toilet, int workbench, int fake_chest, int overrideStation = -1)
		{
			Recipe.currentRecipe.createItem.SetDefaults(bathtub, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				primaryMaterial,
				14
			});
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(bed, null);
			Recipe recipe = Recipe.currentRecipe;
			int[] array = new int[]
			{
				0,
				15,
				225,
				5
			};
			array[0] = primaryMaterial;
			recipe.SetIngredients(array);
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(bookcase, null);
			Recipe recipe2 = Recipe.currentRecipe;
			int[] array2 = new int[]
			{
				0,
				20,
				149,
				10
			};
			array2[0] = primaryMaterial;
			recipe2.SetIngredients(array2);
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(dresser, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				primaryMaterial,
				16
			});
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(candelabra, null);
			Recipe recipe3 = Recipe.currentRecipe;
			int[] array3 = new int[]
			{
				0,
				5,
				8,
				3
			};
			array3[0] = primaryMaterial;
			recipe3.SetIngredients(array3);
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(candle, null);
			Recipe recipe4 = Recipe.currentRecipe;
			int[] array4 = new int[]
			{
				0,
				4,
				8,
				1
			};
			array4[0] = primaryMaterial;
			recipe4.SetIngredients(array4);
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(chair, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				primaryMaterial,
				4
			});
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(chandelier, null);
			Recipe recipe5 = Recipe.currentRecipe;
			int[] array5 = new int[]
			{
				0,
				4,
				8,
				4,
				85,
				1
			};
			array5[0] = primaryMaterial;
			recipe5.SetIngredients(array5);
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 16);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(chest, null);
			Recipe recipe6 = Recipe.currentRecipe;
			int[] array6 = new int[]
			{
				0,
				8,
				22,
				2
			};
			array6[0] = primaryMaterial;
			recipe6.SetIngredients(array6);
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 18);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(clock, null);
			Recipe recipe7 = Recipe.currentRecipe;
			int[] array7 = new int[]
			{
				0,
				10,
				22,
				3,
				170,
				6
			};
			array7[0] = primaryMaterial;
			recipe7.SetIngredients(array7);
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 106);
			Recipe.currentRecipe.RequireGroup(RecipeGroups.IronBar);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(door, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				primaryMaterial,
				6
			});
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(lamp, null);
			Recipe recipe8 = Recipe.currentRecipe;
			int[] array8 = new int[]
			{
				8,
				1,
				0,
				3
			};
			array8[2] = primaryMaterial;
			recipe8.SetIngredients(array8);
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(lantern, null);
			Recipe recipe9 = Recipe.currentRecipe;
			int[] array9 = new int[]
			{
				0,
				6,
				8,
				1
			};
			array9[0] = primaryMaterial;
			recipe9.SetIngredients(array9);
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(piano, null);
			Recipe recipe10 = Recipe.currentRecipe;
			int[] array10 = new int[]
			{
				0,
				15,
				154,
				4,
				149,
				1
			};
			array10[0] = primaryMaterial;
			recipe10.SetIngredients(array10);
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(sink, null);
			Recipe recipe11 = Recipe.currentRecipe;
			int[] array11 = new int[]
			{
				0,
				6,
				206,
				1
			};
			array11[0] = primaryMaterial;
			recipe11.SetIngredients(array11);
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(sofa, null);
			Recipe recipe12 = Recipe.currentRecipe;
			int[] array12 = new int[]
			{
				0,
				5,
				225,
				2
			};
			array12[0] = primaryMaterial;
			recipe12.SetIngredients(array12);
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 106);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(table, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				primaryMaterial,
				8
			});
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 18);
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(workbench, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				primaryMaterial,
				10
			});
			if (overrideStation >= 0)
			{
				Recipe.currentRecipe.SetCraftingStation(overrideStation);
			}
			Recipe.AddRecipe();
			Recipe.currentRecipe.createItem.SetDefaults(toilet, null);
			Recipe.currentRecipe.SetIngredients(new int[]
			{
				primaryMaterial,
				6
			});
			Recipe.currentRecipe.SetCraftingStation((overrideStation >= 0) ? overrideStation : 106);
			Recipe.AddRecipe();
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0016D1EC File Offset: 0x0016B3EC
		private static void AddCritterStatueRecipe(int statueItem, int critterItem = 0, RecipeGroup critterGroup = null)
		{
			Recipe.currentRecipe.createItem.SetDefaults(statueItem, null);
			int num = 0;
			Recipe.currentRecipe.requiredItem[num].SetDefaults(3, null);
			Recipe.currentRecipe.requiredItem[num].stack = 50;
			if (critterGroup != null)
			{
				critterItem = critterGroup.GetPlaceholderItemType();
				Recipe.currentRecipe.RequireGroup(critterGroup);
			}
			Recipe.currentRecipe.requiredItem[++num].SetDefaults(critterItem, null);
			Recipe.currentRecipe.requiredItem[num].stack = 5;
			Recipe.currentRecipe.requiredTile = 283;
			Recipe.currentRecipe.needGraveyardBiome = true;
			Recipe.AddRecipe();
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0016D290 File Offset: 0x0016B490
		private static void CreateReversePlatformRecipes()
		{
			int num = Recipe.numRecipes;
			for (int i = 0; i < num; i++)
			{
				if (Main.recipe[i].createItem.createTile >= 0 && TileID.Sets.Platforms[Main.recipe[i].createItem.createTile] && Main.recipe[i].requiredItem[1].type == 0)
				{
					Recipe.currentRecipe.createItem.SetDefaults(Main.recipe[i].requiredItem[0].type, null);
					Recipe.currentRecipe.createItem.stack = Main.recipe[i].requiredItem[0].stack;
					Recipe.currentRecipe.requiredItem[0].SetDefaults(Main.recipe[i].createItem.type, null);
					Recipe.currentRecipe.requiredItem[0].stack = Main.recipe[i].createItem.stack;
					Recipe.currentRecipe.requiredTile = Main.recipe[i].requiredTile;
					Recipe.AddRecipe();
					Recipe recipe = Main.recipe[Recipe.numRecipes - 1];
					for (int j = Recipe.numRecipes - 2; j > i; j--)
					{
						Main.recipe[j + 1] = Main.recipe[j];
					}
					Main.recipe[i + 1] = recipe;
					Main.recipe[i + 1].notDecraftable = true;
				}
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0016D3F0 File Offset: 0x0016B5F0
		private static void CreateReverseWallRecipes()
		{
			int num = Recipe.numRecipes;
			for (int i = 0; i < num; i++)
			{
				if (Main.recipe[i].createItem.createWall > 0 && Main.recipe[i].requiredItem[1].type == 0 && Main.recipe[i].requiredItem[0].createWall == -1)
				{
					Recipe.currentRecipe.createItem.SetDefaults(Main.recipe[i].requiredItem[0].type, null);
					Recipe.currentRecipe.createItem.stack = Main.recipe[i].requiredItem[0].stack;
					Recipe.currentRecipe.requiredItem[0].SetDefaults(Main.recipe[i].createItem.type, null);
					Recipe.currentRecipe.requiredItem[0].stack = Main.recipe[i].createItem.stack;
					Recipe.currentRecipe.requiredTile = Main.recipe[i].requiredTile;
					Recipe.AddRecipe();
					Recipe recipe = Main.recipe[Recipe.numRecipes - 1];
					for (int j = Recipe.numRecipes - 2; j > i; j--)
					{
						Main.recipe[j + 1] = Main.recipe[j];
					}
					Main.recipe[i + 1] = recipe;
					Main.recipe[i + 1].notDecraftable = true;
				}
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0016D54C File Offset: 0x0016B74C
		public void SetIngredients(params int[] ingredients)
		{
			if (ingredients.Length == 1)
			{
				ingredients = new int[]
				{
					ingredients[0],
					1
				};
			}
			if (ingredients.Length % 2 != 0)
			{
				throw new Exception("Bad ingredients amount");
			}
			for (int i = 0; i < ingredients.Length; i += 2)
			{
				int num = i / 2;
				this.requiredItem[num].SetDefaults(ingredients[i], null);
				this.requiredItem[num].stack = ingredients[i + 1];
			}
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0016D5B8 File Offset: 0x0016B7B8
		public void SetCraftingStation(int tileType)
		{
			this.requiredTile = tileType;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0016D5C4 File Offset: 0x0016B7C4
		private static void AddRecipe()
		{
			if (Recipe.currentRecipe.requiredTile == 13)
			{
				Recipe.currentRecipe.alchemy = true;
			}
			Main.recipe[Recipe.numRecipes] = Recipe.currentRecipe;
			if (Recipe.currentRecipe.requiredTile >= 0)
			{
				Recipe.TileUsedInRecipes[Recipe.currentRecipe.requiredTile] = true;
			}
			Recipe.currentRecipe = new Recipe();
			Recipe.numRecipes++;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0016D62E File Offset: 0x0016B82E
		public static int GetRequiredTileStyle(int tileID)
		{
			if (tileID != 26)
			{
				return 0;
			}
			if (!WorldGen.crimson)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0016D644 File Offset: 0x0016B844
		public static string GetRequiredTileName(int tileId)
		{
			int requiredTileStyle = Recipe.GetRequiredTileStyle(tileId);
			return Lang.GetMapObjectName(MapHelper.TileToLookup(tileId, requiredTileStyle));
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0016D664 File Offset: 0x0016B864
		public bool ContainsIngredient(int itemType)
		{
			foreach (Recipe.RequiredItemEntry requiredItemEntry in this.requiredItemQuickLookup)
			{
				if (requiredItemEntry.itemIdOrRecipeGroup == 0)
				{
					break;
				}
				if (requiredItemEntry.itemIdOrRecipeGroup == itemType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040002F3 RID: 755
		public static int maxRequirements = 15;

		// Token: 0x040002F4 RID: 756
		public static int maxRecipes = 3600;

		// Token: 0x040002F5 RID: 757
		public static int numRecipes;

		// Token: 0x040002F6 RID: 758
		public static bool[] TileUsedInRecipes = new bool[(int)TileID.Count];

		// Token: 0x040002F7 RID: 759
		public static List<int>[] TileCountsAs = new List<int>[(int)TileID.Count];

		// Token: 0x040002F8 RID: 760
		private static Recipe currentRecipe = new Recipe();

		// Token: 0x040002F9 RID: 761
		public Item createItem = new Item();

		// Token: 0x040002FA RID: 762
		public Item[] requiredItem = new Item[Recipe.maxRequirements];

		// Token: 0x040002FB RID: 763
		public int requiredTile = -1;

		// Token: 0x040002FC RID: 764
		public int[] acceptedGroups = new int[Recipe.maxRequirements];

		// Token: 0x040002FD RID: 765
		public Recipe.RequiredItemEntry[] requiredItemQuickLookup = new Recipe.RequiredItemEntry[Recipe.maxRequirements];

		// Token: 0x040002FE RID: 766
		public List<Item> customShimmerResults;

		// Token: 0x040002FF RID: 767
		public bool needHoney;

		// Token: 0x04000300 RID: 768
		public bool needWater;

		// Token: 0x04000301 RID: 769
		public bool needLava;

		// Token: 0x04000302 RID: 770
		public bool needTorchGodsFavor;

		// Token: 0x04000303 RID: 771
		public bool alchemy;

		// Token: 0x04000304 RID: 772
		public bool needSnowBiome;

		// Token: 0x04000305 RID: 773
		public bool needGraveyardBiome;

		// Token: 0x04000306 RID: 774
		public bool needMechdusa;

		// Token: 0x04000307 RID: 775
		public bool notDecraftable;

		// Token: 0x04000308 RID: 776
		public bool crimson;

		// Token: 0x04000309 RID: 777
		public bool corruption;

		// Token: 0x0400030A RID: 778
		private static Dictionary<int, int> _ownedItems = new Dictionary<int, int>();

		// Token: 0x0400030B RID: 779
		internal static List<Chest> _recipeChests = new List<Chest>();

		// Token: 0x0200060F RID: 1551
		public struct RequiredItemEntry
		{
			// Token: 0x06003BD4 RID: 15316 RVA: 0x0065B339 File Offset: 0x00659539
			public RequiredItemEntry(RecipeGroup recipeGroup, int stack = 1)
			{
				this = new Recipe.RequiredItemEntry(recipeGroup.GetGroupFakeItemId(), stack);
			}

			// Token: 0x06003BD5 RID: 15317 RVA: 0x0065B348 File Offset: 0x00659548
			public RequiredItemEntry(int itemIdOrRecipeGroup, int stack = 1)
			{
				this.itemIdOrRecipeGroup = itemIdOrRecipeGroup;
				this.stack = stack;
			}

			// Token: 0x170004C4 RID: 1220
			// (get) Token: 0x06003BD6 RID: 15318 RVA: 0x0065B358 File Offset: 0x00659558
			public bool IsRecipeGroup
			{
				get
				{
					return this.itemIdOrRecipeGroup >= RecipeGroup.FakeItemIdOffset;
				}
			}

			// Token: 0x170004C5 RID: 1221
			// (get) Token: 0x06003BD7 RID: 15319 RVA: 0x0065B36A File Offset: 0x0065956A
			public RecipeGroup RecipeGroup
			{
				get
				{
					return RecipeGroup.recipeGroups[this.itemIdOrRecipeGroup - RecipeGroup.FakeItemIdOffset];
				}
			}

			// Token: 0x06003BD8 RID: 15320 RVA: 0x0065B382 File Offset: 0x00659582
			public bool Matches(int itemType)
			{
				if (!this.IsRecipeGroup)
				{
					return this.itemIdOrRecipeGroup == itemType;
				}
				return this.RecipeGroup.Contains(itemType);
			}

			// Token: 0x06003BD9 RID: 15321 RVA: 0x0065B3A2 File Offset: 0x006595A2
			public static implicit operator Recipe.RequiredItemEntry(RecipeGroup recipeGroup)
			{
				return new Recipe.RequiredItemEntry(recipeGroup, 1);
			}

			// Token: 0x06003BDA RID: 15322 RVA: 0x0065B3AB File Offset: 0x006595AB
			public static implicit operator Recipe.RequiredItemEntry(int itemId)
			{
				return new Recipe.RequiredItemEntry(itemId, 1);
			}

			// Token: 0x04006417 RID: 25623
			public int itemIdOrRecipeGroup;

			// Token: 0x04006418 RID: 25624
			public int stack;
		}
	}
}
