using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x0200020F RID: 527
	public class FlameParticle : ABasicParticle
	{
		// Token: 0x06002173 RID: 8563 RVA: 0x0052ED18 File Offset: 0x0052CF18
		public override void FetchFromPool()
		{
			base.FetchFromPool();
			this.FadeOutNormalizedTime = 1f;
			this._timeTolive = 0f;
			this._timeSinceSpawn = 0f;
			this._indexOfPlayerWhoSpawnedThis = 0;
			this._packedShaderIndex = 0;
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x0052ED4F File Offset: 0x0052CF4F
		public override void SetBasicInfo(Asset<Texture2D> textureAsset, Rectangle? frame, Vector2 initialVelocity, Vector2 initialLocalPosition)
		{
			base.SetBasicInfo(textureAsset, frame, initialVelocity, initialLocalPosition);
			this._origin = new Vector2((float)(this._frame.Width / 2), (float)(this._frame.Height - 2));
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x0052ED83 File Offset: 0x0052CF83
		public void SetTypeInfo(float timeToLive, int indexOfPlayerWhoSpawnedIt, int packedShaderIndex)
		{
			this._timeTolive = timeToLive;
			this._indexOfPlayerWhoSpawnedThis = indexOfPlayerWhoSpawnedIt;
			this._packedShaderIndex = packedShaderIndex;
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x0052ED9A File Offset: 0x0052CF9A
		public override void Update(ref ParticleRendererSettings settings)
		{
			base.Update(ref settings);
			this._timeSinceSpawn += 1f;
			if (this._timeSinceSpawn >= this._timeTolive)
			{
				base.ShouldBeRemovedFromRenderer = true;
			}
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x0052EDCC File Offset: 0x0052CFCC
		public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Color color = new Color(120, 120, 120, 60) * Utils.GetLerpValue(1f, this.FadeOutNormalizedTime, this._timeSinceSpawn / this._timeTolive, true);
			Vector2 value = settings.AnchorPosition + this.LocalPosition;
			ulong num = Main.TileFrameSeed ^ ((ulong)this.LocalPosition.X << 32 | (ulong)((uint)this.LocalPosition.Y));
			Player player = Main.player[this._indexOfPlayerWhoSpawnedThis];
			for (int i = 0; i < 4; i++)
			{
				Vector2 value2 = new Vector2((float)Utils.RandomInt(ref num, -2, 3), (float)Utils.RandomInt(ref num, -2, 3));
				DrawData drawData = new DrawData(this._texture.Value, value + value2 * this.Scale, new Rectangle?(this._frame), color, this.Rotation, this._origin, this.Scale, SpriteEffects.None, 0f)
				{
					shader = this._packedShaderIndex
				};
				PlayerDrawHelper.SetShaderForData(player, 0, ref drawData);
				drawData.Draw(spritebatch);
			}
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		// Token: 0x04004BD4 RID: 19412
		public float FadeOutNormalizedTime = 1f;

		// Token: 0x04004BD5 RID: 19413
		private float _timeTolive;

		// Token: 0x04004BD6 RID: 19414
		private float _timeSinceSpawn;

		// Token: 0x04004BD7 RID: 19415
		private int _indexOfPlayerWhoSpawnedThis;

		// Token: 0x04004BD8 RID: 19416
		private int _packedShaderIndex;
	}
}
