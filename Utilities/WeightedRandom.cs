using System;
using System.Collections.Generic;
using System.Linq;

namespace Terraria.Utilities
{
	// Token: 0x020000D5 RID: 213
	public class WeightedRandom<T>
	{
		// Token: 0x0600184A RID: 6218 RVA: 0x004E1579 File Offset: 0x004DF779
		public WeightedRandom()
		{
			this.random = new UnifiedRandom();
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x004E159E File Offset: 0x004DF79E
		public WeightedRandom(int seed)
		{
			this.random = new UnifiedRandom(seed);
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x004E15C4 File Offset: 0x004DF7C4
		public WeightedRandom(UnifiedRandom random)
		{
			this.random = random;
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x004E15E5 File Offset: 0x004DF7E5
		public WeightedRandom(params Tuple<T, double>[] theElements)
		{
			this.random = new UnifiedRandom();
			this.elements = theElements.ToList<Tuple<T, double>>();
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x004E1616 File Offset: 0x004DF816
		public WeightedRandom(int seed, params Tuple<T, double>[] theElements)
		{
			this.random = new UnifiedRandom(seed);
			this.elements = theElements.ToList<Tuple<T, double>>();
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x004E1648 File Offset: 0x004DF848
		public WeightedRandom(UnifiedRandom random, params Tuple<T, double>[] theElements)
		{
			this.random = random;
			this.elements = theElements.ToList<Tuple<T, double>>();
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x004E1675 File Offset: 0x004DF875
		public void Add(T element, double weight = 1.0)
		{
			this.elements.Add(new Tuple<T, double>(element, weight));
			this.needsRefresh = true;
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x004E1690 File Offset: 0x004DF890
		public T Get()
		{
			if (this.needsRefresh)
			{
				this.CalculateTotalWeight();
			}
			double num = this.random.NextDouble();
			num *= this._totalWeight;
			foreach (Tuple<T, double> tuple in this.elements)
			{
				if (num <= tuple.Item2)
				{
					return tuple.Item1;
				}
				num -= tuple.Item2;
			}
			return default(T);
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x004E1728 File Offset: 0x004DF928
		public void CalculateTotalWeight()
		{
			this._totalWeight = 0.0;
			foreach (Tuple<T, double> tuple in this.elements)
			{
				this._totalWeight += tuple.Item2;
			}
			this.needsRefresh = false;
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x004E17A0 File Offset: 0x004DF9A0
		public void Clear()
		{
			this.elements.Clear();
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x004E17AD File Offset: 0x004DF9AD
		public static implicit operator T(WeightedRandom<T> weightedRandom)
		{
			return weightedRandom.Get();
		}

		// Token: 0x040012BC RID: 4796
		public readonly List<Tuple<T, double>> elements = new List<Tuple<T, double>>();

		// Token: 0x040012BD RID: 4797
		public readonly UnifiedRandom random;

		// Token: 0x040012BE RID: 4798
		public bool needsRefresh = true;

		// Token: 0x040012BF RID: 4799
		private double _totalWeight;
	}
}
