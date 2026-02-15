using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x0200020B RID: 523
	public class CreativeSacrificeParticle : IParticle
	{
		// Token: 0x17000343 RID: 835
		// (get) Token: 0x0600215B RID: 8539 RVA: 0x0052E623 File Offset: 0x0052C823
		// (set) Token: 0x0600215C RID: 8540 RVA: 0x0052E62B File Offset: 0x0052C82B
		public bool ShouldBeRemovedFromRenderer { get; private set; }

		// Token: 0x0600215D RID: 8541 RVA: 0x0052E634 File Offset: 0x0052C834
		public CreativeSacrificeParticle(Asset<Texture2D> textureAsset, Rectangle? frame, Vector2 initialVelocity, Vector2 initialLocalPosition)
		{
			this._texture = textureAsset;
			this._frame = ((frame != null) ? frame.Value : this._texture.Frame(1, 1, 0, 0, 0, 0));
			this._origin = this._frame.Size() / 2f;
			this.Velocity = initialVelocity;
			this.LocalPosition = initialLocalPosition;
			this.StopWhenBelowXScale = 0f;
			this.ShouldBeRemovedFromRenderer = false;
			this._scale = 0.6f;
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x0052E6C0 File Offset: 0x0052C8C0
		public void Update(ref ParticleRendererSettings settings)
		{
			this.Velocity += this.AccelerationPerFrame;
			this.LocalPosition += this.Velocity;
			this._scale += this.ScaleOffsetPerFrame;
			if (this._scale <= this.StopWhenBelowXScale)
			{
				this.ShouldBeRemovedFromRenderer = true;
			}
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x0052E724 File Offset: 0x0052C924
		public void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Color color = Color.Lerp(Color.White, new Color(255, 255, 255, 0), Utils.GetLerpValue(0.1f, 0.5f, this._scale, false));
			spritebatch.Draw(this._texture.Value, settings.AnchorPosition + this.LocalPosition, new Rectangle?(this._frame), color, 0f, this._origin, this._scale, SpriteEffects.None, 0f);
		}

		// Token: 0x04004BB3 RID: 19379
		public Vector2 AccelerationPerFrame;

		// Token: 0x04004BB4 RID: 19380
		public Vector2 Velocity;

		// Token: 0x04004BB5 RID: 19381
		public Vector2 LocalPosition;

		// Token: 0x04004BB6 RID: 19382
		public float ScaleOffsetPerFrame;

		// Token: 0x04004BB7 RID: 19383
		public float StopWhenBelowXScale;

		// Token: 0x04004BB8 RID: 19384
		private Asset<Texture2D> _texture;

		// Token: 0x04004BB9 RID: 19385
		private Rectangle _frame;

		// Token: 0x04004BBA RID: 19386
		private Vector2 _origin;

		// Token: 0x04004BBB RID: 19387
		private float _scale;
	}
}
