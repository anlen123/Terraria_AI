using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Terraria.GameContent.Drawing
{
	// Token: 0x0200043E RID: 1086
	public class OriginalNatureRenderer : INatureRenderer
	{
		// Token: 0x060030C7 RID: 12487 RVA: 0x005BD0C0 File Offset: 0x005BB2C0
		public void DrawNature(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth, SideFlags seams = SideFlags.None)
		{
			Main.spriteBatch.Draw(texture, position, new Rectangle?(sourceRectangle), color, rotation, origin, scale, effects, layerDepth);
		}

		// Token: 0x060030C8 RID: 12488 RVA: 0x005BD0EC File Offset: 0x005BB2EC
		public void DrawGlowmask(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
		{
			Main.spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
		}

		// Token: 0x060030C9 RID: 12489 RVA: 0x00009E06 File Offset: 0x00008006
		public void DrawAfterAllObjects(SpriteBatchBeginner beginner)
		{
		}
	}
}
