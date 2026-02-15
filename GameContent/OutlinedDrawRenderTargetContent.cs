using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent
{
	// Token: 0x02000254 RID: 596
	public class OutlinedDrawRenderTargetContent : AnOutlinedDrawRenderTargetContent
	{
		// Token: 0x0600232E RID: 9006 RVA: 0x0053C3A7 File Offset: 0x0053A5A7
		public void SetTexture(Texture2D texture)
		{
			if (this._theTexture == texture)
			{
				return;
			}
			this._theTexture = texture;
			this._wasPrepared = false;
			this.width = texture.Width + 8;
			this.height = texture.Height + 8;
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x0053C3E0 File Offset: 0x0053A5E0
		internal override void DrawTheContent(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this._theTexture, new Vector2(4f, 4f), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}

		// Token: 0x04004D38 RID: 19768
		private Texture2D _theTexture;
	}
}
