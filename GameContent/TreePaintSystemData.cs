using System;

namespace Terraria.GameContent
{
	// Token: 0x02000259 RID: 601
	public static class TreePaintSystemData
	{
		// Token: 0x0600233A RID: 9018 RVA: 0x0053C77C File Offset: 0x0053A97C
		public static TreePaintingSettings GetCageTopSettings()
		{
			return TreePaintSystemData.DefaultNoSpecialGroups;
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x0053C784 File Offset: 0x0053A984
		public static TreePaintingSettings GetTileSettings(int tileType, int tileStyle)
		{
			if (tileType <= 109)
			{
				if (tileType > 5)
				{
					if (tileType <= 60)
					{
						if (tileType == 23)
						{
							goto IL_104;
						}
						if (tileType - 59 > 1)
						{
							goto IL_FE;
						}
					}
					else if (tileType != 70)
					{
						if (tileType != 109)
						{
							goto IL_FE;
						}
						goto IL_104;
					}
					return TreePaintSystemData.CullMud;
				}
				if (tileType == 0 || tileType == 2)
				{
					goto IL_104;
				}
				if (tileType == 5)
				{
					switch (tileStyle)
					{
					default:
						return TreePaintSystemData.WoodPurity;
					case 0:
						return TreePaintSystemData.WoodCorruption;
					case 1:
						return TreePaintSystemData.WoodJungle;
					case 2:
						return TreePaintSystemData.WoodHallow;
					case 3:
						return TreePaintSystemData.WoodSnow;
					case 4:
						return TreePaintSystemData.WoodCrimson;
					case 5:
						return TreePaintSystemData.WoodJungleUnderground;
					case 6:
						return TreePaintSystemData.WoodGlowingMushroom;
					}
				}
			}
			else if (tileType <= 492)
			{
				if (tileType <= 323)
				{
					if (tileType == 199)
					{
						goto IL_104;
					}
					if (tileType == 323)
					{
						switch (tileStyle)
						{
						case 0:
						case 4:
							return TreePaintSystemData.PalmTreePurity;
						case 1:
						case 5:
							return TreePaintSystemData.PalmTreeCrimson;
						case 2:
						case 6:
							return TreePaintSystemData.PalmTreeHallow;
						case 3:
						case 7:
							return TreePaintSystemData.PalmTreeCorruption;
						default:
							return TreePaintSystemData.WoodPurity;
						}
					}
				}
				else if (tileType == 477 || tileType == 492)
				{
					goto IL_104;
				}
			}
			else if (tileType <= 616)
			{
				switch (tileType)
				{
				case 583:
					return TreePaintSystemData.GemTreeTopaz;
				case 584:
					return TreePaintSystemData.GemTreeAmethyst;
				case 585:
					return TreePaintSystemData.GemTreeSapphire;
				case 586:
					return TreePaintSystemData.GemTreeEmerald;
				case 587:
					return TreePaintSystemData.GemTreeRuby;
				case 588:
					return TreePaintSystemData.GemTreeDiamond;
				case 589:
					return TreePaintSystemData.GemTreeAmber;
				case 590:
				case 591:
				case 592:
				case 593:
				case 594:
					break;
				case 595:
				case 596:
					return TreePaintSystemData.VanityCherry;
				default:
					if (tileType - 615 <= 1)
					{
						return TreePaintSystemData.VanityYellowWillow;
					}
					break;
				}
			}
			else
			{
				if (tileType == 633)
				{
					goto IL_104;
				}
				if (tileType == 634)
				{
					return TreePaintSystemData.TreeAsh;
				}
			}
			IL_FE:
			return TreePaintSystemData.DefaultNoSpecialGroups;
			IL_104:
			return TreePaintSystemData.DefaultDirt;
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x0053C978 File Offset: 0x0053AB78
		public static TreePaintingSettings GetTreeFoliageSettings(int foliageIndex, int foliageStyle)
		{
			switch (foliageIndex)
			{
			case 0:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
				return TreePaintSystemData.WoodPurity;
			case 1:
				return TreePaintSystemData.WoodCorruption;
			case 2:
			case 11:
			case 13:
				return TreePaintSystemData.WoodJungle;
			case 3:
			case 19:
			case 20:
				return TreePaintSystemData.WoodHallow;
			case 4:
			case 12:
			case 16:
			case 17:
			case 18:
				return TreePaintSystemData.WoodSnow;
			case 5:
				return TreePaintSystemData.WoodCrimson;
			case 14:
				return TreePaintSystemData.WoodGlowingMushroom;
			case 15:
			case 21:
				switch (foliageStyle)
				{
				case 0:
				case 4:
					return TreePaintSystemData.PalmTreePurity;
				case 1:
				case 5:
					return TreePaintSystemData.PalmTreeCrimson;
				case 2:
				case 6:
					return TreePaintSystemData.PalmTreeHallow;
				case 3:
				case 7:
					return TreePaintSystemData.PalmTreeCorruption;
				default:
					return TreePaintSystemData.WoodPurity;
				}
				break;
			case 22:
				return TreePaintSystemData.GemTreeTopaz;
			case 23:
				return TreePaintSystemData.GemTreeAmethyst;
			case 24:
				return TreePaintSystemData.GemTreeSapphire;
			case 25:
				return TreePaintSystemData.GemTreeEmerald;
			case 26:
				return TreePaintSystemData.GemTreeRuby;
			case 27:
				return TreePaintSystemData.GemTreeDiamond;
			case 28:
				return TreePaintSystemData.GemTreeAmber;
			case 29:
				return TreePaintSystemData.VanityCherry;
			case 30:
				return TreePaintSystemData.VanityYellowWillow;
			default:
				return TreePaintSystemData.DefaultDirt;
			}
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x0053CAB0 File Offset: 0x0053ACB0
		public static TreePaintingSettings GetWallSettings(int wallType)
		{
			return TreePaintSystemData.DefaultNoSpecialGroups_ForWalls;
		}

		// Token: 0x04004D42 RID: 19778
		private static TreePaintingSettings DefaultNoSpecialGroups = new TreePaintingSettings
		{
			UseSpecialGroups = false
		};

		// Token: 0x04004D43 RID: 19779
		private static TreePaintingSettings DefaultNoSpecialGroups_ForWalls = new TreePaintingSettings
		{
			UseSpecialGroups = false,
			UseWallShaderHacks = true
		};

		// Token: 0x04004D44 RID: 19780
		private static TreePaintingSettings DefaultDirt = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0.03f,
			SpecialGroupMaximumHueValue = 0.08f,
			SpecialGroupMinimumSaturationValue = 0.38f,
			SpecialGroupMaximumSaturationValue = 0.53f,
			InvertSpecialGroupResult = true
		};

		// Token: 0x04004D45 RID: 19781
		private static TreePaintingSettings CullMud = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			HueTestOffset = 0.5f,
			SpecialGroupMinimalHueValue = 0.42f,
			SpecialGroupMaximumHueValue = 0.55f,
			SpecialGroupMinimumSaturationValue = 0.2f,
			SpecialGroupMaximumSaturationValue = 0.27f,
			InvertSpecialGroupResult = true
		};

		// Token: 0x04004D46 RID: 19782
		private static TreePaintingSettings WoodPurity = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0.16666667f,
			SpecialGroupMaximumHueValue = 0.8333333f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		// Token: 0x04004D47 RID: 19783
		private static TreePaintingSettings WoodCorruption = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0.5f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0.27f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		// Token: 0x04004D48 RID: 19784
		private static TreePaintingSettings WoodJungle = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0.16666667f,
			SpecialGroupMaximumHueValue = 0.8333333f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		// Token: 0x04004D49 RID: 19785
		private static TreePaintingSettings WoodHallow = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 0.34f,
			InvertSpecialGroupResult = true
		};

