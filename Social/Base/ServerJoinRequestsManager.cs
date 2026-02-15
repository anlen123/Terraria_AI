using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Terraria.Social.Base
{
	// Token: 0x02000152 RID: 338
	public class ServerJoinRequestsManager
	{
		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06001D16 RID: 7446 RVA: 0x00500408 File Offset: 0x004FE608
		// (remove) Token: 0x06001D17 RID: 7447 RVA: 0x00500440 File Offset: 0x004FE640
		public event ServerJoinRequestEvent OnRequestAdded;

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06001D18 RID: 7448 RVA: 0x00500478 File Offset: 0x004FE678
		// (remove) Token: 0x06001D19 RID: 7449 RVA: 0x005004B0 File Offset: 0x004FE6B0
		public event ServerJoinRequestEvent OnRequestRemoved;

		// Token: 0x06001D1A RID: 7450 RVA: 0x005004E5 File Offset: 0x004FE6E5
		public ServerJoinRequestsManager()
		{
			this._requests = new List<UserJoinToServerRequest>();
			this.CurrentRequests = new ReadOnlyCollection<UserJoinToServerRequest>(this._requests);
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x0050050C File Offset: 0x004FE70C
		public void Update()
		{
			for (int i = this._requests.Count - 1; i >= 0; i--)
			{
				if (!this._requests[i].IsValid())
				{
					this.RemoveRequestAtIndex(i);
				}
			}
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x0050054C File Offset: 0x004FE74C
		public void Add(UserJoinToServerRequest request)
		{
			for (int i = this._requests.Count - 1; i >= 0; i--)
			{
				if (this._requests[i].Equals(request))
				{
					this.RemoveRequestAtIndex(i);
				}
			}
			this._requests.Add(request);
			request.OnAccepted += delegate()
			{
				this.RemoveRequest(request);
			};
			request.OnRejected += delegate()
			{
				this.RemoveRequest(request);
			};
			if (this.OnRequestAdded != null)
			{
				this.OnRequestAdded(request);
			}
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x00500600 File Offset: 0x004FE800
		private void RemoveRequestAtIndex(int i)
		{
			UserJoinToServerRequest request = this._requests[i];
			this._requests.RemoveAt(i);
			if (this.OnRequestRemoved != null)
			{
				this.OnRequestRemoved(request);
			}
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x0050063A File Offset: 0x004FE83A
		private void RemoveRequest(UserJoinToServerRequest request)
		{
			if (this._requests.Remove(request) && this.OnRequestRemoved != null)
			{
				this.OnRequestRemoved(request);
			}
		}

		// Token: 0x04001610 RID: 5648
		private readonly List<UserJoinToServerRequest> _requests;

		// Token: 0x04001611 RID: 5649
		public readonly ReadOnlyCollection<UserJoinToServerRequest> CurrentRequests;
	}
}
