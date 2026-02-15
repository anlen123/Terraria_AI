using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics;

namespace Terraria.DataStructures
{
	// Token: 0x0200055C RID: 1372
	public struct SpriteBatchBeginner
	{
		// Token: 0x060037AD RID: 14253 RVA: 0x0062ED9D File Offset: 0x0062CF9D
		public SpriteBatchBeginner(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix transformMatrix)
		{
			this.sortMode = sortMode;
			this.blendState = blendState;
			this.samplerState = samplerState;
			this.depthStencilState = depthStencilState;
			this.rasterizerState = rasterizerState;
			this.effect = effect;
			this.transformMatrix = transformMatrix;
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x0062EDD4 File Offset: 0x0062CFD4
		public void Begin(SpriteBatch spriteBatch)
		{
			spriteBatch.Begin(this.sortMode, this.blendState, this.samplerState, this.depthStencilState, this.rasterizerState, this.effect, this.transformMatrix);
		}

		// Token: 0x060037AF RID: 14255 RVA: 0x0062EE06 File Offset: 0x0062D006
		public void Begin(SpriteBatch spriteBatch, SpriteSortMode sortMode)
		{
			spriteBatch.Begin(sortMode, this.blendState, this.samplerState, this.depthStencilState, this.rasterizerState, this.effect, this.transformMatrix);
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x0062EE33 File Offset: 0x0062D033
		public void Begin(TileBatch tileBatch)
		{
			tileBatch.Begin(this.rasterizerState, this.transformMatrix);
		}

		// Token: 0x04005BA9 RID: 23465
		private SpriteSortMode sortMode;

		// Token: 0x04005BAA RID: 23466
		private BlendState blendState;

		// Token: 0x04005BAB RID: 23467
		private SamplerState samplerState;

		// Token: 0x04005BAC RID: 23468
		private DepthStencilState depthStencilState;

		// Token: 0x04005BAD RID: 23469
		private RasterizerState rasterizerState;

		// Token: 0x04005BAE RID: 23470
		private Effect effect;

		// Token: 0x04005BAF RID: 23471
		public Matrix transformMatrix;
	}
}
