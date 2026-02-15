using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x0200039F RID: 927
	public class UIWorldCreationAdvanced : UIState, IHaveBackButtonCommand
	{
		// Token: 0x06002A53 RID: 10835 RVA: 0x0058344A File Offset: 0x0058164A
		public UIWorldCreationAdvanced(UIWorldCreation state, bool allowScrolling = false)
		{
			this._creationState = state;
			this._creationState.SubmitSeed = new UIWorldCreation.SubmitSeedEvent(this.UpdateContents);
			this._allowScrolling = allowScrolling;
			this.BuildPage();
			this.Prepare();
		}

		// Token: 0x06002A54 RID: 10836 RVA: 0x00583483 File Offset: 0x00581683
		private void Prepare()
		{
			this.UpdateContents();
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x0058348C File Offset: 0x0058168C
		private void UpdateContents()
		{
			this._creationState.FillSeedContent(this._seedPlate);
			foreach (GroupOptionButton<AWorldGenerationOption> groupOptionButton in this._seedButtons)
			{
				groupOptionButton.SetCurrentOption(groupOptionButton.OptionValue.Enabled ? groupOptionButton.OptionValue : null);
			}
		}

		// Token: 0x06002A56 RID: 10838 RVA: 0x005834E0 File Offset: 0x005816E0
		private void BuildPage()
		{
			base.RemoveAllChildren();
			UIElement uielement = new UIElement
			{
				Width = StyleDimension.FromPixels(500f),
				Height = StyleDimension.FromPixelsAndPercent(-200f, 1f),
				Top = StyleDimension.FromPixels(202f),
				HAlign = 0.5f,
				VAlign = 0f
			};
			if (!this._allowScrolling)
			{
				uielement.MaxHeight = StyleDimension.FromPixels(400f);
			}
			uielement.SetPadding(0f);
			base.Append(uielement);
			UIPanel uipanel = new UIPanel
			{
				Width = StyleDimension.FromPercent(1f),
				Height = StyleDimension.FromPixelsAndPercent(-102f, 1f),
				BackgroundColor = new Color(33, 43, 79) * 0.8f
			};
			uipanel.SetPadding(0f);
			uielement.Append(uipanel);
			this.MakeBackAndCreatebuttons(uielement);
			UIElement uielement2 = new UIElement
			{
				Top = StyleDimension.FromPixelsAndPercent(0f, 0f),
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				HAlign = 1f
			};
			uielement2.SetPadding(0f);
			uielement2.PaddingTop = 8f;
			uielement2.PaddingBottom = 12f;
			uipanel.Append(uielement2);
			this.MakeInfoMenu(uielement2);
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x0058364C File Offset: 0x0058184C
		private void MakeInfoMenu(UIElement parentContainer)
		{
			UIElement uielement = new UIElement
			{
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				HAlign = 0.5f,
				VAlign = 0f
			};
			uielement.SetPadding(10f);
			uielement.PaddingBottom = 0f;
			uielement.PaddingTop = 0f;
			parentContainer.Append(uielement);
			this.AddSeedButtons(uielement);
			this.AddListArea(uielement);
			this.AddDescriptionPanel(uielement);
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x005836DC File Offset: 0x005818DC
		private void AddListArea(UIElement infoContainer)
		{
			int num = 0;
			UIList uilist = new UIList
			{
				Width = StyleDimension.FromPixelsAndPercent(-48f, 1f),
				Height = StyleDimension.FromPixelsAndPercent((float)(-138 - num * 2), 1f),
				HAlign = 0f,
				VAlign = 0f,
				Top = StyleDimension.FromPixels((float)(44 + num)),
				Left = StyleDimension.FromPixels(24f)
			};
			num = 4;
			UIScrollbar uiscrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Blue)
			{
				Height = StyleDimension.FromPixelsAndPercent((float)(-138 - num * 2), 1f),
				Top = StyleDimension.FromPixels((float)(44 + num)),
				HAlign = 1f
			};
			uilist.SetScrollbar(uiscrollbar);
			infoContainer.Append(uilist);
			if (this._allowScrolling)
			{
				infoContainer.Append(uiscrollbar);
			}
			this.AddSpecialSeedOptions(uilist);
			this._optionList = uilist;
		}

		// Token: 0x06002A59 RID: 10841 RVA: 0x005837C0 File Offset: 0x005819C0
		public void RefreshSecretSeedButton()
		{
			bool flag = SecretSeedsTracker.SeedsForInterface.Count > 0 || this._creationState.HasEnteredSpecialSeed || this._creationState.HasDisabledSecretSeed;
			if (this._secretSeedButton == null && flag)
			{
				int num = this._seedButtons.Length;
				int num2 = num % 6;
				int num3 = num / 6;
				this._secretSeedButton = new GroupOptionButton<bool>(true, null, null, Color.White, null, 1f, 0.5f, 10f)
				{
					Width = StyleDimension.FromPixels(60f),
					Height = StyleDimension.FromPixels(60f),
					InnerHighlightRim = 4,
					HAlign = (float)num2 / 5f,
					Top = StyleDimension.FromPixelsAndPercent((float)(num3 * 67 + 3), 0f),
					ShowHighlightWhenSelected = true
				};
				this._secretSeedButton.SetCurrentOption(this._creationState.HasEnteredSpecialSeed);
				UIImage uiimage = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/Seed_Secret", 1).Value)
				{
					Left = StyleDimension.FromPixels(-1f)
				};
				uiimage.OnUpdate += this.UpdateIconOpacity;
				this._secretSeedButton.Append(uiimage);
				this._secretSeedButton.SetSnapPoint("seeds", num, null, null);
				this._secretSeedButton.OnMouseOver += this.ShowSecretSeedDescription;
				this._secretSeedButton.OnMouseOut += this.ClearOptionDescription;
				this._secretSeedButton.OnDraw += this._creationState.DrawSpecialSeedRingCallback;
				this._secretSeedButton.OnLeftClick += this.SecretSeedButton_OnLeftClick;
				this._seedButtonRegion.Append(this._secretSeedButton);
				return;
			}
			if (this._secretSeedButton != null && !flag)
			{
				this._seedButtonRegion.RemoveChild(this._secretSeedButton);
				this._secretSeedButton = null;
				return;
			}
			if (this._secretSeedButton != null)
			{
				this._secretSeedButton.SetCurrentOption(this._creationState.HasEnteredSpecialSeed);
			}
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x005839CC File Offset: 0x00581BCC
		private void SecretSeedButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
		{
			UIWorldCreationAdvancedSecretSeedsList state = new UIWorldCreationAdvancedSecretSeedsList(this, this._creationState);
			Main.MenuUI.SetState(state);
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x00583A08 File Offset: 0x00581C08
		private void AddSpecialSeedOptions(UIList listArea)
		{
			int num = 6;
			GroupOptionButton<AWorldGenerationOption>[] array = this.PrepareSeedButtons();
			this._seedButtons = array;
			this._seedButtonRegion = new UIElement
			{
				Width = StyleDimension.FromPercent(1f),
				Height = StyleDimension.FromPixels((float)Math.Ceiling((double)array.Length / (double)num) * 70f - 10f)
			};
			listArea.Add(this._seedButtonRegion);
			for (int i = 0; i < array.Length; i++)
			{
				GroupOptionButton<AWorldGenerationOption> groupOptionButton = array[i];
				int num2 = i % 6;
				int num3 = i / 6;
				groupOptionButton.HAlign = (float)num2 / 5f;
				groupOptionButton.Top.Set((float)(num3 * 67 + 3), 0f);
				groupOptionButton.OnLeftMouseDown += this.ClickSeedOption;
				groupOptionButton.SetSnapPoint("seeds", i, null, null);
				this._seedButtonRegion.Append(groupOptionButton);
				array[i] = groupOptionButton;
			}
			this.RefreshSecretSeedButton();
		}

		// Token: 0x06002A5C RID: 10844 RVA: 0x00583B00 File Offset: 0x00581D00
		private GroupOptionButton<AWorldGenerationOption>[] PrepareSeedButtons()
		{
			List<GroupOptionButton<AWorldGenerationOption>> list = new List<GroupOptionButton<AWorldGenerationOption>>();
			foreach (AWorldGenerationOption aworldGenerationOption in WorldGenerationOptions.Options)
			{
				aworldGenerationOption.Load();
				list.Add(this.CreateButton(new UIWorldCreationAdvanced.WorldSpecialSeedOption
				{
					Seed = aworldGenerationOption,
					Description = aworldGenerationOption.Description,
					Title = aworldGenerationOption.Title,
					Element = aworldGenerationOption.ProvideUIElement()
				}));
			}
			return list.ToArray();
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x00583B9C File Offset: 0x00581D9C
		private void ClickSeedOption(UIMouseEvent evt, UIElement listeningElement)
		{
			AWorldGenerationOption optionValue = ((GroupOptionButton<AWorldGenerationOption>)listeningElement).OptionValue;
			this._creationState.ToggleSeedOption(optionValue);
			this.UpdateContents();
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x00583BC8 File Offset: 0x00581DC8
		private GroupOptionButton<AWorldGenerationOption> CreateButton(UIWorldCreationAdvanced.WorldSpecialSeedOption option)
		{
			GroupOptionButton<AWorldGenerationOption> groupOptionButton = new GroupOptionButton<AWorldGenerationOption>(option.Seed, null, option.Description, Color.White, null, 1f, 1f, 16f)
			{
				Width = StyleDimension.FromPixels(60f),
				Height = StyleDimension.FromPixels(60f),
				InnerHighlightRim = 4
			};
			groupOptionButton.OnMouseOver += delegate(UIMouseEvent evt, UIElement elem)
			{
				this.ShowOptionDescription(option.Description, option.Title);
			};
			groupOptionButton.OnMouseOut += this.ClearOptionDescription;
			UIElement element = option.Element;
			element.OnUpdate += this.UpdateIconOpacity;
			groupOptionButton.Append(element);
			if (false)
			{
				UIImage element2 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/IconCompletion", 1))
				{
					HAlign = 0.5f,
					VAlign = 0.5f,
					Top = new StyleDimension(-9f, 0f),
					Left = new StyleDimension(-3f, 0f),
					IgnoresMouseInteraction = true
				};
				groupOptionButton.Append(element2);
			}
			return groupOptionButton;
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x00583CF4 File Offset: 0x00581EF4
		private void UpdateIconOpacity(UIElement affectedElement)
		{
			GroupOptionButton<AWorldGenerationOption> groupOptionButton = affectedElement.Parent as GroupOptionButton<AWorldGenerationOption>;
			if (groupOptionButton == null)
			{
				return;
			}
			float scale = 0.5f;
			bool flag = groupOptionButton.IsSelected || groupOptionButton.IsMouseHovering;
			UIImage uiimage = affectedElement as UIImage;
			if (uiimage != null)
			{
				uiimage.Color = (flag ? Color.White : (Color.White * scale));
			}
			UIImageFramed uiimageFramed = affectedElement as UIImageFramed;
			if (uiimageFramed != null)
			{
				uiimageFramed.Color = (flag ? Color.White : (Color.White * scale));
			}
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x00583D78 File Offset: 0x00581F78
		private void AddDescriptionPanel(UIElement container)
		{
			float num = 0f;
			UISlicedImage uislicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", 1))
			{
				HAlign = 0.5f,
				VAlign = 1f,
				Width = StyleDimension.FromPixelsAndPercent(-num * 2f, 1f),
				Left = StyleDimension.FromPixels(-num),
				Height = StyleDimension.FromPixelsAndPercent(88f, 0f),
				Top = StyleDimension.FromPixels(2f)
			};
			uislicedImage.SetSliceDepths(10);
			uislicedImage.Color = Color.LightGray * 0.7f;
			container.Append(uislicedImage);
			UIText uitext = new UIText(Language.GetText("UI.WorldDescriptionDefault"), 0.82f, false)
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(24f, 0f),
				Top = StyleDimension.FromPixelsAndPercent(0f, 0f),
				PaddingLeft = 20f,
				PaddingRight = 20f,
				PaddingTop = 6f
			};
			uislicedImage.Append(uitext);
			this._titleText = uitext;
			UIHorizontalSeparator element = new UIHorizontalSeparator(2, true)
			{
				Width = StyleDimension.FromPercent(1f),
				Top = StyleDimension.FromPixels(22f),
				VAlign = 0f,
				Color = new Color(131, 135, 183, 255)
			};
			uislicedImage.Append(element);
			UIText uitext2 = new UIText(Language.GetText("UI.WorldDescriptionDefault"), 0.7f, false)
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(-30f, 1f),
				Top = StyleDimension.FromPixelsAndPercent(25f, 0f),
				PaddingLeft = 20f,
				PaddingRight = 20f,
				PaddingTop = 6f,
				IsWrapped = true
			};
			uislicedImage.Append(uitext2);
			this._descriptionText = uitext2;
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x00583FBC File Offset: 0x005821BC
		private void ShowOptionDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			LocalizedText localizedText = null;
			UICharacterNameButton uicharacterNameButton = listeningElement as UICharacterNameButton;
			if (uicharacterNameButton != null)
			{
				localizedText = uicharacterNameButton.Description;
			}
			GroupOptionButton<bool> groupOptionButton = listeningElement as GroupOptionButton<bool>;
			if (groupOptionButton != null)
			{
				localizedText = groupOptionButton.Description;
			}
			if (localizedText != null)
			{
				this.ShowOptionDescription(localizedText, Language.Exists(localizedText.Key + "_Title") ? Language.GetText(localizedText.Key + "_Title") : null);
			}
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x00584028 File Offset: 0x00582228
		private void ShowSecretSeedDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			DynamicSpriteFont value = FontAssets.MouseText.Value;
			string joinedSecretSeedString = this._creationState.GetJoinedSecretSeedString(value, this._descriptionText.GetInnerDimensions().Width / 0.7f, this._descriptionText.GetInnerDimensions().Height / 0.7f);
			this._descriptionText.SetText(joinedSecretSeedString);
			this._titleText.SetText(Language.GetText("UI.WorldDescriptionSecretSeeds_Title"));
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x0058409A File Offset: 0x0058229A
		private void ShowOptionDescription(LocalizedText description, LocalizedText title)
		{
			this._descriptionText.SetText(description);
			if (title != null)
			{
				this._titleText.SetText(title);
			}
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x005840B7 File Offset: 0x005822B7
		private void ClearOptionDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			this.ShowOptionDescription(Language.GetText("UI.WorldDescriptionDefault"), Language.GetText("UI.WorldDescriptionDefault_Title"));
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x005840D4 File Offset: 0x005822D4
		private void AddSeedButtons(UIElement infoContainer)
		{
			float num = 0f;
			float num2 = 44f;
			float num3 = num + num2;
			float pixels = num2;
			float pixels2 = 0f;
			GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(true, null, Language.GetText("UI.WorldCreationRandomizeSeedDescription"), Color.White, null, 1f, 0.5f, 10f)
			{
				Width = StyleDimension.FromPixelsAndPercent(40f, 0f),
				Height = new StyleDimension(40f, 0f),
				HAlign = 0f,
				Top = StyleDimension.FromPixelsAndPercent(pixels2, 0f),
				ShowHighlightWhenSelected = false
			};
			UIImage element = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/IconEvilRandom", 1))
			{
				IgnoresMouseInteraction = true,
				HAlign = 0.5f,
				VAlign = 0.5f
			};
			groupOptionButton.Append(element);
			groupOptionButton.OnLeftMouseDown += this.ClickRandomizeSeed;
			groupOptionButton.OnMouseOver += this.ShowOptionDescription;
			groupOptionButton.OnMouseOut += this.ClearOptionDescription;
			groupOptionButton.SetSnapPoint("RandomizeSeed", 0, null, null);
			infoContainer.Append(groupOptionButton);
			this._randomButton = groupOptionButton;
			UICharacterNameButton uicharacterNameButton = new UICharacterNameButton(Language.GetText("UI.WorldCreationSeed"), Language.GetText("UI.WorldCreationSeedEmpty"), Language.GetText("UI.WorldDescriptionSeed"))
			{
				Width = StyleDimension.FromPixelsAndPercent(-num3, 1f),
				HAlign = 0f,
				Left = new StyleDimension(pixels, 0f),
				Top = StyleDimension.FromPixelsAndPercent(pixels2, 0f),
				DistanceFromTitleToOption = 29f
			};
			uicharacterNameButton.OnLeftMouseDown += this.Click_SetSeed;
			uicharacterNameButton.OnMouseOver += this.ShowOptionDescription;
			uicharacterNameButton.OnMouseOut += this.ClearOptionDescription;
			uicharacterNameButton.SetSnapPoint("Seed", 0, null, null);
			infoContainer.Append(uicharacterNameButton);
			this._seedPlate = uicharacterNameButton;
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x005842EC File Offset: 0x005824EC
		private void ClickRandomizeSeed(UIMouseEvent evt, UIElement listeningElement)
		{
			this._creationState.RandomizeSeed();
			this.UpdateContents();
		}

		// Token: 0x06002A67 RID: 10855 RVA: 0x005842FF File Offset: 0x005824FF
		private void Click_SetSeed(UIMouseEvent evt, UIElement listeningElement)
		{
			this._creationState.OpenSeedInputMenu();
		}

		// Token: 0x06002A68 RID: 10856 RVA: 0x0058430C File Offset: 0x0058250C
		private void MakeBackAndCreatebuttons(UIElement outerContainer)
		{
			UITextPanel<LocalizedText> uitextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Apply"), 0.65f, true)
			{
				Width = StyleDimension.FromPixelsAndPercent(-10f, 0.5f),
				Height = StyleDimension.FromPixels(50f),
				VAlign = 1f,
				HAlign = 0.5f,
				Top = StyleDimension.FromPixels(-43f)
			};
			uitextPanel.OnMouseOver += this.FadedMouseOver;
			uitextPanel.OnMouseOut += this.FadedMouseOut;
			uitextPanel.OnLeftMouseDown += this.Click_GoBack;
			uitextPanel.SetSnapPoint("Back", 0, null, null);
			outerContainer.Append(uitextPanel);
			this._backButton = uitextPanel;
		}

		// Token: 0x06002A69 RID: 10857 RVA: 0x005843DC File Offset: 0x005825DC
		private void Click_GoBack(UIMouseEvent evt, UIElement listeningElement)
		{
			this.GoBack();
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x005843E4 File Offset: 0x005825E4
		private void GoBack()
		{
			this._creationState.ResetSpecialSeedRing();
			this._creationState.SetGoBackTarget(this._creationState);
			SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
			Main.MenuUI.SetState(this._creationState);
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x00584434 File Offset: 0x00582634
		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002A6C RID: 10860 RVA: 0x00584489 File Offset: 0x00582689
		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x005844C8 File Offset: 0x005826C8
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			this.SetupGamepadPoints(spriteBatch);
			this._creationState.DrawSeedSystems(spriteBatch);
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x005844E4 File Offset: 0x005826E4
		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 7;
			int num = 3000;
			int idRangeEndExclusive = num;
			this.GetSnapPoints();
			UILinkPoint linkPoint = this._helper.GetLinkPoint(idRangeEndExclusive++, this._backButton);
			UILinkPoint linkPoint2 = this._helper.GetLinkPoint(idRangeEndExclusive++, this._seedPlate);
			UILinkPoint linkPoint3 = this._helper.GetLinkPoint(idRangeEndExclusive++, this._randomButton);
			List<SnapPoint> snapPoints = this._optionList.GetSnapPoints();
			UILinkPoint[,] array = this._helper.CreateUILinkPointGrid(ref idRangeEndExclusive, snapPoints, 6, linkPoint2, null, null, linkPoint);
			this._helper.PairLeftRight(linkPoint3, linkPoint2);
			UILinkPoint downSide = array[0, 0];
			this._helper.PairUpDown(linkPoint3, downSide);
			this._helper.PairUpDown(linkPoint2, downSide);
			UILinkPoint upSide = array[0, array.GetLength(1) - 1];
			this._helper.PairUpDown(upSide, linkPoint);
			this._helper.MoveToVisuallyClosestPoint(num, idRangeEndExclusive);
		}

		// Token: 0x06002A6F RID: 10863 RVA: 0x005845D4 File Offset: 0x005827D4
		public GroupOptionButton<bool> GetSecretSeedButton()
		{
			return this._secretSeedButton;
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x005843DC File Offset: 0x005825DC
		public void HandleBackButtonUsage()
		{
			this.GoBack();
		}

		// Token: 0x040052EB RID: 21227
		private UIWorldCreation _creationState;

		// Token: 0x040052EC RID: 21228
		private UIText _descriptionText;

		// Token: 0x040052ED RID: 21229
		private UIText _titleText;

		// Token: 0x040052EE RID: 21230
		private UICharacterNameButton _seedPlate;

		// Token: 0x040052EF RID: 21231
		private UIElement _backButton;

		// Token: 0x040052F0 RID: 21232
		private UIElement _optionList;

		// Token: 0x040052F1 RID: 21233
		private UIElement _randomButton;

		// Token: 0x040052F2 RID: 21234
		private GroupOptionButton<AWorldGenerationOption>[] _seedButtons;

		// Token: 0x040052F3 RID: 21235
		private UIElement _seedButtonRegion;

		// Token: 0x040052F4 RID: 21236
		private GroupOptionButton<bool> _secretSeedButton;

		// Token: 0x040052F5 RID: 21237
		private bool _allowScrolling;

		// Token: 0x040052F6 RID: 21238
		private UIGamepadHelper _helper;

		// Token: 0x020008F3 RID: 2291
		private struct WorldSpecialSeedOption
		{
			// Token: 0x040073BF RID: 29631
			public AWorldGenerationOption Seed;

			// Token: 0x040073C0 RID: 29632
			public UIElement Element;

			// Token: 0x040073C1 RID: 29633
			public LocalizedText Description;

			// Token: 0x040073C2 RID: 29634
			public LocalizedText Title;
		}
	}
}
