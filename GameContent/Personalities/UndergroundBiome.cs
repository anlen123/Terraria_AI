using System;

namespace Terraria.GameContent.Personalities
{
	// Token: 0x02000428 RID: 1064
	public class UndergroundBiome : AShoppingBiome
	{
		// Token: 0x06003072 RID: 12402 RVA: 0x005B96E5 File Offset: 0x005B78E5
		public UndergroundBiome()
		{
			base.NameKey = "NormalUnderground";
		}

		// Token: 0x06003073 RID: 12403 RVA: 0x005B96F8 File Offset: 0x005B78F8
		public override bool IsInBiome(Player player)
		{
			return player.ShoppingZone_BelowSurface;
		}
	}
}
