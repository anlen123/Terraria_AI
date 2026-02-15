using System;

namespace Terraria.DataStructures
{
	// Token: 0x0200056C RID: 1388
	public class EntitySource_TileInteraction : AEntitySource_Tile
	{
		// Token: 0x060037D7 RID: 14295 RVA: 0x0062F500 File Offset: 0x0062D700
		public EntitySource_TileInteraction(IEntitySourceTarget entity, int tileCoordsX, int tileCoordsY) : base(tileCoordsX, tileCoordsY)
		{
			this.Entity = entity;
		}

		// Token: 0x04005BF9 RID: 23545
		public readonly IEntitySourceTarget Entity;
	}
}
