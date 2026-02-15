using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;

namespace Terraria.Social.WeGame
{
	// Token: 0x02000132 RID: 306
	public abstract class IPCBase
	{
		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06001C19 RID: 7193 RVA: 0x004FC88F File Offset: 0x004FAA8F
		// (set) Token: 0x06001C18 RID: 7192 RVA: 0x004FC886 File Offset: 0x004FAA86
		public int BufferSize { get; set; }

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06001C1A RID: 7194 RVA: 0x004FC897 File Offset: 0x004FAA97
		// (remove) Token: 0x06001C1B RID: 7195 RVA: 0x004FC8B0 File Offset: 0x004FAAB0
		public virtual event Action<byte[]> OnDataArrive
		{
			add
			{
				this._onDataArrive = (Action<byte[]>)Delegate.Combine(this._onDataArrive, value);
			}
			remove
			{
				this._onDataArrive = (Action<byte[]>)Delegate.Remove(this._onDataArrive, value);
			}
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x004FC8C9 File Offset: 0x004FAAC9
		public IPCBase()
		{
			this.BufferSize = 256;
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x004FC908 File Offset: 0x004FAB08
		protected void AddPackToList(List<byte> pack)
		{
			object listLock = this._listLock;
			lock (listLock)
			{
				this._producer.Add(pack);
				this._haveDataToReadFlag = true;
			}
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x004FC958 File Offset: 0x004FAB58
		protected List<List<byte>> GetPackList()
		{
			List<List<byte>> result = null;
			object listLock = this._listLock;
			lock (listLock)
			{
				List<List<byte>> producer = this._producer;
				this._producer = this._consumer;
				this._consumer = producer;
				this._producer.Clear();
				result = this._consumer;
				this._haveDataToReadFlag = false;
			}
			return result;
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x004FC9CC File Offset: 0x004FABCC
		protected bool HaveDataToRead()
		{
			return this._haveDataToReadFlag;
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x004FC9D6 File Offset: 0x004FABD6
		public virtual void Reset()
		{
			this._cancelTokenSrc.Cancel();
			this._pipeStream.Dispose();
			this._pipeStream = null;
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x004FC9F8 File Offset: 0x004FABF8
		public virtual void ProcessDataArriveEvent()
		{
			if (this.HaveDataToRead())
			{
				List<List<byte>> packList = this.GetPackList();
				if (packList != null && this._onDataArrive != null)
				{
					foreach (List<byte> list in packList)
					{
						this._onDataArrive(list.ToArray());
					}
				}
			}
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x004FCA6C File Offset: 0x004FAC6C
		protected virtual bool BeginReadData()
		{
			bool result = false;
			IPCContent ipccontent = new IPCContent
			{
				data = new byte[this.BufferSize],
				CancelToken = this._cancelTokenSrc.Token
			};
			WeGameHelper.WriteDebugString("BeginReadData", new object[0]);
			try
			{
				if (this._pipeStream != null)
				{
					this._pipeStream.BeginRead(ipccontent.data, 0, this.BufferSize, new AsyncCallback(this.ReadCallback), ipccontent);
					result = true;
				}
			}
			catch (IOException ex)
			{
				this._pipeBrokenFlag = true;
				WeGameHelper.WriteDebugString("BeginReadData Exception, {0}", new object[]
				{
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x004FCB1C File Offset: 0x004FAD1C
		public virtual void ReadCallback(IAsyncResult result)
		{
			WeGameHelper.WriteDebugString("ReadCallback: " + Thread.CurrentThread.ManagedThreadId.ToString(), new object[0]);
			IPCContent ipccontent = (IPCContent)result.AsyncState;
			try
			{
				int num = this._pipeStream.EndRead(result);
				if (!ipccontent.CancelToken.IsCancellationRequested)
				{
					if (num > 0)
					{
						this._totalData.AddRange(ipccontent.data.Take(num));
						if (this._pipeStream.IsMessageComplete)
						{
							this.AddPackToList(this._totalData);
							this._totalData = new List<byte>();
						}
					}
				}
				else
				{
					WeGameHelper.WriteDebugString("IPCBase.ReadCallback.cancel", new object[0]);
				}
			}
			catch (IOException ex)
			{
				this._pipeBrokenFlag = true;
				WeGameHelper.WriteDebugString("ReadCallback Exception, {0}", new object[]
				{
					ex.Message
				});
			}
			catch (InvalidOperationException ex2)
			{
				this._pipeBrokenFlag = true;
				WeGameHelper.WriteDebugString("ReadCallback Exception, {0}", new object[]
				{
					ex2.Message
				});
			}
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x004FCC38 File Offset: 0x004FAE38
		public virtual bool Send(string value)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			return this.Send(bytes);
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x004FCC58 File Offset: 0x004FAE58
		public virtual bool Send(byte[] data)
		{
			bool result = false;
			if (this._pipeStream != null && this._pipeStream.IsConnected)
			{
				try
				{
					this._pipeStream.BeginWrite(data, 0, data.Length, new AsyncCallback(this.SendCallback), null);
					result = true;
				}
				catch (IOException ex)
				{
					this._pipeBrokenFlag = true;
					WeGameHelper.WriteDebugString("Send Exception, {0}", new object[]
					{
						ex.Message
					});
				}
			}
			return result;
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x004FCCD8 File Offset: 0x004FAED8
		protected virtual void SendCallback(IAsyncResult result)
		{
			try
			{
				if (this._pipeStream != null)
				{
					this._pipeStream.EndWrite(result);
				}
			}
			catch (IOException ex)
			{
				this._pipeBrokenFlag = true;
				WeGameHelper.WriteDebugString("SendCallback Exception, {0}", new object[]
				{
					ex.Message
				});
			}
		}

		// Token: 0x0400159A RID: 5530
		private List<List<byte>> _producer = new List<List<byte>>();

		// Token: 0x0400159B RID: 5531
		private List<List<byte>> _consumer = new List<List<byte>>();

		// Token: 0x0400159C RID: 5532
		private List<byte> _totalData = new List<byte>();

		// Token: 0x0400159D RID: 5533
		private object _listLock = new object();

		// Token: 0x0400159E RID: 5534
		private volatile bool _haveDataToReadFlag;

		// Token: 0x0400159F RID: 5535
		protected volatile bool _pipeBrokenFlag;

		// Token: 0x040015A0 RID: 5536
		protected PipeStream _pipeStream;

		// Token: 0x040015A1 RID: 5537
		protected CancellationTokenSource _cancelTokenSrc;

		// Token: 0x040015A2 RID: 5538
		protected Action<byte[]> _onDataArrive;
	}
}
