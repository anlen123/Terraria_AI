using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.ObjectInteractions
{
	// Token: 0x020002DB RID: 731
	public class PotionOfReturnHelper
	{
		// Token: 0x06002606 RID: 9734 RVA: 0x0055CCE4 File Offset: 0x0055AEE4
		public static bool TryGetGateHitbox(Player player, out Rectangle homeHitbox)
		{
			homeHitbox = Rectangle.Empty;
			if (player.PotionOfReturnHomePosition == null)
			{
				return false;
			}
			Vector2 value = new Vector2(0f, -21f);
			Vector2 center = player.PotionOfReturnHomePosition.Value + value;
			homeHitbox = Utils.CenteredRectangle(center, new Vector2(24f, 40f));
			return true;
		}
	}
}
