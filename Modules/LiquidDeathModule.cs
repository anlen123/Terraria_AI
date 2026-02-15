using System;

namespace Terraria.Modules
{
	// Token: 0x02000062 RID: 98
	public class LiquidDeathModule
	{
		// Token: 0x0600144E RID: 5198 RVA: 0x004BA77C File Offset: 0x004B897C
		public LiquidDeathModule(LiquidDeathModule copyFrom = null)
		{
			if (copyFrom == null)
			{
				this.water = false;
				this.lava = false;
				return;
			}
			this.water = copyFrom.water;
			this.lava = copyFrom.lava;
		}

		// Token: 0x04001042 RID: 4162
		public bool water;

		// Token: 0x04001043 RID: 4163
		public bool lava;
	}
}
