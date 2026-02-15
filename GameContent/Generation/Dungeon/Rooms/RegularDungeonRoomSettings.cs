using System;

namespace Terraria.GameContent.Generation.Dungeon.Rooms
{
	// Token: 0x020004AE RID: 1198
	public class RegularDungeonRoomSettings : DungeonRoomSettings
	{
		// Token: 0x06003445 RID: 13381 RVA: 0x006029FC File Offset: 0x00600BFC
		public override int GetBoundingRadius()
		{
			return (this.OverrideInnerBoundsSize + this.OverrideOuterBoundsSize) * 142 / 100;
		}

		// Token: 0x040059D7 RID: 22999
		public int OverrideInnerBoundsSize;

		// Token: 0x040059D8 RID: 23000
		public int OverrideOuterBoundsSize;
	}
}
