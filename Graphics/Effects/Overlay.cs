using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Effects
{
	// Token: 0x020001F3 RID: 499
	public abstract class Overlay : GameEffect
	{
		// Token: 0x1700032D RID: 813
		// (get) Token: 0x060020AF RID: 8367 RVA: 0x00522960 File Offset: 0x00520B60
		public RenderLayers Layer
		{
			get
			{
				return this._layer;
			}
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x00522968 File Offset: 0x00520B68
		public Overlay(EffectPriority priority, RenderLayers layer)
		{
			this._priority = priority;
			this._layer = layer;
		}

		// Token: 0x060020B1 RID: 8369
		public abstract void Draw(SpriteBatch spriteBatch);

		// Token: 0x060020B2 RID: 8370
		public abstract void Update(GameTime gameTime);

		// Token: 0x04004B0F RID: 19215
		public OverlayMode Mode = OverlayMode.Inactive;

		// Token: 0x04004B10 RID: 19216
		private RenderLayers _layer = RenderLayers.All;
	}
}
