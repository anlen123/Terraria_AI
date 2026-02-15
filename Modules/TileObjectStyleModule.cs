using System;

namespace Terraria.Modules
{
	// Token: 0x02000067 RID: 103
	public class TileObjectStyleModule
	{
		// Token: 0x06001453 RID: 5203 RVA: 0x004BA948 File Offset: 0x004B8B48
		public TileObjectStyleModule(TileObjectStyleModule copyFrom = null)
		{
			if (copyFrom == null)
			{
				this.style = 0;
				this.horizontal = false;
				this.styleWrapLimit = 0;
				this.styleWrapLimitVisualOverride = null;
				this.styleLineSkipVisualoverride = null;
				this.styleMultiplier = 1;
				this.styleLineSkip = 1;
				return;
			}
			this.style = copyFrom.style;
			this.horizontal = copyFrom.horizontal;
			this.styleWrapLimit = copyFrom.styleWrapLimit;
			this.styleMultiplier = copyFrom.styleMultiplier;
			this.styleLineSkip = copyFrom.styleLineSkip;
			this.styleWrapLimitVisualOverride = copyFrom.styleWrapLimitVisualOverride;
			this.styleLineSkipVisualoverride = copyFrom.styleLineSkipVisualoverride;
		}

		// Token: 0x04001050 RID: 4176
		public int style;

		// Token: 0x04001051 RID: 4177
		public bool horizontal;

		// Token: 0x04001052 RID: 4178
		public int styleWrapLimit;

		// Token: 0x04001053 RID: 4179
		public int styleMultiplier;

		// Token: 0x04001054 RID: 4180
		public int styleLineSkip;

		// Token: 0x04001055 RID: 4181
		public int? styleWrapLimitVisualOverride;

		// Token: 0x04001056 RID: 4182
		public int? styleLineSkipVisualoverride;
	}
}
