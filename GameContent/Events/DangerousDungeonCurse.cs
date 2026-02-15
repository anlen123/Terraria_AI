using System;
using Terraria.GameContent.Generation.Dungeon;

namespace Terraria.GameContent.Events
{
	// Token: 0x020004F5 RID: 1269
	public class DangerousDungeonCurse
	{
		// Token: 0x0600353A RID: 13626 RVA: 0x0061708C File Offset: 0x0061528C
		public static int GetProgressPlayerNeedsToMatch(Player player)
		{
			if (player.ZoneLihzhardTemple)
			{
				return DualDungeonUnbreakableWallTiers.Temple;
			}
			if (player.ZoneHallow)
			{
				return DualDungeonUnbreakableWallTiers.Hallow;
			}
			if (player.ZoneDungeon)
			{
				return DualDungeonUnbreakableWallTiers.Dungeon;
			}
			if (player.ZoneJungle)
			{
				return DualDungeonUnbreakableWallTiers.JungleBoss;
			}
			if (player.ZoneCrimson || player.ZoneCorrupt)
			{
				return DualDungeonUnbreakableWallTiers.EvilBoss;
			}
			return DualDungeonUnbreakableWallTiers.EarlyGame;
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x006170EC File Offset: 0x006152EC
		public static int GetProgressPlayerCanSafelyMatch()
		{
			if (NPC.downedMechBossAny || NPC.downedQueenSlime)
			{
				return DualDungeonUnbreakableWallTiers.Temple;
			}
			if (NPC.downedBoss3 || Main.hardMode)
			{
				return DualDungeonUnbreakableWallTiers.Hallow;
			}
			if (NPC.downedQueenBee)
			{
				return DualDungeonUnbreakableWallTiers.Dungeon;
			}
			if (NPC.downedBoss2)
			{
				return DualDungeonUnbreakableWallTiers.JungleBoss;
			}
			if (NPC.downedSlimeKing || NPC.downedBoss1)
			{
				return DualDungeonUnbreakableWallTiers.EvilBoss;
			}
			return DualDungeonUnbreakableWallTiers.EarlyGame;
		}
	}
}
