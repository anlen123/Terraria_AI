using System;
using Terraria.Enums;

namespace Terraria.Modules
{
	// Token: 0x02000063 RID: 99
	public class LiquidPlacementModule
	{
		// Token: 0x0600144F RID: 5199 RVA: 0x004BA7AE File Offset: 0x004B89AE
		public LiquidPlacementModule(LiquidPlacementModule copyFrom = null)
		{
			if (copyFrom == null)
			{
				this.water = LiquidPlacement.Allowed;
				this.lava = LiquidPlacement.Allowed;
				return;
			}
			this.water = copyFrom.water;
			this.lava = copyFrom.lava;
		}

		// Token: 0x04001044 RID: 4164
		public LiquidPlacement water;

		// Token: 0x04001045 RID: 4165
		public LiquidPlacement lava;
	}
}
