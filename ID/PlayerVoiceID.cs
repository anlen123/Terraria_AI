using System;
using Microsoft.Xna.Framework;

namespace Terraria.ID
{
	// Token: 0x02000193 RID: 403
	public class PlayerVoiceID
	{
		// Token: 0x040017F2 RID: 6130
		public static int[] VariantOrder = new int[]
		{
			1,
			2,
			3
		};

		// Token: 0x040017F3 RID: 6131
		public const int None = 0;

		// Token: 0x040017F4 RID: 6132
		public const int Male = 1;

		// Token: 0x040017F5 RID: 6133
		public const int Female = 2;

		// Token: 0x040017F6 RID: 6134
		public const int Other = 3;

		// Token: 0x040017F7 RID: 6135
		public const int Count = 4;

		// Token: 0x0200075C RID: 1884
		public static class Sets
		{
			// Token: 0x040069CD RID: 27085
			public static SetFactory Factory = new SetFactory(4);

			// Token: 0x040069CE RID: 27086
			public static Color[] Colors = PlayerVoiceID.Sets.Factory.CreateCustomSet<Color>(Color.White, new object[]
			{
				1,
				Color.CornflowerBlue,
				2,
				Color.HotPink,
				3,
				Color.LimeGreen
			});
		}
	}
}
