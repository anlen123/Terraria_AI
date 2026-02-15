using System;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004C9 RID: 1225
	public class DungeonTileClumpSettings : DungeonFeatureSettings
	{
		// Token: 0x04005A3D RID: 23101
		public int RandomSeed;

		// Token: 0x04005A3E RID: 23102
		public double Strength;

		// Token: 0x04005A3F RID: 23103
		public int Steps;

		// Token: 0x04005A40 RID: 23104
		public ushort TileType;

		// Token: 0x04005A41 RID: 23105
		public ushort WallType;

		// Token: 0x04005A42 RID: 23106
		public DungeonBounds AreaToGenerateIn;

		// Token: 0x04005A43 RID: 23107
		public ushort? OnlyReplaceThisTileType;

		// Token: 0x04005A44 RID: 23108
		public ushort? OnlyReplaceThisWallType;
	}
}
