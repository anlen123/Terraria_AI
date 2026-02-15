using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.DataStructures
{
	// Token: 0x020005A3 RID: 1443
	public class TileDrawInfo
	{
		// Token: 0x04005D2A RID: 23850
		public Tile tileCache;

		// Token: 0x04005D2B RID: 23851
		public ushort typeCache;

		// Token: 0x04005D2C RID: 23852
		public short tileFrameX;

		// Token: 0x04005D2D RID: 23853
		public short tileFrameY;

		// Token: 0x04005D2E RID: 23854
		public Texture2D drawTexture;

		// Token: 0x04005D2F RID: 23855
		public Color tileLight;

		// Token: 0x04005D30 RID: 23856
		public int tileTop;

		// Token: 0x04005D31 RID: 23857
		public int tileWidth;

		// Token: 0x04005D32 RID: 23858
		public int tileHeight;

		// Token: 0x04005D33 RID: 23859
		public int halfBrickHeight;

		// Token: 0x04005D34 RID: 23860
		public int addFrY;

		// Token: 0x04005D35 RID: 23861
		public int addFrX;

		// Token: 0x04005D36 RID: 23862
		public SpriteEffects tileSpriteEffect;

		// Token: 0x04005D37 RID: 23863
		public Texture2D glowTexture;

		// Token: 0x04005D38 RID: 23864
		public Rectangle glowSourceRect;

		// Token: 0x04005D39 RID: 23865
		public Color glowColor;

		// Token: 0x04005D3A RID: 23866
		public Vector3[] colorSlices = new Vector3[9];

		// Token: 0x04005D3B RID: 23867
		public Color finalColor;

		// Token: 0x04005D3C RID: 23868
		public Color colorTint;
	}
}
