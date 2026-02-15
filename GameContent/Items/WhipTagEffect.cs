using System;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.Items
{
	// Token: 0x0200046D RID: 1133
	public class WhipTagEffect : UniqueTagEffect
	{
		// Token: 0x060032E5 RID: 13029 RVA: 0x005F1FCD File Offset: 0x005F01CD
		public WhipTagEffect()
		{
			this.TagDuration = 240;
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x005F1FE0 File Offset: 0x005F01E0
		public override bool CanApplyTagToNPC(int npcType)
		{
			NPCDebuffImmunityData npcdebuffImmunityData;
			return !NPCID.Sets.DebuffImmunitySets.TryGetValue(npcType, out npcdebuffImmunityData) || npcdebuffImmunityData == null || !npcdebuffImmunityData.ImmuneToWhips;
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x005F200C File Offset: 0x005F020C
		public override void OnRemovedFromPlayer(Player player)
		{
			if (player == Main.LocalPlayer)
			{
				player.ClearBuff(this.PlayerBuffId);
			}
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x005F2022 File Offset: 0x005F0222
		public override void OnTagAppliedToNPC(Player player, NPC npc)
		{
			if (player == Main.LocalPlayer)
			{
				this.AddTheBuff(player);
			}
		}

		// Token: 0x060032E9 RID: 13033 RVA: 0x005F2033 File Offset: 0x005F0233
		protected void AddTheBuff(Player player)
		{
			if (this.PlayerBuffAppliedManually)
			{
				return;
			}
			if (this.PlayerBuffId <= 0)
			{
				return;
			}
			player.AddBuff(this.PlayerBuffId, this.PlayerBuffTime, false);
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x005F205B File Offset: 0x005F025B
		public override void ModifyTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, ref int damageDealt, ref bool crit)
		{
			if (optionalProjectile != null)
			{
				damageDealt += (int)((float)(this.TagDamage + optionalProjectile.bonusTagDamage) * ProjectileID.Sets.SummonTagDamageMultiplier[optionalProjectile.type]);
			}
			if (Main.rand.Next(100) < this.CritChance)
			{
				crit = true;
			}
		}

		// Token: 0x060032EB RID: 13035 RVA: 0x005F209B File Offset: 0x005F029B
		public override bool CanRunHitEffects(Player owner, Projectile optionalProjectile, NPC npcHit)
		{
			return optionalProjectile != null && optionalProjectile.OwnedBySomeone && (optionalProjectile.minion || ProjectileID.Sets.MinionShot[optionalProjectile.type] || optionalProjectile.sentry || ProjectileID.Sets.SentryShot[optionalProjectile.type]);
		}

		// Token: 0x0400584B RID: 22603
		public int PlayerBuffId;

		// Token: 0x0400584C RID: 22604
		public int PlayerBuffTime;

		// Token: 0x0400584D RID: 22605
		public bool PlayerBuffAppliedManually;

		// Token: 0x0400584E RID: 22606
		public int CritChance;

		// Token: 0x0400584F RID: 22607
		public int TagDamage;

		// Token: 0x04005850 RID: 22608
		private const int generalWhipMarkDuration = 240;
	}
}
