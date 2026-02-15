using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.ObjectInteractions
{
	// Token: 0x020002DA RID: 730
	public class PotionOfReturnSmartInteractCandidateProvider : ISmartInteractCandidateProvider
	{
		// Token: 0x06002603 RID: 9731 RVA: 0x0055CC2F File Offset: 0x0055AE2F
		public void ClearSelfAndPrepareForCheck()
		{
			Main.SmartInteractPotionOfReturn = false;
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x0055CC38 File Offset: 0x0055AE38
		public bool ProvideCandidate(SmartInteractScanSettings settings, out ISmartInteractCandidate candidate)
		{
			candidate = null;
			Rectangle r;
			if (!PotionOfReturnHelper.TryGetGateHitbox(settings.player, out r))
			{
				return false;
			}
			Vector2 vector = r.ClosestPointInRect(settings.mousevec);
			float distanceFromCursor = vector.Distance(settings.mousevec);
			Point point = vector.ToTileCoordinates();
			if (point.X < settings.LX || point.X > settings.HX || point.Y < settings.LY || point.Y > settings.HY)
			{
				return false;
			}
			this._candidate.Reuse(distanceFromCursor);
			candidate = this._candidate;
			return true;
		}

		// Token: 0x0400503E RID: 20542
		private PotionOfReturnSmartInteractCandidateProvider.ReusableCandidate _candidate = new PotionOfReturnSmartInteractCandidateProvider.ReusableCandidate();

		// Token: 0x02000823 RID: 2083
		private class ReusableCandidate : ISmartInteractCandidate
		{
			// Token: 0x17000540 RID: 1344
			// (get) Token: 0x06004306 RID: 17158 RVA: 0x006BF3A1 File Offset: 0x006BD5A1
			// (set) Token: 0x06004307 RID: 17159 RVA: 0x006BF3A9 File Offset: 0x006BD5A9
			public float DistanceFromCursor { get; private set; }

			// Token: 0x06004308 RID: 17160 RVA: 0x006BF3B2 File Offset: 0x006BD5B2
			public void WinCandidacy()
			{
				Main.SmartInteractPotionOfReturn = true;
				Main.SmartInteractShowingGenuine = true;
			}

			// Token: 0x06004309 RID: 17161 RVA: 0x006BF3C0 File Offset: 0x006BD5C0
			public void Reuse(float distanceFromCursor)
			{
				this.DistanceFromCursor = distanceFromCursor;
			}
		}
	}
}
