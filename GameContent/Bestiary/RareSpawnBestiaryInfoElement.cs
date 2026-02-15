using System;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x0200034B RID: 843
	public class RareSpawnBestiaryInfoElement : IBestiaryInfoElement, IProvideSearchFilterString
	{
		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06002860 RID: 10336 RVA: 0x0057280A File Offset: 0x00570A0A
		// (set) Token: 0x06002861 RID: 10337 RVA: 0x00572812 File Offset: 0x00570A12
		public int RarityLevel { get; private set; }

		// Token: 0x06002862 RID: 10338 RVA: 0x0057281B File Offset: 0x00570A1B
		public RareSpawnBestiaryInfoElement(int rarityLevel)
		{
			this.RarityLevel = rarityLevel;
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x000762F3 File Offset: 0x000744F3
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x0057282A File Offset: 0x00570A2A
		public string GetSearchString(ref BestiaryUICollectionInfo info)
		{
			if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
			{
				return null;
			}
			return Language.GetText("BestiaryInfo.IsRare").Value;
		}
	}
}
