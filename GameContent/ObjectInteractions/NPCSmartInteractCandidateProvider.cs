using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.GameContent.ObjectInteractions
{
	// Token: 0x020002D8 RID: 728
	public class NPCSmartInteractCandidateProvider : ISmartInteractCandidateProvider
	{
		// Token: 0x060025FD RID: 9725 RVA: 0x0055CA09 File Offset: 0x0055AC09
		public void ClearSelfAndPrepareForCheck()
		{
			Main.SmartInteractNPC = -1;
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x0055CA14 File Offset: 0x0055AC14
		public bool ProvideCandidate(SmartInteractScanSettings settings, out ISmartInteractCandidate candidate)
		{
			candidate = null;
			if (!settings.FullInteraction)
			{
				return false;
			}
			Rectangle worldRegion = TileReachCheckSettings.Simple.GetWorldRegion(settings.player, 0);
			Vector2 mousevec = settings.mousevec;
			mousevec.ToPoint();
			bool flag = false;
			int num = -1;
			float npcDistanceFromCursor = -1f;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active && npc.townNPC && npc.Hitbox.Intersects(worldRegion) && !flag)
				{
					float num2 = npc.Hitbox.Distance(mousevec);
					if (num == -1 || Main.npc[num].Hitbox.Distance(mousevec) > num2)
					{
						num = i;
						npcDistanceFromCursor = num2;
					}
					if (num2 == 0f)
					{
						flag = true;
						num = i;
						npcDistanceFromCursor = num2;
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
				this._candidate.Reuse(num, npcDistanceFromCursor);
				candidate = this._candidate;
				return true;
			}
			return false;
		}

		// Token: 0x0400503C RID: 20540
		private NPCSmartInteractCandidateProvider.ReusableCandidate _candidate = new NPCSmartInteractCandidateProvider.ReusableCandidate();

		// Token: 0x02000821 RID: 2081
		private class ReusableCandidate : ISmartInteractCandidate
		{
			// Token: 0x1700053E RID: 1342
			// (get) Token: 0x060042FC RID: 17148 RVA: 0x006BF339 File Offset: 0x006BD539
			// (set) Token: 0x060042FD RID: 17149 RVA: 0x006BF341 File Offset: 0x006BD541
			public float DistanceFromCursor { get; private set; }

			// Token: 0x060042FE RID: 17150 RVA: 0x006BF34A File Offset: 0x006BD54A
			public void WinCandidacy()
			{
				Main.SmartInteractNPC = this._npcIndexToTarget;
				Main.SmartInteractShowingGenuine = true;
			}

			// Token: 0x060042FF RID: 17151 RVA: 0x006BF35D File Offset: 0x006BD55D
			public void Reuse(int npcIndex, float npcDistanceFromCursor)
			{
				this._npcIndexToTarget = npcIndex;
				this.DistanceFromCursor = npcDistanceFromCursor;
			}

			// Token: 0x04007217 RID: 29207
			private int _npcIndexToTarget;
		}
	}
}
