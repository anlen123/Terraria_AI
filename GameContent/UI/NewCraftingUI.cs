using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI
{
	// Token: 0x02000365 RID: 869
	public class NewCraftingUI : UIState
	{
		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x060028D4 RID: 10452 RVA: 0x00574C0C File Offset: 0x00572E0C
		public static bool Visible
		{
			get
			{
				return NewCraftingUI._ui.CurrentState != null;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x060028D5 RID: 10453 RVA: 0x00574C1B File Offset: 0x00572E1B
		private NewCraftingUI.RecipeEntry SelectedEntry
		{
			get
			{
				if (this._selectedRecipeIndex == null)
				{
					return null;
				}
				return this._recipeListLookup[this._selectedRecipeIndex.Value];
			}
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x00574C40 File Offset: 0x00572E40
		public NewCraftingUI()
		{
			UILinkPage page = UILinkPointNavigator.Pages[24];
			page.OnSpecialInteracts += this.GetGamepadInstructions;
			page.UpdateEvent += delegate()
			{
				PlayerInput.GamepadAllowScrolling = true;
			};
			page.EnterEvent += delegate()
			{
				page.CurrentPoint = (Main.InGuideCraftMenu ? 20020 : 20000);
			};
			this._filterer = new EntryFilterer<Item, IItemEntryFilter>();
			List<IItemEntryFilter> list = new List<IItemEntryFilter>
			{
				new ItemFilters.Weapon(),
				new ItemFilters.Armor(),
				new ItemFilters.Vanity(),
				new ItemFilters.BuildingBlock(),
				new ItemFilters.Furniture(),
				new ItemFilters.Accessories(),
				new ItemFilters.MiscAccessories(),
				new ItemFilters.Consumables(),
				new ItemFilters.Tools(),
				new ItemFilters.Materials()
			};
			List<IItemEntryFilter> list2 = new List<IItemEntryFilter>();
			list2.AddRange(list);
			list2.Add(new ItemFilters.MiscFallback(list));
			this._filterer.AddFilters(list2);
			this._filterer.SetSearchFilterObject<ItemFilters.BySearch>(new ItemFilters.BySearch());
			this.HAlign = 0f;
			this.VAlign = 0f;
			this.Left = new StyleDimension(20f, 0f);
			this.Top = new StyleDimension(312f, 0f);
			this.Width = new StyleDimension(490f, 0f);
			this.Height = new StyleDimension(-350f, 1f);
			base.SetPadding(0f);
			UIElement uielement = new UIElement
			{
				Width = StyleDimension.Fill,
				Height = StyleDimension.Fill
			};
			uielement.SetPadding(0f);
			this.BuildInfinitesMenuContents(uielement);
			base.Append(uielement);
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x00574E44 File Offset: 0x00573044
		private void BuildInfinitesMenuContents(UIElement totalContainer)
		{
			UIPanel uipanel = new UIPanel
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(-38f, 1f),
				Top = new StyleDimension(38f, 0f),
				PaddingRight = 8f
			};
			uipanel.BackgroundColor = Utils.ShiftBlueToCyanTheme(uipanel.BackgroundColor);
			uipanel.BorderColor = Utils.ShiftBlueToCyanTheme(uipanel.BorderColor);
			uipanel.BackgroundColor *= 0.8f;
			uipanel.BorderColor *= 0.8f;
			totalContainer.Append(uipanel);
			UIText uitext = new UIText("", 1f, false)
			{
				Left = new StyleDimension(-1f, 0f),
				Top = new StyleDimension(-2f, 0f)
			};
			uipanel.Append(uitext);
			this._text = uitext;
			UIWrappedSearchBar uiwrappedSearchBar = new UIWrappedSearchBar(new Action(this.GoBackFromVirtualKeyboard), null, UIWrappedSearchBar.ColorTheme.Red)
			{
				Top = new StyleDimension(-4f, 0f),
				HAlign = 1f
			};
			uiwrappedSearchBar.CustomOpenVirtualKeyboard = delegate(UIState state)
			{
				IngameFancyUI.OpenUIState(state, false);
			};
			uiwrappedSearchBar.OnSearchContentsChanged += this.OnSearchContentsChanged;
			uiwrappedSearchBar.SetSearchSnapPoint("NewCraftingUISearch", 0, null, null);
			uipanel.Append(uiwrappedSearchBar);
			this._searchBar = uiwrappedSearchBar;
			UIElement uielement = new UIElement
			{
				Width = StyleDimension.Fill,
				Height = StyleDimension.Fill,
				VAlign = 1f
			};
			this._gridContainer = uielement;
			uipanel.Append(uielement);
			UIHorizontalSeparator uihorizontalSeparator = new UIHorizontalSeparator(2, true)
			{
				Width = new StyleDimension(-8f, 1f),
				HAlign = 0.5f,
				Color = new Color(89, 116, 213, 255) * 0.9f
			};
			uihorizontalSeparator.Color = Utils.ShiftBlueToCyanTheme(uihorizontalSeparator.Color);
			uielement.Append(uihorizontalSeparator);
			UIList uilist = new UIList
			{
				Width = new StyleDimension(-20f, 1f),
				Height = new StyleDimension(-7f, 1f),
				VAlign = 1f,
				HAlign = 0f
			};
			uielement.Append(uilist);
			float num = 4f;
			UIScrollbar uiscrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Cyan)
			{
				AutoHide = true,
				Height = new StyleDimension(-num * 2f - 11f, 1f),
				Top = new StyleDimension(-num, 0f),
				VAlign = 1f,
				HAlign = 1f
			};
			uielement.Append(uiscrollbar);
			uilist.SetScrollbar(uiscrollbar);
			uilist.Add(this._itemGrid = new NewCraftingUI.ItemGrid(this));
			UICreativeItemsInfiniteFilteringOptions uicreativeItemsInfiniteFilteringOptions = new UICreativeItemsInfiniteFilteringOptions(this._filterer, "NewCraftingUIFilters", UICreativeItemsInfiniteFilteringOptions.ColorTheme.Cyan)
			{
				HAlign = 0.5f
			};
			uicreativeItemsInfiniteFilteringOptions.OnClickingOption += this.ResetRecipes;
			totalContainer.Append(uicreativeItemsInfiniteFilteringOptions);
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x00575183 File Offset: 0x00573383
		public override void Draw(SpriteBatch spriteBatch)
		{
			if (base.IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}
			this.UpdateCraftAreaSize();
			this.UpdateText();
			this.UpdateContents();
			base.Draw(spriteBatch);
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x005751B4 File Offset: 0x005733B4
		private void UpdateCraftAreaSize()
		{
			int num = Main.InGuideCraftMenu ? 130 : 77;
			if (this._gridContainer.Height.Pixels != (float)(-(float)num))
			{
				this._gridContainer.Height.Pixels = (float)(-(float)num);
				this._gridContainer.Recalculate();
			}
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x00575208 File Offset: 0x00573408
		private void UpdateText()
		{
			string text = (this._filter != null) ? this._filter.GetWindowDescription() : Lang.inter[25].Value;
			if (text != this._text.Text)
			{
				this._text.SetText(text);
				this._text.Recalculate();
				this._searchBar.Width = new StyleDimension(-this._text.GetOuterDimensions().Width - 10f, 1f);
				this._searchBar.Recalculate();
			}
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x0057529C File Offset: 0x0057349C
		protected override void DrawChildren(SpriteBatch spriteBatch)
		{
			this._hoveredEntry = null;
			this._missingRequirementsTooltipText = null;
			base.DrawChildren(spriteBatch);
			if (PlayerInput.UsingGamepad && this._hoveredEntry != null)
			{
				this._selectedRecipeIndex = new int?(this._hoveredEntry.index);
			}
			Vector2 vector = base.GetInnerDimensions().ToRectangle().TopLeft() + new Vector2(24f, 73f);
			if (Main.InGuideCraftMenu)
			{
				if (this.DrawRecipeSlot(spriteBatch, Main.guideItem, 7, vector + new Vector2(0f, 58f), true, 1f))
				{
					ItemSlot.Handle(ref Main.guideItem, 7, true);
				}
				string text = Main.guideItem.IsAir ? Lang.inter[24].Value : (Lang.inter[21].Value + " " + Main.guideItem.Name);
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, text, vector + new Vector2(52f, 73f), Color.White, 0f, Vector2.Zero, Vector2.One, -1f, 1f);
			}
			int num = 0;
			NewCraftingUI.RecipeEntry recipeEntry = this._hoveredEntry ?? this.SelectedEntry;
			if (recipeEntry != null)
			{
				Recipe recipe = recipeEntry.Recipe;
				if (Main.InGuideCraftMenu)
				{
					string recipeRequirementsText = Main.GetRecipeRequirementsText(recipe, false);
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, recipeRequirementsText, vector + new Vector2(52f, 36f), Color.White, 0f, Vector2.Zero, Vector2.One, -1f, 1f);
				}
				if (this.DrawRecipeSlot(spriteBatch, recipe.createItem, 42, vector, recipeEntry.Available, 1f))
				{
					this.HandleCraftSlot(recipeEntry, 42);
				}
				spriteBatch.Draw(Main.Assets.Request<Texture2D>("Images/UI/Craft", 1).Value, vector + new Vector2(47f, 13f), null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
				while (num < Recipe.maxRequirements && !recipe.requiredItem[num].IsAir)
				{
					num++;
				}
				vector += new Vector2(72f, 22f);
				float num2 = Math.Min(11f / (float)num, 1f);
				for (int i = 0; i < num; i++)
				{
					Item item = recipe.requiredItem[i];
					int availableItemCount = Recipe.GetAvailableItemCount(recipe.requiredItemQuickLookup[i]);
					bool flag = Main.InGuideCraftMenu || availableItemCount >= item.stack;
					Vector2 vector2 = vector + new Vector2((float)(i * 34), -16f) * num2;
					UILinkPointNavigator.Shortcuts.NewCraftingUI_MaterialIndex = i;
					if (this.DrawRecipeSlot(spriteBatch, item, 43, vector2, flag, 0.7f * num2))
					{
						ItemSlot.HoverOverrideClick(item, 43);
						CraftingUI.SetRecipeMaterialDisplayName(recipe, item);
					}
					if (!Main.InGuideCraftMenu)
					{
						this.DrawOwnedItemCount(spriteBatch, availableItemCount, flag, vector2, num2);
					}
				}
			}
			int num3 = 42;
			int num4 = 285;
			if (Main.LocalPlayer.difficulty == 3 && !Main.CreativeMenu.Blocked)
			{
				num3 += 40;
			}
			CraftingUI.DrawGridToggle(spriteBatch, num3, num4, 20030);
			num3 += 40;
			if (!Main.InGuideCraftMenu)
			{
				CraftingUI.DrawCraftFromNearbyChestsToggle(spriteBatch, num3, num4, 20031);
			}
			this.SetupGamepadPoints(recipeEntry != null, num);
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x00575620 File Offset: 0x00573820
		private void HandleCraftSlot(NewCraftingUI.RecipeEntry entry, int context)
		{
			Recipe recipe = entry.Recipe;
			int? selectedRecipeIndex = this._selectedRecipeIndex;
			int index = entry.index;
			bool flag = !(selectedRecipeIndex.GetValueOrDefault() == index & selectedRecipeIndex != null) || (PlayerInput.UsingGamepad && context == 41);
			if (!entry.Available || flag)
			{
				if (!ItemSlot.HoverOverrideClick(recipe.createItem, context) && flag && ((Main.mouseLeft && Main.mouseLeftRelease) || (Main.mouseRight && Main.mouseRightRelease)))
				{
					this._selectedRecipeIndex = new int?(entry.index);
					if (entry.Available)
					{
						Main.focusRecipe = entry.availableIndex;
					}
					UILinkPointNavigator.ChangePoint(20000);
					this._gamepadReturnToGridEntry = true;
					Main.stackSplit = 15;
					Main._preventCraftingBecauseClickWasUsedToChangeFocusedRecipe = true;
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				}
				ItemSlot.MouseHover(recipe.createItem, context);
			}
			else
			{
				Main.HoverOverCraftingItemButton(Main.focusRecipe);
			}
			if (!entry.Available)
			{
				this._missingRequirementsTooltipText = this.GetReasonForRecipeNotAvailable(recipe);
			}
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x0057572C File Offset: 0x0057392C
		private string GetReasonForRecipeNotAvailable(Recipe recipe)
		{
			NewCraftingUI._missingObjects.Clear();
			recipe.PlayerMeetsEnvironmentConditions(Main.LocalPlayer, NewCraftingUI._missingObjects);
			if (NewCraftingUI._missingObjects.Count > 0)
			{
				return Lang.inter[22].Value + " " + string.Join(", ", NewCraftingUI._missingObjects);
			}
			return Language.GetTextValue("GameUI.NotEnoughMaterials");
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x00575794 File Offset: 0x00573994
		internal static void AddTooltipLines(Item hoverItem, ref int numLines, string[] lineText, Color[] lineColors)
		{
			if (NewCraftingUI._instance == null || (NewCraftingUI._instance._missingRequirementsTooltipText != null && !hoverItem.IsAir))
			{
				lineText[numLines] = NewCraftingUI._instance._missingRequirementsTooltipText;
				lineColors[numLines] = new Color(255, 140, 160, 255);
				numLines++;
			}
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x005757F4 File Offset: 0x005739F4
		private void DrawOwnedItemCount(SpriteBatch spriteBatch, int owned, bool enough, Vector2 mpos, float mscale)
		{
			mpos += new Vector2(3f, 32f) * mscale;
			string text = (owned > 999) ? "999+" : owned.ToString();
			Color baseColor = enough ? new Color(144, 238, 144, 255) : new Color(255, 140, 160, 255);
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text, mpos, baseColor, 0f, Vector2.Zero, Vector2.One * 0.8f * mscale, -1f, 1f);
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x005758B0 File Offset: 0x00573AB0
		private bool DrawRecipeSlot(SpriteBatch spriteBatch, Item item, int context, Vector2 pos, bool enabled, float scale)
		{
			Color inventoryBack = Main.inventoryBack;
			Main.inventoryBack = Color.White * 0.7490196f;
			float inventoryScale = Main.inventoryScale;
			Main.inventoryScale *= scale;
			ItemSlot.Draw(spriteBatch, ref item, context, pos, enabled ? Color.White : NewCraftingUI.DisabledSlotColor);
			Main.inventoryScale = inventoryScale;
			Main.inventoryBack = inventoryBack;
			Rectangle rectangle = new Rectangle((int)pos.X, (int)pos.Y, (int)((float)TextureAssets.InventoryBack.Width() * scale), (int)((float)TextureAssets.InventoryBack.Height() * scale));
			return rectangle.Contains(Main.MouseScreen.ToPoint());
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x00575954 File Offset: 0x00573B54
		private void ResetRecipes()
		{
			this._resetForGuideItem = (Main.InGuideCraftMenu ? Main.guideItem : null);
			this._gamepadReturnToGridEntry = false;
			this._gamepadMoveToGridEntryHack = false;
			Array.Resize<NewCraftingUI.RecipeEntry>(ref this._recipeListLookup, Recipe.maxRecipes);
			if (this._recipes.Count == 0)
			{
				return;
			}
			this._recipes.Clear();
			this._filteredRecipes.Clear();
			Array.Clear(this._recipeListLookup, 0, this._recipeListLookup.Length);
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x005759CC File Offset: 0x00573BCC
		private void UpdateContents()
		{
			Recipe.UpdateRecipeList();
			if (Main.InGuideCraftMenu && Main.guideItem != this._resetForGuideItem)
			{
				this.ResetRecipes();
			}
			foreach (NewCraftingUI.RecipeEntry recipeEntry in this._recipes)
			{
				recipeEntry.availableIndex = -1;
			}
			int num = Main.numAvailableRecipes;
			if (Main.InGuideCraftMenu && Main.guideItem.IsAir)
			{
				num = 0;
			}
			bool flag = this._filteredRecipes.Count == 0;
			for (int i = 0; i < num; i++)
			{
				int num2 = Main.availableRecipe[i];
				NewCraftingUI.RecipeEntry recipeEntry2 = this._recipeListLookup[num2];
				if (recipeEntry2 == null)
				{
					recipeEntry2 = (this._recipeListLookup[num2] = new NewCraftingUI.RecipeEntry(num2));
					this._recipes.Add(recipeEntry2);
					if (this.FitsFilter(recipeEntry2.Recipe))
					{
						recipeEntry2.gridIndex = this._filteredRecipes.Count;
						this._filteredRecipes.Add(recipeEntry2);
					}
				}
				recipeEntry2.availableIndex = i;
			}
			if (this.SelectedEntry == null)
			{
				this._selectedRecipeIndex = null;
			}
			else if (this._filter != null && !this._filter.Accepts(this.SelectedEntry.Recipe))
			{
				this._selectedRecipeIndex = null;
			}
			else if (this.SelectedEntry.Available)
			{
				Main.focusRecipe = this.SelectedEntry.availableIndex;
			}
			if (this._itemGrid.Count != this._filteredRecipes.Count || (flag && this._filteredRecipes.Count > 0))
			{
				this._itemGrid.SetContentsToShow(this._filteredRecipes);
			}
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x00575B7C File Offset: 0x00573D7C
		private bool FitsFilter(Recipe recipe)
		{
			return this._filterer.FitsFilter(recipe.createItem) && (this._filter == null || this._filter.Accepts(recipe));
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x00575BA9 File Offset: 0x00573DA9
		private void OnSearchContentsChanged(string contents)
		{
			this._filterer.SetSearchFilter(contents);
			this.ResetRecipes();
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x00575BBD File Offset: 0x00573DBD
		public override void Update(GameTime gameTime)
		{
			if (this._filter != null && !this._filter.CanRemainOpen())
			{
				NewCraftingUI.Close(false, true);
				return;
			}
			base.Update(gameTime);
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x00575BE4 File Offset: 0x00573DE4
		private void SetupGamepadPoints(bool craftSlotVisible, int materialCount)
		{
			UILinkPage uilinkPage = UILinkPointNavigator.Pages[24];
			int num = 20050;
			List<SnapPoint> snapPoints = this.GetSnapPoints();
			UILinkPage uilinkPage2 = UILinkPointNavigator.Pages[0];
			UILinkPoint uilinkPoint = uilinkPage2.LinkMap[300];
			UILinkPoint uilinkPoint2 = (!craftSlotVisible) ? null : uilinkPage.LinkMap[20000];
			UILinkPoint uilinkPoint3 = (!Main.InGuideCraftMenu) ? null : uilinkPage.LinkMap[20020];
			UILinkPoint uilinkPoint4 = (Main.LocalPlayer.difficulty != 3 || Main.CreativeMenu.Blocked) ? null : uilinkPage2.LinkMap[311];
			UILinkPoint uilinkPoint5 = uilinkPage.LinkMap[20030];
			UILinkPoint uilinkPoint6 = Main.InGuideCraftMenu ? null : uilinkPage.LinkMap[20031];
			UILinkPoint uilinkPoint7 = this._helper.MakeLinkPointFromSnapPoint(num++, snapPoints.First((SnapPoint pt) => pt.Name == "NewCraftingUISearch"));
			if (this._gamepadMoveToSearchButtonHack)
			{
				this._gamepadMoveToSearchButtonHack = false;
				UILinkPointNavigator.ChangePoint(uilinkPoint7.ID);
			}
			List<SnapPoint> orderedPointsByCategoryName = this._helper.GetOrderedPointsByCategoryName(snapPoints, "NewCraftingUIFilters");
			UILinkPoint[] array = this._helper.CreateUILinkStripHorizontal(ref num, orderedPointsByCategoryName);
			uilinkPoint7.Up = array[0].ID;
			for (int i = 0; i < array.Length; i++)
			{
				UILinkPoint upSide = (i == 10) ? uilinkPoint : uilinkPage2.LinkMap[40 + (int)Math.Round((double)((float)(i * 10) / 11f))];
				this._helper.PairUpDown(upSide, array[i]);
				array[i].Down = uilinkPoint7.ID;
			}
			int num2 = 0;
			if (uilinkPoint4 != null)
			{
				this._helper.PairUpDown(uilinkPoint4, array[num2]);
				this._helper.PairUpDown(uilinkPage2.LinkMap[40], uilinkPoint4);
				num2++;
			}
			this._helper.PairLeftRight(uilinkPoint4, uilinkPoint5);
			this._helper.PairUpDown(uilinkPoint5, array[num2]);
			this._helper.PairUpDown(uilinkPage2.LinkMap[40 + num2], uilinkPoint5);
			num2++;
			this._helper.PairLeftRight(uilinkPoint5, uilinkPoint6);
			if (uilinkPoint6 != null)
			{
				this._helper.PairUpDown(uilinkPoint6, array[num2]);
				this._helper.PairUpDown(uilinkPage2.LinkMap[40 + num2], uilinkPoint6);
			}
			this._helper.PairLeftRight(uilinkPoint6 ?? uilinkPoint5, uilinkPoint);
			this._helper.PairUpDown(uilinkPoint2, uilinkPoint3);
			this._helper.PairUpDown(uilinkPoint7, uilinkPoint2 ?? uilinkPoint3);
			UILinkPoint uilinkPoint8;
			if ((uilinkPoint8 = uilinkPoint3) == null)
			{
				uilinkPoint8 = (uilinkPoint2 ?? uilinkPoint7);
			}
			UILinkPoint uilinkPoint9 = uilinkPoint8;
			UILinkPoint uilinkPoint10 = null;
			List<SnapPoint> orderedPointsByCategoryName2 = this._helper.GetOrderedPointsByCategoryName(snapPoints, "DynamicItemCollectionSlot");
			if (orderedPointsByCategoryName2.Count > 0)
			{
				int num3 = 20100;
				int itemsPerLine = this._itemGrid.GetItemsPerLine();
				UILinkPoint[,] array2 = this._helper.CreateUILinkPointGrid(ref num3, orderedPointsByCategoryName2, itemsPerLine, uilinkPoint9, null, null, null);
				uilinkPoint10 = array2[0, 0];
				if (this.SelectedEntry != null && this.SelectedEntry.gridIndex >= 0)
				{
					int num4 = this.SelectedEntry.gridIndex - orderedPointsByCategoryName2[0].Id;
					if (num4 >= 0 && num4 < orderedPointsByCategoryName2.Count)
					{
						uilinkPoint10 = array2[num4 % itemsPerLine, num4 / itemsPerLine];
						if (this._gamepadMoveToGridEntryHack)
						{
							UILinkPointNavigator.ChangePoint(uilinkPoint10.ID);
						}
					}
				}
			}
			uilinkPoint9.Down = ((uilinkPoint10 == null) ? -1 : uilinkPoint10.ID);
			this._gamepadMoveToGridEntryHack = false;
			UILinkPoint rightSide = (materialCount == 0) ? null : uilinkPage.LinkMap[20001];
			UILinkPoint uilinkPoint11 = (materialCount == 0) ? null : uilinkPage.LinkMap[20001 + materialCount - 1];
			this._helper.PairLeftRight(uilinkPoint2, rightSide);
			for (int j = 0; j < Recipe.maxRequirements - 1; j++)
			{
				UILinkPoint uilinkPoint12 = uilinkPage.LinkMap[20001 + j];
				this._helper.PairLeftRight(uilinkPoint12, uilinkPage.LinkMap[20001 + j + 1]);
				uilinkPoint12.Down = ((uilinkPoint2 == null) ? -1 : uilinkPoint2.Down);
				uilinkPoint12.Up = ((uilinkPoint2 == null) ? -1 : uilinkPoint2.Up);
			}
			if (uilinkPoint11 != null)
			{
				uilinkPoint11.Right = -1;
			}
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x0057604E File Offset: 0x0057424E
		private static bool IsGridPoint(int point)
		{
			return point >= 20100 && point < 21000;
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x00576062 File Offset: 0x00574262
		private static bool IsMaterialPoint(int point)
		{
			return point >= 20001 && point < 20016;
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x00576078 File Offset: 0x00574278
		private string GetGamepadInstructions()
		{
			if (!NewCraftingUI.Visible)
			{
				return "";
			}
			bool flag = true;
			int num = -1;
			if (UILinkPointNavigator.CurrentPoint == 20000)
			{
				num = 42;
			}
			else if (UILinkPointNavigator.CurrentPoint == 20020)
			{
				num = 7;
			}
			else if (NewCraftingUI.IsMaterialPoint(UILinkPointNavigator.CurrentPoint))
			{
				num = 43;
			}
			else if (this._hoveredEntry != null)
			{
				num = 41;
			}
			else
			{
				flag = false;
			}
			if (num != 42 && num != 43)
			{
				this._gamepadReturnToGridEntry = false;
			}
			string text = "";
			if (this._gamepadReturnToGridEntry)
			{
				text += PlayerInput.BuildCommand(Language.GetTextValue("UI.Back"), new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
				});
				Main.LocalPlayer.releaseInventory = false;
				if (PlayerInput.AllowExecutionOfGamepadInstructions && PlayerInput.Triggers.JustPressed.Inventory)
				{
					this._gamepadMoveToGridEntryHack = true;
				}
			}
			else
			{
				text += PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
				});
			}
			text += PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			});
			if (!flag || this._hoveredEntry != null)
			{
				text += PlayerInput.BuildCommand(Lang.misc[53].Value, new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
				});
				if (this._hoveredEntry != null && this._hoveredEntry.Available && !Main.InGuideCraftMenu)
				{
					string quickCraftGamepadInstructions = ItemSlot.GetQuickCraftGamepadInstructions(this._hoveredEntry.Recipe);
					if (quickCraftGamepadInstructions != null)
					{
						if (PlayerInput.AllowExecutionOfGamepadInstructions && PlayerInput.Triggers.Current.Grapple)
						{
							this._selectedRecipeIndex = new int?(this._hoveredEntry.index);
						}
						text += quickCraftGamepadInstructions;
					}
				}
			}
			else if (num == 42)
			{
				NewCraftingUI.RecipeEntry selectedEntry = this.SelectedEntry;
				if (selectedEntry != null && selectedEntry.Available)
				{
					text += ItemSlot.GetCraftSlotGamepadInstructions();
				}
			}
			if (num == 7)
			{
				text += ItemSlot.GetGamepadInstructions(ref Main.guideItem, num);
			}
			else if (!flag)
			{
				text += ItemSlot.GetGamepadInstructions(43);
			}
			else
			{
				text += ItemSlot.GetGamepadInstructions(num);
			}
			return text;
		}

		// Token: 0x060028EA RID: 10474 RVA: 0x005762E6 File Offset: 0x005744E6
		private void GoBackFromVirtualKeyboard()
		{
			IngameFancyUI.Close(true);
			this._gamepadMoveToSearchButtonHack = true;
		}

		// Token: 0x060028EB RID: 10475 RVA: 0x005762F5 File Offset: 0x005744F5
		public static void Close(bool quiet = false, bool returnToInventory = false)
		{
			if (!NewCraftingUI.Visible)
			{
				return;
			}
			NewCraftingUI._ui.SetState(null);
			Main.PipsFastScroll = true;
			if (!returnToInventory)
			{
				Main.playerInventory = false;
			}
			if (!quiet)
			{
				SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
			}
		}

		// Token: 0x060028EC RID: 10476 RVA: 0x00576334 File Offset: 0x00574534
		public static void Open(bool quiet = false, NewCraftingUI.RecipeFilter filter = null)
		{
			if (NewCraftingUI.Visible)
			{
				return;
			}
			if (!Main.playerInventory || (Main.LocalPlayer.chest == -1 && !Main.InGuideCraftMenu))
			{
				IngameUIWindows.CloseAll(true);
			}
			Main.playerInventory = true;
			Main.PipsCurrentPage = Main.PipPage.Recipes;
			Main._preventCraftingBecauseClickWasUsedToChangeFocusedRecipe = true;
			if (NewCraftingUI._instance == null)
			{
				NewCraftingUI._instance = new NewCraftingUI();
			}
			NewCraftingUI._instance.SetFilter(filter);
			NewCraftingUI._ui.SetState(NewCraftingUI._instance);
			if (!quiet)
			{
				SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			}
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x005763C0 File Offset: 0x005745C0
		public override void OnActivate()
		{
			NewCraftingUI._instance._openedWithoutFilter = (this._filter == null);
			this._selectedRecipeIndex = ((Main.numAvailableRecipes < 0) ? null : new int?(Main.availableRecipe[Main.focusRecipe]));
			this.ResetRecipes();
			this._searchBar.SetContents("", false);
			this._filterer.ActiveFilters.Clear();
			UILinkPointNavigator.ChangePage(24);
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x00576437 File Offset: 0x00574637
		public override void OnDeactivate()
		{
			this._filter = null;
			this._selectedRecipeIndex = null;
			this._hoveredEntry = null;
			this._missingRequirementsTooltipText = null;
			UILinkPointNavigator.ChangePoint(1500);
		}

		// Token: 0x060028EF RID: 10479 RVA: 0x00576464 File Offset: 0x00574664
		public static void ToggleInInventory(bool quiet = false)
		{
			if (NewCraftingUI.Visible)
			{
				NewCraftingUI.Close(quiet, true);
				return;
			}
			NewCraftingUI.Open(quiet, null);
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x0057647C File Offset: 0x0057467C
		public static void OpenCloseFilter(NewCraftingUI.RecipeFilter filter)
		{
			if (!NewCraftingUI.Visible)
			{
				NewCraftingUI.Open(false, filter);
				return;
			}
			if (NewCraftingUI._instance._filter == null || !NewCraftingUI._instance._filter.Matches(filter))
			{
				if (Main.InGuideCraftMenu)
				{
					Main.LocalPlayer.SetTalkNPC(-1);
					Main.InGuideCraftMenu = false;
					Main.LocalPlayer.dropItemCheck();
				}
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				NewCraftingUI._instance.SetFilter(filter);
				return;
			}
			if (NewCraftingUI._instance._openedWithoutFilter)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				NewCraftingUI._instance.SetFilter(null);
				return;
			}
			NewCraftingUI.Close(false, true);
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x0057652D File Offset: 0x0057472D
		private void SetFilter(NewCraftingUI.RecipeFilter filter)
		{
			this._filter = filter;
			this.ResetRecipes();
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x0057653C File Offset: 0x0057473C
		public static void UpdateUI(GameTime gameTime)
		{
			if (!NewCraftingUI.Visible || Main.inFancyUI)
			{
				return;
			}
			NewCraftingUI._ui.Update(gameTime);
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x00576558 File Offset: 0x00574758
		public static void DrawUI(SpriteBatch spriteBatch)
		{
			if (!NewCraftingUI.Visible || Main.inFancyUI)
			{
				return;
			}
			NewCraftingUI._ui.Draw(spriteBatch, Main.gameTimeCache);
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x00576579 File Offset: 0x00574779
		public static void RefreshGrid()
		{
			if (NewCraftingUI.Visible)
			{
				NewCraftingUI._instance.ResetRecipes();
			}
		}

		// Token: 0x04005168 RID: 20840
		private static UserInterface _ui = new UserInterface();

		// Token: 0x04005169 RID: 20841
		private static NewCraftingUI _instance;

		// Token: 0x0400516A RID: 20842
		private bool _openedWithoutFilter;

		// Token: 0x0400516B RID: 20843
		private NewCraftingUI.RecipeFilter _filter;

		// Token: 0x0400516C RID: 20844
		private int? _selectedRecipeIndex;

		// Token: 0x0400516D RID: 20845
		private NewCraftingUI.RecipeEntry _hoveredEntry;

		// Token: 0x0400516E RID: 20846
		private string _missingRequirementsTooltipText;

		// Token: 0x0400516F RID: 20847
		private NewCraftingUI.ItemGrid _itemGrid;

		// Token: 0x04005170 RID: 20848
		private UIText _text;

		// Token: 0x04005171 RID: 20849
		private UIWrappedSearchBar _searchBar;

		// Token: 0x04005172 RID: 20850
		private UIElement _gridContainer;

		// Token: 0x04005173 RID: 20851
		private bool _gamepadMoveToSearchButtonHack;

		// Token: 0x04005174 RID: 20852
		private bool _gamepadMoveToGridEntryHack;

		// Token: 0x04005175 RID: 20853
		private bool _gamepadReturnToGridEntry;

		// Token: 0x04005176 RID: 20854
		private EntryFilterer<Item, IItemEntryFilter> _filterer;

		// Token: 0x04005177 RID: 20855
		public const string SnapPointName_Search = "NewCraftingUISearch";

		// Token: 0x04005178 RID: 20856
		public const string SnapPointName_Filters = "NewCraftingUIFilters";

		// Token: 0x04005179 RID: 20857
		private static List<string> _missingObjects = new List<string>();

		// Token: 0x0400517A RID: 20858
		private static readonly Color DisabledSlotColor = new Color(160, 160, 160, 255);

		// Token: 0x0400517B RID: 20859
		private List<NewCraftingUI.RecipeEntry> _recipes = new List<NewCraftingUI.RecipeEntry>(Recipe.maxRecipes);

		// Token: 0x0400517C RID: 20860
		private List<NewCraftingUI.RecipeEntry> _filteredRecipes = new List<NewCraftingUI.RecipeEntry>(Recipe.maxRecipes);

		// Token: 0x0400517D RID: 20861
		private NewCraftingUI.RecipeEntry[] _recipeListLookup;

		// Token: 0x0400517E RID: 20862
		private Item _resetForGuideItem;

		// Token: 0x0400517F RID: 20863
		private UIGamepadHelper _helper;

		// Token: 0x020008C9 RID: 2249
		private class RecipeEntry
		{
			// Token: 0x06004647 RID: 17991 RVA: 0x006C5150 File Offset: 0x006C3350
			public RecipeEntry(int index)
			{
				this.index = index;
			}

			// Token: 0x17000567 RID: 1383
			// (get) Token: 0x06004648 RID: 17992 RVA: 0x006C516D File Offset: 0x006C336D
			public bool Available
			{
				get
				{
					return this.availableIndex >= 0;
				}
			}

			// Token: 0x17000568 RID: 1384
			// (get) Token: 0x06004649 RID: 17993 RVA: 0x006C517B File Offset: 0x006C337B
			public Recipe Recipe
			{
				get
				{
					return Main.recipe[this.index];
				}
			}

			// Token: 0x04007326 RID: 29478
			public readonly int index;

			// Token: 0x04007327 RID: 29479
			public int availableIndex = -1;

			// Token: 0x04007328 RID: 29480
			public int gridIndex = -1;
		}

		// Token: 0x020008CA RID: 2250
		private class ItemGrid : UIDynamicItemCollection<NewCraftingUI.RecipeEntry>
		{
			// Token: 0x0600464A RID: 17994 RVA: 0x006C5189 File Offset: 0x006C3389
			public ItemGrid(NewCraftingUI parent)
			{
				this.parent = parent;
			}

			// Token: 0x0600464B RID: 17995 RVA: 0x006C5198 File Offset: 0x006C3398
			protected override Item GetItem(NewCraftingUI.RecipeEntry entry)
			{
				return entry.Recipe.createItem;
			}

			// Token: 0x0600464C RID: 17996 RVA: 0x006C51A8 File Offset: 0x006C33A8
			protected override void DrawSlot(SpriteBatch spriteBatch, NewCraftingUI.RecipeEntry entry, Vector2 pos, bool hovering)
			{
				Item createItem = entry.Recipe.createItem;
				int context = 41;
				if (PlayerInput.UsingGamepad && NewCraftingUI.IsGridPoint(UILinkPointNavigator.CurrentPoint))
				{
					ItemSlot.DrawSelectionHighlightForGridSlot = false;
					UILinkPointNavigator.Shortcuts.ItemSlotShouldHighlightAsSelected = hovering;
				}
				else
				{
					int index = entry.index;
					int? selectedRecipeIndex = this.parent._selectedRecipeIndex;
					ItemSlot.DrawSelectionHighlightForGridSlot = (index == selectedRecipeIndex.GetValueOrDefault() & selectedRecipeIndex != null);
					UILinkPointNavigator.Shortcuts.ItemSlotShouldHighlightAsSelected = false;
				}
				if (hovering)
				{
					this.parent._hoveredEntry = entry;
					this.parent.HandleCraftSlot(entry, 41);
				}
				ItemSlot.Draw(spriteBatch, ref createItem, context, pos, entry.Available ? Color.White : NewCraftingUI.DisabledSlotColor);
			}

			// Token: 0x04007329 RID: 29481
			private readonly NewCraftingUI parent;
		}

		// Token: 0x020008CB RID: 2251
		public interface RecipeFilter
		{
			// Token: 0x0600464D RID: 17997
			string GetWindowDescription();

			// Token: 0x0600464E RID: 17998
			bool Accepts(Recipe recipe);

			// Token: 0x0600464F RID: 17999
			bool CanRemainOpen();

			// Token: 0x06004650 RID: 18000
			bool Matches(NewCraftingUI.RecipeFilter other);
		}

		// Token: 0x020008CC RID: 2252
		public abstract class TileBasedRecipeFilter : NewCraftingUI.RecipeFilter
		{
			// Token: 0x06004651 RID: 18001 RVA: 0x006C5250 File Offset: 0x006C3450
			public TileBasedRecipeFilter(int tileType, int tileStyle)
			{
				this.tileType = tileType;
				this.tileStyle = tileStyle;
			}

			// Token: 0x06004652 RID: 18002 RVA: 0x006C5268 File Offset: 0x006C3468
			public string GetWindowDescription()
			{
				string mapObjectName = Lang.GetMapObjectName(MapHelper.TileToLookup(this.tileType, this.tileStyle));
				return Language.GetTextValue("CombineFormat.Crafting", mapObjectName);
			}

			// Token: 0x06004653 RID: 18003
			public abstract bool Accepts(Recipe recipe);

			// Token: 0x06004654 RID: 18004 RVA: 0x006C5297 File Offset: 0x006C3497
			public bool CanRemainOpen()
			{
				return Main.LocalPlayer.adjTile[this.tileType];
			}

			// Token: 0x06004655 RID: 18005 RVA: 0x006C52AA File Offset: 0x006C34AA
			public bool Matches(NewCraftingUI.RecipeFilter other)
			{
				return other is NewCraftingUI.TileBasedRecipeFilter && NewCraftingUI.TileBasedRecipeFilter.Matches(this, (NewCraftingUI.TileBasedRecipeFilter)other);
			}

			// Token: 0x06004656 RID: 18006 RVA: 0x006C52C2 File Offset: 0x006C34C2
			private static bool Matches(NewCraftingUI.TileBasedRecipeFilter a, NewCraftingUI.TileBasedRecipeFilter b)
			{
				return a.tileType == b.tileType && a.tileStyle == b.tileStyle;
			}

			// Token: 0x0400732A RID: 29482
			public readonly int tileType;

			// Token: 0x0400732B RID: 29483
			public readonly int tileStyle;
		}

		// Token: 0x020008CD RID: 2253
		public class CraftStationRecipeFilter : NewCraftingUI.TileBasedRecipeFilter
		{
			// Token: 0x06004657 RID: 18007 RVA: 0x006C52E2 File Offset: 0x006C34E2
			public CraftStationRecipeFilter(int tileType, int tileStyle) : base(tileType, tileStyle)
			{
				this.acceptTileTypes = new bool[(int)TileID.Count];
				this.AcceptTileType(tileType);
			}

			// Token: 0x06004658 RID: 18008 RVA: 0x006C5304 File Offset: 0x006C3504
			private void AcceptTileType(int tileType)
			{
				this.acceptTileTypes[tileType] = true;
				List<int> list = Recipe.TileCountsAs[tileType];
				if (list != null)
				{
					foreach (int tileType2 in list)
					{
						this.AcceptTileType(tileType2);
					}
				}
			}

			// Token: 0x06004659 RID: 18009 RVA: 0x006C5368 File Offset: 0x006C3568
			public override bool Accepts(Recipe recipe)
			{
				return recipe.DoesNotNeedTileOrLiquid || (recipe.requiredTile >= 0 && this.acceptTileTypes[recipe.requiredTile]);
			}

			// Token: 0x0400732C RID: 29484
			private bool[] acceptTileTypes;
		}

		// Token: 0x020008CE RID: 2254
		public class WaterSourceRecipeFilter : NewCraftingUI.TileBasedRecipeFilter
		{
			// Token: 0x0600465A RID: 18010 RVA: 0x006C538C File Offset: 0x006C358C
			public WaterSourceRecipeFilter(int tileType, int tileStyle) : base(tileType, tileStyle)
			{
			}

			// Token: 0x0600465B RID: 18011 RVA: 0x006C5396 File Offset: 0x006C3596
			public override bool Accepts(Recipe recipe)
			{
				return recipe.DoesNotNeedTileOrLiquid || recipe.needWater;
			}
		}
	}
}
