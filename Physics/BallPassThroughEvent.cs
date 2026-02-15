using System;

namespace Terraria.Physics
{
	// Token: 0x0200007A RID: 122
	public struct BallPassThroughEvent
	{
		// Token: 0x06001557 RID: 5463 RVA: 0x004C3CD7 File Offset: 0x004C1ED7
		public BallPassThroughEvent(float timeScale, Tile tile, Entity entity, BallPassThroughType type)
		{
			this.Tile = tile;
			this.Entity = entity;
			this.Type = type;
			this.TimeScale = timeScale;
		}

		// Token: 0x040010DE RID: 4318
		public readonly Tile Tile;

		// Token: 0x040010DF RID: 4319
		public readonly Entity Entity;

		// Token: 0x040010E0 RID: 4320
		public readonly BallPassThroughType Type;

		// Token: 0x040010E1 RID: 4321
		public readonly float TimeScale;
	}
}
