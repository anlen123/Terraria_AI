using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x02000405 RID: 1029
	public class UIText : UIElement
	{
		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06002F24 RID: 12068 RVA: 0x005B0E66 File Offset: 0x005AF066
		public string Text
		{
			get
			{
				return this._text.ToString();
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06002F25 RID: 12069 RVA: 0x005B0E73 File Offset: 0x005AF073
		// (set) Token: 0x06002F26 RID: 12070 RVA: 0x005B0E7B File Offset: 0x005AF07B
		public float TextOriginX { get; set; }

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06002F27 RID: 12071 RVA: 0x005B0E84 File Offset: 0x005AF084
		// (set) Token: 0x06002F28 RID: 12072 RVA: 0x005B0E8C File Offset: 0x005AF08C
		public float TextOriginY { get; set; }

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06002F29 RID: 12073 RVA: 0x005B0E95 File Offset: 0x005AF095
		// (set) Token: 0x06002F2A RID: 12074 RVA: 0x005B0E9D File Offset: 0x005AF09D
		public float WrappedTextBottomPadding { get; set; }

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06002F2B RID: 12075 RVA: 0x005B0EA6 File Offset: 0x005AF0A6
		// (set) Token: 0x06002F2C RID: 12076 RVA: 0x005B0EAE File Offset: 0x005AF0AE
		public bool IsWrapped
		{
			get
			{
				return this._isWrapped;
			}
			set
			{
				this._isWrapped = value;
				this.InternalSetText(this._text, this._textScale, this._isLarge);
			}
		}

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x06002F2D RID: 12077 RVA: 0x005B0ED0 File Offset: 0x005AF0D0
		// (remove) Token: 0x06002F2E RID: 12078 RVA: 0x005B0F08 File Offset: 0x005AF108
		public event Action OnInternalTextChange;

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06002F2F RID: 12079 RVA: 0x005B0F3D File Offset: 0x005AF13D
		// (set) Token: 0x06002F30 RID: 12080 RVA: 0x005B0F45 File Offset: 0x005AF145
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

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06002F31 RID: 12081 RVA: 0x005B0F4E File Offset: 0x005AF14E
		// (set) Token: 0x06002F32 RID: 12082 RVA: 0x005B0F56 File Offset: 0x005AF156
		public Color ShadowColor
		{
			get
			{
				return this._shadowColor;
			}
			set
			{
				this._shadowColor = value;
			}
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x005B0F60 File Offset: 0x005AF160
		public UIText(string text, float textScale = 1f, bool large = false)
		{
			this.TextOriginX = 0.5f;
			this.TextOriginY = 0f;
			this.IsWrapped = false;
			this.WrappedTextBottomPadding = 20f;
			this.InternalSetText(text, textScale, large);
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x005B0FDC File Offset: 0x005AF1DC
		public UIText(LocalizedText text, float textScale = 1f, bool large = false)
		{
			this.TextOriginX = 0.5f;
			this.TextOriginY = 0f;
			this.IsWrapped = false;
			this.WrappedTextBottomPadding = 20f;
			this.InternalSetText(text, textScale, large);
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x005B1057 File Offset: 0x005AF257
		public override void Recalculate()
		{
			this.InternalSetText(this._text, this._textScale, this._isLarge);
			base.Recalculate();
		}

		// Token: 0x06002F36 RID: 12086 RVA: 0x005B1077 File Offset: 0x005AF277
		public void SetText(string text)
		{
			this.InternalSetText(text, this._textScale, this._isLarge);
		}

		// Token: 0x06002F37 RID: 12087 RVA: 0x005B1077 File Offset: 0x005AF277
		public void SetText(LocalizedText text)
		{
			this.InternalSetText(text, this._textScale, this._isLarge);
		}

		// Token: 0x06002F38 RID: 12088 RVA: 0x005B108C File Offset: 0x005AF28C
		public void SetText(string text, float textScale, bool large)
		{
			this.InternalSetText(text, textScale, large);
		}

		// Token: 0x06002F39 RID: 12089 RVA: 0x005B108C File Offset: 0x005AF28C
		public void SetText(LocalizedText text, float textScale, bool large)
		{
			this.InternalSetText(text, textScale, large);
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x005B1098 File Offset: 0x005AF298
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			this.VerifyTextState();
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			Vector2 position = innerDimensions.Position();
			if (this._isLarge)
			{
				position.Y -= 10f * this._textScale;
			}
			else
			{
				position.Y -= 2f * this._textScale;
			}
			List<PositionedSnippet> list = this._textLayout;
			Vector2 vector = new Vector2(this._textScale);
			Vector2 vector2 = this._textSize;
			if (this.DynamicallyScaleDownToWidth && vector2.X > innerDimensions.Width)
			{
				float num = innerDimensions.Width / vector2.X;
				list = new List<PositionedSnippet>();
				for (int i = 0; i < list.Count; i++)
				{
					list[i].Scale(num);
				}
				vector *= num;
				vector2 *= num;
			}
			position.X += (innerDimensions.Width - vector2.X) * this.TextOriginX;
			position.Y += (innerDimensions.Height - vector2.Y) * this.TextOriginY;
			Color shadowColor = this._shadowColor * ((float)this._color.A / 255f);
			DynamicSpriteFont font = this._isLarge ? FontAssets.DeathText.Value : FontAssets.MouseText.Value;
			ChatManager.DrawColorCodedStringShadow(spriteBatch, font, this._textLayout, position, shadowColor, 0f, Vector2.Zero, vector, 1.5f);
			int num2;
			ChatManager.DrawColorCodedString(spriteBatch, font, this._textLayout, position, 0f, Vector2.Zero, vector, out num2, null);
		}

		// Token: 0x06002F3B RID: 12091 RVA: 0x005B1244 File Offset: 0x005AF444
		private void VerifyTextState()
		{
			if (this._lastTextReference == this.Text)
			{
				return;
			}
			this.InternalSetText(this._text, this._textScale, this._isLarge);
		}

		// Token: 0x06002F3C RID: 12092 RVA: 0x005B1270 File Offset: 0x005AF470
		private void InternalSetText(object text, float textScale, bool large)
		{
			this._text = text;
			this._isLarge = large;
			this._textScale = textScale;
			this._lastTextReference = this._text.ToString();
			List<TextSnippet> snippets = ChatManager.ParseMessage(this._lastTextReference, this._color);
			ChatManager.ConvertNormalSnippets(snippets);
			DynamicSpriteFont font = large ? FontAssets.DeathText.Value : FontAssets.MouseText.Value;
			this._textLayout = ChatManager.LayoutSnippets(font, snippets, new Vector2(this._textScale), this.IsWrapped ? base.GetInnerDimensions().Width : -1f).ToList<PositionedSnippet>();
			this._textSize = ChatManager.GetStringSize(this._textLayout);
			if (this.IsWrapped)
			{
				this._textSize.Y = this._textSize.Y + this.WrappedTextBottomPadding * this._textScale;
			}
			else
			{
				this._textSize.Y = (large ? 32f : 16f) * this._textScale;
			}
			this.MinWidth.Set((this.IsWrapped || this.DynamicallyScaleDownToWidth) ? 0f : (this._textSize.X + this.PaddingLeft + this.PaddingRight), 0f);
			this.MinHeight.Set(this._textSize.Y + this.PaddingTop + this.PaddingBottom, 0f);
			if (this.OnInternalTextChange != null)
			{
				this.OnInternalTextChange();
			}
		}

		// Token: 0x04005625 RID: 22053
		private object _text = "";

		// Token: 0x04005626 RID: 22054
		private float _textScale = 1f;

		// Token: 0x04005627 RID: 22055
		private Vector2 _textSize = Vector2.Zero;

		// Token: 0x04005628 RID: 22056
		private bool _isLarge;

		// Token: 0x04005629 RID: 22057
		private Color _color = Color.White;

		// Token: 0x0400562A RID: 22058
		private Color _shadowColor = Color.Black;

		// Token: 0x0400562B RID: 22059
		private bool _isWrapped;

		// Token: 0x0400562F RID: 22063
		public bool DynamicallyScaleDownToWidth;

		// Token: 0x04005630 RID: 22064
		private List<PositionedSnippet> _textLayout;

		// Token: 0x04005631 RID: 22065
		private string _lastTextReference;
	}
}
