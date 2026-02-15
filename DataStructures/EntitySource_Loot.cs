using System;

namespace Terraria.DataStructures
{
	// Token: 0x0200057D RID: 1405
	public class EntitySource_Loot : IEntitySource
	{
		// Token: 0x060037E8 RID: 14312 RVA: 0x0062F58D File Offset: 0x0062D78D
		public EntitySource_Loot(IEntitySourceTarget entity)
		{
			this.Entity = entity;
		}

		// Token: 0x04005C02 RID: 23554
		public readonly IEntitySourceTarget Entity;
	}
}
