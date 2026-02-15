using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.ObjectInteractions
{
	// Token: 0x020002D9 RID: 729
	public class ProjectileSmartInteractCandidateProvider : ISmartInteractCandidateProvider
	{
		// Token: 0x06002600 RID: 9728 RVA: 0x0055CB2F File Offset: 0x0055AD2F
		public void ClearSelfAndPrepareForCheck()
		{
			Main.SmartInteractProj = -1;
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x0055CB38 File Offset: 0x0055AD38
		public bool ProvideCandidate(SmartInteractScanSettings settings, out ISmartInteractCandidate candidate)
		{
			candidate = null;
			if (!settings.FullInteraction)
			{
				return false;
			}
			List<int> listOfProjectilesToInteractWithHack = settings.player.GetListOfProjectilesToInteractWithHack();
			bool flag = false;
			Vector2 mousevec = settings.mousevec;
			mousevec.ToPoint();
			int num = -1;
			float projectileDistanceFromCursor = -1f;
			for (int i = 0; i < listOfProjectilesToInteractWithHack.Count; i++)
			{
				int num2 = listOfProjectilesToInteractWithHack[i];
				Projectile projectile = Main.projectile[num2];
				if (projectile.active)
				{
					float num3 = projectile.Hitbox.Distance(mousevec);
					if (num == -1 || Main.projectile[num].Hitbox.Distance(mousevec) > num3)
					{
						num = num2;
						projectileDistanceFromCursor = num3;
					}
					if (num3 == 0f)
					{
						flag = true;
						num = num2;
						projectileDistanceFromCursor = num3;
						break;
					}
				}
			}
			if (settings.DemandOnlyZeroDistanceTargets && !flag)
			{
				return false;
			}
			if (num != -1)
			{
				this._candidate.Reuse(num, projectileDistanceFromCursor);
				candidate = this._candidate;
				return true;
			}
			return false;
		}

		// Token: 0x0400503D RID: 20541
		private ProjectileSmartInteractCandidateProvider.ReusableCandidate _candidate = new ProjectileSmartInteractCandidateProvider.ReusableCandidate();

		// Token: 0x02000822 RID: 2082
		private class ReusableCandidate : ISmartInteractCandidate
		{
			// Token: 0x1700053F RID: 1343
			// (get) Token: 0x06004301 RID: 17153 RVA: 0x006BF36D File Offset: 0x006BD56D
			// (set) Token: 0x06004302 RID: 17154 RVA: 0x006BF375 File Offset: 0x006BD575
			public float DistanceFromCursor { get; private set; }

			// Token: 0x06004303 RID: 17155 RVA: 0x006BF37E File Offset: 0x006BD57E
			public void WinCandidacy()
			{
				Main.SmartInteractProj = this._projectileIndexToTarget;
				Main.SmartInteractShowingGenuine = true;
			}

			// Token: 0x06004304 RID: 17156 RVA: 0x006BF391 File Offset: 0x006BD591
			public void Reuse(int projectileIndex, float projectileDistanceFromCursor)
			{
				this._projectileIndexToTarget = projectileIndex;
				this.DistanceFromCursor = projectileDistanceFromCursor;
			}

			// Token: 0x04007219 RID: 29209
			private int _projectileIndexToTarget;
		}
	}
}
