using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x0200020C RID: 524
	public abstract class ABasicParticle : IPooledParticle, IParticle
	{
		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06002160 RID: 8544 RVA: 0x0052E7AC File Offset: 0x0052C9AC
		// (set) Token: 0x06002161 RID: 8545 RVA: 0x0052E7B4 File Offset: 0x0052C9B4
		public bool ShouldBeRemovedFromRenderer { get; protected set; }

		// Token: 0x06002162 RID: 8546 RVA: 0x0052E7C0 File Offset: 0x0052C9C0
		public ABasicParticle()
		{
			this._texture = null;
			this._frame = Rectangle.Empty;
			this._origin = Vector2.Zero;
			this.Velocity = Vector2.Zero;
			this.LocalPosition = Vector2.Zero;
			this.ShouldBeRemovedFromRenderer = false;
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x0052E810 File Offset: 0x0052CA10
		public virtual void SetBasicInfo(Asset<Texture2D> textureAsset, Rectangle? frame, Vector2 initialVelocity, Vector2 initialLocalPosition)
		{
			this._texture = textureAsset;
			this._frame = ((frame != null) ? frame.Value : this._texture.Frame(1, 1, 0, 0, 0, 0));
			this._origin = this._frame.Size() / 2f;
			this.Velocity = initialVelocity;
			this.LocalPosition = initialLocalPosition;
			this.ShouldBeRemovedFromRenderer = false;
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x0052E880 File Offset: 0x0052CA80
		public virtual void Update(ref ParticleRendererSettings settings)
		{
			this.Velocity += this.AccelerationPerFrame;
			this.LocalPosition += this.Velocity;
			this.RotationVelocity += this.RotationAcceleration;
			this.Rotation += this.RotationVelocity;
			this.ScaleVelocity += this.ScaleAcceleration;
			this.Scale += this.ScaleVelocity;
		}

		// Token: 0x06002165 RID: 8549
		public abstract void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch);

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06002166 RID: 8550 RVA: 0x0052E90F File Offset: 0x0052CB0F
		// (set) Token: 0x06002167 RID: 8551 RVA: 0x0052E917 File Offset: 0x0052CB17
		public bool IsRestingInPool { get; private set; }

		// Token: 0x06002168 RID: 8552 RVA: 0x0052E920 File Offset: 0x0052CB20
		public void RestInPool()
		{
			this.IsRestingInPool = true;
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x0052E92C File Offset: 0x0052CB2C
		public virtual void FetchFromPool()
		{
			this.IsRestingInPool = false;
			this.ShouldBeRemovedFromRenderer = false;
			this.AccelerationPerFrame = Vector2.Zero;
			this.Velocity = Vector2.Zero;
			this.LocalPosition = Vector2.Zero;
			this._texture = null;
			this._frame = Rectangle.Empty;
			this._origin = Vector2.Zero;
			this.Rotation = 0f;
			this.RotationVelocity = 0f;
			this.RotationAcceleration = 0f;
			this.Scale = Vector2.Zero;
			this.ScaleVelocity = Vector2.Zero;
			this.ScaleAcceleration = Vector2.Zero;
		}

		// Token: 0x04004BBD RID: 19389
		public Vector2 AccelerationPerFrame;

		// Token: 0x04004BBE RID: 19390
		public Vector2 Velocity;

		// Token: 0x04004BBF RID: 19391
		public Vector2 LocalPosition;

		// Token: 0x04004BC0 RID: 19392
		protected Asset<Texture2D> _texture;

		// Token: 0x04004BC1 RID: 19393
		protected Rectangle _frame;

		// Token: 0x04004BC2 RID: 19394
		protected Vector2 _origin;

		// Token: 0x04004BC3 RID: 19395
		public float Rotation;

		// Token: 0x04004BC4 RID: 19396
		public float RotationVelocity;

		// Token: 0x04004BC5 RID: 19397
		public float RotationAcceleration;

		// Token: 0x04004BC6 RID: 19398
		public Vector2 Scale;

		// Token: 0x04004BC7 RID: 19399
		public Vector2 ScaleVelocity;

		// Token: 0x04004BC8 RID: 19400
		public Vector2 ScaleAcceleration;
	}
}
