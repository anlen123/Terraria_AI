using System;
using Terraria.Utilities;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000A5 RID: 165
	public class GenBase
	{
		// Token: 0x1700027D RID: 637
		// (get) Token: 0x0600172C RID: 5932 RVA: 0x004DD195 File Offset: 0x004DB395
		protected static UnifiedRandom _random
		{
			get
			{
				return WorldGen.genRand;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x0600172D RID: 5933 RVA: 0x004DD19C File Offset: 0x004DB39C
		protected static Tile[,] _tiles
		{
			get
			{
				return Main.tile;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600172E RID: 5934 RVA: 0x004DD1A3 File Offset: 0x004DB3A3
		protected static int _worldWidth
		{
			get
			{
				return Main.maxTilesX;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x0600172F RID: 5935 RVA: 0x004DD1AA File Offset: 0x004DB3AA
		protected static int _worldHeight
		{
			get
			{
				return Main.maxTilesY;
			}
		}

		// Token: 0x020006AF RID: 1711
		// (Invoke) Token: 0x06003EC7 RID: 16071
		public delegate bool CustomPerUnitAction(int x, int y, params object[] args);
	}
}
