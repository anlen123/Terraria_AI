using System;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x020001FF RID: 511
	public interface IParticle
	{
		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06002111 RID: 8465
		bool ShouldBeRemovedFromRenderer { get; }

		// Token: 0x06002112 RID: 8466
		void Update(ref ParticleRendererSettings settings);

		// Token: 0x06002113 RID: 8467
		void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch);
	}
}
