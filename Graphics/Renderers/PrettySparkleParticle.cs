using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x02000214 RID: 532
	public class PrettySparkleParticle : ABasicParticle
	{
		// Token: 0x0600218B RID: 8587 RVA: 0x0052FBA8 File Offset: 0x0052DDA8
		public override void FetchFromPool()
		{
			base.FetchFromPool();
			this.ColorTint = Color.Transparent;
			this._timeSinceSpawn = 0f;
			this.Opacity = 0f;
			this.FadeInNormalizedTime = 0.05f;
			this.FadeOutNormalizedTime = 0.9f;
			this.TimeToLive = 60f;
			this.AdditiveAmount = 1f;
			this.FadeInEnd = 20f;
			this.FadeOutStart = 30f;
			this.FadeOutEnd = 45f;
			this.DrawVerticalAxis = (this.DrawHorizontalAxis = true);
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x0052FC3C File Offset: 0x0052DE3C
		public override void Update(ref ParticleRendererSettings settings)
		{
			base.Update(ref settings);
			this._timeSinceSpawn += 1f;
			this.Opacity = Utils.GetLerpValue(0f, this.FadeInNormalizedTime, this._timeSinceSpawn / this.TimeToLive, true) * Utils.GetLerpValue(1f, this.FadeOutNormalizedTime, this._timeSinceSpawn / this.TimeToLive, true);
			if (this._timeSinceSpawn >= this.TimeToLive)
			{
				base.ShouldBeRemovedFromRenderer = true;
			}
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x0052FCBC File Offset: 0x0052DEBC
		public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Color value = Color.White * this.Opacity * 0.9f;
			value.A /= 2;
			Texture2D value2 = TextureAssets.Extra[98].Value;
			Color color = this.ColorTint * this.Opacity * 0.5f;
			color.A = (byte)((float)color.A * (1f - this.AdditiveAmount));
			Vector2 origin = value2.Size() / 2f;
			Color color2 = value * 0.5f;
			float t = this._timeSinceSpawn / this.TimeToLive * 60f;
			float num = Utils.GetLerpValue(0f, this.FadeInEnd, t, true) * Utils.GetLerpValue(this.FadeOutEnd, this.FadeOutStart, t, true);
			Vector2 vector = new Vector2(0.3f, 2f) * num * this.Scale;
			Vector2 vector2 = new Vector2(0.3f, 1f) * num * this.Scale;
			color *= num;
			color2 *= num;
			Vector2 position = settings.AnchorPosition + this.LocalPosition;
			SpriteEffects effects = SpriteEffects.None;
			if (this.DrawHorizontalAxis)
			{
				spritebatch.Draw(value2, position, null, color, 1.5707964f + this.Rotation, origin, vector, effects, 0f);
			}
			if (this.DrawVerticalAxis)
			{
				spritebatch.Draw(value2, position, null, color, 0f + this.Rotation, origin, vector2, effects, 0f);
			}
			if (this.DrawHorizontalAxis)
			{
				spritebatch.Draw(value2, position, null, color2, 1.5707964f + this.Rotation, origin, vector * 0.6f, effects, 0f);
			}
			if (this.DrawVerticalAxis)
			{
				spritebatch.Draw(value2, position, null, color2, 0f + this.Rotation, origin, vector2 * 0.6f, effects, 0f);
			}
		}

		// Token: 0x04004C02 RID: 19458
		public float FadeInNormalizedTime = 0.05f;

		// Token: 0x04004C03 RID: 19459
		public float FadeOutNormalizedTime = 0.9f;

		// Token: 0x04004C04 RID: 19460
		public float TimeToLive = 60f;

		// Token: 0x04004C05 RID: 19461
		public Color ColorTint;

		// Token: 0x04004C06 RID: 19462
		public float Opacity;

		// Token: 0x04004C07 RID: 19463
		public float AdditiveAmount = 1f;

		// Token: 0x04004C08 RID: 19464
		public float FadeInEnd = 20f;

		// Token: 0x04004C09 RID: 19465
		public float FadeOutStart = 30f;

		// Token: 0x04004C0A RID: 19466
		public float FadeOutEnd = 45f;

		// Token: 0x04004C0B RID: 19467
		public bool DrawHorizontalAxis = true;

		// Token: 0x04004C0C RID: 19468
		public bool DrawVerticalAxis = true;

		// Token: 0x04004C0D RID: 19469
		private float _timeSinceSpawn;
	}
}
