using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x02000219 RID: 537
	public class OutlinedTextureRenderer : INeedRenderTargetContent
	{
		// Token: 0x060021AC RID: 8620 RVA: 0x0053185E File Offset: 0x0052FA5E
		public OutlinedTextureRenderer(Asset<Texture2D>[] matchingArray)
		{
			this._matchingArray = matchingArray;
			this.Reset();
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x00531873 File Offset: 0x0052FA73
		public void Reset()
		{
			this._contents = new OutlinedDrawRenderTargetContent[this._matchingArray.Length];
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x00531888 File Offset: 0x0052FA88
		public void DrawWithOutlines(int textureIndex, Vector2 position, Color color, float rotation, float scale, SpriteEffects effects)
		{
			if (this._contents[textureIndex] == null)
			{
				this._contents[textureIndex] = new OutlinedDrawRenderTargetContent();
				this._contents[textureIndex].SetTexture(this._matchingArray[textureIndex].Value);
			}
			OutlinedDrawRenderTargetContent outlinedDrawRenderTargetContent = this._contents[textureIndex];
			if (outlinedDrawRenderTargetContent.IsReady)
			{
				RenderTarget2D target = outlinedDrawRenderTargetContent.GetTarget();
				Main.spriteBatch.Draw(target, position, null, color, rotation, target.Size() / 2f, scale, effects, 0f);
				return;
			}
			outlinedDrawRenderTargetContent.Request();
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060021AF RID: 8623 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public bool IsReady
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x00531918 File Offset: 0x0052FB18
		public bool RequestAndTryGet(int textureIndex, out RenderTarget2D renderTarget)
		{
			renderTarget = null;
			if (this._contents[textureIndex] == null)
			{
				this._contents[textureIndex] = new OutlinedDrawRenderTargetContent();
				this._contents[textureIndex].SetTexture(this._matchingArray[textureIndex].Value);
			}
			OutlinedDrawRenderTargetContent outlinedDrawRenderTargetContent = this._contents[textureIndex];
			if (!outlinedDrawRenderTargetContent.IsReady)
			{
				outlinedDrawRenderTargetContent.Request();
				return false;
			}
			renderTarget = outlinedDrawRenderTargetContent.GetTarget();
			return true;
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x0053197C File Offset: 0x0052FB7C
		public void PrepareRenderTarget(GraphicsDevice device, SpriteBatch spriteBatch)
		{
			for (int i = 0; i < this._contents.Length; i++)
			{
				if (this._contents[i] != null && !this._contents[i].IsReady)
				{
					this._contents[i].PrepareRenderTarget(device, spriteBatch);
				}
			}
		}

		// Token: 0x04004C17 RID: 19479
		private OutlinedDrawRenderTargetContent[] _contents;

		// Token: 0x04004C18 RID: 19480
		private Asset<Texture2D>[] _matchingArray;
	}
}
