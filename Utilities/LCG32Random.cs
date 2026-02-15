using System;

namespace Terraria.Utilities
{
	// Token: 0x020000CB RID: 203
	public struct LCG32Random
	{
		// Token: 0x060017F1 RID: 6129 RVA: 0x004E02B5 File Offset: 0x004DE4B5
		public LCG32Random(uint seed)
		{
			this.state = seed;
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x004E02BE File Offset: 0x004DE4BE
		public void Advance()
		{
			this.state = this.state * 2438952949U + 1U;
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x004E02D4 File Offset: 0x004DE4D4
		public uint Next(uint maxValue)
		{
			this.Advance();
			return (uint)((ulong)this.state * (ulong)maxValue >> 32);
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x004E02EA File Offset: 0x004DE4EA
		public int Next(int maxValue)
		{
			if (maxValue < 0)
			{
				throw new ArgumentOutOfRangeException("maxValue", "maxValue must be positive.");
			}
			return (int)this.Next((uint)maxValue);
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x004E0307 File Offset: 0x004DE507
		public int Next(int minValue, int maxValue)
		{
			return minValue + (int)this.Next((uint)(maxValue - minValue));
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x004E0314 File Offset: 0x004DE514
		public double NextDouble()
		{
			this.Advance();
			return this.state / 4294967296.0;
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x004E032E File Offset: 0x004DE52E
		public float NextFloat()
		{
			return (float)this.NextDouble();
		}

		// Token: 0x040012A1 RID: 4769
		public uint state;
	}
}
