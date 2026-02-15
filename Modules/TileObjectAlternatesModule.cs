using System;
using System.Collections.Generic;
using Terraria.ObjectData;

namespace Terraria.Modules
{
	// Token: 0x02000060 RID: 96
	public class TileObjectAlternatesModule
	{
		// Token: 0x0600144C RID: 5196 RVA: 0x004BA63C File Offset: 0x004B883C
		public TileObjectAlternatesModule(TileObjectAlternatesModule copyFrom = null)
		{
			if (copyFrom == null)
			{
				this.data = null;
				return;
			}
			if (copyFrom.data == null)
			{
				this.data = null;
				return;
			}
			this.data = new List<TileObjectData>(copyFrom.data.Count);
			for (int i = 0; i < copyFrom.data.Count; i++)
			{
				this.data.Add(new TileObjectData(copyFrom.data[i]));
			}
		}

		// Token: 0x0400103A RID: 4154
		public List<TileObjectData> data;
	}
}
