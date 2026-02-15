using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Items
{
	// Token: 0x02000470 RID: 1136
	public class WhipTagEffect_ViolentDisplayOfFlower : WhipTagEffect
	{
		// Token: 0x060032F0 RID: 13040 RVA: 0x005F211F File Offset: 0x005F031F
		public override void OnProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, int calcDamage)
		{
			this.SpawnFlowerExplosionOn(optionalProjectile, npcHit, 40);
		}

		// Token: 0x060032F1 RID: 13041 RVA: 0x005F212C File Offset: 0x005F032C
		private void SpawnFlowerExplosionOn(Projectile projectile, NPC targetNPC, int petalDamage)
		{
			float num = Main.rand.NextFloat() * 6.2831855f;
			float num2 = 3f;
			int num3 = 0;
			while ((float)num3 < num2)
			{
				float num4 = (float)num3 / num2 * 6.2831855f + num;
				float scaleFactor = (float)((targetNPC.width > targetNPC.height) ? targetNPC.width : targetNPC.height) / 8f;
				Vector2 velocity = Vector2.UnitX.RotatedBy((double)num4, default(Vector2)).RotatedByRandom(0.39269909262657166) * scaleFactor;
				int num5 = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), targetNPC.Center, velocity, 1038, petalDamage, 0f, projectile.owner, Main.rand.NextFloat() * -20f, 0f, 0f, null);
				Main.projectile[num5].localNPCImmunity[targetNPC.whoAmI] = 30;
				num3++;
			}
		}
	}
}
