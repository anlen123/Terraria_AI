using System;

namespace Terraria.GameContent.Generation.Dungeon.Halls
{
	// Token: 0x020004C2 RID: 1218
	public abstract class DungeonHallSettings
	{
		// Token: 0x04005A1F RID: 23071
		public DungeonHallType HallType;

		// Token: 0x04005A20 RID: 23072
		public int RandomSeed;

		// Token: 0x04005A21 RID: 23073
		public DungeonGenerationStyleData StyleData;

		// Token: 0x04005A22 RID: 23074
		public int OverridePaintTile = -1;

		// Token: 0x04005A23 RID: 23075
		public int OverridePaintWall = -1;

		// Token: 0x04005A24 RID: 23076
		public double CrackedBrickChance = 0.166;

		// Token: 0x04005A25 RID: 23077
		public bool PlaceOverProtectedBricks;

		// Token: 0x04005A26 RID: 23078
		public double ZigzagChance = 0.66;

		// Token: 0x04005A27 RID: 23079
		public bool ForceStyleForDoorsAndPlatforms;

		// Token: 0x04005A28 RID: 23080
		public bool CarveOnly;
	}
}
