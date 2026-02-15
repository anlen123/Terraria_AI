using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace Terraria.Social.WeGame
{
	// Token: 0x02000133 RID: 307
	public class IPCServer : IPCBase
	{
		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06001C27 RID: 7207 RVA: 0x004FCD30 File Offset: 0x004FAF30
		// (remove) Token: 0x06001C28 RID: 7208 RVA: 0x004FCD68 File Offset: 0x004FAF68
		public event Action OnClientAccess;

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06001C29 RID: 7209 RVA: 0x004FC897 File Offset: 0x004FAA97
		// (remove) Token: 0x06001C2A RID: 7210 RVA: 0x004FC8B0 File Offset: 0x004FAAB0
		public override event Action<byte[]> OnDataArrive
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

		// Token: 0x06001C2B RID: 7211 RVA: 0x004FCD9D File Offset: 0x004FAF9D
		private NamedPipeServerStream GetPipeStream()
		{
			return (NamedPipeServerStream)this._pipeStream;
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x004FCDAA File Offset: 0x004FAFAA
		public void Init(string serverName)
		{
			this._serverName = serverName;
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x004FCDB3 File Offset: 0x004FAFB3
		private void LazyCreatePipe()
		{
			if (this.GetPipeStream() == null)
			{
				this._pipeStream = new NamedPipeServerStream(this._serverName, PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
				this._cancelTokenSrc = new CancellationTokenSource();
			}
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x004FCDE4 File Offset: 0x004FAFE4
		public override void ReadCallback(IAsyncResult result)
		{
			IPCContent ipccontent = (IPCContent)result.AsyncState;
			base.ReadCallback(result);
			if (!ipccontent.CancelToken.IsCancellationRequested)
			{
				this.ContinueReadOrWait();
				return;
			}
			WeGameHelper.WriteDebugString("servcer.ReadCallback cancel", new object[0]);
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x004FCE29 File Offset: 0x004FB029
		public void StartListen()
		{
			this.LazyCreatePipe();
			WeGameHelper.WriteDebugString("begin listen", new object[0]);
			this.GetPipeStream().BeginWaitForConnection(new AsyncCallback(this.ConnectionCallback), this._cancelTokenSrc.Token);
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x004FCE69 File Offset: 0x004FB069
		private void RestartListen()
		{
			this.StartListen();
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x004FCE74 File Offset: 0x004FB074
		private void ConnectionCallback(IAsyncResult result)
		{
			try
			{
				this._haveClientAccessFlag = true;
				WeGameHelper.WriteDebugString("Connected in", new object[0]);
				this.GetPipeStream().EndWaitForConnection(result);
				if (!((CancellationToken)result.AsyncState).IsCancellationRequested)
				{
					this.BeginReadData();
				}
				else
				{
					WeGameHelper.WriteDebugString("ConnectionCallback but user cancel", new object[0]);
				}
			}
			catch (IOException ex)
			{
				this._pipeBrokenFlag = true;
				WeGameHelper.WriteDebugString("ConnectionCallback Exception, {0}", new object[]
				{
					ex.Message
				});
			}
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x004FCF0C File Offset: 0x004FB10C
		public void ContinueReadOrWait()
		{
			if (this.GetPipeStream().IsConnected)
			{
				this.BeginReadData();
				return;
			}
			try
			{
				this.GetPipeStream().BeginWaitForConnection(new AsyncCallback(this.ConnectionCallback), null);
			}
			catch (IOException ex)
			{
				this._pipeBrokenFlag = true;
				WeGameHelper.WriteDebugString("ContinueReadOrWait Exception, {0}", new object[]
				{
					ex.Message
				});
			}
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x004FCF80 File Offset: 0x004FB180
		private void ProcessClientAccessEvent()
		{
			if (this._haveClientAccessFlag)
			{
				if (this.OnClientAccess != null)
				{
					this.OnClientAccess();
				}
				this._haveClientAccessFlag = false;
			}
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x004FCFA4 File Offset: 0x004FB1A4
		private void CheckFlagAndFireEvent()
		{
			this.ProcessClientAccessEvent();
			this.ProcessDataArriveEvent();
			this.ProcessPipeBrokenEvent();
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x004FCFB8 File Offset: 0x004FB1B8
		private void ProcessPipeBrokenEvent()
		{
			if (this._pipeBrokenFlag)
			{
				this.Reset();
				this._pipeBrokenFlag = false;
				this.RestartListen();
			}
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x004FCFD9 File Offset: 0x004FB1D9
		public void Tick()
		{
			this.CheckFlagAndFireEvent();
		}

		// Token: 0x040015A4 RID: 5540
		private string _serverName;

		// Token: 0x040015A5 RID: 5541
		private bool _haveClientAccessFlag;
	}
}
