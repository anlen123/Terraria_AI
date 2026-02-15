using System;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x0200035B RID: 859
	public class NamePlateInfoElement : IBestiaryInfoElement, IProvideSearchFilterString
	{
		// Token: 0x060028A4 RID: 10404 RVA: 0x00573127 File Offset: 0x00571327
		public NamePlateInfoElement(string languageKey, int npcNetId)
		{
			this._key = languageKey;
			this._npcNetId = npcNetId;
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x00573140 File Offset: 0x00571340
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			UIElement uielement;
			if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
			{
				uielement = new UIText("???", 1f, false);
			}
			else
			{
				uielement = new UIText(Language.GetText(this._key), 1f, false);
			}
			uielement.HAlign = 0.5f;
			uielement.VAlign = 0.5f;
			uielement.Top = new StyleDimension(2f, 0f);
			uielement.IgnoresMouseInteraction = true;
			UIElement uielement2 = new UIElement();
			uielement2.Width = new StyleDimension(0f, 1f);
			uielement2.Height = new StyleDimension(24f, 0f);
			uielement2.Append(uielement);
			return uielement2;
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x005731E7 File Offset: 0x005713E7
		public string GetSearchString(ref BestiaryUICollectionInfo info)
		{
			if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
			{
				return null;
			}
			return Language.GetText(this._key).Value;
		}

		// Token: 0x0400513C RID: 20796
		private string _key;

		// Token: 0x0400513D RID: 20797
		private int _npcNetId;
	}
}
