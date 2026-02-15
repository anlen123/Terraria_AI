using System;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x02000469 RID: 1129
	public class SnailLeashedCritter : CrawlerLeashedCritter
	{
		// Token: 0x060032C5 RID: 12997 RVA: 0x005F1AE7 File Offset: 0x005EFCE7
		protected override void SetDefaults(Item sample)
		{
			base.SetDefaults(sample);
			if (this.npcType == 359)
			{
				this.scale = (float)Main.rand.Next(80, 111) * 0.01f;
			}
		}

		// Token: 0x060032C6 RID: 12998 RVA: 0x005F1B18 File Offset: 0x005EFD18
		protected override void VisualEffects()
		{
			base.VisualEffects();
			int npcType = this.npcType;
			if (npcType == 360)
			{
				Lighting.AddLight((int)base.Center.X / 16, (int)base.Center.Y / 16, 0.1f, 0.2f, 0.7f);
				return;
			}
			if (npcType != 655)
			{
				return;
			}
			Lighting.AddLight((int)base.Center.X / 16, (int)base.Center.Y / 16, 0.6f, 0.3f, 0.1f);
		}

		// Token: 0x04005841 RID: 22593
		public new static SnailLeashedCritter Prototype = new SnailLeashedCritter();
	}
}
