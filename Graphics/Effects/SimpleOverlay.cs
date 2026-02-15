using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics.Effects
{
	// Token: 0x020001F6 RID: 502
	public class SimpleOverlay : Overlay
	{
		// Token: 0x060020B8 RID: 8376 RVA: 0x00522C8B File Offset: 0x00520E8B
		public SimpleOverlay(string textureName, ScreenShaderData shader, EffectPriority priority = EffectPriority.VeryLow, RenderLayers layer = RenderLayers.All) : base(priority, layer)
		{
			this._texture = Main.Assets.Request<Texture2D>((textureName == null) ? "" : textureName, 1);
			this._shader = shader;
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x00522CC4 File Offset: 0x00520EC4
		public SimpleOverlay(string textureName, string shaderName = "Default", EffectPriority priority = EffectPriority.VeryLow, RenderLayers layer = RenderLayers.All) : base(priority, layer)
		{
			this._texture = Main.Assets.Request<Texture2D>((textureName == null) ? "" : textureName, 1);
			this._shader = new ScreenShaderData(Main.ScreenShaderRef, shaderName);
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x00522D12 File Offset: 0x00520F12
		public ScreenShaderData GetShader()
		{
			return this._shader;
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x00522D1C File Offset: 0x00520F1C
		public override void Draw(SpriteBatch spriteBatch)
		{
			this._shader.UseGlobalOpacity(this.Opacity);
			this._shader.UseTargetPosition(this.TargetPosition);
			this._shader.Apply();
			spriteBatch.Draw(this._texture.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Main.ColorOfTheSkies);
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x00522D7F File Offset: 0x00520F7F
		public override void Update(GameTime gameTime)
		{
			this._shader.Update(gameTime);
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x00522D8D File Offset: 0x00520F8D
		public override void Activate(Vector2 position, params object[] args)
		{
			this.TargetPosition = position;
			this.Mode = OverlayMode.FadeIn;
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x00522D9D File Offset: 0x00520F9D
		public override void Deactivate(params object[] args)
		{
			this.Mode = OverlayMode.FadeOut;
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x00522DA6 File Offset: 0x00520FA6
		public override bool IsVisible()
		{
			return this._shader.CombinedOpacity > 0f;
		}

		// Token: 0x04004B16 RID: 19222
		private Asset<Texture2D> _texture;

		// Token: 0x04004B17 RID: 19223
		private ScreenShaderData _shader;

		// Token: 0x04004B18 RID: 19224
		public Vector2 TargetPosition = Vector2.Zero;
	}
}
