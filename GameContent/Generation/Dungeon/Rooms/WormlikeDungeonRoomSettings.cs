using System;

namespace Terraria.GameContent.Generation.Dungeon.Rooms
{
	// Token: 0x020004A9 RID: 1193
	public class WormlikeDungeonRoomSettings : DungeonRoomSettings
	{
		// Token: 0x0600342D RID: 13357 RVA: 0x00601BCA File Offset: 0x005FFDCA
		public override int GetBoundingRadius()
		{
			return (int)((16.200000000000003 + (double)(this.FirstSideIterations + this.SecondSideIterations) * 0.5 * 1.4) * 0.5);
		}

		// Token: 0x040059C2 RID: 22978
		public int FirstSideIterations;

		// Token: 0x040059C3 RID: 22979
		public int SecondSideIterations;
	}
}
