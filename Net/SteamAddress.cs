using System;
using Steamworks;

namespace Terraria.Net
{
	// Token: 0x0200016A RID: 362
	public class SteamAddress : RemoteAddress
	{
		// Token: 0x06001DAE RID: 7598 RVA: 0x00501081 File Offset: 0x004FF281
		public SteamAddress(CSteamID steamId)
		{
			this.Type = AddressType.Steam;
			this.SteamId = steamId;
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x00501098 File Offset: 0x004FF298
		public override string ToString()
		{
			string str = (this.SteamId.m_SteamID % 2UL).ToString();
			string str2 = ((this.SteamId.m_SteamID - (76561197960265728UL + this.SteamId.m_SteamID % 2UL)) / 2UL).ToString();
			return "STEAM_0:" + str + ":" + str2;
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x00501079 File Offset: 0x004FF279
		public override string GetIdentifier()
		{
			return this.ToString();
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x00501100 File Offset: 0x004FF300
		public override bool IsLocalHost()
		{
			return Program.LaunchParameters.ContainsKey("-localsteamid") && Program.LaunchParameters["-localsteamid"].Equals(this.SteamId.m_SteamID.ToString());
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x00501147 File Offset: 0x004FF347
		public override string GetFriendlyName()
		{
			if (this._friendlyName == null)
			{
				this._friendlyName = SteamFriends.GetFriendPersonaName(this.SteamId);
			}
			return this._friendlyName;
		}

		// Token: 0x0400164A RID: 5706
		public readonly CSteamID SteamId;

		// Token: 0x0400164B RID: 5707
		private string _friendlyName;
	}
}
