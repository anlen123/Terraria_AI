using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x02000207 RID: 519
	public class LittleFlyingCritterParticle : IPooledParticle, IParticle, IParticleRepel
	{
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x0600212A RID: 8490 RVA: 0x0052C4FF File Offset: 0x0052A6FF
		// (set) Token: 0x0600212B RID: 8491 RVA: 0x0052C507 File Offset: 0x0052A707
		public bool IsRestingInPool { get; private set; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x0600212C RID: 8492 RVA: 0x0052C510 File Offset: 0x0052A710
		// (set) Token: 0x0600212D RID: 8493 RVA: 0x0052C518 File Offset: 0x0052A718
		public bool ShouldBeRemovedFromRenderer { get; private set; }

		// Token: 0x0600212F RID: 8495 RVA: 0x0052C524 File Offset: 0x0052A724
		public void Prepare(LittleFlyingCritterParticle.FlyType type, Vector2 position, int duration, Color overrideColor = default(Color), int repelLifetimeDecay = 0)
		{
			this._type = type;
			this._variantRow = Main.rand.Next(8);
			this._variantColumn = ((Main.rand.Next(5) == 0) ? 1 : 0);
			this._spawnPosition = position;
			this._localPosition = position + Main.rand.NextVector2Circular(4f, 8f);
			this._neverGoBelowThis = position.Y + 8f;
			this.RandomizeVelocity();
			this._lifeTimeCounted = 0;
			this._lifeTimeTotal = 300 + Main.rand.Next(6) * 60;
			this._overrideColor = overrideColor;
			this._repelLifetimeDecay = repelLifetimeDecay;
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x0052C5D0 File Offset: 0x0052A7D0
		private void RandomizeVelocity()
		{
			this._velocity = Main.rand.NextVector2Circular(1f, 1f);
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x0052C5EC File Offset: 0x0052A7EC
		public void RestInPool()
		{
			this.IsRestingInPool = true;
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x0052C5F5 File Offset: 0x0052A7F5
		public virtual void FetchFromPool()
		{
			this.IsRestingInPool = false;
			this.ShouldBeRemovedFromRenderer = false;
			this._addedVelocity = Vector2.Zero;
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x0052C610 File Offset: 0x0052A810
		public void Update(ref ParticleRendererSettings settings)
		{
			int num = this._lifeTimeCounted + 1;
			this._lifeTimeCounted = num;
			if (num >= this._lifeTimeTotal)
			{
				this.ShouldBeRemovedFromRenderer = true;
			}
			float num2 = 0.02f;
			int num3 = 30;
			if (this._type == LittleFlyingCritterParticle.FlyType.ButterFly)
			{
				num2 = 0.01f;
				num3 = 600;
			}
			this._velocity += new Vector2((float)Math.Sign(this._spawnPosition.X - this._localPosition.X) * num2, (float)Math.Sign(this._spawnPosition.Y - this._localPosition.Y) * num2);
			if (this._lifeTimeCounted % num3 == 0 && Main.rand.Next(2) == 0)
			{
				this.RandomizeVelocity();
				if (Main.rand.Next(2) == 0)
				{
					this._velocity /= 2f;
				}
			}
			this._addedVelocity *= 0.98f;
			if (this._addedVelocity.Length() < 0.01f)
			{
				this._addedVelocity = new Vector2(0f, 0f);
			}
			this._localPosition += this._velocity + this._addedVelocity;
			if (this._localPosition.Y > this._neverGoBelowThis)
			{
				this._localPosition.Y = this._neverGoBelowThis;
				if (this._velocity.Y > 0f)
				{
					this._velocity.Y = this._velocity.Y * -1f;
				}
				if (this._addedVelocity.Y > 0f)
				{
					this._addedVelocity.Y = this._addedVelocity.Y * -1f;
				}
			}
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x0052C7C0 File Offset: 0x0052A9C0
		public void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Vector2 vector = settings.AnchorPosition + this._localPosition;
			if (vector.X < -10f || vector.X > (float)(Main.screenWidth + 10) || vector.Y < -10f || vector.Y > (float)(Main.screenHeight + 10))
			{
				this.ShouldBeRemovedFromRenderer = true;
				return;
			}
			LittleFlyingCritterParticle.FlyType type = this._type;
			if (type == LittleFlyingCritterParticle.FlyType.RegularFly)
			{
				this.Draw_Fly(ref settings, spritebatch);
				return;
			}
			if (type != LittleFlyingCritterParticle.FlyType.ButterFly)
			{
				return;
			}
			this.Draw_ButterFly(ref settings, spritebatch);
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x0052C844 File Offset: 0x0052AA44
		private void Draw_ButterFly(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Vector2 vector = this._velocity + this._addedVelocity;
			Texture2D value = TextureAssets.Extra[281].Value;
			int num = this._lifeTimeCounted % 10 / 5;
			int variantRow = this._variantRow;
			bool flag = this._variantColumn == 1;
			Rectangle rectangle = new Rectangle(flag ? 10 : 0, (variantRow * 2 + num) * 10, flag ? 14 : 8, 8);
			Vector2 origin = rectangle.Size() / 2f;
			float scale = Utils.Remap((float)this._lifeTimeCounted, 0f, 90f, 0f, 1f, true) * Utils.Remap((float)this._lifeTimeCounted, (float)(this._lifeTimeTotal - 90), (float)this._lifeTimeTotal, 1f, 0f, true);
			Color color = Lighting.GetColor(this._localPosition.ToTileCoordinates());
			this._overrideColor = Color.White;
			Vector4 vector2 = this._overrideColor.ToVector4() * color.ToVector4();
			Color value2 = new Color(vector2);
			float scale2 = 0.75f;
			spritebatch.Draw(value, settings.AnchorPosition + this._localPosition, new Rectangle?(rectangle), value2 * scale, 0f, origin, scale2, (vector.X < 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x0052C9A0 File Offset: 0x0052ABA0
		private void Draw_Fly(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Vector2 vector = this._velocity + this._addedVelocity;
			Texture2D value = TextureAssets.Extra[262].Value;
			int frameY = this._lifeTimeCounted % 6 / 3;
			Rectangle value2 = value.Frame(1, 6, 0, frameY, 0, 0);
			Vector2 origin = new Vector2((float)((vector.X > 0f) ? 3 : 1), 3f);
			float scale = Utils.Remap((float)this._lifeTimeCounted, 0f, 90f, 0f, 1f, true) * Utils.Remap((float)this._lifeTimeCounted, (float)(this._lifeTimeTotal - 90), (float)this._lifeTimeTotal, 1f, 0f, true);
			Color color = Lighting.GetColor(this._localPosition.ToTileCoordinates());
			if (this._overrideColor == default(Color))
			{
				spritebatch.Draw(value, settings.AnchorPosition + this._localPosition, new Rectangle?(value2), color * scale, 0f, origin, 1f, (vector.X > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				return;
			}
			Vector4 vector2 = this._overrideColor.ToVector4() * color.ToVector4();
			Color value3 = new Color(vector2);
			value2.Offset(0, 12);
			spritebatch.Draw(value, settings.AnchorPosition + this._localPosition, new Rectangle?(value2), value3 * scale, 0f, origin, 1f, (vector.X > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			value2.Offset(0, 12);
			spritebatch.Draw(value, settings.AnchorPosition + this._localPosition, new Rectangle?(value2), color * scale, 0f, origin, 1f, (vector.X > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x0052CB90 File Offset: 0x0052AD90
		public void BeRepelled(ref ParticleRepelDetails details)
		{
			float num = Utils.Remap(this._localPosition.Distance(details.SourcePosition) - details.Radius, 0f, 100f, 1f, 0f, true);
			if (num <= 0f)
			{
				return;
			}
			Vector2 value = this._localPosition.DirectionFrom(details.SourcePosition).SafeNormalize(-Vector2.UnitY).RotatedByRandom(0.5235987901687622);
			this._addedVelocity = value * 3.5f * num;
			this._lifeTimeCounted += this._repelLifetimeDecay;
		}

		// Token: 0x04004B7C RID: 19324
		private int _lifeTimeCounted;

		// Token: 0x04004B7D RID: 19325
		private int _lifeTimeTotal;

		// Token: 0x04004B80 RID: 19328
		private Vector2 _spawnPosition;

		// Token: 0x04004B81 RID: 19329
		private Vector2 _localPosition;

		// Token: 0x04004B82 RID: 19330
		private Vector2 _velocity;

		// Token: 0x04004B83 RID: 19331
		private float _neverGoBelowThis;

		// Token: 0x04004B84 RID: 19332
		private Vector2 _addedVelocity;

		// Token: 0x04004B85 RID: 19333
		private int _repelLifetimeDecay;

		// Token: 0x04004B86 RID: 19334
		private Color _overrideColor;

		// Token: 0x04004B87 RID: 19335
		private LittleFlyingCritterParticle.FlyType _type;

		// Token: 0x04004B88 RID: 19336
		private int _variantRow;

		// Token: 0x04004B89 RID: 19337
		private int _variantColumn;

		// Token: 0x020007AA RID: 1962
		public enum FlyType
		{
			// Token: 0x0400706C RID: 28780
			RegularFly,
			// Token: 0x0400706D RID: 28781
			ButterFly
		}
	}
}
