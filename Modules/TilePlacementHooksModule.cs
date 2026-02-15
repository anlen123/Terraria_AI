using System;
using Terraria.DataStructures;

namespace Terraria.Modules
{
	// Token: 0x02000066 RID: 102
	public class TilePlacementHooksModule
	{
		// Token: 0x06001452 RID: 5202 RVA: 0x004BA8D0 File Offset: 0x004B8AD0
		public TilePlacementHooksModule(TilePlacementHooksModule copyFrom = null)
		{
			if (copyFrom == null)
			{
				this.check = default(PlacementHook);
				this.postPlaceEveryone = default(PlacementHook);
				this.postPlaceMyPlayer = default(PlacementHook);
				this.placeOverride = default(PlacementHook);
				return;
			}
			this.check = copyFrom.check;
			this.postPlaceEveryone = copyFrom.postPlaceEveryone;
			this.postPlaceMyPlayer = copyFrom.postPlaceMyPlayer;
			this.placeOverride = copyFrom.placeOverride;
		}

		// Token: 0x0400104C RID: 4172
		public PlacementHook check;

		// Token: 0x0400104D RID: 4173
		public PlacementHook postPlaceEveryone;

		// Token: 0x0400104E RID: 4174
		public PlacementHook postPlaceMyPlayer;

		// Token: 0x0400104F RID: 4175
		public PlacementHook placeOverride;
	}
}
