using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Social;
using Terraria.Testing;
using Terraria.UI;
using Terraria.UI.Gamepad;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003AD RID: 941
	public class UIWorldCreation : UIState, IHaveBackButtonCommand
	{
		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06002B70 RID: 11120 RVA: 0x0058DA09 File Offset: 0x0058BC09
		// (set) Token: 0x06002B71 RID: 11121 RVA: 0x0058DA10 File Offset: 0x0058BC10
		private UIWorldCreation.WorldSizeId _optionSize
		{
			get
			{
				return (UIWorldCreation.WorldSizeId)WorldGen.GetWorldSize();
			}
			set
			{
				WorldGen.SetWorldSize((int)value);
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06002B72 RID: 11122 RVA: 0x0058DA18 File Offset: 0x0058BC18
		// (set) Token: 0x06002B73 RID: 11123 RVA: 0x0058DA1F File Offset: 0x0058BC1F
		private UIWorldCreation.WorldDifficultyId _optionDifficulty
		{
			get
			{
				return (UIWorldCreation.WorldDifficultyId)Main.GameMode;
			}
			set
			{
				Main.GameMode = (int)value;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06002B74 RID: 11124 RVA: 0x0058DA27 File Offset: 0x0058BC27
		// (set) Token: 0x06002B75 RID: 11125 RVA: 0x0058DA30 File Offset: 0x0058BC30
		private UIWorldCreation.WorldEvilId _optionEvil
		{
			get
			{
				return WorldGen.WorldGenParam_Evil + UIWorldCreation.WorldEvilId.Corruption;
			}
			set
			{
				WorldGen.WorldGenParam_Evil = value - UIWorldCreation.WorldEvilId.Corruption;
			}
		}

		// Token: 0x06002B76 RID: 11126 RVA: 0x0058DA3C File Offset: 0x0058BC3C
		public UIWorldCreation()
		{
			this._goBackTarget = this;
			this.BuildPage();
			this.SeedDust.Clear();
			this.SeedParticleSystem.Clear();
			this.ResetSpecialSeedRing();
		}

		// Token: 0x06002B77 RID: 11127 RVA: 0x0058DAEA File Offset: 0x0058BCEA
		public void SetGoBackTarget(UIState state)
		{
			this._goBackTarget = state;
		}

		// Token: 0x06002B78 RID: 11128 RVA: 0x0058DAF4 File Offset: 0x0058BCF4
		private void BuildPage()
		{
			int num = 18;
			base.RemoveAllChildren();
			UIElement uielement = new UIElement
			{
				Width = StyleDimension.FromPixels(500f),
				Height = StyleDimension.FromPixels(434f + (float)num),
				Top = StyleDimension.FromPixels(170f - (float)num),
				HAlign = 0.5f,
				VAlign = 0f
			};
			uielement.SetPadding(0f);
			base.Append(uielement);
			UIPanel uipanel = new UIPanel
			{
				Width = StyleDimension.FromPercent(1f),
				Height = StyleDimension.FromPixels((float)(280 + num)),
				Top = StyleDimension.FromPixels(50f),
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

		// Token: 0x06002B79 RID: 11129 RVA: 0x0058DC5A File Offset: 0x0058BE5A
		private void PreparePreviouslyUnlockedSecretSeeds()
		{
			SecretSeedsTracker.PrepareInterface();
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x0058DC64 File Offset: 0x0058BE64
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
			float num = 0f;
			float num2 = 88f;
			float num3 = 44f;
			float num4 = num2 + num3;
			float pixels = num3;
			GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(true, null, Language.GetText("UI.WorldCreationRandomizeNameDescription"), Color.White, "Images/UI/WorldCreation/IconRandomName", 1f, 0.5f, 10f)
			{
				Width = StyleDimension.FromPixelsAndPercent(40f, 0f),
				Height = new StyleDimension(40f, 0f),
				HAlign = 0f,
				Top = StyleDimension.FromPixelsAndPercent(num, 0f),
				ShowHighlightWhenSelected = false
			};
			groupOptionButton.OnLeftMouseDown += this.ClickRandomizeName;
			groupOptionButton.OnMouseOver += this.ShowOptionDescription;
			groupOptionButton.OnMouseOut += this.ClearOptionDescription;
			groupOptionButton.SetSnapPoint("RandomizeName", 0, null, null);
			uielement.Append(groupOptionButton);
			UICharacterNameButton uicharacterNameButton = new UICharacterNameButton(Language.GetText("UI.WorldCreationName"), Language.GetText("UI.WorldCreationNameEmpty"), Language.GetText("UI.WorldDescriptionName"))
			{
				Width = StyleDimension.FromPixelsAndPercent(-num4, 1f),
				HAlign = 0f,
				Left = new StyleDimension(pixels, 0f),
				Top = StyleDimension.FromPixelsAndPercent(num, 0f)
			};
			uicharacterNameButton.OnLeftMouseDown += this.Click_SetName;
			uicharacterNameButton.OnMouseOver += this.ShowOptionDescription;
			uicharacterNameButton.OnMouseOut += this.ClearOptionDescription;
			uicharacterNameButton.SetSnapPoint("Name", 0, null, null);
			uielement.Append(uicharacterNameButton);
			this._namePlate = uicharacterNameButton;
			CalculatedStyle dimensions = uicharacterNameButton.GetDimensions();
			num += dimensions.Height + 4f;
			this._advancedSeedButton = new GroupOptionButton<bool>(true, null, Language.GetText("UI.WorldCreationSeedMenuDescription"), Color.White, "Images/UI/WorldCreation/IconRandomSeed", 1f, 0.5f, 10f)
			{
				Width = StyleDimension.FromPixelsAndPercent(40f, 0f),
				Height = new StyleDimension(40f, 0f),
				HAlign = 0f,
				Top = StyleDimension.FromPixelsAndPercent(num, 0f),
				ShowHighlightWhenSelected = false
			};
			this._advancedSeedButton.OnLeftMouseDown += this.ClickAdvancedSeedMenu;
			this._advancedSeedButton.OnMouseOver += this.ShowOptionDescription;
			this._advancedSeedButton.OnMouseOut += this.ClearOptionDescription;
			this._advancedSeedButton.SetSnapPoint("RandomizeSeed", 0, null, null);
			this._advancedSeedButton.OnDraw += this.DrawSpecialSeedRingCallback;
			uielement.Append(this._advancedSeedButton);
			UICharacterNameButton uicharacterNameButton2 = new UICharacterNameButton(Language.GetText("UI.WorldCreationSeed"), Language.GetText("UI.WorldCreationSeedEmpty"), Language.GetText("UI.WorldDescriptionSeed"))
			{
				Width = StyleDimension.FromPixelsAndPercent(-num4, 1f),
				HAlign = 0f,
				Left = new StyleDimension(pixels, 0f),
				Top = StyleDimension.FromPixelsAndPercent(num, 0f),
				DistanceFromTitleToOption = 29f
			};
			uicharacterNameButton2.OnLeftMouseDown += this.Click_SetSeed;
			uicharacterNameButton2.OnMouseOver += this.ShowOptionDescription;
			uicharacterNameButton2.OnMouseOut += this.ClearOptionDescription;
			uicharacterNameButton2.SetSnapPoint("Seed", 0, null, null);
			uielement.Append(uicharacterNameButton2);
			this._seedPlate = uicharacterNameButton2;
			UIWorldCreationPreview uiworldCreationPreview = new UIWorldCreationPreview
			{
				Width = StyleDimension.FromPixels(84f),
				Height = StyleDimension.FromPixels(84f),
				HAlign = 1f,
				VAlign = 0f
			};
			uielement.Append(uiworldCreationPreview);
			this._previewPlate = uiworldCreationPreview;
			dimensions = uicharacterNameButton2.GetDimensions();
			num += dimensions.Height + 10f;
			UIWorldCreation.AddHorizontalSeparator(uielement, num + 2f);
			float usableWidthPercent = 1f;
			this.AddWorldSizeOptions(uielement, num, new UIElement.MouseEvent(this.ClickSizeOption), "size", usableWidthPercent);
			num += 48f;
			UIWorldCreation.AddHorizontalSeparator(uielement, num);
			this.AddWorldDifficultyOptions(uielement, num, new UIElement.MouseEvent(this.ClickDifficultyOption), "difficulty", usableWidthPercent);
			num += 48f;
			UIWorldCreation.AddHorizontalSeparator(uielement, num);
			this.AddWorldEvilOptions(uielement, num, new UIElement.MouseEvent(this.ClickEvilOption), "evil", usableWidthPercent);
			num += 48f;
			UIWorldCreation.AddHorizontalSeparator(uielement, num);
			this.AddDescriptionPanel(uielement, num, "desc");
			this.SetDefaultOptions();
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x0058E1A8 File Offset: 0x0058C3A8
		private static void AddHorizontalSeparator(UIElement Container, float accumualtedHeight)
		{
			UIHorizontalSeparator element = new UIHorizontalSeparator(2, true)
			{
				Width = StyleDimension.FromPercent(1f),
				Top = StyleDimension.FromPixels(accumualtedHeight - 8f),
				Color = Color.Lerp(Color.White, new Color(63, 65, 151, 255), 0.85f) * 0.9f
			};
			Container.Append(element);
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x0058E218 File Offset: 0x0058C418
		private void SetDefaultOptions()
		{
			Main.ActiveWorldFileData = new WorldFileData();
			this.AssignRandomWorldName();
			this.ClearSeed();
			this._optionSize = UIWorldCreation.WorldSizeId.Medium;
			if (Main.ActivePlayerFileData.Player.difficulty == 3)
			{
				this._optionDifficulty = UIWorldCreation.WorldDifficultyId.Creative;
			}
			this._optionEvil = UIWorldCreation.WorldEvilId.Random;
			this.UpdateSliders();
			this.UpdatePreviewPlate();
		}

		// Token: 0x06002B7D RID: 11133 RVA: 0x0058E270 File Offset: 0x0058C470
		private void AddDescriptionPanel(UIElement container, float accumulatedHeight, string tagGroup)
		{
			float num = 0f;
			UISlicedImage uislicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", 1))
			{
				HAlign = 0.5f,
				VAlign = 1f,
				Width = StyleDimension.FromPixelsAndPercent(-num * 2f, 1f),
				Left = StyleDimension.FromPixels(-num),
				Height = StyleDimension.FromPixelsAndPercent(40f, 0f),
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
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Top = StyleDimension.FromPixelsAndPercent(5f, 0f)
			};
			uitext.PaddingLeft = 20f;
			uitext.PaddingRight = 20f;
			uitext.PaddingTop = 6f;
			uislicedImage.Append(uitext);
			this._descriptionText = uitext;
		}

		// Token: 0x06002B7E RID: 11134 RVA: 0x0058E3B8 File Offset: 0x0058C5B8
		private void AddWorldSizeOptions(UIElement container, float accumualtedHeight, UIElement.MouseEvent clickEvent, string tagGroup, float usableWidthPercent)
		{
			UIWorldCreation.WorldSizeId[] array = new UIWorldCreation.WorldSizeId[]
			{
				UIWorldCreation.WorldSizeId.Small,
				UIWorldCreation.WorldSizeId.Medium,
				UIWorldCreation.WorldSizeId.Large
			};
			LocalizedText[] array2 = new LocalizedText[]
			{
				Lang.menu[92],
				Lang.menu[93],
				Lang.menu[94]
			};
			LocalizedText[] array3 = new LocalizedText[]
			{
				Language.GetText("UI.WorldDescriptionSizeSmall"),
				Language.GetText("UI.WorldDescriptionSizeMedium"),
				Language.GetText("UI.WorldDescriptionSizeLarge")
			};
			Color[] array4 = new Color[]
			{
				Color.Cyan,
				Color.Lerp(Color.Cyan, Color.LimeGreen, 0.5f),
				Color.LimeGreen
			};
			string[] array5 = new string[]
			{
				"Images/UI/WorldCreation/IconSizeSmall",
				"Images/UI/WorldCreation/IconSizeMedium",
				"Images/UI/WorldCreation/IconSizeLarge"
			};
			GroupOptionButton<UIWorldCreation.WorldSizeId>[] array6 = new GroupOptionButton<UIWorldCreation.WorldSizeId>[array.Length];
			for (int i = 0; i < array6.Length; i++)
			{
				GroupOptionButton<UIWorldCreation.WorldSizeId> groupOptionButton = new GroupOptionButton<UIWorldCreation.WorldSizeId>(array[i], array2[i], array3[i], array4[i], array5[i], 1f, 1f, 16f);
				groupOptionButton.Width = StyleDimension.FromPixelsAndPercent((float)(-4 * (array6.Length - 1)), 1f / (float)array6.Length * usableWidthPercent);
				groupOptionButton.Left = StyleDimension.FromPercent(1f - usableWidthPercent);
				groupOptionButton.HAlign = (float)i / (float)(array6.Length - 1);
				groupOptionButton.Top.Set(accumualtedHeight, 0f);
				groupOptionButton.OnLeftMouseDown += clickEvent;
				groupOptionButton.OnMouseOver += this.ShowOptionDescription;
				groupOptionButton.OnMouseOut += this.ClearOptionDescription;
				groupOptionButton.SetSnapPoint(tagGroup, i, null, null);
				container.Append(groupOptionButton);
				array6[i] = groupOptionButton;
			}
			this._sizeButtons = array6;
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x0058E598 File Offset: 0x0058C798
		private void AddWorldDifficultyOptions(UIElement container, float accumualtedHeight, UIElement.MouseEvent clickEvent, string tagGroup, float usableWidthPercent)
		{
			UIWorldCreation.WorldDifficultyId[] array = new UIWorldCreation.WorldDifficultyId[]
			{
				UIWorldCreation.WorldDifficultyId.Creative,
				UIWorldCreation.WorldDifficultyId.Normal,
				UIWorldCreation.WorldDifficultyId.Expert,
				UIWorldCreation.WorldDifficultyId.Master
			};
			LocalizedText[] array2 = new LocalizedText[]
			{
				Language.GetText("UI.Creative"),
				Language.GetText("UI.Normal"),
				Language.GetText("UI.Expert"),
				Language.GetText("UI.Master")
			};
			LocalizedText[] array3 = new LocalizedText[]
			{
				Language.GetText("UI.WorldDescriptionCreative"),
				Language.GetText("UI.WorldDescriptionNormal"),
				Language.GetText("UI.WorldDescriptionExpert"),
				Language.GetText("UI.WorldDescriptionMaster")
			};
			Color[] array4 = new Color[]
			{
				Main.creativeModeColor,
				Color.White,
				Main.mcColor,
				Main.hcColor
			};
			string[] array5 = new string[]
			{
				"Images/UI/WorldCreation/IconDifficultyCreative",
				"Images/UI/WorldCreation/IconDifficultyNormal",
				"Images/UI/WorldCreation/IconDifficultyExpert",
				"Images/UI/WorldCreation/IconDifficultyMaster"
			};
			GroupOptionButton<UIWorldCreation.WorldDifficultyId>[] array6 = new GroupOptionButton<UIWorldCreation.WorldDifficultyId>[array.Length];
			for (int i = 0; i < array6.Length; i++)
			{
				GroupOptionButton<UIWorldCreation.WorldDifficultyId> groupOptionButton = new GroupOptionButton<UIWorldCreation.WorldDifficultyId>(array[i], array2[i], array3[i], array4[i], array5[i], 1f, 1f, 16f);
				groupOptionButton.Width = StyleDimension.FromPixelsAndPercent((float)(-1 * (array6.Length - 1)), 1f / (float)array6.Length * usableWidthPercent);
				groupOptionButton.Left = StyleDimension.FromPercent(1f - usableWidthPercent);
				groupOptionButton.HAlign = (float)i / (float)(array6.Length - 1);
				groupOptionButton.Top.Set(accumualtedHeight, 0f);
				groupOptionButton.OnLeftMouseDown += clickEvent;
				groupOptionButton.OnMouseOver += this.ShowOptionDescription;
				groupOptionButton.OnMouseOut += this.ClearOptionDescription;
				groupOptionButton.SetSnapPoint(tagGroup, i, null, null);
				container.Append(groupOptionButton);
				array6[i] = groupOptionButton;
			}
			this._difficultyButtons = array6;
		}

		// Token: 0x06002B80 RID: 11136 RVA: 0x0058E7A0 File Offset: 0x0058C9A0
		private void AddWorldEvilOptions(UIElement container, float accumualtedHeight, UIElement.MouseEvent clickEvent, string tagGroup, float usableWidthPercent)
		{
			UIWorldCreation.WorldEvilId[] array = new UIWorldCreation.WorldEvilId[]
			{
				UIWorldCreation.WorldEvilId.Random,
				UIWorldCreation.WorldEvilId.Corruption,
				UIWorldCreation.WorldEvilId.Crimson
			};
			LocalizedText[] array2 = new LocalizedText[]
			{
				Lang.misc[103],
				Lang.misc[101],
				Lang.misc[102]
			};
			LocalizedText[] array3 = new LocalizedText[]
			{
				Language.GetText("UI.WorldDescriptionEvilRandom"),
				Language.GetText("UI.WorldDescriptionEvilCorrupt"),
				Language.GetText("UI.WorldDescriptionEvilCrimson")
			};
			Color[] array4 = new Color[]
			{
				Color.White,
				Color.MediumPurple,
				Color.IndianRed
			};
			string[] array5 = new string[]
			{
				"Images/UI/WorldCreation/IconEvilRandom",
				"Images/UI/WorldCreation/IconEvilCorruption",
				"Images/UI/WorldCreation/IconEvilCrimson"
			};
			GroupOptionButton<UIWorldCreation.WorldEvilId>[] array6 = new GroupOptionButton<UIWorldCreation.WorldEvilId>[array.Length];
			for (int i = 0; i < array6.Length; i++)
			{
				GroupOptionButton<UIWorldCreation.WorldEvilId> groupOptionButton = new GroupOptionButton<UIWorldCreation.WorldEvilId>(array[i], array2[i], array3[i], array4[i], array5[i], 1f, 1f, 16f);
				groupOptionButton.Width = StyleDimension.FromPixelsAndPercent((float)(-4 * (array6.Length - 1)), 1f / (float)array6.Length * usableWidthPercent);
				groupOptionButton.Left = StyleDimension.FromPercent(1f - usableWidthPercent);
				groupOptionButton.HAlign = (float)i / (float)(array6.Length - 1);
				groupOptionButton.Top.Set(accumualtedHeight, 0f);
				groupOptionButton.OnLeftMouseDown += clickEvent;
				groupOptionButton.OnMouseOver += this.ShowOptionDescription;
				groupOptionButton.OnMouseOut += this.ClearOptionDescription;
				groupOptionButton.SetSnapPoint(tagGroup, i, null, null);
				container.Append(groupOptionButton);
				array6[i] = groupOptionButton;
			}
			this._evilButtons = array6;
		}

		// Token: 0x06002B81 RID: 11137 RVA: 0x0058E96F File Offset: 0x0058CB6F
		private void ClickRandomizeName(UIMouseEvent evt, UIElement listeningElement)
		{
			this.AssignRandomWorldName();
			this.UpdateInputFields();
			this.UpdateSliders();
			this.UpdatePreviewPlate();
		}

		// Token: 0x06002B82 RID: 11138 RVA: 0x0058E98C File Offset: 0x0058CB8C
		private void ClickAdvancedSeedMenu(UIMouseEvent evt, UIElement listeningElement)
		{
			this.ResetSpecialSeedRing();
			UIWorldCreationAdvanced uiworldCreationAdvanced = new UIWorldCreationAdvanced(this, false);
			this.SetGoBackTarget(uiworldCreationAdvanced);
			Main.MenuUI.SetState(uiworldCreationAdvanced);
		}

		// Token: 0x06002B83 RID: 11139 RVA: 0x0058E9B9 File Offset: 0x0058CBB9
		public void ClearSeedText()
		{
			this._optionSeed = "";
			this._isSpecialSeedText = false;
			this.UpdateInputFields();
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x0058E9D3 File Offset: 0x0058CBD3
		public void ClearSeed()
		{
			this._optionSeed = string.Empty;
			this._isSpecialSeedText = false;
			this._secretSeedTextsEntered.Clear();
			this._disabledSecretSeedTextsEntered.Clear();
			WorldGenerationOptions.Reset();
			WorldGen.SecretSeed.ClearAllSeeds();
			this.PreparePreviouslyUnlockedSecretSeeds();
			this.UpdateInputFields();
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x0058EA14 File Offset: 0x0058CC14
		public void RandomizeSeed()
		{
			this._optionSeed = Main.rand.Next().ToString();
			this._isSpecialSeedText = false;
			this.UpdateInputFields();
			this.UpdateSliders();
			this.UpdatePreviewPlate();
		}

		// Token: 0x06002B86 RID: 11142 RVA: 0x0058EA54 File Offset: 0x0058CC54
		private void ClickSizeOption(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<UIWorldCreation.WorldSizeId> groupOptionButton = (GroupOptionButton<UIWorldCreation.WorldSizeId>)listeningElement;
			this._optionSize = groupOptionButton.OptionValue;
			GroupOptionButton<UIWorldCreation.WorldSizeId>[] sizeButtons = this._sizeButtons;
			for (int i = 0; i < sizeButtons.Length; i++)
			{
				sizeButtons[i].SetCurrentOption(groupOptionButton.OptionValue);
			}
			this.UpdatePreviewPlate();
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x0058EAA0 File Offset: 0x0058CCA0
		private void ClickDifficultyOption(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<UIWorldCreation.WorldDifficultyId> groupOptionButton = (GroupOptionButton<UIWorldCreation.WorldDifficultyId>)listeningElement;
			this._optionDifficulty = groupOptionButton.OptionValue;
			GroupOptionButton<UIWorldCreation.WorldDifficultyId>[] difficultyButtons = this._difficultyButtons;
			for (int i = 0; i < difficultyButtons.Length; i++)
			{
				difficultyButtons[i].SetCurrentOption(groupOptionButton.OptionValue);
			}
			this.UpdatePreviewPlate();
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x0058EAEC File Offset: 0x0058CCEC
		private void ClickEvilOption(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<UIWorldCreation.WorldEvilId> groupOptionButton = (GroupOptionButton<UIWorldCreation.WorldEvilId>)listeningElement;
			this._optionEvil = groupOptionButton.OptionValue;
			GroupOptionButton<UIWorldCreation.WorldEvilId>[] evilButtons = this._evilButtons;
			for (int i = 0; i < evilButtons.Length; i++)
			{
				evilButtons[i].SetCurrentOption(groupOptionButton.OptionValue);
			}
			this.UpdatePreviewPlate();
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x0058EB35 File Offset: 0x0058CD35
		private void UpdatePreviewPlate()
		{
			this._previewPlate.UpdateOption((byte)this._optionDifficulty, (byte)this._optionEvil, (byte)this._optionSize);
		}

		// Token: 0x06002B8A RID: 11146 RVA: 0x0058EB58 File Offset: 0x0058CD58
		private void UpdateSliders()
		{
			GroupOptionButton<UIWorldCreation.WorldSizeId>[] sizeButtons = this._sizeButtons;
			for (int i = 0; i < sizeButtons.Length; i++)
			{
				sizeButtons[i].SetCurrentOption(this._optionSize);
			}
			GroupOptionButton<UIWorldCreation.WorldDifficultyId>[] difficultyButtons = this._difficultyButtons;
			for (int i = 0; i < difficultyButtons.Length; i++)
			{
				difficultyButtons[i].SetCurrentOption(this._optionDifficulty);
			}
			GroupOptionButton<UIWorldCreation.WorldEvilId>[] evilButtons = this._evilButtons;
			for (int i = 0; i < evilButtons.Length; i++)
			{
				evilButtons[i].SetCurrentOption(this._optionEvil);
			}
		}

		// Token: 0x06002B8B RID: 11147 RVA: 0x0058EBD0 File Offset: 0x0058CDD0
		public void ShowOptionDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			LocalizedText localizedText = null;
			GroupOptionButton<UIWorldCreation.WorldSizeId> groupOptionButton = listeningElement as GroupOptionButton<UIWorldCreation.WorldSizeId>;
			if (groupOptionButton != null)
			{
				localizedText = groupOptionButton.Description;
			}
			GroupOptionButton<UIWorldCreation.WorldDifficultyId> groupOptionButton2 = listeningElement as GroupOptionButton<UIWorldCreation.WorldDifficultyId>;
			if (groupOptionButton2 != null)
			{
				localizedText = groupOptionButton2.Description;
			}
			GroupOptionButton<UIWorldCreation.WorldEvilId> groupOptionButton3 = listeningElement as GroupOptionButton<UIWorldCreation.WorldEvilId>;
			if (groupOptionButton3 != null)
			{
				localizedText = groupOptionButton3.Description;
			}
			UICharacterNameButton uicharacterNameButton = listeningElement as UICharacterNameButton;
			if (uicharacterNameButton != null)
			{
				localizedText = uicharacterNameButton.Description;
			}
			GroupOptionButton<bool> groupOptionButton4 = listeningElement as GroupOptionButton<bool>;
			if (groupOptionButton4 != null)
			{
				localizedText = groupOptionButton4.Description;
			}
			if (localizedText != null)
			{
				this._descriptionText.SetText(localizedText);
			}
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x0058EC49 File Offset: 0x0058CE49
		public void ClearOptionDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			this._descriptionText.SetText(Language.GetText("UI.WorldDescriptionDefault"));
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x0058EC60 File Offset: 0x0058CE60
		private void MakeBackAndCreatebuttons(UIElement outerContainer)
		{
			UITextPanel<LocalizedText> uitextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, true)
			{
				Width = StyleDimension.FromPixelsAndPercent(-10f, 0.5f),
				Height = StyleDimension.FromPixels(50f),
				VAlign = 1f,
				HAlign = 0f,
				Top = StyleDimension.FromPixels(-45f)
			};
			uitextPanel.OnMouseOver += this.FadedMouseOver;
			uitextPanel.OnMouseOut += this.FadedMouseOut;
			uitextPanel.OnLeftMouseDown += this.Click_GoBack;
			uitextPanel.SetSnapPoint("Back", 0, null, null);
			outerContainer.Append(uitextPanel);
			UITextPanel<LocalizedText> uitextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Create"), 0.7f, true)
			{
				Width = StyleDimension.FromPixelsAndPercent(-10f, 0.5f),
				Height = StyleDimension.FromPixels(50f),
				VAlign = 1f,
				HAlign = 1f,
				Top = StyleDimension.FromPixels(-45f)
			};
			uitextPanel2.OnMouseOver += this.FadedMouseOver;
			uitextPanel2.OnMouseOut += this.FadedMouseOut;
			uitextPanel2.OnLeftMouseDown += this.Click_NamingAndCreating;
			uitextPanel2.SetSnapPoint("Create", 0, null, null);
			outerContainer.Append(uitextPanel2);
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x0058EDE5 File Offset: 0x0058CFE5
		private void Click_GoBack(UIMouseEvent evt, UIElement listeningElement)
		{
			UIWorldCreation.GoBack();
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x0058EDEC File Offset: 0x0058CFEC
		private static void GoBack()
		{
			SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
			Main.OpenWorldSelectUI();
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x0058EE08 File Offset: 0x0058D008
		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x00584489 File Offset: 0x00582689
		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x0058EE60 File Offset: 0x0058D060
		private void Click_SetName(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			Main.clrInput();
			UIVirtualKeyboard state = new UIVirtualKeyboard(Lang.menu[48].Value, "", new UIVirtualKeyboard.KeyboardSubmitEvent(this.OnFinishedSettingName), new Action(this.GoBackHere), 0, true, 27);
			Main.MenuUI.SetState(state);
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x0058EEC6 File Offset: 0x0058D0C6
		private void Click_SetSeed(UIMouseEvent evt, UIElement listeningElement)
		{
			this.OpenSeedInputMenu();
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x0058EED0 File Offset: 0x0058D0D0
		public void OpenSeedInputMenu()
		{
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			Main.clrInput();
			UIVirtualKeyboard state = new UIVirtualKeyboard(Language.GetTextValue("UI.EnterSeed"), "", new UIVirtualKeyboard.KeyboardSubmitEvent(this.OnFinishedSettingSeed), new Action(this.GoBackHere), 0, true, int.MaxValue);
			Main.MenuUI.SetState(state);
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x0058EF38 File Offset: 0x0058D138
		private void Click_NamingAndCreating(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			if (string.IsNullOrEmpty(this._optionwWorldName))
			{
				this._optionwWorldName = "";
				Main.clrInput();
				UIVirtualKeyboard state = new UIVirtualKeyboard(Lang.menu[48].Value, "", new UIVirtualKeyboard.KeyboardSubmitEvent(this.OnFinishedNamingAndCreating), new Action(this.GoBackHere), 0, false, 27);
				Main.MenuUI.SetState(state);
				return;
			}
			this.FinishCreatingWorld();
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x0058EFBD File Offset: 0x0058D1BD
		private void OnFinishedSettingName(string name)
		{
			this._optionwWorldName = name.Trim();
			this.UpdateInputFields();
			this.GoBackHere();
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x0058EFD8 File Offset: 0x0058D1D8
		private void UpdateInputFields()
		{
			this._namePlate.SetContents(this._optionwWorldName);
			this._namePlate.Recalculate();
			this._namePlate.TrimDisplayIfOverElementDimensions(27);
			this._namePlate.Recalculate();
			this.FillSeedContent(this._seedPlate);
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x0058F025 File Offset: 0x0058D225
		public void FillSeedContent(UICharacterNameButton button)
		{
			button.SetContents(this._optionSeed);
			button.Recalculate();
			button.TrimDisplayIfOverElementDimensions(WorldFileData.MAX_USER_SEED_TEXT_LENGTH);
			button.Recalculate();
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x0058F04A File Offset: 0x0058D24A
		public void ToggleSeedOption(AWorldGenerationOption seedOption)
		{
			if (this._isSpecialSeedText)
			{
				this._optionSeed = string.Empty;
				this._isSpecialSeedText = false;
				this.UpdateInputFields();
				this.UpdateSliders();
				this.UpdatePreviewPlate();
			}
			seedOption.Enabled = !seedOption.Enabled;
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06002B9A RID: 11162 RVA: 0x0058F087 File Offset: 0x0058D287
		public bool HasEnteredSpecialSeed
		{
			get
			{
				return this._secretSeedTextsEntered.Count > 0;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06002B9B RID: 11163 RVA: 0x0058F097 File Offset: 0x0058D297
		public bool HasDisabledSecretSeed
		{
			get
			{
				return this._disabledSecretSeedTextsEntered.Count > 0;
			}
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x0058F0A8 File Offset: 0x0058D2A8
		public void EnableSecretSeedOptions(bool enabled)
		{
			if (enabled)
			{
				for (int i = 0; i < this._disabledSecretSeedTextsEntered.Count; i++)
				{
					WorldGen.SecretSeed secretSeed;
					if (WorldGen.SecretSeed.CheckInputForSecretSeed(this._disabledSecretSeedTextsEntered[i], out secretSeed) && !secretSeed.Enabled)
					{
						this._secretSeedTextsEntered.Add(this._disabledSecretSeedTextsEntered[i]);
						WorldGen.SecretSeed.Enable(secretSeed, false);
					}
				}
				this._disabledSecretSeedTextsEntered.Clear();
				return;
			}
			this._disabledSecretSeedTextsEntered.Clear();
			this._disabledSecretSeedTextsEntered.AddRange(this._secretSeedTextsEntered);
			WorldGen.SecretSeed.ClearAllSeeds();
			this._secretSeedTextsEntered.Clear();
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x0058F144 File Offset: 0x0058D344
		public string GetJoinedSecretSeedString(DynamicSpriteFont font, float maxWidth, float maxHeight)
		{
			float num = 0f;
			string text = string.Empty;
			List<string> list = this.HasEnteredSpecialSeed ? this._secretSeedTextsEntered : this._disabledSecretSeedTextsEntered;
			if (list.Count == 0)
			{
				return "-";
			}
			string text2 = list[0];
			for (int i = 1; i < list.Count; i++)
			{
				string text3 = string.Format("{0}|{1}", text2, list[i]);
				if (font.MeasureString(text3).X >= maxWidth)
				{
					if (num <= maxHeight)
					{
						text = text + text2 + "\n";
					}
					num += (float)font.LineSpacing;
					text3 = list[i];
				}
				text2 = text3;
			}
			if (num <= maxHeight)
			{
				text += text2;
			}
			return text;
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x0058F1FC File Offset: 0x0058D3FC
		private void OnFinishedSettingSeed(string seed)
		{
			this._optionSeed = seed.Trim();
			string optionSeed;
			string text;
			List<string> secretSeedTextsEntered;
			if (WorldFileData.TryApplyingCopiedSeed(this._optionSeed, true, out optionSeed, out text, out secretSeedTextsEntered))
			{
				this._optionSeed = optionSeed;
				this._secretSeedTextsEntered = secretSeedTextsEntered;
				this._disabledSecretSeedTextsEntered.Clear();
			}
			else
			{
				this._optionSeed = Utils.TrimUserString(this._optionSeed, WorldFileData.MAX_USER_SEED_TEXT_LENGTH);
				AWorldGenerationOption optionFromSeedText = WorldGenerationOptions.GetOptionFromSeedText(this._optionSeed);
				this._isSpecialSeedText = (optionFromSeedText != null);
				if (this._isSpecialSeedText)
				{
					WorldGenerationOptions.SelectOption(optionFromSeedText);
					SoundEngine.PlaySound(24, -1, -1, 1, 1f, 0f);
				}
				WorldGen.SecretSeed secretSeed;
				if (WorldGen.SecretSeed.CheckInputForSecretSeed(this._optionSeed, out secretSeed))
				{
					if (!secretSeed.Enabled)
					{
						this._secretSeedTextsEntered.Add(this._optionSeed);
						WorldGen.SecretSeed.Enable(secretSeed, true);
						this.EnableSecretSeedOptions(true);
						CalculatedStyle dimensions = this._advancedSeedButton.GetDimensions();
						if (this._goBackTarget != this)
						{
							UIWorldCreationAdvanced uiworldCreationAdvanced = this._goBackTarget as UIWorldCreationAdvanced;
							if (uiworldCreationAdvanced != null)
							{
								uiworldCreationAdvanced.RefreshSecretSeedButton();
								dimensions = uiworldCreationAdvanced.GetSecretSeedButton().GetDimensions();
								uiworldCreationAdvanced.GetSecretSeedButton().SetCurrentOption(this.HasEnteredSpecialSeed);
							}
						}
						Vector2 vector = dimensions.Center();
						Vector2 value = Main.rand.NextVector2Circular(5f, 5f);
						this.Spawn_RainbowRodHit(new ParticleOrchestraSettings
						{
							PositionInWorld = vector,
							MovementVector = new Vector2(16f, 0f) + value
						});
						if (this._goBackTarget != this)
						{
							this.Spawn_RainbowRodHit(new ParticleOrchestraSettings
							{
								PositionInWorld = vector,
								MovementVector = new Vector2(16f, 0f) - value
							});
						}
						Vector2 value2 = Main.rand.NextVector2Circular(5f, 5f);
						this.Spawn_RainbowRodHit(new ParticleOrchestraSettings
						{
							PositionInWorld = vector,
							MovementVector = new Vector2(0f, 16f) + value2
						});
						if (this._goBackTarget != this)
						{
							this.Spawn_RainbowRodHit(new ParticleOrchestraSettings
							{
								PositionInWorld = vector,
								MovementVector = new Vector2(0f, 16f) - value2
							});
						}
						for (int i = 0; i < 3; i++)
						{
							this.Spawn_BestReforge(new ParticleOrchestraSettings
							{
								PositionInWorld = vector + new Vector2(dimensions.Width * 0.25f * (float)(i - 1), 0f)
							});
						}
					}
					this.ClearSeedText();
				}
			}
			this.UpdateInputFields();
			this.UpdateSliders();
			this.UpdatePreviewPlate();
			if (this.SubmitSeed != null)
			{
				this.SubmitSeed();
			}
			this.GoBackHere();
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x0058F4C0 File Offset: 0x0058D6C0
		private void Spawn_BestReforge(ParticleOrchestraSettings settings)
		{
			Vector2 accelerationPerFrame = new Vector2(0f, 0.16350001f);
			Asset<Texture2D> textureAsset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Research_Spark", 1);
			for (int i = 0; i < 8; i++)
			{
				Vector2 value = Main.rand.NextVector2Circular(3f, 4f);
				this.SeedParticleSystem.Add(new CreativeSacrificeParticle(textureAsset, null, settings.MovementVector + value, settings.PositionInWorld)
				{
					AccelerationPerFrame = accelerationPerFrame,
					ScaleOffsetPerFrame = -0.016666668f
				});
			}
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x0058F558 File Offset: 0x0058D758
		private void Spawn_RainbowRodHit(ParticleOrchestraSettings settings)
		{
			float num = Main.rand.NextFloat() * 6.2831855f;
			float num2 = 6f;
			float num3 = Main.rand.NextFloat();
			for (float num4 = 0f; num4 < 1f; num4 += 1f / num2)
			{
				Vector2 vector = settings.MovementVector * Main.rand.NextFloatDirection() * 0.15f;
				Vector2 vector2 = new Vector2(Main.rand.NextFloat() * 0.4f + 0.4f);
				float f = num + Main.rand.NextFloat() * 6.2831855f;
				float rotation = 1.5707964f;
				Vector2 vector3 = 1.5f * vector2;
				float divider = 60f;
				Vector2 value = Main.rand.NextVector2Circular(8f, 8f) * vector2;
				PrettySparkleParticle prettySparkleParticle = new PrettySparkleParticle();
				prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
				prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / divider) - vector * 1f / 60f;
				prettySparkleParticle.ColorTint = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.33f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f, byte.MaxValue);
				prettySparkleParticle.ColorTint.A = 0;
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = rotation;
				prettySparkleParticle.Scale = vector2;
				this.SeedParticleSystem.Add(prettySparkleParticle);
				prettySparkleParticle = new PrettySparkleParticle();
				prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
				prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / divider) - vector * 1f / 60f;
				prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = rotation;
				prettySparkleParticle.Scale = vector2 * 0.6f;
				this.SeedParticleSystem.Add(prettySparkleParticle);
			}
			for (int i = 0; i < 12; i++)
			{
				Color newColor = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.12f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f, byte.MaxValue);
				Dust dust = this.SeedDust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor, 1f);
				dust.velocity = Main.rand.NextVector2Circular(1f, 1f);
				dust.velocity += settings.MovementVector * Main.rand.NextFloatDirection() * 0.5f;
				dust.noGravity = true;
				dust.scale = 0.6f + Main.rand.NextFloat() * 0.9f;
				dust.fadeIn = 0.7f + Main.rand.NextFloat() * 0.8f;
				if (dust.dustIndex != 200)
				{
					Dust dust2 = this.SeedDust.CloneDust(dust);
					dust2.scale /= 2f;
					dust2.fadeIn *= 0.75f;
					dust2.color = new Color(255, 255, 255, 255);
				}
			}
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x0058F93A File Offset: 0x0058DB3A
		private void GoBackHere()
		{
			Main.MenuUI.SetState(this._goBackTarget);
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x0058F94C File Offset: 0x0058DB4C
		private void OnFinishedNamingAndCreating(string name)
		{
			this.OnFinishedSettingName(name);
			this.FinishCreatingWorld();
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x0058F95C File Offset: 0x0058DB5C
		private void FinishCreatingWorld()
		{
			string name = Main.worldName = this._optionwWorldName.Trim();
			UIWorldCreation.WorldDifficultyId optionDifficulty = this._optionDifficulty;
			Main.ActiveWorldFileData = WorldFile.CreateMetadata(name, SocialAPI.Cloud != null && SocialAPI.Cloud.EnabledByDefault, Main.GameMode);
			this._optionDifficulty = optionDifficulty;
			if (this._optionSeed.Length == 0 || this._isSpecialSeedText)
			{
				Main.ActiveWorldFileData.SetSeedToRandomWithCurrentEvents();
			}
			else
			{
				Main.ActiveWorldFileData.SetSeed(this._optionSeed);
			}
			if (this._secretSeedTextsEntered.Count > 0)
			{
				string seed = string.Join("|", this._secretSeedTextsEntered) + "|" + Main.ActiveWorldFileData.SeedText;
				Main.ActiveWorldFileData.SetSeed(seed);
			}
			WorldGenerator.Controller controller = new WorldGenerator.Controller(null)
			{
				Paused = (DebugOptions.enableDebugCommands && Main.keyState.PressingControl())
			};
			WorldGen.CreateNewWorld(null, controller, null);
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x0058FA44 File Offset: 0x0058DC44
		private void AssignRandomWorldName()
		{
			do
			{
				LocalizedText localizedText = Language.SelectRandom(Lang.CreateDialogFilter("RandomWorldName_Composition.", false), null);
				LocalizedText localizedText2 = Language.SelectRandom(Lang.CreateDialogFilter("RandomWorldName_Adjective.", true), null);
				LocalizedText localizedText3 = Language.SelectRandom(Lang.CreateDialogFilter("RandomWorldName_Location.", true), null);
				LocalizedText localizedText4 = Language.SelectRandom(Lang.CreateDialogFilter("RandomWorldName_Noun.", true), null);
				var obj = new
				{
					Adjective = localizedText2.Value,
					Location = localizedText3.Value,
					Noun = localizedText4.Value
				};
				this._optionwWorldName = localizedText.FormatWith(obj);
				if (Main.rand.Next(10000) == 0)
				{
					this._optionwWorldName = Language.GetTextValue("SpecialWorldName.TheConstant");
				}
			}
			while (this._optionwWorldName.Length > 27);
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x0058FAEF File Offset: 0x0058DCEF
		public override void Draw(SpriteBatch spriteBatch)
		{
			if (this._goBackTarget != this)
			{
				this._goBackTarget = this;
			}
			base.Draw(spriteBatch);
			this.SetupGamepadPoints(spriteBatch);
			this.DrawSeedSystems(spriteBatch);
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x0058FB16 File Offset: 0x0058DD16
		public void ResetSpecialSeedRing()
		{
			this.ringPoint = 0f;
			Array.Clear(this.oldPos, 0, this.oldPos.Length);
			Array.Clear(this.oldTangent, 0, this.oldTangent.Length);
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x0058FB4C File Offset: 0x0058DD4C
		public void DrawSpecialSeedRingCallback(UIElement element, SpriteBatch spriteBatch)
		{
			if (this.HasEnteredSpecialSeed)
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
				if (this.oldPos[0] == Vector2.Zero)
				{
					for (int i = 0; i < 61; i++)
					{
						this.UpdateSpecialSeedRing(element);
					}
				}
				else
				{
					this.specialSeedIndex = (this.specialSeedIndex + 1) % 4;
					if (this.specialSeedIndex % 4 == 0)
					{
						this.UpdateSpecialSeedRing(element);
					}
				}
				this.DrawSpecialSeedRing();
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
			}
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x0058FC08 File Offset: 0x0058DE08
		public void DrawSpecialSeedRingCallbackWithoutCondition(UIElement element, SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
			if (this.oldPos[0] == Vector2.Zero)
			{
				for (int i = 0; i < 61; i++)
				{
					this.UpdateSpecialSeedRing(element);
				}
			}
			else
			{
				this.UpdateSpecialSeedRing(element);
			}
			this.DrawSpecialSeedRing();
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x0058FCA0 File Offset: 0x0058DEA0
		private void UpdateSpecialSeedRing(UIElement element)
		{
			CalculatedStyle dimensions = this._advancedSeedButton.GetDimensions();
			if (this._goBackTarget != this)
			{
				UIWorldCreationAdvanced uiworldCreationAdvanced = this._goBackTarget as UIWorldCreationAdvanced;
				if (uiworldCreationAdvanced != null)
				{
					uiworldCreationAdvanced.RefreshSecretSeedButton();
					dimensions = uiworldCreationAdvanced.GetSecretSeedButton().GetDimensions();
				}
			}
			if (element is GroupOptionButton<WorldGen.SecretSeed>)
			{
				dimensions = element.GetDimensions();
			}
			Rectangle rectangle = dimensions.ToRectangle();
			rectangle.Inflate(-1, -1);
			int num = rectangle.Width * 2 + rectangle.Height * 2;
			float num2 = (float)num / 60f;
			this.ringPoint += num2;
			if (this.ringPoint >= (float)num)
			{
				this.ringPoint -= (float)num;
			}
			float scaleFactor = (float)Math.Sqrt((double)(rectangle.Width / 2 * rectangle.Width / 2 + rectangle.Height / 2 * rectangle.Height / 2));
			float num3 = 6.2831855f * this.ringPoint / (float)num;
			Vector2 vector = new Vector2((float)Math.Cos((double)num3), (float)Math.Sin((double)num3));
			Vector2 vector2 = vector * scaleFactor;
			float num4 = Math.Abs(vector2.X) / ((float)rectangle.Width / 2f);
			float num5 = Math.Abs(vector2.Y) / ((float)rectangle.Height / 2f);
			if (num4 > num5)
			{
				vector2 /= num4;
				vector /= num4;
			}
			else
			{
				vector2 /= num5;
				vector /= num5;
			}
			vector2 += rectangle.Center.ToVector2();
			for (int i = this.oldPos.Length - 1; i > 0; i--)
			{
				this.oldPos[i] = this.oldPos[i - 1];
				this.oldTangent[i] = this.oldTangent[i - 1];
			}
			this.oldPos[0] = vector2;
			this.oldTangent[0] = vector;
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x0058FE98 File Offset: 0x0058E098
		private void DrawSpecialSeedRing()
		{
			MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
			miscShaderData.UseSaturation(this.trial);
			miscShaderData.UseOpacity((this._goBackTarget != this) ? this.opacity2 : this.opacity);
			miscShaderData.UseSpriteTransformMatrix(new Matrix?(Main.UIScaleMatrix));
			miscShaderData.Apply(null);
			float scaleFactor = 4f;
			if (this._goBackTarget != this)
			{
				scaleFactor = 5f;
			}
			int num = this.oldPos.Length;
			UIWorldCreation._vertexStrip.Reset(num * 2);
			int num2 = num;
			int num3 = 0;
			while (num3 < num && !(this.oldPos[num3] == Vector2.Zero))
			{
				Vector2 vector = this.oldPos[num3];
				float num4 = (float)num3 / (float)(num2 - 1);
				num4 *= 0.6f;
				Color vertexColor = this.StripColors(num4);
				float num5 = this.StripWidth(num4);
				Vector2 value = this.oldTangent[num3] * scaleFactor;
				Vector3 uvA = new Vector3(num4, num5 / 2f, num5);
				Vector3 uvB = new Vector3(num4, num5 / 2f, num5);
				UIWorldCreation._vertexStrip.AddVertexPair(vector + value, vector, uvA, uvB, vertexColor);
				num3++;
			}
			UIWorldCreation._vertexStrip.PrepareIndices(true);
			UIWorldCreation._vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			miscShaderData.UseSpriteTransformMatrix(null);
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x00590030 File Offset: 0x0058E230
		private Color StripColors(float progressOnStrip)
		{
			Color result = Main.hslToRgb((progressOnStrip - Main.GlobalTimeWrappedHourly * this.animationSpeed) % 1f, this.saturation, 0.5f, byte.MaxValue);
			result.A = 0;
			return result;
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x00590070 File Offset: 0x0058E270
		private float StripWidth(float progressOnStrip)
		{
			float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, true);
			return 24f * lerpValue;
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x0059009C File Offset: 0x0058E29C
		public void DrawSeedSystems(SpriteBatch spriteBatch)
		{
			this.SeedDust.UpdateDust();
			this.SeedDust.DrawDust();
			this.SeedParticleSystem.Update();
			this.SeedParticleSystem.Draw(spriteBatch);
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x005900CC File Offset: 0x0058E2CC
		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 7;
			int num = 3000;
			List<SnapPoint> snapPoints = this.GetSnapPoints();
			SnapPoint snapPoint = null;
			SnapPoint snapPoint2 = null;
			SnapPoint snapPoint3 = null;
			SnapPoint snapPoint4 = null;
			SnapPoint snapPoint5 = null;
			SnapPoint snapPoint6 = null;
			for (int i = 0; i < snapPoints.Count; i++)
			{
				SnapPoint snapPoint7 = snapPoints[i];
				string name = snapPoint7.Name;
				if (!(name == "Back"))
				{
					if (!(name == "Create"))
					{
						if (!(name == "Name"))
						{
							if (!(name == "Seed"))
							{
								if (!(name == "RandomizeName"))
								{
									if (name == "RandomizeSeed")
									{
										snapPoint6 = snapPoint7;
									}
								}
								else
								{
									snapPoint5 = snapPoint7;
								}
							}
							else
							{
								snapPoint4 = snapPoint7;
							}
						}
						else
						{
							snapPoint3 = snapPoint7;
						}
					}
					else
					{
						snapPoint2 = snapPoint7;
					}
				}
				else
				{
					snapPoint = snapPoint7;
				}
			}
			List<SnapPoint> snapGroup = this.GetSnapGroup(snapPoints, "size");
			List<SnapPoint> snapGroup2 = this.GetSnapGroup(snapPoints, "difficulty");
			List<SnapPoint> snapGroup3 = this.GetSnapGroup(snapPoints, "evil");
			UILinkPointNavigator.SetPosition(num, snapPoint.Position);
			UILinkPoint uilinkPoint = UILinkPointNavigator.Points[num];
			uilinkPoint.Unlink();
			UILinkPoint uilinkPoint2 = uilinkPoint;
			num++;
			UILinkPointNavigator.SetPosition(num, snapPoint2.Position);
			uilinkPoint = UILinkPointNavigator.Points[num];
			uilinkPoint.Unlink();
			UILinkPoint uilinkPoint3 = uilinkPoint;
			num++;
			UILinkPointNavigator.SetPosition(num, snapPoint5.Position);
			uilinkPoint = UILinkPointNavigator.Points[num];
			uilinkPoint.Unlink();
			UILinkPoint uilinkPoint4 = uilinkPoint;
			num++;
			UILinkPointNavigator.SetPosition(num, snapPoint3.Position);
			uilinkPoint = UILinkPointNavigator.Points[num];
			uilinkPoint.Unlink();
			UILinkPoint uilinkPoint5 = uilinkPoint;
			num++;
			UILinkPointNavigator.SetPosition(num, snapPoint6.Position);
			uilinkPoint = UILinkPointNavigator.Points[num];
			uilinkPoint.Unlink();
			UILinkPoint uilinkPoint6 = uilinkPoint;
			num++;
			UILinkPointNavigator.SetPosition(num, snapPoint4.Position);
			uilinkPoint = UILinkPointNavigator.Points[num];
			uilinkPoint.Unlink();
			UILinkPoint uilinkPoint7 = uilinkPoint;
			num++;
			UILinkPoint[] array = new UILinkPoint[snapGroup.Count];
			for (int j = 0; j < snapGroup.Count; j++)
			{
				UILinkPointNavigator.SetPosition(num, snapGroup[j].Position);
				uilinkPoint = UILinkPointNavigator.Points[num];
				uilinkPoint.Unlink();
				array[j] = uilinkPoint;
				num++;
			}
			UILinkPoint[] array2 = new UILinkPoint[snapGroup2.Count];
			for (int k = 0; k < snapGroup2.Count; k++)
			{
				UILinkPointNavigator.SetPosition(num, snapGroup2[k].Position);
				uilinkPoint = UILinkPointNavigator.Points[num];
				uilinkPoint.Unlink();
				array2[k] = uilinkPoint;
				num++;
			}
			UILinkPoint[] array3 = new UILinkPoint[snapGroup3.Count];
			for (int l = 0; l < snapGroup3.Count; l++)
			{
				UILinkPointNavigator.SetPosition(num, snapGroup3[l].Position);
				uilinkPoint = UILinkPointNavigator.Points[num];
				uilinkPoint.Unlink();
				array3[l] = uilinkPoint;
				num++;
			}
			this.LoopHorizontalLineLinks(array);
			this.LoopHorizontalLineLinks(array2);
			this.EstablishUpDownRelationship(array, array2);
			for (int m = 0; m < array.Length; m++)
			{
				array[m].Up = uilinkPoint7.ID;
			}
			if (true)
			{
				this.LoopHorizontalLineLinks(array3);
				this.EstablishUpDownRelationship(array2, array3);
				for (int n = 0; n < array3.Length; n++)
				{
					array3[n].Down = uilinkPoint2.ID;
				}
				array3[array3.Length - 1].Down = uilinkPoint3.ID;
				uilinkPoint3.Up = array3[array3.Length - 1].ID;
				uilinkPoint2.Up = array3[0].ID;
			}
			else
			{
				for (int num2 = 0; num2 < array2.Length; num2++)
				{
					array2[num2].Down = uilinkPoint2.ID;
				}
				array2[array2.Length - 1].Down = uilinkPoint3.ID;
				uilinkPoint3.Up = array2[array2.Length - 1].ID;
				uilinkPoint2.Up = array2[0].ID;
			}
			uilinkPoint3.Left = uilinkPoint2.ID;
			uilinkPoint2.Right = uilinkPoint3.ID;
			uilinkPoint5.Down = uilinkPoint7.ID;
			uilinkPoint5.Left = uilinkPoint4.ID;
			uilinkPoint4.Right = uilinkPoint5.ID;
			uilinkPoint7.Up = uilinkPoint5.ID;
			uilinkPoint7.Down = array[0].ID;
			uilinkPoint7.Left = uilinkPoint6.ID;
			uilinkPoint6.Right = uilinkPoint7.ID;
			uilinkPoint6.Up = uilinkPoint4.ID;
			uilinkPoint6.Down = array[0].ID;
			uilinkPoint4.Down = uilinkPoint6.ID;
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x00590568 File Offset: 0x0058E768
		private void EstablishUpDownRelationship(UILinkPoint[] topSide, UILinkPoint[] bottomSide)
		{
			int num = Math.Max(topSide.Length, bottomSide.Length);
			for (int i = 0; i < num; i++)
			{
				int num2 = Math.Min(i, topSide.Length - 1);
				int num3 = Math.Min(i, bottomSide.Length - 1);
				topSide[num2].Down = bottomSide[num3].ID;
				bottomSide[num3].Up = topSide[num2].ID;
			}
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x005905C8 File Offset: 0x0058E7C8
		private void LoopHorizontalLineLinks(UILinkPoint[] pointsLine)
		{
			for (int i = 1; i < pointsLine.Length - 1; i++)
			{
				pointsLine[i - 1].Right = pointsLine[i].ID;
				pointsLine[i].Left = pointsLine[i - 1].ID;
				pointsLine[i].Right = pointsLine[i + 1].ID;
				pointsLine[i + 1].Left = pointsLine[i].ID;
			}
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x00590630 File Offset: 0x0058E830
		private List<SnapPoint> GetSnapGroup(List<SnapPoint> ptsOnPage, string groupName)
		{
			List<SnapPoint> list = (from a in ptsOnPage
			where a.Name == groupName
			select a).ToList<SnapPoint>();
			list.Sort(new Comparison<SnapPoint>(this.SortPoints));
			return list;
		}

		// Token: 0x06002BB2 RID: 11186 RVA: 0x00590674 File Offset: 0x0058E874
		private int SortPoints(SnapPoint a, SnapPoint b)
		{
			return a.Id.CompareTo(b.Id);
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x00590695 File Offset: 0x0058E895
		public void AddSeedFromSeedmenu(string seed)
		{
			this._secretSeedTextsEntered.Add(seed);
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x005906A3 File Offset: 0x0058E8A3
		public void RemoveSeedFromSeedMenu(string seed)
		{
			this._secretSeedTextsEntered.Remove(seed);
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x0058EDE5 File Offset: 0x0058CFE5
		public void HandleBackButtonUsage()
		{
			UIWorldCreation.GoBack();
		}

		// Token: 0x0400536A RID: 21354
		private string _optionwWorldName;

		// Token: 0x0400536B RID: 21355
		private string _optionSeed;

		// Token: 0x0400536C RID: 21356
		private bool _isSpecialSeedText;

		// Token: 0x0400536D RID: 21357
		private List<string> _secretSeedTextsEntered = new List<string>();

		// Token: 0x0400536E RID: 21358
		private List<string> _disabledSecretSeedTextsEntered = new List<string>();

		// Token: 0x0400536F RID: 21359
		private ParticleRenderer SeedParticleSystem = new ParticleRenderer();

		// Token: 0x04005370 RID: 21360
		private UIDust SeedDust = new UIDust();

		// Token: 0x04005371 RID: 21361
		private GroupOptionButton<bool> _advancedSeedButton;

		// Token: 0x04005372 RID: 21362
		private UICharacterNameButton _namePlate;

		// Token: 0x04005373 RID: 21363
		private UICharacterNameButton _seedPlate;

		// Token: 0x04005374 RID: 21364
		private UIWorldCreationPreview _previewPlate;

		// Token: 0x04005375 RID: 21365
		private GroupOptionButton<UIWorldCreation.WorldSizeId>[] _sizeButtons;

		// Token: 0x04005376 RID: 21366
		private GroupOptionButton<UIWorldCreation.WorldDifficultyId>[] _difficultyButtons;

		// Token: 0x04005377 RID: 21367
		private GroupOptionButton<UIWorldCreation.WorldEvilId>[] _evilButtons;

		// Token: 0x04005378 RID: 21368
		private UIText _descriptionText;

		// Token: 0x04005379 RID: 21369
		public const int MAX_NAME_LENGTH = 27;

		// Token: 0x0400537A RID: 21370
		private UIState _goBackTarget;

		// Token: 0x0400537B RID: 21371
		public UIWorldCreation.SubmitSeedEvent SubmitSeed;

		// Token: 0x0400537C RID: 21372
		private float ringPoint;

		// Token: 0x0400537D RID: 21373
		private const int numSteps = 61;

		// Token: 0x0400537E RID: 21374
		private Vector2[] oldPos = new Vector2[61];

		// Token: 0x0400537F RID: 21375
		private Vector2[] oldTangent = new Vector2[61];

		// Token: 0x04005380 RID: 21376
		private int specialSeedIndex;

		// Token: 0x04005381 RID: 21377
		private static VertexStrip _vertexStrip = new VertexStrip();

		// Token: 0x04005382 RID: 21378
		private float opacity = 0.6f;

		// Token: 0x04005383 RID: 21379
		private float opacity2 = 0.5f;

		// Token: 0x04005384 RID: 21380
		private float trial;

		// Token: 0x04005385 RID: 21381
		private float animationSpeed = 0.5f;

		// Token: 0x04005386 RID: 21382
		private float saturation = 0.5f;

		// Token: 0x02000905 RID: 2309
		private enum WorldSizeId
		{
			// Token: 0x040073FA RID: 29690
			Small,
			// Token: 0x040073FB RID: 29691
			Medium,
			// Token: 0x040073FC RID: 29692
			Large
		}

		// Token: 0x02000906 RID: 2310
		private enum WorldDifficultyId
		{
			// Token: 0x040073FE RID: 29694
			Normal,
			// Token: 0x040073FF RID: 29695
			Expert,
			// Token: 0x04007400 RID: 29696
			Master,
			// Token: 0x04007401 RID: 29697
			Creative
		}

		// Token: 0x02000907 RID: 2311
		private enum WorldEvilId
		{
			// Token: 0x04007403 RID: 29699
			Random,
			// Token: 0x04007404 RID: 29700
			Corruption,
			// Token: 0x04007405 RID: 29701
			Crimson
		}

		// Token: 0x02000908 RID: 2312
		// (Invoke) Token: 0x06004748 RID: 18248
		public delegate void SubmitSeedEvent();
	}
}
