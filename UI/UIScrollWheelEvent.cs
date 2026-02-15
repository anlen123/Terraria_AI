using System;
using Microsoft.Xna.Framework;

namespace Terraria.UI
{
	// Token: 0x020000FF RID: 255
	public class UIScrollWheelEvent : UIMouseEvent
	{
		// Token: 0x06001A12 RID: 6674 RVA: 0x004F3F06 File Offset: 0x004F2106
		public UIScrollWheelEvent(UIElement target, Vector2 mousePosition, int scrollWheelValue) : base(target, mousePosition)
		{
			this.ScrollWheelValue = scrollWheelValue;
		}

		// Token: 0x0400137F RID: 4991
		public readonly int ScrollWheelValue;
	}
}
