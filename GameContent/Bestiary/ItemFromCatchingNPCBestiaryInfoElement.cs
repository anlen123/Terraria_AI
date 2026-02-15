using System;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000358 RID: 856
	public class ItemFromCatchingNPCBestiaryInfoElement : IItemBestiaryInfoElement, IBestiaryInfoElement, IProvideSearchFilterString
	{
		// Token: 0x0600289B RID: 10395 RVA: 0x00572D4B File Offset: 0x00570F4B
		public ItemFromCatchingNPCBestiaryInfoElement(int itemId)
		{
			this._itemType = itemId;
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x00572D5A File Offset: 0x00570F5A
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			if (info.UnlockState < BestiaryEntryUnlockState.CanShowDropsWithoutDropRates_3)
			{
				return null;
			}
			return new UIBestiaryInfoLine<string>(("catch item #" + this._itemType) ?? "", 1f);
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x00572D8F File Offset: 0x00570F8F
		public string GetSearchString(ref BestiaryUICollectionInfo info)
		{
			if (info.UnlockState < BestiaryEntryUnlockState.CanShowDropsWithoutDropRates_3)
			{
				return null;
			}
			return ContentSamples.ItemsByType[this._itemType].Name;
		}

		// Token: 0x04005137 RID: 20791
		private int _itemType;
	}
}
