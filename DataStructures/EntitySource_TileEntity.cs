using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000583 RID: 1411
	public class EntitySource_TileEntity : IEntitySource
	{
		// Token: 0x060037EE RID: 14318 RVA: 0x0062F5D4 File Offset: 0x0062D7D4
		public EntitySource_TileEntity(TileEntity tileEntity)
		{
			this.TileEntity = tileEntity;
		}

		// Token: 0x04005C07 RID: 23559
		public TileEntity TileEntity;
	}
}
