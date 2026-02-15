using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Utilities;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x02000206 RID: 518
	public class StormLightningParticle : IPooledParticle, IParticle
	{
		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06002120 RID: 8480 RVA: 0x0052BDA8 File Offset: 0x00529FA8
		// (set) Token: 0x06002121 RID: 8481 RVA: 0x0052BDB0 File Offset: 0x00529FB0
		public bool ShouldBeRemovedFromRenderer { get; private set; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06002122 RID: 8482 RVA: 0x0052BDB9 File Offset: 0x00529FB9
		// (set) Token: 0x06002123 RID: 8483 RVA: 0x0052BDC1 File Offset: 0x00529FC1
		public bool IsRestingInPool { get; private set; }

		// Token: 0x06002124 RID: 8484 RVA: 0x0052BDCA File Offset: 0x00529FCA
		public void RestInPool()
		{
			this.IsRestingInPool = true;
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x0052BDD3 File Offset: 0x00529FD3
		public virtual void FetchFromPool()
		{
			this._lifeTimeCounted = 0;
			this._lifeTimeTotal = 0;
			this.IsRestingInPool = false;
			this.ShouldBeRemovedFromRenderer = false;
			this.bolts.Clear();
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x0052BDFC File Offset: 0x00529FFC
		public void Prepare(uint seed, Vector2 targetPosition, int lifeTimeTotal, Color color)
		{
			this.Color = color;
			this._lifeTimeTotal = lifeTimeTotal;
			LightningGenerator.Bolt bolt = LightningGenerator.StormLightning.Generate(this.bolts, seed, targetPosition, true, true);
			this.StartPosition = bolt.positions[0];
			this.EndPosition = bolt.positions[bolt.positions.Length - 1];
			LCG32Random lcg32Random = new LCG32Random(seed);
			int maxValue = (int)Math.Ceiling((double)((float)bolt.positions.Length / 10f));
			for (int i = 0; i < bolt.positions.Length; i++)
			{
				if (lcg32Random.Next(maxValue) == 0)
				{
					Vector2 position = bolt.positions[i];
					Vector2 velocity = Vector2.UnitY;
					if (bolt.rotations != null)
					{
						velocity = -bolt.rotations[i].ToRotationVector2();
					}
					Dust dust = Dust.NewDustPerfect(position, 226, null, 0, default(Color), 1f);
					dust.HackFrame(278);
					dust.color = color;
					dust.customData = dust.color;
					dust.velocity = velocity;
					dust.velocity *= 3f + lcg32Random.NextFloat() * 6.5f;
					dust.fadeIn = 0f;
					dust.scale = 0.4f + lcg32Random.NextFloat() * 0.5f;
					dust.noGravity = true;
					dust.position -= dust.velocity * 6f;
					Dust.CloneDust(dust).velocity *= 0.5f;
					dust.scale -= 0.3f;
				}
			}
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x0052BFC8 File Offset: 0x0052A1C8
		public void Update(ref ParticleRendererSettings settings)
		{
			Color color = new Color(80, 220, 220);
			float num = (float)this._lifeTimeCounted / (float)this._lifeTimeTotal;
			float num2 = Utils.Remap(num, 0f, 0.4f, 1f, 0f, true);
			if (num < 0.3f)
			{
				ParticleOrchestrator.RequestParticleSpawn(true, ParticleOrchestraType.StormlightningWindup, new ParticleOrchestraSettings
				{
					PositionInWorld = this.StartPosition,
					MovementVector = Vector2.Zero,
					UniqueInfoPiece = (int)color.PackedValue
				}, null);
			}
			for (int i = 0; i < 3; i++)
			{
				if (Main.rand.Next(4) == 0 && Main.rand.NextFloat() <= num2 * 0.13f)
				{
					Dust dust = Dust.NewDustDirect(this.StartPosition, 16, 16, 306, 0f, 0f, 0, new Color((int)color.R, (int)color.G, (int)color.B, 0), 1f);
					dust.velocity = new Vector2(0f, -4f).RotatedByRandom(1.5707963705062866) * (0.5f + 0.2f * Main.rand.NextFloatDirection());
					dust.scale = 1.8f;
					dust.fadeIn = 0f;
					dust.noGravity = (Main.rand.Next(3) != 0);
					dust.noLight = (dust.noLightEmittence = true);
					Dust dust2 = Dust.CloneDust(dust);
					dust2.color = new Color(255, 255, 255, 0);
					dust2.scale = 1.3f;
				}
			}
			for (int j = -1; j <= 1; j += 2)
			{
				if (Main.rand.Next(4) == 0 && Main.rand.NextFloat() <= num2 * 0.2f)
				{
					Dust dust3 = Dust.NewDustPerfect(this.StartPosition, 306, new Vector2?(new Vector2(0f, -4f).RotatedBy((double)(0.7853982f * (float)j * 1f), default(Vector2))), 0, default(Color), 1f);
					dust3.color = new Color((int)color.R, (int)color.G, (int)color.B, 0);
					dust3.scale = 1.8f;
					dust3.fadeIn = 0f;
					dust3.noGravity = (Main.rand.Next(3) != 0);
					dust3.noLight = (dust3.noLightEmittence = true);
					Dust dust4 = Dust.CloneDust(dust3);
					dust4.color = new Color(255, 255, 255, 0);
					dust4.scale = 1.3f;
				}
			}
			for (int k = 0; k < 2; k++)
			{
				if (Main.rand.Next(4) == 0 && Main.rand.NextFloat() <= 0.2f)
				{
					Dust dust5 = Dust.NewDustPerfect(this.StartPosition, 226, null, 0, default(Color), 1f);
					dust5.HackFrame(278);
					dust5.color = color;
					dust5.customData = dust5.color;
					dust5.velocity *= 1f + Main.rand.NextFloat() * 2.5f;
					dust5.velocity += new Vector2(0f, -2f);
					dust5.fadeIn = 0f;
					dust5.scale = 0.4f + Main.rand.NextFloat() * 0.5f;
					Dust dust6 = dust5;
					dust6.velocity.X = dust6.velocity.X * 2f;
					dust5.velocity = Main.rand.NextVector2Circular(3f, 2f) + new Vector2(0f, -2f);
					dust5.noLight = (dust5.noLightEmittence = true);
					dust5.position -= dust5.velocity * 3f;
				}
			}
			int num3 = this._lifeTimeCounted + 1;
			this._lifeTimeCounted = num3;
			if (num3 >= this._lifeTimeTotal)
			{
				this.ShouldBeRemovedFromRenderer = true;
			}
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x0052C430 File Offset: 0x0052A630
		public void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			StormLightningDrawer stormLightningDrawer = default(StormLightningDrawer);
			foreach (LightningGenerator.Bolt bolt in this.bolts)
			{
				float intensity = bolt.IsMainBolt ? 1f : (0.5f * (float)Math.Pow(0.8, (double)(bolt.forkDepth - 1)));
				stormLightningDrawer.Draw(bolt.positions, bolt.rotations, 16f, this.Color, (float)this._lifeTimeCounted / (float)this._lifeTimeTotal, bolt.IsMainBolt, bolt.progressRange, intensity);
			}
		}

		// Token: 0x04004B75 RID: 19317
		public Color Color;

		// Token: 0x04004B76 RID: 19318
		public Vector2 EndPosition;

		// Token: 0x04004B77 RID: 19319
		public Vector2 StartPosition;

		// Token: 0x04004B78 RID: 19320
		private List<LightningGenerator.Bolt> bolts = new List<LightningGenerator.Bolt>();

		// Token: 0x04004B7A RID: 19322
		private int _lifeTimeCounted;

		// Token: 0x04004B7B RID: 19323
		private int _lifeTimeTotal;
	}
}
