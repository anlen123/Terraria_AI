using System;

namespace Terraria.DataStructures
{
	// Token: 0x0200057F RID: 1407
	public class EntitySource_OnHit_ByProjectileSourceID : AEntitySource_OnHit
	{
		// Token: 0x060037EA RID: 14314 RVA: 0x0062F5B2 File Offset: 0x0062D7B2
		public EntitySource_OnHit_ByProjectileSourceID(IEntitySourceTarget entityStriking, IEntitySourceTarget entityStruck, int projectileSourceId) : base(entityStriking, entityStruck)
		{
			this.SourceId = projectileSourceId;
		}

		// Token: 0x04005C05 RID: 23557
		public readonly int SourceId;
	}
}
