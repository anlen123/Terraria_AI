using System;

namespace Terraria.GameContent.ObjectInteractions
{
	// Token: 0x020002D3 RID: 723
	public interface ISmartInteractCandidateProvider
	{
		// Token: 0x060025F2 RID: 9714
		void ClearSelfAndPrepareForCheck();

		// Token: 0x060025F3 RID: 9715
		bool ProvideCandidate(SmartInteractScanSettings settings, out ISmartInteractCandidate candidate);
	}
}
