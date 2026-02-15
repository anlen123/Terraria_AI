using System;
using Microsoft.Xna.Framework;

namespace Terraria.UI.Chat
{
	// Token: 0x02000109 RID: 265
	public class ChatLine
	{
		// Token: 0x06001A71 RID: 6769 RVA: 0x004F5EF8 File Offset: 0x004F40F8
		public void UpdateTimeLeft()
		{
			if (this.showTime > 0)
			{
				this.showTime--;
			}
			if (this.needsParsing)
			{
				this.needsParsing = false;
			}
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x004F5F20 File Offset: 0x004F4120
		public void Copy(ChatLine other)
		{
			this.needsParsing = other.needsParsing;
			this.parsingPixelLimit = other.parsingPixelLimit;
			this.originalText = other.originalText;
			this.parsedText = other.parsedText;
			this.showTime = other.showTime;
			this.color = other.color;
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x004F5F75 File Offset: 0x004F4175
		public void FlagAsNeedsReprocessing()
		{
			this.needsParsing = true;
		}

		// Token: 0x040014DE RID: 5342
		public Color color = Color.White;

		// Token: 0x040014DF RID: 5343
		public int showTime;

		// Token: 0x040014E0 RID: 5344
		public string originalText = "";

		// Token: 0x040014E1 RID: 5345
		public TextSnippet[] parsedText = new TextSnippet[0];

		// Token: 0x040014E2 RID: 5346
		private int? parsingPixelLimit;

		// Token: 0x040014E3 RID: 5347
		private bool needsParsing;
	}
}
