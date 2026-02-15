using System;

namespace Terraria.GameContent.Personalities
{
	// Token: 0x02000424 RID: 1060
	public class ForestBiome : AShoppingBiome
	{
		// Token: 0x0600306A RID: 12394 RVA: 0x005B9679 File Offset: 0x005B7879
		public ForestBiome()
		{
			base.NameKey = "Forest";
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x005B968C File Offset: 0x005B788C
		public override bool IsInBiome(Player player)
		{
			return player.ShoppingZone_Forest;
		}
	}
}
