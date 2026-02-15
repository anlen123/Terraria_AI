using System;

namespace Terraria.DataStructures
{
	// Token: 0x0200056D RID: 1389
	public class EntitySource_OverfullChest : AEntitySource_Tile
	{
		// Token: 0x060037D8 RID: 14296 RVA: 0x0062F511 File Offset: 0x0062D711
		public EntitySource_OverfullChest(int tileCoordsX, int tileCoordsY, Chest chest) : base(tileCoordsX, tileCoordsY)
		{
			this.Chest = chest;
		}

		// Token: 0x04005BFA RID: 23546
		public readonly Chest Chest;
	}
}
