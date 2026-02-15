using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent.UI
{
	// Token: 0x0200037D RID: 893
	public class ItemRarity
	{
		// Token: 0x0600297F RID: 10623 RVA: 0x0057CB7C File Offset: 0x0057AD7C
		public static void Initialize()
		{
			ItemRarity._rarities.Clear();
			ItemRarity._rarities.Add(-11, Colors.RarityAmber);
			ItemRarity._rarities.Add(-1, Colors.RarityTrash);
			ItemRarity._rarities.Add(1, Colors.RarityBlue);
			ItemRarity._rarities.Add(2, Colors.RarityGreen);
			ItemRarity._rarities.Add(3, Colors.RarityOrange);
			ItemRarity._rarities.Add(4, Colors.RarityRed);
			ItemRarity._rarities.Add(5, Colors.RarityPink);
			ItemRarity._rarities.Add(6, Colors.RarityPurple);
			ItemRarity._rarities.Add(7, Colors.RarityLime);
			ItemRarity._rarities.Add(8, Colors.RarityYellow);
			ItemRarity._rarities.Add(9, Colors.RarityCyan);
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x0057CC48 File Offset: 0x0057AE48
		public static Color GetColor(int rarity)
		{
			Color result = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
			if (ItemRarity._rarities.ContainsKey(rarity))
			{
				return ItemRarity._rarities[rarity];
			}
			return result;
		}

		// Token: 0x04005278 RID: 21112
		private static Dictionary<int, Color> _rarities = new Dictionary<int, Color>();
	}
}
