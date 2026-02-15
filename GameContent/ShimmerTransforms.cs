using System;
using Terraria.Enums;
using Terraria.ID;

namespace Terraria.GameContent
{
	// Token: 0x02000249 RID: 585
	public static class ShimmerTransforms
	{
		// Token: 0x060022E9 RID: 8937 RVA: 0x0053B124 File Offset: 0x00539324
		public static int GetDecraftingRecipeIndex(int type)
		{
			int num = ItemID.Sets.IsCrafted[type];
			if (num < 0)
			{
				return -1;
			}
			if (WorldGen.crimson && ItemID.Sets.IsCraftedCrimson[type] >= 0)
			{
				return ItemID.Sets.IsCraftedCrimson[type];
			}
			if (!WorldGen.crimson && ItemID.Sets.IsCraftedCorruption[type] >= 0)
			{
				return ItemID.Sets.IsCraftedCorruption[type];
			}
			return num;
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x0053B172 File Offset: 0x00539372
		public static bool IsItemTransformLocked(int type)
		{
			return !NPC.downedMoonlord && ItemID.Sets.ShimmerPostMoonlord[type];
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x0053B187 File Offset: 0x00539387
		public static bool IsItemDecraftLocked(int type)
		{
			return ShimmerTransforms.IsRecipeIndexDecraftLocked(ShimmerTransforms.GetDecraftingRecipeIndex(type));
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x0053B194 File Offset: 0x00539394
		public static bool IsRecipeIndexDecraftLocked(int recipeIndex)
		{
			return recipeIndex >= 0 && ((!NPC.downedBoss3 && ShimmerTransforms.RecipeSets.PostSkeletron[recipeIndex]) || (!NPC.downedGolemBoss && ShimmerTransforms.RecipeSets.PostGolem[recipeIndex]));
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x0053B1C4 File Offset: 0x005393C4
		public static bool IsItemDecraftableAndIsDecraftUnlocked(Item item)
		{
			if (item == null)
			{
				return false;
			}
			int decraftingRecipeIndex = ShimmerTransforms.GetDecraftingRecipeIndex(item.GetShimmerEquivalentType(true));
			return !ShimmerTransforms.IsRecipeIndexDecraftLocked(decraftingRecipeIndex) && decraftingRecipeIndex >= 0 && item.stack / Main.recipe[decraftingRecipeIndex].createItem.stack > 0;
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x0053B210 File Offset: 0x00539410
		public static void UpdateRecipeSets()
		{
			ShimmerTransforms.RecipeSets.PostSkeletron = Utils.MapArray<Recipe, bool>(Main.recipe, (Recipe r) => r.ContainsIngredient(154));
			ShimmerTransforms.RecipeSets.PostGolem = Utils.MapArray<Recipe, bool>(Main.recipe, (Recipe r) => r.ContainsIngredient(1101));
		}

		// Token: 0x060022EF RID: 8943 RVA: 0x0053B27C File Offset: 0x0053947C
		public static int GetTransformToItem(int type)
		{
			int num = ItemID.Sets.ShimmerTransformToItem[type];
			if (num > 0)
			{
				return num;
			}
			if (ContentSamples.ItemsByType[type].createTile == 139)
			{
				int placeStyle = ContentSamples.ItemsByType[type].placeStyle;
				if (placeStyle == 90)
				{
					return 5538;
				}
				if (placeStyle == 89)
				{
					return 5579;
				}
				if (placeStyle == 97)
				{
					return 5638;
				}
				if (placeStyle == 96)
				{
					return 5639;
				}
				return 576;
			}
			else
			{
				if (type == 3461)
				{
					return ShimmerTransforms.GetLunarBrickTransformFromMoonPhase(Main.GetMoonPhase());
				}
				return 0;
			}
		}

		// Token: 0x060022F0 RID: 8944 RVA: 0x0053B308 File Offset: 0x00539508
		private static int GetLunarBrickTransformFromMoonPhase(MoonPhase moonPhase)
		{
			switch (moonPhase)
			{
			case MoonPhase.Full:
				return 5408;
			case MoonPhase.ThreeQuartersAtLeft:
				return 5401;
			case MoonPhase.HalfAtLeft:
				return 5403;
			case MoonPhase.QuarterAtLeft:
				return 5402;
			default:
				return 5406;
			case MoonPhase.QuarterAtRight:
				return 5407;
			case MoonPhase.HalfAtRight:
				return 5405;
			case MoonPhase.ThreeQuartersAtRight:
				return 5404;
			}
		}

		// Token: 0x020007D1 RID: 2001
		public static class RecipeSets
		{
			// Token: 0x040070D2 RID: 28882
			public static bool[] PostSkeletron;

			// Token: 0x040070D3 RID: 28883
			public static bool[] PostGolem;
		}
	}
}
