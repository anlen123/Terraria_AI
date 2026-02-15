using System;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000347 RID: 839
	public class CommonEnemyUICollectionInfoProvider : IBestiaryUICollectionInfoProvider
	{
		// Token: 0x0600284E RID: 10318 RVA: 0x005723F4 File Offset: 0x005705F4
		public CommonEnemyUICollectionInfoProvider(string persistentId, bool quickUnlock)
		{
			this._persistentIdentifierToCheck = persistentId;
			this._quickUnlock = quickUnlock;
			this._killCountNeededToFullyUnlock = CommonEnemyUICollectionInfoProvider.GetKillCountNeeded(persistentId);
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x00572418 File Offset: 0x00570618
		public static int GetKillCountNeeded(string persistentId)
		{
			int defaultKillsForBannerNeeded = ItemID.Sets.DefaultKillsForBannerNeeded;
			int key;
			if (!ContentSamples.NpcNetIdsByPersistentIds.TryGetValue(persistentId, out key))
			{
				return defaultKillsForBannerNeeded;
			}
			NPC npc;
			if (!ContentSamples.NpcsByNetId.TryGetValue(key, out npc))
			{
				return defaultKillsForBannerNeeded;
			}
			int num = BannerSystem.BannerToItem(BannerSystem.NPCtoBanner(npc.BannerID()));
			return ItemID.Sets.KillsToBanner[num];
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x00572468 File Offset: 0x00570668
		public BestiaryUICollectionInfo GetEntryUICollectionInfo()
		{
			int killCount = Main.BestiaryTracker.Kills.GetKillCount(this._persistentIdentifierToCheck);
			BestiaryEntryUnlockState unlockStateByKillCount = this.GetUnlockStateByKillCount(killCount, this._quickUnlock);
			return new BestiaryUICollectionInfo
			{
				UnlockState = unlockStateByKillCount
			};
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x005724AC File Offset: 0x005706AC
		public BestiaryEntryUnlockState GetUnlockStateByKillCount(int killCount, bool quickUnlock)
		{
			int killCountNeededToFullyUnlock = this._killCountNeededToFullyUnlock;
			return CommonEnemyUICollectionInfoProvider.GetUnlockStateByKillCount(killCount, quickUnlock, killCountNeededToFullyUnlock);
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x005724C8 File Offset: 0x005706C8
		public static BestiaryEntryUnlockState GetUnlockStateByKillCount(int killCount, bool quickUnlock, int fullKillCountNeeded)
		{
			int num = fullKillCountNeeded / 2;
			int num2 = fullKillCountNeeded / 5;
			BestiaryEntryUnlockState result;
			if (quickUnlock && killCount > 0)
			{
				result = BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
			}
			else if (killCount >= fullKillCountNeeded)
			{
				result = BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
			}
			else if (killCount >= num)
			{
				result = BestiaryEntryUnlockState.CanShowDropsWithoutDropRates_3;
			}
			else if (killCount >= num2)
			{
				result = BestiaryEntryUnlockState.CanShowStats_2;
			}
			else if (killCount >= 1)
			{
				result = BestiaryEntryUnlockState.CanShowPortraitOnly_1;
			}
			else
			{
				result = BestiaryEntryUnlockState.NotKnownAtAll_0;
			}
			return result;
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x000762F3 File Offset: 0x000744F3
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}

		// Token: 0x04005119 RID: 20761
		private string _persistentIdentifierToCheck;

		// Token: 0x0400511A RID: 20762
		private bool _quickUnlock;

		// Token: 0x0400511B RID: 20763
		private int _killCountNeededToFullyUnlock;
	}
}
