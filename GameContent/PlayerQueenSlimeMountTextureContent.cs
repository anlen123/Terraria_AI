using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent
{
	// Token: 0x02000256 RID: 598
	public class PlayerQueenSlimeMountTextureContent : ARenderTargetContentByRequest
	{
		// Token: 0x06002333 RID: 9011 RVA: 0x0053C4E0 File Offset: 0x0053A6E0
		protected override void HandleUseRequest(GraphicsDevice device, SpriteBatch spriteBatch)
		{
			Asset<Texture2D> asset = TextureAssets.Extra[204];
			base.PrepareARenderTarget_AndListenToEvents(ref this._target, device, asset.Width(), asset.Height(), RenderTargetUsage.PreserveContents);
			device.SetRenderTarget(this._target);
			device.Clear(Color.Transparent);
			DrawData value = new DrawData(asset.Value, Vector2.Zero, Color.White);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			GameShaders.Misc["QueenSlime"].Apply(new DrawData?(value));
			value.Draw(spriteBatch);
			spriteBatch.End();
			device.SetRenderTarget(null);
			this._wasPrepared = true;
		}
	}
}
