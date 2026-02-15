using System;
using System.Collections.Generic;

namespace Terraria.GameContent.Personalities
{
	// Token: 0x02000430 RID: 1072
	public struct HelperInfo
	{
		// Token: 0x040056B8 RID: 22200
		public Player player;

		// Token: 0x040056B9 RID: 22201
		public NPC npc;

		// Token: 0x040056BA RID: 22202
		public List<NPC> NearbyNPCs;

		// Token: 0x040056BB RID: 22203
		public bool[] nearbyNPCsByType;
	}
}
