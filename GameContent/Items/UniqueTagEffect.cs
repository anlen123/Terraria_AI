using System;

namespace Terraria.GameContent.Items
{
	// Token: 0x0200046C RID: 1132
	public abstract class UniqueTagEffect
	{
		// Token: 0x060032DB RID: 13019 RVA: 0x000379F1 File Offset: 0x00035BF1
		public virtual bool CanApplyTagToNPC(int npcType)
		{
			return true;
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnRemovedFromPlayer(Player owner)
		{
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnSetToPlayer(Player owner)
		{
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnTagAppliedToNPC(Player owner, NPC npc)
		{
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x000379F1 File Offset: 0x00035BF1
		public virtual bool CanRunHitEffects(Player owner, Projectile optionalProjectile, NPC npcHit)
		{
			return true;
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void ModifyTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, ref int damageDealt, ref bool crit)
		{
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void ModifyProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, ref int damageDealt, ref bool crit)
		{
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, int calcDamage)
		{
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, int calcDamage)
		{
		}

		// Token: 0x04005848 RID: 22600
		public bool NetSync;

		// Token: 0x04005849 RID: 22601
		public bool SyncProcs;

		// Token: 0x0400584A RID: 22602
		public int TagDuration;
	}
}
