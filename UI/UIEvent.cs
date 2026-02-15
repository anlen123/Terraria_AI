using System;

namespace Terraria.UI
{
	// Token: 0x020000FD RID: 253
	public class UIEvent
	{
		// Token: 0x06001A10 RID: 6672 RVA: 0x004F3EE7 File Offset: 0x004F20E7
		public UIEvent(UIElement target)
		{
			this.Target = target;
		}

		// Token: 0x0400137D RID: 4989
		public readonly UIElement Target;
	}
}
