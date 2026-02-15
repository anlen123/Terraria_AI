using System;

namespace Terraria.Net
{
	// Token: 0x02000168 RID: 360
	public abstract class RemoteAddress
	{
		// Token: 0x06001DA5 RID: 7589
		public abstract string GetIdentifier();

		// Token: 0x06001DA6 RID: 7590
		public abstract string GetFriendlyName();

		// Token: 0x06001DA7 RID: 7591
		public abstract bool IsLocalHost();

		// Token: 0x04001647 RID: 5703
		public AddressType Type;
	}
}
