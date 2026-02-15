using System;

namespace Terraria.GameContent.Items
{
	// Token: 0x0200046F RID: 1135
	public class WhipTagEffect_Possession : WhipTagEffect
	{
		// Token: 0x060032EE RID: 13038 RVA: 0x005F2113 File Offset: 0x005F0313
		public override void OnProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, int calcDamage)
		{
			Projectile.SpawnMoonLordWhipProc(optionalProjectile, npcHit, 20, 0);
		}
	}
}
