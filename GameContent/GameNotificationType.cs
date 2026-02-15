using System;

namespace Terraria.GameContent
{
	// Token: 0x0200027B RID: 635
	[Flags]
	public enum GameNotificationType
	{
		// Token: 0x04004DE5 RID: 19941
		None = 0,
		// Token: 0x04004DE6 RID: 19942
		Damage = 1,
		// Token: 0x04004DE7 RID: 19943
		SpawnOrDeath = 2,
		// Token: 0x04004DE8 RID: 19944
		WorldGen = 4,
		// Token: 0x04004DE9 RID: 19945
		All = 7
	}
}
