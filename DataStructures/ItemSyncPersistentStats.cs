using System;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	// Token: 0x02000550 RID: 1360
	public struct ItemSyncPersistentStats
	{
		// Token: 0x06003772 RID: 14194 RVA: 0x0062E50E File Offset: 0x0062C70E
		public void CopyFrom(WorldItem item)
		{
			this.type = item.type;
			this.color = item.color;
		}

		// Token: 0x06003773 RID: 14195 RVA: 0x0062E528 File Offset: 0x0062C728
		public void PasteInto(WorldItem item)
		{
			if (this.type != item.type)
			{
				return;
			}
			item.color = this.color;
		}

		// Token: 0x04005B8A RID: 23434
		private Color color;

		// Token: 0x04005B8B RID: 23435
		private int type;
	}
}
