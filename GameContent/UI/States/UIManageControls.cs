using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Chat;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003B2 RID: 946
	public class UIManageControls : UIState
	{
		// Token: 0x06002C46 RID: 11334 RVA: 0x0059746C File Offset: 0x0059566C
		public override void OnInitialize()
		{
			this._KeyboardGamepadTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Inputs", 1);
			this._keyboardGamepadBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Inputs_Border", 1);
			this._GameplayVsUITexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Inputs_2", 1);
			this._GameplayVsUIBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Inputs_2_Border", 1);
			UIElement uielement = new UIElement();
			uielement.Width.Set(0f, 0.8f);
			uielement.MaxWidth.Set(600f, 0f);
			uielement.Top.Set(220f, 0f);
			uielement.Height.Set(-200f, 1f);
			uielement.HAlign = 0.5f;
			this._outerContainer = uielement;
			UIPanel uipanel = new UIPanel();
			uipanel.Width.Set(0f, 1f);
			uipanel.Height.Set(-110f, 1f);
			uipanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
			uielement.Append(uipanel);
			this._buttonKeyboard = new UIImageFramed(this._KeyboardGamepadTexture, this._KeyboardGamepadTexture.Frame(2, 2, 0, 0, 0, 0));
			this._buttonKeyboard.VAlign = 0f;
			this._buttonKeyboard.HAlign = 0f;
			this._buttonKeyboard.Left.Set(0f, 0f);
			this._buttonKeyboard.Top.Set(8f, 0f);
			this._buttonKeyboard.OnLeftClick += this.KeyboardButtonClick;
			this._buttonKeyboard.OnMouseOver += this.ManageBorderKeyboardOn;
			this._buttonKeyboard.OnMouseOut += this.ManageBorderKeyboardOff;
			uipanel.Append(this._buttonKeyboard);
			this._buttonGamepad = new UIImageFramed(this._KeyboardGamepadTexture, this._KeyboardGamepadTexture.Frame(2, 2, 1, 1, 0, 0));
			this._buttonGamepad.VAlign = 0f;
			this._buttonGamepad.HAlign = 0f;
			this._buttonGamepad.Left.Set(76f, 0f);
			this._buttonGamepad.Top.Set(8f, 0f);
			this._buttonGamepad.OnLeftClick += this.GamepadButtonClick;
			this._buttonGamepad.OnMouseOver += this.ManageBorderGamepadOn;
			this._buttonGamepad.OnMouseOut += this.ManageBorderGamepadOff;
			uipanel.Append(this._buttonGamepad);
			this._buttonBorder1 = new UIImageFramed(this._keyboardGamepadBorderTexture, this._keyboardGamepadBorderTexture.Frame(1, 1, 0, 0, 0, 0));
			this._buttonBorder1.VAlign = 0f;
			this._buttonBorder1.HAlign = 0f;
			this._buttonBorder1.Left.Set(0f, 0f);
			this._buttonBorder1.Top.Set(8f, 0f);
			this._buttonBorder1.Color = Color.Silver;
			this._buttonBorder1.IgnoresMouseInteraction = true;
			uipanel.Append(this._buttonBorder1);
			this._buttonBorder2 = new UIImageFramed(this._keyboardGamepadBorderTexture, this._keyboardGamepadBorderTexture.Frame(1, 1, 0, 0, 0, 0));
			this._buttonBorder2.VAlign = 0f;
			this._buttonBorder2.HAlign = 0f;
			this._buttonBorder2.Left.Set(76f, 0f);
			this._buttonBorder2.Top.Set(8f, 0f);
			this._buttonBorder2.Color = Color.Transparent;
			this._buttonBorder2.IgnoresMouseInteraction = true;
			uipanel.Append(this._buttonBorder2);
			this._buttonVs1 = new UIImageFramed(this._GameplayVsUITexture, this._GameplayVsUITexture.Frame(2, 2, 0, 0, 0, 0));
			this._buttonVs1.VAlign = 0f;
			this._buttonVs1.HAlign = 0f;
			this._buttonVs1.Left.Set(172f, 0f);
			this._buttonVs1.Top.Set(8f, 0f);
			this._buttonVs1.OnLeftClick += this.VsGameplayButtonClick;
			this._buttonVs1.OnMouseOver += this.ManageBorderGameplayOn;
			this._buttonVs1.OnMouseOut += this.ManageBorderGameplayOff;
			uipanel.Append(this._buttonVs1);
			this._buttonVs2 = new UIImageFramed(this._GameplayVsUITexture, this._GameplayVsUITexture.Frame(2, 2, 1, 1, 0, 0));
			this._buttonVs2.VAlign = 0f;
			this._buttonVs2.HAlign = 0f;
			this._buttonVs2.Left.Set(212f, 0f);
			this._buttonVs2.Top.Set(8f, 0f);
			this._buttonVs2.OnLeftClick += this.VsMenuButtonClick;
			this._buttonVs2.OnMouseOver += this.ManageBorderMenuOn;
			this._buttonVs2.OnMouseOut += this.ManageBorderMenuOff;
			uipanel.Append(this._buttonVs2);
			this._buttonBorderVs1 = new UIImageFramed(this._GameplayVsUIBorderTexture, this._GameplayVsUIBorderTexture.Frame(1, 1, 0, 0, 0, 0));
			this._buttonBorderVs1.VAlign = 0f;
			this._buttonBorderVs1.HAlign = 0f;
			this._buttonBorderVs1.Left.Set(172f, 0f);
			this._buttonBorderVs1.Top.Set(8f, 0f);
			this._buttonBorderVs1.Color = Color.Silver;
			this._buttonBorderVs1.IgnoresMouseInteraction = true;
			uipanel.Append(this._buttonBorderVs1);
			this._buttonBorderVs2 = new UIImageFramed(this._GameplayVsUIBorderTexture, this._GameplayVsUIBorderTexture.Frame(1, 1, 0, 0, 0, 0));
			this._buttonBorderVs2.VAlign = 0f;
			this._buttonBorderVs2.HAlign = 0f;
			this._buttonBorderVs2.Left.Set(212f, 0f);
			this._buttonBorderVs2.Top.Set(8f, 0f);
			this._buttonBorderVs2.Color = Color.Transparent;
			this._buttonBorderVs2.IgnoresMouseInteraction = true;
			uipanel.Append(this._buttonBorderVs2);
			this._buttonStyle = new UIKeybindingSimpleListItem(() => this.controllerGlyphStyle(), new Color(73, 94, 171, 255) * 0.9f);
			this._buttonStyle.VAlign = 0f;
			this._buttonStyle.HAlign = 1f;
			this._buttonStyle.Width.Set(100f, 0f);
			this._buttonStyle.Height.Set(30f, 0f);
			this._buttonStyle.MarginRight = 220f;
			this._buttonStyle.Left.Set(0f, 0f);
			this._buttonStyle.Top.Set(8f, 0f);
			this._buttonStyle.OnLeftClick += this.controllerGlyphButtonClick;
			uipanel.Append(this._buttonStyle);
			this._buttonProfile = new UIKeybindingSimpleListItem(() => PlayerInput.CurrentProfile.ShowName, new Color(73, 94, 171, 255) * 0.9f);
			this._buttonProfile.VAlign = 0f;
			this._buttonProfile.HAlign = 1f;
			this._buttonProfile.Width.Set(180f, 0f);
			this._buttonProfile.Height.Set(30f, 0f);
			this._buttonProfile.MarginRight = 30f;
			this._buttonProfile.Left.Set(0f, 0f);
			this._buttonProfile.Top.Set(8f, 0f);
			this._buttonProfile.OnLeftClick += this.profileButtonClick;
			uipanel.Append(this._buttonProfile);
			this._uilist = new UIList();
			this._uilist.Width.Set(-25f, 1f);
			this._uilist.Height.Set(-50f, 1f);
			this._uilist.VAlign = 1f;
			this._uilist.PaddingBottom = 5f;
			this._uilist.ListPadding = 20f;
			uipanel.Append(this._uilist);
			this.AssembleBindPanels();
			this.FillList();
			UIScrollbar uiscrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Blue);
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(-67f, 1f);
			uiscrollbar.HAlign = 1f;
			uiscrollbar.VAlign = 1f;
			uiscrollbar.MarginBottom = 11f;
			uipanel.Append(uiscrollbar);
			this._uilist.SetScrollbar(uiscrollbar);
			UITextPanel<LocalizedText> uitextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Keybindings"), 0.7f, true);
			uitextPanel.HAlign = 0.5f;
			uitextPanel.Top.Set(-45f, 0f);
			uitextPanel.Left.Set(-10f, 0f);
			uitextPanel.SetPadding(15f);
			uitextPanel.BackgroundColor = new Color(73, 94, 171);
			uielement.Append(uitextPanel);
			UITextPanel<LocalizedText> uitextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, true);
			uitextPanel2.Width.Set(-10f, 0.5f);
			uitextPanel2.Height.Set(50f, 0f);
			uitextPanel2.VAlign = 1f;
			uitextPanel2.HAlign = 0.5f;
			uitextPanel2.Top.Set(-45f, 0f);
			uitextPanel2.OnMouseOver += this.FadedMouseOver;
			uitextPanel2.OnMouseOut += this.FadedMouseOut;
			uitextPanel2.OnLeftClick += this.GoBackClick;
			uielement.Append(uitextPanel2);
			this._buttonBack = uitextPanel2;
			base.Append(uielement);
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x00597F30 File Offset: 0x00596130
		private void AssembleBindPanels()
		{
			List<string> bindings = new List<string>
			{
				"MouseLeft",
				"MouseRight",
				"Up",
				"Down",
				"Left",
				"Right",
				"Jump",
				"Grapple",
				"SmartSelect",
				"SmartCursor",
				"QuickMount",
				"QuickHeal",
				"QuickMana",
				"QuickBuff",
				"Throw",
				"Inventory",
				"ToggleCreativeMenu",
				"ViewZoomIn",
				"ViewZoomOut",
				"Loadout1",
				"Loadout2",
				"Loadout3",
				"NextLoadout",
				"PreviousLoadout",
				"ToggleCameraMode",
				"ArmorSetAbility",
				"Dash",
				"sp20",
				"sp9"
			};
			List<string> bindings2 = new List<string>
			{
				"MouseLeft",
				"MouseRight",
				"Up",
				"Down",
				"Left",
				"Right",
				"Jump",
				"Grapple",
				"SmartSelect",
				"SmartCursor",
				"QuickMount",
				"QuickHeal",
				"QuickMana",
				"QuickBuff",
				"LockOn",
				"Throw",
				"Inventory",
				"Loadout1",
				"Loadout2",
				"Loadout3",
				"NextLoadout",
				"PreviousLoadout",
				"ToggleCameraMode",
				"ArmorSetAbility",
				"Dash",
				"sp20",
				"sp9"
			};
			List<string> bindings3 = new List<string>
			{
				"HotbarMinus",
				"HotbarPlus",
				"Hotbar1",
				"Hotbar2",
				"Hotbar3",
				"Hotbar4",
				"Hotbar5",
				"Hotbar6",
				"Hotbar7",
				"Hotbar8",
				"Hotbar9",
				"Hotbar10",
				"sp10"
			};
			List<string> bindings4 = new List<string>
			{
				"MapZoomIn",
				"MapZoomOut",
				"MapAlphaUp",
				"MapAlphaDown",
				"MapFull",
				"MapStyle",
				"sp11"
			};
			List<string> bindings5 = new List<string>
			{
				"sp1",
				"sp2",
				"RadialHotbar",
				"RadialQuickbar",
				"sp12"
			};
			List<string> bindings6 = new List<string>
			{
				"sp3",
				"sp4",
				"sp5",
				"sp6",
				"sp7",
				"sp8",
				"sp14",
				"sp15",
				"sp16",
				"sp17",
				"sp18",
				"sp19",
				"sp13"
			};
			InputMode currentInputMode = InputMode.Keyboard;
			this._bindsKeyboard.Add(this.CreateBindingGroup(0, bindings, currentInputMode));
			this._bindsKeyboard.Add(this.CreateBindingGroup(1, bindings4, currentInputMode));
			this._bindsKeyboard.Add(this.CreateBindingGroup(2, bindings3, currentInputMode));
			currentInputMode = InputMode.XBoxGamepad;
			this._bindsGamepad.Add(this.CreateBindingGroup(0, bindings2, currentInputMode));
			this._bindsGamepad.Add(this.CreateBindingGroup(1, bindings4, currentInputMode));
			this._bindsGamepad.Add(this.CreateBindingGroup(2, bindings3, currentInputMode));
			this._bindsGamepad.Add(this.CreateBindingGroup(3, bindings5, currentInputMode));
			this._bindsGamepad.Add(this.CreateBindingGroup(4, bindings6, currentInputMode));
			currentInputMode = InputMode.KeyboardUI;
			this._bindsKeyboardUI.Add(this.CreateBindingGroup(0, bindings, currentInputMode));
			this._bindsKeyboardUI.Add(this.CreateBindingGroup(1, bindings4, currentInputMode));
			this._bindsKeyboardUI.Add(this.CreateBindingGroup(2, bindings3, currentInputMode));
			currentInputMode = InputMode.XBoxGamepadUI;
			this._bindsGamepadUI.Add(this.CreateBindingGroup(0, bindings2, currentInputMode));
			this._bindsGamepadUI.Add(this.CreateBindingGroup(1, bindings4, currentInputMode));
			this._bindsGamepadUI.Add(this.CreateBindingGroup(2, bindings3, currentInputMode));
			this._bindsGamepadUI.Add(this.CreateBindingGroup(3, bindings5, currentInputMode));
			this._bindsGamepadUI.Add(this.CreateBindingGroup(4, bindings6, currentInputMode));
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x005984D0 File Offset: 0x005966D0
		private UISortableElement CreateBindingGroup(int elementIndex, List<string> bindings, InputMode currentInputMode)
		{
			UISortableElement uisortableElement = new UISortableElement(elementIndex);
			uisortableElement.HAlign = 0.5f;
			uisortableElement.Width.Set(0f, 1f);
			uisortableElement.Height.Set(2000f, 0f);
			UIPanel uipanel = new UIPanel();
			uipanel.Width.Set(0f, 1f);
			uipanel.Height.Set(-16f, 1f);
			uipanel.VAlign = 1f;
			uipanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
			uisortableElement.Append(uipanel);
			UIList uilist = new UIList();
			uilist.OverflowHidden = false;
			uilist.Width.Set(0f, 1f);
			uilist.Height.Set(-8f, 1f);
			uilist.VAlign = 1f;
			uilist.ListPadding = 5f;
			uipanel.Append(uilist);
			Color backgroundColor = uipanel.BackgroundColor;
			switch (elementIndex)
			{
			case 0:
				uipanel.BackgroundColor = Color.Lerp(uipanel.BackgroundColor, Color.Green, 0.18f);
				break;
			case 1:
				uipanel.BackgroundColor = Color.Lerp(uipanel.BackgroundColor, Color.Goldenrod, 0.18f);
				break;
			case 2:
				uipanel.BackgroundColor = Color.Lerp(uipanel.BackgroundColor, Color.HotPink, 0.18f);
				break;
			case 3:
				uipanel.BackgroundColor = Color.Lerp(uipanel.BackgroundColor, Color.Indigo, 0.18f);
				break;
			case 4:
				uipanel.BackgroundColor = Color.Lerp(uipanel.BackgroundColor, Color.Turquoise, 0.18f);
				break;
			}
			this.CreateElementGroup(uilist, bindings, currentInputMode, uipanel.BackgroundColor);
			uipanel.BackgroundColor = uipanel.BackgroundColor.MultiplyRGBA(new Color(111, 111, 111));
			LocalizedText text = LocalizedText.Empty;
			switch (elementIndex)
			{
			case 0:
				text = ((currentInputMode == InputMode.Keyboard || currentInputMode == InputMode.XBoxGamepad) ? Lang.menu[164] : Lang.menu[243]);
				break;
			case 1:
				text = Lang.menu[165];
				break;
			case 2:
				text = Lang.menu[166];
				break;
			case 3:
				text = Lang.menu[167];
				break;
			case 4:
				text = Lang.menu[198];
				break;
			}
			UITextPanel<LocalizedText> element = new UITextPanel<LocalizedText>(text, 0.7f, false)
			{
				VAlign = 0f,
				HAlign = 0.5f
			};
			uisortableElement.Append(element);
			uisortableElement.Recalculate();
			float totalHeight = uilist.GetTotalHeight();
			uisortableElement.Width.Set(0f, 1f);
			uisortableElement.Height.Set(totalHeight + 30f + 16f, 0f);
			return uisortableElement;
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x00598794 File Offset: 0x00596994
		private void CreateElementGroup(UIList parent, List<string> bindings, InputMode currentInputMode, Color color)
		{
			for (int i = 0; i < bindings.Count; i++)
			{
				string text = bindings[i];
				UISortableElement uisortableElement = new UISortableElement(i);
				uisortableElement.Width.Set(0f, 1f);
				uisortableElement.Height.Set(30f, 0f);
				uisortableElement.HAlign = 0.5f;
				parent.Add(uisortableElement);
				if (UIManageControls._BindingsHalfSingleLine.Contains(bindings[i]))
				{
					UIElement uielement = this.CreatePanel(bindings[i], currentInputMode, color);
					uielement.Width.Set(0f, 0.5f);
					uielement.HAlign = 0.5f;
					uielement.Height.Set(0f, 1f);
					uielement.SetSnapPoint("Wide", UIManageControls.SnapPointIndex++, null, null);
					uisortableElement.Append(uielement);
				}
				else if (UIManageControls._BindingsFullLine.Contains(bindings[i]))
				{
					UIElement uielement2 = this.CreatePanel(bindings[i], currentInputMode, color);
					uielement2.Width.Set(0f, 1f);
					uielement2.Height.Set(0f, 1f);
					uielement2.SetSnapPoint("Wide", UIManageControls.SnapPointIndex++, null, null);
					uisortableElement.Append(uielement2);
				}
				else
				{
					UIElement uielement3 = this.CreatePanel(bindings[i], currentInputMode, color);
					uielement3.Width.Set(-5f, 0.5f);
					uielement3.Height.Set(0f, 1f);
					uielement3.SetSnapPoint("Thin", UIManageControls.SnapPointIndex++, null, null);
					uisortableElement.Append(uielement3);
					i++;
					if (i < bindings.Count)
					{
						uielement3 = this.CreatePanel(bindings[i], currentInputMode, color);
						uielement3.Width.Set(-5f, 0.5f);
						uielement3.Height.Set(0f, 1f);
						uielement3.HAlign = 1f;
						uielement3.SetSnapPoint("Thin", UIManageControls.SnapPointIndex++, null, null);
						uisortableElement.Append(uielement3);
					}
				}
			}
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x00598A18 File Offset: 0x00596C18
		public UIElement CreatePanel(string bind, InputMode currentInputMode, Color color)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(bind);
			if (num <= 1316519504U)
			{
				if (num <= 172078476U)
				{
					if (num <= 121745619U)
					{
						if (num != 104968000U)
						{
							if (num == 121745619U)
							{
								if (bind == "sp7")
								{
									return new UIKeybindingSliderItem(() => Lang.menu[203].Value + " (" + PlayerInput.CurrentProfile.RightThumbstickDeadzoneX.ToString("P1") + ")", () => PlayerInput.CurrentProfile.RightThumbstickDeadzoneX, delegate(float f)
									{
										PlayerInput.CurrentProfile.RightThumbstickDeadzoneX = f;
									}, delegate()
									{
										PlayerInput.CurrentProfile.RightThumbstickDeadzoneX = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.RightThumbstickDeadzoneX, 0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
									}, 1004, color);
								}
							}
						}
						else if (bind == "sp6")
						{
							return new UIKeybindingSliderItem(() => Lang.menu[202].Value + " (" + PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY.ToString("P1") + ")", () => PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY, delegate(float f)
							{
								PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY = f;
							}, delegate()
							{
								PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY, 0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
							}, 1003, color);
						}
					}
					else if (num != 138523238U)
					{
						if (num != 155300857U)
						{
							if (num == 172078476U)
							{
								if (bind == "sp2")
								{
									UIKeybindingToggleListItem uikeybindingToggleListItem = new UIKeybindingToggleListItem(() => Lang.menu[197].Value, () => PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"].Contains(Buttons.DPadUp.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"].Contains(Buttons.DPadRight.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"].Contains(Buttons.DPadDown.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"].Contains(Buttons.DPadLeft.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"].Contains(Buttons.DPadUp.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"].Contains(Buttons.DPadRight.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"].Contains(Buttons.DPadDown.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"].Contains(Buttons.DPadLeft.ToString()), color);
									uikeybindingToggleListItem.OnLeftClick += this.RadialButtonClick;
									return uikeybindingToggleListItem;
								}
							}
						}
						else if (bind == "sp5")
						{
							return new UIKeybindingSliderItem(() => Lang.menu[201].Value + " (" + PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX.ToString("P1") + ")", () => PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX, delegate(float f)
							{
								PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX = f;
							}, delegate()
							{
								PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX, 0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
							}, 1002, color);
						}
					}
					else if (bind == "sp4")
					{
						return new UIKeybindingSliderItem(() => Lang.menu[200].Value + " (" + PlayerInput.CurrentProfile.InterfaceDeadzoneX.ToString("P1") + ")", () => PlayerInput.CurrentProfile.InterfaceDeadzoneX, delegate(float f)
						{
							PlayerInput.CurrentProfile.InterfaceDeadzoneX = f;
						}, delegate()
						{
							PlayerInput.CurrentProfile.InterfaceDeadzoneX = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0f, 0.95f, 0.35f, 0.35f);
						}, 1001, color);
					}
				}
				else if (num <= 222411333U)
				{
					if (num != 188856095U)
					{
						if (num == 222411333U)
						{
							if (bind == "sp1")
							{
								UIKeybindingToggleListItem uikeybindingToggleListItem2 = new UIKeybindingToggleListItem(() => Lang.menu[196].Value, () => PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"].Contains(Buttons.DPadUp.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"].Contains(Buttons.DPadRight.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"].Contains(Buttons.DPadDown.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"].Contains(Buttons.DPadLeft.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"].Contains(Buttons.DPadUp.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"].Contains(Buttons.DPadRight.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"].Contains(Buttons.DPadDown.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"].Contains(Buttons.DPadLeft.ToString()), color);
								uikeybindingToggleListItem2.OnLeftClick += this.SnapButtonClick;
								return uikeybindingToggleListItem2;
							}
						}
					}
					else if (bind == "sp3")
					{
						return new UIKeybindingSliderItem(() => Lang.menu[199].Value + " (" + PlayerInput.CurrentProfile.TriggersDeadzone.ToString("P1") + ")", () => PlayerInput.CurrentProfile.TriggersDeadzone, delegate(float f)
						{
							PlayerInput.CurrentProfile.TriggersDeadzone = f;
						}, delegate()
						{
							PlayerInput.CurrentProfile.TriggersDeadzone = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.TriggersDeadzone, 0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
						}, 1000, color);
					}
				}
				else if (num != 339854666U)
				{
					if (num != 356632285U)
					{
						if (num == 1316519504U)
						{
							if (bind == "sp15")
							{
								UIKeybindingToggleListItem uikeybindingToggleListItem3 = new UIKeybindingToggleListItem(() => Lang.menu[206].Value, () => PlayerInput.CurrentProfile.LeftThumbstickInvertY, color);
								uikeybindingToggleListItem3.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
								{
									if (PlayerInput.CurrentProfile.AllowEditting)
									{
										PlayerInput.CurrentProfile.LeftThumbstickInvertY = !PlayerInput.CurrentProfile.LeftThumbstickInvertY;
									}
								};
								return uikeybindingToggleListItem3;
							}
						}
					}
					else if (bind == "sp9")
					{
						UIKeybindingSimpleListItem uikeybindingSimpleListItem = new UIKeybindingSimpleListItem(() => Lang.menu[86].Value, color);
						uikeybindingSimpleListItem.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
						{
							string copyableProfileName = UIManageControls.GetCopyableProfileName();
							PlayerInput.CurrentProfile.CopyGameplaySettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName], currentInputMode);
						};
						return uikeybindingSimpleListItem;
					}
				}
				else if (bind == "sp8")
				{
					return new UIKeybindingSliderItem(() => Lang.menu[204].Value + " (" + PlayerInput.CurrentProfile.RightThumbstickDeadzoneY.ToString("P1") + ")", () => PlayerInput.CurrentProfile.RightThumbstickDeadzoneY, delegate(float f)
					{
						PlayerInput.CurrentProfile.RightThumbstickDeadzoneY = f;
					}, delegate()
					{
						PlayerInput.CurrentProfile.RightThumbstickDeadzoneY = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.RightThumbstickDeadzoneY, 0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
					}, 1005, color);
				}
			}
			else if (num <= 1400407599U)
			{
				if (num <= 1350074742U)
				{
					if (num != 1333297123U)
					{
						if (num == 1350074742U)
						{
							if (bind == "sp17")
							{
								UIKeybindingToggleListItem uikeybindingToggleListItem4 = new UIKeybindingToggleListItem(() => Lang.menu[208].Value, () => PlayerInput.CurrentProfile.RightThumbstickInvertY, color);
								uikeybindingToggleListItem4.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
								{
									if (PlayerInput.CurrentProfile.AllowEditting)
									{
										PlayerInput.CurrentProfile.RightThumbstickInvertY = !PlayerInput.CurrentProfile.RightThumbstickInvertY;
									}
								};
								return uikeybindingToggleListItem4;
							}
						}
					}
					else if (bind == "sp14")
					{
						UIKeybindingToggleListItem uikeybindingToggleListItem5 = new UIKeybindingToggleListItem(() => Lang.menu[205].Value, () => PlayerInput.CurrentProfile.LeftThumbstickInvertX, color);
						uikeybindingToggleListItem5.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
						{
							if (PlayerInput.CurrentProfile.AllowEditting)
							{
								PlayerInput.CurrentProfile.LeftThumbstickInvertX = !PlayerInput.CurrentProfile.LeftThumbstickInvertX;
							}
						};
						return uikeybindingToggleListItem5;
					}
				}
				else if (num != 1366852361U)
				{
					if (num != 1383629980U)
					{
						if (num == 1400407599U)
						{
							if (bind == "sp10")
							{
								UIKeybindingSimpleListItem uikeybindingSimpleListItem2 = new UIKeybindingSimpleListItem(() => Lang.menu[86].Value, color);
								uikeybindingSimpleListItem2.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
								{
									string copyableProfileName = UIManageControls.GetCopyableProfileName();
									PlayerInput.CurrentProfile.CopyHotbarSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName], currentInputMode);
								};
								return uikeybindingSimpleListItem2;
							}
						}
					}
					else if (bind == "sp11")
					{
						UIKeybindingSimpleListItem uikeybindingSimpleListItem3 = new UIKeybindingSimpleListItem(() => Lang.menu[86].Value, color);
						uikeybindingSimpleListItem3.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
						{
							string copyableProfileName = UIManageControls.GetCopyableProfileName();
							PlayerInput.CurrentProfile.CopyMapSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName], currentInputMode);
						};
						return uikeybindingSimpleListItem3;
					}
				}
				else if (bind == "sp16")
				{
					UIKeybindingToggleListItem uikeybindingToggleListItem6 = new UIKeybindingToggleListItem(() => Lang.menu[207].Value, () => PlayerInput.CurrentProfile.RightThumbstickInvertX, color);
					uikeybindingToggleListItem6.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
					{
						if (PlayerInput.CurrentProfile.AllowEditting)
						{
							PlayerInput.CurrentProfile.RightThumbstickInvertX = !PlayerInput.CurrentProfile.RightThumbstickInvertX;
						}
					};
					return uikeybindingToggleListItem6;
				}
			}
			else if (num <= 1433962837U)
			{
				if (num != 1417185218U)
				{
					if (num == 1433962837U)
					{
						if (bind == "sp12")
						{
							UIKeybindingSimpleListItem uikeybindingSimpleListItem4 = new UIKeybindingSimpleListItem(() => Lang.menu[86].Value, color);
							uikeybindingSimpleListItem4.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
							{
								string copyableProfileName = UIManageControls.GetCopyableProfileName();
								PlayerInput.CurrentProfile.CopyGamepadSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName], currentInputMode);
							};
							return uikeybindingSimpleListItem4;
						}
					}
				}
				else if (bind == "sp13")
				{
					UIKeybindingSimpleListItem uikeybindingSimpleListItem5 = new UIKeybindingSimpleListItem(() => Lang.menu[86].Value, color);
					uikeybindingSimpleListItem5.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
					{
						string copyableProfileName = UIManageControls.GetCopyableProfileName();
						PlayerInput.CurrentProfile.CopyGamepadAdvancedSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName], currentInputMode);
					};
					return uikeybindingSimpleListItem5;
				}
			}
			else if (num != 1517850932U)
			{
				if (num != 1534628551U)
				{
					if (num == 3782285044U)
					{
						if (bind == "sp20")
						{
							UIKeybindingToggleListItem uikeybindingToggleListItem7 = new UIKeybindingToggleListItem(() => Language.GetTextValue("UI.DoubleTapDash"), () => Player.Settings.DashControl == Player.Settings.DashPreference.AllowDoubleTap, color);
							uikeybindingToggleListItem7.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
							{
								Player.Settings.DashPreference dashControl = Player.Settings.DashControl;
								if (dashControl == Player.Settings.DashPreference.AllowDoubleTap)
								{
									Player.Settings.DashControl = Player.Settings.DashPreference.OnlyThroughHotkeys;
									return;
								}
								if (dashControl != Player.Settings.DashPreference.OnlyThroughHotkeys)
								{
									return;
								}
								Player.Settings.DashControl = Player.Settings.DashPreference.AllowDoubleTap;
							};
							return uikeybindingToggleListItem7;
						}
					}
				}
				else if (bind == "sp18")
				{
					return new UIKeybindingSliderItem(delegate()
					{
						int hotbarRadialHoldTimeRequired = PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired;
						if (hotbarRadialHoldTimeRequired == -1)
						{
							return Lang.menu[228].Value;
						}
						return Lang.menu[227].Value + " (" + ((float)hotbarRadialHoldTimeRequired / 60f).ToString("F2") + "s)";
					}, delegate()
					{
						if (PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired == -1)
						{
							return 1f;
						}
						return (float)PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired / 301f;
					}, delegate(float f)
					{
						PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired = (int)(f * 301f);
						if ((float)PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired == 301f)
						{
							PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired = -1;
						}
					}, delegate()
					{
						float num2 = (PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired == -1) ? 1f : ((float)PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired / 301f);
						num2 = UILinksInitializer.HandleSliderHorizontalInput(num2, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.5f);
						PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired = (int)(num2 * 301f);
						if ((float)PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired == 301f)
						{
							PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired = -1;
						}
					}, 1007, color);
				}
			}
			else if (bind == "sp19")
			{
				return new UIKeybindingSliderItem(delegate()
				{
					int inventoryMoveCD = PlayerInput.CurrentProfile.InventoryMoveCD;
					return Lang.menu[252].Value + " (" + ((float)inventoryMoveCD / 60f).ToString("F2") + "s)";
				}, () => Utils.GetLerpValue(4f, 12f, (float)PlayerInput.CurrentProfile.InventoryMoveCD, true), delegate(float f)
				{
					PlayerInput.CurrentProfile.InventoryMoveCD = (int)Math.Round((double)MathHelper.Lerp(4f, 12f, f));
				}, delegate()
				{
					if (UILinkPointNavigator.Shortcuts.INV_MOVE_OPTION_CD > 0)
					{
						UILinkPointNavigator.Shortcuts.INV_MOVE_OPTION_CD--;
					}
					if (UILinkPointNavigator.Shortcuts.INV_MOVE_OPTION_CD == 0)
					{
						float lerpValue = Utils.GetLerpValue(4f, 12f, (float)PlayerInput.CurrentProfile.InventoryMoveCD, true);
						float num2 = UILinksInitializer.HandleSliderHorizontalInput(lerpValue, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.5f);
						if (lerpValue != num2)
						{
							UILinkPointNavigator.Shortcuts.INV_MOVE_OPTION_CD = 8;
							int num3 = Math.Sign(num2 - lerpValue);
							PlayerInput.CurrentProfile.InventoryMoveCD = (int)MathHelper.Clamp((float)(PlayerInput.CurrentProfile.InventoryMoveCD + num3), 4f, 12f);
						}
					}
				}, 1008, color);
			}
			return new UIKeybindingListItem(bind, currentInputMode, color);
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x00599544 File Offset: 0x00597744
		public override void OnActivate()
		{
			if (Main.gameMenu)
			{
				this._outerContainer.Top.Set(220f, 0f);
				this._outerContainer.Height.Set(-220f, 1f);
			}
			else
			{
				this._outerContainer.Top.Set(120f, 0f);
				this._outerContainer.Height.Set(-120f, 1f);
			}
			if (PlayerInput.UsingGamepadUI)
			{
				UILinkPointNavigator.ChangePoint(3002);
			}
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x005995D4 File Offset: 0x005977D4
		private static string GetCopyableProfileName()
		{
			string result = "Redigit's Pick";
			if (PlayerInput.OriginalProfiles.ContainsKey(PlayerInput.CurrentProfile.Name))
			{
				result = PlayerInput.CurrentProfile.Name;
			}
			return result;
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x0059960C File Offset: 0x0059780C
		private void FillList()
		{
			List<UIElement> list = this._bindsKeyboard;
			if (!this.OnKeyboard)
			{
				list = this._bindsGamepad;
			}
			if (!this.OnGameplay)
			{
				list = (this.OnKeyboard ? this._bindsKeyboardUI : this._bindsGamepadUI);
			}
			this._uilist.Clear();
			foreach (UIElement item in list)
			{
				this._uilist.Add(item);
			}
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x005996A0 File Offset: 0x005978A0
		private void SnapButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (PlayerInput.CurrentProfile.AllowEditting)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				if (PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"].Contains(Buttons.DPadUp.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"].Contains(Buttons.DPadRight.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"].Contains(Buttons.DPadDown.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"].Contains(Buttons.DPadLeft.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"].Contains(Buttons.DPadUp.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"].Contains(Buttons.DPadRight.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"].Contains(Buttons.DPadDown.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"].Contains(Buttons.DPadLeft.ToString()))
				{
					PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"].Clear();
					PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"].Clear();
					PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"].Clear();
					PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"].Clear();
					PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"].Clear();
					PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"].Clear();
					PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"].Clear();
					PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"].Clear();
					return;
				}
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"] = new List<string>
				{
					Buttons.DPadUp.ToString()
				};
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"] = new List<string>
				{
					Buttons.DPadRight.ToString()
				};
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"] = new List<string>
				{
					Buttons.DPadDown.ToString()
				};
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"] = new List<string>
				{
					Buttons.DPadLeft.ToString()
				};
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"] = new List<string>
				{
					Buttons.DPadUp.ToString()
				};
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"] = new List<string>
				{
					Buttons.DPadRight.ToString()
				};
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"] = new List<string>
				{
					Buttons.DPadDown.ToString()
				};
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"] = new List<string>
				{
					Buttons.DPadLeft.ToString()
				};
			}
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x00599C9C File Offset: 0x00597E9C
		private void RadialButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (PlayerInput.CurrentProfile.AllowEditting)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				if (PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"].Contains(Buttons.DPadUp.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"].Contains(Buttons.DPadRight.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"].Contains(Buttons.DPadDown.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"].Contains(Buttons.DPadLeft.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"].Contains(Buttons.DPadUp.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"].Contains(Buttons.DPadRight.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"].Contains(Buttons.DPadDown.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"].Contains(Buttons.DPadLeft.ToString()))
				{
					PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"].Clear();
					PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"].Clear();
					PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"].Clear();
					PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"].Clear();
					PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"].Clear();
					PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"].Clear();
					PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"].Clear();
					PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"].Clear();
					return;
				}
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"] = new List<string>
				{
					Buttons.DPadUp.ToString()
				};
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"] = new List<string>
				{
					Buttons.DPadRight.ToString()
				};
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"] = new List<string>
				{
					Buttons.DPadDown.ToString()
				};
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"] = new List<string>
				{
					Buttons.DPadLeft.ToString()
				};
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"] = new List<string>
				{
					Buttons.DPadUp.ToString()
				};
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"] = new List<string>
				{
					Buttons.DPadRight.ToString()
				};
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"] = new List<string>
				{
					Buttons.DPadDown.ToString()
				};
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"] = new List<string>
				{
					Buttons.DPadLeft.ToString()
				};
			}
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x0059A298 File Offset: 0x00598498
		private void KeyboardButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonKeyboard.SetFrame(this._KeyboardGamepadTexture.Frame(2, 2, 0, 0, 0, 0));
			this._buttonGamepad.SetFrame(this._KeyboardGamepadTexture.Frame(2, 2, 1, 1, 0, 0));
			this.OnKeyboard = true;
			this.FillList();
		}

		// Token: 0x06002C51 RID: 11345 RVA: 0x0059A2EC File Offset: 0x005984EC
		private void GamepadButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonKeyboard.SetFrame(this._KeyboardGamepadTexture.Frame(2, 2, 0, 1, 0, 0));
			this._buttonGamepad.SetFrame(this._KeyboardGamepadTexture.Frame(2, 2, 1, 0, 0, 0));
			this.OnKeyboard = false;
			this.FillList();
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x0059A33E File Offset: 0x0059853E
		private void ManageBorderKeyboardOn(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonBorder2.Color = ((!this.OnKeyboard) ? Color.Silver : Color.Black);
			this._buttonBorder1.Color = Main.OurFavoriteColor;
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x0059A36F File Offset: 0x0059856F
		private void ManageBorderKeyboardOff(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonBorder2.Color = ((!this.OnKeyboard) ? Color.Silver : Color.Black);
			this._buttonBorder1.Color = (this.OnKeyboard ? Color.Silver : Color.Black);
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x0059A3AF File Offset: 0x005985AF
		private void ManageBorderGamepadOn(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonBorder1.Color = (this.OnKeyboard ? Color.Silver : Color.Black);
			this._buttonBorder2.Color = Main.OurFavoriteColor;
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x0059A3E0 File Offset: 0x005985E0
		private void ManageBorderGamepadOff(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonBorder1.Color = (this.OnKeyboard ? Color.Silver : Color.Black);
			this._buttonBorder2.Color = ((!this.OnKeyboard) ? Color.Silver : Color.Black);
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x0059A420 File Offset: 0x00598620
		private void VsGameplayButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonVs1.SetFrame(this._GameplayVsUITexture.Frame(2, 2, 0, 0, 0, 0));
			this._buttonVs2.SetFrame(this._GameplayVsUITexture.Frame(2, 2, 1, 1, 0, 0));
			this.OnGameplay = true;
			this.FillList();
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x0059A474 File Offset: 0x00598674
		private void VsMenuButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonVs1.SetFrame(this._GameplayVsUITexture.Frame(2, 2, 0, 1, 0, 0));
			this._buttonVs2.SetFrame(this._GameplayVsUITexture.Frame(2, 2, 1, 0, 0, 0));
			this.OnGameplay = false;
			this.FillList();
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x0059A4C6 File Offset: 0x005986C6
		private void ManageBorderGameplayOn(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonBorderVs2.Color = ((!this.OnGameplay) ? Color.Silver : Color.Black);
			this._buttonBorderVs1.Color = Main.OurFavoriteColor;
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x0059A4F7 File Offset: 0x005986F7
		private void ManageBorderGameplayOff(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonBorderVs2.Color = ((!this.OnGameplay) ? Color.Silver : Color.Black);
			this._buttonBorderVs1.Color = (this.OnGameplay ? Color.Silver : Color.Black);
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x0059A537 File Offset: 0x00598737
		private void ManageBorderMenuOn(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonBorderVs1.Color = (this.OnGameplay ? Color.Silver : Color.Black);
			this._buttonBorderVs2.Color = Main.OurFavoriteColor;
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x0059A568 File Offset: 0x00598768
		private void ManageBorderMenuOff(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonBorderVs1.Color = (this.OnGameplay ? Color.Silver : Color.Black);
			this._buttonBorderVs2.Color = ((!this.OnGameplay) ? Color.Silver : Color.Black);
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x0059A5A8 File Offset: 0x005987A8
		private string controllerGlyphStyle()
		{
			switch (GlyphTagHandler.GlyphStyle)
			{
			case -1:
				return Language.GetText("UI.ControllerGlyphAuto").Value;
			case 1:
				return Language.GetText("UI.ControllerGlyphPlayStation").Value;
			case 2:
				return Language.GetText("UI.ControllerGlyphSwitch").Value;
			}
			return Language.GetText("UI.ControllerGlyphXbox").Value;
		}

		// Token: 0x06002C5D RID: 11357 RVA: 0x0059A61C File Offset: 0x0059881C
		private void controllerGlyphButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			GlyphTagHandler.GlyphStyle++;
			if (GlyphTagHandler.GlyphStyle > 2)
			{
				GlyphTagHandler.GlyphStyle = -1;
			}
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x0059A638 File Offset: 0x00598838
		private void profileButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			string name = PlayerInput.CurrentProfile.Name;
			List<string> list = PlayerInput.Profiles.Keys.ToList<string>();
			int num = list.IndexOf(name);
			num++;
			if (num >= list.Count)
			{
				num -= list.Count;
			}
			PlayerInput.SetSelectedProfile(list[num]);
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x0059A68C File Offset: 0x0059888C
		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002C60 RID: 11360 RVA: 0x00587B9D File Offset: 0x00585D9D
		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.7f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		// Token: 0x06002C61 RID: 11361 RVA: 0x0059A6E1 File Offset: 0x005988E1
		private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
		{
			Main.menuMode = 1127;
			IngameFancyUI.Close(false);
		}

		// Token: 0x06002C62 RID: 11362 RVA: 0x0059A6F3 File Offset: 0x005988F3
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			this.SetupGamepadPoints(spriteBatch);
		}

		// Token: 0x06002C63 RID: 11363 RVA: 0x0059A704 File Offset: 0x00598904
		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 4;
			int num = 3000;
			UILinkPointNavigator.SetPosition(3000, this._buttonBack.GetInnerDimensions().ToRectangle().Center.ToVector2());
			UILinkPointNavigator.SetPosition(3001, this._buttonKeyboard.GetInnerDimensions().ToRectangle().Center.ToVector2());
			UILinkPointNavigator.SetPosition(3002, this._buttonGamepad.GetInnerDimensions().ToRectangle().Center.ToVector2());
			UILinkPointNavigator.SetPosition(3003, this._buttonProfile.GetInnerDimensions().ToRectangle().Center.ToVector2());
			UILinkPointNavigator.SetPosition(3004, this._buttonVs1.GetInnerDimensions().ToRectangle().Center.ToVector2());
			UILinkPointNavigator.SetPosition(3005, this._buttonVs2.GetInnerDimensions().ToRectangle().Center.ToVector2());
			UILinkPointNavigator.SetPosition(3006, this._buttonStyle.GetInnerDimensions().ToRectangle().Center.ToVector2());
			int num2 = num;
			UILinkPoint uilinkPoint = UILinkPointNavigator.Points[num2];
			uilinkPoint.Unlink();
			uilinkPoint.Up = num + 7;
			num2 = 3001;
			UILinkPoint uilinkPoint2 = UILinkPointNavigator.Points[num2];
			uilinkPoint2.Unlink();
			uilinkPoint2.Right = 3002;
			uilinkPoint2.Down = num + 7;
			num2 = 3002;
			UILinkPoint uilinkPoint3 = UILinkPointNavigator.Points[num2];
			uilinkPoint3.Unlink();
			uilinkPoint3.Left = 3001;
			uilinkPoint3.Right = 3004;
			uilinkPoint3.Down = num + 7;
			num2 = 3004;
			UILinkPoint uilinkPoint4 = UILinkPointNavigator.Points[num2];
			uilinkPoint4.Unlink();
			uilinkPoint4.Left = 3002;
			uilinkPoint4.Right = 3005;
			uilinkPoint4.Down = num + 7;
			num2 = 3005;
			UILinkPoint uilinkPoint5 = UILinkPointNavigator.Points[num2];
			uilinkPoint5.Unlink();
			uilinkPoint5.Left = 3004;
			uilinkPoint5.Right = 3006;
			uilinkPoint5.Down = num + 7;
			num2 = 3006;
			UILinkPoint uilinkPoint6 = UILinkPointNavigator.Points[num2];
			uilinkPoint6.Unlink();
			uilinkPoint6.Left = 3005;
			uilinkPoint6.Right = 3003;
			uilinkPoint6.Down = num + 7;
			num2 = 3003;
			UILinkPoint uilinkPoint7 = UILinkPointNavigator.Points[num2];
			uilinkPoint7.Unlink();
			uilinkPoint7.Left = 3006;
			uilinkPoint7.Down = num + 7;
			float scaleFactor = 1f / Main.UIScale;
			Rectangle clippingRectangle = this._uilist.GetClippingRectangle(spriteBatch);
			Vector2 minimum = clippingRectangle.TopLeft() * scaleFactor;
			Vector2 maximum = clippingRectangle.BottomRight() * scaleFactor;
			List<SnapPoint> snapPoints = this._uilist.GetSnapPoints();
			for (int i = 0; i < snapPoints.Count; i++)
			{
				if (!snapPoints[i].Position.Between(minimum, maximum))
				{
					Vector2 position = snapPoints[i].Position;
					snapPoints.Remove(snapPoints[i]);
					i--;
				}
			}
			snapPoints.Sort((SnapPoint x, SnapPoint y) => x.Id.CompareTo(y.Id));
			for (int j = 0; j < snapPoints.Count; j++)
			{
				num2 = num + 7 + j;
				if (snapPoints[j].Name == "Thin")
				{
					UILinkPoint uilinkPoint8 = UILinkPointNavigator.Points[num2];
					uilinkPoint8.Unlink();
					UILinkPointNavigator.SetPosition(num2, snapPoints[j].Position);
					uilinkPoint8.Right = num2 + 1;
					uilinkPoint8.Down = ((j < snapPoints.Count - 2) ? (num2 + 2) : num);
					uilinkPoint8.Up = ((j < 2) ? 3001 : ((snapPoints[j - 1].Name == "Wide") ? (num2 - 1) : (num2 - 2)));
					UILinkPointNavigator.Points[num].Up = num2;
					UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = num2;
					j++;
					if (j < snapPoints.Count)
					{
						num2 = num + 7 + j;
						UILinkPoint uilinkPoint9 = UILinkPointNavigator.Points[num2];
						uilinkPoint9.Unlink();
						UILinkPointNavigator.SetPosition(num2, snapPoints[j].Position);
						uilinkPoint9.Left = num2 - 1;
						uilinkPoint9.Down = ((j < snapPoints.Count - 1) ? ((snapPoints[j + 1].Name == "Wide") ? (num2 + 1) : (num2 + 2)) : num);
						uilinkPoint9.Up = ((j < 2) ? 3001 : (num2 - 2));
						UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = num2;
					}
				}
				else
				{
					UILinkPoint uilinkPoint10 = UILinkPointNavigator.Points[num2];
					uilinkPoint10.Unlink();
					UILinkPointNavigator.SetPosition(num2, snapPoints[j].Position);
					uilinkPoint10.Down = ((j < snapPoints.Count - 1) ? (num2 + 1) : num);
					uilinkPoint10.Up = ((j < 1) ? 3001 : ((snapPoints[j - 1].Name == "Wide") ? (num2 - 1) : (num2 - 2)));
					UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = num2;
					UILinkPointNavigator.Points[num].Up = num2;
				}
			}
			if (UIManageControls.ForceMoveTo != -1)
			{
				UILinkPointNavigator.ChangePoint((int)MathHelper.Clamp((float)UIManageControls.ForceMoveTo, (float)num, (float)UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX));
				UIManageControls.ForceMoveTo = -1;
			}
		}

		// Token: 0x040053CA RID: 21450
		public static int ForceMoveTo = -1;

		// Token: 0x040053CB RID: 21451
		private const float PanelTextureHeight = 30f;

		// Token: 0x040053CC RID: 21452
		private static List<string> _BindingsFullLine = new List<string>
		{
			"Throw",
			"Inventory",
			"RadialHotbar",
			"RadialQuickbar",
			"LockOn",
			"ToggleCreativeMenu",
			"Loadout1",
			"Loadout2",
			"Loadout3",
			"ToggleCameraMode",
			"sp3",
			"sp4",
			"sp5",
			"sp6",
			"sp7",
			"sp8",
			"sp18",
			"sp19",
			"sp9",
			"sp10",
			"sp11",
			"sp12",
			"sp13",
			"ArmorSetAbility"
		};

		// Token: 0x040053CD RID: 21453
		private static List<string> _BindingsHalfSingleLine = new List<string>
		{
			"sp9",
			"sp10",
			"sp11",
			"sp12",
			"sp13",
			"sp20"
		};

		// Token: 0x040053CE RID: 21454
		private bool OnKeyboard = true;

		// Token: 0x040053CF RID: 21455
		private bool OnGameplay = true;

		// Token: 0x040053D0 RID: 21456
		private List<UIElement> _bindsKeyboard = new List<UIElement>();

		// Token: 0x040053D1 RID: 21457
		private List<UIElement> _bindsGamepad = new List<UIElement>();

		// Token: 0x040053D2 RID: 21458
		private List<UIElement> _bindsKeyboardUI = new List<UIElement>();

		// Token: 0x040053D3 RID: 21459
		private List<UIElement> _bindsGamepadUI = new List<UIElement>();

		// Token: 0x040053D4 RID: 21460
		private UIElement _outerContainer;

		// Token: 0x040053D5 RID: 21461
		private UIList _uilist;

		// Token: 0x040053D6 RID: 21462
		private UIImageFramed _buttonKeyboard;

		// Token: 0x040053D7 RID: 21463
		private UIImageFramed _buttonGamepad;

		// Token: 0x040053D8 RID: 21464
		private UIImageFramed _buttonBorder1;

		// Token: 0x040053D9 RID: 21465
		private UIImageFramed _buttonBorder2;

		// Token: 0x040053DA RID: 21466
		private UIKeybindingSimpleListItem _buttonProfile;

		// Token: 0x040053DB RID: 21467
		private UIKeybindingSimpleListItem _buttonStyle;

		// Token: 0x040053DC RID: 21468
		private UIElement _buttonBack;

		// Token: 0x040053DD RID: 21469
		private UIImageFramed _buttonVs1;

		// Token: 0x040053DE RID: 21470
		private UIImageFramed _buttonVs2;

		// Token: 0x040053DF RID: 21471
		private UIImageFramed _buttonBorderVs1;

		// Token: 0x040053E0 RID: 21472
		private UIImageFramed _buttonBorderVs2;

		// Token: 0x040053E1 RID: 21473
		private Asset<Texture2D> _KeyboardGamepadTexture;

		// Token: 0x040053E2 RID: 21474
		private Asset<Texture2D> _keyboardGamepadBorderTexture;

		// Token: 0x040053E3 RID: 21475
		private Asset<Texture2D> _GameplayVsUITexture;

		// Token: 0x040053E4 RID: 21476
		private Asset<Texture2D> _GameplayVsUIBorderTexture;

		// Token: 0x040053E5 RID: 21477
		private static int SnapPointIndex;

		// Token: 0x02000912 RID: 2322
		public class SpecialControls
		{
			// Token: 0x04007431 RID: 29745
			public const string MouseSnapToggle = "sp1";

			// Token: 0x04007432 RID: 29746
			public const string MouseHotbarToggle = "sp2";

			// Token: 0x04007433 RID: 29747
			public const string TriggersDeadZone = "sp3";

			// Token: 0x04007434 RID: 29748
			public const string SlidersDeadZone = "sp4";

			// Token: 0x04007435 RID: 29749
			public const string LeftXDeadZone = "sp5";

			// Token: 0x04007436 RID: 29750
			public const string LeftYDeadZone = "sp6";

			// Token: 0x04007437 RID: 29751
			public const string RightXDeadZone = "sp7";

			// Token: 0x04007438 RID: 29752
			public const string RightYDeadZone = "sp8";

			// Token: 0x04007439 RID: 29753
			public const string ResetGameplay = "sp9";

			// Token: 0x0400743A RID: 29754
			public const string ResetHotbar = "sp10";

			// Token: 0x0400743B RID: 29755
			public const string ResetMap = "sp11";

			// Token: 0x0400743C RID: 29756
			public const string ResetGamepad = "sp12";

			// Token: 0x0400743D RID: 29757
			public const string ResetGamepadAdvanced = "sp13";

			// Token: 0x0400743E RID: 29758
			public const string InvertLeftX = "sp14";

			// Token: 0x0400743F RID: 29759
			public const string InvertLeftY = "sp15";

			// Token: 0x04007440 RID: 29760
			public const string InvertRightX = "sp16";

			// Token: 0x04007441 RID: 29761
			public const string InvertRightY = "sp17";

			// Token: 0x04007442 RID: 29762
			public const string TimeBeforeRadial = "sp18";

			// Token: 0x04007443 RID: 29763
			public const string TicksPerInventoryMovement = "sp19";

			// Token: 0x04007444 RID: 29764
			public const string DisableDoubleTapForDashing = "sp20";
		}
	}
}
