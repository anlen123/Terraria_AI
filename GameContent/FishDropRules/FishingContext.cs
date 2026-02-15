using System;
using Terraria.DataStructures;
using Terraria.Utilities;

namespace Terraria.GameContent.FishDropRules
{
	// Token: 0x0200047C RID: 1148
	public class FishingContext
	{
		// Token: 0x0400588C RID: 22668
		public UnifiedRandom Random = new UnifiedRandom();

		// Token: 0x0400588D RID: 22669
		public FishingAttempt Fisher;

		// Token: 0x0400588E RID: 22670
		public Player Player;

		// Token: 0x0400588F RID: 22671
		public bool RolledCorruption;

		// Token: 0x04005890 RID: 22672
		public bool RolledCrimson;

		// Token: 0x04005891 RID: 22673
		public bool RolledJungle;

		// Token: 0x04005892 RID: 22674
		public bool RolledSnow;

		// Token: 0x04005893 RID: 22675
		public bool RolledDesert;

		// Token: 0x04005894 RID: 22676
		public bool RolledInfectedDesert;

		// Token: 0x04005895 RID: 22677
		public bool RolledRemixOcean;
	}
}
