using System;
using Terraria.ID;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000349 RID: 841
	public class GoldCritterUICollectionInfoProvider : IBestiaryUICollectionInfoProvider
	{
		// Token: 0x06002857 RID: 10327 RVA: 0x00572588 File Offset: 0x00570788
		public GoldCritterUICollectionInfoProvider(int[] normalCritterPersistentId, string goldCritterPersistentId)
		{
			this._normalCritterPersistentId = new string[normalCritterPersistentId.Length];
			for (int i = 0; i < normalCritterPersistentId.Length; i++)
			{
				this._normalCritterPersistentId[i] = ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[normalCritterPersistentId[i]];
			}
			this._goldCritterPersistentId = goldCritterPersistentId;
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x005725D4 File Offset: 0x005707D4
		public BestiaryUICollectionInfo GetEntryUICollectionInfo()
		{
			BestiaryEntryUnlockState unlockStateForCritter = this.GetUnlockStateForCritter(this._goldCritterPersistentId);
			BestiaryEntryUnlockState bestiaryEntryUnlockState = BestiaryEntryUnlockState.NotKnownAtAll_0;
			if (unlockStateForCritter > bestiaryEntryUnlockState)
			{
				bestiaryEntryUnlockState = unlockStateForCritter;
			}
			foreach (string persistentId in this._normalCritterPersistentId)
			{
				BestiaryEntryUnlockState unlockStateForCritter2 = this.GetUnlockStateForCritter(persistentId);
				if (unlockStateForCritter2 > bestiaryEntryUnlockState)
				{
					bestiaryEntryUnlockState = unlockStateForCritter2;
				}
			}
			BestiaryUICollectionInfo result = new BestiaryUICollectionInfo
			{
				UnlockState = bestiaryEntryUnlockState
			};
			if (bestiaryEntryUnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
			{
				return result;
			}
			if (!this.TryFindingOneGoldCritterThatIsAlreadyUnlocked())
			{
				return new BestiaryUICollectionInfo
				{
					UnlockState = BestiaryEntryUnlockState.NotKnownAtAll_0
				};
			}
			return result;
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x0057265C File Offset: 0x0057085C
		private bool TryFindingOneGoldCritterThatIsAlreadyUnlocked()
		{
			for (int i = 0; i < NPCID.Sets.GoldCrittersCollection.Count; i++)
			{
				int key = NPCID.Sets.GoldCrittersCollection[i];
				string persistentId = ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[key];
				if (this.GetUnlockStateForCritter(persistentId) > BestiaryEntryUnlockState.NotKnownAtAll_0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x005726A3 File Offset: 0x005708A3
		private BestiaryEntryUnlockState GetUnlockStateForCritter(string persistentId)
		{
			if (!Main.BestiaryTracker.Sights.GetWasNearbyBefore(persistentId))
			{
				return BestiaryEntryUnlockState.NotKnownAtAll_0;
			}
			return BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
		}

		// Token: 0x0400511E RID: 20766
		private string[] _normalCritterPersistentId;

		// Token: 0x0400511F RID: 20767
		private string _goldCritterPersistentId;
	}
}
