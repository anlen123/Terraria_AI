using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000568 RID: 1384
	public class EntitySource_ItemOpen : IEntitySource
	{
		// Token: 0x060037D3 RID: 14291 RVA: 0x0062F4AE File Offset: 0x0062D6AE
		public EntitySource_ItemOpen(IEntitySourceTarget entity, int itemType)
		{
			this.Entity = entity;
			this.ItemType = itemType;
		}

		// Token: 0x04005BF3 RID: 23539
		public readonly IEntitySourceTarget Entity;

		// Token: 0x04005BF4 RID: 23540
		public readonly int ItemType;
	}
}
