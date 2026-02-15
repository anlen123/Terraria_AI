using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000342 RID: 834
	public interface IBestiaryBackgroundImagePathAndColorProvider
	{
		// Token: 0x06002842 RID: 10306
		Asset<Texture2D> GetBackgroundImage();

		// Token: 0x06002843 RID: 10307
		Color? GetBackgroundColor();
	}
}
