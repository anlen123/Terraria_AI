using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003A9 RID: 937
	public class UIBestiaryTest : UIState
	{
		// Token: 0x06002AF7 RID: 10999 RVA: 0x005896F0 File Offset: 0x005878F0
		public UIBestiaryTest(BestiaryDatabase database)
		{
			this._filterer.SetSearchFilterObject<Filters.BySearch>(new Filters.BySearch());
			this._originalEntriesList = new List<BestiaryEntry>(database.Entries);
			this._workingSetEntries = new List<BestiaryEntry>(this._originalEntriesList);
			this._filterer.AddFilters(database.Filters);
			this._sorter.AddSortSteps(database.SortSteps);
			this.BuildPage();
		}

		// Token: 0x06002AF8 RID: 11000 RVA: 0x00589773 File Offset: 0x00587973
		public void OnOpenPage()
		{
			this.UpdateBestiaryContents();
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x0058977C File Offset: 0x0058797C
		private void BuildPage()
		{
			base.RemoveAllChildren();
			int num = true.ToInt() * 100;
			UIElement uielement = new UIElement();
			uielement.Width.Set(0f, 0.875f);
			uielement.MaxWidth.Set(800f + (float)num, 0f);
			uielement.MinWidth.Set(600f + (float)num, 0f);
			uielement.Top.Set(180f, 0f);
			uielement.Height.Set(-220f, 1f);
			uielement.HAlign = 0.5f;
			base.Append(uielement);
			this.MakeExitButton(uielement);
			UIPanel uipanel = new UIPanel();
			uipanel.Width.Set(0f, 1f);
			uipanel.Height.Set(-90f, 1f);
			uipanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
			uielement.Append(uipanel);
			uipanel.PaddingTop -= 4f;
			uipanel.PaddingBottom -= 4f;
			int num2 = 24;
			UIElement uielement2 = new UIElement
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension((float)num2, 0f),
				VAlign = 0f
			};
			uielement2.SetPadding(0f);
			uipanel.Append(uielement2);
			UIBestiaryEntryInfoPage uibestiaryEntryInfoPage = new UIBestiaryEntryInfoPage
			{
				Height = new StyleDimension(12f, 1f),
				HAlign = 1f
			};
			this.AddSortAndFilterButtons(uielement2, uibestiaryEntryInfoPage);
			UIWrappedSearchBar uiwrappedSearchBar = new UIWrappedSearchBar(new Action(this.GoBackFromVirtualKeyboard), null, UIWrappedSearchBar.ColorTheme.Blue)
			{
				Width = new StyleDimension(uibestiaryEntryInfoPage.Width.Pixels, 0f),
				HAlign = 1f
			};
			uiwrappedSearchBar.CustomOpenVirtualKeyboard = new Action<UIState>(IngameFancyUI.OpenUIState);
			uiwrappedSearchBar.OnSearchContentsChanged += this.OnSearchContentsChanged;
			uiwrappedSearchBar.SetSearchSnapPoint("SearchButton", 0, null, null);
			uielement2.Append(uiwrappedSearchBar);
			int num3 = 20;
			UIElement uielement3 = new UIElement
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension((float)(-(float)num2 - 6 - num3), 1f),
				VAlign = 1f,
				Top = new StyleDimension((float)(-(float)num3), 0f)
			};
			uielement3.SetPadding(0f);
			uipanel.Append(uielement3);
			UIElement uielement4 = new UIElement
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(20f, 0f),
				VAlign = 1f
			};
			uipanel.Append(uielement4);
			uielement4.SetPadding(0f);
			this.FillProgressBottomBar(uielement4);
			UIElement uielement5 = new UIElement
			{
				Width = new StyleDimension(-12f - uibestiaryEntryInfoPage.Width.Pixels, 1f),
				Height = new StyleDimension(-4f, 1f),
				VAlign = 1f
			};
			uielement3.Append(uielement5);
			uielement5.SetPadding(0f);
			this._bestiarySpace = uielement5;
			UIBestiaryEntryGrid uibestiaryEntryGrid = new UIBestiaryEntryGrid(this._workingSetEntries, new UIElement.MouseEvent(this.Click_SelectEntryButton));
			uielement5.Append(uibestiaryEntryGrid);
			this._entryGrid = uibestiaryEntryGrid;
			this._entryGrid.OnGridContentsChanged += this.UpdateBestiaryGridRange;
			uielement3.Append(uibestiaryEntryInfoPage);
			this._infoSpace = uibestiaryEntryInfoPage;
			this.AddBackAndForwardButtons(uielement2);
			this._sortingGrid = new UIBestiarySortingOptionsGrid(this._sorter);
			this._sortingGrid.OnLeftClick += this.Click_CloseSortingGrid;
			this._sortingGrid.OnClickingOption += this.UpdateBestiaryContents;
			this._filteringGrid = new UIBestiaryFilteringOptionsGrid(this._filterer);
			this._filteringGrid.OnLeftClick += this.Click_CloseFilteringGrid;
			this._filteringGrid.OnClickingOption += this.UpdateBestiaryContents;
			this._filteringGrid.SetupAvailabilityTest(this._originalEntriesList);
			this.UpdateBestiaryContents();
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x00589BC4 File Offset: 0x00587DC4
		private void FillProgressBottomBar(UIElement container)
		{
			UIText progressPercentText = new UIText("", 0.8f, false)
			{
				HAlign = 0f,
				VAlign = 1f,
				TextOriginX = 0f,
				TextOriginY = 0f
			};
			this._progressPercentText = progressPercentText;
			UIColoredSliderSimple uicoloredSliderSimple = new UIColoredSliderSimple
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(15f, 0f),
				VAlign = 1f,
				FilledColor = new Color(51, 137, 255),
				EmptyColor = new Color(35, 43, 81),
				FillPercent = 0f
			};
			uicoloredSliderSimple.OnUpdate += this.ShowStats_Completion;
			this._unlocksProgressBar = uicoloredSliderSimple;
			container.Append(uicoloredSliderSimple);
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x00589CA4 File Offset: 0x00587EA4
		private void ShowStats_Completion(UIElement element)
		{
			if (!element.IsMouseHovering)
			{
				return;
			}
			string completionPercentText = this.GetCompletionPercentText();
			Main.instance.MouseText(completionPercentText, 0, 0, -1, -1, -1, -1, 0);
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x00589CD4 File Offset: 0x00587ED4
		private string GetCompletionPercentText()
		{
			string percent = Utils.PrettifyPercentDisplay(this.GetProgressPercent(), "P2");
			return Language.GetTextValueWith("BestiaryInfo.PercentCollected", new
			{
				Percent = percent
			});
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x00589D02 File Offset: 0x00587F02
		private float GetProgressPercent()
		{
			return this._progressReport.CompletionPercent;
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x00009E06 File Offset: 0x00008006
		private void EmptyInteraction(float input)
		{
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x00009E06 File Offset: 0x00008006
		private void EmptyInteraction2()
		{
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x00589D0F File Offset: 0x00587F0F
		private Color GetColorAtBlip(float percentile)
		{
			if (percentile < this.GetProgressPercent())
			{
				return new Color(51, 137, 255);
			}
			return new Color(35, 40, 83);
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x00589D38 File Offset: 0x00587F38
		private void AddBackAndForwardButtons(UIElement innerTopContainer)
		{
			UIImageButton uiimageButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Back", 1), null);
			uiimageButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Border", 1), null);
			uiimageButton.SetVisibility(1f, 1f);
			uiimageButton.SetSnapPoint("BackPage", 0, null, null);
			this._entryGrid.MakeButtonGoByOffset(uiimageButton, -1);
			innerTopContainer.Append(uiimageButton);
			UIImageButton uiimageButton2 = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Forward", 1), null)
			{
				Left = new StyleDimension(uiimageButton.Width.Pixels + 1f, 0f)
			};
			uiimageButton2.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Border", 1), null);
			uiimageButton2.SetVisibility(1f, 1f);
			uiimageButton2.SetSnapPoint("NextPage", 0, null, null);
			this._entryGrid.MakeButtonGoByOffset(uiimageButton2, 1);
			innerTopContainer.Append(uiimageButton2);
			UIPanel uipanel = new UIPanel
			{
				Left = new StyleDimension(uiimageButton.Width.Pixels + 1f + uiimageButton2.Width.Pixels + 3f, 0f),
				Width = new StyleDimension(135f, 0f),
				Height = new StyleDimension(0f, 1f),
				VAlign = 0.5f
			};
			uipanel.BackgroundColor = new Color(35, 40, 83);
			uipanel.BorderColor = new Color(35, 40, 83);
			uipanel.SetPadding(0f);
			innerTopContainer.Append(uipanel);
			UIText uitext = new UIText("9000-9999 (9001)", 0.8f, false)
			{
				HAlign = 0.5f,
				VAlign = 0.5f
			};
			uipanel.Append(uitext);
			this._indexesRangeText = uitext;
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x00589F48 File Offset: 0x00588148
		private void AddSortAndFilterButtons(UIElement innerTopContainer, UIBestiaryEntryInfoPage infoSpace)
		{
			int num = 17;
			UIImageButton uiimageButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Filtering", 1), null)
			{
				Left = new StyleDimension(-infoSpace.Width.Pixels - (float)num, 0f),
				HAlign = 1f
			};
			uiimageButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Wide_Border", 1), null);
			uiimageButton.SetVisibility(1f, 1f);
			uiimageButton.SetSnapPoint("FilterButton", 0, null, null);
			uiimageButton.OnLeftClick += this.OpenOrCloseFilteringGrid;
			innerTopContainer.Append(uiimageButton);
			UIText uitext = new UIText("", 0.8f, false)
			{
				Left = new StyleDimension(34f, 0f),
				Top = new StyleDimension(2f, 0f),
				VAlign = 0.5f,
				TextOriginX = 0f,
				TextOriginY = 0f
			};
			uiimageButton.Append(uitext);
			this._filteringText = uitext;
			UIImageButton uiimageButton2 = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Sorting", 1), null)
			{
				Left = new StyleDimension(-infoSpace.Width.Pixels - uiimageButton.Width.Pixels - 3f - (float)num, 0f),
				HAlign = 1f
			};
			uiimageButton2.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Wide_Border", 1), null);
			uiimageButton2.SetVisibility(1f, 1f);
			uiimageButton2.SetSnapPoint("SortButton", 0, null, null);
			uiimageButton2.OnLeftClick += this.OpenOrCloseSortingOptions;
			innerTopContainer.Append(uiimageButton2);
			UIText uitext2 = new UIText("", 0.8f, false)
			{
				Left = new StyleDimension(34f, 0f),
				Top = new StyleDimension(2f, 0f),
				VAlign = 0.5f,
				TextOriginX = 0f,
				TextOriginY = 0f
			};
			uiimageButton2.Append(uitext2);
			this._sortingText = uitext2;
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x0058A1A7 File Offset: 0x005883A7
		private void GoBackFromVirtualKeyboard()
		{
			UserInterface.ActiveInstance.SetState(this);
			UILinkPointNavigator.ChangePoint(this.searchButtonLink.ID);
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x0058A1C4 File Offset: 0x005883C4
		private void OnSearchContentsChanged(string contents)
		{
			this._filterer.SetSearchFilter(contents);
			this.UpdateBestiaryContents();
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x0058A1D8 File Offset: 0x005883D8
		private void FilterEntries()
		{
			this._workingSetEntries.Clear();
			this._workingSetEntries.AddRange(this._originalEntriesList.Where(new Func<BestiaryEntry, bool>(this._filterer.FitsFilter)));
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x0058A20C File Offset: 0x0058840C
		private void SortEntries()
		{
			foreach (BestiaryEntry bestiaryEntry in this._workingSetEntries)
			{
				foreach (IBestiaryInfoElement bestiaryInfoElement in bestiaryEntry.Info)
				{
					IUpdateBeforeSorting updateBeforeSorting = bestiaryInfoElement as IUpdateBeforeSorting;
					if (updateBeforeSorting != null)
					{
						updateBeforeSorting.UpdateBeforeSorting();
					}
				}
			}
			this._workingSetEntries.Sort(this._sorter);
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x0058A2B0 File Offset: 0x005884B0
		private void FillBestiarySpaceWithEntries()
		{
			if (this._entryGrid == null || this._entryGrid.Parent == null)
			{
				return;
			}
			this.DeselectEntryButton();
			this._progressReport = this.GetUnlockProgress();
			this._entryGrid.FillBestiarySpaceWithEntries();
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x0058A2E5 File Offset: 0x005884E5
		public void UpdateBestiaryGridRange()
		{
			this._indexesRangeText.SetText(this._entryGrid.GetRangeText());
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x0058A2FD File Offset: 0x005884FD
		public override void Recalculate()
		{
			base.Recalculate();
			this.FillBestiarySpaceWithEntries();
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x0058A30C File Offset: 0x0058850C
		private void GetEntriesToShow(out int maxEntriesWidth, out int maxEntriesHeight, out int maxEntriesToHave)
		{
			Rectangle rectangle = this._bestiarySpace.GetDimensions().ToRectangle();
			maxEntriesWidth = rectangle.Width / 72;
			maxEntriesHeight = rectangle.Height / 72;
			int num = 0;
			maxEntriesToHave = maxEntriesWidth * maxEntriesHeight - num;
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x0058A350 File Offset: 0x00588550
		private void MakeExitButton(UIElement outerContainer)
		{
			UITextPanel<LocalizedText> uitextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, true)
			{
				Width = StyleDimension.FromPixelsAndPercent(-10f, 0.5f),
				Height = StyleDimension.FromPixels(50f),
				VAlign = 1f,
				HAlign = 0.5f,
				Top = StyleDimension.FromPixels(-25f)
			};
			uitextPanel.OnMouseOver += this.FadedMouseOver;
			uitextPanel.OnMouseOut += this.FadedMouseOut;
			uitextPanel.OnLeftMouseDown += this.Click_GoBack;
			uitextPanel.SetSnapPoint("ExitButton", 0, null, null);
			outerContainer.Append(uitextPanel);
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x0058A419 File Offset: 0x00588619
		private void Click_GoBack(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
			if (Main.gameMenu)
			{
				Main.menuMode = 0;
				return;
			}
			IngameFancyUI.Close(true);
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x0058A444 File Offset: 0x00588644
		private void OpenOrCloseSortingOptions(UIMouseEvent evt, UIElement listeningElement)
		{
			if (this._sortingGrid.Parent != null)
			{
				this.CloseSortingGrid();
				return;
			}
			this._bestiarySpace.RemoveChild(this._sortingGrid);
			this._bestiarySpace.RemoveChild(this._filteringGrid);
			this._bestiarySpace.Append(this._sortingGrid);
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x0058A49C File Offset: 0x0058869C
		private void OpenOrCloseFilteringGrid(UIMouseEvent evt, UIElement listeningElement)
		{
			if (this._filteringGrid.Parent != null)
			{
				this.CloseFilteringGrid();
				return;
			}
			this._bestiarySpace.RemoveChild(this._sortingGrid);
			this._bestiarySpace.RemoveChild(this._filteringGrid);
			this._bestiarySpace.Append(this._filteringGrid);
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x0058A4F3 File Offset: 0x005886F3
		private void Click_CloseFilteringGrid(UIMouseEvent evt, UIElement listeningElement)
		{
			if (evt.Target != this._filteringGrid)
			{
				return;
			}
			this.CloseFilteringGrid();
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x0058A50A File Offset: 0x0058870A
		private void CloseFilteringGrid()
		{
			this.UpdateBestiaryContents();
			this._bestiarySpace.RemoveChild(this._filteringGrid);
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x0058A524 File Offset: 0x00588724
		private void UpdateBestiaryContents()
		{
			this._filteringGrid.UpdateAvailability();
			this._sortingText.SetText(this._sorter.GetDisplayName());
			this._filteringText.SetText(this._filterer.GetDisplayName());
			this.FilterEntries();
			this.SortEntries();
			this.FillBestiarySpaceWithEntries();
			this._progressReport = this.GetUnlockProgress();
			string completionPercentText = this.GetCompletionPercentText();
			this._progressPercentText.SetText(completionPercentText);
			this._unlocksProgressBar.FillPercent = this.GetProgressPercent();
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x0058A5AA File Offset: 0x005887AA
		private void Click_CloseSortingGrid(UIMouseEvent evt, UIElement listeningElement)
		{
			if (evt.Target != this._sortingGrid)
			{
				return;
			}
			this.CloseSortingGrid();
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x0058A5C1 File Offset: 0x005887C1
		private void CloseSortingGrid()
		{
			this.UpdateBestiaryContents();
			this._bestiarySpace.RemoveChild(this._sortingGrid);
		}

		// Token: 0x06002B14 RID: 11028 RVA: 0x0058A5DC File Offset: 0x005887DC
		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002B15 RID: 11029 RVA: 0x00584489 File Offset: 0x00582689
		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x0058A634 File Offset: 0x00588834
		private void Click_SelectEntryButton(UIMouseEvent evt, UIElement listeningElement)
		{
			UIBestiaryEntryButton uibestiaryEntryButton = (UIBestiaryEntryButton)listeningElement;
			if (uibestiaryEntryButton != null)
			{
				this.SelectEntryButton(uibestiaryEntryButton);
			}
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x0058A654 File Offset: 0x00588854
		private void SelectEntryButton(UIBestiaryEntryButton button)
		{
			this.DeselectEntryButton();
			this._selectedEntryButton = button;
			this._infoSpace.FillInfoForEntry(button.Entry, new ExtraBestiaryInfoPageInformation
			{
				BestiaryProgressReport = this._progressReport
			});
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x0058A698 File Offset: 0x00588898
		private void DeselectEntryButton()
		{
			this._infoSpace.FillInfoForEntry(null, default(ExtraBestiaryInfoPageInformation));
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x0058A6BC File Offset: 0x005888BC
		public BestiaryUnlockProgressReport GetUnlockProgress()
		{
			float num = 0f;
			int num2 = 0;
			List<BestiaryEntry> originalEntriesList = this._originalEntriesList;
			for (int i = 0; i < originalEntriesList.Count; i++)
			{
				int num3 = (originalEntriesList[i].UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0) ? 1 : 0;
				num2++;
				num += (float)num3;
			}
			return new BestiaryUnlockProgressReport
			{
				EntriesTotal = num2,
				CompletionAmountTotal = num
			};
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x0058A72C File Offset: 0x0058892C
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			this.SetupGamepadPoints(spriteBatch);
		}

		// Token: 0x06002B1B RID: 11035 RVA: 0x0058A73C File Offset: 0x0058893C
		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 1;
			int num = 3000;
			int num2 = num;
			List<SnapPoint> snapPoints = this.GetSnapPoints();
			SnapPoint snap = null;
			SnapPoint snap2 = null;
			SnapPoint snap3 = null;
			SnapPoint snap4 = null;
			SnapPoint snap5 = null;
			SnapPoint snap6 = null;
			for (int i = 0; i < snapPoints.Count; i++)
			{
				SnapPoint snapPoint = snapPoints[i];
				string name = snapPoint.Name;
				if (!(name == "BackPage"))
				{
					if (!(name == "NextPage"))
					{
						if (!(name == "ExitButton"))
						{
							if (!(name == "FilterButton"))
							{
								if (!(name == "SortButton"))
								{
									if (name == "SearchButton")
									{
										snap5 = snapPoint;
									}
								}
								else
								{
									snap3 = snapPoint;
								}
							}
							else
							{
								snap4 = snapPoint;
							}
						}
						else
						{
							snap6 = snapPoint;
						}
					}
					else
					{
						snap2 = snapPoint;
					}
				}
				else
				{
					snap = snapPoint;
				}
			}
			UILinkPoint uilinkPoint = this.MakeLinkPointFromSnapPoint(num2++, snap);
			UILinkPoint uilinkPoint2 = this.MakeLinkPointFromSnapPoint(num2++, snap2);
			UILinkPoint uilinkPoint3 = this.MakeLinkPointFromSnapPoint(num2++, snap6);
			UILinkPoint uilinkPoint4 = this.MakeLinkPointFromSnapPoint(num2++, snap4);
			UILinkPoint uilinkPoint5 = this.MakeLinkPointFromSnapPoint(num2++, snap3);
			this.searchButtonLink = this.MakeLinkPointFromSnapPoint(num2++, snap5);
			this.PairLeftRight(uilinkPoint, uilinkPoint2);
			this.PairLeftRight(uilinkPoint2, uilinkPoint5);
			this.PairLeftRight(uilinkPoint5, uilinkPoint4);
			this.PairLeftRight(uilinkPoint4, this.searchButtonLink);
			uilinkPoint3.Up = uilinkPoint2.ID;
			UILinkPoint[,] array = new UILinkPoint[1, 1];
			if (this._filteringGrid.Parent != null)
			{
				int num3;
				int num4;
				this.SetupPointsForFilterGrid(ref num2, snapPoints, out num3, out num4, out array);
				this.PairUpDown(uilinkPoint2, uilinkPoint3);
				this.PairUpDown(uilinkPoint, uilinkPoint3);
				for (int j = num3 - 1; j >= 0; j--)
				{
					UILinkPoint uilinkPoint6 = array[j, num4 - 1];
					if (uilinkPoint6 != null)
					{
						this.PairUpDown(uilinkPoint6, uilinkPoint3);
					}
					UILinkPoint uilinkPoint7 = array[j, num4 - 2];
					if (uilinkPoint7 != null && uilinkPoint6 == null)
					{
						this.PairUpDown(uilinkPoint7, uilinkPoint3);
					}
					UILinkPoint uilinkPoint8 = array[j, 0];
					if (uilinkPoint8 != null)
					{
						if (j < num3 - 3)
						{
							this.PairUpDown(uilinkPoint5, uilinkPoint8);
						}
						else
						{
							this.PairUpDown(uilinkPoint4, uilinkPoint8);
						}
					}
				}
			}
			else if (this._sortingGrid.Parent != null)
			{
				int num3;
				int num4;
				this.SetupPointsForSortingGrid(ref num2, snapPoints, out num3, out num4, out array);
				this.PairUpDown(uilinkPoint2, uilinkPoint3);
				this.PairUpDown(uilinkPoint, uilinkPoint3);
				for (int k = num3 - 1; k >= 0; k--)
				{
					UILinkPoint uilinkPoint9 = array[k, num4 - 1];
					if (uilinkPoint9 != null)
					{
						this.PairUpDown(uilinkPoint9, uilinkPoint3);
					}
					UILinkPoint uilinkPoint10 = array[k, 0];
					if (uilinkPoint10 != null)
					{
						this.PairUpDown(uilinkPoint4, uilinkPoint10);
						this.PairUpDown(uilinkPoint5, uilinkPoint10);
					}
				}
			}
			else if (this._entryGrid.Parent != null)
			{
				int num3;
				int num4;
				this.SetupPointsForEntryGrid(ref num2, snapPoints, out num3, out num4, out array);
				for (int l = 0; l < num3; l++)
				{
					if (num4 - 1 >= 0)
					{
						UILinkPoint uilinkPoint11 = array[l, num4 - 1];
						if (uilinkPoint11 != null)
						{
							this.PairUpDown(uilinkPoint11, uilinkPoint3);
						}
						if (num4 - 2 >= 0)
						{
							UILinkPoint uilinkPoint12 = array[l, num4 - 2];
							if (uilinkPoint12 != null && uilinkPoint11 == null)
							{
								this.PairUpDown(uilinkPoint12, uilinkPoint3);
							}
						}
					}
					UILinkPoint uilinkPoint13 = array[l, 0];
					if (uilinkPoint13 != null)
					{
						if (l < num3 / 2)
						{
							this.PairUpDown(uilinkPoint2, uilinkPoint13);
						}
						else if (l == num3 - 1)
						{
							this.PairUpDown(uilinkPoint4, uilinkPoint13);
						}
						else
						{
							this.PairUpDown(uilinkPoint5, uilinkPoint13);
						}
					}
				}
				UILinkPoint uilinkPoint14 = array[0, 0];
				if (uilinkPoint14 != null)
				{
					this.PairUpDown(uilinkPoint2, uilinkPoint14);
					this.PairUpDown(uilinkPoint, uilinkPoint14);
				}
				else
				{
					this.PairUpDown(uilinkPoint2, uilinkPoint3);
					this.PairUpDown(uilinkPoint, uilinkPoint3);
					this.PairUpDown(uilinkPoint4, uilinkPoint3);
					this.PairUpDown(uilinkPoint5, uilinkPoint3);
				}
			}
			List<UILinkPoint> list = new List<UILinkPoint>();
			for (int m = num; m < num2; m++)
			{
				list.Add(UILinkPointNavigator.Points[m]);
			}
			if (PlayerInput.UsingGamepadUI && UILinkPointNavigator.CurrentPoint >= num2)
			{
				this.MoveToVisuallyClosestPoint(list);
			}
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x0058AB48 File Offset: 0x00588D48
		private void MoveToVisuallyClosestPoint(List<UILinkPoint> lostrefpoints)
		{
			Dictionary<int, UILinkPoint> points = UILinkPointNavigator.Points;
			Vector2 mouseScreen = Main.MouseScreen;
			UILinkPoint uilinkPoint = null;
			foreach (UILinkPoint uilinkPoint2 in lostrefpoints)
			{
				if (uilinkPoint == null || Vector2.Distance(mouseScreen, uilinkPoint.Position) > Vector2.Distance(mouseScreen, uilinkPoint2.Position))
				{
					uilinkPoint = uilinkPoint2;
				}
			}
			if (uilinkPoint != null)
			{
				UILinkPointNavigator.ChangePoint(uilinkPoint.ID);
			}
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x0058ABCC File Offset: 0x00588DCC
		private void SetupPointsForEntryGrid(ref int currentID, List<SnapPoint> pts, out int gridWidth, out int gridHeight, out UILinkPoint[,] gridPoints)
		{
			List<SnapPoint> orderedPointsByCategoryName = UIBestiaryTest.GetOrderedPointsByCategoryName(pts, "Entries");
			int num;
			this._entryGrid.GetEntriesToShow(out gridWidth, out gridHeight, out num);
			gridPoints = new UILinkPoint[gridWidth, gridHeight];
			for (int i = 0; i < orderedPointsByCategoryName.Count; i++)
			{
				int num2 = i % gridWidth;
				int num3 = i / gridWidth;
				UILinkPoint[,] array = gridPoints;
				int num4 = num2;
				int num5 = num3;
				int num6 = currentID;
				currentID = num6 + 1;
				array[num4, num5] = this.MakeLinkPointFromSnapPoint(num6, orderedPointsByCategoryName[i]);
			}
			for (int j = 0; j < gridWidth; j++)
			{
				for (int k = 0; k < gridHeight; k++)
				{
					UILinkPoint uilinkPoint = gridPoints[j, k];
					if (j < gridWidth - 1)
					{
						UILinkPoint uilinkPoint2 = gridPoints[j + 1, k];
						if (uilinkPoint != null && uilinkPoint2 != null)
						{
							this.PairLeftRight(uilinkPoint, uilinkPoint2);
						}
					}
					if (k < gridHeight - 1)
					{
						UILinkPoint uilinkPoint3 = gridPoints[j, k + 1];
						if (uilinkPoint != null && uilinkPoint3 != null)
						{
							this.PairUpDown(uilinkPoint, uilinkPoint3);
						}
					}
				}
			}
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x0058ACCC File Offset: 0x00588ECC
		private void SetupPointsForFilterGrid(ref int currentID, List<SnapPoint> pts, out int gridWidth, out int gridHeight, out UILinkPoint[,] gridPoints)
		{
			List<SnapPoint> orderedPointsByCategoryName = UIBestiaryTest.GetOrderedPointsByCategoryName(pts, "Filters");
			int num;
			this._filteringGrid.GetEntriesToShow(out gridWidth, out gridHeight, out num);
			gridPoints = new UILinkPoint[gridWidth, gridHeight];
			for (int i = 0; i < orderedPointsByCategoryName.Count; i++)
			{
				int num2 = i % gridWidth;
				int num3 = i / gridWidth;
				UILinkPoint[,] array = gridPoints;
				int num4 = num2;
				int num5 = num3;
				int num6 = currentID;
				currentID = num6 + 1;
				array[num4, num5] = this.MakeLinkPointFromSnapPoint(num6, orderedPointsByCategoryName[i]);
			}
			for (int j = 0; j < gridWidth; j++)
			{
				for (int k = 0; k < gridHeight; k++)
				{
					UILinkPoint uilinkPoint = gridPoints[j, k];
					if (j < gridWidth - 1)
					{
						UILinkPoint uilinkPoint2 = gridPoints[j + 1, k];
						if (uilinkPoint != null && uilinkPoint2 != null)
						{
							this.PairLeftRight(uilinkPoint, uilinkPoint2);
						}
					}
					if (k < gridHeight - 1)
					{
						UILinkPoint uilinkPoint3 = gridPoints[j, k + 1];
						if (uilinkPoint != null && uilinkPoint3 != null)
						{
							this.PairUpDown(uilinkPoint, uilinkPoint3);
						}
					}
				}
			}
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x0058ADCC File Offset: 0x00588FCC
		private void SetupPointsForSortingGrid(ref int currentID, List<SnapPoint> pts, out int gridWidth, out int gridHeight, out UILinkPoint[,] gridPoints)
		{
			List<SnapPoint> orderedPointsByCategoryName = UIBestiaryTest.GetOrderedPointsByCategoryName(pts, "SortSteps");
			int num;
			this._sortingGrid.GetEntriesToShow(out gridWidth, out gridHeight, out num);
			gridPoints = new UILinkPoint[gridWidth, gridHeight];
			for (int i = 0; i < orderedPointsByCategoryName.Count; i++)
			{
				int num2 = i % gridWidth;
				int num3 = i / gridWidth;
				UILinkPoint[,] array = gridPoints;
				int num4 = num2;
				int num5 = num3;
				int num6 = currentID;
				currentID = num6 + 1;
				array[num4, num5] = this.MakeLinkPointFromSnapPoint(num6, orderedPointsByCategoryName[i]);
			}
			for (int j = 0; j < gridWidth; j++)
			{
				for (int k = 0; k < gridHeight; k++)
				{
					UILinkPoint uilinkPoint = gridPoints[j, k];
					if (j < gridWidth - 1)
					{
						UILinkPoint uilinkPoint2 = gridPoints[j + 1, k];
						if (uilinkPoint != null && uilinkPoint2 != null)
						{
							this.PairLeftRight(uilinkPoint, uilinkPoint2);
						}
					}
					if (k < gridHeight - 1)
					{
						UILinkPoint uilinkPoint3 = gridPoints[j, k + 1];
						if (uilinkPoint != null && uilinkPoint3 != null)
						{
							this.PairUpDown(uilinkPoint, uilinkPoint3);
						}
					}
				}
			}
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x0058AECC File Offset: 0x005890CC
		private static List<SnapPoint> GetOrderedPointsByCategoryName(List<SnapPoint> pts, string name)
		{
			return (from x in pts
			where x.Name == name
			orderby x.Id
			select x).ToList<SnapPoint>();
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x0058AF21 File Offset: 0x00589121
		private void PairLeftRight(UILinkPoint leftSide, UILinkPoint rightSide)
		{
			leftSide.Right = rightSide.ID;
			rightSide.Left = leftSide.ID;
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x0058AF3B File Offset: 0x0058913B
		private void PairUpDown(UILinkPoint upSide, UILinkPoint downSide)
		{
			upSide.Down = downSide.ID;
			downSide.Up = upSide.ID;
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x0058AF55 File Offset: 0x00589155
		private UILinkPoint MakeLinkPointFromSnapPoint(int id, SnapPoint snap)
		{
			UILinkPointNavigator.SetPosition(id, snap.Position);
			UILinkPoint uilinkPoint = UILinkPointNavigator.Points[id];
			uilinkPoint.Unlink();
			return uilinkPoint;
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x0058AF74 File Offset: 0x00589174
		public override void ScrollWheel(UIScrollWheelEvent evt)
		{
			base.ScrollWheel(evt);
			this._infoSpace.UpdateScrollbar(evt.ScrollWheelValue);
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x0058AF8E File Offset: 0x0058918E
		public void TryMovingPages(int direction)
		{
			this._entryGrid.OffsetLibraryByPages(direction);
		}

		// Token: 0x04005341 RID: 21313
		private UIElement _bestiarySpace;

		// Token: 0x04005342 RID: 21314
		private UIBestiaryEntryInfoPage _infoSpace;

		// Token: 0x04005343 RID: 21315
		private UIBestiaryEntryButton _selectedEntryButton;

		// Token: 0x04005344 RID: 21316
		private List<BestiaryEntry> _originalEntriesList;

		// Token: 0x04005345 RID: 21317
		private List<BestiaryEntry> _workingSetEntries;

		// Token: 0x04005346 RID: 21318
		private UIText _indexesRangeText;

		// Token: 0x04005347 RID: 21319
		private EntryFilterer<BestiaryEntry, IBestiaryEntryFilter> _filterer = new EntryFilterer<BestiaryEntry, IBestiaryEntryFilter>();

		// Token: 0x04005348 RID: 21320
		private EntrySorter<BestiaryEntry, IBestiarySortStep> _sorter = new EntrySorter<BestiaryEntry, IBestiarySortStep>();

		// Token: 0x04005349 RID: 21321
		private UIBestiaryEntryGrid _entryGrid;

		// Token: 0x0400534A RID: 21322
		private UIBestiarySortingOptionsGrid _sortingGrid;

		// Token: 0x0400534B RID: 21323
		private UIBestiaryFilteringOptionsGrid _filteringGrid;

		// Token: 0x0400534C RID: 21324
		private UIText _sortingText;

		// Token: 0x0400534D RID: 21325
		private UIText _filteringText;

		// Token: 0x0400534E RID: 21326
		private BestiaryUnlockProgressReport _progressReport;

		// Token: 0x0400534F RID: 21327
		private UIText _progressPercentText;

		// Token: 0x04005350 RID: 21328
		private UIColoredSliderSimple _unlocksProgressBar;

		// Token: 0x04005351 RID: 21329
		private UILinkPoint searchButtonLink;
	}
}
