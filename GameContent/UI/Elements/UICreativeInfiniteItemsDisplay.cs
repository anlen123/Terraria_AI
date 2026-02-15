using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003CC RID: 972
	public class UICreativeInfiniteItemsDisplay : UIElement
	{
		// Token: 0x06002D65 RID: 11621 RVA: 0x005A30B0 File Offset: 0x005A12B0
		public UICreativeInfiniteItemsDisplay()
		{
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
			this._sorter = new EntrySorter<Item, ICreativeItemSortStep>();
			this._sorter.AddSortSteps(new List<ICreativeItemSortStep>
			{
				new SortingSteps.ByUnlockStatus(),
				new SortingSteps.ByCreativeSortingId(),
				new SortingSteps.Alphabetical()
			});
			this.BuildPage();
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x005A31EC File Offset: 0x005A13EC
		private void BuildPage()
		{
			this._lastCheckedVersionForEdits = -1;
			base.RemoveAllChildren();
			base.SetPadding(0f);
			UIElement uielement = new UIElement
			{
				Width = StyleDimension.Fill,
				Height = StyleDimension.Fill
			};
			uielement.SetPadding(0f);
			this._containerInfinites = uielement;
			UIElement uielement2 = new UIElement
			{
				Width = StyleDimension.Fill,
				Height = StyleDimension.Fill
			};
			uielement2.SetPadding(0f);
			this._containerSacrifice = uielement2;
			this.BuildInfinitesMenuContents(uielement);
			this.BuildSacrificeMenuContents(uielement2);
			this.UpdateContents();
			base.OnUpdate += this.UICreativeInfiniteItemsDisplay_OnUpdate;
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x005A3293 File Offset: 0x005A1493
		private void Hover_OnUpdate(UIElement affectedElement)
		{
			if (this._hovered)
			{
				Main.LocalPlayer.mouseInterface = true;
			}
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x005A32A8 File Offset: 0x005A14A8
		private void Hover_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			this._hovered = false;
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x005A32B1 File Offset: 0x005A14B1
		private void Hover_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			this._hovered = true;
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x005A32BA File Offset: 0x005A14BA
		private static UIPanel CreateBasicPanel()
		{
			UIPanel uipanel = new UIPanel();
			UICreativeInfiniteItemsDisplay.SetBasicSizesForCreativeSacrificeOrInfinitesPanel(uipanel);
			uipanel.BackgroundColor *= 0.8f;
			uipanel.BorderColor *= 0.8f;
			return uipanel;
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x005A32F4 File Offset: 0x005A14F4
		private static void SetBasicSizesForCreativeSacrificeOrInfinitesPanel(UIElement element)
		{
			element.Width = new StyleDimension(0f, 1f);
			element.Height = new StyleDimension(-38f, 1f);
			element.Top = new StyleDimension(38f, 0f);
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x005A3340 File Offset: 0x005A1540
		private void BuildInfinitesMenuContents(UIElement totalContainer)
		{
			UIPanel uipanel = UICreativeInfiniteItemsDisplay.CreateBasicPanel();
			totalContainer.Append(uipanel);
			uipanel.OnUpdate += this.Hover_OnUpdate;
			uipanel.OnMouseOver += this.Hover_OnMouseOver;
			uipanel.OnMouseOut += this.Hover_OnMouseOut;
			this._itemGrid = new UICreativeItemGrid();
			UIWrappedSearchBar uiwrappedSearchBar = new UIWrappedSearchBar(new Action(this.GoBackFromVirtualKeyboard), null, UIWrappedSearchBar.ColorTheme.Blue);
			uiwrappedSearchBar.CustomOpenVirtualKeyboard = new Action<UIState>(IngameFancyUI.OpenUIState);
			uiwrappedSearchBar.OnSearchContentsChanged += this.OnSearchContentsChanged;
			uiwrappedSearchBar.SetSearchSnapPoint("CreativeInfinitesSearch", 0, null, null);
			uipanel.Append(uiwrappedSearchBar);
			UIList uilist = new UIList
			{
				Width = new StyleDimension(-25f, 1f),
				Height = new StyleDimension(-28f, 1f),
				VAlign = 1f,
				HAlign = 0f
			};
			uipanel.Append(uilist);
			float num = 4f;
			UIScrollbar uiscrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Blue)
			{
				Height = new StyleDimension(-28f - num * 2f, 1f),
				Top = new StyleDimension(-num, 0f),
				VAlign = 1f,
				HAlign = 1f
			};
			uipanel.Append(uiscrollbar);
			uilist.SetScrollbar(uiscrollbar);
			uilist.Add(this._itemGrid);
			UICreativeItemsInfiniteFilteringOptions uicreativeItemsInfiniteFilteringOptions = new UICreativeItemsInfiniteFilteringOptions(this._filterer, "CreativeInfinitesFilter", UICreativeItemsInfiniteFilteringOptions.ColorTheme.Blue);
			uicreativeItemsInfiniteFilteringOptions.OnClickingOption += this.filtersHelper_OnClickingOption;
			uicreativeItemsInfiniteFilteringOptions.Left = new StyleDimension(20f, 0f);
			totalContainer.Append(uicreativeItemsInfiniteFilteringOptions);
			uicreativeItemsInfiniteFilteringOptions.OnUpdate += this.Hover_OnUpdate;
			uicreativeItemsInfiniteFilteringOptions.OnMouseOver += this.Hover_OnMouseOver;
			uicreativeItemsInfiniteFilteringOptions.OnMouseOut += this.Hover_OnMouseOut;
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x005A3534 File Offset: 0x005A1734
		private void BuildSacrificeMenuContents(UIElement totalContainer)
		{
			UIPanel uipanel = UICreativeInfiniteItemsDisplay.CreateBasicPanel();
			uipanel.VAlign = 0.5f;
			uipanel.Height = new StyleDimension(170f, 0f);
			uipanel.Width = new StyleDimension(170f, 0f);
			uipanel.Top = default(StyleDimension);
			totalContainer.Append(uipanel);
			uipanel.OnUpdate += this.Hover_OnUpdate;
			uipanel.OnMouseOver += this.Hover_OnMouseOver;
			uipanel.OnMouseOut += this.Hover_OnMouseOut;
			this.AddCogsForSacrificeMenu(uipanel);
			this._pistonParticleAsset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Research_Spark", 1);
			float pixels = 0f;
			UIImage uiimage = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Creative/Research_Slots", 1))
			{
				HAlign = 0.5f,
				VAlign = 0.5f,
				Top = new StyleDimension(-20f, 0f),
				Left = new StyleDimension(pixels, 0f)
			};
			uipanel.Append(uiimage);
			Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Research_FramedPistons", 1);
			UIImageFramed uiimageFramed = new UIImageFramed(asset, asset.Frame(1, 9, 0, 0, 0, 0))
			{
				HAlign = 0.5f,
				VAlign = 0.5f,
				Top = new StyleDimension(-20f, 0f),
				Left = new StyleDimension(pixels, 0f),
				IgnoresMouseInteraction = true
			};
			uipanel.Append(uiimageFramed);
			this._sacrificePistons = uiimageFramed;
			UIParticleLayer pistonParticleSystem = new UIParticleLayer
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(0f, 1f),
				AnchorPositionOffsetByPercents = Vector2.One / 2f,
				AnchorPositionOffsetByPixels = Vector2.Zero
			};
			this._pistonParticleSystem = pistonParticleSystem;
			uiimageFramed.Append(this._pistonParticleSystem);
			UIElement uielement = Main.CreativeMenu.ProvideItemSlotElement(0);
			uielement.HAlign = 0.5f;
			uielement.VAlign = 0.5f;
			uielement.Top = new StyleDimension(-15f, 0f);
			uielement.Left = new StyleDimension(pixels, 0f);
			uielement.SetSnapPoint("CreativeSacrificeSlot", 0, null, null);
			uiimage.Append(uielement);
			UIText uitext = new UIText("(0/50)", 0.8f, false)
			{
				Top = new StyleDimension(10f, 0f),
				Left = new StyleDimension(pixels, 0f),
				HAlign = 0.5f,
				VAlign = 0.5f,
				IgnoresMouseInteraction = true
			};
			uitext.OnUpdate += this.descriptionText_OnUpdate;
			uipanel.Append(uitext);
			UIPanel uipanel2 = new UIPanel
			{
				Top = new StyleDimension(0f, 0f),
				Left = new StyleDimension(pixels, 0f),
				HAlign = 0.5f,
				VAlign = 1f,
				Width = new StyleDimension(124f, 0f),
				Height = new StyleDimension(30f, 0f)
			};
			UIText element = new UIText(Language.GetText("CreativePowers.ConfirmInfiniteItemSacrifice"), 0.8f, false)
			{
				IgnoresMouseInteraction = true,
				HAlign = 0.5f,
				VAlign = 0.5f
			};
			uipanel2.Append(element);
			uipanel2.SetSnapPoint("CreativeSacrificeConfirm", 0, null, null);
			uipanel2.OnLeftClick += this.sacrificeButton_OnClick;
			uipanel2.OnMouseOver += this.FadedMouseOver;
			uipanel2.OnMouseOut += this.FadedMouseOut;
			uipanel2.OnUpdate += this.research_OnUpdate;
			uipanel.Append(uipanel2);
			uipanel.OnUpdate += this.sacrificeWindow_OnUpdate;
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x005A3934 File Offset: 0x005A1B34
		private void research_OnUpdate(UIElement affectedElement)
		{
			if (affectedElement.IsMouseHovering)
			{
				Main.instance.MouseTextNoOverride(Language.GetTextValue("CreativePowers.ResearchButtonTooltip"), 0, 0, -1, -1, -1, -1, 0);
			}
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x005A3964 File Offset: 0x005A1B64
		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x005A3990 File Offset: 0x005A1B90
		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x005A39A8 File Offset: 0x005A1BA8
		private void AddCogsForSacrificeMenu(UIElement sacrificesContainer)
		{
			UIElement uielement = new UIElement();
			uielement.IgnoresMouseInteraction = true;
			UICreativeInfiniteItemsDisplay.SetBasicSizesForCreativeSacrificeOrInfinitesPanel(uielement);
			uielement.VAlign = 0.5f;
			uielement.Height = new StyleDimension(170f, 0f);
			uielement.Width = new StyleDimension(280f, 0f);
			uielement.Top = default(StyleDimension);
			uielement.SetPadding(0f);
			sacrificesContainer.Append(uielement);
			Vector2 value = new Vector2(-10f, -10f);
			this.AddSymetricalCogsPair(uielement, new Vector2(22f, 1f) + value, "Images/UI/Creative/Research_GearC", this._sacrificeCogsSmall);
			this.AddSymetricalCogsPair(uielement, new Vector2(1f, 28f) + value, "Images/UI/Creative/Research_GearB", this._sacrificeCogsMedium);
			this.AddSymetricalCogsPair(uielement, new Vector2(5f, 5f) + value, "Images/UI/Creative/Research_GearA", this._sacrificeCogsBig);
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x005A3AA1 File Offset: 0x005A1CA1
		private void sacrificeWindow_OnUpdate(UIElement affectedElement)
		{
			this.UpdateVisualFrame();
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x005A3AAC File Offset: 0x005A1CAC
		private void UpdateVisualFrame()
		{
			float num = 0.05f;
			float sacrificeAnimationProgress = this.GetSacrificeAnimationProgress();
			float lerpValue = Utils.GetLerpValue(1f, 0.7f, sacrificeAnimationProgress, true);
			float num2 = lerpValue * lerpValue;
			num2 *= 2f;
			float num3 = 1f + num2;
			num *= num3;
			float num4 = 2f;
			float num5 = 1.1428572f;
			float num6 = 1f;
			UICreativeInfiniteItemsDisplay.OffsetRotationsForCogs(num4 * num, this._sacrificeCogsSmall);
			UICreativeInfiniteItemsDisplay.OffsetRotationsForCogs(num5 * num, this._sacrificeCogsMedium);
			UICreativeInfiniteItemsDisplay.OffsetRotationsForCogs(-num6 * num, this._sacrificeCogsBig);
			int frameY = 0;
			if (this._sacrificeAnimationTimeLeft != 0)
			{
				float num7 = 0.1f;
				float num8 = 0.06666667f;
				if (sacrificeAnimationProgress >= 1f - num7)
				{
					frameY = 8;
				}
				else if (sacrificeAnimationProgress >= 1f - num7 * 2f)
				{
					frameY = 7;
				}
				else if (sacrificeAnimationProgress >= 1f - num7 * 3f)
				{
					frameY = 6;
				}
				else if (sacrificeAnimationProgress >= num8 * 4f)
				{
					frameY = 5;
				}
				else if (sacrificeAnimationProgress >= num8 * 3f)
				{
					frameY = 4;
				}
				else if (sacrificeAnimationProgress >= num8 * 2f)
				{
					frameY = 3;
				}
				else if (sacrificeAnimationProgress >= num8)
				{
					frameY = 2;
				}
				else
				{
					frameY = 1;
				}
				if (this._sacrificeAnimationTimeLeft == 56)
				{
					SoundEngine.PlaySound(63, -1, -1, 1, 1f, 0f);
					Vector2 accelerationPerFrame = new Vector2(0f, 0.16350001f);
					for (int i = 0; i < 15; i++)
					{
						Vector2 vector = Main.rand.NextVector2Circular(4f, 3f);
						if (vector.Y > 0f)
						{
							vector.Y = -vector.Y;
						}
						vector.Y -= 2f;
						this._pistonParticleSystem.AddParticle(new CreativeSacrificeParticle(this._pistonParticleAsset, null, vector, Vector2.Zero)
						{
							AccelerationPerFrame = accelerationPerFrame,
							ScaleOffsetPerFrame = -0.016666668f
						});
					}
				}
				if (this._sacrificeAnimationTimeLeft == 40 && this._researchComplete)
				{
					this._researchComplete = false;
					SoundEngine.PlaySound(64, -1, -1, 1, 1f, 0f);
				}
			}
			this._sacrificePistons.SetFrame(1, 9, 0, frameY, 0, 0);
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x005A3CCC File Offset: 0x005A1ECC
		private static void OffsetRotationsForCogs(float rotationOffset, List<UIImage> cogsList)
		{
			cogsList[0].Rotation += rotationOffset;
			cogsList[1].Rotation -= rotationOffset;
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x005A3CF8 File Offset: 0x005A1EF8
		private void AddSymetricalCogsPair(UIElement sacrificesContainer, Vector2 cogOFfsetsInPixels, string assetPath, List<UIImage> imagesList)
		{
			Asset<Texture2D> asset = Main.Assets.Request<Texture2D>(assetPath, 1);
			cogOFfsetsInPixels += -asset.Size() / 2f;
			UIImage uiimage = new UIImage(asset)
			{
				NormalizedOrigin = Vector2.One / 2f,
				Left = new StyleDimension(cogOFfsetsInPixels.X, 0f),
				Top = new StyleDimension(cogOFfsetsInPixels.Y, 0f)
			};
			imagesList.Add(uiimage);
			sacrificesContainer.Append(uiimage);
			uiimage = new UIImage(asset)
			{
				NormalizedOrigin = Vector2.One / 2f,
				HAlign = 1f,
				Left = new StyleDimension(-cogOFfsetsInPixels.X, 0f),
				Top = new StyleDimension(cogOFfsetsInPixels.Y, 0f)
			};
			imagesList.Add(uiimage);
			sacrificesContainer.Append(uiimage);
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x005A3DEC File Offset: 0x005A1FEC
		private void descriptionText_OnUpdate(UIElement affectedElement)
		{
			UIText uitext = affectedElement as UIText;
			int num;
			int num2;
			int num3;
			bool sacrificeNumbers = Main.CreativeMenu.GetSacrificeNumbers(out num, out num2, out num3);
			Main.CreativeMenu.ShouldDrawSacrificeArea();
			if (!Main.mouseItem.IsAir)
			{
				this.ForgetItemSacrifice();
			}
			if (num == 0)
			{
				if (this._lastItemIdSacrificed != 0 && this._lastItemAmountWeNeededTotal != this._lastItemAmountWeHad)
				{
					uitext.SetText(string.Format("({0}/{1})", this._lastItemAmountWeHad, this._lastItemAmountWeNeededTotal));
					return;
				}
				uitext.SetText("???");
				return;
			}
			else
			{
				this.ForgetItemSacrifice();
				if (!sacrificeNumbers)
				{
					uitext.SetText("X");
					return;
				}
				uitext.SetText(string.Format("({0}/{1})", num2, num3));
				return;
			}
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x005A3EAD File Offset: 0x005A20AD
		private void sacrificeButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			this.SacrificeWhatYouCan();
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x005A3EB8 File Offset: 0x005A20B8
		public void SacrificeWhatYouCan()
		{
			int itemId;
			int num;
			int amountWeNeedTotal;
			Main.CreativeMenu.GetSacrificeNumbers(out itemId, out num, out amountWeNeedTotal);
			int num2;
			CreativeUI.ItemSacrificeResult itemSacrificeResult = Main.CreativeMenu.SacrificeItem(out num2);
			if (itemSacrificeResult != CreativeUI.ItemSacrificeResult.SacrificedButNotDone)
			{
				if (itemSacrificeResult == CreativeUI.ItemSacrificeResult.SacrificedAndDone)
				{
					this._researchComplete = true;
					this.BeginSacrificeAnimation();
					this.RememberItemSacrifice(itemId, num + num2, amountWeNeedTotal);
					return;
				}
			}
			else
			{
				this._researchComplete = false;
				this.BeginSacrificeAnimation();
				this.RememberItemSacrifice(itemId, num + num2, amountWeNeedTotal);
			}
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x005A3F1F File Offset: 0x005A211F
		public void StopPlayingAnimation()
		{
			this.ForgetItemSacrifice();
			this._sacrificeAnimationTimeLeft = 0;
			this._pistonParticleSystem.ClearParticles();
			this.UpdateVisualFrame();
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x005A3F3F File Offset: 0x005A213F
		private void RememberItemSacrifice(int itemId, int amountWeHave, int amountWeNeedTotal)
		{
			this._lastItemIdSacrificed = itemId;
			this._lastItemAmountWeHad = amountWeHave;
			this._lastItemAmountWeNeededTotal = amountWeNeedTotal;
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x005A3F56 File Offset: 0x005A2156
		private void ForgetItemSacrifice()
		{
			this._lastItemIdSacrificed = 0;
			this._lastItemAmountWeHad = 0;
			this._lastItemAmountWeNeededTotal = 0;
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x005A3F6D File Offset: 0x005A216D
		private void BeginSacrificeAnimation()
		{
			this._sacrificeAnimationTimeLeft = 60;
		}

		// Token: 0x06002D7D RID: 11645 RVA: 0x005A3F77 File Offset: 0x005A2177
		private void UpdateSacrificeAnimation()
		{
			if (this._sacrificeAnimationTimeLeft > 0)
			{
				this._sacrificeAnimationTimeLeft--;
			}
		}

		// Token: 0x06002D7E RID: 11646 RVA: 0x005A3F90 File Offset: 0x005A2190
		private float GetSacrificeAnimationProgress()
		{
			return Utils.GetLerpValue(60f, 0f, (float)this._sacrificeAnimationTimeLeft, true);
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x005A3FA9 File Offset: 0x005A21A9
		public void SetPageTypeToShow(UICreativeInfiniteItemsDisplay.InfiniteItemsDisplayPage page)
		{
			this._showSacrificesInsteadOfInfinites = (page == UICreativeInfiniteItemsDisplay.InfiniteItemsDisplayPage.InfiniteItemsResearch);
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x005A3FB8 File Offset: 0x005A21B8
		private void UICreativeInfiniteItemsDisplay_OnUpdate(UIElement affectedElement)
		{
			base.RemoveAllChildren();
			CreativeUnlocksTracker localPlayerCreativeTracker = Main.LocalPlayerCreativeTracker;
			if (this._lastTrackerCheckedForEdits != localPlayerCreativeTracker)
			{
				this._lastTrackerCheckedForEdits = localPlayerCreativeTracker;
				this._lastCheckedVersionForEdits = -1;
			}
			int lastEditId = localPlayerCreativeTracker.ItemSacrifices.LastEditId;
			if (this._lastCheckedVersionForEdits != lastEditId)
			{
				this._lastCheckedVersionForEdits = lastEditId;
				this.UpdateContents();
			}
			if (this._showSacrificesInsteadOfInfinites)
			{
				base.Append(this._containerSacrifice);
			}
			else
			{
				base.Append(this._containerInfinites);
			}
			this.UpdateSacrificeAnimation();
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x005A4032 File Offset: 0x005A2232
		private void filtersHelper_OnClickingOption()
		{
			this.UpdateContents();
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x005A403C File Offset: 0x005A223C
		private void UpdateContents()
		{
			this._itemList.Clear();
			Main.LocalPlayerCreativeTracker.ItemSacrifices.ForEachItemWithResearchProgress(delegate(int type)
			{
				Item item = ContentSamples.ItemsByType[type];
				if (this._filterer.FitsFilter(item))
				{
					this._itemList.Add(item);
				}
			});
			this._itemList.Sort(this._sorter);
			this._itemGrid.SetContentsToShow(this._itemList);
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x005A4091 File Offset: 0x005A2291
		private void OnSearchContentsChanged(string contents)
		{
			this._filterer.SetSearchFilter(contents);
			this.UpdateContents();
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x005A40A8 File Offset: 0x005A22A8
		private static UserInterface GetCurrentInterface()
		{
			UserInterface result = UserInterface.ActiveInstance;
			if (Main.gameMenu)
			{
				result = Main.MenuUI;
			}
			else
			{
				result = Main.InGameUI;
			}
			return result;
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x005A40D1 File Offset: 0x005A22D1
		private void GoBackFromVirtualKeyboard()
		{
			IngameFancyUI.Close(true);
			Main.playerInventory = true;
			Main.CreativeMenu.ResumeMenuFromGamepadSearch();
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x005A40E9 File Offset: 0x005A22E9
		public int GetItemsPerLine()
		{
			return this._itemGrid.GetItemsPerLine();
		}

		// Token: 0x040054A6 RID: 21670
		private CreativeUnlocksTracker _lastTrackerCheckedForEdits;

		// Token: 0x040054A7 RID: 21671
		private int _lastCheckedVersionForEdits = -1;

		// Token: 0x040054A8 RID: 21672
		private UICreativeItemGrid _itemGrid;

		// Token: 0x040054A9 RID: 21673
		private EntryFilterer<Item, IItemEntryFilter> _filterer;

		// Token: 0x040054AA RID: 21674
		private EntrySorter<Item, ICreativeItemSortStep> _sorter;

		// Token: 0x040054AB RID: 21675
		private UIElement _containerInfinites;

		// Token: 0x040054AC RID: 21676
		private UIElement _containerSacrifice;

		// Token: 0x040054AD RID: 21677
		private bool _showSacrificesInsteadOfInfinites;

		// Token: 0x040054AE RID: 21678
		public const string SnapPointName_SacrificeSlot = "CreativeSacrificeSlot";

		// Token: 0x040054AF RID: 21679
		public const string SnapPointName_SacrificeConfirmButton = "CreativeSacrificeConfirm";

		// Token: 0x040054B0 RID: 21680
		public const string SnapPointName_InfinitesFilter = "CreativeInfinitesFilter";

		// Token: 0x040054B1 RID: 21681
		public const string SnapPointName_InfinitesSearch = "CreativeInfinitesSearch";

		// Token: 0x040054B2 RID: 21682
		private List<UIImage> _sacrificeCogsSmall = new List<UIImage>();

		// Token: 0x040054B3 RID: 21683
		private List<UIImage> _sacrificeCogsMedium = new List<UIImage>();

		// Token: 0x040054B4 RID: 21684
		private List<UIImage> _sacrificeCogsBig = new List<UIImage>();

		// Token: 0x040054B5 RID: 21685
		private UIImageFramed _sacrificePistons;

		// Token: 0x040054B6 RID: 21686
		private UIParticleLayer _pistonParticleSystem;

		// Token: 0x040054B7 RID: 21687
		private Asset<Texture2D> _pistonParticleAsset;

		// Token: 0x040054B8 RID: 21688
		private int _sacrificeAnimationTimeLeft;

		// Token: 0x040054B9 RID: 21689
		private bool _researchComplete;

		// Token: 0x040054BA RID: 21690
		private bool _hovered;

		// Token: 0x040054BB RID: 21691
		private int _lastItemIdSacrificed;

		// Token: 0x040054BC RID: 21692
		private int _lastItemAmountWeHad;

		// Token: 0x040054BD RID: 21693
		private int _lastItemAmountWeNeededTotal;

		// Token: 0x040054BE RID: 21694
		private List<Item> _itemList = new List<Item>();

		// Token: 0x02000924 RID: 2340
		public enum InfiniteItemsDisplayPage
		{
			// Token: 0x040074B9 RID: 29881
			InfiniteItemsPickup,
			// Token: 0x040074BA RID: 29882
			InfiniteItemsResearch
		}
	}
}
