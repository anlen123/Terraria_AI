using System;
using Microsoft.Xna.Framework;

namespace Terraria.UI
{
	// Token: 0x020000FE RID: 254
	public class UIMouseEvent : UIEvent
	{
		// Token: 0x06001A11 RID: 6673 RVA: 0x004F3EF6 File Offset: 0x004F20F6
		public UIMouseEvent(UIElement target, Vector2 mousePosition) : base(target)
		{
			this.MousePosition = mousePosition;
		}

		// Token: 0x0400137E RID: 4990
		public readonly Vector2 MousePosition;
	}
}
