using System;

namespace Terraria.Graphics.Capture
{
	// Token: 0x020001DA RID: 474
	public class CaptureBiome
	{
		// Token: 0x06001FD1 RID: 8145 RVA: 0x0051DC40 File Offset: 0x0051BE40
		public CaptureBiome(int backgroundIndex, int waterStyle, CaptureBiome.TileColorStyle tileColorStyle = CaptureBiome.TileColorStyle.Normal)
		{
			this.BackgroundIndex = backgroundIndex;
			this.WaterStyle = waterStyle;
			this.TileColor = tileColorStyle;
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x0051DC60 File Offset: 0x0051BE60
		public static CaptureBiome GetCaptureBiome(int biomeChoice)
		{
			switch (biomeChoice)
			{
			case 1:
				return CaptureBiome.GetPurityForPlayer();
			case 2:
				return CaptureBiome.Styles.Corruption;
			case 3:
				return CaptureBiome.Styles.Jungle;
			case 4:
				return CaptureBiome.Styles.Hallow;
			case 5:
				return CaptureBiome.Styles.Snow;
			case 6:
				return CaptureBiome.Styles.Desert;
			case 7:
				return CaptureBiome.Styles.DirtLayer;
			case 8:
				return CaptureBiome.Styles.RockLayer;
			case 9:
				return CaptureBiome.Styles.Crimson;
			case 10:
				return CaptureBiome.Styles.UndergroundDesert;
			case 11:
				return CaptureBiome.Styles.Ocean;
			case 12:
				return CaptureBiome.Styles.Mushroom;
			}
			CaptureBiome biomeByLocation = CaptureBiome.GetBiomeByLocation();
			if (biomeByLocation != null)
			{
				return biomeByLocation;
			}
			CaptureBiome biomeByWater = CaptureBiome.GetBiomeByWater();
			if (biomeByWater != null)
			{
				return biomeByWater;
			}
			return CaptureBiome.GetPurityForPlayer();
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x0051DD14 File Offset: 0x0051BF14
		private static CaptureBiome GetBiomeByWater()
		{
			int num = Main.CalculateWaterStyle(true);
			for (int i = 0; i < CaptureBiome.BiomesByWaterStyle.Length; i++)
			{
				CaptureBiome captureBiome = CaptureBiome.BiomesByWaterStyle[i];
				if (captureBiome != null && captureBiome.WaterStyle == num)
				{
					return captureBiome;
				}
			}
			return null;
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0051DD54 File Offset: 0x0051BF54
		private static CaptureBiome GetBiomeByLocation()
		{
			switch (Main.GetPreferredBGStyleForPlayer())
			{
			case 0:
				return CaptureBiome.Styles.Purity;
			case 1:
				return CaptureBiome.Styles.Corruption;
			case 2:
			case 5:
			case 13:
			case 14:
				return CaptureBiome.Styles.Desert;
			case 3:
				return CaptureBiome.Styles.Jungle;
			case 4:
				return CaptureBiome.Styles.Ocean;
			case 6:
				return CaptureBiome.Styles.Hallow;
			case 7:
				return CaptureBiome.Styles.Snow;
			case 8:
				return CaptureBiome.Styles.Crimson;
			case 9:
				return CaptureBiome.Styles.Mushroom;
			case 10:
				return CaptureBiome.Styles.Purity2;
			case 11:
				return CaptureBiome.Styles.Purity3;
			case 12:
				return CaptureBiome.Styles.Purity4;
			default:
				return null;
			}
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x0051DDF4 File Offset: 0x0051BFF4
		private static CaptureBiome GetPurityForPlayer()
		{
			int num = (int)Main.LocalPlayer.Center.X / 16;
			if (num < Main.treeX[0])
			{
				return CaptureBiome.Styles.Purity;
			}
			if (num < Main.treeX[1])
			{
				return CaptureBiome.Styles.Purity2;
			}
			if (num < Main.treeX[2])
			{
				return CaptureBiome.Styles.Purity3;
			}
			return CaptureBiome.Styles.Purity4;
		}

		// Token: 0x04004A3C RID: 19004
		public static readonly CaptureBiome DefaultPurity = new CaptureBiome(0, 0, CaptureBiome.TileColorStyle.Normal);

		// Token: 0x04004A3D RID: 19005
		public static CaptureBiome[] BiomesByWaterStyle = new CaptureBiome[]
		{
			null,
			null,
			CaptureBiome.Styles.Corruption,
			CaptureBiome.Styles.Jungle,
			CaptureBiome.Styles.Hallow,
			CaptureBiome.Styles.Snow,
			CaptureBiome.Styles.Desert,
			CaptureBiome.Styles.DirtLayer,
			CaptureBiome.Styles.RockLayer,
			CaptureBiome.Styles.BloodMoon,
			CaptureBiome.Styles.Crimson,
			null,
			CaptureBiome.Styles.UndergroundDesert,
			CaptureBiome.Styles.Ocean,
			CaptureBiome.Styles.Mushroom
		};

		// Token: 0x04004A3E RID: 19006
		public readonly int WaterStyle;

		// Token: 0x04004A3F RID: 19007
		public readonly int BackgroundIndex;

		// Token: 0x04004A40 RID: 19008
		public readonly CaptureBiome.TileColorStyle TileColor;

		// Token: 0x02000794 RID: 1940
		public enum TileColorStyle
		{
			// Token: 0x0400700C RID: 28684
			Normal,
			// Token: 0x0400700D RID: 28685
			Jungle,
			// Token: 0x0400700E RID: 28686
			Crimson,
			// Token: 0x0400700F RID: 28687
			Corrupt,
			// Token: 0x04007010 RID: 28688
			Mushroom
		}

		// Token: 0x02000795 RID: 1941
		public class Sets
		{
			// Token: 0x02000AAD RID: 2733
			public class WaterStyles
			{
				// Token: 0x04007829 RID: 30761
				public const int BloodMoon = 9;
			}
		}

		// Token: 0x02000796 RID: 1942
		public class Styles
		{
			// Token: 0x04007011 RID: 28689
			public static CaptureBiome Purity = new CaptureBiome(0, 0, CaptureBiome.TileColorStyle.Normal);

			// Token: 0x04007012 RID: 28690
			public static CaptureBiome Purity2 = new CaptureBiome(10, 0, CaptureBiome.TileColorStyle.Normal);

			// Token: 0x04007013 RID: 28691
			public static CaptureBiome Purity3 = new CaptureBiome(11, 0, CaptureBiome.TileColorStyle.Normal);

			// Token: 0x04007014 RID: 28692
			public static CaptureBiome Purity4 = new CaptureBiome(12, 0, CaptureBiome.TileColorStyle.Normal);

			// Token: 0x04007015 RID: 28693
			public static CaptureBiome Corruption = new CaptureBiome(1, 2, CaptureBiome.TileColorStyle.Corrupt);

			// Token: 0x04007016 RID: 28694
			public static CaptureBiome Jungle = new CaptureBiome(3, 3, CaptureBiome.TileColorStyle.Jungle);

			// Token: 0x04007017 RID: 28695
			public static CaptureBiome Hallow = new CaptureBiome(6, 4, CaptureBiome.TileColorStyle.Normal);

			// Token: 0x04007018 RID: 28696
			public static CaptureBiome Snow = new CaptureBiome(7, 5, CaptureBiome.TileColorStyle.Normal);

			// Token: 0x04007019 RID: 28697
			public static CaptureBiome Desert = new CaptureBiome(2, 6, CaptureBiome.TileColorStyle.Normal);

			// Token: 0x0400701A RID: 28698
			public static CaptureBiome DirtLayer = new CaptureBiome(0, 7, CaptureBiome.TileColorStyle.Normal);

			// Token: 0x0400701B RID: 28699
			public static CaptureBiome RockLayer = new CaptureBiome(0, 8, CaptureBiome.TileColorStyle.Normal);

			// Token: 0x0400701C RID: 28700
			public static CaptureBiome BloodMoon = new CaptureBiome(0, 9, CaptureBiome.TileColorStyle.Normal);

			// Token: 0x0400701D RID: 28701
			public static CaptureBiome Crimson = new CaptureBiome(8, 10, CaptureBiome.TileColorStyle.Crimson);

			// Token: 0x0400701E RID: 28702
			public static CaptureBiome UndergroundDesert = new CaptureBiome(2, 12, CaptureBiome.TileColorStyle.Normal);

			// Token: 0x0400701F RID: 28703
			public static CaptureBiome Ocean = new CaptureBiome(4, 13, CaptureBiome.TileColorStyle.Normal);

			// Token: 0x04007020 RID: 28704
			public static CaptureBiome Mushroom = new CaptureBiome(9, 7, CaptureBiome.TileColorStyle.Mushroom);
		}

		// Token: 0x02000797 RID: 1943
		private enum BiomeChoiceIndex
		{
			// Token: 0x04007022 RID: 28706
			AutomatedForPlayer = -1,
			// Token: 0x04007023 RID: 28707
			Purity = 1,
			// Token: 0x04007024 RID: 28708
			Corruption,
			// Token: 0x04007025 RID: 28709
			Jungle,
			// Token: 0x04007026 RID: 28710
			Hallow,
			// Token: 0x04007027 RID: 28711
			Snow,
			// Token: 0x04007028 RID: 28712
			Desert,
			// Token: 0x04007029 RID: 28713
			DirtLayer,
			// Token: 0x0400702A RID: 28714
			RockLayer,
			// Token: 0x0400702B RID: 28715
			Crimson,
			// Token: 0x0400702C RID: 28716
			UndergroundDesert,
			// Token: 0x0400702D RID: 28717
			Ocean,
			// Token: 0x0400702E RID: 28718
			Mushroom
		}
	}
}
