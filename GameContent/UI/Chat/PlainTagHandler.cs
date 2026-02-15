using System;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
	// Token: 0x02000387 RID: 903
	public class PlainTagHandler : ITagHandler
	{
		// Token: 0x060029B5 RID: 10677 RVA: 0x0057DE93 File Offset: 0x0057C093
		TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
		{
			return new PlainTagHandler.PlainSnippet(text);
		}

		// Token: 0x020008DE RID: 2270
		public class PlainSnippet : TextSnippet
		{
			// Token: 0x0600468D RID: 18061 RVA: 0x006C716F File Offset: 0x006C536F
			public PlainSnippet(string text = "") : base(text)
			{
			}

			// Token: 0x0600468E RID: 18062 RVA: 0x006C7178 File Offset: 0x006C5378
			public PlainSnippet(string text, Color color) : base(text, color)
			{
			}

			// Token: 0x0600468F RID: 18063 RVA: 0x006C7182 File Offset: 0x006C5382
			public override Color GetVisibleColor()
			{
				return this.Color;
			}
		}
	}
}
