using System;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x02000396 RID: 918
	public class SolarFlarePillarBigProgressBar : LunarPillarBigProgessBar
	{
		// Token: 0x060029EC RID: 10732 RVA: 0x0057F65C File Offset: 0x0057D85C
		internal override float GetCurrentShieldValue()
		{
			return (float)NPC.ShieldStrengthTowerSolar;
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x0057F664 File Offset: 0x0057D864
		internal override float GetMaxShieldValue()
		{
			return (float)NPC.ShieldStrengthTowerMax;
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x0057F66C File Offset: 0x0057D86C
		internal override bool IsPlayerInCombatArea()
		{
			return Main.LocalPlayer.ZoneTowerSolar;
		}
	}
}
