using System;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000350 RID: 848
	public class BestiaryPortraitBackgroundProviderPreferenceInfoElement : IPreferenceProviderElement, IBestiaryInfoElement
	{
		// Token: 0x06002877 RID: 10359 RVA: 0x00572AD5 File Offset: 0x00570CD5
		public BestiaryPortraitBackgroundProviderPreferenceInfoElement(IBestiaryBackgroundImagePathAndColorProvider preferredProvider)
		{
			this._preferredProvider = preferredProvider;
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x000762F3 File Offset: 0x000744F3
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}

		// Token: 0x06002879 RID: 10361 RVA: 0x00572AE4 File Offset: 0x00570CE4
		public bool Matches(IBestiaryBackgroundImagePathAndColorProvider provider)
		{
			return provider == this._preferredProvider;
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x00572AEF File Offset: 0x00570CEF
		public IBestiaryBackgroundImagePathAndColorProvider GetPreferredProvider()
		{
			return this._preferredProvider;
		}

		// Token: 0x04005129 RID: 20777
		private IBestiaryBackgroundImagePathAndColorProvider _preferredProvider;
	}
}
