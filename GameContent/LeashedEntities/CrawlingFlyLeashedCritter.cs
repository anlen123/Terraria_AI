using System;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x0200045E RID: 1118
	public class CrawlingFlyLeashedCritter : FlyerLeashedCritter
	{
		// Token: 0x06003281 RID: 12929 RVA: 0x005EF9FB File Offset: 0x005EDBFB
		public CrawlingFlyLeashedCritter()
		{
			this.hasGroundBias = true;
		}

		// Token: 0x06003282 RID: 12930 RVA: 0x005EFA0A File Offset: 0x005EDC0A
		protected override void SetDefaults(Item sample)
		{
			base.SetDefaults(sample);
			this.scale = Main.rand.NextFloat() * 0.2f + 0.7f;
		}

		// Token: 0x04005804 RID: 22532
		public new static CrawlingFlyLeashedCritter Prototype = new CrawlingFlyLeashedCritter();
	}
}
