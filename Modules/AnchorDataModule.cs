using System;
using Terraria.DataStructures;

namespace Terraria.Modules
{
	// Token: 0x0200005E RID: 94
	public class AnchorDataModule
	{
		// Token: 0x0600144A RID: 5194 RVA: 0x004BA488 File Offset: 0x004B8688
		public AnchorDataModule(AnchorDataModule copyFrom = null)
		{
			if (copyFrom == null)
			{
				this.top = default(AnchorData);
				this.bottom = default(AnchorData);
				this.left = default(AnchorData);
				this.right = default(AnchorData);
				this.wall = false;
				return;
			}
			this.top = copyFrom.top;
			this.bottom = copyFrom.bottom;
			this.left = copyFrom.left;
			this.right = copyFrom.right;
			this.wall = copyFrom.wall;
		}

		// Token: 0x04001031 RID: 4145
		public AnchorData top;

		// Token: 0x04001032 RID: 4146
		public AnchorData bottom;

		// Token: 0x04001033 RID: 4147
		public AnchorData left;

		// Token: 0x04001034 RID: 4148
		public AnchorData right;

		// Token: 0x04001035 RID: 4149
		public bool wall;
	}
}
