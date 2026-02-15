using System;
using System.Collections.Generic;
using Steamworks;

namespace Terraria.Social.Steam
{
	// Token: 0x02000149 RID: 329
	public class Lobby
	{
		// Token: 0x06001CC3 RID: 7363 RVA: 0x004FECB8 File Offset: 0x004FCEB8
		public Lobby()
		{
			this._lobbyEnter = CallResult<LobbyEnter_t>.Create(new CallResult<LobbyEnter_t>.APIDispatchDelegate(this.OnLobbyEntered));
			this._lobbyCreated = CallResult<LobbyCreated_t>.Create(new CallResult<LobbyCreated_t>.APIDispatchDelegate(this.OnLobbyCreated));
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x004FED2C File Offset: 0x004FCF2C
		public void Create(bool inviteOnly, CallResult<LobbyCreated_t>.APIDispatchDelegate callResult)
		{
			SteamAPICall_t steamAPICall_t = SteamMatchmaking.CreateLobby(inviteOnly ? 0 : 1, 256);
			this._lobbyCreatedExternalCallback = callResult;
			this._lobbyCreated.Set(steamAPICall_t, null);
			this.State = LobbyState.Creating;
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x004FED66 File Offset: 0x004FCF66
		public void OpenInviteOverlay()
		{
			if (this.State == LobbyState.Inactive)
			{
				SteamFriends.ActivateGameOverlayInviteDialog(new CSteamID(Main.LobbyId));
				return;
			}
			SteamFriends.ActivateGameOverlayInviteDialog(this.Id);
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x004FED8C File Offset: 0x004FCF8C
		public void Join(CSteamID lobbyId, CallResult<LobbyEnter_t>.APIDispatchDelegate callResult)
		{
			if (this.State != LobbyState.Inactive)
			{
				return;
			}
			this.State = LobbyState.Connecting;
			this._lobbyEnterExternalCallback = callResult;
			SteamAPICall_t steamAPICall_t = SteamMatchmaking.JoinLobby(lobbyId);
			this._lobbyEnter.Set(steamAPICall_t, null);
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x004FEDC4 File Offset: 0x004FCFC4
		public byte[] GetMessage(int index)
		{
			CSteamID csteamID;
			EChatEntryType echatEntryType;
			int lobbyChatEntry = SteamMatchmaking.GetLobbyChatEntry(this.Id, index, ref csteamID, this._messageBuffer, this._messageBuffer.Length, ref echatEntryType);
			byte[] array = new byte[lobbyChatEntry];
			Array.Copy(this._messageBuffer, array, lobbyChatEntry);
			return array;
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x004FEE05 File Offset: 0x004FD005
		public int GetUserCount()
		{
			return SteamMatchmaking.GetNumLobbyMembers(this.Id);
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x004FEE12 File Offset: 0x004FD012
		public CSteamID GetUserByIndex(int index)
		{
			return SteamMatchmaking.GetLobbyMemberByIndex(this.Id, index);
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x004FEE20 File Offset: 0x004FD020
		public bool SendMessage(byte[] data)
		{
			return this.SendMessage(data, data.Length);
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x004FEE2C File Offset: 0x004FD02C
		public bool SendMessage(byte[] data, int length)
		{
			return this.State == LobbyState.Active && SteamMatchmaking.SendLobbyChatMsg(this.Id, data, length);
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x004FEE46 File Offset: 0x004FD046
		public void Set(CSteamID lobbyId)
		{
			this.Id = lobbyId;
			this.State = LobbyState.Active;
			this.Owner = SteamMatchmaking.GetLobbyOwner(lobbyId);
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x004FEE62 File Offset: 0x004FD062
		public void SetPlayedWith(CSteamID userId)
		{
			if (this._usersSeen.Contains(userId))
			{
				return;
			}
			SteamFriends.SetPlayedWith(userId);
			this._usersSeen.Add(userId);
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x004FEE86 File Offset: 0x004FD086
		public void Leave()
		{
			if (this.State == LobbyState.Active)
			{
				SteamMatchmaking.LeaveLobby(this.Id);
			}
			this.State = LobbyState.Inactive;
			this._usersSeen.Clear();
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x004FEEB0 File Offset: 0x004FD0B0
		private void OnLobbyEntered(LobbyEnter_t result, bool failure)
		{
			if (this.State != LobbyState.Connecting)
			{
				return;
			}
			if (failure)
			{
				this.State = LobbyState.Inactive;
			}
			else
			{
				this.State = LobbyState.Active;
			}
			this.Id = new CSteamID(result.m_ulSteamIDLobby);
			this.Owner = SteamMatchmaking.GetLobbyOwner(this.Id);
			this._lobbyEnterExternalCallback.Invoke(result, failure);
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x004FEF0C File Offset: 0x004FD10C
		private void OnLobbyCreated(LobbyCreated_t result, bool failure)
		{
			if (this.State != LobbyState.Creating)
			{
				return;
			}
			if (failure)
			{
				this.State = LobbyState.Inactive;
			}
			else
			{
				this.State = LobbyState.Active;
			}
			this.Id = new CSteamID(result.m_ulSteamIDLobby);
			this.Owner = SteamMatchmaking.GetLobbyOwner(this.Id);
			this._lobbyCreatedExternalCallback.Invoke(result, failure);
		}

		// Token: 0x040015E1 RID: 5601
		private HashSet<CSteamID> _usersSeen = new HashSet<CSteamID>();

		// Token: 0x040015E2 RID: 5602
		private byte[] _messageBuffer = new byte[1024];

		// Token: 0x040015E3 RID: 5603
		public CSteamID Id = CSteamID.Nil;

		// Token: 0x040015E4 RID: 5604
		public CSteamID Owner = CSteamID.Nil;

		// Token: 0x040015E5 RID: 5605
		public LobbyState State;

		// Token: 0x040015E6 RID: 5606
		private CallResult<LobbyEnter_t> _lobbyEnter;

		// Token: 0x040015E7 RID: 5607
		private CallResult<LobbyEnter_t>.APIDispatchDelegate _lobbyEnterExternalCallback;

		// Token: 0x040015E8 RID: 5608
		private CallResult<LobbyCreated_t> _lobbyCreated;

		// Token: 0x040015E9 RID: 5609
		private CallResult<LobbyCreated_t>.APIDispatchDelegate _lobbyCreatedExternalCallback;
	}
}
