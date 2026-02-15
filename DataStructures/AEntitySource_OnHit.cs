using System;

namespace Terraria.DataStructures
{
	// Token: 0x0200057E RID: 1406
	public abstract class AEntitySource_OnHit : IEntitySource
	{
		// Token: 0x060037E9 RID: 14313 RVA: 0x0062F59C File Offset: 0x0062D79C
		public AEntitySource_OnHit(IEntitySourceTarget entityStriking, IEntitySourceTarget entityStruck)
		{
			this.EntityStriking = entityStriking;
			this.EntityStruck = entityStruck;
		}

		// Token: 0x04005C03 RID: 23555
		public readonly IEntitySourceTarget EntityStriking;

		// Token: 0x04005C04 RID: 23556
		public readonly IEntitySourceTarget EntityStruck;
	}
}
