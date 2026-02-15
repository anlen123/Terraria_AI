using System;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	// Token: 0x02000595 RID: 1429
	public struct LineSegment
	{
		// Token: 0x0600384D RID: 14413 RVA: 0x006312DE File Offset: 0x0062F4DE
		public LineSegment(Vector2 start, Vector2 end)
		{
			this.Start = start;
			this.End = end;
		}

		// Token: 0x04005C5D RID: 23645
		public Vector2 Start;

		// Token: 0x04005C5E RID: 23646
		public Vector2 End;
	}
}
