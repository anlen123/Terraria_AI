using System;

namespace Terraria.Social.Base
{
	// Token: 0x02000164 RID: 356
	public abstract class AchievementsSocialModule : ISocialModule
	{
		// Token: 0x06001D8F RID: 7567
		public abstract void Initialize();

		// Token: 0x06001D90 RID: 7568
		public abstract void Shutdown();

		// Token: 0x06001D91 RID: 7569
		public abstract byte[] GetEncryptionKey();

		// Token: 0x06001D92 RID: 7570
		public abstract string GetSavePath();

		// Token: 0x06001D93 RID: 7571
		public abstract void UpdateIntStat(string name, int value);

		// Token: 0x06001D94 RID: 7572
		public abstract void UpdateFloatStat(string name, float value);

		// Token: 0x06001D95 RID: 7573
		public abstract void CompleteAchievement(string name);

		// Token: 0x06001D96 RID: 7574
		public abstract bool IsAchievementCompleted(string name);

		// Token: 0x06001D97 RID: 7575
		public abstract void StoreStats();
	}
}
