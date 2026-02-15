using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Steamworks;
using Terraria.Localization;
using Terraria.Net;
using Terraria.Net.Sockets;

namespace Terraria.Social.Steam
{
	// Token: 0x0200014B RID: 331
	public class NetServerSocialModule : NetSocialModule
	{
		// Token: 0x06001CE6 RID: 7398 RVA: 0x004FF58F File Offset: 0x004FD78F
		public NetServerSocialModule() : base(1, 2)
		{
		}

		// Token: 0x06001CE7 RID: 7399 RVA: 0x004FF59C File Offset: 0x004FD79C
		private void BroadcastConnectedUsers()
		{
			List<ulong> list = new List<ulong>();
			foreach (KeyValuePair<CSteamID, NetSocialModule.ConnectionState> keyValuePair in this._connectionStateMap)
			{
				if (keyValuePair.Value == NetSocialModule.ConnectionState.Connected)
				{
					list.Add(keyValuePair.Key.m_SteamID);
				}
			}
			byte[] array = new byte[list.Count * 8 + 1];
			using (MemoryStream memoryStream = new MemoryStream(array))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write(1);
					foreach (ulong value in list)
					{
						binaryWriter.Write(value);
					}
				}
			}
			this._lobby.SendMessage(array);
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x004FF6AC File Offset: 0x004FD8AC
		public override void Initialize()
		{
			base.Initialize();
			this._reader.SetReadEvent(new SteamP2PReader.OnReadEvent(this.OnPacketRead));
			this._p2pSessionRequest = Callback<P2PSessionRequest_t>.Create(new Callback<P2PSessionRequest_t>.DispatchDelegate(this.OnP2PSessionRequest));
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
						this._lobby.Create(false, new CallResult<LobbyCreated_t>.APIDispatchDelegate(this.OnLobbyCreated));
					}
				}
				else
				{
					this._lobby.Create(true, new CallResult<LobbyCreated_t>.APIDispatchDelegate(this.OnLobbyCreated));
				}
			}
			if (Program.LaunchParameters.ContainsKey("-friendsoffriends"))
			{
				this._mode |= ServerMode.FriendsOfFriends;
			}
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x004FF7B5 File Offset: 0x004FD9B5
		public override ulong GetLobbyId()
		{
			return this._lobby.Id.m_SteamID;
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x00009E06 File Offset: 0x00008006
		public override void OpenInviteInterface()
		{
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x00009E06 File Offset: 0x00008006
		public override void CancelJoin()
		{
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public override bool CanInvite()
		{
			return false;
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x00009E06 File Offset: 0x00008006
		public override void LaunchLocalServer(Process process, ServerMode mode)
		{
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x004FF7C7 File Offset: 0x004FD9C7
		public override bool StartListening(SocketConnectionAccepted callback)
		{
			this._acceptingClients = true;
			this._connectionAcceptedCallback = callback;
			return true;
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x004FF7D8 File Offset: 0x004FD9D8
		public override void StopListening()
		{
			this._acceptingClients = false;
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Connect(RemoteAddress address)
		{
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x004FF7E4 File Offset: 0x004FD9E4
		public override void Close(RemoteAddress address)
		{
			CSteamID user = base.RemoteAddressToSteamId(address);
			this.Close(user);
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x004FF800 File Offset: 0x004FDA00
		private void Close(CSteamID user)
		{
			if (!this._connectionStateMap.ContainsKey(user))
			{
				return;
			}
			Task.Factory.StartNew(delegate()
			{
				Thread.Sleep(2000);
				SteamUser.EndAuthSession(user);
				SteamNetworking.CloseP2PSessionWithUser(user);
			});
			this._connectionStateMap[user] = NetSocialModule.ConnectionState.Inactive;
			this._reader.ClearUser(user);
			this._writer.ClearUser(user);
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x004FF879 File Offset: 0x004FDA79
		private void OnLobbyCreated(LobbyCreated_t result, bool failure)
		{
			if (failure)
			{
				return;
			}
			SteamFriends.SetRichPresence("status", Language.GetTextValue("Social.StatusInGame"));
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x004FF894 File Offset: 0x004FDA94
		private bool OnPacketRead(byte[] data, int length, CSteamID userId)
		{
			if (!this._connectionStateMap.ContainsKey(userId) || this._connectionStateMap[userId] == NetSocialModule.ConnectionState.Inactive)
			{
				P2PSessionRequest_t result;
				result.m_steamIDRemote = userId;
				this.OnP2PSessionRequest(result);
				if (!this._connectionStateMap.ContainsKey(userId) || this._connectionStateMap[userId] == NetSocialModule.ConnectionState.Inactive)
				{
					return false;
				}
			}
			NetSocialModule.ConnectionState connectionState = this._connectionStateMap[userId];
			if (connectionState != NetSocialModule.ConnectionState.Authenticating)
			{
				return connectionState == NetSocialModule.ConnectionState.Connected;
			}
			if (length < 3)
			{
				return false;
			}
			if (((int)data[1] << 8 | (int)data[0]) != length)
			{
				return false;
			}
			if (data[2] != 93)
			{
				return false;
			}
			byte[] array = new byte[data.Length - 3];
			Array.Copy(data, 3, array, 0, array.Length);
			switch (SteamUser.BeginAuthSession(array, array.Length, userId))
			{
			case 0:
				this._connectionStateMap[userId] = NetSocialModule.ConnectionState.Connected;
				this.BroadcastConnectedUsers();
				break;
			case 1:
				this.Close(userId);
				break;
			case 2:
				this.Close(userId);
				break;
			case 3:
				this.Close(userId);
				break;
			case 4:
				this.Close(userId);
				break;
			case 5:
				this.Close(userId);
				break;
			}
			return false;
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x004FF9A4 File Offset: 0x004FDBA4
		private void OnP2PSessionRequest(P2PSessionRequest_t result)
		{
			CSteamID steamIDRemote = result.m_steamIDRemote;
			if (this._connectionStateMap.ContainsKey(steamIDRemote) && this._connectionStateMap[steamIDRemote] != NetSocialModule.ConnectionState.Inactive)
			{
				SteamNetworking.AcceptP2PSessionWithUser(steamIDRemote);
				return;
			}
			if (!this._acceptingClients)
			{
				return;
			}
			if ((this._mode & ServerMode.FriendsOfFriends) == ServerMode.None && SteamFriends.GetFriendRelationship(steamIDRemote) != 3 && steamIDRemote != SteamUser.GetSteamID())
			{
				return;
			}
			SteamNetworking.AcceptP2PSessionWithUser(steamIDRemote);
			P2PSessionState_t p2PSessionState_t;
			while (SteamNetworking.GetP2PSessionState(steamIDRemote, ref p2PSessionState_t) && p2PSessionState_t.m_bConnecting == 1)
			{
			}
			if (p2PSessionState_t.m_bConnectionActive == 0)
			{
				this.Close(steamIDRemote);
			}
			this._connectionStateMap[steamIDRemote] = NetSocialModule.ConnectionState.Authenticating;
			this._connectionAcceptedCallback(new SocialSocket(new SteamAddress(steamIDRemote)));
		}

		// Token: 0x040015F1 RID: 5617
		private ServerMode _mode;

		// Token: 0x040015F2 RID: 5618
		private Callback<P2PSessionRequest_t> _p2pSessionRequest;

		// Token: 0x040015F3 RID: 5619
		private bool _acceptingClients;

		// Token: 0x040015F4 RID: 5620
		private SocketConnectionAccepted _connectionAcceptedCallback;
	}
}
