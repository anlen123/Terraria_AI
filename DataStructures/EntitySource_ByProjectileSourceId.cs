using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000572 RID: 1394
	public class EntitySource_ByProjectileSourceId : IEntitySource
	{
		// Token: 0x060037DD RID: 14301 RVA: 0x0062F52C File Offset: 0x0062D72C
		public EntitySource_ByProjectileSourceId(int projectileSourceId)
		{
			this.SourceId = projectileSourceId;
		}

		// Token: 0x04005BFB RID: 23547
		public readonly int SourceId;
	}
}
