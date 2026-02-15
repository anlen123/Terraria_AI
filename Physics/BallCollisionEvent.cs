using System;
using Microsoft.Xna.Framework;

namespace Terraria.Physics
{
	// Token: 0x02000077 RID: 119
	public struct BallCollisionEvent
	{
		// Token: 0x06001554 RID: 5460 RVA: 0x004C3CB0 File Offset: 0x004C1EB0
		public BallCollisionEvent(float timeScale, Vector2 normal, Vector2 impactPoint, Tile tile, Entity entity)
		{
			this.Normal = normal;
			this.ImpactPoint = impactPoint;
			this.Tile = tile;
			this.Entity = entity;
			this.TimeScale = timeScale;
		}

		// Token: 0x040010D3 RID: 4307
		public readonly Vector2 Normal;

		// Token: 0x040010D4 RID: 4308
		public readonly Vector2 ImpactPoint;

		// Token: 0x040010D5 RID: 4309
		public readonly Tile Tile;

		// Token: 0x040010D6 RID: 4310
		public readonly Entity Entity;

		// Token: 0x040010D7 RID: 4311
		public readonly float TimeScale;
	}
}
