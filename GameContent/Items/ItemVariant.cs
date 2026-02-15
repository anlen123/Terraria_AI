using System;
using Terraria.Localization;

namespace Terraria.GameContent.Items
{
	// Token: 0x02000474 RID: 1140
	public class ItemVariant
	{
		// Token: 0x060032FF RID: 13055 RVA: 0x005F2479 File Offset: 0x005F0679
		public ItemVariant(NetworkText description)
		{
			this.Description = description;
		}

		// Token: 0x06003300 RID: 13056 RVA: 0x005F2488 File Offset: 0x005F0688
		public override string ToString()
		{
			return this.Description.ToString();
		}

		// Token: 0x04005852 RID: 22610
		public readonly NetworkText Description;
	}
}
