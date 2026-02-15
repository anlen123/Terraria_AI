using System;

namespace Terraria.Social.WeGame
{
	// Token: 0x02000138 RID: 312
	public class MessageDispatcherClient
	{
		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06001C58 RID: 7256 RVA: 0x004FD40C File Offset: 0x004FB60C
		// (remove) Token: 0x06001C59 RID: 7257 RVA: 0x004FD444 File Offset: 0x004FB644
		public event Action<IPCMessage> OnMessage;

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06001C5A RID: 7258 RVA: 0x004FD47C File Offset: 0x004FB67C
		// (remove) Token: 0x06001C5B RID: 7259 RVA: 0x004FD4B4 File Offset: 0x004FB6B4
		public event Action OnConnected;

		// Token: 0x06001C5C RID: 7260 RVA: 0x004FD4EC File Offset: 0x004FB6EC
		public void Init(string clientName, string serverName)
		{
			this._clientName = clientName;
			this._severName = serverName;
			this._ipcClient.Init(clientName);
			this._ipcClient.OnDataArrive += this.OnDataArrive;
			this._ipcClient.OnConnected += this.OnServerConnected;
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x004FD541 File Offset: 0x004FB741
		public void Start()
		{
			this._ipcClient.ConnectTo(this._severName);
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x004FD554 File Offset: 0x004FB754
		private void OnDataArrive(byte[] data)
		{
			IPCMessage ipcmessage = new IPCMessage();
			ipcmessage.BuildFrom(data);
			if (this.OnMessage != null)
			{
				this.OnMessage(ipcmessage);
			}
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x004FD582 File Offset: 0x004FB782
		private void OnServerConnected()
		{
			if (this.OnConnected != null)
			{
				this.OnConnected();
			}
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x004FD597 File Offset: 0x004FB797
		public void Tick()
		{
			this._ipcClient.Tick();
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x004FD5A4 File Offset: 0x004FB7A4
		public bool SendMessage(IPCMessage msg)
		{
			return this._ipcClient.Send(msg.GetBytes());
		}

		// Token: 0x040015AE RID: 5550
		private IPCClient _ipcClient = new IPCClient();

		// Token: 0x040015AF RID: 5551
		private string _severName;

		// Token: 0x040015B0 RID: 5552
		private string _clientName;
	}
}
