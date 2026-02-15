using System;
using ReLogic.Reflection;

namespace Terraria.ID
{
	// Token: 0x020001A1 RID: 417
	public class SurfaceBackgroundID
	{
		// Token: 0x0400192C RID: 6444
		public const int Forest1 = 0;

		// Token: 0x0400192D RID: 6445
		public const int Corruption = 1;

		// Token: 0x0400192E RID: 6446
		public const int Desert = 2;

		// Token: 0x0400192F RID: 6447
		public const int Jungle = 3;

		// Token: 0x04001930 RID: 6448
		public const int Ocean = 4;

		// Token: 0x04001931 RID: 6449
		public const int CorruptDesert = 5;

		// Token: 0x04001932 RID: 6450
		public const int Hallow = 6;

		// Token: 0x04001933 RID: 6451
		public const int Snow = 7;

		// Token: 0x04001934 RID: 6452
		public const int Crimson = 8;

		// Token: 0x04001935 RID: 6453
		public const int Mushroom = 9;

		// Token: 0x04001936 RID: 6454
		public const int Forest2 = 10;

		// Token: 0x04001937 RID: 6455
		public const int Forest3 = 11;

		// Token: 0x04001938 RID: 6456
		public const int Forest4 = 12;

		// Token: 0x04001939 RID: 6457
		public const int HallowDesert = 13;

		// Token: 0x0400193A RID: 6458
		public const int CrimsonDesert = 14;

		// Token: 0x0400193B RID: 6459
		public const int Empty = 15;

		// Token: 0x0400193C RID: 6460
		public const int Count = 16;

		// Token: 0x0400193D RID: 6461
		public static readonly IdDictionary Search = IdDictionary.Create<SurfaceBackgroundID, int>();

		// Token: 0x02000762 RID: 1890
		public static class Sets
		{
			// Token: 0x040069DA RID: 27098
			public static SetFactory Factory = new SetFactory(16);

			// Token: 0x040069DB RID: 27099
			public static bool[] IsDesertVariant = SurfaceBackgroundID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				2,
				5,
				13,
				14
			});

			// Token: 0x040069DC RID: 27100
			public static bool[] IsForest = SurfaceBackgroundID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				0,
				10,
				11,
				12
			});
		}
	}
}
