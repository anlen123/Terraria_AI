using System;

namespace Terraria.GameContent.Personalities
{
	// Token: 0x0200042A RID: 1066
	public class MushroomBiome : AShoppingBiome
	{
		// Token: 0x06003076 RID: 12406 RVA: 0x005B971B File Offset: 0x005B791B
		public MushroomBiome()
		{
			base.NameKey = "Mushroom";
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x005B972E File Offset: 0x005B792E
		public override bool IsInBiome(Player player)
		{
			return player.ZoneGlowshroom;
		}
	}
}
