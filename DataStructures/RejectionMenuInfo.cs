using System;
using Terraria.Audio;

namespace Terraria.DataStructures
{
	// Token: 0x02000586 RID: 1414
	public class RejectionMenuInfo
	{
		// Token: 0x060037F4 RID: 14324 RVA: 0x0062F5E3 File Offset: 0x0062D7E3
		public void DefaultExitAction()
		{
			SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
			Main.menuMode = 0;
			Main.netMode = 0;
		}

		// Token: 0x04005C08 RID: 23560
		public ReturnFromRejectionMenuAction ExitAction;

		// Token: 0x04005C09 RID: 23561
		public string TextToShow;
	}
}
