using System;

namespace Terraria.Social.Base
{
	// Token: 0x02000153 RID: 339
	public abstract class UserJoinToServerRequest
	{
		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06001D1F RID: 7455 RVA: 0x0050065E File Offset: 0x004FE85E
		// (set) Token: 0x06001D20 RID: 7456 RVA: 0x00500666 File Offset: 0x004FE866
		internal string UserDisplayName { get; private set; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06001D21 RID: 7457 RVA: 0x0050066F File Offset: 0x004FE86F
		// (set) Token: 0x06001D22 RID: 7458 RVA: 0x00500677 File Offset: 0x004FE877
		internal string UserFullIdentifier { get; private set; }

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06001D23 RID: 7459 RVA: 0x00500680 File Offset: 0x004FE880
		// (remove) Token: 0x06001D24 RID: 7460 RVA: 0x005006B8 File Offset: 0x004FE8B8
		public event Action OnAccepted;

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06001D25 RID: 7461 RVA: 0x005006F0 File Offset: 0x004FE8F0
		// (remove) Token: 0x06001D26 RID: 7462 RVA: 0x00500728 File Offset: 0x004FE928
		public event Action OnRejected;

		// Token: 0x06001D27 RID: 7463 RVA: 0x0050075D File Offset: 0x004FE95D
		public UserJoinToServerRequest(string userDisplayName, string fullIdentifier)
		{
			this.UserDisplayName = userDisplayName;
			this.UserFullIdentifier = fullIdentifier;
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x00500773 File Offset: 0x004FE973
		public void Accept()
		{
			if (this.OnAccepted != null)
			{
				this.OnAccepted();
			}
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x00500788 File Offset: 0x004FE988
		public void Reject()
		{
			if (this.OnRejected != null)
			{
				this.OnRejected();
			}
		}

		// Token: 0x06001D2A RID: 7466
		public abstract bool IsValid();

		// Token: 0x06001D2B RID: 7467
		public abstract string GetUserWrapperText();
	}
}
