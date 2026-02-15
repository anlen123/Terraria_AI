using System;
using Terraria.Social.Base;

namespace Terraria.Social.WeGame
{
	// Token: 0x0200012D RID: 301
	public class OverlaySocialModule : OverlaySocialModule
	{
		// Token: 0x06001C05 RID: 7173 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Initialize()
		{
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Shutdown()
		{
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x004FC6EA File Offset: 0x004FA8EA
		public override bool IsGamepadTextInputActive()
		{
			return this._gamepadTextInputActive;
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public override bool ShowGamepadTextInput(string description, uint maxLength, bool multiLine = false, string existingText = "", bool password = false)
		{
			return false;
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x004FC6F2 File Offset: 0x004FA8F2
		public override string GetGamepadText()
		{
			return "";
		}

		// Token: 0x04001595 RID: 5525
		private bool _gamepadTextInputActive;
	}
}
