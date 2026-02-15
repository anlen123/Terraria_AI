using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Terraria.GameContent.Drawing
{
	// Token: 0x0200043D RID: 1085
	public interface INatureRenderer
	{
		// Token: 0x060030C4 RID: 12484
		void DrawNature(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth, SideFlags seams = SideFlags.None);

		// Token: 0x060030C5 RID: 12485
		void DrawGlowmask(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);

		// Token: 0x060030C6 RID: 12486
		void DrawAfterAllObjects(SpriteBatchBeginner beginner);
	}
}
