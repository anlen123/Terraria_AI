using System;
using rail;

namespace Terraria.Social.WeGame
{
	// Token: 0x02000129 RID: 297
	public class Lobby
	{
		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06001B99 RID: 7065 RVA: 0x004FAB57 File Offset: 0x004F8D57
		// (set) Token: 0x06001B9A RID: 7066 RVA: 0x004FAB69 File Offset: 0x004F8D69
		private IRailGameServer RailServerHelper
		{
			get
			{
				if (this._gameServerInitSuccess)
				{
					return this._gameServer;
				}
				return null;
			}
			set
			{
				this._gameServer = value;
			}
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x004FAB85 File Offset: 0x004F8D85
		private IRailGameServerHelper GetRailServerHelper()
		{
			return rail_api.RailFactory().RailGameServerHelper();
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x004FAB91 File Offset: 0x004F8D91
		private void RegisterGameServerEvent()
		{
			if (this._callbackHelper != null)
			{
				this._callbackHelper.RegisterCallback(3002, new RailEventCallBackHandler(this.OnRailEvent));
			}
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x004FABB8 File Offset: 0x004F8DB8
		public void OnRailEvent(RAILEventID id, EventBase data)
		{
			WeGameHelper.WriteDebugString("OnRailEvent,id=" + id.ToString() + " ,result=" + data.result.ToString(), new object[0]);
			if (id == 3002)
			{
				this.OnGameServerCreated((CreateGameServerResult)data);
			}
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x004FAC11 File Offset: 0x004F8E11
		private void OnGameServerCreated(CreateGameServerResult result)
		{
			if (result.result == null)
			{
				this._gameServerInitSuccess = true;
				this._lobbyCreatedExternalCallback(result.game_server_id);
				this._server_id = result.game_server_id;
			}
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x004FAC40 File Offset: 0x004F8E40
		public void Create(bool inviteOnly)
		{
			if (this.State == LobbyState.Inactive)
			{
				this.RegisterGameServerEvent();
			}
			IRailGameServer railServerHelper = rail_api.RailFactory().RailGameServerHelper().AsyncCreateGameServer(new CreateGameServerOptions
			{
				has_password = false,
				enable_team_voice = false
			}, "terraria", "terraria");
			this.RailServerHelper = railServerHelper;
			this.State = LobbyState.Creating;
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x004FAC98 File Offset: 0x004F8E98
		public void OpenInviteOverlay()
		{
			WeGameHelper.WriteDebugString("OpenInviteOverlay by wegame", new object[0]);
			rail_api.RailFactory().RailFloatingWindow().AsyncShowRailFloatingWindow(10, "");
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x004FACC1 File Offset: 0x004F8EC1
		public void Join(RailID local_peer, RailID remote_peer)
		{
			if (this.State != LobbyState.Inactive)
			{
				WeGameHelper.WriteDebugString("Lobby connection attempted while already in a lobby. This should never happen?", new object[0]);
				return;
			}
			this.State = LobbyState.Connecting;
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x000762F3 File Offset: 0x000744F3
		public byte[] GetMessage(int index)
		{
			return null;
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public int GetUserCount()
		{
			return 0;
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x000762F3 File Offset: 0x000744F3
		public RailID GetUserByIndex(int index)
		{
			return null;
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x004FACE3 File Offset: 0x004F8EE3
		public bool SendMessage(byte[] data)
		{
			return this.SendMessage(data, data.Length);
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public bool SendMessage(byte[] data, int length)
		{
			return false;
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x00009E06 File Offset: 0x00008006
		public void Set(RailID lobbyId)
		{
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x00009E06 File Offset: 0x00008006
		public void SetPlayedWith(RailID userId)
		{
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x004FACEF File Offset: 0x004F8EEF
		public void Leave()
		{
			this.State = LobbyState.Inactive;
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x004FACF8 File Offset: 0x004F8EF8
		public IRailGameServer GetServer()
		{
			return this.RailServerHelper;
		}

		// Token: 0x04001579 RID: 5497
		private RailCallBackHelper _callbackHelper = new RailCallBackHelper();

		// Token: 0x0400157A RID: 5498
		public LobbyState State;

		// Token: 0x0400157B RID: 5499
		private bool _gameServerInitSuccess;

		// Token: 0x0400157C RID: 5500
		private IRailGameServer _gameServer;

		// Token: 0x0400157D RID: 5501
		public Action<RailID> _lobbyCreatedExternalCallback;

		// Token: 0x0400157E RID: 5502
		private RailID _server_id;
	}
}
