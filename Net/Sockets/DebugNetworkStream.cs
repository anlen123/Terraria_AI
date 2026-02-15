using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Terraria.Net.Sockets
{
	// Token: 0x02000171 RID: 369
	public class DebugNetworkStream
	{
		// Token: 0x06001DDB RID: 7643 RVA: 0x00501A85 File Offset: 0x004FFC85
		public DebugNetworkStream(NetworkStream stream)
		{
			this._stream = stream;
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06001DDC RID: 7644 RVA: 0x00501AB8 File Offset: 0x004FFCB8
		public bool DataAvailable
		{
			get
			{
				Queue<DebugNetworkStream.Packet> incomingQueue = this._incomingQueue;
				bool result;
				lock (incomingQueue)
				{
					if (this._incomingQueue.Count > 0)
					{
						result = this._incomingQueue.Peek().IsReady();
					}
					else if (this._readMode == DebugNetworkStream.ReadMode.None)
					{
						result = this._stream.DataAvailable;
					}
					else
					{
						result = false;
					}
				}
				return result;
			}
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x00501B2C File Offset: 0x004FFD2C
		public void BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (this._closed)
			{
				throw new ObjectDisposedException("NetworkStream");
			}
			if (this._writeException != null)
			{
				throw this._writeException;
			}
			Queue<DebugNetworkStream.Packet> outgoingQueue = this._outgoingQueue;
			lock (outgoingQueue)
			{
				if (DebugNetworkStream.Latency == 0U && this._outgoingQueue.Count == 0)
				{
					this._stream.BeginWrite(buffer, offset, count, callback, state);
				}
				else
				{
					this._outgoingQueue.Enqueue(DebugNetworkStream.Packet.CopyOfSlice(buffer, offset, count));
					callback(new DebugNetworkStream.CompletedAsyncResult
					{
						AsyncState = state
					});
				}
			}
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x00501BD8 File Offset: 0x004FFDD8
		public void EndWrite(IAsyncResult result)
		{
			if (result is DebugNetworkStream.CompletedAsyncResult)
			{
				return;
			}
			this._stream.EndWrite(result);
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x00501BF0 File Offset: 0x004FFDF0
		public void BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (this._closed)
			{
				throw new ObjectDisposedException("NetworkStream");
			}
			if (this._readException != null)
			{
				throw this._readException;
			}
			this._beginReadBuf = new ArraySegment<byte>(buffer, offset, count);
			Queue<DebugNetworkStream.Packet> incomingQueue = this._incomingQueue;
			lock (incomingQueue)
			{
				while (this._readMode == DebugNetworkStream.ReadMode.Buffered && this._incomingQueue.Count == 0)
				{
					Monitor.Exit(this._incomingQueue);
					Thread.Sleep(1);
					Monitor.Enter(this._incomingQueue);
					if (this._readException != null)
					{
						throw this._readException;
					}
				}
				if (this._readMode == DebugNetworkStream.ReadMode.None && this._incomingQueue.Count == 0)
				{
					this._readMode = DebugNetworkStream.ReadMode.Direct;
					this._stream.BeginRead(buffer, offset, count, callback, state);
				}
				else
				{
					int num = 0;
					while (count > 0 && this._incomingQueue.Count > 0 && this._incomingQueue.Peek().IsReady())
					{
						DebugNetworkStream.Packet packet = this._incomingQueue.Peek();
						if (packet.Data.Count == 0)
						{
							break;
						}
						int num2 = Math.Min(packet.Data.Count, count);
						Array.Copy(packet.Data.Array, packet.Data.Offset, buffer, offset, num2);
						offset += num2;
						count -= num2;
						num += num2;
						if (num2 == packet.Data.Count)
						{
							this._incomingQueue.Dequeue();
						}
						else
						{
							packet.Data = new ArraySegment<byte>(packet.Data.Array, packet.Data.Offset + num2, packet.Data.Count - num2);
						}
					}
					callback(new DebugNetworkStream.CompletedAsyncResult
					{
						AsyncState = state,
						Read = num
					});
				}
			}
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x00501DD4 File Offset: 0x004FFFD4
		public int EndRead(IAsyncResult result)
		{
			if (result is DebugNetworkStream.CompletedAsyncResult)
			{
				return ((DebugNetworkStream.CompletedAsyncResult)result).Read;
			}
			this._readMode = DebugNetworkStream.ReadMode.None;
			return this._stream.EndRead(result);
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x00501E00 File Offset: 0x00500000
		public void Close()
		{
			this._closed = true;
			try
			{
				if (this._logWriter != null)
				{
					BinaryWriter logWriter = this._logWriter;
					lock (logWriter)
					{
						this._logWriter.Close();
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x00501E68 File Offset: 0x00500068
		private void Run()
		{
			byte[] array = null;
			while (!this._closed)
			{
				Queue<DebugNetworkStream.Packet> obj = this._outgoingQueue;
				lock (obj)
				{
					while (this._writeException == null && this._outgoingQueue.Count > 0 && this._outgoingQueue.Peek().IsReady())
					{
						this.BeginBufferedWrite(this._outgoingQueue.Dequeue());
					}
				}
				obj = this._incomingQueue;
				lock (obj)
				{
					if (this._readMode == DebugNetworkStream.ReadMode.None && DebugNetworkStream.Latency > 0U)
					{
						this._readMode = DebugNetworkStream.ReadMode.Buffered;
						if (array == null)
						{
							array = new byte[65536];
						}
						this.BeginBufferedRead(array);
					}
				}
				Thread.Sleep(1);
			}
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x00501F4C File Offset: 0x0050014C
		private void BeginBufferedWrite(DebugNetworkStream.Packet packet)
		{
			try
			{
				this._stream.BeginWrite(packet.Data.Array, packet.Data.Offset, packet.Data.Count, new AsyncCallback(this.BufferedWriteCallback), null);
			}
			catch (Exception writeException)
			{
				this._writeException = writeException;
			}
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x00501FB0 File Offset: 0x005001B0
		private void BufferedWriteCallback(IAsyncResult result)
		{
			try
			{
				this._stream.EndWrite(result);
			}
			catch (Exception writeException)
			{
				this._writeException = writeException;
			}
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x00501FE8 File Offset: 0x005001E8
		private void BeginBufferedRead(byte[] buffer)
		{
			try
			{
				this._stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(this.BufferedReadCallback), buffer);
			}
			catch (Exception readException)
			{
				this._readException = readException;
			}
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x00502030 File Offset: 0x00500230
		private void BufferedReadCallback(IAsyncResult result)
		{
			int num;
			try
			{
				num = this._stream.EndRead(result);
			}
			catch (Exception readException)
			{
				this._readException = readException;
				return;
			}
			Queue<DebugNetworkStream.Packet> incomingQueue = this._incomingQueue;
			lock (incomingQueue)
			{
				byte[] buffer = (byte[])result.AsyncState;
				this._incomingQueue.Enqueue(DebugNetworkStream.Packet.CopyOfSlice(buffer, 0, num));
				if (num != 0)
				{
					if (DebugNetworkStream.Latency == 0U)
					{
						this._readMode = DebugNetworkStream.ReadMode.None;
					}
					else
					{
						this.BeginBufferedRead(buffer);
					}
				}
			}
		}

		// Token: 0x04001665 RID: 5733
		public static uint Latency;

		// Token: 0x04001666 RID: 5734
		private readonly NetworkStream _stream;

		// Token: 0x04001667 RID: 5735
		private Queue<DebugNetworkStream.Packet> _outgoingQueue = new Queue<DebugNetworkStream.Packet>();

		// Token: 0x04001668 RID: 5736
		private Queue<DebugNetworkStream.Packet> _incomingQueue = new Queue<DebugNetworkStream.Packet>();

		// Token: 0x04001669 RID: 5737
		private Exception _writeException;

		// Token: 0x0400166A RID: 5738
		private Exception _readException;

		// Token: 0x0400166B RID: 5739
		private DebugNetworkStream.ReadMode _readMode;

		// Token: 0x0400166C RID: 5740
		private bool _closed;

		// Token: 0x0400166D RID: 5741
		private long _startTicks = Stopwatch.GetTimestamp();

		// Token: 0x0400166E RID: 5742
		private BinaryWriter _logWriter;

		// Token: 0x0400166F RID: 5743
		private ArraySegment<byte> _beginReadBuf;

		// Token: 0x0200074B RID: 1867
		private class Packet
		{
			// Token: 0x060040CB RID: 16587 RVA: 0x0069D6D3 File Offset: 0x0069B8D3
			public bool IsReady()
			{
				return this.BaseTimestamp + TimeSpan.FromMilliseconds(DebugNetworkStream.Latency) <= DateTime.Now;
			}

			// Token: 0x060040CC RID: 16588 RVA: 0x0069D6F8 File Offset: 0x0069B8F8
			public static DebugNetworkStream.Packet CopyOfSlice(byte[] buffer, int offset, int count)
			{
				byte[] array = new byte[count];
				Array.Copy(buffer, offset, array, 0, count);
				return new DebugNetworkStream.Packet
				{
					BaseTimestamp = DateTime.Now,
					Data = new ArraySegment<byte>(array)
				};
			}

			// Token: 0x04006999 RID: 27033
			public DateTime BaseTimestamp;

			// Token: 0x0400699A RID: 27034
			public ArraySegment<byte> Data;
		}

		// Token: 0x0200074C RID: 1868
		private class CompletedAsyncResult : IAsyncResult
		{
			// Token: 0x17000524 RID: 1316
			// (get) Token: 0x060040CE RID: 16590 RVA: 0x0069D732 File Offset: 0x0069B932
			// (set) Token: 0x060040CF RID: 16591 RVA: 0x0069D73A File Offset: 0x0069B93A
			public object AsyncState { get; set; }

			// Token: 0x17000525 RID: 1317
			// (get) Token: 0x060040D0 RID: 16592 RVA: 0x0069D743 File Offset: 0x0069B943
			// (set) Token: 0x060040D1 RID: 16593 RVA: 0x0069D74B File Offset: 0x0069B94B
			public int Read { get; set; }

			// Token: 0x17000526 RID: 1318
			// (get) Token: 0x060040D2 RID: 16594 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool IsCompleted
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000527 RID: 1319
			// (get) Token: 0x060040D3 RID: 16595 RVA: 0x000379F1 File Offset: 0x00035BF1
			public bool CompletedSynchronously
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000528 RID: 1320
			// (get) Token: 0x060040D4 RID: 16596 RVA: 0x0069C970 File Offset: 0x0069AB70
			public WaitHandle AsyncWaitHandle
			{
				get
				{
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x0200074D RID: 1869
		private enum ReadMode
		{
			// Token: 0x0400699E RID: 27038
			None,
			// Token: 0x0400699F RID: 27039
			Direct,
			// Token: 0x040069A0 RID: 27040
			Buffered
		}
	}
}
