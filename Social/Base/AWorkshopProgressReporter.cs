using System;

namespace Terraria.Social.Base
{
	// Token: 0x0200015A RID: 346
	public abstract class AWorkshopProgressReporter
	{
		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06001D47 RID: 7495
		public abstract bool HasOngoingTasks { get; }

		// Token: 0x06001D48 RID: 7496
		public abstract bool TryGetProgress(out float progress);
	}
}
