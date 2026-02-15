using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000567 RID: 1383
	public class EntitySource_ItemUse : IEntitySource
	{
		// Token: 0x060037D2 RID: 14290 RVA: 0x0062F498 File Offset: 0x0062D698
		public EntitySource_ItemUse(IEntitySourceTarget entity, Item item)
		{
			this.Entity = entity;
			this.Item = item;
		}

		// Token: 0x04005BF1 RID: 23537
		public readonly IEntitySourceTarget Entity;

		// Token: 0x04005BF2 RID: 23538
		public readonly Item Item;
	}
}
