using System;
using Terraria.GameContent.Drawing;

namespace Terraria.GameContent.Items
{
	// Token: 0x0200046E RID: 1134
	public class WhipTagEffect_Kaleidoscope : WhipTagEffect
	{
		// Token: 0x060032EC RID: 13036 RVA: 0x005F20D8 File Offset: 0x005F02D8
		public override void OnTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, int calcDamage)
		{
			ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.RainbowRodHit, new ParticleOrchestraSettings
			{
				PositionInWorld = optionalProjectile.Center
			}, null);
		}
	}
}
