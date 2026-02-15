using System;

namespace Terraria.GameContent.Generation.Dungeon.Rooms
{
	// Token: 0x020004A7 RID: 1191
	public class LivingTreeDungeonRoomSettings : DungeonRoomSettings
	{
		// Token: 0x06003421 RID: 13345 RVA: 0x006012C2 File Offset: 0x005FF4C2
		public override int GetBoundingRadius()
		{
			return this.BoundingRadius;
		}

		// Token: 0x040059BA RID: 22970
		public int InnerWidth;

		// Token: 0x040059BB RID: 22971
		public int InnerHeight;

		// Token: 0x040059BC RID: 22972
		public int Depth;

		// Token: 0x040059BD RID: 22973
		public int BoundingRadius;
	}
}
