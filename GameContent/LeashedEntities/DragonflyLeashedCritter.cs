using System;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x0200045D RID: 1117
	public class DragonflyLeashedCritter : FlyerLeashedCritter
	{
		// Token: 0x0600327F RID: 12927 RVA: 0x005EF9C1 File Offset: 0x005EDBC1
		public DragonflyLeashedCritter()
		{
			this.minWaitTime = 10;
			this.maxFlySpeed = 2.5f;
			this.acceleration = 0.4f;
			this.brakeDuration = 10;
		}

		// Token: 0x04005803 RID: 22531
		public new static DragonflyLeashedCritter Prototype = new DragonflyLeashedCritter();
	}
}
