using System;
using Microsoft.Xna.Framework;

namespace Terraria.ID
{
	// Token: 0x020001B2 RID: 434
	public static class Colors
	{
		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06001F20 RID: 7968 RVA: 0x005154E0 File Offset: 0x005136E0
		public static Color CurrentLiquidColor
		{
			get
			{
				Color color = Color.Transparent;
				bool flag = true;
				for (int i = 0; i < 11; i++)
				{
					if (Main.liquidAlpha[i] > 0f)
					{
						if (flag)
						{
							flag = false;
							color = Colors._liquidColors[i];
						}
						else
						{
							color = Color.Lerp(color, Colors._liquidColors[i], Main.liquidAlpha[i]);
						}
					}
				}
				return color;
			}
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x0051553D File Offset: 0x0051373D
		public static Color AlphaDarken(Color input)
		{
			return input * ((float)Main.mouseTextColor / 255f);
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x00515554 File Offset: 0x00513754
		public static Color GetSelectionGlowColor(bool isTileSelected, int averageTileLighting)
		{
			Color result;
			if (isTileSelected)
			{
				result = new Color(averageTileLighting, averageTileLighting, averageTileLighting / 3, averageTileLighting);
			}
			else
			{
				result = new Color(averageTileLighting / 2, averageTileLighting / 2, averageTileLighting / 2, averageTileLighting);
			}
			return result;
		}

		// Token: 0x04001C7D RID: 7293
		public static readonly Color RarityAmber = new Color(255, 175, 0);

		// Token: 0x04001C7E RID: 7294
		public static readonly Color RarityTrash = new Color(130, 130, 130);

		// Token: 0x04001C7F RID: 7295
		public static readonly Color RarityNormal = Color.White;

		// Token: 0x04001C80 RID: 7296
		public static readonly Color RarityBlue = new Color(150, 150, 255);

		// Token: 0x04001C81 RID: 7297
		public static readonly Color RarityGreen = new Color(150, 255, 150);

		// Token: 0x04001C82 RID: 7298
		public static readonly Color RarityOrange = new Color(255, 200, 150);

		// Token: 0x04001C83 RID: 7299
		public static readonly Color RarityRed = new Color(255, 150, 150);

		// Token: 0x04001C84 RID: 7300
		public static readonly Color RarityPink = new Color(255, 150, 255);

		// Token: 0x04001C85 RID: 7301
		public static readonly Color RarityPurple = new Color(210, 160, 255);

		// Token: 0x04001C86 RID: 7302
		public static readonly Color RarityLime = new Color(150, 255, 10);

		// Token: 0x04001C87 RID: 7303
		public static readonly Color RarityYellow = new Color(255, 255, 10);

		// Token: 0x04001C88 RID: 7304
		public static readonly Color RarityCyan = new Color(5, 200, 255);

		// Token: 0x04001C89 RID: 7305
		public static readonly Color CoinPlatinum = new Color(220, 220, 198);

		// Token: 0x04001C8A RID: 7306
		public static readonly Color CoinGold = new Color(224, 201, 92);

		// Token: 0x04001C8B RID: 7307
		public static readonly Color CoinSilver = new Color(181, 192, 193);

		// Token: 0x04001C8C RID: 7308
		public static readonly Color CoinCopper = new Color(246, 138, 96);

		// Token: 0x04001C8D RID: 7309
		public static readonly Color AmbientNPCGastropodLight = new Color(102, 0, 63);

		// Token: 0x04001C8E RID: 7310
		public static readonly Color JourneyMode = Color.Lerp(Color.HotPink, Color.White, 0.1f);

		// Token: 0x04001C8F RID: 7311
		public static readonly Color Mediumcore = new Color(1f, 0.6f, 0f);

		// Token: 0x04001C90 RID: 7312
		public static readonly Color Hardcore = new Color(1f, 0.15f, 0.1f);

		// Token: 0x04001C91 RID: 7313
		public static readonly Color LanternBG = new Color(120, 50, 20);

		// Token: 0x04001C92 RID: 7314
		public static readonly Color[] _waterfallColors = new Color[]
		{
			new Color(9, 61, 191),
			new Color(253, 32, 3),
			new Color(143, 143, 143),
			new Color(59, 29, 131),
			new Color(7, 145, 142),
			new Color(171, 11, 209),
			new Color(9, 137, 191),
			new Color(168, 106, 32),
			new Color(36, 60, 148),
			new Color(65, 59, 101),
			new Color(200, 0, 0),
			default(Color),
			default(Color),
			new Color(177, 54, 79),
			new Color(255, 156, 12),
			new Color(91, 34, 104),
			new Color(102, 104, 34),
			new Color(34, 43, 104),
			new Color(34, 104, 38),
			new Color(104, 34, 34),
			new Color(76, 79, 102),
			new Color(104, 61, 34)
		};

		// Token: 0x04001C93 RID: 7315
		public static readonly Color[] _liquidColors = new Color[]
		{
			new Color(9, 61, 191),
			new Color(253, 32, 3),
			new Color(59, 29, 131),
			new Color(7, 145, 142),
			new Color(171, 11, 209),
			new Color(9, 137, 191),
			new Color(168, 106, 32),
			new Color(36, 60, 148),
			new Color(65, 59, 101),
			new Color(200, 0, 0),
			new Color(177, 54, 79),
			new Color(255, 156, 12)
		};

		// Token: 0x04001C94 RID: 7316
		public static readonly Color FancyUIFatButtonMouseOver = Main.OurFavoriteColor;

		// Token: 0x04001C95 RID: 7317
		public static readonly Color InventoryDefaultColor = new Color(63, 65, 151, 255);

		// Token: 0x04001C96 RID: 7318
		public static readonly Color InventoryDefaultColorWithOpacity = new Color(63, 65, 151, 255) * 0.785f;
	}
}
