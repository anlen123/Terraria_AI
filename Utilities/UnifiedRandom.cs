using System;

namespace Terraria.Utilities
{
	// Token: 0x020000D4 RID: 212
	[Serializable]
	public class UnifiedRandom
	{
		// Token: 0x0600183E RID: 6206 RVA: 0x004E12AE File Offset: 0x004DF4AE
		public UnifiedRandom() : this(Environment.TickCount)
		{
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x004E12BB File Offset: 0x004DF4BB
		public UnifiedRandom(int Seed)
		{
			this.SetSeed(Seed);
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x004E12D8 File Offset: 0x004DF4D8
		public void SetSeed(int Seed)
		{
			for (int i = 0; i < this.SeedArray.Length; i++)
			{
				this.SeedArray[i] = 0;
			}
			int num = (Seed == int.MinValue) ? int.MaxValue : Math.Abs(Seed);
			int num2 = 161803398 - num;
			this.SeedArray[55] = num2;
			int num3 = 1;
			for (int j = 1; j < 55; j++)
			{
				int num4 = 21 * j % 55;
				this.SeedArray[num4] = num3;
				num3 = num2 - num3;
				if (num3 < 0)
				{
					num3 += int.MaxValue;
				}
				num2 = this.SeedArray[num4];
			}
			for (int k = 1; k < 5; k++)
			{
				for (int l = 1; l < 56; l++)
				{
					this.SeedArray[l] -= this.SeedArray[1 + (l + 30) % 55];
					if (this.SeedArray[l] < 0)
					{
						this.SeedArray[l] += int.MaxValue;
					}
				}
			}
			this.inext = 0;
			this.inextp = 21;
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x004E13E0 File Offset: 0x004DF5E0
		protected double Sample()
		{
			return (double)this.InternalSample() * 4.656612875245797E-10;
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x004E13F4 File Offset: 0x004DF5F4
		private int InternalSample()
		{
			int num = this.inext;
			int num2 = this.inextp;
			if (++num >= 56)
			{
				num = 1;
			}
			if (++num2 >= 56)
			{
				num2 = 1;
			}
			int num3 = this.SeedArray[num] - this.SeedArray[num2];
			if (num3 == 2147483647)
			{
				num3--;
			}
			if (num3 < 0)
			{
				num3 += int.MaxValue;
			}
			this.SeedArray[num] = num3;
			this.inext = num;
			this.inextp = num2;
			return num3;
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x004E1467 File Offset: 0x004DF667
		public int Peek()
		{
			return this.SeedArray[this.inext] - this.SeedArray[this.inextp];
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x004E1484 File Offset: 0x004DF684
		public int Next()
		{
			return this.InternalSample();
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x004E148C File Offset: 0x004DF68C
		private double GetSampleForLargeRange()
		{
			int num = this.InternalSample();
			if (this.InternalSample() % 2 == 0)
			{
				num = -num;
			}
			return ((double)num + 2147483646.0) / 4294967293.0;
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x004E14CC File Offset: 0x004DF6CC
		public int Next(int minValue, int maxValue)
		{
			if (minValue > maxValue)
			{
				throw new ArgumentOutOfRangeException("minValue", "minValue must be less than maxValue");
			}
			long num = (long)maxValue - (long)minValue;
			if (num <= 2147483647L)
			{
				return (int)(this.Sample() * (double)num) + minValue;
			}
			return (int)((long)(this.GetSampleForLargeRange() * (double)num) + (long)minValue);
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x004E1517 File Offset: 0x004DF717
		public int Next(int maxValue)
		{
			if (maxValue < 0)
			{
				throw new ArgumentOutOfRangeException("maxValue", "maxValue must be positive.");
			}
			return (int)(this.Sample() * (double)maxValue);
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x004E1537 File Offset: 0x004DF737
		public double NextDouble()
		{
			return this.Sample();
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x004E1540 File Offset: 0x004DF740
		public void NextBytes(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = (byte)(this.InternalSample() % 256);
			}
		}

		// Token: 0x040012B6 RID: 4790
		private const int MBIG = 2147483647;

		// Token: 0x040012B7 RID: 4791
		private const int MSEED = 161803398;

		// Token: 0x040012B8 RID: 4792
		private const int MZ = 0;

		// Token: 0x040012B9 RID: 4793
		private int inext;

		// Token: 0x040012BA RID: 4794
		private int inextp;

		// Token: 0x040012BB RID: 4795
		private int[] SeedArray = new int[56];
	}
}
