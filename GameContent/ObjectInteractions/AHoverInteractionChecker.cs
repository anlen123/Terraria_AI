using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameInput;

namespace Terraria.GameContent.ObjectInteractions
{
	// Token: 0x020002D0 RID: 720
	public abstract class AHoverInteractionChecker
	{
		// Token: 0x060025E9 RID: 9705 RVA: 0x0055B818 File Offset: 0x00559A18
		internal AHoverInteractionChecker.HoverStatus AttemptInteraction(Player player, Rectangle Hitbox)
		{
			Point point = Hitbox.ClosestPointInRect(player.Center).ToTileCoordinates();
			if (!player.IsInTileInteractionRange(point.X, point.Y, TileReachCheckSettings.Simple, 0))
			{
				return AHoverInteractionChecker.HoverStatus.NotSelectable;
			}
			Vector2 v = Main.ReverseGravitySupport(Main.MouseScreen, 0f) + Main.screenPosition;
			bool flag = Hitbox.Contains(v.ToPoint());
			bool flag2 = flag;
			bool? flag3 = this.AttemptOverridingHoverStatus(player, Hitbox);
			if (flag3 != null)
			{
				flag2 = flag3.Value;
			}
			flag2 &= !player.lastMouseInterface;
			bool flag4 = !Main.SmartCursorIsUsed && !PlayerInput.UsingGamepad;
			if (!flag2)
			{
				if (!flag4)
				{
					return AHoverInteractionChecker.HoverStatus.SelectableButNotSelected;
				}
				return AHoverInteractionChecker.HoverStatus.NotSelectable;
			}
			else
			{
				Main.HasInteractableObjectThatIsNotATile = true;
				if (flag)
				{
					this.DoHoverEffect(player, Hitbox);
				}
				if (PlayerInput.UsingGamepad)
				{
					player.GamepadEnableGrappleCooldown();
				}
				bool flag5 = this.ShouldBlockInteraction(player, Hitbox);
				if (Main.mouseRight && Main.mouseRightRelease && !flag5)
				{
					Main.mouseRightRelease = false;
					player.tileInteractAttempted = true;
					player.tileInteractionHappened = true;
					player.releaseUseTile = false;
					this.PerformInteraction(player, Hitbox);
				}
				if (!Main.SmartCursorIsUsed && !PlayerInput.UsingGamepad)
				{
					return AHoverInteractionChecker.HoverStatus.NotSelectable;
				}
				if (!flag4)
				{
					return AHoverInteractionChecker.HoverStatus.Selected;
				}
				return AHoverInteractionChecker.HoverStatus.NotSelectable;
			}
		}

		// Token: 0x060025EA RID: 9706
		internal abstract bool? AttemptOverridingHoverStatus(Player player, Rectangle rectangle);

		// Token: 0x060025EB RID: 9707
		internal abstract void DoHoverEffect(Player player, Rectangle hitbox);

		// Token: 0x060025EC RID: 9708
		internal abstract bool ShouldBlockInteraction(Player player, Rectangle hitbox);

		// Token: 0x060025ED RID: 9709
		internal abstract void PerformInteraction(Player player, Rectangle hitbox);

		// Token: 0x0200081F RID: 2079
		internal enum HoverStatus
		{
			// Token: 0x0400720B RID: 29195
			NotSelectable,
			// Token: 0x0400720C RID: 29196
			SelectableButNotSelected,
			// Token: 0x0400720D RID: 29197
			Selected
		}
	}
}
