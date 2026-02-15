using System;
using System.Collections.Generic;

namespace Terraria.Net
{
	// Token: 0x02000166 RID: 358
	public class NetGroupInfo
	{
		// Token: 0x06001DA0 RID: 7584 RVA: 0x00500EA4 File Offset: 0x004FF0A4
		public NetGroupInfo()
		{
			this._infoProviders = new List<NetGroupInfo.INetGroupInfoProvider>();
			this._infoProviders.Add(new NetGroupInfo.IPAddressInfoProvider());
			this._infoProviders.Add(new NetGroupInfo.SteamLobbyInfoProvider());
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x00500F0C File Offset: 0x004FF10C
		public string ComposeInfo()
		{
			List<string> list = new List<string>();
			foreach (NetGroupInfo.INetGroupInfoProvider netGroupInfoProvider in this._infoProviders)
			{
				if (netGroupInfoProvider.HasValidInfo)
				{
					string text = (int)netGroupInfoProvider.Id + this._separatorBetweenIdAndInfo[0] + netGroupInfoProvider.ProvideInfoNeededToJoin();
					string item = this.ConvertToSafeInfo(text);
					list.Add(item);
				}
			}
			return string.Join(this._separatorBetweenInfos[0], list.ToArray());
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x00500FAC File Offset: 0x004FF1AC
		public Dictionary<NetGroupInfo.InfoProviderId, string> DecomposeInfo(string info)
		{
			Dictionary<NetGroupInfo.InfoProviderId, string> dictionary = new Dictionary<NetGroupInfo.InfoProviderId, string>();
			foreach (string text in info.Split(this._separatorBetweenInfos, StringSplitOptions.RemoveEmptyEntries))
			{
				string[] array2 = this.ConvertFromSafeInfo(text).Split(this._separatorBetweenIdAndInfo, StringSplitOptions.RemoveEmptyEntries);
				int key;
				if (array2.Length == 2 && int.TryParse(array2[0], out key))
				{
					dictionary[(NetGroupInfo.InfoProviderId)key] = array2[1];
				}
			}
			return dictionary;
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x00501015 File Offset: 0x004FF215
		private string ConvertToSafeInfo(string text)
		{
			return Uri.EscapeDataString(text);
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x0050101D File Offset: 0x004FF21D
		private string ConvertFromSafeInfo(string text)
		{
			return Uri.UnescapeDataString(text);
		}

		// Token: 0x04001640 RID: 5696
		private readonly string[] _separatorBetweenInfos = new string[]
		{
			", "
		};

		// Token: 0x04001641 RID: 5697
		private readonly string[] _separatorBetweenIdAndInfo = new string[]
		{
			":"
		};

		// Token: 0x04001642 RID: 5698
		private List<NetGroupInfo.INetGroupInfoProvider> _infoProviders;

		// Token: 0x02000745 RID: 1861
		public enum InfoProviderId
		{
			// Token: 0x04006995 RID: 27029
			IPAddress,
			// Token: 0x04006996 RID: 27030
			Steam
		}

		// Token: 0x02000746 RID: 1862
		private interface INetGroupInfoProvider
		{
			// Token: 0x1700051E RID: 1310
			// (get) Token: 0x060040BB RID: 16571
			NetGroupInfo.InfoProviderId Id { get; }

			// Token: 0x1700051F RID: 1311
			// (get) Token: 0x060040BC RID: 16572
			bool HasValidInfo { get; }

			// Token: 0x060040BD RID: 16573
			string ProvideInfoNeededToJoin();
		}

		// Token: 0x02000747 RID: 1863
		private class IPAddressInfoProvider : NetGroupInfo.INetGroupInfoProvider
		{
			// Token: 0x17000520 RID: 1312
			// (get) Token: 0x060040BE RID: 16574 RVA: 0x001DA9FB File Offset: 0x001D8BFB
			public NetGroupInfo.InfoProviderId Id
			{
				get
				{
					return NetGroupInfo.InfoProviderId.IPAddress;
				}
			}

			// Token: 0x17000521 RID: 1313
			// (get) Token: 0x060040BF RID: 16575 RVA: 0x001DA9FB File Offset: 0x001D8BFB
			public bool HasValidInfo
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060040C0 RID: 16576 RVA: 0x004FC6F2 File Offset: 0x004FA8F2
			public string ProvideInfoNeededToJoin()
			{
				return "";
			}
		}

		// Token: 0x02000748 RID: 1864
		private class SteamLobbyInfoProvider : NetGroupInfo.INetGroupInfoProvider
		{
			// Token: 0x17000522 RID: 1314
			// (get) Token: 0x060040C2 RID: 16578 RVA: 0x000379F1 File Offset: 0x00035BF1
			public NetGroupInfo.InfoProviderId Id
			{
				get
				{
					return NetGroupInfo.InfoProviderId.Steam;
				}
			}

			// Token: 0x17000523 RID: 1315
			// (get) Token: 0x060040C3 RID: 16579 RVA: 0x0069D6BC File Offset: 0x0069B8BC
			public bool HasValidInfo
			{
				get
				{
					return Main.LobbyId > 0UL;
				}
			}

			// Token: 0x060040C4 RID: 16580 RVA: 0x0069D6C7 File Offset: 0x0069B8C7
			public string ProvideInfoNeededToJoin()
			{
				return Main.LobbyId.ToString();
			}
		}
	}
}
