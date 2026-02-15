using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000569 RID: 1385
	public class EntitySource_ItemUse_WithAmmo : EntitySource_ItemUse
	{
		// Token: 0x060037D4 RID: 14292 RVA: 0x0062F4C4 File Offset: 0x0062D6C4
		public EntitySource_ItemUse_WithAmmo(IEntitySourceTarget entity, Item item, int ammoItemIdUsed) : base(entity, item)
		{
			this.AmmoItemIdUsed = ammoItemIdUsed;
		}

		// Token: 0x04005BF5 RID: 23541
		public readonly int AmmoItemIdUsed;
	}
}
