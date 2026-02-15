using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.DataStructures
{
	// Token: 0x02000590 RID: 1424
	public class DrawAnimation
	{
		// Token: 0x06003839 RID: 14393 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void Update()
		{
		}

		// Token: 0x0600383A RID: 14394 RVA: 0x00630BDA File Offset: 0x0062EDDA
		public virtual Rectangle GetFrame(Texture2D texture, int frameCounterOverride = -1)
		{
			return texture.Frame(1, 1, 0, 0, 0, 0);
		}

		// Token: 0x04005C45 RID: 23621
		public int Frame;

		// Token: 0x04005C46 RID: 23622
		public int FrameCount;

		// Token: 0x04005C47 RID: 23623
		public int TicksPerFrame;

		// Token: 0x04005C48 RID: 23624
		public int FrameCounter;
	}
}
