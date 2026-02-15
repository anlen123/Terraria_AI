using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x02000203 RID: 515
	public class ParticleRenderer
	{
		// Token: 0x06002115 RID: 8469 RVA: 0x0052BB2C File Offset: 0x00529D2C
		public ParticleRenderer()
		{
			this.Settings = default(ParticleRendererSettings);
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x0052BB59 File Offset: 0x00529D59
		public void Add(IParticle particle)
		{
			this.Particles.Add(particle);
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x0052BB68 File Offset: 0x00529D68
		public void Clear()
		{
			for (int i = 0; i < this.Particles.Count; i++)
			{
				IPooledParticle pooledParticle = this.Particles[i] as IPooledParticle;
				if (pooledParticle != null)
				{
					pooledParticle.RestInPool();
				}
			}
			this.Particles.Clear();
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x0052BBB4 File Offset: 0x00529DB4
		public void Update()
		{
			for (int i = 0; i < this.Particles.Count; i++)
			{
				if (this.Particles[i].ShouldBeRemovedFromRenderer)
				{
					IPooledParticle pooledParticle = this.Particles[i] as IPooledParticle;
					if (pooledParticle != null)
					{
						pooledParticle.RestInPool();
					}
					this.Particles.RemoveAt(i);
					i--;
				}
				else
				{
					this.Particles[i].Update(ref this.Settings);
				}
			}
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x0052BC30 File Offset: 0x00529E30
		public void Draw(SpriteBatch spriteBatch)
		{
			TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
			for (int i = 0; i < this.Particles.Count; i++)
			{
				if (!this.Particles[i].ShouldBeRemovedFromRenderer)
				{
					this.Particles[i].Draw(ref this.Settings, spriteBatch);
				}
			}
			TimeLogger.Particles.AddTime(fromTimestamp);
		}

		// Token: 0x04004B70 RID: 19312
		public ParticleRendererSettings Settings;

		// Token: 0x04004B71 RID: 19313
		public List<IParticle> Particles = new List<IParticle>();
	}
}
