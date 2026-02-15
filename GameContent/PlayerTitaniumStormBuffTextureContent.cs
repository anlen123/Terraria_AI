using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent
{
	// Token: 0x02000257 RID: 599
	public class PlayerTitaniumStormBuffTextureContent : ARenderTargetContentByRequest
	{
		// Token: 0x06002335 RID: 9013 RVA: 0x0053C583 File Offset: 0x0053A783
		public PlayerTitaniumStormBuffTextureContent()
		{
			this._shaderData = new MiscShaderData(Main.PixelShaderRef, "TitaniumStorm");
			this._shaderData.UseImage1("Images/Extra_" + 156);
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x0053C5C0 File Offset: 0x0053A7C0
		protected override void HandleUseRequest(GraphicsDevice device, SpriteBatch spriteBatch)
		{
			Main.instance.LoadProjectile(908);
			Asset<Texture2D> asset = TextureAssets.Projectile[908];
			this.UpdateSettingsForRendering(0.6f, 0f, Main.GlobalTimeWrappedHourly, 0.3f);
			base.PrepareARenderTarget_AndListenToEvents(ref this._target, device, asset.Width(), asset.Height(), RenderTargetUsage.PreserveContents);
			device.SetRenderTarget(this._target);
			device.Clear(Color.Transparent);
			DrawData value = new DrawData(asset.Value, Vector2.Zero, Color.White);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			this._shaderData.Apply(new DrawData?(value));
			value.Draw(spriteBatch);
			spriteBatch.End();
			device.SetRenderTarget(null);
			this._wasPrepared = true;
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x0053C683 File Offset: 0x0053A883
		public void UpdateSettingsForRendering(float gradientContributionFromOriginalTexture, float gradientScrollingSpeed, float flatGradientOffset, float gradientColorDominance)
		{
			this._shaderData.UseColor(gradientScrollingSpeed, gradientContributionFromOriginalTexture, gradientColorDominance);
			this._shaderData.UseOpacity(flatGradientOffset);
		}

		// Token: 0x04004D39 RID: 19769
		private MiscShaderData _shaderData;
	}
}
