using System;

namespace Terraria.GameContent.Personalities
{
	// Token: 0x02000427 RID: 1063
	public class JungleBiome : AShoppingBiome
	{
		// Token: 0x06003070 RID: 12400 RVA: 0x005B96CA File Offset: 0x005B78CA
		public JungleBiome()
		{
			base.NameKey = "Jungle";
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x005B96DD File Offset: 0x005B78DD
		public override bool IsInBiome(Player player)
		{
			return player.ZoneJungle;
		}
	}
}
