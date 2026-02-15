using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent
{
	// Token: 0x0200026C RID: 620
	public interface ITownNPCProfile
	{
		// Token: 0x060023F4 RID: 9204
		int RollVariation();

		// Token: 0x060023F5 RID: 9205
		string GetNameForVariant(NPC npc);

		// Token: 0x060023F6 RID: 9206
		Asset<Texture2D> GetTextureNPCShouldUse(NPC npc);

		// Token: 0x060023F7 RID: 9207
		int GetHeadTextureIndex(NPC npc);
	}
}
