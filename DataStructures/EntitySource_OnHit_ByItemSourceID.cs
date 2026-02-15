using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000580 RID: 1408
	public class EntitySource_OnHit_ByItemSourceID : AEntitySource_OnHit
	{
		// Token: 0x060037EB RID: 14315 RVA: 0x0062F5C3 File Offset: 0x0062D7C3
		public EntitySource_OnHit_ByItemSourceID(IEntitySourceTarget entityStriking, IEntitySourceTarget entityStruck, int itemSourceId) : base(entityStriking, entityStruck)
		{
			this.SourceId = itemSourceId;
		}

		// Token: 0x04005C06 RID: 23558
		public readonly int SourceId;
	}
}
