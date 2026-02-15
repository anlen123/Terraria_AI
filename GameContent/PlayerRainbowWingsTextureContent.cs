using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent
{
	// Token: 0x02000255 RID: 597
	public class PlayerRainbowWingsTextureContent : ARenderTargetContentByRequest
	{
		// Token: 0x06002331 RID: 9009 RVA: 0x0053C434 File Offset: 0x0053A634
		protected override void HandleUseRequest(GraphicsDevice device, SpriteBatch spriteBatch)
		{
			Asset<Texture2D> asset = TextureAssets.Extra[171];
			base.PrepareARenderTarget_AndListenToEvents(ref this._target, device, asset.Width(), asset.Height(), RenderTargetUsage.PreserveContents);
			device.SetRenderTarget(this._target);
			device.Clear(Color.Transparent);
			DrawData value = new DrawData(asset.Value, Vector2.Zero, Color.White);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			GameShaders.Misc["HallowBoss"].Apply(new DrawData?(value));
			value.Draw(spriteBatch);
			spriteBatch.End();
			device.SetRenderTarget(null);
			this._wasPrepared = true;
		}
	}
}
