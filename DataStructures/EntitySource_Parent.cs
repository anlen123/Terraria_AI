using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000565 RID: 1381
	public class EntitySource_Parent : IEntitySource
	{
		// Token: 0x060037D0 RID: 14288 RVA: 0x0062F46C File Offset: 0x0062D66C
		public EntitySource_Parent(IEntitySourceTarget entity)
		{
			this.Entity = entity;
		}

		// Token: 0x04005BED RID: 23533
		public readonly IEntitySourceTarget Entity;
	}
}
