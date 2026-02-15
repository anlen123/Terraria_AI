using System;

namespace Terraria.GameContent.UI.ResourceSets
{
	// Token: 0x020003BE RID: 958
	public struct PlayerStatsSnapshot
	{
		// Token: 0x06002D08 RID: 11528 RVA: 0x005A14A0 File Offset: 0x0059F6A0
		public PlayerStatsSnapshot(Player player)
		{
			this.Life = player.statLife;
			this.Mana = player.statMana;
			this.LifeMax = player.statLifeMax2;
			this.ManaMax = player.statManaMax2;
			float num = 20f;
			int num2 = player.statLifeMax / 20;
			int num3 = (player.statLifeMax - 400) / 5;
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num3 > 0)
			{
				num2 = player.statLifeMax / (20 + num3 / 4);
				num = (float)player.statLifeMax / 20f;
			}
			int num4 = player.statLifeMax2 - player.statLifeMax;
			if (num2 > 0)
			{
				num += (float)(num4 / num2);
			}
			this.LifeFruitCount = num3;
			this.LifePerSegment = num;
			this.ManaPerSegment = 20f;
		}

		// Token: 0x04005462 RID: 21602
		public int Life;

		// Token: 0x04005463 RID: 21603
		public int LifeMax;

		// Token: 0x04005464 RID: 21604
		public int LifeFruitCount;

		// Token: 0x04005465 RID: 21605
		public float LifePerSegment;

		// Token: 0x04005466 RID: 21606
		public int Mana;

		// Token: 0x04005467 RID: 21607
		public int ManaMax;

		// Token: 0x04005468 RID: 21608
		public float ManaPerSegment;
	}
}
