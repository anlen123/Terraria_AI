using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent
{
	// Token: 0x02000241 RID: 577
	public struct PlayerPettingInfo
	{
		// Token: 0x06002298 RID: 8856 RVA: 0x00538A5E File Offset: 0x00536C5E
		public PlayerPettingInfo(NPC npc, Vector2 offsetFromPet, bool isPetSmall)
		{
			this.isPetting = false;
			this.npc = npc.whoAmI;
			this.proj = -1;
			this.type = npc.type;
			this.offsetFromPet = offsetFromPet;
			this.isPetSmall = isPetSmall;
			this.mount = false;
		}

		// Token: 0x06002299 RID: 8857 RVA: 0x00538A9B File Offset: 0x00536C9B
		public PlayerPettingInfo(Projectile proj, Vector2 offsetFromPet, bool isPetSmall)
		{
			this.isPetting = false;
			this.npc = -1;
			this.proj = proj.whoAmI;
			this.type = proj.type;
			this.offsetFromPet = offsetFromPet;
			this.isPetSmall = isPetSmall;
			this.mount = false;
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x00538AD8 File Offset: 0x00536CD8
		public PlayerPettingInfo(int mountId, bool isPetSmall)
		{
			this.isPetting = false;
			this.npc = -1;
			this.proj = -1;
			this.type = mountId;
			this.offsetFromPet = Vector2.Zero;
			this.isPetSmall = isPetSmall;
			this.mount = true;
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x00538B10 File Offset: 0x00536D10
		public bool TryGetTarget(out Entity target)
		{
			if (this.npc >= 0)
			{
				NPC npc = Main.npc[this.npc];
				target = npc;
				return npc.active && npc.type == this.type;
			}
			if (this.mount)
			{
				target = null;
				return true;
			}
			Projectile projectile = Main.projectile[this.proj];
			target = projectile;
			return projectile.active && projectile.type == this.type;
		}

		// Token: 0x04004CEA RID: 19690
		public bool isPetting;

		// Token: 0x04004CEB RID: 19691
		public int npc;

		// Token: 0x04004CEC RID: 19692
		public int proj;

		// Token: 0x04004CED RID: 19693
		public int type;

		// Token: 0x04004CEE RID: 19694
		public bool mount;

		// Token: 0x04004CEF RID: 19695
		public Vector2 offsetFromPet;

		// Token: 0x04004CF0 RID: 19696
		public bool isPetSmall;
	}
}
