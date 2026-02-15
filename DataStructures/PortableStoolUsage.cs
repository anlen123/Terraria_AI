using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000563 RID: 1379
	public struct PortableStoolUsage
	{
		// Token: 0x060037CE RID: 14286 RVA: 0x0062F429 File Offset: 0x0062D629
		public void Reset()
		{
			this.HasAStool = false;
			this.IsInUse = false;
			this.HeightBoost = 0;
			this.VisualYOffset = 0;
			this.MapYOffset = 0;
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x0062F44E File Offset: 0x0062D64E
		public void SetStats(int heightBoost, int visualYOffset, int mapYOffset)
		{
			this.HasAStool = true;
			this.HeightBoost = heightBoost;
			this.VisualYOffset = visualYOffset;
			this.MapYOffset = mapYOffset;
		}

		// Token: 0x04005BE8 RID: 23528
		public bool HasAStool;

		// Token: 0x04005BE9 RID: 23529
		public bool IsInUse;

		// Token: 0x04005BEA RID: 23530
		public int HeightBoost;

		// Token: 0x04005BEB RID: 23531
		public int VisualYOffset;

		// Token: 0x04005BEC RID: 23532
		public int MapYOffset;
	}
}
