using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ReLogic.OS;
using Terraria.Localization;

namespace Terraria.Net.Sockets
{
	// Token: 0x02000177 RID: 375
	public class TcpSocket : ISocket
	{
		// Token: 0x06001E09 RID: 7689 RVA: 0x005021D0 File Offset: 0x005003D0
		private DebugNetworkStream GetStream()
		{
			if (this._debugStream == null)
			{
				return this._debugStream = new DebugNetworkStream(this._connection.GetStream());
			}
			return this._debugStream;
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x00502205 File Offset: 0x00500405
		public TcpSocket()
		{
			this._connection = new TcpClient
			{
				NoDelay = true
			};
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x00502220 File Offset: 0x00500420
		public TcpSocket(TcpClient tcpClient)
		{
			this._connection = tcpClient;
			this._connection.NoDelay = true;
			IPEndPoint ipendPoint = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
			this._remoteAddress = new TcpAddress(ipendPoint.Address, ipendPoint.Port);
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x0050226E File Offset: 0x0050046E
		void ISocket.Close()
		{
			this._remoteAddress = null;
			this._connection.Close();
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x00502282 File Offset: 0x00500482
		bool ISocket.IsConnected()
		{
			return this._connection != null && this._connection.Client != null && this._connection.Connected;
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x005022A8 File Offset: 0x005004A8
		void ISocket.Connect(RemoteAddress address)
		{
			TcpAddress tcpAddress = (TcpAddress)address;
			this._connection.Connect(tcpAddress.Address, tcpAddress.Port);
			this._remoteAddress = address;
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x005022DC File Offset: 0x005004DC
		private void ReadCallback(IAsyncResult result)
		{
			try
			{
				Tuple<SocketReceiveCallback, object> tuple = (Tuple<SocketReceiveCallback, object>)result.AsyncState;
				tuple.Item1(tuple.Item2, this.GetStream().EndRead(result));
			}
			catch (ObjectDisposedException)
			{
				((ISocket)this).Close();
			}
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x00502330 File Offset: 0x00500530
		private void SendCallback(IAsyncResult result)
		{
			Tuple<SocketSendCallback, object> tuple;
			if (Platform.IsWindows)
			{
				tuple = (Tuple<SocketSendCallback, object>)result.AsyncState;
			}
			else
			{
				object[] array = (object[])result.AsyncState;
				LegacyNetBufferPool.ReturnBuffer((byte[])array[1]);
				tuple = (Tuple<SocketSendCallback, object>)array[0];
			}
			try
			{
				this.GetStream().EndWrite(result);
				tuple.Item1(tuple.Item2);
			}
			catch (Exception)
			{
				((ISocket)this).Close();
			}
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x005023AC File Offset: 0x005005AC
		void ISocket.AsyncSend(byte[] data, int offset, int size, SocketSendCallback callback, object state)
		{
			if (!Platform.IsWindows)
			{
				byte[] array = LegacyNetBufferPool.RequestBuffer(data, offset, size);
				this.GetStream().BeginWrite(array, 0, size, new AsyncCallback(this.SendCallback), new object[]
				{
					new Tuple<SocketSendCallback, object>(callback, state),
					array
				});
				return;
			}
			this.GetStream().BeginWrite(data, 0, size, new AsyncCallback(this.SendCallback), new Tuple<SocketSendCallback, object>(callback, state));
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x0050241D File Offset: 0x0050061D
		void ISocket.AsyncReceive(byte[] data, int offset, int size, SocketReceiveCallback callback, object state)
		{
			this.GetStream().BeginRead(data, offset, size, new AsyncCallback(this.ReadCallback), new Tuple<SocketReceiveCallback, object>(callback, state));
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x00502442 File Offset: 0x00500642
		bool ISocket.IsDataAvailable()
		{
			return this._connection.Connected && this.GetStream().DataAvailable;
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x0050245E File Offset: 0x0050065E
		RemoteAddress ISocket.GetRemoteAddress()
		{
			return this._remoteAddress;
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x00502468 File Offset: 0x00500668
		bool ISocket.StartListening(SocketConnectionAccepted callback)
		{
			IPAddress any = IPAddress.Any;
			string ipString;
			if (Program.LaunchParameters.TryGetValue("-ip", out ipString) && !IPAddress.TryParse(ipString, out any))
			{
				any = IPAddress.Any;
			}
			this._isListening = true;
			this._listenerCallback = callback;
			if (this._listener == null)
			{
				this._listener = new TcpListener(any, Netplay.ListenPort);
			}
			try
			{
				this._listener.Start();
			}
			catch (Exception)
			{
				return false;
			}
			new Thread(new ThreadStart(this.ListenLoop))
			{
				IsBackground = true,
				Name = "TCP Listen Thread"
			}.Start();
			return true;
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x00502514 File Offset: 0x00500714
		void ISocket.StopListening()
		{
			this._isListening = false;
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x00502520 File Offset: 0x00500720
		private void ListenLoop()
		{
			while (this._isListening && !Netplay.Disconnect)
			{
				try
				{
					ISocket socket = new TcpSocket(this._listener.AcceptTcpClient());
					Console.WriteLine(Language.GetTextValue("Net.ClientConnecting", socket.GetRemoteAddress()));
					this._listenerCallback(socket);
				}
				catch (Exception)
				{
				}
			}
			this._listener.Stop();
		}

		// Token: 0x04001671 RID: 5745
		private TcpClient _connection;

		// Token: 0x04001672 RID: 5746
		private TcpListener _listener;

		// Token: 0x04001673 RID: 5747
		private SocketConnectionAccepted _listenerCallback;

		// Token: 0x04001674 RID: 5748
		private RemoteAddress _remoteAddress;

		// Token: 0x04001675 RID: 5749
		private bool _isListening;

		// Token: 0x04001676 RID: 5750
		private DebugNetworkStream _debugStream;
	}
}
