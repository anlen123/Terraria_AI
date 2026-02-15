using System;

namespace Terraria
{
	// Token: 0x0200004D RID: 77
	public struct TileColorCache
	{
		// Token: 0x06000BB6 RID: 2998 RVA: 0x00355D93 File Offset: 0x00353F93
		public void ApplyToBlock(Tile tile)
		{
			tile.color(this.Color);
			tile.fullbrightBlock(this.FullBright);
			tile.invisibleBlock(this.Invisible);
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00355DB9 File Offset: 0x00353FB9
		public void ApplyToWall(Tile tile)
		{
			tile.wallColor(this.Color);
			tile.fullbrightWall(this.FullBright);
			tile.invisibleWall(this.Invisible);
		}

		// Token: 0x040009D0 RID: 2512
		public byte Color;

		// Token: 0x040009D1 RID: 2513
		public bool FullBright;

		// Token: 0x040009D2 RID: 2514
		public bool Invisible;
	}
}
