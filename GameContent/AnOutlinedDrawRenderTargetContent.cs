using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent
{
	// Token: 0x02000252 RID: 594
	public abstract class AnOutlinedDrawRenderTargetContent : ARenderTargetContentByRequest
	{
		// Token: 0x06002327 RID: 8999 RVA: 0x0053C0AE File Offset: 0x0053A2AE
		public void UseColor(Color color)
		{
			this._borderColor = color;
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x0053C0B8 File Offset: 0x0053A2B8
		protected override void HandleUseRequest(GraphicsDevice device, SpriteBatch spriteBatch)
		{
			Effect pixelShader = Main.pixelShader;
			EffectPass effectPass = pixelShader.CurrentTechnique.Passes["ColorOnly"];
			new Rectangle(0, 0, this.width, this.height);
			base.PrepareARenderTarget_AndListenToEvents(ref this._target, device, this.width, this.height, RenderTargetUsage.PreserveContents);
			base.PrepareARenderTarget_WithoutListeningToEvents(ref this._helperTarget, device, this.width, this.height, RenderTargetUsage.DiscardContents);
			device.SetRenderTarget(this._helperTarget);
			device.Clear(Color.Transparent);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);
			this.DrawTheContent(spriteBatch);
			spriteBatch.End();
			device.SetRenderTarget(this._target);
			device.Clear(Color.Transparent);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);
			effectPass.Apply();
			int num = 2;
			int num2 = num * 2;
			for (int i = -num2; i <= num2; i += num)
			{
				for (int j = -num2; j <= num2; j += num)
				{
					if (Math.Abs(i) + Math.Abs(j) == num2)
					{
						spriteBatch.Draw(this._helperTarget, new Vector2((float)i, (float)j), Color.Black);
					}
				}
			}
			num2 = num;
			for (int k = -num2; k <= num2; k += num)
			{
				for (int l = -num2; l <= num2; l += num)
				{
					if (Math.Abs(k) + Math.Abs(l) == num2)
					{
						spriteBatch.Draw(this._helperTarget, new Vector2((float)k, (float)l), this._borderColor);
					}
				}
			}
			pixelShader.CurrentTechnique.Passes[0].Apply();
			spriteBatch.Draw(this._helperTarget, Vector2.Zero, Color.White);
			spriteBatch.End();
			device.SetRenderTarget(null);
			this._wasPrepared = true;
		}

		// Token: 0x06002329 RID: 9001
		internal abstract void DrawTheContent(SpriteBatch spriteBatch);

		// Token: 0x04004D30 RID: 19760
		protected int width = 84;

		// Token: 0x04004D31 RID: 19761
		protected int height = 84;

		// Token: 0x04004D32 RID: 19762
		public Color _borderColor = Color.White;

		// Token: 0x04004D33 RID: 19763
		private RenderTarget2D _helperTarget;
	}
}
