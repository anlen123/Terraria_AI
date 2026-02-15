using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003E7 RID: 999
	public interface IGroupOptionButton
	{
		// Token: 0x06002E44 RID: 11844
		void SetColorsBasedOnSelectionState(Color pickedColor, Color unpickedColor, float opacityPicked, float opacityNotPicked);

		// Token: 0x06002E45 RID: 11845
		void SetBorderColor(Color color);
	}
}
