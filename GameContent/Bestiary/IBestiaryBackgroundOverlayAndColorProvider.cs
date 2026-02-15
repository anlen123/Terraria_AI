using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000344 RID: 836
	public interface IBestiaryBackgroundOverlayAndColorProvider
	{
		// Token: 0x06002845 RID: 10309
		Asset<Texture2D> GetBackgroundOverlayImage();

		// Token: 0x06002846 RID: 10310
		Color? GetBackgroundOverlayColor();

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06002847 RID: 10311
		float DisplayPriority { get; }
	}
}
