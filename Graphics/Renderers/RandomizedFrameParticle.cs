using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x02000210 RID: 528
	public class RandomizedFrameParticle : ABasicParticle
	{
		// Token: 0x06002179 RID: 8569 RVA: 0x0052EF18 File Offset: 0x0052D118
		public override void FetchFromPool()
		{
			base.FetchFromPool();
			this.FadeInNormalizedTime = 0f;
			this.FadeOutNormalizedTime = 1f;
			this.ColorTint = Color.White;
			this.AnimationFramesAmount = 0;
			this.GameFramesPerAnimationFrame = 0;
			this._timeTolive = 0f;
			this._timeSinceSpawn = 0f;
			this._gameFramesCounted = 0;
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x0052EF77 File Offset: 0x0052D177
		public void SetTypeInfo(int animationFramesAmount, int gameFramesPerAnimationFrame, float timeToLive)
		{
			this._timeTolive = timeToLive;
			this.GameFramesPerAnimationFrame = gameFramesPerAnimationFrame;
			this.AnimationFramesAmount = animationFramesAmount;
			this.RandomizeFrame();
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x0052EF94 File Offset: 0x0052D194
		private void RandomizeFrame()
		{
			this._frame = this._texture.Frame(1, this.AnimationFramesAmount, 0, Main.rand.Next(this.AnimationFramesAmount), 0, 0);
			this._origin = this._frame.Size() / 2f;
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x0052EFE8 File Offset: 0x0052D1E8
		public override void Update(ref ParticleRendererSettings settings)
		{
			base.Update(ref settings);
			this._timeSinceSpawn += 1f;
			if (this._timeSinceSpawn >= this._timeTolive)
			{
				base.ShouldBeRemovedFromRenderer = true;
			}
			int num = this._gameFramesCounted + 1;
			this._gameFramesCounted = num;
			if (num >= this.GameFramesPerAnimationFrame)
			{
				this._gameFramesCounted = 0;
				this.RandomizeFrame();
			}
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x0052F04C File Offset: 0x0052D24C
		public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Color color = this.ColorTint * Utils.GetLerpValue(0f, this.FadeInNormalizedTime, this._timeSinceSpawn / this._timeTolive, true) * Utils.GetLerpValue(1f, this.FadeOutNormalizedTime, this._timeSinceSpawn / this._timeTolive, true);
			spritebatch.Draw(this._texture.Value, settings.AnchorPosition + this.LocalPosition, new Rectangle?(this._frame), color, this.Rotation, this._origin, this.Scale, SpriteEffects.None, 0f);
		}

		// Token: 0x04004BD9 RID: 19417
		public float FadeInNormalizedTime;

		// Token: 0x04004BDA RID: 19418
		public float FadeOutNormalizedTime = 1f;

		// Token: 0x04004BDB RID: 19419
		public Color ColorTint = Color.White;

		// Token: 0x04004BDC RID: 19420
		public int AnimationFramesAmount;

		// Token: 0x04004BDD RID: 19421
		public int GameFramesPerAnimationFrame;

		// Token: 0x04004BDE RID: 19422
		private float _timeTolive;

		// Token: 0x04004BDF RID: 19423
		private float _timeSinceSpawn;

		// Token: 0x04004BE0 RID: 19424
		private int _gameFramesCounted;
	}
}
