using System;

namespace Terraria.GameContent.Personalities
{
	// Token: 0x0200042B RID: 1067
	public class DungeonBiome : AShoppingBiome
	{
		// Token: 0x06003078 RID: 12408 RVA: 0x005B9736 File Offset: 0x005B7936
		public DungeonBiome()
		{
			base.NameKey = "Dungeon";
		}

		// Token: 0x06003079 RID: 12409 RVA: 0x005B9749 File Offset: 0x005B7949
		public override bool IsInBiome(Player player)
		{
			return player.ZoneDungeon;
		}
	}
}
