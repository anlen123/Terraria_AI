using System;

namespace Terraria.DataStructures
{
	// Token: 0x0200057C RID: 1404
	public class EntitySource_Gift : IEntitySource
	{
		// Token: 0x060037E7 RID: 14311 RVA: 0x0062F57E File Offset: 0x0062D77E
		public EntitySource_Gift(Entity entity)
		{
			this.Entity = entity;
		}

		// Token: 0x04005C01 RID: 23553
		public readonly IEntitySourceTarget Entity;
	}
}
