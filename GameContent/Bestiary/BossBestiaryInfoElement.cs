using System;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x0200034C RID: 844
	public class BossBestiaryInfoElement : IBestiaryInfoElement, IProvideSearchFilterString
	{
		// Token: 0x06002865 RID: 10341 RVA: 0x000762F3 File Offset: 0x000744F3
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x00572845 File Offset: 0x00570A45
		public string GetSearchString(ref BestiaryUICollectionInfo info)
		{
			if (info.UnlockState < BestiaryEntryUnlockState.CanShowPortraitOnly_1)
			{
				return null;
			}
			return Language.GetText("BestiaryInfo.IsBoss").Value;
		}
	}
}
