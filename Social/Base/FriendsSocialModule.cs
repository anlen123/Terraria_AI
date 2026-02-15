using System;

namespace Terraria.Social.Base
{
	// Token: 0x02000162 RID: 354
	public abstract class FriendsSocialModule : ISocialModule
	{
		// Token: 0x06001D7A RID: 7546
		public abstract string GetUsername();

		// Token: 0x06001D7B RID: 7547
		public abstract void OpenJoinInterface();

		// Token: 0x06001D7C RID: 7548
		public abstract void Initialize();

		// Token: 0x06001D7D RID: 7549
		public abstract void Shutdown();
	}
}
