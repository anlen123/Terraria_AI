using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.Drawing
{
	// Token: 0x02000439 RID: 1081
	public class BackgroundGradientDrawer
	{
		// Token: 0x060030A7 RID: 12455 RVA: 0x005BB847 File Offset: 0x005B9A47
		public BackgroundGradientDrawer(Color gradientColor, GetBackgroundDrawWeightMethod weightGetter, BackgroundArrayGetterMethod textureGetter, params int[] textureIndexesToCheck)
		{
			this._color = gradientColor;
			this._weightGetter = weightGetter;
			this._textureGetter = textureGetter;
			this._textureIndexesToCheck = textureIndexesToCheck;
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x005BB86C File Offset: 0x005B9A6C
		public void Draw()
		{
			if (!Main.BackgroundEnabled)
			{
				return;
			}
			float num = this._weightGetter();
			if (num <= 0f)
			{
				return;
			}
			if (!this.ShouldDrawForTextures())
			{
				return;
			}
			if (!Main.ShouldDrawSurfaceBackground())
			{
				return;
			}
			if (BackgroundGradientDrawer._sunflareGradientDitherTexture == null)
			{
				BackgroundGradientDrawer._sunflareGradientDitherTexture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/colorgradientdither", 1);
			}
			SpriteBatch spriteBatch = Main.spriteBatch;
			Color value = new Color(this._color.ToVector3() * Main.ColorOfSurfaceBackgroundsBase.ToVector3());
			spriteBatch.Draw(BackgroundGradientDrawer._sunflareGradientDitherTexture.Value, BackgroundGradientDrawer.GetGradientRect(), null, value * num, 0f, Vector2.Zero, SpriteEffects.None, 0f);
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x005BB920 File Offset: 0x005B9B20
		private static Rectangle GetGradientRect()
		{
			int num = 400;
			int y = Math.Max(0, (int)((Main.worldSurface * 16.0 - (double)Main.screenPosition.Y - 2400.0) * 0.10000000149011612)) - num;
			return new Rectangle(0, y, Main.screenWidth, Main.screenHeight + num);
		}

		// Token: 0x060030AA RID: 12458 RVA: 0x005BB980 File Offset: 0x005B9B80
		private bool ShouldDrawForTextures()
		{
			IEnumerable<int> enumerable = this._textureGetter();
			foreach (int num in this._textureIndexesToCheck)
			{
				foreach (int num2 in enumerable)
				{
					if (num == num2)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040056D4 RID: 22228
		private Color _color;

		// Token: 0x040056D5 RID: 22229
		private GetBackgroundDrawWeightMethod _weightGetter;

		// Token: 0x040056D6 RID: 22230
		private BackgroundArrayGetterMethod _textureGetter;

		// Token: 0x040056D7 RID: 22231
		private int[] _textureIndexesToCheck;

		// Token: 0x040056D8 RID: 22232
		private static Asset<Texture2D> _sunflareGradientDitherTexture;
	}
}
