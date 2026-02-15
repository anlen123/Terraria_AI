using System;

namespace Terraria.GameContent.Personalities
{
	// Token: 0x0200042D RID: 1069
	public class CrimsonBiome : AShoppingBiome
	{
		// Token: 0x0600307C RID: 12412 RVA: 0x005B976C File Offset: 0x005B796C
		public CrimsonBiome()
		{
			base.NameKey = "Crimson";
		}

		// Token: 0x0600307D RID: 12413 RVA: 0x005B977F File Offset: 0x005B797F
		public override bool IsInBiome(Player player)
		{
			return player.ZoneCrimson;
		}
	}
}
