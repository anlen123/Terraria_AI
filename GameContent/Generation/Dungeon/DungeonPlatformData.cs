using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Generation.Dungeon
{
	// Token: 0x02000493 RID: 1171
	public struct DungeonPlatformData
	{
		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060033A8 RID: 13224 RVA: 0x005F8E88 File Offset: 0x005F7088
		public bool IsAShelf
		{
			get
			{
				return this.PlaceBooksChance > 0.0 || this.PlacePotsChance > 0.0 || this.PlaceWaterCandlesChance > 0.0 || this.PlacePotionBottlesChance > 0.0;
			}
		}

		// Token: 0x040058D3 RID: 22739
		public Point Position;

		// Token: 0x040058D4 RID: 22740
		public int? OverrideStyle;

		// Token: 0x040058D5 RID: 22741
		public int OverrideMaxLengthAllowed;

		// Token: 0x040058D6 RID: 22742
		public int? OverrideHeightFluff;

		// Token: 0x040058D7 RID: 22743
		public bool InAHallway;

		// Token: 0x040058D8 RID: 22744
		public bool ForcePlacement;

		// Token: 0x040058D9 RID: 22745
		public bool SkipOtherPlatformsCheck;

		// Token: 0x040058DA RID: 22746
		public bool SkipSpaceCheck;

		// Token: 0x040058DB RID: 22747
		public double PlaceBooksChance;

		// Token: 0x040058DC RID: 22748
		public bool NoWaterbolt;

		// Token: 0x040058DD RID: 22749
		public double PlacePotsChance;

		// Token: 0x040058DE RID: 22750
		public double PlaceWaterCandlesChance;

		// Token: 0x040058DF RID: 22751
		public double PlacePotionBottlesChance;

		// Token: 0x040058E0 RID: 22752
		public Func<DungeonData, int, int, bool> canPlaceHereCallback;
	}
}
