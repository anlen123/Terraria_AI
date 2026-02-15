using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x02000407 RID: 1031
	public class UIToggleImage : UIElement
	{
		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06002F4F RID: 12111 RVA: 0x005B16EE File Offset: 0x005AF8EE
		public bool IsOn
		{
			get
			{
				return this._isOn;
			}
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x005B16F8 File Offset: 0x005AF8F8
		public UIToggleImage(Asset<Texture2D> texture, int width, int height, Point onTextureOffset, Point offTextureOffset)
		{
			this._onTexture = texture;
			this._offTexture = texture;
			this._offTextureOffset = offTextureOffset;
			this._onTextureOffset = onTextureOffset;
			this._drawWidth = width;
			this._drawHeight = height;
			this.Width.Set((float)width, 0f);
			this.Height.Set((float)height, 0f);
		}

		// Token: 0x06002F51 RID: 12113 RVA: 0x005B1774 File Offset: 0x005AF974
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			Texture2D value;
			Point point;
			if (this._isOn)
			{
				value = this._onTexture.Value;
				point = this._onTextureOffset;
			}
			else
			{
				value = this._offTexture.Value;
				point = this._offTextureOffset;
			}
			Color color = base.IsMouseHovering ? Color.White : Color.Silver;
			spriteBatch.Draw(value, new Rectangle((int)dimensions.X, (int)dimensions.Y, this._drawWidth, this._drawHeight), new Rectangle?(new Rectangle(point.X, point.Y, this._drawWidth, this._drawHeight)), color);
		}

		// Token: 0x06002F52 RID: 12114 RVA: 0x005B1816 File Offset: 0x005AFA16
		public override void LeftClick(UIMouseEvent evt)
		{
			this.Toggle();
			base.LeftClick(evt);
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x005B1825 File Offset: 0x005AFA25
		public void SetState(bool value)
		{
			this._isOn = value;
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x005B182E File Offset: 0x005AFA2E
		public void Toggle()
		{
			this._isOn = !this._isOn;
		}

		// Token: 0x0400563C RID: 22076
		private Asset<Texture2D> _onTexture;

		// Token: 0x0400563D RID: 22077
		private Asset<Texture2D> _offTexture;

		// Token: 0x0400563E RID: 22078
		private int _drawWidth;

		// Token: 0x0400563F RID: 22079
		private int _drawHeight;

		// Token: 0x04005640 RID: 22080
		private Point _onTextureOffset = Point.Zero;

		// Token: 0x04005641 RID: 22081
		private Point _offTextureOffset = Point.Zero;

		// Token: 0x04005642 RID: 22082
		private bool _isOn;
	}
}
