using System;
using Terraria.GameContent.Generation.Dungeon.Rooms;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004E9 RID: 1257
	public class DungeonPitTrapSettings : DungeonFeatureSettings
	{
		// Token: 0x04005A6D RID: 23149
		public DungeonGenerationStyleData Style;

		// Token: 0x04005A6E RID: 23150
		public int Width;

		// Token: 0x04005A6F RID: 23151
		public int Height;

		// Token: 0x04005A70 RID: 23152
		public int EdgeWidth;

		// Token: 0x04005A71 RID: 23153
		public int EdgeHeight;

		// Token: 0x04005A72 RID: 23154
		public int TopDensity;

		// Token: 0x04005A73 RID: 23155
		public bool Flooded;

		// Token: 0x04005A74 RID: 23156
		public DungeonRoom ConnectedRoom;
	}
}
