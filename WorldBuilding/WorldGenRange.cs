using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Terraria.Utilities;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000BF RID: 191
	public class WorldGenRange
	{
		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060017A9 RID: 6057 RVA: 0x004DEFF0 File Offset: 0x004DD1F0
		public int ScaledMinimum
		{
			get
			{
				return this.ScaleValue(this.Minimum);
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060017AA RID: 6058 RVA: 0x004DEFFE File Offset: 0x004DD1FE
		public int ScaledMaximum
		{
			get
			{
				return this.ScaleValue(this.Maximum);
			}
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x004DF00C File Offset: 0x004DD20C
		public WorldGenRange(int minimum, int maximum)
		{
			this.Minimum = minimum;
			this.Maximum = maximum;
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x004DF022 File Offset: 0x004DD222
		public int GetRandom(UnifiedRandom random)
		{
			return random.Next(this.ScaledMinimum, this.ScaledMaximum + 1);
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x004DF038 File Offset: 0x004DD238
		private int ScaleValue(int value)
		{
			double num = 1.0;
			switch (this.ScaleWith)
			{
			case WorldGenRange.ScalingMode.None:
				num = 1.0;
				break;
			case WorldGenRange.ScalingMode.WorldArea:
				num = (double)(Main.maxTilesX * Main.maxTilesY) / 5040000.0;
				break;
			case WorldGenRange.ScalingMode.WorldWidth:
				num = (double)Main.maxTilesX / 4200.0;
				break;
			}
			return (int)(num * (double)value);
		}

		// Token: 0x0400127C RID: 4732
		public static readonly WorldGenRange Empty = new WorldGenRange(0, 0);

		// Token: 0x0400127D RID: 4733
		[JsonProperty("Min")]
		public readonly int Minimum;

		// Token: 0x0400127E RID: 4734
		[JsonProperty("Max")]
		public readonly int Maximum;

		// Token: 0x0400127F RID: 4735
		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		public readonly WorldGenRange.ScalingMode ScaleWith;

		// Token: 0x020006EB RID: 1771
		public enum ScalingMode
		{
			// Token: 0x040067C3 RID: 26563
			None,
			// Token: 0x040067C4 RID: 26564
			WorldArea,
			// Token: 0x040067C5 RID: 26565
			WorldWidth
		}
	}
}
