using System;

namespace Terraria.GameContent.Personalities
{
	// Token: 0x02000425 RID: 1061
	public class SnowBiome : AShoppingBiome
	{
		// Token: 0x0600306C RID: 12396 RVA: 0x005B9694 File Offset: 0x005B7894
		public SnowBiome()
		{
			base.NameKey = "Snow";
		}

		// Token: 0x0600306D RID: 12397 RVA: 0x005B96A7 File Offset: 0x005B78A7
		public override bool IsInBiome(Player player)
		{
			return player.ZoneSnow;
		}
	}
}
