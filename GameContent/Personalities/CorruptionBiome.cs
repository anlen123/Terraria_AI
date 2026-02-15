using System;

namespace Terraria.GameContent.Personalities
{
	// Token: 0x0200042C RID: 1068
	public class CorruptionBiome : AShoppingBiome
	{
		// Token: 0x0600307A RID: 12410 RVA: 0x005B9751 File Offset: 0x005B7951
		public CorruptionBiome()
		{
			base.NameKey = "Corruption";
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x005B9764 File Offset: 0x005B7964
		public override bool IsInBiome(Player player)
		{
			return player.ZoneCorrupt;
		}
	}
}
