using System;
using Microsoft.Xna.Framework;

namespace Terraria.Physics
{
	// Token: 0x02000079 RID: 121
	public interface IBallContactListener
	{
		// Token: 0x06001555 RID: 5461
		void OnCollision(PhysicsProperties properties, ref Vector2 position, ref Vector2 velocity, ref BallCollisionEvent collision);

		// Token: 0x06001556 RID: 5462
		void OnPassThrough(PhysicsProperties properties, ref Vector2 position, ref Vector2 velocity, ref float angularVelocity, ref BallPassThroughEvent passThrough);
	}
}
