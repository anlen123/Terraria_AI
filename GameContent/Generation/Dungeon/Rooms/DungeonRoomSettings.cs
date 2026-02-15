using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Biomes;

namespace Terraria.GameContent.Generation.Dungeon.Rooms
{
	// Token: 0x020004B2 RID: 1202
	public abstract class DungeonRoomSettings
	{
		// Token: 0x0600344B RID: 13387
		public abstract int GetBoundingRadius();

		// Token: 0x040059EB RID: 23019
		public DungeonControlLine ControlLine;

		// Token: 0x040059EC RID: 23020
		public Point RoomPosition;

		// Token: 0x040059ED RID: 23021
		public DungeonRoomType RoomType;

		// Token: 0x040059EE RID: 23022
		public int RandomSeed;

		// Token: 0x040059EF RID: 23023
		public DungeonGenerationStyleData StyleData;

		// Token: 0x040059F0 RID: 23024
		public int ProgressionStage;

		// Token: 0x040059F1 RID: 23025
		public bool StartingRoom;

		// Token: 0x040059F2 RID: 23026
		public int OverridePaintTile = -1;

		// Token: 0x040059F3 RID: 23027
		public int OverridePaintWall = -1;

		// Token: 0x040059F4 RID: 23028
		public bool ForceStyleForDoorsAndPlatforms;

		// Token: 0x040059F5 RID: 23029
		public bool OnCurvedLine;

		// Token: 0x040059F6 RID: 23030
		public SnakeOrientation Orientation;

		// Token: 0x040059F7 RID: 23031
		public DungeonUtils.GetHallwayConnectionPoint HallwayConnectionPointOverride;

		// Token: 0x040059F8 RID: 23032
		public int? HallwayPointAdjuster;
	}
}
