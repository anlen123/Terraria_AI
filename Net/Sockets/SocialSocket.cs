using System;
using System.Threading;
using Terraria.Social;

namespace Terraria.Net.Sockets
{
	// Token: 0x02000176 RID: 374
	public class SocialSocket : ISocket
	{
		// Token: 0x06001DFD RID: 7677 RVA: 0x0000357B File Offset: 0x0000177B
		public SocialSocket()
		{
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x005020D0 File Offset: 0x005002D0
		public SocialSocket(RemoteAddress remoteAddress)
		{
			this._remoteAddress = remoteAddress;
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x005020DF File Offset: 0x005002DF
		void ISocket.Close()
		{
			if (this._remoteAddress == null)
			{
				return;
			}
			SocialAPI.Network.Close(this._remoteAddress);
			this._remoteAddress = null;
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x00502101 File Offset: 0x00500301
		bool ISocket.IsConnected()
		{
			return SocialAPI.Network.IsConnected(this._remoteAddress);
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x00502113 File Offset: 0x00500313
		void ISocket.Connect(RemoteAddress address)
		{
			this._remoteAddress = address;
			SocialAPI.Network.Connect(address);
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x00502127 File Offset: 0x00500327
		void ISocket.AsyncSend(byte[] data, int offset, int size, SocketSendCallback callback, object state)
		{
			SocialAPI.Network.Send(this._remoteAddress, data, size);
			callback.BeginInvoke(state, null, null);
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x00502148 File Offset: 0x00500348
		private void ReadCallback(byte[] data, int offset, int size, SocketReceiveCallback callback, object state)
		{
			int size2;
			while ((size2 = SocialAPI.Network.Receive(this._remoteAddress, data, offset, size)) == 0)
			{
				Thread.Sleep(1);
			}
			callback(state, size2);
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x0050217E File Offset: 0x0050037E
		void ISocket.AsyncReceive(byte[] data, int offset, int size, SocketReceiveCallback callback, object state)
		{
			new SocialSocket.InternalReadCallback(this.ReadCallback).BeginInvoke(data, offset, size, callback, state, null, null);
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x0050219B File Offset: 0x0050039B
		bool ISocket.IsDataAvailable()
		{
			return SocialAPI.Network.IsDataAvailable(this._remoteAddress);
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x005021AD File Offset: 0x005003AD
		RemoteAddress ISocket.GetRemoteAddress()
		{
			return this._remoteAddress;
		}

		// Token: 0x06001E07 RID: 7687 RVA: 0x005021B5 File Offset: 0x005003B5
		bool ISocket.StartListening(SocketConnectionAccepted callback)
		{
			return SocialAPI.Network.StartListening(callback);
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x005021C2 File Offset: 0x005003C2
		void ISocket.StopListening()
		{
			SocialAPI.Network.StopListening();
		}

		// Token: 0x04001670 RID: 5744
		private RemoteAddress _remoteAddress;

		// Token: 0x0200074E RID: 1870
		// (Invoke) Token: 0x060040D7 RID: 16599
		private delegate void InternalReadCallback(byte[] data, int offset, int size, SocketReceiveCallback callback, object state);
	}
}
