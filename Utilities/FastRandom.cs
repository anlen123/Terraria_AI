using System;

namespace Terraria.Utilities
{
	// Token: 0x020000CE RID: 206
	public struct FastRandom
	{
		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06001809 RID: 6153 RVA: 0x004E04D6 File Offset: 0x004DE6D6
		// (set) Token: 0x0600180A RID: 6154 RVA: 0x004E04DE File Offset: 0x004DE6DE
		public ulong Seed { get; private set; }

		// Token: 0x0600180B RID: 6155 RVA: 0x004E04E7 File Offset: 0x004DE6E7
		public FastRandom(ulong seed)
		{
			this = default(FastRandom);
			this.Seed = seed;
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x004E04F7 File Offset: 0x004DE6F7
		public FastRandom(int seed)
		{
			this = default(FastRandom);
			this.Seed = (ulong)((long)seed);
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x004E0508 File Offset: 0x004DE708
		public FastRandom WithModifier(ulong modifier)
		{
			return new FastRandom(FastRandom.NextSeed(modifier) ^ this.Seed);
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x004E051C File Offset: 0x004DE71C
		public FastRandom WithModifier(int x, int y)
		{
			return this.WithModifier((ulong)((long)x + (long)((ulong)-1640531527) + ((long)y << 6) + (long)((ulong)((long)y) >> 2)));
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x004E0538 File Offset: 0x004DE738
		public static FastRandom CreateWithRandomSeed()
		{
			return new FastRandom((ulong)((long)Guid.NewGuid().GetHashCode()));
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x004E055E File Offset: 0x004DE75E
		public void NextSeed()
		{
			this.Seed = FastRandom.NextSeed(this.Seed);
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x004E0571 File Offset: 0x004DE771
		private int NextBits(int bits)
		{
			this.Seed = FastRandom.NextSeed(this.Seed);
			return (int)(this.Seed >> 48 - bits);
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x004E0593 File Offset: 0x004DE793
		public float NextFloat()
		{
			return (float)this.NextBits(24) * 5.9604645E-08f;
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x004E05A4 File Offset: 0x004DE7A4
		public double NextDouble()
		{
			return (double)((float)this.NextBits(32) * 4.656613E-10f);
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x004E05B8 File Offset: 0x004DE7B8
		public int Next(int max)
		{
			if ((max & -max) == max)
			{
				return (int)((long)max * (long)this.NextBits(31) >> 31);
			}
			int num;
			int num2;
			do
			{
				num = this.NextBits(31);
				num2 = num % max;
			}
			while (num - num2 + (max - 1) < 0);
			return num2;
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x004E05F5 File Offset: 0x004DE7F5
		public int Next(int min, int max)
		{
			return this.Next(max - min) + min;
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x0040E6E5 File Offset: 0x0040C8E5
		private static ulong NextSeed(ulong seed)
		{
			return seed * 25214903917UL + 11UL & 281474976710655UL;
		}

		// Token: 0x040012A7 RID: 4775
		private const ulong RANDOM_MULTIPLIER = 25214903917UL;

		// Token: 0x040012A8 RID: 4776
		private const ulong RANDOM_ADD = 11UL;

		// Token: 0x040012A9 RID: 4777
		private const ulong RANDOM_MASK = 281474976710655UL;
	}
}
