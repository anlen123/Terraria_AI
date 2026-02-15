using System;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000345 RID: 837
	public class CritterUICollectionInfoProvider : IBestiaryUICollectionInfoProvider
	{
		// Token: 0x06002848 RID: 10312 RVA: 0x00572365 File Offset: 0x00570565
		public CritterUICollectionInfoProvider(string persistentId)
		{
			this._persistentIdentifierToCheck = persistentId;
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x00572374 File Offset: 0x00570574
		public BestiaryUICollectionInfo GetEntryUICollectionInfo()
		{
			return new BestiaryUICollectionInfo
			{
				UnlockState = (Main.BestiaryTracker.Sights.GetWasNearbyBefore(this._persistentIdentifierToCheck) ? BestiaryEntryUnlockState.CanShowDropsWithDropRates_4 : BestiaryEntryUnlockState.NotKnownAtAll_0)
			};
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x000762F3 File Offset: 0x000744F3
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}

		// Token: 0x04005117 RID: 20759
		private string _persistentIdentifierToCheck;
	}
}
