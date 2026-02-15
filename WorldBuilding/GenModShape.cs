using System;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000AA RID: 170
	public abstract class GenModShape : GenShape
	{
		// Token: 0x06001744 RID: 5956 RVA: 0x004DD3E8 File Offset: 0x004DB5E8
		public GenModShape(ShapeData data)
		{
			this._data = data;
		}

		// Token: 0x040011C6 RID: 4550
		protected ShapeData _data;
	}
}
