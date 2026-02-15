using System;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x02000460 RID: 1120
	public class WaterfowlLeashedCritter : BirdLeashedCritter
	{
		// Token: 0x06003286 RID: 12934 RVA: 0x005EFAB4 File Offset: 0x005EDCB4
		public WaterfowlLeashedCritter()
		{
			this.hasGroundBias = true;
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x005EFAC3 File Offset: 0x005EDCC3
		protected override void CopyToDummy()
		{
			base.CopyToDummy();
			if (this.velocity.Y != 0f)
			{
				LeashedCritter._dummy.type++;
			}
		}

		// Token: 0x04005806 RID: 22534
		public new static WaterfowlLeashedCritter Prototype = new WaterfowlLeashedCritter();
	}
}
