using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ObjectInteractions
{
	// Token: 0x020002D5 RID: 725
	public class SmartInteractSystem
	{
		// Token: 0x060025F4 RID: 9716 RVA: 0x0055B938 File Offset: 0x00559B38
		public SmartInteractSystem()
		{
			this._candidateProvidersByOrderOfPriority.Add(new PotionOfReturnSmartInteractCandidateProvider());
			this._candidateProvidersByOrderOfPriority.Add(new ProjectileSmartInteractCandidateProvider());
			this._candidateProvidersByOrderOfPriority.Add(new NPCSmartInteractCandidateProvider());
			this._candidateProvidersByOrderOfPriority.Add(new TileSmartInteractCandidateProvider());
			this._blockProviders.Add(new BlockBecauseYouAreOverAnImportantTile());
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x0055B9BC File Offset: 0x00559BBC
		public void Clear()
		{
			this._candidates.Clear();
			foreach (ISmartInteractCandidateProvider smartInteractCandidateProvider in this._candidateProvidersByOrderOfPriority)
			{
				smartInteractCandidateProvider.ClearSelfAndPrepareForCheck();
			}
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x0055BA18 File Offset: 0x00559C18
		public void RunQuery(SmartInteractScanSettings settings)
		{
			this.Clear();
			using (List<ISmartInteractBlockReasonProvider>.Enumerator enumerator = this._blockProviders.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ShouldBlockSmartInteract(settings))
					{
						return;
					}
				}
			}
			using (List<ISmartInteractCandidateProvider>.Enumerator enumerator2 = this._candidateProvidersByOrderOfPriority.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ISmartInteractCandidate smartInteractCandidate;
					if (enumerator2.Current.ProvideCandidate(settings, out smartInteractCandidate))
					{
						this._candidates.Add(smartInteractCandidate);
						if (smartInteractCandidate.DistanceFromCursor == 0f)
						{
							break;
						}
					}
				}
			}
			ISmartInteractCandidate smartInteractCandidate2 = null;
			foreach (ISmartInteractCandidate smartInteractCandidate3 in this._candidates)
			{
				if (smartInteractCandidate2 == null || smartInteractCandidate2.DistanceFromCursor > smartInteractCandidate3.DistanceFromCursor)
				{
					smartInteractCandidate2 = smartInteractCandidate3;
				}
			}
			if (smartInteractCandidate2 == null)
			{
				return;
			}
			smartInteractCandidate2.WinCandidacy();
		}

		// Token: 0x04005037 RID: 20535
		private List<ISmartInteractCandidateProvider> _candidateProvidersByOrderOfPriority = new List<ISmartInteractCandidateProvider>();

		// Token: 0x04005038 RID: 20536
		private List<ISmartInteractBlockReasonProvider> _blockProviders = new List<ISmartInteractBlockReasonProvider>();

		// Token: 0x04005039 RID: 20537
		private List<ISmartInteractCandidate> _candidates = new List<ISmartInteractCandidate>();
	}
}
