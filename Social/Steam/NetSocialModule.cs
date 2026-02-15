using System;
using System.Collections.Concurrent;
using System.IO;
using Steamworks;
using Terraria.Net;
using Terraria.Social.Base;

namespace Terraria.Social.Steam
{
	// Token: 0x0200014C RID: 332
	public abstract class NetSocialModule : NetSocialModule
	{
		// Token: 0x06001CF6 RID: 7414 RVA: 0x004FFA54 File Offset: 0x004FDC54
		protected NetSocialModule(int readChannel, int writeChannel)
		{
			this._reader = new SteamP2PReader(readChannel);
			this._writer = new SteamP2PWriter(writeChannel);
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x004FFAA0 File Offset: 0x004FDCA0
		public override void Initialize()
		{
			CoreSocialModule.OnTick += this._reader.ReadTick;
			CoreSocialModule.OnTick += this._writer.SendAll;
			this._lobbyChatMessage = Callback<LobbyChatMsg_t>.Create(new Callback<LobbyChatMsg_t>.DispatchDelegate(this.OnLobbyChatMessage));
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x004FFAF1 File Offset: 0x004FDCF1
		public override void Shutdown()
		{
			this._lobby.Leave();
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x004FFB00 File Offset: 0x004FDD00
		public override bool IsConnected(RemoteAddress address)
		{
			if (address == null)
			{
				return false;
			}
			CSteamID csteamID = this.RemoteAddressToSteamId(address);
			if (!this._connectionStateMap.ContainsKey(csteamID) || this._connectionStateMap[csteamID] != NetSocialModule.ConnectionState.Connected)
			{
				return false;
			}
			if (this.GetSessionState(csteamID).m_bConnectionActive != 1)
			{
				this.Close(address);
				return false;
			}
			return true;
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x004FFB54 File Offset: 0x004FDD54
		protected virtual void OnLobbyChatMessage(LobbyChatMsg_t result)
		{
			if (result.m_ulSteamIDLobby != this._lobby.Id.m_SteamID)
			{
				return;
			}
			if (result.m_eChatEntryType != 1)
			{
				return;
			}
			if (result.m_ulSteamIDUser != this._lobby.Owner.m_SteamID)
			{
				return;
			}
			byte[] message = this._lobby.GetMessage((int)result.m_iChatID);
			if (message.Length == 0)
			{
				return;
			}
			using (MemoryStream memoryStream = new MemoryStream(message))
			{
				using (BinaryReader binaryReader = new BinaryReader(memoryStream))
				{
					byte b = binaryReader.ReadByte();
					if (b == 1)
					{
						while ((long)message.Length - memoryStream.Position >= 8L)
						{
							CSteamID csteamID;
							csteamID..ctor(binaryReader.ReadUInt64());
							if (csteamID != SteamUser.GetSteamID())
							{
								this._lobby.SetPlayedWith(csteamID);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x004FFC38 File Offset: 0x004FDE38
		protected P2PSessionState_t GetSessionState(CSteamID userId)
		{
			P2PSessionState_t result;
			SteamNetworking.GetP2PSessionState(userId, ref result);
			return result;
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x004FFC4F File Offset: 0x004FDE4F
		protected CSteamID RemoteAddressToSteamId(RemoteAddress address)
		{
			return ((SteamAddress)address).SteamId;
		}

		// Token: 0x06001CFD RID: 7421 RVA: 0x004FFC5C File Offset: 0x004FDE5C
		public override bool Send(RemoteAddress address, byte[] data, int length)
		{
			CSteamID user = this.RemoteAddressToSteamId(address);
			this._writer.QueueSend(user, data, length);
			return true;
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x004FFC80 File Offset: 0x004FDE80
		public override int Receive(RemoteAddress address, byte[] data, int offset, int length)
		{
			if (address == null)
			{
				return 0;
			}
			CSteamID user = this.RemoteAddressToSteamId(address);
			return this._reader.Receive(user, data, offset, length);
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x004FFCAC File Offset: 0x004FDEAC
		public override bool IsDataAvailable(RemoteAddress address)
		{
			CSteamID id = this.RemoteAddressToSteamId(address);
			return this._reader.IsDataAvailable(id);
		}

		// Token: 0x040015F5 RID: 5621
		protected const int ServerReadChannel = 1;

		// Token: 0x040015F6 RID: 5622
		protected const int ClientReadChannel = 2;

		// Token: 0x040015F7 RID: 5623
		protected const int LobbyMessageJoin = 1;

		// Token: 0x040015F8 RID: 5624
		protected const ushort GamePort = 27005;

		// Token: 0x040015F9 RID: 5625
		protected const ushort SteamPort = 27006;

		// Token: 0x040015FA RID: 5626
		protected const ushort QueryPort = 27007;

		// Token: 0x040015FB RID: 5627
		protected static readonly byte[] _handshake = new byte[]
		{
			10,
			0,
			93,
			114,
			101,
			108,
			111,
			103,
			105,
			99
		};

		// Token: 0x040015FC RID: 5628
		protected SteamP2PReader _reader;

		// Token: 0x040015FD RID: 5629
		protected SteamP2PWriter _writer;

		// Token: 0x040015FE RID: 5630
		protected Lobby _lobby = new Lobby();

		// Token: 0x040015FF RID: 5631
		protected ConcurrentDictionary<CSteamID, NetSocialModule.ConnectionState> _connectionStateMap = new ConcurrentDictionary<CSteamID, NetSocialModule.ConnectionState>();

		// Token: 0x04001600 RID: 5632
		protected object _steamLock = new object();

		// Token: 0x04001601 RID: 5633
		private Callback<LobbyChatMsg_t> _lobbyChatMessage;

		// Token: 0x0200073D RID: 1853
		public enum ConnectionState
		{
			// Token: 0x04006982 RID: 27010
			Inactive,
			// Token: 0x04006983 RID: 27011
			Authenticating,
			// Token: 0x04006984 RID: 27012
			Connected
		}

		// Token: 0x0200073E RID: 1854
		// (Invoke) Token: 0x060040AB RID: 16555
		protected delegate void AsyncHandshake(CSteamID client);
	}
}
