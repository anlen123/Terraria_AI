using System;
using System.Collections.Generic;
using Steamworks;

namespace Terraria.Social.Steam
{
	// Token: 0x0200014D RID: 333
	public class SteamP2PReader
	{
		// Token: 0x06001D01 RID: 7425 RVA: 0x004FFCE6 File Offset: 0x004FDEE6
		public SteamP2PReader(int channel)
		{
			this._channel = channel;
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x004FFD24 File Offset: 0x004FDF24
		public void ClearUser(CSteamID id)
		{
			Dictionary<CSteamID, Queue<SteamP2PReader.ReadResult>> pendingReadBuffers = this._pendingReadBuffers;
			lock (pendingReadBuffers)
			{
				this._deletionQueue.Enqueue(id);
			}
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x004FFD6C File Offset: 0x004FDF6C
		public bool IsDataAvailable(CSteamID id)
		{
			Dictionary<CSteamID, Queue<SteamP2PReader.ReadResult>> pendingReadBuffers = this._pendingReadBuffers;
			bool result;
			lock (pendingReadBuffers)
			{
				if (!this._pendingReadBuffers.ContainsKey(id))
				{
					result = false;
				}
				else
				{
					Queue<SteamP2PReader.ReadResult> queue = this._pendingReadBuffers[id];
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

		// Token: 0x06001D04 RID: 7428 RVA: 0x004FFDE4 File Offset: 0x004FDFE4
		public void SetReadEvent(SteamP2PReader.OnReadEvent method)
		{
			this._readEvent = method;
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x004FFDF0 File Offset: 0x004FDFF0
		private bool IsPacketAvailable(out uint size)
		{
			object steamLock = this.SteamLock;
			bool result;
			lock (steamLock)
			{
				result = SteamNetworking.IsP2PPacketAvailable(ref size, this._channel);
			}
			return result;
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x004FFE38 File Offset: 0x004FE038
		public void ReadTick()
		{
			Dictionary<CSteamID, Queue<SteamP2PReader.ReadResult>> pendingReadBuffers = this._pendingReadBuffers;
			lock (pendingReadBuffers)
			{
				while (this._deletionQueue.Count > 0)
				{
					this._pendingReadBuffers.Remove(this._deletionQueue.Dequeue());
				}
				uint val;
				while (this.IsPacketAvailable(out val))
				{
					byte[] array;
					if (this._bufferPool.Count == 0)
					{
						array = new byte[Math.Max(val, 4096U)];
					}
					else
					{
						array = this._bufferPool.Dequeue();
					}
					object steamLock = this.SteamLock;
					uint size;
					CSteamID csteamID;
					bool flag3;
					lock (steamLock)
					{
						flag3 = SteamNetworking.ReadP2PPacket(array, (uint)array.Length, ref size, ref csteamID, this._channel);
					}
					if (flag3)
					{
						if (this._readEvent == null || this._readEvent(array, (int)size, csteamID))
						{
							if (!this._pendingReadBuffers.ContainsKey(csteamID))
							{
								this._pendingReadBuffers[csteamID] = new Queue<SteamP2PReader.ReadResult>();
							}
							this._pendingReadBuffers[csteamID].Enqueue(new SteamP2PReader.ReadResult(array, size));
						}
						else
						{
							this._bufferPool.Enqueue(array);
						}
					}
				}
			}
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x004FFF9C File Offset: 0x004FE19C
		public int Receive(CSteamID user, byte[] buffer, int bufferOffset, int bufferSize)
		{
			uint num = 0U;
			Dictionary<CSteamID, Queue<SteamP2PReader.ReadResult>> pendingReadBuffers = this._pendingReadBuffers;
			lock (pendingReadBuffers)
			{
				if (!this._pendingReadBuffers.ContainsKey(user))
				{
					return 0;
				}
				Queue<SteamP2PReader.ReadResult> queue = this._pendingReadBuffers[user];
				while (queue.Count > 0)
				{
					SteamP2PReader.ReadResult readResult = queue.Peek();
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

		// Token: 0x04001602 RID: 5634
		public object SteamLock = new object();

		// Token: 0x04001603 RID: 5635
		private const int BUFFER_SIZE = 4096;

		// Token: 0x04001604 RID: 5636
		private Dictionary<CSteamID, Queue<SteamP2PReader.ReadResult>> _pendingReadBuffers = new Dictionary<CSteamID, Queue<SteamP2PReader.ReadResult>>();

		// Token: 0x04001605 RID: 5637
		private Queue<CSteamID> _deletionQueue = new Queue<CSteamID>();

		// Token: 0x04001606 RID: 5638
		private Queue<byte[]> _bufferPool = new Queue<byte[]>();

		// Token: 0x04001607 RID: 5639
		private int _channel;

		// Token: 0x04001608 RID: 5640
		private SteamP2PReader.OnReadEvent _readEvent;

		// Token: 0x0200073F RID: 1855
		public class ReadResult
		{
			// Token: 0x060040AE RID: 16558 RVA: 0x0069D643 File Offset: 0x0069B843
			public ReadResult(byte[] data, uint size)
			{
				this.Data = data;
				this.Size = size;
				this.Offset = 0U;
			}

			// Token: 0x04006985 RID: 27013
			public byte[] Data;

			// Token: 0x04006986 RID: 27014
			public uint Size;

			// Token: 0x04006987 RID: 27015
			public uint Offset;
		}

		// Token: 0x02000740 RID: 1856
		// (Invoke) Token: 0x060040B0 RID: 16560
		public delegate bool OnReadEvent(byte[] data, int size, CSteamID user);
	}
}
