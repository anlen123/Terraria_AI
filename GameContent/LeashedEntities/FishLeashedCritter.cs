using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x02000461 RID: 1121
	public class FishLeashedCritter : FlyerLeashedCritter
	{
		// Token: 0x06003289 RID: 12937 RVA: 0x005EFAFC File Offset: 0x005EDCFC
		public FishLeashedCritter()
		{
			this.anchorStyle = 3;
			this.minWaitTime = 120;
			this.maxFlySpeed = 0.5f;
			this.acceleration = 0.015f;
			this.hoverAmplitude = 10f;
			this.hoverPeriod = 0.003f;
			this.isAquatic = true;
		}

		// Token: 0x0600328A RID: 12938 RVA: 0x005EFB51 File Offset: 0x005EDD51
		protected override void CopyToDummy()
		{
			base.CopyToDummy();
			LeashedCritter._dummy.wet = true;
		}

		// Token: 0x0600328B RID: 12939 RVA: 0x005EFB64 File Offset: 0x005EDD64
		public override Vector2 GetDrawOffset()
		{
			return base.GetBobbingOffset();
		}

		// Token: 0x04005807 RID: 22535
		public new static FishLeashedCritter Prototype = new FishLeashedCritter();
	}
}
