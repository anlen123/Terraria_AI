using System;
using Newtonsoft.Json;

namespace Terraria.Utilities
{
	// Token: 0x020000D1 RID: 209
	public struct IntRange
	{
		// Token: 0x06001820 RID: 6176 RVA: 0x004E09A7 File Offset: 0x004DEBA7
		public IntRange(int minimum, int maximum)
		{
			this.Minimum = minimum;
			this.Maximum = maximum;
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x004E09B7 File Offset: 0x004DEBB7
		public static IntRange operator *(IntRange range, float scale)
		{
			return new IntRange((int)((float)range.Minimum * scale), (int)((float)range.Maximum * scale));
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x004E09D2 File Offset: 0x004DEBD2
		public static IntRange operator *(float scale, IntRange range)
		{
			return range * scale;
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x004E09DB File Offset: 0x004DEBDB
		public static IntRange operator /(IntRange range, float scale)
		{
			return new IntRange((int)((float)range.Minimum / scale), (int)((float)range.Maximum / scale));
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x004E09F6 File Offset: 0x004DEBF6
		public static IntRange operator /(float scale, IntRange range)
		{
			return range / scale;
		}

		// Token: 0x040012AB RID: 4779
		[JsonProperty("Min")]
		public readonly int Minimum;

		// Token: 0x040012AC RID: 4780
		[JsonProperty("Max")]
		public readonly int Maximum;
	}
}
