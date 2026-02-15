using System;

namespace Terraria.DataStructures
{
	// Token: 0x0200056A RID: 1386
	public class EntitySource_Mount : IEntitySource
	{
		// Token: 0x060037D5 RID: 14293 RVA: 0x0062F4D5 File Offset: 0x0062D6D5
		public EntitySource_Mount(IEntitySourceTarget entity, int mountId)
		{
			this.Entity = entity;
			this.MountId = mountId;
		}

		// Token: 0x04005BF6 RID: 23542
		public readonly IEntitySourceTarget Entity;

		// Token: 0x04005BF7 RID: 23543
		public readonly int MountId;
	}
}
