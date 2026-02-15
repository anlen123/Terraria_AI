using System;
using Terraria.Localization;

namespace Terraria.GameContent.Items
{
	// Token: 0x02000475 RID: 1141
	public class ItemVariantCondition
	{
		// Token: 0x06003301 RID: 13057 RVA: 0x005F2495 File Offset: 0x005F0695
		public ItemVariantCondition(NetworkText description, ItemVariantCondition.Condition condition)
		{
			this.Description = description;
			this.IsMet = condition;
		}

		// Token: 0x06003302 RID: 13058 RVA: 0x005F24AB File Offset: 0x005F06AB
		public override string ToString()
		{
			return this.Description.ToString();
		}

		// Token: 0x04005853 RID: 22611
		public readonly NetworkText Description;

		// Token: 0x04005854 RID: 22612
		public readonly ItemVariantCondition.Condition IsMet;

		// Token: 0x0200096E RID: 2414
		// (Invoke) Token: 0x060048F0 RID: 18672
		public delegate bool Condition();
	}
}
