using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x02000212 RID: 530
	public class ShockIconParticle : ABasicParticle
	{
		// Token: 0x06002183 RID: 8579 RVA: 0x0052F57C File Offset: 0x0052D77C
		public override void FetchFromPool()
		{
			base.FetchFromPool();
			this._timeSinceSpawn = 0f;
			this.Opacity = 0f;
			this.FadeInNormalizedTime = 0.1f;
			this.FadeOutNormalizedTime = 0.9f;
			this.TimeToLive = 20f;
			this.InitialScale = 1f;
			this.ColorTint = Color.White;
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x0052F5DC File Offset: 0x0052D7DC
		public override void Update(ref ParticleRendererSettings settings)
		{
			if (this._timeSinceSpawn == 0f)
			{
				this.initialPosition = this.LocalPosition;
			}
			base.Update(ref settings);
			this._timeSinceSpawn += 1f;
			float num = this._timeSinceSpawn / this.TimeToLive;
			this.Scale = Vector2.One * this.InitialScale * Utils.MultiLerp(num, new float[]
			{
				0.2f,
				0.9f,
				1.3f,
				0.9f
			});
			this.Opacity = MathHelper.Clamp(Utils.Remap(num, 0f, this.FadeInNormalizedTime, 0f, 1f, true) * Utils.Remap(num, this.FadeOutNormalizedTime, 1f, 1f, 0f, true), 0f, 1f) * 0.5f;
			if (this.ParentProjectileID != -1 && this.ParentProjectileID >= 0 && this.ParentProjectileID < 1000)
			{
				Projectile projectile = Main.projectile[this.ParentProjectileID];
				this.LocalPosition = projectile.Top + num * this.OffsetFromParent;
			}
			else
			{
				this.LocalPosition = this.initialPosition + num * this.OffsetFromParent;
			}
			if (this._timeSinceSpawn >= this.TimeToLive)
			{
				base.ShouldBeRemovedFromRenderer = true;
			}
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x0052F72C File Offset: 0x0052D92C
		public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Vector2 position = settings.AnchorPosition + this.LocalPosition;
			Texture2D value = TextureAssets.Extra[268].Value;
			Vector2 origin = new Vector2((float)(value.Width / 2), (float)(value.Height / 2));
			Vector2 scale = this.Scale;
			Color color = Color.Lerp(Lighting.GetColor(this.LocalPosition.ToTileCoordinates()).MultiplyRGBA(this.ColorTint), this.ColorTint, 0.75f) * this.Opacity;
			spritebatch.Draw(value, position, new Rectangle?(value.Frame(1, 1, 0, 0, 0, 0)), color, this.Rotation, origin, scale, SpriteEffects.None, 0f);
		}

		// Token: 0x04004BEA RID: 19434
		public float FadeInNormalizedTime = 0.25f;

		// Token: 0x04004BEB RID: 19435
		public float FadeOutNormalizedTime = 0.75f;

		// Token: 0x04004BEC RID: 19436
		public float TimeToLive = 20f;

		// Token: 0x04004BED RID: 19437
		public float Opacity;

		// Token: 0x04004BEE RID: 19438
		public float InitialScale = 1f;

		// Token: 0x04004BEF RID: 19439
		public Color ColorTint = Color.White;

		// Token: 0x04004BF0 RID: 19440
		public int ParentProjectileID = -1;

		// Token: 0x04004BF1 RID: 19441
		public Vector2 OffsetFromParent;

		// Token: 0x04004BF2 RID: 19442
		private Vector2 initialPosition;

		// Token: 0x04004BF3 RID: 19443
		private float _timeSinceSpawn;
	}
}
