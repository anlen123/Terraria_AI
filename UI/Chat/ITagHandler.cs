using System;
using Microsoft.Xna.Framework;

namespace Terraria.UI.Chat
{
	// Token: 0x0200010C RID: 268
	public interface ITagHandler
	{
		// Token: 0x06001A94 RID: 6804
		TextSnippet Parse(string text, Color baseColor = default(Color), string options = null);
	}
}
