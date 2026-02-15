using System;

namespace Terraria.UI
{
	// Token: 0x02000100 RID: 256
	public class UIState : UIElement
	{
		// Token: 0x06001A13 RID: 6675 RVA: 0x004F3F17 File Offset: 0x004F2117
		public UIState()
		{
			this.Width.Precent = 1f;
			this.Height.Precent = 1f;
			this.Recalculate();
		}

		// Token: 0x04001380 RID: 4992
		public bool NoGamepadSupport;
	}
}
