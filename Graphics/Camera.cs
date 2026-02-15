using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics
{
	// Token: 0x020001D5 RID: 469
	public class Camera
	{
		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06001F8D RID: 8077 RVA: 0x0051C387 File Offset: 0x0051A587
		public Vector2 UnscaledPosition
		{
			get
			{
				return Main.screenPosition;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06001F8E RID: 8078 RVA: 0x0051C38E File Offset: 0x0051A58E
		public Vector2 UnscaledSize
		{
			get
			{
				return new Vector2((float)Main.screenWidth, (float)Main.screenHeight);
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06001F8F RID: 8079 RVA: 0x0051C3A1 File Offset: 0x0051A5A1
		public Vector2 ScaledPosition
		{
			get
			{
				return this.UnscaledPosition + this.GameViewMatrix.Translation;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06001F90 RID: 8080 RVA: 0x0051C3B9 File Offset: 0x0051A5B9
		public Vector2 ScaledSize
		{
			get
			{
				return this.UnscaledSize - this.GameViewMatrix.Translation * 2f;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x0051C3DC File Offset: 0x0051A5DC
		public float BiggerScaledAxis
		{
			get
			{
				Vector2 scaledSize = this.ScaledSize;
				if (scaledSize.X <= scaledSize.Y)
				{
					return scaledSize.Y;
				}
				return scaledSize.X;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06001F92 RID: 8082 RVA: 0x0051C40C File Offset: 0x0051A60C
		public float SmallerScaledAxis
		{
			get
			{
				Vector2 scaledSize = this.ScaledSize;
				if (scaledSize.X >= scaledSize.Y)
				{
					return scaledSize.Y;
				}
				return scaledSize.X;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06001F93 RID: 8083 RVA: 0x0051C43B File Offset: 0x0051A63B
		public RasterizerState Rasterizer
		{
			get
			{
				return Main.Rasterizer;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06001F94 RID: 8084 RVA: 0x0051C442 File Offset: 0x0051A642
		public SamplerState Sampler
		{
			get
			{
				return Main.DefaultSamplerState;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06001F95 RID: 8085 RVA: 0x0051C449 File Offset: 0x0051A649
		public SpriteViewMatrix GameViewMatrix
		{
			get
			{
				return Main.GameViewMatrix;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06001F96 RID: 8086 RVA: 0x0051C450 File Offset: 0x0051A650
		public SpriteBatch SpriteBatch
		{
			get
			{
				return Main.spriteBatch;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06001F97 RID: 8087 RVA: 0x0051C457 File Offset: 0x0051A657
		public Vector2 Center
		{
			get
			{
				return this.UnscaledPosition + this.UnscaledSize * 0.5f;
			}
		}
	}
}
