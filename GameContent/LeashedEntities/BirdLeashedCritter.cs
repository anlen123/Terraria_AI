using System;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x0200045F RID: 1119
	public class BirdLeashedCritter : FlyerLeashedCritter
	{
		// Token: 0x06003284 RID: 12932 RVA: 0x005EFA3C File Offset: 0x005EDC3C
		public BirdLeashedCritter()
		{
			this.anchorStyle = 2;
			this.minWaitTime = 120;
			this.maxWaitTime = 420;
			this.maxFlySpeed = 1.2f;
			this.acceleration = 0.1f;
			this.rotationScalar = 0.25f;
			this.brakeDuration = 10;
			this.hoverAmplitude = 3f;
			this.hoverPeriod = 0.005f;
		}

		// Token: 0x04005805 RID: 22533
		public new static BirdLeashedCritter Prototype = new BirdLeashedCritter();
	}
}
