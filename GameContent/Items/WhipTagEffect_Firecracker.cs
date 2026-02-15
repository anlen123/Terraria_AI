using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Items
{
	// Token: 0x02000473 RID: 1139
	public class WhipTagEffect_Firecracker : WhipTagEffect
	{
		// Token: 0x060032FA RID: 13050 RVA: 0x005F23DF File Offset: 0x005F05DF
		public override void ModifyProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, ref int damageDealt, ref bool crit)
		{
			base.ModifyProcHit(owner, optionalProjectile, npcHit, ref damageDealt, ref crit);
			damageDealt += (int)((float)damageDealt * WhipTagEffect_Firecracker.ProcDamageMultiplier);
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x005F2400 File Offset: 0x005F0600
		public override void OnProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, int calcDamage)
		{
			WhipTagEffect_Firecracker.CreateExplosion(optionalProjectile, npcHit, (int)((float)calcDamage * WhipTagEffect_Firecracker.ProcDamageMultiplier));
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x005F2414 File Offset: 0x005F0614
		private static void CreateExplosion(Projectile projectile, NPC npcHit, int procDamage)
		{
			int num = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), npcHit.Center, Vector2.Zero, 918, procDamage, 0f, projectile.owner, 0f, 0f, 0f, null);
			Main.projectile[num].localNPCImmunity[npcHit.whoAmI] = -1;
		}

		// Token: 0x04005851 RID: 22609
		private static float ProcDamageMultiplier = 1.75f;
	}
}
