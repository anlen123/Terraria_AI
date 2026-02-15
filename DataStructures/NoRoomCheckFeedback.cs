using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000531 RID: 1329
	public class NoRoomCheckFeedback : IRoomCheckFeedback, IRoomCheckFeedback_Spread, IRoomCheckFeedback_Scoring
	{
		// Token: 0x060036F0 RID: 14064 RVA: 0x0062B604 File Offset: 0x00629804
		public NoRoomCheckFeedback(bool displayText)
		{
			this.DisplayText = displayText;
		}

		// Token: 0x060036F1 RID: 14065 RVA: 0x00009E06 File Offset: 0x00008006
		public void BeginSpread(int x, int y)
		{
		}

		// Token: 0x060036F2 RID: 14066 RVA: 0x00009E06 File Offset: 0x00008006
		public void StartedInASolidTile(int x, int y)
		{
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x00009E06 File Offset: 0x00008006
		public void TooCloseToWorldEdge(int x, int y, int iteration)
		{
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x00009E06 File Offset: 0x00008006
		public void AnyBlockScannedHere(int x, int y, int iteration)
		{
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x00009E06 File Offset: 0x00008006
		public void RoomTooBig(int x, int y, int iteration)
		{
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x00009E06 File Offset: 0x00008006
		public void BlockingWall(int x, int y, int iteration)
		{
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x00009E06 File Offset: 0x00008006
		public void BlockingOpenGate(int x, int y, int iteration)
		{
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x00009E06 File Offset: 0x00008006
		public void Stinkbug(int x, int y, int iteration)
		{
		}

		// Token: 0x060036F9 RID: 14073 RVA: 0x00009E06 File Offset: 0x00008006
		public void EchoStinkbug(int x, int y, int iteration)
		{
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x00009E06 File Offset: 0x00008006
		public void MissingAWall(int x, int y, int iteration)
		{
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x00009E06 File Offset: 0x00008006
		public void UnsafeWall(int x, int y, int iteration)
		{
		}

		// Token: 0x060036FC RID: 14076 RVA: 0x00009E06 File Offset: 0x00008006
		public void EndSpread()
		{
		}

		// Token: 0x060036FD RID: 14077 RVA: 0x00009E06 File Offset: 0x00008006
		public void BeginScoring()
		{
		}

		// Token: 0x060036FE RID: 14078 RVA: 0x00009E06 File Offset: 0x00008006
		public void ReportScore(int x, int y, int score)
		{
		}

		// Token: 0x060036FF RID: 14079 RVA: 0x00009E06 File Offset: 0x00008006
		public void SetAsHighScore(int x, int y, int score)
		{
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x00009E06 File Offset: 0x00008006
		public void EndScoring()
		{
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06003701 RID: 14081 RVA: 0x000379F1 File Offset: 0x00035BF1
		public bool StopOnFail
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06003702 RID: 14082 RVA: 0x0062B613 File Offset: 0x00629813
		// (set) Token: 0x06003703 RID: 14083 RVA: 0x0062B61B File Offset: 0x0062981B
		public bool DisplayText { get; private set; }

		// Token: 0x04005B2E RID: 23342
		public static NoRoomCheckFeedback WithText = new NoRoomCheckFeedback(true);

		// Token: 0x04005B2F RID: 23343
		public static NoRoomCheckFeedback WithoutText = new NoRoomCheckFeedback(false);
	}
}
