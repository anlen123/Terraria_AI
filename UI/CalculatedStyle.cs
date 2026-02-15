using System;
using Microsoft.Xna.Framework;

namespace Terraria.UI
{
	// Token: 0x020000E6 RID: 230
	public struct CalculatedStyle
	{
		// Token: 0x060018D0 RID: 6352 RVA: 0x004E4F41 File Offset: 0x004E3141
		public CalculatedStyle(float x, float y, float width, float height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x004E4F60 File Offset: 0x004E3160
		public Rectangle ToRectangle()
		{
			return new Rectangle((int)this.X, (int)this.Y, (int)this.Width, (int)this.Height);
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x004E4F83 File Offset: 0x004E3183
		public Vector2 Position()
		{
			return new Vector2(this.X, this.Y);
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x004E4F96 File Offset: 0x004E3196
		public Vector2 Center()
		{
			return new Vector2(this.X + this.Width * 0.5f, this.Y + this.Height * 0.5f);
		}

		// Token: 0x040012F6 RID: 4854
		public float X;

		// Token: 0x040012F7 RID: 4855
		public float Y;

		// Token: 0x040012F8 RID: 4856
		public float Width;

		// Token: 0x040012F9 RID: 4857
		public float Height;
	}
}
