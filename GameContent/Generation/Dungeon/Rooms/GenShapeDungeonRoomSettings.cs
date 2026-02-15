using System;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.Rooms
{
	// Token: 0x020004AC RID: 1196
	public class GenShapeDungeonRoomSettings : DungeonRoomSettings
	{
		// Token: 0x06003437 RID: 13367 RVA: 0x006023B9 File Offset: 0x006005B9
		public override int GetBoundingRadius()
		{
			return this.BoundingRadius;
		}

		// Token: 0x040059D0 RID: 22992
		public GenShapeType ShapeType;

		// Token: 0x040059D1 RID: 22993
		public GenShape InnerShape;

		// Token: 0x040059D2 RID: 22994
		public GenShape OuterShape;

		// Token: 0x040059D3 RID: 22995
		public int BoundingRadius;
	}
}
