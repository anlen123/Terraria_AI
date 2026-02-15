using System;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000346 RID: 838
	public class TownNPCUICollectionInfoProvider : IBestiaryUICollectionInfoProvider
	{
		// Token: 0x0600284B RID: 10315 RVA: 0x005723AC File Offset: 0x005705AC
		public TownNPCUICollectionInfoProvider(string persistentId)
		{
			this._persistentIdentifierToCheck = persistentId;
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x005723BC File Offset: 0x005705BC
		public BestiaryUICollectionInfo GetEntryUICollectionInfo()
		{
			return new BestiaryUICollectionInfo
			{
				UnlockState = (Main.BestiaryTracker.Chats.GetWasChatWith(this._persistentIdentifierToCheck) ? BestiaryEntryUnlockState.CanShowDropsWithDropRates_4 : BestiaryEntryUnlockState.NotKnownAtAll_0)
			};
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x000762F3 File Offset: 0x000744F3
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}

		// Token: 0x04005118 RID: 20760
		private string _persistentIdentifierToCheck;
	}
}
