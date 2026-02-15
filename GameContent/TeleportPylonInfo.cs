using System;
using Terraria.DataStructures;

namespace Terraria.GameContent
{
	// Token: 0x02000269 RID: 617
	public struct TeleportPylonInfo : IEquatable<TeleportPylonInfo>
	{
		// Token: 0x060023ED RID: 9197 RVA: 0x00548DD3 File Offset: 0x00546FD3
		public bool Equals(TeleportPylonInfo other)
		{
			return this.PositionInTiles == other.PositionInTiles && this.TypeOfPylon == other.TypeOfPylon;
		}

		// Token: 0x04004D93 RID: 19859
		public Point16 PositionInTiles;

		// Token: 0x04004D94 RID: 19860
		public TeleportPylonType TypeOfPylon;
	}
}
