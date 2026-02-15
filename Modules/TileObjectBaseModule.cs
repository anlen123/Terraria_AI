using System;
using Terraria.DataStructures;
using Terraria.Enums;

namespace Terraria.Modules
{
	// Token: 0x02000061 RID: 97
	public class TileObjectBaseModule
	{
		// Token: 0x0600144D RID: 5197 RVA: 0x004BA6B4 File Offset: 0x004B88B4
		public TileObjectBaseModule(TileObjectBaseModule copyFrom = null)
		{
			if (copyFrom == null)
			{
				this.width = 1;
				this.height = 1;
				this.origin = Point16.Zero;
				this.direction = TileObjectDirection.None;
				this.randomRange = 0;
				this.flattenAnchors = false;
				this.specificRandomStyles = null;
				return;
			}
			this.width = copyFrom.width;
			this.height = copyFrom.height;
			this.origin = copyFrom.origin;
			this.direction = copyFrom.direction;
			this.randomRange = copyFrom.randomRange;
			this.flattenAnchors = copyFrom.flattenAnchors;
			this.specificRandomStyles = null;
			if (copyFrom.specificRandomStyles != null)
			{
				this.specificRandomStyles = new int[copyFrom.specificRandomStyles.Length];
				copyFrom.specificRandomStyles.CopyTo(this.specificRandomStyles, 0);
			}
		}

		// Token: 0x0400103B RID: 4155
		public int width;

		// Token: 0x0400103C RID: 4156
		public int height;

		// Token: 0x0400103D RID: 4157
		public Point16 origin;

		// Token: 0x0400103E RID: 4158
		public TileObjectDirection direction;

		// Token: 0x0400103F RID: 4159
		public int randomRange;

		// Token: 0x04001040 RID: 4160
		public bool flattenAnchors;

		// Token: 0x04001041 RID: 4161
		public int[] specificRandomStyles;
	}
}
