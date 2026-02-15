using System;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x02000205 RID: 517
	public interface IPooledParticle : IParticle
	{
		// Token: 0x1700033A RID: 826
		// (get) Token: 0x0600211D RID: 8477
		bool IsRestingInPool { get; }

		// Token: 0x0600211E RID: 8478
		void RestInPool();

		// Token: 0x0600211F RID: 8479
		void FetchFromPool();
	}
}
