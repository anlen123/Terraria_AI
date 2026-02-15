using System;
using System.Collections.Generic;
using rail;

namespace Terraria.Social.WeGame
{
	// Token: 0x0200013A RID: 314
	public class WeGameP2PWriter
	{
		// Token: 0x06001C6E RID: 7278 RVA: 0x004FDA2C File Offset: 0x004FBC2C
		public void QueueSend(RailID user, byte[] data, int length)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				Queue<WeGameP2PWriter.WriteInformation> queue;
				if (this._pendingSendData.ContainsKey(user))
				{
					queue = this._pendingSendData[user];
				}
				else
				{
					queue = (this._pendingSendData[user] = new Queue<WeGameP2PWriter.WriteInformation>());
				}
				int i = length;
				int num = 0;
				while (i > 0)
				{
					WeGameP2PWriter.WriteInformation writeInformation;
					if (queue.Count == 0 || 1024 - queue.Peek().Size == 0)
					{
						if (this._bufferPool.Count > 0)
						{
							writeInformation = new WeGameP2PWriter.WriteInformation(this._bufferPool.Dequeue());
						}
						else
						{
							writeInformation = new WeGameP2PWriter.WriteInformation();
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

		// Token: 0x06001C6F RID: 7279 RVA: 0x004FDB48 File Offset: 0x004FBD48
		public void ClearUser(RailID user)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				if (this._pendingSendData.ContainsKey(user))
				{
					Queue<WeGameP2PWriter.WriteInformation> queue = this._pendingSendData[user];
					while (queue.Count > 0)
					{
						this._bufferPool.Enqueue(queue.Dequeue().Data);
					}
				}
				if (this._pendingSendDataSwap.ContainsKey(user))
				{
					Queue<WeGameP2PWriter.WriteInformation> queue2 = this._pendingSendDataSwap[user];
					while (queue2.Count > 0)
					{
						this._bufferPool.Enqueue(queue2.Dequeue().Data);
					}
				}
			}
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x004FDBFC File Offset: 0x004FBDFC
		public void SetLocalPeer(RailID rail_id)
		{
			if (this._local_id == null)
			{
				this._local_id = new RailID();
			}
			this._local_id.id_ = rail_id.id_;
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x004FDC28 File Offset: 0x004FBE28
		private RailID GetLocalPeer()
		{
			return this._local_id;
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x004FDC30 File Offset: 0x004FBE30
		private bool IsValid()
		{
			return this._local_id != null && this._local_id.IsValid();
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x004FDC50 File Offset: 0x004FBE50
		public void SendAll()
		{
			if (!this.IsValid())
			{
				return;
			}
			object @lock = this._lock;
			lock (@lock)
			{
				Utils.Swap<Dictionary<RailID, Queue<WeGameP2PWriter.WriteInformation>>>(ref this._pendingSendData, ref this._pendingSendDataSwap);
			}
			foreach (KeyValuePair<RailID, Queue<WeGameP2PWriter.WriteInformation>> keyValuePair in this._pendingSendDataSwap)
			{
				Queue<WeGameP2PWriter.WriteInformation> value = keyValuePair.Value;
				while (value.Count > 0)
				{
					WeGameP2PWriter.WriteInformation writeInformation = value.Dequeue();
					bool flag2 = rail_api.RailFactory().RailNetworkHelper().SendData(this.GetLocalPeer(), keyValuePair.Key, writeInformation.Data, (uint)writeInformation.Size) == 0;
					this._bufferPool.Enqueue(writeInformation.Data);
				}
			}
		}

		// Token: 0x040015BA RID: 5562
		private const int BUFFER_SIZE = 1024;

		// Token: 0x040015BB RID: 5563
		private RailID _local_id;

		// Token: 0x040015BC RID: 5564
		private Dictionary<RailID, Queue<WeGameP2PWriter.WriteInformation>> _pendingSendData = new Dictionary<RailID, Queue<WeGameP2PWriter.WriteInformation>>();

		// Token: 0x040015BD RID: 5565
		private Dictionary<RailID, Queue<WeGameP2PWriter.WriteInformation>> _pendingSendDataSwap = new Dictionary<RailID, Queue<WeGameP2PWriter.WriteInformation>>();

		// Token: 0x040015BE RID: 5566
		private Queue<byte[]> _bufferPool = new Queue<byte[]>();

		// Token: 0x040015BF RID: 5567
		private object _lock = new object();

		// Token: 0x02000736 RID: 1846
		public class WriteInformation
		{
			// Token: 0x0600409C RID: 16540 RVA: 0x0069D501 File Offset: 0x0069B701
			public WriteInformation()
			{
				this.Data = new byte[1024];
				this.Size = 0;
			}

			// Token: 0x0600409D RID: 16541 RVA: 0x0069D520 File Offset: 0x0069B720
			public WriteInformation(byte[] data)
			{
				this.Data = data;
				this.Size = 0;
			}

			// Token: 0x04006975 RID: 26997
			public byte[] Data;

			// Token: 0x04006976 RID: 26998
			public int Size;
		}
	}
}
