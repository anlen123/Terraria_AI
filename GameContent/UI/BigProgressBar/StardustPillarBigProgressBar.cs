using System;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x02000399 RID: 921
	public class StardustPillarBigProgressBar : LunarPillarBigProgessBar
	{
		// Token: 0x060029F8 RID: 10744 RVA: 0x0057F6A8 File Offset: 0x0057D8A8
		internal override float GetCurrentShieldValue()
		{
			return (float)NPC.ShieldStrengthTowerStardust;
		}

		// Token: 0x060029F9 RID: 10745 RVA: 0x0057F664 File Offset: 0x0057D864
		internal override float GetMaxShieldValue()
		{
			return (float)NPC.ShieldStrengthTowerMax;
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x0057F6B0 File Offset: 0x0057D8B0
		internal override bool IsPlayerInCombatArea()
		{
			return Main.LocalPlayer.ZoneTowerStardust;
		}
	}
}
