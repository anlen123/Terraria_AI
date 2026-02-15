using System;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000348 RID: 840
	public class HighestOfMultipleUICollectionInfoProvider : IBestiaryUICollectionInfoProvider
	{
		// Token: 0x06002854 RID: 10324 RVA: 0x0057250D File Offset: 0x0057070D
		public HighestOfMultipleUICollectionInfoProvider(params IBestiaryUICollectionInfoProvider[] providers)
		{
			this._providers = providers;
			this._mainProviderIndex = 0;
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x00572524 File Offset: 0x00570724
		public BestiaryUICollectionInfo GetEntryUICollectionInfo()
		{
			BestiaryUICollectionInfo entryUICollectionInfo = this._providers[this._mainProviderIndex].GetEntryUICollectionInfo();
			BestiaryEntryUnlockState unlockState = entryUICollectionInfo.UnlockState;
			for (int i = 0; i < this._providers.Length; i++)
			{
				BestiaryUICollectionInfo entryUICollectionInfo2 = this._providers[i].GetEntryUICollectionInfo();
				if (unlockState < entryUICollectionInfo2.UnlockState)
				{
					unlockState = entryUICollectionInfo2.UnlockState;
				}
			}
			entryUICollectionInfo.UnlockState = unlockState;
			return entryUICollectionInfo;
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x000762F3 File Offset: 0x000744F3
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}

		// Token: 0x0400511C RID: 20764
		private IBestiaryUICollectionInfoProvider[] _providers;

		// Token: 0x0400511D RID: 20765
		private int _mainProviderIndex;
	}
}
