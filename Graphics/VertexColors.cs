using System;
using Microsoft.Xna.Framework;

namespace Terraria.Graphics
{
	// Token: 0x020001D9 RID: 473
	public struct VertexColors
	{
		// Token: 0x06001FCE RID: 8142 RVA: 0x0051DBFB File Offset: 0x0051BDFB
		public VertexColors(Color color)
		{
			this.TopLeftColor = color;
			this.TopRightColor = color;
			this.BottomRightColor = color;
			this.BottomLeftColor = color;
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x0051DC19 File Offset: 0x0051BE19
		public VertexColors(Color topLeft, Color topRight, Color bottomRight, Color bottomLeft)
		{
			this.TopLeftColor = topLeft;
			this.TopRightColor = topRight;
			this.BottomLeftColor = bottomLeft;
			this.BottomRightColor = bottomRight;
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x0051DC38 File Offset: 0x0051BE38
		public static implicit operator VertexColors(Color color)
		{
			return new VertexColors(color);
		}

		// Token: 0x04004A38 RID: 19000
		public Color TopLeftColor;

		// Token: 0x04004A39 RID: 19001
		public Color TopRightColor;

		// Token: 0x04004A3A RID: 19002
		public Color BottomLeftColor;

		// Token: 0x04004A3B RID: 19003
		public Color BottomRightColor;
	}
}
