using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
	// Token: 0x02000384 RID: 900
	public class ColorTagHandler : ITagHandler
	{
		// Token: 0x060029AD RID: 10669 RVA: 0x0057DC1C File Offset: 0x0057BE1C
		TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
		{
			TextSnippet textSnippet = new TextSnippet(text);
			int num;
			if (!int.TryParse(options, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out num))
			{
				return textSnippet;
			}
			textSnippet.Color = new Color(num >> 16 & 255, num >> 8 & 255, num & 255);
			return textSnippet;
		}
	}
}
