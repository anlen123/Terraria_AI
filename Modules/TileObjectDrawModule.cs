using System;

namespace Terraria.Modules
{
	// Token: 0x02000064 RID: 100
	public class TileObjectDrawModule
	{
		// Token: 0x06001450 RID: 5200 RVA: 0x004BA7E0 File Offset: 0x004B89E0
		public TileObjectDrawModule(TileObjectDrawModule copyFrom = null)
		{
			if (copyFrom == null)
			{
				this.xOffset = 0;
				this.yOffset = 0;
				this.flipHorizontal = false;
				this.flipVertical = false;
				this.stepDown = 0;
				return;
			}
			this.xOffset = copyFrom.xOffset;
			this.yOffset = copyFrom.yOffset;
			this.flipHorizontal = copyFrom.flipHorizontal;
			this.flipVertical = copyFrom.flipVertical;
			this.stepDown = copyFrom.stepDown;
		}

		// Token: 0x04001046 RID: 4166
		public int xOffset;

		// Token: 0x04001047 RID: 4167
		public int yOffset;

		// Token: 0x04001048 RID: 4168
		public bool flipHorizontal;

		// Token: 0x04001049 RID: 4169
		public bool flipVertical;

		// Token: 0x0400104A RID: 4170
		public int stepDown;
	}
}
