using System;

namespace Terraria.ID
{
	// Token: 0x02000198 RID: 408
	internal class GameModeID
	{
		// Token: 0x06001EFB RID: 7931 RVA: 0x00512A79 File Offset: 0x00510C79
		public static bool IsValid(int gameMode)
		{
			return gameMode >= 0 && gameMode < 4;
		}

		// Token: 0x04001828 RID: 6184
		public const short Normal = 0;

		// Token: 0x04001829 RID: 6185
		public const short Expert = 1;

		// Token: 0x0400182A RID: 6186
		public const short Master = 2;

		// Token: 0x0400182B RID: 6187
		public const short Creative = 3;

		// Token: 0x0400182C RID: 6188
		public const short Count = 4;
	}
}
