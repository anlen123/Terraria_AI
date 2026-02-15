using System;
using System.Collections.Generic;
using ReLogic.Utilities;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.Generation.Dungeon.Entrances;

namespace Terraria.GameContent.Generation.Dungeon
{
	// Token: 0x02000498 RID: 1176
	public class DungeonGenVars
	{
		// Token: 0x040058F1 RID: 22769
		public int dungeonSide;

		// Token: 0x040058F2 RID: 22770
		public int dungeonLocation;

		// Token: 0x040058F3 RID: 22771
		public DungeonColor dungeonColor;

		// Token: 0x040058F4 RID: 22772
		public ushort brickTileType = 41;

		// Token: 0x040058F5 RID: 22773
		public ushort brickWallType = 7;

		// Token: 0x040058F6 RID: 22774
		public ushort brickCrackedTileType = 481;

		// Token: 0x040058F7 RID: 22775
		public ushort windowGlassWallType = 91;

		// Token: 0x040058F8 RID: 22776
		public ushort windowClosedGlassWallType = 149;

		// Token: 0x040058F9 RID: 22777
		public ushort windowEdgeWallType = 8;

		// Token: 0x040058FA RID: 22778
		public int[] windowPlatformItemTypes;

		// Token: 0x040058FB RID: 22779
		public int generatingDungeonPositionX;

		// Token: 0x040058FC RID: 22780
		public int generatingDungeonPositionY;

		// Token: 0x040058FD RID: 22781
		public int generatingDungeonTopX;

		// Token: 0x040058FE RID: 22782
		public int dungeonLootStyle;

		// Token: 0x040058FF RID: 22783
		public DungeonBounds outerPotentialDungeonBounds = new DungeonBounds();

		// Token: 0x04005900 RID: 22784
		public DungeonBounds innerPotentialDungeonBounds = new DungeonBounds();

		// Token: 0x04005901 RID: 22785
		public DungeonGenerationStyleData dungeonStyle;

		// Token: 0x04005902 RID: 22786
		public List<DungeonGenerationStyleData> dungeonGenerationStyles = new List<DungeonGenerationStyleData>();

		// Token: 0x04005903 RID: 22787
		public DitherSnake dungeonDitherSnake = new DitherSnake();

		// Token: 0x04005904 RID: 22788
		public bool[] isCrackedBrick;

		// Token: 0x04005905 RID: 22789
		public bool[] isPitTrapTile;

		// Token: 0x04005906 RID: 22790
		public bool[] isDungeonTile;

		// Token: 0x04005907 RID: 22791
		public bool[] isDungeonWall;

		// Token: 0x04005908 RID: 22792
		public bool[] isDungeonWallGlass;

		// Token: 0x04005909 RID: 22793
		public bool GeneratingDungeon;

		// Token: 0x0400590A RID: 22794
		public PreGenDungeonEntranceSettings preGenDungeonEntranceSettings;

		// Token: 0x0400590B RID: 22795
		public Vector2D dungeonEntrancePosition;

		// Token: 0x0400590C RID: 22796
		public bool desertChestLootState;
	}
}
