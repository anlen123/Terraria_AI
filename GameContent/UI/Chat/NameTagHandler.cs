using System;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
	// Token: 0x02000386 RID: 902
	public class NameTagHandler : ITagHandler
	{
		// Token: 0x060029B2 RID: 10674 RVA: 0x0057DE2D File Offset: 0x0057C02D
		TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
		{
			return new TextSnippet("<" + text.Replace("\\[", "[").Replace("\\]", "]") + ">", baseColor);
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x0057DE63 File Offset: 0x0057C063
		public static string GenerateTag(string name)
		{
			return "[n:" + name.Replace("[", "\\[").Replace("]", "\\]") + "]";
		}
	}
}
