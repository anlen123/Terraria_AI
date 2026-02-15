using System;
using System.Collections.Concurrent;
using rail;
using Terraria.Net;
using Terraria.Social.Base;

namespace Terraria.Social.WeGame
{
	// Token: 0x0200012C RID: 300
	public abstract class NetSocialModule : NetSocialModule
	{
		// Token: 0x06001BFA RID: 7162 RVA: 0x004FC554 File Offset: 0x004FA754
		protected NetSocialModule()
		{
			this._reader = new WeGameP2PReader();
			this._writer = new WeGameP2PWriter();
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x004FC588 File Offset: 0x004FA788
		public override void Initialize()
		{
			CoreSocialModule.OnTick += this._reader.ReadTick;
			CoreSocialModule.OnTick += this._writer.SendAll;
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x004FC5B6 File Offset: 0x004FA7B6
		public override void Shutdown()
		{
			this._lobby.Leave();
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x004FC5C4 File Offset: 0x004FA7C4
		public override bool IsConnected(RemoteAddress address)
		{
			if (address == null)
			{
				return false;
			}
			RailID key = this.RemoteAddressToRailId(address);
			return this._connectionStateMap.ContainsKey(key) && this._connectionStateMap[key] == NetSocialModule.ConnectionState.Connected;
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x004FC5FE File Offset: 0x004FA7FE
		protected RailID GetLocalPeer()
		{
			return rail_api.RailFactory().RailPlayer().GetRailID();
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x004FC610 File Offset: 0x004FA810
		protected bool GetSessionState(RailID userId, RailNetworkSessionState state)
		{
			IRailNetwork railNetwork = rail_api.RailFactory().RailNetworkHelper();
			if (railNetwork.GetSessionState(userId, state) != null)
			{
				WeGameHelper.WriteDebugString("GetSessionState Failed user:{0}", new object[]
				{
					userId.id_
				});
				return false;
			}
			return true;
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x004FC653 File Offset: 0x004FA853
		protected RailID RemoteAddressToRailId(RemoteAddress address)
		{
			return ((WeGameAddress)address).rail_id;
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x004FC660 File Offset: 0x004FA860
		public override bool Send(RemoteAddress address, byte[] data, int length)
		{
			RailID user = this.RemoteAddressToRailId(address);
			this._writer.QueueSend(user, data, length);
			return true;
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x004FC684 File Offset: 0x004FA884
		public override int Receive(RemoteAddress address, byte[] data, int offset, int length)
		{
			if (address == null)
			{
				return 0;
			}
			RailID user = this.RemoteAddressToRailId(address);
			return this._reader.Receive(user, data, offset, length);
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x004FC6B0 File Offset: 0x004FA8B0
		public override bool IsDataAvailable(RemoteAddress address)
		{
			RailID id = this.RemoteAddressToRailId(address);
			return this._reader.IsDataAvailable(id);
		}

		// Token: 0x0400158F RID: 5519
		protected const int LobbyMessageJoin = 1;

		// Token: 0x04001590 RID: 5520
		protected Lobby _lobby = new Lobby();

		// Token: 0x04001591 RID: 5521
		protected WeGameP2PReader _reader;

		// Token: 0x04001592 RID: 5522
		protected WeGameP2PWriter _writer;

		// Token: 0x04001593 RID: 5523
		protected ConcurrentDictionary<RailID, NetSocialModule.ConnectionState> _connectionStateMap = new ConcurrentDictionary<RailID, NetSocialModule.ConnectionState>();

		// Token: 0x04001594 RID: 5524
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

		// Token: 0x02000732 RID: 1842
		public enum ConnectionState
		{
			// Token: 0x0400696C RID: 26988
			Inactive,
			// Token: 0x0400696D RID: 26989
			Authenticating,
			// Token: 0x0400696E RID: 26990
			Connected
		}
	}
}
