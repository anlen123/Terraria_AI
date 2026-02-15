using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x02000213 RID: 531
	public class GasParticle : ABasicParticle
	{
		// Token: 0x06002187 RID: 8583 RVA: 0x0052F834 File Offset: 0x0052DA34
		public override void FetchFromPool()
		{
			base.FetchFromPool();
			this.ColorTint = Color.Transparent;
			this._timeSinceSpawn = 0f;
			this.Opacity = 0f;
			this.FadeInNormalizedTime = 0.25f;
			this.FadeOutNormalizedTime = 0.75f;
			this.TimeToLive = 80f;
			this._internalIndentifier = Main.rand.Next(255);
			this.SlowdownScalar = 0.95f;
			this.LightColorTint = Color.Transparent;
			this.InitialScale = 1f;
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x0052F8C0 File Offset: 0x0052DAC0
		public override void Update(ref ParticleRendererSettings settings)
		{
			base.Update(ref settings);
			this._timeSinceSpawn += 1f;
			float fromValue = this._timeSinceSpawn / this.TimeToLive;
			this.Scale = Vector2.One * this.InitialScale * Utils.Remap(fromValue, 0f, 0.95f, 1f, 1.3f, true);
			this.Opacity = MathHelper.Clamp(Utils.Remap(fromValue, 0f, this.FadeInNormalizedTime, 0f, 1f, true) * Utils.Remap(fromValue, this.FadeOutNormalizedTime, 1f, 1f, 0f, true), 0f, 1f) * 0.85f;
			this.Rotation = (float)this._internalIndentifier * 0.4002029f + this._timeSinceSpawn * 6.2831855f / 480f * 0.5f;
			this.Velocity *= this.SlowdownScalar;
			if (this.LightColorTint != Color.Transparent)
			{
				Color color = this.LightColorTint * this.Opacity;
				Lighting.AddLight(this.LocalPosition, (float)color.R / 255f, (float)color.G / 255f, (float)color.B / 255f);
			}
			if (this._timeSinceSpawn >= this.TimeToLive)
			{
				base.ShouldBeRemovedFromRenderer = true;
			}
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x0052FA30 File Offset: 0x0052DC30
		public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Main.instance.LoadProjectile(1007);
			Texture2D value = TextureAssets.Projectile[1007].Value;
			Vector2 origin = new Vector2((float)(value.Width / 2), (float)(value.Height / 2));
			Vector2 position = settings.AnchorPosition + this.LocalPosition;
			Color color = Color.Lerp(Lighting.GetColor(this.LocalPosition.ToTileCoordinates()), this.ColorTint, 0.2f) * this.Opacity;
			Vector2 scale = this.Scale;
			spritebatch.Draw(value, position, new Rectangle?(value.Frame(1, 1, 0, 0, 0, 0)), color, this.Rotation, origin, scale, SpriteEffects.None, 0f);
			spritebatch.Draw(value, position, new Rectangle?(value.Frame(1, 1, 0, 0, 0, 0)), color * 0.25f, this.Rotation, origin, scale * (1f + this.Opacity * 1.5f), SpriteEffects.None, 0f);
		}

		// Token: 0x04004BF4 RID: 19444
		public float FadeInNormalizedTime = 0.25f;

		// Token: 0x04004BF5 RID: 19445
		public float FadeOutNormalizedTime = 0.75f;

		// Token: 0x04004BF6 RID: 19446
		public float TimeToLive = 80f;

		// Token: 0x04004BF7 RID: 19447
		public Color ColorTint;

		// Token: 0x04004BF8 RID: 19448
		public float Opacity;

		// Token: 0x04004BF9 RID: 19449
		public float AdditiveAmount = 1f;

		// Token: 0x04004BFA RID: 19450
		public float FadeInEnd = 20f;

		// Token: 0x04004BFB RID: 19451
		public float FadeOutStart = 30f;

		// Token: 0x04004BFC RID: 19452
		public float FadeOutEnd = 45f;

		// Token: 0x04004BFD RID: 19453
		public float SlowdownScalar = 0.95f;

		// Token: 0x04004BFE RID: 19454
		private float _timeSinceSpawn;

		// Token: 0x04004BFF RID: 19455
		public Color LightColorTint;

		// Token: 0x04004C00 RID: 19456
		private int _internalIndentifier;

		// Token: 0x04004C01 RID: 19457
		public float InitialScale = 1f;
	}
}
