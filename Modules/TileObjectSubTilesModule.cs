using System;
using System.Collections.Generic;
using Terraria.ObjectData;

namespace Terraria.Modules
{
	// Token: 0x02000065 RID: 101
	public class TileObjectSubTilesModule
	{
		// Token: 0x06001451 RID: 5201 RVA: 0x004BA858 File Offset: 0x004B8A58
		public TileObjectSubTilesModule(TileObjectSubTilesModule copyFrom = null, List<TileObjectData> newData = null)
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
			for (int i = 0; i < this.data.Count; i++)
			{
				this.data.Add(new TileObjectData(copyFrom.data[i]));
			}
		}

		// Token: 0x0400104B RID: 4171
		public List<TileObjectData> data;
	}
}
