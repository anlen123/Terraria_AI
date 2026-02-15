using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Drawing;
using Terraria.ID;

namespace Terraria.GameContent.Items
{
	// Token: 0x02000472 RID: 1138
	public class WhipTagEffect_DarkHarvest : WhipTagEffect
	{
		// Token: 0x060032F6 RID: 13046 RVA: 0x005F2327 File Offset: 0x005F0527
		public override void OnTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, int calcDamage)
		{
			this.SpawnBlackLightning(optionalProjectile, npcHit);
		}

		// Token: 0x060032F7 RID: 13047 RVA: 0x005F2334 File Offset: 0x005F0534
		private void SpawnBlackLightning(Projectile projectile, NPC npcHit)
		{
			int damage = (int)((float)this.TagDamage * ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type]);
			int num = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), npcHit.Center, Vector2.Zero, 916, damage, 0f, projectile.owner, 0f, 0f, 0f, null);
			Main.projectile[num].localNPCImmunity[npcHit.whoAmI] = -1;
			WhipTagEffect_DarkHarvest.EmitBlackLightningParticles(npcHit);
		}

		// Token: 0x060032F8 RID: 13048 RVA: 0x005F23AC File Offset: 0x005F05AC
		private static void EmitBlackLightningParticles(NPC targetNPC)
		{
			ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.BlackLightningHit, new ParticleOrchestraSettings
			{
				PositionInWorld = targetNPC.Center
			}, null);
		}
	}
}
