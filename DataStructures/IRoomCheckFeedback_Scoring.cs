using System;

namespace Terraria.DataStructures
{
	// Token: 0x0200052F RID: 1327
	public interface IRoomCheckFeedback_Scoring
	{
		// Token: 0x060036EC RID: 14060
		void BeginScoring();

		// Token: 0x060036ED RID: 14061
		void ReportScore(int x, int y, int score);

		// Token: 0x060036EE RID: 14062
		void SetAsHighScore(int x, int y, int score);

		// Token: 0x060036EF RID: 14063
		void EndScoring();
	}
}
