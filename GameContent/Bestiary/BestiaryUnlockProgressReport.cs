using System;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x0200032D RID: 813
	public struct BestiaryUnlockProgressReport
	{
		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x060027E2 RID: 10210 RVA: 0x00568DDC File Offset: 0x00566FDC
		public float CompletionPercent
		{
			get
			{
				if (this.EntriesTotal == 0)
				{
					return 1f;
				}
				return this.CompletionAmountTotal / (float)this.EntriesTotal;
			}
		}

		// Token: 0x040050F0 RID: 20720
		public int EntriesTotal;

		// Token: 0x040050F1 RID: 20721
		public float CompletionAmountTotal;
	}
}
