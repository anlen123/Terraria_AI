using System;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	// Token: 0x0200056B RID: 1387
	public abstract class AEntitySource_Tile : IEntitySource
	{
		// Token: 0x060037D6 RID: 14294 RVA: 0x0062F4EB File Offset: 0x0062D6EB
		public AEntitySource_Tile(int tileCoordsX, int tileCoordsY)
		{
			this.TileCoords = new Point(tileCoordsX, tileCoordsY);
		}

		// Token: 0x04005BF8 RID: 23544
		public readonly Point TileCoords;
	}
}
