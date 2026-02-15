using System;

namespace Terraria.Physics
{
	// Token: 0x0200007D RID: 125
	public class PhysicsProperties
	{
		// Token: 0x0600155C RID: 5468 RVA: 0x004C3D17 File Offset: 0x004C1F17
		public PhysicsProperties(float gravity, float drag)
		{
			this.Gravity = gravity;
			this.Drag = drag;
		}

		// Token: 0x040010E7 RID: 4327
		public readonly float Gravity;

		// Token: 0x040010E8 RID: 4328
		public readonly float Drag;
	}
}
