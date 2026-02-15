using System;

namespace Terraria.GameContent.Generation.Dungeon.Entrances
{
	// Token: 0x020004EF RID: 1263
	public abstract class DungeonEntranceSettings
	{
		// Token: 0x04005A7E RID: 23166
		public DungeonEntranceType EntranceType;

		// Token: 0x04005A7F RID: 23167
		public int RandomSeed;

		// Token: 0x04005A80 RID: 23168
		public DungeonGenerationStyleData StyleData;

		// Token: 0x04005A81 RID: 23169
		public bool PrecalculateEntrancePosition;
	}
}
