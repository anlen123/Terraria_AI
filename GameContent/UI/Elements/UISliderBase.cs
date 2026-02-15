using System;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003EF RID: 1007
	public class UISliderBase : UIElement
	{
		// Token: 0x06002E86 RID: 11910 RVA: 0x005AAE74 File Offset: 0x005A9074
		internal int GetUsageLevel()
		{
			int result = 0;
			if (UISliderBase.CurrentLockedSlider == this)
			{
				result = 1;
			}
			else if (UISliderBase.CurrentLockedSlider != null)
			{
				result = 2;
			}
			return result;
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x005AAE99 File Offset: 0x005A9099
		public static void EscapeElements()
		{
			UISliderBase.CurrentLockedSlider = null;
			UISliderBase.CurrentAimedSlider = null;
		}

		// Token: 0x04005590 RID: 21904
		internal const int UsageLevel_NotSelected = 0;

		// Token: 0x04005591 RID: 21905
		internal const int UsageLevel_SelectedAndLocked = 1;

		// Token: 0x04005592 RID: 21906
		internal const int UsageLevel_OtherElementIsLocked = 2;

		// Token: 0x04005593 RID: 21907
		public static UIElement CurrentLockedSlider;

		// Token: 0x04005594 RID: 21908
		public static UIElement CurrentAimedSlider;
	}
}
