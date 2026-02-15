using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Items
{
	// Token: 0x02000471 RID: 1137
	public class WhipTagEffect_Starcrash : WhipTagEffect
	{
		// Token: 0x060032F3 RID: 13043 RVA: 0x005F221A File Offset: 0x005F041A
		public override void OnProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, int calcDamage)
		{
			this.SpawnMeteorWhipMeteorOn(optionalProjectile, npcHit, calcDamage);
		}

		// Token: 0x060032F4 RID: 13044 RVA: 0x005F2228 File Offset: 0x005F0428
		private void SpawnMeteorWhipMeteorOn(Projectile projectile, NPC targetNPC, int calcDamage)
		{
			int num = 200;
			int num2 = 600;
			int damage = (int)((float)calcDamage * 1.33f);
			Vector2 vector = new Vector2((float)(-(float)num + Main.rand.Next(num * 2)), (float)(-(float)num2));
			Vector2 vector2 = targetNPC.Center + vector;
			Vector2 velocity = vector.SafeNormalize(Vector2.Zero) * -12f;
			int num3 = 8;
			int num4 = 35;
			vector2 = targetNPC.Center + new Vector2(0f, (float)(-(float)num3 * num4)).RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.125f), default(Vector2));
			velocity = targetNPC.DirectionFrom(vector2) * (float)num3;
			Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), vector2, velocity, 1037, damage, projectile.knockBack, projectile.owner, (float)Main.rand.Next(3), targetNPC.position.Y, 0f, null);
		}
	}
}
