using System;

namespace Terraria.GameContent.Personalities
{
	// Token: 0x02000422 RID: 1058
	public abstract class AShoppingBiome
	{
		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06003064 RID: 12388 RVA: 0x005B964D File Offset: 0x005B784D
		// (set) Token: 0x06003065 RID: 12389 RVA: 0x005B9655 File Offset: 0x005B7855
		public string NameKey { get; protected set; }

		// Token: 0x06003066 RID: 12390
		public abstract bool IsInBiome(Player player);
	}
}
