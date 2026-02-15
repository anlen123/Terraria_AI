using System;

namespace Terraria.Achievements
{
	// Token: 0x020005E5 RID: 1509
	public interface IAchievementTracker
	{
		// Token: 0x06003B2F RID: 15151
		void ReportAs(string name);

		// Token: 0x06003B30 RID: 15152
		TrackerType GetTrackerType();

		// Token: 0x06003B31 RID: 15153
		void Load();

		// Token: 0x06003B32 RID: 15154
		void Clear();
	}
}
