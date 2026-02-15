using System;

namespace Terraria.Social.Base
{
	// Token: 0x02000160 RID: 352
	public abstract class OverlaySocialModule : ISocialModule
	{
		// Token: 0x06001D64 RID: 7524
		public abstract void Initialize();

		// Token: 0x06001D65 RID: 7525
		public abstract void Shutdown();

		// Token: 0x06001D66 RID: 7526
		public abstract bool IsGamepadTextInputActive();

		// Token: 0x06001D67 RID: 7527
		public abstract bool ShowGamepadTextInput(string description, uint maxLength, bool multiLine = false, string existingText = "", bool password = false);

		// Token: 0x06001D68 RID: 7528
		public abstract string GetGamepadText();
	}
}
