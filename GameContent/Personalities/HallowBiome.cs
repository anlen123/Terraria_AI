using System;

namespace Terraria.GameContent.Personalities
{
	// Token: 0x02000429 RID: 1065
	public class HallowBiome : AShoppingBiome
	{
		// Token: 0x06003074 RID: 12404 RVA: 0x005B9700 File Offset: 0x005B7900
		public HallowBiome()
		{
			base.NameKey = "Hallow";
		}

		// Token: 0x06003075 RID: 12405 RVA: 0x005B9713 File Offset: 0x005B7913
		public override bool IsInBiome(Player player)
		{
			return player.ZoneHallow;
		}
	}
}
