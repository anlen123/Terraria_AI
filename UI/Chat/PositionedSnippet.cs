using System;
using Microsoft.Xna.Framework;

namespace Terraria.UI.Chat
{
	// Token: 0x02000108 RID: 264
	public struct PositionedSnippet
	{
		// Token: 0x06001A6F RID: 6767 RVA: 0x004F5EAB File Offset: 0x004F40AB
		public PositionedSnippet(TextSnippet snippet, int origIndex, int line, Vector2 position, Vector2 size)
		{
			this.Snippet = snippet;
			this.OrigIndex = origIndex;
			this.Line = line;
			this.Position = position;
			this.Size = size;
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x004F5ED2 File Offset: 0x004F40D2
		public void Scale(float scale)
		{
			this.Position *= scale;
			this.Size *= scale;
		}

		// Token: 0x040014D9 RID: 5337
		public readonly TextSnippet Snippet;

		// Token: 0x040014DA RID: 5338
		public readonly int OrigIndex;

		// Token: 0x040014DB RID: 5339
		public readonly int Line;

		// Token: 0x040014DC RID: 5340
		public Vector2 Position;

		// Token: 0x040014DD RID: 5341
		public Vector2 Size;
	}
}