		// Token: 0x04004D4A RID: 19786
		private static TreePaintingSettings WoodSnow = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 0.06944445f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		// Token: 0x04004D4B RID: 19787
		private static TreePaintingSettings WoodCrimson = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0.33333334f,
			SpecialGroupMaximumHueValue = 0.6666667f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f,
			InvertSpecialGroupResult = true
		};

		// Token: 0x04004D4C RID: 19788
		private static TreePaintingSettings WoodJungleUnderground = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0.16666667f,
			SpecialGroupMaximumHueValue = 0.8333333f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		// Token: 0x04004D4D RID: 19789
		private static TreePaintingSettings WoodGlowingMushroom = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0.5f,
			SpecialGroupMaximumHueValue = 0.8333333f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		// Token: 0x04004D4E RID: 19790
		private static TreePaintingSettings VanityCherry = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0.8333333f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		// Token: 0x04004D4F RID: 19791
		private static TreePaintingSettings VanityYellowWillow = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 0.025f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f,
			InvertSpecialGroupResult = true
		};

		// Token: 0x04004D50 RID: 19792
		private static TreePaintingSettings TreeAsh = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 0.025f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f,
			InvertSpecialGroupResult = true
		};

		// Token: 0x04004D51 RID: 19793
		private static TreePaintingSettings GemTreeRuby = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 0.0027777778f,
			InvertSpecialGroupResult = true
		};

		// Token: 0x04004D52 RID: 19794
		private static TreePaintingSettings GemTreeAmber = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 0.0027777778f,
			InvertSpecialGroupResult = true
		};

		// Token: 0x04004D53 RID: 19795
		private static TreePaintingSettings GemTreeSapphire = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 0.0027777778f,
			InvertSpecialGroupResult = true
		};

		// Token: 0x04004D54 RID: 19796
		private static TreePaintingSettings GemTreeEmerald = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 0.0027777778f,
			InvertSpecialGroupResult = true
		};

		// Token: 0x04004D55 RID: 19797
		private static TreePaintingSettings GemTreeAmethyst = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 0.0027777778f,
			InvertSpecialGroupResult = true
		};

		// Token: 0x04004D56 RID: 19798
		private static TreePaintingSettings GemTreeTopaz = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 0.0027777778f,
			InvertSpecialGroupResult = true
		};

		// Token: 0x04004D57 RID: 19799
		private static TreePaintingSettings GemTreeDiamond = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 0.0027777778f,
			InvertSpecialGroupResult = true
		};

		// Token: 0x04004D58 RID: 19800
		private static TreePaintingSettings PalmTreePurity = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0.15277778f,
			SpecialGroupMaximumHueValue = 0.25f,
			SpecialGroupMinimumSaturationValue = 0.88f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		// Token: 0x04004D59 RID: 19801
		private static TreePaintingSettings PalmTreeCorruption = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0.4f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		// Token: 0x04004D5A RID: 19802
		private static TreePaintingSettings PalmTreeCrimson = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			HueTestOffset = 0.5f,
			SpecialGroupMinimalHueValue = 0.33333334f,
			SpecialGroupMaximumHueValue = 0.5277778f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		// Token: 0x04004D5B RID: 19803
		private static TreePaintingSettings PalmTreeHallow = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0.5f,
			SpecialGroupMaximumHueValue = 0.6111111f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f
		};
	}
}
