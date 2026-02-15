using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003FE RID: 1022
	public class UIImage : UIElement
	{
		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06002EEB RID: 12011 RVA: 0x005AFBF5 File Offset: 0x005ADDF5
		public Asset<Texture2D> Texture
		{
			get
			{
				return this._texture;
			}
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x005AFBFD File Offset: 0x005ADDFD
		protected UIImage()
		{
		}

		// Token: 0x06002EED RID: 12013 RVA: 0x005AFC34 File Offset: 0x005ADE34
		public UIImage(Asset<Texture2D> texture)
		{
			this.SetImage(texture);
		}

		// Token: 0x06002EEE RID: 12014 RVA: 0x005AFC72 File Offset: 0x005ADE72
		public UIImage(Texture2D nonReloadingTexture)
		{
			this.SetImage(nonReloadingTexture);
		}

		// Token: 0x06002EEF RID: 12015 RVA: 0x005AFCB0 File Offset: 0x005ADEB0
		public void SetImage(Asset<Texture2D> texture)
		{
			this._texture = texture;
			this._nonReloadingTexture = null;
			if (this.AllowResizingDimensions)
			{
				this.Width.Set((float)this._texture.Width(), 0f);
				this.Height.Set((float)this._texture.Height(), 0f);
			}
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x005AFD0C File Offset: 0x005ADF0C
		public void SetImage(Texture2D nonReloadingTexture)
		{
			this._texture = null;
			this._nonReloadingTexture = nonReloadingTexture;
			if (this.AllowResizingDimensions)
			{
				this.Width.Set((float)this._nonReloadingTexture.Width, 0f);
				this.Height.Set((float)this._nonReloadingTexture.Height, 0f);
			}
		}

		// Token: 0x06002EF1 RID: 12017 RVA: 0x005AFD68 File Offset: 0x005ADF68
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			Texture2D texture2D = null;
			if (this._texture != null)
			{
				texture2D = this._texture.Value;
			}
			if (this._nonReloadingTexture != null)
			{
				texture2D = this._nonReloadingTexture;
			}
			if (this.ScaleToFit)
			{
				spriteBatch.Draw(texture2D, dimensions.ToRectangle(), this.Frame, this.Color);
				return;
			}
			Vector2 vector = texture2D.Size();
			Vector2 vector2 = new Vector2(dimensions.Width, dimensions.Height);
			if (this.UseTextureSizeForOrigin)
			{
				vector2 = vector;
			}
			Vector2 vector3 = dimensions.Position() + vector2 * (1f - this.ImageScale) / 2f + vector2 * this.NormalizedOrigin;
			if (this.RemoveFloatingPointsFromDrawPosition)
			{
				vector3 = vector3.Floor();
			}
			spriteBatch.Draw(texture2D, vector3, this.Frame, this.Color, this.Rotation, vector * this.NormalizedOrigin, this.ImageScale, SpriteEffects.None, 0f);
		}

		// Token: 0x040055F9 RID: 22009
		private Asset<Texture2D> _texture;

		// Token: 0x040055FA RID: 22010
		public float ImageScale = 1f;

		// Token: 0x040055FB RID: 22011
		public float Rotation;

		// Token: 0x040055FC RID: 22012
		public bool ScaleToFit;

		// Token: 0x040055FD RID: 22013
		public bool AllowResizingDimensions = true;

		// Token: 0x040055FE RID: 22014
		public Color Color = Color.White;

		// Token: 0x040055FF RID: 22015
		public Vector2 NormalizedOrigin = Vector2.Zero;

		// Token: 0x04005600 RID: 22016
		public Rectangle? Frame;

		// Token: 0x04005601 RID: 22017
		public bool RemoveFloatingPointsFromDrawPosition;

		// Token: 0x04005602 RID: 22018
		public bool UseTextureSizeForOrigin = true;

		// Token: 0x04005603 RID: 22019
		private Texture2D _nonReloadingTexture;
	}
}
