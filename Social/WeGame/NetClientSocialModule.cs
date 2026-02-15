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
	// Token: 0x0200012A RID: 298
	public class NetClientSocialModule : NetSocialModule
	{
		// Token: 0x06001BAD RID: 7085 RVA: 0x004FAD34 File Offset: 0x004F8F34
		private void OnIPCClientAccess()
		{
			WeGameHelper.WriteDebugString("IPC client access", new object[0]);
			this.SendFriendListToLocalServer();
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x004FAD50 File Offset: 0x004F8F50
		private void LazyCreateWeGameMsgServer()
		{
			if (this._msgServer == null)
			{
				this._msgServer = new MessageDispatcherServer();
				this._msgServer.Init("WeGame.Terraria.Message.Server");
				this._msgServer.OnMessage += this.OnWegameMessage;
				this._msgServer.OnIPCClientAccess += this.OnIPCClientAccess;
				CoreSocialModule.OnTick += this._msgServer.Tick;
				this._msgServer.Start();
			}
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x004FADD0 File Offset: 0x004F8FD0
		private void OnWegameMessage(IPCMessage message)
		{
			if (message.GetCmd() == IPCMessageType.IPCMessageTypeReportServerID)
			{
				ReportServerID reportServerID;
				message.Parse<ReportServerID>(out reportServerID);
				this.OnReportServerID(reportServerID);
			}
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x004FADF6 File Offset: 0x004F8FF6
		private void OnReportServerID(ReportServerID reportServerID)
		{
			WeGameHelper.WriteDebugString("OnReportServerID - " + reportServerID._serverID, new object[0]);
			this.AsyncSetMyMetaData(this._serverIDMedataKey, reportServerID._serverID);
			this.AsyncSetInviteCommandLine(reportServerID._serverID);
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x004FAE34 File Offset: 0x004F9034
		public override void Initialize()
		{
			base.Initialize();
			this.RegisterRailEvent();
			this.AsyncGetFriendsInfo();
			this._reader.SetReadEvent(new WeGameP2PReader.OnReadEvent(this.OnPacketRead));
			this._reader.SetLocalPeer(base.GetLocalPeer());
			this._writer.SetLocalPeer(base.GetLocalPeer());
			Main.OnEngineLoad += this.CheckParameters;
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x004FAE9D File Offset: 0x004F909D
		private void AsyncSetInviteCommandLine(string cmdline)
		{
			rail_api.RailFactory().RailFriends().AsyncSetInviteCommandLine(cmdline, "");
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x004FAEB8 File Offset: 0x004F90B8
		private void AsyncSetMyMetaData(string key, string value)
		{
			List<RailKeyValue> list = new List<RailKeyValue>();
			list.Add(new RailKeyValue
			{
				key = key,
				value = value
			});
			rail_api.RailFactory().RailFriends().AsyncSetMyMetadata(list, "");
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x004FAEFC File Offset: 0x004F90FC
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

		// Token: 0x06001BB5 RID: 7093 RVA: 0x004FAFCC File Offset: 0x004F91CC
		private bool OnPacketRead(byte[] data, int size, RailID user)
		{
			if (!this._connectionStateMap.ContainsKey(user))
			{
				return false;
			}
			NetSocialModule.ConnectionState connectionState = this._connectionStateMap[user];
			if (connectionState == NetSocialModule.ConnectionState.Authenticating)
			{
				if (!this.TryAuthUserByRecvData(user, data, size))
				{
					WeGameHelper.WriteDebugString(" Auth Server Ticket Failed", new object[0]);
					this.Close(user);
				}
				else
				{
					WeGameHelper.WriteDebugString("OnRailAuthSessionTicket Auth Success..", new object[0]);
					this.OnAuthSuccess(user);
				}
				return false;
			}
			return connectionState == NetSocialModule.ConnectionState.Connected;
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x004FB03C File Offset: 0x004F923C
		private void OnAuthSuccess(RailID remote_peer)
		{
			if (!this._connectionStateMap.ContainsKey(remote_peer))
			{
				return;
			}
			this._connectionStateMap[remote_peer] = NetSocialModule.ConnectionState.Connected;
			this.AsyncSetPlayWith(this._inviter_id);
			this.AsyncSetMyMetaData("status", Language.GetTextValue("Social.StatusInGame"));
			this.AsyncSetMyMetaData(this._serverIDMedataKey, remote_peer.id_.ToString());
			Main.clrInput();
			Netplay.ServerPassword = "";
			Main.GetInputText("", false);
			Main.autoPass = false;
			Main.netMode = 1;
			Netplay.OnConnectedToSocialServer(new SocialSocket(new WeGameAddress(remote_peer, this.GetFriendNickname(this._inviter_id))));
			WeGameHelper.WriteDebugString("OnConnectToSocialServer server:" + remote_peer.id_.ToString(), new object[0]);
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x004FB100 File Offset: 0x004F9300
		private bool GetRailConnectIDFromCmdLine(RailID server_id)
		{
			foreach (string text in Environment.GetCommandLineArgs())
			{
				string text2 = "--rail_connect_cmd=";
				int num = text.IndexOf(text2);
				if (num != -1)
				{
					ulong id_ = 0UL;
					if (ulong.TryParse(text.Substring(num + text2.Length), out id_))
					{
						server_id.id_ = id_;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x004FB160 File Offset: 0x004F9360
		private void CheckParameters()
		{
			RailID server_id = new RailID();
			if (this.GetRailConnectIDFromCmdLine(server_id))
			{
				if (server_id.IsValid())
				{
					Main.OpenPlayerSelectFromNet(delegate
					{
						Main.menuMode = 882;
						Main.statusText = Language.GetTextValue("Social.Joining");
						WeGameHelper.WriteDebugString(" CheckParameters， lobby.join", new object[0]);
						this.JoinServer(server_id);
					});
					return;
				}
				WeGameHelper.WriteDebugString("Invalid RailID passed to +connect_lobby", new object[0]);
			}
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x004FB1C4 File Offset: 0x004F93C4
		public override void LaunchLocalServer(Process process, ServerMode mode)
		{
			if (this._lobby.State != LobbyState.Inactive)
			{
				this._lobby.Leave();
			}
			this.LazyCreateWeGameMsgServer();
			ProcessStartInfo startInfo = process.StartInfo;
			startInfo.Arguments = startInfo.Arguments + " -wegame -localwegameid " + base.GetLocalPeer().id_;
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
			string str;
			rail_api.RailFactory().RailUtils().GetLaunchAppParameters(2, ref str);
			ProcessStartInfo startInfo5 = process.StartInfo;
			startInfo5.Arguments = startInfo5.Arguments + " " + str;
			WeGameHelper.WriteDebugString("LaunchLocalServer,cmd_line:" + process.StartInfo.Arguments, new object[0]);
			this.AsyncSetMyMetaData("status", Language.GetTextValue("Social.StatusInGame"));
			Netplay.OnDisconnect += this.OnDisconnect;
			process.Start();
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x004FB2FF File Offset: 0x004F94FF
		public override void Shutdown()
		{
			this.AsyncSetInviteCommandLine("");
			this.CleanMyMetaData();
			this.UnRegisterRailEvent();
			base.Shutdown();
		}

		// Token: 0x06001BBB RID: 7099 RVA: 0x004DFDD7 File Offset: 0x004DDFD7
		public override ulong GetLobbyId()
		{
			return 0UL;
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public override bool StartListening(SocketConnectionAccepted callback)
		{
			return false;
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x00009E06 File Offset: 0x00008006
		public override void StopListening()
		{
		}

		// Token: 0x06001BBE RID: 7102 RVA: 0x004FB320 File Offset: 0x004F9520
		public override void Close(RemoteAddress address)
		{
			this.CleanMyMetaData();
			RailID remote_peer = base.RemoteAddressToRailId(address);
			this.Close(remote_peer);
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x004FB342 File Offset: 0x004F9542
		public override bool CanInvite()
		{
			return (this._hasLocalHost || this._lobby.State == LobbyState.Active || Main.LobbyId != 0UL) && Main.netMode != 0;
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x004FB36B File Offset: 0x004F956B
		public override void OpenInviteInterface()
		{
			this._lobby.OpenInviteOverlay();
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x004FB378 File Offset: 0x004F9578
		private void Close(RailID remote_peer)
		{
			if (!this._connectionStateMap.ContainsKey(remote_peer))
			{
				return;
			}
			WeGameHelper.WriteDebugString("CloseRemotePeer, remote:{0}", new object[]
			{
				remote_peer.id_
			});
			rail_api.RailFactory().RailNetworkHelper().CloseSession(base.GetLocalPeer(), remote_peer);
			this._connectionStateMap[remote_peer] = NetSocialModule.ConnectionState.Inactive;
			this._lobby.Leave();
			this._reader.ClearUser(remote_peer);
			this._writer.ClearUser(remote_peer);
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Connect(RemoteAddress address)
		{
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x004FB3F9 File Offset: 0x004F95F9
		public override void CancelJoin()
		{
			if (this._lobby.State != LobbyState.Inactive)
			{
				this._lobby.Leave();
			}
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x004FB414 File Offset: 0x004F9614
		private void RegisterRailEvent()
		{
			foreach (RAILEventID raileventID in new RAILEventID[]
			{
				16001,
				16002,
				13503,
				13501,
				12003,
				12002,
				12010
			})
			{
				this._callbackHelper.RegisterCallback(raileventID, new RailEventCallBackHandler(this.OnRailEvent));
			}
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x004FB48A File Offset: 0x004F968A
		private void UnRegisterRailEvent()
		{
			this._callbackHelper.UnregisterAllCallback();
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x004FB498 File Offset: 0x004F9698
		public void OnRailEvent(RAILEventID id, EventBase data)
		{
			WeGameHelper.WriteDebugString("OnRailEvent,id=" + id.ToString() + " ,result=" + data.result.ToString(), new object[0]);
			if (id <= 12010)
			{
				if (id == 12002)
				{
					this.OnRailSetMetaData((RailFriendsSetMetadataResult)data);
					return;
				}
				if (id == 12003)
				{
					this.OnGetFriendMetaData((RailFriendsGetMetadataResult)data);
					return;
				}
				if (id != 12010)
				{
					return;
				}
				this.OnFriendlistChange((RailFriendsListChanged)data);
				return;
			}
			else if (id <= 13503)
			{
				if (id == 13501)
				{
					this.OnRailGetUsersInfo((RailUsersInfoData)data);
					return;
				}
				if (id != 13503)
				{
					return;
				}
				this.OnRailRespondInvation((RailUsersRespondInvitation)data);
				return;
			}
			else
			{
				if (id == 16001)
				{
					this.OnRailCreateSessionRequest((CreateSessionRequest)data);
					return;
				}
				if (id != 16002)
				{
					return;
				}
				this.OnRailCreateSessionFailed((CreateSessionFailed)data);
				return;
			}
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x004FB584 File Offset: 0x004F9784
		private string DumpMataDataString(List<RailKeyValueResult> list)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (RailKeyValueResult railKeyValueResult in list)
			{
				stringBuilder.Append("key: " + railKeyValueResult.key + " value: " + railKeyValueResult.value);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x004FB5FC File Offset: 0x004F97FC
		private string GetValueByKey(string key, List<RailKeyValueResult> list)
		{
			string result = null;
			foreach (RailKeyValueResult railKeyValueResult in list)
			{
				if (railKeyValueResult.key == key)
				{
					result = railKeyValueResult.value;
					break;
				}
			}
			return result;
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x004FB660 File Offset: 0x004F9860
		private bool SendFriendListToLocalServer()
		{
			bool result = false;
			if (this._hasLocalHost)
			{
				List<RailFriendInfo> list = new List<RailFriendInfo>();
				if (this.GetRailFriendList(list))
				{
					WeGameFriendListInfo t = new WeGameFriendListInfo
					{
						_friendList = list
					};
					IPCMessage ipcmessage = new IPCMessage();
					ipcmessage.Build<WeGameFriendListInfo>(IPCMessageType.IPCMessageTypeNotifyFriendList, t);
					result = this._msgServer.SendMessage(ipcmessage);
					WeGameHelper.WriteDebugString("NotifyFriendListToServer: " + result.ToString(), new object[0]);
				}
			}
			return result;
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x004FB6CC File Offset: 0x004F98CC
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

		// Token: 0x06001BCB RID: 7115 RVA: 0x004FB6F8 File Offset: 0x004F98F8
		private void OnGetFriendMetaData(RailFriendsGetMetadataResult data)
		{
			if (data.result == null && data.friend_kvs.Count > 0)
			{
				WeGameHelper.WriteDebugString("OnGetFriendMetaData - " + this.DumpMataDataString(data.friend_kvs), new object[0]);
				string valueByKey = this.GetValueByKey(this._serverIDMedataKey, data.friend_kvs);
				if (valueByKey != null)
				{
					if (valueByKey.Length > 0)
					{
						RailID railID = new RailID();
						railID.id_ = ulong.Parse(valueByKey);
						if (railID.IsValid())
						{
							this.JoinServer(railID);
							return;
						}
						WeGameHelper.WriteDebugString("JoinServer failed, invalid server id", new object[0]);
						return;
					}
					else
					{
						WeGameHelper.WriteDebugString("can not find server id key", new object[0]);
					}
				}
			}
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x004FB7A4 File Offset: 0x004F99A4
		private void JoinServer(RailID server_id)
		{
			WeGameHelper.WriteDebugString("JoinServer:{0}", new object[]
			{
				server_id.id_
			});
			this._connectionStateMap[server_id] = NetSocialModule.ConnectionState.Authenticating;
			int num = 3;
			byte[] array = new byte[num];
			array[0] = (byte)(num & 255);
			array[1] = (byte)(num >> 8 & 255);
			array[2] = 93;
			rail_api.RailFactory().RailNetworkHelper().SendReliableData(base.GetLocalPeer(), server_id, array, (uint)num);
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x004FB81C File Offset: 0x004F9A1C
		private string GetFriendNickname(RailID rail_id)
		{
			if (this._player_info_list != null)
			{
				foreach (PlayerPersonalInfo playerPersonalInfo in this._player_info_list)
				{
					if (playerPersonalInfo.rail_id == rail_id)
					{
						return playerPersonalInfo.rail_name;
					}
				}
			}
			return "";
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x004FB890 File Offset: 0x004F9A90
		private void OnRailGetUsersInfo(RailUsersInfoData data)
		{
			this._player_info_list = data.user_info_list;
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x004FB89E File Offset: 0x004F9A9E
		private void OnFriendlistChange(RailFriendsListChanged data)
		{
			if (this._hasLocalHost)
			{
				this.SendFriendListToLocalServer();
			}
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x004FB8B0 File Offset: 0x004F9AB0
		private void AsyncGetFriendsInfo()
		{
			IRailFriends railFriends = rail_api.RailFactory().RailFriends();
			if (railFriends != null)
			{
				List<RailFriendInfo> list = new List<RailFriendInfo>();
				railFriends.GetFriendsList(list);
				List<RailID> list2 = new List<RailID>();
				foreach (RailFriendInfo railFriendInfo in list)
				{
					list2.Add(railFriendInfo.friend_rail_id);
				}
				railFriends.AsyncGetPersonalInfo(list2, "");
			}
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x004FB934 File Offset: 0x004F9B34
		private void AsyncSetPlayWith(RailID rail_id)
		{
			List<RailUserPlayedWith> list = new List<RailUserPlayedWith>();
			list.Add(new RailUserPlayedWith
			{
				rail_id = rail_id
			});
			IRailFriends railFriends = rail_api.RailFactory().RailFriends();
			if (railFriends != null)
			{
				railFriends.AsyncReportPlayedWithUserList(list, "");
			}
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x004FB976 File Offset: 0x004F9B76
		private void OnRailSetMetaData(RailFriendsSetMetadataResult data)
		{
			WeGameHelper.WriteDebugString("OnRailSetMetaData - " + data.result.ToString(), new object[0]);
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x004FB9A0 File Offset: 0x004F9BA0
		private void OnRailRespondInvation(RailUsersRespondInvitation data)
		{
			WeGameHelper.WriteDebugString(" request join game", new object[0]);
			if (this._lobby.State != LobbyState.Inactive)
			{
				this._lobby.Leave();
			}
			this._inviter_id = data.inviter_id;
			Main.OpenPlayerSelectFromNet(delegate
			{
				Main.menuMode = 882;
				Main.statusText = Language.GetTextValue("Social.JoiningFriend", this.GetFriendNickname(data.inviter_id));
				this.AsyncGetServerIDByOwener(data.inviter_id);
				WeGameHelper.WriteDebugString("inviter_id: " + data.inviter_id.id_, new object[0]);
			});
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x004FBA0C File Offset: 0x004F9C0C
		private void AsyncGetServerIDByOwener(RailID ownerID)
		{
			List<string> list = new List<string>();
			list.Add(this._serverIDMedataKey);
			IRailFriends railFriends = rail_api.RailFactory().RailFriends();
			if (railFriends != null)
			{
				railFriends.AsyncGetFriendMetadata(ownerID, list, "");
			}
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x004FBA48 File Offset: 0x004F9C48
		private void OnRailCreateSessionRequest(CreateSessionRequest result)
		{
			WeGameHelper.WriteDebugString("OnRailCreateSessionRequest", new object[0]);
			if (this._connectionStateMap.ContainsKey(result.remote_peer) && this._connectionStateMap[result.remote_peer] != NetSocialModule.ConnectionState.Inactive)
			{
				WeGameHelper.WriteDebugString("AcceptSessionRequest, local{0}, remote:{1}", new object[]
				{
					result.local_peer.id_,
					result.remote_peer.id_
				});
				rail_api.RailFactory().RailNetworkHelper().AcceptSessionRequest(result.local_peer, result.remote_peer);
			}
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x004FBAE0 File Offset: 0x004F9CE0
		private void OnRailCreateSessionFailed(CreateSessionFailed result)
		{
			WeGameHelper.WriteDebugString("OnRailCreateSessionFailed, CloseRemote: local:{0}, remote:{1}", new object[]
			{
				result.local_peer.id_,
				result.remote_peer.id_
			});
			this.Close(result.remote_peer);
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x004FBB30 File Offset: 0x004F9D30
		private void CleanMyMetaData()
		{
			IRailFriends railFriends = rail_api.RailFactory().RailFriends();
			if (railFriends != null)
			{
				railFriends.AsyncClearAllMyMetadata("");
			}
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x004FBB57 File Offset: 0x004F9D57
		private void OnDisconnect()
		{
			this.CleanMyMetaData();
			this._hasLocalHost = false;
			Netplay.OnDisconnect -= this.OnDisconnect;
		}

		// Token: 0x0400157F RID: 5503
		private RailCallBackHelper _callbackHelper = new RailCallBackHelper();

		// Token: 0x04001580 RID: 5504
		private bool _hasLocalHost;

		// Token: 0x04001581 RID: 5505
		private IPCServer server = new IPCServer();

		// Token: 0x04001582 RID: 5506
		private readonly string _serverIDMedataKey = "terraria.serverid";

		// Token: 0x04001583 RID: 5507
		private RailID _inviter_id = new RailID();

		// Token: 0x04001584 RID: 5508
		private List<PlayerPersonalInfo> _player_info_list;

		// Token: 0x04001585 RID: 5509
		private MessageDispatcherServer _msgServer;
	}
}
