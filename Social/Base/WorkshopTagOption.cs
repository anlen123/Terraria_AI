using System;

namespace Terraria.Social.Base
{
	// Token: 0x0200015E RID: 350
	public class WorkshopTagOption
	{
		// Token: 0x06001D60 RID: 7520 RVA: 0x00500CB1 File Offset: 0x004FEEB1
		public WorkshopTagOption(string nameKey, string internalName)
		{
			this.NameKey = nameKey;
			this.InternalNameForAPIs = internalName;
		}

		// Token: 0x04001638 RID: 5688
		public readonly string NameKey;

		// Token: 0x04001639 RID: 5689
		public readonly string InternalNameForAPIs;
	}
}
