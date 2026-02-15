using System;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000351 RID: 849
	public class BestiaryPortraitBackgroundBasedOnWorldEvilProviderPreferenceInfoElement : IPreferenceProviderElement, IBestiaryInfoElement
	{
		// Token: 0x0600287B RID: 10363 RVA: 0x00572AF7 File Offset: 0x00570CF7
		public BestiaryPortraitBackgroundBasedOnWorldEvilProviderPreferenceInfoElement(IBestiaryBackgroundImagePathAndColorProvider preferredProviderCorrupt, IBestiaryBackgroundImagePathAndColorProvider preferredProviderCrimson)
		{
			this._preferredProviderCorrupt = preferredProviderCorrupt;
			this._preferredProviderCrimson = preferredProviderCrimson;
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x000762F3 File Offset: 0x000744F3
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x00572B0D File Offset: 0x00570D0D
		public bool Matches(IBestiaryBackgroundImagePathAndColorProvider provider)
		{
			if (Main.ActiveWorldFileData == null || !WorldGen.crimson)
			{
				return provider == this._preferredProviderCorrupt;
			}
			return provider == this._preferredProviderCrimson;
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x00572B30 File Offset: 0x00570D30
		public IBestiaryBackgroundImagePathAndColorProvider GetPreferredProvider()
		{
			if (Main.ActiveWorldFileData == null || !WorldGen.crimson)
			{
				return this._preferredProviderCorrupt;
			}
			return this._preferredProviderCrimson;
		}

		// Token: 0x0400512A RID: 20778
		private IBestiaryBackgroundImagePathAndColorProvider _preferredProviderCorrupt;

		// Token: 0x0400512B RID: 20779
		private IBestiaryBackgroundImagePathAndColorProvider _preferredProviderCrimson;
	}
}
