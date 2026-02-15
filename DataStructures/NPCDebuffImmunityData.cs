using System;
using Terraria.ID;

namespace Terraria.DataStructures
{
	// Token: 0x02000555 RID: 1365
	public class NPCDebuffImmunityData
	{
		// Token: 0x0600377E RID: 14206 RVA: 0x0062E708 File Offset: 0x0062C908
		public void ApplyToNPC(NPC npc)
		{
			if (this.ImmuneToWhips || this.ImmuneToAllBuffsThatAreNotWhips)
			{
				for (int i = 1; i < BuffID.Count; i++)
				{
					bool flag = BuffID.Sets.IsAnNPCWhipDebuff[i];
					bool flag2 = false;
					flag2 |= (flag && this.ImmuneToWhips);
					flag2 |= (!flag && this.ImmuneToAllBuffsThatAreNotWhips);
					npc.buffImmune[i] = flag2;
				}
			}
			if (this.SpecificallyImmuneTo != null)
			{
				for (int j = 0; j < this.SpecificallyImmuneTo.Length; j++)
				{
					int num = this.SpecificallyImmuneTo[j];
					npc.buffImmune[num] = true;
				}
			}
		}

		// Token: 0x04005B94 RID: 23444
		public bool ImmuneToWhips;

		// Token: 0x04005B95 RID: 23445
		public bool ImmuneToAllBuffsThatAreNotWhips;

		// Token: 0x04005B96 RID: 23446
		public int[] SpecificallyImmuneTo;
	}
}
