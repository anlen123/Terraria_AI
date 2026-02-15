using System;

namespace Terraria.Net
{
	// Token: 0x0200016C RID: 364
	[Flags]
	public enum ServerMode : byte
	{
		// Token: 0x0400164F RID: 5711
		None = 0,
		// Token: 0x04001650 RID: 5712
		Lobby = 1,
		// Token: 0x04001651 RID: 5713
		FriendsCanJoin = 2,
		// Token: 0x04001652 RID: 5714
		FriendsOfFriends = 4
	}
}
