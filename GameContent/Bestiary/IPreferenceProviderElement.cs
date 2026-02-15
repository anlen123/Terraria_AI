using System;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x0200034F RID: 847
	public interface IPreferenceProviderElement : IBestiaryInfoElement
	{
		// Token: 0x06002875 RID: 10357
		IBestiaryBackgroundImagePathAndColorProvider GetPreferredProvider();

		// Token: 0x06002876 RID: 10358
		bool Matches(IBestiaryBackgroundImagePathAndColorProvider provider);
	}
}
