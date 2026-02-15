using System;
using Terraria.GameContent.Generation.Dungeon.Rooms;

namespace Terraria.GameContent.Generation.Dungeon
{
	// Token: 0x02000494 RID: 1172
	public struct DungeonRoomSearchSettings
	{
		// Token: 0x040058E1 RID: 22753
		public int Fluff;

		// Token: 0x040058E2 RID: 22754
		public DungeonRoom ExcludedRoom;

		// Token: 0x040058E3 RID: 22755
		public ProgressionStageCheck ProgressionStageCheck;

		// Token: 0x040058E4 RID: 22756
		public int? ProgressionStage;

		// Token: 0x040058E5 RID: 22757
		public int? MaximumDistance;
	}
}
