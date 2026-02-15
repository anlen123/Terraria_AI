using System;

namespace Terraria.GameContent.Personalities
{
	// Token: 0x02000423 RID: 1059
	public class OceanBiome : AShoppingBiome
	{
		// Token: 0x06003068 RID: 12392 RVA: 0x005B965E File Offset: 0x005B785E
		public OceanBiome()
		{
			base.NameKey = "Ocean";
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x005B9671 File Offset: 0x005B7871
		public override bool IsInBiome(Player player)
		{
			return player.ZoneBeach;
		}
	}
}
