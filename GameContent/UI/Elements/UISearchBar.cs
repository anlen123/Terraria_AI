using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003DF RID: 991
	public class UISearchBar : UIElement
	{
		// Token: 0x14000053 RID: 83
		// (add) Token: 0x06002DF7 RID: 11767 RVA: 0x005A6F14 File Offset: 0x005A5114
		// (remove) Token: 0x06002DF8 RID: 11768 RVA: 0x005A6F4C File Offset: 0x005A514C
		public event Action<string> OnContentsChanged;

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x06002DF9 RID: 11769 RVA: 0x005A6F84 File Offset: 0x005A5184
		// (remove) Token: 0x06002DFA RID: 11770 RVA: 0x005A6FBC File Offset: 0x005A51BC
		public event Action OnStartTakingInput;

		// Token: 0x14000055 RID: 85
		// (add) Token: 0x06002DFB RID: 11771 RVA: 0x005A6FF4 File Offset: 0x005A51F4
		// (remove) Token: 0x06002DFC RID: 11772 RVA: 0x005A702C File Offset: 0x005A522C
		public event Action OnEndTakingInput;

		// Token: 0x14000056 RID: 86
		// (add) Token: 0x06002DFD RID: 11773 RVA: 0x005A7064 File Offset: 0x005A5264
		// (remove) Token: 0x06002DFE RID: 11774 RVA: 0x005A709C File Offset: 0x005A529C
		public event Action OnNeedingVirtualKeyboard;

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06002DFF RID: 11775 RVA: 0x005A70D1 File Offset: 0x005A52D1
		public bool HasContents
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this.actualContents);
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06002E00 RID: 11776 RVA: 0x005A70E1 File Offset: 0x005A52E1
		// (set) Token: 0x06002E01 RID: 11777 RVA: 0x005A70E9 File Offset: 0x005A52E9
		public bool IsWritingText { get; private set; }

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06002E02 RID: 11778 RVA: 0x005A70F2 File Offset: 0x005A52F2
		// (set) Token: 0x06002E03 RID: 11779 RVA: 0x005A70FA File Offset: 0x005A52FA
		public int MaxInputLength
		{
			get
			{
				return this._maxInputLength;
			}
			set
			{
				this._maxInputLength = value;
				this._text.SetTextMaxLength(this._maxInputLength);
			}
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x005A7114 File Offset: 0x005A5314
		public UISearchBar(LocalizedText emptyContentText, float scale)
		{
			this._textToShowWhenEmpty = emptyContentText;
			this._textScale = scale;
			this._text = new UITextBox("", scale, false)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				BackgroundColor = Color.Transparent,
				BorderColor = Color.Transparent,
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(0f, 1f),
				TextHAlign = 0f,
				ShowInputTicker = false
			};
			this.MaxInputLength = 50;
			base.Append(this._text);
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x005A71C4 File Offset: 0x005A53C4
		public void SetContents(string contents, bool forced = false)
		{
			if (this.actualContents == contents && !forced)
			{
				return;
			}
			this.actualContents = contents;
			if (string.IsNullOrEmpty(this.actualContents))
			{
				this._text.TextColor = Color.Gray;
				this._text.SetText(this._textToShowWhenEmpty.Value, this._textScale, false);
			}
			else
			{
				this._text.TextColor = Color.White;
				this._text.SetText(this.actualContents);
				this.actualContents = this._text.Text;
			}
			this.TrimDisplayIfOverElementDimensions(0);
			if (this.OnContentsChanged != null)
			{
				this.OnContentsChanged(contents);
			}
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x005A7274 File Offset: 0x005A5474
		public void TrimDisplayIfOverElementDimensions(int padding)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			if (dimensions.Width == 0f && dimensions.Height == 0f)
			{
				return;
			}
			Point point = new Point((int)dimensions.X, (int)dimensions.Y);
			Point point2 = new Point(point.X + (int)dimensions.Width, point.Y + (int)dimensions.Height);
			Rectangle rectangle = new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
			CalculatedStyle dimensions2 = this._text.GetDimensions();
			Point point3 = new Point((int)dimensions2.X, (int)dimensions2.Y);
			Point point4 = new Point(point3.X + (int)this._text.MinWidth.Pixels, point3.Y + (int)this._text.MinHeight.Pixels);
			Rectangle rectangle2 = new Rectangle(point3.X, point3.Y, point4.X - point3.X, point4.Y - point3.Y);
			while (rectangle2.Right > rectangle.Right - padding && this._text.Text.Length > 0)
			{
				this._text.SetText(Utils.TrimLastCharacter(this._text.Text));
				this.RecalculateChildren();
				dimensions2 = this._text.GetDimensions();
				point3 = new Point((int)dimensions2.X, (int)dimensions2.Y);
				point4 = new Point(point3.X + (int)this._text.MinWidth.Pixels, point3.Y + (int)this._text.MinHeight.Pixels);
				rectangle2 = new Rectangle(point3.X, point3.Y, point4.X - point3.X, point4.Y - point3.Y);
				this.actualContents = this._text.Text;
			}
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x005A7487 File Offset: 0x005A5687
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x005A74A5 File Offset: 0x005A56A5
		public override void Update(GameTime gameTime)
		{
			if (this.IsWritingText)
			{
				if (this.NeedsVirtualkeyboard())
				{
					if (this.OnNeedingVirtualKeyboard != null)
					{
						this.OnNeedingVirtualKeyboard();
					}
					return;
				}
				PlayerInput.WritingText = true;
				Main.CurrentInputTextTakerOverride = this;
			}
			base.Update(gameTime);
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x0059BF2C File Offset: 0x0059A12C
		private bool NeedsVirtualkeyboard()
		{
			return PlayerInput.SettingsForUI.ShowGamepadHints;
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x005A74E0 File Offset: 0x005A56E0
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			if (!this.IsWritingText)
			{
				return;
			}
			PlayerInput.WritingText = true;
			Main.instance.HandleIME();
			Rectangle rectangle = this._text.GetDimensions().ToRectangle();
			Vector2 position = new Vector2((float)rectangle.Left, (float)(rectangle.Bottom + 32));
			Main.instance.SetIMEPanelAnchor(position, 0f);
			string inputText = Main.GetInputText(this.actualContents, false);
			if (Main.inputTextEnter)
			{
				this.ToggleTakingText();
			}
			else if (Main.inputTextEscape)
			{
				Main.inputTextEscape = false;
				this.ToggleTakingText();
			}
			this.SetContents(inputText, false);
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x005A7584 File Offset: 0x005A5784
		public void ToggleTakingText()
		{
			this.IsWritingText = !this.IsWritingText;
			this._text.ShowInputTicker = this.IsWritingText;
			Main.clrInput();
			if (this.IsWritingText)
			{
				if (this.OnStartTakingInput != null)
				{
					this.OnStartTakingInput();
					return;
				}
			}
			else
			{
				if (this.OnEndTakingInput != null)
				{
					this.OnEndTakingInput();
				}
				PlayerInput.WritingText = false;
				Main.instance.HandleIME();
			}
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x005A75F5 File Offset: 0x005A57F5
		public override void OnDeactivate()
		{
			if (this.IsWritingText)
			{
				this.ToggleTakingText();
			}
		}

		// Token: 0x040054F9 RID: 21753
		private readonly LocalizedText _textToShowWhenEmpty;

		// Token: 0x040054FA RID: 21754
		private UITextBox _text;

		// Token: 0x040054FB RID: 21755
		private string actualContents;

		// Token: 0x040054FC RID: 21756
		private float _textScale;

		// Token: 0x04005502 RID: 21762
		private int _maxInputLength;
	}
}
