using System;
using System.Threading;

namespace Terraria.Social.WeGame
{
	// Token: 0x02000131 RID: 305
	public class IPCContent
	{
		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06001C15 RID: 7189 RVA: 0x004FC875 File Offset: 0x004FAA75
		// (set) Token: 0x06001C16 RID: 7190 RVA: 0x004FC87D File Offset: 0x004FAA7D
		public CancellationToken CancelToken { get; set; }

		// Token: 0x04001598 RID: 5528
		public byte[] data;
	}
}
