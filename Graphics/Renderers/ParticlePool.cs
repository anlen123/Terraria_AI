using System;
using System.Collections.Generic;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x02000204 RID: 516
	public class ParticlePool<T> where T : IPooledParticle
	{
		// Token: 0x0600211A RID: 8474 RVA: 0x0052BC90 File Offset: 0x00529E90
		public int CountParticlesInUse()
		{
			int num = 0;
			for (int i = 0; i < num; i++)
			{
				T t = this._particles[i];
				if (!t.IsRestingInPool)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x0052BCCC File Offset: 0x00529ECC
		public ParticlePool(int initialPoolSize, ParticlePool<T>.ParticleInstantiator instantiator)
		{
			this._particles = new List<T>(initialPoolSize);
			this._instantiator = instantiator;
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x0052BCE8 File Offset: 0x00529EE8
		public T RequestParticle()
		{
			if (Main.NoPooling)
			{
				this._particles.RemoveAll((T p) => p.IsRestingInPool);
			}
			int count = this._particles.Count;
			for (int i = 0; i < count; i++)
			{
				T t = this._particles[i];
				if (t.IsRestingInPool)
				{
					t = this._particles[i];
					t.FetchFromPool();
					return this._particles[i];
				}
			}
			T t2 = this._instantiator();
			this._particles.Add(t2);
			t2.FetchFromPool();
			return t2;
		}

		// Token: 0x04004B72 RID: 19314
		private ParticlePool<T>.ParticleInstantiator _instantiator;

		// Token: 0x04004B73 RID: 19315
		private List<T> _particles;

		// Token: 0x020007A8 RID: 1960
		// (Invoke) Token: 0x060041AB RID: 16811
		public delegate T ParticleInstantiator();
	}
}
