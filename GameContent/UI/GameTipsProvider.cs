using System;
using System.Collections.Generic;
using Terraria.GameInput;
using Terraria.Localization;

namespace Terraria.GameContent.UI
{
	// Token: 0x02000377 RID: 887
	public class GameTipsProvider : ITipProvider
	{
		// Token: 0x06002954 RID: 10580 RVA: 0x0057AA30 File Offset: 0x00578C30
		public GameTipsProvider()
		{
			this._tipsDefault = Language.FindAll(Lang.CreateDialogFilter("LoadingTips_Default.", false));
			this._tipsGamepad = Language.FindAll(Lang.CreateDialogFilter("LoadingTips_GamePad.", false));
			this._tipsKeyboard = Language.FindAll(Lang.CreateDialogFilter("LoadingTips_Keyboard.", false));
			this._lastTip = null;
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x0057AA8C File Offset: 0x00578C8C
		public LocalizedText RollAvailableTip()
		{
			List<LocalizedText> list = new List<LocalizedText>();
			list.AddRange(this._tipsDefault);
			if (PlayerInput.UsingGamepad)
			{
				list.AddRange(this._tipsGamepad);
			}
			else
			{
				list.AddRange(this._tipsKeyboard);
			}
			do
			{
				list.Remove(this._lastTip);
				if (list.Count == 0)
				{
					this._lastTip = LocalizedText.Empty;
				}
				else
				{
					this._lastTip = list[Main.rand.Next(list.Count)];
				}
			}
			while (!this._lastTip.ConditionsMet);
			return this._lastTip;
		}

		// Token: 0x040051C5 RID: 20933
		private LocalizedText[] _tipsDefault;

		// Token: 0x040051C6 RID: 20934
		private LocalizedText[] _tipsGamepad;

		// Token: 0x040051C7 RID: 20935
		private LocalizedText[] _tipsKeyboard;

		// Token: 0x040051C8 RID: 20936
		private LocalizedText _lastTip;
	}
}
