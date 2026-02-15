using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using rail;
using Terraria.Localization;
using Terraria.Net;
using Terraria.Net.Sockets;

namespace Terraria.Social.WeGame
{
	// Token: 0x0200012B RID: 299
	public class NetServerSocialModule : NetSocialModule
	{
		// Token: 0x06001BD9 RID: 7129 RVA: 0x004FBB77 File Offset: 0x004F9D77
		public NetServerSocialModule()
		{
			this._lobby._lobbyCreatedExternalCallback = new Action<RailID>(this.OnLobbyCreated);
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x00009E06 File Offset: 0x00008006
		private void BroadcastConnectedUsers()
		{
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x004FBBB8 File Offset: 0x004F9DB8
		private bool AcceptAnUserSession(RailID local_peer, RailID remote_peer)
		{
			bool result = false;
			WeGameHelper.WriteDebugString("AcceptAnUserSession server:" + local_peer.id_.ToString() + " remote:" + remote_peer.id_.ToString(), new object[0]);
			IRailNetwork railNetwork = rail_api.RailFactory().RailNetworkHelper();
			if (railNetwork != null)
			{
				result = (railNetwork.AcceptSessionRequest(local_peer, remote_peer) == 0);
			}
			return result;
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x004FBC14 File Offset: 0x004F9E14
		private void TerminateRemotePlayerSession(RailID remote_id)
		{
			IRailPlayer railPlayer = rail_api.RailFactory().RailPlayer();
			if (railPlayer != null)
			{
				railPlayer.TerminateSessionOfPlayer(remote_id);
			}
		}

		// Token: 0x06001BDD RID: 7133 RVA: 0x004FBC38 File Offset: 0x004F9E38
		private bool CloseNetWorkSession(RailID remote_peer)
		{
			bool result = false;
			IRailNetwork railNetwork = rail_api.RailFactory().RailNetworkHelper();
			if (railNetwork != null)
			{
				result = (railNetwork.CloseSession(this._serverID, remote_peer) == 0);
			}
			return result;
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x004FBC68 File Offset: 0x004F9E68
		private RailID GetServerID()
		{
			RailID railID = null;
			IRailGameServer server = this._lobby.GetServer();
			if (server != null)
			{
				railID = server.GetGameServerRailID();
			}
			return railID ?? new RailID();
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x004FBC98 File Offset: 0x004F9E98
		private void CloseAndUpdateUserState(RailID remote_peer)
		{
			if (!this._connectionStateMap.ContainsKey(remote_peer))
			{
				return;
			}
			WeGameHelper.WriteDebugString("CloseAndUpdateUserState, remote:{0}", new object[]
			{
				remote_peer.id_
			});
			this.TerminateRemotePlayerSession(remote_peer);
			this.CloseNetWorkSession(remote_peer);
			this._connectionStateMap[remote_peer] = NetSocialModule.ConnectionState.Inactive;
			this._reader.ClearUser(remote_peer);
			this._writer.ClearUser(remote_peer);
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x004FBD06 File Offset: 0x004F9F06
		public void OnConnected()
		{
			this._serverConnected = true;
			if (this._ipcConnetedAction != null)
			{
				this._ipcConnetedAction();
			}
			this._ipcConnetedAction = null;
			WeGameHelper.WriteDebugString("IPC connected", new object[0]);
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x004FBD3C File Offset: 0x004F9F3C
		private void OnCreateSessionRequest(CreateSessionRequest data)
		{
			if (!this._acceptingClients)
			{
				WeGameHelper.WriteDebugString(" - Ignoring connection from " + data.remote_peer.id_ + " while _acceptionClients is false.", new object[0]);
				return;
			}
			if ((this._mode & ServerMode.FriendsOfFriends) == ServerMode.None && !this.IsWeGameFriend(data.remote_peer))
			{
				WeGameHelper.WriteDebugString("Ignoring connection from " + data.remote_peer.id_ + ". Friends of friends is disabled.", new object[0]);
				return;
			}
			WeGameHelper.WriteDebugString("pass wegame friend check", new object[0]);
			this.AcceptAnUserSession(data.local_peer, data.remote_peer);
			this._connectionStateMap[data.remote_peer] = NetSocialModule.ConnectionState.Authenticating;
			if (this._connectionAcceptedCallback != null)
			{
				this._connectionAcceptedCallback(new SocialSocket(new WeGameAddress(data.remote_peer, "")));
			}
		}

		// Token: 0x06001BE2 RID: 7138 RVA: 0x004FBE1C File Offset: 0x004FA01C
		private void OnCreateSessionFailed(CreateSessionFailed data)
		{
			WeGameHelper.WriteDebugString("CreateSessionFailed, local:{0}, remote:{1}", new object[]
			{
				data.local_peer.id_,
				data.remote_peer.id_
			});
			this.CloseAndUpdateUserState(data.remote_peer);
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x004FBE6C File Offset: 0x004FA06C
		private bool GetRailFriendList(List<RailFriendInfo> list)
		{
			bool result = false;
			IRailFriends railFriends = rail_api.RailFactory().RailFriends();
			if (railFriends != null)
			{
				result = (railFriends.GetFriendsList(list) == 0);
			}
			return result;
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x004FBE98 File Offset: 0x004FA098
		private void OnWegameMessage(IPCMessage message)
		{
			IPCMessageType cmd = message.GetCmd();
			if (cmd == IPCMessageType.IPCMessageTypeNotifyFriendList)
			{
				WeGameFriendListInfo friendListInfo;
				message.Parse<WeGameFriendListInfo>(out friendListInfo);
				this.UpdateFriendList(friendListInfo);
			}
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x004FBEBF File Offset: 0x004FA0BF
		private void UpdateFriendList(WeGameFriendListInfo friendListInfo)
		{
			this._wegameFriendList = friendListInfo._friendList;
			WeGameHelper.WriteDebugString("On update friend list - " + this.DumpFriendListString(friendListInfo._friendList), new object[0]);
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x004FBEF0 File Offset: 0x004FA0F0
		private bool IsWeGameFriend(RailID id)
		{
			bool result = false;
			if (this._wegameFriendList != null)
			{
				using (List<RailFriendInfo>.Enumerator enumerator = this._wegameFriendList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.friend_rail_id == id)
						{
							result = true;
							break;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x004FBF58 File Offset: 0x004FA158
		private string DumpFriendListString(List<RailFriendInfo> list)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (RailFriendInfo railFriendInfo in list)
			{
				stringBuilder.AppendLine(string.Format("friend_id: {0}, type: {1}, online: {2}, playing: {3}", new object[]
				{
					railFriendInfo.friend_rail_id.id_,
					railFriendInfo.friend_type,
					railFriendInfo.online_state.friend_online_state.ToString(),
					railFriendInfo.online_state.game_define_game_playing_state
				}));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x004FC010 File Offset: 0x004FA210
		private bool IsActiveUser(RailID user)
		{
			return this._connectionStateMap.ContainsKey(user) && this._connectionStateMap[user] > NetSocialModule.ConnectionState.Inactive;
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x004FC034 File Offset: 0x004FA234
		private void UpdateUserStateBySessionAuthResult(GameServerStartSessionWithPlayerResponse data)
		{
			RailID remote_rail_id = data.remote_rail_id;
			RailResult result = data.result;
			if (this._connectionStateMap.ContainsKey(remote_rail_id))
			{
				if (result == null)
				{
					WeGameHelper.WriteDebugString("UpdateUserStateBySessionAuthResult Auth Success", new object[0]);
					this.BroadcastConnectedUsers();
					return;
				}
				WeGameHelper.WriteDebugString("UpdateUserStateBySessionAuthResult Auth Failed", new object[0]);
				this.CloseAndUpdateUserState(remote_rail_id);
			}
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x004FC090 File Offset: 0x004FA290
		private bool TryAuthUserByRecvData(RailID user, byte[] data, int length)
		{
			WeGameHelper.WriteDebugString("TryAuthUserByRecvData user:{0}", new object[]
			{
				user.id_
			});
			if (length < 3)
			{
				WeGameHelper.WriteDebugString("Failed to validate authentication packet: Too short. (Length: " + length + ")", new object[0]);
				return false;
			}
			int num = (int)data[1] << 8 | (int)data[0];
			if (num != length)
			{
				WeGameHelper.WriteDebugString(string.Concat(new object[]
				{
					"Failed to validate authentication packet: Packet size mismatch. (",
					num,
					"!=",
					length,
					")"
				}), new object[0]);
				return false;
			}
			if (data[2] != 93)
			{
				WeGameHelper.WriteDebugString("Failed to validate authentication packet: Packet type is not correct. (Type: " + data[2] + ")", new object[0]);
				return false;
			}
			return true;
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x004FC160 File Offset: 0x004FA360
		private bool OnPacketRead(byte[] data, int size, RailID user)
		{
			if (!this.IsActiveUser(user))
			{
				WeGameHelper.WriteDebugString("OnPacketRead IsActiveUser false", new object[0]);
				return false;
			}
			NetSocialModule.ConnectionState connectionState = this._connectionStateMap[user];
			if (connectionState == NetSocialModule.ConnectionState.Authenticating)
			{
				if (!this.TryAuthUserByRecvData(user, data, size))
				{
					this.CloseAndUpdateUserState(user);
				}
				else
				{
					this.OnAuthSuccess(user);
				}
				return false;
			}
			return connectionState == NetSocialModule.ConnectionState.Connected;
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x004FC1BC File Offset: 0x004FA3BC
		private void OnAuthSuccess(RailID remote_peer)
		{
			if (!this._connectionStateMap.ContainsKey(remote_peer))
			{
				return;
			}
			this._connectionStateMap[remote_peer] = NetSocialModule.ConnectionState.Connected;
			int num = 3;
			byte[] array = new byte[num];
			array[0] = (byte)(num & 255);
			array[1] = (byte)(num >> 8 & 255);
			array[2] = 93;
			rail_api.RailFactory().RailNetworkHelper().SendReliableData(this._serverID, remote_peer, array, (uint)num);
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x004FC224 File Offset: 0x004FA424
		public void OnRailEvent(RAILEventID event_id, EventBase data)
		{
			WeGameHelper.WriteDebugString("OnRailEvent,id=" + event_id.ToString() + " ,result=" + data.result.ToString(), new object[0]);
			if (event_id == 3006)
			{
				this.UpdateUserStateBySessionAuthResult((GameServerStartSessionWithPlayerResponse)data);
				return;
			}
			if (event_id == 16001)
			{
				this.OnCreateSessionRequest((CreateSessionRequest)data);
				return;
			}
			if (event_id != 16002)
			{
				return;
			}
			this.OnCreateSessionFailed((CreateSessionFailed)data);
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x004FC2A8 File Offset: 0x004FA4A8
		private void OnLobbyCreated(RailID lobbyID)
		{
			WeGameHelper.WriteDebugString("SetLocalPeer: {0}", new object[]
			{
				lobbyID.id_
			});
			this._reader.SetLocalPeer(lobbyID);
			this._writer.SetLocalPeer(lobbyID);
			this._serverID = lobbyID;
			Action action = delegate()
			{
				ReportServerID t = new ReportServerID
				{
					_serverID = lobbyID.id_.ToString()
				};
				IPCMessage ipcmessage = new IPCMessage();
				ipcmessage.Build<ReportServerID>(IPCMessageType.IPCMessageTypeReportServerID, t);
				WeGameHelper.WriteDebugString("Send serverID to game client - " + this._client.SendMessage(ipcmessage).ToString(), new object[0]);
			};
			if (this._serverConnected)
			{
				action();
				return;
			}
			this._ipcConnetedAction = (Action)Delegate.Combine(this._ipcConnetedAction, action);
			WeGameHelper.WriteDebugString("report server id fail, no connection", new object[0]);
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x004FC360 File Offset: 0x004FA560
		private void RegisterRailEvent()
		{
			foreach (RAILEventID raileventID in new RAILEventID[]
			{
				16001,
				16002,
				3006,
				3005
			})
			{
				this._callbackHelper.RegisterCallback(raileventID, new RailEventCallBackHandler(this.OnRailEvent));
			}
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x004FC3C0 File Offset: 0x004FA5C0
		public override void Initialize()
		{
			base.Initialize();
			this._mode |= ServerMode.Lobby;
			this.RegisterRailEvent();
			this._reader.SetReadEvent(new WeGameP2PReader.OnReadEvent(this.OnPacketRead));
			if (Program.LaunchParameters.ContainsKey("-lobby"))
			{
				this._mode |= ServerMode.Lobby;
				string a = Program.LaunchParameters["-lobby"];
				if (!(a == "private"))
				{
					if (!(a == "friends"))
					{
						Console.WriteLine(Language.GetTextValue("Error.InvalidLobbyFlag", "private", "friends"));
					}
					else
					{
						this._mode |= ServerMode.FriendsCanJoin;
						this._lobby.Create(false);
					}
				}
				else
				{
					this._lobby.Create(true);
				}
			}
			if (Program.LaunchParameters.ContainsKey("-friendsoffriends"))
			{
				this._mode |= ServerMode.FriendsOfFriends;
			}
			this._client.Init("WeGame.Terraria.Message.Client", "WeGame.Terraria.Message.Server");
			this._client.OnConnected += this.OnConnected;
			this._client.OnMessage += this.OnWegameMessage;
			CoreSocialModule.OnTick += this._client.Tick;
			this._client.Start();
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x004FC50F File Offset: 0x004FA70F
		public override ulong GetLobbyId()
		{
			return this._serverID.id_;
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x00009E06 File Offset: 0x00008006
		public override void OpenInviteInterface()
		{
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x00009E06 File Offset: 0x00008006
		public override void CancelJoin()
		{
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public override bool CanInvite()
		{
			return false;
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x00009E06 File Offset: 0x00008006
		public override void LaunchLocalServer(Process process, ServerMode mode)
		{
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x004FC51C File Offset: 0x004FA71C
		public override bool StartListening(SocketConnectionAccepted callback)
		{
			this._acceptingClients = true;
			this._connectionAcceptedCallback = callback;
			return false;
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x004FC52D File Offset: 0x004FA72D
		public override void StopListening()
		{
			this._acceptingClients = false;
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Connect(RemoteAddress address)
		{
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x004FC538 File Offset: 0x004FA738
		public override void Close(RemoteAddress address)
		{
			RailID remote_peer = base.RemoteAddressToRailId(address);
			this.CloseAndUpdateUserState(remote_peer);
		}

		// Token: 0x04001586 RID: 5510
		private SocketConnectionAccepted _connectionAcceptedCallback;

		// Token: 0x04001587 RID: 5511
		private bool _acceptingClients;

		// Token: 0x04001588 RID: 5512
		private ServerMode _mode;

		// Token: 0x04001589 RID: 5513
		private RailCallBackHelper _callbackHelper = new RailCallBackHelper();

		// Token: 0x0400158A RID: 5514
		private MessageDispatcherClient _client = new MessageDispatcherClient();

		// Token: 0x0400158B RID: 5515
		private bool _serverConnected;

		// Token: 0x0400158C RID: 5516
		private RailID _serverID = new RailID();

		// Token: 0x0400158D RID: 5517
		private Action _ipcConnetedAction;

		// Token: 0x0400158E RID: 5518
		private List<RailFriendInfo> _wegameFriendList;
	}
}
