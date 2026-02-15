using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000557 RID: 1367
	public struct NPCKillAttempt
	{
		// Token: 0x0600378D RID: 14221 RVA: 0x0062E9D7 File Offset: 0x0062CBD7
		public NPCKillAttempt(NPC target)
		{
			this.npc = target;
			this.netId = target.netID;
			this.active = target.active;
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x0062E9F8 File Offset: 0x0062CBF8
		public bool DidNPCDie()
		{
			return !this.npc.active;
		}

		// Token: 0x0600378F RID: 14223 RVA: 0x0062EA08 File Offset: 0x0062CC08
		public bool DidNPCDieOrTransform()
		{
			return this.DidNPCDie() || this.npc.netID != this.netId;
		}

		// Token: 0x04005B9A RID: 23450
		public readonly NPC npc;

		// Token: 0x04005B9B RID: 23451
		public readonly int netId;

		// Token: 0x04005B9C RID: 23452
		public readonly bool active;
	}
}
