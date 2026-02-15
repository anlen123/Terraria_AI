using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000573 RID: 1395
	public class EntitySource_ByItemSourceId : IEntitySource
	{
		// Token: 0x060037DE RID: 14302 RVA: 0x0062F53B File Offset: 0x0062D73B
		public EntitySource_ByItemSourceId(IEntitySourceTarget entity, int itemSourceId)
		{
			this.Entity = entity;
			this.SourceId = itemSourceId;
		}

		// Token: 0x04005BFC RID: 23548
		public readonly IEntitySourceTarget Entity;

		// Token: 0x04005BFD RID: 23549
		public readonly int SourceId;
	}
}
