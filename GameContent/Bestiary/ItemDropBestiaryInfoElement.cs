using System;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000357 RID: 855
	public class ItemDropBestiaryInfoElement : IItemBestiaryInfoElement, IBestiaryInfoElement, IProvideSearchFilterString
	{
		// Token: 0x06002897 RID: 10391 RVA: 0x00572C66 File Offset: 0x00570E66
		public ItemDropBestiaryInfoElement(DropRateInfo info)
		{
			this._droprateInfo = info;
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x00572C78 File Offset: 0x00570E78
		public virtual UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			bool flag = ItemDropBestiaryInfoElement.ShouldShowItem(ref this._droprateInfo);
			if (info.UnlockState < BestiaryEntryUnlockState.CanShowStats_2)
			{
				flag = false;
			}
			if (!flag)
			{
				return null;
			}
			return new UIBestiaryInfoItemLine(this._droprateInfo, info, 1f);
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x00572CB4 File Offset: 0x00570EB4
		private static bool ShouldShowItem(ref DropRateInfo dropRateInfo)
		{
			bool result = true;
			if (dropRateInfo.conditions != null && dropRateInfo.conditions.Count > 0)
			{
				for (int i = 0; i < dropRateInfo.conditions.Count; i++)
				{
					if (!dropRateInfo.conditions[i].CanShowItemDropInUI())
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x00572D08 File Offset: 0x00570F08
		public string GetSearchString(ref BestiaryUICollectionInfo info)
		{
			bool flag = ItemDropBestiaryInfoElement.ShouldShowItem(ref this._droprateInfo);
			if (info.UnlockState < BestiaryEntryUnlockState.CanShowStats_2)
			{
				flag = false;
			}
			if (!flag)
			{
				return null;
			}
			return ContentSamples.ItemsByType[this._droprateInfo.itemId].Name;
		}

		// Token: 0x04005136 RID: 20790
		protected DropRateInfo _droprateInfo;
	}
}
