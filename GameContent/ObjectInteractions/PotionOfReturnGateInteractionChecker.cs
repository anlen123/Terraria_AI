using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.ObjectInteractions
{
	// Token: 0x020002DC RID: 732
	public class PotionOfReturnGateInteractionChecker : AHoverInteractionChecker
	{
		// Token: 0x06002608 RID: 9736 RVA: 0x0055CD4C File Offset: 0x0055AF4C
		internal override bool? AttemptOverridingHoverStatus(Player player, Rectangle rectangle)
		{
			if (Main.SmartInteractPotionOfReturn)
			{
				return new bool?(true);
			}
			return null;
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x0055CD70 File Offset: 0x0055AF70
		internal override void DoHoverEffect(Player player, Rectangle hitbox)
		{
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = 4870;
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x0055CD8B File Offset: 0x0055AF8B
		internal override bool ShouldBlockInteraction(Player player, Rectangle hitbox)
		{
			return Player.BlockInteractionWithProjectiles != 0;
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x0055CD95 File Offset: 0x0055AF95
		internal override void PerformInteraction(Player player, Rectangle hitbox)
		{
			player.DoPotionOfReturnReturnToOriginalUsePosition();
		}
	}
}
