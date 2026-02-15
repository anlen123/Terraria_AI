using System;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x02000398 RID: 920
	public class NebulaPillarBigProgressBar : LunarPillarBigProgessBar
	{
		// Token: 0x060029F4 RID: 10740 RVA: 0x0057F694 File Offset: 0x0057D894
		internal override float GetCurrentShieldValue()
		{
			return (float)NPC.ShieldStrengthTowerNebula;
		}

		// Token: 0x060029F5 RID: 10741 RVA: 0x0057F664 File Offset: 0x0057D864
		internal override float GetMaxShieldValue()
		{
			return (float)NPC.ShieldStrengthTowerMax;
		}

		// Token: 0x060029F6 RID: 10742 RVA: 0x0057F69C File Offset: 0x0057D89C
		internal override bool IsPlayerInCombatArea()
		{
			return Main.LocalPlayer.ZoneTowerNebula;
		}
	}
}
