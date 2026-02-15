using System;

namespace Terraria.Net.Sockets
{
	// Token: 0x02000175 RID: 373
	public interface ISocket
	{
		// Token: 0x06001DF4 RID: 7668
		void Close();

		// Token: 0x06001DF5 RID: 7669
		bool IsConnected();

		// Token: 0x06001DF6 RID: 7670
		void Connect(RemoteAddress address);

		// Token: 0x06001DF7 RID: 7671
		void AsyncSend(byte[] data, int offset, int size, SocketSendCallback callback, object state = null);

		// Token: 0x06001DF8 RID: 7672
		void AsyncReceive(byte[] data, int offset, int size, SocketReceiveCallback callback, object state = null);

		// Token: 0x06001DF9 RID: 7673
		bool IsDataAvailable();

		// Token: 0x06001DFA RID: 7674
		bool StartListening(SocketConnectionAccepted callback);

		// Token: 0x06001DFB RID: 7675
		void StopListening();

		// Token: 0x06001DFC RID: 7676
		RemoteAddress GetRemoteAddress();
	}
}
