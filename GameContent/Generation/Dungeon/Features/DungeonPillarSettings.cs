using System;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004D2 RID: 1234
	public class DungeonPillarSettings : DungeonFeatureSettings
	{
		// Token: 0x04005A54 RID: 23124
		public DungeonGenerationStyleData Style;

		// Token: 0x04005A55 RID: 23125
		public PillarType PillarType;

		// Token: 0x04005A56 RID: 23126
		public int Width;

		// Token: 0x04005A57 RID: 23127
		public int Height;

		// Token: 0x04005A58 RID: 23128
		public bool Wall;

		// Token: 0x04005A59 RID: 23129
		public int OverridePaintTile = -1;

		// Token: 0x04005A5A RID: 23130
		public int OverridePaintWall = -1;

		// Token: 0x04005A5B RID: 23131
		public bool CrowningOnTop;

		// Token: 0x04005A5C RID: 23132
		public bool CrowningOnBottom;

		// Token: 0x04005A5D RID: 23133
		public bool CrowningStopsAtPillar;

		// Token: 0x04005A5E RID: 23134
		public bool AlwaysPlaceEntirePillar = true;
	}
}
