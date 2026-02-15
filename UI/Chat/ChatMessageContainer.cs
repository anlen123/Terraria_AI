using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace Terraria.UI.Chat
{
	// Token: 0x0200010A RID: 266
	public class ChatMessageContainer
	{
		// Token: 0x06001A75 RID: 6773 RVA: 0x004F5FA8 File Offset: 0x004F41A8
		public void SetContents(string text, Color color, int widthLimitInPixels)
		{
			this.OriginalText = text;
			this._color = color;
			this._widthLimitInPixels = widthLimitInPixels;
			this._parsedText = new List<TextSnippet[]>();
			this._timeLeft = 600;
			this.Refresh();
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x004F5FDB File Offset: 0x004F41DB
		public void OnWidthLimitChanged()
		{
			if (this._widthLimitInPixels == -1)
			{
				this._prepared = false;
			}
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x004F5FED File Offset: 0x004F41ED
		public void Update()
		{
			if (this._timeLeft > 0)
			{
				this._timeLeft--;
			}
			this.Refresh();
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x004F600C File Offset: 0x004F420C
		public TextSnippet[] GetSnippetWithInversedIndex(int snippetIndex)
		{
			int index = this._parsedText.Count - 1 - snippetIndex;
			return this._parsedText[index];
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06001A79 RID: 6777 RVA: 0x004F6035 File Offset: 0x004F4235
		public int LineCount
		{
			get
			{
				return this._parsedText.Count;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06001A7A RID: 6778 RVA: 0x004F6042 File Offset: 0x004F4242
		public bool CanBeShownWhenChatIsClosed
		{
			get
			{
				return this._timeLeft > 0;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06001A7B RID: 6779 RVA: 0x004F604D File Offset: 0x004F424D
		public bool Prepared
		{
			get
			{
				return this._prepared;
			}
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x004F6058 File Offset: 0x004F4258
		private void Refresh()
		{
			if (this._prepared)
			{
				return;
			}
			this._prepared = true;
			int num = this._widthLimitInPixels;
			if (num == -1)
			{
				num = Main.ChatLineWidthLimit;
			}
			List<List<TextSnippet>> list = Utils.WordwrapStringSmart(this.OriginalText, this._color, FontAssets.MouseText.Value, (float)num, 10);
			this._parsedText.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				this._parsedText.Add(list[i].ToArray());
			}
		}

		// Token: 0x040014E4 RID: 5348
		public string OriginalText;

		// Token: 0x040014E5 RID: 5349
		private bool _prepared;

		// Token: 0x040014E6 RID: 5350
		private List<TextSnippet[]> _parsedText;

		// Token: 0x040014E7 RID: 5351
		private Color _color;

		// Token: 0x040014E8 RID: 5352
		private int _widthLimitInPixels;

		// Token: 0x040014E9 RID: 5353
		private int _timeLeft;
	}
}
