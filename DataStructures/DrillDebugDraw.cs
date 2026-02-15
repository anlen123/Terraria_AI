using System;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	// Token: 0x02000594 RID: 1428
	public struct DrillDebugDraw
	{
		// Token: 0x0600384C RID: 14412 RVA: 0x006312CE File Offset: 0x0062F4CE
		public DrillDebugDraw(Vector2 p, Color c)
		{
			this.point = p;
			this.color = c;
		}

		// Token: 0x04005C5B RID: 23643
		public Vector2 point;

		// Token: 0x04005C5C RID: 23644
		public Color color;
	}
}
