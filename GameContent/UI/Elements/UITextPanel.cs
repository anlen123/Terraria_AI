using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x02000406 RID: 1030
	public class UITextPanel<T> : UIPanel
	{
		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06002F3D RID: 12093 RVA: 0x005B13E0 File Offset: 0x005AF5E0
		public bool IsLarge
		{
			get
			{
				return this._isLarge;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06002F3E RID: 12094 RVA: 0x005B13E8 File Offset: 0x005AF5E8
		// (set) Token: 0x06002F3F RID: 12095 RVA: 0x005B13F0 File Offset: 0x005AF5F0
		public bool DrawPanel
		{
			get
			{
				return this._drawPanel;
			}
			set
			{
				this._drawPanel = value;
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06002F40 RID: 12096 RVA: 0x005B13F9 File Offset: 0x005AF5F9
		// (set) Token: 0x06002F41 RID: 12097 RVA: 0x005B1401 File Offset: 0x005AF601
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

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06002F42 RID: 12098 RVA: 0x005B140A File Offset: 0x005AF60A
		public Vector2 TextSize
		{
			get
			{
				return this._textSize;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06002F43 RID: 12099 RVA: 0x005B1412 File Offset: 0x005AF612
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

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06002F44 RID: 12100 RVA: 0x005B1438 File Offset: 0x005AF638
		// (set) Token: 0x06002F45 RID: 12101 RVA: 0x005B1440 File Offset: 0x005AF640
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

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06002F46 RID: 12102 RVA: 0x005B1449 File Offset: 0x005AF649
		protected DynamicSpriteFont Font
		{
			get
			{
				if (!this._isLarge)
				{
					return FontAssets.MouseText.Value;
				}
				return FontAssets.DeathText.Value;
			}
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x005B1468 File Offset: 0x005AF668
		public UITextPanel(T text, float textScale = 1f, bool large = false)
		{
			this.SetText(text, textScale, large);
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x005B14B7 File Offset: 0x005AF6B7
		public override void Recalculate()
		{
			this.SetText(this._text, this._textScale, this._isLarge);
			base.Recalculate();
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x005B14D7 File Offset: 0x005AF6D7
		public void SetText(T text)
		{
			this.SetText(text, this._textScale, this._isLarge);
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x005B14EC File Offset: 0x005AF6EC
		public virtual void SetText(T text, float textScale, bool large)
		{
			this._text = text;
			this._textScale = textScale;
			this._isLarge = large;
			this._textSize = new Vector2(this.Font.MeasureString(text.ToString()).X, large ? 32f : 16f) * textScale;
			this.MinWidth.Set(this._textSize.X + this.PaddingLeft + this.PaddingRight, 0f);
			this.MinHeight.Set(this._textSize.Y + this.PaddingTop + this.PaddingBottom, 0f);
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x005B159D File Offset: 0x005AF79D
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (this._drawPanel)
			{
				base.DrawSelf(spriteBatch);
			}
			this.DrawText(spriteBatch);
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06002F4C RID: 12108 RVA: 0x005B15B8 File Offset: 0x005AF7B8
		protected virtual Vector2 TextDrawPosition
		{
			get
			{
				CalculatedStyle innerDimensions = base.GetInnerDimensions();
				Vector2 result = innerDimensions.Position();
				result.X += (innerDimensions.Width - this._textSize.X) * this.TextHAlign;
				if (this._isLarge)
				{
					result.Y -= 10f * this._textScale * this._textScale;
				}
				else
				{
					result.Y -= 2f * this._textScale;
				}
				return result;
			}
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x005B1638 File Offset: 0x005AF838
		protected void DrawText(SpriteBatch spriteBatch)
		{
			string text = this.Text;
			if (this.HideContents)
			{
				if (this._asterisks == null || this._asterisks.Length != text.Length)
				{
					this._asterisks = new string('*', text.Length);
				}
				text = this._asterisks;
			}
			this.DrawText(spriteBatch, text, this.TextDrawPosition, this._color);
		}

		// Token: 0x06002F4E RID: 12110 RVA: 0x005B16A0 File Offset: 0x005AF8A0
		protected void DrawText(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
		{
			if (this._isLarge)
			{
				Utils.DrawBorderStringBig(spriteBatch, text, position, color, this._textScale, 0f, 0f, -1);
				return;
			}
			Utils.DrawBorderString(spriteBatch, text, position, color, this._textScale, 0f, 0f, -1);
		}

		// Token: 0x04005633 RID: 22067
		protected T _text;

		// Token: 0x04005634 RID: 22068
		protected float _textScale = 1f;

		// Token: 0x04005635 RID: 22069
		protected Vector2 _textSize = Vector2.Zero;

		// Token: 0x04005636 RID: 22070
		protected bool _isLarge;

		// Token: 0x04005637 RID: 22071
		protected Color _color = Color.White;

		// Token: 0x04005638 RID: 22072
		protected bool _drawPanel = true;

		// Token: 0x04005639 RID: 22073
		public float TextHAlign = 0.5f;

		// Token: 0x0400563A RID: 22074
		public bool HideContents;

		// Token: 0x0400563B RID: 22075
		private string _asterisks;
	}
}
