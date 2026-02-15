using System;

namespace Terraria.DataStructures
{
	// Token: 0x0200057B RID: 1403
	public class EntitySource_FishedOut : IEntitySource
	{
		// Token: 0x060037E6 RID: 14310 RVA: 0x0062F56F File Offset: 0x0062D76F
		public EntitySource_FishedOut(IEntitySourceTarget entity)
		{
			this.Entity = entity;
		}

		// Token: 0x04005C00 RID: 23552
		public readonly IEntitySourceTarget Entity;
	}
}
