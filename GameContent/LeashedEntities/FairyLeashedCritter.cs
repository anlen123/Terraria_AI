using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x02000462 RID: 1122
	public class FairyLeashedCritter : FlyerLeashedCritter
	{
		// Token: 0x0600328D RID: 12941 RVA: 0x005EFB78 File Offset: 0x005EDD78
		public FairyLeashedCritter()
		{
			this.minWaitTime = 30;
			this.maxWaitTime = 90;
			this.maxFlySpeed = 1.1f;
			this.acceleration = 0.05f;
			this.rotationScalar = 0.25f;
			this.brakeDuration = 30;
		}

		// Token: 0x0600328E RID: 12942 RVA: 0x005EFBC4 File Offset: 0x005EDDC4
		protected override void VisualEffects()
		{
			base.VisualEffects();
			Color value = Color.HotPink;
			Color value2 = Color.LightPink;
			int num = 4;
			if (this.npcType == 584)
			{
				value = Color.LimeGreen;
				value2 = Color.LightSeaGreen;
			}
			if (this.npcType == 585)
			{
				value = Color.RoyalBlue;
				value2 = Color.LightBlue;
			}
			if ((int)Main.timeForVisualEffects % 4 == 0 && Main.rand.Next(4) != 0)
			{
				this.position += this.netOffset;
				Dust dust = Dust.NewDustDirect(base.Center - new Vector2(4f) + Main.rand.NextVector2Circular(2f, 2f), num, num, 278, 0f, 0f, 200, Color.Lerp(value, value2, Main.rand.NextFloat()), 0.65f);
				dust.velocity *= 0f;
				dust.velocity += this.velocity * 0.3f;
				dust.noGravity = true;
				dust.noLight = true;
				this.position -= this.netOffset;
			}
			Lighting.AddLight(base.Center, value.ToVector3() * 0.7f);
		}

		// Token: 0x04005808 RID: 22536
		public new static FairyLeashedCritter Prototype = new FairyLeashedCritter();
	}
}
