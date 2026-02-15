using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x0200045A RID: 1114
	public class HellButterflyLeashedCritter : FlyLeashedCritter
	{
		// Token: 0x0600326F RID: 12911 RVA: 0x005EF620 File Offset: 0x005ED820
		protected override void VisualEffects()
		{
			base.VisualEffects();
			this.position += this.netOffset;
			Lighting.AddLight((int)base.Center.X / 16, (int)base.Center.Y / 16, 0.6f, 0.3f, 0.1f);
			if (Main.rand.Next(60) == 0)
			{
				int num = Dust.NewDust(this.position, this.width, this.height, 6, 0f, 0f, 254, default(Color), 1f);
				Main.dust[num].velocity *= 0f;
			}
			this.position -= this.netOffset;
		}

		// Token: 0x040057FC RID: 22524
		public new static HellButterflyLeashedCritter Prototype = new HellButterflyLeashedCritter();
	}
}
