using System;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004CF RID: 1231
	public abstract class DungeonWindowSettings : DungeonFeatureSettings
	{
		// Token: 0x04005A49 RID: 23113
		public DungeonGenerationStyleData Style;

		// Token: 0x04005A4A RID: 23114
		public int OverrideGlassPaint = -1;

		// Token: 0x04005A4B RID: 23115
		public int OverrideGlassType = -1;

		// Token: 0x04005A4C RID: 23116
		public bool Closed;
	}
}
