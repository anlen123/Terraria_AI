using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000588 RID: 1416
	[Flags]
	public enum TileDataType
	{
		// Token: 0x04005C0D RID: 23565
		Tile = 1,
		// Token: 0x04005C0E RID: 23566
		TilePaint = 2,
		// Token: 0x04005C0F RID: 23567
		Wall = 4,
		// Token: 0x04005C10 RID: 23568
		WallPaint = 8,
		// Token: 0x04005C11 RID: 23569
		Liquid = 16,
		// Token: 0x04005C12 RID: 23570
		Wiring = 32,
		// Token: 0x04005C13 RID: 23571
		Actuator = 64,
		// Token: 0x04005C14 RID: 23572
		Slope = 128,
		// Token: 0x04005C15 RID: 23573
		All = 255
	}
}
