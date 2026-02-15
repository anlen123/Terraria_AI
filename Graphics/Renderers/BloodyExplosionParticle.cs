using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x02000211 RID: 529
	public class BloodyExplosionParticle : ABasicParticle
	{
		// Token: 0x0600217F RID: 8575 RVA: 0x0052F10C File Offset: 0x0052D30C
		public override void FetchFromPool()
		{
			base.FetchFromPool();
			this._timeSinceSpawn = 0f;
			this.Opacity = 0f;
			this.InnerOpacity = 0f;
			this.FadeInNormalizedTime = 0.1f;
			this.FadeOutNormalizedTime = 0.9f;
			this.TimeToLive = 20f;
			this.InitialScale = 1f;
			this.ColorTint = Color.White;
			this.LightColorTint = Color.Transparent;
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x0052F184 File Offset: 0x0052D384
		public override void Update(ref ParticleRendererSettings settings)
		{
			base.Update(ref settings);
			this._timeSinceSpawn += 1f;
			float fromValue = this._timeSinceSpawn / this.TimeToLive;
			this.Scale = Vector2.One * this.InitialScale * Utils.Remap(fromValue, 0f, 0.3f, 0.5f, 1f, true);
			float num = 0.4f;
			this.Opacity = MathHelper.Clamp(Utils.Remap(fromValue, 0f, this.FadeInNormalizedTime, 0f, 1f, true) * Utils.Remap(fromValue, this.FadeOutNormalizedTime, 1f, 1f, 0f, true), 0f, 1f) * num;
			this.InnerOpacity = MathHelper.Clamp(Utils.Remap(fromValue, 0f, this.FadeInNormalizedTime * 0.75f, 0f, 1f, true) * Utils.Remap(fromValue, 0.3f, 0.45f, 1f, 0f, true), 0f, 1f) * num;
			if (this._timeSinceSpawn == 3f)
			{
				Rectangle rectangle = Utils.CenteredRectangle(this.LocalPosition, new Vector2(16f, 16f));
				for (int i = 0; i < 50; i++)
				{
					Vector2 vector = Main.rand.NextVector2CircularEdge(4f, 4f);
					if (i % 2 == 0)
					{
						vector *= 0.5f;
					}
					Dust dust = Main.dust[Dust.NewDust(rectangle.TopLeft(), rectangle.Width, rectangle.Height, 5, 0f, 0f, 100, default(Color), 1.25f + Main.rand.NextFloat() * 0.5f)];
					dust.velocity = vector;
					dust.noGravity = (i % 3 == 0);
				}
			}
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

		// Token: 0x06002181 RID: 8577 RVA: 0x0052F3CC File Offset: 0x0052D5CC
		public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			float num = this._timeSinceSpawn / this.TimeToLive;
			Vector2 position = settings.AnchorPosition + this.LocalPosition;
			Color value = Color.Lerp(Lighting.GetColor(this.LocalPosition.ToTileCoordinates()).MultiplyRGBA(this.ColorTint), this.ColorTint, 0.65f);
			Texture2D value2 = TextureAssets.Extra[174].Value;
			Vector2 origin = new Vector2((float)(value2.Width / 2), (float)(value2.Height / 2));
			Vector2 scale = this.Scale * (1.1f + 0.15f * num);
			Color color = value * this.Opacity;
			Texture2D value3 = TextureAssets.Extra[267].Value;
			Vector2 origin2 = new Vector2((float)(value3.Width / 2), (float)(value3.Height / 2));
			Vector2 scale2 = this.Scale * (1f + 0.05f * num);
			Color color2 = value * this.InnerOpacity;
			spritebatch.Draw(value2, position, new Rectangle?(value2.Frame(1, 1, 0, 0, 0, 0)), color, this.Rotation, origin, scale, SpriteEffects.None, 0f);
			spritebatch.Draw(value3, position, new Rectangle?(value3.Frame(1, 1, 0, 0, 0, 0)), color2, this.Rotation, origin2, scale2, SpriteEffects.None, 0f);
		}

		// Token: 0x04004BE1 RID: 19425
		public float FadeInNormalizedTime = 0.25f;

		// Token: 0x04004BE2 RID: 19426
		public float FadeOutNormalizedTime = 0.75f;

		// Token: 0x04004BE3 RID: 19427
		public float TimeToLive = 20f;

		// Token: 0x04004BE4 RID: 19428
		public float Opacity;

		// Token: 0x04004BE5 RID: 19429
		public float InnerOpacity;

		// Token: 0x04004BE6 RID: 19430
		public float InitialScale = 1f;

		// Token: 0x04004BE7 RID: 19431
		public Color ColorTint = Color.White;

		// Token: 0x04004BE8 RID: 19432
		public Color LightColorTint = Color.Transparent;

		// Token: 0x04004BE9 RID: 19433
		private float _timeSinceSpawn;
	}
}
