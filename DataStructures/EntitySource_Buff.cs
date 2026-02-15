using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000566 RID: 1382
	public class EntitySource_Buff : IEntitySource
	{
		// Token: 0x060037D1 RID: 14289 RVA: 0x0062F47B File Offset: 0x0062D67B
		public EntitySource_Buff(IEntitySourceTarget entity, int buffId, int buffIndex)
		{
			this.Entity = entity;
			this.BuffId = buffId;
			this.BuffIndex = buffIndex;
		}

		// Token: 0x04005BEE RID: 23534
		public readonly IEntitySourceTarget Entity;

		// Token: 0x04005BEF RID: 23535
		public readonly int BuffId;

		// Token: 0x04005BF0 RID: 23536
		public readonly int BuffIndex;
	}
}
