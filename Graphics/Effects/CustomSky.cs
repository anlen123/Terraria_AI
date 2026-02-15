using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Effects
{
	// Token: 0x020001E9 RID: 489
	public abstract class CustomSky : GameEffect
	{
		// Token: 0x06002070 RID: 8304
		public abstract void Update(GameTime gameTime);

		// Token: 0x06002071 RID: 8305
		public abstract void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth);

		// Token: 0x06002072 RID: 8306
		public abstract bool IsActive();

		// Token: 0x06002073 RID: 8307
		public abstract void Reset();

		// Token: 0x06002074 RID: 8308 RVA: 0x001FC399 File Offset: 0x001FA599
		public virtual Color OnTileColor(Color inColor)
		{
			return inColor;
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x00043EE7 File Offset: 0x000420E7
		public virtual float GetCloudAlpha()
		{
			return 1f;
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x000379F1 File Offset: 0x00035BF1
		public override bool IsVisible()
		{
			return true;
		}
	}
}
