using System;

namespace Terraria.ID
{
	// Token: 0x020001B6 RID: 438
	public static class PlayerVariantID
	{
		// Token: 0x040020BE RID: 8382
		public const int MaleStarter = 0;

		// Token: 0x040020BF RID: 8383
		public const int MaleSticker = 1;

		// Token: 0x040020C0 RID: 8384
		public const int MaleGangster = 2;

		// Token: 0x040020C1 RID: 8385
		public const int MaleCoat = 3;

		// Token: 0x040020C2 RID: 8386
		public const int FemaleStarter = 4;

		// Token: 0x040020C3 RID: 8387
		public const int FemaleSticker = 5;

		// Token: 0x040020C4 RID: 8388
		public const int FemaleGangster = 6;

		// Token: 0x040020C5 RID: 8389
		public const int FemaleCoat = 7;

		// Token: 0x040020C6 RID: 8390
		public const int MaleDress = 8;

		// Token: 0x040020C7 RID: 8391
		public const int FemaleDress = 9;

		// Token: 0x040020C8 RID: 8392
		public const int MaleDisplayDoll = 10;

		// Token: 0x040020C9 RID: 8393
		public const int FemaleDisplayDoll = 11;

		// Token: 0x040020CA RID: 8394
		public static readonly int Count = 12;

		// Token: 0x02000782 RID: 1922
		public class Sets
		{
			// Token: 0x04006E46 RID: 28230
			public static SetFactory Factory = new SetFactory(PlayerVariantID.Count);

			// Token: 0x04006E47 RID: 28231
			public static bool[] Male = PlayerVariantID.Sets.Factory.CreateBoolSet(new int[]
			{
				0,
				1,
				2,
				3,
				8,
				10
			});

			// Token: 0x04006E48 RID: 28232
			public static int[] AltGenderReference = PlayerVariantID.Sets.Factory.CreateIntSet(0, new int[]
			{
				0,
				4,
				4,
				0,
				1,
				5,
				5,
				1,
				2,
				6,
				6,
				2,
				3,
				7,
				7,
				3,
				8,
				9,
				9,
				8,
				10,
				11,
				11,
				10
			});

			// Token: 0x04006E49 RID: 28233
			public static int[] VariantOrderMale = new int[]
			{
				0,
				1,
				2,
				3,
				8,
				10
			};

			// Token: 0x04006E4A RID: 28234
			public static int[] VariantOrderFemale = new int[]
			{
				4,
				5,
				6,
				7,
				9,
				11
			};
		}
	}
}
