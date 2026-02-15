using System;
using System.Collections.Generic;
using Steamworks;

namespace Terraria.Social.Steam
{
	// Token: 0x0200014E RID: 334
	public class SteamP2PWriter
	{
		// Token: 0x06001D08 RID: 7432 RVA: 0x00500098 File Offset: 0x004FE298
		public SteamP2PWriter(int channel)
		{
			this._channel = channel;
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x005000D4 File Offset: 0x004FE2D4
		public void QueueSend(CSteamID user, byte[] data, int length)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				Queue<SteamP2PWriter.WriteInformation> queue;
				if (this._pendingSendData.ContainsKey(user))
				{
					queue = this._pendingSendData[user];
				}
				else
				{
					queue = (this._pendingSendData[user] = new Queue<SteamP2PWriter.WriteInformation>());
				}
				int i = length;
				int num = 0;
				while (i > 0)
				{
					SteamP2PWriter.WriteInformation writeInformation;
					if (queue.Count == 0 || 1024 - queue.Peek().Size == 0)
					{
						if (this._bufferPool.Count > 0)
						{
							writeInformation = new SteamP2PWriter.WriteInformation(this._bufferPool.Dequeue());
						}
						else
						{
							writeInformation = new SteamP2PWriter.WriteInformation();
						}
						queue.Enqueue(writeInformation);
					}
					else
					{
						writeInformation = queue.Peek();
					}
					int num2 = Math.Min(i, 1024 - writeInformation.Size);
					Array.Copy(data, num, writeInformation.Data, writeInformation.Size, num2);
					writeInformation.Size += num2;
					i -= num2;
					num += num2;
				}
			}
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x005001F0 File Offset: 0x004FE3F0
		public void ClearUser(CSteamID user)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				if (this._pendingSendData.ContainsKey(user))
				{
					Queue<SteamP2PWriter.WriteInformation> queue = this._pendingSendData[user];
					while (queue.Count > 0)
					{
						this._bufferPool.Enqueue(queue.Dequeue().Data);
					}
				}
				if (this._pendingSendDataSwap.ContainsKey(user))
				{
					Queue<SteamP2PWriter.WriteInformation> queue2 = this._pendingSendDataSwap[user];
					while (queue2.Count > 0)
					{
						this._bufferPool.Enqueue(queue2.Dequeue().Data);
					}
				}
			}
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x005002A4 File Offset: 0x004FE4A4
		public void SendAll()
		{
			object @lock = this._lock;
			lock (@lock)
			{
				Utils.Swap<Dictionary<CSteamID, Queue<SteamP2PWriter.WriteInformation>>>(ref this._pendingSendData, ref this._pendingSendDataSwap);
			}
			foreach (KeyValuePair<CSteamID, Queue<SteamP2PWriter.WriteInformation>> keyValuePair in this._pendingSendDataSwap)
			{
				Queue<SteamP2PWriter.WriteInformation> value = keyValuePair.Value;
				while (value.Count > 0)
				{
					SteamP2PWriter.WriteInformation writeInformation = value.Dequeue();
					SteamNetworking.SendP2PPacket(keyValuePair.Key, writeInformation.Data, (uint)writeInformation.Size, 2, this._channel);
					this._bufferPool.Enqueue(writeInformation.Data);
				}
			}
		}

		// Token: 0x04001609 RID: 5641
		private const int BUFFER_SIZE = 1024;

		// Token: 0x0400160A RID: 5642
		private Dictionary<CSteamID, Queue<SteamP2PWriter.WriteInformation>> _pendingSendData = new Dictionary<CSteamID, Queue<SteamP2PWriter.WriteInformation>>();

		// Token: 0x0400160B RID: 5643
		private Dictionary<CSteamID, Queue<SteamP2PWriter.WriteInformation>> _pendingSendDataSwap = new Dictionary<CSteamID, Queue<SteamP2PWriter.WriteInformation>>();

		// Token: 0x0400160C RID: 5644
		private Queue<byte[]> _bufferPool = new Queue<byte[]>();

		// Token: 0x0400160D RID: 5645
		private int _channel;

		// Token: 0x0400160E RID: 5646
		private object _lock = new object();

		// Token: 0x02000741 RID: 1857
		public class WriteInformation
		{
			// Token: 0x060040B3 RID: 16563 RVA: 0x0069D660 File Offset: 0x0069B860
			public WriteInformation()
			{
				this.Data = new byte[1024];
				this.Size = 0;
			}

			// Token: 0x060040B4 RID: 16564 RVA: 0x0069D67F File Offset: 0x0069B87F
			public WriteInformation(byte[] data)
			{
				this.Data = data;
				this.Size = 0;
			}

			// Token: 0x04006988 RID: 27016
			public byte[] Data;

			// Token: 0x04006989 RID: 27017
			public int Size;
		}
	}
}
