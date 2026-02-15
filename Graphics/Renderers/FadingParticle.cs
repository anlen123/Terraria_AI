using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x0200020E RID: 526
	public class FadingParticle : ABasicParticle
	{
		// Token: 0x0600216E RID: 8558 RVA: 0x0052EB48 File Offset: 0x0052CD48
		public override void FetchFromPool()
		{
			base.FetchFromPool();
			this.FadeInNormalizedTime = 0f;
			this.FadeOutNormalizedTime = 1f;
			this.ColorTint = Color.White;
			this.timeTolive = 0f;
			this.timeSinceSpawn = 0f;
			this.followPlayerIndex = -1;
			this.Delay = 0;
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x0052EBA0 File Offset: 0x0052CDA0
		public void SetTypeInfo(float timeToLive, bool fullbright = true)
		{
			this.timeTolive = timeToLive;
			this.fullbright = fullbright;
		}

		// Token: 0x06002170 RID: 8560 RVA: 0x0052EBB0 File Offset: 0x0052CDB0
		public override void Update(ref ParticleRendererSettings settings)
		{
			if (this.Delay > 0)
			{
				this.Delay--;
				return;
			}
			base.Update(ref settings);
			this.timeSinceSpawn += 1f;
			if (this.timeSinceSpawn >= this.timeTolive)
			{
				base.ShouldBeRemovedFromRenderer = true;
			}
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x0052EC04 File Offset: 0x0052CE04
		public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Vector2 vector = settings.AnchorPosition + this.LocalPosition;
			if (this.followPlayerIndex != -1)
			{
				vector += Main.player[this.followPlayerIndex].MountedCenter;
			}
			Color color = (this.fullbright ? this.ColorTint : this.ColorTint.MultiplyRGB(Lighting.GetColor(this.LocalPosition.ToTileCoordinates()))) * Utils.GetLerpValue(0f, this.FadeInNormalizedTime, this.timeSinceSpawn / this.timeTolive, true) * Utils.GetLerpValue(1f, this.FadeOutNormalizedTime, this.timeSinceSpawn / this.timeTolive, true);
			spritebatch.Draw(this._texture.Value, vector, new Rectangle?(this._frame), color, this.Rotation, this._origin, this.Scale, SpriteEffects.None, 0f);
		}

		// Token: 0x04004BCC RID: 19404
		public float FadeInNormalizedTime;

		// Token: 0x04004BCD RID: 19405
		public float FadeOutNormalizedTime = 1f;

		// Token: 0x04004BCE RID: 19406
		public Color ColorTint = Color.White;

		// Token: 0x04004BCF RID: 19407
		public int Delay;

		// Token: 0x04004BD0 RID: 19408
		protected float timeTolive;

		// Token: 0x04004BD1 RID: 19409
		protected float timeSinceSpawn;

		// Token: 0x04004BD2 RID: 19410
		protected bool fullbright = true;

		// Token: 0x04004BD3 RID: 19411
		public int followPlayerIndex = -1;
	}
}
