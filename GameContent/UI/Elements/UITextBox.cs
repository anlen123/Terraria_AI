using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Localization.IME;
using ReLogic.OS;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003F9 RID: 1017
	internal class UITextBox : UITextPanel<string>
	{
		// Token: 0x06002EB4 RID: 11956 RVA: 0x005AD589 File Offset: 0x005AB789
		public UITextBox(string text, float textScale = 1f, bool large = false) : base(text, textScale, large)
		{
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x005AD5A3 File Offset: 0x005AB7A3
		public void Write(string text)
		{
			base.SetText(base.Text.Insert(this._cursor, text));
			this._cursor += text.Length;
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x005AD5D0 File Offset: 0x005AB7D0
		public override void SetText(string text, float textScale, bool large)
		{
			text = Utils.TrimUserString(text ?? "", this._maxLength);
			base.SetText(text, textScale, large);
			this._cursor = Math.Min(base.Text.Length, this._cursor);
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x005AD60E File Offset: 0x005AB80E
		public void SetTextMaxLength(int maxLength)
		{
			this._maxLength = maxLength;
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x005AD617 File Offset: 0x005AB817
		public void Backspace()
		{
			if (this._cursor == 0)
			{
				return;
			}
			base.SetText(Utils.TrimLastCharacter(base.Text));
		}

		// Token: 0x06002EB9 RID: 11961 RVA: 0x005AD633 File Offset: 0x005AB833
		public void CursorLeft()
		{
			if (this._cursor == 0)
			{
				return;
			}
			this._cursor--;
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x005AD64C File Offset: 0x005AB84C
		public void CursorRight()
		{
			if (this._cursor < base.Text.Length)
			{
				this._cursor++;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06002EBB RID: 11963 RVA: 0x005AD670 File Offset: 0x005AB870
		protected override Vector2 TextDrawPosition
		{
			get
			{
				Vector2 textDrawPosition = base.TextDrawPosition;
				if (this.ShowInputTicker)
				{
					string compositionString = Platform.Get<IImeService>().CompositionString;
					if (!string.IsNullOrEmpty(compositionString))
					{
						textDrawPosition.X -= base.Font.MeasureString(compositionString).X * base.TextScale * this.TextHAlign;
					}
				}
				return textDrawPosition;
			}
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x005AD6CC File Offset: 0x005AB8CC
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (this.HideSelf)
			{
				return;
			}
			this._cursor = base.Text.Length;
			base.DrawSelf(spriteBatch);
			if (!this.ShowInputTicker)
			{
				return;
			}
			Vector2 textDrawPosition = this.TextDrawPosition;
			string compositionString = Platform.Get<IImeService>().CompositionString;
			if (!string.IsNullOrEmpty(compositionString))
			{
				textDrawPosition.X += base.Font.MeasureString(compositionString).X * base.TextScale;
				base.DrawText(spriteBatch, compositionString, this.TextDrawPosition + new Vector2(base.TextSize.X, 0f), Main.imeCompositionStringColor);
			}
			this._frameCount++;
			if ((this._frameCount %= 40) > 20)
			{
				return;
			}
			textDrawPosition.X += base.Font.MeasureString(base.Text.Substring(0, this._cursor)).X * base.TextScale;
			textDrawPosition.X += 6f - (base.IsLarge ? 8f : 4f) * base.TextScale;
			if (base.IsLarge)
			{
				textDrawPosition.Y += 2f * base.TextScale;
			}
			base.DrawText(spriteBatch, "|", textDrawPosition, base.TextColor);
		}

		// Token: 0x040055C9 RID: 21961
		private int _cursor;

		// Token: 0x040055CA RID: 21962
		private int _frameCount;

		// Token: 0x040055CB RID: 21963
		private int _maxLength = 20;

		// Token: 0x040055CC RID: 21964
		public bool ShowInputTicker = true;

		// Token: 0x040055CD RID: 21965
		public bool HideSelf;
	}
}
