using System;

namespace Terraria.GameContent.Personalities
{
	// Token: 0x02000426 RID: 1062
	public class DesertBiome : AShoppingBiome
	{
		// Token: 0x0600306E RID: 12398 RVA: 0x005B96AF File Offset: 0x005B78AF
		public DesertBiome()
		{
			base.NameKey = "Desert";
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x005B96C2 File Offset: 0x005B78C2
		public override bool IsInBiome(Player player)
		{
			return player.ZoneDesert;
		}
	}
}
