using System;
using rail;

namespace Terraria.Net
{
	// Token: 0x0200016B RID: 363
	public class WeGameAddress : RemoteAddress
	{
		// Token: 0x06001DB3 RID: 7603 RVA: 0x00501168 File Offset: 0x004FF368
		public WeGameAddress(RailID id, string name)
		{
			this.Type = AddressType.WeGame;
			this.rail_id = id;
			this.nickname = name;
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x00501185 File Offset: 0x004FF385
		public override string ToString()
		{
			return "WEGAME_0:" + this.rail_id.id_.ToString();
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x00501079 File Offset: 0x004FF279
		public override string GetIdentifier()
		{
			return this.ToString();
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x005011A1 File Offset: 0x004FF3A1
		public override bool IsLocalHost()
		{
			return Program.LaunchParameters.ContainsKey("-localwegameid") && Program.LaunchParameters["-localwegameid"].Equals(this.rail_id.id_.ToString());
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x005011DA File Offset: 0x004FF3DA
		public override string GetFriendlyName()
		{
			return this.nickname;
		}

		// Token: 0x0400164C RID: 5708
		public readonly RailID rail_id;

		// Token: 0x0400164D RID: 5709
		private string nickname;
	}
}
