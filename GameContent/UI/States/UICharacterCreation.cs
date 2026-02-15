using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using ReLogic.Content;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003AE RID: 942
	public class UICharacterCreation : UIState, IHaveBackButtonCommand
	{
		// Token: 0x06002BB7 RID: 11191 RVA: 0x005906C0 File Offset: 0x0058E8C0
		public UICharacterCreation(Player player)
		{
			this._player = player;
			this._player.difficulty = 0;
			this._tips = new GameTipsDisplay(new CharacterCreationTipsProvider());
			this.BuildPage();
			this.initialState = this.GetPlayerTemplateValues();
			UICharacterCreation.dirty = false;
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x005907C9 File Offset: 0x0058E9C9
		public override void Update(GameTime gameTime)
		{
			this._playedVoicePreviewThisFrame = false;
			base.Update(gameTime);
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x005907DC File Offset: 0x0058E9DC
		private void BuildPage()
		{
			base.RemoveAllChildren();
			int num = 4;
			UIElement uielement = new UIElement
			{
				Width = StyleDimension.FromPixels(500f),
				Height = StyleDimension.FromPixels((float)(380 + num)),
				Top = StyleDimension.FromPixels(220f),
				HAlign = 0.5f,
				VAlign = 0f
			};
			uielement.SetPadding(0f);
			base.Append(uielement);
			UIPanel uipanel = new UIPanel
			{
				Width = StyleDimension.FromPercent(1f),
				Height = StyleDimension.FromPixels(uielement.Height.Pixels - 150f - (float)num),
				Top = StyleDimension.FromPixels(50f),
				BackgroundColor = new Color(33, 43, 79) * 0.8f
			};
			uipanel.SetPadding(0f);
			uielement.Append(uipanel);
			this.MakeBackAndCreatebuttons(uielement);
			this.MakeCharPreview(uipanel);
			UIElement uielement2 = new UIElement
			{
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(50f, 0f)
			};
			uielement2.SetPadding(0f);
			uielement2.PaddingTop = 4f;
			uielement2.PaddingBottom = 0f;
			uipanel.Append(uielement2);
			UIElement uielement3 = new UIElement
			{
				Top = StyleDimension.FromPixelsAndPercent(uielement2.Height.Pixels + 6f, 0f),
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(uipanel.Height.Pixels - 70f, 0f)
			};
			uielement3.SetPadding(0f);
			uielement3.PaddingTop = 3f;
			uielement3.PaddingBottom = 0f;
			uipanel.Append(uielement3);
			this._topContainer = uielement2;
			this._middleContainer = uielement3;
			this.MakeInfoMenu(uielement3);
			this.MakeHSLMenu(uielement3);
			this.MakeHairsylesMenu(uielement3);
			this.MakeClothStylesMenu(uielement3);
			this.MakeCategoriesBar(uielement2);
			this.Click_CharInfo(null, null);
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x005909F4 File Offset: 0x0058EBF4
		private void MakeCharPreview(UIPanel container)
		{
			float num = 70f;
			for (float num2 = 0f; num2 < 1f; num2 += 1f)
			{
				UICharacter uicharacter = new UICharacter(this._player, true, false, 1.5f, false)
				{
					Width = StyleDimension.FromPixels(80f),
					Height = StyleDimension.FromPixelsAndPercent(80f, 0f),
					Top = StyleDimension.FromPixelsAndPercent(-num, 0f),
					VAlign = 0f,
					HAlign = 0.5f
				};
				uicharacter.PrepareAction = new Action(this.PreparePreview_Main);
				container.Append(uicharacter);
			}
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x00590AA0 File Offset: 0x0058ECA0
		private void MakeHairsylesMenu(UIElement middleInnerPanel)
		{
			Main.Hairstyles.UpdateUnlocks();
			UIElement uielement = new UIElement
			{
				Width = StyleDimension.FromPixelsAndPercent(-10f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				HAlign = 0.5f,
				VAlign = 0.5f,
				Top = StyleDimension.FromPixels(6f)
			};
			middleInnerPanel.Append(uielement);
			uielement.SetPadding(0f);
			UIList uilist = new UIList
			{
				Width = StyleDimension.FromPixelsAndPercent(-18f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(-6f, 1f)
			};
			uilist.SetPadding(4f);
			uielement.Append(uilist);
			UIScrollbar uiscrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Blue)
			{
				HAlign = 1f,
				Height = StyleDimension.FromPixelsAndPercent(-30f, 1f),
				Top = StyleDimension.FromPixels(10f)
			};
			uiscrollbar.SetView(100f, 1000f);
			uilist.SetScrollbar(uiscrollbar);
			uielement.Append(uiscrollbar);
			int count = Main.Hairstyles.AvailableHairstyles.Count;
			UIElement uielement2 = new UIElement
			{
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent((float)(48 * (count / 10 + ((count % 10 == 0) ? 0 : 1))), 0f)
			};
			uilist.Add(uielement2);
			uielement2.SetPadding(0f);
			for (int i = 0; i < count; i++)
			{
				UIHairStyleButton uihairStyleButton = new UIHairStyleButton(this._player, Main.Hairstyles.AvailableHairstyles[i])
				{
					Left = StyleDimension.FromPixels((float)(i % 10) * 46f + 6f),
					Top = StyleDimension.FromPixels((float)(i / 10) * 48f + 1f)
				};
				uihairStyleButton.SetSnapPoint("Middle", i, null, null);
				uihairStyleButton.SkipRenderingContent(i);
				uihairStyleButton.OnLeftMouseDown += this.RecordThatHairWasSelected;
				uielement2.Append(uihairStyleButton);
			}
			this._hairstylesContainer = uielement;
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x00590CD5 File Offset: 0x0058EED5
		private void RecordThatHairWasSelected(UIMouseEvent evt, UIElement listeningElement)
		{
			this._lastSelectedHairstyle = new int?(this._player.hair);
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x00590CF0 File Offset: 0x0058EEF0
		private void MakeClothStylesMenu(UIElement middleInnerPanel)
		{
			UIElement uielement = new UIElement
			{
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				HAlign = 0.5f,
				VAlign = 0.5f
			};
			middleInnerPanel.Append(uielement);
			uielement.SetPadding(0f);
			int num = 0;
			for (int i = 0; i < this._validClothStyles.Length; i++)
			{
				int num2 = 19;
				if (i >= this._validClothStyles.Length / 2)
				{
					num2 += 10;
				}
				else
				{
					num2 -= 8;
				}
				UIClothStyleButton uiclothStyleButton = new UIClothStyleButton(this._player, this._validClothStyles[i], new Action(this.PreparePreview_ClothStyle))
				{
					Left = StyleDimension.FromPixels((float)i * 46f + (float)num2),
					Top = StyleDimension.FromPixels((float)num)
				};
				uiclothStyleButton.OnLeftMouseDown += this.Click_CharClothStyle;
				uiclothStyleButton.SetSnapPoint("Middle", i, null, null);
				uielement.Append(uiclothStyleButton);
			}
			int num3 = 15;
			int num4 = 60;
			UIElement uielement2 = new UIElement
			{
				Width = StyleDimension.FromPixels(170f),
				Height = StyleDimension.FromPixels(50f),
				HAlign = 0f,
				Left = new StyleDimension((float)num4 - 34f, 0.5f),
				VAlign = 1f,
				Top = StyleDimension.FromPixels((float)(-(float)num3 - 7))
			};
			uielement.Append(uielement2);
			UIColoredImageButton uicoloredImageButton = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/Item_" + 271, 1), true)
			{
				VAlign = 0.5f,
				HAlign = 0f,
				Left = StyleDimension.FromPixelsAndPercent(0f, 0f)
			};
			uicoloredImageButton.SetColor(this._player.hairColor);
			uicoloredImageButton.OnLeftMouseDown += this.EquipArmorNone;
			uielement2.Append(uicoloredImageButton);
			UIColoredImageButton uicoloredImageButton2 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/Item_" + 5660, 1), true)
			{
				VAlign = 0.5f,
				HAlign = 0.5f
			};
			uicoloredImageButton2.OnLeftMouseDown += this.EquipArmorHallowed;
			uielement2.Append(uicoloredImageButton2);
			UIColoredImageButton uicoloredImageButton3 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/Item_" + 91, 1), true)
			{
				VAlign = 0.5f,
				HAlign = 0.25f
			};
			uicoloredImageButton3.OnLeftMouseDown += this.EquipArmorSilver;
			uielement2.Append(uicoloredImageButton3);
			UIColoredImageButton uicoloredImageButton4 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/Item_" + 239, 1), true)
			{
				VAlign = 0.5f,
				HAlign = 0.75f
			};
			uicoloredImageButton4.OnLeftMouseDown += this.EquipArmorFormal;
			uielement2.Append(uicoloredImageButton4);
			UIColoredImageButton uicoloredImageButton5 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/Item_" + 237, 1), true)
			{
				VAlign = 0.5f,
				HAlign = 1f
			};
			uicoloredImageButton5.OnLeftMouseDown += this.EquipArmorSwimming;
			uielement2.Append(uicoloredImageButton5);
			this._previewArmorButton = new UIElement[5];
			this._previewArmorButton[0] = uicoloredImageButton;
			this._previewArmorButton[1] = uicoloredImageButton2;
			this._previewArmorButton[2] = uicoloredImageButton3;
			this._previewArmorButton[3] = uicoloredImageButton4;
			this._previewArmorButton[4] = uicoloredImageButton5;
			this._previewArmorButton[0].SetSnapPoint("Preview", 0, null, null);
			this._previewArmorButton[2].SetSnapPoint("Preview", 1, null, null);
			this._previewArmorButton[1].SetSnapPoint("Preview", 2, null, null);
			this._previewArmorButton[3].SetSnapPoint("Preview", 3, null, null);
			this._previewArmorButton[4].SetSnapPoint("Preview", 4, null, null);
			UIElement uielement3 = new UIElement
			{
				Width = StyleDimension.FromPixels(100f),
				Height = StyleDimension.FromPixels(50f),
				HAlign = 0f,
				Left = new StyleDimension((float)num4, 0.5f),
				VAlign = 1f,
				Top = StyleDimension.FromPixels((float)(-(float)num3 + 38 - 9))
			};
			uielement.Append(uielement3);
			UIColoredImageButton uicoloredImageButton6 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Copy", 1), true)
			{
				VAlign = 0.5f,
				HAlign = 0f,
				Left = StyleDimension.FromPixelsAndPercent(0f, 0f)
			};
			uicoloredImageButton6.OnLeftMouseDown += this.Click_CopyPlayerTemplate;
			uielement3.Append(uicoloredImageButton6);
			this._copyTemplateButton = uicoloredImageButton6;
			UIColoredImageButton uicoloredImageButton7 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Paste", 1), true)
			{
				VAlign = 0.5f,
				HAlign = 0.5f
			};
			uicoloredImageButton7.OnLeftMouseDown += this.Click_PastePlayerTemplate;
			uielement3.Append(uicoloredImageButton7);
			this._pasteTemplateButton = uicoloredImageButton7;
			UIColoredImageButton uicoloredImageButton8 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Randomize", 1), true)
			{
				VAlign = 0.5f,
				HAlign = 1f
			};
			uicoloredImageButton8.OnLeftMouseDown += this.Click_RandomizePlayer;
			uielement3.Append(uicoloredImageButton8);
			this._randomizePlayerButton = uicoloredImageButton8;
			UIElement uielement4 = new UIElement
			{
				Width = StyleDimension.FromPixels(90f),
				Height = StyleDimension.FromPixels(50f),
				HAlign = 1f,
				Left = new StyleDimension((float)(-(float)num4), -0.5f),
				VAlign = 1f,
				Top = StyleDimension.FromPixels((float)(-(float)num3))
			};
			uielement.Append(uielement4);
			UIHorizontalSeparator element = new UIHorizontalSeparator(2, true)
			{
				Width = StyleDimension.FromPixelsAndPercent(-38f, 1f),
				HAlign = 0.5f,
				VAlign = 1f,
				Top = StyleDimension.FromPixelsAndPercent((float)(-52 - num3), 0f),
				Left = new StyleDimension(-3f, 0f),
				Color = Color.Lerp(Color.White, new Color(63, 65, 151, 255), 0.85f) * 0.9f
			};
			uielement.Append(element);
			Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/TexturePackButtons", 1);
			Asset<Texture2D> asset2 = Main.Assets.Request<Texture2D>("Images/UI/TexturePackButtonsOutline", 1);
			UIImageButton uiimageButton = new UIImageButton(asset, new Rectangle?(asset.Frame(2, 2, 0, 1, 0, 0)))
			{
				VAlign = 0.5f,
				HAlign = 0f,
				Left = StyleDimension.FromPixelsAndPercent(0f, 0f),
				BorderColor = Main.OurFavoriteColor
			};
			uiimageButton.SetVisibility(1f, 1f);
			uiimageButton.SetHoverImage(asset2, new Rectangle?(asset2.Frame(2, 2, 0, 1, 0, 0)));
			uiimageButton.OnLeftMouseDown += this.Click_VoiceCycleBack;
			uielement4.Append(uiimageButton);
			UIImageButton uiimageButton2 = new UIImageButton(asset, new Rectangle?(asset.Frame(2, 2, 1, 1, 0, 0)))
			{
				VAlign = 0.5f,
				HAlign = 1f,
				Left = StyleDimension.FromPixelsAndPercent(0f, 0f),
				BorderColor = Main.OurFavoriteColor
			};
			uiimageButton2.SetVisibility(1f, 1f);
			uiimageButton2.SetHoverImage(asset2, new Rectangle?(asset2.Frame(2, 2, 1, 1, 0, 0)));
			uiimageButton2.OnLeftMouseDown += this.Click_VoiceCycleForward;
			uielement4.Append(uiimageButton2);
			UIColoredImageButton uicoloredImageButton9 = new UIColoredImageButton(null, false)
			{
				VAlign = 0.5f,
				HAlign = 0.5f,
				Left = StyleDimension.FromPixelsAndPercent(0f, 0f),
				Width = StyleDimension.FromPixels(52f),
				Height = StyleDimension.FromPixels(52f)
			};
			UIImage uiimage = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Voice", 1))
			{
				VAlign = 0.5f,
				HAlign = 0.5f,
				IgnoresMouseInteraction = true,
				Color = Main.OurFavoriteColor
			};
			uiimage.OnUpdate += this.voiceIcon_OnUpdate;
			uicoloredImageButton9.Append(uiimage);
			UIText uitext = new UIText("", 0.85f, false)
			{
				VAlign = 1f,
				HAlign = 1f,
				TextOriginX = 0.5f,
				TextOriginY = 1f,
				Top = StyleDimension.FromPixels(-6f),
				Left = StyleDimension.FromPixels(-12f),
				ShadowColor = Color.Black * 0.3f
			};
			uitext.OnUpdate += this.voiceNumber_OnUpdate;
			uicoloredImageButton9.Append(uitext);
			uicoloredImageButton9.OnLeftMouseDown += this.Click_VoicePlay;
			uielement4.Append(uicoloredImageButton9);
			UIColoredSlider uicoloredSlider = new UIColoredSlider(LocalizedText.Empty, new Func<float>(this.GetPitchSlider), new Action<float>(this.SetPitchSlider_Keyboard), new Action(this.SetPitchSlider_GamePad), new Func<float, Color>(this.GetVoicePitchColorAt), Color.Transparent)
			{
				VAlign = 1f,
				HAlign = 0.5f,
				Width = StyleDimension.FromPixelsAndPercent(187f, 0f),
				Top = StyleDimension.FromPixels(-10f),
				Left = StyleDimension.FromPixels(55f)
			};
			uicoloredSlider.OnLeftMouseDown += this.Click_VoicePitch;
			uicoloredSlider.OnUpdate += this.PitchSliderUpdate;
			uicoloredSlider.SetSnapPoint("pitch", 0, null, new Vector2?(new Vector2(-93f, 16f)));
			uielement4.Append(uicoloredSlider);
			this._pitchSlider = uicoloredSlider;
			uiimageButton.SetSnapPoint("Low", 1, null, null);
			uicoloredImageButton9.SetSnapPoint("Low", 2, null, null);
			uiimageButton2.SetSnapPoint("Low", 3, null, null);
			this._voicePrevious = uiimageButton;
			this._voiceNext = uiimageButton2;
			this._voicePlay = uicoloredImageButton9;
			uicoloredImageButton6.SetSnapPoint("Low", 4, null, null);
			uicoloredImageButton7.SetSnapPoint("Low", 5, null, null);
			uicoloredImageButton8.SetSnapPoint("Low", 6, null, null);
			this._clothStylesContainer = uielement;
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x00591860 File Offset: 0x0058FA60
		private void EquipArmorNone(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			this._femaleArmor = (this._maleArmor = default(UICharacterCreation.ArmorAssignments));
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x0059189C File Offset: 0x0058FA9C
		private void EquipArmorGold(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			this._femaleArmor = (this._maleArmor = new UICharacterCreation.ArmorAssignments
			{
				HeadItem = 92,
				BodyItem = 83,
				LegItem = 79
			});
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x005918F4 File Offset: 0x0058FAF4
		private void EquipArmorSilver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			this._femaleArmor = (this._maleArmor = new UICharacterCreation.ArmorAssignments
			{
				HeadItem = 91,
				BodyItem = 82,
				LegItem = 78
			});
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x0059194C File Offset: 0x0058FB4C
		private void EquipArmorFuneral(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			this._femaleArmor = (this._maleArmor = new UICharacterCreation.ArmorAssignments
			{
				HeadItem = 4704,
				BodyItem = 4705,
				LegItem = 4706
			});
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x005919AC File Offset: 0x0058FBAC
		private void EquipArmorHallowed(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			this._femaleArmor = (this._maleArmor = new UICharacterCreation.ArmorAssignments
			{
				HeadItem = 5660,
				BodyItem = 551,
				LegItem = 552
			});
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x00591A0C File Offset: 0x0058FC0C
		private void EquipArmorFormal(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			this._maleArmor = new UICharacterCreation.ArmorAssignments
			{
				HeadItem = 239,
				BodyItem = 240,
				LegItem = 241
			};
			this._femaleArmor = new UICharacterCreation.ArmorAssignments
			{
				HeadItem = 3478,
				BodyItem = 3479,
				LegItem = 0
			};
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x00591A90 File Offset: 0x0058FC90
		private void EquipArmorSwimming(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			this._maleArmor = new UICharacterCreation.ArmorAssignments
			{
				HeadItem = 237,
				BodyItem = 3785,
				LegItem = 5649
			};
			this._femaleArmor = new UICharacterCreation.ArmorAssignments
			{
				HeadItem = 237,
				BodyItem = 5646,
				LegItem = 5647,
				Accessory1Item = 208
			};
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x00591B24 File Offset: 0x0058FD24
		private void PreparePreview_Main()
		{
			this._player.direction = 1;
			this.TryAutoAssigningHair();
			this.UpdatePreviewItems();
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x00591B3E File Offset: 0x0058FD3E
		private void PreparePreview_ClothStyle()
		{
			this._player.direction = (this._player.Male ? 1 : -1);
			this.TryAutoAssigningHair();
			this.UpdatePreviewItems();
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x00591B68 File Offset: 0x0058FD68
		private void TryAutoAssigningHair()
		{
			if (this._lastSelectedHairstyle != null)
			{
				return;
			}
			int hair;
			if (this._defaultHairstylesForClothStyle.TryGetValue(this._player.skinVariant, out hair))
			{
				this._player.hair = hair;
			}
		}

		// Token: 0x06002BC8 RID: 11208 RVA: 0x00591BAC File Offset: 0x0058FDAC
		private void UpdatePreviewItems()
		{
			UICharacterCreation.ArmorAssignments armorAssignments = this._femaleArmor;
			if (this._player.Male)
			{
				armorAssignments = this._maleArmor;
			}
			this._player.armor[0].SetDefaults(armorAssignments.HeadItem, null);
			this._player.armor[1].SetDefaults(armorAssignments.BodyItem, null);
			this._player.armor[2].SetDefaults(armorAssignments.LegItem, null);
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x00591C20 File Offset: 0x0058FE20
		private void PitchSliderUpdate(UIElement affectedElement)
		{
			if (!this._pitchChanged)
			{
				return;
			}
			int num = this._pitchChangedCooldown - 1;
			this._pitchChangedCooldown = num;
			if (num > 0)
			{
				return;
			}
			this._pitchChanged = false;
			this.PlayVoicePreview();
		}

		// Token: 0x06002BCA RID: 11210 RVA: 0x00591C58 File Offset: 0x0058FE58
		private void PitchChanged()
		{
			this._pitchChanged = true;
			this._pitchChangedCooldown = 3;
		}

		// Token: 0x06002BCB RID: 11211 RVA: 0x00591C68 File Offset: 0x0058FE68
		private void SetPitchSlider_GamePad()
		{
			if (!PlayerInput.UsingGamepad)
			{
				return;
			}
			float pitchAmount = this._pitchAmount;
			float num = UILinksInitializer.HandleSliderHorizontalInput(Utils.Remap(this._pitchAmount, -1f, 1f, 0f, 1f, true), 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			this._pitchAmount = Utils.Remap(num, 0f, 1f, -1f, 1f, true);
			num = this.RemapPitchSliderKnob(num);
			this._player.voicePitchOffset = Utils.Remap(num, 0f, 1f, -1f, 1f, true);
			if (pitchAmount != this._pitchAmount)
			{
				this.PitchChanged();
			}
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x00591D20 File Offset: 0x0058FF20
		private float RemapPitchSliderKnob(float pitchSliderValue)
		{
			int num = 20;
			return (float)Math.Round((double)(pitchSliderValue * (float)num)) / (float)num;
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x00591D40 File Offset: 0x0058FF40
		private void SetPitchSlider_Keyboard(float amount)
		{
			amount = this.RemapPitchSliderKnob(amount);
			float voicePitchOffset = this._player.voicePitchOffset;
			this._pitchAmount = (this._player.voicePitchOffset = Utils.Remap(amount, 0f, 1f, -1f, 1f, true));
			this._pitchChangedCooldown = 3;
			if (voicePitchOffset != this._player.voicePitchOffset)
			{
				this.PitchChanged();
			}
		}

		// Token: 0x06002BCE RID: 11214 RVA: 0x00591DAA File Offset: 0x0058FFAA
		private float GetPitchSlider()
		{
			return Utils.Remap(this.RemapPitchSliderKnob(this._pitchAmount), -1f, 1f, 0f, 1f, true);
		}

		// Token: 0x06002BCF RID: 11215 RVA: 0x00591DD4 File Offset: 0x0058FFD4
		private Color GetVoicePitchColorAt(float x)
		{
			float fromValue = (x * 4f + 0.5f) % 1f;
			float num = Utils.Remap(fromValue, 0f, 0.5f, 0f, 1f, true) * Utils.Remap(fromValue, 0.5f, 1f, 1f, 0f, true);
			float amount = num * num * num * num * num;
			return Color.Lerp(new Color(90, 90, 120), Color.White, amount);
		}

		// Token: 0x06002BD0 RID: 11216 RVA: 0x00591E50 File Offset: 0x00590050
		private void voiceNumber_OnUpdate(UIElement affectedElement)
		{
			int num = 0;
			int[] variantOrder = PlayerVoiceID.VariantOrder;
			for (int i = 0; i < variantOrder.Length; i++)
			{
				if (variantOrder[i] == this._player.voiceVariant)
				{
					num = i;
					break;
				}
			}
			(affectedElement as UIText).SetText((num + 1).ToString());
		}

		// Token: 0x06002BD1 RID: 11217 RVA: 0x00591E9D File Offset: 0x0059009D
		private void voiceIcon_OnUpdate(UIElement affectedElement)
		{
			(affectedElement as UIImage).Color = PlayerVoiceID.Sets.Colors[this._player.voiceVariant];
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x00591EC0 File Offset: 0x005900C0
		private void MakeCategoriesBar(UIElement categoryContainer)
		{
			float xPositionStart = -240f;
			float xPositionPerId = 48f;
			this._colorPickers = new UIColoredImageButton[10];
			categoryContainer.Append(this.CreateColorPicker(UICharacterCreation.CategoryId.HairColor, "Images/UI/CharCreation/ColorHair", xPositionStart, xPositionPerId));
			categoryContainer.Append(this.CreateColorPicker(UICharacterCreation.CategoryId.Eye, "Images/UI/CharCreation/ColorEye", xPositionStart, xPositionPerId));
			categoryContainer.Append(this.CreateColorPicker(UICharacterCreation.CategoryId.Skin, "Images/UI/CharCreation/ColorSkin", xPositionStart, xPositionPerId));
			categoryContainer.Append(this.CreateColorPicker(UICharacterCreation.CategoryId.Shirt, "Images/UI/CharCreation/ColorShirt", xPositionStart, xPositionPerId));
			categoryContainer.Append(this.CreateColorPicker(UICharacterCreation.CategoryId.Undershirt, "Images/UI/CharCreation/ColorUndershirt", xPositionStart, xPositionPerId));
			categoryContainer.Append(this.CreateColorPicker(UICharacterCreation.CategoryId.Pants, "Images/UI/CharCreation/ColorPants", xPositionStart, xPositionPerId));
			categoryContainer.Append(this.CreateColorPicker(UICharacterCreation.CategoryId.Shoes, "Images/UI/CharCreation/ColorShoes", xPositionStart, xPositionPerId));
			this._colorPickers[4].SetMiddleTexture(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/ColorEyeBack", 1));
			this._clothingStylesCategoryButton = this.CreatePickerWithoutClick(UICharacterCreation.CategoryId.Clothing, "Images/UI/CharCreation/ClothStyleMale", xPositionStart, xPositionPerId);
			this._clothingStylesCategoryButton.OnLeftMouseDown += this.Click_ClothStyles;
			this._clothingStylesCategoryButton.SetSnapPoint("Top", 1, null, null);
			categoryContainer.Append(this._clothingStylesCategoryButton);
			Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/ColorCharacter", 1);
			this._clothingStylesCategoryButton.SetColor(Color.Transparent);
			for (int i = 0; i < this._characterPreviewLayers.Length; i++)
			{
				UIImageFramed uiimageFramed = new UIImageFramed(asset, asset.Frame(1, 7, 0, i, 0, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
				this._characterPreviewLayers[i] = uiimageFramed;
				this._clothingStylesCategoryButton.Append(uiimageFramed);
				this._clothingStylesCategoryButton.OnUpdate += this._clothingStylesCategoryButton_OnUpdate;
			}
			this._hairStylesCategoryButton = this.CreatePickerWithoutClick(UICharacterCreation.CategoryId.HairStyle, "Images/UI/CharCreation/HairStyle_Hair", xPositionStart, xPositionPerId);
			this._hairStylesCategoryButton.OnLeftMouseDown += this.Click_HairStyles;
			this._hairStylesCategoryButton.SetMiddleTexture(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/HairStyle_Arrow", 1));
			this._hairStylesCategoryButton.SetSnapPoint("Top", 2, null, null);
			categoryContainer.Append(this._hairStylesCategoryButton);
			this._charInfoCategoryButton = this.CreatePickerWithoutClick(UICharacterCreation.CategoryId.CharInfo, "Images/UI/CharCreation/CharInfo", xPositionStart, xPositionPerId);
			this._charInfoCategoryButton.OnLeftMouseDown += this.Click_CharInfo;
			this._charInfoCategoryButton.SetSnapPoint("Top", 0, null, null);
			categoryContainer.Append(this._charInfoCategoryButton);
			this.UpdateColorPickers();
			UIHorizontalSeparator element = new UIHorizontalSeparator(2, true)
			{
				Width = StyleDimension.FromPixelsAndPercent(-25f, 1f),
				Top = StyleDimension.FromPixels(6f),
				Left = new StyleDimension(-2.5f, 0f),
				VAlign = 1f,
				HAlign = 0.5f,
				Color = Color.Lerp(Color.White, new Color(63, 65, 151, 255), 0.85f) * 0.9f
			};
			categoryContainer.Append(element);
			int num = 21;
			UIText uitext = new UIText(PlayerInput.GenerateInputTag_ForCurrentGamemode(false, "HotbarMinus"), 1f, false)
			{
				Left = new StyleDimension((float)(-(float)num), 0f),
				VAlign = 0.5f,
				Top = new StyleDimension(-4f, 0f)
			};
			categoryContainer.Append(uitext);
			UIText uitext2 = new UIText(PlayerInput.GenerateInputTag_ForCurrentGamemode(false, "HotbarMinus"), 1f, false)
			{
				HAlign = 1f,
				Left = new StyleDimension((float)(12 + num), 0f),
				VAlign = 0.5f,
				Top = new StyleDimension(-4f, 0f)
			};
			categoryContainer.Append(uitext2);
			this._helpGlyphLeft = uitext;
			this._helpGlyphRight = uitext2;
			categoryContainer.OnUpdate += this.UpdateHelpGlyphs;
		}

		// Token: 0x06002BD3 RID: 11219 RVA: 0x005922C4 File Offset: 0x005904C4
		private void _clothingStylesCategoryButton_OnUpdate(UIElement affectedElement)
		{
			this._characterPreviewLayers[0].Color = this._player.hairColor;
			this._characterPreviewLayers[1].Color = this._player.eyeColor;
			this._characterPreviewLayers[2].Color = this._player.skinColor;
			this._characterPreviewLayers[3].Color = this._player.shirtColor;
			this._characterPreviewLayers[4].Color = this._player.underShirtColor;
			this._characterPreviewLayers[5].Color = this._player.pantsColor;
			this._characterPreviewLayers[6].Color = this._player.shoeColor;
		}

		// Token: 0x06002BD4 RID: 11220 RVA: 0x0059237C File Offset: 0x0059057C
		private void UpdateHelpGlyphs(UIElement element)
		{
			string text = "";
			string text2 = "";
			if (PlayerInput.UsingGamepad)
			{
				text = PlayerInput.GenerateInputTag_ForCurrentGamemode(false, "HotbarMinus");
				text2 = PlayerInput.GenerateInputTag_ForCurrentGamemode(false, "HotbarPlus");
			}
			this._helpGlyphLeft.SetText(text);
			this._helpGlyphRight.SetText(text2);
		}

		// Token: 0x06002BD5 RID: 11221 RVA: 0x005923CC File Offset: 0x005905CC
		private UIColoredImageButton CreateColorPicker(UICharacterCreation.CategoryId id, string texturePath, float xPositionStart, float xPositionPerId)
		{
			UIColoredImageButton uicoloredImageButton = new UIColoredImageButton(Main.Assets.Request<Texture2D>(texturePath, 1), false);
			this._colorPickers[(int)id] = uicoloredImageButton;
			uicoloredImageButton.VAlign = 0f;
			uicoloredImageButton.HAlign = 0f;
			uicoloredImageButton.Left.Set(xPositionStart + (float)id * xPositionPerId, 0.5f);
			uicoloredImageButton.OnLeftMouseDown += this.Click_ColorPicker;
			uicoloredImageButton.SetSnapPoint("Top", (int)id, null, null);
			return uicoloredImageButton;
		}

		// Token: 0x06002BD6 RID: 11222 RVA: 0x00592454 File Offset: 0x00590654
		private UIColoredImageButton CreatePickerWithoutClick(UICharacterCreation.CategoryId id, string texturePath, float xPositionStart, float xPositionPerId)
		{
			UIColoredImageButton uicoloredImageButton = new UIColoredImageButton(Main.Assets.Request<Texture2D>(texturePath, 1), false);
			uicoloredImageButton.VAlign = 0f;
			uicoloredImageButton.HAlign = 0f;
			uicoloredImageButton.Left.Set(xPositionStart + (float)id * xPositionPerId, 0.5f);
			return uicoloredImageButton;
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x005924A0 File Offset: 0x005906A0
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
			UICharacterNameButton uicharacterNameButton = new UICharacterNameButton(Language.GetText("UI.WorldCreationName"), Language.GetText("UI.PlayerEmptyName"), null);
			uicharacterNameButton.Width = StyleDimension.FromPixelsAndPercent(0f, 1f);
			uicharacterNameButton.HAlign = 0.5f;
			uielement.Append(uicharacterNameButton);
			this._charName = uicharacterNameButton;
			uicharacterNameButton.OnLeftMouseDown += this.Click_Naming;
			uicharacterNameButton.SetSnapPoint("Middle", 0, null, null);
			float num = 4f;
			float num2 = 0f;
			float num3 = 0.4f;
			UIElement uielement2 = new UIElement
			{
				HAlign = 0f,
				VAlign = 1f,
				Width = StyleDimension.FromPixelsAndPercent(-num, num3),
				Height = StyleDimension.FromPixelsAndPercent(-50f, 1f)
			};
			uielement2.SetPadding(0f);
			uielement.Append(uielement2);
			UISlicedImage uislicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", 1))
			{
				HAlign = 1f,
				VAlign = 1f,
				Width = StyleDimension.FromPixelsAndPercent(-num * 2f, 1f - num3),
				Left = StyleDimension.FromPixels(-num),
				Height = StyleDimension.FromPixelsAndPercent(uielement2.Height.Pixels, uielement2.Height.Precent)
			};
			uislicedImage.SetSliceDepths(10);
			uislicedImage.Color = Color.LightGray * 0.7f;
			uielement.Append(uislicedImage);
			float num4 = 4f;
			UIDifficultyButton uidifficultyButton = new UIDifficultyButton(this._player, Lang.menu[26], Lang.menu[31], 0, Color.Cyan)
			{
				HAlign = 0f,
				VAlign = 1f / (num4 - 1f),
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(-num2, 1f / num4)
			};
			UIDifficultyButton uidifficultyButton2 = new UIDifficultyButton(this._player, Lang.menu[25], Lang.menu[30], 1, Main.mcColor)
			{
				HAlign = 0f,
				VAlign = 2f / (num4 - 1f),
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(-num2, 1f / num4)
			};
			UIDifficultyButton uidifficultyButton3 = new UIDifficultyButton(this._player, Lang.menu[24], Lang.menu[29], 2, Main.hcColor)
			{
				HAlign = 0f,
				VAlign = 1f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(-num2, 1f / num4)
			};
			UIDifficultyButton uidifficultyButton4 = new UIDifficultyButton(this._player, Language.GetText("UI.Creative"), Language.GetText("UI.CreativeDescriptionPlayer"), 3, Main.creativeModeColor)
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(-num2, 1f / num4)
			};
			UIText uitext = new UIText(Lang.menu[26], 1f, false)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Top = StyleDimension.FromPixelsAndPercent(15f, 0f),
				IsWrapped = true
			};
			uitext.PaddingLeft = 20f;
			uitext.PaddingRight = 20f;
			uislicedImage.Append(uitext);
			uielement2.Append(uidifficultyButton4);
			uielement2.Append(uidifficultyButton);
			uielement2.Append(uidifficultyButton2);
			uielement2.Append(uidifficultyButton3);
			this._infoContainer = uielement;
			this._difficultyDescriptionText = uitext;
			uidifficultyButton4.OnLeftMouseDown += this.UpdateDifficultyDescription;
			uidifficultyButton.OnLeftMouseDown += this.UpdateDifficultyDescription;
			uidifficultyButton2.OnLeftMouseDown += this.UpdateDifficultyDescription;
			uidifficultyButton3.OnLeftMouseDown += this.UpdateDifficultyDescription;
			this.UpdateDifficultyDescription(null, null);
			uidifficultyButton4.SetSnapPoint("Middle", 1, null, null);
			uidifficultyButton.SetSnapPoint("Middle", 2, null, null);
			uidifficultyButton2.SetSnapPoint("Middle", 3, null, null);
			uidifficultyButton3.SetSnapPoint("Middle", 4, null, null);
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x005929E4 File Offset: 0x00590BE4
		private void UpdateDifficultyDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			LocalizedText text = Lang.menu[31];
			switch (this._player.difficulty)
			{
			case 0:
				text = Lang.menu[31];
				break;
			case 1:
				text = Lang.menu[30];
				break;
			case 2:
				text = Lang.menu[29];
				break;
			case 3:
				text = Language.GetText("UI.CreativeDescriptionPlayer");
				break;
			}
			this._difficultyDescriptionText.SetText(text);
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x00592A58 File Offset: 0x00590C58
		private void MakeHSLMenu(UIElement parentContainer)
		{
			UIElement uielement = new UIElement
			{
				Width = StyleDimension.FromPixelsAndPercent(220f, 0f),
				Height = StyleDimension.FromPixelsAndPercent(158f, 0f),
				HAlign = 0.5f,
				VAlign = 0f
			};
			uielement.SetPadding(0f);
			parentContainer.Append(uielement);
			UIElement uielement2 = new UIPanel
			{
				Width = StyleDimension.FromPixelsAndPercent(220f, 0f),
				Height = StyleDimension.FromPixelsAndPercent(104f, 0f),
				HAlign = 0.5f,
				VAlign = 0f,
				Top = StyleDimension.FromPixelsAndPercent(10f, 0f)
			};
			uielement2.SetPadding(0f);
			uielement2.PaddingTop = 3f;
			uielement.Append(uielement2);
			uielement2.Append(this.CreateHSLSlider(UICharacterCreation.HSLSliderId.Hue));
			uielement2.Append(this.CreateHSLSlider(UICharacterCreation.HSLSliderId.Saturation));
			uielement2.Append(this.CreateHSLSlider(UICharacterCreation.HSLSliderId.Luminance));
			UIPanel uipanel = new UIPanel
			{
				VAlign = 1f,
				HAlign = 1f,
				Width = StyleDimension.FromPixelsAndPercent(100f, 0f),
				Height = StyleDimension.FromPixelsAndPercent(32f, 0f)
			};
			UIText uitext = new UIText("FFFFFF", 1f, false)
			{
				VAlign = 0.5f,
				HAlign = 0.5f
			};
			uipanel.Append(uitext);
			uielement.Append(uipanel);
			UIColoredImageButton uicoloredImageButton = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Copy", 1), true)
			{
				VAlign = 1f,
				HAlign = 0f,
				Left = StyleDimension.FromPixelsAndPercent(0f, 0f)
			};
			uicoloredImageButton.OnLeftMouseDown += this.Click_CopyHex;
			uielement.Append(uicoloredImageButton);
			this._copyHexButton = uicoloredImageButton;
			UIColoredImageButton uicoloredImageButton2 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Paste", 1), true)
			{
				VAlign = 1f,
				HAlign = 0f,
				Left = StyleDimension.FromPixelsAndPercent(40f, 0f)
			};
			uicoloredImageButton2.OnLeftMouseDown += this.Click_PasteHex;
			uielement.Append(uicoloredImageButton2);
			this._pasteHexButton = uicoloredImageButton2;
			UIColoredImageButton uicoloredImageButton3 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Randomize", 1), true)
			{
				VAlign = 1f,
				HAlign = 0f,
				Left = StyleDimension.FromPixelsAndPercent(80f, 0f)
			};
			uicoloredImageButton3.OnLeftMouseDown += this.Click_RandomizeSingleColor;
			uielement.Append(uicoloredImageButton3);
			this._randomColorButton = uicoloredImageButton3;
			this._hslContainer = uielement;
			this._hslHexText = uitext;
			uicoloredImageButton.SetSnapPoint("Low", 0, null, null);
			uicoloredImageButton2.SetSnapPoint("Low", 1, null, null);
			uicoloredImageButton3.SetSnapPoint("Low", 2, null, null);
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x00592D7A File Offset: 0x00590F7A
		private void Click_VoicePitch(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x00592D94 File Offset: 0x00590F94
		private UIColoredSlider CreateHSLSlider(UICharacterCreation.HSLSliderId id)
		{
			UIColoredSlider uicoloredSlider = this.CreateHSLSliderButtonBase(id);
			uicoloredSlider.VAlign = 0f;
			uicoloredSlider.HAlign = 0f;
			uicoloredSlider.Width = StyleDimension.FromPixelsAndPercent(-10f, 1f);
			uicoloredSlider.Top.Set((float)((UICharacterCreation.HSLSliderId)30 * id), 0f);
			uicoloredSlider.OnLeftMouseDown += this.Click_ColorPicker;
			uicoloredSlider.SetSnapPoint("Middle", (int)id, null, new Vector2?(new Vector2(0f, 20f)));
			return uicoloredSlider;
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x00592E24 File Offset: 0x00591024
		private UIColoredSlider CreateHSLSliderButtonBase(UICharacterCreation.HSLSliderId id)
		{
			UIColoredSlider result;
			if (id != UICharacterCreation.HSLSliderId.Saturation)
			{
				if (id != UICharacterCreation.HSLSliderId.Luminance)
				{
					result = new UIColoredSlider(LocalizedText.Empty, () => this.GetHSLSliderPosition(UICharacterCreation.HSLSliderId.Hue), delegate(float x)
					{
						this.UpdateHSLValue(UICharacterCreation.HSLSliderId.Hue, x);
					}, new Action(this.UpdateHSL_H), (float x) => this.GetHSLSliderColorAt(UICharacterCreation.HSLSliderId.Hue, x), Color.Transparent);
				}
				else
				{
					result = new UIColoredSlider(LocalizedText.Empty, () => this.GetHSLSliderPosition(UICharacterCreation.HSLSliderId.Luminance), delegate(float x)
					{
						this.UpdateHSLValue(UICharacterCreation.HSLSliderId.Luminance, x);
					}, new Action(this.UpdateHSL_L), (float x) => this.GetHSLSliderColorAt(UICharacterCreation.HSLSliderId.Luminance, x), Color.Transparent);
				}
			}
			else
			{
				result = new UIColoredSlider(LocalizedText.Empty, () => this.GetHSLSliderPosition(UICharacterCreation.HSLSliderId.Saturation), delegate(float x)
				{
					this.UpdateHSLValue(UICharacterCreation.HSLSliderId.Saturation, x);
				}, new Action(this.UpdateHSL_S), (float x) => this.GetHSLSliderColorAt(UICharacterCreation.HSLSliderId.Saturation, x), Color.Transparent);
			}
			return result;
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x00592F08 File Offset: 0x00591108
		private void UpdateHSL_H()
		{
			float value = UILinksInitializer.HandleSliderHorizontalInput(this._currentColorHSL.X, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			this.UpdateHSLValue(UICharacterCreation.HSLSliderId.Hue, value);
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x00592F48 File Offset: 0x00591148
		private void UpdateHSL_S()
		{
			float value = UILinksInitializer.HandleSliderHorizontalInput(this._currentColorHSL.Y, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			this.UpdateHSLValue(UICharacterCreation.HSLSliderId.Saturation, value);
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x00592F88 File Offset: 0x00591188
		private void UpdateHSL_L()
		{
			float value = UILinksInitializer.HandleSliderHorizontalInput(this._currentColorHSL.Z, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			this.UpdateHSLValue(UICharacterCreation.HSLSliderId.Luminance, value);
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x00592FC7 File Offset: 0x005911C7
		private float GetHSLSliderPosition(UICharacterCreation.HSLSliderId id)
		{
			switch (id)
			{
			case UICharacterCreation.HSLSliderId.Hue:
				return this._currentColorHSL.X;
			case UICharacterCreation.HSLSliderId.Saturation:
				return this._currentColorHSL.Y;
			case UICharacterCreation.HSLSliderId.Luminance:
				return this._currentColorHSL.Z;
			default:
				return 1f;
			}
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x00593008 File Offset: 0x00591208
		private void UpdateHSLValue(UICharacterCreation.HSLSliderId id, float value)
		{
			switch (id)
			{
			case UICharacterCreation.HSLSliderId.Hue:
				this._currentColorHSL.X = value;
				break;
			case UICharacterCreation.HSLSliderId.Saturation:
				this._currentColorHSL.Y = value;
				break;
			case UICharacterCreation.HSLSliderId.Luminance:
				this._currentColorHSL.Z = value;
				break;
			}
			Color color = UICharacterCreation.ScaledHslToRgb(this._currentColorHSL.X, this._currentColorHSL.Y, this._currentColorHSL.Z);
			this.ApplyPendingColor(color);
			UIColoredImageButton uicoloredImageButton = this._colorPickers[(int)this._selectedPicker];
			if (uicoloredImageButton != null)
			{
				uicoloredImageButton.SetColor(color);
			}
			if (this._selectedPicker == UICharacterCreation.CategoryId.HairColor)
			{
				this._hairStylesCategoryButton.SetColor(color);
			}
			this.UpdateHexText(color);
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x005930B4 File Offset: 0x005912B4
		private Color GetHSLSliderColorAt(UICharacterCreation.HSLSliderId id, float pointAt)
		{
			switch (id)
			{
			case UICharacterCreation.HSLSliderId.Hue:
				return UICharacterCreation.ScaledHslToRgb(pointAt, 1f, 0.5f);
			case UICharacterCreation.HSLSliderId.Saturation:
				return UICharacterCreation.ScaledHslToRgb(this._currentColorHSL.X, pointAt, this._currentColorHSL.Z);
			case UICharacterCreation.HSLSliderId.Luminance:
				return UICharacterCreation.ScaledHslToRgb(this._currentColorHSL.X, this._currentColorHSL.Y, pointAt);
			default:
				return Color.White;
			}
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x00593128 File Offset: 0x00591328
		private void ApplyPendingColor(Color pendingColor)
		{
			switch (this._selectedPicker)
			{
			case UICharacterCreation.CategoryId.HairColor:
				this._player.hairColor = pendingColor;
				return;
			case UICharacterCreation.CategoryId.Eye:
				this._player.eyeColor = pendingColor;
				return;
			case UICharacterCreation.CategoryId.Skin:
				this._player.skinColor = pendingColor;
				return;
			case UICharacterCreation.CategoryId.Shirt:
				this._player.shirtColor = pendingColor;
				return;
			case UICharacterCreation.CategoryId.Undershirt:
				this._player.underShirtColor = pendingColor;
				return;
			case UICharacterCreation.CategoryId.Pants:
				this._player.pantsColor = pendingColor;
				return;
			case UICharacterCreation.CategoryId.Shoes:
				this._player.shoeColor = pendingColor;
				return;
			default:
				return;
			}
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x005931BB File Offset: 0x005913BB
		private void UpdateHexText(Color pendingColor)
		{
			this._hslHexText.SetText(UICharacterCreation.GetHexText(pendingColor));
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x005931CE File Offset: 0x005913CE
		private static string GetHexText(Color pendingColor)
		{
			return "#" + pendingColor.Hex3().ToUpper();
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x005931E8 File Offset: 0x005913E8
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

		// Token: 0x06002BE7 RID: 11239 RVA: 0x0059336D File Offset: 0x0059156D
		private void Click_GoBack(UIMouseEvent evt, UIElement listeningElement)
		{
			UICharacterCreation.GoBack();
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x00593374 File Offset: 0x00591574
		private static void GoBack()
		{
			SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
			if (UICharacterCreation.dirty)
			{
				UICharacterCreation.BackupConfirmationState = Main.MenuUI.CurrentState;
				Main.menuMode = 40;
				return;
			}
			Main.OpenCharacterSelectUI();
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x005933B0 File Offset: 0x005915B0
		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x00584489 File Offset: 0x00582689
		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x00593408 File Offset: 0x00591608
		private void Click_ColorPicker(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			for (int i = 0; i < this._colorPickers.Length; i++)
			{
				if (this._colorPickers[i] == evt.Target)
				{
					this.SelectColorPicker((UICharacterCreation.CategoryId)i);
					return;
				}
			}
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x00593458 File Offset: 0x00591658
		private void Click_ClothStyles(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			this.UnselectAllCategories();
			this._selectedPicker = UICharacterCreation.CategoryId.Clothing;
			this._middleContainer.Append(this._clothStylesContainer);
			this._clothingStylesCategoryButton.SetSelected(true);
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x005934A4 File Offset: 0x005916A4
		private void Click_HairStyles(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			this.UnselectAllCategories();
			this._selectedPicker = UICharacterCreation.CategoryId.HairStyle;
			this._middleContainer.Append(this._hairstylesContainer);
			this._hairStylesCategoryButton.SetSelected(true);
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x005934F0 File Offset: 0x005916F0
		private void Click_CharInfo(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			this.UnselectAllCategories();
			this._selectedPicker = UICharacterCreation.CategoryId.CharInfo;
			this._middleContainer.Append(this._infoContainer);
			this._charInfoCategoryButton.SetSelected(true);
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x0059353C File Offset: 0x0059173C
		private void Click_CharClothStyle(UIMouseEvent evt, UIElement listeningElement)
		{
			if (this._maleArmor.HeadItem != 0 || this._maleArmor.BodyItem != 0 || this._maleArmor.LegItem != 0)
			{
				this.EquipArmorNone(evt, listeningElement);
				return;
			}
			UIClothStyleButton uiclothStyleButton = listeningElement as UIClothStyleButton;
			if (uiclothStyleButton != null)
			{
				int clothStyleId = uiclothStyleButton.ClothStyleId;
				this._player.skinVariant = clothStyleId;
			}
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			this._clothingStylesCategoryButton.SetImageWithoutSettingSize(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/" + (this._player.Male ? "ClothStyleMale" : "ClothStyleFemale"), 1));
			this.UpdateSelectedGender();
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x005935EC File Offset: 0x005917EC
		private void TryChangingVoice()
		{
			if (this._player.Male && this._player.voiceVariant == 2)
			{
				this._player.voiceVariant = 1;
			}
			if (!this._player.Male && this._player.voiceVariant == 1)
			{
				this._player.voiceVariant = 2;
			}
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x00593648 File Offset: 0x00591848
		private void UpdateSelectedGender()
		{
			if (this._oldMaleForVoiceAutoSwitch == this._player.Male)
			{
				this.PlayVoicePreview();
				return;
			}
			int voiceVariant = this._player.voiceVariant;
			if (voiceVariant != 1)
			{
				if (voiceVariant == 2)
				{
					if (!this._oldMaleForVoiceAutoSwitch)
					{
						this._player.voiceVariant = 1;
					}
				}
			}
			else if (this._oldMaleForVoiceAutoSwitch)
			{
				this._player.voiceVariant = 2;
			}
			this._oldMaleForVoiceAutoSwitch = this._player.Male;
			this.PlayVoicePreview();
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x005936C6 File Offset: 0x005918C6
		private void Click_CopyHex(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			Platform.Get<IClipboard>().Value = this._hslHexText.Text;
		}

		// Token: 0x06002BF3 RID: 11251 RVA: 0x005936F4 File Offset: 0x005918F4
		private void Click_PasteHex(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			string value = Platform.Get<IClipboard>().Value;
			Vector3 vector;
			if (this.GetHexColor(value, out vector))
			{
				this.ApplyPendingColor(UICharacterCreation.ScaledHslToRgb(vector.X, vector.Y, vector.Z));
				this._currentColorHSL = vector;
				this.UpdateHexText(UICharacterCreation.ScaledHslToRgb(vector.X, vector.Y, vector.Z));
				this.UpdateColorPickers();
			}
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x00593774 File Offset: 0x00591974
		private string GetPlayerTemplateValues()
		{
			string result = JsonConvert.SerializeObject(new Dictionary<string, object>
			{
				{
					"version",
					1
				},
				{
					"hairStyle",
					this._player.hair
				},
				{
					"clothingStyle",
					this._player.skinVariant
				},
				{
					"voiceStyle",
					this._player.voiceVariant
				},
				{
					"voicePitch",
					this._player.voicePitchOffset
				},
				{
					"hairColor",
					UICharacterCreation.GetHexText(this._player.hairColor)
				},
				{
					"eyeColor",
					UICharacterCreation.GetHexText(this._player.eyeColor)
				},
				{
					"skinColor",
					UICharacterCreation.GetHexText(this._player.skinColor)
				},
				{
					"shirtColor",
					UICharacterCreation.GetHexText(this._player.shirtColor)
				},
				{
					"underShirtColor",
					UICharacterCreation.GetHexText(this._player.underShirtColor)
				},
				{
					"pantsColor",
					UICharacterCreation.GetHexText(this._player.pantsColor)
				},
				{
					"shoeColor",
					UICharacterCreation.GetHexText(this._player.shoeColor)
				}
			}, new JsonSerializerSettings
			{
				TypeNameHandling = 4,
				MetadataPropertyHandling = 1,
				Formatting = 1
			});
			PlayerInput.PrettyPrintProfiles(ref result);
			return result;
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x005938E8 File Offset: 0x00591AE8
		private void Click_CopyPlayerTemplate(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			Platform.Get<IClipboard>().Value = this.GetPlayerTemplateValues();
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x00593910 File Offset: 0x00591B10
		private void Click_PastePlayerTemplate(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			try
			{
				string text = Platform.Get<IClipboard>().Value;
				int num = text.IndexOf("{");
				if (num != -1)
				{
					text = text.Substring(num);
					int num2 = text.LastIndexOf("}");
					if (num2 != -1)
					{
						text = text.Substring(0, num2 + 1);
						Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
						if (dictionary != null)
						{
							Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
							foreach (KeyValuePair<string, object> keyValuePair in dictionary)
							{
								dictionary2[keyValuePair.Key.ToLower()] = keyValuePair.Value;
							}
							object obj;
							if (dictionary2.TryGetValue("version", out obj))
							{
								long num3 = (long)obj;
							}
							if (dictionary2.TryGetValue("hairstyle", out obj))
							{
								int num4 = (int)((long)obj);
								if (Main.Hairstyles.AvailableHairstyles.Contains(num4))
								{
									this._player.hair = num4;
									this._lastSelectedHairstyle = new int?(num4);
								}
							}
							if (dictionary2.TryGetValue("clothingstyle", out obj))
							{
								int num5 = (int)((long)obj);
								if (this._validClothStyles.Contains(num5))
								{
									this._player.skinVariant = num5;
								}
							}
							if (dictionary2.TryGetValue("voicestyle", out obj))
							{
								int num6 = (int)((long)obj);
								if (this._validVoiceStyles.Contains(num6))
								{
									this._player.voiceVariant = num6;
								}
							}
							if (dictionary2.TryGetValue("voicepitch", out obj))
							{
								float num7 = (float)((double)obj);
								this._player.voicePitchOffset = num7;
								this._pitchAmount = num7;
							}
							Vector3 hsl;
							if (dictionary2.TryGetValue("haircolor", out obj) && this.GetHexColor((string)obj, out hsl))
							{
								this._player.hairColor = UICharacterCreation.ScaledHslToRgb(hsl);
							}
							if (dictionary2.TryGetValue("eyecolor", out obj) && this.GetHexColor((string)obj, out hsl))
							{
								this._player.eyeColor = UICharacterCreation.ScaledHslToRgb(hsl);
							}
							if (dictionary2.TryGetValue("skincolor", out obj) && this.GetHexColor((string)obj, out hsl))
							{
								this._player.skinColor = UICharacterCreation.ScaledHslToRgb(hsl);
							}
							if (dictionary2.TryGetValue("shirtcolor", out obj) && this.GetHexColor((string)obj, out hsl))
							{
								this._player.shirtColor = UICharacterCreation.ScaledHslToRgb(hsl);
							}
							if (dictionary2.TryGetValue("undershirtcolor", out obj) && this.GetHexColor((string)obj, out hsl))
							{
								this._player.underShirtColor = UICharacterCreation.ScaledHslToRgb(hsl);
							}
							if (dictionary2.TryGetValue("pantscolor", out obj) && this.GetHexColor((string)obj, out hsl))
							{
								this._player.pantsColor = UICharacterCreation.ScaledHslToRgb(hsl);
							}
							if (dictionary2.TryGetValue("shoecolor", out obj) && this.GetHexColor((string)obj, out hsl))
							{
								this._player.shoeColor = UICharacterCreation.ScaledHslToRgb(hsl);
							}
							this.Click_CharClothStyle(null, null);
							this.UpdateColorPickers();
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x00593C78 File Offset: 0x00591E78
		private void Click_VoicePlay(UIMouseEvent evt, UIElement listeningElement)
		{
			this.PlayVoicePreview();
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x00593C80 File Offset: 0x00591E80
		private void PlayVoicePreview()
		{
			if (this._playedVoicePreviewThisFrame)
			{
				return;
			}
			this._playedVoicePreviewThisFrame = true;
			Vector2 position = this._player.position;
			this._player.position = new Vector2(-1f, -1f);
			this._player.PlayHurtSound();
			this._player.position = position;
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x00593CDA File Offset: 0x00591EDA
		private void Click_VoiceCycleBack(UIMouseEvent evt, UIElement listeningElement)
		{
			Main.CycleVoiceStyle(this._player, -1);
			this.PlayVoicePreview();
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x00593CEE File Offset: 0x00591EEE
		private void Click_VoiceCycleForward(UIMouseEvent evt, UIElement listeningElement)
		{
			Main.CycleVoiceStyle(this._player, 1);
			this.PlayVoicePreview();
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x00009E06 File Offset: 0x00008006
		private void Update_VoiceIconColor()
		{
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x00593D04 File Offset: 0x00591F04
		private void Click_RandomizePlayer(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			Player player = this._player;
			int index = Main.rand.Next(Main.Hairstyles.AvailableHairstyles.Count);
			player.hair = Main.Hairstyles.AvailableHairstyles[index];
			this._lastSelectedHairstyle = new int?(player.hair);
			player.eyeColor = UICharacterCreation.ScaledHslToRgb(UICharacterCreation.GetRandomColorVector());
			while ((int)(player.eyeColor.R + player.eyeColor.G + player.eyeColor.B) > 300)
			{
				player.eyeColor = UICharacterCreation.ScaledHslToRgb(UICharacterCreation.GetRandomColorVector());
			}
			float num = (float)Main.rand.Next(60, 120) * 0.01f;
			if (num > 1f)
			{
				num = 1f;
			}
			player.skinColor.R = (byte)((float)Main.rand.Next(240, 255) * num);
			player.skinColor.G = (byte)((float)Main.rand.Next(110, 140) * num);
			player.skinColor.B = (byte)((float)Main.rand.Next(75, 110) * num);
			player.hairColor = UICharacterCreation.ScaledHslToRgb(UICharacterCreation.GetRandomColorVector());
			player.shirtColor = UICharacterCreation.ScaledHslToRgb(UICharacterCreation.GetRandomColorVector());
			player.underShirtColor = UICharacterCreation.ScaledHslToRgb(UICharacterCreation.GetRandomColorVector());
			player.pantsColor = UICharacterCreation.ScaledHslToRgb(UICharacterCreation.GetRandomColorVector());
			player.shoeColor = UICharacterCreation.ScaledHslToRgb(UICharacterCreation.GetRandomColorVector());
			player.skinVariant = this._validClothStyles[Main.rand.Next(this._validClothStyles.Length)];
			player.voiceVariant = (player.Male ? 1 : 2);
			if (Main.rand.Next(2) == 0)
			{
				player.voiceVariant = 3;
			}
			int num2 = player.hair + 1;
			if (num2 <= 135)
			{
				if (num2 <= 124)
				{
					switch (num2)
					{
					case 5:
					case 6:
					case 7:
					case 10:
					case 12:
					case 19:
					case 22:
					case 23:
					case 26:
					case 27:
					case 30:
					case 33:
					case 34:
					case 35:
					case 37:
					case 38:
					case 39:
					case 40:
					case 41:
					case 44:
					case 45:
					case 46:
					case 47:
					case 48:
					case 49:
					case 51:
					case 56:
					case 65:
					case 66:
					case 67:
					case 68:
					case 69:
					case 70:
					case 71:
					case 72:
					case 73:
					case 74:
					case 79:
					case 80:
					case 81:
					case 82:
					case 84:
					case 85:
					case 86:
					case 87:
					case 88:
					case 90:
					case 91:
					case 92:
					case 93:
					case 95:
					case 96:
					case 98:
					case 100:
					case 102:
					case 104:
					case 107:
					case 108:
					case 113:
						break;
					case 8:
					case 9:
					case 11:
					case 13:
					case 14:
					case 15:
					case 16:
					case 17:
					case 18:
					case 20:
					case 21:
					case 24:
					case 25:
					case 28:
					case 29:
					case 31:
					case 32:
					case 36:
					case 42:
					case 43:
					case 50:
					case 52:
					case 53:
					case 54:
					case 55:
					case 57:
					case 58:
					case 59:
					case 60:
					case 61:
					case 62:
					case 63:
					case 64:
					case 75:
					case 76:
					case 77:
					case 78:
					case 83:
					case 89:
					case 94:
					case 97:
					case 99:
					case 101:
					case 103:
					case 105:
					case 106:
					case 109:
					case 110:
					case 111:
					case 112:
						goto IL_3E7;
					default:
						if (num2 != 124)
						{
							goto IL_3E7;
						}
						break;
					}
				}
				else if (num2 != 126 && num2 - 133 > 2)
				{
					goto IL_3E7;
				}
			}
			else if (num2 <= 147)
			{
				if (num2 != 144 && num2 - 146 > 1)
				{
					goto IL_3E7;
				}
			}
			else if (num2 != 163 && num2 != 165)
			{
				goto IL_3E7;
			}
			player.Male = false;
			goto IL_3EE;
			IL_3E7:
			player.Male = true;
			IL_3EE:
			this._femaleArmor = (this._maleArmor = default(UICharacterCreation.ArmorAssignments));
			this.Click_CharClothStyle(null, null);
			this.UpdateSelectedGender();
			this.UpdateColorPickers();
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x00594130 File Offset: 0x00592330
		private void Click_Naming(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			this._player.name = "";
			Main.clrInput();
			UIVirtualKeyboard state = new UIVirtualKeyboard(Lang.menu[45].Value, "", new UIVirtualKeyboard.KeyboardSubmitEvent(this.OnFinishedNaming), new Action(this.OnCanceledNaming), 0, true, 20);
			Main.MenuUI.SetState(state);
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x005941A8 File Offset: 0x005923A8
		private void Click_NamingAndCreating(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			if (string.IsNullOrEmpty(this._player.name))
			{
				this._player.name = "";
				Main.clrInput();
				UIVirtualKeyboard state = new UIVirtualKeyboard(Lang.menu[45].Value, "", new UIVirtualKeyboard.KeyboardSubmitEvent(this.OnFinishedNamingAndCreating), new Action(this.OnCanceledNaming), 0, false, 20);
				Main.MenuUI.SetState(state);
				return;
			}
			this.FinishCreatingCharacter();
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x00594237 File Offset: 0x00592437
		private void OnFinishedNaming(string name)
		{
			this._player.name = name.Trim();
			Main.MenuUI.SetState(this);
			this._charName.SetContents(this._player.name);
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x0059426B File Offset: 0x0059246B
		private void OnCanceledNaming()
		{
			Main.MenuUI.SetState(this);
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x00594278 File Offset: 0x00592478
		private void OnFinishedNamingAndCreating(string name)
		{
			this._player.name = name.Trim();
			Main.MenuUI.SetState(this);
			this._charName.SetContents(this._player.name);
			this.FinishCreatingCharacter();
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x005942B2 File Offset: 0x005924B2
		private void FinishCreatingCharacter()
		{
			this.TryAutoAssigningHair();
			this.SetupPlayerStatsAndInventoryBasedOnDifficulty();
			PlayerFileData.CreateAndSave(this._player);
			Main.LoadPlayers();
			Main.menuMode = 1;
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x005942D8 File Offset: 0x005924D8
		private void SetupPlayerStatsAndInventoryBasedOnDifficulty()
		{
			this._femaleArmor = (this._maleArmor = default(UICharacterCreation.ArmorAssignments));
			this.UpdatePreviewItems();
			int num = 0;
			byte difficulty = this._player.difficulty;
			if (difficulty == 3)
			{
				this._player.statLife = (this._player.statLifeMax = 100);
				this._player.statMana = (this._player.statManaMax = 20);
				this._player.inventory[num].SetDefaults(6, null);
				this._player.inventory[num++].Prefix(-1);
				this._player.inventory[num].SetDefaults(1, null);
				this._player.inventory[num++].Prefix(-1);
				this._player.inventory[num].SetDefaults(10, null);
				this._player.inventory[num++].Prefix(-1);
				this._player.inventory[num].SetDefaults(7, null);
				this._player.inventory[num++].Prefix(-1);
				this._player.inventory[num].SetDefaults(4281, null);
				this._player.inventory[num++].Prefix(-1);
				this._player.inventory[num].SetDefaults(8, null);
				this._player.inventory[num++].stack = 100;
				this._player.inventory[num].SetDefaults(965, null);
				this._player.inventory[num++].stack = 100;
				this._player.inventory[num++].SetDefaults(50, null);
				this._player.inventory[num++].SetDefaults(84, null);
				this._player.armor[3].SetDefaults(4978, null);
				this._player.armor[3].Prefix(-1);
				string a = this._player.name.ToLower();
				if (a == "wolf pet" || a == "wolfpet")
				{
					this._player.miscEquips[3].SetDefaults(5130, null);
				}
				this._player.AddBuff(216, 3600, false);
			}
			else
			{
				this._player.inventory[num].SetDefaults(3507, null);
				this._player.inventory[num++].Prefix(-1);
				this._player.inventory[num].SetDefaults(3509, null);
				this._player.inventory[num++].Prefix(-1);
				this._player.inventory[num].SetDefaults(3506, null);
				this._player.inventory[num++].Prefix(-1);
			}
			if (Main.runningCollectorsEdition)
			{
				this._player.inventory[num++].SetDefaults(603, null);
			}
			this._player.savedPerPlayerFieldsThatArentInThePlayerClass = new Player.SavedPlayerDataWithAnnoyingRules();
			CreativePowerManager.Instance.ResetDataForNewPlayer(this._player);
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x00594620 File Offset: 0x00592820
		private bool GetHexColor(string hexString, out Vector3 hsl)
		{
			if (hexString.StartsWith("#"))
			{
				hexString = hexString.Substring(1);
			}
			uint num;
			if (hexString.Length <= 6 && uint.TryParse(hexString, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out num))
			{
				uint b = num & 255U;
				uint g = num >> 8 & 255U;
				uint r = num >> 16 & 255U;
				hsl = UICharacterCreation.RgbToScaledHsl(new Color((int)r, (int)g, (int)b));
				return true;
			}
			hsl = Vector3.Zero;
			return false;
		}

		// Token: 0x06002C05 RID: 11269 RVA: 0x005946A0 File Offset: 0x005928A0
		private void Click_RandomizeSingleColor(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			Vector3 randomColorVector = UICharacterCreation.GetRandomColorVector();
			this.ApplyPendingColor(UICharacterCreation.ScaledHslToRgb(randomColorVector.X, randomColorVector.Y, randomColorVector.Z));
			this._currentColorHSL = randomColorVector;
			this.UpdateHexText(UICharacterCreation.ScaledHslToRgb(randomColorVector.X, randomColorVector.Y, randomColorVector.Z));
			this.UpdateColorPickers();
		}

		// Token: 0x06002C06 RID: 11270 RVA: 0x0059470F File Offset: 0x0059290F
		private static Vector3 GetRandomColorVector()
		{
			return new Vector3(Main.rand.NextFloat(), Main.rand.NextFloat(), Main.rand.NextFloat());
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x00594734 File Offset: 0x00592934
		private void UnselectAllCategories()
		{
			foreach (UIColoredImageButton uicoloredImageButton in this._colorPickers)
			{
				if (uicoloredImageButton != null)
				{
					uicoloredImageButton.SetSelected(false);
				}
			}
			this._clothingStylesCategoryButton.SetSelected(false);
			this._hairStylesCategoryButton.SetSelected(false);
			this._charInfoCategoryButton.SetSelected(false);
			this._hslContainer.Remove();
			this._hairstylesContainer.Remove();
			this._clothStylesContainer.Remove();
			this._infoContainer.Remove();
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x005947B4 File Offset: 0x005929B4
		private void SelectColorPicker(UICharacterCreation.CategoryId selection)
		{
			this._selectedPicker = selection;
			if (selection == UICharacterCreation.CategoryId.CharInfo)
			{
				this.Click_CharInfo(null, null);
				return;
			}
			if (selection == UICharacterCreation.CategoryId.Clothing)
			{
				this.Click_ClothStyles(null, null);
				return;
			}
			if (selection == UICharacterCreation.CategoryId.HairStyle)
			{
				this.Click_HairStyles(null, null);
				return;
			}
			this.UnselectAllCategories();
			this._middleContainer.Append(this._hslContainer);
			for (int i = 0; i < this._colorPickers.Length; i++)
			{
				if (this._colorPickers[i] != null)
				{
					this._colorPickers[i].SetSelected(i == (int)selection);
				}
			}
			Vector3 vector = Vector3.One;
			switch (this._selectedPicker)
			{
			case UICharacterCreation.CategoryId.HairColor:
				vector = UICharacterCreation.RgbToScaledHsl(this._player.hairColor);
				break;
			case UICharacterCreation.CategoryId.Eye:
				vector = UICharacterCreation.RgbToScaledHsl(this._player.eyeColor);
				break;
			case UICharacterCreation.CategoryId.Skin:
				vector = UICharacterCreation.RgbToScaledHsl(this._player.skinColor);
				break;
			case UICharacterCreation.CategoryId.Shirt:
				vector = UICharacterCreation.RgbToScaledHsl(this._player.shirtColor);
				break;
			case UICharacterCreation.CategoryId.Undershirt:
				vector = UICharacterCreation.RgbToScaledHsl(this._player.underShirtColor);
				break;
			case UICharacterCreation.CategoryId.Pants:
				vector = UICharacterCreation.RgbToScaledHsl(this._player.pantsColor);
				break;
			case UICharacterCreation.CategoryId.Shoes:
				vector = UICharacterCreation.RgbToScaledHsl(this._player.shoeColor);
				break;
			}
			this._currentColorHSL = vector;
			this.UpdateHexText(UICharacterCreation.ScaledHslToRgb(vector.X, vector.Y, vector.Z));
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x00594910 File Offset: 0x00592B10
		private void UpdateColorPickers()
		{
			UICharacterCreation.CategoryId selectedPicker = this._selectedPicker;
			this._colorPickers[3].SetColor(this._player.hairColor);
			this._hairStylesCategoryButton.SetColor(this._player.hairColor);
			this._colorPickers[4].SetColor(this._player.eyeColor);
			this._colorPickers[5].SetColor(this._player.skinColor);
			this._colorPickers[6].SetColor(this._player.shirtColor);
			this._colorPickers[7].SetColor(this._player.underShirtColor);
			this._colorPickers[8].SetColor(this._player.pantsColor);
			this._colorPickers[9].SetColor(this._player.shoeColor);
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x005949E4 File Offset: 0x00592BE4
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			string text = null;
			if (this._copyHexButton.IsMouseHovering)
			{
				text = Language.GetTextValue("UI.CopyColorToClipboard");
			}
			if (this._pasteHexButton.IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PasteColorFromClipboard");
			}
			if (this._randomColorButton.IsMouseHovering)
			{
				text = Language.GetTextValue("UI.RandomizeColor");
			}
			if (this._copyTemplateButton.IsMouseHovering)
			{
				text = Language.GetTextValue("UI.CopyPlayerToClipboard");
			}
			if (this._pasteTemplateButton.IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PastePlayerFromClipboard");
			}
			if (this._randomizePlayerButton.IsMouseHovering)
			{
				text = Language.GetTextValue("UI.RandomizePlayer");
			}
			if (this._previewArmorButton[0].IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PreviewArmorNone");
			}
			if (this._previewArmorButton[1].IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PreviewArmorHallowed");
			}
			if (this._previewArmorButton[2].IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PreviewArmorSilver");
			}
			if (this._previewArmorButton[3].IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PreviewArmorFormal");
			}
			if (this._previewArmorButton[4].IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PreviewArmorSwimming");
			}
			if (UISliderBase.CurrentAimedSlider == this._pitchSlider)
			{
				text = Language.GetTextValue("UI.PlayerCreateVoicePitch");
			}
			if (this._voicePrevious.IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PlayerCreateVoicePrev");
			}
			if (this._voiceNext.IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PlayerCreateVoiceNext");
			}
			if (this._voicePlay.IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PlayerCreateVoicePlay");
			}
			if (this._charInfoCategoryButton.IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PlayerCreateCategoryInfo");
			}
			if (this._clothingStylesCategoryButton.IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PlayerCreateCategoryBodyStyle");
			}
			if (this._hairStylesCategoryButton.IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PlayerCreateCategoryHairStyle");
			}
			if (this._colorPickers[3].IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PlayerCreateCategoryHairColor");
			}
			if (this._colorPickers[4].IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PlayerCreateCategoryEyeColor");
			}
			if (this._colorPickers[5].IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PlayerCreateCategorySkinColor");
			}
			if (this._colorPickers[6].IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PlayerCreateCategoryShirtColor");
			}
			if (this._colorPickers[7].IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PlayerCreateCategoryUndershirtColor");
			}
			if (this._colorPickers[8].IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PlayerCreateCategoryPantsColor");
			}
			if (this._colorPickers[9].IsMouseHovering)
			{
				text = Language.GetTextValue("UI.PlayerCreateCategoryShoesColor");
			}
			if (text != null)
			{
				float x = FontAssets.MouseText.Value.MeasureString(text).X;
				Vector2 vector = new Vector2((float)Main.mouseX, (float)Main.mouseY) + new Vector2(16f);
				if (vector.Y > (float)(Main.screenHeight - 30))
				{
					vector.Y = (float)(Main.screenHeight - 30);
				}
				if (vector.X > (float)Main.screenWidth - x)
				{
					vector.X = (float)(Main.screenWidth - 460);
				}
				Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, text, vector.X, vector.Y, new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor), Color.Black, Vector2.Zero, 1f);
			}
			this.SetupGamepadPoints(spriteBatch);
			this._tips.Update();
			int num = Main.screenHeight - 560;
			if (num < 0)
			{
				num = 0;
			}
			int num2 = 150;
			if (num < 300)
			{
				num2 = num / 2;
			}
			if (num > 30)
			{
				this._tips.TipOffsetY = (float)(-(float)num2);
				this._tips.Draw();
			}
			if (!UICharacterCreation.dirty)
			{
				if (!string.IsNullOrEmpty(this._player.name))
				{
					UICharacterCreation.dirty = true;
				}
				if (this.GetPlayerTemplateValues() != this.initialState)
				{
					UICharacterCreation.dirty = true;
				}
			}
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x00594DC4 File Offset: 0x00592FC4
		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 7;
			int num = 3000;
			int num2 = num + 20;
			int num3 = num;
			List<SnapPoint> snapPoints = this.GetSnapPoints();
			SnapPoint snapPoint = snapPoints.First((SnapPoint a) => a.Name == "Back");
			SnapPoint snapPoint2 = snapPoints.First((SnapPoint a) => a.Name == "Create");
			UILinkPoint uilinkPoint = UILinkPointNavigator.Points[num3];
			uilinkPoint.Unlink();
			UILinkPointNavigator.SetPosition(num3, snapPoint.Position);
			num3++;
			UILinkPoint uilinkPoint2 = UILinkPointNavigator.Points[num3];
			uilinkPoint2.Unlink();
			UILinkPointNavigator.SetPosition(num3, snapPoint2.Position);
			num3++;
			uilinkPoint.Right = uilinkPoint2.ID;
			uilinkPoint2.Left = uilinkPoint.ID;
			this._foundPoints.Clear();
			this._foundPoints.Add(uilinkPoint.ID);
			this._foundPoints.Add(uilinkPoint2.ID);
			List<SnapPoint> list = (from a in snapPoints
			where a.Name == "Top"
			select a).ToList<SnapPoint>();
			list.Sort(new Comparison<SnapPoint>(this.SortPoints));
			for (int i = 0; i < list.Count; i++)
			{
				UILinkPoint uilinkPoint3 = UILinkPointNavigator.Points[num3];
				uilinkPoint3.Unlink();
				UILinkPointNavigator.SetPosition(num3, list[i].Position);
				uilinkPoint3.Left = num3 - 1;
				uilinkPoint3.Right = num3 + 1;
				uilinkPoint3.Down = num2;
				if (i == 0)
				{
					uilinkPoint3.Left = -3;
				}
				if (i == list.Count - 1)
				{
					uilinkPoint3.Right = -4;
				}
				if (this._selectedPicker == UICharacterCreation.CategoryId.HairStyle || this._selectedPicker == UICharacterCreation.CategoryId.Clothing)
				{
					uilinkPoint3.Down = num2 + i;
				}
				this._foundPoints.Add(num3);
				num3++;
			}
			List<SnapPoint> list2 = (from a in snapPoints
			where a.Name == "Middle"
			select a).ToList<SnapPoint>();
			list2.Sort(new Comparison<SnapPoint>(this.SortPoints));
			num3 = num2;
			switch (this._selectedPicker)
			{
			case UICharacterCreation.CategoryId.CharInfo:
				for (int j = 0; j < list2.Count; j++)
				{
					UILinkPoint andSet = this.GetAndSet(num3, list2[j]);
					andSet.Up = andSet.ID - 1;
					andSet.Down = andSet.ID + 1;
					if (j == 0)
					{
						andSet.Up = num + 2;
					}
					if (j == list2.Count - 1)
					{
						andSet.Down = uilinkPoint.ID;
						uilinkPoint.Up = andSet.ID;
						uilinkPoint2.Up = andSet.ID;
					}
					this._foundPoints.Add(num3);
					num3++;
				}
				break;
			case UICharacterCreation.CategoryId.Clothing:
			{
				List<SnapPoint> list3 = (from a in snapPoints
				where a.Name == "Preview"
				select a).ToList<SnapPoint>();
				list3.Sort(new Comparison<SnapPoint>(this.SortPoints));
				List<SnapPoint> list4 = (from a in snapPoints
				where a.Name == "Low"
				select a).ToList<SnapPoint>();
				list4.Sort(new Comparison<SnapPoint>(this.SortPoints));
				int down = -2;
				SnapPoint snap = null;
				UILinkPoint uilinkPoint4 = null;
				if (this._pitchSlider.GetSnapPoint(out snap))
				{
					uilinkPoint4 = this.GetAndSet(num2 + 40, snap);
					this._foundPoints.Add(uilinkPoint4.ID);
				}
				uilinkPoint4.Down = uilinkPoint.ID;
				int num4 = num2 + 20;
				num3 = num2 + 20;
				int num5 = num3 + list4.Count;
				UILinkPoint uilinkPoint5 = null;
				for (int k = 0; k < list4.Count; k++)
				{
					UILinkPoint andSet2 = this.GetAndSet(num3, list4[k]);
					andSet2.Up = num2 + k + 2;
					andSet2.Down = uilinkPoint4.ID;
					if (k >= 3)
					{
						andSet2.Up = num5 + (k - 3) + 1;
						andSet2.Down = uilinkPoint2.ID;
					}
					andSet2.Left = andSet2.ID - 1;
					andSet2.Right = andSet2.ID + 1;
					if (k == 0)
					{
						down = andSet2.ID;
						andSet2.Left = andSet2.ID + 5;
						uilinkPoint.Up = andSet2.ID;
					}
					if (k == list4.Count - 1)
					{
						int id = andSet2.ID;
						andSet2.Right = andSet2.ID - 5;
						uilinkPoint2.Up = andSet2.ID;
					}
					if (k == 1)
					{
						uilinkPoint5 = andSet2;
					}
					this._foundPoints.Add(num3);
					num3++;
				}
				for (int l = 0; l < list3.Count; l++)
				{
					UILinkPoint andSet3 = this.GetAndSet(num3, list3[l]);
					andSet3.Up = num2 + l + 5;
					andSet3.Down = num4 + ((int)MathHelper.Clamp((float)l, 1f, 4f) - 1) + 3;
					andSet3.Left = andSet3.ID - 1;
					andSet3.Right = andSet3.ID + 1;
					if (l == 0)
					{
						andSet3.Left = num4 + 2;
					}
					if (l == list3.Count - 1)
					{
						andSet3.Right = num4;
					}
					this._foundPoints.Add(num3);
					num3++;
				}
				if (list4.Count > 1)
				{
					uilinkPoint4.Up = uilinkPoint5.ID;
				}
				uilinkPoint.Up = uilinkPoint4.ID;
				num3 = num2;
				for (int m = 0; m < list2.Count; m++)
				{
					UILinkPoint andSet4 = this.GetAndSet(num3, list2[m]);
					andSet4.Up = num + 2 + m;
					andSet4.Left = andSet4.ID - 1;
					andSet4.Right = andSet4.ID + 1;
					if (m == 0)
					{
						andSet4.Left = andSet4.ID + 9;
					}
					if (m == list2.Count - 1)
					{
						andSet4.Right = andSet4.ID - 9;
					}
					andSet4.Down = down;
					if (m >= 5)
					{
						andSet4.Down = num5 + m - 5;
					}
					this._foundPoints.Add(num3);
					num3++;
				}
				break;
			}
			case UICharacterCreation.CategoryId.HairStyle:
				if (list2.Count != 0)
				{
					this._helper.CullPointsOutOfElementArea(spriteBatch, list2, this._hairstylesContainer);
					SnapPoint snapPoint3 = list2[list2.Count - 1];
					int num6 = snapPoint3.Id / 10;
					int num7 = snapPoint3.Id % 10;
					int count = Main.Hairstyles.AvailableHairstyles.Count;
					for (int n = 0; n < list2.Count; n++)
					{
						SnapPoint snapPoint4 = list2[n];
						UILinkPoint andSet5 = this.GetAndSet(num3, snapPoint4);
						andSet5.Left = andSet5.ID - 1;
						if (snapPoint4.Id == 0)
						{
							andSet5.Left = -3;
						}
						andSet5.Right = andSet5.ID + 1;
						if (snapPoint4.Id == count - 1)
						{
							andSet5.Right = -4;
						}
						andSet5.Up = andSet5.ID - 10;
						if (n < 10)
						{
							andSet5.Up = num + 2 + n;
						}
						andSet5.Down = andSet5.ID + 10;
						if (snapPoint4.Id + 10 > snapPoint3.Id)
						{
							if (snapPoint4.Id % 10 < 5)
							{
								andSet5.Down = uilinkPoint.ID;
							}
							else
							{
								andSet5.Down = uilinkPoint2.ID;
							}
						}
						if (n == list2.Count - 1)
						{
							uilinkPoint.Up = andSet5.ID;
							uilinkPoint2.Up = andSet5.ID;
						}
						this._foundPoints.Add(num3);
						num3++;
					}
				}
				break;
			default:
			{
				List<SnapPoint> list5 = (from a in snapPoints
				where a.Name == "Low"
				select a).ToList<SnapPoint>();
				list5.Sort(new Comparison<SnapPoint>(this.SortPoints));
				num3 = num2 + 20;
				for (int num8 = 0; num8 < list5.Count; num8++)
				{
					UILinkPoint andSet6 = this.GetAndSet(num3, list5[num8]);
					andSet6.Up = num2 + 2;
					andSet6.Down = uilinkPoint.ID;
					andSet6.Left = andSet6.ID - 1;
					andSet6.Right = andSet6.ID + 1;
					if (num8 == 0)
					{
						andSet6.Left = andSet6.ID + 2;
						uilinkPoint.Up = andSet6.ID;
					}
					if (num8 == list5.Count - 1)
					{
						andSet6.Right = andSet6.ID - 2;
						uilinkPoint2.Up = andSet6.ID;
					}
					this._foundPoints.Add(num3);
					num3++;
				}
				num3 = num2;
				for (int num9 = 0; num9 < list2.Count; num9++)
				{
					UILinkPoint andSet7 = this.GetAndSet(num3, list2[num9]);
					andSet7.Up = andSet7.ID - 1;
					andSet7.Down = andSet7.ID + 1;
					if (num9 == 0)
					{
						andSet7.Up = num + 2 + 5;
					}
					if (num9 == list2.Count - 1)
					{
						andSet7.Down = num2 + 20 + 2;
					}
					this._foundPoints.Add(num3);
					num3++;
				}
				break;
			}
			}
			if (PlayerInput.UsingGamepadUI && !this._foundPoints.Contains(UILinkPointNavigator.CurrentPoint))
			{
				this.MoveToVisuallyClosestPoint();
			}
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x00595770 File Offset: 0x00593970
		private void MoveToVisuallyClosestPoint()
		{
			Dictionary<int, UILinkPoint> points = UILinkPointNavigator.Points;
			Vector2 mouseScreen = Main.MouseScreen;
			UILinkPoint uilinkPoint = null;
			foreach (int key in this._foundPoints)
			{
				UILinkPoint uilinkPoint2;
				if (!points.TryGetValue(key, out uilinkPoint2))
				{
					return;
				}
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

		// Token: 0x06002C0D RID: 11277 RVA: 0x00595808 File Offset: 0x00593A08
		public void TryMovingCategory(int direction)
		{
			int num = (int)((this._selectedPicker + direction) % UICharacterCreation.CategoryId.Count);
			if (num < 0)
			{
				num += 10;
			}
			this.SelectColorPicker((UICharacterCreation.CategoryId)num);
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x00595831 File Offset: 0x00593A31
		private UILinkPoint GetAndSet(int ptid, SnapPoint snap)
		{
			UILinkPoint uilinkPoint = UILinkPointNavigator.Points[ptid];
			uilinkPoint.Unlink();
			UILinkPointNavigator.SetPosition(uilinkPoint.ID, snap.Position);
			return uilinkPoint;
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x00595855 File Offset: 0x00593A55
		private bool PointWithName(SnapPoint a, string comp)
		{
			return a.Name == comp;
		}

		// Token: 0x06002C10 RID: 11280 RVA: 0x00595864 File Offset: 0x00593A64
		private int SortPoints(SnapPoint a, SnapPoint b)
		{
			return a.Id.CompareTo(b.Id);
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x00595885 File Offset: 0x00593A85
		private static Color ScaledHslToRgb(Vector3 hsl)
		{
			return UICharacterCreation.ScaledHslToRgb(hsl.X, hsl.Y, hsl.Z);
		}

		// Token: 0x06002C12 RID: 11282 RVA: 0x0059589E File Offset: 0x00593A9E
		private static Color ScaledHslToRgb(float hue, float saturation, float luminosity)
		{
			return Main.hslToRgb(hue, saturation, luminosity * 0.85f + 0.15f, byte.MaxValue);
		}

		// Token: 0x06002C13 RID: 11283 RVA: 0x005958BC File Offset: 0x00593ABC
		private static Vector3 RgbToScaledHsl(Color color)
		{
			Vector3 vector = Main.rgbToHsl(color);
			vector.Z = (vector.Z - 0.15f) / 0.85f;
			vector = Vector3.Clamp(vector, Vector3.Zero, Vector3.One);
			return vector;
		}

		// Token: 0x06002C14 RID: 11284 RVA: 0x005958FC File Offset: 0x00593AFC
		public void HandleBackButtonUsage()
		{
			if (this._selectedPicker != UICharacterCreation.CategoryId.CharInfo)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				this.UnselectAllCategories();
				this._selectedPicker = UICharacterCreation.CategoryId.CharInfo;
				this._middleContainer.Append(this._infoContainer);
				this._charInfoCategoryButton.SetSelected(true);
				return;
			}
			UICharacterCreation.GoBack();
		}

		// Token: 0x04005387 RID: 21383
		private int[] _validClothStyles = new int[]
		{
			0,
			2,
			1,
			3,
			8,
			9,
			7,
			5,
			6,
			4
		};

		// Token: 0x04005388 RID: 21384
		private Dictionary<int, int> _defaultHairstylesForClothStyle = new Dictionary<int, int>
		{
			{
				0,
				0
			},
			{
				2,
				1
			},
			{
				1,
				12
			},
			{
				3,
				2
			},
			{
				8,
				28
			},
			{
				9,
				68
			},
			{
				7,
				18
			},
			{
				5,
				22
			},
			{
				6,
				81
			},
			{
				4,
				5
			}
		};

		// Token: 0x04005389 RID: 21385
		private int[] _validVoiceStyles = new int[]
		{
			1,
			2,
			3
		};

		// Token: 0x0400538A RID: 21386
		private readonly Player _player;

		// Token: 0x0400538B RID: 21387
		private UIColoredImageButton[] _colorPickers;

		// Token: 0x0400538C RID: 21388
		private UICharacterCreation.CategoryId _selectedPicker;

		// Token: 0x0400538D RID: 21389
		private Vector3 _currentColorHSL;

		// Token: 0x0400538E RID: 21390
		private UIColoredImageButton _clothingStylesCategoryButton;

		// Token: 0x0400538F RID: 21391
		private UIColoredImageButton _hairStylesCategoryButton;

		// Token: 0x04005390 RID: 21392
		private UIColoredImageButton _charInfoCategoryButton;

		// Token: 0x04005391 RID: 21393
		private UIElement _topContainer;

		// Token: 0x04005392 RID: 21394
		private UIElement _middleContainer;

		// Token: 0x04005393 RID: 21395
		private UIElement _hslContainer;

		// Token: 0x04005394 RID: 21396
		private UIElement _hairstylesContainer;

		// Token: 0x04005395 RID: 21397
		private UIElement _clothStylesContainer;

		// Token: 0x04005396 RID: 21398
		private UIElement _infoContainer;

		// Token: 0x04005397 RID: 21399
		private UIText _hslHexText;

		// Token: 0x04005398 RID: 21400
		private UIText _difficultyDescriptionText;

		// Token: 0x04005399 RID: 21401
		private UIElement _copyHexButton;

		// Token: 0x0400539A RID: 21402
		private UIElement _pasteHexButton;

		// Token: 0x0400539B RID: 21403
		private UIElement _randomColorButton;

		// Token: 0x0400539C RID: 21404
		private UIElement _copyTemplateButton;

		// Token: 0x0400539D RID: 21405
		private UIElement _pasteTemplateButton;

		// Token: 0x0400539E RID: 21406
		private UIElement _randomizePlayerButton;

		// Token: 0x0400539F RID: 21407
		private UIElement _pitchSlider;

		// Token: 0x040053A0 RID: 21408
		private UIElement _voiceNext;

		// Token: 0x040053A1 RID: 21409
		private UIElement _voicePrevious;

		// Token: 0x040053A2 RID: 21410
		private UIElement _voicePlay;

		// Token: 0x040053A3 RID: 21411
		private float _pitchAmount;

		// Token: 0x040053A4 RID: 21412
		private UIElement[] _previewArmorButton = new UIElement[0];

		// Token: 0x040053A5 RID: 21413
		private UICharacterNameButton _charName;

		// Token: 0x040053A6 RID: 21414
		private UIText _helpGlyphLeft;

		// Token: 0x040053A7 RID: 21415
		private UIText _helpGlyphRight;

		// Token: 0x040053A8 RID: 21416
		private bool _oldMaleForVoiceAutoSwitch = true;

		// Token: 0x040053A9 RID: 21417
		private int? _lastSelectedHairstyle;

		// Token: 0x040053AA RID: 21418
		private UIImageFramed[] _characterPreviewLayers = new UIImageFramed[7];

		// Token: 0x040053AB RID: 21419
		public const int MAX_NAME_LENGTH = 20;

		// Token: 0x040053AC RID: 21420
		private bool _playedVoicePreviewThisFrame;

		// Token: 0x040053AD RID: 21421
		private UICharacterCreation.ArmorAssignments _maleArmor;

		// Token: 0x040053AE RID: 21422
		private UICharacterCreation.ArmorAssignments _femaleArmor;

		// Token: 0x040053AF RID: 21423
		private GameTipsDisplay _tips;

		// Token: 0x040053B0 RID: 21424
		public static UIState BackupConfirmationState;

		// Token: 0x040053B1 RID: 21425
		private static bool dirty;

		// Token: 0x040053B2 RID: 21426
		private string initialState;

		// Token: 0x040053B3 RID: 21427
		private bool _pitchChanged;

		// Token: 0x040053B4 RID: 21428
		private int _pitchChangedCooldown;

		// Token: 0x040053B5 RID: 21429
		private UIGamepadHelper _helper;

		// Token: 0x040053B6 RID: 21430
		private List<int> _foundPoints = new List<int>();

		// Token: 0x0200090A RID: 2314
		private enum CategoryId
		{
			// Token: 0x04007408 RID: 29704
			CharInfo,
			// Token: 0x04007409 RID: 29705
			Clothing,
			// Token: 0x0400740A RID: 29706
			HairStyle,
			// Token: 0x0400740B RID: 29707
			HairColor,
			// Token: 0x0400740C RID: 29708
			Eye,
			// Token: 0x0400740D RID: 29709
			Skin,
			// Token: 0x0400740E RID: 29710
			Shirt,
			// Token: 0x0400740F RID: 29711
			Undershirt,
			// Token: 0x04007410 RID: 29712
			Pants,
			// Token: 0x04007411 RID: 29713
			Shoes,
			// Token: 0x04007412 RID: 29714
			Count
		}

		// Token: 0x0200090B RID: 2315
		private enum HSLSliderId
		{
			// Token: 0x04007414 RID: 29716
			Hue,
			// Token: 0x04007415 RID: 29717
			Saturation,
			// Token: 0x04007416 RID: 29718
			Luminance
		}

		// Token: 0x0200090C RID: 2316
		private struct ArmorAssignments
		{
			// Token: 0x04007417 RID: 29719
			public int HeadItem;

			// Token: 0x04007418 RID: 29720
			public int BodyItem;

			// Token: 0x04007419 RID: 29721
			public int LegItem;

			// Token: 0x0400741A RID: 29722
			public int Accessory1Item;
		}
	}
}
