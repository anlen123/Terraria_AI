using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003DD RID: 989
	public class UIBestiaryInfoLine<T> : UIElement, IManuallyOrderedUIElement
	{
		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06002DE7 RID: 11751 RVA: 0x005A6C76 File Offset: 0x005A4E76
		// (set) Token: 0x06002DE8 RID: 11752 RVA: 0x005A6C7E File Offset: 0x005A4E7E
		public int OrderInUIList { get; set; }

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06002DE9 RID: 11753 RVA: 0x005A6C87 File Offset: 0x005A4E87
		// (set) Token: 0x06002DEA RID: 11754 RVA: 0x005A6C8F File Offset: 0x005A4E8F
		public float TextScale
		{
			get
			{
				return this._textScale;
			}
			set
			{
				this._textScale = value;
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06002DEB RID: 11755 RVA: 0x005A6C98 File Offset: 0x005A4E98
		public Vector2 TextSize
		{
			get
			{
				return this._textSize;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06002DEC RID: 11756 RVA: 0x005A6CA0 File Offset: 0x005A4EA0
		public string Text
		{
			get
			{
				if (this._text != null)
				{
					return this._text.ToString();
				}
				return "";
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06002DED RID: 11757 RVA: 0x005A6CC6 File Offset: 0x005A4EC6
		// (set) Token: 0x06002DEE RID: 11758 RVA: 0x005A6CCE File Offset: 0x005A4ECE
		public Color TextColor
		{
			get
			{
				return this._color;
			}
			set
			{
				this._color = value;
			}
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x005A6CD7 File Offset: 0x005A4ED7
		public UIBestiaryInfoLine(T text, float textScale = 1f)
		{
			this.SetText(text, textScale);
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x005A6D08 File Offset: 0x005A4F08
		public override void Recalculate()
		{
			this.SetText(this._text, this._textScale);
			base.Recalculate();
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x005A6D22 File Offset: 0x005A4F22
		public void SetText(T text)
		{
			this.SetText(text, this._textScale);
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x005A6D34 File Offset: 0x005A4F34
		public virtual void SetText(T text, float textScale)
		{
			Vector2 vector = new Vector2(FontAssets.MouseText.Value.MeasureString(text.ToString()).X, 16f) * textScale;
			this._text = text;
			this._textScale = textScale;
			this._textSize = vector;
			this.MinWidth.Set(vector.X + this.PaddingLeft + this.PaddingRight, 0f);
			this.MinHeight.Set(vector.Y + this.PaddingTop + this.PaddingBottom, 0f);
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x005A6DD0 File Offset: 0x005A4FD0
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			Vector2 pos = innerDimensions.Position();
			pos.Y -= 2f * this._textScale;
			pos.X += (innerDimensions.Width - this._textSize.X) * 0.5f;
			Utils.DrawBorderString(spriteBatch, this.Text, pos, this._color, this._textScale, 0f, 0f, -1);
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x005A6E4C File Offset: 0x005A504C
		public override int CompareTo(object obj)
		{
			IManuallyOrderedUIElement manuallyOrderedUIElement = obj as IManuallyOrderedUIElement;
			if (manuallyOrderedUIElement != null)
			{
				return this.OrderInUIList.CompareTo(manuallyOrderedUIElement.OrderInUIList);
			}
			return base.CompareTo(obj);
		}

		// Token: 0x040054F2 RID: 21746
		private T _text;

		// Token: 0x040054F3 RID: 21747
		private float _textScale = 1f;

		// Token: 0x040054F4 RID: 21748
		private Vector2 _textSize = Vector2.Zero;

		// Token: 0x040054F5 RID: 21749
		private Color _color = Color.White;
	}
}
