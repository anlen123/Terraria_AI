using System;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000361 RID: 865
	public class SearchAliasInfoElement : IBestiaryInfoElement, IProvideSearchFilterString
	{
		// Token: 0x060028BF RID: 10431 RVA: 0x0057436D File Offset: 0x0057256D
		public SearchAliasInfoElement(string alias)
		{
			this._alias = alias;
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x0057437C File Offset: 0x0057257C
		public string GetSearchString(ref BestiaryUICollectionInfo info)
		{
			if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
			{
				return null;
			}
			return this._alias;
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x000762F3 File Offset: 0x000744F3
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}

		// Token: 0x0400514A RID: 20810
		private readonly string _alias;
	}
}
