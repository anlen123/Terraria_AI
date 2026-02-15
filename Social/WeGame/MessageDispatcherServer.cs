using System;

namespace Terraria.Social.WeGame
{
	// Token: 0x02000137 RID: 311
	public class MessageDispatcherServer
	{
		// Token: 0x14000037 RID: 55
		// (add) Token: 0x06001C4D RID: 7245 RVA: 0x004FD26C File Offset: 0x004FB46C
		// (remove) Token: 0x06001C4E RID: 7246 RVA: 0x004FD2A4 File Offset: 0x004FB4A4
		public event Action OnIPCClientAccess;

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06001C4F RID: 7247 RVA: 0x004FD2DC File Offset: 0x004FB4DC
		// (remove) Token: 0x06001C50 RID: 7248 RVA: 0x004FD314 File Offset: 0x004FB514
		public event Action<IPCMessage> OnMessage;

		// Token: 0x06001C51 RID: 7249 RVA: 0x004FD349 File Offset: 0x004FB549
		public void Init(string serverName)
		{
			this._ipcSever.Init(serverName);
			this._ipcSever.OnDataArrive += this.OnDataArrive;
			this._ipcSever.OnClientAccess += this.OnClientAccess;
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x004FD385 File Offset: 0x004FB585
		public void OnClientAccess()
		{
			if (this.OnIPCClientAccess != null)
			{
				this.OnIPCClientAccess();
			}
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x004FD39A File Offset: 0x004FB59A
		public void Start()
		{
			this._ipcSever.StartListen();
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x004FD3A8 File Offset: 0x004FB5A8
		private void OnDataArrive(byte[] data)
		{
			IPCMessage ipcmessage = new IPCMessage();
			ipcmessage.BuildFrom(data);
			if (this.OnMessage != null)
			{
				this.OnMessage(ipcmessage);
			}
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x004FD3D6 File Offset: 0x004FB5D6
		public void Tick()
		{
			this._ipcSever.Tick();
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x004FD3E3 File Offset: 0x004FB5E3
		public bool SendMessage(IPCMessage msg)
		{
			return this._ipcSever.Send(msg.GetBytes());
		}

		// Token: 0x040015AB RID: 5547
		private IPCServer _ipcSever = new IPCServer();
	}
}
