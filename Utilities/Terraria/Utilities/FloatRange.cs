using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Terraria.Utilities.Terraria.Utilities
{
	// Token: 0x020000D8 RID: 216
	public struct FloatRange
	{
		// Token: 0x06001870 RID: 6256 RVA: 0x004E1E0F File Offset: 0x004E000F
		public FloatRange(float minimum, float maximum)
		{
			this.Minimum = minimum;
			this.Maximum = maximum;
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x004E1E1F File Offset: 0x004E001F
		public bool Contains(float f)
		{
			return this.Minimum <= f && f <= this.Maximum;
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x004E1E38 File Offset: 0x004E0038
		public float Lerp(float amount)
		{
			return MathHelper.Lerp(this.Minimum, this.Maximum, amount);
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x004E1E4C File Offset: 0x004E004C
		public static FloatRange operator *(FloatRange range, float scale)
		{
			return new FloatRange(range.Minimum * scale, range.Maximum * scale);
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x004E1E63 File Offset: 0x004E0063
		public static FloatRange operator *(float scale, FloatRange range)
		{
			return range * scale;
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x004E1E6C File Offset: 0x004E006C
		public static FloatRange operator /(FloatRange range, float scale)
		{
			return new FloatRange(range.Minimum / scale, range.Maximum / scale);
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x004E1E83 File Offset: 0x004E0083
		public static FloatRange operator /(float scale, FloatRange range)
		{
			return range / scale;
		}

		// Token: 0x040012C1 RID: 4801
		[JsonProperty("Min")]
		public readonly float Minimum;

		// Token: 0x040012C2 RID: 4802
		[JsonProperty("Max")]
		public readonly float Maximum;
	}
}
