using System;

namespace Terraria.DataStructures
{
	// Token: 0x0200057A RID: 1402
	public class EntitySource_DropAsItem : IEntitySource
	{
		// Token: 0x060037E5 RID: 14309 RVA: 0x0062F560 File Offset: 0x0062D760
		public EntitySource_DropAsItem(IEntitySourceTarget entity)
		{
			this.Entity = entity;
		}

		// Token: 0x04005BFF RID: 23551
		public readonly IEntitySourceTarget Entity;
	}
}
