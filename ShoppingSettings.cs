using System;

namespace Terraria
{
	// Token: 0x02000012 RID: 18
	public struct ShoppingSettings
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00009DD8 File Offset: 0x00007FD8
		public static ShoppingSettings NotInShop
		{
			get
			{
				return new ShoppingSettings
				{
					PriceAdjustment = 1f,
					HappinessReport = ""
				};
			}
		}

		// Token: 0x04000041 RID: 65
		public float PriceAdjustment;

		// Token: 0x04000042 RID: 66
		public string HappinessReport;
	}
}
