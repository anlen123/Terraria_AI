using System;
using Terraria.Utilities;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x020002F2 RID: 754
	public struct DropAttemptInfo
	{
		// Token: 0x04005063 RID: 20579
		public NPC npc;

		// Token: 0x04005064 RID: 20580
		public Player player;

		// Token: 0x04005065 RID: 20581
		public UnifiedRandom rng;

		// Token: 0x04005066 RID: 20582
		public bool IsInSimulation;

		// Token: 0x04005067 RID: 20583
		public bool IsExpertMode;

		// Token: 0x04005068 RID: 20584
		public bool IsMasterMode;
	}
}
