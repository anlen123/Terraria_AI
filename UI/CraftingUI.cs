using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI.Gamepad;

namespace Terraria.UI
{
	// Token: 0x020000E1 RID: 225
	public class CraftingUI : ICraftingUI
	{
		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x0600189D RID: 6301 RVA: 0x004E23BD File Offset: 0x004E05BD
		// (set) Token: 0x0600189E RID: 6302 RVA: 0x004E3230 File Offset: 0x004E1430
		private static float inventoryScale
		{
			get
			{
				return Main.inventoryScale;
			}
			set
			{
				Main.inventoryScale = value;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x0600189F RID: 6303 RVA: 0x004CC349 File Offset: 0x004CA549
		private static int numAvailableRecipes
		{
			get
			{
				return Main.numAvailableRecipes;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x060018A0 RID: 6304 RVA: 0x004CC33A File Offset: 0x004CA53A
		// (set) Token: 0x060018A1 RID: 6305 RVA: 0x004CC341 File Offset: 0x004CA541
		private static int focusRecipe
		{
			get
			{
				return Main.focusRecipe;
			}
			set
			{
				Main.focusRecipe = value;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060018A2 RID: 6306 RVA: 0x004E3238 File Offset: 0x004E1438
		private static int mouseX
		{
			get
			{
				return Main.mouseX;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060018A3 RID: 6307 RVA: 0x004E323F File Offset: 0x004E143F
		private static int mouseY
		{
			get
			{
				return Main.mouseY;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060018A4 RID: 6308 RVA: 0x004E23CC File Offset: 0x004E05CC
		// (set) Token: 0x060018A5 RID: 6309 RVA: 0x004E3246 File Offset: 0x004E1446
		private static Color inventoryBack
		{
			get
			{
				return Main.inventoryBack;
			}
			set
			{
				Main.inventoryBack = value;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060018A6 RID: 6310 RVA: 0x004E324E File Offset: 0x004E144E
		// (set) Token: 0x060018A7 RID: 6311 RVA: 0x004E3255 File Offset: 0x004E1455
		private static bool recFastScroll
		{
			get
			{
				return Main.PipsFastScroll;
			}
			set
			{
				Main.PipsFastScroll = value;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060018A8 RID: 6312 RVA: 0x004E325D File Offset: 0x004E145D
		// (set) Token: 0x060018A9 RID: 6313 RVA: 0x004E3264 File Offset: 0x004E1464
		private static int recStart
		{
			get
			{
				return Main.recStart;
			}
			set
			{
				Main.recStart = value;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x060018AA RID: 6314 RVA: 0x004E326C File Offset: 0x004E146C
		public static bool AnyAdvancedGridVisible
		{
			get
			{
				return NewCraftingUI.Visible;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x060018AB RID: 6315 RVA: 0x004E3273 File Offset: 0x004E1473
		public static string CraftingWindowTextKey
		{
			get
			{
				if (Player.Settings.CraftingGridControl != Player.Settings.CraftingGridMode.Classic)
				{
					return "GameUI.CraftingWindow";
				}
				return "GameUI.CraftingWindowClassic";
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x060018AC RID: 6316 RVA: 0x004E3288 File Offset: 0x004E1488
		public static string CraftingWindowTextTipKey
		{
			get
			{
				if (Player.Settings.CraftingGridControl != Player.Settings.CraftingGridMode.Classic)
				{
					return "GameUI.CraftingWindowTip";
				}
				return "GameUI.CraftingWindowClassicTip";
			}
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x004E32A0 File Offset: 0x004E14A0
		public CraftingUI()
		{
			for (int i = 0; i < CraftingUI.availableRecipeY.Length; i++)
			{
				CraftingUI.availableRecipeY[i] = (float)(65 * i);
			}
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x004E32D4 File Offset: 0x004E14D4
		public void VisuallyRepositionRecipes(int oldRecipe)
		{
			float num = CraftingUI.availableRecipeY[Main.focusRecipe] - CraftingUI.availableRecipeY[oldRecipe];
			for (int i = 0; i < CraftingUI.availableRecipeY.Length; i++)
			{
				CraftingUI.availableRecipeY[i] -= num;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x060018AF RID: 6319 RVA: 0x004E3317 File Offset: 0x004E1517
		public static NewCraftingUI.RecipeFilter RecipeFilterHack
		{
			get
			{
				if (!Main.playerInventory || Main.PipsCurrentPage != Main.PipPage.Recipes || Player.Settings.CraftingGridControl != Player.Settings.CraftingGridMode.Classic)
				{
					return null;
				}
				return CraftingUI._lastFilter;
			}
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x004E333B File Offset: 0x004E153B
		public static void ClearHacks()
		{
			CraftingUI._lastFilter = null;
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x004E3344 File Offset: 0x004E1544
		public void OpenCloseFilter(NewCraftingUI.RecipeFilter filter)
		{
			if (Main.playerInventory && Main.PipsCurrentPage == Main.PipPage.Recipes && Main.PipsUseGrid)
			{
				CraftingUI._lastFilter = null;
				IngameUIWindows.CloseAll(false);
				return;
			}
			CraftingUI._lastFilter = filter;
			IngameUIWindows.CloseAll(true);
			Player.OpenInventory(false);
			Main.PipsUseGrid = true;
			Main.PipsCurrentPage = Main.PipPage.Recipes;
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x004E3394 File Offset: 0x004E1594
		public void DrawRecipesList(SpriteBatch spriteBatch, int adjY, int middleY, Color craftingTipColor)
		{
			UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig = -1;
			UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = -1;
			if (CraftingUI.numAvailableRecipes > 0)
			{
				string text = Lang.inter[25].Value;
				if (CraftingUI.RecipeFilterHack != null)
				{
					text = CraftingUI.RecipeFilterHack.GetWindowDescription();
				}
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, text, new Vector2(76f, (float)(414 + adjY)), craftingTipColor, 0f, default(Vector2), 1f, SpriteEffects.None, 0f, null, null);
			}
			this.AdjustRecipeOffsets();
			for (int i = 0; i < Recipe.maxRecipes; i++)
			{
				if (i < CraftingUI.numAvailableRecipes && Math.Abs(CraftingUI.availableRecipeY[i]) <= (float)middleY)
				{
					CraftingUI.inventoryScale = 100f / (Math.Abs(CraftingUI.availableRecipeY[i]) + 100f);
					if ((double)CraftingUI.inventoryScale < 0.75)
					{
						CraftingUI.inventoryScale = 0.75f;
					}
					if (CraftingUI.recFastScroll)
					{
						CraftingUI.inventoryScale = 0.75f;
					}
					double num;
					Color lightColor;
					this.GetItemSlotColors(middleY, 100f, i, out num, out lightColor);
					int num2 = (int)(46f - 26f * CraftingUI.inventoryScale);
					int num3 = (int)(410f + CraftingUI.availableRecipeY[i] * CraftingUI.inventoryScale - 30f * CraftingUI.inventoryScale + (float)adjY);
					if (!Main.LocalPlayer.creativeInterface && CraftingUI.mouseX >= num2 && (float)CraftingUI.mouseX <= (float)num2 + (float)TextureAssets.InventoryBack.Width() * CraftingUI.inventoryScale && CraftingUI.mouseY >= num3 && (float)CraftingUI.mouseY <= (float)num3 + (float)TextureAssets.InventoryBack.Height() * CraftingUI.inventoryScale && !PlayerInput.IgnoreMouseInterface)
					{
						Main.HoverOverCraftingItemButton(i);
					}
					if (CraftingUI.numAvailableRecipes > 0)
					{
						num -= 50.0;
						if (num < 0.0)
						{
							num = 0.0;
						}
						if (i == CraftingUI.focusRecipe)
						{
							UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = 0;
							if (PlayerInput.SettingsForUI.HighlightThingsForMouse)
							{
								ItemSlot.DrawGoldBGForCraftingMaterial = true;
							}
						}
						else
						{
							UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = -1;
						}
						Color inventoryBack = CraftingUI.inventoryBack;
						CraftingUI.inventoryBack = new Color((int)((byte)num), (int)((byte)num), (int)((byte)num), (int)((byte)num));
						ItemSlot.Draw(spriteBatch, ref Main.recipe[Main.availableRecipe[i]].createItem, 22, new Vector2((float)num2, (float)num3), lightColor);
						CraftingUI.inventoryBack = inventoryBack;
					}
				}
			}
			CraftingUI.inventoryScale = 0.6f;
			if (CraftingUI.numAvailableRecipes > 0)
			{
				UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig = -1;
				UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = -1;
				for (int j = 0; j < Recipe.maxRequirements; j++)
				{
					Recipe recipe = Main.recipe[Main.availableRecipe[CraftingUI.focusRecipe]];
					Item item = recipe.requiredItem[j];
					if (item.type == 0)
					{
						UILinkPointNavigator.Shortcuts.CRAFT_CurrentIngredientsCount = j + 1;
						return;
					}
					int num4 = 80 + j * 40;
					int num5 = 380 + adjY;
					double num6 = (double)((float)(CraftingUI.inventoryBack.A + 50) - Math.Abs(CraftingUI.availableRecipeY[CraftingUI.focusRecipe]) * 2f);
					if (num6 == 0.0)
					{
						break;
					}
					if (CraftingUI.mouseX >= num4 && (float)CraftingUI.mouseX <= (float)num4 + (float)TextureAssets.InventoryBack.Width() * CraftingUI.inventoryScale && CraftingUI.mouseY >= num5 && (float)CraftingUI.mouseY <= (float)num5 + (float)TextureAssets.InventoryBack.Height() * CraftingUI.inventoryScale && !PlayerInput.IgnoreMouseInterface)
					{
						Main.craftingHide = true;
						Main.LocalPlayer.mouseInterface = true;
						ItemSlot.HoverOverrideClick(item, 22);
						CraftingUI.SetRecipeMaterialDisplayName(recipe, item);
					}
					num6 -= 50.0;
					if (num6 < 0.0)
					{
						num6 = 0.0;
					}
					UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = 1 + j;
					Color inventoryBack2 = CraftingUI.inventoryBack;
					CraftingUI.inventoryBack = new Color((int)((byte)num6), (int)((byte)num6), (int)((byte)num6), (int)((byte)num6));
					ItemSlot.Draw(spriteBatch, ref item, 22, new Vector2((float)num4, (float)num5), default(Color));
					CraftingUI.inventoryBack = inventoryBack2;
				}
			}
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x004E377C File Offset: 0x004E197C
		public static void DrawGridToggle(SpriteBatch spriteBatch, int craftX, int craftY, int gamepadPointId)
		{
			if (CraftingUI._lastFilter != null && (!CraftingUI._lastFilter.CanRemainOpen() || Main.PipsCurrentPage != Main.PipPage.Recipes || !Main.playerInventory))
			{
				CraftingUI._lastFilter = null;
			}
			UILinkPointNavigator.SetPosition(gamepadPointId, new Vector2((float)craftX, (float)craftY));
			if (CraftingUI.numAvailableRecipes == 0 && !CraftingUI.AnyAdvancedGridVisible)
			{
				if (Main.PipsCurrentPage == Main.PipPage.Recipes)
				{
					Main.PipsUseGrid = false;
					return;
				}
			}
			else
			{
				bool flag = CraftingUI.mouseX > craftX - 15 && CraftingUI.mouseX < craftX + 15 && CraftingUI.mouseY > craftY - 15 && CraftingUI.mouseY < craftY + 15 && !PlayerInput.IgnoreMouseInterface;
				if (Main.PipsCurrentPage == Main.PipPage.Recipes)
				{
					Utils.DrawSelectedCraftingBarIndicator(spriteBatch, craftX, craftY);
				}
				bool flag2 = Player.Settings.CraftingGridControl == Player.Settings.CraftingGridMode.Classic;
				int num = 2;
				if (NewCraftingUI.Visible)
				{
					num = 4;
				}
				if (Main.PipsCurrentPage == Main.PipPage.Recipes && Main.PipsUseGrid)
				{
					num = 0;
				}
				num += flag.ToInt();
				spriteBatch.Draw(TextureAssets.CraftToggle[num].Value, new Vector2((float)craftX, (float)craftY), null, Color.White, 0f, TextureAssets.CraftToggle[num].Value.Size() / 2f, 1f, SpriteEffects.None, 0f);
				if (flag)
				{
					Main.instance.MouseTextNoOverride(Language.GetTextValue(CraftingUI.CraftingWindowTextTipKey), 0, 0, -1, -1, -1, -1, 0);
					Main.player[Main.myPlayer].mouseInterface = true;
					if (Main.mouseLeft && Main.mouseLeftRelease)
					{
						if (!Main.TryChangePipsPage(Main.PipPage.Recipes))
						{
							if (flag2)
							{
								NewCraftingUI.Close(true, true);
								Main.PipsUseGrid = !Main.PipsUseGrid;
							}
							else
							{
								Main.PipsUseGrid = false;
								if (CraftingUI.AnyAdvancedGridVisible)
								{
									UILinkPointNavigator.ChangePoint(11001);
								}
								NewCraftingUI.ToggleInInventory(false);
							}
							Main.mouseLeftRelease = false;
						}
						SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					}
					if (Main.mouseRight && Main.mouseRightRelease)
					{
						Main.mouseRightRelease = false;
						SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
						Player.Settings.CraftingGridMode craftingGridControl = Player.Settings.CraftingGridControl;
						if (craftingGridControl != Player.Settings.CraftingGridMode.Modern)
						{
							if (craftingGridControl == Player.Settings.CraftingGridMode.Classic)
							{
								Player.Settings.CraftingGridControl = Player.Settings.CraftingGridMode.Modern;
							}
						}
						else
						{
							Player.Settings.CraftingGridControl = Player.Settings.CraftingGridMode.Classic;
						}
					}
				}
				Main.DoStatefulTickSound(ref Main.GridToggleMouseOverCrafting, flag);
			}
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x004E3994 File Offset: 0x004E1B94
		public static void DrawCraftFromNearbyChestsToggle(SpriteBatch spriteBatch, int toggleNearbyX, int toggleNearbyY, int gamepadPointId)
		{
			UILinkPointNavigator.SetPosition(gamepadPointId, new Vector2((float)toggleNearbyX, (float)toggleNearbyY));
			bool flag = CraftingUI.mouseX > toggleNearbyX - 15 && CraftingUI.mouseX < toggleNearbyX + 15 && CraftingUI.mouseY > toggleNearbyY - 15 && CraftingUI.mouseY < toggleNearbyY + 15 && !PlayerInput.IgnoreMouseInterface;
			int num = 2 - Player.Settings.CraftFromNearbyChests.ToInt() * 2 + flag.ToInt();
			int num2 = 1;
			spriteBatch.Draw(TextureAssets.ChestCraft[num].Value, new Vector2((float)toggleNearbyX, (float)toggleNearbyY), null, Color.White, 0f, TextureAssets.ChestCraft[num].Value.Size() / 2f, (float)num2, SpriteEffects.None, 0f);
			if (flag)
			{
				Main.instance.MouseTextNoOverride(Language.GetTextValue(Player.Settings.CraftFromNearbyChests ? "GameUI.CraftFromNearbyChestsOn" : "GameUI.CraftFromNearbyChestsOff"), 0, 0, -1, -1, -1, -1, 0);
				Main.player[Main.myPlayer].mouseInterface = true;
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Player.Settings.CraftFromNearbyChests = !Player.Settings.CraftFromNearbyChests;
					NewCraftingUI.RefreshGrid();
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					Main.SaveSettings();
				}
			}
			Main.DoStatefulTickSound(ref Main.nearbyCraftingMouseOver, flag);
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x004E3AD4 File Offset: 0x004E1CD4
		private void GetItemSlotColors(int middleY, float fadeInValue, int recipeIndex, out double inventoryAlpha, out Color inventoryColor2)
		{
			inventoryAlpha = (double)(CraftingUI.inventoryBack.A + 50);
			double num = 255.0;
			if (Math.Abs(CraftingUI.availableRecipeY[recipeIndex]) > (float)middleY - fadeInValue)
			{
				inventoryAlpha = (double)(150f * (fadeInValue - (Math.Abs(CraftingUI.availableRecipeY[recipeIndex]) - ((float)middleY - fadeInValue)))) * 0.01;
				num = (double)(255f * (fadeInValue - (Math.Abs(CraftingUI.availableRecipeY[recipeIndex]) - ((float)middleY - fadeInValue)))) * 0.01;
			}
			new Color((int)((byte)inventoryAlpha), (int)((byte)inventoryAlpha), (int)((byte)inventoryAlpha), (int)((byte)inventoryAlpha));
			inventoryColor2 = new Color((int)((byte)num), (int)((byte)num), (int)((byte)num), (int)((byte)num));
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x004E3B87 File Offset: 0x004E1D87
		private void AdjustRecipeOffsets()
		{
			this.DrawRecipes_AdjustRecipeOffsetSnappy();
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x004E3B90 File Offset: 0x004E1D90
		private void DrawRecipes_AdjustRecipeOffsetSnappy()
		{
			int num = 65;
			float amount = (float)num / 10f;
			float num2 = CraftingUI.availableRecipeY[CraftingUI.focusRecipe];
			float num3 = num2 * 0.97f;
			num3 = Utils.MoveTowards(num3, 0f, amount);
			if (CraftingUI.recFastScroll)
			{
				num3 = 0f;
			}
			CraftingUI.availableRecipeY[CraftingUI.focusRecipe] = num3;
			int num4 = (int)(num2 / (float)num);
			int num5 = (int)(num3 / (float)num);
			if (num4 != num5)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}
			for (int i = 0; i < CraftingUI.numAvailableRecipes; i++)
			{
				float num6 = CraftingUI.availableRecipeY[i];
				int num7 = (i - CraftingUI.focusRecipe) * num;
				CraftingUI.availableRecipeY[i] = num3 + (float)num7;
			}
			if (num2 == 0f)
			{
				CraftingUI.recFastScroll = false;
				return;
			}
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x004E3C50 File Offset: 0x004E1E50
		private void DrawRecipes_AdjustRecipeOffset(int recipeIndex)
		{
			int num = 65;
			float amount = (float)num / 10f;
			int num2 = (recipeIndex - CraftingUI.focusRecipe) * num;
			if (CraftingUI.availableRecipeY[recipeIndex] == (float)num2)
			{
				CraftingUI.recFastScroll = false;
				return;
			}
			if (CraftingUI.availableRecipeY[recipeIndex] == 0f && !CraftingUI.recFastScroll)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}
			if (CraftingUI.recFastScroll)
			{
				CraftingUI.availableRecipeY[recipeIndex] = (float)num2;
				return;
			}
			CraftingUI.availableRecipeY[recipeIndex] = Utils.MoveTowards(CraftingUI.availableRecipeY[recipeIndex], (float)num2, amount);
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x004E3CD8 File Offset: 0x004E1ED8
		public static void SetRecipeMaterialDisplayName(Recipe recipe, Item material)
		{
			Item item = material.Clone();
			ItemSlot.MouseHover(item, 22);
			item = Main.HoverItem;
			string nameOverride;
			if (recipe.ProcessGroupsForText(material.type, out nameOverride))
			{
				item.SetNameOverride(nameOverride);
			}
			Main.hoverItemName = item.Name;
			if (material.stack > 1)
			{
				Main.hoverItemName = string.Concat(new object[]
				{
					Main.hoverItemName,
					" (",
					material.stack,
					")"
				});
			}
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x004E3D5C File Offset: 0x004E1F5C
		public void DrawRecipesGrid(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig = -1;
			UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = -1;
			int num = 42;
			CraftingUI.inventoryScale = 0.75f;
			int num2 = 340;
			int num3 = 310;
			int num4 = (Main.screenWidth - num3 - 280) / num;
			int num5 = (Main.screenHeight - num2 - 20) / num;
			UILinkPointNavigator.Shortcuts.CRAFT_IconsPerRow = num4;
			UILinkPointNavigator.Shortcuts.CRAFT_IconsPerColumn = num5;
			int num6 = 0;
			int num7 = 0;
			int num8 = num3;
			int num9 = num2;
			int num10 = num3 - 20;
			int num11 = num2 + 2;
			if (CraftingUI.recStart > CraftingUI.numAvailableRecipes - num4 * num5)
			{
				CraftingUI.recStart = CraftingUI.numAvailableRecipes - num4 * num5;
				if (CraftingUI.recStart < 0)
				{
					CraftingUI.recStart = 0;
				}
			}
			if (CraftingUI.recStart > 0)
			{
				if (CraftingUI.mouseX >= num10 && CraftingUI.mouseX <= num10 + TextureAssets.CraftUpButton.Width() && CraftingUI.mouseY >= num11 && CraftingUI.mouseY <= num11 + TextureAssets.CraftUpButton.Height() && !PlayerInput.IgnoreMouseInterface)
				{
					Main.LocalPlayer.mouseInterface = true;
					if (Main.mouseLeftRelease && Main.mouseLeft)
					{
						CraftingUI.recStart -= num4;
						if (CraftingUI.recStart < 0)
						{
							CraftingUI.recStart = 0;
						}
						SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
						Main.mouseLeftRelease = false;
					}
				}
				spriteBatch.Draw(TextureAssets.CraftUpButton.Value, new Vector2((float)num10, (float)num11), null, new Color(200, 200, 200, 200), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			}
			if (CraftingUI.recStart < CraftingUI.numAvailableRecipes - num4 * num5)
			{
				num11 += 20;
				if (CraftingUI.mouseX >= num10 && CraftingUI.mouseX <= num10 + TextureAssets.CraftUpButton.Width() && CraftingUI.mouseY >= num11 && CraftingUI.mouseY <= num11 + TextureAssets.CraftUpButton.Height() && !PlayerInput.IgnoreMouseInterface)
				{
					Main.LocalPlayer.mouseInterface = true;
					if (Main.mouseLeftRelease && Main.mouseLeft)
					{
						CraftingUI.recStart += num4;
						SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
						if (CraftingUI.recStart > CraftingUI.numAvailableRecipes - num4)
						{
							CraftingUI.recStart = CraftingUI.numAvailableRecipes - num4;
						}
						Main.mouseLeftRelease = false;
					}
				}
				spriteBatch.Draw(TextureAssets.CraftDownButton.Value, new Vector2((float)num10, (float)num11), null, new Color(200, 200, 200, 200), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			}
			int num12 = CraftingUI.recStart;
			while (num12 < Recipe.maxRecipes && num12 < CraftingUI.numAvailableRecipes)
			{
				int num13 = num8;
				int num14 = num9;
				double num15 = (double)(CraftingUI.inventoryBack.A + 50);
				double num16 = 255.0;
				new Color((int)((byte)num15), (int)((byte)num15), (int)((byte)num15), (int)((byte)num15));
				new Color((int)((byte)num16), (int)((byte)num16), (int)((byte)num16), (int)((byte)num16));
				if (CraftingUI.mouseX >= num13 && (float)CraftingUI.mouseX <= (float)num13 + (float)TextureAssets.InventoryBack.Width() * CraftingUI.inventoryScale && CraftingUI.mouseY >= num14 && (float)CraftingUI.mouseY <= (float)num14 + (float)TextureAssets.InventoryBack.Height() * CraftingUI.inventoryScale && !PlayerInput.IgnoreMouseInterface)
				{
					Main.LocalPlayer.mouseInterface = true;
					if (Main.mouseLeftRelease && Main.mouseLeft)
					{
						CraftingUI.focusRecipe = num12;
						CraftingUI.recFastScroll = true;
						Main.PipsUseGrid = false;
						SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
						Main.mouseLeftRelease = false;
						if (PlayerInput.UsingGamepadUI)
						{
							UILinkPointNavigator.ChangePage(9);
							Main.LockCraftingForThisCraftClickDuration();
						}
					}
					Main.craftingHide = true;
					Item createItem = Main.recipe[Main.availableRecipe[num12]].createItem;
					Main.HoverItem = createItem.Clone();
					ItemSlot.MouseHover(22);
					Main.hoverItemName = createItem.Name;
					if (createItem.stack > 1)
					{
						Main.hoverItemName = string.Concat(new object[]
						{
							Main.hoverItemName,
							" (",
							createItem.stack,
							")"
						});
					}
				}
				if (CraftingUI.numAvailableRecipes > 0)
				{
					num15 -= 50.0;
					if (num15 < 0.0)
					{
						num15 = 0.0;
					}
					UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig = num12 - CraftingUI.recStart;
					Color inventoryBack = CraftingUI.inventoryBack;
					CraftingUI.inventoryBack = new Color((int)((byte)num15), (int)((byte)num15), (int)((byte)num15), (int)((byte)num15));
					ItemSlot.Draw(spriteBatch, ref Main.recipe[Main.availableRecipe[num12]].createItem, 22, new Vector2((float)num13, (float)num14), default(Color));
					CraftingUI.inventoryBack = inventoryBack;
				}
				num8 += num;
				num6++;
				if (num6 >= num4)
				{
					num8 = num3;
					num9 += num;
					num6 = 0;
					num7++;
					if (num7 >= num5)
					{
						break;
					}
				}
				num12++;
			}
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x004E4267 File Offset: 0x004E2467
		public void ScrollCraftingList(int mouseWheel)
		{
			CraftingUI.focusRecipe += mouseWheel;
			if (CraftingUI.focusRecipe > CraftingUI.numAvailableRecipes - 1)
			{
				CraftingUI.focusRecipe = CraftingUI.numAvailableRecipes - 1;
			}
			if (CraftingUI.focusRecipe < 0)
			{
				CraftingUI.focusRecipe = 0;
			}
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x004E42A0 File Offset: 0x004E24A0
		public void ScrollCraftingGrid(int mouseWheel, int width)
		{
			if (mouseWheel < 0)
			{
				CraftingUI.recStart -= width;
				if (CraftingUI.recStart < 0)
				{
					CraftingUI.recStart = 0;
					return;
				}
			}
			else
			{
				CraftingUI.recStart += width;
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				if (CraftingUI.recStart > CraftingUI.numAvailableRecipes - width)
				{
					CraftingUI.recStart = CraftingUI.numAvailableRecipes - width;
				}
			}
		}

		// Token: 0x040012DC RID: 4828
		public static float[] availableRecipeY = new float[Recipe.maxRecipes];

		// Token: 0x040012DD RID: 4829
		private static NewCraftingUI.RecipeFilter _lastFilter;
	}
}
