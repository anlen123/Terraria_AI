using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace Terraria.Social.WeGame
{
	// Token: 0x02000134 RID: 308
	public class IPCClient : IPCBase
	{
		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06001C38 RID: 7224 RVA: 0x004FCFEC File Offset: 0x004FB1EC
		// (remove) Token: 0x06001C39 RID: 7225 RVA: 0x004FD024 File Offset: 0x004FB224
		public event Action OnConnected;

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06001C3A RID: 7226 RVA: 0x004FC897 File Offset: 0x004FAA97
		// (remove) Token: 0x06001C3B RID: 7227 RVA: 0x004FC8B0 File Offset: 0x004FAAB0
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

		// Token: 0x06001C3C RID: 7228 RVA: 0x004FD059 File Offset: 0x004FB259
		private NamedPipeClientStream GetPipeStream()
		{
			return (NamedPipeClientStream)this._pipeStream;
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x004FD066 File Offset: 0x004FB266
		private void ProcessConnectedEvent()
		{
			if (this._connectedFlag)
			{
				if (this.OnConnected != null)
				{
					this.OnConnected();
				}
				this._connectedFlag = false;
			}
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x004FD08A File Offset: 0x004FB28A
		private void ProcessPipeBrokenEvent()
		{
			if (this._pipeBrokenFlag)
			{
				this.Reset();
				this._pipeBrokenFlag = false;
			}
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x004FD0A5 File Offset: 0x004FB2A5
		private void CheckFlagAndFireEvent()
		{
			this.ProcessConnectedEvent();
			this.ProcessDataArriveEvent();
			this.ProcessPipeBrokenEvent();
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x00009E06 File Offset: 0x00008006
		public void Init(string clientName)
		{
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x004FD0BC File Offset: 0x004FB2BC
		public void ConnectTo(string serverName)
		{
			if (this.GetPipeStream() == null)
			{
				this._pipeStream = new NamedPipeClientStream(".", serverName, PipeDirection.InOut, PipeOptions.Asynchronous);
				this._cancelTokenSrc = new CancellationTokenSource();
				Task.Factory.StartNew(delegate(object content)
				{
					this.GetPipeStream().Connect();
					if (!((CancellationToken)content).IsCancellationRequested)
					{
						this.GetPipeStream().ReadMode = PipeTransmissionMode.Message;
						this.BeginReadData();
						this._connectedFlag = true;
					}
				}, this._cancelTokenSrc.Token);
			}
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x004FD11A File Offset: 0x004FB31A
		public void Tick()
		{
			this.CheckFlagAndFireEvent();
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x004FD124 File Offset: 0x004FB324
		public override void ReadCallback(IAsyncResult result)
		{
			IPCContent ipccontent = (IPCContent)result.AsyncState;
			base.ReadCallback(result);
			if (!ipccontent.CancelToken.IsCancellationRequested)
			{
				if (this.GetPipeStream().IsConnected)
				{
					this.BeginReadData();
					return;
				}
			}
			else
			{
				WeGameHelper.WriteDebugString("ReadCallback cancel", new object[0]);
			}
		}

		// Token: 0x040015A7 RID: 5543
		private bool _connectedFlag;
	}
}
