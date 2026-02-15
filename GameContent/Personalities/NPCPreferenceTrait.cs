using System;

namespace Terraria.GameContent.Personalities
{
	// Token: 0x0200041F RID: 1055
	public class NPCPreferenceTrait : IShopPersonalityTrait
	{
		// Token: 0x0600305B RID: 12379 RVA: 0x005B918C File Offset: 0x005B738C
		public void ModifyShopPrice(HelperInfo info, ShopHelper shopHelperInstance)
		{
			if (!info.nearbyNPCsByType[this.NpcId])
			{
				return;
			}
			AffectionLevel level = this.Level;
			if (level <= AffectionLevel.Dislike)
			{
				if (level != AffectionLevel.Hate)
				{
					if (level != AffectionLevel.Dislike)
					{
						return;
					}
					shopHelperInstance.DislikeNPC(this.NpcId);
					return;
				}
				else
				{
					shopHelperInstance.HateNPC(this.NpcId);
				}
			}
			else
			{
				if (level == AffectionLevel.Like)
				{
					shopHelperInstance.LikeNPC(this.NpcId);
					return;
				}
				if (level == AffectionLevel.Love)
				{
					shopHelperInstance.LoveNPC(this.NpcId);
					return;
				}
			}
		}

		// Token: 0x040056B1 RID: 22193
		public AffectionLevel Level;

		// Token: 0x040056B2 RID: 22194
		public int NpcId;
	}
}
