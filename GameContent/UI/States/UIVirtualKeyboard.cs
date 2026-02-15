using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003B3 RID: 947
	public class UIVirtualKeyboard : UIState
	{
		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06002C67 RID: 11367 RVA: 0x0059AE3D File Offset: 0x0059903D
		// (set) Token: 0x06002C68 RID: 11368 RVA: 0x0059AE4A File Offset: 0x0059904A
		public string Text
		{
			get
			{
				return this._textBox.Text;
			}
			set
			{
				this._textBox.SetText(value);
				this.ValidateText();
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06002C69 RID: 11369 RVA: 0x0059AE5E File Offset: 0x0059905E
		// (set) Token: 0x06002C6A RID: 11370 RVA: 0x0059AE6B File Offset: 0x0059906B
		public bool HideContents
		{
			get
			{
				return this._textBox.HideContents;
			}
			set
			{
				this._textBox.HideContents = value;
			}
		}

		// Token: 0x06002C6B RID: 11371 RVA: 0x0059AE7C File Offset: 0x0059907C
		public UIVirtualKeyboard(string labelText, string startingText, UIVirtualKeyboard.KeyboardSubmitEvent submitAction, Action cancelAction, int inputMode = 0, bool allowEmpty = false, int maxLength = 20)
		{
			this._keyboardContext = inputMode;
			this._allowEmpty = allowEmpty;
			UIVirtualKeyboard.OffsetDown = 0;
			UIVirtualKeyboard.ShouldHideText = false;
			this._lastOffsetDown = 0;
			this._edittingSign = (this._keyboardContext == 1);
			this._edittingChest = (this._keyboardContext == 2);
			UIVirtualKeyboard._currentInstance = this;
			this._submitAction = submitAction;
			this._cancelAction = cancelAction;
			this._textureShift = Main.Assets.Request<Texture2D>("Images/UI/VK_Shift", 1);
			this._textureBackspace = Main.Assets.Request<Texture2D>("Images/UI/VK_Backspace", 1);
			this.Top.Pixels = (float)this._lastOffsetDown;
			float num = (float)(-5000 * this._edittingSign.ToInt());
			float num2 = 270f;
			float precent = 0f;
			float num3 = 516f;
			UIElement uielement = new UIElement();
			uielement.Width.Pixels = num3 + 8f + 16f;
			uielement.Top.Precent = precent;
			uielement.Top.Pixels = num2;
			uielement.Height.Pixels = 266f;
			uielement.HAlign = 0.5f;
			uielement.SetPadding(0f);
			this.outerLayer1 = uielement;
			UIElement uielement2 = new UIElement();
			uielement2.Width.Pixels = num3 + 8f + 16f;
			uielement2.Top.Precent = precent;
			uielement2.Top.Pixels = num2;
			uielement2.Height.Pixels = 266f;
			uielement2.HAlign = 0.5f;
			uielement2.SetPadding(0f);
			this.outerLayer2 = uielement2;
			UIPanel uipanel = new UIPanel();
			uipanel.Width.Precent = 1f;
			uipanel.Height.Pixels = 225f;
			uipanel.BackgroundColor = new Color(23, 33, 69) * 0.7f;
			uielement.Append(uipanel);
			float num4 = -50f;
			this._textBox = new UITextBox("", 0.78f, true);
			this._textBox.BackgroundColor = Color.Transparent;
			this._textBox.BorderColor = Color.Transparent;
			this._textBox.HAlign = 0.5f;
			this._textBox.Width.Pixels = num3;
			this._textBox.Top.Pixels = num4 + num2 - 10f + num;
			this._textBox.Top.Precent = precent;
			this._textBox.Height.Pixels = 37f;
			base.Append(this._textBox);
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					int index = j * 10 + i;
					UITextPanel<object> uitextPanel = this.CreateKeyboardButton("1234567890qwertyuiopasdfghjkl'zxcvbnm,.?"[index].ToString(), i, j, 1, true);
					uitextPanel.OnLeftClick += this.TypeText;
					uipanel.Append(uitextPanel);
				}
			}
			this._shiftButton = this.CreateKeyboardButton("", 0, 4, 1, false);
			this._shiftButton.PaddingLeft = 0f;
			this._shiftButton.PaddingRight = 0f;
			this._shiftButton.PaddingBottom = (this._shiftButton.PaddingTop = 0f);
			this._shiftButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
			this._shiftButton.BorderColor = this._internalBorderColor * 0.7f;
			this._shiftButton.OnMouseOver += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				this._shiftButton.BorderColor = this._internalBorderColorSelected;
				if (this._keyState != UIVirtualKeyboard.KeyState.Shift)
				{
					this._shiftButton.BackgroundColor = new Color(73, 94, 171);
				}
			};
			this._shiftButton.OnMouseOut += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				this._shiftButton.BorderColor = this._internalBorderColor * 0.7f;
				if (this._keyState != UIVirtualKeyboard.KeyState.Shift)
				{
					this._shiftButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
				}
			};
			this._shiftButton.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				this.SetKeyState((this._keyState == UIVirtualKeyboard.KeyState.Shift) ? UIVirtualKeyboard.KeyState.Default : UIVirtualKeyboard.KeyState.Shift);
			};
			UIImage uiimage = new UIImage(this._textureShift);
			uiimage.HAlign = 0.5f;
			uiimage.VAlign = 0.5f;
			uiimage.ImageScale = 0.85f;
			this._shiftButton.Append(uiimage);
			uipanel.Append(this._shiftButton);
			this._symbolButton = this.CreateKeyboardButton("@%", 1, 4, 1, false);
			this._symbolButton.PaddingLeft = 0f;
			this._symbolButton.PaddingRight = 0f;
			this._symbolButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
			this._symbolButton.BorderColor = this._internalBorderColor * 0.7f;
			this._symbolButton.OnMouseOver += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				this._symbolButton.BorderColor = this._internalBorderColorSelected;
				if (this._keyState != UIVirtualKeyboard.KeyState.Symbol)
				{
					this._symbolButton.BackgroundColor = new Color(73, 94, 171);
				}
			};
			this._symbolButton.OnMouseOut += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				this._symbolButton.BorderColor = this._internalBorderColor * 0.7f;
				if (this._keyState != UIVirtualKeyboard.KeyState.Symbol)
				{
					this._symbolButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
				}
			};
			this._symbolButton.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				this.SetKeyState((this._keyState == UIVirtualKeyboard.KeyState.Symbol) ? UIVirtualKeyboard.KeyState.Default : UIVirtualKeyboard.KeyState.Symbol);
			};
			uipanel.Append(this._symbolButton);
			this.BuildSpaceBarArea(uipanel);
			this._submitButton = new UITextPanel<LocalizedText>((this._edittingSign || this._edittingChest) ? Language.GetText("UI.Save") : Language.GetText("UI.Submit"), 0.4f, true);
			this._submitButton.Height.Pixels = 37f;
			this._submitButton.Width.Precent = 0.4f;
			this._submitButton.HAlign = 1f;
			this._submitButton.VAlign = 1f;
			this._submitButton.PaddingLeft = 0f;
			this._submitButton.PaddingRight = 0f;
			this.ValidateText();
			this._submitButton.OnMouseOver += this.FadedMouseOver;
			this._submitButton.OnMouseOut += this.FadedMouseOut;
			this._submitButton.OnMouseOver += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				this.ValidateText();
			};
			this._submitButton.OnMouseOut += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				this.ValidateText();
			};
			this._submitButton.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				UIVirtualKeyboard.Submit();
			};
			uielement.Append(this._submitButton);
			this._cancelButton = new UITextPanel<LocalizedText>(Language.GetText("UI.Cancel"), 0.4f, true);
			this.StyleKey<LocalizedText>(this._cancelButton, true);
			this._cancelButton.Height.Pixels = 37f;
			this._cancelButton.Width.Precent = 0.4f;
			this._cancelButton.VAlign = 1f;
			this._cancelButton.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
				this._cancelAction();
			};
			this._cancelButton.OnMouseOver += this.FadedMouseOver;
			this._cancelButton.OnMouseOut += this.FadedMouseOut;
			uielement.Append(this._cancelButton);
			this._submitButton2 = new UITextPanel<LocalizedText>((this._edittingSign || this._edittingChest) ? Language.GetText("UI.Save") : Language.GetText("UI.Submit"), 0.72f, true);
			this._submitButton2.TextColor = Color.Silver;
			this._submitButton2.DrawPanel = false;
			this._submitButton2.Height.Pixels = 60f;
			this._submitButton2.Width.Precent = 0.4f;
			this._submitButton2.HAlign = 0.5f;
			this._submitButton2.VAlign = 0f;
			this._submitButton2.OnMouseOver += delegate(UIMouseEvent a, UIElement b)
			{
				((UITextPanel<LocalizedText>)b).TextScale = 0.85f;
				((UITextPanel<LocalizedText>)b).TextColor = Color.White;
			};
			this._submitButton2.OnMouseOut += delegate(UIMouseEvent a, UIElement b)
			{
				((UITextPanel<LocalizedText>)b).TextScale = 0.72f;
				((UITextPanel<LocalizedText>)b).TextColor = Color.Silver;
			};
			this._submitButton2.Top.Pixels = 50f;
			this._submitButton2.PaddingLeft = 0f;
			this._submitButton2.PaddingRight = 0f;
			this.ValidateText();
			this._submitButton2.OnMouseOver += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				this.ValidateText();
			};
			this._submitButton2.OnMouseOut += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				this.ValidateText();
			};
			this._submitButton2.OnMouseOver += this.FadedMouseOver;
			this._submitButton2.OnMouseOut += this.FadedMouseOut;
			this._submitButton2.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				if (this.TextIsValidForSubmit())
				{
					SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
					this._submitAction(this.Text.Trim());
				}
			};
			this.outerLayer2.Append(this._submitButton2);
			this._cancelButton2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Cancel"), 0.72f, true);
			this._cancelButton2.TextColor = Color.Silver;
			this._cancelButton2.DrawPanel = false;
			this._cancelButton2.OnMouseOver += delegate(UIMouseEvent a, UIElement b)
			{
				((UITextPanel<LocalizedText>)b).TextScale = 0.85f;
				((UITextPanel<LocalizedText>)b).TextColor = Color.White;
			};
			this._cancelButton2.OnMouseOut += delegate(UIMouseEvent a, UIElement b)
			{
				((UITextPanel<LocalizedText>)b).TextScale = 0.72f;
				((UITextPanel<LocalizedText>)b).TextColor = Color.Silver;
			};
			this._cancelButton2.Height.Pixels = 60f;
			this._cancelButton2.Width.Precent = 0.4f;
			this._cancelButton2.Top.Pixels = 114f;
			this._cancelButton2.VAlign = 0f;
			this._cancelButton2.HAlign = 0.5f;
			this._cancelButton2.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
				this._cancelAction();
			};
			this.outerLayer2.Append(this._cancelButton2);
			UITextPanel<object> uitextPanel2 = this.CreateKeyboardButton("", 8, 4, 2, true);
			uitextPanel2.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				this._textBox.Backspace();
				this.ValidateText();
			};
			uitextPanel2.PaddingLeft = 0f;
			uitextPanel2.PaddingRight = 0f;
			uitextPanel2.PaddingBottom = (uitextPanel2.PaddingTop = 0f);
			uitextPanel2.Append(new UIImage(this._textureBackspace)
			{
				HAlign = 0.5f,
				VAlign = 0.5f,
				ImageScale = 0.92f
			});
			uipanel.Append(uitextPanel2);
			UIText uitext = new UIText(labelText, 0.75f, true);
			uitext.HAlign = 0.5f;
			uitext.Width.Pixels = num3;
			uitext.Top.Pixels = num4 - 37f - 4f + num2 + num;
			uitext.Top.Precent = precent;
			uitext.Height.Pixels = 37f;
			base.Append(uitext);
			this._label = uitext;
			base.Append(uielement);
			this._textBox.SetTextMaxLength(maxLength);
			this.Text = startingText;
			if (this.Text.Length == 0)
			{
				this.SetKeyState(UIVirtualKeyboard.KeyState.Shift);
			}
			UIVirtualKeyboard.ShouldHideText = true;
			UIVirtualKeyboard.OffsetDown = 9999;
			this.UpdateOffsetDown();
		}

		// Token: 0x06002C6C RID: 11372 RVA: 0x0059B9AA File Offset: 0x00599BAA
		public void SetMaxInputLength(int length)
		{
			this._textBox.SetTextMaxLength(length);
		}

		// Token: 0x06002C6D RID: 11373 RVA: 0x0059B9B8 File Offset: 0x00599BB8
		private void BuildSpaceBarArea(UIPanel mainPanel)
		{
			UIElement.MouseEvent <>9__1;
			UIElement.MouseEvent <>9__2;
			Action createTheseTwo = delegate()
			{
				bool flag = this.CanRestore();
				int x = flag ? 4 : 5;
				bool edittingSign = this._edittingSign;
				int num = (flag && edittingSign) ? 2 : 3;
				UITextPanel<object> uitextPanel = this.CreateKeyboardButton(Language.GetText("UI.SpaceButton"), 2, 4, (this._edittingSign || (this._edittingChest && flag)) ? num : 6, true);
				UIElement uielement = uitextPanel;
				UIElement.MouseEvent value;
				if ((value = <>9__1) == null)
				{
					value = (<>9__1 = delegate(UIMouseEvent evt, UIElement listeningElement)
					{
						this.PressSpace();
					});
				}
				uielement.OnLeftClick += value;
				mainPanel.Append(uitextPanel);
				this._spacebarButton = uitextPanel;
				if (edittingSign)
				{
					UITextPanel<object> uitextPanel2 = this.CreateKeyboardButton(Language.GetText("UI.EnterButton"), x, 4, num, true);
					UIElement uielement2 = uitextPanel2;
					UIElement.MouseEvent value2;
					if ((value2 = <>9__2) == null)
					{
						value2 = (<>9__2 = delegate(UIMouseEvent evt, UIElement listeningElement)
						{
							SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
							this._textBox.Write("\n");
							this.ValidateText();
						});
					}
					uielement2.OnLeftClick += value2;
					mainPanel.Append(uitextPanel2);
					this._enterButton = uitextPanel2;
				}
			};
			createTheseTwo();
			if (this.CanRestore())
			{
				UITextPanel<object> restoreBar = this.CreateKeyboardButton(Language.GetText("UI.RestoreButton"), 6, 4, 2, true);
				restoreBar.OnLeftClick += delegate(UIMouseEvent evt, UIElement listeningElement)
				{
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					this.RestoreCanceledInput(this._keyboardContext);
					this.ValidateText();
					restoreBar.Remove();
					this._enterButton.Remove();
					this._spacebarButton.Remove();
					createTheseTwo();
				};
				mainPanel.Append(restoreBar);
				this._restoreButton = restoreBar;
			}
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x0059BA60 File Offset: 0x00599C60
		private void PressSpace()
		{
			string text = " ";
			if (this.CustomTextValidationForUpdate != null && !this.CustomTextValidationForUpdate(this.Text + text))
			{
				SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
				return;
			}
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			this._textBox.Write(text);
			this.ValidateText();
		}

		// Token: 0x06002C6F RID: 11375 RVA: 0x0059BAD1 File Offset: 0x00599CD1
		private bool CanRestore()
		{
			if (this._edittingSign)
			{
				return UIVirtualKeyboard._cancelCacheSign.Length > 0;
			}
			return this._edittingChest && UIVirtualKeyboard._cancelCacheChest.Length > 0;
		}

		// Token: 0x06002C70 RID: 11376 RVA: 0x0059BB00 File Offset: 0x00599D00
		private void TypeText(UIMouseEvent evt, UIElement listeningElement)
		{
			string text = ((UITextPanel<object>)listeningElement).Text;
			if (this.CustomTextValidationForUpdate != null && !this.CustomTextValidationForUpdate(this.Text + text))
			{
				SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
				return;
			}
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			bool flag = this.Text.Length == 0;
			this._textBox.Write(text);
			this.ValidateText();
			if (flag && this.Text.Length > 0 && this._keyState == UIVirtualKeyboard.KeyState.Shift)
			{
				this.SetKeyState(UIVirtualKeyboard.KeyState.Default);
			}
		}

		// Token: 0x06002C71 RID: 11377 RVA: 0x0059BBA8 File Offset: 0x00599DA8
		public void SetKeyState(UIVirtualKeyboard.KeyState keyState)
		{
			UITextPanel<object> uitextPanel = null;
			UIVirtualKeyboard.KeyState keyState2 = this._keyState;
			if (keyState2 != UIVirtualKeyboard.KeyState.Symbol)
			{
				if (keyState2 == UIVirtualKeyboard.KeyState.Shift)
				{
					uitextPanel = this._shiftButton;
				}
			}
			else
			{
				uitextPanel = this._symbolButton;
			}
			if (uitextPanel != null)
			{
				if (uitextPanel.IsMouseHovering)
				{
					uitextPanel.BackgroundColor = new Color(73, 94, 171);
				}
				else
				{
					uitextPanel.BackgroundColor = new Color(63, 82, 151) * 0.7f;
				}
			}
			string text = null;
			UITextPanel<object> uitextPanel2 = null;
			switch (keyState)
			{
			case UIVirtualKeyboard.KeyState.Default:
				text = "1234567890qwertyuiopasdfghjkl'zxcvbnm,.?";
				break;
			case UIVirtualKeyboard.KeyState.Symbol:
				text = "1234567890!@#$%^&*()-_+=/\\{}[]<>;:\"`|~£¥";
				uitextPanel2 = this._symbolButton;
				break;
			case UIVirtualKeyboard.KeyState.Shift:
				text = "1234567890QWERTYUIOPASDFGHJKL'ZXCVBNM,.?";
				uitextPanel2 = this._shiftButton;
				break;
			}
			for (int i = 0; i < text.Length; i++)
			{
				this._keyList[i].SetText(text[i].ToString());
			}
			this._keyState = keyState;
			if (uitextPanel2 != null)
			{
				uitextPanel2.BackgroundColor = new Color(93, 114, 191);
			}
		}

		// Token: 0x06002C72 RID: 11378 RVA: 0x0059BCA4 File Offset: 0x00599EA4
		private void ValidateText()
		{
			if (this.TextIsValidForSubmit())
			{
				this._canSubmit = true;
				this._submitButton.TextColor = Color.White;
				if (this._submitButton.IsMouseHovering)
				{
					this._submitButton.BackgroundColor = new Color(73, 94, 171);
					return;
				}
				this._submitButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
				return;
			}
			else
			{
				this._canSubmit = false;
				this._submitButton.TextColor = Color.Gray;
				if (this._submitButton.IsMouseHovering)
				{
					this._submitButton.BackgroundColor = new Color(180, 60, 60) * 0.85f;
					return;
				}
				this._submitButton.BackgroundColor = new Color(150, 40, 40) * 0.85f;
				return;
			}
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x0059BD88 File Offset: 0x00599F88
		private bool TextIsValidForSubmit()
		{
			if (this.CustomTextValidationForUpdate != null)
			{
				return this.CustomTextValidationForUpdate(this.Text);
			}
			return this.Text.Trim().Length > 0 || this._edittingSign || this._edittingChest || this._allowEmpty;
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x0059BDDC File Offset: 0x00599FDC
		private void StyleKey<T>(UITextPanel<T> button, bool external = false)
		{
			button.PaddingLeft = 0f;
			button.PaddingRight = 0f;
			button.BackgroundColor = new Color(63, 82, 151) * 0.7f;
			if (!external)
			{
				button.BorderColor = this._internalBorderColor * 0.7f;
			}
			button.OnMouseOver += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				((UITextPanel<T>)listeningElement).BackgroundColor = new Color(73, 94, 171) * 0.85f;
				if (!external)
				{
					((UITextPanel<T>)listeningElement).BorderColor = this._internalBorderColorSelected * 0.85f;
				}
			};
			button.OnMouseOut += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				((UITextPanel<T>)listeningElement).BackgroundColor = new Color(63, 82, 151) * 0.7f;
				if (!external)
				{
					((UITextPanel<T>)listeningElement).BorderColor = this._internalBorderColor * 0.7f;
				}
			};
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x0059BE74 File Offset: 0x0059A074
		private UITextPanel<object> CreateKeyboardButton(object text, int x, int y, int width = 1, bool style = true)
		{
			float num = 516f;
			UITextPanel<object> uitextPanel = new UITextPanel<object>(text, 0.4f, true);
			uitextPanel.Width.Pixels = 48f * (float)width + 4f * (float)(width - 1);
			uitextPanel.Height.Pixels = 37f;
			uitextPanel.Left.Precent = 0.5f;
			uitextPanel.Left.Pixels = 52f * (float)x - num * 0.5f;
			uitextPanel.Top.Pixels = 41f * (float)y;
			if (style)
			{
				this.StyleKey<object>(uitextPanel, false);
			}
			for (int i = 0; i < width; i++)
			{
				this._keyList[y * 10 + x + i] = uitextPanel;
			}
			return uitextPanel;
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x0059BF2C File Offset: 0x0059A12C
		private bool ShouldShowKeyboard()
		{
			return PlayerInput.SettingsForUI.ShowGamepadHints;
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x0059BF34 File Offset: 0x0059A134
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (Main.gameMenu)
			{
				if (this.ShouldShowKeyboard())
				{
					this.outerLayer2.Remove();
					if (!this.Elements.Contains(this.outerLayer1))
					{
						base.Append(this.outerLayer1);
					}
					this.outerLayer1.Activate();
					this.outerLayer2.Deactivate();
					this.Recalculate();
					this.RecalculateChildren();
					if (this._labelHeight != 0f)
					{
						this._textBox.Top.Pixels = this._textBoxHeight;
						this._label.Top.Pixels = this._labelHeight;
						this._textBox.Recalculate();
						this._label.Recalculate();
						this._labelHeight = (this._textBoxHeight = 0f);
						UserInterface.ActiveInstance.ResetLasts();
					}
				}
				else
				{
					this.outerLayer1.Remove();
					if (!this.Elements.Contains(this.outerLayer2))
					{
						base.Append(this.outerLayer2);
					}
					this.outerLayer2.Activate();
					this.outerLayer1.Deactivate();
					this.Recalculate();
					this.RecalculateChildren();
					if (this._textBoxHeight == 0f)
					{
						this._textBoxHeight = this._textBox.Top.Pixels;
						this._labelHeight = this._label.Top.Pixels;
						UITextBox textBox = this._textBox;
						textBox.Top.Pixels = textBox.Top.Pixels + 50f;
						UIText label = this._label;
						label.Top.Pixels = label.Top.Pixels + 50f;
						this._textBox.Recalculate();
						this._label.Recalculate();
						UserInterface.ActiveInstance.ResetLasts();
					}
				}
			}
			if (!Main.editSign && this._edittingSign)
			{
				IngameFancyUI.Close(false);
				return;
			}
			if (!Main.editChest && this._edittingChest)
			{
				IngameFancyUI.Close(false);
				return;
			}
			bool flag = this._textBox.GetOuterDimensions().Width > this._textBox.Parent.GetInnerDimensions().Width;
			this._textBox.HAlign = (flag ? 1f : 0.5f);
			this._textBox.Recalculate();
			base.DrawSelf(spriteBatch);
			this.UpdateOffsetDown();
			UIVirtualKeyboard.OffsetDown = 0;
			UIVirtualKeyboard.ShouldHideText = false;
			this.SetupGamepadPoints(spriteBatch);
			PlayerInput.WritingText = true;
			Main.instance.HandleIME();
			Vector2 position = new Vector2((float)(Main.screenWidth / 2), (float)(this._textBox.GetDimensions().ToRectangle().Bottom + 32));
			Main.instance.SetIMEPanelAnchor(position, 0.5f);
			string text = Main.GetInputText(this.Text, this._edittingSign);
			if (this._edittingSign && Main.inputTextEnter)
			{
				text += "\n";
			}
			else
			{
				if (this._edittingChest && Main.inputTextEnter)
				{
					ChestUI.RenameChestSubmit(Main.player[Main.myPlayer]);
					IngameFancyUI.Close(false);
					return;
				}
				if (Main.inputTextEnter && UIVirtualKeyboard.CanSubmit)
				{
					UIVirtualKeyboard.Submit();
				}
				else if (this._edittingChest && Main.player[Main.myPlayer].chest < 0)
				{
					ChestUI.RenameChestCancel();
				}
				else if (Main.inputTextEscape && this.TryEscapingMenu())
				{
					return;
				}
			}
			if (IngameFancyUI.CanShowVirtualKeyboard(this._keyboardContext))
			{
				if (text != this.Text)
				{
					if (this.CustomTextValidationForUpdate == null || this.CustomTextValidationForUpdate(text))
					{
						this.Text = text;
					}
					else
					{
						SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
					}
				}
				if (this._edittingSign)
				{
					this.CopyTextToSign();
				}
				if (this._edittingChest)
				{
					this.CopyTextToChest();
				}
			}
			byte b = (byte.MaxValue + Main.tileColor.R * 2) / 3;
			Color value = new Color((int)b, (int)b, (int)b, 255);
			this._textBox.TextColor = Color.Lerp(Color.White, value, 0.2f);
			this._label.TextColor = Color.Lerp(Color.White, value, 0.2f);
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x0059C33D File Offset: 0x0059A53D
		private bool TryEscapingMenu()
		{
			if (this._cancelAction != null)
			{
				UIVirtualKeyboard.Cancel();
				return true;
			}
			if (this._edittingSign)
			{
				Main.InputTextSignCancel();
			}
			if (this._edittingChest)
			{
				ChestUI.RenameChestCancel();
			}
			if (!Main.gameMenu)
			{
				IngameFancyUI.Close(false);
			}
			return true;
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x0059C378 File Offset: 0x0059A578
		private void UpdateOffsetDown()
		{
			this._textBox.HideSelf = UIVirtualKeyboard.ShouldHideText;
			int num = UIVirtualKeyboard.OffsetDown - this._lastOffsetDown;
			int num2 = num;
			if (Math.Abs(num) < 10)
			{
				num2 = num;
			}
			this._lastOffsetDown += num2;
			if (num2 == 0)
			{
				return;
			}
			this.Top.Pixels = this.Top.Pixels + (float)num2;
			this.Recalculate();
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x0059C3D9 File Offset: 0x0059A5D9
		public override void OnActivate()
		{
			this.SetupGamepadPoints(null);
			if (PlayerInput.UsingGamepadUI)
			{
				UILinkPointNavigator.ChangePoint(3002);
			}
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x0059C3F3 File Offset: 0x0059A5F3
		public override void OnDeactivate()
		{
			base.OnDeactivate();
			PlayerInput.WritingText = false;
			Main.instance.HandleIME();
			UILinkPointNavigator.Shortcuts.FANCYUI_SPECIAL_INSTRUCTIONS = 0;
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x0059C414 File Offset: 0x0059A614
		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 6;
			UILinkPointNavigator.Shortcuts.FANCYUI_SPECIAL_INSTRUCTIONS = 1;
			int num = 3002;
			int num2 = 5;
			int num3 = 10;
			int num4 = num3 * num2 - 1;
			int num5 = num3 * (num2 - 1);
			UILinkPointNavigator.SetPosition(3000, this._cancelButton.GetDimensions().Center());
			UILinkPoint uilinkPoint = UILinkPointNavigator.Points[3000];
			uilinkPoint.Unlink();
			uilinkPoint.Right = 3001;
			uilinkPoint.Up = num + num5;
			UILinkPointNavigator.SetPosition(3001, this._submitButton.GetDimensions().Center());
			uilinkPoint = UILinkPointNavigator.Points[3001];
			uilinkPoint.Unlink();
			uilinkPoint.Left = 3000;
			uilinkPoint.Up = num + num4;
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num3; j++)
				{
					int num6 = i * num3 + j;
					int num7 = num + num6;
					if (this._keyList[num6] != null)
					{
						UILinkPointNavigator.SetPosition(num7, this._keyList[num6].GetDimensions().Center());
						uilinkPoint = UILinkPointNavigator.Points[num7];
						uilinkPoint.Unlink();
						int num8 = j - 1;
						while (num8 >= 0 && this._keyList[i * num3 + num8] == this._keyList[num6])
						{
							num8--;
						}
						if (num8 != -1)
						{
							uilinkPoint.Left = i * num3 + num8 + num;
						}
						else
						{
							uilinkPoint.Left = i * num3 + (num3 - 1) + num;
						}
						int num9 = j + 1;
						while (num9 <= num3 - 1 && this._keyList[i * num3 + num9] == this._keyList[num6])
						{
							num9++;
						}
						if (num9 != num3 && this._keyList[num6] != this._keyList[num9])
						{
							uilinkPoint.Right = i * num3 + num9 + num;
						}
						else
						{
							uilinkPoint.Right = i * num3 + num;
						}
						if (i != 0)
						{
							uilinkPoint.Up = num7 - num3;
						}
						if (i != num2 - 1)
						{
							uilinkPoint.Down = num7 + num3;
						}
						else
						{
							uilinkPoint.Down = ((j < num2) ? 3000 : 3001);
						}
					}
				}
			}
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x0059C650 File Offset: 0x0059A850
		public static void CycleSymbols()
		{
			if (UIVirtualKeyboard._currentInstance == null)
			{
				return;
			}
			switch (UIVirtualKeyboard._currentInstance._keyState)
			{
			case UIVirtualKeyboard.KeyState.Default:
				UIVirtualKeyboard._currentInstance.SetKeyState(UIVirtualKeyboard.KeyState.Shift);
				return;
			case UIVirtualKeyboard.KeyState.Symbol:
				UIVirtualKeyboard._currentInstance.SetKeyState(UIVirtualKeyboard.KeyState.Default);
				return;
			case UIVirtualKeyboard.KeyState.Shift:
				UIVirtualKeyboard._currentInstance.SetKeyState(UIVirtualKeyboard.KeyState.Symbol);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x0059C6A6 File Offset: 0x0059A8A6
		public static void BackSpace()
		{
			if (UIVirtualKeyboard._currentInstance == null)
			{
				return;
			}
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			UIVirtualKeyboard._currentInstance._textBox.Backspace();
			UIVirtualKeyboard._currentInstance.ValidateText();
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06002C7F RID: 11391 RVA: 0x0059C6DE File Offset: 0x0059A8DE
		public static bool CanSubmit
		{
			get
			{
				return UIVirtualKeyboard._currentInstance != null && UIVirtualKeyboard._currentInstance._canSubmit;
			}
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x0059C6F3 File Offset: 0x0059A8F3
		public static void Submit()
		{
			if (UIVirtualKeyboard._currentInstance != null)
			{
				UIVirtualKeyboard._currentInstance.InternalSubmit();
			}
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x0059C708 File Offset: 0x0059A908
		private void InternalSubmit()
		{
			string text = this.Text.Trim();
			if (this.TextIsValidForSubmit())
			{
				SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
				this._submitAction(text);
			}
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x0059C74A File Offset: 0x0059A94A
		public static void Cancel()
		{
			if (UIVirtualKeyboard._currentInstance == null)
			{
				return;
			}
			SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
			UIVirtualKeyboard._currentInstance._cancelAction();
		}

		// Token: 0x06002C83 RID: 11395 RVA: 0x0059C778 File Offset: 0x0059A978
		public static void Write(string text)
		{
			if (UIVirtualKeyboard._currentInstance == null)
			{
				return;
			}
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			bool flag = UIVirtualKeyboard._currentInstance.Text.Length == 0;
			UIVirtualKeyboard._currentInstance._textBox.Write(text);
			UIVirtualKeyboard._currentInstance.ValidateText();
			if (flag && UIVirtualKeyboard._currentInstance.Text.Length > 0 && UIVirtualKeyboard._currentInstance._keyState == UIVirtualKeyboard.KeyState.Shift)
			{
				UIVirtualKeyboard._currentInstance.SetKeyState(UIVirtualKeyboard.KeyState.Default);
			}
		}

		// Token: 0x06002C84 RID: 11396 RVA: 0x0059C7FA File Offset: 0x0059A9FA
		public static void CursorLeft()
		{
			if (UIVirtualKeyboard._currentInstance == null)
			{
				return;
			}
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			UIVirtualKeyboard._currentInstance._textBox.CursorLeft();
		}

		// Token: 0x06002C85 RID: 11397 RVA: 0x0059C828 File Offset: 0x0059AA28
		public static void CursorRight()
		{
			if (UIVirtualKeyboard._currentInstance == null)
			{
				return;
			}
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			UIVirtualKeyboard._currentInstance._textBox.CursorRight();
		}

		// Token: 0x06002C86 RID: 11398 RVA: 0x0059C856 File Offset: 0x0059AA56
		public static bool CanDisplay(int keyboardContext)
		{
			return keyboardContext != 1 || Main.screenHeight > 700;
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06002C87 RID: 11399 RVA: 0x0059C86A File Offset: 0x0059AA6A
		public static int KeyboardContext
		{
			get
			{
				if (UIVirtualKeyboard._currentInstance == null)
				{
					return -1;
				}
				return UIVirtualKeyboard._currentInstance._keyboardContext;
			}
		}

		// Token: 0x06002C88 RID: 11400 RVA: 0x0059C87F File Offset: 0x0059AA7F
		public static void CacheCanceledInput(int cacheMode)
		{
			if (cacheMode == 1)
			{
				UIVirtualKeyboard._cancelCacheSign = Main.npcChatText;
			}
		}

		// Token: 0x06002C89 RID: 11401 RVA: 0x0059C88F File Offset: 0x0059AA8F
		private void RestoreCanceledInput(int cacheMode)
		{
			if (cacheMode == 1)
			{
				Main.npcChatText = UIVirtualKeyboard._cancelCacheSign;
				this.Text = Main.npcChatText;
				UIVirtualKeyboard._cancelCacheSign = "";
			}
		}

		// Token: 0x06002C8A RID: 11402 RVA: 0x0059C8B4 File Offset: 0x0059AAB4
		private void CopyTextToSign()
		{
			if (!this._edittingSign)
			{
				return;
			}
			int sign = Main.player[Main.myPlayer].sign;
			if (sign < 0 || Main.sign[sign] == null)
			{
				return;
			}
			Main.npcChatText = this.Text;
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x0059C8F4 File Offset: 0x0059AAF4
		private void CopyTextToChest()
		{
			if (!this._edittingChest)
			{
				return;
			}
			Main.npcChatText = this.Text;
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x0059C90C File Offset: 0x0059AB0C
		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x00587B9D File Offset: 0x00585D9D
		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.7f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		// Token: 0x040053E6 RID: 21478
		private static UIVirtualKeyboard _currentInstance;

		// Token: 0x040053E7 RID: 21479
		private static string _cancelCacheSign = "";

		// Token: 0x040053E8 RID: 21480
		private static string _cancelCacheChest = "";

		// Token: 0x040053E9 RID: 21481
		private const string DEFAULT_KEYS = "1234567890qwertyuiopasdfghjkl'zxcvbnm,.?";

		// Token: 0x040053EA RID: 21482
		private const string SHIFT_KEYS = "1234567890QWERTYUIOPASDFGHJKL'ZXCVBNM,.?";

		// Token: 0x040053EB RID: 21483
		private const string SYMBOL_KEYS = "1234567890!@#$%^&*()-_+=/\\{}[]<>;:\"`|~£¥";

		// Token: 0x040053EC RID: 21484
		private const float KEY_SPACING = 4f;

		// Token: 0x040053ED RID: 21485
		private const float KEY_WIDTH = 48f;

		// Token: 0x040053EE RID: 21486
		private const float KEY_HEIGHT = 37f;

		// Token: 0x040053EF RID: 21487
		private UITextPanel<object>[] _keyList = new UITextPanel<object>[50];

		// Token: 0x040053F0 RID: 21488
		private UITextPanel<object> _shiftButton;

		// Token: 0x040053F1 RID: 21489
		private UITextPanel<object> _symbolButton;

		// Token: 0x040053F2 RID: 21490
		private UITextBox _textBox;

		// Token: 0x040053F3 RID: 21491
		private UITextPanel<LocalizedText> _submitButton;

		// Token: 0x040053F4 RID: 21492
		private UITextPanel<LocalizedText> _cancelButton;

		// Token: 0x040053F5 RID: 21493
		private UIText _label;

		// Token: 0x040053F6 RID: 21494
		private UITextPanel<object> _enterButton;

		// Token: 0x040053F7 RID: 21495
		private UITextPanel<object> _spacebarButton;

		// Token: 0x040053F8 RID: 21496
		private UITextPanel<object> _restoreButton;

		// Token: 0x040053F9 RID: 21497
		private Asset<Texture2D> _textureShift;

		// Token: 0x040053FA RID: 21498
		private Asset<Texture2D> _textureBackspace;

		// Token: 0x040053FB RID: 21499
		private Color _internalBorderColor = new Color(89, 116, 213);

		// Token: 0x040053FC RID: 21500
		private Color _internalBorderColorSelected = Main.OurFavoriteColor;

		// Token: 0x040053FD RID: 21501
		private UITextPanel<LocalizedText> _submitButton2;

		// Token: 0x040053FE RID: 21502
		private UITextPanel<LocalizedText> _cancelButton2;

		// Token: 0x040053FF RID: 21503
		private UIElement outerLayer1;

		// Token: 0x04005400 RID: 21504
		private UIElement outerLayer2;

		// Token: 0x04005401 RID: 21505
		private bool _allowEmpty;

		// Token: 0x04005402 RID: 21506
		private UIVirtualKeyboard.KeyState _keyState;

		// Token: 0x04005403 RID: 21507
		private UIVirtualKeyboard.KeyboardSubmitEvent _submitAction;

		// Token: 0x04005404 RID: 21508
		private Action _cancelAction;

		// Token: 0x04005405 RID: 21509
		private int _lastOffsetDown;

		// Token: 0x04005406 RID: 21510
		public static int OffsetDown;

		// Token: 0x04005407 RID: 21511
		public static bool ShouldHideText;

		// Token: 0x04005408 RID: 21512
		private int _keyboardContext;

		// Token: 0x04005409 RID: 21513
		private bool _edittingSign;

		// Token: 0x0400540A RID: 21514
		private bool _edittingChest;

		// Token: 0x0400540B RID: 21515
		private float _textBoxHeight;

		// Token: 0x0400540C RID: 21516
		private float _labelHeight;

		// Token: 0x0400540D RID: 21517
		public Func<string, bool> CustomTextValidationForUpdate;

		// Token: 0x0400540E RID: 21518
		public Func<string, bool> CustomTextValidationForSubmit;

		// Token: 0x0400540F RID: 21519
		private bool _canSubmit;

		// Token: 0x02000915 RID: 2325
		// (Invoke) Token: 0x060047A9 RID: 18345
		public delegate void KeyboardSubmitEvent(string text);

		// Token: 0x02000916 RID: 2326
		public enum KeyState
		{
			// Token: 0x04007482 RID: 29826
			Default,
			// Token: 0x04007483 RID: 29827
			Symbol,
			// Token: 0x04007484 RID: 29828
			Shift
		}
	}
}
