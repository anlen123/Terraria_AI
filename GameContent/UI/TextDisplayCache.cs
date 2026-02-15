using System;
using Terraria.GameInput;

namespace Terraria.GameContent.UI
{
	// Token: 0x02000367 RID: 871
	public class TextDisplayCache
	{
		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06002905 RID: 10501 RVA: 0x00576FBD File Offset: 0x005751BD
		// (set) Token: 0x06002906 RID: 10502 RVA: 0x00576FC5 File Offset: 0x005751C5
		public string[] TextLines { get; private set; }

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06002907 RID: 10503 RVA: 0x00576FCE File Offset: 0x005751CE
		// (set) Token: 0x06002908 RID: 10504 RVA: 0x00576FD6 File Offset: 0x005751D6
		public int AmountOfLines { get; private set; }

		// Token: 0x06002909 RID: 10505 RVA: 0x00576FE0 File Offset: 0x005751E0
		public void PrepareCache(string text)
		{
			if (!(false | Main.screenWidth != this._lastScreenWidth | Main.screenHeight != this._lastScreenHeight | this._originalText != text | PlayerInput.CurrentInputMode != this._lastInputMode))
			{
				return;
			}
			this._lastScreenWidth = Main.screenWidth;
			this._lastScreenHeight = Main.screenHeight;
			this._originalText = text;
			this._lastInputMode = PlayerInput.CurrentInputMode;
			int amountOfLines;
			this.TextLines = Utils.WordwrapString(text, FontAssets.MouseText.Value, 460, 10, out amountOfLines);
			this.AmountOfLines = amountOfLines;
		}

		// Token: 0x04005187 RID: 20871
		private string _originalText;

		// Token: 0x04005188 RID: 20872
		private int _lastScreenWidth;

		// Token: 0x04005189 RID: 20873
		private int _lastScreenHeight;

		// Token: 0x0400518A RID: 20874
		private InputMode _lastInputMode;
	}
}
