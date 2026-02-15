using System;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent
{
	// Token: 0x02000250 RID: 592
	public interface INeedRenderTargetContent
	{
		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06002319 RID: 8985
		bool IsReady { get; }

		// Token: 0x0600231A RID: 8986
		void PrepareRenderTarget(GraphicsDevice device, SpriteBatch spriteBatch);

		// Token: 0x0600231B RID: 8987
		void Reset();
	}
}
