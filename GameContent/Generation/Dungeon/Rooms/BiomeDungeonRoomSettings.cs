using System;

namespace Terraria.GameContent.Generation.Dungeon.Rooms
{
	// Token: 0x020004A2 RID: 1186
	public class BiomeDungeonRoomSettings : DungeonRoomSettings
	{
		// Token: 0x060033FF RID: 13311 RVA: 0x005FF5E2 File Offset: 0x005FD7E2
		public override int GetBoundingRadius()
		{
			return BiomeDungeonRoom.GetBiomeRoomOuterSize(this.StyleData);
		}
	}
}
