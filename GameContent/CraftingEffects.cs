using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Renderers;
using Terraria.ID;

namespace Terraria.GameContent
{
	// Token: 0x0200022C RID: 556
	public class CraftingEffects
	{
		// Token: 0x060021C4 RID: 8644 RVA: 0x00531C9C File Offset: 0x0052FE9C
		public static void OnCraft(Recipe recipe, bool quickCraft)
		{
			CraftingEffects._justCraftedItemType = recipe.createItem.type;
			Item createItem = recipe.createItem;
			CraftingEffects.SpawnEffects_BeforeGrantingItem(recipe, createItem);
			if (!quickCraft)
			{
				CraftingEffects._mouseItemGlow = 1f;
			}
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x00531CD4 File Offset: 0x0052FED4
		public static void OnCraftItemGranted(Recipe recipe, Item result, bool quickCraft)
		{
			PopupText.NewText(PopupTextContext.ItemCraft, result, Main.LocalPlayer.Center, recipe.createItem.stack, false, false);
			CraftingEffects.SpawnEffects_AfterGrantingItem(recipe, result, quickCraft);
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x00531CFD File Offset: 0x0052FEFD
		public static void Update()
		{
			if (CraftingEffects._mouseItemGlow > 0f)
			{
				CraftingEffects._mouseItemGlow -= 0.035f;
			}
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x00531D1B File Offset: 0x0052FF1B
		public static float GetGlow(Item cursorItem)
		{
			if (CraftingEffects._mouseItemGlow <= 0f || CraftingEffects._justCraftedItemType != cursorItem.type)
			{
				return 0f;
			}
			return CraftingEffects._mouseItemGlow;
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x00531D41 File Offset: 0x0052FF41
		private static void SpawnEffects_BeforeGrantingItem(Recipe recipe, Item result)
		{
			SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x00009E06 File Offset: 0x00008006
		public static void SpawnEffects_AfterGrantingItem(Recipe recipe, Item result, bool quickCraft)
		{
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x00531D57 File Offset: 0x0052FF57
		private static bool RecipeUsesCraftingStation(Recipe recipe, int tileId)
		{
			return recipe.requiredTile == tileId;
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x00531D64 File Offset: 0x0052FF64
		public static CraftingEffectDetails GetEffectDetails(Item newItem)
		{
			int rare = newItem.rare;
			CraftingEffectDetails result = default(CraftingEffectDetails);
			result.Rarity = rare;
			if ((newItem.healLife > 0 || newItem.healMana > 0 || newItem.buffType > 0 || ItemID.Sets.IsFood[newItem.type] || ItemID.Sets.SortingPriorityPotionsBuffs[newItem.type] != -1) & newItem.consumable)
			{
				result.Style = PopupEffectStyle.Potion;
				result.Intensity = rare;
			}
			bool flag = newItem.GetRollablePrefixes() != null || newItem.accessory || newItem.bodySlot != -1 || newItem.headSlot != -1 || newItem.legSlot != -1 || (newItem.shoot != 0 && Main.projHook[newItem.shoot]) || newItem.mountType != -1;
			if (flag)
			{
				result.Style = PopupEffectStyle.Metal;
				result.Intensity = rare;
			}
			if (flag && newItem.magic)
			{
				result.Style = PopupEffectStyle.MagicWeapon;
				result.Intensity = rare;
			}
			if (flag && newItem.melee)
			{
				result.Style = PopupEffectStyle.MeleeWeapon;
				result.Intensity = rare;
			}
			if (flag && newItem.ranged)
			{
				result.Style = PopupEffectStyle.RangedWeapon;
				result.Intensity = rare;
			}
			return result;
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x00531E94 File Offset: 0x00530094
		private static void CreateBubbleParticles(int n)
		{
			for (float num = 0f; num < 2f; num += 0.083333336f)
			{
				float num2 = 15f;
				float f = 6.2831855f * (num + Main.rand.NextFloat());
				FadingParticle fadingParticle = ParticleOrchestrator._poolFading.RequestParticle();
				fadingParticle.SetBasicInfo(TextureAssets.Bubble, null, f.ToRotationVector2() * (2f + 3f * Main.rand.NextFloat()), Main.MouseScreen + f.ToRotationVector2() * (10f + 40f * Main.rand.NextFloat()));
				fadingParticle.SetTypeInfo(num2, true);
				fadingParticle.AccelerationPerFrame = fadingParticle.Velocity * (-1f / num2);
				fadingParticle.LocalPosition -= fadingParticle.Velocity * 4f;
				fadingParticle.FadeInNormalizedTime = 0.2f;
				fadingParticle.FadeOutNormalizedTime = 0.7f;
				fadingParticle.Scale = Vector2.One;
				Main.ParticleSystem_OverInventory.Add(fadingParticle);
			}
		}

		// Token: 0x04004C92 RID: 19602
		private static int _justCraftedItemType;

		// Token: 0x04004C93 RID: 19603
		private static float _mouseItemGlow;
	}
}
