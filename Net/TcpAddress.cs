using System;
using System.Net;

namespace Terraria.Net
{
	// Token: 0x02000169 RID: 361
	public class TcpAddress : RemoteAddress
	{
		// Token: 0x06001DA9 RID: 7593 RVA: 0x00501025 File Offset: 0x004FF225
		public TcpAddress(IPAddress address, int port)
		{
			this.Type = AddressType.Tcp;
			this.Address = address;
			this.Port = port;
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x00501042 File Offset: 0x004FF242
		public override string GetIdentifier()
		{
			return this.Address.ToString();
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x0050104F File Offset: 0x004FF24F
		public override bool IsLocalHost()
		{
			return this.Address.Equals(IPAddress.Loopback);
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x00501061 File Offset: 0x004FF261
		public override string ToString()
		{
			return new IPEndPoint(this.Address, this.Port).ToString();
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x00501079 File Offset: 0x004FF279
		public override string GetFriendlyName()
		{
			return this.ToString();
		}

		// Token: 0x04001648 RID: 5704
		public IPAddress Address;

		// Token: 0x04001649 RID: 5705
		public int Port;
	}
}
