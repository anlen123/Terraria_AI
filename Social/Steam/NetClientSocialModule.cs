using System;
using System.Diagnostics;
using Steamworks;
using Terraria.Localization;
using Terraria.Net;
using Terraria.Net.Sockets;
using Terraria.Social.WeGame;

namespace Terraria.Social.Steam
{
	// Token: 0x0200014A RID: 330
	public class NetClientSocialModule : NetSocialModule
	{
		// Token: 0x06001CD1 RID: 7377 RVA: 0x004FEF65 File Offset: 0x004FD165
		public NetClientSocialModule() : base(2, 1)
		{
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x004FEF8C File Offset: 0x004FD18C
		public override void Initialize()
		{
			base.Initialize();
			this._gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(new Callback<GameLobbyJoinRequested_t>.DispatchDelegate(this.OnLobbyJoinRequest));
			this._p2pSessionRequest = Callback<P2PSessionRequest_t>.Create(new Callback<P2PSessionRequest_t>.DispatchDelegate(this.OnP2PSessionRequest));
			this._p2pSessionConnectfail = Callback<P2PSessionConnectFail_t>.Create(new Callback<P2PSessionConnectFail_t>.DispatchDelegate(this.OnSessionConnectFail));
			Main.OnEngineLoad += this.CheckParameters;
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x004FEFF8 File Offset: 0x004FD1F8
		private void CheckParameters()
		{
			ulong lobbyId;
			if (Program.LaunchParameters.ContainsKey("+connect_lobby") && ulong.TryParse(Program.LaunchParameters["+connect_lobby"], out lobbyId))
			{
				this.ConnectToLobby(lobbyId);
			}
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x004FF038 File Offset: 0x004FD238
		public void ConnectToLobby(ulong lobbyId)
		{
			CSteamID lobbySteamId = new CSteamID(lobbyId);
			if (lobbySteamId.IsValid())
			{
				Main.OpenPlayerSelectFromNet(delegate
				{
					Main.menuMode = 882;
					Main.statusText = Language.GetTextValue("Social.Joining");
					this._lobby.Join(lobbySteamId, new CallResult<LobbyEnter_t>.APIDispatchDelegate(this.OnLobbyEntered));
				});
			}
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x004FF07C File Offset: 0x004FD27C
		public override void LaunchLocalServer(Process process, ServerMode mode)
		{
			WeGameHelper.WriteDebugString("LaunchLocalServer", new object[0]);
			if (this._lobby.State != LobbyState.Inactive)
			{
				this._lobby.Leave();
			}
			ProcessStartInfo startInfo = process.StartInfo;
			startInfo.Arguments = startInfo.Arguments + " -steam -localsteamid " + SteamUser.GetSteamID().m_SteamID;
			if ((mode & ServerMode.Lobby) != ServerMode.None)
			{
				this._hasLocalHost = true;
				if ((mode & ServerMode.FriendsCanJoin) != ServerMode.None)
				{
					ProcessStartInfo startInfo2 = process.StartInfo;
					startInfo2.Arguments += " -lobby friends";
				}
				else
				{
					ProcessStartInfo startInfo3 = process.StartInfo;
					startInfo3.Arguments += " -lobby private";
				}
				if ((mode & ServerMode.FriendsOfFriends) != ServerMode.None)
				{
					ProcessStartInfo startInfo4 = process.StartInfo;
					startInfo4.Arguments += " -friendsoffriends";
				}
			}
			SteamFriends.SetRichPresence("status", Language.GetTextValue("Social.StatusInGame"));
			Netplay.OnDisconnect += this.OnDisconnect;
			process.Start();
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x004DFDD7 File Offset: 0x004DDFD7
		public override ulong GetLobbyId()
		{
			return 0UL;
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public override bool StartListening(SocketConnectionAccepted callback)
		{
			return false;
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x00009E06 File Offset: 0x00008006
		public override void StopListening()
		{
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x004FF174 File Offset: 0x004FD374
		public override void Close(RemoteAddress address)
		{
			SteamFriends.ClearRichPresence();
			CSteamID user = base.RemoteAddressToSteamId(address);
			this.Close(user);
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x004FF195 File Offset: 0x004FD395
		public override bool CanInvite()
		{
			return (this._hasLocalHost || this._lobby.State == LobbyState.Active || Main.LobbyId != 0UL) && Main.netMode != 0;
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x004FF1BE File Offset: 0x004FD3BE
		public override void OpenInviteInterface()
		{
			this._lobby.OpenInviteOverlay();
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x004FF1CC File Offset: 0x004FD3CC
		private void Close(CSteamID user)
		{
			if (!this._connectionStateMap.ContainsKey(user))
			{
				return;
			}
			SteamNetworking.CloseP2PSessionWithUser(user);
			this.ClearAuthTicket();
			this._connectionStateMap[user] = NetSocialModule.ConnectionState.Inactive;
			this._lobby.Leave();
			this._reader.ClearUser(user);
			this._writer.ClearUser(user);
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Connect(RemoteAddress address)
		{
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x004FF225 File Offset: 0x004FD425
		public override void CancelJoin()
		{
			if (this._lobby.State != LobbyState.Inactive)
			{
				this._lobby.Leave();
			}
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x004FF240 File Offset: 0x004FD440
		private void OnLobbyJoinRequest(GameLobbyJoinRequested_t result)
		{
			WeGameHelper.WriteDebugString(" OnLobbyJoinRequest", new object[0]);
			if (this._lobby.State != LobbyState.Inactive)
			{
				this._lobby.Leave();
			}
			string friendName = SteamFriends.GetFriendPersonaName(result.m_steamIDFriend);
			Main.OpenPlayerSelectFromNet(delegate
			{
				Main.menuMode = 882;
				Main.statusText = Language.GetTextValue("Social.JoiningFriend", friendName);
				this._lobby.Join(result.m_steamIDLobby, new CallResult<LobbyEnter_t>.APIDispatchDelegate(this.OnLobbyEntered));
			});
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x004FF2B0 File Offset: 0x004FD4B0
		private void OnLobbyEntered(LobbyEnter_t result, bool failure)
		{
			WeGameHelper.WriteDebugString(" OnLobbyEntered", new object[0]);
			SteamNetworking.AllowP2PPacketRelay(true);
			this.SendAuthTicket(this._lobby.Owner);
			int num = 0;
			P2PSessionState_t p2PSessionState_t;
			while (SteamNetworking.GetP2PSessionState(this._lobby.Owner, ref p2PSessionState_t) && p2PSessionState_t.m_bConnectionActive != 1)
			{
				switch (p2PSessionState_t.m_eP2PSessionError)
				{
				case 1:
					this.ClearAuthTicket();
					return;
				case 2:
					this.ClearAuthTicket();
					return;
				case 3:
					this.ClearAuthTicket();
					return;
				case 4:
					if (++num > 5)
					{
						this.ClearAuthTicket();
						return;
					}
					SteamNetworking.CloseP2PSessionWithUser(this._lobby.Owner);
					this.SendAuthTicket(this._lobby.Owner);
					break;
				case 5:
					this.ClearAuthTicket();
					return;
				}
			}
			this._connectionStateMap[this._lobby.Owner] = NetSocialModule.ConnectionState.Connected;
			SteamFriends.SetPlayedWith(this._lobby.Owner);
			SteamFriends.SetRichPresence("status", Language.GetTextValue("Social.StatusInGame"));
			Main.clrInput();
			Netplay.ServerPassword = "";
			Main.GetInputText("", false);
			Main.autoPass = false;
			Main.netMode = 1;
			Netplay.OnConnectedToSocialServer(new SocialSocket(new SteamAddress(this._lobby.Owner)));
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x004FF3F8 File Offset: 0x004FD5F8
		private void SendAuthTicket(CSteamID address)
		{
			WeGameHelper.WriteDebugString(" SendAuthTicket", new object[0]);
			if (this._authTicket == HAuthTicket.Invalid)
			{
				SteamNetworkingIdentity steamNetworkingIdentity = default(SteamNetworkingIdentity);
				steamNetworkingIdentity.SetSteamID(address);
				this._authTicket = SteamUser.GetAuthSessionTicket(this._authData, this._authData.Length, ref this._authDataLength, ref steamNetworkingIdentity);
			}
			int num = (int)(this._authDataLength + 3U);
			byte[] array = new byte[num];
			array[0] = (byte)(num & 255);
			array[1] = (byte)(num >> 8 & 255);
			array[2] = 93;
			int num2 = 0;
			while ((long)num2 < (long)((ulong)this._authDataLength))
			{
				array[num2 + 3] = this._authData[num2];
				num2++;
			}
			SteamNetworking.SendP2PPacket(address, array, (uint)num, 2, 1);
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x004FF4B0 File Offset: 0x004FD6B0
		private void ClearAuthTicket()
		{
			if (this._authTicket != HAuthTicket.Invalid)
			{
				SteamUser.CancelAuthTicket(this._authTicket);
			}
			this._authTicket = HAuthTicket.Invalid;
			for (int i = 0; i < this._authData.Length; i++)
			{
				this._authData[i] = 0;
			}
			this._authDataLength = 0U;
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x004FF508 File Offset: 0x004FD708
		private void OnDisconnect()
		{
			SteamFriends.ClearRichPresence();
			this._hasLocalHost = false;
			Netplay.OnDisconnect -= this.OnDisconnect;
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x004FF527 File Offset: 0x004FD727
		private void OnSessionConnectFail(P2PSessionConnectFail_t result)
		{
			WeGameHelper.WriteDebugString(" OnSessionConnectFail", new object[0]);
			this.Close(result.m_steamIDRemote);
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x004FF548 File Offset: 0x004FD748
		private void OnP2PSessionRequest(P2PSessionRequest_t result)
		{
			WeGameHelper.WriteDebugString(" OnP2PSessionRequest", new object[0]);
			CSteamID steamIDRemote = result.m_steamIDRemote;
			if (this._connectionStateMap.ContainsKey(steamIDRemote) && this._connectionStateMap[steamIDRemote] != NetSocialModule.ConnectionState.Inactive)
			{
				SteamNetworking.AcceptP2PSessionWithUser(steamIDRemote);
			}
		}

		// Token: 0x040015EA RID: 5610
		private Callback<GameLobbyJoinRequested_t> _gameLobbyJoinRequested;

		// Token: 0x040015EB RID: 5611
		private Callback<P2PSessionRequest_t> _p2pSessionRequest;

		// Token: 0x040015EC RID: 5612
		private Callback<P2PSessionConnectFail_t> _p2pSessionConnectfail;

		// Token: 0x040015ED RID: 5613
		private HAuthTicket _authTicket = HAuthTicket.Invalid;

		// Token: 0x040015EE RID: 5614
		private byte[] _authData = new byte[1021];

		// Token: 0x040015EF RID: 5615
		private uint _authDataLength;

		// Token: 0x040015F0 RID: 5616
		private bool _hasLocalHost;
	}
}
