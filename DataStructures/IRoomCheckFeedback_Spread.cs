using System;

namespace Terraria.DataStructures
{
	// Token: 0x0200052E RID: 1326
	public interface IRoomCheckFeedback_Spread
	{
		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x060036DE RID: 14046
		bool StopOnFail { get; }

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x060036DF RID: 14047
		bool DisplayText { get; }

		// Token: 0x060036E0 RID: 14048
		void BeginSpread(int x, int y);

		// Token: 0x060036E1 RID: 14049
		void StartedInASolidTile(int x, int y);

		// Token: 0x060036E2 RID: 14050
		void TooCloseToWorldEdge(int x, int y, int iteration);

		// Token: 0x060036E3 RID: 14051
		void AnyBlockScannedHere(int x, int y, int iteration);

		// Token: 0x060036E4 RID: 14052
		void RoomTooBig(int x, int y, int iteration);

		// Token: 0x060036E5 RID: 14053
		void BlockingWall(int x, int y, int iteration);

		// Token: 0x060036E6 RID: 14054
		void BlockingOpenGate(int x, int y, int iteration);

		// Token: 0x060036E7 RID: 14055
		void Stinkbug(int x, int y, int iteration);

		// Token: 0x060036E8 RID: 14056
		void EchoStinkbug(int x, int y, int iteration);

		// Token: 0x060036E9 RID: 14057
		void MissingAWall(int x, int y, int iteration);

		// Token: 0x060036EA RID: 14058
		void UnsafeWall(int x, int y, int iteration);

		// Token: 0x060036EB RID: 14059
		void EndSpread();
	}
}
