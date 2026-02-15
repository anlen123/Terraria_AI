using System;
using Terraria.DataStructures;

namespace Terraria.GameContent.FishDropRules
{
	// Token: 0x0200047D RID: 1149
	public class Roller
	{
		// Token: 0x06003334 RID: 13108 RVA: 0x005F47A5 File Offset: 0x005F29A5
		public void Roll(Projectile projectile, FishingAttempt fisher)
		{
			FishingContext context = this._context;
			context.Player = Main.player[projectile.owner];
			context.Fisher = fisher;
		}

		// Token: 0x04005896 RID: 22678
		private FishingContext _context = new FishingContext();

		// Token: 0x04005897 RID: 22679
		private FishDropRuleList _ruleList = new FishDropRuleList();
	}
}
