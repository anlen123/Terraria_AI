using System;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x0200038A RID: 906
	internal interface IBigProgressBar
	{
		// Token: 0x060029C1 RID: 10689
		bool ValidateAndCollectNecessaryInfo(ref BigProgressBarInfo info);

		// Token: 0x060029C2 RID: 10690
		void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch);
	}
}
