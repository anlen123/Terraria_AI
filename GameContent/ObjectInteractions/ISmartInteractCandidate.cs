using System;

namespace Terraria.GameContent.ObjectInteractions
{
	// Token: 0x020002D1 RID: 721
	public interface ISmartInteractCandidate
	{
		// Token: 0x17000388 RID: 904
		// (get) Token: 0x060025EF RID: 9711
		float DistanceFromCursor { get; }

		// Token: 0x060025F0 RID: 9712
		void WinCandidacy();
	}
}
