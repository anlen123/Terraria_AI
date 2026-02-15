using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003AA RID: 938
	public class UICreativePowersMenu : UIState
	{
		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06002B26 RID: 11046 RVA: 0x0058AF9C File Offset: 0x0058919C
		public bool IsShowingResearchMenu
		{
			get
			{
				return this._mainCategory.CurrentOption == 2;
			}
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x0058AFAC File Offset: 0x005891AC
		public override void OnActivate()
		{
			this.InitializePage();
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x0058AFB4 File Offset: 0x005891B4
		private void InitializePage()
		{
			int num = 270;
			int num2 = 20;
			this._container = new UIElement
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension((float)(-(float)num - num2), 1f),
				Top = new StyleDimension((float)num, 0f)
			};
			base.Append(this._container);
			List<UIElement> buttons = this.CreateMainPowerStrip();
			PowerStripUIElement powerStripUIElement = new PowerStripUIElement("strip 0", buttons)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				Left = new StyleDimension(20f, 0f)
			};
			powerStripUIElement.OnMouseOver += this.strip_OnMouseOver;
			powerStripUIElement.OnMouseOut += this.strip_OnMouseOut;
			this._mainPowerStrip = powerStripUIElement;
			List<UIElement> buttons2 = this.CreateTimePowerStrip();
			PowerStripUIElement powerStripUIElement2 = new PowerStripUIElement("strip 1", buttons2)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				Left = new StyleDimension(80f, 0f)
			};
			powerStripUIElement2.OnMouseOver += this.strip_OnMouseOver;
			powerStripUIElement2.OnMouseOut += this.strip_OnMouseOut;
			this._timePowersStrip = powerStripUIElement2;
			List<UIElement> buttons3 = this.CreateWeatherPowerStrip();
			PowerStripUIElement powerStripUIElement3 = new PowerStripUIElement("strip 1", buttons3)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				Left = new StyleDimension(80f, 0f)
			};
			powerStripUIElement3.OnMouseOver += this.strip_OnMouseOver;
			powerStripUIElement3.OnMouseOut += this.strip_OnMouseOut;
			this._weatherPowersStrip = powerStripUIElement3;
			List<UIElement> buttons4 = this.CreatePersonalPowerStrip();
			PowerStripUIElement powerStripUIElement4 = new PowerStripUIElement("strip 1", buttons4)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				Left = new StyleDimension(80f, 0f)
			};
			powerStripUIElement4.OnMouseOver += this.strip_OnMouseOver;
			powerStripUIElement4.OnMouseOut += this.strip_OnMouseOut;
			this._personalPowersStrip = powerStripUIElement4;
			this._infiniteItemsWindow = new UICreativeInfiniteItemsDisplay
			{
				HAlign = 0f,
				VAlign = 0.5f,
				Left = new StyleDimension(80f, 0f),
				Width = new StyleDimension(480f, 0f),
				Height = new StyleDimension(-88f, 1f)
			};
			this.RefreshElementsOrder();
			base.OnUpdate += this.UICreativePowersMenu_OnUpdate;
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x0058B264 File Offset: 0x00589464
		private List<UIElement> CreateMainPowerStrip()
		{
			UICreativePowersMenu.MenuTree<UICreativePowersMenu.OpenMainSubCategory> mainCategory = this._mainCategory;
			mainCategory.Buttons.Clear();
			List<UIElement> list = new List<UIElement>();
			CreativePowerUIElementRequestInfo info = new CreativePowerUIElementRequestInfo
			{
				PreferredButtonWidth = 40,
				PreferredButtonHeight = 40
			};
			GroupOptionButton<int> groupOptionButton = CreativePowersHelper.CreateCategoryButton<int>(info, 1, 0);
			groupOptionButton.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.ItemDuplication));
			groupOptionButton.OnLeftClick += this.MainCategoryButtonClick;
			groupOptionButton.OnUpdate += this.itemsWindowButton_OnUpdate;
			mainCategory.Buttons.Add(1, groupOptionButton);
			list.Add(groupOptionButton);
			this._infiniteItemsButton = groupOptionButton;
			GroupOptionButton<int> groupOptionButton2 = CreativePowersHelper.CreateCategoryButton<int>(info, 2, 0);
			groupOptionButton2.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.ItemResearch));
			groupOptionButton2.OnLeftClick += this.MainCategoryButtonClick;
			groupOptionButton2.OnUpdate += this.researchWindowButton_OnUpdate;
			mainCategory.Buttons.Add(2, groupOptionButton2);
			list.Add(groupOptionButton2);
			GroupOptionButton<int> groupOptionButton3 = CreativePowersHelper.CreateCategoryButton<int>(info, 3, 0);
			groupOptionButton3.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.TimeCategory));
			groupOptionButton3.OnLeftClick += this.MainCategoryButtonClick;
			groupOptionButton3.OnUpdate += this.timeCategoryButton_OnUpdate;
			mainCategory.Buttons.Add(3, groupOptionButton3);
			list.Add(groupOptionButton3);
			GroupOptionButton<int> groupOptionButton4 = CreativePowersHelper.CreateCategoryButton<int>(info, 4, 0);
			groupOptionButton4.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.WeatherCategory));
			groupOptionButton4.OnLeftClick += this.MainCategoryButtonClick;
			groupOptionButton4.OnUpdate += this.weatherCategoryButton_OnUpdate;
			mainCategory.Buttons.Add(4, groupOptionButton4);
			list.Add(groupOptionButton4);
			GroupOptionButton<int> groupOptionButton5 = CreativePowersHelper.CreateCategoryButton<int>(info, 6, 0);
			groupOptionButton5.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.PersonalCategory));
			groupOptionButton5.OnLeftClick += this.MainCategoryButtonClick;
			groupOptionButton5.OnUpdate += this.personalCategoryButton_OnUpdate;
			mainCategory.Buttons.Add(6, groupOptionButton5);
			list.Add(groupOptionButton5);
			CreativePowerManager.Instance.GetPower<CreativePowers.StopBiomeSpreadPower>().ProvidePowerButtons(info, list);
			GroupOptionButton<int> groupOptionButton6 = this.CreateSubcategoryButton<CreativePowers.DifficultySliderPower>(ref info, 1, "strip 1", 5, 0, mainCategory.Buttons, mainCategory.Sliders);
			groupOptionButton6.OnLeftClick += this.MainCategoryButtonClick;
			list.Add(groupOptionButton6);
			return list;
		}

		// Token: 0x06002B2A RID: 11050 RVA: 0x0058B4A8 File Offset: 0x005896A8
		private static void CategoryButton_OnUpdate_DisplayTooltips(UIElement affectedElement, string categoryNameKey)
		{
			GroupOptionButton<int> groupOptionButton = affectedElement as GroupOptionButton<int>;
			if (affectedElement.IsMouseHovering)
			{
				string textValue = Language.GetTextValue(groupOptionButton.IsSelected ? (categoryNameKey + "Opened") : (categoryNameKey + "Closed"));
				CreativePowersHelper.AddDescriptionIfNeeded(ref textValue, categoryNameKey);
				Main.instance.MouseTextNoOverride(textValue, 0, 0, -1, -1, -1, -1, 0);
			}
		}

		// Token: 0x06002B2B RID: 11051 RVA: 0x0058B504 File Offset: 0x00589704
		private void itemsWindowButton_OnUpdate(UIElement affectedElement)
		{
			UICreativePowersMenu.CategoryButton_OnUpdate_DisplayTooltips(affectedElement, "CreativePowers.InfiniteItemsCategory");
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x0058B511 File Offset: 0x00589711
		private void researchWindowButton_OnUpdate(UIElement affectedElement)
		{
			UICreativePowersMenu.CategoryButton_OnUpdate_DisplayTooltips(affectedElement, "CreativePowers.ResearchItemsCategory");
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x0058B51E File Offset: 0x0058971E
		private void timeCategoryButton_OnUpdate(UIElement affectedElement)
		{
			UICreativePowersMenu.CategoryButton_OnUpdate_DisplayTooltips(affectedElement, "CreativePowers.TimeCategory");
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x0058B52B File Offset: 0x0058972B
		private void weatherCategoryButton_OnUpdate(UIElement affectedElement)
		{
			UICreativePowersMenu.CategoryButton_OnUpdate_DisplayTooltips(affectedElement, "CreativePowers.WeatherCategory");
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x0058B538 File Offset: 0x00589738
		private void personalCategoryButton_OnUpdate(UIElement affectedElement)
		{
			UICreativePowersMenu.CategoryButton_OnUpdate_DisplayTooltips(affectedElement, "CreativePowers.PersonalCategory");
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x0058B545 File Offset: 0x00589745
		private void UICreativePowersMenu_OnUpdate(UIElement affectedElement)
		{
			if (this._hovered)
			{
				Main.LocalPlayer.mouseInterface = true;
			}
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x0058B55A File Offset: 0x0058975A
		private void strip_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			this._hovered = false;
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x0058B563 File Offset: 0x00589763
		private void strip_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			this._hovered = true;
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x0058B56C File Offset: 0x0058976C
		private void MainCategoryButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<int> groupOptionButton = listeningElement as GroupOptionButton<int>;
			this.ToggleMainCategory(groupOptionButton.OptionValue);
			this.RefreshElementsOrder();
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x0058B592 File Offset: 0x00589792
		private void ToggleMainCategory(int option)
		{
			this.ToggleCategory<UICreativePowersMenu.OpenMainSubCategory>(this._mainCategory, option, UICreativePowersMenu.OpenMainSubCategory.None);
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x0058B5A2 File Offset: 0x005897A2
		private void ToggleWeatherCategory(int option)
		{
			this.ToggleCategory<UICreativePowersMenu.WeatherSubcategory>(this._weatherCategory, option, UICreativePowersMenu.WeatherSubcategory.None);
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x0058B5B2 File Offset: 0x005897B2
		private void ToggleTimeCategory(int option)
		{
			this.ToggleCategory<UICreativePowersMenu.TimeSubcategory>(this._timeCategory, option, UICreativePowersMenu.TimeSubcategory.None);
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x0058B5C2 File Offset: 0x005897C2
		private void TogglePersonalCategory(int option)
		{
			this.ToggleCategory<UICreativePowersMenu.PersonalSubcategory>(this._personalCategory, option, UICreativePowersMenu.PersonalSubcategory.None);
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x0058B5D2 File Offset: 0x005897D2
		public void SacrificeWhatsInResearchMenu()
		{
			this._infiniteItemsWindow.SacrificeWhatYouCan();
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x0058B5DF File Offset: 0x005897DF
		public void StopPlayingResearchAnimations()
		{
			this._infiniteItemsWindow.StopPlayingAnimation();
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x0058B5EC File Offset: 0x005897EC
		private void ToggleCategory<TEnum>(UICreativePowersMenu.MenuTree<TEnum> tree, int option, TEnum defaultOption) where TEnum : struct, IConvertible
		{
			if (tree.CurrentOption == option)
			{
				option = defaultOption.ToInt32(null);
			}
			tree.CurrentOption = option;
			foreach (GroupOptionButton<int> groupOptionButton in tree.Buttons.Values)
			{
				groupOptionButton.SetCurrentOption(option);
			}
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x0058B664 File Offset: 0x00589864
		private List<UIElement> CreateTimePowerStrip()
		{
			UICreativePowersMenu.MenuTree<UICreativePowersMenu.TimeSubcategory> timeCategory = this._timeCategory;
			List<UIElement> list = new List<UIElement>();
			CreativePowerUIElementRequestInfo info = new CreativePowerUIElementRequestInfo
			{
				PreferredButtonWidth = 40,
				PreferredButtonHeight = 40
			};
			CreativePowerManager.Instance.GetPower<CreativePowers.FreezeTime>().ProvidePowerButtons(info, list);
			CreativePowerManager.Instance.GetPower<CreativePowers.StartDayImmediately>().ProvidePowerButtons(info, list);
			CreativePowerManager.Instance.GetPower<CreativePowers.StartNoonImmediately>().ProvidePowerButtons(info, list);
			CreativePowerManager.Instance.GetPower<CreativePowers.StartNightImmediately>().ProvidePowerButtons(info, list);
			CreativePowerManager.Instance.GetPower<CreativePowers.StartMidnightImmediately>().ProvidePowerButtons(info, list);
			GroupOptionButton<int> groupOptionButton = this.CreateSubcategoryButton<CreativePowers.ModifyTimeRate>(ref info, 2, "strip 2", 1, 0, timeCategory.Buttons, timeCategory.Sliders);
			groupOptionButton.OnLeftClick += this.TimeCategoryButtonClick;
			list.Add(groupOptionButton);
			return list;
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x0058B728 File Offset: 0x00589928
		private List<UIElement> CreatePersonalPowerStrip()
		{
			UICreativePowersMenu.MenuTree<UICreativePowersMenu.PersonalSubcategory> personalCategory = this._personalCategory;
			List<UIElement> list = new List<UIElement>();
			CreativePowerUIElementRequestInfo info = new CreativePowerUIElementRequestInfo
			{
				PreferredButtonWidth = 40,
				PreferredButtonHeight = 40
			};
			CreativePowerManager.Instance.GetPower<CreativePowers.GodmodePower>().ProvidePowerButtons(info, list);
			CreativePowerManager.Instance.GetPower<CreativePowers.FarPlacementRangePower>().ProvidePowerButtons(info, list);
			GroupOptionButton<int> groupOptionButton = this.CreateSubcategoryButton<CreativePowers.SpawnRateSliderPerPlayerPower>(ref info, 2, "strip 2", 1, 0, personalCategory.Buttons, personalCategory.Sliders);
			groupOptionButton.OnLeftClick += this.PersonalCategoryButtonClick;
			list.Add(groupOptionButton);
			return list;
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x0058B7B8 File Offset: 0x005899B8
		private List<UIElement> CreateWeatherPowerStrip()
		{
			UICreativePowersMenu.MenuTree<UICreativePowersMenu.WeatherSubcategory> weatherCategory = this._weatherCategory;
			List<UIElement> list = new List<UIElement>();
			CreativePowerUIElementRequestInfo info = new CreativePowerUIElementRequestInfo
			{
				PreferredButtonWidth = 40,
				PreferredButtonHeight = 40
			};
			GroupOptionButton<int> groupOptionButton = this.CreateSubcategoryButton<CreativePowers.ModifyWindDirectionAndStrength>(ref info, 2, "strip 2", 1, 0, weatherCategory.Buttons, weatherCategory.Sliders);
			groupOptionButton.OnLeftClick += this.WeatherCategoryButtonClick;
			list.Add(groupOptionButton);
			CreativePowerManager.Instance.GetPower<CreativePowers.FreezeWindDirectionAndStrength>().ProvidePowerButtons(info, list);
			GroupOptionButton<int> groupOptionButton2 = this.CreateSubcategoryButton<CreativePowers.ModifyRainPower>(ref info, 2, "strip 2", 2, 0, weatherCategory.Buttons, weatherCategory.Sliders);
			groupOptionButton2.OnLeftClick += this.WeatherCategoryButtonClick;
			list.Add(groupOptionButton2);
			CreativePowerManager.Instance.GetPower<CreativePowers.FreezeRainPower>().ProvidePowerButtons(info, list);
			return list;
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x0058B884 File Offset: 0x00589A84
		private GroupOptionButton<int> CreateSubcategoryButton<T>(ref CreativePowerUIElementRequestInfo request, int subcategoryDepth, string subcategoryName, int subcategoryIndex, int currentSelectedInSubcategory, Dictionary<int, GroupOptionButton<int>> subcategoryButtons, Dictionary<int, UIElement> slidersSet) where T : ICreativePower, IProvideSliderElement, IPowerSubcategoryElement
		{
			T power = CreativePowerManager.Instance.GetPower<T>();
			UIElement uielement = power.ProvideSlider();
			uielement.Left = new StyleDimension((float)(20 + subcategoryDepth * 60), 0f);
			slidersSet[subcategoryIndex] = uielement;
			uielement.SetSnapPoint(subcategoryName, 0, new Vector2?(new Vector2(0f, 0.5f)), new Vector2?(new Vector2(28f, 0f)));
			GroupOptionButton<int> optionButton = power.GetOptionButton(request, subcategoryIndex, currentSelectedInSubcategory);
			subcategoryButtons[subcategoryIndex] = optionButton;
			CreativePowersHelper.UpdateUnlockStateByPower(power, optionButton, CreativePowersHelper.CommonSelectedColor);
			return optionButton;
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x0058B930 File Offset: 0x00589B30
		private void WeatherCategoryButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<int> groupOptionButton = listeningElement as GroupOptionButton<int>;
			int optionValue = groupOptionButton.OptionValue;
			if (optionValue != 1)
			{
				if (optionValue == 2 && !CreativePowerManager.Instance.GetPower<CreativePowers.ModifyRainPower>().GetIsUnlocked())
				{
					return;
				}
			}
			else if (!CreativePowerManager.Instance.GetPower<CreativePowers.ModifyWindDirectionAndStrength>().GetIsUnlocked())
			{
				return;
			}
			this.ToggleWeatherCategory(groupOptionButton.OptionValue);
			this.RefreshElementsOrder();
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x0058B98C File Offset: 0x00589B8C
		private void TimeCategoryButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<int> groupOptionButton = listeningElement as GroupOptionButton<int>;
			int optionValue = groupOptionButton.OptionValue;
			if (optionValue == 1 && !CreativePowerManager.Instance.GetPower<CreativePowers.ModifyTimeRate>().GetIsUnlocked())
			{
				return;
			}
			this.ToggleTimeCategory(groupOptionButton.OptionValue);
			this.RefreshElementsOrder();
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x0058B9D0 File Offset: 0x00589BD0
		private void PersonalCategoryButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<int> groupOptionButton = listeningElement as GroupOptionButton<int>;
			int optionValue = groupOptionButton.OptionValue;
			if (optionValue == 1 && !CreativePowerManager.Instance.GetPower<CreativePowers.SpawnRateSliderPerPlayerPower>().GetIsUnlocked())
			{
				return;
			}
			this.TogglePersonalCategory(groupOptionButton.OptionValue);
			this.RefreshElementsOrder();
		}

		// Token: 0x06002B42 RID: 11074 RVA: 0x0058BA14 File Offset: 0x00589C14
		private void RefreshElementsOrder()
		{
			this._container.RemoveAllChildren();
			this._container.Append(this._mainPowerStrip);
			UIElement element = null;
			UICreativePowersMenu.MenuTree<UICreativePowersMenu.OpenMainSubCategory> mainCategory = this._mainCategory;
			if (mainCategory.Sliders.TryGetValue(mainCategory.CurrentOption, out element))
			{
				this._container.Append(element);
			}
			if (mainCategory.CurrentOption == 1)
			{
				Main.LocalPlayerCreativeTracker.ItemSacrifices.DismissNewlyUnlockedFromTeamMatesIcon();
				this._infiniteItemsWindow.SetPageTypeToShow(UICreativeInfiniteItemsDisplay.InfiniteItemsDisplayPage.InfiniteItemsPickup);
				this._container.Append(this._infiniteItemsWindow);
			}
			if (mainCategory.CurrentOption == 2)
			{
				this._infiniteItemsWindow.SetPageTypeToShow(UICreativeInfiniteItemsDisplay.InfiniteItemsDisplayPage.InfiniteItemsResearch);
				this._container.Append(this._infiniteItemsWindow);
			}
			if (mainCategory.CurrentOption == 3)
			{
				this._container.Append(this._timePowersStrip);
				UICreativePowersMenu.MenuTree<UICreativePowersMenu.TimeSubcategory> timeCategory = this._timeCategory;
				if (timeCategory.Sliders.TryGetValue(timeCategory.CurrentOption, out element))
				{
					this._container.Append(element);
				}
			}
			if (mainCategory.CurrentOption == 4)
			{
				this._container.Append(this._weatherPowersStrip);
				UICreativePowersMenu.MenuTree<UICreativePowersMenu.WeatherSubcategory> weatherCategory = this._weatherCategory;
				if (weatherCategory.Sliders.TryGetValue(weatherCategory.CurrentOption, out element))
				{
					this._container.Append(element);
				}
			}
			if (mainCategory.CurrentOption == 6)
			{
				this._container.Append(this._personalPowersStrip);
				UICreativePowersMenu.MenuTree<UICreativePowersMenu.PersonalSubcategory> personalCategory = this._personalCategory;
				if (personalCategory.Sliders.TryGetValue(personalCategory.CurrentOption, out element))
				{
					this._container.Append(element);
				}
			}
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x0058BB8C File Offset: 0x00589D8C
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			if (Main.LocalPlayerCreativeTracker.ItemSacrifices.AnyNewUnlocksFromTeammates)
			{
				Rectangle hitbox = this._infiniteItemsButton.GetDimensions().ToRectangle();
				Utils.DrawNotificationIcon(spriteBatch, hitbox, 1f, false);
			}
			this.SetupGamepadPoints();
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x0058BBD8 File Offset: 0x00589DD8
		private void SetupGamepadPoints()
		{
			int num = 10000;
			List<SnapPoint> snapPoints = this.GetSnapPoints();
			List<SnapPoint> orderedPointsByCategoryName = this._helper.GetOrderedPointsByCategoryName(snapPoints, "strip 0");
			List<SnapPoint> orderedPointsByCategoryName2 = this._helper.GetOrderedPointsByCategoryName(snapPoints, "strip 1");
			List<SnapPoint> orderedPointsByCategoryName3 = this._helper.GetOrderedPointsByCategoryName(snapPoints, "strip 2");
			UILinkPoint[] array = null;
			UILinkPoint[] array2 = null;
			UILinkPoint[] array3 = null;
			if (orderedPointsByCategoryName.Count > 0)
			{
				array = this._helper.CreateUILinkStripVertical(ref num, orderedPointsByCategoryName);
			}
			if (orderedPointsByCategoryName2.Count > 0)
			{
				array2 = this._helper.CreateUILinkStripVertical(ref num, orderedPointsByCategoryName2);
			}
			if (orderedPointsByCategoryName3.Count > 0)
			{
				array3 = this._helper.CreateUILinkStripVertical(ref num, orderedPointsByCategoryName3);
			}
			if (array != null && array2 != null)
			{
				this._helper.LinkVerticalStrips(array, array2, (array.Length - array2.Length) / 2);
			}
			if (array2 != null && array3 != null)
			{
				this._helper.LinkVerticalStrips(array2, array3, (array.Length - array2.Length) / 2);
			}
			UILinkPoint uilinkPoint = null;
			UILinkPoint uilinkPoint2 = null;
			for (int i = 0; i < snapPoints.Count; i++)
			{
				SnapPoint snapPoint = snapPoints[i];
				string name = snapPoint.Name;
				if (!(name == "CreativeSacrificeConfirm"))
				{
					if (name == "CreativeInfinitesSearch")
					{
						uilinkPoint2 = this._helper.MakeLinkPointFromSnapPoint(num++, snapPoint);
					}
				}
				else
				{
					uilinkPoint = this._helper.MakeLinkPointFromSnapPoint(num++, snapPoint);
				}
			}
			UILinkPoint uilinkPoint3 = UILinkPointNavigator.Points[15000];
			List<SnapPoint> orderedPointsByCategoryName4 = this._helper.GetOrderedPointsByCategoryName(snapPoints, "CreativeInfinitesFilter");
			if (orderedPointsByCategoryName4.Count > 0)
			{
				UILinkPoint[] array4 = this._helper.CreateUILinkStripHorizontal(ref num, orderedPointsByCategoryName4);
				if (uilinkPoint2 != null)
				{
					uilinkPoint2.Up = array4[0].ID;
					for (int j = 0; j < array4.Length; j++)
					{
						array4[j].Down = uilinkPoint2.ID;
					}
				}
			}
			List<SnapPoint> orderedPointsByCategoryName5 = this._helper.GetOrderedPointsByCategoryName(snapPoints, "DynamicItemCollectionSlot");
			UILinkPoint[,] array5 = null;
			if (orderedPointsByCategoryName5.Count > 0)
			{
				array5 = this._helper.CreateUILinkPointGrid(ref num, orderedPointsByCategoryName5, this._infiniteItemsWindow.GetItemsPerLine(), uilinkPoint2, array[0], null, null);
				this._helper.LinkVerticalStripRightSideToSingle(array, array5[0, 0]);
			}
			else if (uilinkPoint2 != null)
			{
				this._helper.LinkVerticalStripRightSideToSingle(array, uilinkPoint2);
			}
			if (uilinkPoint2 != null && array5 != null)
			{
				this._helper.PairUpDown(uilinkPoint2, array5[0, 0]);
			}
			if (uilinkPoint3 != null && this.IsShowingResearchMenu)
			{
				this._helper.LinkVerticalStripRightSideToSingle(array, uilinkPoint3);
			}
			if (uilinkPoint != null)
			{
				this._helper.PairUpDown(uilinkPoint3, uilinkPoint);
				uilinkPoint.Left = array[0].ID;
			}
			if (Main.CreativeMenu.GamepadMoveToSearchButtonHack)
			{
				Main.CreativeMenu.GamepadMoveToSearchButtonHack = false;
				if (uilinkPoint2 != null)
				{
					UILinkPointNavigator.ChangePoint(uilinkPoint2.ID);
				}
			}
		}

		// Token: 0x04005352 RID: 21330
		private bool _hovered;

		// Token: 0x04005353 RID: 21331
		private PowerStripUIElement _mainPowerStrip;

		// Token: 0x04005354 RID: 21332
		private PowerStripUIElement _timePowersStrip;

		// Token: 0x04005355 RID: 21333
		private PowerStripUIElement _weatherPowersStrip;

		// Token: 0x04005356 RID: 21334
		private PowerStripUIElement _personalPowersStrip;

		// Token: 0x04005357 RID: 21335
		private UICreativeInfiniteItemsDisplay _infiniteItemsWindow;

		// Token: 0x04005358 RID: 21336
		private UIElement _infiniteItemsButton;

		// Token: 0x04005359 RID: 21337
		private UIElement _container;

		// Token: 0x0400535A RID: 21338
		private UICreativePowersMenu.MenuTree<UICreativePowersMenu.OpenMainSubCategory> _mainCategory = new UICreativePowersMenu.MenuTree<UICreativePowersMenu.OpenMainSubCategory>(UICreativePowersMenu.OpenMainSubCategory.None);

		// Token: 0x0400535B RID: 21339
		private UICreativePowersMenu.MenuTree<UICreativePowersMenu.WeatherSubcategory> _weatherCategory = new UICreativePowersMenu.MenuTree<UICreativePowersMenu.WeatherSubcategory>(UICreativePowersMenu.WeatherSubcategory.None);

		// Token: 0x0400535C RID: 21340
		private UICreativePowersMenu.MenuTree<UICreativePowersMenu.TimeSubcategory> _timeCategory = new UICreativePowersMenu.MenuTree<UICreativePowersMenu.TimeSubcategory>(UICreativePowersMenu.TimeSubcategory.None);

		// Token: 0x0400535D RID: 21341
		private UICreativePowersMenu.MenuTree<UICreativePowersMenu.PersonalSubcategory> _personalCategory = new UICreativePowersMenu.MenuTree<UICreativePowersMenu.PersonalSubcategory>(UICreativePowersMenu.PersonalSubcategory.None);

		// Token: 0x0400535E RID: 21342
		private const int INITIAL_LEFT_PIXELS = 20;

		// Token: 0x0400535F RID: 21343
		private const int LEFT_PIXELS_PER_STRIP_DEPTH = 60;

		// Token: 0x04005360 RID: 21344
		private const string STRIP_MAIN = "strip 0";

		// Token: 0x04005361 RID: 21345
		private const string STRIP_DEPTH_1 = "strip 1";

		// Token: 0x04005362 RID: 21346
		private const string STRIP_DEPTH_2 = "strip 2";

		// Token: 0x04005363 RID: 21347
		private UIGamepadHelper _helper;

		// Token: 0x020008FD RID: 2301
		private class MenuTree<TEnum> where TEnum : struct, IConvertible
		{
			// Token: 0x0600473F RID: 18239 RVA: 0x006C9D56 File Offset: 0x006C7F56
			public MenuTree(TEnum defaultValue)
			{
				this.CurrentOption = defaultValue.ToInt32(null);
			}

			// Token: 0x040073E0 RID: 29664
			public int CurrentOption;

			// Token: 0x040073E1 RID: 29665
			public Dictionary<int, GroupOptionButton<int>> Buttons = new Dictionary<int, GroupOptionButton<int>>();

			// Token: 0x040073E2 RID: 29666
			public Dictionary<int, UIElement> Sliders = new Dictionary<int, UIElement>();
		}

		// Token: 0x020008FE RID: 2302
		private enum OpenMainSubCategory
		{
			// Token: 0x040073E4 RID: 29668
			None,
			// Token: 0x040073E5 RID: 29669
			InfiniteItems,
			// Token: 0x040073E6 RID: 29670
			ResearchWindow,
			// Token: 0x040073E7 RID: 29671
			Time,
			// Token: 0x040073E8 RID: 29672
			Weather,
			// Token: 0x040073E9 RID: 29673
			EnemyStrengthSlider,
			// Token: 0x040073EA RID: 29674
			PersonalPowers
		}

		// Token: 0x020008FF RID: 2303
		private enum WeatherSubcategory
		{
			// Token: 0x040073EC RID: 29676
			None,
			// Token: 0x040073ED RID: 29677
			WindSlider,
			// Token: 0x040073EE RID: 29678
			RainSlider
		}

		// Token: 0x02000900 RID: 2304
		private enum TimeSubcategory
		{
			// Token: 0x040073F0 RID: 29680
			None,
			// Token: 0x040073F1 RID: 29681
			TimeRate
		}

		// Token: 0x02000901 RID: 2305
		private enum PersonalSubcategory
		{
			// Token: 0x040073F3 RID: 29683
			None,
			// Token: 0x040073F4 RID: 29684
			EnemySpawnRateSlider
		}
	}
}
