using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Social;
using Terraria.Social.Base;
using Terraria.UI;
using Terraria.UI.Gamepad;
using Terraria.Utilities.FileBrowser;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003A0 RID: 928
	public abstract class AWorkshopPublishInfoState<TPublishedObjectType> : UIState, IHaveBackButtonCommand
	{
		// Token: 0x06002A71 RID: 10865 RVA: 0x005845DC File Offset: 0x005827DC
		public AWorkshopPublishInfoState(UIState stateToGoBackTo, TPublishedObjectType dataObject)
		{
			this._previousUIState = stateToGoBackTo;
			this._dataObject = dataObject;
		}

		// Token: 0x06002A72 RID: 10866 RVA: 0x005845FC File Offset: 0x005827FC
		public override void OnInitialize()
		{
			base.OnInitialize();
			int num = 40;
			int num2 = 200;
			int num3 = 50 + num + 10;
			int num4 = 70;
			UIElement uielement = new UIElement();
			uielement.Width.Set(600f, 0f);
			uielement.Top.Set((float)num2, 0f);
			uielement.Height.Set((float)(-(float)num2), 1f);
			uielement.HAlign = 0.5f;
			UIPanel uipanel = new UIPanel();
			uipanel.Width.Set(0f, 1f);
			uipanel.Height.Set((float)(-(float)num3), 1f);
			uipanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
			this.AddBackButton(num, uielement);
			this.AddPublishButton(num, uielement);
			int num5 = 6 + num4;
			UIList uiList = this.AddUIList(uipanel, (float)num5);
			this.FillUIList(uiList);
			this.AddHorizontalSeparator(uipanel, 0f, 0).Top = new StyleDimension((float)(-(float)num4 + 3), 1f);
			this.AddDescriptionPanel(uipanel, (float)(num4 - 6), "desc");
			uielement.Append(uipanel);
			base.Append(uielement);
			this.SetDefaultOptions();
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x00584738 File Offset: 0x00582938
		private void SetDefaultOptions()
		{
			this._optionPublicity = WorkshopItemPublicSettingId.Public;
			GroupOptionButton<WorkshopItemPublicSettingId>[] publicityOptions = this._publicityOptions;
			for (int i = 0; i < publicityOptions.Length; i++)
			{
				publicityOptions[i].SetCurrentOption(this._optionPublicity);
			}
			this.SetTagsFromFoundEntry();
			this.UpdateImagePreview();
		}

		// Token: 0x06002A74 RID: 10868 RVA: 0x0058477C File Offset: 0x0058297C
		private void FillUIList(UIList uiList)
		{
			UIElement uielement = new UIElement
			{
				Width = new StyleDimension(0f, 0f),
				Height = new StyleDimension(0f, 0f)
			};
			uielement.SetPadding(0f);
			uiList.Add(uielement);
			uiList.Add(this.CreateSteamDisclaimer("disclaimer"));
			uiList.Add(this.CreatePreviewImageSelectionPanel("image"));
			uiList.Add(this.CreatePublicSettingsRow(0f, 44f, "public"));
			uiList.Add(this.CreateTagOptionsPanel(0f, 44, "tags"));
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x00584820 File Offset: 0x00582A20
		private UIElement CreatePreviewImageSelectionPanel(string tagGroup)
		{
			UIElement uielement = new UIElement();
			uielement.Width = new StyleDimension(0f, 1f);
			uielement.Height = new StyleDimension(80f, 0f);
			UIElement uielement2 = new UIElement
			{
				Width = new StyleDimension(72f, 0f),
				Height = new StyleDimension(72f, 0f),
				HAlign = 1f,
				VAlign = 0.5f,
				Left = new StyleDimension(-6f, 0f),
				Top = new StyleDimension(0f, 0f)
			};
			uielement2.SetPadding(0f);
			uielement.Append(uielement2);
			float num = 86f;
			this._defaultPreviewImageTexture = Main.Assets.Request<Texture2D>("Images/UI/Workshop/DefaultPreviewImage", 1);
			UIImage uiimage = new UIImage(this._defaultPreviewImageTexture)
			{
				Width = new StyleDimension(-4f, 1f),
				Height = new StyleDimension(-4f, 1f),
				HAlign = 0.5f,
				VAlign = 0.5f,
				ScaleToFit = true,
				AllowResizingDimensions = false
			};
			UIImage element = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders", 1))
			{
				HAlign = 0.5f,
				VAlign = 0.5f
			};
			uielement2.Append(uiimage);
			uielement2.Append(element);
			this._previewImageUIElement = uiimage;
			UICharacterNameButton uicharacterNameButton = new UICharacterNameButton(Language.GetText("Workshop.PreviewImagePathTitle"), Language.GetText("Workshop.PreviewImagePathEmpty"), Language.GetText("Workshop.PreviewImagePathDescription"))
			{
				Width = StyleDimension.FromPixelsAndPercent(-num, 1f),
				Height = new StyleDimension(0f, 1f)
			};
			uicharacterNameButton.OnLeftMouseDown += this.Click_SetPreviewImage;
			uicharacterNameButton.OnMouseOver += this.ShowOptionDescription;
			uicharacterNameButton.OnMouseOut += this.ClearOptionDescription;
			uicharacterNameButton.SetSnapPoint(tagGroup, 0, null, null);
			uielement.Append(uicharacterNameButton);
			this._previewImagePathPlate = uicharacterNameButton;
			return uielement;
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x00584A48 File Offset: 0x00582C48
		private void SetTagsFromFoundEntry()
		{
			FoundWorkshopEntryInfo foundWorkshopEntryInfo;
			if (!this.TryFindingTags(out foundWorkshopEntryInfo))
			{
				return;
			}
			if (foundWorkshopEntryInfo.tags != null)
			{
				foreach (GroupOptionButton<WorkshopTagOption> groupOptionButton in this._tagOptions)
				{
					bool flag = foundWorkshopEntryInfo.tags.Contains(groupOptionButton.OptionValue.InternalNameForAPIs);
					groupOptionButton.SetCurrentOption(flag ? groupOptionButton.OptionValue : null);
					groupOptionButton.SetColor(groupOptionButton.IsSelected ? new Color(152, 175, 235) : Colors.InventoryDefaultColor, 1f);
				}
			}
			this._optionPublicity = foundWorkshopEntryInfo.publicity;
			GroupOptionButton<WorkshopItemPublicSettingId>[] publicityOptions = this._publicityOptions;
			for (int i = 0; i < publicityOptions.Length; i++)
			{
				publicityOptions[i].SetCurrentOption(foundWorkshopEntryInfo.publicity);
			}
		}

		// Token: 0x06002A77 RID: 10871
		protected abstract bool TryFindingTags(out FoundWorkshopEntryInfo info);

		// Token: 0x06002A78 RID: 10872 RVA: 0x00584B38 File Offset: 0x00582D38
		private void Click_SetPreviewImage(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			this.OpenFileDialogueToSelectPreviewImage();
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x00584B58 File Offset: 0x00582D58
		private UIElement CreateSteamDisclaimer(string tagGroup)
		{
			float num = 60f;
			float num2 = 0f + num;
			GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(true, null, null, Color.White, null, 1f, 0.5f, 16f);
			groupOptionButton.HAlign = 0.5f;
			groupOptionButton.VAlign = 0f;
			groupOptionButton.Width = StyleDimension.FromPixelsAndPercent(0f, 1f);
			groupOptionButton.Left = StyleDimension.FromPixels(0f);
			groupOptionButton.Height = StyleDimension.FromPixelsAndPercent(num2 + 4f, 0f);
			groupOptionButton.Top = StyleDimension.FromPixels(0f);
			groupOptionButton.ShowHighlightWhenSelected = false;
			groupOptionButton.SetCurrentOption(false);
			groupOptionButton.Width.Set(0f, 1f);
			UIElement uielement = new UIElement
			{
				HAlign = 0.5f,
				VAlign = 1f,
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(num, 0f)
			};
			groupOptionButton.Append(uielement);
			UIText uitext = new UIText(Language.GetText("Workshop.SteamDisclaimer"), 1f, false)
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(-40f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				TextColor = Color.Cyan,
				IgnoresMouseInteraction = true
			};
			uitext.PaddingLeft = 20f;
			uitext.PaddingRight = 20f;
			uitext.PaddingTop = 4f;
			uitext.IsWrapped = true;
			this._disclaimerText = uitext;
			groupOptionButton.OnLeftClick += this.steamDisclaimerText_OnClick;
			groupOptionButton.OnMouseOver += this.steamDisclaimerText_OnMouseOver;
			groupOptionButton.OnMouseOut += this.steamDisclaimerText_OnMouseOut;
			uielement.Append(uitext);
			uitext.SetSnapPoint(tagGroup, 0, null, null);
			this._steamDisclaimerButton = uitext;
			return groupOptionButton;
		}

		// Token: 0x06002A7A RID: 10874 RVA: 0x00584D55 File Offset: 0x00582F55
		private void steamDisclaimerText_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			this._disclaimerText.TextColor = Color.Cyan;
			this.ClearOptionDescription(evt, listeningElement);
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x00584D6F File Offset: 0x00582F6F
		private void steamDisclaimerText_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			this._disclaimerText.TextColor = Color.LightCyan;
			this.ShowOptionDescription(evt, listeningElement);
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x00584DA0 File Offset: 0x00582FA0
		private void steamDisclaimerText_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			try
			{
				Platform.Get<IPathService>().OpenURL("https://steamcommunity.com/sharedfiles/workshoplegalagreement");
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06002A7D RID: 10877 RVA: 0x00584DD4 File Offset: 0x00582FD4
		public override void Recalculate()
		{
			this.UpdateScrollbar();
			base.Recalculate();
		}

		// Token: 0x06002A7E RID: 10878 RVA: 0x00584DE4 File Offset: 0x00582FE4
		private void UpdateScrollbar()
		{
			if (this._scrollbar == null)
			{
				return;
			}
			if (this._isScrollbarAttached && !this._scrollbar.CanScroll)
			{
				this._uiListContainer.RemoveChild(this._scrollbar);
				this._isScrollbarAttached = false;
				this._uiListRect.Width.Set(0f, 1f);
				return;
			}
			if (!this._isScrollbarAttached && this._scrollbar.CanScroll)
			{
				this._uiListContainer.Append(this._scrollbar);
				this._isScrollbarAttached = true;
				this._uiListRect.Width.Set(-25f, 1f);
			}
		}

		// Token: 0x06002A7F RID: 10879 RVA: 0x00584E8C File Offset: 0x0058308C
		private UIList AddUIList(UIElement container, float antiHeight)
		{
			this._uiListContainer = container;
			float num = 0f;
			UIElement uielement = new UIElement
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(-num * 2f, 1f),
				Left = StyleDimension.FromPixels(-num),
				Height = StyleDimension.FromPixelsAndPercent(-2f - antiHeight, 1f),
				OverflowHidden = true
			};
			this._listContainer = uielement;
			UISlicedImage uislicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/Workshop/ListBackground", 1))
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(0f, 1f),
				Color = Color.White * 0.7f
			};
			uislicedImage.SetSliceDepths(4);
			container.Append(uielement);
			uielement.Append(uislicedImage);
			UIList uilist = new UIList
			{
				Width = StyleDimension.FromPixelsAndPercent(-10f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(-4f, 1f),
				HAlign = 0.5f,
				VAlign = 0.5f,
				OverflowHidden = true
			};
			uilist.ManualSortMethod = new Action<List<UIElement>>(this.ManualIfnoSortingMethod);
			uilist.ListPadding = 5f;
			uielement.Append(uilist);
			UIScrollbar uiscrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Blue)
			{
				HAlign = 1f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(-num * 2f, 1f),
				Left = StyleDimension.FromPixels(-num),
				Height = StyleDimension.FromPixelsAndPercent(-14f - antiHeight, 1f),
				Top = StyleDimension.FromPixels(6f)
			};
			uiscrollbar.SetView(100f, 1000f);
			uilist.SetScrollbar(uiscrollbar);
			this._uiListRect = uielement;
			this._scrollbar = uiscrollbar;
			return uilist;
		}

		// Token: 0x06002A80 RID: 10880 RVA: 0x00009E06 File Offset: 0x00008006
		private void ManualIfnoSortingMethod(List<UIElement> list)
		{
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x0058507C File Offset: 0x0058327C
		private UIElement CreatePublicSettingsRow(float accumulatedHeight, float height, string tagGroup)
		{
			UIElement result;
			UIElement uielement;
			this.CreateStylizedCategoryPanel(height, "Workshop.CategoryTitlePublicity", out result, out uielement);
			WorkshopItemPublicSettingId[] array = new WorkshopItemPublicSettingId[3];
			array[0] = WorkshopItemPublicSettingId.Public;
			array[1] = WorkshopItemPublicSettingId.FriendsOnly;
			WorkshopItemPublicSettingId[] array2 = array;
			LocalizedText[] array3 = new LocalizedText[]
			{
				Language.GetText("Workshop.SettingsPublicityPublic"),
				Language.GetText("Workshop.SettingsPublicityFriendsOnly"),
				Language.GetText("Workshop.SettingsPublicityPrivate")
			};
			LocalizedText[] array4 = new LocalizedText[]
			{
				Language.GetText("Workshop.SettingsPublicityPublicDescription"),
				Language.GetText("Workshop.SettingsPublicityFriendsOnlyDescription"),
				Language.GetText("Workshop.SettingsPublicityPrivateDescription")
			};
			Color[] array5 = new Color[]
			{
				Color.White,
				Color.White,
				Color.White
			};
			string[] array6 = new string[]
			{
				"Images/UI/Workshop/PublicityPublic",
				"Images/UI/Workshop/PublicityFriendsOnly",
				"Images/UI/Workshop/PublicityPrivate"
			};
			float num = 0.98f;
			GroupOptionButton<WorkshopItemPublicSettingId>[] array7 = new GroupOptionButton<WorkshopItemPublicSettingId>[array2.Length];
			for (int i = 0; i < array7.Length; i++)
			{
				GroupOptionButton<WorkshopItemPublicSettingId> groupOptionButton = new GroupOptionButton<WorkshopItemPublicSettingId>(array2[i], array3[i], array4[i], array5[i], array6[i], 1f, 1f, 16f);
				groupOptionButton.Width = StyleDimension.FromPixelsAndPercent((float)(-4 * (array7.Length - 1)), 1f / (float)array7.Length * num);
				groupOptionButton.HAlign = (float)i / (float)(array7.Length - 1);
				groupOptionButton.Left = StyleDimension.FromPercent((1f - num) * (1f - groupOptionButton.HAlign * 2f));
				groupOptionButton.Top.Set(accumulatedHeight, 0f);
				groupOptionButton.OnLeftMouseDown += this.ClickPublicityOption;
				groupOptionButton.OnMouseOver += this.ShowOptionDescription;
				groupOptionButton.OnMouseOut += this.ClearOptionDescription;
				groupOptionButton.SetSnapPoint(tagGroup, i, null, null);
				uielement.Append(groupOptionButton);
				array7[i] = groupOptionButton;
			}
			this._publicityOptions = array7;
			return result;
		}

		// Token: 0x06002A82 RID: 10882 RVA: 0x0058528C File Offset: 0x0058348C
		private UIElement CreateTagOptionsPanel(float accumulatedHeight, int heightPerRow, string tagGroup)
		{
			List<WorkshopTagOption> tagsToShow = this.GetTagsToShow();
			int num = 3;
			int num2 = (int)Math.Ceiling((double)((float)tagsToShow.Count / (float)num));
			int num3 = heightPerRow * num2;
			UIElement result;
			UIElement uielement;
			this.CreateStylizedCategoryPanel((float)num3, "Workshop.CategoryTitleTags", out result, out uielement);
			float num4 = 0.98f;
			List<GroupOptionButton<WorkshopTagOption>> list = new List<GroupOptionButton<WorkshopTagOption>>();
			for (int i = 0; i < tagsToShow.Count; i++)
			{
				WorkshopTagOption workshopTagOption = tagsToShow[i];
				GroupOptionButton<WorkshopTagOption> groupOptionButton = new GroupOptionButton<WorkshopTagOption>(workshopTagOption, Language.GetText(workshopTagOption.NameKey), Language.GetText(workshopTagOption.NameKey + "Description"), Color.White, null, 1f, 0.5f, 16f);
				groupOptionButton.ShowHighlightWhenSelected = false;
				groupOptionButton.SetCurrentOption(null);
				int num5 = i / num;
				int num6 = i - num5 * num;
				groupOptionButton.Width = StyleDimension.FromPixelsAndPercent((float)(-4 * (num - 1)), 1f / (float)num * num4);
				groupOptionButton.HAlign = (float)num6 / (float)(num - 1);
				groupOptionButton.Left = StyleDimension.FromPercent((1f - num4) * (1f - groupOptionButton.HAlign * 2f));
				groupOptionButton.Top.Set((float)(num5 * heightPerRow), 0f);
				groupOptionButton.OnLeftMouseDown += this.ClickTagOption;
				groupOptionButton.OnMouseOver += this.ShowOptionDescription;
				groupOptionButton.OnMouseOut += this.ClearOptionDescription;
				groupOptionButton.SetSnapPoint(tagGroup, i, null, null);
				uielement.Append(groupOptionButton);
				list.Add(groupOptionButton);
			}
			this._tagOptions = list;
			return result;
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x0058543C File Offset: 0x0058363C
		private void CreateStylizedCategoryPanel(float height, string titleTextKey, out UIElement entirePanel, out UIElement innerPanel)
		{
			float num = 44f;
			UISlicedImage uislicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel", 1))
			{
				HAlign = 0.5f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Left = StyleDimension.FromPixels(0f),
				Height = StyleDimension.FromPixelsAndPercent(height + num + 4f, 0f),
				Top = StyleDimension.FromPixels(0f)
			};
			uislicedImage.SetSliceDepths(8);
			uislicedImage.Color = Color.White * 0.7f;
			innerPanel = new UIElement
			{
				HAlign = 0.5f,
				VAlign = 1f,
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(height, 0f)
			};
			uislicedImage.Append(innerPanel);
			this.AddHorizontalSeparator(uislicedImage, num, 4);
			UIText uitext = new UIText(Language.GetText(titleTextKey), 1f, false)
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(-40f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(num, 0f),
				Top = StyleDimension.FromPixelsAndPercent(5f, 0f)
			};
			uitext.PaddingLeft = 20f;
			uitext.PaddingRight = 20f;
			uitext.PaddingTop = 6f;
			uitext.IsWrapped = false;
			uislicedImage.Append(uitext);
			entirePanel = uislicedImage;
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x005855D4 File Offset: 0x005837D4
		private void ClickTagOption(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<WorkshopTagOption> groupOptionButton = (GroupOptionButton<WorkshopTagOption>)listeningElement;
			groupOptionButton.SetCurrentOption(groupOptionButton.IsSelected ? null : groupOptionButton.OptionValue);
			groupOptionButton.SetColor(groupOptionButton.IsSelected ? new Color(152, 175, 235) : Colors.InventoryDefaultColor, 1f);
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x00585630 File Offset: 0x00583830
		private void ClickPublicityOption(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<WorkshopItemPublicSettingId> groupOptionButton = (GroupOptionButton<WorkshopItemPublicSettingId>)listeningElement;
			this._optionPublicity = groupOptionButton.OptionValue;
			GroupOptionButton<WorkshopItemPublicSettingId>[] publicityOptions = this._publicityOptions;
			for (int i = 0; i < publicityOptions.Length; i++)
			{
				publicityOptions[i].SetCurrentOption(groupOptionButton.OptionValue);
			}
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x00585674 File Offset: 0x00583874
		public void ShowOptionDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			LocalizedText localizedText = null;
			GroupOptionButton<WorkshopItemPublicSettingId> groupOptionButton = listeningElement as GroupOptionButton<WorkshopItemPublicSettingId>;
			if (groupOptionButton != null)
			{
				localizedText = groupOptionButton.Description;
			}
			UICharacterNameButton uicharacterNameButton = listeningElement as UICharacterNameButton;
			if (uicharacterNameButton != null)
			{
				localizedText = uicharacterNameButton.Description;
			}
			GroupOptionButton<bool> groupOptionButton2 = listeningElement as GroupOptionButton<bool>;
			if (groupOptionButton2 != null)
			{
				localizedText = groupOptionButton2.Description;
			}
			GroupOptionButton<WorkshopTagOption> groupOptionButton3 = listeningElement as GroupOptionButton<WorkshopTagOption>;
			if (groupOptionButton3 != null)
			{
				localizedText = groupOptionButton3.Description;
			}
			if (listeningElement == this._steamDisclaimerButton)
			{
				localizedText = Language.GetText("Workshop.SteamDisclaimerDescrpition");
			}
			if (localizedText != null)
			{
				this._descriptionText.SetText(localizedText);
			}
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x005856ED File Offset: 0x005838ED
		public void ClearOptionDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			this._descriptionText.SetText(Language.GetText("Workshop.InfoDescriptionDefault"));
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x00585704 File Offset: 0x00583904
		private UIElement CreateInsturctionsPanel(float accumulatedHeight, float height, string tagGroup)
		{
			float num = 0f;
			UISlicedImage uislicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", 1));
			uislicedImage.HAlign = 0.5f;
			uislicedImage.VAlign = 0f;
			uislicedImage.Width = StyleDimension.FromPixelsAndPercent(-num * 2f, 1f);
			uislicedImage.Left = StyleDimension.FromPixels(-num);
			uislicedImage.Height = StyleDimension.FromPixelsAndPercent(height, 0f);
			uislicedImage.Top = StyleDimension.FromPixels(accumulatedHeight);
			uislicedImage.SetSliceDepths(10);
			uislicedImage.Color = Color.LightGray * 0.7f;
			UIText uitext = new UIText(Language.GetText(this._instructionsTextKey), 1f, false)
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(-40f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Top = StyleDimension.FromPixelsAndPercent(5f, 0f)
			};
			uitext.PaddingLeft = 20f;
			uitext.PaddingRight = 20f;
			uitext.PaddingTop = 6f;
			uitext.IsWrapped = true;
			uislicedImage.Append(uitext);
			return uislicedImage;
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x0058583C File Offset: 0x00583A3C
		private void AddDescriptionPanel(UIElement container, float height, string tagGroup)
		{
			float num = 0f;
			UISlicedImage uislicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", 1))
			{
				HAlign = 0.5f,
				VAlign = 1f,
				Width = StyleDimension.FromPixelsAndPercent(-num * 2f, 1f),
				Left = StyleDimension.FromPixels(-num),
				Height = StyleDimension.FromPixelsAndPercent(height, 0f),
				Top = StyleDimension.FromPixels(2f)
			};
			uislicedImage.SetSliceDepths(10);
			uislicedImage.Color = Color.LightGray * 0.7f;
			container.Append(uislicedImage);
			UIText uitext = new UIText(Language.GetText("Workshop.InfoDescriptionDefault"), 0.85f, false)
			{
				HAlign = 0f,
				VAlign = 1f,
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(0f, 1f)
			};
			uitext.PaddingLeft = 4f;
			uitext.PaddingRight = 4f;
			uitext.PaddingTop = 4f;
			uitext.IsWrapped = true;
			uislicedImage.Append(uitext);
			this._descriptionText = uitext;
		}

		// Token: 0x06002A8A RID: 10890
		protected abstract string GetPublishedObjectDisplayName();

		// Token: 0x06002A8B RID: 10891
		protected abstract List<WorkshopTagOption> GetTagsToShow();

		// Token: 0x06002A8C RID: 10892 RVA: 0x00585971 File Offset: 0x00583B71
		private void Click_GoBack(UIMouseEvent evt, UIElement listeningElement)
		{
			this.HandleBackButtonUsage();
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x00585979 File Offset: 0x00583B79
		public void HandleBackButtonUsage()
		{
			if (this._previousUIState == null)
			{
				Main.menuMode = 0;
				return;
			}
			Main.menuMode = 888;
			Main.MenuUI.SetState(this._previousUIState);
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x005859A4 File Offset: 0x00583BA4
		private void Click_Publish(UIMouseEvent evt, UIElement listeningElement)
		{
			this.GoToPublishConfirmation();
		}

		// Token: 0x06002A8F RID: 10895
		protected abstract void GoToPublishConfirmation();

		// Token: 0x06002A90 RID: 10896 RVA: 0x005859AC File Offset: 0x00583BAC
		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x00584489 File Offset: 0x00582689
		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x00585A04 File Offset: 0x00583C04
		private void AddPublishButton(int backButtonYLift, UIElement outerContainer)
		{
			UITextPanel<LocalizedText> uitextPanel = new UITextPanel<LocalizedText>(Language.GetText("Workshop.Publish"), 0.7f, true);
			uitextPanel.Width.Set(-10f, 0.5f);
			uitextPanel.Height.Set(50f, 0f);
			uitextPanel.VAlign = 1f;
			uitextPanel.Top.Set((float)(-(float)backButtonYLift), 0f);
			uitextPanel.HAlign = 1f;
			uitextPanel.OnMouseOver += this.FadedMouseOver;
			uitextPanel.OnMouseOut += this.FadedMouseOut;
			uitextPanel.OnLeftClick += this.Click_Publish;
			uitextPanel.SetSnapPoint("publish", 0, null, null);
			outerContainer.Append(uitextPanel);
			this._publishButton = uitextPanel;
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x00585ADC File Offset: 0x00583CDC
		private void AddBackButton(int backButtonYLift, UIElement outerContainer)
		{
			UITextPanel<LocalizedText> uitextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, true);
			uitextPanel.Width.Set(-10f, 0.5f);
			uitextPanel.Height.Set(50f, 0f);
			uitextPanel.VAlign = 1f;
			uitextPanel.Top.Set((float)(-(float)backButtonYLift), 0f);
			uitextPanel.HAlign = 0f;
			uitextPanel.OnMouseOver += this.FadedMouseOver;
			uitextPanel.OnMouseOut += this.FadedMouseOut;
			uitextPanel.OnLeftClick += this.Click_GoBack;
			uitextPanel.SetSnapPoint("back", 0, null, null);
			outerContainer.Append(uitextPanel);
			this._backButton = uitextPanel;
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x00585BB4 File Offset: 0x00583DB4
		private UIElement AddHorizontalSeparator(UIElement Container, float accumualtedHeight, int widthReduction = 0)
		{
			UIHorizontalSeparator uihorizontalSeparator = new UIHorizontalSeparator(2, true)
			{
				Width = StyleDimension.FromPixelsAndPercent((float)(-(float)widthReduction), 1f),
				HAlign = 0.5f,
				Top = StyleDimension.FromPixels(accumualtedHeight - 8f),
				Color = Color.Lerp(Color.White, new Color(63, 65, 151, 255), 0.85f) * 0.9f
			};
			Container.Append(uihorizontalSeparator);
			return uihorizontalSeparator;
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x00585C34 File Offset: 0x00583E34
		protected WorkshopItemPublishSettings GetPublishSettings()
		{
			WorkshopItemPublishSettings workshopItemPublishSettings = new WorkshopItemPublishSettings();
			workshopItemPublishSettings.Publicity = this._optionPublicity;
			workshopItemPublishSettings.UsedTags = (from x in this._tagOptions
			where x.IsSelected
			select x.OptionValue).ToArray<WorkshopTagOption>();
			workshopItemPublishSettings.PreviewImagePath = this._previewImagePath;
			return workshopItemPublishSettings;
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x00585CB8 File Offset: 0x00583EB8
		private void OpenFileDialogueToSelectPreviewImage()
		{
			ExtensionFilter[] extensions = new ExtensionFilter[]
			{
				new ExtensionFilter("Image files", new string[]
				{
					"png",
					"jpg",
					"jpeg"
				})
			};
			string text = FileBrowser.OpenFilePanel("Open icon", extensions);
			if (text != null)
			{
				this._previewImagePath = text;
				this.UpdateImagePreview();
			}
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x00585D18 File Offset: 0x00583F18
		private string PrettifyPath(string path)
		{
			if (path == null)
			{
				return path;
			}
			char[] anyOf = new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			};
			int num = path.LastIndexOfAny(anyOf);
			if (num != -1)
			{
				path = path.Substring(num + 1);
			}
			if (path.Length > 30)
			{
				path = path.Substring(0, 30) + "…";
			}
			return path;
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x00585D78 File Offset: 0x00583F78
		private void UpdateImagePreview()
		{
			Texture2D texture2D = null;
			string contents = this.PrettifyPath(this._previewImagePath);
			this._previewImagePathPlate.SetContents(contents);
			if (this._previewImagePath != null)
			{
				try
				{
					using (FileStream fileStream = File.OpenRead(this._previewImagePath))
					{
						texture2D = Texture2D.FromStream(Main.graphics.GraphicsDevice, fileStream);
					}
				}
				catch (Exception exception)
				{
					FancyErrorPrinter.ShowFailedToLoadAssetError(exception, this._previewImagePath);
				}
			}
			if (texture2D != null && (texture2D.Width > 512 || texture2D.Height > 512))
			{
				object obj = new
				{
					texture2D.Width,
					texture2D.Height
				};
				string textValueWith = Language.GetTextValueWith("Workshop.ReportIssue_FailedToPublish_ImageSizeIsTooLarge", obj);
				if (SocialAPI.Workshop != null)
				{
					SocialAPI.Workshop.IssueReporter.ReportInstantUploadProblemFromValue(textValueWith);
				}
				this._previewImagePath = null;
				this._previewImagePathPlate.SetContents(null);
				this._previewImageUIElement.SetImage(this._defaultPreviewImageTexture);
				return;
			}
			if (this._previewImageTransientTexture != null)
			{
				this._previewImageTransientTexture.Dispose();
				this._previewImageTransientTexture = null;
			}
			if (texture2D != null)
			{
				this._previewImageUIElement.SetImage(texture2D);
				this._previewImageTransientTexture = texture2D;
				return;
			}
			this._previewImageUIElement.SetImage(this._defaultPreviewImageTexture);
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x00585EB8 File Offset: 0x005840B8
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			this.SetupGamepadPoints(spriteBatch);
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x00585EC8 File Offset: 0x005840C8
		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 7;
			int num = 3000;
			int num2 = num;
			List<SnapPoint> snapPoints = this.GetSnapPoints();
			this._helper.RemovePointsOutOfView(snapPoints, this._listContainer, spriteBatch);
			UILinkPoint linkPoint = this._helper.GetLinkPoint(num2++, this._backButton);
			UILinkPoint linkPoint2 = this._helper.GetLinkPoint(num2++, this._publishButton);
			SnapPoint snap = null;
			SnapPoint snap2 = null;
			for (int i = 0; i < snapPoints.Count; i++)
			{
				SnapPoint snapPoint = snapPoints[i];
				string name = snapPoint.Name;
				if (!(name == "disclaimer"))
				{
					if (name == "image")
					{
						snap2 = snapPoint;
					}
				}
				else
				{
					snap = snapPoint;
				}
			}
			UILinkPoint upSide = this._helper.TryMakeLinkPoint(ref num2, snap);
			UILinkPoint uilinkPoint = this._helper.TryMakeLinkPoint(ref num2, snap2);
			this._helper.PairLeftRight(linkPoint, linkPoint2);
			this._helper.PairUpDown(upSide, uilinkPoint);
			UILinkPoint[] array = this._helper.CreateUILinkStripHorizontal(ref num2, (from x in snapPoints
			where x.Name == "public"
			select x).ToList<SnapPoint>());
			if (array.Length != 0)
			{
				this._helper.LinkHorizontalStripUpSideToSingle(array, uilinkPoint);
			}
			UILinkPoint topLinkPoint = (array.Length != 0) ? array[0] : null;
			UILinkPoint bottomLinkPoint = linkPoint;
			List<SnapPoint> pointsForGrid = (from x in snapPoints
			where x.Name == "tags"
			select x).ToList<SnapPoint>();
			UILinkPoint[,] array2 = this._helper.CreateUILinkPointGrid(ref num2, pointsForGrid, 3, topLinkPoint, null, null, bottomLinkPoint);
			int num3 = array2.GetLength(1) - 1;
			if (num3 >= 0)
			{
				this._helper.LinkHorizontalStripBottomSideToSingle(array, array2[0, 0]);
				for (int j = array2.GetLength(0) - 1; j >= 0; j--)
				{
					if (array2[j, num3] != null)
					{
						this._helper.PairUpDown(array2[j, num3], linkPoint2);
						break;
					}
				}
			}
			UILinkPoint upSide2 = UILinkPointNavigator.Points[num2 - 1];
			this._helper.PairUpDown(upSide2, linkPoint);
			this._helper.MoveToVisuallyClosestPoint(num, num2);
		}

		// Token: 0x040052F7 RID: 21239
		protected UIState _previousUIState;

		// Token: 0x040052F8 RID: 21240
		protected TPublishedObjectType _dataObject;

		// Token: 0x040052F9 RID: 21241
		protected string _publishedObjectNameDescriptorTexKey;

		// Token: 0x040052FA RID: 21242
		protected string _instructionsTextKey;

		// Token: 0x040052FB RID: 21243
		private UIElement _uiListContainer;

		// Token: 0x040052FC RID: 21244
		private UIElement _uiListRect;

		// Token: 0x040052FD RID: 21245
		private UIScrollbar _scrollbar;

		// Token: 0x040052FE RID: 21246
		private bool _isScrollbarAttached;

		// Token: 0x040052FF RID: 21247
		private UIText _descriptionText;

		// Token: 0x04005300 RID: 21248
		private UIElement _listContainer;

		// Token: 0x04005301 RID: 21249
		private UIElement _backButton;

		// Token: 0x04005302 RID: 21250
		private UIElement _publishButton;

		// Token: 0x04005303 RID: 21251
		private WorkshopItemPublicSettingId _optionPublicity = WorkshopItemPublicSettingId.Public;

		// Token: 0x04005304 RID: 21252
		private GroupOptionButton<WorkshopItemPublicSettingId>[] _publicityOptions;

		// Token: 0x04005305 RID: 21253
		private List<GroupOptionButton<WorkshopTagOption>> _tagOptions;

		// Token: 0x04005306 RID: 21254
		private UICharacterNameButton _previewImagePathPlate;

		// Token: 0x04005307 RID: 21255
		private Texture2D _previewImageTransientTexture;

		// Token: 0x04005308 RID: 21256
		private UIImage _previewImageUIElement;

		// Token: 0x04005309 RID: 21257
		private string _previewImagePath;

		// Token: 0x0400530A RID: 21258
		private Asset<Texture2D> _defaultPreviewImageTexture;

		// Token: 0x0400530B RID: 21259
		private UIElement _steamDisclaimerButton;

		// Token: 0x0400530C RID: 21260
		private UIText _disclaimerText;

		// Token: 0x0400530D RID: 21261
		private UIGamepadHelper _helper;
	}
}
