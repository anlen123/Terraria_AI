using System;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x02000397 RID: 919
	public class VortexPillarBigProgressBar : LunarPillarBigProgessBar
	{
		// Token: 0x060029F0 RID: 10736 RVA: 0x0057F680 File Offset: 0x0057D880
		internal override float GetCurrentShieldValue()
		{
			return (float)NPC.ShieldStrengthTowerVortex;
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x0057F664 File Offset: 0x0057D864
		internal override float GetMaxShieldValue()
		{
			return (float)NPC.ShieldStrengthTowerMax;
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x0057F688 File Offset: 0x0057D888
		internal override bool IsPlayerInCombatArea()
		{
			return Main.LocalPlayer.ZoneTowerVortex;
		}
	}
}
