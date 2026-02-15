using System;
using System.Diagnostics;
using Terraria.Net;
using Terraria.Net.Sockets;

namespace Terraria.Social.Base
{
	// Token: 0x02000163 RID: 355
	public abstract class NetSocialModule : ISocialModule
	{
		// Token: 0x06001D7F RID: 7551
		public abstract void Initialize();

		// Token: 0x06001D80 RID: 7552
		public abstract void Shutdown();

		// Token: 0x06001D81 RID: 7553
		public abstract void Close(RemoteAddress address);

		// Token: 0x06001D82 RID: 7554
		public abstract bool IsConnected(RemoteAddress address);

		// Token: 0x06001D83 RID: 7555
		public abstract void Connect(RemoteAddress address);

		// Token: 0x06001D84 RID: 7556
		public abstract bool Send(RemoteAddress address, byte[] data, int length);

		// Token: 0x06001D85 RID: 7557
		public abstract int Receive(RemoteAddress address, byte[] data, int offset, int length);

		// Token: 0x06001D86 RID: 7558
		public abstract bool IsDataAvailable(RemoteAddress address);

		// Token: 0x06001D87 RID: 7559
		public abstract void LaunchLocalServer(Process process, ServerMode mode);

		// Token: 0x06001D88 RID: 7560
		public abstract bool CanInvite();

		// Token: 0x06001D89 RID: 7561
		public abstract void OpenInviteInterface();

		// Token: 0x06001D8A RID: 7562
		public abstract void CancelJoin();

		// Token: 0x06001D8B RID: 7563
		public abstract bool StartListening(SocketConnectionAccepted callback);

		// Token: 0x06001D8C RID: 7564
		public abstract void StopListening();

		// Token: 0x06001D8D RID: 7565
		public abstract ulong GetLobbyId();
	}
}
