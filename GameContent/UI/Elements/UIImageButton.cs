using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003FF RID: 1023
	public class UIImageButton : UIElement
	{
		// Token: 0x06002EF2 RID: 12018 RVA: 0x005AFE68 File Offset: 0x005AE068
		public UIImageButton(Asset<Texture2D> texture, Rectangle? frame = null)
		{
			this._texture = texture;
			this._frame = frame;
			this.Width.Set((float)((frame != null) ? frame.Value.Width : this._texture.Width()), 0f);
			this.Height.Set((float)((frame != null) ? frame.Value.Height : this._texture.Height()), 0f);
		}

		// Token: 0x06002EF3 RID: 12019 RVA: 0x005AFF1B File Offset: 0x005AE11B
		public void SetHoverImage(Asset<Texture2D> texture, Rectangle? frame = null)
		{
			this._borderTexture = texture;
			this._borderFrame = frame;
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x005AFF2C File Offset: 0x005AE12C
		public void SetImage(Asset<Texture2D> texture, Rectangle? frame = null)
		{
			this._texture = texture;
			this.Width.Set((float)((this._frame != null) ? this._frame.Value.Width : this._texture.Width()), 0f);
			this.Height.Set((float)((this._frame != null) ? this._frame.Value.Height : this._texture.Height()), 0f);
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x005AFFB8 File Offset: 0x005AE1B8
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			spriteBatch.Draw(this._texture.Value, dimensions.Position(), this._frame, this.Color * (base.IsMouseHovering ? this._visibilityActive : this._visibilityInactive));
			if (this._borderTexture != null && base.IsMouseHovering)
			{
				spriteBatch.Draw(this._borderTexture.Value, dimensions.Position(), this._borderFrame, this.BorderColor);
			}
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x005A7487 File Offset: 0x005A5687
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x005B003F File Offset: 0x005AE23F
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
		}

		// Token: 0x06002EF8 RID: 12024 RVA: 0x005B0048 File Offset: 0x005AE248
		public void SetVisibility(float whenActive, float whenInactive)
		{
			this._visibilityActive = MathHelper.Clamp(whenActive, 0f, 1f);
			this._visibilityInactive = MathHelper.Clamp(whenInactive, 0f, 1f);
		}

		// Token: 0x04005604 RID: 22020
		private Asset<Texture2D> _texture;

		// Token: 0x04005605 RID: 22021
		protected float _visibilityActive = 1f;

		// Token: 0x04005606 RID: 22022
		protected float _visibilityInactive = 0.4f;

		// Token: 0x04005607 RID: 22023
		private Asset<Texture2D> _borderTexture;

		// Token: 0x04005608 RID: 22024
		private Rectangle? _frame;

		// Token: 0x04005609 RID: 22025
		private Rectangle? _borderFrame;

		// Token: 0x0400560A RID: 22026
		public Color Color = Color.White;

		// Token: 0x0400560B RID: 22027
		public Color BorderColor = Color.White;
	}
}
