using System;
using System.Collections.Generic;
using rail;

namespace Terraria.Social.WeGame
{
	// Token: 0x02000139 RID: 313
	public class WeGameP2PReader
	{
		// Token: 0x06001C64 RID: 7268 RVA: 0x004FD600 File Offset: 0x004FB800
		public void ClearUser(RailID id)
		{
			Dictionary<RailID, Queue<WeGameP2PReader.ReadResult>> pendingReadBuffers = this._pendingReadBuffers;
			lock (pendingReadBuffers)
			{
				this._deletionQueue.Enqueue(id);
			}
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x004FD648 File Offset: 0x004FB848
		public bool IsDataAvailable(RailID id)
		{
			Dictionary<RailID, Queue<WeGameP2PReader.ReadResult>> pendingReadBuffers = this._pendingReadBuffers;
			bool result;
			lock (pendingReadBuffers)
			{
				if (!this._pendingReadBuffers.ContainsKey(id))
				{
					result = false;
				}
				else
				{
					Queue<WeGameP2PReader.ReadResult> queue = this._pendingReadBuffers[id];
					if (queue.Count == 0 || queue.Peek().Size == 0U)
					{
						result = false;
					}
					else
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x004FD6C0 File Offset: 0x004FB8C0
		public void SetReadEvent(WeGameP2PReader.OnReadEvent method)
		{
			this._readEvent = method;
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x004FD6CC File Offset: 0x004FB8CC
		private bool IsPacketAvailable(out uint size)
		{
			object railLock = this.RailLock;
			bool result;
			lock (railLock)
			{
				result = rail_api.RailFactory().RailNetworkHelper().IsDataReady(new RailID
				{
					id_ = this.GetLocalPeer().id_
				}, ref size);
			}
			return result;
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x004FD730 File Offset: 0x004FB930
		private RailID GetLocalPeer()
		{
			return this._local_id;
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x004FD738 File Offset: 0x004FB938
		public void SetLocalPeer(RailID rail_id)
		{
			if (this._local_id == null)
			{
				this._local_id = new RailID();
			}
			this._local_id.id_ = rail_id.id_;
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x004FD764 File Offset: 0x004FB964
		private bool IsValid()
		{
			return this._local_id != null && this._local_id.IsValid();
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x004FD784 File Offset: 0x004FB984
		public void ReadTick()
		{
			if (!this.IsValid())
			{
				return;
			}
			Dictionary<RailID, Queue<WeGameP2PReader.ReadResult>> pendingReadBuffers = this._pendingReadBuffers;
			lock (pendingReadBuffers)
			{
				while (this._deletionQueue.Count > 0)
				{
					this._pendingReadBuffers.Remove(this._deletionQueue.Dequeue());
				}
				uint num;
				while (this.IsPacketAvailable(out num))
				{
					byte[] array;
					if (this._bufferPool.Count == 0)
					{
						array = new byte[Math.Max(num, 4096U)];
					}
					else
					{
						array = this._bufferPool.Dequeue();
					}
					RailID railID = new RailID();
					object railLock = this.RailLock;
					bool flag3;
					lock (railLock)
					{
						flag3 = (rail_api.RailFactory().RailNetworkHelper().ReadData(this.GetLocalPeer(), railID, array, num) == 0);
					}
					if (flag3)
					{
						if (this._readEvent == null || this._readEvent(array, (int)num, railID))
						{
							if (!this._pendingReadBuffers.ContainsKey(railID))
							{
								this._pendingReadBuffers[railID] = new Queue<WeGameP2PReader.ReadResult>();
							}
							this._pendingReadBuffers[railID].Enqueue(new WeGameP2PReader.ReadResult(array, num));
						}
						else
						{
							this._bufferPool.Enqueue(array);
						}
					}
				}
			}
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x004FD8FC File Offset: 0x004FBAFC
		public int Receive(RailID user, byte[] buffer, int bufferOffset, int bufferSize)
		{
			uint num = 0U;
			Dictionary<RailID, Queue<WeGameP2PReader.ReadResult>> pendingReadBuffers = this._pendingReadBuffers;
			lock (pendingReadBuffers)
			{
				if (!this._pendingReadBuffers.ContainsKey(user))
				{
					return 0;
				}
				Queue<WeGameP2PReader.ReadResult> queue = this._pendingReadBuffers[user];
				while (queue.Count > 0)
				{
					WeGameP2PReader.ReadResult readResult = queue.Peek();
					uint num2 = Math.Min((uint)(bufferSize - (int)num), readResult.Size - readResult.Offset);
					if (num2 == 0U)
					{
						return (int)num;
					}
					Array.Copy(readResult.Data, (long)((ulong)readResult.Offset), buffer, (long)bufferOffset + (long)((ulong)num), (long)((ulong)num2));
					if (num2 == readResult.Size - readResult.Offset)
					{
						this._bufferPool.Enqueue(queue.Dequeue().Data);
					}
					else
					{
						readResult.Offset += num2;
					}
					num += num2;
				}
			}
			return (int)num;
		}

		// Token: 0x040015B3 RID: 5555
		public object RailLock = new object();

		// Token: 0x040015B4 RID: 5556
		private const int BUFFER_SIZE = 4096;

		// Token: 0x040015B5 RID: 5557
		private Dictionary<RailID, Queue<WeGameP2PReader.ReadResult>> _pendingReadBuffers = new Dictionary<RailID, Queue<WeGameP2PReader.ReadResult>>();

		// Token: 0x040015B6 RID: 5558
		private Queue<RailID> _deletionQueue = new Queue<RailID>();

		// Token: 0x040015B7 RID: 5559
		private Queue<byte[]> _bufferPool = new Queue<byte[]>();

		// Token: 0x040015B8 RID: 5560
		private WeGameP2PReader.OnReadEvent _readEvent;

		// Token: 0x040015B9 RID: 5561
		private RailID _local_id;

		// Token: 0x02000734 RID: 1844
		public class ReadResult
		{
			// Token: 0x06004097 RID: 16535 RVA: 0x0069D4E4 File Offset: 0x0069B6E4
			public ReadResult(byte[] data, uint size)
			{
				this.Data = data;
				this.Size = size;
				this.Offset = 0U;
			}

			// Token: 0x04006972 RID: 26994
			public byte[] Data;

			// Token: 0x04006973 RID: 26995
			public uint Size;

			// Token: 0x04006974 RID: 26996
			public uint Offset;
		}

		// Token: 0x02000735 RID: 1845
		// (Invoke) Token: 0x06004099 RID: 16537
		public delegate bool OnReadEvent(byte[] data, int size, RailID user);
	}
}
