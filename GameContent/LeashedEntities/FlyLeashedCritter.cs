using System;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x02000457 RID: 1111
	public abstract class FlyLeashedCritter : FlyerLeashedCritter
	{
		// Token: 0x06003262 RID: 12898 RVA: 0x005EF2AA File Offset: 0x005ED4AA
		protected override void SetDefaults(Item sample)
		{
			base.SetDefaults(sample);
			this.scale = (float)Main.rand.Next(75, 111) * 0.01f;
		}
	}
}
