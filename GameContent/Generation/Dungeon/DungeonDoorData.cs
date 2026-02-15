using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Generation.Dungeon
{
	// Token: 0x02000492 RID: 1170
	public struct DungeonDoorData
	{
		// Token: 0x040058C9 RID: 22729
		public Point Position;

		// Token: 0x040058CA RID: 22730
		public ushort? OverrideBrickTileType;

		// Token: 0x040058CB RID: 22731
		public ushort? OverrideBrickWallType;

		// Token: 0x040058CC RID: 22732
		public int? OverrideStyle;

		// Token: 0x040058CD RID: 22733
		public int Direction;

		// Token: 0x040058CE RID: 22734
		public bool InAHallway;

		// Token: 0x040058CF RID: 22735
		public int? OverrideWidthFluff;

		// Token: 0x040058D0 RID: 22736
		public bool SkipOtherDoorsCheck;

		// Token: 0x040058D1 RID: 22737
		public bool SkipSpaceCheck;

		// Token: 0x040058D2 RID: 22738
		public bool AlwaysClearArea;
	}
}
