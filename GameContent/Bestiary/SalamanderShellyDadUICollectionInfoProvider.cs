using System;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x0200034A RID: 842
	public class SalamanderShellyDadUICollectionInfoProvider : IBestiaryUICollectionInfoProvider
	{
		// Token: 0x0600285B RID: 10331 RVA: 0x005726BA File Offset: 0x005708BA
		public SalamanderShellyDadUICollectionInfoProvider(string persistentId)
		{
			this._persistentIdentifierToCheck = persistentId;
			this._killCountNeededToFullyUnlock = CommonEnemyUICollectionInfoProvider.GetKillCountNeeded(persistentId);
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x005726D8 File Offset: 0x005708D8
		public BestiaryUICollectionInfo GetEntryUICollectionInfo()
		{
			BestiaryEntryUnlockState bestiaryEntryUnlockState = CommonEnemyUICollectionInfoProvider.GetUnlockStateByKillCount(Main.BestiaryTracker.Kills.GetKillCount(this._persistentIdentifierToCheck), false, this._killCountNeededToFullyUnlock);
			if (!this.IsIncludedInCurrentWorld())
			{
				bestiaryEntryUnlockState = this.GetLowestAvailableUnlockStateFromEntriesThatAreInWorld(bestiaryEntryUnlockState);
			}
			return new BestiaryUICollectionInfo
			{
				UnlockState = bestiaryEntryUnlockState
			};
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x00572728 File Offset: 0x00570928
		private BestiaryEntryUnlockState GetLowestAvailableUnlockStateFromEntriesThatAreInWorld(BestiaryEntryUnlockState unlockstatus)
		{
			BestiaryEntryUnlockState bestiaryEntryUnlockState = BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
			int[,] cavernMonsterType = NPC.cavernMonsterType;
			for (int i = 0; i < cavernMonsterType.GetLength(0); i++)
			{
				for (int j = 0; j < cavernMonsterType.GetLength(1); j++)
				{
					string persistentId = ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[cavernMonsterType[i, j]];
					BestiaryEntryUnlockState unlockStateByKillCount = CommonEnemyUICollectionInfoProvider.GetUnlockStateByKillCount(Main.BestiaryTracker.Kills.GetKillCount(persistentId), false, this._killCountNeededToFullyUnlock);
					if (bestiaryEntryUnlockState > unlockStateByKillCount)
					{
						bestiaryEntryUnlockState = unlockStateByKillCount;
					}
				}
			}
			return bestiaryEntryUnlockState;
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x005727A0 File Offset: 0x005709A0
		private bool IsIncludedInCurrentWorld()
		{
			int num = ContentSamples.NpcNetIdsByPersistentIds[this._persistentIdentifierToCheck];
			int[,] cavernMonsterType = NPC.cavernMonsterType;
			for (int i = 0; i < cavernMonsterType.GetLength(0); i++)
			{
				for (int j = 0; j < cavernMonsterType.GetLength(1); j++)
				{
					if (ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[cavernMonsterType[i, j]] == this._persistentIdentifierToCheck)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x000762F3 File Offset: 0x000744F3
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}

		// Token: 0x04005120 RID: 20768
		private string _persistentIdentifierToCheck;

		// Token: 0x04005121 RID: 20769
		private int _killCountNeededToFullyUnlock;
	}
}
