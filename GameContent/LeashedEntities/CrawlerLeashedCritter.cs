using System;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x02000468 RID: 1128
	public class CrawlerLeashedCritter : WalkerLeashedCritter
	{
		// Token: 0x060032C3 RID: 12995 RVA: 0x005F1AC1 File Offset: 0x005EFCC1
		public CrawlerLeashedCritter()
		{
			this.anchorStyle = 1;
			this.walkingPace = 0.4f;
		}

		// Token: 0x04005840 RID: 22592
		public new static CrawlerLeashedCritter Prototype = new CrawlerLeashedCritter();
	}
}
