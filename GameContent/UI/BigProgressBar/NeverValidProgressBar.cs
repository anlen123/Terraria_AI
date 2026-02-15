using System;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x02000392 RID: 914
	public class NeverValidProgressBar : IBigProgressBar
	{
		// Token: 0x060029DE RID: 10718 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public bool ValidateAndCollectNecessaryInfo(ref BigProgressBarInfo info)
		{
			return false;
		}

		// Token: 0x060029DF RID: 10719 RVA: 0x00009E06 File Offset: 0x00008006
		public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
		}
	}
}
