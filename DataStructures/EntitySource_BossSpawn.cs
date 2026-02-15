using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000579 RID: 1401
	public class EntitySource_BossSpawn : IEntitySource
	{
		// Token: 0x060037E4 RID: 14308 RVA: 0x0062F551 File Offset: 0x0062D751
		public EntitySource_BossSpawn(IEntitySourceTarget entity)
		{
			this.Entity = entity;
		}

		// Token: 0x04005BFE RID: 23550
		public readonly IEntitySourceTarget Entity;
	}
}
