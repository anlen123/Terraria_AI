using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003C9 RID: 969
	public class UIImageWithBorder : UIImage
	{
		// Token: 0x06002D57 RID: 11607 RVA: 0x005A2A2B File Offset: 0x005A0C2B
		public UIImageWithBorder(Asset<Texture2D> texture, Asset<Texture2D> borderTexture) : base(texture)
		{
			this.SetBorder(borderTexture);
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x005A2A3B File Offset: 0x005A0C3B
		public UIImageWithBorder(Texture2D nonReloadingTexture, Texture2D nonReloadingBorderTexture) : base(nonReloadingTexture)
		{
			this.SetBorder(nonReloadingBorderTexture);
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x005A2A4C File Offset: 0x005A0C4C
		public void SetBorder(Asset<Texture2D> texture)
		{
			this._borderTexture = texture;
			this._nonReloadingBorderTexture = null;
			this.Width.Set((float)this._borderTexture.Width(), 0f);
			this.Height.Set((float)this._borderTexture.Height(), 0f);
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x005A2AA0 File Offset: 0x005A0CA0
		public void SetBorder(Texture2D nonReloadingTexture)
		{
			this._borderTexture = null;
			this._nonReloadingBorderTexture = nonReloadingTexture;
			this.Width.Set((float)this._nonReloadingBorderTexture.Width, 0f);
			this.Height.Set((float)this._nonReloadingBorderTexture.Height, 0f);
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x005A2AF4 File Offset: 0x005A0CF4
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			CalculatedStyle dimensions = base.GetDimensions();
			Texture2D texture2D = null;
			if (this._borderTexture != null)
			{
				texture2D = this._borderTexture.Value;
			}
			if (this._nonReloadingBorderTexture != null)
			{
				texture2D = this._nonReloadingBorderTexture;
			}
			if (this.ScaleToFit)
			{
				spriteBatch.Draw(texture2D, dimensions.ToRectangle(), this.Color);
				return;
			}
			Vector2 vector = texture2D.Size();
			Vector2 vector2 = dimensions.Position() + vector * (1f - this.ImageScale) / 2f + vector * this.NormalizedOrigin;
			if (this.RemoveFloatingPointsFromDrawPosition)
			{
				vector2 = vector2.Floor();
			}
			spriteBatch.Draw(texture2D, vector2, null, this.Color, this.Rotation, vector * this.NormalizedOrigin, this.ImageScale, SpriteEffects.None, 0f);
		}

		// Token: 0x04005497 RID: 21655
		private Asset<Texture2D> _borderTexture;

		// Token: 0x04005498 RID: 21656
		private Texture2D _nonReloadingBorderTexture;
	}
}
