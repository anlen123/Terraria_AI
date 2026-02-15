using System;
using Terraria.Localization;

namespace Terraria.GameContent.UI
{
	// Token: 0x02000378 RID: 888
	public class CharacterCreationTipsProvider : ITipProvider
	{
		// Token: 0x06002956 RID: 10582 RVA: 0x0057AB1D File Offset: 0x00578D1D
		public LocalizedText RollAvailableTip()
		{
			return Language.SelectRandom(Lang.CreateDialogFilter("LoadingTips_CharacterCreation.", true), null);
		}
	}
}
